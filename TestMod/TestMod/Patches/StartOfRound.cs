using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMod.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {


        [HarmonyPatch("OnShipLandedMiscEvents")]
        //[HarmonyPatch("SceneManager_OnLoad")]
        [HarmonyPostfix]
        static void ImpostorStartGame(ref Dictionary<ulong, int> ___ClientPlayerList, ref int ___randomMapSeed, ref int ___thisClientPlayerId, ref UnityEngine.GameObject[] ___allPlayerObjects,ref int ___currentLevelID)
        {   if(___currentLevelID != 3)
            {

                TestModBase.mls.LogInfo("Seed is : " + ___randomMapSeed);

                Random random = new Random(___randomMapSeed);
                int currentImpostorID;
                int impostorsToSpawn;
                List<int> impostorsIDs = new List<int>();

                //Customizable sprawn rate
                float impostorSpawnRate = 0.5f;
                bool isImposterCountRandom = false;

                impostorsToSpawn = (int)(___ClientPlayerList.Count * impostorSpawnRate);

                if (isImposterCountRandom)
                {
                    impostorsToSpawn = random.Next(0, impostorsToSpawn + 1);
                    TestModBase.mls.LogInfo("Spawn rate is randomized");
                }

                while (impostorsIDs.Count < impostorsToSpawn)
                {
                    currentImpostorID = random.Next(0, ___ClientPlayerList.Count);
                    if (!impostorsIDs.Contains(currentImpostorID))
                    {
                        impostorsIDs.Add(currentImpostorID);
                        TestModBase.mls.LogInfo("Player " + currentImpostorID + " is impostor");

                    }
                }
                GameNetcodeStuff.PlayerControllerB playerControler;
                

                if (impostorsIDs.Contains(___thisClientPlayerId))
                {
                    PlayerControllerBPatch.isImpostor = true;
                   // playerControler = ___allPlayerObjects[].GetComponent<PlayerControllerB>();

                    HUDManager.Instance.DisplayTip("Alert", "You Are Impostor!", true, false, "");
                    HUDManager.Instance.AddTextToChatOnServer("I'm the impostor");
                }

            }
        
        }

        [HarmonyPatch(nameof(StartOfRound.ShipHasLeft))]
        [HarmonyPrefix]
        static void ShipLeftRemoveImposter()
        {
            PlayerControllerBPatch.isImpostor = false;
            TestModBase.mls.LogInfo("Removing Impostors");
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void CheckForImposterVictory(ref UnityEngine.GameObject[] ___allPlayerObjects, ref bool ___travellingToNewLevel)
        {
            GameNetcodeStuff.PlayerControllerB playerControler;
            int aliveCrewMates = 0;

            for (int i = 0; i < ___allPlayerObjects.Count(); i++)
            { 
                playerControler = ___allPlayerObjects[i].GetComponent<PlayerControllerB>();
                if (playerControler.isPlayerControlled)
                {
                    TestModBase.mls.LogInfo(playerControler + PlayerControllerBPatch.isImpostor.ToString());

                    if (playerControler.isPlayerDead || !PlayerControllerBPatch.isImpostor)
                    {
                        aliveCrewMates++;
                    }
                }
            }
                    
            if (aliveCrewMates == 0)
            {
                TestModBase.mls.LogInfo("Impostors Won");
            }

        }
        [HarmonyPatch(nameof(StartOfRound.OnLocalDisconnect))]
        [HarmonyPrefix]
        static void onDC()
        {
            PlayerControllerBPatch.isImpostor = false;
            TestModBase.mls.LogInfo("Disconected clearing impostor status");
        }

     }

}
