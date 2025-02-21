// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateRoutesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Prefabs;
using Game.Routes;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateRoutesSystem : GameSystemBase
  {
    private ModificationBarrier2 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_SubElementQuery;
    private GenerateRoutesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreationDefinition>(), ComponentType.ReadOnly<WaypointDefinition>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubElementQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Waypoint>(),
          ComponentType.ReadOnly<Segment>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_SubElementQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_ColorDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new GenerateRoutesSystem.CreateRoutesJob()
      {
        m_SubElementChunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_WaypointType = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle,
        m_SegmentType = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle,
        m_WaypointDefinitionType = this.__TypeHandle.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle,
        m_ColorDefinitionType = this.__TypeHandle.__Game_Tools_ColorDefinition_RO_ComponentTypeHandle,
        m_ColorData = this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_RouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
        m_TransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<GenerateRoutesSystem.CreateRoutesJob>(this.m_DefinitionQuery, JobHandle.CombineDependencies(outJobHandle, this.Dependency));
      archetypeChunkListAsync.Dispose(jobHandle);
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
    public GenerateRoutesSystem()
    {
    }

    [BurstCompile]
    private struct CreateRoutesJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_SubElementChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<Waypoint> m_WaypointType;
      [ReadOnly]
      public ComponentTypeHandle<Segment> m_SegmentType;
      [ReadOnly]
      public ComponentTypeHandle<ColorDefinition> m_ColorDefinitionType;
      [ReadOnly]
      public BufferTypeHandle<WaypointDefinition> m_WaypointDefinitionType;
      [ReadOnly]
      public ComponentLookup<Color> m_ColorData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_RouteData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_TransportLineData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray1 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ColorDefinition> nativeArray2 = chunk.GetNativeArray<ColorDefinition>(ref this.m_ColorDefinitionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<WaypointDefinition> bufferAccessor = chunk.GetBufferAccessor<WaypointDefinition>(ref this.m_WaypointDefinitionType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          CreationDefinition creationDefinition = nativeArray1[index];
          DynamicBuffer<WaypointDefinition> dynamicBuffer1 = bufferAccessor[index];
          RouteFlags routeFlags = (RouteFlags) 0;
          TempFlags tempFlags = (TempFlags) 0;
          RouteData routeData;
          TempFlags flags;
          if (creationDefinition.m_Original != Entity.Null)
          {
            routeFlags |= RouteFlags.Complete;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Hidden>(unfilteredChunkIndex, creationDefinition.m_Original, new Hidden());
            // ISSUE: reference to a compiler-generated field
            creationDefinition.m_Prefab = this.m_PrefabRefData[creationDefinition.m_Original].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            routeData = this.m_RouteData[creationDefinition.m_Prefab] with
            {
              m_Color = this.m_ColorData[creationDefinition.m_Original].m_Color
            };
            flags = (creationDefinition.m_Flags & CreationFlags.Delete) == (CreationFlags) 0 ? ((creationDefinition.m_Flags & CreationFlags.Select) == (CreationFlags) 0 ? tempFlags | TempFlags.Modify : tempFlags | TempFlags.Select) : tempFlags | TempFlags.Delete;
          }
          else
          {
            flags = tempFlags | TempFlags.Create;
            // ISSUE: reference to a compiler-generated field
            routeData = this.m_RouteData[creationDefinition.m_Prefab];
            if (nativeArray2.Length != 0)
              routeData.m_Color = nativeArray2[index].m_Color;
          }
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, routeData.m_RouteArchetype);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<RouteWaypoint> dynamicBuffer2 = this.m_CommandBuffer.SetBuffer<RouteWaypoint>(unfilteredChunkIndex, entity);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<RouteSegment> dynamicBuffer3 = this.m_CommandBuffer.SetBuffer<RouteSegment>(unfilteredChunkIndex, entity);
          if ((routeFlags & RouteFlags.Complete) == (RouteFlags) 0 && dynamicBuffer1.Length >= 3 && dynamicBuffer1[0].m_Position.Equals(dynamicBuffer1[dynamicBuffer1.Length - 1].m_Position))
          {
            CollectionUtils.ResizeInitialized<RouteWaypoint>(dynamicBuffer2, dynamicBuffer1.Length - 1);
            CollectionUtils.ResizeInitialized<RouteSegment>(dynamicBuffer3, dynamicBuffer1.Length - 1);
            // ISSUE: reference to a compiler-generated method
            this.FindSubElements(dynamicBuffer2, dynamicBuffer3);
            routeFlags |= RouteFlags.Complete;
          }
          else
          {
            CollectionUtils.ResizeInitialized<RouteWaypoint>(dynamicBuffer2, dynamicBuffer1.Length);
            CollectionUtils.ResizeInitialized<RouteSegment>(dynamicBuffer3, dynamicBuffer1.Length);
            // ISSUE: reference to a compiler-generated method
            this.FindSubElements(dynamicBuffer2, dynamicBuffer3);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Route>(unfilteredChunkIndex, entity, new Route()
          {
            m_Flags = routeFlags
          });
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Temp>(unfilteredChunkIndex, entity, new Temp(creationDefinition.m_Original, flags));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(unfilteredChunkIndex, entity, new PrefabRef(creationDefinition.m_Prefab));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Color>(unfilteredChunkIndex, entity, new Color(routeData.m_Color));
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportLineData.HasComponent(creationDefinition.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            TransportLineData transportLineData = this.m_TransportLineData[creationDefinition.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<TransportLine>(unfilteredChunkIndex, entity, new TransportLine(transportLineData));
          }
        }
      }

      private void FindSubElements(
        DynamicBuffer<RouteWaypoint> routeWaypoints,
        DynamicBuffer<RouteSegment> routeSegments)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_SubElementChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk subElementChunk = this.m_SubElementChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = subElementChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Waypoint> nativeArray2 = subElementChunk.GetNativeArray<Waypoint>(ref this.m_WaypointType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Segment> nativeArray3 = subElementChunk.GetNativeArray<Segment>(ref this.m_SegmentType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Waypoint waypoint = nativeArray2[index2];
            routeWaypoints[waypoint.m_Index] = new RouteWaypoint(nativeArray1[index2]);
          }
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Segment segment = nativeArray3[index3];
            routeSegments[segment.m_Index] = new RouteSegment(nativeArray1[index3]);
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Waypoint> __Game_Routes_Waypoint_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Segment> __Game_Routes_Segment_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<WaypointDefinition> __Game_Routes_WaypointDefinition_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ColorDefinition> __Game_Tools_ColorDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Color> __Game_Routes_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteData> __Game_Prefabs_RouteData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle = state.GetBufferTypeHandle<WaypointDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_ColorDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ColorDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Color_RO_ComponentLookup = state.GetComponentLookup<Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentLookup = state.GetComponentLookup<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
      }
    }
  }
}
