/*using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SusMod.Patches

{
    [HarmonyPatch(nameof(Shovel))]
    internal class ShovelPatch
    {
        [HarmonyPatch("reelUpShovel")]
        [HarmonyPostfix]
        static void OveridePenaltyForImposortors(ref PlayerControllerB ___previousPlayerHeldBy)
        {
            if (___previousPlayerHeldBy.IsLocalPlayer && SusModBase.impostorsIDs.Contains((int)___previousPlayerHeldBy.actualClientId){

                //set previousPlayerHeldBy to false

            }
        }


    }
}*/