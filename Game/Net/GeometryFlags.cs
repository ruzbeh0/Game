// Decompiled with JetBrains decompiler
// Type: Game.Net.GeometryFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum GeometryFlags
  {
    StraightEdges = 1,
    StrictNodes = 2,
    SnapCellSize = 4,
    SupportRoundabout = 8,
    LoweredIsTunnel = 16, // 0x00000010
    RaisedIsElevated = 32, // 0x00000020
    NoEdgeConnection = 64, // 0x00000040
    SnapToNetAreas = 128, // 0x00000080
    StraightEnds = 256, // 0x00000100
    RequireElevated = 512, // 0x00000200
    SymmetricalEdges = 1024, // 0x00000400
    BlockZone = 2048, // 0x00000800
    Directional = 4096, // 0x00001000
    SmoothSlopes = 8192, // 0x00002000
    SmoothElevation = 16384, // 0x00004000
    FlipTrafficHandedness = 32768, // 0x00008000
    Asymmetric = 65536, // 0x00010000
    FlattenTerrain = 131072, // 0x00020000
    ClipTerrain = 262144, // 0x00040000
    Marker = 524288, // 0x00080000
    MiddlePillars = 1048576, // 0x00100000
    StandingNodes = 2097152, // 0x00200000
    ExclusiveGround = 4194304, // 0x00400000
    NoCurveSplit = 8388608, // 0x00800000
    SubOwner = 16777216, // 0x01000000
    OnWater = 33554432, // 0x02000000
    IsLefthanded = 67108864, // 0x04000000
    InvertCompositionHandedness = 134217728, // 0x08000000
    FlipCompositionHandedness = 268435456, // 0x10000000
  }
}
