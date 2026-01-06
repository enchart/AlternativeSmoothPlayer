using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AlternativeSmoothPlayer;

[BepInPlugin(Guid, "AlternativeSmoothPlayer", MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string Guid = "com.enchart.AlternativeSmoothPlayer";
    
    internal new static ManualLogSource Logger;
        
    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {Guid} is loaded!");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Guid);
    }
}