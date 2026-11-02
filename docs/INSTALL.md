# 📥 安装指南

## 系统要求

- Windows 10/11 (x64)
- PvZ Hybrid Remake (Unity 版, 0.x 系列)
- .NET 6.0 Runtime（BepInEx 6 自带，通常无需单独安装）

## 第一步：安装 BepInEx

1. 前往 [BepInEx 6 IL2Cpp 发布页](https://github.com/BepInEx/BepInEx/releases)
2. 下载 `BepInEx_x64_6.0.0.zip`（或最新 pre-release）
3. 解压到 PvZ Hybrid Remake 的游戏根目录
   - 即包含 `PvZHybrid.exe` 的同一级目录
   - 解压后应看到 `winhttp.dll`、`doorstop_config.ini` 等文件

## 第二步：首次运行游戏

1. 启动一次游戏，让 BepInEx 初始化
2. 游戏启动后会自动在游戏目录生成以下结构：

```
游戏目录/
├── PvZHybrid.exe
├── winhttp.dll
└── BepInEx/
    ├── plugins/          ← 模组放这里
    ├── config/           ← 配置文件生成在这里
    └── Logs/             ← 日志文件
```

3. 确认 `BepInEx/LogOutput.log` 文件存在且无报错后，关闭游戏

## 第三步：安装 Sun Tuner 模组

1. 从本项目的 [Releases](../../releases) 页面下载最新的 `SunTuner.dll`
2. 将 `SunTuner.dll` 复制到 `BepInEx/plugins/` 目录

## 第四步：配置模组

1. 启动游戏一次（让模组生成配置文件），然后关闭
2. 打开 `BepInEx/config/com.minizyz.pvzhybrid.suntuner.cfg`
3. 根据需要修改配置参数（参考 [README](../README.md#%EF%B8%8F-配置说明)）
4. 保存后重新启动游戏即可生效

## 验证安装是否成功

1. 启动游戏后打开 `BepInEx/LogOutput.log`
2. 搜索以下日志：

```
[Info:SunTuner] [PvZ Hybrid Sun Tuner] v0.1.0 加载中...
[Info:SunTuner] [PvZ Hybrid Sun Tuner] Harmony 补丁应用成功。
[Info:SunTuner] [PvZ Hybrid Sun Tuner] 加载完成！
```

3. 进入游戏观察：天空阳光应比原版掉落更频繁

## 卸载模组

1. 从 `BepInEx/plugins/` 中删除 `SunTuner.dll`
2. （可选）删除配置文件 `BepInEx/config/com.minizyz.pvzhybrid.suntuner.cfg`
3. 重启游戏即可

## 常见问题

### Q: 安装后没有效果？

A: 请检查：
1. 游戏版本是否为 **Remake (0.x)** 而非 Classic (3.x)
2. BepInEx 是否正确安装（查看日志中是否有 BepInEx 字样）
3. `SunTuner.dll` 是否在 `BepInEx/plugins/` 目录下
4. 配置文件中 `Enabled = true`

### Q: 游戏崩溃了？

A: 频率设置过低可能导致游戏卡顿或崩溃，建议：
- 天空阳光间隔不低于 1.0 秒
- 向日葵间隔不低于 1.0 秒
- 如果仍然崩溃，请在 [Issues](../../issues) 提交报告

### Q: 支持 Classic 3.x 版本吗？

A: 本模组仅适配 Remake (Unity) 版本。Classic 版本基于原版游戏引擎，需要不同的补丁方式。
