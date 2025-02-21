// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficLightType
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Flags]
  public enum TrafficLightType
  {
    VehicleLeft = 1,
    VehicleRight = 2,
    CrossingLeft = 4,
    CrossingRight = 8,
    AllowFlipped = 16, // 0x00000010
  }
}
