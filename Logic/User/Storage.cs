using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Standalone
{
	class Storage : Regulus.Project.Crystal.IStorage
	{
        
        public Storage()
        { 

        }
		Regulus.Remoting.Value<AccountInfomation> IStorage.FindAccountInfomation(string name)
		{
			return new AccountInfomation() { Id = Guid.Empty , Name = name , Password = "1" };
		}

		void IStorage.Add(AccountInfomation ai)
		{
			
		}

        Regulus.Remoting.Value<Pet> IStorage.FindPet(Guid id)
        {
            var pet = new Pet() { Id = Guid.NewGuid(), Owner = id };
            pet.Energy = new Energy(7);
            Func<bool>[] energyIncs = new Func<bool>[]{pet.Energy.IncGreen , pet.Energy.IncRed , pet.Energy.IncYellow }; 
            for(int i = 0 ; i < 3 ; ++i)
            {
                energyIncs[Regulus.Utility.Random.Next(0, 3)]();
            }
            pet.Name = Regulus.Utility.Random.Next(0, 1) == 0 ? "蝙蝠" : "甲蟲";
            return pet;
        }

        void IStorage.Add(Pet pet)
        {
            
        }
    }
}

