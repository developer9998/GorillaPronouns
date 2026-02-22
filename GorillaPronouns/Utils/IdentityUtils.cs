using GorillaPronouns.Tools;
using System.Linq;

namespace GorillaPronouns.Utils
{
    public class IdentityUtils
    {
        public static bool IsPronounsRecognized(string pronouns)
        {
            if (string.IsNullOrEmpty(pronouns))
                return pronouns is not null;

            if (!pronouns.Contains("/"))
                return Constants.PronounPresets.Contains(pronouns);

            string subjectStart = Constants.SubjectPronouns.FirstOrDefault(pronouns.StartsWith) ?? null;
            string subjectEnd = Constants.SubjectPronouns.FirstOrDefault(pronouns.EndsWith) ?? null;
            bool objectPronoun = Constants.ObjectPronouns.Any(pronouns.EndsWith);

            Logging.Info($"subject {subjectStart} & {subjectEnd} : object? {objectPronoun}");

            return (subjectStart != null && subjectEnd != null) || (subjectStart != null && objectPronoun);
        }
    }
}
