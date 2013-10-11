using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game.Stage
{
    class Adventure : Regulus.Game.IStage, IAdventure
    {
        public delegate void OnBattle(IBattleAdmissionTickets battle_admission_tickets);
        public event OnBattle ToBattleStageEvent;

        public delegate void OnParking();
        public event OnParking ParkingEvent;
        private Remoting.ISoulBinder _Binder;
        IMap _Map;
        ActorInfomation _ActorInfomation;

        Entity _Entity;
        public Adventure(ActorInfomation actor_infomation , Remoting.ISoulBinder binder, IMap zone)
        {
            _ActorInfomation = actor_infomation;
            _Map = zone;
            
            this._Binder = binder;

            _Entity = new Entity();
        }

        void Regulus.Game.IStage.Enter()
        {
            _Binder.Bind<IAdventure>(this);
            _Map.Enter(_Entity);            
            _Map.BattleResponseEvent += _OnBattleResponse;
            
        }

        void Regulus.Game.IStage.Leave()
        {
            _Map.BattleResponseEvent -= _OnBattleResponse;
            _Map.Leave(_Entity);
            _Binder.Unbind<IAdventure>(this);            
        }


        void _OnBattleResponse(Guid battler, IBattleAdmissionTickets battle_admission_tickets)
        {
            if (_Entity.Id == battler)
            {
                ToBattleStageEvent(battle_admission_tickets);
            }
        }
        

        void Regulus.Game.IStage.Update()
        {
            
        }
        void IAdventure.InBattle()
        {
            _Map.BattleRequest(_Entity.Id);
        }
        
    }
}
