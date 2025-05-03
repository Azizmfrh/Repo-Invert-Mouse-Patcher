# Repo-Invert-Mouse-Patcher

A simple one-click tool to invert the Y-axis in Unity games by patching `SemiFunc.InputMouseY()`.

## Requirements

- Windows
- .NET 6+ runtime (or self-contained build)
- The game’s `Managed/assembly-csharp.dll`

## Usage

1. Download the latest `InvertMousePatcher.exe` from Releases.
2. Copy it into your game folder (next to the game’s `.exe`).
3. Run it (double-click or `.\InvertMousePatcher.exe`).
4. You’ll see `✅ Patched InputMouseY() — Y inverted.`
5. Launch the game; vertical look is now inverted.

## Build from Source

```bash
git clone https://github.com/Azizmfrh/Repo-Invert-Mouse-Patcher.git
cd Repo-Invert-Mouse-Patcher/InvertMousePatcher
dotnet add package Mono.Cecil
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```
