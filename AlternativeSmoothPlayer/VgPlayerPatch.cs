using HarmonyLib;
using UnityEngine;

namespace AlternativeSmoothPlayer;

[HarmonyPatch(typeof(VGPlayer))]
public static class VgPlayerPatch
{
    [HarmonyPatch(nameof(VGPlayer.Update))]
    [HarmonyPrefix]
    public static bool UpdatePrefix(VGPlayer __instance)
    {
        // TODO: this is bad!!! but VGPlayer has no Start() nor Awake() :sob:
        if (__instance.GetComponent<VgPlayerFix>() == null)
            __instance.gameObject.AddComponent<VgPlayerFix>();

        return true;
    }
}