# 🌻 PvZ Hybrid Sun Tuner

> 植物大战僵尸杂交版 Remake（**Godot 引擎**）阳光生产频率调节模组
> Adjustable sun production frequency mod for PvZ Hybrid Remake
>
> **Windows (PC) + 📱 Android 手机 双端支持**

[![Godot](https://img.shields.io/badge/Godot-4.x-478CBF)](https://godotengine.org/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Android-brightgreen)](#-安装)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## ✨ 功能特性

- ☀️ **天空阳光掉落频率调节** — 自定义每隔多少秒掉一次阳光
- 🌻 **向日葵产阳光频率调节** — 含杂交向日葵变种
- 🎮 **游戏内悬浮面板** — 屏幕角落 🌻 按钮，滑块实时调节
- 🎚️ **四档预设** — 原版 / 轻松 / 爽玩 / 硬核 一键切换
- 💾 **配置自动保存** — 调节结果写入 `user://sun_tuner.cfg`
- 🔌 **零侵入原文件** — 自动备份原 pck 为 `.bak`

## 🏗️ 技术栈（Godot 引擎）

- **GDScript 4.x** Autoload：`mods/sun_tuner.gd`
- **节点嗅探**：按名字/脚本路径关键词（`sun_spawn` / `sunflower` / `向日葵`）自动定位阳光生成器和向日葵
- **Timer 补丁**：直接修改 Godot `Timer.wait_time` 实现频率调节
- **PCK 工具**：Python 脚本 `tools/patch_pck.py` 一键解包/注入/重打包

## 📦 安装

### Windows

1. 从 [Releases](../../releases) 下载打好补丁的 `PvZHybrid.pck`，或自己用工具打补丁（见下文）
2. 关闭游戏
3. 把补丁后的 pck 替换游戏目录里的原 pck（原文件会自动备份为 `.bak`）
4. 启动游戏，屏幕右上角出现 🌻 按钮

详细步骤：[docs/INSTALL.md](docs/INSTALL.md)

### 📱 Android

1. 用 MT管理器 打开游戏 APK
2. 找到 `assets/` 目录下的 `.pck` 文件
3. 用 `tools/patch_pck.py`（在 Termux 里跑）对该 pck 打补丁
4. 把补丁后的 pck 塞回 APK，重新签名安装

详细步骤：[docs/INSTALL-ANDROID.md](docs/INSTALL-ANDROID.md)

## 🎮 使用

进入游戏后，**屏幕右上角**有一个 🌻 按钮：

- 点开后是控制面板：
  - 模组 ON/OFF 开关
  - 天空阳光间隔滑块（1~60 秒）
  - 天空阳光数量滑块（5~500）
  - 向日葵产光间隔滑块（1~120 秒）
  - 向日葵数量倍率滑块（0.1~5x）
  - 4 个预设按钮
- 拖动滑块**实时生效**，自动保存到配置文件

### 预设

| 预设 | 天空间隔 | 天空数量 | 向日葵间隔 | 倍率 |
|------|---------|---------|-----------|------|
| 原版 | 10s | 25 | 24s | 1.0x |
| 轻松 | 5s | 25 | 12s | 1.0x |
| 爽玩 | 1s | 50 | 3s | 2.0x |
| 硬核 | 20s | 15 | 48s | 0.5x |

## 📁 项目结构

```
pvz-hybrid-sun-tuner/
├── mods/
│   ├── sun_tuner.gd          # GDScript autoload 主脚本
│   └── sun_tuner.cfg        # 默认配置模板
├── tools/
│   ├── patch_pck.py          # 一键 pck 补丁工具
│   └── README.md             # 工具说明
├── docs/
│   ├── INSTALL.md            # Windows 安装
│   └── INSTALL-ANDROID.md     # Android 安装
├── README.md
└── CHANGELOG.md
```

## 🔧 自己打补丁

详见 [tools/README.md](tools/README.md)。一句话：

```bash
python tools/patch_pck.py /path/to/PvZHybrid.pck
```

## ⚠️ 注意

- 仅适配 **Godot 版 Remake (0.x)**，不适用于 Classic 3.x
- 游戏版本更新后如果节点名变了，改 `mods/sun_tuner.gd` 顶部的关键词列表即可
- 频率设太低（< 1 秒）可能卡顿

## 📄 License

[MIT](LICENSE) © minizyz
