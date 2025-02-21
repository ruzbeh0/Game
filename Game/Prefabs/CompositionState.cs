// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CompositionState
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum CompositionState
  {
    BlockUTurn = 1,
    ExclusiveGround = 2,
    HasSurface = 4,
    HasForwardRoadLanes = 8,
    SeparatedCarriageways = 16, // 0x00000010
    HasPedestrianLanes = 32, // 0x00000020
    HasBackwardRoadLanes = 64, // 0x00000040
    HasForwardTrackLanes = 128, // 0x00000080
    HasBackwardTrackLanes = 256, // 0x00000100
    Asymmetric = 512, // 0x00000200
    Marker = 1024, // 0x00000400
    BlockZone = 2048, // 0x00000800
    Multilane = 4096, // 0x00001000
    LowerToTerrain = 8192, // 0x00002000
    RaiseToTerrain = 16384, // 0x00004000
    NoSubCollisions = 32768, // 0x00008000
    Airspace = 65536, // 0x00010000
  }
}
