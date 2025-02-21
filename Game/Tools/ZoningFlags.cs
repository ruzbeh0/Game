// Decompiled with JetBrains decompiler
// Type: Game.Tools.ZoningFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Tools
{
  [Flags]
  public enum ZoningFlags : uint
  {
    FloodFill = 1,
    Marquee = 2,
    Zone = 4,
    Dezone = 8,
    Paint = 16, // 0x00000010
    Overwrite = 32, // 0x00000020
  }
}
