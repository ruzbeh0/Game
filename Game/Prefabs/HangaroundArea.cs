// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HangaroundArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new Type[] {typeof (LotPrefab), typeof (SpacePrefab)})]
  public class HangaroundArea : ComponentBase
  {
    public ActivityType[] m_Activities;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SpawnLocationData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<HangaroundLocation>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      NavigationArea component = this.GetComponent<NavigationArea>();
      SpawnLocationData componentData;
      componentData.m_ConnectionType = component.m_ConnectionType;
      componentData.m_TrackTypes = component.m_TrackTypes;
      componentData.m_RoadTypes = component.m_RoadTypes;
      componentData.m_RequireAuthorization = false;
      if (this.m_Activities != null && this.m_Activities.Length != 0)
      {
        componentData.m_ActivityMask = new ActivityMask();
        for (int index = 0; index < this.m_Activities.Length; ++index)
          componentData.m_ActivityMask.m_Mask |= new ActivityMask(this.m_Activities[index]).m_Mask;
      }
      else
        componentData.m_ActivityMask = new ActivityMask(ActivityType.Standing);
      entityManager.SetComponentData<SpawnLocationData>(entity, componentData);
    }
  }
}
