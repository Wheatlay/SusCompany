using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using LC_API.GameInterfaceAPI.Features;

namespace TestMod.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    class PlayerControllerBPatch
    {
        static public void GiveImpostorTest()
        {
            int a = 111111111;
            int b = 4;
            if (!TestModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
            {
                StartOfRoundPatch.ImpostorStartGame(ref a, ref b);
            }
        }
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

            //TestModBase.mls.LogInfo("try to get player.localplayer");
            //TestModBase.mls.LogInfo("Player.LocalPlayer is : " + Player.LocalPlayer.name);
            Player.GetOrAdd(StartOfRound.Instance.localPlayerController);
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

            //get impostor status debug
            if (BepInEx.UnityInput.Current.GetKeyDown("F5"))       
            {
                TestModBase.mls.LogInfo("F5 pressed");
                GiveImpostorTest();
            }

            //remove impostor status debug
            if (BepInEx.UnityInput.Current.GetKeyDown("F6"))
            {
                TestModBase.mls.LogInfo("F6 pressed");
                RemoveImposterTest();
            }

            //Active list debug
            if (BepInEx.UnityInput.Current.GetKeyDown("F9"))
            {
                TestModBase.mls.LogInfo("F9 pressed");
                IEnumerator<Player> activePlayers2 = Player.ActiveList.GetEnumerator();
                while (activePlayers2.MoveNext())
                {
                    if (activePlayers2.Current.IsLocalPlayer)
                    {
                        TestModBase.mls.LogInfo("Local player " + activePlayers2.Current.ClientId + " is " + activePlayers2.Current.PlayerController.name);
                    }
                    else
                    {
                        TestModBase.mls.LogInfo("Player " + activePlayers2.Current.ClientId + " is " + activePlayers2.Current.PlayerController.name);
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
