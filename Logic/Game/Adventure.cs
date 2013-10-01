using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game.Stage
{
    class Adventure : Regulus.Game.IStage
    {
        public delegate void OnBattle();
        public event OnBattle BattleEvent;

        public delegate void OnParking();
        public event OnParking ParkingEvent;

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
}
