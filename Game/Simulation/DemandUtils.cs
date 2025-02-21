// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DemandUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public static class DemandUtils
  {
    public const int kUpdateInterval = 16;
    public const int kCountCompanyUpdateOffset = 1;
    public const int kCommercialUpdateOffset = 4;
    public const int kIndustrialUpdateOffset = 7;
    public const int kResidentialUpdateOffset = 10;
    public const int kZoneSpawnUpdateOffset = 13;

    public static int GetDemandFactorEffect(int total, float effect)
    {
      return Mathf.RoundToInt(100f * effect);
    }
  }
}
