// Decompiled with JetBrains decompiler
// Type: Game.Tools.Snap
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Tools
{
  [Flags]
  public enum Snap : uint
  {
    ExistingGeometry = 1,
    CellLength = 2,
    StraightDirection = 4,
    NetSide = 8,
    NetArea = 16, // 0x00000010
    OwnerSide = 32, // 0x00000020
    ObjectSide = 64, // 0x00000040
    NetMiddle = 128, // 0x00000080
    Shoreline = 256, // 0x00000100
    NearbyGeometry = 512, // 0x00000200
    GuideLines = 1024, // 0x00000400
    ZoneGrid = 2048, // 0x00000800
    NetNode = 4096, // 0x00001000
    ObjectSurface = 8192, // 0x00002000
    Upright = 16384, // 0x00004000
    LotGrid = 32768, // 0x00008000
    AutoParent = 65536, // 0x00010000
    PrefabType = 131072, // 0x00020000
    ContourLines = 262144, // 0x00040000
    Distance = 524288, // 0x00080000
    None = 0,
    All = 4294967295, // 0xFFFFFFFF
  }
}
