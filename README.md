# 🌻 PvZ Hybrid Sun Tuner

> 植物大战僵尸杂交版 Remake 阳光生产频率调节模组
> Adjustable sun production frequency mod for PvZ Hybrid Remake
>
> **支持 Windows (x64) + 📱 Android (arm64) 双端！**

[![BepInEx](https://img.shields.io/badge/BepInEx-6.x-6e4a8a)](https://github.com/BepInEx/BepInEx)
[![Unity](https://img.shields.io/badge/Unity-IL2Cpp-000000)](https://unity.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Android-brightgreen)](#-android-手机版)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## ✨ 功能特性

- ☀️ **天空阳光掉落频率调节** — 自定义天空阳光每隔多少秒掉落一次
- 🌻 **向日葵产阳光频率调节** — 调整向日葵（含杂交植物）产出阳光的间隔
- 🎮 **游戏内悬浮面板** — 手机左上角🌻按钮点开滑块直接调，**实时生效**
- 📱 **双端支持** — Windows PC + Android 手机同一个 DLL
- 🎚️ **一键预设** — 原版 / 轻松 / 爽玩 / 硬核 四档快捷切换
- 🔌 **零侵入** — 纯 Harmony 补丁，不修改游戏文件本体

## 📦 安装

### Windows 版

详细步骤见 [docs/INSTALL.md](docs/INSTALL.md)

简要：安装 BepInEx 6 → 把 `SunTuner.dll` 丢进 `BepInEx/plugins/` → 启动游戏

### 📱 Android 手机版

详细步骤见 [docs/INSTALL-ANDROID.md](docs/INSTALL-ANDROID.md)

简要：APK 注入 BepInEx → 把 `SunTuner.dll` 丢进 `/sdcard/Android/data/<包名>/files/BepInEx/plugins/` → 启动游戏 → 点左上角 🌻

## 🎮 游戏内面板

进入游戏后，**屏幕左上角**会有一个 🌻 小按钮：

- **点一下**展开 / 收起控制面板
- 展开后可以看到：
  - 模组启用开关（ON/OFF）
  - 天空阳光「掉落间隔」「每次数量」滑块
  - 向日葵「产光间隔」「数量倍率」滑块
  - 4 个预设按钮：原版 / 轻松 / 爽玩 / 硬核
- **所有滑块拖完立即生效**，不用退出游戏、不用改文件
- 面板可以拖动（按住标题栏任意位置）

## ⚙️ 配置项（Windows 端）

配置文件路径：`BepInEx/config/com.minizyz.pvzhybrid.suntuner.cfg`

```ini
[General]
Enabled = true

[SkySun]
IntervalSeconds = 5.0      # 天空阳光掉落间隔（1~60 秒）
Amount = 25                # 每次掉落数量（1~500）

[Sunflower]
IntervalSeconds = 12.0     # 向日葵产光间隔（1~120 秒）
AmountMultiplier = 1.0     # 数量倍率（0.1~10.0）
```

手机端直接在游戏里的悬浮面板调即可，自动同步到这个文件。

### 快速预设

| 预设 | 天空间隔 | 天空数量 | 向日葵间隔 | 倍率 | 效果 |
|------|---------|---------|-----------|------|------|
| 原版 | 10.0s | 25 | 24.0s | 1.0x | 原汁原味 |
| 轻松 | 5.0s | 25 | 12.0s | 1.0x | 阳光翻倍 |
| 爽玩 | 1.0s | 50 | 3.0s | 2.0x | 阳光如雨 |
| 硬核 | 20.0s | 15 | 48.0s | 0.5x | 阳光稀缺 |

## 🏗️ 技术原理

本模组使用 **HarmonyX** 对游戏内的阳光生产逻辑进行运行时补丁：

1. **天空阳光**：补丁修改天空阳光生成器的定时器间隔字段
2. **向日葵**：补丁修改向日葵（及杂交向日葵类）的产阳光冷却时间
3. **OnGUI 面板**：独立 MonoBehaviour 挂载在 `DontDestroyOnLoad`，跨场景存活
4. 所有修改仅在内存中生效，不写入游戏文件

## 📁 项目结构

```
pvz-hybrid-sun-tuner/
├── src/
│   ├── SunTunerPlugin.cs       # 插件入口（双端）
│   ├── Configuration.cs         # 配置定义
│   ├── UI/
│   │   └── ConfigPanel.cs       # 游戏内悬浮面板（手机触屏优化）
│   └── Patches/
│       ├── SkySunPatch.cs       # 天空阳光补丁
│       └── SunflowerPatch.cs    # 向日葵补丁
├── docs/
│   ├── INSTALL.md              # Windows 安装指南
│   └── INSTALL-ANDROID.md     # Android 手机安装指南
├── pvz-hybrid-sun-tuner.csproj
└── README.md
```

## 🛠️ 从源码编译

```bash
git clone https://github.com/minizyz/pvz-hybrid-sun-tuner.git
cd pvz-hybrid-sun-tuner
dotnet build -c Release
# 输出: bin/Release/net6.0/SunTuner.dll
# 同一个 DLL 同时跑 Windows 和 Android
```

## ⚠️ 注意事项

- 本模组仅适配 **PvZ Hybrid Remake (Unity/IL2Cpp)** 版本，不适用于 Classic 3.x
- 手机端需要 arm64-v8a 架构 + Android 8.0+
- 使用前建议备份存档
- 频率设置过低（< 1 秒）可能导致游戏卡顿

## 📄 License

[MIT](LICENSE) © minizyz
