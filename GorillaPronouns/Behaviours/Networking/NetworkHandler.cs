﻿using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using GorillaPronouns.Tools;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GorillaPronouns.Behaviours.Networking
{
    internal class NetworkHandler : Singleton<NetworkHandler>, IInRoomCallbacks
    {
        public Action<NetPlayer, Dictionary<string, object>> OnPlayerPropertyChanged;

        private readonly Dictionary<string, object> properties = [];
        private bool set_properties = false;
        private float properties_timer;

        public void Start()
        {
            if (NetworkSystem.Instance && NetworkSystem.Instance is NetworkSystemPUN)
            {
                SetProperty("Version", Constants.Version);

                PhotonNetwork.AddCallbackTarget(this);
                Application.quitting += () => PhotonNetwork.RemoveCallbackTarget(this);
                return;
            }

            enabled = false; // either no netsys or not in a pun environment - i doubt fusion will ever come
        }

        public void FixedUpdate()
        {
            properties_timer -= Time.deltaTime;

            if (set_properties && properties.Count > 0 && properties_timer <= 0)
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new()
                {
                    {
                        Constants.CustomProperty,
                        new Dictionary<string, object>(properties)
                    }
                });

                set_properties = false;
                properties_timer = Constants.NetworkSetInterval;
            }
        }

        public void SetProperty(string key, object value)
        {
            if (properties.ContainsKey(key)) properties[key] = value;
            else properties.Add(key, value);
            set_properties = true;
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            NetPlayer netPlayer = NetworkSystem.Instance.GetPlayer(targetPlayer.ActorNumber);

            if (netPlayer.IsLocal || !VRRigCache.Instance.TryGetVrrig(netPlayer, out RigContainer playerRig) || !playerRig.TryGetComponent(out NetworkedPlayer networkedPlayer))
                return;

            if (changedProps.TryGetValue(Constants.CustomProperty, out object props_object) && props_object is Dictionary<string, object> properties)
            {
                networkedPlayer.HasConfiguredPronouns = true;

                Logging.Info($"Recieved properties from {netPlayer.NickName}: {string.Join(", ", properties.Select(prop => $"[{prop.Key}: {prop.Value}]"))}");
                OnPlayerPropertyChanged?.Invoke(netPlayer, properties);
            }
        }

        #region unused interface methods

        public void OnPlayerEnteredRoom(Player newPlayer)
        {

        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {

        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {

        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {

        }

        #endregion
    }
}
