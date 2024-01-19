using GameNetcodeStuff;
using HarmonyLib;

namespace TestMod.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    class EnemyAiPatch
    {
        //Larwa get stuck when it fall on impostor (at least on single player)

        [HarmonyPatch(nameof(EnemyAI.PlayerIsTargetable))]
        [HarmonyPostfix]
        static public void MonstersDontTarget(PlayerControllerB playerScript, ref bool __result)
        {
            if (TestModBase.impostorsIDs.Contains((int)playerScript.actualClientId))
            {
                __result = false;
                TestModBase.mls.LogInfo("Player " + playerScript.actualClientId + " is impostor and is not targetable");
            }
        }

        [HarmonyPatch((typeof(EnemyAI)), (nameof(EnemyAI.MeetsStandardPlayerCollisionConditions)))]
        [HarmonyPostfix]
        static public void MeetsStandardPlayerCollisionConditions(ref PlayerControllerB __result)
        {
            try
            {
                if (TestModBase.impostorsIDs.Contains((int)__result.actualClientId))
                {
                    TestModBase.mls.LogInfo("Player " + __result.actualClientId + " is impostor and shouldnt colide with mobs");
                    __result = null;
                }
            }
            catch
            {

            }

        }

        [HarmonyPatch(nameof(EnemyAI.CheckLineOfSightForPlayer))]
        [HarmonyPostfix]
        static public void CheckLineOfSightForPlayerOverwrite(ref PlayerControllerB __result)
        {
            try
            {
                if (TestModBase.impostorsIDs.Contains((int)__result.actualClientId))
                {
                    TestModBase.mls.LogInfo("Player " + __result.actualClientId + " is impostor and should not be in line of sight check");
                    __result = null;
                    //We might have a problem with situation when impostor is first in check line of sight, mob might get stuck then.
                }
            }
            catch
            {

            }

        }

    }

}