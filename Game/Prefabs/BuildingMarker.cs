// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingMarker
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Notifications/", new Type[] {typeof (NotificationIconPrefab)})]
  public class BuildingMarker : ComponentBase
  {
    public BuildingType m_BuildingType;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<BuildingMarkerData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      BuildingMarkerData componentData;
      componentData.m_BuildingType = this.m_BuildingType;
      entityManager.SetComponentData<BuildingMarkerData>(entity, componentData);
    }
  }
}
