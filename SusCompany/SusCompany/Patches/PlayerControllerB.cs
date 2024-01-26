using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using LC_API.GameInterfaceAPI.Features;

namespace SusMod.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    class PlayerControllerBPatch
    {
        static float DefSpeed = 0;
        static float DefClimbSpeed = 0;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static public void PlayerControllerUpdate()
        {

            IEnumerator<Player> activePlayers = Player.ActiveList.GetEnumerator();
            while (activePlayers.MoveNext())
            {
                if (SusModBase.impostorsIDs.Contains((int)activePlayers.Current.ClientId) && activePlayers.Current.IsControlled && 
                    (SusModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId)))
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
                if (SusModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
                {
                    Player.LocalPlayer.PlayerController.sprintMeter = 1f;
                    Player.LocalPlayer.PlayerController.nightVision.enabled = true;
                }

            }
            catch
            {
                SusModBase.mls.LogInfo("Failed to get Player.LocalPlayer.ClientId");
            }

            //get impostor status debug
            if (BepInEx.UnityInput.Current.GetKeyDown("F5"))       
            {
                SusModBase.mls.LogInfo("F5 pressed");
                if (SusModBase.DebugMode)
                {
                    int a = 111111111;
                    int b = 4;
                    if (!SusModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
                    {
                        StartOfRoundPatch.ImpostorStartGame(ref a, ref b);
                    }
                }
            }

            //remove impostor status debug
            if (BepInEx.UnityInput.Current.GetKeyDown("F6"))
            {
                SusModBase.mls.LogInfo("F6 pressed");
                if (SusModBase.DebugMode)
                {
                    OtherFunctions.RemoveImposter();
                }
            }

            //give impostor item debug shotgun
            if (BepInEx.UnityInput.Current.GetKeyDown("F7"))
            {
                SusModBase.mls.LogInfo("F7 pressed");
                if (SusModBase.DebugMode)
                {
                    OtherFunctions.GetImpostorStartingItem(7,player:Player.LocalPlayer);
                }
            }


            if (BepInEx.UnityInput.Current.GetKeyDown("F8"))
            {
                SusModBase.mls.LogInfo("F8 pressed");
                if (SusModBase.DebugMode)
                {
                    DefSpeed = Player.LocalPlayer.PlayerController.movementSpeed;
                    Player.LocalPlayer.PlayerController.movementSpeed = 30f;
                    DefClimbSpeed = Player.LocalPlayer.PlayerController.climbSpeed;
                    Player.LocalPlayer.PlayerController.climbSpeed = 100f;
                }
            }

            if (BepInEx.UnityInput.Current.GetKeyDown("F9"))
            {
                SusModBase.mls.LogInfo("F9 pressed");
                if (SusModBase.DebugMode)
                {
                }
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
