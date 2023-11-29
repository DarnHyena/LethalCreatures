using GameNetcodeStuff;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine;
using System.Collections.Generic;

namespace CreatureModels
{
    public class LethalCreature
    {
        public class CreatureController : MonoBehaviour
        {
                     

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

                AssetHandler.TryLoadAssets();

                if(lethalyeenObj == null)
                {   
                    lethalyeenObj = Instantiate(AssetHandler.lethalyeen);
                }

                //=================
                //Scaling//========
                lethalyeenObj.transform.localScale = Vector3.one;
                //var rig = gameObject.transform.Find("ScavengerModel").Find("metarig");
                var rig = gameObject.transform.Find("ScavengerModel/metarig");
                //var spine = rig.Find("spine").Find("spine.001");
                var spine = rig.Find("spine/spine.001");

                lethalyeenObj.transform.SetParent(spine);
                //newLethalyeen.transform.localPosition = new Vector3(0, 0f, 0);
                lethalyeenObj.transform.localPosition = Vector3.zero;
                //newLethalyeen.transform.localEulerAngles = Vector3.zero;
                lethalyeenObj.transform.localRotation = Quaternion.identity;

                var LOD1 = gameObject.GetComponent<PlayerControllerB>().thisPlayerModel;
                var goodShader = LOD1.material.shader; //Should be using sharedMaterial, however not sure if Zeekerss just made it this way >.>
                var mesh = lethalyeenObj.GetComponentInChildren<SkinnedMeshRenderer>();

                //=================
                //Materials and Textures//========          

                MaterialAssembler.UpdateMaterial(ref mesh.materials[0], ref goodShader, ref AssetHandler.TexBase01);
                HDMaterial.ValidateMaterial(mesh.materials[0]);


                //=================
                //Dark magik IK voodoo//========

                var anim = lethalyeenObj.GetComponentInChildren<Animator>();
                anim.runtimeAnimatorController = AssetHandler.animController;


                var ikController = lethalyeenObj.AddComponent<IKController>();
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
            }

            private void LateUpdate()//LateUpdate? this has to run multiple times??
            {
                if (lethalyeenObj != null)
                {
                    lethalyeenObj.transform.localPosition = new Vector3(0, -0.15f, 0);
                    //var rig = gameObject.transform.Find("ScavengerModel").Find("metarig");
                    var rig = gameObject.transform.Find("ScavengerModel/metarig");
                    //var trans = rig.Find("spine").Find("spine.001").Find("spine.002").Find("spine.003");
                    var trans = rig.Find("spine/spine.001/spine.002/spine.003");
                    //lethalyeenObj.transform.Find("Armature").Find("Hips").Find("Spine").Find("Chest").localEulerAngles = trans.localEulerAngles;
                    lethalyeenObj.transform.Find("Armature/Hips/Spine/Chest").localRotation = trans.localRotation;
                }

            }
        }
    }

}

