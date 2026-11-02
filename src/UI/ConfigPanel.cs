using UnityEngine;

namespace SunTuner.UI;

/// <summary>
/// 游戏内悬浮配置面板（手机/PC 通用）
/// 
/// 手机端没有文件编辑器，所以提供一个 OnGUI 绘制的浮动面板：
/// - 屏幕左上角小按钮点开/收起面板
/// - 滑块直接调节频率，实时写入 Config
/// - 大字号、大点击区域，适合触屏
/// </summary>
public class ConfigPanel : MonoBehaviour
{
    private bool _visible = true;
    private bool _enabled = true;
    
    private float _skySunInterval = 5.0f;
    private float _skySunAmount = 25f;
    private float _sunflowerInterval = 12.0f;
    private float _sunflowerMult = 1.0f;

    private Rect _windowRect;
    private Rect _toggleButtonRect;

    // 面板尺寸（按屏幕比例适配）
    private float PanelWidth => Mathf.Min(420f, Screen.width * 0.85f);
    private float PanelHeight => 520f;
    private float ToggleSize => 72f;

    private GUIStyle? _labelStyle;
    private GUIStyle? _valueStyle;
    private GUIStyle? _buttonStyle;
    private GUIStyle? _headerStyle;
    private bool _stylesInitialized;

    private void Awake()
    {
        // 从配置同步初始值
        var cfg = SunTunerPlugin.Config;
        if (cfg != null)
        {
            _enabled = cfg.Enabled.Value;
            _skySunInterval = cfg.SkySunInterval.Value;
            _skySunAmount = cfg.SkySunAmount.Value;
            _sunflowerInterval = cfg.SunflowerInterval.Value;
            _sunflowerMult = cfg.SunflowerAmountMultiplier.Value;
        }

        _windowRect = new Rect(20, 120, PanelWidth, PanelHeight);
        _toggleButtonRect = new Rect(16, 16, ToggleSize, ToggleSize);
    }

    private void InitStyles()
    {
        if (_stylesInitialized) return;

        var baseSize = Mathf.Max(20, Screen.width / 40f);

        _headerStyle = new GUIStyle(GUI.skin.box)
        {
            fontSize = Mathf.RoundToInt(baseSize * 1.2f),
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = new Color(1f, 0.85f, 0.2f) }
        };

        _labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(baseSize),
            normal = { textColor = Color.white }
        };

        _valueStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(baseSize),
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleRight,
            normal = { textColor = new Color(0.6f, 1f, 0.6f) }
        };

        _buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = Mathf.RoundToInt(baseSize * 1.1f),
            padding = new RectOffset(12, 12, 10, 10),
            fixedHeight = 56f
        };

        _stylesInitialized = true;
    }

    private void OnGUI()
    {
        InitStyles();

        // 左上角悬浮开关按钮
        if (GUI.Button(_toggleButtonRect, "\U0001F33B", _buttonStyle!))
        {
            _visible = !_visible;
        }

        if (!_visible) return;

        _windowRect = GUI.Window(99999, _windowRect, DrawWindow, "Sun Tuner 控制面板", _headerStyle!);
    }

    private void DrawWindow(int id)
    {
        GUILayout.Space(10);

        // 启用开关
        GUILayout.BeginHorizontal();
        GUILayout.Label("模组启用", _labelStyle!, GUILayout.Width(140));
        var newEnabled = GUILayout.Toggle(_enabled, _enabled ? "  ON" : "  OFF", _buttonStyle!);
        if (newEnabled != _enabled)
        {
            _enabled = newEnabled;
            ApplyConfig();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(14);

        // === 天空阳光 ===
        GUILayout.Label("—— 天空阳光 ——", _headerStyle!);
        GUILayout.Space(8);

        DrawSlider("掉落间隔", ref _skySunInterval, 1f, 60f, "秒");
        DrawSlider("每次数量", ref _skySunAmount, 5f, 500f, null, 0);

        GUILayout.Space(14);

        // === 向日葵 ===
        GUILayout.Label("—— 向日葵产光 ——", _headerStyle!);
        GUILayout.Space(8);

        DrawSlider("产光间隔", ref _sunflowerInterval, 1f, 120f, "秒");
        DrawSlider("数量倍率", ref _sunflowerMult, 0.1f, 5f, "x", 1);

        GUILayout.Space(20);

        // === 预设按钮 ===
        GUILayout.Label("快速预设", _headerStyle!);
        GUILayout.Space(8);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("原版", _buttonStyle!)) ApplyPreset(10f, 25f, 24f, 1f);
        if (GUILayout.Button("轻松", _buttonStyle!)) ApplyPreset(5f, 25f, 12f, 1f);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("爽玩", _buttonStyle!)) ApplyPreset(1f, 50f, 3f, 2f);
        if (GUILayout.Button("硬核", _buttonStyle!)) ApplyPreset(20f, 15f, 48f, 0.5f);
        GUILayout.EndHorizontal();

        // 拖拽窗口
        GUI.DragWindow(new Rect(0, 0, _windowRect.width, 40));
    }

    private void DrawSlider(string label, ref float value, float min, float max, string? unit = null, int decimals = 1)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, _labelStyle!, GUILayout.Width(120));
        var newValue = GUILayout.HorizontalSlider(value, min, max, GUILayout.Height(30));
        GUILayout.Label(value.ToString("F" + decimals) + (unit ?? ""), _valueStyle!, GUILayout.Width(80));
        GUILayout.EndHorizontal();

        if (Math.Abs(newValue - value) > 0.001f)
        {
            value = newValue;
            ApplyConfig();
        }
    }

    private void ApplyPreset(float skyInterval, float skyAmt, float sunInterval, float sunMult)
    {
        _skySunInterval = skyInterval;
        _skySunAmount = skyAmt;
        _sunflowerInterval = sunInterval;
        _sunflowerMult = sunMult;
        ApplyConfig();
    }

    /// <summary>
    /// 实时写入配置（并持久化到 cfg 文件）
    /// </summary>
    private void ApplyConfig()
    {
        var cfg = SunTunerPlugin.Config;
        if (cfg == null) return;

        cfg.Enabled.Value = _enabled;
        cfg.SkySunInterval.Value = _skySunInterval;
        cfg.SkySunAmount.Value = (int)_skySunAmount;
        cfg.SunflowerInterval.Value = _sunflowerInterval;
        cfg.SunflowerAmountMultiplier.Value = _sunflowerMult;

        cfg.Save();

        SunTunerPlugin.Log?.LogInfo(
            $"[ConfigPanel] 设置已更新: 天空{_skySunInterval:F1}s/{_skySunAmount:F0}, " +
            $"向日葵{_sunflowerInterval:F1}s x{_sunflowerMult:F1}");
    }

    /// <summary>
    /// 由插件入口调用，挂载面板到 DontDestroyOnLoad
    /// </summary>
    public static ConfigPanel Create()
    {
        var go = new GameObject("SunTuner_ConfigPanel");
        UnityEngine.Object.DontDestroyOnLoad(go);
        return go.AddComponent<ConfigPanel>();
    }
}
