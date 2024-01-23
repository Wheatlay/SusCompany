using GameNetcodeStuff;
using HarmonyLib;
using LC_API.GameInterfaceAPI.Features;
using LC_API.Networking;
using LC_API.Networking.Serializers;
using System.Collections.Generic;

internal class Networking
{
    public List <int> ImpostorList { get; set; }
}

namespace SusMod.Patches
{
    class NetworkingPatch
    {
        public static void SynchronizeImpList()
        {
            SusModBase.mls.LogInfo("Sending imp list to clients");
            Network.Broadcast("SyncImpList", new Networking() { ImpostorList=SusModBase.impostorsIDs,});
        }
    }
    
}
