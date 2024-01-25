using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LC_API.Networking;
using System.Collections.Generic;
using SusMod.Patches;

namespace SusMod
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class SusModBase : BaseUnityPlugin
    {
        private const string modGUID = "Sussy";
        private const string modName = "SusCompany";
        private const string modVersion = "1.0.3";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static SusModBase Instance;
        public static ManualLogSource mls;
        public static List<int> impostorsIDs = new List<int>();
        public static bool DebugMode = false;
        public static ConfigEntry<float> ConfigimpostorSpawnRate;
        public static ConfigEntry<bool> ConfigisImposterCountRandom;
        public static ConfigEntry<bool> ConfigVents;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            ConfigimpostorSpawnRate = Config.Bind("General", "SpawnRate", 0.25f, "Spawn rate of impostors");
            ConfigisImposterCountRandom = Config.Bind("General.Toggles", "IsRandomSpawnRate", true, "If true, impostor spawn rate will be randomized between 0 and SpawnRate");
            ConfigVents = Config.Bind("General.Toggles", "AreVentsOn", true, "If true, impostor is albe to teleports beetwen vents");

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("The "+ modName + " mod has awaken");
            harmony.PatchAll();

            LC_API.GameInterfaceAPI.Events.Handlers.Player.Dying += OtherFunctions.OnDiedCheckForImpostorVictory;
            LC_API.GameInterfaceAPI.Events.Handlers.Player.Left += OtherFunctions.OnLeftCheckForImpostorVictory;
            Network.RegisterAll();
            ConsoleCommands.RegisterConsoleCommands();


        }

    }

}
