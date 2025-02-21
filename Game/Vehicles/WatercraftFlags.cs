// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.WatercraftFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum WatercraftFlags : uint
  {
    StayOnWaterway = 1,
    AnyLaneTarget = 2,
    Queueing = 4,
    DeckLights = 8,
    LightsOff = 16, // 0x00000010
  }
}
