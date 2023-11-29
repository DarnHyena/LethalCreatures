using GameNetcodeStuff;
using HarmonyLib;
using System.Diagnostics;
using System.Numerics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CreatureModels.Patches
{

    [HarmonyPatch]
    internal class PlayerObjects
    {
        public static void InitModels()
        {
            var localPlayer = GameNetworkManager.Instance.localPlayerController;
            Debug.Log(localPlayer.gameObject.name);
            var players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>();
            foreach (var player in players)
            {
                if (player == localPlayer)
                {

                }
                else
                {
                    var Creature = player.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>().ToList().Find(x => x.name.Contains("Body"));
                    if (Creature != null) continue;
                    player.gameObject.AddComponent<LethalCreature.CreatureController>();
                }
            }
        }


        [HarmonyPatch(typeof(PlayerControllerB), "SpawnPlayerAnimation")]
        [HarmonyPostfix]
        public static void InitModel(ref PlayerControllerB __instance)
        {
            InitModels();
        }

        // Hides the vanilla player
        [HarmonyPatch(typeof(PlayerControllerB), "DisablePlayerModel")]
        [HarmonyPostfix] 
        public static void DisablePlayerModel(ref PlayerControllerB __instance, GameObject playerObject)
        {
            var localPlayer = GameNetworkManager.Instance.localPlayerController;
            //if (playerObject == localPlayer) return;
            playerObject.gameObject.GetComponentInChildren<LODGroup>().enabled = false;
            var meshes = playerObject.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var LODmesh in meshes)
            {
                if (LODmesh.name == "Body") continue;
                LODmesh.enabled = false;
            }

        }
    }

    //Outfits
    [HarmonyPatch(typeof(UnlockableSuit))]
    internal class UnlockableSuitPatch
    {
        [HarmonyPatch("SwitchSuitForPlayer")]
        [HarmonyPrefix]
        static void SwitchSuitForPlayerPatch(PlayerControllerB player, int suitID, bool playAudio = true)
        {
            Texture tex;
            switch (suitID)
            {
                case 0:
                    tex = AssetHandler.TexBase01;
                    break;
                case 1:
                    tex = AssetHandler.TexBase02;
                    break;
                case 2:
                    tex = AssetHandler.TexBase03;
                    break;
                case 3:
                    tex = AssetHandler.TexBase04;
                    break;
                default:
                    tex = AssetHandler.TexBase01;
                    break;
            }

            SkinnedMeshRenderer[] meshes = player.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            Debug.Log("Looking for meshes...");
            foreach (SkinnedMeshRenderer mesh in meshes)
            {
                Debug.Log("Found a mesh: " + mesh.name);
                mesh.materials[0].SetTexture("_BaseColorMap", tex);
            }
        }
    }

}
