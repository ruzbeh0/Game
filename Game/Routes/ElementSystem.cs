// Decompiled with JetBrains decompiler
// Type: Game.Routes.ElementSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Routes
{
  [CompilerGenerated]
  public class ElementSystem : GameSystemBase
  {
    private EntityQuery m_RouteQuery;
    private ModificationBarrier2B m_ModificationBarrier;
    private ElementSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2B>();
      // ISSUE: reference to a compiler-generated field
      this.m_RouteQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.ReadOnly<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RouteQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new ElementSystem.CheckRouteElementsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RouteWaypointType = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle,
        m_RouteSegmentType = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<ElementSystem.CheckRouteElementsJob>(this.m_RouteQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public ElementSystem()
    {
    }

    [BurstCompile]
    private struct CheckRouteElementsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> m_RouteWaypointType;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> m_RouteSegmentType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RouteWaypoint> bufferAccessor1 = chunk.GetBufferAccessor<RouteWaypoint>(ref this.m_RouteWaypointType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RouteSegment> bufferAccessor2 = chunk.GetBufferAccessor<RouteSegment>(ref this.m_RouteSegmentType);
        for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
        {
          Entity entity = nativeArray[index1];
          DynamicBuffer<RouteWaypoint> dynamicBuffer = bufferAccessor1[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity waypoint = dynamicBuffer[index2].m_Waypoint;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData[waypoint].m_Owner == entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, waypoint, new Deleted());
            }
          }
        }
        for (int index3 = 0; index3 < bufferAccessor2.Length; ++index3)
        {
          Entity entity = nativeArray[index3];
          DynamicBuffer<RouteSegment> dynamicBuffer = bufferAccessor2[index3];
          for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
          {
            Entity segment = dynamicBuffer[index4].m_Segment;
            // ISSUE: reference to a compiler-generated field
            if (segment != Entity.Null && this.m_OwnerData[segment].m_Owner == entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, segment, new Deleted());
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> __Game_Routes_RouteSegment_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferTypeHandle = state.GetBufferTypeHandle<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
      }
    }
  }
}
