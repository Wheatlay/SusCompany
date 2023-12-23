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
    [HarmonyPatch(typeof(PlayerControllerB))]
    class PlayerControllerBPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static public void PlayerControllerUpdate()
        {
            /*  TestModBase.mls.LogInfo("Player.ActiveList.Count is : " + Player.ActiveList.Count);
              IEnumerator<Player> activePlayers = Player.ActiveList.GetEnumerator();
              while (activePlayers.MoveNext())
              {
                  TestModBase.mls.LogInfo("Username is :" + activePlayers.Current.Username + " ClientId is : " + activePlayers.Current.ClientId);
              }
              TestModBase.mls.LogInfo("StartOfRound.Instance.ClientPlayerList.Count is: "+StartOfRound.Instance.ClientPlayerList.Count);*/
            try
            {
                if (TestModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
                {
                    Player.LocalPlayer.PlayerController.sprintMeter = 1f;
                }
            }
            catch
            {
                TestModBase.mls.LogInfo("Failed to get Player.LocalPlayer.ClientId");
            }
        }

        [HarmonyPatch("KillPlayerClientRpc")]
        [HarmonyPostfix]
        static public void KillPlayerClientRpcPatch()
        {
            OtherFunctions.CheckForImpostorVictory();
        }

        [HarmonyPatch("KillPlayerServerRpc")]
        [HarmonyPostfix]
        static public void KillPlayerServerRpcPatch()
        {
            OtherFunctions.CheckForImpostorVictory();
        }

    }
}
