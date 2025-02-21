// Decompiled with JetBrains decompiler
// Type: Game.GameModeExtensions
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game
{
  public static class GameModeExtensions
  {
    public static string ToRichPresence(this GameMode gameMode)
    {
      switch (gameMode)
      {
        case GameMode.Game:
          return "#StatusInGame";
        case GameMode.Editor:
          return "#StatusInEditor";
        case GameMode.MainMenu:
          return "#StatusInMainMenu";
        default:
          return string.Empty;
      }
    }

    public static bool IsEditor(this GameMode gameMode)
    {
      return (gameMode & GameMode.Editor) == GameMode.Editor;
    }

    public static bool IsGame(this GameMode gameMode)
    {
      return (gameMode & GameMode.Game) == GameMode.Game;
    }

    public static bool IsGameOrEditor(this GameMode gameMode)
    {
      return (gameMode & GameMode.GameOrEditor) != 0;
    }
  }
}
