// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.GarbageTruckFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum GarbageTruckFlags : uint
  {
    Returning = 1,
    IndustrialWasteOnly = 2,
    Unloading = 4,
    Disabled = 8,
    EstimatedFull = 16, // 0x00000010
    ClearChecked = 32, // 0x00000020
  }
}
