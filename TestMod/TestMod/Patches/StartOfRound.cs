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
                TestModBase.impostorsIDs.Clear();

                Random random = new Random(___randomMapSeed);
                int currentImpostorID;
                int impostorsToSpawn;

                //Customizable sprawn rate
                float impostorSpawnRate = 0.5f;
                bool isImposterCountRandom = false;

                impostorsToSpawn = (int)(___ClientPlayerList.Count * impostorSpawnRate);

                if (isImposterCountRandom)
                {
                    impostorsToSpawn = random.Next(0, impostorsToSpawn + 1);
                    TestModBase.mls.LogInfo("Spawn rate is randomized");
                }

                while (TestModBase.impostorsIDs.Count < impostorsToSpawn)
                {
                    currentImpostorID = random.Next(0, ___ClientPlayerList.Count);

                    if (!TestModBase.impostorsIDs.Contains(currentImpostorID))
                    {
                        TestModBase.impostorsIDs.Add(currentImpostorID);

                        TestModBase.mls.LogInfo("Player " + currentImpostorID + " is impostor");
                        //playerControler = ___allPlayerObjects[currentImpostorID].GetComponent<PlayerControllerB>();
                        //playerControler.isImpostor = true;

                    }
                }
                
                

                if (TestModBase.impostorsIDs.Contains(___thisClientPlayerId))
                {
                    //playerControler = ___allPlayerObjects[].GetComponent<PlayerControllerB>();
                    HUDManager.Instance.DisplayTip("Alert", "You Are Impostor!", true, false, "");
                    HUDManager.Instance.AddTextToChatOnServer("I'm the impostor");
                }

            }
        
        }

        [HarmonyPatch(nameof(StartOfRound.ShipHasLeft))]
        [HarmonyPrefix]
        static void ShipLeftRemoveImposter()
        {
            //PlayerControllerBPatch.isImpostor = false;
            TestModBase.impostorsIDs.Clear();
            TestModBase.mls.LogInfo("Removing Impostors");
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void CheckForImposterVictory(ref UnityEngine.GameObject[] ___allPlayerObjects)
        {
            GameNetcodeStuff.PlayerControllerB playerControler;
            int aliveCrewMates = 0;

            for (int i = 0; i < ___allPlayerObjects.Count(); i++)
            { 
                playerControler = ___allPlayerObjects[i].GetComponent<PlayerControllerB>();

                if (playerControler.isPlayerControlled && !playerControler.isPlayerDead && !TestModBase.impostorsIDs.Contains(i))
                    {
                        aliveCrewMates++;

                    }
                
            }

            //TestModBase.mls.LogInfo("aliveCrewMates is " +aliveCrewMates);

            if (aliveCrewMates == 0)
            {
                //TestModBase.mls.LogInfo("Impostors Won");
                StartOfRound.Instance.ShipLeaveAutomatically();
            }

        }
    }

}
