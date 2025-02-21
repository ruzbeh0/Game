// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CraneObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab)})]
  public class CraneObject : ComponentBase
  {
    public Bounds1 m_DistanceRange = new Bounds1(0.0f, float.MaxValue);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CraneData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Crane>());
      components.Add(ComponentType.ReadWrite<PointOfInterest>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      CraneData componentData;
      componentData.m_DistanceRange = this.m_DistanceRange;
      entityManager.SetComponentData<CraneData>(entity, componentData);
    }
  }
}
