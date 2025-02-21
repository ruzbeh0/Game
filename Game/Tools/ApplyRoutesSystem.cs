// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyRoutesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Pathfind;
using Game.Routes;
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
  public class ApplyRoutesSystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private EntityQuery m_TempQuery;
    private EntityArchetype m_PathTargetEventArchetype;
    private ComponentTypeSet m_AppliedTypes;
    private ApplyRoutesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Route>(),
          ComponentType.ReadOnly<Waypoint>(),
          ComponentType.ReadOnly<Segment>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PathTargetEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<PathTargetMoved>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ConnectedRoute_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ApplyRoutesSystem.PatchTempReferencesJob jobData1 = new ApplyRoutesSystem.PatchTempReferencesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_WaypointType = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle,
        m_WaypointConnectionData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_Routes = this.__TypeHandle.__Game_Routes_ConnectedRoute_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_VehicleTiming_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_PathTargets_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ApplyRoutesSystem.HandleTempEntitiesJob jobData2 = new ApplyRoutesSystem.HandleTempEntitiesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_WaypointType = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle,
        m_SegmentType = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle,
        m_RoutePositionType = this.__TypeHandle.__Game_Routes_Position_RO_ComponentTypeHandle,
        m_RouteConnectedType = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentTypeHandle,
        m_RoutePathTargetsType = this.__TypeHandle.__Game_Routes_PathTargets_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_RouteWaypointType = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle,
        m_RouteSegmentType = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_RoutePositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_RouteConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_VehicleTimingData = this.__TypeHandle.__Game_Routes_VehicleTiming_RO_ComponentLookup,
        m_RouteInfoData = this.__TypeHandle.__Game_Routes_RouteInfo_RO_ComponentLookup,
        m_Waypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_Segments = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup,
        m_PathTargetEventArchetype = this.m_PathTargetEventArchetype,
        m_AppliedTypes = this.m_AppliedTypes,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle job0 = jobData1.Schedule<ApplyRoutesSystem.PatchTempReferencesJob>(this.m_TempQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery tempQuery = this.m_TempQuery;
      JobHandle dependency = this.Dependency;
      JobHandle jobHandle = jobData2.ScheduleParallel<ApplyRoutesSystem.HandleTempEntitiesJob>(tempQuery, dependency);
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
    public ApplyRoutesSystem()
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
      public ComponentTypeHandle<Waypoint> m_WaypointType;
      [ReadOnly]
      public ComponentLookup<Connected> m_WaypointConnectionData;
      public BufferLookup<ConnectedRoute> m_Routes;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Waypoint> nativeArray1 = chunk.GetNativeArray<Waypoint>(ref this.m_WaypointType);
        if (nativeArray1.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray2[index];
          Temp temp = nativeArray3[index];
          if (temp.m_Original != Entity.Null)
          {
            Connected connected1 = new Connected();
            Connected connected2 = new Connected();
            // ISSUE: reference to a compiler-generated field
            if (this.m_WaypointConnectionData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              connected1 = this.m_WaypointConnectionData[entity];
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_WaypointConnectionData.HasComponent(temp.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              connected2 = this.m_WaypointConnectionData[temp.m_Original];
            }
            if (connected1.m_Connected != connected2.m_Connected)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_Routes.HasBuffer(connected2.m_Connected))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<ConnectedRoute>(this.m_Routes[connected2.m_Connected], new ConnectedRoute(temp.m_Original));
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_Routes.HasBuffer(connected1.m_Connected))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<ConnectedRoute>(this.m_Routes[connected1.m_Connected], new ConnectedRoute(temp.m_Original));
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
      public ComponentTypeHandle<Waypoint> m_WaypointType;
      [ReadOnly]
      public ComponentTypeHandle<Segment> m_SegmentType;
      [ReadOnly]
      public ComponentTypeHandle<Position> m_RoutePositionType;
      [ReadOnly]
      public ComponentTypeHandle<Connected> m_RouteConnectedType;
      [ReadOnly]
      public ComponentTypeHandle<PathTargets> m_RoutePathTargetsType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> m_RouteWaypointType;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> m_RouteSegmentType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Position> m_RoutePositionData;
      [ReadOnly]
      public ComponentLookup<Connected> m_RouteConnectedData;
      [ReadOnly]
      public ComponentLookup<VehicleTiming> m_VehicleTimingData;
      [ReadOnly]
      public ComponentLookup<RouteInfo> m_RouteInfoData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_Waypoints;
      [ReadOnly]
      public BufferLookup<RouteSegment> m_Segments;
      [ReadOnly]
      public EntityArchetype m_PathTargetEventArchetype;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
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
        NativeArray<Waypoint> nativeArray3 = chunk.GetNativeArray<Waypoint>(ref this.m_WaypointType);
        if (nativeArray3.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Position> nativeArray4 = chunk.GetNativeArray<Position>(ref this.m_RoutePositionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Connected> nativeArray5 = chunk.GetNativeArray<Connected>(ref this.m_RouteConnectedType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Temp temp = nativeArray2[index];
            if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Delete(unfilteredChunkIndex, entity, temp);
            }
            else if (temp.m_Original != Entity.Null)
            {
              if (nativeArray5.Length != 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.Update(unfilteredChunkIndex, entity, temp, nativeArray3[index], nativeArray4[index], nativeArray5[index]);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.Update(unfilteredChunkIndex, entity, temp, nativeArray3[index], nativeArray4[index], new Connected());
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<VehicleTiming>(unfilteredChunkIndex, entity, temp.m_Original, this.m_VehicleTimingData, false);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Segment> nativeArray6 = chunk.GetNativeArray<Segment>(ref this.m_SegmentType);
          if (nativeArray6.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<PathTargets> nativeArray7 = chunk.GetNativeArray<PathTargets>(ref this.m_RoutePathTargetsType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PathInformation> nativeArray8 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              Temp temp = nativeArray2[index];
              if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.Delete(unfilteredChunkIndex, entity, temp);
              }
              else if (temp.m_Original != Entity.Null)
              {
                if (nativeArray7.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CopyToOriginal<PathTargets>(unfilteredChunkIndex, temp, nativeArray7[index]);
                }
                if (nativeArray8.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdatePathInfo(unfilteredChunkIndex, temp, nativeArray8[index]);
                }
                if (bufferAccessor.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CopyToOriginal<PathElement>(unfilteredChunkIndex, temp, bufferAccessor[index]);
                }
                // ISSUE: reference to a compiler-generated method
                this.Update(unfilteredChunkIndex, entity, temp, nativeArray6[index]);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.Create(unfilteredChunkIndex, entity);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<RouteWaypoint> bufferAccessor1 = chunk.GetBufferAccessor<RouteWaypoint>(ref this.m_RouteWaypointType);
            if (bufferAccessor1.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<RouteSegment> bufferAccessor2 = chunk.GetBufferAccessor<RouteSegment>(ref this.m_RouteSegmentType);
              for (int index = 0; index < nativeArray1.Length; ++index)
              {
                Entity entity = nativeArray1[index];
                Temp temp = nativeArray2[index];
                if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
                {
                  if (temp.m_Original != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.Delete(unfilteredChunkIndex, entity, temp, bufferAccessor1[index], bufferAccessor2[index]);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.Delete(unfilteredChunkIndex, entity, temp);
                  }
                }
                else if (temp.m_Original != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Update(unfilteredChunkIndex, entity, temp, bufferAccessor1[index], bufferAccessor2[index]);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Create(unfilteredChunkIndex, entity);
                }
              }
            }
            else
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
                else if (temp.m_Original != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Update(unfilteredChunkIndex, entity, temp);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Create(unfilteredChunkIndex, entity);
                }
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

      private void Update(int chunkIndex, Entity entity, Temp temp, bool updateOriginal = true)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HiddenData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(chunkIndex, temp.m_Original);
        }
        if (updateOriginal)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(chunkIndex, temp.m_Original, new Updated());
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Update(
        int chunkIndex,
        Entity entity,
        Temp temp,
        Waypoint waypoint,
        Position position,
        Connected connected)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Waypoint>(chunkIndex, temp.m_Original, waypoint);
        // ISSUE: reference to a compiler-generated field
        Position position1 = this.m_RoutePositionData[temp.m_Original];
        if (!position1.m_Position.Equals(position.m_Position))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity(chunkIndex, this.m_PathTargetEventArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathTargetMoved>(chunkIndex, entity1, new PathTargetMoved(temp.m_Original, position1.m_Position, position.m_Position));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Position>(chunkIndex, temp.m_Original, position);
        }
        if (connected.m_Connected != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_RouteConnectedData.HasComponent(temp.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Connected>(chunkIndex, temp.m_Original, connected);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Connected>(chunkIndex, temp.m_Original, connected);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_RouteConnectedData.HasComponent(temp.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Connected>(chunkIndex, temp.m_Original);
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.Update(chunkIndex, entity, temp);
      }

      private void CopyToOriginal<T>(int chunkIndex, Temp temp, T data) where T : unmanaged, IComponentData
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<T>(chunkIndex, temp.m_Original, data);
      }

      private void CopyToOriginal<T>(int chunkIndex, Temp temp, DynamicBuffer<T> data) where T : unmanaged, IBufferElementData
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetBuffer<T>(chunkIndex, temp.m_Original).CopyFrom(data.AsNativeArray());
      }

      private void UpdatePathInfo(int chunkIndex, Temp temp, PathInformation data)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PathInformation>(chunkIndex, temp.m_Original, data);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_RouteInfoData.HasComponent(temp.m_Original))
          return;
        // ISSUE: reference to a compiler-generated field
        RouteInfo component = this.m_RouteInfoData[temp.m_Original];
        component.m_Distance = math.max(component.m_Distance, data.m_Distance);
        component.m_Duration = math.max(component.m_Duration, data.m_Duration);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RouteInfo>(chunkIndex, temp.m_Original, component);
      }

      private void Update(int chunkIndex, Entity entity, Temp temp, Segment segment)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Segment>(chunkIndex, temp.m_Original, segment);
        // ISSUE: reference to a compiler-generated method
        this.Update(chunkIndex, entity, temp);
      }

      private void Delete(
        int chunkIndex,
        Entity entity,
        Temp temp,
        DynamicBuffer<RouteWaypoint> waypoints,
        DynamicBuffer<RouteSegment> segments)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> waypoint1 = this.m_Waypoints[temp.m_Original];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteSegment> segment1 = this.m_Segments[temp.m_Original];
        NativeParallelHashMap<Entity, int> nativeParallelHashMap = new NativeParallelHashMap<Entity, int>(waypoint1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < waypoints.Length; ++index)
        {
          RouteWaypoint waypoint2 = waypoints[index];
          if (waypoint2.m_Waypoint != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            Temp temp1 = this.m_TempData[waypoint2.m_Waypoint];
            if (temp1.m_Original != Entity.Null)
              nativeParallelHashMap.TryAdd(temp1.m_Original, 1);
          }
        }
        for (int index = 0; index < waypoint1.Length; ++index)
        {
          RouteWaypoint routeWaypoint = waypoint1[index];
          if (routeWaypoint.m_Waypoint != Entity.Null && !nativeParallelHashMap.TryGetValue(routeWaypoint.m_Waypoint, out int _))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, routeWaypoint.m_Waypoint, new Deleted());
          }
        }
        nativeParallelHashMap.Clear();
        for (int index = 0; index < segments.Length; ++index)
        {
          RouteSegment segment2 = segments[index];
          if (segment2.m_Segment != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            Temp temp2 = this.m_TempData[segment2.m_Segment];
            if (temp2.m_Original != Entity.Null)
              nativeParallelHashMap.TryAdd(temp2.m_Original, 1);
          }
        }
        for (int index = 0; index < segment1.Length; ++index)
        {
          RouteSegment routeSegment = segment1[index];
          if (routeSegment.m_Segment != Entity.Null && !nativeParallelHashMap.TryGetValue(routeSegment.m_Segment, out int _))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, routeSegment.m_Segment, new Deleted());
          }
        }
        nativeParallelHashMap.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, temp.m_Original, new Deleted());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Update(
        int chunkIndex,
        Entity entity,
        Temp temp,
        DynamicBuffer<RouteWaypoint> waypoints,
        DynamicBuffer<RouteSegment> segments)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> waypoint1 = this.m_Waypoints[temp.m_Original];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteSegment> segment1 = this.m_Segments[temp.m_Original];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> dynamicBuffer1 = this.m_CommandBuffer.SetBuffer<RouteWaypoint>(chunkIndex, temp.m_Original);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteSegment> dynamicBuffer2 = this.m_CommandBuffer.SetBuffer<RouteSegment>(chunkIndex, temp.m_Original);
        dynamicBuffer1.ResizeUninitialized(waypoints.Length);
        dynamicBuffer2.ResizeUninitialized(segments.Length);
        NativeParallelHashMap<Entity, int> nativeParallelHashMap = new NativeParallelHashMap<Entity, int>(waypoint1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < waypoints.Length; ++index)
        {
          RouteWaypoint waypoint2 = waypoints[index];
          if (waypoint2.m_Waypoint != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            Temp temp1 = this.m_TempData[waypoint2.m_Waypoint];
            if (temp1.m_Original != Entity.Null)
            {
              nativeParallelHashMap.TryAdd(temp1.m_Original, 1);
              waypoint2.m_Waypoint = temp1.m_Original;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Owner>(chunkIndex, waypoint2.m_Waypoint, new Owner(temp.m_Original));
            }
          }
          dynamicBuffer1[index] = waypoint2;
        }
        for (int index = 0; index < waypoint1.Length; ++index)
        {
          RouteWaypoint routeWaypoint = waypoint1[index];
          if (routeWaypoint.m_Waypoint != Entity.Null && !nativeParallelHashMap.TryGetValue(routeWaypoint.m_Waypoint, out int _))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, routeWaypoint.m_Waypoint, new Deleted());
          }
        }
        nativeParallelHashMap.Clear();
        for (int index = 0; index < segments.Length; ++index)
        {
          RouteSegment segment2 = segments[index];
          if (segment2.m_Segment != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            Temp temp2 = this.m_TempData[segment2.m_Segment];
            if (temp2.m_Original != Entity.Null)
            {
              nativeParallelHashMap.TryAdd(temp2.m_Original, 1);
              segment2.m_Segment = temp2.m_Original;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Owner>(chunkIndex, segment2.m_Segment, new Owner(temp.m_Original));
            }
          }
          dynamicBuffer2[index] = segment2;
        }
        for (int index = 0; index < segment1.Length; ++index)
        {
          RouteSegment routeSegment = segment1[index];
          if (routeSegment.m_Segment != Entity.Null && !nativeParallelHashMap.TryGetValue(routeSegment.m_Segment, out int _))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, routeSegment.m_Segment, new Deleted());
          }
        }
        nativeParallelHashMap.Dispose();
        // ISSUE: reference to a compiler-generated method
        this.Update(chunkIndex, entity, temp);
      }

      private void Create(int chunkIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Temp>(chunkIndex, entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(chunkIndex, entity, in this.m_AppliedTypes);
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
      public ComponentTypeHandle<Waypoint> __Game_Routes_Waypoint_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      public BufferLookup<ConnectedRoute> __Game_Routes_ConnectedRoute_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Segment> __Game_Routes_Segment_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Position> __Game_Routes_Position_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Connected> __Game_Routes_Connected_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathTargets> __Game_Routes_PathTargets_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> __Game_Routes_RouteSegment_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleTiming> __Game_Routes_VehicleTiming_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteInfo> __Game_Routes_RouteInfo_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteSegment> __Game_Routes_RouteSegment_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RW_BufferLookup = state.GetBufferLookup<ConnectedRoute>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathTargets_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathTargets>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_VehicleTiming_RO_ComponentLookup = state.GetComponentLookup<VehicleTiming>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteInfo_RO_ComponentLookup = state.GetComponentLookup<RouteInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferLookup = state.GetBufferLookup<RouteSegment>(true);
      }
    }
  }
}
