using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal
{
    using Regulus.Extension;
    public class UserCommand
    {
        private IUser _System;
        Regulus.Utility.Console.IViewer _View;
        Regulus.Utility.Command _Command;
        System.Collections.Generic.Dictionary<object, Action[]> _RemoveEvents;
        
        public UserCommand(IUser system , Regulus.Utility.Console.IViewer view , Regulus.Utility.Command command)
        {
            _RemoveEvents = new Dictionary<object, Action[]>();
            _System = system;
            _View = view;
            _Command = command;

            _System.VerifyProvider.Supply += _OnVerifySupply ;            
            _System.VerifyProvider.Unsupply += _Unsupply;

            _System.StatusProvider.Supply += _OnStatusSupply;
            _System.StatusProvider.Unsupply += _Unsupply;

            _System.ParkingProvider.Supply += _OnParkingSupply;
            _System.ParkingProvider.Unsupply += _Unsupply;

            _System.AdventureProvider.Supply += _OnAdventureSupply;
            _System.AdventureProvider.Unsupply += _Unsupply;

            _System.BattleProvider.Supply += _OnBattleSupply;
            _System.BattleProvider.Unsupply += _Unsupply;

            _System.BattleReadyCaptureEnergyProvider.Supply += _OnBattleReadyCaptureEnergySupply;
            _System.BattleReadyCaptureEnergyProvider.Unsupply += _Unsupply;

            _System.BattleCaptureEnergyProvider.Supply += _OnBattleCaptureEnergyProviderSupply;
            _System.BattleCaptureEnergyProvider.Unsupply += _Unsupply;

            _System.BattleDrawChipProvider.Supply += _OnBattleDrawChipSupply;
            _System.BattleDrawChipProvider.Unsupply += _Unsupply;

            _System.BattleEnableChipProvider.Supply += _OnBattleEnableChipSupply;
            _System.BattleEnableChipProvider.Unsupply += _Unsupply;
        }
        internal void Release()
        {
            _System.BattleEnableChipProvider.Supply -= _OnBattleEnableChipSupply;
            _System.BattleEnableChipProvider.Unsupply -= _Unsupply;

            _System.VerifyProvider.Supply -= _OnVerifySupply;
            _System.VerifyProvider.Unsupply -= _Unsupply;

            _System.StatusProvider.Supply -= _OnStatusSupply;
            _System.StatusProvider.Unsupply -= _Unsupply;

            _System.ParkingProvider.Supply -= _OnParkingSupply;
            _System.ParkingProvider.Unsupply -= _Unsupply;

            _System.AdventureProvider.Supply -= _OnAdventureSupply;
            _System.AdventureProvider.Unsupply -= _Unsupply;

            _System.BattleProvider.Supply -= _OnBattleSupply;
            _System.BattleProvider.Unsupply -= _Unsupply;

            _System.BattleReadyCaptureEnergyProvider.Supply -= _OnBattleReadyCaptureEnergySupply;
            _System.BattleReadyCaptureEnergyProvider.Unsupply -= _Unsupply;

            _System.BattleCaptureEnergyProvider.Supply -= _OnBattleCaptureEnergyProviderSupply;
            _System.BattleCaptureEnergyProvider.Unsupply -= _Unsupply;

            _System.BattleDrawChipProvider.Supply -= _OnBattleDrawChipSupply;
            _System.BattleDrawChipProvider.Unsupply -= _Unsupply;

            foreach (var command in _RemoveCommands)
            {
                foreach (var cmd in command.Value)
                {
                    _Command.Unregister(cmd);
                }
            }

            foreach (var removerEvent in _RemoveEvents)
            {
                var removers = removerEvent.Value;
                foreach (var remover in removers)
                {
                    remover();
                }
            }
        }


        private void _OnBattleEnableChipSupply(IEnableChip obj)
        {
            _Command.Register<int>("CardLaunched", obj.Enable );
            _Command.Register("Done", obj.Done);

            _RemoveCommands.Add(obj, new string[] 
            {
                "CardLaunched" , "Done"
            });

        }


        private void _OnBattleSupply(IBattle obj)
        {
            obj.PlayingPetEvent += (pet) =>
            {
                _View.WriteLine("選擇的寵物");
                _View.WriteLine(pet.Owner.ToString());
                _View.WriteLine(pet.Energy.Green.ToString());
                _View.WriteLine(pet.Energy.Red.ToString());
                _View.WriteLine(pet.Energy.Yellow.ToString());
            };
        }

        

        void _OnBattleDrawChipSupply(IDrawChip obj)
        {
            
        }

        private void _OnBattleCaptureEnergyProviderSupply(ICaptureEnergy obj)
        {
            _Command.RemotingRegister<EnergyGroup[]>("QueryEnergys", obj.QueryEnergyGroups , (energy_groups) => 
            {
                foreach(var eg in energy_groups)
                {
                    _View.WriteLine("====能源包====");
                    _View.WriteLine("R:" + eg.Energy.Red);
                    _View.WriteLine("Y:" + eg.Energy.Yellow);
                    _View.WriteLine("G:" + eg.Energy.Green);
                    _View.WriteLine("P:" + eg.Energy.Power);
                    
                }
            });
            _Command.RemotingRegister<int, bool>("Capture", obj.Capture, (success) => 
            {
                _View.WriteLine("奪能" + (success? "成功" : "失敗"));
            });

            _RemoveCommands.Add(obj, new string[] 
            {
                "Capture" , "QueryEnergys"
            });
        }

        

        private void _OnBattleReadyCaptureEnergySupply(IReadyCaptureEnergy readycaptureenergy)
        {
            _Command.Register<string>("CoverCard", (param) =>
            {
                var indexs = param.Split(',');
                int[] idxs = (from index in indexs select int.Parse(index)).ToArray();
                readycaptureenergy.UseChip(idxs);
            });
            
            readycaptureenergy.UsedChipEvent += _OnUseChipResult;

            _RemoveEvents.Add(readycaptureenergy, new Action[] { () => { readycaptureenergy.UsedChipEvent -= _OnUseChipResult; } });
            _RemoveCommands.Add( readycaptureenergy , new string[] 
            {
                "CoverCard" 
            });
        }

        void _OnUseChipResult(Battle.Chip[] chips)
        {
            foreach(var chip in chips)
            {
                _View.WriteLine("覆蓋了卡片 " + chip.Name);
            }
            
        }

        
        private void _OnAdventureSupply(IAdventure adventure)
        {
            _Command.RemotingRegister<bool>("InBattle", () => 
            {
                return adventure.InBattle();
            }, (result) =>
            {
                if (result)
                {
                    _View.WriteLine("開始戰鬥.");
                }
                else
                {
                    _View.WriteLine("無法戰鬥. 可能是人數不足兩人");
                }
            });            
            _RemoveCommands.Add(adventure, new string[] 
            {
                "InBattle" 
            });
        }

        private void _OnParkingSupply(IParking parking)
        {
            
            
            Action<ActorInfomation> selectActorReturn = (actorInfomation) =>
            {
                _View.WriteLine("選擇角色 : " + actorInfomation.Id );
                _View.WriteLine("Name : " + actorInfomation.Name);
            };

            _Command.RemotingRegister<string, ActorInfomation>("SelectActor", parking.SelectActor, selectActorReturn);

            _RemoveCommands.Add(parking, new string[] 
            {
                "SelectActor" 
            });
        }

        

        private void _OnStatusSupply(IUserStatus status)
        {
            status.StatusEvent += _OnUserStatusChanged;
        }

        void _OnUserStatusChanged(UserStatus status)
        {
            _View.WriteLine("遊戲狀態改變" + status);
        }

        void _Unsupply<T>(T obj)
        {
            string[] commands;
            if (_RemoveCommands.TryGetValue(obj, out commands))
            {
                foreach (var command in commands)
                {
                    _Command.Unregister(command);
                }
            }

            Action[] removers;
            if (_RemoveEvents.TryGetValue(obj, out removers))
            {
                foreach (var remover in removers)
                {
                    remover();
                }
            }

            _RemoveCommands.Remove(obj);
            _RemoveEvents.Remove(obj);
        }

        System.Collections.Generic.Dictionary<object, string[]> _RemoveCommands = new Dictionary<object, string[]>();
        private void _OnVerifySupply(IVerify obj)
        {
            _Command.RemotingRegister<string,string,bool>("CreateAccount", obj.CreateAccount, (result) => 
            {

            });
            _Command.RemotingRegister<string, string, LoginResult>("Login", obj.Login, (result) => 
            {
                if (result == LoginResult.Success)
                    _View.WriteLine("登入成功.");
                else
                    _View.WriteLine("登入失敗.");
            });
            _Command.Register("Exit" , obj.Quit);

            _RemoveCommands.Add(obj, new string[] 
            {
                "CreateAccount" , "Login" , "Exit"
            });
        }

        
    }
}

