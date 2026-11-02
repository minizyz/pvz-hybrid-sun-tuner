using HarmonyLib;
using SunTuner;

namespace SunTuner.Patches;

/// <summary>
/// 天空阳光生成补丁
/// 
/// 原理：PvZ Hybrid Remake (Unity/IL2Cpp) 中，天空阳光由一个
/// 定时生成器（SkySunSpawner / SunDropManager）控制。
/// 通过 Harmony 补丁修改其定时器间隔字段，实现频率调节。
/// 
/// 注意：不同游戏版本的内部类名可能不同。如果补丁未生效，
/// 请使用 BepInEx 日志确认实际类名，并在此处调整目标方法。
/// </summary>
[HarmonyPatch]
public static class SkySunPatch
{
    /// <summary>
    /// 目标类型名（IL2Cpp 元数据中的类名）
    /// 游戏版本更新后可能需要修改此值
    /// </summary>
    private const string TargetTypeName = "SunDropManager";

    /// <summary>
    /// 目标方法名 —— 天空阳光初始化/设置定时器的方法
    /// </summary>
    private const string TargetMethodName = "Start";

    /// <summary>
    /// 后处理补丁：在原始 Start 方法执行后，修改阳光生成间隔
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(TargetTypeName, TargetMethodName)]
    // ReSharper disable once UnusedMember.Local
    private static void Postfix(object __instance)
    {
        try
        {
            var config = SunTunerPlugin.Config;
            if (config == null || !config.Enabled.Value) return;

            // 通过反射修改间隔字段
            // 常见字段名: sunInterval / dropInterval / spawnInterval / intervalSeconds
            var type = __instance.GetType();
            
            var intervalField = type.GetField("sunInterval") 
                                ?? type.GetField("dropInterval")
                                ?? type.GetField("spawnInterval")
                                ?? type.GetField("intervalSeconds");
            
            if (intervalField != null && intervalField.FieldType == typeof(float))
            {
                intervalField.SetValue(__instance, config.SkySunInterval.Value);
                SunTunerPlugin.Log.LogInfo(
                    $"[SkySunPatch] 天空阳光间隔已设为 {config.SkySunInterval.Value}s");
            }

            // 修改阳光数量
            var amountField = type.GetField("sunAmount")
                             ?? type.GetField("dropAmount")
                             ?? type.GetField("amount");
            
            if (amountField != null)
            {
                if (amountField.FieldType == typeof(int))
                    amountField.SetValue(__instance, config.SkySunAmount.Value);
                else if (amountField.FieldType == typeof(float))
                    amountField.SetValue(__instance, (float)config.SkySunAmount.Value);
                
                SunTunerPlugin.Log.LogInfo(
                    $"[SkySunPatch] 天空阳光数量已设为 {config.SkySunAmount.Value}");
            }
        }
        catch (Exception ex)
        {
            SunTunerPlugin.Log.LogWarning($"[SkySunPatch] 补丁执行异常: {ex.Message}");
        }
    }
}
