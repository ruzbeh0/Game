// Decompiled with JetBrains decompiler
// Type: Game.Zones.CellFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Zones
{
  [Flags]
  public enum CellFlags : ushort
  {
    None = 0,
    Blocked = 1,
    Shared = 2,
    Roadside = 4,
    Visible = 8,
    Overridden = 16, // 0x0010
    Occupied = 32, // 0x0020
    Selected = 64, // 0x0040
    Redundant = 128, // 0x0080
    Updating = 256, // 0x0100
    RoadLeft = 512, // 0x0200
    RoadRight = 1024, // 0x0400
    RoadBack = 2048, // 0x0800
  }
}
