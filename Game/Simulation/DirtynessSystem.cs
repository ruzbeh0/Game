// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DirtynessSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class DirtynessSystem : GameSystemBase
  {
    private CitySystem m_CitySystem;
    private EntityQuery m_SurfaceQuery;
    private DirtynessSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SurfaceQuery = this.GetEntityQuery(ComponentType.ReadWrite<Surface>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Overridden>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SurfaceQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Surface_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_BuildingCondition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      this.Dependency = new DirtynessSystem.DirtynessJob()
      {
        m_BuildingConditionType = this.__TypeHandle.__Game_Buildings_BuildingCondition_RO_ComponentTypeHandle,
        m_BuildingAbandonedType = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingEfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_ObjectSurfaceType = this.__TypeHandle.__Game_Objects_Surface_RW_ComponentTypeHandle,
        m_SpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_ZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_BuildingPropertyData = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_CityEffects = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_City = this.m_CitySystem.City
      }.ScheduleParallel<DirtynessSystem.DirtynessJob>(this.m_SurfaceQuery, this.Dependency);
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
    public DirtynessSystem()
    {
    }

    [BurstCompile]
    private struct DirtynessJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<BuildingCondition> m_BuildingConditionType;
      [ReadOnly]
      public ComponentTypeHandle<Abandoned> m_BuildingAbandonedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_BuildingEfficiencyType;
      public ComponentTypeHandle<Surface> m_ObjectSurfaceType;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneData;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityEffects;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public Entity m_City;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Surface> nativeArray1 = chunk.GetNativeArray<Surface>(ref this.m_ObjectSurfaceType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<BuildingCondition> nativeArray2 = chunk.GetNativeArray<BuildingCondition>(ref this.m_BuildingConditionType);
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Abandoned>(ref this.m_BuildingAbandonedType))
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
              nativeArray1.ElementAt<Surface>(index).m_Dirtyness = byte.MaxValue;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CityModifier> cityEffect = this.m_CityEffects[this.m_City];
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              BuildingCondition buildingCondition = nativeArray2[index];
              PrefabRef prefabRef = nativeArray3[index];
              ref Surface local = ref nativeArray1.ElementAt<Surface>(index);
              if (buildingCondition.m_Condition < 0)
              {
                int x = 0;
                SpawnableBuildingData componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SpawnableBuildingData.TryGetComponent(prefabRef.m_Prefab, out componentData))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  x = BuildingUtils.GetLevelingCost(this.m_ZoneData[componentData.m_ZonePrefab].m_AreaType, this.m_BuildingPropertyData[prefabRef.m_Prefab], math.min(4, (int) componentData.m_Level), cityEffect);
                }
                int num = math.max(x, -buildingCondition.m_Condition);
                local.m_Dirtyness = (byte) ((buildingCondition.m_Condition * -255 + (num >> 1)) / num);
              }
              else
                local.m_Dirtyness = (byte) 0;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Efficiency> bufferAccessor = chunk.GetBufferAccessor<Efficiency>(ref this.m_BuildingEfficiencyType);
          if (bufferAccessor.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
            for (int index = 0; index < bufferAccessor.Length; ++index)
            {
              DynamicBuffer<Efficiency> buffer = bufferAccessor[index];
              ref Surface local = ref nativeArray1.ElementAt<Surface>(index);
              float num = math.clamp(math.saturate(1f - BuildingUtils.GetEfficiency(buffer)) - (float) local.m_Dirtyness * 0.003921569f, -0.1f, 0.01f);
              local.m_Dirtyness = (byte) math.clamp((int) local.m_Dirtyness + MathUtils.RoundToIntRandom(ref random, num * (float) byte.MaxValue), 0, (int) byte.MaxValue);
            }
          }
          else
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
              nativeArray1.ElementAt<Surface>(index).m_Dirtyness = (byte) 0;
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
      public ComponentTypeHandle<BuildingCondition> __Game_Buildings_BuildingCondition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Abandoned> __Game_Buildings_Abandoned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      public ComponentTypeHandle<Surface> __Game_Objects_Surface_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_BuildingCondition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Surface_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Surface>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
