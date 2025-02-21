// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ParkInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class ParkInitializeSystem : GameSystemBase
  {
    private CitySystem m_CitySystem;
    private EntityQuery m_ParkQuery;
    private ParkInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ParkQuery = this.GetEntityQuery(ComponentType.ReadWrite<Park>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ParkQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ModifiedServiceCoverage_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      this.Dependency = new ParkInitializeSystem.InitializeParksJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ParkType = this.__TypeHandle.__Game_Buildings_Park_RW_ComponentTypeHandle,
        m_ModifiedServiceCoverageType = this.__TypeHandle.__Game_Buildings_ModifiedServiceCoverage_RW_ComponentTypeHandle,
        m_ParkData = this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup,
        m_CoverageData = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_City = this.m_CitySystem.City
      }.ScheduleParallel<ParkInitializeSystem.InitializeParksJob>(this.m_ParkQuery, this.Dependency);
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
    public ParkInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeParksJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Park> m_ParkType;
      public ComponentTypeHandle<ModifiedServiceCoverage> m_ModifiedServiceCoverageType;
      [ReadOnly]
      public ComponentLookup<ParkData> m_ParkData;
      [ReadOnly]
      public ComponentLookup<CoverageData> m_CoverageData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public Entity m_City;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        DynamicBuffer<CityModifier> cityModifiers = new DynamicBuffer<CityModifier>();
        // ISSUE: reference to a compiler-generated field
        if (this.m_City != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cityModifiers = this.m_CityModifiers[this.m_City];
        }
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Park> nativeArray2 = chunk.GetNativeArray<Park>(ref this.m_ParkType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ModifiedServiceCoverage> nativeArray3 = chunk.GetNativeArray<ModifiedServiceCoverage>(ref this.m_ModifiedServiceCoverageType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity prefab = nativeArray1[index].m_Prefab;
          Park park = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ParkData prefabParkData = this.m_ParkData[prefab];
            park.m_Maintenance = prefabParkData.m_MaintenancePool;
            nativeArray2[index] = park;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CoverageData.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              CoverageData prefabCoverageData = this.m_CoverageData[prefab];
              // ISSUE: reference to a compiler-generated method
              nativeArray3[index] = ParkAISystem.GetModifiedServiceCoverage(park, prefabParkData, prefabCoverageData, cityModifiers);
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Park> __Game_Buildings_Park_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ModifiedServiceCoverage> __Game_Buildings_ModifiedServiceCoverage_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ParkData> __Game_Prefabs_ParkData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CoverageData> __Game_Prefabs_CoverageData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Park>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ModifiedServiceCoverage_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ModifiedServiceCoverage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkData_RO_ComponentLookup = state.GetComponentLookup<ParkData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CoverageData_RO_ComponentLookup = state.GetComponentLookup<CoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
