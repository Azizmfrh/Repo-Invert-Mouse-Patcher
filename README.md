# R.E.P.O-Invert-Mouse-Patcher

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

##Reset back to original

If you want to restore the original DLL:

1. **Delete** the patched `Assembly-CSharp.dll` in your game’s `*_Data/Managed/` folder.  
2. **Verify Integrity** (Steam):  
- Open **Steam**, right-click your game → **Properties**.  
- Go to **Installed Files** (or **Local Files**) tab → click **Verify integrity of game files…**  
- Wait for Steam to re-download the original DLL.  

After that, your game will be back to stock.

## Build from Source

```bash
git clone https://github.com/Azizmfrh/Repo-Invert-Mouse-Patcher.git
cd Repo-Invert-Mouse-Patcher/InvertMousePatcher
dotnet add package Mono.Cecil
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

Credit goes to Reddit user u/no-enjoyment for the initial mod
