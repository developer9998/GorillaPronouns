namespace GorillaPronouns
{
    internal class Constants
    {
        public const string Guid = "dev.gorillapronouns";

        public const string Name = "GorillaPronouns";

        public const string Version = "1.0.2";

        public readonly static string[] PronounPresets = [string.Empty, "They/Them", "He/Him", "She/Her", "It/Its", "Any", "Ask"];

        public readonly static string[] SubjectPronouns = ["They", "He", "She", "It"];

        public readonly static string[] ObjectPronouns = ["Them", "Him", "Her", "Its"];

        public readonly static string[] UnknownPronouns = ["Unknown", "N/A", "Silly", "Looked at me funny", "Dev was here", "Didn't set my pronouns correctly", "Yits been.."]; // https://youtu.be/fC_q9KPczAg

        public const float NetworkSetInterval = 2f;

        public const string CustomProperty = "GPronouns";
    }
}
