using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game.Stage
{
    class Adventure : Regulus.Game.IStage, IAdventure
    {
        public delegate void OnBattle();
        public event OnBattle BattleEvent;

        public delegate void OnParking();
        public event OnParking ParkingEvent;
        private Remoting.ISoulBinder _Binder;

        public Adventure(Remoting.ISoulBinder binder)
        {
            // TODO: Complete member initialization
            this._Binder = binder;
            
        }

        void Regulus.Game.IStage.Enter()
        {
            _Binder.Bind<IAdventure>(this);
        }

        void Regulus.Game.IStage.Leave()
        {
            _Binder.Unbind<IAdventure>(this);
        }

        void Regulus.Game.IStage.Update()
        {
            
        }

        void IAdventure.InBattle()
        {
            /*
             * 測試用Function 需要產生對應的戰鬥資訊
             */
            BattleEvent();
        }
    }
}
