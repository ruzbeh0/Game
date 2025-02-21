// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ActivityLocation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Rendering;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new System.Type[] {typeof (StaticObjectPrefab), typeof (MarkerObjectPrefab), typeof (VehiclePrefab)})]
  public class ActivityLocation : ComponentBase
  {
    public ActivityLocation.LocationInfo[] m_Locations;
    public NetInvertMode m_InvertWhen;
    public string m_AnimatedPropName;
    public bool m_RequireAuthorization;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Locations == null)
        return;
      for (int index = 0; index < this.m_Locations.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Locations[index].m_Activity);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      if (!(this.prefab is VehiclePrefab) && !(this.prefab is BuildingPrefab))
        components.Add(ComponentType.ReadWrite<SpawnLocationData>());
      components.Add(ComponentType.ReadWrite<ActivityLocationElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (this.prefab is VehiclePrefab || this.prefab is BuildingPrefab)
        return;
      components.Add(ComponentType.ReadWrite<Game.Objects.SpawnLocation>());
      components.Add(ComponentType.ReadWrite<Game.Objects.ActivityLocation>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      SpawnLocationData componentData1;
      componentData1.m_ConnectionType = RouteConnectionType.Pedestrian;
      componentData1.m_ActivityMask = new ActivityMask();
      componentData1.m_RoadTypes = RoadTypes.None;
      componentData1.m_TrackTypes = TrackTypes.None;
      componentData1.m_RequireAuthorization = this.m_RequireAuthorization;
      if (this.m_Locations != null && this.m_Locations.Length != 0)
      {
        DynamicBuffer<ActivityLocationElement> buffer = entityManager.GetBuffer<ActivityLocationElement>(entity);
        buffer.ResizeUninitialized(this.m_Locations.Length);
        // ISSUE: variable of a compiler-generated type
        PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
        // ISSUE: reference to a compiler-generated method
        AnimatedPropID propId = entityManager.World.GetExistingSystemManaged<AnimatedSystem>().GetPropID(this.m_AnimatedPropName);
        for (int index = 0; index < this.m_Locations.Length; ++index)
        {
          ActivityLocation.LocationInfo location = this.m_Locations[index];
          // ISSUE: reference to a compiler-generated method
          ActivityLocationElement activityLocationElement = new ActivityLocationElement()
          {
            m_Prefab = existingSystemManaged.GetEntity((PrefabBase) location.m_Activity),
            m_Position = location.m_Position,
            m_Rotation = location.m_Rotation,
            m_PropID = propId
          };
          switch (this.m_InvertWhen)
          {
            case NetInvertMode.LefthandTraffic:
              activityLocationElement.m_ActivityFlags |= ActivityFlags.InvertLefthandTraffic;
              break;
            case NetInvertMode.RighthandTraffic:
              activityLocationElement.m_ActivityFlags |= ActivityFlags.InvertRighthandTraffic;
              break;
          }
          ActivityLocationData componentData2 = entityManager.GetComponentData<ActivityLocationData>(activityLocationElement.m_Prefab);
          activityLocationElement.m_ActivityMask = componentData2.m_ActivityMask;
          componentData1.m_ActivityMask.m_Mask |= componentData2.m_ActivityMask.m_Mask;
          buffer[index] = activityLocationElement;
        }
      }
      else
        ComponentBase.baseLog.ErrorFormat((UnityEngine.Object) this.prefab, "Empty activity location array: {0}", (object) this.prefab.name);
      if (this.prefab is VehiclePrefab || this.prefab is BuildingPrefab)
        return;
      entityManager.SetComponentData<SpawnLocationData>(entity, componentData1);
    }

    [Serializable]
    public class LocationInfo
    {
      public ActivityLocationPrefab m_Activity;
      public float3 m_Position = float3.zero;
      public quaternion m_Rotation = quaternion.identity;
    }
  }
}
