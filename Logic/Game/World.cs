using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game
{
    public class World
    {
        Regulus.Project.Crystal.Game.Hall _Hall;
        IStorage _Storage;

        public World(IStorage storage)
        {
            _Storage = storage;
            _Hall = new Hall();
        }

        public void Enter(Regulus.Remoting.ISoulBinder binder)
        {
            _Hall.CreateUser(binder, _Storage);
        }

        public void Update()
        {
            _Hall.Update();
        }
    }
}
