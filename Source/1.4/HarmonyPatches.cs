using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using MFSpacer;

namespace FasterRepairBench
{
    [HarmonyPatch]
    public static class RepairBench_Patch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ThingWithComps), "Tick")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void BaseTick(ThingWithComps instance)
        {
            return;
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Thing), "get_Position")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static IntVec3 BasePosition(Thing instance)
        {
            return IntVec3.Zero;
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Thing), "get_Map")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static Map BaseMap(Thing instance)
        {
            return null;
        }

        [HarmonyPatch(typeof(Building_RepairStored), "Tick")]
        static bool Prefix(Building_RepairStored __instance, CompPowerTrader ___powerComp, bool ___shouldAutoForbid, ref int ___TicksCounted)
        {
            bool isPowered = ___powerComp == null || ___powerComp.PowerOn;
            if (isPowered)
            {
                ___TicksCounted++;
                if (___TicksCounted >= FasterRepairBenchMod.repairInterval)
                {
                    Thing firstItem = BasePosition(__instance).GetFirstItem(BaseMap(__instance));
                    if (firstItem != null) {
                        bool shouldRepair = firstItem.HitPoints != firstItem.MaxHitPoints && (firstItem.def.IsWithinCategory(ThingCategoryDefOf.Weapons) || firstItem.def.IsWithinCategory(ThingCategoryDefOf.Apparel));
                        if (shouldRepair)
                        {
                            firstItem.HitPoints = Math.Min(firstItem.HitPoints + FasterRepairBenchMod.repairStep, firstItem.MaxHitPoints);
                        }

                        if (___shouldAutoForbid)
                        {
                            firstItem.SetForbidden(firstItem.HitPoints < firstItem.MaxHitPoints, true);
                        }
                    }
                    ___TicksCounted = 0;
                }
            }
            BaseTick(__instance);

            return false;
        }
    }
}

