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


namespace SusMod.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    class StartOfRoundPatch
    {
        [HarmonyPatch("OnShipLandedMiscEvents")]
        //[HarmonyPatch("SceneManager_OnLoad")]
        [HarmonyPostfix]
        public static void ImpostorStartGame(ref int ___randomMapSeed,ref int ___currentLevelID)
        {
            if (___currentLevelID != 3 && Player.LocalPlayer.IsHost)
            {
                SusModBase.mls.LogInfo("Seed is : " + ___randomMapSeed);
                SusModBase.impostorsIDs.Clear();

                System.Random random = new System.Random(___randomMapSeed);
                int choosenImpostorID;
                int impostorsToSpawn;

                //Customizable sprawn rate
                float impostorSpawnRate = SusModBase.ConfigimpostorSpawnRate.Value;
                bool isImposterCountRandom = SusModBase.ConfigisImposterCountRandom.Value;

                impostorsToSpawn = (int)(Player.ActiveList.Count *impostorSpawnRate);

                SusModBase.mls.LogInfo("Player.ActiveList.Count is : " + Player.ActiveList.Count);
                SusModBase.mls.LogInfo("impostorsToSpawn is : " + impostorsToSpawn);

                if (isImposterCountRandom)
                {
                    impostorsToSpawn = random.Next(0, impostorsToSpawn + 1);
                    SusModBase.mls.LogInfo("Spawn rate is randomized");
                }
                while (SusModBase.impostorsIDs.Count < impostorsToSpawn)
                {
                    List<Player> players = Player.ActiveList.ToList();
                    players.Sort((x, y) => x.ClientId.CompareTo(y.ClientId));
                    choosenImpostorID = (int)players.ElementAt(random.Next(0, players.Count)).ClientId;

                    if (!SusModBase.impostorsIDs.Contains(choosenImpostorID))
                    {
                        SusModBase.impostorsIDs.Add(choosenImpostorID);
                        OtherFunctions.GetImpostorStartingItem(random.Next(1, 6), Player.ActiveList.FirstOrDefault(p => (int)p.ClientId == choosenImpostorID));
                        SusModBase.mls.LogInfo("Client ID " + choosenImpostorID + " is impostor");
                    }
                }
                NetworkingPatch.SynchronizeImpList();
            }
            
        }

        [HarmonyPatch("ShipHasLeft")]
        [HarmonyPrefix]
        static public void ShipHasLeftPatch()
        {
            OtherFunctions.RemoveImposter();
        }

        [NetworkMessage("SyncImpList")]
        public static void SyncHandler(ulong sender, Networking message)
        {
            SusModBase.mls.LogInfo("Recived imp list from host");
            SusModBase.impostorsIDs = message.ImpostorList;
            if (SusModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
            {
                HUDManager.Instance.DisplayTip("Alert", "You Are The Impostor!", true, false, "");
                Player.LocalPlayer.PlayerController.nightVision.intensity = 3000;
                Player.LocalPlayer.PlayerController.nightVision.range = 5000;
            }
        }

    }

}
