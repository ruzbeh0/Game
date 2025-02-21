// Decompiled with JetBrains decompiler
// Type: Game.Creatures.CreatureLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Creatures
{
  [Flags]
  public enum CreatureLaneFlags : uint
  {
    EndOfPath = 1,
    EndReached = 2,
    TransformTarget = 4,
    ParkingSpace = 8,
    Obsolete = 16, // 0x00000010
    Transport = 32, // 0x00000020
    Connection = 64, // 0x00000040
    Taxi = 128, // 0x00000080
    Backward = 256, // 0x00000100
    WaitSignal = 512, // 0x00000200
    FindLane = 1024, // 0x00000400
    Stuck = 2048, // 0x00000800
    Area = 4096, // 0x00001000
    Hangaround = 8192, // 0x00002000
    Checked = 16384, // 0x00004000
    Action = 32768, // 0x00008000
    ActivityDone = 65536, // 0x00010000
    Swimming = 131072, // 0x00020000
    Flying = 262144, // 0x00040000
    WaitPosition = 524288, // 0x00080000
    Leader = 1048576, // 0x00100000
    EmergeUnspawned = 2097152, // 0x00200000
  }
}
