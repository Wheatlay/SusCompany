using GameNetcodeStuff;
using HarmonyLib;
using LC_API.GameInterfaceAPI.Features;
using LC_API.Networking;
using LC_API.Networking.Serializers;
using System.Collections.Generic;

internal class Networking
{
    public float HostImpostorSpawnRate { get; set; }
    public bool isImposterCountRandom { get; set; }
}

namespace SusMod.Patches
{
    [HarmonyPatch(typeof(StartMatchLever))]
    class StartMatchLeverPatch
    {
        [HarmonyPatch("StartGame")]
        [HarmonyPostfix]
        public static void GameStartSynchronizeConfig()
        {
            if (Player.LocalPlayer.IsHost)
            {
                SusModBase.mls.LogInfo("Sending config to clients");
                Network.Broadcast("SyncConfig", new Networking() { HostImpostorSpawnRate = SusModBase.ConfigimpostorSpawnRate.Value,
                    isImposterCountRandom = SusModBase.ConfigisImposterCountRandom.Value});
            }
        }
    }
    
}
