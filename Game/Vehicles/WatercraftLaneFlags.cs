// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.WatercraftLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum WatercraftLaneFlags : uint
  {
    EndOfPath = 1,
    EndReached = 2,
    UpdateOptimalLane = 4,
    TransformTarget = 8,
    ResetSpeed = 16, // 0x00000010
    FixedStart = 32, // 0x00000020
    Obsolete = 64, // 0x00000040
    Reserved = 128, // 0x00000080
    FixedLane = 256, // 0x00000100
    GroupTarget = 2048, // 0x00000800
    Queue = 4096, // 0x00001000
    IgnoreBlocker = 8192, // 0x00002000
    IsBlocked = 16384, // 0x00004000
    QueueReached = 32768, // 0x00008000
    Connection = 65536, // 0x00010000
    AlignLeft = 524288, // 0x00080000
    AlignRight = 1048576, // 0x00100000
  }
}
