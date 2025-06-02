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

        if (__instance.IsPaused)
        {
            __instance.Player_Rigidbody.velocity = Vector2.zero;
            return false;
        }

        __instance.p_MonoColor = !__instance.CanTakeDamage;
        __instance.HandleBoost();
        __instance.HandleLastMove();
        if (__instance.IsHurt)
        {
            __instance.isHurting -= Time.deltaTime;
        }

        if (__instance.CanMove)
        {
            __instance.HandlePlayerInput();
            __instance.RotatePlayer();
            // __instance.UpdatePlayerVelocity();
            return false;
        }

        __instance.Player_Rigidbody.velocity = Vector2.zero;
        return false;
    }
}