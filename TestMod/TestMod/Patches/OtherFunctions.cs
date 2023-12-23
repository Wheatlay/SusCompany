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
    class OtherFunctions
    {
        static public void CheckForImpostorVictory()
        {
            TestModBase.mls.LogInfo("Checking for Impostor Victory");
            int aliveCrewMates = 0;
            IEnumerator<Player> activePlayers = Player.ActiveList.GetEnumerator();
            while (activePlayers.MoveNext())
            {
                if (!activePlayers.Current.IsDead && !TestModBase.impostorsIDs.Contains((int)activePlayers.Current.ClientId))
                {
                    aliveCrewMates++;
                }

            }
            TestModBase.mls.LogInfo("aliveCrewMates is : " + aliveCrewMates);
            if (aliveCrewMates == 0)
            {
                TestModBase.mls.LogInfo("Impostors Won");
                StartOfRound.Instance.ShipLeaveAutomatically();
            }
        }

        // Ten system ssie pento, ale nie potrafie tego przepisac w lepszy sposób
        static public void OnDiedCheckForImpostorVictory(LC_API.GameInterfaceAPI.Events.EventArgs.Player.DyingEventArgs dyingEventArgs)
        {
            CheckForImpostorVictory();
        }
        static public void OnLeftCheckForImpostorVictory(LC_API.GameInterfaceAPI.Events.EventArgs.Player.LeftEventArgs leftEventArgs)
        {
            if (leftEventArgs.Player.IsLocalPlayer)
            {
                TestModBase.mls.LogInfo("Local player has left, Clearing Player.Dictionary");
                Player.Dictionary.Clear();
            }
            CheckForImpostorVictory();
        }
        static public void OnJoinedAddOtherClients(LC_API.GameInterfaceAPI.Events.EventArgs.Player.JoinedEventArgs joinedEventArgs)
        {

            if (joinedEventArgs.Player.IsLocalPlayer && !joinedEventArgs.Player.IsHost)
            {
                TestModBase.mls.LogInfo("Local player joined");

                for (int i = 0; i <= StartOfRound.Instance.allPlayerObjects.Length; i++)
                {
                    if (StartOfRound.Instance.allPlayerObjects[i].GetComponent<PlayerControllerB>().isPlayerControlled)
                    {
                        Player.GetOrAdd(StartOfRound.Instance.allPlayerObjects[i].GetComponent<PlayerControllerB>());
                        TestModBase.mls.LogInfo("Adding other player" + StartOfRound.Instance.allPlayerObjects[i].GetComponent<PlayerControllerB>().name +
                            " [" + StartOfRound.Instance.allPlayerObjects[i].GetComponent<PlayerControllerB>().actualClientId + "]");
                    }

                }
            }
        }
        public static void RemoveImposter()
        {
            TestModBase.impostorsIDs.Clear();
            TestModBase.mls.LogInfo("Removing Impostors");
        }
    }
}
