// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ITradeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Economy;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public interface ITradeSystem
  {
    float GetTradePrice(
      Resource resource,
      OutsideConnectionTransferType type,
      bool import,
      DynamicBuffer<CityModifier> cityEffects);
  }
}
