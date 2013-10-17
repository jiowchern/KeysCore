using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserConsole
{
    class BatchCommander
    {
        private Regulus.Utility.Command _Command;

        struct CommandString
        { 
            public string Name ;
            public string[] Args;
        }

        Queue<CommandString> _CommandStrings;
        public BatchCommander(Regulus.Utility.Command command)
        {
            // TODO: Complete member initialization
            this._Command = command;

            _CommandStrings = new Queue<CommandString>();
            _CommandStrings.Enqueue(new CommandString() { Name = "standalong" , Args = new string[]{}});
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "spawncontroller", Args = new string[] { "1"} });
            _CommandStrings.Enqueue(new CommandString() { Name = "selectcontroller", Args = new string[] { "1"} });
            _CommandStrings.Enqueue(new CommandString() { Name = "login", Args = new string[] { "1" ,"1" } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });            
            _CommandStrings.Enqueue(new CommandString() { Name = "selectactor", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });            
            _CommandStrings.Enqueue(new CommandString() { Name = "inbattle", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });
            _CommandStrings.Enqueue(new CommandString() { Name = "", Args = new string[] { } });            
            _CommandStrings.Enqueue(new CommandString() { Name = "usecard", Args = new string[] { "1,2"} });
        }


        internal void Update()
        {
            if (_CommandStrings.Count > 0)
            {
                var c = _CommandStrings.Dequeue();
                _Command.Run(c.Name, c.Args);
            }            
        }
    }
    
    class Program
    {
        class Input : Regulus.Utility.ConsoleInput, Regulus.Game.IFramework
        {


            public Input(Regulus.Utility.ConsoleViewer view)
                : base(view)
            {
                // TODO: Complete member initialization
            
            }
            
            void Regulus.Game.IFramework.Launch()
            {
                
            }

            void Regulus.Game.IFramework.Shutdown()
            {
                
            }

            bool Regulus.Game.IFramework.Update()
            {
                base.Update();
                return true;
            }
        }
        static void Main(string[] args)
        {
            var view = new Regulus.Utility.ConsoleViewer();
            var input = new Input(view);

            var application = new Regulus.Project.Crystal.Application(view, input);

            application.UserSpawnEvent += application_UserSpawnEvent;
            
            Regulus.Game.IFramework app = application;

            
            app.Launch();
            var batchCommander = new BatchCommander(application.Command);
            while (app.Update())
            {                
                input.Update();
                batchCommander.Update();                
            }

            app.Shutdown();
            
       
        }

        static void application_UserSpawnEvent(Regulus.Project.Crystal.IUser user)
        {            

        }
		
		
    }
}
