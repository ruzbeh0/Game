// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyNetSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ApplyNetSystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_TempQuery;
    private EntityQuery m_EconomyParameterQuery;
    private ComponentTypeSet m_ApplyCreatedTypes;
    private ComponentTypeSet m_ApplyUpdatedTypes;
    private ComponentTypeSet m_ApplyDeletedTypes;
    private ApplyNetSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Lane>(),
          ComponentType.ReadOnly<Aggregate>()
        },
        None = new ComponentType[0]
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyCreatedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyUpdatedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyDeletedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ApplyNetSystem.PatchTempReferencesJob jobData1 = new ApplyNetSystem.PatchTempReferencesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RW_ComponentTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RW_ComponentLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RW_BufferLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup,
        m_Nodes = this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ApplyNetSystem.FixConnectedEdgesJob jobData2 = new ApplyNetSystem.FixConnectedEdgesJob()
      {
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Marker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Standalone_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Aggregated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrafficLights_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Pollution_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrainTrack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ApplyNetSystem.HandleTempEntitiesJob jobData3 = new ApplyNetSystem.HandleTempEntitiesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_NetNodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_NetEdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_NetLaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
        m_NetEdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_LandValueData = this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
        m_LocalConnectData = this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentLookup,
        m_TramTrackData = this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentLookup,
        m_TrainTrackData = this.__TypeHandle.__Game_Net_TrainTrack_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_PollutionData = this.__TypeHandle.__Game_Net_Pollution_RO_ComponentLookup,
        m_TrafficLightsData = this.__TypeHandle.__Game_Net_TrafficLights_RO_ComponentLookup,
        m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
        m_AggregatedData = this.__TypeHandle.__Game_Net_Aggregated_RO_ComponentLookup,
        m_StandaloneData = this.__TypeHandle.__Game_Net_Standalone_RO_ComponentLookup,
        m_MarkerData = this.__TypeHandle.__Game_Net_Marker_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_RecentData = this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_SubReplacements = this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_EconomyParameterData = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_ApplyCreatedTypes = this.m_ApplyCreatedTypes,
        m_ApplyUpdatedTypes = this.m_ApplyUpdatedTypes,
        m_ApplyDeletedTypes = this.m_ApplyDeletedTypes,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn1 = jobData1.Schedule<ApplyNetSystem.PatchTempReferencesJob>(this.m_TempQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      JobHandle job0 = jobData2.ScheduleParallel<ApplyNetSystem.FixConnectedEdgesJob>(this.m_TempQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      EntityQuery tempQuery = this.m_TempQuery;
      JobHandle dependsOn2 = dependsOn1;
      JobHandle jobHandle = jobData3.ScheduleParallel<ApplyNetSystem.HandleTempEntitiesJob>(tempQuery, dependsOn2);
      this.Dependency = JobHandle.CombineDependencies(job0, jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public ApplyNetSystem()
    {
    }

    [BurstCompile]
    private struct PatchTempReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      public ComponentLookup<Owner> m_OwnerData;
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      public BufferLookup<ConnectedEdge> m_Edges;
      public BufferLookup<ConnectedNode> m_Nodes;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Edge> nativeArray3 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Node> nativeArray4 = chunk.GetNativeArray<Game.Net.Node>(ref this.m_NodeType);
        for (int index1 = 0; index1 < nativeArray3.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Edge edge1 = nativeArray3[index1];
          Temp temp1 = nativeArray2[index1];
          if ((temp1.m_Flags & TempFlags.Delete) == (TempFlags) 0)
          {
            if (temp1.m_Original != Entity.Null && (temp1.m_Flags & (TempFlags.Replace | TempFlags.Combine)) == (TempFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              Temp temp2 = this.m_TempData[edge1.m_Start];
              if (temp2.m_Original != Entity.Null && (temp2.m_Flags & (TempFlags.Delete | TempFlags.Replace)) == (TempFlags) 0)
                edge1.m_Start = temp2.m_Original;
              // ISSUE: reference to a compiler-generated field
              Temp temp3 = this.m_TempData[edge1.m_End];
              if (temp3.m_Original != Entity.Null && (temp3.m_Flags & (TempFlags.Delete | TempFlags.Replace)) == (TempFlags) 0)
                edge1.m_End = temp3.m_Original;
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedNode> node1 = this.m_Nodes[temp1.m_Original];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedNode> node2 = this.m_Nodes[entity];
              for (int index2 = 0; index2 < node1.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<ConnectedEdge>(this.m_Edges[node1[index2].m_Node], new ConnectedEdge(temp1.m_Original));
              }
              node1.Clear();
              for (int index3 = 0; index3 < node2.Length; ++index3)
              {
                ConnectedNode connectedNode = node2[index3];
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.HasComponent(connectedNode.m_Node))
                {
                  // ISSUE: reference to a compiler-generated field
                  Temp temp4 = this.m_TempData[connectedNode.m_Node];
                  if (temp4.m_Original != Entity.Null && (temp4.m_Flags & (TempFlags.Delete | TempFlags.Replace)) == (TempFlags) 0)
                    connectedNode.m_Node = temp4.m_Original;
                }
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ConnectedEdge> edge2 = this.m_Edges[connectedNode.m_Node];
                CollectionUtils.TryAddUniqueValue<ConnectedNode>(node1, connectedNode);
                ConnectedEdge connectedEdge = new ConnectedEdge(temp1.m_Original);
                CollectionUtils.TryAddUniqueValue<ConnectedEdge>(edge2, connectedEdge);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Temp temp5 = this.m_TempData[edge1.m_Start];
              if (temp5.m_Original != Entity.Null && (temp5.m_Flags & (TempFlags.Delete | TempFlags.Replace)) == (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<ConnectedEdge>(this.m_Edges[edge1.m_Start], new ConnectedEdge(entity));
                edge1.m_Start = temp5.m_Original;
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<ConnectedEdge>(this.m_Edges[edge1.m_Start], new ConnectedEdge(entity));
              }
              // ISSUE: reference to a compiler-generated field
              Temp temp6 = this.m_TempData[edge1.m_End];
              if (temp6.m_Original != Entity.Null && (temp6.m_Flags & (TempFlags.Delete | TempFlags.Replace)) == (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<ConnectedEdge>(this.m_Edges[edge1.m_End], new ConnectedEdge(entity));
                edge1.m_End = temp6.m_Original;
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<ConnectedEdge>(this.m_Edges[edge1.m_End], new ConnectedEdge(entity));
              }
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedNode> node = this.m_Nodes[entity];
              for (int index4 = 0; index4 < node.Length; ++index4)
              {
                ConnectedNode connectedNode = node[index4];
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.HasComponent(connectedNode.m_Node))
                {
                  // ISSUE: reference to a compiler-generated field
                  Temp temp7 = this.m_TempData[connectedNode.m_Node];
                  if (temp7.m_Original != Entity.Null && (temp7.m_Flags & (TempFlags.Delete | TempFlags.Replace)) == (TempFlags) 0)
                  {
                    connectedNode.m_Node = temp7.m_Original;
                    node[index4] = connectedNode;
                  }
                }
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<ConnectedEdge>(this.m_Edges[connectedNode.m_Node], new ConnectedEdge(entity));
              }
            }
            nativeArray3[index1] = edge1;
          }
        }
        if (nativeArray4.Length == 0 && nativeArray3.Length == 0)
          return;
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity original = nativeArray1[index];
          Temp temp8 = nativeArray2[index];
          Entity entity1 = Entity.Null;
          Entity entity2 = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(original))
          {
            // ISSUE: reference to a compiler-generated field
            entity2 = this.m_OwnerData[original].m_Owner;
            entity1 = entity2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(entity2))
            {
              // ISSUE: reference to a compiler-generated field
              Temp temp9 = this.m_TempData[entity2];
              if (temp9.m_Original != Entity.Null && (temp9.m_Flags & (TempFlags.Replace | TempFlags.Combine)) == (TempFlags) 0)
              {
                entity2 = temp9.m_Original;
                // ISSUE: reference to a compiler-generated field
                this.m_OwnerData[original] = new Owner(entity2);
              }
            }
          }
          if (temp8.m_Original != Entity.Null && (temp8.m_Flags & (TempFlags.Delete | TempFlags.Replace)) == (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (entity1 != entity2 && this.m_SubNets.HasBuffer(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.RemoveValue<Game.Net.SubNet>(this.m_SubNets[entity1], new Game.Net.SubNet(original));
            }
            original = temp8.m_Original;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            entity1 = !this.m_OwnerData.HasComponent(original) ? Entity.Null : this.m_OwnerData[original].m_Owner;
          }
          if (entity1 != entity2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubNets.HasBuffer(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.RemoveValue<Game.Net.SubNet>(this.m_SubNets[entity1], new Game.Net.SubNet(original));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubNets.HasBuffer(entity2))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<Game.Net.SubNet>(this.m_SubNets[entity2], new Game.Net.SubNet(original));
            }
          }
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct FixConnectedEdgesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_Nodes;
      [NativeDisableParallelForRestriction]
      public BufferLookup<ConnectedEdge> m_Edges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<Game.Net.Node>(ref this.m_NodeType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Temp temp = nativeArray[index1];
          // ISSUE: reference to a compiler-generated field
          if (temp.m_Original != Entity.Null && (temp.m_Flags & (TempFlags.Delete | TempFlags.Replace)) == (TempFlags) 0 && this.m_Edges.HasBuffer(temp.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[temp.m_Original];
label_13:
            for (int index2 = edge1.Length - 1; index2 >= 0; --index2)
            {
              Entity edge2 = edge1[index2].m_Edge;
              // ISSUE: reference to a compiler-generated field
              Edge edge3 = this.m_EdgeData[edge2];
              if (!(edge3.m_Start == temp.m_Original) && !(edge3.m_End == temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_Nodes.HasBuffer(edge2))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<ConnectedNode> node = this.m_Nodes[edge2];
                  for (int index3 = 0; index3 < node.Length; ++index3)
                  {
                    if (node[index3].m_Node == temp.m_Original)
                      goto label_13;
                  }
                }
                edge1.RemoveAt(index2);
              }
            }
          }
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct HandleTempEntitiesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NetNodeType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_NetEdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_NetLaneType;
      [ReadOnly]
      public ComponentLookup<Edge> m_NetEdgeData;
      [ReadOnly]
      public ComponentLookup<LandValue> m_LandValueData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<LocalConnect> m_LocalConnectData;
      [ReadOnly]
      public ComponentLookup<TramTrack> m_TramTrackData;
      [ReadOnly]
      public ComponentLookup<TrainTrack> m_TrainTrackData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Pollution> m_PollutionData;
      [ReadOnly]
      public ComponentLookup<TrafficLights> m_TrafficLightsData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<Aggregated> m_AggregatedData;
      [ReadOnly]
      public ComponentLookup<Standalone> m_StandaloneData;
      [ReadOnly]
      public ComponentLookup<Marker> m_MarkerData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Recent> m_RecentData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public BufferLookup<SubReplacement> m_SubReplacements;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameterData;
      [ReadOnly]
      public ComponentTypeSet m_ApplyCreatedTypes;
      [ReadOnly]
      public ComponentTypeSet m_ApplyUpdatedTypes;
      [ReadOnly]
      public ComponentTypeSet m_ApplyDeletedTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Node> nativeArray3 = chunk.GetNativeArray<Game.Net.Node>(ref this.m_NetNodeType);
        if (nativeArray3.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Temp temp = nativeArray2[index];
            if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Delete(unfilteredChunkIndex, entity, temp);
            }
            else if ((temp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            {
              if (temp.m_Original != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, temp.m_Original, new Deleted());
              }
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity, temp);
            }
            else if (temp.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Game.Net.Node>(unfilteredChunkIndex, temp.m_Original, nativeArray3[index]);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<Owner>(unfilteredChunkIndex, entity, temp.m_Original, this.m_OwnerData, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<PrefabRef>(unfilteredChunkIndex, entity, temp.m_Original, this.m_PrefabRefData, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<Upgraded>(unfilteredChunkIndex, entity, temp.m_Original, this.m_UpgradedData, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<LocalConnect>(unfilteredChunkIndex, entity, temp.m_Original, this.m_LocalConnectData, false);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<TrafficLights>(unfilteredChunkIndex, entity, temp.m_Original, this.m_TrafficLightsData, false);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<Orphan>(unfilteredChunkIndex, entity, temp.m_Original, this.m_OrphanData, false);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<EditorContainer>(unfilteredChunkIndex, entity, temp.m_Original, this.m_EditorContainerData, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<Native>(unfilteredChunkIndex, entity, temp.m_Original, this.m_NativeData, false);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<Standalone>(unfilteredChunkIndex, entity, temp.m_Original, this.m_StandaloneData, false);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabData.IsComponentEnabled(this.m_PrefabRefData[entity].m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<LandValue>(unfilteredChunkIndex, entity, temp.m_Original, this.m_LandValueData, false);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<TramTrack>(unfilteredChunkIndex, entity, temp.m_Original, this.m_TramTrackData, false);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<TrainTrack>(unfilteredChunkIndex, entity, temp.m_Original, this.m_TrainTrackData, false);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent(unfilteredChunkIndex, entity, temp.m_Original, this.m_RoadData);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<Game.Net.Pollution>(unfilteredChunkIndex, entity, temp.m_Original, this.m_PollutionData, false);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<Marker>(unfilteredChunkIndex, entity, temp.m_Original, this.m_MarkerData, false);
              }
              // ISSUE: reference to a compiler-generated method
              this.Update(unfilteredChunkIndex, entity, temp);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity, temp);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.GetNativeArray<Edge>(ref this.m_NetEdgeType).Length != 0)
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              Temp temp = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_AggregatedData.HasComponent(entity) && this.m_TempData.HasComponent(this.m_AggregatedData[entity].m_Aggregate))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Aggregated>(unfilteredChunkIndex, entity, new Aggregated());
              }
              if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.Delete(unfilteredChunkIndex, entity, temp);
              }
              else if ((temp.m_Flags & (TempFlags.Replace | TempFlags.Combine)) != (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, temp.m_Original, new Deleted());
                // ISSUE: reference to a compiler-generated method
                this.Create(unfilteredChunkIndex, entity, temp);
              }
              else if (temp.m_Original != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<PrefabRef>(unfilteredChunkIndex, entity, temp.m_Original, this.m_PrefabRefData, true);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<Upgraded>(unfilteredChunkIndex, entity, temp.m_Original, this.m_UpgradedData, true);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateBuffer<SubReplacement>(unfilteredChunkIndex, entity, temp.m_Original, this.m_SubReplacements, out DynamicBuffer<SubReplacement> _, true);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<Curve>(unfilteredChunkIndex, entity, temp.m_Original, this.m_CurveData, true);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<Edge>(unfilteredChunkIndex, entity, temp.m_Original, this.m_NetEdgeData, true);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<EditorContainer>(unfilteredChunkIndex, entity, temp.m_Original, this.m_EditorContainerData, true);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<Native>(unfilteredChunkIndex, entity, temp.m_Original, this.m_NativeData, false);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabData.IsComponentEnabled(this.m_PrefabRefData[entity].m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateComponent(unfilteredChunkIndex, entity, temp.m_Original, this.m_RoadData);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateComponent<Game.Net.Pollution>(unfilteredChunkIndex, entity, temp.m_Original, this.m_PollutionData, false);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateComponent<TramTrack>(unfilteredChunkIndex, entity, temp.m_Original, this.m_TramTrackData, false);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateComponent<LandValue>(unfilteredChunkIndex, entity, temp.m_Original, this.m_LandValueData, false);
                }
                // ISSUE: reference to a compiler-generated method
                this.Update(unfilteredChunkIndex, entity, temp);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.Create(unfilteredChunkIndex, entity, temp);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.GetNativeArray<Lane>(ref this.m_NetLaneType).Length != 0)
            {
              for (int index = 0; index < nativeArray1.Length; ++index)
              {
                Entity entity = nativeArray1[index];
                Temp temp1 = nativeArray2[index];
                if ((temp1.m_Flags & TempFlags.Delete) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Delete(unfilteredChunkIndex, entity, temp1);
                }
                else if ((temp1.m_Flags & TempFlags.Replace) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, temp1.m_Original, new Deleted());
                  // ISSUE: reference to a compiler-generated method
                  this.Create(unfilteredChunkIndex, entity, temp1);
                }
                else if (temp1.m_Original != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.HasComponent(entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Owner owner = this.m_OwnerData[entity];
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TempData.HasComponent(owner.m_Owner))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Temp temp2 = this.m_TempData[owner.m_Owner];
                      if (temp2.m_Original != Entity.Null && (temp2.m_Flags & (TempFlags.Replace | TempFlags.Combine)) != (TempFlags) 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, temp1.m_Original, new Deleted());
                        // ISSUE: reference to a compiler-generated method
                        this.Create(unfilteredChunkIndex, entity, temp1);
                        continue;
                      }
                    }
                  }
                  // ISSUE: reference to a compiler-generated method
                  this.Update(unfilteredChunkIndex, entity, temp1);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.HasComponent(entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Owner owner = this.m_OwnerData[entity];
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TempData.HasComponent(owner.m_Owner))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Temp temp3 = this.m_TempData[owner.m_Owner];
                      if (temp3.m_Original != Entity.Null && (temp3.m_Flags & (TempFlags.Replace | TempFlags.Combine)) == (TempFlags) 0)
                      {
                        if ((temp3.m_Flags & TempFlags.Upgrade) != (TempFlags) 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          this.m_CommandBuffer.AddComponent(unfilteredChunkIndex, entity, in this.m_ApplyDeletedTypes);
                          continue;
                        }
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity, new Deleted());
                        continue;
                      }
                    }
                  }
                  // ISSUE: reference to a compiler-generated method
                  this.Create(unfilteredChunkIndex, entity, temp1);
                }
              }
            }
            else
            {
              for (int index = 0; index < nativeArray1.Length; ++index)
              {
                Entity e = nativeArray1[index];
                Temp temp = nativeArray2[index];
                // ISSUE: reference to a compiler-generated field
                if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0 && temp.m_Original != Entity.Null && this.m_HiddenData.HasComponent(temp.m_Original))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Hidden>(unfilteredChunkIndex, temp.m_Original);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, e, new Deleted());
              }
            }
          }
        }
      }

      private void Delete(int chunkIndex, Entity entity, Temp temp)
      {
        if (temp.m_Original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, temp.m_Original, new Deleted());
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void UpdateComponent<T>(
        int chunkIndex,
        Entity entity,
        Entity original,
        ComponentLookup<T> data,
        bool updateValue)
        where T : unmanaged, IComponentData
      {
        if (data.HasComponent(entity))
        {
          if (data.HasComponent(original))
          {
            if (!updateValue)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<T>(chunkIndex, original, data[entity]);
          }
          else if (updateValue)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<T>(chunkIndex, original, data[entity]);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<T>(chunkIndex, original, default (T));
          }
        }
        else
        {
          if (!data.HasComponent(original))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<T>(chunkIndex, original);
        }
      }

      private bool UpdateBuffer<T>(
        int chunkIndex,
        Entity entity,
        Entity original,
        BufferLookup<T> data,
        out DynamicBuffer<T> oldBuffer,
        bool updateValue)
        where T : unmanaged, IBufferElementData
      {
        if (data.HasBuffer(entity))
        {
          if (data.HasBuffer(original))
          {
            if (updateValue)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetBuffer<T>(chunkIndex, original).CopyFrom(data[entity]);
            }
          }
          else if (updateValue)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<T>(chunkIndex, original).CopyFrom(data[entity]);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<T>(chunkIndex, original);
          }
        }
        else if (data.TryGetBuffer(original, out oldBuffer))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<T>(chunkIndex, original);
          return true;
        }
        oldBuffer = new DynamicBuffer<T>();
        return false;
      }

      private void UpdateComponent(
        int chunkIndex,
        Entity entity,
        Entity original,
        ComponentLookup<Road> data)
      {
        if (data.HasComponent(entity))
        {
          if (data.HasComponent(original))
          {
            Road component = data[original];
            Road road = data[entity];
            component.m_Flags = road.m_Flags;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Road>(chunkIndex, original, component);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Road>(chunkIndex, original, data[entity]);
          }
        }
        else
        {
          if (!data.HasComponent(original))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Road>(chunkIndex, original);
        }
      }

      private void Update(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HiddenData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(chunkIndex, temp.m_Original);
        }
        if (temp.m_Cost != 0)
        {
          // ISSUE: reference to a compiler-generated field
          Recent component = new Recent()
          {
            m_ModificationFrame = this.m_SimulationFrame,
            m_ModificationCost = temp.m_Cost
          };
          Recent componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RecentData.TryGetComponent(temp.m_Original, out componentData))
          {
            component.m_ModificationCost += componentData.m_ModificationCost;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component.m_ModificationCost += NetUtils.GetRefundAmount(componentData, this.m_SimulationFrame, this.m_EconomyParameterData);
            // ISSUE: reference to a compiler-generated field
            componentData.m_ModificationFrame = this.m_SimulationFrame;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component.m_ModificationCost -= NetUtils.GetRefundAmount(componentData, this.m_SimulationFrame, this.m_EconomyParameterData);
            component.m_ModificationCost = math.min(component.m_ModificationCost, temp.m_Value);
            if (component.m_ModificationCost > 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Recent>(chunkIndex, temp.m_Original, component);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Recent>(chunkIndex, temp.m_Original);
            }
          }
          else if (component.m_ModificationCost > 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Recent>(chunkIndex, temp.m_Original, component);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(chunkIndex, temp.m_Original, in this.m_ApplyUpdatedTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Create(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Temp>(chunkIndex, entity);
        if (temp.m_Cost > 0)
        {
          // ISSUE: reference to a compiler-generated field
          Recent component = new Recent()
          {
            m_ModificationFrame = this.m_SimulationFrame,
            m_ModificationCost = temp.m_Cost
          };
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Recent>(chunkIndex, entity, component);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(chunkIndex, entity, in this.m_ApplyCreatedTypes);
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> __Game_Net_Node_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      public ComponentLookup<Owner> __Game_Common_Owner_RW_ComponentLookup;
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RW_BufferLookup;
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RW_BufferLookup;
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<LandValue> __Game_Net_LandValue_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnect> __Game_Net_LocalConnect_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TramTrack> __Game_Net_TramTrack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainTrack> __Game_Net_TrainTrack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Pollution> __Game_Net_Pollution_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficLights> __Game_Net_TrafficLights_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Aggregated> __Game_Net_Aggregated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Standalone> __Game_Net_Standalone_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Marker> __Game_Net_Marker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Recent> __Game_Tools_Recent_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubReplacement> __Game_Net_SubReplacement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RW_ComponentLookup = state.GetComponentLookup<Owner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RW_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RW_BufferLookup = state.GetBufferLookup<ConnectedEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RW_BufferLookup = state.GetBufferLookup<ConnectedNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LandValue_RO_ComponentLookup = state.GetComponentLookup<LandValue>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentLookup = state.GetComponentLookup<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LocalConnect_RO_ComponentLookup = state.GetComponentLookup<LocalConnect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TramTrack_RO_ComponentLookup = state.GetComponentLookup<TramTrack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrainTrack_RO_ComponentLookup = state.GetComponentLookup<TrainTrack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Pollution_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Pollution>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrafficLights_RO_ComponentLookup = state.GetComponentLookup<TrafficLights>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Aggregated_RO_ComponentLookup = state.GetComponentLookup<Aggregated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Standalone_RO_ComponentLookup = state.GetComponentLookup<Standalone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Marker_RO_ComponentLookup = state.GetComponentLookup<Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Recent_RO_ComponentLookup = state.GetComponentLookup<Recent>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubReplacement_RO_BufferLookup = state.GetBufferLookup<SubReplacement>(true);
      }
    }
  }
}
