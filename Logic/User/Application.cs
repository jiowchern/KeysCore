using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Regulus.Project.Crystal
{
    public class Application : Regulus.Game.ConsoleFramework<Regulus.Project.Crystal.IUser> 
    {
        Regulus.Utility.Console.IViewer _View;
        Regulus.Utility.Console.IInput _Input;
        static Application.ControllerProvider[] providers = new Application.ControllerProvider[] 
            {
                new Application.ControllerProvider { Command = "standalong" , Spawn =  _BuildStandalong},
                new Application.ControllerProvider { Command = "remoting" , Spawn = _BuildRemoting}
            };

        private static IController _BuildRemoting()
        {
            return null;
        }

        private static IController _BuildStandalong()
        {
            return new Standalong.UserController();
        }
        public Application(Regulus.Utility.Console.IViewer view, Regulus.Utility.Console.IInput input)
            : base(view, input, providers)
        {
            _View = view;
            _Input = input;
            
        }
    }
}
