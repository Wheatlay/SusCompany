using GameNetcodeStuff;
using LC_API.GameInterfaceAPI.Features;
using SusMod;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace LCShrinkRay.comp
{
    internal class VentsPatch
    {
        private static EnemyVent[] getAllVents()
        {
            if (RoundManager.Instance.allEnemyVents != null && RoundManager.Instance.allEnemyVents.Length > 0)
                return RoundManager.Instance.allEnemyVents;

            return UnityEngine.Object.FindObjectsOfType<EnemyVent>();
        }

        public static void SussifyAll()
        {
            System.Random rnd = new System.Random(StartOfRound.Instance.randomMapSeed);
            if (!SusModBase.ConfigVents.Value)
            {
                SusModBase.mls.LogInfo("Sussification of vents disabled.");
                return;
            }

            if (!SusModBase.impostorsIDs.Contains((int)Player.LocalPlayer.ClientId))
            {
                SusModBase.mls.LogInfo("Sussification of vents only for sussy players.");
                return;
            }

            SusModBase.mls.LogInfo("SUSSIFYING VENTS");

            var vents = getAllVents();
            if (vents == null || vents.Length == 0)
            {
                SusModBase.mls.LogInfo("No vents to sussify.");
                return;
            }

            GameObject dungeonEntrance = GameObject.Find("EntranceTeleportA(Clone)");
            for (int i = 0; i < vents.Length; i++)
            {
                SusModBase.mls.LogInfo("SUSSIFYING VENT " + i);

                int siblingIndex = vents.Length - i - 1;
                if (siblingIndex == i) // maybe "while" instead of "if"?
                {
                    siblingIndex = rnd.Next(0, vents.Length);
                }

                SusModBase.mls.LogInfo("\tPairing with vent " + siblingIndex);

                sussify(vents[i], vents[siblingIndex]);
            }
            coroutines.RenderVents.StartRoutine(dungeonEntrance);
        }

        public static void sussify(EnemyVent enemyVent, EnemyVent siblingVent)
        {
            GameObject vent = enemyVent.gameObject.transform.Find("Hinge").gameObject.transform.Find("VentCover").gameObject;
            if (!vent)
            {
                SusModBase.mls.LogInfo("Vent has no cover to sussify");
                return;
            }

            vent.GetComponent<MeshRenderer>();
            vent.tag = "InteractTrigger";
            vent.layer = LayerMask.NameToLayer("InteractableObject");
            var sussifiedVent = enemyVent.gameObject.AddComponent<SussifiedVent>();
            sussifiedVent.siblingVent = siblingVent;
            var trigger = vent.AddComponent<InteractTrigger>();
            vent.AddComponent<BoxCollider>();

            // Add interaction
            trigger.hoverIcon = GameObject.Find("StartGameLever")?.GetComponent<InteractTrigger>()?.hoverIcon;
            trigger.hoverTip = "Enter : [LMB]";
            trigger.interactable = true;
            trigger.oneHandedItemAllowed = true;
            trigger.twoHandedItemAllowed = true;
            trigger.holdInteraction = true;
            trigger.timeToHold = 1.5f;
            trigger.timeToHoldSpeedMultiplier = 1f;

            // Create new instances of InteractEvent for each trigger
            trigger.holdingInteractEvent = new InteractEventFloat();
            trigger.onInteract = new InteractEvent();
            trigger.onInteractEarly = new InteractEvent();
            trigger.onStopInteract = new InteractEvent();
            trigger.onCancelAnimation = new InteractEvent();

            //checks that we don't set a vent to have itself as a sibling if their is an odd number

            trigger.onInteract.AddListener((player) => sussifiedVent.TeleportPlayer(player));
            trigger.enabled = true;
            vent.GetComponent<Renderer>().enabled = true;
            SusModBase.mls.LogInfo("VentCover Object: " + vent.name);
            SusModBase.mls.LogInfo("VentCover Renderer Enabled: " + vent.GetComponent<Renderer>().enabled);
            SusModBase.mls.LogInfo("Hover Icon: " + (trigger.hoverIcon != null ? trigger.hoverIcon.name : "null"));
        }

        public static void rerenderAllSussified()
        {
            MeshRenderer renderer = GameObject.Find("VentEntrance").gameObject.transform.Find("Hinge").gameObject.transform.Find("VentCover").gameObject.GetComponentsInChildren<MeshRenderer>()[0];
            renderer.enabled = true; // re-enable renderers for all vent covers
        }

        // when unshrinking will be a thing
        public static void unsussifyAll()
        {
            foreach (var vent in getAllVents())
                unsussify(vent);
        }

        public static void unsussify(EnemyVent enemyVent)
        {
            GameObject vent = enemyVent.gameObject.transform.Find("Hinge").gameObject.transform.Find("VentCover").gameObject;
            if (!vent)
                return;

            SusModBase.mls.LogInfo("0");
            if (enemyVent.gameObject.AddComponent<SussifiedVent>() != null)
            {
                SusModBase.mls.LogInfo("1");
                UnityEngine.Object.Destroy(enemyVent.gameObject.AddComponent<SussifiedVent>());
            }
            if (vent.GetComponent<BoxCollider>() != null)
            {
                SusModBase.mls.LogInfo("2");
                UnityEngine.Object.Destroy(vent.GetComponent<BoxCollider>());
            }
            if (vent.GetComponent<InteractTrigger>() != null)
            {
                SusModBase.mls.LogInfo("3");
                UnityEngine.Object.Destroy(vent.GetComponent<InteractTrigger>());
            }
        }


        internal class SussifiedVent : NetworkBehaviour
        {
            public EnemyVent siblingVent { get; set; }

            internal void Start() { }

            internal void TeleportPlayer(PlayerControllerB player)
            {
                Transform transform = player.gameObject.transform;
                //teleport da playa to dis vent
                if (siblingVent != null)
                {
                    //SusModBase.mls.LogInfo("\n⠀⠀⠀⠀⢀⣴⣶⠿⠟⠻⠿⢷⣦⣄⠀⠀⠀\r\n⠀⠀⠀⠀⣾⠏⠀⠀⣠⣤⣤⣤⣬⣿⣷⣄⡀\r\n⠀⢀⣀⣸⡿⠀⠀⣼⡟⠁⠀⠀⠀⠀⠀⠙⣷\r\n⢸⡟⠉⣽⡇⠀⠀⣿⡇⠀⠀⠀⠀⠀⠀⢀⣿\r\n⣾⠇⠀⣿⡇⠀⠀⠘⠿⢶⣶⣤⣤⣶⡶⣿⠋\r\n⣿⠂⠀⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠃\r\n⣿⡆⠀⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀\r\n⢿⡇⠀⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣿⠀\r\n⠘⠻⠷⢿⡇⠀⠀⠀⣴⣶⣶⠶⠖⠀⢸⡟⠀\r\n⠀⠀⠀⢸⣇⠀⠀⠀⣿⡇⣿⡄⠀⢀⣿⠇⠀\r\n⠀⠀⠀⠘⣿⣤⣤⣴⡿⠃⠙⠛⠛⠛⠋⠀⠀");
                    //StartCoroutine(OccupyVent(siblingVent));
                    siblingVent.ventAudio.Play();
                    transform.position = siblingVent.floorNode.transform.position;
                }
                else
                {
                    //7.9186 0.286 -14.1901
                    transform.position = new Vector3(7.9186f, 0.286f, -14.1901f);
                }
            }

            private IEnumerator OccupyVent()
            {
                EnemyVent thisVent = this.transform.parent.gameObject.transform.GetComponent<EnemyVent>();
                thisVent.OpenVentClientRpc();
                thisVent.occupied = true;
                siblingVent.occupied = true;
                float delay = 0.2f;
                yield return new WaitForSeconds(delay);
                thisVent.occupied = false;
                siblingVent.occupied = false;
                siblingVent.OpenVentClientRpc();
            }
        }
    }
}

namespace LCShrinkRay.coroutines
{
    internal class RenderVents : MonoBehaviour
    {
        public GameObject go { get; private set; }

        public static void StartRoutine(GameObject gameObject)
        {
            var routine = gameObject.AddComponent<RenderVents>();
            routine.go = gameObject;
            routine.StartCoroutine(routine.run());
        }

        private IEnumerator run()
        {
            yield return new WaitForSeconds(1f);

            var vents = UnityEngine.Object.FindObjectsOfType<EnemyVent>();
            foreach (var vent in vents)
            {
                var gameObject = vent.gameObject.transform.Find("Hinge").gameObject.transform.Find("VentCover").gameObject;
                if (gameObject == null)
                {
                    SusModBase.mls.LogInfo("A vent gameObject was null.");
                    continue;
                }
                var meshRenderer = gameObject.GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    SusModBase.mls.LogInfo("A vent mesh renderer was null.");
                    continue;
                }

                meshRenderer.enabled = true;
            }
        }
    }
}