// Decompiled with JetBrains decompiler
// Type: Game.Serialization.SubLaneSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class SubLaneSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private SubLaneSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<Lane>(), ComponentType.ReadOnly<Owner>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SubLaneSystem.SubLaneJob jobData = new SubLaneSystem.SubLaneJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PedestrianLaneType = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle,
        m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle,
        m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle,
        m_ParkingLaneType = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle,
        m_ConnectionLaneType = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<SubLaneSystem.SubLaneJob>(this.m_Query, this.Dependency);
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
    public SubLaneSystem()
    {
    }

    [BurstCompile]
    private struct SubLaneJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PedestrianLane> m_PedestrianLaneType;
      [ReadOnly]
      public ComponentTypeHandle<CarLane> m_CarLaneType;
      [ReadOnly]
      public ComponentTypeHandle<TrackLane> m_TrackLaneType;
      [ReadOnly]
      public ComponentTypeHandle<ParkingLane> m_ParkingLaneType;
      [ReadOnly]
      public ComponentTypeHandle<ConnectionLane> m_ConnectionLaneType;
      public BufferLookup<SubLane> m_SubLanes;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ParkingLane> nativeArray3 = chunk.GetNativeArray<ParkingLane>(ref this.m_ParkingLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ConnectionLane> nativeArray4 = chunk.GetNativeArray<ConnectionLane>(ref this.m_ConnectionLaneType);
        PathMethod pathMethod = (PathMethod) 0;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<PedestrianLane>(ref this.m_PedestrianLaneType))
          pathMethod |= PathMethod.Pedestrian;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<CarLane>(ref this.m_CarLaneType))
          pathMethod |= PathMethod.Road;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<TrackLane>(ref this.m_TrackLaneType))
          pathMethod |= PathMethod.Track;
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity lane = nativeArray1[index];
          DynamicBuffer<SubLane> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubLanes.TryGetBuffer(nativeArray2[index].m_Owner, out bufferData))
          {
            PathMethod pathMethods = pathMethod;
            ParkingLane parkingLane;
            if (CollectionUtils.TryGet<ParkingLane>(nativeArray3, index, out parkingLane))
            {
              if ((parkingLane.m_Flags & ParkingLaneFlags.SpecialVehicles) != (ParkingLaneFlags) 0)
                pathMethods |= PathMethod.Boarding | PathMethod.SpecialParking;
              else
                pathMethods |= PathMethod.Parking | PathMethod.Boarding;
            }
            ConnectionLane connectionLane;
            if (CollectionUtils.TryGet<ConnectionLane>(nativeArray4, index, out connectionLane))
            {
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
                pathMethods |= PathMethod.Pedestrian;
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0)
                pathMethods |= PathMethod.Road;
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0)
                pathMethods |= PathMethod.Track;
              if ((connectionLane.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
                pathMethods |= PathMethod.Parking | PathMethod.Boarding;
            }
            bufferData.Add(new SubLane(lane, pathMethods));
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarLane> __Game_Net_CarLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrackLane> __Game_Net_TrackLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkingLane> __Game_Net_ParkingLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentTypeHandle;
      public BufferLookup<SubLane> __Game_Net_SubLane_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RW_BufferLookup = state.GetBufferLookup<SubLane>();
      }
    }
  }
}
