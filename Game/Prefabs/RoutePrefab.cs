// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RoutePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Rendering;
using Game.Routes;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Routes/", new System.Type[] {})]
  public class RoutePrefab : PrefabBase, IColored
  {
    public Material m_Material;
    public float m_Width = 4f;
    public float m_SegmentLength = 64f;
    public UnityEngine.Color m_Color = UnityEngine.Color.magenta;
    public string m_LocaleID;

    public Color32 color => (Color32) this.m_Color;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<RouteData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<Route>()))
      {
        components.Add(ComponentType.ReadWrite<RouteWaypoint>());
        components.Add(ComponentType.ReadWrite<RouteSegment>());
        components.Add(ComponentType.ReadWrite<Game.Routes.Color>());
        components.Add(ComponentType.ReadWrite<RouteBufferIndex>());
      }
      else if (components.Contains(ComponentType.ReadWrite<Waypoint>()))
      {
        components.Add(ComponentType.ReadWrite<Position>());
        components.Add(ComponentType.ReadWrite<Owner>());
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<Segment>()))
          return;
        components.Add(ComponentType.ReadWrite<CurveElement>());
        components.Add(ComponentType.ReadWrite<Owner>());
      }
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      HashSet<ComponentType> componentTypeSet1 = new HashSet<ComponentType>();
      HashSet<ComponentType> componentTypeSet2 = new HashSet<ComponentType>();
      HashSet<ComponentType> componentTypeSet3 = new HashSet<ComponentType>();
      HashSet<ComponentType> componentTypeSet4 = new HashSet<ComponentType>();
      componentTypeSet1.Add(ComponentType.ReadWrite<Route>());
      componentTypeSet2.Add(ComponentType.ReadWrite<Waypoint>());
      componentTypeSet3.Add(ComponentType.ReadWrite<Waypoint>());
      componentTypeSet3.Add(ComponentType.ReadWrite<Connected>());
      componentTypeSet4.Add(ComponentType.ReadWrite<Segment>());
      for (int index = 0; index < list.Count; ++index)
      {
        list[index].GetArchetypeComponents(componentTypeSet1);
        list[index].GetArchetypeComponents(componentTypeSet2);
        list[index].GetArchetypeComponents(componentTypeSet3);
        list[index].GetArchetypeComponents(componentTypeSet4);
      }
      componentTypeSet1.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet2.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet3.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet4.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet1.Add(ComponentType.ReadWrite<Updated>());
      componentTypeSet2.Add(ComponentType.ReadWrite<Updated>());
      componentTypeSet3.Add(ComponentType.ReadWrite<Updated>());
      componentTypeSet4.Add(ComponentType.ReadWrite<Updated>());
      RouteData componentData = entityManager.GetComponentData<RouteData>(entity) with
      {
        m_RouteArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet1)),
        m_WaypointArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet2)),
        m_ConnectedArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet3)),
        m_SegmentArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet4))
      };
      entityManager.SetComponentData<RouteData>(entity, componentData);
    }
  }
}
