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
    [HarmonyPatch(typeof(PlayerControllerB))]
    class PlayerControllerBPatch
    {
        public static bool isImpostor;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static public void PlayerControllerBuPDATE(ref float ___sprintMeter)
        {
            if (isImpostor == true)
            {
                ___sprintMeter = 1f;
            }

            
        }

    }

}


