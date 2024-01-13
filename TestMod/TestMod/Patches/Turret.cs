using GameNetcodeStuff;
using HarmonyLib;


namespace TestMod.Patches
{
    [HarmonyPatch(typeof(Turret))]
    class TurretPach
    {
        [HarmonyPatch("CheckForPlayersInLineOfSight")]
        [HarmonyPostfix]
        static public void ExcludeImposterFromLineOfSight(ref PlayerControllerB __result)
        {
            //TestModBase.mls.LogInfo("Enetering CheckForPlayersInLineOfSight");
            //TestModBase.mls.LogInfo("__result.actualClientId) is : " + __result.actualClientId);
            try
            {
                if (TestModBase.impostorsIDs.Contains((int)__result.actualClientId))
                {
                    TestModBase.mls.LogInfo("Player " + __result.actualClientId + " is impostor and is not targetable by turret");
                    __result = null;
                    
                }
            }
            catch 
            { 

            }

        }
    }
}
