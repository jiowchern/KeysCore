﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game.Stage
{
	using Regulus.Project.Crystal.Game;
	public class First : Regulus.Game.IStage, Regulus.Project.Crystal.IVerify
	{
		Core _Core;
        public First(Core core)
        {
            _Core = core;
        }
        void Regulus.Game.IStage.Enter()
		{			
			_Core.Binder.Bind<Regulus.Project.Crystal.IVerify>(this);
            
		}

		void Regulus.Game.IStage.Leave()
		{
			_Core.Binder.Unbind<Regulus.Project.Crystal.IVerify>(this);
		}

		void Regulus.Game.IStage.Update()
		{
			
		}

		Regulus.Remoting.Value<bool> IVerify.CreateAccount(string name, string password)
		{
			var val =  _Core.Storage.FindAccountInfomation(name);

			Regulus.Remoting.Value<bool> ret = new Regulus.Remoting.Value<bool>();
			val.OnValue += (AccountInfomation result_account) =>
			{
				if (result_account == null)
				{
					AccountInfomation ai = new AccountInfomation();
					ai.Id = Guid.NewGuid();
					ai.Name = name;
					ai.Password = password;
					_Core.Storage.Add(ai);
					ret.SetValue(true);
				}
				ret.SetValue(false);
			};
			return ret;
		}


        public delegate void OnLoginSuccess(AccountInfomation account_infomation);
        public event OnLoginSuccess LoginSuccessEvent;

		Regulus.Remoting.Value<LoginResult> IVerify.Login(string name, string password)
		{
			Regulus.Remoting.Value<LoginResult> ret = new Regulus.Remoting.Value<LoginResult>();
			var val = _Core.Storage.FindAccountInfomation(name);
			val.OnValue += (account_infomation) =>
			{
				if (account_infomation != null && account_infomation.Password == password)
				{
					ret.SetValue(LoginResult.Success);
                    LoginSuccessEvent(account_infomation);
				}
                else
				    ret.SetValue(LoginResult.Fail);
			};
			return ret;
		}

		void IVerify.Quit()
		{
			
		}
	}
}
