using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestMod.Patches
    
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        [HarmonyPatch(nameof(HUDManager.ApplyPenalty))]
        [HarmonyPrefix]
        static void OveridePenaltyForImposortors(ref int playersDead,ref int bodiesInsured)
        {
            //playersDead -= TestModBase.DeadImpostors;
            //bodiesInsured -= TestModBase.RecoveredImpostors;
        }


    }
}