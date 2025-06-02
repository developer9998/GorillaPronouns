using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GorillaPronouns.Behaviours;
using GorillaPronouns.Behaviours.Networking;
using HarmonyLib;
using UnityEngine;

namespace GorillaPronouns
{
    [BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource PluginLogSource;

        public static ConfigFile PluginConfigFile;

        public void Awake()
        {
            PluginLogSource = Logger;
            PluginConfigFile = Config;

            Harmony.CreateAndPatchAll(GetType().Assembly, Constants.Guid);
            GorillaTagger.OnPlayerSpawned(() => new GameObject(Constants.Name, typeof(NetworkHandler), typeof(IdentityHandler)));
        }
    }
}
