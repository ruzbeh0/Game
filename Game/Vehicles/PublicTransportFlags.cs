// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.PublicTransportFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum PublicTransportFlags : uint
  {
    Returning = 1,
    EnRoute = 2,
    Boarding = 4,
    Arriving = 8,
    Launched = 16, // 0x00000010
    Evacuating = 32, // 0x00000020
    PrisonerTransport = 64, // 0x00000040
    RequiresMaintenance = 128, // 0x00000080
    Refueling = 256, // 0x00000100
    AbandonRoute = 512, // 0x00000200
    RouteSource = 1024, // 0x00000400
    Testing = 2048, // 0x00000800
    RequireStop = 4096, // 0x00001000
    DummyTraffic = 8192, // 0x00002000
    StopLeft = 16384, // 0x00004000
    StopRight = 32768, // 0x00008000
    Disabled = 65536, // 0x00010000
    Full = 131072, // 0x00020000
  }
}
