using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Standalone
{
	class Storage : Regulus.Project.Crystal.IStorage
	{
		Regulus.Remoting.Value<AccountInfomation> IStorage.FindAccountInfomation(string name)
		{
			return new AccountInfomation() { Id = Guid.Empty , Name = name , Password = "1" };
		}

		void IStorage.Add(AccountInfomation ai)
		{
			
		}

        Regulus.Remoting.Value<Pet> IStorage.FindPet(Guid id)
        {
            return new Pet();
        }

        void IStorage.Add(Pet pet)
        {
            
        }
    }
}
