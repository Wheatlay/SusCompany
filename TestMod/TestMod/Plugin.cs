using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using TestMod.Patches;

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
            harmony.PatchAll();

            //LC_API.GameInterfaceAPI.Events.Handlers.Player.Joined += OtherFunctions.OnJoinedAddOtherClients;
            //LC_API.GameInterfaceAPI.Events.Handlers.Player.Left += OtherFunctions.LocalPlayerDC;
            LC_API.GameInterfaceAPI.Events.Handlers.Player.Dying += OtherFunctions.OnDiedCheckForImpostorVictory;
            LC_API.GameInterfaceAPI.Events.Handlers.Player.Left += OtherFunctions.OnLeftCheckForImpostorVictory;


        }

    }

}
