using System.Linq;
using System.Text;
using ComputerInterface;
using ComputerInterface.Extensions;
using ComputerInterface.ViewLib;
using GorillaPronouns.Behaviours;
using GorillaPronouns.Models;

namespace GorillaPronouns.ComputerInterface.Models
{
    internal class PronounView : ComputerView
    {
        private IdentityController controller;

        private readonly UISelectionHandler _selectionHandler;

        public PronounView()
        {
            _selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            _selectionHandler.OnSelected += OnSelected;
            _selectionHandler.ConfigureSelectionIndicator($"<color=#{PrimaryColor}> ></color> ", "", "   ", "");
        }

        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            if (controller is not null)
                controller.SwitchState(EIdentityControlState.ViewPronouns);
            else
                controller = Singleton<IdentityHandler>.Instance.GetIdentity(OnConfiguredIdentity);

            Draw();
        }

        private void Draw()
        {
            StringBuilder str = new();

            str.Repeat("=", SCREEN_WIDTH).AppendLine();

            str.BeginCenter().Append("Pronouns - ").Append("GorillaPronouns").Append(" by ").Append("dev (they/them)").AppendLine();
            str.AppendClr("Press 'OPTION 1' to toggle manual entry", "ffffff50").EndAlign().AppendLine();

            str.Repeat("=", SCREEN_WIDTH).AppendLines(2);

            _selectionHandler.MaxIdx = -1;

            EIdentityControlState state = controller.GetState(out object[] arguments);

            switch (state)
            {
                case EIdentityControlState.ViewPronouns:
                    str.Append("Pronouns: ").Append(string.IsNullOrEmpty(Singleton<IdentityHandler>.Instance.LocalPlayer.Pronouns) ? "Unlisted" : Singleton<IdentityHandler>.Instance.LocalPlayer.Pronouns.ToLower());
                    if (arguments.ElementAtOrDefault(0) is string[] presets)
                    {
                        for(int i = 0; i < presets.Length; i++)
                        {
                            str.AppendLine().Append(_selectionHandler.GetIndicatedText(i, string.IsNullOrEmpty(presets[i]) ? "Unlisted" : presets[i]));
                            _selectionHandler.MaxIdx += 1;
                        }
                    }
                    break;
                case EIdentityControlState.DefineSubject:
                    str.AppendLine("Select the first pronoun");
                    if (arguments.ElementAtOrDefault(0) is string[] subjects)
                    {
                        for(int i = 0; i < subjects.Length; i++)
                        {
                            str.AppendLine().Append(i + 1).Append(". ").Append(_selectionHandler.GetIndicatedText(i, subjects[i]));
                            _selectionHandler.MaxIdx += 1;
                        }
                    }
                    break;
                case EIdentityControlState.DefineObject:
                    str.AppendLine("Select the second pronoun");
                    if (arguments.ElementAtOrDefault(0) is string[] objects)
                    {
                        for (int i = 0; i < objects.Length; i++)
                        {
                            str.AppendLine().Append(i + 1).Append(". ").Append(_selectionHandler.GetIndicatedText(i, objects[i]));
                            _selectionHandler.MaxIdx += 1;
                        }
                    }
                    break;
            }

            Text = str.ToString();
        }

        public void OnSelected(int index)
        {
            EIdentityControlState state = controller.GetState(out object[] arguments);
            switch (state)
            {
                case EIdentityControlState.ViewPronouns:
                    if (arguments.ElementAtOrDefault(0) is string[] presets && presets.ElementAtOrDefault(index) is string presetName)
                    {
                        controller.ConfigurePronouns(presetName);
                    }
                    break;
                case EIdentityControlState.DefineSubject:
                    if (arguments.ElementAtOrDefault(0) is string[] subjects && subjects.ElementAtOrDefault(index) is string subjectPronoun)
                    {
                        controller.ConfigurePronoun(EIdentityControlState.DefineSubject, subjectPronoun);
                        _selectionHandler.CurrentSelectionIndex = 0;
                    }
                    break;
                case EIdentityControlState.DefineObject:
                    if (arguments.ElementAtOrDefault(0) is string[] objects && objects.ElementAtOrDefault(index) is string objectPronoun)
                    {
                        controller.ConfigurePronoun(EIdentityControlState.DefineObject, objectPronoun);
                        _selectionHandler.CurrentSelectionIndex = 0;
                    }
                    break;
            }
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (_selectionHandler.HandleKeypress(key))
            {
                Draw();
                return;
            }

            if (key == EKeyboardKey.Back)
            {
                ReturnToMainMenu();
                return;
            }

            if (key == EKeyboardKey.Option1)
            {
                EIdentityControlState state = controller.GetState(out _);
                controller.SwitchState(state == EIdentityControlState.ViewPronouns ? EIdentityControlState.DefineSubject : EIdentityControlState.ViewPronouns);
                _selectionHandler.CurrentSelectionIndex = 0;
                Draw();
            }
        }

        public void OnConfiguredIdentity(string pronouns)
        {
            Draw();
        }
    }
}
