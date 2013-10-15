using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{

    public partial class Field : Regulus.Game.IFramework
    {

        public class Player : IBattleStage
            , IReadyCaptureEnergy
            
            , IEnableChip
            , IDrawChip
        {
            public int Hp { get; private set; }
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

            public event Action<Chip[]> UsedChipEvent;
            public Player()
            {                
                Hp = 100;
            }

            public Player(Crystal.Pet pet) : this()                
            {                
                this.Pet = pet;
            }
            
            
            Chip[] _UseChip(int[] chip_indexs)
            {
                var usedChips = _EnableChips(chip_indexs);
                
                Queue<Chip> chips = _Supplementary(usedChips.Length);
                _InsertStandby(chips);

                UsedChipEvent(usedChips);
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
                
                if (EnableChips.Count < EnableChipCount)
                {
                    foreach (var index in chip_indexs)
                    {
                        if (index < StandbyChip.Length && StandbyChip[index] != null)
                        {
                            var chip = StandbyChip[index];
                            StandbyChip[index] = null;
                            EnableChips.Add(chip);
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

            internal void OnCaptureEnergy(ICaptureEnergy capturer)
            {
                _CaptureEnergyEvent(capturer);
            }

            internal void OnReadyCaptureEnergy()
            {
                _ReadyCaptureEnergyEvent(this);
            }

            event Action<IReadyCaptureEnergy> _ReadyCaptureEnergyEvent;
            event Action<IReadyCaptureEnergy> IBattleStage.ReadyCaptureEnergyEvent
            {
                add { _ReadyCaptureEnergyEvent += value; }
                remove { _ReadyCaptureEnergyEvent -= value; }
            }

            event Action<ICaptureEnergy> _CaptureEnergyEvent;
            event Action<ICaptureEnergy> IBattleStage.CaptureEnergyEvent
            {
                add { _CaptureEnergyEvent += value; }
                remove { _CaptureEnergyEvent -= value; }
            }

            event Action<IEnableChip> _EnableChipEvent;
            event Action<IEnableChip> IBattleStage.EnableChipEvent
            {
                add { _EnableChipEvent += value; }
                remove { _EnableChipEvent -= value; }
            }

            event Action<IDrawChip> _DrawChipEvent;
            event Action<IDrawChip> IBattleStage.DrawChipEvent
            {
                add { _DrawChipEvent += value; }
                remove { _DrawChipEvent -= value; }
            }

            void IReadyCaptureEnergy.UseChip(int[] chip_indexs)
            {
                _UseChip(chip_indexs);    
            }

            void IEnableChip.Enable(int index)
            {
                throw new NotImplementedException();
            }

            void IDrawChip.Draw(int index)
            {
                throw new NotImplementedException();
            }



            
        }
    }
    
}
