// Decompiled with JetBrains decompiler
// Type: Game.Simulation.IServiceFeeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public interface IServiceFeeSystem
  {
    int3 GetServiceFees(PlayerResource resource);

    int GetServiceFeeIncomeEstimate(PlayerResource resource, float fee);
  }
}
