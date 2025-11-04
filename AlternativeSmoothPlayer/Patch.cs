using HarmonyLib;
using UnityEngine;
using VGPlayerData = VGPlayerManager.VGPlayerData;

namespace AlternativeSmoothPlayer;

[HarmonyPatch]
public static class Patch
{
    private const float RotateSpeed = 40.0f;
    private const float FollowSpeed = 10.0f;
    private const float EdgeSpeed = 25.0f;
    
    [HarmonyPatch(typeof(VGPlayerData), nameof(VGPlayerData.SpawnPlayerObject))]
    [HarmonyPostfix]
    public static void VGPlayerData_SpawnPlayerObject_Postfix(VGPlayer __result)
    {
        var visual = __result.transform.Find("Player");
        Object.Destroy(visual.GetComponent<DelayTracker>());
    }
    
    [HarmonyPatch(typeof(VGPlayer), nameof(VGPlayer.Update))]
    [HarmonyPostfix]
    public static void VGPlayer_Update_Postfix(VGPlayer __instance)
    {
        var visual = __instance.transform.Find("Player");
        var positionDelta = (Vector3)__instance.internalVelocity;
        var rotationDelta = Quaternion.Euler(0f, 0f, __instance.Player_Rigidbody.transform.eulerAngles.z);

        visual.position += positionDelta * Time.deltaTime;
        visual.rotation = Quaternion.Lerp(visual.rotation, rotationDelta, Time.deltaTime * RotateSpeed);
    }
    
    [HarmonyPatch(typeof(VGPlayer), nameof(VGPlayer.LateUpdate))]
    [HarmonyPostfix]
    public static void VGPlayer_LateUpdate_Postfix(VGPlayer __instance)
    {
        var visual = __instance.transform.Find("Player");
        visual.position = Vector3.Lerp(visual.position, __instance.Player_Rigidbody.transform.position, Time.deltaTime * FollowSpeed);

        var vector = __instance.ObjectCamera.WorldToViewportPoint(visual.position);
        var edgeOffset = VGPlayer.EDGE_OFFSET;
        var num = (float)Screen.height / Screen.width * edgeOffset;

        if (vector.x < num || vector.x > 1f - num || vector.y < edgeOffset || vector.y > 1f - edgeOffset)
        {
            vector.x = Mathf.Clamp(vector.x, num, 1f - num);
            vector.y = Mathf.Clamp(vector.y, edgeOffset, 1f - edgeOffset);
            visual.position = Vector3.Lerp(visual.position, __instance.ObjectCamera.ViewportToWorldPoint(vector), Time.deltaTime * EdgeSpeed);
        }
    }
}