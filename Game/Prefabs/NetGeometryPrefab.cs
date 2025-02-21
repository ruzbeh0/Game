// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetGeometryPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Rendering;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Prefab/", new System.Type[] {})]
  public class NetGeometryPrefab : NetPrefab
  {
    public NetSectionInfo[] m_Sections;
    public NetEdgeStateInfo[] m_EdgeStates;
    public NetNodeStateInfo[] m_NodeStates;
    public float m_MaxSlopeSteepness = 0.2f;
    public AggregateNetPrefab m_AggregateType;
    public CompositionInvertMode m_InvertMode;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Sections.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Sections[index].m_Section);
      if (!((UnityEngine.Object) this.m_AggregateType != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_AggregateType);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<NetGeometryData>());
      components.Add(ComponentType.ReadWrite<NetGeometryComposition>());
      components.Add(ComponentType.ReadWrite<NetGeometrySection>());
      components.Add(ComponentType.ReadWrite<NetGeometryEdgeState>());
      components.Add(ComponentType.ReadWrite<NetGeometryNodeState>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<Game.Net.Node>()))
      {
        components.Add(ComponentType.ReadWrite<Game.Net.SubLane>());
        components.Add(ComponentType.ReadWrite<Game.Objects.SubObject>());
        components.Add(ComponentType.ReadWrite<NodeGeometry>());
        components.Add(ComponentType.ReadWrite<CullingInfo>());
        components.Add(ComponentType.ReadWrite<MeshBatch>());
        components.Add(ComponentType.ReadWrite<PseudoRandomSeed>());
      }
      else if (components.Contains(ComponentType.ReadWrite<Game.Net.Edge>()))
      {
        if ((UnityEngine.Object) this.m_AggregateType != (UnityEngine.Object) null)
          components.Add(ComponentType.ReadWrite<Aggregated>());
        components.Add(ComponentType.ReadWrite<Game.Net.SubLane>());
        components.Add(ComponentType.ReadWrite<Game.Objects.SubObject>());
        components.Add(ComponentType.ReadWrite<Curve>());
        components.Add(ComponentType.ReadWrite<Composition>());
        components.Add(ComponentType.ReadWrite<EdgeGeometry>());
        components.Add(ComponentType.ReadWrite<StartNodeGeometry>());
        components.Add(ComponentType.ReadWrite<EndNodeGeometry>());
        components.Add(ComponentType.ReadWrite<BuildOrder>());
        components.Add(ComponentType.ReadWrite<CullingInfo>());
        components.Add(ComponentType.ReadWrite<MeshBatch>());
        components.Add(ComponentType.ReadWrite<PseudoRandomSeed>());
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<NetCompositionData>()))
          return;
        components.Add(ComponentType.ReadWrite<NetCompositionPiece>());
        components.Add(ComponentType.ReadWrite<NetCompositionMeshRef>());
        components.Add(ComponentType.ReadWrite<NetCompositionArea>());
        components.Add(ComponentType.ReadWrite<NetCompositionObject>());
        components.Add(ComponentType.ReadWrite<NetCompositionCarriageway>());
      }
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      NetGeometryData componentData = entityManager.GetComponentData<NetGeometryData>(entity);
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
      componentTypeSet.Add(ComponentType.ReadWrite<NetCompositionData>());
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      componentTypeSet.Add(ComponentType.ReadWrite<NetCompositionCrosswalk>());
      componentData.m_NodeCompositionArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      componentTypeSet.Remove(ComponentType.ReadWrite<NetCompositionCrosswalk>());
      componentTypeSet.Add(ComponentType.ReadWrite<NetCompositionLane>());
      componentData.m_EdgeCompositionArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      entityManager.SetComponentData<NetGeometryData>(entity, componentData);
    }
  }
}
