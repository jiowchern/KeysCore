﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Remoting
{
	class User : IUser
	{
		private Regulus.Remoting.Ghost.Agent _Complex { get; set; }
		public void Update()
		{
		    	
		}

        Regulus.Remoting.Ghost.IProviderNotice<IVerify> IUser.VerifyProvider
        {
            get { throw new NotImplementedException(); }
        }

        void Regulus.Game.IFramework.Launch()
        {
            throw new NotImplementedException();
        }

        void Regulus.Game.IFramework.Shutdown()
        {
            throw new NotImplementedException();
        }

        bool Regulus.Game.IFramework.Update()
        {
            throw new NotImplementedException();
        }
    }
}

