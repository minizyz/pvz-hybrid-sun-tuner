#!/usr/bin/env python3
"""
Sun Tuner — PvZ Hybrid Remake .pck 补丁工具

自动完成：解包 pck → 注入 sun_tuner.gd 作为 autoload → 重新打包 pck。
支持 Windows / Linux / Android (APK 内 pck)。

依赖：godotpcktool (https://github.com/bdach/godotpcktool)

用法：
   python patch_pck.py <game.pck> [--output patched.pck]
"""

import argparse
import os
import re
import shutil
import subprocess
import sys
import tempfile
from pathlib import Path


def find_godotpcktool() -> str:
    for candidate in ["godotpcktool", "godotpcktool.exe"]:
        path = shutil.which(candidate)
        if path:
            return path
    for p in [
        "./godotpcktool",
        "./godotpcktool.exe",
        "/usr/local/bin/godotpcktool",
        str(Path.home() / "bin" / "godotpcktool"),
    ]:
        if os.path.exists(p):
            return p
    return ""


def unpack_pck(tool: str, pck_path: str, out_dir: str) -> None:
    print(f"[*] 解包 {pck_path} -> {out_dir}")
    subprocess.run([tool, pck_path, "-x", out_dir], check=True)


def repack_pck(tool: str, src_dir: str, out_pck: str) -> None:
    print(f"[*] 重新打包 {src_dir} -> {out_pck}")
    subprocess.run([tool, out_pck, "-a", src_dir], check=True)


def inject_autoload(project_godot: Path) -> None:
    if not project_godot.exists():
        print(f"[!] 未找到 {project_godot}，跳过 autoload 注入")
        return

    text = project_godot.read_text(encoding="utf-8", errors="replace")
    if "SunTuner" in text and "sun_tuner.gd" in text:
        print("[*] autoload 已存在，跳过")
        return

    autoload_line = 'SunTuner="*res://mods/sun_tuner.gd"'

    if "[autoload]" in text:
        text = re.sub(
            r"(\[autoload\][^\[]*?)$",
            lambda m: m.group(1).rstrip() + "\n" + autoload_line + "\n",
            text,
            count=1,
            flags=re.MULTILINE,
        )
    else:
        text = text.rstrip() + "\n\n[autoload]\n" + autoload_line + "\n"

    project_godot.write_text(text, encoding="utf-8")
    print(f"[+] 已注入 autoload: {autoload_line}")


def copy_mod_files(repo_dir: Path, unpacked_dir: Path) -> None:
    src_mods = repo_dir / "mods"
    dst_mods = unpacked_dir / "mods"
    dst_mods.mkdir(parents=True, exist_ok=True)
    for f in src_mods.glob("*"):
        if f.is_file():
            shutil.copy2(f, dst_mods / f.name)
            print(f"[+] 复制 {f.name} -> mods/")


def main() -> int:
    parser = argparse.ArgumentParser(description="Sun Tuner .pck 补丁工具")
    parser.add_argument("pck", help="游戏原 pck 文件路径")
    parser.add_argument("--output", "-o", help="输出 patched pck 路径", default=None)
    parser.add_argument("--godotpcktool", help="godotpcktool 可执行文件路径", default="")
    args = parser.parse_args()

    pck_path = Path(args.pck).resolve()
    if not pck_path.exists():
        print(f"[!] 文件不存在: {pck_path}")
        return 1

    tool = args.godotpcktool or find_godotpcktool()
    if not tool:
        print("[!] 未找到 godotpcktool。请下载: https://github.com/bdach/godotpcktool/releases")
        print("    下载后用 --godotpcktool /path/to/godotpcktool 指定路径")
        return 1
    print(f"[*] 使用工具: {tool}")

    output = Path(args.output) if args.output else pck_path.with_suffix(".patched.pck")

    backup = pck_path.with_suffix(pck_path.suffix + ".bak")
    if not backup.exists():
        shutil.copy2(pck_path, backup)
        print(f"[*] 已备份原文件 -> {backup}")

    repo_dir = Path(__file__).resolve().parent.parent

    with tempfile.TemporaryDirectory() as tmp:
        tmp_dir = Path(tmp)
        unpack_pck(tool, str(pck_path), str(tmp_dir))
        copy_mod_files(repo_dir, tmp_dir)
        project_godot = tmp_dir / "project.godot"
        inject_autoload(project_godot)
        repack_pck(tool, str(tmp_dir), str(output))

    print("")
    print(f"[✓] 完成！补丁后的 pck: {output}")
    print(f"[*] 替换游戏原 pck 文件即可（已自动备份为 .bak）")
    print(f"[*] 启动游戏后，屏幕右上角应出现 🌻 按钮")
    return 0


if __name__ == "__main__":
    sys.exit(main())
