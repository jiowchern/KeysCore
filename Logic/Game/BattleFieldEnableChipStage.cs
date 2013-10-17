using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    partial class Field
    {
        public class EnableChipStage : Regulus.Game.IStage
        {
            public delegate void OnTimeOut(KillingStage stage);
            public event OnTimeOut TimeOutEvent;
            Regulus.Utility.TimeCounter _Timeout;
            private List<Player> _Players;
            private ChipLibrary _ChipLibrary;
            private int _RoundCount;

            public EnableChipStage(List<Player> players, ChipLibrary chiplibrary, int roundcount)
            {                
                this._Players = players;
                this._ChipLibrary = chiplibrary;
                this._RoundCount = roundcount;
            }

            void Regulus.Game.IStage.Enter()
            {
                _Timeout = new Utility.TimeCounter();
            }

            void Regulus.Game.IStage.Leave()
            {
                
            }

            void Regulus.Game.IStage.Update()
            {
                var couuent = new System.TimeSpan(_Timeout.Ticks);
                if (couuent.TotalSeconds > 1000)
                {
                    if (TimeOutEvent != null)
                    {
                        TimeOutEvent(new KillingStage(_Players.ToArray(), _ChipLibrary, _RoundCount));
                    }
                    TimeOutEvent = null;
                }    
            }
        }
    }
}
