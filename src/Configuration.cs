using BepInEx.Configuration;

namespace SunTuner;

/// <summary>
/// 模组配置项定义
/// </summary>
public class Configuration
{
    /// <summary>是否启用模组</summary>
    public ConfigEntry<bool> Enabled { get; }

    /// <summary>天空阳光掉落间隔（秒）</summary>
    public ConfigEntry<float> SkySunInterval { get; }

    /// <summary>天空阳光每次掉落数量</summary>
    public ConfigEntry<int> SkySunAmount { get; }

    /// <summary>向日葵产阳光间隔（秒）</summary>
    public ConfigEntry<float> SunflowerInterval { get; }

    /// <summary>向日葵产阳光数量倍率</summary>
    public ConfigEntry<float> SunflowerAmountMultiplier { get; }

    public Configuration(ConfigFile configFile)
    {
        Enabled = configFile.Bind(
            section: "General",
            key: "Enabled",
            defaultValue: true,
            description: "是否启用阳光频率调节模组"
        );

        SkySunInterval = configFile.Bind(
            section: "SkySun",
            key: "IntervalSeconds",
            defaultValue: 5.0f,
            new ConfigDescription(
                "天空阳光掉落间隔（秒）。原版约为 10 秒。",
                new AcceptableValueRange<float>(1.0f, 60.0f)
            )
        );

        SkySunAmount = configFile.Bind(
            section: "SkySun",
            key: "Amount",
            defaultValue: 25,
            new ConfigDescription(
                "每次天空掉落的阳光数量。原版为 25。",
                new AcceptableValueRange<int>(1, 500)
            )
        );

        SunflowerInterval = configFile.Bind(
            section: "Sunflower",
            key: "IntervalSeconds",
            defaultValue: 12.0f,
            new ConfigDescription(
                "向日葵产阳光间隔（秒）。原版约为 24 秒。",
                new AcceptableValueRange<float>(1.0f, 120.0f)
            )
        );

        SunflowerAmountMultiplier = configFile.Bind(
            section: "Sunflower",
            key: "AmountMultiplier",
            defaultValue: 1.0f,
            new ConfigDescription(
                "向日葵产阳光数量倍率。1.0 = 原版，2.0 = 双倍。",
                new AcceptableValueRange<float>(0.1f, 10.0f)
            )
        );
    }
}
