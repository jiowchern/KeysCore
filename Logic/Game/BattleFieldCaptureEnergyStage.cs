using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    partial class Field
    {
        public class CaptureEnergyStage : Regulus.Game.IStage
        {
            public class Capturer : ICaptureEnergy
            {
                public Player Player;
                public event Func<Player, int, bool> CaptureEvent;
                int _Difficulty;
                public bool Done { get; private set; }
                public Capturer(Player player, int difficulty)
                {
                    _Difficulty = difficulty;
                    Player = player;
                }

                Remoting.Value<bool> ICaptureEnergy.Capture(int idx)
                {
                    if (Done == false && CaptureEvent(Player, idx))
                    {
                        Done = true;
                        return true;
                    }
                    return false;
                }
            }

            public delegate void OnTimeOut(EnableChipStage stage);
            public event OnTimeOut TimeOutEvent;
            Regulus.Utility.TimeCounter _Timeout;
            
            private ChipLibrary _ChipLibrary;
            private int _RoundCount;            

            EnergyGroup[] _Groups;

            List<Capturer> _Capturers;
            public CaptureEnergyStage(List<Capturer> capturers, ChipLibrary chiplibrary, int roundcount)
            {
                _Capturers = capturers;             
                this._ChipLibrary = chiplibrary;
                this._RoundCount = roundcount;
                _Groups = new EnergyGroup[_Capturers.Count + 1];
            }

            bool _OnCapture(Player player, int idx)
            {
                if (idx < _Groups.Length && _Groups[idx].Owner == Guid.Empty)
                {
                    _Groups[idx].Owner = player.Pet.Owner;
                    return true;
                }
                return false;
            }
            void Regulus.Game.IStage.Enter()
            {                
                for (int i = 0; i < _Groups.Length; ++i )
                {
                    var energy = new Energy(3);
                    var eg = new EnergyGroup() { Energy = energy, Round = Regulus.Utility.Random.Next(0, 3) } ;
                    Action[] incs1 = 
                    {
                        energy.IncGreen , energy.IncRed , energy.IncYellow 
                    };
                    incs1[Regulus.Utility.Random.Next(0, incs1.Length)]();
                    incs1[Regulus.Utility.Random.Next(0, incs1.Length)]();

                    Action[] incs2 = 
                    {
                        energy.IncPower , ()=>{eg.Hp = 1;} , ()=>{eg.Change = 1;}
                    };

                    incs1[Regulus.Utility.Random.Next(0, incs2.Length)]();
                    _Groups[i] = eg ;
                }

                foreach (var capture in _Capturers)
                {
                    capture.Player.OnSpawnCaptureEnergy(capture);                    
                    capture.CaptureEvent += _OnCapture;
                }

                _Timeout = new Utility.TimeCounter();

                
            }

            void Regulus.Game.IStage.Leave()
            {
                foreach (var capture in _Capturers)
                {
                    capture.CaptureEvent -= _OnCapture;
                    capture.Player.OnUnspawnCaptureEnergy();                    
                }
            }


            
            
            void Regulus.Game.IStage.Update()
            {                
                var couuent = new System.TimeSpan(_Timeout.Ticks);
                if (couuent.TotalSeconds > 1000 || (from d in _Capturers where d.Done == true select d).Count() == _Capturers.Count())
                {
                    _ToNext();
                }
            }

            private void _ToNext()
            {
                if (TimeOutEvent != null)
                {
                    TimeOutEvent(new EnableChipStage((from c in _Capturers select c.Player).ToList(), _ChipLibrary, _RoundCount));
                }
                TimeOutEvent = null;
            }
        }
    }
    
}
