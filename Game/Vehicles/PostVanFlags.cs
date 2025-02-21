// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.PostVanFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Vehicles
{
  [Flags]
  public enum PostVanFlags : uint
  {
    Returning = 1,
    Delivering = 2,
    Collecting = 4,
    DeliveryEmpty = 8,
    CollectFull = 16, // 0x00000010
    EstimatedEmpty = 32, // 0x00000020
    EstimatedFull = 64, // 0x00000040
    Disabled = 128, // 0x00000080
    ClearChecked = 256, // 0x00000100
  }
}
