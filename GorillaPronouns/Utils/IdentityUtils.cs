using System.Linq;
using GorillaPronouns.Tools;

namespace GorillaPronouns.Utils
{
    public class IdentityUtils
    {
        public static bool IsValidPronouns(string pronouns)
        {
            if (pronouns == null || string.IsNullOrEmpty(pronouns))
                return true;

            if (!pronouns.Contains("/"))
                return Constants.PronounPresets.Contains(pronouns);

            string subjectStart = Constants.SubjectPronouns.FirstOrDefault(pronoun => pronouns.StartsWith(pronoun)) ?? null;
            string subjectEnd = Constants.SubjectPronouns.FirstOrDefault(pronoun => pronouns.EndsWith(pronoun)) ?? null;
            bool objectPronoun = Constants.ObjectPronouns.Any(pronoun => pronouns.EndsWith(pronoun));

            Logging.Info($"subject {subjectStart} & {subjectEnd} : object? {objectPronoun}");

            return (subjectStart != null && subjectEnd != null) || (subjectStart != null && objectPronoun);
        }
    }
}
