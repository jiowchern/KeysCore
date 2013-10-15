using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    class Battler
    {
        public Guid Id { get; set; }

        public BattlerSide Side { get; set; }

        public Pet Pet { get; set; }
    }
    public class Chip
    {

    };

    public interface IZone
    {
        Regulus.Remoting.Value<IBattleAdmissionTickets> Open(BattleRequester requester);        
    }
    
    

    
}

namespace Regulus.Project.Crystal.Game.Stage
{
    class Battle : Regulus.Game.IStage 
        

    {

        public delegate void OnEnd();
        public event OnEnd EndEvent;        
        private AccountInfomation _AccountInfomation;
        private Remoting.ISoulBinder _Binder;
        
        IStorage _Storage;

        Pet _Pet;
        IBattleAdmissionTickets _BattleAdmissionTickets;
        public Battle(IBattleAdmissionTickets battle_admission_tickets, AccountInfomation account_infomation, Remoting.ISoulBinder binder, IStorage stroage)
        {
            _BattleAdmissionTickets = battle_admission_tickets;
            _AccountInfomation = account_infomation;
            _Binder = binder;
        
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
            var value = _BattleAdmissionTickets.Visit(_Pet);
            value.OnValue += _OnStart;
        }

        void _OnStart(IBattleStage battle_stage)
        {
            battle_stage.ReadyCaptureEnergyEvent += _OnReadyCaptureEnergy;
            battle_stage.CaptureEnergyEvent += _OnCaptureEnergy;
            battle_stage.DrawChipEvent += _OnDrawChip;
            battle_stage.EnableChipEvent += _OnEnableChip;
        }

        

        
        IReadyCaptureEnergy _ReadyCaptureEnergy;
        ICaptureEnergy _CaptureEnergy;
        IEnableChip _EnableChip;
        IDrawChip  _DrawChip;
        void _OnReadyCaptureEnergy(IReadyCaptureEnergy obj)
        {
            _ReadyCaptureEnergy = obj;
            _ReadyCaptureEnergy.UsedChipEvent += (chips) => 
            {
                _Binder.Unbind<IReadyCaptureEnergy>(_ReadyCaptureEnergy);
            };

            _Binder.Bind<IReadyCaptureEnergy>(_ReadyCaptureEnergy);
            _Binder.Unbind<ICaptureEnergy>(_CaptureEnergy);
            _Binder.Unbind<IEnableChip>(_EnableChip);
            _Binder.Unbind<IDrawChip>(_DrawChip);
        }

        void _OnCaptureEnergy(ICaptureEnergy obj)
        {
            _CaptureEnergy = obj;

            _Binder.Unbind<IReadyCaptureEnergy>(_ReadyCaptureEnergy);
            _Binder.Bind<ICaptureEnergy>(_CaptureEnergy);
            _Binder.Unbind<IEnableChip>(_EnableChip);
            _Binder.Unbind<IDrawChip>(_DrawChip);
        }
        void _OnDrawChip(IDrawChip obj)
        {
            _DrawChip = obj;

            _Binder.Unbind<IReadyCaptureEnergy>(_ReadyCaptureEnergy);
            _Binder.Unbind<ICaptureEnergy>(_CaptureEnergy);
            _Binder.Unbind<IEnableChip>(_EnableChip);
            _Binder.Bind<IDrawChip>(_DrawChip);
        }
        void _OnEnableChip(IEnableChip obj)
        {
            _EnableChip = obj;

            _Binder.Unbind<IReadyCaptureEnergy>(_ReadyCaptureEnergy);
            _Binder.Unbind<ICaptureEnergy>(_CaptureEnergy);
            _Binder.Bind<IEnableChip>(_EnableChip);
            _Binder.Unbind<IDrawChip>(_DrawChip);
        }
        

        void Regulus.Game.IStage.Leave()
        {
            _Binder.Unbind<IReadyCaptureEnergy>(_ReadyCaptureEnergy);
            _Binder.Unbind<ICaptureEnergy>(_CaptureEnergy);
            _Binder.Unbind<IEnableChip>(_EnableChip);
            _Binder.Unbind<IDrawChip>(_DrawChip);
        }

        void Regulus.Game.IStage.Update()
        {
            
        }

        
    }
}
