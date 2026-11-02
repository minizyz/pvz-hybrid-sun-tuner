# 🔧 Sun Tuner .pck 补丁工具

把 `sun_tuner.gd` 注入到 PvZ Hybrid Remake 的 Godot `.pck` 资源包中。

## 前置依赖

下载 **godotpcktool**（单文件，跨平台）：

- GitHub Releases: https://github.com/bdach/godotpcktool/releases
- Windows: `godotpcktool-windows-x86_64.exe`
- Linux: `godotpcktool-linux-x86_64`
- Android: 用 Termux 跑 Linux 版

下载后放到 PATH，或用 `--godotpcktool` 指定路径。

## 用法

```bash
# 基本用法
python patch_pck.py /path/to/PvZHybrid.pck

# 指定输出
python patch_pck.py /path/to/PvZHybrid.pck -o PvZHybrid_mod.pck

# 指定工具路径
python patch_pck.py game.pck --godotpcktool ./godotpcktool.exe
```

## Android (Termux)

```bash
pkg install python
# 用 MT管理器 解压 APK，找到 assets/ 下的 .pck 文件
python patch_pck.py /sdcard/Download/PvZHybrid.pck -o /sdcard/Download/PvZHybrid_mod.pck
# 再用 MT管理器 把 mod pck 塞回 APK assets/ 替换原文件，重新签名
```

## 工作原理

1. godotpcktool 解包 `.pck` 到临时目录
2. 复制 `mods/sun_tuner.gd` + `sun_tuner.cfg` 到解包目录 `mods/`
3. 修改 `project.godot`，在 `[autoload]` 段注册 `SunTuner="*res://mods/sun_tuner.gd"`
4. 重新打包为新 pck
5. 自动备份原 pck 为 `.bak`

## 常见问题

### godotpcktool 报错 "unsupported pck version"

换最新版 godotpcktool，或用 [GDSDecomp](https://github.com/bruvzg/gdsdecomp)。

### 解包后没有 project.godot

有些导出游戏把 project.godot 编译进 exe，pck 里只有资源。这种情况需要用 GDSDecomp 解包，或手动注入。

### 补丁后游戏闪退

1. 先恢复 `.bak`
2. 检查 Godot 版本是否和游戏一致
3. 来 GitHub 提 Issue 附上游戏版本号
