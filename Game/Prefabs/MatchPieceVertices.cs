// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MatchPieceVertices
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new Type[] {typeof (NetPiecePrefab)})]
  public class MatchPieceVertices : ComponentBase
  {
    public float[] m_Offsets;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<NetVertexMatchData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      NetVertexMatchData componentData = new NetVertexMatchData();
      componentData.m_Offsets = (float3) float.NaN;
      if (this.m_Offsets != null)
      {
        if (this.m_Offsets.Length >= 1)
          componentData.m_Offsets.x = this.m_Offsets[0];
        if (this.m_Offsets.Length >= 2)
          componentData.m_Offsets.y = this.m_Offsets[1];
        if (this.m_Offsets.Length >= 3)
          componentData.m_Offsets.z = this.m_Offsets[2];
      }
      entityManager.SetComponentData<NetVertexMatchData>(entity, componentData);
    }
  }
}
