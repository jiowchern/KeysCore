using System;
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


        Regulus.Remoting.Ghost.IProviderNotice<IUserStatus> IUser.StatusProvider
        {
            get { throw new NotImplementedException(); }
        }


        Regulus.Remoting.Ghost.IProviderNotice<IParking> IUser.ParkingProvider
        {
            get { throw new NotImplementedException(); }
        }


        Regulus.Remoting.Ghost.IProviderNotice<IAdventure> IUser.AdventureProvider
        {
            get { throw new NotImplementedException(); }
        }


        Regulus.Remoting.Ghost.IProviderNotice<IReadyCaptureEnergy> IUser.BattleReadyCaptureEnergyProvider
        {
            get { throw new NotImplementedException(); }
        }

        Regulus.Remoting.Ghost.IProviderNotice<ICaptureEnergy> IUser.BattleCaptureEnergyProvider
        {
            get { throw new NotImplementedException(); }
        }




        Regulus.Remoting.Ghost.IProviderNotice<IEnableChip> IUser.BattleEnableChipProvider
        {
            get { throw new NotImplementedException(); }
        }

        Regulus.Remoting.Ghost.IProviderNotice<IDrawChip> IUser.BattleDrawChipProvider
        {
            get { throw new NotImplementedException(); }
        }


        Regulus.Remoting.Ghost.IProviderNotice<IBattler> IUser.BattleProvider
        {
            get { throw new NotImplementedException(); }
        }
    }
}

