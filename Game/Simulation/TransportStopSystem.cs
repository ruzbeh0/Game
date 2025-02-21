// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportStopSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
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
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TransportStopSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 256;
    private EntityQuery m_StopQuery;
    private EndFrameBarrier m_EndFrameBarrier;
    private TransportStopSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_StopQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Routes.TransportStop>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_StopQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportStop_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new TransportStopSystem.TransportStopTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TaxiStandType = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ConnectedRouteType = this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferTypeHandle,
        m_TransportStopType = this.__TypeHandle.__Game_Routes_TransportStop_RW_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransportStationData = this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup,
        m_PrefabTransportStopData = this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<TransportStopSystem.TransportStopTickJob>(this.m_StopQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
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
    public TransportStopSystem()
    {
    }

    [BurstCompile]
    private struct TransportStopTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<TaxiStand> m_TaxiStandType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedRoute> m_ConnectedRouteType;
      public ComponentTypeHandle<Game.Routes.TransportStop> m_TransportStopType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> m_TransportStationData;
      [ReadOnly]
      public ComponentLookup<TransportStopData> m_PrefabTransportStopData;
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
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Routes.TransportStop> nativeArray4 = chunk.GetNativeArray<Game.Routes.TransportStop>(ref this.m_TransportStopType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedRoute> bufferAccessor = chunk.GetBufferAccessor<ConnectedRoute>(ref this.m_ConnectedRouteType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<TaxiStand>(ref this.m_TaxiStandType);
        for (int index1 = 0; index1 < nativeArray4.Length; ++index1)
        {
          PrefabRef prefabRef = nativeArray2[index1];
          Game.Routes.TransportStop transportStop = nativeArray4[index1];
          TransportStopData transportStopData = new TransportStopData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTransportStopData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            transportStopData = this.m_PrefabTransportStopData[prefabRef.m_Prefab];
          }
          float num1 = math.saturate(transportStopData.m_ComfortFactor);
          float num2 = math.max(0.0f, 1f + transportStopData.m_LoadingFactor);
          bool flag2 = true;
          if (nativeArray3.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            Entity transportStation1 = this.GetTransportStation(nativeArray3[index1].m_Owner);
            if (transportStation1 != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              Game.Buildings.TransportStation transportStation2 = this.m_TransportStationData[transportStation1];
              num1 = math.saturate(num1 + (1f - num1) * transportStation2.m_ComfortFactor);
              num2 = math.max(0.0f, num2 * transportStation2.m_LoadingFactor);
              flag2 = (transportStation2.m_Flags & TransportStationFlags.TransportStopsActive) != 0;
            }
          }
          if ((double) num1 != (double) transportStop.m_ComfortFactor || (double) num2 != (double) transportStop.m_LoadingFactor || flag2 != ((transportStop.m_Flags & StopFlags.Active) != 0))
          {
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, nativeArray1[index1], new PathfindUpdated());
            }
            if (bufferAccessor.Length != 0)
            {
              DynamicBuffer<ConnectedRoute> dynamicBuffer = bufferAccessor[index1];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(unfilteredChunkIndex, dynamicBuffer[index2].m_Waypoint, new PathfindUpdated());
              }
            }
          }
          transportStop.m_ComfortFactor = num1;
          transportStop.m_LoadingFactor = num2;
          if (flag2)
            transportStop.m_Flags |= StopFlags.Active;
          else
            transportStop.m_Flags &= ~StopFlags.Active;
          nativeArray4[index1] = transportStop;
        }
      }

      private Entity GetTransportStation(Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; !this.m_TransportStationData.HasComponent(owner); owner = this.m_OwnerData[owner].m_Owner)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OwnerData.HasComponent(owner))
            return Entity.Null;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(owner))
        {
          // ISSUE: reference to a compiler-generated field
          Entity owner1 = this.m_OwnerData[owner].m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportStationData.HasComponent(owner1))
            return owner1;
        }
        return owner;
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
      public ComponentTypeHandle<TaxiStand> __Game_Routes_TaxiStand_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedRoute> __Game_Routes_ConnectedRoute_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Routes.TransportStop> __Game_Routes_TransportStop_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> __Game_Buildings_TransportStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportStopData> __Game_Prefabs_TransportStopData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TaxiStand_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TaxiStand>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.TransportStop>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStopData_RO_ComponentLookup = state.GetComponentLookup<TransportStopData>(true);
      }
    }
  }
}
