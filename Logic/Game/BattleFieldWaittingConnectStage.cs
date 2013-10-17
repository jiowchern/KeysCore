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
                public Regulus.Remoting.Value<IBattleStage> ReadyCaptureEnergy;
                public BattlerInfomation Battler;
                public Pet Pet;
            }

            List<ReadyInfomation> _ReadyInfomations;
            
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
                        var cl = _GenerateCommonChipSet();
                        List<Player> players = new List<Player>();
                        foreach (var ri in _ReadyInfomations)
                        {
                            var player = new Player(ri.Pet);
                            ri.ReadyCaptureEnergy.SetValue(player);
                            players.Add(player);
                        }

                        _ReadyEvent(new ReadyCaptureEnergyStage(players.ToArray(), cl , 10));
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

            private ChipLibrary _GenerateCommonChipSet()
            {                
                Chip[] chips = new Chip[] { new Chip() { Id = 1, Yellow = 2, Initiatives = new int[] { 1 }, Passives = new int[] { 2 } }, new Chip() { Id = 2, Red = 1, Initiatives = new int[] { 3 }, Passives = new int[] { 4 } } };
                ChipLibrary cl = new ChipLibrary();
                for (int i = 0; i < 500; ++i )
                {
                    cl.Push(chips[0]);
                    cl.Push(chips[1]);
                }
                cl.Shuffle();
                return cl;
            }

            Regulus.Remoting.Value<IBattleStage> IBattleAdmissionTickets.Visit(Pet pet)
            {
                Regulus.Remoting.Value<IBattleStage> rce = new Remoting.Value<IBattleStage>();
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
        
    }
}
