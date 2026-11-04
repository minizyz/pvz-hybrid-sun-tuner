# 📥 Windows 安装指南

## 第一步：找到游戏 pck 文件

PvZ Hybrid Remake (Godot 版) 的资源包通常是：

```
游戏安装目录/
├── PvZHybrid.exe
└── PvZHybrid.pck          ← 这个就是要补丁的文件
```

如果目录下没有 `.pck`，说明资源被编译进了 exe，需要用 [GDSDecomp](https://github.com/bruvzg/gdsdecomp) 解包 exe。

## 第二步：下载补丁工具

1. 克隆本仓库：
   ```bash
   git clone https://github.com/minizyz/pvz-hybrid-sun-tuner.git
   cd pvz-hybrid-sun-tuner
   ```

2. 下载 [godotpcktool](https://github.com/bdach/godotpcktool/releases) 的 Windows 版（`godotpcktool-windows-x86_64.exe`），放到 PATH 或同目录。

## 第三步：打补丁

```bash
python tools/patch_pck.py "C:\Games\PvZHybrid\PvZHybrid.pck"
```

输出：
- `PvZHybrid.pck.bak` — 原文件备份
- `PvZHybrid.patched.pck` — 补丁后的文件

## 第四步：替换原文件

```bash
copy /Y PvZHybrid.patched.pck PvZHybrid.pck
```

或者直接在文件管理器里把 `PvZHybrid.patched.pck` 重命名为 `PvZHybrid.pck` 替换原文件。

## 第五步：验证

启动游戏，进入任意关卡：

- 屏幕**右上角**应出现 🌻 按钮
- 点击展开控制面板
- 拖动「天空阳光间隔」滑块到 1 秒，应能明显看到阳光掉落变快

如果没有看到按钮，检查日志：
- Godot 日志位置：`%APPDATA%\Godot\app_userdata\<游戏名>\logs\`
- 或控制台输出（如果用命令行启动 exe）
- 搜索 `[SunTuner]` 关键字

## 卸载

把 `.bak` 文件改回 `.pck` 即可。
