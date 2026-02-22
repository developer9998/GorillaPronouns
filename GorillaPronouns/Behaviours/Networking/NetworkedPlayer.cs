using ExitGames.Client.Photon;
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

        public void Awake()
        {
            if (!TryGetComponent(out playerIdentity)) playerIdentity = gameObject.AddComponent<PlayerCustomIdentity>();
        }

        public void OnDestroy()
        {
            if (HasConfiguredPronouns)
            {
                HasConfiguredPronouns = false;
                playerIdentity.Pronouns = string.Empty;
                playerIdentity.UpdateName();
            }
        }

        public void OnPlayerPropertyChanged(Hashtable properties)
        {
            if (properties.TryGetValue("Pronouns", out object obj) && obj is string pronouns)
            {
                playerIdentity.Pronouns = pronouns;
                playerIdentity.UpdateName();
            }
        }
    }
}
