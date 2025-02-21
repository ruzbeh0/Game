// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StatisticParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct StatisticParameterData : IBufferElementData
  {
    public int m_Value;
    public Color m_Color;

    public StatisticParameterData(int value, Color color)
    {
      this.m_Value = value;
      this.m_Color = color;
    }
  }
}
