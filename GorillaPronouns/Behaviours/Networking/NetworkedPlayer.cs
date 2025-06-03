using System.Collections.Generic;
using System.Linq;
using GorillaPronouns.Tools;
using Photon.Realtime;
using UnityEngine;

namespace GorillaPronouns.Behaviours.Networking
{
    [RequireComponent(typeof(RigContainer)), DisallowMultipleComponent]
    internal class NetworkedPlayer : MonoBehaviour
    {
        public VRRig Rig;
        public NetPlayer Owner;

        public bool HasConfiguredPronouns;

        private PlayerCustomIdentity playerIdentity;

        public void Start()
        {
            if (!TryGetComponent(out playerIdentity))
                playerIdentity = gameObject.AddComponent<PlayerCustomIdentity>();

            NetworkHandler.Instance.OnPlayerPropertyChanged += OnPlayerPropertyChanged;

            if (!HasConfiguredPronouns && Owner is PunNetPlayer punPlayer && punPlayer.PlayerRef is Player playerRef)
                NetworkHandler.Instance.OnPlayerPropertiesUpdate(playerRef, playerRef.CustomProperties);
        }

        public void OnDestroy()
        {
            NetworkHandler.Instance.OnPlayerPropertyChanged -= OnPlayerPropertyChanged;

            if (HasConfiguredPronouns)
            {
                HasConfiguredPronouns = false;
                playerIdentity.Pronouns = string.Empty;
                playerIdentity.UpdateName();
            }
        }

        public void OnPlayerPropertyChanged(NetPlayer player, Dictionary<string, object> properties)
        {
            if (player == Owner)
            {
                Logging.Info($"{player.NickName} got properties: {string.Join(", ", properties.Select(prop => $"[{prop.Key}: {prop.Value}]"))}");

                if (properties.TryGetValue("Pronouns", out object obj) && obj is string pronouns)
                {
                    playerIdentity.Pronouns = pronouns;
                    playerIdentity.UpdateName();
                }
            }
        }
    }
}
