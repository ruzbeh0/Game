// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EnclosedArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new Type[] {typeof (LotPrefab), typeof (SpacePrefab), typeof (SurfacePrefab)})]
  public class EnclosedArea : ComponentBase
  {
    public NetLanePrefab m_BorderLaneType;
    public bool m_CounterClockWise;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_BorderLaneType);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<EnclosedAreaData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Net.SubLane>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      EnclosedAreaData componentData;
      // ISSUE: reference to a compiler-generated method
      componentData.m_BorderLanePrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_BorderLaneType);
      componentData.m_CounterClockWise = this.m_CounterClockWise;
      entityManager.SetComponentData<EnclosedAreaData>(entity, componentData);
    }
  }
}
