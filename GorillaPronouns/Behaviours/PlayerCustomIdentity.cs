using GorillaExtensions;
using GorillaPronouns.Utils;
using TMPro;
using UnityEngine;

namespace GorillaPronouns.Behaviours
{
    [RequireComponent(typeof(VRRig))]
    public class PlayerCustomIdentity : MonoBehaviour
    {
        public string Pronouns = string.Empty;

        public VRRig Rig;
        public NetPlayer Player;

        private TMP_Text pronounText; //, pronounTextOutline;

        public void Awake()
        {
            enabled = false;

            Rig = GetComponent<VRRig>();
            Player = Rig.isOfflineVRRig ? NetworkSystem.Instance.GetLocalPlayer() : Rig.Creator;

            #region Pronoun nametag setup

            TMP_Text playerText1 = Rig.playerText1;

            Transform pronounText1 = Instantiate(playerText1.gameObject, playerText1.transform.parent).transform;

            pronounText1.transform.localPosition = playerText1.transform.localPosition;
            pronounText1.transform.eulerAngles = playerText1.transform.eulerAngles;

            pronounText = pronounText1.GetComponent<TMP_Text>();
            pronounText.enabled = true;

            // TMP_Text playerText2 = Rig.playerText2;

            // Transform pronounText2 = Instantiate(playerText2.gameObject, playerText2.transform.parent).transform;

            // pronounText2.transform.localPosition = playerText2.transform.localPosition;
            // pronounText2.transform.localEulerAngles = playerText2.transform.localEulerAngles;

            // pronounTextOutline = pronounText2.GetComponent<TMP_Text>();
            // pronounTextOutline.enabled = true;

            // pronounText2.transform.SetParent(pronounText1);
            pronounText1.transform.SetParent(playerText1.transform);
            pronounText1.transform.localPosition = Vector3.down * 6.45f;
            pronounText1.transform.localEulerAngles = Vector3.zero;
            pronounText1.transform.localScale = Vector3.one * 0.5f;
            pronounText1.gameObject.SetActive(false);

            #endregion

            enabled = true;
        }

        public void Update()
        {
            if (pronounText.color != Rig.playerText1.color) pronounText.color = Rig.playerText1.color;

            /*
            if (pronounTextOutline.color != Rig.playerText2.color) pronounTextOutline.color = Rig.playerText2.color;
            */
        }

        public void OnDestroy()
        {
            if (enabled)
            {
                Destroy(pronounText.gameObject);
            }
        }

        public void UpdateName()
        {
            #region Pronoun nametag adjustment

            Rig.playerText1.ForceMeshUpdate();
            float lineHeight = (Rig.playerText1.textInfo.lineCount / 2f) - 0.5f;
            float textHeight = 6.45f + lineHeight;
            pronounText.transform.localPosition = Vector3.down * textHeight;

            #endregion

            string pronounsToSet = IdentityUtils.IsPronounsRecognized(Pronouns) ? Constants.UnknownPronouns.GetRandomItem() : Pronouns.ToUpper();
            pronounText.gameObject.SetActive(!string.IsNullOrEmpty(pronounsToSet) && !string.IsNullOrWhiteSpace(pronounsToSet));
            if (pronounText.gameObject.activeSelf)
            {
                string displayPronouns = Pronouns.ToUpper();
                pronounText.text = displayPronouns;
                // pronounTextOutline.text = displayPronouns;
            }
        }
    }
}
