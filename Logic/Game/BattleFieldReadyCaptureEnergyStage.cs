
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    partial class Field
    {
        public class ReadyCaptureEnergyStage : Regulus.Game.IStage
        {
            int _RoundCount;
            private ChipLibrary _ChipLibrary;
            private List<Player> _Players;
            class Decided
            {
                public Player Owner;
                public int ChipCount;
                public void OnChips(Chip[] chips)
                {
                    ChipCount = chips.Length;
                    IsDecided = true;
                }
                public bool IsDecided;

            }
            List<Decided> _Decideds;

            public delegate void OnTimeOut(CaptureEnergyStage stage);
            public event OnTimeOut TimeOutEvent;
            Regulus.Utility.TimeCounter _Timeout;            

            public ReadyCaptureEnergyStage(Player[] players, ChipLibrary common_chips , int round_count)
            {
                _Players = new List<Player>(players);
                _ChipLibrary = common_chips;
                _RoundCount = round_count;
            }

            void Regulus.Game.IStage.Enter()
            {
                _Decideds = new List<Decided>();
                foreach(var player in _Players)
                {
                    player.OnReadyCaptureEnergy();
                    var decided = new Decided() { Owner = player };                    
                    decided.Owner.UsedChipEvent += decided.OnChips;                    
                    _Decideds.Add(decided);
                }
            }

            void Regulus.Game.IStage.Leave()
            {
                foreach (var decided in _Decideds)
                {
                    decided.Owner.UsedChipEvent -= decided.OnChips;
                }
            }

            void Regulus.Game.IStage.Update()
            {
                var couuent = new System.TimeSpan(_Timeout.Ticks);
                if (couuent.TotalSeconds > 1000 || (from d in  _Decideds where d.IsDecided == true select d).Count() == _Players.Count())
                {
                    if (TimeOutEvent != null)
                    {
                        var players = from d in _Decideds orderby d.ChipCount, d.Owner.Speed select d.Owner;
                        TimeOutEvent(new CaptureEnergyStage(players.ToList(), _ChipLibrary, _RoundCount));
                    }
                    TimeOutEvent = null;
                }
            }

            

            
        }
    }
    
}
