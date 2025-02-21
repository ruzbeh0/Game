// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StandingObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab)})]
  public class StandingObject : ComponentBase
  {
    public float3 m_LegSize = new float3(0.3f, 2.5f, 0.3f);
    public bool m_CircularLeg = true;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ObjectGeometryData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      ObjectGeometryData componentData = entityManager.GetComponentData<ObjectGeometryData>(entity) with
      {
        m_LegSize = this.m_LegSize
      };
      componentData.m_Flags |= this.m_CircularLeg ? GeometryFlags.Standing | GeometryFlags.CircularLeg : GeometryFlags.Standing;
      entityManager.SetComponentData<ObjectGeometryData>(entity, componentData);
    }
  }
}
