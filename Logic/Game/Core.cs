using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game
{
	public class Core : Regulus.Game.IFramework
	{
		Regulus.Remoting.ISoulBinder _Binder;
		public IStorage	Storage {get ; private set;}

		public Regulus.Remoting.ISoulBinder Binder { get { return _Binder; }}
		Regulus.Game.StageMachine _StageMachine;
		public Core(Regulus.Remoting.ISoulBinder binder , IStorage storage)
		{
			Storage = storage;
			_Binder = binder;
			_StageMachine = new Regulus.Game.StageMachine();

			binder.BreakEvent += _OnInactive;
		}
		~Core()
		{
			_Binder.BreakEvent -= _OnInactive;
		}
		void _OnInactive()
		{
			if (InactiveEvent != null)
				InactiveEvent();			
		}

		public void Launch()
		{
			_ToFirst();
		}

		void _ToFirst()
		{
            var first = new Regulus.Project.Crystal.Game.Stage.First(this);
			_StageMachine.Push( first );
            first.LoginSuccessEvent += _ToParking;
		}

        AccountInfomation _AccountInfomation;
        void _ToParking(AccountInfomation account_infomation)
        {
            var stage = new Regulus.Project.Crystal.Game.Stage.Parking(account_infomation);
            stage.SelectCompiledEvent += _ToAdventure;
            stage.VerifyEvent += _ToFirst;
            _StageMachine.Push(stage);
            _AccountInfomation = account_infomation;
        }

        ActorInfomation _ActorInfomation;
        void _ToAdventure(ActorInfomation actor_infomation)
        {
            
            var stage = new Regulus.Project.Crystal.Game.Stage.Adventure();
            stage.BattleEvent += _ToBattle;
            stage.ParkingEvent += () => { _ToParking(_AccountInfomation); };
            _StageMachine.Push(stage);

            _ActorInfomation = actor_infomation;
        }

        void _ToBattle()
        {
            var stage = new Regulus.Project.Crystal.Game.Stage.Battle();
            stage.EndEvent += () =>
            {
                _ToAdventure(_ActorInfomation);
            };
            _StageMachine.Push(stage);
        }

		public bool Update()
		{
			_StageMachine.Update();
			return true;
		}
		public void Shutdown()
		{
			_StageMachine.Termination();
		}

		public event Action InactiveEvent;
	}
}
