# 📱 Android 手机版安装指南

> 目标：在 Android 手机上玩 Godot 版 PvZ Hybrid Remake，并加载 Sun Tuner mod。
> 无需 root。

## 前置要求

- Android 8.0+
- 已安装 [MT管理器](https://www.coolapk.com/apk/243929)（或 NP管理器）
- 已安装 [Termux](https://termux.dev/)（用于跑 Python 补丁脚本）
- 已下载 PvZ Hybrid Remake 的 Android APK

## 第一步：从 APK 提取 pck

1. 用 MT管理器 打开 PvZ Hybrid APK（视为压缩包）
2. 进入 `assets/` 目录
3. 找到 `.pck` 文件（通常叫 `__pck__` 或 `<游戏名>.pck`）
4. 长按 → 提取到 `/sdcard/Download/`

## 第二步：在 Termux 里打补丁

```bash
# 安装 Python
pkg update
pkg install python

# 克隆仓库
git clone https://github.com/minizyz/pvz-hybrid-sun-tuner.git
cd pvz-hybrid-sun-tuner

# 打补丁（注意：godotpcktool 可能没有 arm64 Linux 版，备选方案见下文）
python tools/patch_pck.py /sdcard/Download/PvZHybrid.pck -o /sdcard/Download/PvZHybrid_mod.pck
```

> **如果 godotpcktool 没有 arm64 版**：在 Windows/Mac/Linux 电脑上跑补丁脚本（见 [INSTALL.md](INSTALL.md)），把生成的 `patched.pck` 传回手机 `/sdcard/Download/`。

## 第三步：把 mod pck 塞回 APK

1. 用 MT管理器 打开原 APK
2. 进入 `assets/` 目录
3. 删掉原 `.pck` 文件
4. 把 `PvZHybrid_mod.pck` 复制进 `assets/`，**并重命名为原 pck 的文件名**
5. 保存 APK（MT管理器会自动签名）
6. 卸载手机上原有的游戏，安装修改后的 APK

## 第四步：使用

启动游戏 → 进入关卡 → **屏幕右上角 🌻 按钮** → 拖动滑块调节。

配置文件在：
```
/storage/emulated/0/Android/data/<包名>/files/sun_tuner.cfg
```
可以用文件管理器直接编辑这个文件。

## 常见问题

### Q: 塞回 pck 后游戏闪退？

A: 检查：
1. pck 文件名是否和原文件完全一致（包括大小写）
2. 是否在 `assets/` 根目录，而不是子目录
3. APK 是否重新签名了
4. 原 pck 是否备份过（恢复测试）

### Q: godotpcktool 跑不起来？

A: 直接在电脑上打好补丁，把 `patched.pck` 传手机即可。

### Q: 🌻 按钮太小不好点？

A: 编辑 `sun_tuner.gd` 里 `btn_size` 常量（默认 64x64），改大后重新打包 pck。

### Q: iOS 支持吗？

A: 暂不支持。iOS 上 Godot 游戏需要重签 IPA，门槛较高。
