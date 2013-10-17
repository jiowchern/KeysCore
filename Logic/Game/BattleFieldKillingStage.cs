﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Crystal.Battle
{
    partial class Field
    {
        public class KillingStage : Regulus.Game.IStage
        {
            public delegate void OnEnd(BattlerSide side);
            public event OnEnd EndEvent;

            public delegate void OnReadyCaptureEnergyStage(ReadyCaptureEnergyStage stage);
            public event OnReadyCaptureEnergyStage ReadyCaptureEnergyStageEvent;
            Player[] _Players;
            ChipLibrary _CommonChips;
            int _RoundCount;            
            public KillingStage(Player[] players , ChipLibrary common_chips , int round_count)
            {
                _Players = players;
                _CommonChips = common_chips;
                _RoundCount = round_count;
            }

            
            void Regulus.Game.IStage.Enter()
            {
                
            }

            void Regulus.Game.IStage.Leave()
            {
                
            }

            void Regulus.Game.IStage.Update()
            {
                if (_RoundCount == 0)
                {
                    var winnter = (from player in _Players orderby player.Hp descending select player.Side).FirstOrDefault();
                    EndEvent(winnter);
                }
                else
                {
                    ReadyCaptureEnergyStageEvent(new ReadyCaptureEnergyStage(_Players.ToArray(), _CommonChips, _RoundCount));
                }
            }
        }
    }
    
}