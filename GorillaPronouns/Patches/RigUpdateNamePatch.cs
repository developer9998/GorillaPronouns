using GorillaPronouns.Behaviours;
using HarmonyLib;

namespace GorillaPronouns.Patches
{
    [HarmonyPatch(typeof(VRRig), "UpdateName", [typeof(bool)])]
    public class RigUpdateNamePatch
    {
        [HarmonyWrapSafe, HarmonyPriority(Priority.Low)]
        public static void Postfix(VRRig __instance)
        {
            if (__instance.TryGetComponent(out PlayerCustomIdentity component))
                component.UpdateName();
        }
    }
}
