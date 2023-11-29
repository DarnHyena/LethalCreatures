using BepInEx;
using HarmonyLib;
using BepInEx.Logging;

namespace CreatureModels 
{
    [BepInPlugin(GUID, ModName, Version)]
    public class Plugin : BaseUnityPlugin
    {
        public const string Version = "1.2.3"; //since new refactor but no new code or assets
        public const string ModName = "Local Hyena Creature Models";
        public const string GUID = "lh.creaturemodels";

        public const string assetBundleName = "lethalcreature";

        public static Harmony _harmony = new Harmony(GUID);
        public static ManualLogSource Log;
        private void Awake()
        {
            //_harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            _harmony.PatchAll();
            Log = Logger;
            Log.LogInfo("HyenaNoises");
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} loaded");
        }
    }
}
