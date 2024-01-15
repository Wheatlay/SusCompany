using HarmonyLib;
using System;
using System.Linq;
using LC_API.GameInterfaceAPI.Features;
using System.Collections.Generic;
using LC_API.Networking;
using LC_API.Networking.Serializers;
using LC_API;
using System.Collections;
using UnityEngine;


namespace TestMod.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    class StartOfRoundPatch
    {
        [HarmonyPatch("OnShipLandedMiscEvents")]
        //[HarmonyPatch("SceneManager_OnLoad")]
        [HarmonyPostfix]
        public static void ImpostorStartGame(ref int ___randomMapSeed,ref int ___currentLevelID)
        {
            if (___currentLevelID != 3)
            {
                TestModBase.mls.LogInfo("Seed is : " + ___randomMapSeed);
                TestModBase.impostorsIDs.Clear();

                System.Random random = new System.Random(___randomMapSeed);
                int choosenImpostorID;
                int impostorsToSpawn;

                //Customizable sprawn rate
                float impostorSpawnRate = TestModBase.ConfigimpostorSpawnRate.Value;
                bool isImposterCountRandom = TestModBase.ConfigisImposterCountRandom.Value;

                impostorsToSpawn = (int)(Player.ActiveList.Count *impostorSpawnRate);

                TestModBase.mls.LogInfo("Player.ActiveList.Count is : " + Player.ActiveList.Count);
                TestModBase.mls.LogInfo("impostorsToSpawn is : " + impostorsToSpawn);

                if (isImposterCountRandom)
                {
                    impostorsToSpawn = random.Next(0, impostorsToSpawn + 1);
                    TestModBase.mls.LogInfo("Spawn rate is randomized");
                }
                while (TestModBase.impostorsIDs.Count < impostorsToSpawn)
                {
                    List<Player> players = Player.ActiveList.ToList();
                    players.Sort((x, y) => x.ClientId.CompareTo(y.ClientId));
                    choosenImpostorID = (int)players.ElementAt(random.Next(0, players.Count)).ClientId;

                    if (!TestModBase.impostorsIDs.Contains(choosenImpostorID))
                    {
                        TestModBase.impostorsIDs.Add(choosenImpostorID);
                       
                        if (Player.LocalPlayer.IsHost)
                        {
                            OtherFunctions.GetImpostorStartingItem(random.Next(1, 6), Player.ActiveList.FirstOrDefault(p => (int)p.ClientId == choosenImpostorID));

                        }
                        TestModBase.mls.LogInfo("Client ID " + choosenImpostorID + " is impostor");
                    }
                }

                if (TestModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
                {
                    HUDManager.Instance.DisplayTip("Alert", "You Are The Impostor!", true, false, "");
                    Player.LocalPlayer.PlayerController.nightVision.intensity = 3000;
                    Player.LocalPlayer.PlayerController.nightVision.range = 5000;
                }
            }
            
        }

        [HarmonyPatch("ShipHasLeft")]
        [HarmonyPrefix]
        static public void ShipHasLeftPatch()
        {
            OtherFunctions.RemoveImposter();
        }

        [NetworkMessage("SyncConfig")]
        public static void SyncHandler(ulong sender, Networking message)
        {
            TestModBase.mls.LogInfo("Recived config from host");
            TestModBase.mls.LogInfo("HostImpostorSpawnRate is : " + message.HostImpostorSpawnRate);
            TestModBase.mls.LogInfo("isImposterCountRandom is : " + message.isImposterCountRandom);
            TestModBase.ConfigimpostorSpawnRate.Value = message.HostImpostorSpawnRate;
            TestModBase.ConfigisImposterCountRandom.Value = message.isImposterCountRandom;
        }

    }

}
