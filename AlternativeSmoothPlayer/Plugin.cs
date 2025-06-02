using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;

namespace AlternativeSmoothPlayer;

[BepInPlugin(Guid, Name, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private const string Guid = "com.enchart.AlternativeSmoothPlayer";
    private const string Name = "AlternativeSmoothPlayer";

    public override void Load()
    {
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        RegisterTypes();

        var harmony = new Harmony(Guid);
        harmony.PatchAll();
    }

    private static void RegisterTypes()
    {
        ClassInjector.RegisterTypeInIl2Cpp<VgPlayerFix>();
    }
}