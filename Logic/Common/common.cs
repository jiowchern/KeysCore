
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
    }

}
