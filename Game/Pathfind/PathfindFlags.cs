// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathfindFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Pathfind
{
  [Flags]
  public enum PathfindFlags : ushort
  {
    Stable = 1,
    IgnoreFlow = 2,
    ForceForward = 4,
    ForceBackward = 8,
    NoHeuristics = 16, // 0x0010
    ParkingReset = 32, // 0x0020
    Simplified = 64, // 0x0040
    MultipleOrigins = 128, // 0x0080
    MultipleDestinations = 256, // 0x0100
    IgnoreExtraStartAccessRequirements = 512, // 0x0200
    IgnoreExtraEndAccessRequirements = 1024, // 0x0400
    IgnorePath = 2048, // 0x0800
    SkipPathfind = 4096, // 0x1000
  }
}
