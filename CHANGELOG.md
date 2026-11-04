# Changelog

本项目遵循 [Semantic Versioning](https://semver.org/lang/zh-CN/)。

## [0.3.0] - 2026-11-04

### ⚠️ 重要重写

- **纠正游戏引擎**：PvZ Hybrid Remake 实际使用 **Godot 4.x**（非 Unity），本版本完全重写

### 新增

- 🔄 **GDScript Autoload 脚本** `mods/sun_tuner.gd`：节点嗅探 + Timer 补丁
- 🐍 **Python 一键补丁工具** `tools/patch_pck.py`：解包 pck → 注入 autoload → 重打包
- 🎮 **Godot Control UI**：屏幕角落 🌻 按钮 + 滑块控制面板
- 📱 **Android 指南**：MT管理器 + Termux 完整流程
- 💾 **配置文件** `user://sun_tuner.cfg`，支持热修改

### 移除

- ❌ 所有错误的 C# / BepInEx / Harmony / IL2Cpp 代码
- ❌ .csproj 项目文件

## [0.2.0] - 2026-11-02

（已废弃 — 基于错误引擎假设）

## [0.1.0] - 2026-11-02

（已废弃 — 基于错误引擎假设）
