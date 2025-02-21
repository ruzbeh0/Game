// Decompiled with JetBrains decompiler
// Type: Game.Serialization.PassengerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Creatures;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class PassengerSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private PassengerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<CurrentVehicle>(),
          ComponentType.ReadOnly<CurrentTransport>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      PassengerSystem.PassengerJob jobData = new PassengerSystem.PassengerJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_CurrentTransportType = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<PassengerSystem.PassengerJob>(this.m_Query, this.Dependency);
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
    public PassengerSystem()
    {
    }

    [BurstCompile]
    private struct PassengerJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentTransport> m_CurrentTransportType;
      public BufferLookup<Passenger> m_Passengers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentVehicle> nativeArray2 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentTransport> nativeArray3 = chunk.GetNativeArray<CurrentTransport>(ref this.m_CurrentTransportType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity passenger = nativeArray1[index];
          CurrentVehicle currentVehicle = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Passengers.HasBuffer(currentVehicle.m_Vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Passengers[currentVehicle.m_Vehicle].Add(new Passenger(passenger));
          }
        }
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity passenger = nativeArray1[index];
          CurrentTransport currentTransport = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Passengers.HasBuffer(currentTransport.m_CurrentTransport))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Passengers[currentTransport.m_CurrentTransport].Add(new Passenger(passenger));
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
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentTypeHandle;
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RW_BufferLookup = state.GetBufferLookup<Passenger>();
      }
    }
  }
}
