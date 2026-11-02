# 🌻 PvZ Hybrid Sun Tuner

> 植物大战僵尸杂交版 Remake 阳光生产频率调节模组
> Adjustable sun production frequency mod for PvZ Hybrid Remake (BepInEx IL2Cpp plugin)

[![BepInEx](https://img.shields.io/badge/BepInEx-6.x-6e4a8a)](https://github.com/BepInEx/BepInEx)
[![Unity](https://img.shields.io/badge/Unity-IL2Cpp-000000)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## ✨ 功能特性

- ☀️ **天空阳光掉落频率调节** — 自定义天空阳光每隔多少秒掉落一次
- 🌻 **向日葵产阳光频率调节** — 调整向日葵（含杂交植物）产出阳光的间隔
- 🎚️ **实时配置生效** — 修改配置文件后无需重启游戏即可生效
- 📐 **范围限制** — 所有频率参数都有合理的上下限，防止游戏崩溃
- 🔌 **零侵入** — 纯 Harmony 补丁，不修改游戏文件本体

## 📦 安装

### 前置要求

1. [PvZ Hybrid Remake](https://pvzhybrid.org/versions) (Unity 版, 0.x)
2. [BepInEx 6 IL2Cpp](https://github.com/BepInEx/BepInEx) (x64)

### 步骤

1. 安装 BepInEx 到游戏目录（将 `winhttp.dll` 放入游戏根目录，首次运行生成 `BepInEx` 文件夹）
2. 下载本模组的 `SunTuner.dll`
3. 将 `SunTuner.dll` 放入 `BepInEx/plugins/` 目录
4. 启动游戏，首次运行会生成配置文件 `BepInEx/config/com.minizyz.pvzhybrid.suntuner.cfg`

详细安装说明见 [docs/INSTALL.md](docs/INSTALL.md)

## ⚙️ 配置说明

配置文件路径：`BepInEx/config/com.minizyz.pvzhybrid.suntuner.cfg`

```ini
[General]

# 是否启用模组
# 启用后所有频率设置生效
Enabled = true

[SkySun]

# 天空阳光掉落间隔（秒）
# 原版约为 10 秒；设为 1 表示每秒掉一次阳光
# 范围：1.0 ~ 60.0
IntervalSeconds = 5.0

# 每次掉落的阳光数量
# 原版为 25；设为 50 即双倍阳光
Amount = 25

[Sunflower]

# 向日葵产阳光间隔（秒）
# 原版约为 24 秒；设为 6 表示 4 倍速产阳光
# 范围：1.0 ~ 120.0
IntervalSeconds = 12.0

# 向日葵每次产出的阳光数量倍率
# 1.0 = 原版；2.0 = 双倍
AmountMultiplier = 1.0
```

### 快速预设

| 场景 | SkySun Interval | Sunflower Interval | 效果 |
|------|----------------|-------------------|------|
| 原版 | 10.0s | 24.0s | 原汁原味 |
| 轻松模式 | 5.0s | 12.0s | 阳光翻倍，轻松过关 |
| 极限爽玩 | 1.0s | 3.0s | 阳光如雨，随意布阵 |
| 硬核挑战 | 20.0s | 48.0s | 阳光稀缺，精打细算 |

## 🏗️ 技术原理

本模组使用 **HarmonyX** 对游戏内的阳光生产逻辑进行运行时补丁（Runtime Patch）：

1. **天空阳光**：补丁修改天空阳光生成器的定时器间隔字段
2. **向日葵**：补丁修改向日葵（及杂交向日葵类）的产阳光冷却时间
3. 所有修改仅在内存中生效，不写入游戏文件

## 📁 项目结构

```
pvz-hybrid-sun-tuner/
├── src/
│   ├── SunTunerPlugin.cs      # 插件入口
│   ├── Configuration.cs        # 配置定义
│   └── Patches/
│       ├── SkySunPatch.cs       # 天空阳光补丁
│       └── SunflowerPatch.cs    # 向日葵补丁
├── docs/
│   └── INSTALL.md              # 详细安装指南
├── pvz-hybrid-sun-tuner.csproj # 项目文件
└── README.md
```

## 🛠️ 从源码编译

```bash
# 克隆仓库
git clone https://github.com/minizyz/pvz-hybrid-sun-tuner.git
cd pvz-hybrid-sun-tuner

# 还原依赖并编译
dotnet build -c Release

# 输出: bin/Release/net6.0/SunTuner.dll
```

## ⚠️ 注意事项

- 本模组仅适配 **PvZ Hybrid Remake (Unity/IL2Cpp)** 版本，不适用于 Classic 3.x 版本
- 使用前建议备份存档
- 在线排行榜可能因模组被判定为作弊，请谨慎使用
- 频率设置过低（如 < 1 秒）可能导致游戏卡顿

## 📄 License

[MIT](LICENSE) © minizyz
