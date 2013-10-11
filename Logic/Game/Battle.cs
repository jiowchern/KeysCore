using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    public interface IZone
    {
        Regulus.Remoting.Value<IBattleAdmissionTickets> Open(BattleRequester requester);
        
    }
    class Battler
    {
        public Guid Id { get; set; }

        public BattlerSide Side { get; set; }

        public Pet Pet { get; set; }
    }
    class Chip
    {

    };
    partial class Field : Regulus.Game.IFramework
    {
        
        public class ChipLibrary
        {
            Queue<Chip> _Chips;

            public int Count { get { return _Chips.Count;  } }
            public ChipLibrary() : this(new Chip[] {})
            { 
            }
            public ChipLibrary(Chip[] chips)
            {
                _Chips = new Queue<Chip>(chips);
                Shuffle();
            }

            public void Shuffle()
            {
                var chips = _Chips.ToArray();
                int[] indexs = new int[chips.Length];
                for (int i = 0; i < chips.Length; ++i)
                {
                    indexs[i] = i;
                }
                for (int i = 0; i < chips.Length; ++i)
                { 
                    var swapIndex = Regulus.Utility.Random.Next(0 , chips.Length);
                    var tempChip = chips[i];
                    chips[i] = chips[swapIndex];
                    chips[swapIndex] = tempChip;
                }
                _Chips = new Queue<Chip>(chips);
            }

            public Chip Pop()
            {
                return _Chips.Dequeue();                   
            }

            public void Push(Chip chip)
            {
                _Chips.Enqueue(chip);
            }
        }

        public class Player : IReadyCaptureEnergy
        {
            public const int EnableChipCount = 5;
            public BattlerSide Side;
            public Pet Pet;
            public Energy Energy;
            public int Attack;
            public int Defence;
            public int Speed;
            public ChipLibrary SourceChip;
            public Chip[] StandbyChip;
            public List<Chip> EnableChips;
            public ChipLibrary RecycleChip;
            public delegate void OnUsedChip (int[] chip_indexs);
            public event OnUsedChip UsedChipEvent;

            
            void IReadyCaptureEnergy.UseChip(int[] chip_indexs)
            {
                int enableCount = _EnableChips(chip_indexs);                
                Queue<Chip> chips = _Supplementary(enableCount);
                _InsertStandby(chips);


            }

            private void _InsertStandby(Queue<Chip> chips)
            {
                for (int i = 0; i < StandbyChip.Length && chips.Count > 0; ++i)
                {
                    if (StandbyChip[i] == null)
                    {
                        StandbyChip[i] = chips.Dequeue();
                    }
                }
            }

            private int _EnableChips(int[] chip_indexs)
            {
                int count = 0;
                if (EnableChips.Count < EnableChipCount)
                {
                    foreach (var index in chip_indexs)
                    {
                        if (index < StandbyChip.Length && StandbyChip[index] != null)
                        {
                            var chip = StandbyChip[index];
                            StandbyChip[index] = null;
                            EnableChips.Add(chip);
                            count++;
                        }
                    }
                }
                return count;
            }

            private Queue<Chip> _Supplementary(int count)
            {
                Queue<Chip> chips = new Queue<Chip>();
                for (int i = 0; i < count; ++i)
                {
                    if (SourceChip.Count == 0)
                    {
                        RecycleChip.Shuffle();
                        SourceChip = RecycleChip;
                        RecycleChip = new ChipLibrary();
                    }

                    chips.Enqueue(SourceChip.Pop());
                }
                return chips;
            }
        }
        public delegate void OnFirst(WaittingConnectStage wcs);
        public OnFirst FirstEvent; 
        public Field(BattlerInfomation[] battlerInfomation)
        {
            Id = Guid.NewGuid();
            _Machine = new Regulus.Game.StageMachine();
            FirstEvent(_Begin(battlerInfomation));
        }
        public Guid Id { get; private set; }        
        Regulus.Game.StageMachine _Machine;
        
        WaittingConnectStage _Begin(BattlerInfomation[] battlerInfomation)
        {
            var rcs = new WaittingConnectStage(battlerInfomation);
            rcs.ReadyEvent += _ToInitialGame;
            _Machine.Push(rcs);
            return rcs;
        }



        void _ToInitialGame(ReadyCaptureEnergyStage stage)
        {
            _Machine.Push(stage);            
        }

        void Regulus.Game.IFramework.Launch()
        {
            
        }

        void Regulus.Game.IFramework.Shutdown()
        {
            
        }

        bool Regulus.Game.IFramework.Update()
        {
            _Machine.Update();
            return true;
        }
    }

    public class Zone : IZone
    {
        Regulus.Game.FrameworkRoot _Fields;
        
        public Zone()
        {
            _Fields = new Regulus.Game.FrameworkRoot();
        }
        Remoting.Value<IBattleAdmissionTickets> IZone.Open(BattleRequester requester)
        {
            Remoting.Value<IBattleAdmissionTickets> ret = new Remoting.Value<IBattleAdmissionTickets>();
            var field = new Field(requester.Battlers.ToArray());
            field.FirstEvent += (val) =>
            {                
                ret.SetValue(val);
            };
            _Fields.AddFramework(field);
            
            return ret;
        }
               
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
        private Crystal.Battle.IZone _BattleZone;
        IStorage _Storage;

        Pet _Pet;
        IBattleAdmissionTickets _BattleAdmissionTickets;
        public Battle(IBattleAdmissionTickets battle_admission_tickets, AccountInfomation account_infomation, Remoting.ISoulBinder binder, Crystal.Battle.IZone battle, IStorage stroage)
        {

            _BattleAdmissionTickets = battle_admission_tickets;
            _AccountInfomation = account_infomation;
            _Binder = binder;
            _BattleZone = battle;
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
            _BattleAdmissionTickets.Visit(_Pet);            
        }

        void Regulus.Game.IStage.Leave()
        {
            
        }

        void Regulus.Game.IStage.Update()
        {
            
        }
    }
}
