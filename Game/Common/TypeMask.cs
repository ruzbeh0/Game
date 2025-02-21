// Decompiled with JetBrains decompiler
// Type: Game.Common.TypeMask
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Common
{
  [Flags]
  public enum TypeMask : uint
  {
    Terrain = 1,
    StaticObjects = 2,
    MovingObjects = 4,
    Net = 8,
    Zones = 16, // 0x00000010
    Areas = 32, // 0x00000020
    RouteWaypoints = 64, // 0x00000040
    RouteSegments = 128, // 0x00000080
    Labels = 256, // 0x00000100
    Water = 512, // 0x00000200
    Icons = 1024, // 0x00000400
    WaterSources = 2048, // 0x00000800
    Lanes = 4096, // 0x00001000
    None = 0,
    All = 4294967295, // 0xFFFFFFFF
  }
}
