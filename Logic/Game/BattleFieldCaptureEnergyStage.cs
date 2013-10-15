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
            class Capturer : ICaptureEnergy
            {
                public Player Player;
                public event Func<Player, int, bool> CaptureEvent;

                public Capturer(Player player)
                {
                    Player = player;
                }
                Remoting.Value<bool> ICaptureEnergy.Capture(int idx)
                {
                    return CaptureEvent(Player, idx);
                }
            }

            public delegate void OnTimeOut(EnableChipStage stage);
            public event OnTimeOut TimeOutEvent;
            Regulus.Utility.TimeCounter _Timeout;
            
            private ChipLibrary _ChipLibrary;
            private int _RoundCount;            

            EnergyGroup[] _Groups;

            Queue<Capturer> _Capturers;
            public CaptureEnergyStage(List<Player> players, ChipLibrary chiplibrary, int roundcount)
            {
                var captures = new List<Capturer>();
                foreach(var player in players )
                {
                    var capture = new Capturer(player);
                    
                    captures.Add(capture);
                }

                _Capturers = new Queue<Capturer>(captures);             
                this._ChipLibrary = chiplibrary;
                this._RoundCount = roundcount;
                _Groups = new EnergyGroup[players.Count + 1];
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
                    Action[] incs = 
                    {
                        energy.IncGreen , energy.IncRed , energy.IncYellow , energy.IncPower
                    };
                    incs[Regulus.Utility.Random.Next(0, 3)]();
                    incs[Regulus.Utility.Random.Next(0, 3)]();
                    incs[Regulus.Utility.Random.Next(0, 3)]();

                    _Groups[i] = new EnergyGroup() { Energy = energy, Round = Regulus.Utility.Random.Next(0, 3) };                    
                }

                _Timeout = new Utility.TimeCounter();

                
            }

            void Regulus.Game.IStage.Leave()
            {
                
            }


            Capturer _CurrentCapturer;
            Regulus.Utility.TimeCounter _CurrentTimeout;
            void Regulus.Game.IStage.Update()
            {
                if (_Capturers.Count > 0)
                {
                    if (_CurrentCapturer == null)
                    {
                        _CurrentCapturer = _Capturers.Dequeue();

                        _CurrentCapturer.Player.OnCaptureEnergy(_CurrentCapturer);

                        _CurrentCapturer.CaptureEvent += _OnCapture;
                        _CurrentTimeout = new Utility.TimeCounter();

                    }
                    var current = new System.TimeSpan(_CurrentTimeout.Ticks);
                    if (current.TotalSeconds > 60)
                    {
                        _CurrentCapturer.CaptureEvent -= _OnCapture;
                        _CurrentCapturer = null;
                    }
                }
                else
                {
                    _ToNext();
                }
                


                var couuent = new System.TimeSpan(_Timeout.Ticks);
                if (couuent.TotalSeconds > 1000)
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
