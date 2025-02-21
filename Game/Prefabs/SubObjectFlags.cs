// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SubObjectFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum SubObjectFlags
  {
    AnchorTop = 1,
    AnchorCenter = 2,
    RequireElevated = 16, // 0x00000010
    RequireOutsideConnection = 32, // 0x00000020
    RequireDeadEnd = 64, // 0x00000040
    RequireOrphan = 128, // 0x00000080
    OnGround = 256, // 0x00000100
    EdgePlacement = 4096, // 0x00001000
    MiddlePlacement = 8192, // 0x00002000
    AllowCombine = 16384, // 0x00004000
    CoursePlacement = 32768, // 0x00008000
    FlipInverted = 65536, // 0x00010000
    StartPlacement = 131072, // 0x00020000
    EndPlacement = 262144, // 0x00040000
    MakeOwner = 1048576, // 0x00100000
    OnMedian = 2097152, // 0x00200000
    FixedPlacement = 4194304, // 0x00400000
    PreserveShape = 8388608, // 0x00800000
    EvenSpacing = 16777216, // 0x01000000
    SpacingOverride = 33554432, // 0x02000000
  }
}
