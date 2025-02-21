// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum LaneFlags
  {
    Invert = 1,
    Slave = 2,
    Master = 4,
    Road = 8,
    Pedestrian = 16, // 0x00000010
    Parking = 32, // 0x00000020
    Track = 64, // 0x00000040
    Twoway = 128, // 0x00000080
    DisconnectedStart = 256, // 0x00000100
    DisconnectedEnd = 512, // 0x00000200
    Secondary = 1024, // 0x00000400
    Utility = 2048, // 0x00000800
    Underground = 4096, // 0x00001000
    CrossRoad = 8192, // 0x00002000
    PublicOnly = 16384, // 0x00004000
    OnWater = 32768, // 0x00008000
    Virtual = 65536, // 0x00010000
    FindAnchor = 131072, // 0x00020000
    LeftLimit = 262144, // 0x00040000
    RightLimit = 524288, // 0x00080000
    ParkingLeft = 1048576, // 0x00100000
    ParkingRight = 2097152, // 0x00200000
    HasAuxiliary = 4194304, // 0x00400000
    EvenSpacing = 8388608, // 0x00800000
    PseudoRandom = 16777216, // 0x01000000
  }
}
