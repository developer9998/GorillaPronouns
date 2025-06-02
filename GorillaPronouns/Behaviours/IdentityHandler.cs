using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using GorillaPronouns.Behaviours.Networking;
using GorillaPronouns.Models;
using GorillaPronouns.Tools;
using GorillaPronouns.Utils;

namespace GorillaPronouns.Behaviours
{
    public class IdentityHandler : Singleton<IdentityHandler>
    {
        private readonly Dictionary<Assembly, IdentityController> identityDevices = [];

        public CustomIdentityPlayer LocalPlayer;

        private ConfigEntry<string> savedPronouns;

        public override void Initialize()
        {
            LocalPlayer = GorillaTagger.Instance.offlineVRRig.gameObject.AddComponent<CustomIdentityPlayer>();
            //LocalPlayer.UpdateName();

            savedPronouns = Plugin.PluginConfigFile.Bind(Constants.Name, "Pronouns", string.Empty, "The pronouns selected by the local player");
            OnConfiguredIdentity(savedPronouns.Value);
        }

        public void OnConfiguredIdentity(string pronouns)
        {
            if (IdentityUtils.IsValidPronouns(pronouns))
            {
                savedPronouns.Value = pronouns;
                NetworkHandler.Instance.SetProperty("Pronouns", pronouns);
                LocalPlayer.Pronouns = pronouns;
                LocalPlayer.UpdateName();
            }
        }

        public IdentityController GetIdentity(Action<string> onConfiguredIdentity)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            if (identityDevices.TryGetValue(callingAssembly, out IdentityController device))
            {
                Logging.Warning($"{callingAssembly.GetName().Name} has a pre-existing identity device!");
                return device;
            }

            device = new(onConfiguredIdentity);
            device.OnConfiguredIdentity += OnConfiguredIdentity;
            identityDevices.TryAdd(callingAssembly, device);

            Logging.Info($"Configured identity device for assembly {callingAssembly.GetName().Name}");
            return device;
        }
    }
}
