// Decompiled with JetBrains decompiler
// Type: Game.Net.ParkingLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum ParkingLaneFlags
  {
    Invert = 1,
    StartingLane = 2,
    EndingLane = 4,
    SecondaryStart = 8,
    FixedSlots = 16, // 0x00000010
    LeftSide = 32, // 0x00000020
    RightSide = 64, // 0x00000040
    TaxiAvailabilityUpdated = 128, // 0x00000080
    TaxiAvailabilityChanged = 256, // 0x00000100
    VirtualLane = 512, // 0x00000200
    FindConnections = 1024, // 0x00000400
    ParkingLeft = 2048, // 0x00000800
    ParkingRight = 4096, // 0x00001000
    ParkingDisabled = 8192, // 0x00002000
    AllowEnter = 16384, // 0x00004000
    AllowExit = 32768, // 0x00008000
    SpecialVehicles = 65536, // 0x00010000
  }
}
