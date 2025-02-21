// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.FireAndRescueInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Events;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class FireAndRescueInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "fireAndRescueInfo";
    private LocalEffectSystem m_LocalEffectSystem;
    private ClimateSystem m_ClimateSystem;
    private FireHazardSystem m_FireHazardSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_FlammableQuery;
    private EntityQuery m_FireStationsModifiedQuery;
    private EntityQuery m_FireConfigQuery;
    private NativeArray<float> m_Results;
    private ValueBinding<IndicatorValue> m_AverageFireHazard;
    private FireAndRescueInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalEffectSystem = this.World.GetOrCreateSystemManaged<LocalEffectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FireHazardSystem = this.World.GetOrCreateSystemManaged<FireHazardSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FlammableQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Tree>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Buildings.FireStation>(),
          ComponentType.ReadOnly<OnFire>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_FireStationsModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.FireStation>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_FireConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageFireHazard = new ValueBinding<IndicatorValue>("fireAndRescueInfo", "averageFireHazard", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<float>(2, Allocator.Persistent);
    }

    protected override bool Active => base.Active || this.m_AverageFireHazard.active;

    protected override bool Modified => !this.m_FireStationsModifiedQuery.IsEmptyIgnoreFilter;

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
    }

    protected override void PerformUpdate()
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      LocalEffectSystem.ReadData readData = this.m_LocalEffectSystem.GetReadData(out dependencies);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      FireConfigurationPrefab prefab = this.m_PrefabSystem.GetPrefab<FireConfigurationPrefab>(this.m_FireConfigQuery.GetSingletonEntity());
      // ISSUE: reference to a compiler-generated field
      if (this.m_Results.IsCreated)
      {
        this.Dependency.Complete();
        // ISSUE: reference to a compiler-generated field
        float result1 = this.m_Results[0];
        // ISSUE: reference to a compiler-generated field
        float result2 = this.m_Results[1];
        // ISSUE: reference to a compiler-generated field
        this.m_AverageFireHazard.Update(new IndicatorValue(0.0f, 100f, (double) result2 > 0.0 ? result1 / result2 : 0.0f));
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] = 0.0f;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new FireAndRescueInfoviewUISystem.FireHazardJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_CurrentDistrictType = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle,
        m_UnderConstructionType = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_FireHazardData = new EventHelpers.FireHazardData((SystemBase) this, readData, prefab, (float) this.m_ClimateSystem.temperature, this.m_FireHazardSystem.noRainDays),
        m_Result = this.m_Results
      }.Schedule<FireAndRescueInfoviewUISystem.FireHazardJob>(this.m_FlammableQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LocalEffectSystem.AddLocalEffectReader(jobHandle);
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
    public FireAndRescueInfoviewUISystem()
    {
    }

    [BurstCompile]
    private struct FireHazardJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictType;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> m_DamagedType;
      [ReadOnly]
      public ComponentTypeHandle<UnderConstruction> m_UnderConstructionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public EventHelpers.FireHazardData m_FireHazardData;
      public NativeArray<float> m_Result;

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
        NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        if (nativeArray3.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray4 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Damaged> nativeArray5 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnderConstruction> nativeArray6 = chunk.GetNativeArray<UnderConstruction>(ref this.m_UnderConstructionType);
        float num1 = 0.0f;
        float num2 = 0.0f;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray2[index];
          Building building = nativeArray3[index];
          CurrentDistrict currentDistrict = nativeArray4[index];
          Damaged damaged;
          CollectionUtils.TryGet<Damaged>(nativeArray5, index, out damaged);
          UnderConstruction underConstruction;
          if (!CollectionUtils.TryGet<UnderConstruction>(nativeArray6, index, out underConstruction))
            underConstruction = new UnderConstruction()
            {
              m_Progress = byte.MaxValue
            };
          float riskFactor;
          // ISSUE: reference to a compiler-generated field
          if (this.m_FireHazardData.GetFireHazard(prefabRef, building, currentDistrict, damaged, underConstruction, out float _, out riskFactor))
          {
            num1 += riskFactor;
            ++num2;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Result[0] += num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Result[1] += num2;
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
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
      }
    }
  }
}
