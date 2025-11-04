using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;

namespace AlternativeSmoothPlayer;

[BepInPlugin(Guid, Name, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private const string Guid = "com.enchart.AlternativeSmoothPlayer";
    private const string Name = "AlternativeSmoothPlayer";
    
    internal new static ManualLogSource Log { get; private set; }

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        var harmony = new Harmony(Guid);
        harmony.PatchAll();
    }
}