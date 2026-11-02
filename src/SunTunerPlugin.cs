using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SunTuner.UI;

namespace SunTuner;

/// <summary>
/// PvZ Hybrid Remake 阳光频率调节插件主入口
/// 同时支持 Windows (x64) 与 Android (arm64-v8a)
/// </summary>
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class SunTunerPlugin : BasePlugin
{
    public const string PluginGuid = "com.minizyz.pvzhybrid.suntuner";
    public const string PluginName = "PvZ Hybrid Sun Tuner";
    public const string PluginVersion = "0.2.0";

    internal static ManualLogSource? Log;
    internal static Configuration? Config;

    public override void Load()
    {
        Log = base.Log;
        Config = new Configuration(base.Config);

        Log.LogInfo($"[{PluginName}] v{PluginVersion} 加载中... 平台: {ApplicationPlatform()}");

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
        }

        // 注册游戏内悬浮配置面板（手机端必备，PC 端也方便）
        try
        {
            ConfigPanel.Create();
            Log.LogInfo($"[{PluginName}] 悬浮配置面板已创建（左上角🌻按钮）。");
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[{PluginName}] 配置面板创建失败: {ex.Message}");
        }

        Log.LogInfo($"[{PluginName}] 加载完成！");
    }

    private static string ApplicationPlatform()
    {
#if UNITY_ANDROID
        return "Android (arm64)";
#elif UNITY_STANDALONE_WIN
        return "Windows (x64)";
#else
        return Environment.OSVersion.Platform.ToString();
#endif
    }
}
