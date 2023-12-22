using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMod.Patches;
using LC_API;

namespace TestMod
{   
    [BepInPlugin(modGUID, modName, modVersion)]
    public class TestModBase : BaseUnityPlugin
    {
        private const string modGUID = "TotalnieUnikatoweID";
        private const string modName = "SusCompany";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static TestModBase Instance;

        public static ManualLogSource mls;

        public static List<int> impostorsIDs = new List<int>();

        public static int DeadImpostors;
        public static int RecoveredImpostors;



        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("The "+ modName + " mod has awaken");

            harmony.PatchAll(typeof(TestModBase));
            harmony.PatchAll(typeof(StartOfRoundPatch));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            
        }

    }

}
