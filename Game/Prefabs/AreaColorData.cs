// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AreaColorData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct AreaColorData : IComponentData, IQueryTypeParameter
  {
    public Color32 m_FillColor;
    public Color32 m_EdgeColor;
    public Color32 m_SelectionFillColor;
    public Color32 m_SelectionEdgeColor;
  }
}
