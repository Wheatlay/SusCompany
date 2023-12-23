using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LC_API.GameInterfaceAPI.Features;

namespace TestMod.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPatch("OnShipLandedMiscEvents")]
        //[HarmonyPatch("SceneManager_OnLoad")]
        [HarmonyPostfix]
        static void ImpostorStartGame(ref Dictionary<ulong, int> ___ClientPlayerList, ref int ___randomMapSeed, ref int ___thisClientPlayerId, ref UnityEngine.GameObject[] ___allPlayerObjects,ref int ___currentLevelID)
        {   if(___currentLevelID != 3)//Level nie jest sklepem
            {

                TestModBase.mls.LogInfo("Seed is : " + ___randomMapSeed);
                TestModBase.impostorsIDs.Clear();

                Random random = new Random(___randomMapSeed);
                int currentImpostorID;
                int impostorsToSpawn;

                //Customizable sprawn rate
                float impostorSpawnRate = 1f;
                bool isImposterCountRandom = false;

                impostorsToSpawn = (int)(Player.ActiveList.Count * impostorSpawnRate);

                if (isImposterCountRandom)
                {
                    impostorsToSpawn = random.Next(0, impostorsToSpawn + 1);
                    TestModBase.mls.LogInfo("Spawn rate is randomized");
                }

                while (TestModBase.impostorsIDs.Count < impostorsToSpawn)
                {
                    currentImpostorID = random.Next(0, Player.ActiveList.Count);

                    if (!TestModBase.impostorsIDs.Contains(currentImpostorID))
                    {
                        TestModBase.impostorsIDs.Add(currentImpostorID);

                        TestModBase.mls.LogInfo("Player " + currentImpostorID + " is impostor");
                    }
                }

                if (TestModBase.impostorsIDs.Contains((int)Player.LocalPlayer.PlayerController.playerClientId))
                {
                    HUDManager.Instance.DisplayTip("Alert", "You Are The Impostor!", true, false, "");
                }
            }
        }

        [HarmonyPatch(nameof(StartOfRound.ShipHasLeft))]
        [HarmonyPrefix]
        static void ShipLeftRemoveImposter()
        {
            TestModBase.impostorsIDs.Clear();
            TestModBase.mls.LogInfo("Removing Impostors");
        }
    }

}
