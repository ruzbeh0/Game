// Decompiled with JetBrains decompiler
// Type: Game.Objects.PlacementFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Objects
{
  [Flags]
  public enum PlacementFlags
  {
    None = 0,
    RoadSide = 1,
    OnGround = 2,
    OwnerSide = 4,
    CanOverlap = 8,
    Shoreline = 16, // 0x00000010
    Floating = 32, // 0x00000020
    Hovering = 64, // 0x00000040
    HasUndergroundElements = 128, // 0x00000080
    RoadNode = 256, // 0x00000100
    Unique = 512, // 0x00000200
    Wall = 1024, // 0x00000400
    Hanging = 2048, // 0x00000800
    NetObject = 4096, // 0x00001000
    RoadEdge = 8192, // 0x00002000
    Swaying = 16384, // 0x00004000
    HasProbability = 32768, // 0x00008000
    Underwater = 65536, // 0x00010000
    Waterway = 131072, // 0x00020000
    SubNetSnap = 262144, // 0x00040000
  }
}
