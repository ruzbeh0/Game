// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PollutionInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PollutionInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "pollutionInfo";
    private AirPollutionSystem m_AirPollutionSystem;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    protected CitySystem m_CitySystem;
    private ValueBinding<IndicatorValue> m_AverageGroundPollution;
    private ValueBinding<IndicatorValue> m_AverageWaterPollution;
    private ValueBinding<IndicatorValue> m_AverageAirPollution;
    private ValueBinding<IndicatorValue> m_AverageNoisePollution;
    private EntityQuery m_HouseholdQuery;
    private NativeArray<int> m_Results;
    private PollutionInfoviewUISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_374463591_0;

    public override GameMode gameMode => GameMode.GameOrEditor;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(8, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdQuery = this.GetEntityQuery(ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<Household>(), ComponentType.ReadOnly<HouseholdCitizen>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<PropertySeeker>(), ComponentType.Exclude<TouristHousehold>(), ComponentType.Exclude<CommuterHousehold>(), ComponentType.Exclude<MovingAway>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageGroundPollution = new ValueBinding<IndicatorValue>("pollutionInfo", "averageGroundPollution", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageWaterPollution = new ValueBinding<IndicatorValue>("pollutionInfo", "averageWaterPollution", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageAirPollution = new ValueBinding<IndicatorValue>("pollutionInfo", "averageAirPollution", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageNoisePollution = new ValueBinding<IndicatorValue>("pollutionInfo", "averageNoisePollution", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      this.RequireForUpdate<CitizenHappinessParameterData>();
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_AverageAirPollution.active || this.m_AverageGroundPollution.active || this.m_AverageNoisePollution.active || this.m_AverageWaterPollution.active;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override void PerformUpdate()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      NativeArray<GroundPollution> map1 = this.m_GroundPollutionSystem.GetMap(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      NativeArray<AirPollution> map2 = this.m_AirPollutionSystem.GetMap(true, out dependencies2);
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      NativeArray<NoisePollution> map3 = this.m_NoisePollutionSystem.GetMap(true, out dependencies3);
      JobHandle job0 = JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3);
      // ISSUE: reference to a compiler-generated field
      CitizenHappinessParameterData singleton = this.__query_374463591_0.GetSingleton<CitizenHappinessParameterData>();
      // ISSUE: reference to a compiler-generated method
      this.ResetResults();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      new PollutionInfoviewUISystem.CalculateAveragePollutionJob()
      {
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_WaterConsumerFromEntity = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
        m_TransformFromEntity = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_AirPollutionMap = map2,
        m_NoisePollutionMap = map3,
        m_GroundPollutionMap = map1,
        m_HappinessParameters = singleton,
        m_City = this.m_CitySystem.City,
        m_Results = this.m_Results
      }.Schedule<PollutionInfoviewUISystem.CalculateAveragePollutionJob>(this.m_HouseholdQuery, JobHandle.CombineDependencies(job0, this.Dependency)).Complete();
      // ISSUE: reference to a compiler-generated field
      int result1 = this.m_Results[0];
      // ISSUE: reference to a compiler-generated field
      int result2 = this.m_Results[4];
      // ISSUE: reference to a compiler-generated field
      int result3 = this.m_Results[2];
      // ISSUE: reference to a compiler-generated field
      int result4 = this.m_Results[1];
      // ISSUE: reference to a compiler-generated field
      int result5 = this.m_Results[5];
      // ISSUE: reference to a compiler-generated field
      int result6 = this.m_Results[3];
      // ISSUE: reference to a compiler-generated field
      int result7 = this.m_Results[6];
      // ISSUE: reference to a compiler-generated field
      int result8 = this.m_Results[7];
      int num1 = result4 > 0 ? result1 / result4 : 0;
      int num2 = result6 > 0 ? result3 / result6 : 0;
      int num3 = result5 > 0 ? result2 / result5 : 0;
      // ISSUE: reference to a compiler-generated field
      this.m_AverageGroundPollution.Update(new IndicatorValue(0.0f, (float) singleton.m_MaxAirAndGroundPollutionBonus, (float) -num1));
      // ISSUE: reference to a compiler-generated field
      this.m_AverageAirPollution.Update(new IndicatorValue(0.0f, (float) singleton.m_MaxAirAndGroundPollutionBonus, (float) -num2));
      // ISSUE: reference to a compiler-generated field
      this.m_AverageNoisePollution.Update(new IndicatorValue(0.0f, (float) singleton.m_MaxNoisePollutionBonus, (float) -num3));
      // ISSUE: reference to a compiler-generated field
      this.m_AverageWaterPollution.Update(new IndicatorValue(0.0f, (float) result7, (float) result8));
    }

    private void ResetResults()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_374463591_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<CitizenHappinessParameterData>()
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
    public PollutionInfoviewUISystem()
    {
    }

    private enum Result
    {
      GroundPollution,
      GroundPollutionCount,
      AirPollution,
      AirPollutionCount,
      NoisePollution,
      NoisePollutionCount,
      ConsumedWater,
      PollutedWater,
      ResultCount,
    }

    [BurstCompile]
    private struct CalculateAveragePollutionJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumerFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformFromEntity;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      [ReadOnly]
      public NativeArray<NoisePollution> m_NoisePollutionMap;
      [ReadOnly]
      public NativeArray<GroundPollution> m_GroundPollutionMap;
      [ReadOnly]
      public Entity m_City;
      public CitizenHappinessParameterData m_HappinessParameters;
      public NativeArray<int> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        float f = 0.0f;
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity property = nativeArray[index].m_Property;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          int2 pollutionBonuses1 = CitizenHappinessSystem.GetAirPollutionBonuses(property, ref this.m_TransformFromEntity, this.m_AirPollutionMap, cityModifier, in this.m_HappinessParameters);
          num3 += pollutionBonuses1.x + pollutionBonuses1.y;
          ++num6;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          int2 pollutionBonuses2 = CitizenHappinessSystem.GetGroundPollutionBonuses(property, ref this.m_TransformFromEntity, this.m_GroundPollutionMap, cityModifier, in this.m_HappinessParameters);
          num1 += pollutionBonuses2.x + pollutionBonuses2.y;
          ++num4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          int2 noiseBonuses = CitizenHappinessSystem.GetNoiseBonuses(property, ref this.m_TransformFromEntity, this.m_NoisePollutionMap, in this.m_HappinessParameters);
          num2 += noiseBonuses.x + noiseBonuses.y;
          ++num5;
          WaterConsumer componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_WaterConsumerFromEntity.TryGetComponent(property, out componentData))
          {
            num7 += componentData.m_FulfilledFresh;
            f += componentData.m_Pollution * (float) componentData.m_FulfilledFresh;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += num4;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += num3;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[3] += num6;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[4] += num2;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[5] += num5;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[6] += num7;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[7] += Mathf.RoundToInt(f);
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
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
