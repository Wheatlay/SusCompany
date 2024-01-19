using GameNetcodeStuff;
using System.Collections.Generic;
using LC_API.GameInterfaceAPI.Features;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace TestMod.Patches
{
    class OtherFunctions
    {
        static public void CheckForImpostorVictory()
        {
            TestModBase.mls.LogInfo("Checking for Impostor Victory");
            int aliveCrewMates = 0;
            IEnumerator<Player> activePlayers = Player.ActiveList.GetEnumerator();
            while (activePlayers.MoveNext())
            {
                if (!activePlayers.Current.IsDead && !TestModBase.impostorsIDs.Contains((int)activePlayers.Current.ClientId))
                {
                    aliveCrewMates++;
                }

            }
            TestModBase.mls.LogInfo("aliveCrewMates is : " + aliveCrewMates);
            if (aliveCrewMates == 0)
            {
                TestModBase.mls.LogInfo("Impostors Won");
                StartOfRound.Instance.ShipLeaveAutomatically();
            }
        }

        static public void OnDiedCheckForImpostorVictory(LC_API.GameInterfaceAPI.Events.EventArgs.Player.DyingEventArgs dyingEventArgs)
        {
            CheckForImpostorVictory();
        }
        static public void OnLeftCheckForImpostorVictory(LC_API.GameInterfaceAPI.Events.EventArgs.Player.LeftEventArgs leftEventArgs)
        {
            if (leftEventArgs.Player.IsLocalPlayer)
            {
                TestModBase.mls.LogInfo("Local player has left, Clearing Player.Dictionary");
                Player.Dictionary.Clear();
            }
            CheckForImpostorVictory();
        }

        public static void RemoveImposter()
        {
            TestModBase.impostorsIDs.Clear();
            Player.LocalPlayer.PlayerController.nightVision.intensity = 1000;
            Player.LocalPlayer.PlayerController.nightVision.range = 2000;
            Player.LocalPlayer.PlayerController.nightVision.enabled = false;
            TestModBase.mls.LogInfo("Removing Impostors");

        }

        public static void GetImpostorStartingItem(int ItemNumber,Player player)
        {
            string itemNameIm;

            switch (ItemNumber)
            {
                case 1:
                    itemNameIm = "Shovel";
                    break;
                case 2:
                    itemNameIm = "Tragedy";
                    break;
                case 3:
                    itemNameIm = "Extension ladder";
                    break;
                case 4:
                    itemNameIm = "Zap gun";
                    break;
                case 5:
                    itemNameIm = "Stun grenade";
                    break;
                default:
                    itemNameIm = "";
                    break;
            }
            TestModBase.mls.LogInfo("itemNameIm is:" + itemNameIm);
            TestModBase.mls.LogInfo("ItemNumber is:" + ItemNumber);

            LC_API.GameInterfaceAPI.Features.Item item;
            item = LC_API.GameInterfaceAPI.Features.Item.CreateAndSpawnItem(itemNameIm, false, player.Position, default);

            TestModBase.mls.LogInfo("item is:" + item);

            item.ItemProperties.itemName = "Impostor's " + itemNameIm;
            item.ItemProperties.twoHanded = false;
            item.ItemProperties.isConductiveMetal = false;
            item.ItemProperties.isScrap = false;
            if (itemNameIm == "Tragedy")
            {
                item.ScanNodeProperties.maxRange = 1;
            }
            TestModBase.mls.LogInfo("Testing item name"+item.name);
            TestModBase.mls.LogInfo("Testing player name" + player.name);
            TestModBase.mls.LogInfo("Trying to add item to slot");
            if (player.IsLocalPlayer)
            {
                try
                {
                    Player.LocalPlayer.Inventory.TryAddItemToSlot(item, 3, false);
                }
                catch
                {
                    TestModBase.mls.LogInfo("Failed to add item to LOCAL slot 3 ");
                }
            }
            else
            {
                try
                {
                    player.Inventory.TryAddItemToSlot(item, 3, false);
                }
                catch
                {
                    TestModBase.mls.LogInfo("Failed to add item to slot 3");
                }
            }
        }

        public static void RegisterConsoleCommands()
        {
                TestModBase.mls.LogInfo("Registering console command");
                LC_API.ClientAPI.CommandHandler.RegisterCommand("suschance", (string[] args) =>
                {
                    if (CheckConsoleCommand() && float.TryParse(args[0], out float number) && 0 <= number && number <= 1 )
                    {
                        if (args[0].Contains("."))
                        {
                            args[0] = args[0].Replace(".", ",");
                        }
                        TestModBase.mls.LogInfo("Impostors chance changed to " + args[0].ToString());
                        TestModBase.ConfigimpostorSpawnRate.Value = float.Parse(args[0]);
                        Player.LocalPlayer.QueueTip("Succes", "Impostors chance changed succesfuly to "+ args[0].ToString(), 1f,0, false);
                    }
                    else
                    {
                        TestModBase.mls.LogInfo("Invalid argument");
                        Player.LocalPlayer.QueueTip("Error", "Invalid argument - accepted arguments: Numbers beetween 0-1", 3f, default, true);
                    }

                });
            

                LC_API.ClientAPI.CommandHandler.RegisterCommand("susrandom", (string[] args) =>
                {
                if (CheckConsoleCommand())
                {
                    if (args[0] == "yes" || args[0] == "true" || args[0] == "1")
                        {
                            TestModBase.mls.LogInfo("Impostors Random changed to true");
                            TestModBase.ConfigisImposterCountRandom.Value = true;
                            Player.LocalPlayer.QueueTip("Succes", "Impostors Randomization changed succesfuly to true ", 1f, 0, false);
                        }
                        else if (args[0] == "no" || args[0] == "false" || args[0] == "0")
                        {
                            TestModBase.mls.LogInfo("Impostors Random changed to false");
                            TestModBase.ConfigisImposterCountRandom.Value = false;
                            Player.LocalPlayer.QueueTip("Succes", "Impostors Randomization changed succesfuly to false ", 1f, 0, false);
                        }
                        else
                        {
                            TestModBase.mls.LogInfo("Invalid argument");
                            Player.LocalPlayer.QueueTip("Error", "Invalid argument - accepted arguments: yes , no", 3f, default, true);
                        }
                    }
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
                    TestModBase.mls.LogInfo("You are not the host");
                    Player.LocalPlayer.QueueTip("Error", "You are not the host", 3f, default, true);
                }
            }
            else
            {
                TestModBase.mls.LogInfo("Can't change impostors config while not in orbit");
            }
            return false;
        }
        public static void LocalPlayerDC(LC_API.GameInterfaceAPI.Events.EventArgs.Player.LeftEventArgs leftEventArgs)
        {
            if (leftEventArgs.Player.IsLocalPlayer)
            {
                OtherFunctions.RemoveImposter();
            }
        }
    }
}

