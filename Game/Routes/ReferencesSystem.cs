// Decompiled with JetBrains decompiler
// Type: Game.Routes.ReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Routes
{
  [CompilerGenerated]
  public class ReferencesSystem : GameSystemBase
  {
    private EntityQuery m_RouteQuery;
    private ReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RouteQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Route>(),
          ComponentType.ReadOnly<RouteWaypoint>(),
          ComponentType.ReadOnly<RouteSegment>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Created>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RouteQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_RouteQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle inputDeps = new ReferencesSystem.UpdateRouteReferencesJob()
      {
        m_RouteChunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RouteWaypointType = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferTypeHandle,
        m_RouteSegmentType = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RW_ComponentLookup
      }.Schedule<ReferencesSystem.UpdateRouteReferencesJob>(JobHandle.CombineDependencies(outJobHandle, this.Dependency));
      archetypeChunkListAsync.Dispose(inputDeps);
      this.Dependency = inputDeps;
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
    public ReferencesSystem()
    {
    }

    [BurstCompile]
    private struct UpdateRouteReferencesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_RouteChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<RouteWaypoint> m_RouteWaypointType;
      [ReadOnly]
      public BufferTypeHandle<RouteSegment> m_RouteSegmentType;
      public ComponentLookup<Owner> m_OwnerData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_RouteChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk routeChunk = this.m_RouteChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray = routeChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<RouteWaypoint> bufferAccessor1 = routeChunk.GetBufferAccessor<RouteWaypoint>(ref this.m_RouteWaypointType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<RouteSegment> bufferAccessor2 = routeChunk.GetBufferAccessor<RouteSegment>(ref this.m_RouteSegmentType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Entity entity = nativeArray[index2];
            DynamicBuffer<RouteWaypoint> dynamicBuffer1 = bufferAccessor1[index2];
            DynamicBuffer<RouteSegment> dynamicBuffer2 = bufferAccessor2[index2];
            for (int index3 = 0; index3 < dynamicBuffer1.Length; ++index3)
            {
              Entity waypoint = dynamicBuffer1[index3].m_Waypoint;
              // ISSUE: reference to a compiler-generated field
              Owner owner = this.m_OwnerData[waypoint] with
              {
                m_Owner = entity
              };
              // ISSUE: reference to a compiler-generated field
              this.m_OwnerData[waypoint] = owner;
            }
            for (int index4 = 0; index4 < dynamicBuffer2.Length; ++index4)
            {
              Entity segment = dynamicBuffer2[index4].m_Segment;
              if (segment != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                Owner owner = this.m_OwnerData[segment] with
                {
                  m_Owner = entity
                };
                // ISSUE: reference to a compiler-generated field
                this.m_OwnerData[segment] = owner;
              }
            }
          }
        }
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
      public ComponentLookup<Owner> __Game_Common_Owner_RW_ComponentLookup;

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
        this.__Game_Common_Owner_RW_ComponentLookup = state.GetComponentLookup<Owner>();
      }
    }
  }
}
