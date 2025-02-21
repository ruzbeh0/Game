// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GoodsDeliveryFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Simulation
{
  [Flags]
  public enum GoodsDeliveryFlags : ushort
  {
    BuildingUpkeep = 1,
    CommercialAllowed = 2,
    IndustrialAllowed = 4,
    ImportAllowed = 8,
    ResourceExportTarget = 16, // 0x0010
  }
}
