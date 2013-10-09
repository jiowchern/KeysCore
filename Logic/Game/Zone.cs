using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game
{
    public interface IMap
    {
        void Enter(IEntity entity);
        void Leave(IEntity entity);

        void Battle(Guid id);

        event OnMapBattle BattleEvent;
    }
    
    public delegate void OnMapBattle(Guid field, Guid battler);
    

    public class Zone 
    {
        Regulus.Project.Crystal.Game.Hall _Hall;
        IStorage _Storage;
        Battle.Zone _Battle;

        Map _Map;
        public Zone(IStorage storage)
        {
            _Storage = storage;
            _Hall = new Hall();
            _Battle = new Battle.Zone();
            _Map = new Map(_Battle);
        }

        public void Enter(Regulus.Remoting.ISoulBinder binder)
        {
            _Hall.CreateUser(binder, _Storage, _Map , _Battle);
        }

        public void Update()
        {
            _Hall.Update();
        }

       

        
        



        
    }
}
