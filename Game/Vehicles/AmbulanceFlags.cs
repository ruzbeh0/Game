// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.AmbulanceFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum AmbulanceFlags : uint
  {
    Returning = 1,
    Dispatched = 2,
    Transporting = 4,
    AnyHospital = 8,
    FindHospital = 16, // 0x00000010
    AtTarget = 32, // 0x00000020
    Disembarking = 64, // 0x00000040
    Disabled = 128, // 0x00000080
    Critical = 256, // 0x00000100
  }
}
