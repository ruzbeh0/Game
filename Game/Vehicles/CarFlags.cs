// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.CarFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum CarFlags : uint
  {
    Emergency = 1,
    StayOnRoad = 2,
    AnyLaneTarget = 4,
    Warning = 8,
    Queueing = 16, // 0x00000010
    UsePublicTransportLanes = 32, // 0x00000020
    PreferPublicTransportLanes = 64, // 0x00000040
    Sign = 128, // 0x00000080
    Interior = 256, // 0x00000100
    Working = 512, // 0x00000200
    SignalAnimation1 = 1024, // 0x00000400
    SignalAnimation2 = 2048, // 0x00000800
  }
}
