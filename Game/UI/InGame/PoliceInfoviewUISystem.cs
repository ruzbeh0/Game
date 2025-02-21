// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PoliceInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PoliceInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "policeInfo";
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ValueBinding<int> m_CrimeProducers;
    private ValueBinding<float> m_CrimeProbability;
    private ValueBinding<int> m_JailCapacity;
    private ValueBinding<int> m_ArrestedCriminals;
    private ValueBinding<int> m_InJail;
    private ValueBinding<int> m_PrisonCapacity;
    private ValueBinding<int> m_Prisoners;
    private ValueBinding<int> m_InPrison;
    private ValueBinding<int> m_Criminals;
    private ValueBinding<int> m_CrimePerMonth;
    private ValueBinding<float> m_EscapedRate;
    private GetterValueBinding<IndicatorValue> m_AverageCrimeProbability;
    private GetterValueBinding<IndicatorValue> m_JailAvailability;
    private GetterValueBinding<IndicatorValue> m_PrisonAvailability;
    private EntityQuery m_PrisonQuery;
    private EntityQuery m_PrisonModifiedQuery;
    private EntityQuery m_CriminalQuery;
    private EntityQuery m_PoliceStationQuery;
    private EntityQuery m_PoliceStationModifiedQuery;
    private EntityQuery m_CrimeProducerQuery;
    private EntityQuery m_CrimeProducerModifiedQuery;
    private NativeArray<float> m_Results;
    private PoliceInfoviewUISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_632591896_0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrisonQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Buildings.Prison>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Owner>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceStationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Buildings.PoliceStation>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Owner>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeProducerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<CrimeProducer>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CriminalQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<Criminal>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_PrisonModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Buildings.Prison>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceStationModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Buildings.PoliceStation>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeProducerModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<CrimeProducer>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CrimeProducers = new ValueBinding<int>("policeInfo", "crimeProducers", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CrimeProbability = new ValueBinding<float>("policeInfo", "crimeProbability", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PrisonCapacity = new ValueBinding<int>("policeInfo", "prisonCapacity", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Prisoners = new ValueBinding<int>("policeInfo", "prisoners", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_InPrison = new ValueBinding<int>("policeInfo", "inPrison", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_JailCapacity = new ValueBinding<int>("policeInfo", "jailCapacity", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ArrestedCriminals = new ValueBinding<int>("policeInfo", "arrestedCriminals", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_InJail = new ValueBinding<int>("policeInfo", "inJail", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Criminals = new ValueBinding<int>("policeInfo", "criminals", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CrimePerMonth = new ValueBinding<int>("policeInfo", "crimePerMonth", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_EscapedRate = new ValueBinding<float>("policeInfo", "escapedRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageCrimeProbability = new GetterValueBinding<IndicatorValue>("policeInfo", "averageCrimeProbability", (Func<IndicatorValue>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = this.m_CrimeProbability.value;
        // ISSUE: reference to a compiler-generated field
        int num2 = this.m_CrimeProducers.value;
        // ISSUE: reference to a compiler-generated field
        return new IndicatorValue(0.0f, this.__query_632591896_0.GetSingleton<PoliceConfigurationData>().m_MaxCrimeAccumulation, num2 > 0 ? num1 / (float) num2 : 0.0f);
      }), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_JailAvailability = new GetterValueBinding<IndicatorValue>("policeInfo", "jailAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_JailCapacity.value, (float) this.m_ArrestedCriminals.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PrisonAvailability = new GetterValueBinding<IndicatorValue>("policeInfo", "prisonAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_PrisonCapacity.value, (float) this.m_Prisoners.value)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<float>(8, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_AverageCrimeProbability.active || this.m_JailAvailability.active || this.m_PrisonAvailability.active || this.m_CrimeProducers.active || this.m_CrimeProbability.active || this.m_PrisonCapacity.active || this.m_Prisoners.active || this.m_InPrison.active || this.m_JailCapacity.active || this.m_ArrestedCriminals.active || this.m_InJail.active;
      }
    }

    protected override bool Modified
    {
      get
      {
        return !this.m_CrimeProducerModifiedQuery.IsEmptyIgnoreFilter || !this.m_PoliceStationModifiedQuery.IsEmptyIgnoreFilter || !this.m_PrisonModifiedQuery.IsEmptyIgnoreFilter;
      }
    }

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.ResetResults();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new PoliceInfoviewUISystem.CrimeProducerJob()
      {
        m_CrimeProducerHandle = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle,
        m_Results = this.m_Results
      }.Schedule<PoliceInfoviewUISystem.CrimeProducerJob>(this.m_CrimeProducerQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PoliceStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Occupant_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new PoliceInfoviewUISystem.PoliceStationJob()
      {
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OccupantHandle = this.__TypeHandle.__Game_Buildings_Occupant_RO_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PoliceStationDataFromEntity = this.__TypeHandle.__Game_Prefabs_PoliceStationData_RO_ComponentLookup,
        m_InstalledUpgradesFromEntity = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_Results = this.m_Results
      }.Schedule<PoliceInfoviewUISystem.PoliceStationJob>(this.m_PoliceStationQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrisonData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Occupant_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new PoliceInfoviewUISystem.PrisonJob()
      {
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OccupantHandle = this.__TypeHandle.__Game_Buildings_Occupant_RO_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrisonDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrisonData_RO_ComponentLookup,
        m_InstalledUpgradesFromEntity = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_Results = this.m_Results
      }.Schedule<PoliceInfoviewUISystem.PrisonJob>(this.m_PrisonQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Criminal_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new PoliceInfoviewUISystem.CriminalJob()
      {
        m_CriminalHandle = this.__TypeHandle.__Game_Citizens_Criminal_RO_ComponentTypeHandle,
        m_Results = this.m_Results
      }.Schedule<PoliceInfoviewUISystem.CriminalJob>(this.m_CriminalQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrisonCapacity.Update((int) this.m_Results[6]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Prisoners.Update((int) this.m_Results[5]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_InPrison.Update((int) this.m_Results[7]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_JailCapacity.Update((int) this.m_Results[3]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ArrestedCriminals.Update((int) this.m_Results[2]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_InJail.Update((int) this.m_Results[4]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeProducers.Update((int) this.m_Results[0]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeProbability.Update(this.m_Results[1]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Criminals.Update(this.m_CriminalQuery.CalculateEntityCount());
      // ISSUE: reference to a compiler-generated field
      this.m_AverageCrimeProbability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_JailAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_PrisonAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CrimePerMonth.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.CrimeCount, 0));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      this.m_EscapedRate.Update(this.m_CrimePerMonth.value == 0 ? 0.0f : math.min(100f, (float) this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.EscapedArrestCount, 0) * 100f / (float) this.m_CrimePerMonth.value));
    }

    private void ResetResults()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0.0f;
      }
    }

    private IndicatorValue GetCrimeProbability()
    {
      // ISSUE: reference to a compiler-generated field
      float num1 = this.m_CrimeProbability.value;
      // ISSUE: reference to a compiler-generated field
      int num2 = this.m_CrimeProducers.value;
      // ISSUE: reference to a compiler-generated field
      return new IndicatorValue(0.0f, this.__query_632591896_0.GetSingleton<PoliceConfigurationData>().m_MaxCrimeAccumulation, num2 > 0 ? num1 / (float) num2 : 0.0f);
    }

    private IndicatorValue GetJailAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_JailCapacity.value, (float) this.m_ArrestedCriminals.value);
    }

    private IndicatorValue GetPrisonAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_PrisonCapacity.value, (float) this.m_Prisoners.value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_632591896_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PoliceConfigurationData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public PoliceInfoviewUISystem()
    {
    }

    private enum Result
    {
      CrimeProducerCount,
      CrimeProbability,
      ArrestedCriminals,
      JailCapacity,
      InJail,
      Prisoners,
      PrisonCapacity,
      InPrison,
      Count,
    }

    [BurstCompile]
    private struct PoliceStationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public BufferTypeHandle<Occupant> m_OccupantHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<PoliceStationData> m_PoliceStationDataFromEntity;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgradesFromEntity;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Occupant> bufferAccessor1 = chunk.GetBufferAccessor<Occupant>(ref this.m_OccupantHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_PrefabRefFromEntity[entity].m_Prefab;
          if ((double) BuildingUtils.GetEfficiency(bufferAccessor2, index) != 0.0)
          {
            PoliceStationData componentData;
            // ISSUE: reference to a compiler-generated field
            this.m_PoliceStationDataFromEntity.TryGetComponent(prefab, out componentData);
            DynamicBuffer<InstalledUpgrade> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InstalledUpgradesFromEntity.TryGetBuffer(entity, out bufferData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<PoliceStationData>(ref componentData, bufferData, ref this.m_PrefabRefFromEntity, ref this.m_PoliceStationDataFromEntity);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_Results[3] += (float) componentData.m_JailCapacity;
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Occupant>(ref this.m_OccupantHandle))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Results[4] += (float) bufferAccessor1[index].Length;
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

    [BurstCompile]
    private struct PrisonJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public BufferTypeHandle<Occupant> m_OccupantHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<PrisonData> m_PrisonDataFromEntity;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgradesFromEntity;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Occupant> bufferAccessor1 = chunk.GetBufferAccessor<Occupant>(ref this.m_OccupantHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_PrefabRefFromEntity[entity].m_Prefab;
          if ((double) BuildingUtils.GetEfficiency(bufferAccessor2, index) != 0.0)
          {
            PrisonData componentData;
            // ISSUE: reference to a compiler-generated field
            this.m_PrisonDataFromEntity.TryGetComponent(prefab, out componentData);
            DynamicBuffer<InstalledUpgrade> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InstalledUpgradesFromEntity.TryGetBuffer(entity, out bufferData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<PrisonData>(ref componentData, bufferData, ref this.m_PrefabRefFromEntity, ref this.m_PrisonDataFromEntity);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_Results[6] += (float) componentData.m_PrisonerCapacity;
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Occupant>(ref this.m_OccupantHandle))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Results[7] += (float) bufferAccessor1[index].Length;
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

    [BurstCompile]
    private struct CrimeProducerJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<CrimeProducer> m_CrimeProducerHandle;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CrimeProducer> nativeArray = chunk.GetNativeArray<CrimeProducer>(ref this.m_CrimeProducerHandle);
        for (int index = 0; index < chunk.Count; ++index)
        {
          CrimeProducer crimeProducer = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          ++this.m_Results[0];
          // ISSUE: reference to a compiler-generated field
          this.m_Results[1] += crimeProducer.m_Crime;
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
    private struct CriminalJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Criminal> m_CriminalHandle;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Criminal> nativeArray = chunk.GetNativeArray<Criminal>(ref this.m_CriminalHandle);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Criminal criminal = nativeArray[index];
          if ((criminal.m_Flags & (CriminalFlags.Prisoner | CriminalFlags.Sentenced)) != (CriminalFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            ++this.m_Results[5];
          }
          else if ((criminal.m_Flags & CriminalFlags.Arrested) != (CriminalFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            ++this.m_Results[2];
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
      public ComponentTypeHandle<CrimeProducer> __Game_Buildings_CrimeProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Occupant> __Game_Buildings_Occupant_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PoliceStationData> __Game_Prefabs_PoliceStationData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrisonData> __Game_Prefabs_PrisonData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Criminal> __Game_Citizens_Criminal_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CrimeProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Occupant_RO_BufferTypeHandle = state.GetBufferTypeHandle<Occupant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PoliceStationData_RO_ComponentLookup = state.GetComponentLookup<PoliceStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrisonData_RO_ComponentLookup = state.GetComponentLookup<PrisonData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Criminal_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Criminal>(true);
      }
    }
  }
}
