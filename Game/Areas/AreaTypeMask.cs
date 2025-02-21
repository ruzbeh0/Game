// Decompiled with JetBrains decompiler
// Type: Game.Areas.AreaTypeMask
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Areas
{
  [Flags]
  public enum AreaTypeMask
  {
    None = 0,
    Lots = 1,
    Districts = 2,
    MapTiles = 4,
    Spaces = 8,
    Surfaces = 16, // 0x00000010
  }
}
