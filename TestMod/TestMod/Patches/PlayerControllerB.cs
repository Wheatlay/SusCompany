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

    [HarmonyPatch(typeof(PlayerControllerB))]
    class PlayerControllerBPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static public void PlayerControllerUpdate()
        {   
            if (TestModBase.impostorsIDs.Contains((int)Player.LocalPlayer.PlayerController.playerClientId))
            {
                Player.LocalPlayer.PlayerController.sprintMeter = 1f;
            }
        }

        
        //DODAĆ SUBSCRYPCJE DO LEFT EVENT
        static public void CheckForImpostorVictory(LC_API.GameInterfaceAPI.Events.EventArgs.Player.DyingEventArgs dyingEventArgs)
        {
            int aliveCrewMates = 0;
            IEnumerator<Player> activePlayers = Player.ActiveList.GetEnumerator();
            while (activePlayers.MoveNext())
            {
                if (!activePlayers.Current.IsDead && !TestModBase.impostorsIDs.Contains((int)activePlayers.Current.ClientId))
                {
                    aliveCrewMates++;
                }

            }
            if (aliveCrewMates == 0)
            {
                TestModBase.mls.LogInfo("Impostors Won");
                StartOfRound.Instance.ShipLeaveAutomatically();
            }
        }
    }
}


