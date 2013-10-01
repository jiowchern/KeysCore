using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game.Stage
{
    class Parking : Regulus.Game.IStage, Regulus.Project.Crystal.IParking
    {
        public delegate void OnSelectCompiled(ActorInfomation actor_infomation);
        public event OnSelectCompiled SelectCompiledEvent;

        public delegate void OnVerify();
        public event OnVerify VerifyEvent;

        private AccountInfomation _AccountInfomation;

        public Parking(AccountInfomation account_infomation)
        {
            // TODO: Complete member initialization
            this._AccountInfomation = account_infomation;
        }
        void Regulus.Game.IStage.Enter()
        {
            
        }

        void Regulus.Game.IStage.Leave()
        {
            
        }

        void Regulus.Game.IStage.Update()
        {
            
        }

        event Action<ActorInfomation> IParking.ActorInfomationEvent
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        void IParking.SelectActor(Guid id)
        {
            
        }
    }
}
