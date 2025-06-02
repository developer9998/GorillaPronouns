namespace GorillaPronouns
{
    internal class Constants
    {
        public const string Guid = "dev.gorillapronouns";

        public const string Name = "GorillaPronouns";

        public const string Version = "1.0.0";

        public readonly static string[] PronounPresets = [string.Empty, "they/them", "he/him", "she/her", "it/its", "any", "ask"];

        public readonly static string[] SubjectPronouns = ["they", "he", "she", "it"];

        public readonly static string[] ObjectPronouns = ["them", "him", "her", "its"];

        public const float NetworkSetInterval = 0.25f;

        public const string CustomProperty = "GPronouns";
    }
}
