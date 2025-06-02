using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorillaInfoWatch.Attributes;
using GorillaInfoWatch.Models;
using GorillaPronouns.Behaviours;
using GorillaPronouns.Models;
using GorillaInfoWatch.Models.Widgets;

[assembly: WatchCompatibleMod]

namespace GorillaPronouns.InfoWatch.Models
{
    [WatchCustomPage, DisplayAtHomeScreen]
    internal class PronounWatchScreen : WatchScreen
    {
        public override string Title => "Pronouns";

        private IdentityController controller;

        public override void OnScreenOpen()
        {
            base.OnScreenOpen();

            controller ??= Singleton<IdentityHandler>.Instance.GetIdentity(OnConfiguredIdentity);
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
                        lines.AddLine($"Pronouns: {(string.IsNullOrEmpty(Singleton<IdentityHandler>.Instance.LocalPlayer.Pronouns) ? "Unlisted" : Singleton<IdentityHandler>.Instance.LocalPlayer.Pronouns)}");
                        lines.AddLines(1);
                        lines.AddLine("Manual Configure Pronouns:", new WidgetButton(OnAdvancedSelected));
                        lines.AddLines(1);
                        foreach(string pronouns in presets)
                        {
                            lines.AddLine(string.IsNullOrEmpty(pronouns) ? "Unlisted" : pronouns, new WidgetButton(OnPresetSelected, pronouns));
                        }
                    }
                    break;
                case EIdentityControlState.DefineSubject:
                    if (arguments.ElementAtOrDefault(0) is string[] subjects)
                    {
                        lines.AddLine("Select a Subject pronoun");
                        lines.AddLines(1);
                        foreach(string pronoun in subjects)
                        {
                            lines.AddLine(pronoun, new WidgetButton(OnSubjectSelected, pronoun));
                        }
                    }
                    break;
                case EIdentityControlState.DefineObject:
                    if (arguments.ElementAtOrDefault(0) is string[] objects && arguments.ElementAtOrDefault(1) is string subject)
                    {
                        lines.AddLine("Select an Object pronoun");
                        lines.AddLines(1);
                        foreach (string pronoun in objects)
                        {
                            lines.AddLine(pronoun, new WidgetButton(OnObjectSelected, pronoun));
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

        public void OnPresetSelected(bool isButtonPressed, object[] parameters)
        {
            if (parameters.ElementAtOrDefault(0) is string presetName)
            {
                controller.ConfigurePronouns(presetName);
                SetContent();
            }
        }

        public void OnAdvancedSelected(bool isButtonPressed, object[] parameters)
        {
            controller.SwitchState(EIdentityControlState.DefineSubject);
            SetContent();
        }

        public void OnSubjectSelected(bool isButtonPressed, object[] parameters)
        {
            if (parameters.ElementAtOrDefault(0) is string subjectPronoun)
            {
                controller.ConfigurePronoun(EIdentityControlState.DefineSubject, subjectPronoun);
                SetContent();
            }
        }

        public void OnObjectSelected(bool isButtonPressed, object[] parameters)
        {
            if (parameters.ElementAtOrDefault(0) is string objectPronoun)
            {
                controller.ConfigurePronoun(EIdentityControlState.DefineObject, objectPronoun);
                SetContent();
            }
        }
    }
}
