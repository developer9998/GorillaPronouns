using System;
using GorillaPronouns.Tools;
using ControlState = GorillaPronouns.Models.EIdentityControlState;

namespace GorillaPronouns.Models
{
    public class IdentityController
    {
        public static ControlState State;

        public static string Subject, Object;

        public static string Pronouns;

        public static string[] Presets => Constants.PronounPresets;
        public static string[] Subjects => Constants.SubjectPronouns;
        public static string[] Objects
        {
            get
            {
                string[] objectPronouns = [.. Constants.ObjectPronouns];
                for (int i = 0; i < objectPronouns.Length; i++)
                {
                    if (Subject != null && Subject != Subjects[i])
                    {
                        objectPronouns[i] = Subjects[i];
                    }
                }
                return objectPronouns;
            }
        }

        public Action<string> OnConfiguredIdentity;

        public IdentityController(Action<string> onConfiguredIdentity)
        {
            OnConfiguredIdentity += onConfiguredIdentity;
        }

        public void SwitchState(ControlState state)
        {
            if (State != state)
            {
                Logging.Info($"SwitchState {state}");
                State = state;
                CheckState();
            }
        }

        public void CheckState()
        {
            switch (State)
            {
                case ControlState.ViewPronouns:
                    Subject = null;
                    Object = null;
                    Pronouns = null;
                    break;

                case ControlState.SubmitPronouns:
                    Logging.Info($"CONFIGURED IDENTITY! User now goes by {Pronouns}");
                    OnConfiguredIdentity?.Invoke(Pronouns);
                    SwitchState(ControlState.ViewPronouns);
                    break;
            }
        }

        public void ConfigurePronoun(ControlState state, string pronoun)
        {
            switch (state)
            {
                case ControlState.DefineSubject:
                    if (Subjects.IndexOfRef(pronoun) is int subIndex && subIndex != -1)
                    {
                        Logging.Info($"Configured subject pronoun to {subIndex} ({pronoun})");
                        Subject = pronoun;
                    }
                    break;
                case ControlState.DefineObject:
                    if (Objects.IndexOfRef(pronoun) is int objIndex && objIndex != -1)
                    {
                        Logging.Info($"Configured object pronoun to {objIndex} ({pronoun})");
                        Object = pronoun;
                    }
                    break;
            }

            CheckPronouns();
        }

        public void ConfigurePronouns(string pronouns)
        {
            Pronouns = pronouns;

            CheckPronouns();
        }

        public void CheckPronouns()
        {
            if (Subject != null && Object == null)
                SwitchState(ControlState.DefineObject);
            else if (Pronouns != null || (Subject != null && Object != null))
            {
                Pronouns ??= string.Format("{0}/{1}", Subject, Object);
                SwitchState(ControlState.SubmitPronouns);
            }
        }

        public ControlState GetState(out object[] content)
        {
            content = [];

            switch (State)
            {
                case ControlState.ViewPronouns:
                    content = [Presets];
                    break;
                case ControlState.DefineSubject:
                    content = [Subjects];
                    break;
                case ControlState.DefineObject:
                    content = [Objects, Subject];
                    break;
            }

            return State;
        }
    }
}
