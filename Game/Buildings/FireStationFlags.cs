// Decompiled with JetBrains decompiler
// Type: Game.Buildings.FireStationFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Buildings
{
  [Flags]
  public enum FireStationFlags : byte
  {
    HasAvailableFireEngines = 1,
    HasFreeFireEngines = 2,
    HasAvailableFireHelicopters = 4,
    HasFreeFireHelicopters = 8,
    DisasterResponseAvailable = 16, // 0x10
  }
}
