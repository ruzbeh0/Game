// Decompiled with JetBrains decompiler
// Type: Game.Serialization.CarKeeperSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Citizens;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class CarKeeperSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private EntityQuery m_KeeperQuery;
    private CarKeeperSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<PersonalCar>());
      // ISSUE: reference to a compiler-generated field
      this.m_KeeperQuery = this.GetEntityQuery(ComponentType.ReadOnly<CarKeeper>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = new CarKeeperSystem.CarKeeperJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PersonalCarType = this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentTypeHandle,
        m_CarKeeperData = this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentLookup
      }.Schedule<CarKeeperSystem.CarKeeperJob>(this.m_Query, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CarKeeperSystem.NoCarJob jobData = new CarKeeperSystem.NoCarJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CarKeeperType = this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentTypeHandle,
        m_PersonalCars = this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<CarKeeperSystem.NoCarJob>(this.m_KeeperQuery, dependsOn);
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
    public CarKeeperSystem()
    {
    }

    [BurstCompile]
    private struct CarKeeperJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PersonalCar> m_PersonalCarType;
      public ComponentLookup<CarKeeper> m_CarKeeperData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PersonalCar> nativeArray2 = chunk.GetNativeArray<PersonalCar>(ref this.m_PersonalCarType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PersonalCar personalCar = nativeArray2[index];
          CarKeeper component;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarKeeperData.TryGetEnabledComponent<CarKeeper>(personalCar.m_Keeper, out component))
          {
            component.m_Car = entity;
            // ISSUE: reference to a compiler-generated field
            this.m_CarKeeperData[personalCar.m_Keeper] = component;
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
    private struct NoCarJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<CarKeeper> m_CarKeeperType;
      [ReadOnly]
      public ComponentLookup<PersonalCar> m_PersonalCars;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarKeeper> nativeArray2 = chunk.GetNativeArray<CarKeeper>(ref this.m_CarKeeperType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.IsComponentEnabled<CarKeeper>(ref this.m_CarKeeperType, index) && !this.m_PersonalCars.HasComponent(nativeArray2[index].m_Car))
          {
            // ISSUE: reference to a compiler-generated field
            chunk.SetComponentEnabled<CarKeeper>(ref this.m_CarKeeperType, index, false);
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
      public ComponentTypeHandle<PersonalCar> __Game_Vehicles_PersonalCar_RO_ComponentTypeHandle;
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RW_ComponentLookup;
      public ComponentTypeHandle<CarKeeper> __Game_Citizens_CarKeeper_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PersonalCar> __Game_Vehicles_PersonalCar_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PersonalCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RW_ComponentLookup = state.GetComponentLookup<CarKeeper>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarKeeper>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RO_ComponentLookup = state.GetComponentLookup<PersonalCar>(true);
      }
    }
  }
}
