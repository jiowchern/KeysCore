﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal
{
    using Regulus.Extension;
    public class UserCommand
    {
        private IUser _System;
        Regulus.Utility.Console.IViewer _View;
        Regulus.Utility.Command _Command;
        public UserCommand(IUser system , Regulus.Utility.Console.IViewer view , Regulus.Utility.Command command)
        {            
            _System = system;
            _View = view;
            _Command = command;
            _System.VerifyProvider.Supply += _OnVerifySupply ;            
            _System.VerifyProvider.Unsupply += _Unsupply;

            _System.StatusProvider.Supply += _OnStatusSupply;
            _System.StatusProvider.Unsupply += _Unsupply;

            _System.ParkingProvider.Supply += _OnParkingSupply;
            _System.ParkingProvider.Unsupply += _Unsupply;

            _System.AdventureProvider.Supply += _OnAdventureSupply;
            _System.AdventureProvider.Unsupply += _Unsupply;
        }

        private void _OnAdventureSupply(IAdventure adventure)
        {
            _Command.Register("InBattle", adventure.InBattle);
            _Commands.Add(adventure, new string[] 
            {
                "InBattle" 
            });
        }

        private void _OnParkingSupply(IParking parking)
        {
            Func<Regulus.Remoting.Value<ActorInfomation>> selectActor = () =>
            {                
                return parking.SelectActor(Guid.NewGuid());
            };
            
            Action<ActorInfomation> selectActorReturn = (actorInfomation) =>
            {
                _View.WriteLine("選擇角色 : " + actorInfomation.Id );
            };

            _Command.RemotingRegister("SelectActor", selectActor, selectActorReturn);

            _Commands.Add(parking, new string[] 
            {
                "SelectActor" 
            });
        }

        

        private void _OnStatusSupply(IUserStatus status)
        {
            status.StatusEvent += _OnUserStatusChanged;
        }

        void _OnUserStatusChanged(UserStatus status)
        {
            _View.WriteLine("遊戲狀態改變" + status);
        }

        void _Unsupply<T>(T obj)
        {
            string[] commands;
            if (_Commands.TryGetValue(obj, out commands))
            {
                foreach (var command in commands)
                {
                    _Command.Unregister(command);
                }
            }

            _Commands.Remove(obj);
        }

        System.Collections.Generic.Dictionary<object, string[]> _Commands = new Dictionary<object, string[]>();
        private void _OnVerifySupply(IVerify obj)
        {
            _Command.RemotingRegister<string,string,bool>("CreateAccount", obj.CreateAccount, (result) => 
            {

            });
            _Command.RemotingRegister<string, string, LoginResult>("Login", obj.Login, (result) => 
            {
                if (result == LoginResult.Success)
                    _View.WriteLine("登入成功.");
                else
                    _View.WriteLine("登入失敗.");
            });
            _Command.Register("Exit" , obj.Quit);

            _Commands.Add(obj, new string[] 
            {
                "CreateAccount" , "Login" , "Exit"
            });
        }

        internal void Release()
        {
            foreach (var command in _Commands)
            {
                foreach(var cmd in command.Value)
                {
                    _Command.Unregister(cmd);
                }                
            }            
        }
    }
}

