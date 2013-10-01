using System;
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
    }
}

