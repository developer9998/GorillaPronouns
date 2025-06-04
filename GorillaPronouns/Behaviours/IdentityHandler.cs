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

        public PlayerCustomIdentity LocalPlayer;

        private ConfigEntry<string> savedPronouns;

        public override void Initialize()
        {
            LocalPlayer = GorillaTagger.Instance.offlineVRRig.gameObject.AddComponent<PlayerCustomIdentity>();

            savedPronouns = Plugin.PluginConfigFile.Bind(Constants.Name, "Pronouns", string.Empty, "The pronouns assigned by the player");
            OnConfiguredIdentity(savedPronouns.Value);
        }

        public void OnConfiguredIdentity(string pronouns)
        {
            try
            {
                if (pronouns is null)
                    throw new ArgumentNullException(nameof(pronouns), "Pronouns are null - if attempting to configure unlisted pronouns, send an empty string");

                if (!IdentityUtils.IsPronounsRecognized(pronouns))
                    throw new ArgumentException("Pronouns are not valid", nameof(pronouns));

                savedPronouns.Value = pronouns;
                NetworkHandler.Instance.SetProperty("Pronouns", pronouns);
                LocalPlayer.Pronouns = pronouns;
                LocalPlayer.UpdateName();
            }
            catch (Exception ex)
            {
                Logging.Fatal("Pronouns failed to configure");
                Logging.Error(ex);

                OnConfiguredIdentity(string.Empty);
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
