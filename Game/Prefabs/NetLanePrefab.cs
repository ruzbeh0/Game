// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetLanePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Lane/", new System.Type[] {})]
  public class NetLanePrefab : PrefabBase
  {
    public PathfindPrefab m_PathfindPrefab;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_PathfindPrefab != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_PathfindPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<NetLaneData>());
      components.Add(ComponentType.ReadWrite<NetLaneArchetypeData>());
      if (this.prefab.Has<SecondaryLane>())
        return;
      components.Add(ComponentType.ReadWrite<SecondaryNetLane>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Curve>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      List<ComponentBase> componentBaseList = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(componentBaseList);
      HashSet<ComponentType> laneComponents = new HashSet<ComponentType>();
      laneComponents.Add(ComponentType.ReadWrite<Lane>());
      NetLaneArchetypeData componentData;
      componentData.m_LaneArchetype = this.CreateArchetype(entityManager, componentBaseList, laneComponents);
      laneComponents.Add(ComponentType.ReadWrite<Lane>());
      laneComponents.Add(ComponentType.ReadWrite<AreaLane>());
      componentData.m_AreaLaneArchetype = this.CreateArchetype(entityManager, componentBaseList, laneComponents);
      laneComponents.Add(ComponentType.ReadWrite<Lane>());
      laneComponents.Add(ComponentType.ReadWrite<EdgeLane>());
      componentData.m_EdgeLaneArchetype = this.CreateArchetype(entityManager, componentBaseList, laneComponents);
      laneComponents.Add(ComponentType.ReadWrite<Lane>());
      laneComponents.Add(ComponentType.ReadWrite<NodeLane>());
      componentData.m_NodeLaneArchetype = this.CreateArchetype(entityManager, componentBaseList, laneComponents);
      laneComponents.Add(ComponentType.ReadWrite<Lane>());
      laneComponents.Add(ComponentType.ReadWrite<SlaveLane>());
      laneComponents.Add(ComponentType.ReadWrite<EdgeLane>());
      componentData.m_EdgeSlaveArchetype = this.CreateArchetype(entityManager, componentBaseList, laneComponents);
      laneComponents.Add(ComponentType.ReadWrite<Lane>());
      laneComponents.Add(ComponentType.ReadWrite<SlaveLane>());
      laneComponents.Add(ComponentType.ReadWrite<NodeLane>());
      componentData.m_NodeSlaveArchetype = this.CreateArchetype(entityManager, componentBaseList, laneComponents);
      laneComponents.Add(ComponentType.ReadWrite<Lane>());
      laneComponents.Add(ComponentType.ReadWrite<MasterLane>());
      laneComponents.Add(ComponentType.ReadWrite<EdgeLane>());
      componentData.m_EdgeMasterArchetype = this.CreateArchetype(entityManager, componentBaseList, laneComponents);
      laneComponents.Add(ComponentType.ReadWrite<Lane>());
      laneComponents.Add(ComponentType.ReadWrite<MasterLane>());
      laneComponents.Add(ComponentType.ReadWrite<NodeLane>());
      componentData.m_NodeMasterArchetype = this.CreateArchetype(entityManager, componentBaseList, laneComponents);
      entityManager.SetComponentData<NetLaneArchetypeData>(entity, componentData);
    }

    private EntityArchetype CreateArchetype(
      EntityManager entityManager,
      List<ComponentBase> unityComponents,
      HashSet<ComponentType> laneComponents)
    {
      for (int index = 0; index < unityComponents.Count; ++index)
        unityComponents[index].GetArchetypeComponents(laneComponents);
      laneComponents.Add(ComponentType.ReadWrite<Created>());
      laneComponents.Add(ComponentType.ReadWrite<Updated>());
      EntityArchetype archetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(laneComponents));
      laneComponents.Clear();
      return archetype;
    }
  }
}
