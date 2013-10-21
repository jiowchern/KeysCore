using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{

    public partial class Field : Regulus.Game.IFramework
    {

        public class Player : IBattleStage            
        {
            public int Hp { get; set; }
            public const int UsedCardCount = 3;
            public const int EnableChipCount = 5;
            public BattlerSide Side;
            public Pet Pet;
            public Energy Energy;
            public int Attack;
            public int Defence;
            public int Speed;
            // 未使用
            public ChipLibrary SourceChip;
            // 手牌區
            public Chip[] StandbyChip;
            // 啟用區
            public Chip[] EnableChips;
            // 棄牌區
            public ChipLibrary RecycleChip;
            
            public Player()
            {
                
                Hp = 100;
            }

            public Player(Crystal.Pet pet) : this()                
            {                
                this.Pet = pet;
            }

            internal void Initial(ChipLibrary chiplibrary)
            {
                EnableChips = new Chip[EnableChipCount];
                RecycleChip = new ChipLibrary();
                StandbyChip = new Chip[5];
                SourceChip = new ChipLibrary();   

                for (int i = 0; i < 10; ++i )
                {
                    SourceChip.Push(chiplibrary.Pop());
                }
                SourceChip.Shuffle();
                Licensing();
            }

            internal void Licensing()
            {
                _InsertStandby(_Supplementary(StandbyChip.Length));
            }

            public Chip[] UseChip(int[] chip_indexs)
            {
                return _UseChip(chip_indexs);
            }
            Chip[] _UseChip(int[] chip_indexs)
            {
                var usedChips = _EnableChips(chip_indexs);
                Queue<Chip> chips = _Supplementary(usedChips.Length);
                _InsertStandby(chips);
                return usedChips;
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

            private Chip[] _EnableChips(int[] chip_indexs)
            {
                List<Chip> chips = new List<Chip>();
                
                if ( (from ec in EnableChips where ec == null select ec).Count() < EnableChipCount)
                {
                    foreach (var index in chip_indexs)
                    {
                        if (index < StandbyChip.Length && StandbyChip[index] != null)
                        {
                            var chip = StandbyChip[index];
                            StandbyChip[index] = null;

                            for (int i = 0; i < EnableChips.Length; ++i)
                            {
                                if (EnableChips[i] == null)
                                    EnableChips[i] = chip;
                            }                                
                            chips.Add(chip);                
                        }
                    }
                }

                return chips.ToArray();
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

            

            

            public Action<IReadyCaptureEnergy> OnSpawnReadyCaptureEnergy;
            event Action<IReadyCaptureEnergy> IBattleStage.SpawnReadyCaptureEnergyEvent
            {
                add { OnSpawnReadyCaptureEnergy += value; }
                remove { OnSpawnReadyCaptureEnergy -= value; }
            }

            public Action<ICaptureEnergy> OnSpawnCaptureEnergy;
            event Action<ICaptureEnergy> IBattleStage.SpawnCaptureEnergyEvent
            {
                add { OnSpawnCaptureEnergy += value; }
                remove { OnSpawnCaptureEnergy -= value; }
            }

            public Action<IEnableChip> OnSpawnEnableChip;
            event Action<IEnableChip> IBattleStage.SpawnEnableChipEvent
            {
                add { OnSpawnEnableChip += value; }
                remove { OnSpawnEnableChip -= value; }
            }

            public Action<IDrawChip> OnSpawnDrawChip;
            event Action<IDrawChip> IBattleStage.SpawnDrawChipEvent
            {
                add { OnSpawnDrawChip += value; }
                remove { OnSpawnDrawChip -= value; }
            }

            public  Action    OnUnspawnReadyCaptureEnergy;
            event Action    IBattleStage.UnspawnReadyCaptureEnergyEvent
            {
                add { OnUnspawnReadyCaptureEnergy += value; }
                remove { OnUnspawnReadyCaptureEnergy -= value; }
            }

            public  Action    OnUnspawnCaptureEnergy;
            event Action    IBattleStage.UnspawnCaptureEnergyEvent
            {
                add { OnUnspawnCaptureEnergy += value; }
                remove { OnUnspawnCaptureEnergy -= value; }
            }

            public Action OnUnspawnEnableChip;
            event Action    IBattleStage.UnspawnEnableChipEvent
            {
                add { OnUnspawnEnableChip += value; }
                remove { OnUnspawnEnableChip -= value; }
            }

            public Action OnUnspawnDrawChip;
            event Action    IBattleStage.UnspawnDrawChipEvent
            {
                add { OnUnspawnDrawChip += value; }
                remove { OnUnspawnDrawChip -= value; }
            }
        }
    }
    
}
