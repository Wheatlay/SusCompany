using HarmonyLib;
using LC_API.GameInterfaceAPI.Features;
using SusMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMod.Patches
{

    [HarmonyPatch(typeof(StartMatchLever))]
    internal class LeverPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void ImpLever()
        {
            try
            {
                if (SusModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
                {
                    InteractTrigger triggerScript = UnityEngine.Object.FindObjectOfType<StartMatchLever>().triggerScript;
                    triggerScript.interactable = false;
                    triggerScript.disabledHoverTip = "Impostor can't start the ship";
                }
            }
            catch
            {
                SusModBase.mls.LogInfo("Error in LeverPatch");
            }


        }
    }
}
