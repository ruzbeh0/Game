// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ResourceConsumer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  [RequireComponent(typeof (CityServiceBuilding))]
  public class ResourceConsumer : ComponentBase
  {
    [CanBeNull]
    public NotificationIconPrefab m_NoResourceNotificationPrefab;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_NoResourceNotificationPrefab != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_NoResourceNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ResourceConsumerData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.ResourceConsumer>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<ResourceConsumerData>(entity, new ResourceConsumerData()
      {
        m_NoResourceNotificationPrefab = (UnityEngine.Object) this.m_NoResourceNotificationPrefab != (UnityEngine.Object) null ? systemManaged.GetEntity((PrefabBase) this.m_NoResourceNotificationPrefab) : Entity.Null
      });
    }
  }
}
