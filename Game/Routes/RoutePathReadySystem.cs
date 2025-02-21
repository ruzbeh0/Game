// Decompiled with JetBrains decompiler
// Type: Game.Routes.RoutePathReadySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Notifications;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Routes
{
  [CompilerGenerated]
  public class RoutePathReadySystem : GameSystemBase
  {
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_PathReadyQuery;
    private EntityQuery m_RouteQuery;
    private EntityQuery m_RouteConfigQuery;
    private bool m_Loaded;
    private RoutePathReadySystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathReadyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<PathUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<RouteConfigurationData>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = this.GetLoaded() ? this.m_RouteQuery : this.m_PathReadyQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_PathTargets_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle handle = new RoutePathReadySystem.RoutePathReadyJob()
      {
        m_PathUpdatedType = this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle,
        m_RouteWaypointType = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle,
        m_RouteSegmentType = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_SegmentData = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_RouteSegments = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup,
        m_PathTargetsData = this.__TypeHandle.__Game_Routes_PathTargets_RW_ComponentLookup,
        m_RouteConfigurationData = this.m_RouteConfigQuery.GetSingleton<RouteConfigurationData>(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      }.Schedule<RoutePathReadySystem.RoutePathReadyJob>(query, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(handle);
      this.Dependency = handle;
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
    public RoutePathReadySystem()
    {
    }

    [BurstCompile]
    private struct RoutePathReadyJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PathUpdated> m_PathUpdatedType;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> m_RouteWaypointType;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> m_RouteSegmentType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Segment> m_SegmentData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<RouteSegment> m_RouteSegments;
      public ComponentLookup<PathTargets> m_PathTargetsData;
      [ReadOnly]
      public RouteConfigurationData m_RouteConfigurationData;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathUpdated> nativeArray = chunk.GetNativeArray<PathUpdated>(ref this.m_PathUpdatedType);
        if (nativeArray.Length != 0)
        {
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            PathUpdated pathUpdated = nativeArray[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathTargetsData.HasComponent(pathUpdated.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              PathTargets pathTargets = this.m_PathTargetsData[pathUpdated.m_Owner] with
              {
                m_ReadyStartPosition = pathUpdated.m_Data.m_Position1,
                m_ReadyEndPosition = pathUpdated.m_Data.m_Position2
              };
              // ISSUE: reference to a compiler-generated field
              this.m_PathTargetsData[pathUpdated.m_Owner] = pathTargets;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathInformationData.HasComponent(pathUpdated.m_Owner) && !this.m_TempData.HasComponent(pathUpdated.m_Owner))
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdatePathfindNotifications(pathUpdated.m_Owner);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<RouteWaypoint> bufferAccessor1 = chunk.GetBufferAccessor<RouteWaypoint>(ref this.m_RouteWaypointType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<RouteSegment> bufferAccessor2 = chunk.GetBufferAccessor<RouteSegment>(ref this.m_RouteSegmentType);
          for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
          {
            DynamicBuffer<RouteWaypoint> dynamicBuffer1 = bufferAccessor1[index1];
            DynamicBuffer<RouteSegment> dynamicBuffer2 = bufferAccessor2[index1];
            for (int index2 = 0; index2 < dynamicBuffer2.Length; ++index2)
            {
              RouteSegment routeSegment = dynamicBuffer2[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathTargetsData.HasComponent(routeSegment.m_Segment))
              {
                RouteWaypoint routeWaypoint1 = dynamicBuffer1[index2];
                RouteWaypoint routeWaypoint2 = dynamicBuffer1[math.select(index2 + 1, 0, index2 + 1 >= dynamicBuffer1.Length)];
                // ISSUE: reference to a compiler-generated field
                PathTargets pathTargets = this.m_PathTargetsData[routeSegment.m_Segment];
                // ISSUE: reference to a compiler-generated field
                if (this.m_PositionData.HasComponent(routeWaypoint1.m_Waypoint))
                {
                  // ISSUE: reference to a compiler-generated field
                  pathTargets.m_ReadyStartPosition = this.m_PositionData[routeWaypoint1.m_Waypoint].m_Position;
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_PositionData.HasComponent(routeWaypoint2.m_Waypoint))
                {
                  // ISSUE: reference to a compiler-generated field
                  pathTargets.m_ReadyEndPosition = this.m_PositionData[routeWaypoint2.m_Waypoint].m_Position;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_PathTargetsData[routeSegment.m_Segment] = pathTargets;
              }
            }
          }
        }
      }

      private void UpdatePathfindNotifications(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SegmentData.HasComponent(entity) || !this.m_OwnerData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        Segment segment = this.m_SegmentData[entity];
        // ISSUE: reference to a compiler-generated field
        Owner owner = this.m_OwnerData[entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[owner.m_Owner];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteSegment> routeSegment = this.m_RouteSegments[owner.m_Owner];
        int index = segment.m_Index;
        int waypointIndex = math.select(segment.m_Index + 1, 0, segment.m_Index == routeWaypoint.Length - 1);
        // ISSUE: reference to a compiler-generated method
        this.UpdatePathfindNotification(routeWaypoint, routeSegment, index);
        // ISSUE: reference to a compiler-generated method
        this.UpdatePathfindNotification(routeWaypoint, routeSegment, waypointIndex);
      }

      private void UpdatePathfindNotification(
        DynamicBuffer<RouteWaypoint> waypoints,
        DynamicBuffer<RouteSegment> segments,
        int waypointIndex)
      {
        int index1 = math.select(waypointIndex - 1, waypoints.Length - 1, waypointIndex == 0);
        int index2 = waypointIndex;
        bool flag = false;
        PathInformation componentData1;
        // ISSUE: reference to a compiler-generated field
        if (index1 < segments.Length && this.m_PathInformationData.TryGetComponent(segments[index1].m_Segment, out componentData1))
          flag |= (double) componentData1.m_Distance < 0.0;
        PathInformation componentData2;
        // ISSUE: reference to a compiler-generated field
        if (index2 < segments.Length && this.m_PathInformationData.TryGetComponent(segments[index2].m_Segment, out componentData2))
          flag |= (double) componentData2.m_Distance < 0.0;
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Add(waypoints[waypointIndex].m_Waypoint, this.m_RouteConfigurationData.m_PathfindNotification, IconPriority.Warning);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Remove(waypoints[waypointIndex].m_Waypoint, this.m_RouteConfigurationData.m_PathfindNotification);
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

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PathUpdated> __Game_Pathfind_PathUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> __Game_Routes_RouteSegment_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Segment> __Game_Routes_Segment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteSegment> __Game_Routes_RouteSegment_RO_BufferLookup;
      public ComponentLookup<PathTargets> __Game_Routes_PathTargets_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentLookup = state.GetComponentLookup<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferLookup = state.GetBufferLookup<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathTargets_RW_ComponentLookup = state.GetComponentLookup<PathTargets>();
      }
    }
  }
}
