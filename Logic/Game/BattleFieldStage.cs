using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    partial class Field
    {
        public class WaittingConnectStage : Regulus.Game.IStage , IBattleAdmissionTickets
        {
            public class ReadyInfomation
            {
                public Regulus.Remoting.Value<IReadyCaptureEnergy> ReadyCaptureEnergy;
                public BattlerInfomation Battler;
                public Pet Pet;
            }

            List<ReadyInfomation> _ReadyInfomations;
            ReadyCaptureEnergyStage _Next;
            public delegate void OnReady(ReadyCaptureEnergyStage stage);
            event OnReady _ReadyEvent;
            public event OnReady ReadyEvent { add { _ReadyEvent += value; } remove { } }
            public delegate void OnTimeOut();
            event OnTimeOut _TimeOutEvent;
            public event OnTimeOut TimeOutEvent { add { _TimeOutEvent += value; } remove { } }

            int _Count;
            Regulus.Utility.TimeCounter _Timeout;
            public WaittingConnectStage(BattlerInfomation[] battlerInfomation)
            {
                _Count = 0;
                _ReadyInfomations = new List<ReadyInfomation>();  
                foreach(var battrler in battlerInfomation)
                {
                    _ReadyInfomations.Add(new ReadyInfomation() { Battler = battrler });
                }

                _Timeout = new Utility.TimeCounter();


                _Next = new ReadyCaptureEnergyStage(battlerInfomation.Length);
            }
            
            void Regulus.Game.IStage.Enter()
            {
                                
            }

            void Regulus.Game.IStage.Leave()
            {
                
            }

            void Regulus.Game.IStage.Update()
            {
                if (_Count == _ReadyInfomations.Count)
                {
                    if (_ReadyEvent != null)
                    {
                        foreach (var ri in _ReadyInfomations)
                        {
                            Player plr = new Player();

                            // TODO 初始化player 

                            ri.ReadyCaptureEnergy.SetValue(plr);
                            _Next.AddPlayer(plr);
                        }

                        _ReadyEvent(_Next);
                    }
                    _ReadyEvent = null;
                }
                else
                {
                    var couuent = new System.TimeSpan(_Timeout.Ticks);
                    if (couuent.TotalSeconds > 1000)
                    {
                        if (_TimeOutEvent != null)
                        {
                            _TimeOutEvent();
                        }
                        _TimeOutEvent = null;
                    }
                }
                
            }

            Regulus.Remoting.Value<IReadyCaptureEnergy> IBattleAdmissionTickets.Visit(Pet pet)
            {
                Regulus.Remoting.Value<IReadyCaptureEnergy> rce = new Remoting.Value<IReadyCaptureEnergy>();
                ReadyInfomation ri = (from readyInfomation in _ReadyInfomations where readyInfomation.Battler.Id == pet.Owner && readyInfomation.Pet == null select readyInfomation).FirstOrDefault();
                if (ri != null)
                {                                     
                    ri.Pet = pet;
                    ri.ReadyCaptureEnergy = rce;                    
                    _Count++;
                }
                return rce;
            }
        }


        public class ReadyCaptureEnergyStage : Regulus.Game.IStage
        {
            private ChipLibrary _ChipLibrary;
            private List<Player> _Players;

            public ReadyCaptureEnergyStage(int player_count)
            {
                _ChipLibrary = new ChipLibrary(new Chip[]{}) ;
                
                _Players = new List<Player>();
            }

            void Regulus.Game.IStage.Enter()
            {
                foreach(var player in _Players)
                {
                    
                }
            }

            void Regulus.Game.IStage.Leave()
            {
                
            }

            void Regulus.Game.IStage.Update()
            {
                
            }

            internal void AddPlayer(Player plr)
            {
                
                _Players.Add(plr);
            }
        }
    }
}
