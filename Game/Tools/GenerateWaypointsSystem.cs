// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateWaypointsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateWaypointsSystem : GameSystemBase
  {
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_DeletedQuery;
    private GenerateWaypointsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreationDefinition>(), ComponentType.ReadOnly<WaypointDefinition>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Segment>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync1 = this.m_DefinitionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync2 = this.m_DeletedQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_PathTargets_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new GenerateWaypointsSystem.CreateWaypointsJob()
      {
        m_DefinitionChunks = archetypeChunkListAsync1,
        m_DeletedChunks = archetypeChunkListAsync2,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_SegmentType = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_WaypointDefinitionType = this.__TypeHandle.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_RouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
        m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_SegmentData = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup,
        m_PathTargetsData = this.__TypeHandle.__Game_Routes_PathTargets_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_RouteSegments = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup,
        m_PathElementData = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<GenerateWaypointsSystem.CreateWaypointsJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2));
      archetypeChunkListAsync1.Dispose(jobHandle);
      archetypeChunkListAsync2.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public GenerateWaypointsSystem()
    {
    }

    [BurstCompile]
    private struct CreateWaypointsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DefinitionChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DeletedChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<Segment> m_SegmentType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<WaypointDefinition> m_WaypointDefinitionType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_RouteData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Segment> m_SegmentData;
      [ReadOnly]
      public ComponentLookup<PathTargets> m_PathTargetsData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<RouteSegment> m_RouteSegments;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElementData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue> oldSegments = new NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_DeletedChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FillOldSegments(this.m_DeletedChunks[index], oldSegments);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_DefinitionChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateWaypointsAndSegments(this.m_DefinitionChunks[index], oldSegments);
        }
        oldSegments.Dispose();
      }

      private void FillOldSegments(
        ArchetypeChunk chunk,
        NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue> oldSegments)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Segment> nativeArray2 = chunk.GetNativeArray<Segment>(ref this.m_SegmentType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity segment1 = nativeArray1[index];
          Segment segment2 = nativeArray2[index];
          Entity owner = nativeArray3[index].m_Owner;
          Entity prefab = nativeArray4[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_RouteWaypoints.HasBuffer(owner) && this.m_TempData.HasComponent(owner))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[owner];
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_TempData[owner];
            if (routeWaypoint.Length > segment2.m_Index)
            {
              Entity waypoint1 = routeWaypoint[segment2.m_Index].m_Waypoint;
              Entity waypoint2 = routeWaypoint[math.select(segment2.m_Index + 1, 0, segment2.m_Index + 1 == routeWaypoint.Length)].m_Waypoint;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PositionData.HasComponent(waypoint1) && this.m_PositionData.HasComponent(waypoint2))
              {
                // ISSUE: reference to a compiler-generated field
                float4 float4_1 = new float4(this.m_PositionData[waypoint1].m_Position, 0.0f);
                // ISSUE: reference to a compiler-generated field
                float4 float4_2 = new float4(this.m_PositionData[waypoint2].m_Position, 1f);
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: object of a compiler-generated type is created
                oldSegments.TryAdd(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, temp.m_Original, float4_1), new GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue(segment1, float4_1, float4_2));
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: object of a compiler-generated type is created
                oldSegments.TryAdd(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, temp.m_Original, float4_2), new GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue(segment1, float4_1, float4_2));
              }
            }
          }
        }
      }

      private void FillOriginalSegments(
        Entity route,
        NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue> originalSegments)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[route];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteSegment> routeSegment = this.m_RouteSegments[route];
        // ISSUE: reference to a compiler-generated field
        Entity prefab = this.m_PrefabRefData[route].m_Prefab;
        for (int index = 0; index < routeSegment.Length; ++index)
        {
          Entity segment1 = routeSegment[index].m_Segment;
          // ISSUE: reference to a compiler-generated field
          Segment segment2 = this.m_SegmentData[segment1];
          if (routeWaypoint.Length > segment2.m_Index)
          {
            Entity waypoint1 = routeWaypoint[segment2.m_Index].m_Waypoint;
            Entity waypoint2 = routeWaypoint[math.select(segment2.m_Index + 1, 0, segment2.m_Index + 1 == routeWaypoint.Length)].m_Waypoint;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PositionData.HasComponent(waypoint1) && this.m_PositionData.HasComponent(waypoint2))
            {
              // ISSUE: reference to a compiler-generated field
              float4 float4_1 = new float4(this.m_PositionData[waypoint1].m_Position, 0.0f);
              // ISSUE: reference to a compiler-generated field
              float4 float4_2 = new float4(this.m_PositionData[waypoint2].m_Position, 1f);
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: object of a compiler-generated type is created
              originalSegments.TryAdd(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, Entity.Null, float4_1), new GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue(segment1, float4_1, float4_2));
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: object of a compiler-generated type is created
              originalSegments.TryAdd(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, Entity.Null, float4_2), new GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue(segment1, float4_1, float4_2));
            }
          }
        }
      }

      private void CreateWaypointsAndSegments(
        ArchetypeChunk chunk,
        NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue> oldSegments)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<WaypointDefinition> bufferAccessor = chunk.GetBufferAccessor<WaypointDefinition>(ref this.m_WaypointDefinitionType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          CreationDefinition creationDefinition = nativeArray[index1];
          DynamicBuffer<WaypointDefinition> dynamicBuffer = bufferAccessor[index1];
          Entity prefab1 = creationDefinition.m_Prefab;
          if (creationDefinition.m_Original != Entity.Null)
          {
            NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue> originalSegments = new NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: reference to a compiler-generated method
            this.FillOriginalSegments(creationDefinition.m_Original, originalSegments);
            // ISSUE: reference to a compiler-generated field
            Entity prefab2 = this.m_PrefabRefData[creationDefinition.m_Original].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            RouteData prefabRouteData = this.m_RouteData[prefab2];
            TempFlags tempFlags = (TempFlags) 0;
            if ((creationDefinition.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
              tempFlags |= TempFlags.Delete;
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated method
              this.CreateWaypoint(prefabRouteData, prefab2, tempFlags, dynamicBuffer[index2], index2);
            }
            if (dynamicBuffer.Length >= 2)
            {
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                WaypointDefinition startDefinition = dynamicBuffer[index3];
                WaypointDefinition endDefinition = dynamicBuffer[math.select(index3 + 1, 0, index3 + 1 == dynamicBuffer.Length)];
                // ISSUE: reference to a compiler-generated method
                Entity originalSegment = this.GetOriginalSegment(originalSegments, prefab2, startDefinition, endDefinition);
                // ISSUE: reference to a compiler-generated method
                this.CreateSegment(oldSegments, prefabRouteData, prefab2, originalSegment, creationDefinition.m_Original, tempFlags, startDefinition, endDefinition, index3);
              }
            }
            originalSegments.Dispose();
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            RouteData prefabRouteData = this.m_RouteData[prefab1];
            int length = dynamicBuffer.Length;
            bool flag = false;
            if (length >= 3 && dynamicBuffer[0].m_Position.Equals(dynamicBuffer[length - 1].m_Position))
            {
              --length;
              flag = true;
            }
            for (int index4 = 0; index4 < length; ++index4)
            {
              // ISSUE: reference to a compiler-generated method
              this.CreateWaypoint(prefabRouteData, prefab1, TempFlags.Create, dynamicBuffer[index4], index4);
            }
            for (int index5 = 1; index5 < length; ++index5)
            {
              WaypointDefinition startDefinition = dynamicBuffer[index5 - 1];
              WaypointDefinition endDefinition = dynamicBuffer[index5];
              // ISSUE: reference to a compiler-generated method
              this.CreateSegment(oldSegments, prefabRouteData, prefab1, Entity.Null, Entity.Null, TempFlags.Create, startDefinition, endDefinition, index5 - 1);
            }
            if (flag)
            {
              WaypointDefinition startDefinition = dynamicBuffer[length - 1];
              WaypointDefinition endDefinition = dynamicBuffer[0];
              // ISSUE: reference to a compiler-generated method
              this.CreateSegment(oldSegments, prefabRouteData, prefab1, Entity.Null, Entity.Null, TempFlags.Create, startDefinition, endDefinition, length - 1);
            }
          }
        }
      }

      private Entity GetOriginalSegment(
        NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue> originalSegments,
        Entity prefab,
        WaypointDefinition startDefinition,
        WaypointDefinition endDefinition)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey key1 = new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, Entity.Null, new float4(startDefinition.m_Position, 0.0f));
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey key2 = new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, Entity.Null, new float4(endDefinition.m_Position, 1f));
        // ISSUE: variable of a compiler-generated type
        GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue segmentValue;
        if (originalSegments.TryGetValue(key1, out segmentValue))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          originalSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, Entity.Null, segmentValue.m_StartPosition));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          originalSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, Entity.Null, segmentValue.m_EndPosition));
          // ISSUE: reference to a compiler-generated field
          return segmentValue.m_Segment;
        }
        if (!originalSegments.TryGetValue(key2, out segmentValue))
          return Entity.Null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        originalSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, Entity.Null, segmentValue.m_StartPosition));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        originalSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, Entity.Null, segmentValue.m_EndPosition));
        // ISSUE: reference to a compiler-generated field
        return segmentValue.m_Segment;
      }

      private void CreateWaypoint(
        RouteData prefabRouteData,
        Entity prefab,
        TempFlags tempFlags,
        WaypointDefinition definition,
        int index)
      {
        Entity entity;
        if (definition.m_Connection != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          entity = this.m_CommandBuffer.CreateEntity(prefabRouteData.m_ConnectedArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Connected>(entity, new Connected(definition.m_Connection));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          entity = this.m_CommandBuffer.CreateEntity(prefabRouteData.m_WaypointArchetype);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Waypoint>(entity, new Waypoint(index));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Position>(entity, new Position(definition.m_Position));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef(prefab));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(entity, new Temp(definition.m_Original, tempFlags));
      }

      private void CreateSegment(
        NativeParallelHashMap<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey, GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue> oldSegments,
        RouteData prefabRouteData,
        Entity prefab,
        Entity original,
        Entity originalRoute,
        TempFlags tempFlags,
        WaypointDefinition startDefinition,
        WaypointDefinition endDefinition,
        int index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey key1 = new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, originalRoute, new float4(startDefinition.m_Position, 0.0f));
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey key2 = new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, originalRoute, new float4(endDefinition.m_Position, 1f));
        bool2 x1;
        // ISSUE: variable of a compiler-generated type
        GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue segmentValue1;
        x1.x = oldSegments.TryGetValue(key1, out segmentValue1);
        // ISSUE: variable of a compiler-generated type
        GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue segmentValue2;
        x1.y = oldSegments.TryGetValue(key2, out segmentValue2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (math.all(x1) && segmentValue1.m_Segment != segmentValue2.m_Segment)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          oldSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, originalRoute, segmentValue1.m_StartPosition));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          oldSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, originalRoute, segmentValue1.m_EndPosition));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          oldSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, originalRoute, segmentValue2.m_StartPosition));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          oldSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, originalRoute, segmentValue2.m_EndPosition));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(segmentValue1.m_Segment);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(segmentValue1.m_Segment, new Updated());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Segment>(segmentValue1.m_Segment, new Segment(index));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Temp>(segmentValue1.m_Segment, new Temp(original, tempFlags));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathInformationData.HasComponent(segmentValue1.m_Segment) && this.m_PathInformationData.HasComponent(segmentValue2.m_Segment))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PathInformation pathInformation1 = this.m_PathInformationData[segmentValue1.m_Segment];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PathInformation pathInformation2 = this.m_PathInformationData[segmentValue2.m_Segment];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PathInformation>(segmentValue1.m_Segment, PathUtils.CombinePaths(pathInformation1, pathInformation2));
          }
          bool2 x2 = (bool2) false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathElementData.HasBuffer(segmentValue1.m_Segment) && this.m_PathElementData.HasBuffer(segmentValue2.m_Segment))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<PathElement> sourceElements1 = this.m_PathElementData[segmentValue1.m_Segment];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<PathElement> sourceElements2 = this.m_PathElementData[segmentValue2.m_Segment];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<PathElement> targetElements = this.m_CommandBuffer.SetBuffer<PathElement>(segmentValue1.m_Segment);
            PathUtils.CombinePaths(sourceElements1, sourceElements2, targetElements);
            x2 = new bool2(sourceElements1.Length == 0, sourceElements2.Length == 0);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PathTargetsData.HasComponent(segmentValue1.m_Segment) || !this.m_PathTargetsData.HasComponent(segmentValue2.m_Segment))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PathTargets component = this.m_PathTargetsData[segmentValue1.m_Segment];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PathTargets pathTargets = this.m_PathTargetsData[segmentValue2.m_Segment];
          component.m_StartLane = Entity.Null;
          component.m_EndLane = Entity.Null;
          component.m_CurvePositions = (float2) 0.0f;
          if (math.all(x2))
            component.m_ReadyEndPosition = component.m_ReadyStartPosition;
          else if (x2.x)
          {
            component.m_ReadyStartPosition = pathTargets.m_ReadyStartPosition;
            component.m_ReadyEndPosition = pathTargets.m_ReadyEndPosition;
          }
          else if (!x2.y)
            component.m_ReadyEndPosition = pathTargets.m_ReadyEndPosition;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathTargets>(segmentValue1.m_Segment, component);
        }
        else if (math.any(x1))
        {
          // ISSUE: variable of a compiler-generated type
          GenerateWaypointsSystem.CreateWaypointsJob.SegmentValue segmentValue3 = x1.x ? segmentValue1 : segmentValue2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          oldSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, originalRoute, segmentValue3.m_StartPosition));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          oldSegments.Remove(new GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey(prefab, originalRoute, segmentValue3.m_EndPosition));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(segmentValue3.m_Segment);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(segmentValue3.m_Segment, new Updated());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Segment>(segmentValue3.m_Segment, new Segment(index));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Temp>(segmentValue3.m_Segment, new Temp(original, tempFlags));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(prefabRouteData.m_SegmentArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Segment>(entity, new Segment(index));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef(prefab));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Temp>(entity, new Temp(original, tempFlags));
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathTargetsData.HasComponent(original))
          {
            // ISSUE: reference to a compiler-generated field
            PathTargets component = this.m_PathTargetsData[original];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PathTargets>(entity, component);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathInformationData.HasComponent(original))
          {
            // ISSUE: reference to a compiler-generated field
            PathInformation component = this.m_PathInformationData[original];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PathInformation>(entity, component);
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PathElementData.HasBuffer(original))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> dynamicBuffer = this.m_PathElementData[original];
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetBuffer<PathElement>(entity).CopyFrom(dynamicBuffer.AsNativeArray());
        }
      }

      private struct SegmentKey : IEquatable<GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey>
      {
        private Entity m_Prefab;
        private Entity m_OriginalRoute;
        private float4 m_Position;

        public SegmentKey(Entity prefab, Entity originalRoute, float4 position)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Prefab = prefab;
          // ISSUE: reference to a compiler-generated field
          this.m_OriginalRoute = originalRoute;
          // ISSUE: reference to a compiler-generated field
          this.m_Position = position;
        }

        public bool Equals(
          GenerateWaypointsSystem.CreateWaypointsJob.SegmentKey other)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_Prefab.Equals(other.m_Prefab) && this.m_OriginalRoute.Equals(other.m_OriginalRoute) && this.m_Position.Equals(other.m_Position);
        }

        public override int GetHashCode()
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_Prefab.GetHashCode() ^ this.m_Position.GetHashCode();
        }
      }

      private struct SegmentValue
      {
        public Entity m_Segment;
        public float4 m_StartPosition;
        public float4 m_EndPosition;

        public SegmentValue(Entity segment, float4 startPosition, float4 endPosition)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Segment = segment;
          // ISSUE: reference to a compiler-generated field
          this.m_StartPosition = startPosition;
          // ISSUE: reference to a compiler-generated field
          this.m_EndPosition = endPosition;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Segment> __Game_Routes_Segment_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<WaypointDefinition> __Game_Routes_WaypointDefinition_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteData> __Game_Prefabs_RouteData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Segment> __Game_Routes_Segment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathTargets> __Game_Routes_PathTargets_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteSegment> __Game_Routes_RouteSegment_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle = state.GetBufferTypeHandle<WaypointDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentLookup = state.GetComponentLookup<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentLookup = state.GetComponentLookup<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathTargets_RO_ComponentLookup = state.GetComponentLookup<PathTargets>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferLookup = state.GetBufferLookup<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
      }
    }
  }
}
