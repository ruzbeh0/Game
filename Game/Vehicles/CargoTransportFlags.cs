// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.CargoTransportFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum CargoTransportFlags : uint
  {
    Returning = 1,
    EnRoute = 2,
    Boarding = 4,
    Arriving = 8,
    RequiresMaintenance = 16, // 0x00000010
    Refueling = 32, // 0x00000020
    AbandonRoute = 64, // 0x00000040
    RouteSource = 128, // 0x00000080
    Testing = 256, // 0x00000100
    RequireStop = 512, // 0x00000200
    DummyTraffic = 1024, // 0x00000400
    Disabled = 2048, // 0x00000800
  }
}
