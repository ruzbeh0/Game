// Decompiled with JetBrains decompiler
// Type: Game.Buildings.BuildingNotification
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Buildings
{
  [Flags]
  public enum BuildingNotification : byte
  {
    None = 0,
    AirPollution = 1,
    NoisePollution = 2,
    GroundPollution = 4,
  }
}
