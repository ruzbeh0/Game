// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TaxableResourceData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TaxableResourceData : IComponentData, IQueryTypeParameter
  {
    public byte m_TaxAreas;

    public bool Contains(TaxAreaType areaType)
    {
      return ((uint) this.m_TaxAreas & (uint) TaxableResourceData.GetBit(areaType)) > 0U;
    }

    public TaxableResourceData(IEnumerable<TaxAreaType> taxAreas)
    {
      this.m_TaxAreas = (byte) 0;
      foreach (TaxAreaType taxArea in taxAreas)
        this.m_TaxAreas |= (byte) TaxableResourceData.GetBit(taxArea);
    }

    private static int GetBit(TaxAreaType areaType) => 1 << (int) (areaType - (byte) 1);
  }
}
