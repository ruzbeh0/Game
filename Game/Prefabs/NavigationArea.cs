// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NavigationArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Net;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new Type[] {typeof (LotPrefab), typeof (SpacePrefab)})]
  public class NavigationArea : ComponentBase
  {
    public RouteConnectionType m_ConnectionType = RouteConnectionType.Pedestrian;
    public RouteConnectionType m_SecondaryType;
    public TrackTypes m_TrackTypes;
    public RoadTypes m_RoadTypes;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<NavigationAreaData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Navigation>());
      components.Add(ComponentType.ReadWrite<Game.Net.SubLane>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      NavigationAreaData componentData;
      componentData.m_ConnectionType = this.m_ConnectionType;
      componentData.m_SecondaryType = this.m_SecondaryType;
      componentData.m_TrackTypes = this.m_TrackTypes;
      componentData.m_RoadTypes = this.m_RoadTypes;
      entityManager.SetComponentData<NavigationAreaData>(entity, componentData);
    }
  }
}
