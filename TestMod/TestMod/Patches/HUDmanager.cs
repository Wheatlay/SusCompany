using BepInEx;
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
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {

        public static string LCM = "";

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void AssignLastChatMessage(ref string ___lastChatMessage)
        {
            if (___lastChatMessage.Length > 4)
            {
                LCM = ___lastChatMessage;
            }

        }
    }
}