using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LC_API.GameInterfaceAPI.Features;

namespace TestMod.Patches
{
/*    class playerControlerFunctions
    {
        static public void CheckForImpostorVictory()
        {
            TestModBase.mls.LogInfo("KUUUUURWA");
            var startOfRound = StartOfRound.Instance;
            int aliveCrewMates = 0;

            for (int i = 0; i < startOfRound.ClientPlayerList.Count(); i++)
            {
                if (TestModBase.Players[i].isPlayerControlled && !TestModBase.Players[i].isPlayerDead && !TestModBase.impostorsIDs.Contains(i))
                {
                    aliveCrewMates++;
                }
            }
            TestModBase.mls.LogInfo("aliveCrewMates is : " + aliveCrewMates);
            if (aliveCrewMates == 0)
            {
                TestModBase.mls.LogInfo("Impostors Won");
                StartOfRound.Instance.ShipLeaveAutomatically();
            }
        }

    }*/


    [HarmonyPatch(typeof(PlayerControllerB))]
    class PlayerControllerBPatch
    {

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static public void PlayerControllerUpdate()
        {
            TestModBase.mls.LogInfo("Player.LocalPlayer is :" + LC_API.GameInterfaceAPI.Features.Player.LocalPlayer);
            TestModBase.mls.LogInfo("Player.ShipState is :" + LC_API.GameInterfaceAPI.GameState.ShipState);




            //           if (TestModBase.impostorsIDs.Contains((int)__instance.playerClientId))
            //            {
            //                __instance.sprintMeter = 1f;
            //            }

        }


    }
}


