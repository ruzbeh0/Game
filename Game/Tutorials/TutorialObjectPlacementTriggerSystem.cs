// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialObjectPlacementTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialObjectPlacementTriggerSystem : TutorialTriggerSystemBase
  {
    private NetToolSystem m_NetToolSystem;
    private ToolSystem m_ToolSystem;
    private EntityQuery m_CreatedObjectQuery;
    private EntityQuery m_ObjectQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private TutorialObjectPlacementTriggerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ActiveTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<ObjectPlacementTriggerData>(), ComponentType.ReadOnly<TriggerActive>(), ComponentType.ReadWrite<ObjectPlacementTriggerCountData>(), ComponentType.Exclude<TriggerCompleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Created>()
        },
        Any = new ComponentType[9]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Game.Net.WaterPipeConnection>(),
          ComponentType.ReadOnly<Game.Net.ElectricityConnection>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Game.Routes.TransportStop>(),
          ComponentType.ReadOnly<Route>(),
          ComponentType.ReadOnly<Tree>(),
          ComponentType.ReadOnly<Waterway>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Native>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[9]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Game.Net.WaterPipeConnection>(),
          ComponentType.ReadOnly<Game.Net.ElectricityConnection>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Game.Routes.TransportStop>(),
          ComponentType.ReadOnly<Route>(),
          ComponentType.ReadOnly<Tree>(),
          ComponentType.ReadOnly<Waterway>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Native>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetToolSystem = this.World.GetOrCreateSystemManaged<NetToolSystem>();
      this.RequireForUpdate(this.m_ActiveTriggerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.triggersChanged)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerCountData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TutorialObjectPlacementTriggerSystem.ClearCountJob jobData1 = new TutorialObjectPlacementTriggerSystem.ClearCountJob()
        {
          m_CountType = this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerCountData_RW_ComponentTypeHandle
        };
        this.Dependency = jobData1.ScheduleParallel<TutorialObjectPlacementTriggerSystem.ClearCountJob>(this.m_ActiveTriggerQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerCountData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_WaterPipeConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_WaterPipeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle;
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TutorialObjectPlacementTriggerSystem.CheckObjectsJob jobData2 = new TutorialObjectPlacementTriggerSystem.CheckObjectsJob()
        {
          m_CreatedObjectChunks = this.m_ObjectQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_Natives = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
          m_ElectricityProducers = this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentLookup,
          m_ElectricityConnections = this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentLookup,
          m_ElectricityConnectionType = this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentTypeHandle,
          m_WaterPipeConnections = this.__TypeHandle.__Game_Net_WaterPipeConnection_RO_ComponentLookup,
          m_WaterPipeConnectionType = this.__TypeHandle.__Game_Net_WaterPipeConnection_RO_ComponentTypeHandle,
          m_SewageOutlets = this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentLookup,
          m_Transformers = this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentLookup,
          m_TriggerType = this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerData_RO_BufferTypeHandle,
          m_CountType = this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerCountData_RW_ComponentTypeHandle,
          m_CommandBuffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter(),
          m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
          m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_ForcedUnlockDataFromEntity = this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup,
          m_UnlockRequirementFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup,
          m_Roads = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
          m_Edges = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_Owners = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_UnlockEventArchetype = this.m_UnlockEventArchetype,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_HasElevation = this.HasElevation(),
          m_FirstTimeCheck = true
        };
        this.Dependency = jobData2.ScheduleParallel<TutorialObjectPlacementTriggerSystem.CheckObjectsJob>(this.m_ActiveTriggerQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        // ISSUE: reference to a compiler-generated field
        jobData2.m_CreatedObjectChunks.Dispose(this.Dependency);
        this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CreatedObjectQuery.IsEmptyIgnoreFilter)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerCountData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerData_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_WaterPipeConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_WaterPipeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle;
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TutorialObjectPlacementTriggerSystem.CheckObjectsJob jobData = new TutorialObjectPlacementTriggerSystem.CheckObjectsJob()
        {
          m_CreatedObjectChunks = this.m_CreatedObjectQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
          m_Natives = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
          m_ElectricityProducers = this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentLookup,
          m_ElectricityConnections = this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentLookup,
          m_ElectricityConnectionType = this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentTypeHandle,
          m_WaterPipeConnections = this.__TypeHandle.__Game_Net_WaterPipeConnection_RO_ComponentLookup,
          m_WaterPipeConnectionType = this.__TypeHandle.__Game_Net_WaterPipeConnection_RO_ComponentTypeHandle,
          m_SewageOutlets = this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentLookup,
          m_Transformers = this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentLookup,
          m_TriggerType = this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerData_RO_BufferTypeHandle,
          m_CountType = this.__TypeHandle.__Game_Tutorials_ObjectPlacementTriggerCountData_RW_ComponentTypeHandle,
          m_CommandBuffer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter(),
          m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
          m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_ForcedUnlockDataFromEntity = this.__TypeHandle.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup,
          m_UnlockRequirementFromEntity = this.__TypeHandle.__Game_Prefabs_UnlockRequirement_RO_BufferLookup,
          m_Roads = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
          m_Edges = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_Owners = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_UnlockEventArchetype = this.m_UnlockEventArchetype,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_HasElevation = this.HasElevation(),
          m_FirstTimeCheck = false
        };
        this.Dependency = jobData.ScheduleParallel<TutorialObjectPlacementTriggerSystem.CheckObjectsJob>(this.m_ActiveTriggerQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        // ISSUE: reference to a compiler-generated field
        jobData.m_CreatedObjectChunks.Dispose(this.Dependency);
        this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
      }
    }

    private bool HasElevation()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_ToolSystem.activeTool == this.m_NetToolSystem && (double) math.abs(this.m_NetToolSystem.elevation) > 9.9999997473787516E-05;
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
    public TutorialObjectPlacementTriggerSystem()
    {
    }

    private struct ClearCountJob : IJobChunk
    {
      public ComponentTypeHandle<ObjectPlacementTriggerCountData> m_CountType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ObjectPlacementTriggerCountData> nativeArray = chunk.GetNativeArray<ObjectPlacementTriggerCountData>(ref this.m_CountType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          ObjectPlacementTriggerCountData triggerCountData = nativeArray[index] with
          {
            m_Count = 0
          };
          nativeArray[index] = triggerCountData;
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
    private struct CheckObjectsJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_CreatedObjectChunks;
      [ReadOnly]
      public BufferLookup<ForceUIGroupUnlockData> m_ForcedUnlockDataFromEntity;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      [ReadOnly]
      public BufferLookup<UnlockRequirement> m_UnlockRequirementFromEntity;
      [ReadOnly]
      public BufferTypeHandle<ObjectPlacementTriggerData> m_TriggerType;
      [ReadOnly]
      public ComponentLookup<Native> m_Natives;
      [ReadOnly]
      public ComponentLookup<ElectricityProducer> m_ElectricityProducers;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.SewageOutlet> m_SewageOutlets;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Transformer> m_Transformers;
      [ReadOnly]
      public ComponentLookup<Edge> m_Edges;
      [ReadOnly]
      public ComponentLookup<Game.Net.ElectricityConnection> m_ElectricityConnections;
      [ReadOnly]
      public ComponentLookup<Road> m_Roads;
      [ReadOnly]
      public ComponentLookup<Game.Net.WaterPipeConnection> m_WaterPipeConnections;
      [ReadOnly]
      public ComponentLookup<Owner> m_Owners;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ElectricityConnection> m_ElectricityConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.WaterPipeConnection> m_WaterPipeConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public EntityArchetype m_UnlockEventArchetype;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public bool m_HasElevation;
      public ComponentTypeHandle<ObjectPlacementTriggerCountData> m_CountType;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public bool m_FirstTimeCheck;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ObjectPlacementTriggerData> bufferAccessor = chunk.GetBufferAccessor<ObjectPlacementTriggerData>(ref this.m_TriggerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ObjectPlacementTriggerCountData> nativeArray2 = chunk.GetNativeArray<ObjectPlacementTriggerCountData>(ref this.m_CountType);
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.Check(bufferAccessor[index]))
          {
            ObjectPlacementTriggerCountData triggerCountData = nativeArray2[index];
            ++triggerCountData.m_Count;
            if (triggerCountData.m_Count >= triggerCountData.m_RequiredCount)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_FirstTimeCheck)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<TriggerPreCompleted>(unfilteredChunkIndex, nativeArray1[index]);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<TriggerCompleted>(unfilteredChunkIndex, nativeArray1[index]);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              TutorialSystem.ManualUnlock(nativeArray1[index], this.m_UnlockEventArchetype, ref this.m_ForcedUnlockDataFromEntity, ref this.m_UnlockRequirementFromEntity, this.m_CommandBuffer, unfilteredChunkIndex);
            }
            nativeArray2[index] = triggerCountData;
          }
        }
      }

      private bool Check(
        DynamicBuffer<ObjectPlacementTriggerData> triggerDatas)
      {
        for (int index1 = 0; index1 < triggerDatas.Length; ++index1)
        {
          ObjectPlacementTriggerData triggerData = triggerDatas[index1];
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_CreatedObjectChunks.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk createdObjectChunk = this.m_CreatedObjectChunks[index2];
            // ISSUE: reference to a compiler-generated method
            int num = TutorialObjectPlacementTriggerSystem.CheckObjectsJob.FlagsMatch(triggerData, ObjectPlacementTriggerFlags.AllowSubObject) ? 1 : 0;
            // ISSUE: reference to a compiler-generated method
            bool flag1 = TutorialObjectPlacementTriggerSystem.CheckObjectsJob.FlagsMatch(triggerData, ObjectPlacementTriggerFlags.RequireElevation);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((num != 0 || !createdObjectChunk.Has<Owner>(ref this.m_OwnerType)) && (!flag1 || this.m_HasElevation))
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<PrefabRef> nativeArray1 = createdObjectChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
              // ISSUE: reference to a compiler-generated field
              if (createdObjectChunk.Has<Edge>(ref this.m_EdgeType))
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Edge> nativeArray2 = createdObjectChunk.GetNativeArray<Edge>(ref this.m_EdgeType);
                // ISSUE: reference to a compiler-generated method
                bool flag2 = TutorialObjectPlacementTriggerSystem.CheckObjectsJob.FlagsMatch(triggerData, ObjectPlacementTriggerFlags.RequireOutsideConnection);
                bool flag3 = false;
                // ISSUE: reference to a compiler-generated field
                if (createdObjectChunk.Has<Game.Net.WaterPipeConnection>(ref this.m_WaterPipeConnectionType))
                {
                  if (flag2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    flag3 = this.CheckNodes<Game.Net.WaterPipeConnection, Native>(triggerData, this.m_WaterPipeConnections, this.m_Natives, 1, nativeArray2, nativeArray1);
                  }
                  if (!flag2 | flag3)
                  {
                    // ISSUE: reference to a compiler-generated method
                    bool flag4 = TutorialObjectPlacementTriggerSystem.CheckObjectsJob.FlagsMatch(triggerData, ObjectPlacementTriggerFlags.RequireRoadConnection);
                    // ISSUE: reference to a compiler-generated method
                    bool flag5 = TutorialObjectPlacementTriggerSystem.CheckObjectsJob.FlagsMatch(triggerData, ObjectPlacementTriggerFlags.RequireSewageOutletConnection);
                    if (flag4 | flag5)
                    {
                      bool flag6 = false;
                      bool flag7 = false;
                      if (flag4)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        flag6 = this.CheckEdges<Game.Net.WaterPipeConnection, Road>(triggerData, this.m_WaterPipeConnections, this.m_Roads, 1, nativeArray2, nativeArray1);
                      }
                      if (flag5)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        flag7 = this.CheckNodes<Game.Net.WaterPipeConnection, Game.Buildings.SewageOutlet>(triggerData, this.m_WaterPipeConnections, this.m_SewageOutlets, 1, nativeArray2, nativeArray1);
                      }
                      if (!flag4 | flag6 && !flag5 | flag7)
                        return true;
                      continue;
                    }
                    // ISSUE: reference to a compiler-generated method
                    if (this.Check(triggerData, nativeArray1))
                      return true;
                  }
                  else
                    continue;
                }
                // ISSUE: reference to a compiler-generated field
                if (createdObjectChunk.Has<Game.Net.ElectricityConnection>(ref this.m_ElectricityConnectionType))
                {
                  if (flag2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    flag3 = this.CheckEdges<Game.Net.ElectricityConnection, Native>(triggerData, this.m_ElectricityConnections, this.m_Natives, 1, nativeArray2, nativeArray1);
                  }
                  if (!flag2 | flag3)
                  {
                    // ISSUE: reference to a compiler-generated method
                    bool flag8 = TutorialObjectPlacementTriggerSystem.CheckObjectsJob.FlagsMatch(triggerData, ObjectPlacementTriggerFlags.RequireRoadConnection);
                    // ISSUE: reference to a compiler-generated method
                    bool flag9 = TutorialObjectPlacementTriggerSystem.CheckObjectsJob.FlagsMatch(triggerData, ObjectPlacementTriggerFlags.RequireTransformerConnection);
                    // ISSUE: reference to a compiler-generated method
                    bool flag10 = TutorialObjectPlacementTriggerSystem.CheckObjectsJob.FlagsMatch(triggerData, ObjectPlacementTriggerFlags.RequireElectricityProducerConnection);
                    if (flag8 | flag9 | flag10)
                    {
                      bool flag11 = false;
                      bool flag12 = false;
                      bool flag13 = false;
                      if (flag8)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        flag11 = this.CheckEdges<Game.Net.ElectricityConnection, Road>(triggerData, this.m_ElectricityConnections, this.m_Roads, 1, nativeArray2, nativeArray1);
                      }
                      if (flag9)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        flag12 = this.CheckNodes<Game.Net.ElectricityConnection, Game.Buildings.Transformer>(triggerData, this.m_ElectricityConnections, this.m_Transformers, 1, nativeArray2, nativeArray1);
                      }
                      if (flag10)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        flag13 = this.CheckNodes<Game.Net.ElectricityConnection, ElectricityProducer>(triggerData, this.m_ElectricityConnections, this.m_ElectricityProducers, 1, nativeArray2, nativeArray1);
                      }
                      if (!flag8 | flag11 && !flag9 | flag12 && !flag10 | flag13)
                        return true;
                      continue;
                    }
                    // ISSUE: reference to a compiler-generated method
                    if (this.Check(triggerData, nativeArray1))
                      return true;
                  }
                  else
                    continue;
                }
                // ISSUE: reference to a compiler-generated method
                if (this.Check(triggerData, nativeArray1))
                  return true;
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                if (this.Check(triggerData, nativeArray1))
                  return true;
              }
            }
          }
        }
        return false;
      }

      private static bool FlagsMatch(
        ObjectPlacementTriggerData triggerData,
        ObjectPlacementTriggerFlags flags)
      {
        return (triggerData.m_Flags & flags) == flags;
      }

      private bool CheckEdges<T1, T2>(
        ObjectPlacementTriggerData triggerData,
        ComponentLookup<T1> matchData,
        ComponentLookup<T2> searchData,
        int requiredCount,
        NativeArray<Edge> edges,
        NativeArray<PrefabRef> prefabRefs)
        where T1 : unmanaged, IComponentData
        where T2 : unmanaged, IComponentData
      {
        NativeList<Entity> stack = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashMap<Entity, int> onStack = new NativeParallelHashMap<Entity, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < prefabRefs.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (prefabRefs[index].m_Prefab == triggerData.m_Object && this.CheckEdgesImpl<T1, T2>(edges[index].m_Start, matchData, searchData, this.m_Edges, this.m_ConnectedEdges, requiredCount, stack, onStack) >= requiredCount)
            return true;
        }
        onStack.Dispose();
        stack.Dispose();
        return false;
      }

      private int CheckEdgesImpl<T1, T2>(
        Entity node,
        ComponentLookup<T1> matchData,
        ComponentLookup<T2> searchData,
        ComponentLookup<Edge> edgesData,
        BufferLookup<ConnectedEdge> connectedEdgesData,
        int requiredCount,
        NativeList<Entity> stack,
        NativeParallelHashMap<Entity, int> onStack)
        where T1 : unmanaged, IComponentData
        where T2 : unmanaged, IComponentData
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated method
        this.Push(node, stack, onStack);
        while (stack.Length > 0)
        {
          // ISSUE: reference to a compiler-generated method
          Entity entity = this.Pop(stack);
          if (connectedEdgesData.HasBuffer(entity))
          {
            DynamicBuffer<ConnectedEdge> dynamicBuffer = connectedEdgesData[entity];
            for (int index = 0; index < dynamicBuffer.Length; ++index)
            {
              Entity edge1 = dynamicBuffer[index].m_Edge;
              if (searchData.HasComponent(edge1) && onStack[entity] == 1)
                ++num;
              if (num >= requiredCount)
                return num;
              if (edgesData.HasComponent(edge1) && matchData.HasComponent(edge1))
              {
                Edge edge2 = edgesData[edge1];
                if (!onStack.ContainsKey(edge2.m_Start) || onStack[edge2.m_Start] < 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Push(edge2.m_Start, stack, onStack);
                }
                if (!onStack.ContainsKey(edge2.m_End) || onStack[edge2.m_End] < 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Push(edge2.m_End, stack, onStack);
                }
              }
            }
          }
        }
        return num;
      }

      private bool CheckNodes<T1, T2>(
        ObjectPlacementTriggerData triggerData,
        ComponentLookup<T1> matchData,
        ComponentLookup<T2> searchData,
        int requiredCount,
        NativeArray<Edge> edges,
        NativeArray<PrefabRef> prefabRefs)
        where T1 : unmanaged, IComponentData
        where T2 : unmanaged, IComponentData
      {
        NativeList<Entity> stack = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashMap<Entity, int> onStack = new NativeParallelHashMap<Entity, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < prefabRefs.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (prefabRefs[index].m_Prefab == triggerData.m_Object && this.CheckNodesImpl<T1, T2>(edges[index].m_Start, matchData, searchData, this.m_Owners, this.m_Edges, this.m_ConnectedEdges, this.m_ConnectedNodes, requiredCount, stack, onStack) >= requiredCount)
            return true;
        }
        onStack.Dispose();
        stack.Dispose();
        return false;
      }

      private int CheckNodesImpl<T1, T2>(
        Entity node,
        ComponentLookup<T1> matchData,
        ComponentLookup<T2> searchData,
        ComponentLookup<Owner> ownerData,
        ComponentLookup<Edge> edgesData,
        BufferLookup<ConnectedEdge> connectedEdgesData,
        BufferLookup<ConnectedNode> connectedNodesData,
        int requiredCount,
        NativeList<Entity> stack,
        NativeParallelHashMap<Entity, int> onStack)
        where T1 : unmanaged, IComponentData
        where T2 : unmanaged, IComponentData
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated method
        this.Push(node, stack, onStack);
        while (stack.Length > 0)
        {
          // ISSUE: reference to a compiler-generated method
          Entity entity = this.Pop(stack);
          if (searchData.HasComponent(entity) && onStack[entity] == 1)
            ++num;
          else if (ownerData.HasComponent(entity))
          {
            Owner owner = ownerData[entity];
            if (searchData.HasComponent(owner.m_Owner) && onStack[entity] == 1)
              ++num;
          }
          if (num >= requiredCount)
            return num;
          if (connectedEdgesData.HasBuffer(entity))
          {
            DynamicBuffer<ConnectedEdge> dynamicBuffer1 = connectedEdgesData[entity];
            for (int index1 = 0; index1 < dynamicBuffer1.Length; ++index1)
            {
              Entity edge1 = dynamicBuffer1[index1].m_Edge;
              if (edgesData.HasComponent(edge1) && matchData.HasComponent(edge1))
              {
                Edge edge2 = edgesData[edge1];
                if (connectedNodesData.HasBuffer(edge1))
                {
                  DynamicBuffer<ConnectedNode> dynamicBuffer2 = connectedNodesData[edge1];
                  for (int index2 = 0; index2 < dynamicBuffer2.Length; ++index2)
                  {
                    Entity node1 = dynamicBuffer2[index2].m_Node;
                    if (!onStack.ContainsKey(node1) || onStack[node1] < 1)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.Push(node1, stack, onStack);
                    }
                  }
                }
                if (!onStack.ContainsKey(edge2.m_Start) || onStack[edge2.m_Start] < 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Push(edge2.m_Start, stack, onStack);
                }
                if (!onStack.ContainsKey(edge2.m_End) || onStack[edge2.m_End] < 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Push(edge2.m_End, stack, onStack);
                }
              }
            }
          }
        }
        return num;
      }

      private void Push(
        Entity entity,
        NativeList<Entity> stack,
        NativeParallelHashMap<Entity, int> onStack)
      {
        if (!onStack.ContainsKey(entity))
          onStack[entity] = 1;
        else
          ++onStack[entity];
        stack.Add(in entity);
      }

      private Entity Pop(NativeList<Entity> stack)
      {
        Entity entity = Entity.Null;
        if (stack.Length > 0)
        {
          entity = stack[stack.Length - 1];
          stack.RemoveAtSwapBack(stack.Length - 1);
        }
        return entity;
      }

      private bool Check(ObjectPlacementTriggerData triggerData, NativeArray<PrefabRef> prefabRefs)
      {
        for (int index = 0; index < prefabRefs.Length; ++index)
        {
          if (prefabRefs[index].m_Prefab == triggerData.m_Object)
            return true;
        }
        return false;
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
      public ComponentTypeHandle<ObjectPlacementTriggerCountData> __Game_Tutorials_ObjectPlacementTriggerCountData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityProducer> __Game_Buildings_ElectricityProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ElectricityConnection> __Game_Net_ElectricityConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ElectricityConnection> __Game_Net_ElectricityConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.WaterPipeConnection> __Game_Net_WaterPipeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.WaterPipeConnection> __Game_Net_WaterPipeConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.SewageOutlet> __Game_Buildings_SewageOutlet_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Transformer> __Game_Buildings_Transformer_RO_ComponentLookup;
      [ReadOnly]
      public BufferTypeHandle<ObjectPlacementTriggerData> __Game_Tutorials_ObjectPlacementTriggerData_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ForceUIGroupUnlockData> __Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<UnlockRequirement> __Game_Prefabs_UnlockRequirement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_ObjectPlacementTriggerCountData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectPlacementTriggerCountData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityProducer_RO_ComponentLookup = state.GetComponentLookup<ElectricityProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ElectricityConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ElectricityConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ElectricityConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.ElectricityConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_WaterPipeConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Net.WaterPipeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_WaterPipeConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.WaterPipeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SewageOutlet_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.SewageOutlet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Transformer_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Transformer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_ObjectPlacementTriggerData_RO_BufferTypeHandle = state.GetBufferTypeHandle<ObjectPlacementTriggerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ForceUIGroupUnlockData_RO_BufferLookup = state.GetBufferLookup<ForceUIGroupUnlockData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirement_RO_BufferLookup = state.GetBufferLookup<UnlockRequirement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
