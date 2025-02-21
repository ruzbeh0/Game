// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.VehicleLaunch
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Events;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Events/", new Type[] {typeof (EventPrefab)})]
  public class VehicleLaunch : ComponentBase
  {
    public TransportType m_TransportType;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<VehicleLaunchData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.VehicleLaunch>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      VehicleLaunchData componentData;
      componentData.m_TransportType = this.m_TransportType;
      entityManager.SetComponentData<VehicleLaunchData>(entity, componentData);
    }
  }
}
