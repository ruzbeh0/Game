// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.AircraftFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum AircraftFlags : uint
  {
    StayOnTaxiway = 1,
    Emergency = 2,
    StayMidAir = 4,
    Blocking = 8,
    Working = 16, // 0x00000010
    IgnoreParkedVehicle = 32, // 0x00000020
  }
}
