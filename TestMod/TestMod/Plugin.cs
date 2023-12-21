using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("The "+ modName + " mod has awaken");

            harmony.PatchAll();
        }

    }

}
