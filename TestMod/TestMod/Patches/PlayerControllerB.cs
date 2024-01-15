using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using LC_API.GameInterfaceAPI.Features;
using System.Linq;
using System.Collections;
using Unity.Netcode;
using static LC_API.GameInterfaceAPI.Features.Player;
using System.Runtime.CompilerServices;

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
                        TestModBase.mls.LogInfo("Local player " + activePlayers2.Current.ClientId + " is " + activePlayers2.Current.Username);
                    }
                    else
                    {
                        TestModBase.mls.LogInfo("Player " + activePlayers2.Current.ClientId + " is " + activePlayers2.Current.Username);
                    }
                    
                }


            }

            //give impostor item debug
            if (BepInEx.UnityInput.Current.GetKeyDown("F10"))
            {
                TestModBase.mls.LogInfo("F10 pressed");

                TestModBase.mls.LogInfo("Giving item 1");
                TestModBase.mls.LogInfo(Player.LocalPlayer.name);
                OtherFunctions.GetImpostorStartingItem(1,Player.LocalPlayer);
            }
            if (BepInEx.UnityInput.Current.GetKeyDown("F11"))
            {
                TestModBase.mls.LogInfo("F11 pressed");

                TestModBase.mls.LogInfo("Giving item 2");
                OtherFunctions.GetImpostorStartingItem(2, Player.LocalPlayer);
            }
            if (BepInEx.UnityInput.Current.GetKeyDown("F12"))
            {
                TestModBase.mls.LogInfo("F12 pressed");

                TestModBase.mls.LogInfo("Giving item 3");
                OtherFunctions.GetImpostorStartingItem(3, Player.LocalPlayer);
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
