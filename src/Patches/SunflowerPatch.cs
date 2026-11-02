using HarmonyLib;
using SunTuner;

namespace SunTuner.Patches;

/// <summary>
/// 向日葵产阳光补丁
/// 
/// 原理：PvZ Hybrid Remake 中，向日葵及其杂交变种通过
/// 产阳光定时器（produceTimer / sunTimer）周期性产出阳光。
/// 补丁在每次产阳光后重置定时器时，使用自定义间隔。
/// </summary>
[HarmonyPatch]
public static class SunflowerPatch
{
    /// <summary>
    /// 目标类型 —— 植物基类或向日葵类
    /// 杂交版中所有植物都继承自 Plant 基类
    /// </summary>
    private const string TargetPlantTypeName = "Plant";

    /// <summary>
    /// 产阳光方法名
    /// </summary>
    private const string TargetProduceMethodName = "ProduceSun";

    /// <summary>
    /// 后处理补丁：向日葵产阳光后，重置为自定义间隔
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(TargetPlantTypeName, TargetProduceMethodName)]
    // ReSharper disable once UnusedMember.Local
    private static void ProduceSunPostfix(object __instance)
    {
        try
        {
            var config = SunTunerPlugin.Config;
            if (config == null || !config.Enabled.Value) return;

            var type = __instance.GetType();

            // 判断是否为向日葵类（含杂交向日葵）
            var isSunProducer = IsSunProducer(type);
            if (!isSunProducer) return;

            // 重置产阳光定时器间隔
            var timerField = type.GetField("produceTimer")
                             ?? type.GetField("sunTimer")
                             ?? type.GetField("productionTimer");

            if (timerField != null && timerField.FieldType == typeof(float))
            {
                // 将定时器重置为自定义间隔（秒）
                // 注意：游戏内定时器可能是倒计时，也可能是累积计时
                // 这里设为间隔值，让下一次产阳光在指定秒数后触发
                timerField.SetValue(__instance, 0f);
                
                // 设置下次产阳光的冷却时间
                var cooldownField = type.GetField("produceCooldown")
                                    ?? type.GetField("sunCooldown")
                                    ?? type.GetField("cooldownDuration");
                
                if (cooldownField != null && cooldownField.FieldType == typeof(float))
                {
                    cooldownField.SetValue(__instance, config.SunflowerInterval.Value);
                }
            }

            // 修改产阳光数量
            if (config.SunflowerAmountMultiplier.Value != 1.0f)
            {
                var amountField = type.GetField("sunAmount")
                                 ?? type.GetField("produceAmount")
                                 ?? type.GetField("outputAmount");
                
                if (amountField != null)
                {
                    if (amountField.FieldType == typeof(int))
                    {
                        var original = (int)amountField.GetValue(__instance)!;
                        amountField.SetValue(__instance, 
                            (int)(original * config.SunflowerAmountMultiplier.Value));
                    }
                    else if (amountField.FieldType == typeof(float))
                    {
                        var original = (float)amountField.GetValue(__instance)!;
                        amountField.SetValue(__instance, 
                            original * config.SunflowerAmountMultiplier.Value);
                    }
                }
            }

            SunTunerPlugin.Log.LogDebug(
                $"[SunflowerPatch] {type.Name} 产阳光完成，下次间隔: {config.SunflowerInterval.Value}s");
        }
        catch (Exception ex)
        {
            SunTunerPlugin.Log.LogWarning($"[SunflowerPatch] 补丁执行异常: {ex.Message}");
        }
    }

    /// <summary>
    /// 判断该植物类型是否为阳光生产者（向日葵及其变种）
    /// </summary>
    private static bool IsSunProducer(Type type)
    {
        // 类名包含 Sun / Sunflower 关键词的即为阳光生产者
        var name = type.Name;
        
        // 直接判断类名
        if (name.Contains("Sun", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("向日葵"))
            return true;

        // 检查基类链
        var baseType = type.BaseType;
        while (baseType != null && baseType.Name != typeof(object).Name)
        {
            if (baseType.Name.Contains("Sun", StringComparison.OrdinalIgnoreCase))
                return true;
            baseType = baseType.BaseType;
        }

        // 检查是否有产阳光相关字段
        var fields = type.GetFields(
            System.Reflection.BindingFlags.Instance | 
            System.Reflection.BindingFlags.Public | 
            System.Reflection.BindingFlags.NonPublic);
        
        foreach (var f in fields)
        {
            if (f.Name.Contains("sun", StringComparison.OrdinalIgnoreCase) &&
                f.Name.Contains("timer", StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
