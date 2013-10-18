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
            public class Battler 
            {
                interface ICommonStage : Regulus.Game.IStage 
                {
                    event Action DoneEvent;
                }
                class EnergyStage : ICommonStage, IRemoveEnergy
                {

                    event Action ICommonStage.DoneEvent
                    {
                        add { throw new NotImplementedException(); }
                        remove { throw new NotImplementedException(); }
                    }

                    void Regulus.Game.IStage.Enter()
                    {
                        throw new NotImplementedException();
                    }

                    void Regulus.Game.IStage.Leave()
                    {
                        throw new NotImplementedException();
                    }

                    void Regulus.Game.IStage.Update()
                    {
                        throw new NotImplementedException();
                    }
                }
                class EnableStage : ICommonStage , IEnableChip
                {
                    Player _Player;
                    public delegate void OnDone();
                    public event OnDone DoneEvent;

                    void IEnableChip.Enable(int index)
                    {
                        
                    }

                    event Action ICommonStage.DoneEvent
                    {
                        add { throw new NotImplementedException(); }
                        remove { throw new NotImplementedException(); }
                    }

                    void Regulus.Game.IStage.Enter()
                    {
                        throw new NotImplementedException();
                    }

                    void Regulus.Game.IStage.Leave()
                    {
                        throw new NotImplementedException();
                    }

                    void Regulus.Game.IStage.Update()
                    {
                        throw new NotImplementedException();
                    }
                }

                Player _Player;
                int _Speed;
                public int Speed { get { return _Speed + _Player.Speed;  } }
                
                Regulus.Game.StageMachine _Machine;
                Queue<ICommonStage> _Stages;
                public Battler(Player player)
                {
                    _Player = player;
                    _Machine = new Regulus.Game.StageMachine();
                    _Stages = new Queue<ICommonStage>();
                }                

                internal void Initial(int speed)
                {
                    _Speed = speed;
                }
                internal void OnStartEnergy()
                {
                    var e = new EnergyStage();
                    _Stages.Enqueue(e);
                }
                internal void OnStartEnable()
                {
                    var e = new EnableStage();
                    _Stages.Enqueue(e);
                }

                internal bool Update()
                {
                    if (_Machine.Update() == false && _Stages.Count() > 0)
                    {
                        var stage = _Stages.Dequeue();
                        stage.DoneEvent += () => 
                        {
                            _Machine.Push(null);
                        };
                        _Machine.Push(stage);
                        return true;
                    }
                    return false;
                }

                
            }
            public delegate void OnTimeOut(KillingStage stage);
            public event OnTimeOut TimeOutEvent;
            Regulus.Utility.TimeCounter _Timeout;            
            private ChipLibrary _ChipLibrary;
            private int _RoundCount;
            Battler[] _Battlers;
            Queue<Battler> _Standby;
            public EnableChipStage(Battler[] battlers, ChipLibrary chiplibrary, int roundcount)
            {
                _Battlers = battlers;
                
                this._ChipLibrary = chiplibrary;
                this._RoundCount = roundcount;
            }

            void Regulus.Game.IStage.Enter()
            {
                
                Queue<int> signs = new Queue<int>(_BuildSigns());
                
                foreach (var battler in _Battlers)
                {
                    battler.Initial(signs.Dequeue());
                }
                _Battlers = (from battler in _Battlers orderby battler.Speed descending select battler).ToArray();
                _Standby = new Queue<Battler>(_Battlers);
                _Timeout = new Utility.TimeCounter();

                
            }

            private static int[] _BuildSigns()
            {
                int[] signs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                for (int i = 0; i < signs.Length; ++i)
                {
                    var idx1 = i;
                    var idx2 = Regulus.Utility.Random.Next(0, signs.Length);
                    var s = signs[idx1];
                    signs[idx1] = signs[idx2];
                    signs[idx2] = s;
                }
                return signs;
            }

            void Regulus.Game.IStage.Leave()
            {
                
            }


            Battler _Current;
            void Regulus.Game.IStage.Update()
            {                
                if (_Current == null && _Standby.Count() > 0)
                {
                    _Current = _Standby.Dequeue();
                    
                }
                
                var couuent = new System.TimeSpan(_Timeout.Ticks);
                if (couuent.TotalSeconds > 1000 || _Current == null)
                {
                    if (TimeOutEvent != null)
                    {
                        TimeOutEvent(new KillingStage(null, _ChipLibrary, _RoundCount));
                    }
                    TimeOutEvent = null;
                }
                else if (_Current.Update())
                {
                    
                    _Current = null;
                }
            }
        }
    }
}
