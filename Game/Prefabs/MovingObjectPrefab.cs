// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MovingObjectPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Rendering;
using Game.Simulation;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {})]
  public class MovingObjectPrefab : ObjectGeometryPrefab
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<MovingObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<UpdateFrame>());
      if (!components.Contains(ComponentType.ReadWrite<Moving>()))
        return;
      components.Add(ComponentType.ReadWrite<TransformFrame>());
      components.Add(ComponentType.ReadWrite<InterpolatedTransform>());
    }

    protected override void RefreshArchetype(EntityManager entityManager, Entity entity)
    {
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
      componentTypeSet.Add(ComponentType.ReadWrite<Moving>());
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      entityManager.SetComponentData<ObjectData>(entity, new ObjectData()
      {
        m_Archetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet))
      });
      componentTypeSet.Clear();
      componentTypeSet.Add(ComponentType.ReadWrite<Stopped>());
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      entityManager.SetComponentData<MovingObjectData>(entity, new MovingObjectData()
      {
        m_StoppedArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet))
      });
    }
  }
}
