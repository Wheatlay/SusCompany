using GameNetcodeStuff;
using HarmonyLib;


namespace SusMod.Patches
{
    [HarmonyPatch(typeof(Turret))]
    class TurretPach
    {
        [HarmonyPatch("CheckForPlayersInLineOfSight")]
        [HarmonyPostfix]
        static public void ExcludeImposterFromLineOfSight(ref PlayerControllerB __result)
        {
            try
            {
                if (SusModBase.impostorsIDs.Contains((int)__result.actualClientId))
                {
                    SusModBase.mls.LogInfo("Player " + __result.actualClientId + " is impostor and is not targetable by turret");
                    __result = null;
                }
            }
            catch 
            { 

            }

        }
    }
}
