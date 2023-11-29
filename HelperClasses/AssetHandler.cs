using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CreatureModels
{
    public static class AssetHandler
    {
        public static Texture TexBase01;
        public static Texture TexBase02;
        public static Texture TexBase03;
        public static Texture TexBase04;

        public static GameObject lethalyeen;

        public static RuntimeAnimatorController animController;

        //New loader stuff
        public static bool resourcesLoaded = false;
        public static Dictionary<int, Dictionary<int, Texture>> BaseTextures = new Dictionary<int, Dictionary<int, Texture>>();

        private static string texturePrefix = "assets/textures/lc";

        public static string[] animalNames = new string[] { "hyena" };
        public static int[] animalIds;

        public static string[] suits = new string[] { "red", "green", "hazard", "pajama" };
        //public static int suitCount = 0;

        //end of new loader stuff

        public static AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Plugin.assetBundleName));
        //var asset = bundle.LoadAsset<GameObject>("assetpath");

        public static void TryLoadAssets()
        {
            //Setup hashes
            if (animalIds == null)
            {
                animalIds = new int[animalNames.Length];
                for (int i = 0; i < animalIds.Length; i++)
                {
                    animalIds[i] = Utilities.GetStableHashCode(animalNames[i]);
                }
            }

            //Init Empty Lists for Animals
            if (BaseTextures.Count == 0)
            {
                for (int i = 0; i < animalIds.Length; i++)
                {
                    Dictionary<int, Texture> innerDic = new Dictionary<int, Texture>();
                    BaseTextures[animalIds[i]] = innerDic;
                }
            }

            //Load Textures, and Animator
            if (!resourcesLoaded)
            {
                //DynamicLoading
                /*
                for (int animal = 0; animal < BaseTextures.Count; animal++)
                {
                    //mesh loading per animal?

                    //Do we need suits for each animal, if using the same UVs for the body, we can just split another material for head/hand/foot differences
                    for (int suitIndex = 0; suitIndex < suits.Length; suitIndex++)
                    {
                        BaseTextures[animalIds[animal]][suitIndex] = LC_API.BundleAPI.BundleLoader.GetLoadedAsset<Texture>(texturePrefix + suits[suitIndex] + ".png");
                        //texturePrefix + suits[suitIndex] + ".png" is essentially "assets/textures/lc" + "red" + ".png"
                    }
                }
                */

                lethalyeen = bundle.LoadAsset<GameObject>("assets/lethalcreature.fbx");//move to asset handler?

                //Textures loading as fixed for now
                TexBase01 = bundle.LoadAsset<Texture>("assets/textures/lcred.png");
                TexBase02 = bundle.LoadAsset<Texture>("assets/textures/lcgreen.png");
                TexBase03 = bundle.LoadAsset<Texture>("assets/textures/lchazard.png");
                TexBase04 = bundle.LoadAsset<Texture>("assets/textures/lcpajama.png");

                //Since it seems these are all the same, I'm assigning them directly to MaterialAssembler
                MaterialAssembler.TexSpec = bundle.LoadAsset<Texture>("assets/textures/creature_spec.png");
                MaterialAssembler.TexEmit = bundle.LoadAsset<Texture>("assets/textures/creature_emt.png");
                MaterialAssembler.TexNorm = bundle.LoadAsset<Texture>("assets/textures/creature_norm.png");

                animController = bundle.LoadAsset<RuntimeAnimatorController>("assets/creaturecontrol.controller");

                resourcesLoaded = true;
            }
        }
    }
}
