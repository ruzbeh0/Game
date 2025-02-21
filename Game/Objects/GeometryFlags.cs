// Decompiled with JetBrains decompiler
// Type: Game.Objects.GeometryFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Objects
{
  [Flags]
  public enum GeometryFlags
  {
    None = 0,
    Circular = 1,
    Overridable = 2,
    Marker = 4,
    ExclusiveGround = 8,
    DeleteOverridden = 16, // 0x00000010
    Physical = 32, // 0x00000020
    WalkThrough = 64, // 0x00000040
    Standing = 128, // 0x00000080
    CircularLeg = 256, // 0x00000100
    OverrideZone = 512, // 0x00000200
    OccupyZone = 1024, // 0x00000400
    CanSubmerge = 2048, // 0x00000800
    BaseCollision = 4096, // 0x00001000
    IgnoreSecondaryCollision = 8192, // 0x00002000
    OptionalAttach = 16384, // 0x00004000
    Brushable = 32768, // 0x00008000
    Stampable = 65536, // 0x00010000
    LowCollisionPriority = 131072, // 0x00020000
    IgnoreBottomCollision = 262144, // 0x00040000
    HasBase = 524288, // 0x00080000
    HasLot = 1048576, // 0x00100000
    IgnoreLegCollision = 2097152, // 0x00200000
  }
}
