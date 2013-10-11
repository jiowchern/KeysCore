
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
        Value<ActorInfomation> SelectActor(Guid id);
    };

    public interface IAdventure
    {
        void InBattle();
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
        Value<IReadyCaptureEnergy> Visit(Pet pet);        
    }

    public interface IReadyCaptureEnergy
    {
        void UseChip(int[] chip_indexs);
    }
    

}
