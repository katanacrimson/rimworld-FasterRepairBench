using System;
using System.Collections.Generic;
using HugsLib;
using HugsLib.Settings;
using RimWorld;
using UnityEngine;
using Verse;

namespace FasterRepairBench
{
    public class FasterRepairBenchMod : ModBase
    {
        public static int repairStep;
        public static int repairInterval;

        private SettingHandle<int> repairStepSetting;

        public override string ModIdentifier
        {
            get
            {
                return "FasterRepairBench";
            }
        }

        public override void DefsLoaded()
        {
            this.repairStepSetting = base.Settings.GetHandle<int>("frb_repairstep", "Repair Amount", "How much should integrity the repair bench restore at a time", 1, null, null);
            this.SettingsChanged();
        }

        public override void SettingsChanged()
        {
            FasterRepairBenchMod.repairStep = Math.Max(0, Math.Min(500, this.repairStepSetting.Value));
            FasterRepairBenchMod.repairInterval = 2500; // hardcoded for now
        }
    }
}
