
using System;
namespace Regulus.Project.Crystal
{
    using Regulus.Remoting;
    public interface IUserStatus
    {
        event Action<UserStatus> StatusEvent;
    }
    public interface IVerify
    {        
        Value<bool> CreateAccount(string name, string password);
        Value<LoginResult> Login(string name, string password);
        void Quit();        
    };

    public interface IParking
    {        
        Value<ActorInfomation> SelectActor(string name);
    };

    public interface IAdventure
    {
        Value<bool> InBattle();
    }

    public interface IStorage
    {
        Value<AccountInfomation> FindAccountInfomation(string name);
		
        void Add(AccountInfomation ai);
        void Add(Pet pet);
        
        Value<Pet> FindPet(Guid id);
    }

    public interface IEntity
    {
        Guid Id { get; }
        T QueryAttrib<T>();
    }

    public interface IBattleAdmissionTickets
    {
        Value<IBattleStage> Visit(Pet pet);
    }
    public interface IBattle
    { 

        event Action<Pet> PlayingPetEvent;  
    }

    public interface IBattleStage
    {
        event Action<IReadyCaptureEnergy> SpawnReadyCaptureEnergyEvent;
        event Action<ICaptureEnergy> SpawnCaptureEnergyEvent;
        event Action<IEnableChip> SpawnEnableChipEvent;
        event Action<IDrawChip> SpawnDrawChipEvent;

        event Action    UnspawnReadyCaptureEnergyEvent;
        event Action    UnspawnCaptureEnergyEvent;
        event Action    UnspawnEnableChipEvent;
        event Action    UnspawnDrawChipEvent;
    }

    public interface IBattler
    { 

    }
    public interface IReadyCaptureEnergy
    {
        void UseChip(int[] chip_indexs);
        event Action<Regulus.Project.Crystal.Battle.Chip[]> UsedChipEvent;
    }
    
    public interface ICaptureEnergy
    {
        //EnergyGroup[] EnergyGroups { get; }
        Value<EnergyGroup[]> QueryEnergyGroups();
        Value<bool> Capture(int idx);
    }

    public interface IRemoveEnergy
    { 

    }
    public interface IEnableChip
    {
        void Enable(int index);
        void Done();
    }
    
    public interface IDrawChip
    {

        void Draw(int index);
    }
    

    

}
