// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum BuildingFlags : uint
  {
    RequireRoad = 1,
    NoRoadConnection = 2,
    LeftAccess = 4,
    RightAccess = 8,
    BackAccess = 16, // 0x00000010
    RestrictedPedestrian = 32, // 0x00000020
    RestrictedCar = 64, // 0x00000040
    ColorizeLot = 128, // 0x00000080
    HasLowVoltageNode = 256, // 0x00000100
    HasWaterNode = 512, // 0x00000200
    HasSewageNode = 1024, // 0x00000400
    HasInsideRoom = 2048, // 0x00000800
    RestrictedParking = 4096, // 0x00001000
    RestrictedTrack = 8192, // 0x00002000
    CanBeOnRoad = 16384, // 0x00004000
  }
}
