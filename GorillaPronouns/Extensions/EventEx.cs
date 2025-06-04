using System;
using System.Linq;

namespace GorillaPronouns.Extensions
{
    public static class EventEx
    {
        public static void SafeInvoke<T>(this Action<T> action, params object[] args)
        {
            foreach (var invocation in (action?.GetInvocationList()).Cast<Action<T>>())
            {
                try
                {
                    invocation?.Method?.Invoke(invocation?.Target, args);
                }
                catch
                {

                }
            }
        }
    }
}
