using GameNetcodeStuff;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine;

namespace CreatureModels
{
    public class LethalCreature
    {
        public class CreatureController : MonoBehaviour
        {
            public static Texture TexBase01;
            public static Texture TexBase02;
            public static Texture TexBase03;
            public static Texture TexBase04;

            GameObject lethalyeenObj = null;
            void Start()
            {
                gameObject.GetComponentInChildren<LODGroup>().enabled = false;
                var meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var LODmesh in meshes)
                {
                    LODmesh.enabled = false;
                }

                //Assetbundle Commune//========

                GameObject lethalyeen = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<GameObject>("assets/lethalcreature.fbx");

                TexBase01 = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<Texture>("assets/textures/lcred.png");
                TexBase02 = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<Texture>("assets/textures/lcgreen.png");
                TexBase03 = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<Texture>("assets/textures/lchazard.png");
                TexBase04 = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<Texture>("assets/textures/lcpajama.png");

                Texture TexSpec = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<Texture>("assets/textures/creature_spec.png");
                Texture TexEmit = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<Texture>("assets/textures/creature_emt.png");
                Texture TexNorm = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<Texture>("assets/textures/creature_norm.png");

                RuntimeAnimatorController animController = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<RuntimeAnimatorController>("assets/creaturecontrol.controller");

                //=================
                //Scaling//========
                var newLethalyeen = Instantiate(lethalyeen);
                newLethalyeen.transform.localScale = new Vector3(1, 1, 1);
                var rig = gameObject.transform.Find("ScavengerModel").Find("metarig");
                var spine = rig.Find("spine").Find("spine.001");
                newLethalyeen.transform.SetParent(spine);
                newLethalyeen.transform.localPosition = new Vector3(0, 0f, 0);
                newLethalyeen.transform.localEulerAngles = Vector3.zero;

                var LOD1 = gameObject.GetComponent<PlayerControllerB>().thisPlayerModel;
                var goodShader = LOD1.material.shader;
                var mesh = newLethalyeen.GetComponentInChildren<SkinnedMeshRenderer>();

                //=================
                //Materials and Textures//========

                mesh.materials[0].shader = goodShader;

                mesh.materials[0].EnableKeyword("_EMISSION");
                mesh.materials[0].EnableKeyword("_SPECGLOSSMAP");
                mesh.materials[0].EnableKeyword("_NORMALMAP");

                mesh.materials[0].SetTexture("_BaseColorMap", TexBase01);
                mesh.materials[0].SetTexture("_SpecularColorMap", TexSpec);
                mesh.materials[0].SetFloat("_Smoothness", .30f);
                mesh.materials[0].SetTexture("_EmissiveColorMap", TexEmit);
                mesh.materials[0].SetTexture("_BumpMap", TexNorm);
                mesh.materials[0].SetColor("_EmissiveColor", Color.white);

                HDMaterial.ValidateMaterial(mesh.materials[0]);


                //=================
                //Dark magik IK voodoo//========

                var anim = newLethalyeen.GetComponentInChildren<Animator>();
                anim.runtimeAnimatorController = animController;
                var ikController = newLethalyeen.AddComponent<IKController>();
                var lthigh = rig.Find("spine").Find("thigh.L");
                var rthigh = rig.Find("spine").Find("thigh.R");
                var lshin = lthigh.Find("shin.L");
                var rshin = rthigh.Find("shin.R");
                var lfoot = lshin.Find("foot.L");
                var rfoot = rshin.Find("foot.R");

                var chest = rig.Find("spine").Find("spine.001").Find("spine.002").Find("spine.003");
                var lshoulder = chest.Find("shoulder.L");
                var rshoulder = chest.Find("shoulder.R");
                var lUpperArm = lshoulder.Find("arm.L_upper");
                var rUpperArm = rshoulder.Find("arm.R_upper");
                var lLowerArm = lUpperArm.Find("arm.L_lower");
                var rLowerArm = rUpperArm.Find("arm.R_lower");
                var lHand = lLowerArm.Find("hand.L");
                var rHand = rLowerArm.Find("hand.R");

                //=================
                //IK Offsets for limbs//=========
                GameObject lFootOffset = new("IK Offset");
                lFootOffset.transform.SetParent(lfoot, false); // X Y Z
                lFootOffset.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                GameObject rFootOffset = new("IK Offset");
                rFootOffset.transform.SetParent(rfoot, false); // X Y Z
                rFootOffset.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                GameObject lHandOffset = new("IK Offset");
                lHandOffset.transform.SetParent(lHand, false); // Z Y
                lHandOffset.transform.localPosition = new Vector3(0.05f, 0f, 0f);
                GameObject rHandOffset = new("IK Offset");
                rHandOffset.transform.SetParent(rHand, false); // Z Y
                rHandOffset.transform.localPosition = new Vector3(-0.05f, 0f, 0f);

                ikController.leftLegTarget = lFootOffset.transform;
                ikController.rightLegTarget = rFootOffset.transform;
                ikController.leftHandTarget = lHandOffset.transform;
                ikController.rightHandTarget = rHandOffset.transform;
                ikController.ikActive = true;

                lethalyeenObj = newLethalyeen;
            }

            private void LateUpdate()
            {
                if (lethalyeenObj != null)
                {
                    lethalyeenObj.transform.localPosition = new Vector3(0, -0.15f, 0);
                    var rig = gameObject.transform.Find("ScavengerModel").Find("metarig");
                    var trans = rig.Find("spine").Find("spine.001").Find("spine.002").Find("spine.003");
                    lethalyeenObj.transform.Find("Armature").Find("Hips").Find("Spine").Find("Chest").localEulerAngles = trans.localEulerAngles;
                }

            }
        }
    }

}

