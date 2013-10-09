using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    public interface IZone
    {
        Regulus.Remoting.Value<BattleResponse> Open(BattleRequester requester);
        
    }


    class Battler
    {
        public Guid Id { get; set; }

        public BattlerSide Side { get; set; }

        public Pet Pet { get; set; }
    }

    class Field
    {
        public Field()
        {
            Id = Guid.NewGuid();
            _Machine = new Regulus.Game.StageMachine();
        }
        public Guid Id { get; private set; }        
        Regulus.Game.StageMachine _Machine;

        internal WaittingConnectStage Begin(BattlerInfomation[] battlerInfomation)
        {
            var rcs = new WaittingConnectStage(battlerInfomation);
            rcs.ReadyEvent += _ToInitialGame;
            _Machine.Push(rcs);
            return rcs;
        }
        public void Update()
        {
            _Machine.Update();
        }
        class Player
        {            
            public BattlerSide Side;
            public Pet Pet;
            public Pet.Energy[] EnergySlots;
            public int Attack;
            public int Defence;
        }
        void _ToInitialGame(Field.WaittingConnectStage.ReadyInfomation[] pets)
        {
            foreach (var pet in pets)
            {
                Player player = new Player();
                player.Pet = pet.Pet;
                player.EnergySlots = 
            }
        }
    }

    public class Zone : IZone
    {
        List<Field> _Fields;
        public Zone()
        {
            _Fields = new List<Field>();
        }
        Remoting.Value<BattleResponse> IZone.Open(BattleRequester requester)
        {
            var field = new Field();
            field.Begin(requester.Battlers.ToArray());
            var response = new BattleResponse();
            response.FieldId = field.Id;
            return response;
        }
               
    }
}

namespace Regulus.Project.Crystal.Game.Stage
{
    class Battle : Regulus.Game.IStage
    {

        public delegate void OnEnd();
        public event OnEnd EndEvent;
        private Guid battle_field;
        private AccountInfomation _AccountInfomation;
        private Remoting.ISoulBinder Binder;
        private Crystal.Battle.IZone _BattleZone;
        IStorage _Storage;

        Pet _Pet;

        public Battle(Guid battle_field, AccountInfomation _AccountInfomation, Remoting.ISoulBinder Binder, Crystal.Battle.IZone battle , IStorage stroage)
        {
            
            this.battle_field = battle_field;
            this._AccountInfomation = _AccountInfomation;
            this.Binder = Binder;
            this._BattleZone = battle;
            _Storage = stroage;
        }
        void Regulus.Game.IStage.Enter()
        {
            var petResult = _Storage.FindPet(_AccountInfomation.Id);
            petResult.OnValue += _OnPetReady;
        }

        
        void _OnPetReady(Pet pet)
        {
            _Pet = pet;
            
        }

        void Regulus.Game.IStage.Leave()
        {
            
        }

        void Regulus.Game.IStage.Update()
        {
            
        }
    }
}
