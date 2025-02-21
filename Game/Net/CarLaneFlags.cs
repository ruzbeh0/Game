// Decompiled with JetBrains decompiler
// Type: Game.Net.CarLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum CarLaneFlags : uint
  {
    Unsafe = 1,
    UTurnLeft = 2,
    Invert = 4,
    SideConnection = 8,
    TurnLeft = 16, // 0x00000010
    TurnRight = 32, // 0x00000020
    LevelCrossing = 64, // 0x00000040
    Twoway = 128, // 0x00000080
    IsSecured = 256, // 0x00000100
    Runway = 512, // 0x00000200
    Yield = 1024, // 0x00000400
    Stop = 2048, // 0x00000800
    ForbidCombustionEngines = 4096, // 0x00001000
    ForbidTransitTraffic = 8192, // 0x00002000
    ForbidHeavyTraffic = 16384, // 0x00004000
    PublicOnly = 32768, // 0x00008000
    Highway = 65536, // 0x00010000
    UTurnRight = 131072, // 0x00020000
    GentleTurnLeft = 262144, // 0x00040000
    GentleTurnRight = 524288, // 0x00080000
    Forward = 1048576, // 0x00100000
    Approach = 2097152, // 0x00200000
    Roundabout = 4194304, // 0x00400000
    RightLimit = 8388608, // 0x00800000
    LeftLimit = 16777216, // 0x01000000
    ForbidPassing = 33554432, // 0x02000000
    RightOfWay = 67108864, // 0x04000000
    TrafficLights = 134217728, // 0x08000000
    ParkingLeft = 268435456, // 0x10000000
    ParkingRight = 536870912, // 0x20000000
    Forbidden = 1073741824, // 0x40000000
    AllowEnter = 2147483648, // 0x80000000
  }
}
