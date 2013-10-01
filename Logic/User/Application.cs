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
        static Application.SystemProvider[] providers = new Application.SystemProvider[] 
            {
                new Application.SystemProvider { Command = "standalong" , Build =  _BuildStandalong},
                new Application.SystemProvider { Command = "remoting" , Build = _BuildRemoting}
            };
        public Application(Regulus.Utility.Console.IViewer view, Regulus.Utility.Console.IInput input, Regulus.Game.IFramework[] frameworks)
            : base(view, input, providers, frameworks)
        {
            _View = view;
            _Input = input;
            SystemCreatedEvent += _OnSystemCreated;
        }
        

        static void _BuildStandalong(Application.BuildCompiled build_compiled, Regulus.Game.StageMachine stage_machine)
        {
            build_compiled(Regulus.Project.Crystal.UserGenerator.BuildStandalong());
        }
        static void _BuildRemoting(Application.BuildCompiled build_compiled, Regulus.Game.StageMachine stage_machine)
        {
            build_compiled(Regulus.Project.Crystal.UserGenerator.BuildRemoting());
        }

        void _OnSystemCreated(Regulus.Project.Crystal.IUser system)
        {
            var command = new Regulus.Project.Crystal.UserCommand(system, _View , Command);

        }
    }
}
