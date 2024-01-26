using GameNetcodeStuff;
using System.Collections.Generic;
using LC_API.GameInterfaceAPI.Features;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using LCShrinkRay.comp;

namespace SusMod.Patches
{
    class OtherFunctions
    {
        static public void CheckForImpostorVictory()
        {
            SusModBase.mls.LogInfo("Checking for Impostor Victory");
            int aliveCrewMates = 0;
            IEnumerator<Player> activePlayers = Player.ActiveList.GetEnumerator();
            while (activePlayers.MoveNext())
            {
                if (!activePlayers.Current.IsDead && !SusModBase.impostorsIDs.Contains((int)activePlayers.Current.ClientId))
                {
                    aliveCrewMates++;
                }

            }
            SusModBase.mls.LogInfo("aliveCrewMates is : " + aliveCrewMates);
            if (aliveCrewMates == 0)
            {
                SusModBase.mls.LogInfo("Impostors Won");
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
                SusModBase.mls.LogInfo("Local player has left, Clearing Player.Dictionary");
                Player.Dictionary.Clear();
            }
            CheckForImpostorVictory();
        }

        public static void RemoveImposter()
        {
            SusModBase.impostorsIDs.Clear();
            Player.LocalPlayer.PlayerController.nightVision.intensity = 1000;
            Player.LocalPlayer.PlayerController.nightVision.range = 2000;
            Player.LocalPlayer.PlayerController.nightVision.enabled = false;
            SusModBase.mls.LogInfo("Removing Impostors");
            

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
                case 6:
                    itemNameIm = "Shotgun";
                    break;
                case 7:
                    itemNameIm = "Key";
                    break;
                default:
                    itemNameIm = "";
                    break;
            }
            SusModBase.mls.LogInfo("itemNameIm is:" + itemNameIm);
            SusModBase.mls.LogInfo("ItemNumber is:" + ItemNumber);

            LC_API.GameInterfaceAPI.Features.Item item;
            item = LC_API.GameInterfaceAPI.Features.Item.CreateAndSpawnItem(itemNameIm, true, player.Position, default);
            //item = LC_API.GameInterfaceAPI.Features.Item.CreateAndGiveItem(itemNameIm, player, true, false);
            SusModBase.mls.LogInfo("item is:" + item);

            item.ItemProperties.itemName = "Impostor's " + itemNameIm;
            item.ItemProperties.twoHanded = false;
            item.ItemProperties.isConductiveMetal = false;
            item.ItemProperties.isScrap = false;
            if (itemNameIm == "Tragedy")
            {
                item.ScanNodeProperties.maxRange = 1;
            }
            SusModBase.mls.LogInfo("Testing item name"+item.name);
            SusModBase.mls.LogInfo("Testing player name" + player.name);
            SusModBase.mls.LogInfo("Trying to add item to slot");
            if (player.IsLocalPlayer)
            {
                try
                {
                    Player.LocalPlayer.Inventory.TryAddItemToSlot(item, 3, false);
                }
                catch
                {
                    SusModBase.mls.LogInfo("Failed to add item to LOCAL slot 3 ");
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
                    SusModBase.mls.LogInfo("Failed to add item to slot 3");
                }
            }
        }

        
        
    }
}

