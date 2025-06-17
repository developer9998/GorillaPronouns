using System.Linq;
using GorillaInfoWatch.Attributes;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Widgets;
using GorillaPronouns.Behaviours;
using GorillaPronouns.Models;

namespace GorillaPronouns.InfoWatch.Models
{
    [ShowOnHomeScreen]
    internal class PronounWatchScreen : InfoWatchScreen
    {
        public override string Title => "Pronouns";

        private IdentityController controller;

        public override void OnShow()
        {
            base.OnShow();

            if (controller is not null)
                controller.SwitchState(EIdentityControlState.ViewPronouns);
            else
                controller = Singleton<IdentityHandler>.Instance.GetIdentity(OnConfiguredIdentity);
        }

        public override ScreenContent GetContent()
        {
            LineBuilder lines = new();

            EIdentityControlState state = controller.GetState(out object[] arguments);

            switch (state)
            {
                case EIdentityControlState.ViewPronouns:
                    if (arguments.ElementAtOrDefault(0) is string[] presets)
                    {
                        lines.Add($"Pronouns: {(string.IsNullOrEmpty(Singleton<IdentityHandler>.Instance.LocalPlayer.Pronouns) ? "Unlisted" : Singleton<IdentityHandler>.Instance.LocalPlayer.Pronouns.ToLower())}");
                        lines.Skip();
                        lines.Add("Select mixed pronoun:", new Widget_PushButton(OnAdvancedSelected));
                        lines.Skip();
                        foreach (string pronouns in presets)
                        {
                            lines.Add(string.IsNullOrEmpty(pronouns) ? "Unlisted" : pronouns, new Widget_PushButton(OnPresetSelected, pronouns));
                        }
                    }
                    break;
                case EIdentityControlState.DefineSubject:
                    if (arguments.ElementAtOrDefault(0) is string[] subjects)
                    {
                        lines.Add("Select the first pronoun");
                        lines.Skip();
                        foreach (string pronoun in subjects)
                        {
                            lines.Add(pronoun, new Widget_PushButton(OnSubjectSelected, pronoun));
                        }
                    }
                    break;
                case EIdentityControlState.DefineObject:
                    if (arguments.ElementAtOrDefault(0) is string[] objects)
                    {
                        lines.Add("Select the second pronoun");
                        lines.Skip();
                        foreach (string pronoun in objects)
                        {
                            lines.Add(pronoun, new Widget_PushButton(OnObjectSelected, pronoun));
                        }
                    }
                    break;
            }

            return lines;
        }

        public void OnConfiguredIdentity(string pronouns)
        {
            SetContent();
        }

        public void OnPresetSelected(object[] parameters)
        {
            if (parameters.ElementAtOrDefault(0) is string presetName)
            {
                controller.ConfigurePronouns(presetName);
                SetContent();
            }
        }

        public void OnAdvancedSelected(object[] parameters)
        {
            controller.SwitchState(EIdentityControlState.DefineSubject);
            SetContent();
        }

        public void OnSubjectSelected(object[] parameters)
        {
            if (parameters.ElementAtOrDefault(0) is string subjectPronoun)
            {
                controller.ConfigurePronoun(EIdentityControlState.DefineSubject, subjectPronoun);
                SetContent();
            }
        }

        public void OnObjectSelected(object[] parameters)
        {
            if (parameters.ElementAtOrDefault(0) is string objectPronoun)
            {
                controller.ConfigurePronoun(EIdentityControlState.DefineObject, objectPronoun);
                SetContent();
            }
        }
    }
}
