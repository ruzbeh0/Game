// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MarkerMarker
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
  public class MarkerMarker : ComponentBase
  {
    public MarkerType m_MarkerType;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<MarkerMarkerData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      MarkerMarkerData componentData;
      componentData.m_MarkerType = this.m_MarkerType;
      entityManager.SetComponentData<MarkerMarkerData>(entity, componentData);
    }
  }
}
