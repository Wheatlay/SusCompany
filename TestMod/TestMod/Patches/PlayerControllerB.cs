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

        [HarmonyPatch("Crouch")]
        [HarmonyPostfix]
        static public void GiveImpostorTest()
        {
            int a = 111111111;
            int b = 4;
            StartOfRoundPatch.ImpostorStartGame(ref a,ref b);
        }

        [HarmonyPatch("LandFromJumpClientRpc")]
        [HarmonyPostfix]
        static public void RemoveImposterTest()
        {
            OtherFunctions.RemoveImposter();
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static public void PlayerControllerUpdate()
        {
            IEnumerator<Player> activePlayers = Player.ActiveList.GetEnumerator();
            while (activePlayers.MoveNext())
            {
                if (TestModBase.impostorsIDs.Contains((int)activePlayers.Current.ClientId) && activePlayers.Current.IsControlled && 
                    (TestModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId)))
                {
                    activePlayers.Current.PlayerController.usernameBillboardText.canvasRenderer.SetColor(UnityEngine.Color.red);
                    activePlayers.Current.PlayerController.usernameAlpha.alpha = 1f;
                }
                else
                {
                    activePlayers.Current.PlayerController.usernameBillboardText.canvasRenderer.SetColor(UnityEngine.Color.white);
                }
            }

            try
            {
                if (TestModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
                {
                    Player.LocalPlayer.PlayerController.sprintMeter = 1f;
                    Player.LocalPlayer.PlayerController.nightVision.enabled = true;
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
