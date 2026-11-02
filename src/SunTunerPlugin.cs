using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SunTuner;

/// <summary>
/// PvZ Hybrid Remake 阳光频率调节插件主入口
/// </summary>
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class SunTunerPlugin : BasePlugin
{
    public const string PluginGuid = "com.minizyz.pvzhybrid.suntuner";
    public const string PluginName = "PvZ Hybrid Sun Tuner";
    public const string PluginVersion = "0.1.0";

    internal static ManualLogSource Log = null!;
    internal static Configuration Config = null!;

    public override void Load()
    {
        Log = base.Log;
        Config = new Configuration(base.Config);

        Log.LogInfo($"[{PluginName}] v{PluginVersion} 加载中...");

        if (!Config.Enabled.Value)
        {
            Log.LogInfo($"[{PluginName}] 模组已在配置中禁用，跳过补丁。");
            return;
        }

        // 应用 Harmony 补丁
        var harmony = new Harmony(PluginGuid);
        
        try
        {
            harmony.PatchAll();
            Log.LogInfo($"[{PluginName}] Harmony 补丁应用成功。");
            Log.LogInfo($"[{PluginName}] 天空阳光间隔: {Config.SkySunInterval.Value}s");
            Log.LogInfo($"[{PluginName}] 向日葵产阳光间隔: {Config.SunflowerInterval.Value}s");
        }
        catch (Exception ex)
        {
            Log.LogError($"[{PluginName}] 补丁应用失败: {ex.Message}");
            Log.LogError($"  这可能是因为游戏版本更新导致内部类名/方法名变化。");
            Log.LogError($"  请在 GitHub 提交 Issue 并附上游戏版本号。");
        }

        Log.LogInfo($"[{PluginName}] 加载完成！");
    }
}
