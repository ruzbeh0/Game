// Decompiled with JetBrains decompiler
// Type: Game.GameMode
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game
{
  [Flags]
  public enum GameMode
  {
    None = 0,
    Other = 1,
    Game = 2,
    Editor = 4,
    MainMenu = 8,
    GameOrEditor = Editor | Game, // 0x00000006
    All = GameOrEditor | MainMenu | Other, // 0x0000000F
  }
}
