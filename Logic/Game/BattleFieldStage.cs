using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    partial class Field
    {
        public class WaittingConnectStage : Regulus.Game.IStage
        {
            public class ReadyInfomation
            {
                public BattlerInfomation Battler;
                public Pet Pet;
            }

            List<ReadyInfomation> _ReadyInfomations;
            public delegate void OnReady(ReadyInfomation[] pets);
            event OnReady _ReadyEvent;
            public event OnReady ReadyEvent { add { _ReadyEvent += value; } }
            public delegate void OnTimeOut();
            event OnTimeOut _TimeOutEvent;
            public event OnTimeOut TimeOutEvent { add { _TimeOutEvent += value; } }

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

            public void Enter(Pet pet)
            {
                ReadyInfomation ri = (from readyInfomation in _ReadyInfomations where readyInfomation.Battler.Id == pet.Owner && readyInfomation.Pet == null select readyInfomation).FirstOrDefault();
                if (ri != null)
                {
                    ri.Pet = pet;
                    _Count++;                
                }                
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
                        _ReadyEvent(_ReadyInfomations.ToArray());
                    _ReadyEvent = null;
                }
                var couuent = new System.TimeSpan(_Timeout.Ticks);
                if (couuent.TotalSeconds > 10)
                {
                    if (_TimeOutEvent != null)
                    {
                        _TimeOutEvent();
                    }
                    _TimeOutEvent = null;
                }
            }
        }
    }
}
