// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LotData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct LotData : IComponentData, IQueryTypeParameter
  {
    public float m_MaxRadius;
    public Color32 m_RangeColor;

    public LotData(float maxRadius, Color32 rangeColor)
    {
      this.m_MaxRadius = maxRadius;
      this.m_RangeColor = rangeColor;
    }
  }
}
