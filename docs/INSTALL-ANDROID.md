# 📱 Android 手机版安装指南

> 本模组支持 PvZ Hybrid Remake 的 **Android 版**（arm64-v8a）。
> 无需 root，通过 BepInEx 6 Android 版注入。

## 前置要求

- 一台 **Android 8.0+** 的手机（arm64 架构，即近 5 年的机型）
- PvZ Hybrid Remake 的 **Android APK 版**（从 [pvzhybrid.org](https://pvzhybrid.org/versions) 下载）
- 已经安装好**原版游戏 APK**（先装原版，再做注入）

---

## 第一步：下载 BepInEx 6 Android

1. 前往 [BepInEx 6 Releases](https://github.com/BepInEx/BepInEx/releases)
2. 下载 `BepInEx_6.0.0-android.zip`（或最新 pre-release 的 Android 包）
3. 解压得到 `libBepInEx.so`、`libdoorstop.so` 等文件

## 第二步：重打包 APK（注入 BepInEx）

需要用到 **APK 工具链**，两种方式任选：

### 方式 A：使用 MT管理器 / NP管理器（手机端，推荐）

1. 用 MT管理器 打开游戏 APK（视为压缩包）
2. 进入 `lib/arm64-v8a/` 目录
3. 将 BepInEx 的 `libBepInEx.so` 和 `libdoorstop.so` 拖入该目录
4. 保存并签名 APK（MT管理器自带签名功能）
5. 卸载原版游戏，安装修改后的 APK

### 方式 B：在电脑上用 apktool

```bash
# 反编译 APK
apktool d PvZHybrid.apk -o pvzhybrid_mod

# 复制 BepInEx so 文件
cp libBepInEx.so libdoorstop.so pvzhybrid_mod/lib/arm64-v8a/

# 重打包
apktool b pvzhybrid_mod -o PvZHybrid_BepInEx.apk

# 签名
apksigner sign --ks my.keystore PvZHybrid_BepInEx.apk
```

## 第三步：放置模组文件

首次启动游戏一次，BepInEx 会自动在外部存储生成目录：

```
/sdcard/Android/data/<游戏包名>/files/BepInEx/
├── plugins/
├── config/
└── Logs/
```

然后通过手机文件管理器：

1. 将 `SunTuner.dll` 复制到
   `/sdcard/Android/data/<游戏包名>/files/BepInEx/plugins/`
2. 重启游戏

## 第四步：使用

进入游戏后，**屏幕左上角会出现一个 🌻 按钮**：

- 点击按钮展开/收起控制面板
- 用滑块直接调节天空阳光间隔、向日葵产光间隔、数量
- 所有修改**实时生效**，不用退出游戏
- 也可以点「原版 / 轻松 / 爽玩 / 硬核」一键切换预设

---

## 常见问题（手机端）

### Q: 左上角没有🌻按钮？

A: 按以下顺序排查：
1. 打开 `/sdcard/Android/data/<包名>/files/BepInEx/Logs/LogOutput.log`
2. 搜索 `SunTuner` 关键字
3. 如果没看到日志，说明 `SunTuner.dll` 没被加载——检查路径是否正确
4. 如果看到「补丁应用失败」，可能是游戏版本更新，来 GitHub 提 Issue

### Q: 面板太小/太大？

A: 面板会自动按屏幕宽度缩放（占屏宽 85%）。如果仍不合适，可以在代码里调整 `PanelWidth` 常量后重新编译。

### Q: 安卓版本要求？

A: Android 8.0 (API 26) 及以上。arm64-v8a 架构。32 位旧机型不支持。

### Q: 会被封号吗？

A: 本模组仅修改单机本地游戏逻辑，不联网、不上传数据。但如果游戏有排行榜/云存档功能，建议谨慎。

### Q: iOS 支持吗？

A: 暂不支持。iOS 上 BepInEx 需要越狱，且签名限制严格，后续可能单独出 IPA 注入指南。

---

## 📁 目录速查

| 平台 | 插件目录 | 配置目录 |
|------|---------|---------|
| Windows | `BepInEx/plugins/` | `BepInEx/config/` |
| Android | `/sdcard/Android/data/<包名>/files/BepInEx/plugins/` | `/sdcard/Android/data/<包名>/files/BepInEx/config/` |
