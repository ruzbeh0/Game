// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetArrowData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct NetArrowData : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public Color32 m_RoadColor;
    public Color32 m_TrackColor;
    public int m_MaterialIndex;
  }
}
