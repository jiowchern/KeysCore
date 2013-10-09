using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game.Stage
{
    class Adventure : Regulus.Game.IStage, IAdventure
    {
        public delegate void OnBattle(Guid battle_field);
        public event OnBattle BattleEvent;

        public delegate void OnParking();
        public event OnParking ParkingEvent;
        private Remoting.ISoulBinder _Binder;
        IMap _Zone;
        ActorInfomation _ActorInfomation;

        Entity _Entity;
        public Adventure(ActorInfomation actor_infomation , Remoting.ISoulBinder binder, IMap zone)
        {
            _ActorInfomation = actor_infomation;
            _Zone = zone;
            
            this._Binder = binder;

            _Entity = new Entity();
        }

        void Regulus.Game.IStage.Enter()
        {
            _Binder.Bind<IAdventure>(this);
            _Zone.Enter(_Entity);
            _Zone.BattleEvent += _OnBattle;
            

            
            
        }

        void _OnBattle(Guid field, Guid battler)
        {
            if (_Entity.Id == battler)
            {
                BattleEvent(field);
            }
        }

        void Regulus.Game.IStage.Leave()
        {
            _Zone.BattleEvent -= _OnBattle;

            _Zone.Leave(_Entity);
            _Binder.Unbind<IAdventure>(this);            
        }

        

        void Regulus.Game.IStage.Update()
        {
            
        }
        void IAdventure.InBattle()
        {
            _Zone.Battle(_Entity.Id);
            
            
        }
        
    }
}
