using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserConsole
{
    
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
            Regulus.Game.IFramework app = new Regulus.Project.Crystal.Application(view, input);
            app.Launch();
            while (app.Update())
            {
                input.Update();
            }
            app.Shutdown();
            
       
        }

        



		
		
    }
}
