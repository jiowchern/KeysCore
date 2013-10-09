using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Game
{
    class Map : IMap
    {
        List<IEntity> _Entitys;
        Battle.IZone _Battle;
        public Map(Battle.IZone battle)
        {
            _Battle = battle;
            _Entitys = new List<IEntity>();
        }
        void IMap.Enter(IEntity entity)
        {
            _Entitys.Add(entity);
        }

        void IMap.Leave(IEntity entity)
        {
            _Entitys.Remove(entity);
        }

        void IMap.Battle(Guid requester)
        {
            BattleRequester br = new BattleRequester();
            int size = 0;
            foreach (var entity in _Entitys)
            {
                BattlerInfomation battler = new BattlerInfomation();
                battler.Id = entity.Id;
                battler.Side = (BattlerSide)(size % 2);
                br.Battlers.Add(battler);
                size++;
            }

            _BroadcastBattler(_Battle.Open(br), (from battler in br.Battlers select battler.Id).ToArray());
        }
        private void _BroadcastBattler(Remoting.Value<BattleResponse> value, Guid[] battlers)
        {
            value.OnValue += (response) =>
            {
                foreach (var battler in battlers)
                {
                    _BattleEvent(response.FieldId, battler);
                }
            };
        }

        event OnMapBattle _BattleEvent;
        event OnMapBattle IMap.BattleEvent
        {
            add { _BattleEvent += value; }
            remove { _BattleEvent -= value; }
        }
    }
}
