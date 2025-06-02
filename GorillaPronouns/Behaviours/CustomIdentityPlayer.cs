using GorillaPronouns.Tools;
using GorillaPronouns.Utils;
using TMPro;
using UnityEngine;

namespace GorillaPronouns.Behaviours
{
    [RequireComponent(typeof(VRRig))]
    public class CustomIdentityPlayer : MonoBehaviour
    {
        public string Pronouns;

        public VRRig Rig;
        public NetPlayer Creator;

        private TMP_Text pronounTextInner, pronounTextOuter;

        public void Awake()
        {
            Rig = GetComponent<VRRig>();
            Creator = Rig.isOfflineVRRig ? NetworkSystem.Instance.LocalPlayer : Rig.Creator;

            #region Pronoun nametag setup

            TMP_Text playerText1 = Rig.playerText1;

            Transform pronounText1 = Instantiate(playerText1.gameObject, playerText1.transform.parent).transform;

            pronounText1.transform.localPosition = playerText1.transform.localPosition;
            pronounText1.transform.eulerAngles = playerText1.transform.eulerAngles;

            pronounTextInner = pronounText1.GetComponent<TMP_Text>();
            pronounTextInner.enabled = true;

            TMP_Text playerText2 = Rig.playerText2;

            Transform pronounText2 = Instantiate(playerText2.gameObject, playerText2.transform.parent).transform;

            pronounText2.transform.localPosition = playerText2.transform.localPosition;
            pronounText2.transform.localEulerAngles = playerText2.transform.localEulerAngles;

            pronounTextOuter = pronounText2.GetComponent<TMP_Text>();
            pronounTextOuter.enabled = true;

            pronounText2.transform.SetParent(pronounText1);
            pronounText1.transform.SetParent(playerText1.transform);
            pronounText1.transform.localPosition = Vector3.down * 6.45f;
            pronounText1.transform.localEulerAngles = Vector3.zero;
            pronounText1.transform.localScale = Vector3.one * 0.6f;

            #endregion
        }

        public void UpdateName()
        {
            #region Pronoun nametag adjustment

            Rig.playerText1.ForceMeshUpdate();
            float lineHeight = (Rig.playerText1.textInfo.lineCount / 2f) - 0.5f;
            float textHeight = 6.45f + lineHeight;
            pronounTextInner.transform.localPosition = Vector3.down * textHeight;

            #endregion

            if (IdentityUtils.IsValidPronouns(Pronouns) && !string.IsNullOrEmpty(Pronouns))
            {
                string displayPronouns = Pronouns.ToUpper();
                pronounTextInner.text = displayPronouns;
                pronounTextOuter.text = displayPronouns;

                return;
            }

            pronounTextInner.text = string.Empty;
            pronounTextOuter.text = string.Empty;
        }
    }
}
