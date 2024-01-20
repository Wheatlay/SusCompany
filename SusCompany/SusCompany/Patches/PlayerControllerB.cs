using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using LC_API.GameInterfaceAPI.Features;
using System.Linq;
using System.Collections;
using Unity.Netcode;
using static LC_API.GameInterfaceAPI.Features.Player;
using System.Runtime.CompilerServices;

namespace SusMod.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    class PlayerControllerBPatch
    {

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

            //Active list debug
            if (BepInEx.UnityInput.Current.GetKeyDown("F9"))
            {
                SusModBase.mls.LogInfo("F9 pressed");
                IEnumerator<Player> activePlayers2 = Player.ActiveList.GetEnumerator();
                while (activePlayers2.MoveNext())
                {
                    if (activePlayers2.Current.IsLocalPlayer)
                    {
                        SusModBase.mls.LogInfo("Local player " + activePlayers2.Current.ClientId + " is " + activePlayers2.Current.Username);
                    }
                    else
                    {
                        SusModBase.mls.LogInfo("Player " + activePlayers2.Current.ClientId + " is " + activePlayers2.Current.Username);
                    }
                    
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
