using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMod.Patches
{
    [HarmonyPatch]
    class PlayerControllerBPatch
    {

        [HarmonyPatch(typeof(PlayerControllerB),"Update")]
        [HarmonyPostfix]
        static public void PlayerControllerUpdate(PlayerControllerB __instance)
        {
            if (TestModBase.impostorsIDs.Contains((int)__instance.playerClientId))
            {
                __instance.sprintMeter = 1f;
            }


        }

    }
}


