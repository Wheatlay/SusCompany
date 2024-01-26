using GameNetcodeStuff;
using HarmonyLib;
using LC_API.GameInterfaceAPI.Features;
using LC_API.Networking;
using LC_API.Networking.Serializers;
using LCShrinkRay.comp;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

internal class Networking
{
    public List <int> ImpostorList { get; set; }
    public bool Vents { get; set; }
}

namespace SusMod.Patches
{
    class NetworkingPatch
    {
        public static void SynchronizeImpList()
        {
            SusModBase.mls.LogInfo("Sending imp list to clients");
            Network.Broadcast("SyncImpList", new Networking() { ImpostorList=SusModBase.impostorsIDs, Vents = SusModBase.ConfigVents.Value });
            //Also synchronize local config
            if (SusModBase.ConfigVents.Value)
            {
                VentsPatch.SussifyAll();
            }
        }
    }
    
    class ConsoleCommands
    {
        public static void RegisterConsoleCommands()
        {
            SusModBase.mls.LogInfo("Registering console commands");
            LC_API.ClientAPI.CommandHandler.RegisterCommand("susmax", (string[] args) =>
            {
                if (CheckConsoleCommand())
                {
                    if (int.TryParse(args[0], out int number) && number <= 100 && number >= 0)
                    {
                        SusModBase.mls.LogInfo("Impostors chance changed to " + args[0].ToString() + "%");
                        SusModBase.ConfigimpostorSpawnRate.Value = int.Parse(args[0]) / 100;
                        Player.LocalPlayer.QueueTip("Succes", "Impostors chance changed succesfuly to " + SusModBase.ConfigimpostorSpawnRate.Value.ToString(), 1f, 0, false);
                    }
                    else
                    {
                        SusModBase.mls.LogInfo("Invalid argument");
                        Player.LocalPlayer.QueueTip("Error", "Invalid argument - accepted arguments: [0-100%]", 3f, default, true);
                    }
                }
            });


            LC_API.ClientAPI.CommandHandler.RegisterCommand("susrandom", (string[] args) =>
            {
                if (CheckConsoleCommand())
                {
                    if (args[0] == "yes" || args[0] == "true" || args[0] == "1")
                    {
                        SusModBase.mls.LogInfo("Impostors Random changed to true");
                        SusModBase.ConfigisImposterCountRandom.Value = true;
                        Player.LocalPlayer.QueueTip("Succes", "Impostors Randomization changed succesfuly to true ", 1f, 0, false);
                    }
                    else if (args[0] == "no" || args[0] == "false" || args[0] == "0")
                    {
                        SusModBase.mls.LogInfo("Impostors Random changed to false");
                        SusModBase.ConfigisImposterCountRandom.Value = false;
                        Player.LocalPlayer.QueueTip("Succes", "Impostors Randomization changed succesfuly to false ", 1f, 0, false);
                    }
                    else
                    {
                        SusModBase.mls.LogInfo("Invalid argument");
                        Player.LocalPlayer.QueueTip("Error", "Invalid argument - accepted arguments: yes , no", 3f, default, true);
                    }
                }
            });

            LC_API.ClientAPI.CommandHandler.RegisterCommand("susdebug", (string[] args) =>
            {
                SusModBase.DebugMode = true;
                SusModBase.mls.LogInfo("Debug mode enabled");
                Player.LocalPlayer.QueueTip("Succes", "Debug mode enabled", 1f, 0, false);
            });

            LC_API.ClientAPI.CommandHandler.RegisterCommand("susvent", (string[] args) =>
            {
                if (CheckConsoleCommand())
                {
                    if (args[0] == "yes" || args[0] == "true" || args[0] == "1")
                    {
                        SusModBase.mls.LogInfo("Impostors Vents changed to true");
                        SusModBase.ConfigVents.Value = true;
                        Player.LocalPlayer.QueueTip("Succes", "Impostors Vents changed succesfuly to true ", 1f, 0, false);
                    }
                    else if (args[0] == "no" || args[0] == "false" || args[0] == "0")
                    {
                        SusModBase.mls.LogInfo("Impostors Vents changed to false");
                        SusModBase.ConfigVents.Value = false;
                        Player.LocalPlayer.QueueTip("Succes", "Impostors Vents changed succesfuly to false ", 1f, 0, false);
                    }
                    else
                    {
                        SusModBase.mls.LogInfo("Invalid argument");
                        Player.LocalPlayer.QueueTip("Error", "Invalid argument - accepted arguments: yes , no", 3f, default, true);
                    }
                }
            });

            LC_API.ClientAPI.CommandHandler.RegisterCommand("sushelp", (string[] args) =>
            {
                Player.LocalPlayer.QueueTip("HELPING", "/susmax [% of all players] | /susrandom [yes,no] | /susvent [yes,no] ", 5f, 0, false);
            });

        }
        public static bool CheckConsoleCommand()
        {
            if (LC_API.GameInterfaceAPI.GameState.ShipState == LC_API.Data.ShipState.InOrbit)
            {
                if (Player.LocalPlayer.IsHost)
                {
                    return true;
                }
                else
                {
                    SusModBase.mls.LogInfo("You are not the host");
                    Player.LocalPlayer.QueueTip("Error", "You are not the host", 3f, default, true);
                }
            }
            else
            {
                SusModBase.mls.LogInfo("Can't change impostors config while not in orbit");
                Player.LocalPlayer.QueueTip("Error", "Can't change impostors config while not in orbit", 3f, default, true);
            }
            return false;
        }
    }
}
