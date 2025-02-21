// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PillarObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab), typeof (MarkerObjectPrefab)})]
  public class PillarObject : ComponentBase
  {
    public PillarType m_Type;
    public float m_AnchorOffset;
    public Bounds1 m_VerticalPillarOffsetRange = new Bounds1(-1f, 1f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PillarData>());
      if ((double) this.m_AnchorOffset == 0.0)
        return;
      components.Add(ComponentType.ReadWrite<PlaceableObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Pillar>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      PillarData componentData1;
      componentData1.m_Type = this.m_Type;
      componentData1.m_OffsetRange = this.m_VerticalPillarOffsetRange;
      entityManager.SetComponentData<PillarData>(entity, componentData1);
      if ((double) this.m_AnchorOffset == 0.0)
        return;
      PlaceableObjectData componentData2 = entityManager.GetComponentData<PlaceableObjectData>(entity);
      componentData2.m_PlacementOffset.y = this.m_AnchorOffset;
      entityManager.SetComponentData<PlaceableObjectData>(entity, componentData2);
    }
  }
}
