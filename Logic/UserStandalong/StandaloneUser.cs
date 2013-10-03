﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Standalone
{	
	using Regulus.Project.Crystal.Game;
	class User : IUser
	{
        
		
		Regulus.Standalong.Agent _Agent ;
		public User()
		{
            _Agent = new Standalong.Agent();

                
		}

		Regulus.Remoting.Ghost.IProviderNotice<IVerify> IUser.VerifyProvider
		{
			get { return _Agent.QueryProvider<IVerify>(); }
		}

		public void Launch()
		{			
			_Agent.Launch();
            _World.Enter(_Agent);			
		}

		public bool Update()
		{
			_Agent.Update();
			
			return true;
		}

		public void Shutdown()
		{
			_Agent.Shutdown();			
		}
	}
}
