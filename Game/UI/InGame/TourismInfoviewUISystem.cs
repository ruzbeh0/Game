// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TourismInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class TourismInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "tourismInfo";
    private ClimateSystem m_ClimateSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CitySystem m_CitySystem;
    private ValueBinding<IndicatorValue> m_Attractiveness;
    private ValueBinding<int> m_TourismRate;
    private ValueBinding<float> m_AverageHotelPrice;
    private ValueBinding<float> m_WeatherEffect;
    private EntityQuery m_HotelQuery;
    private EntityQuery m_HotelModifiedQuery;
    private NativeArray<int> m_Results;
    private TourismInfoviewUISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1647950437_0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Attractiveness = new ValueBinding<IndicatorValue>("tourismInfo", "attractiveness", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TourismRate = new ValueBinding<int>("tourismInfo", "tourismRate", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageHotelPrice = new ValueBinding<float>("tourismInfo", "averageHotelPrice", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_WeatherEffect = new ValueBinding<float>("tourismInfo", "weatherEffect", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.m_HotelQuery = this.GetEntityQuery(ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<LodgingProvider>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_HotelModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PropertyRenter>(),
          ComponentType.ReadOnly<LodgingProvider>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Created>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.RequireForUpdate<AttractivenessParameterData>();
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(2, Allocator.Persistent);
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
        return base.Active || this.m_Attractiveness.active || this.m_TourismRate.active || this.m_AverageHotelPrice.active || this.m_WeatherEffect.active;
      }
    }

    protected override bool Modified => !this.m_HotelModifiedQuery.IsEmptyIgnoreFilter;

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateAttractiveness();
      // ISSUE: reference to a compiler-generated method
      this.UpdateTourismRate();
      // ISSUE: reference to a compiler-generated method
      this.UpdateWeatherEffect();
      // ISSUE: reference to a compiler-generated method
      this.UpdateAverageHotelPrice();
    }

    private void UpdateAttractiveness()
    {
      Tourism component;
      // ISSUE: reference to a compiler-generated field
      if (!this.EntityManager.TryGetComponent<Tourism>(this.m_CitySystem.City, out component))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Attractiveness.Update(new IndicatorValue(0.0f, 100f, (float) component.m_Attractiveness));
    }

    private void UpdateTourismRate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TourismRate.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.TouristCount, 0));
    }

    private void UpdateWeatherEffect()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WeatherEffect.Update((float) (100.0 * -(1.0 - (double) TourismSystem.GetWeatherEffect(this.__query_1647950437_0.GetSingleton<AttractivenessParameterData>(), this.m_ClimateSystem.classification, (float) this.m_ClimateSystem.temperature, (float) this.m_ClimateSystem.precipitation, this.m_ClimateSystem.isRaining, this.m_ClimateSystem.isSnowing))));
    }

    private void UpdateAverageHotelPrice()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_LodgingProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new TourismInfoviewUISystem.CalculateAverageHotelPriceJob()
      {
        m_LodgingProviderHandle = this.__TypeHandle.__Game_Companies_LodgingProvider_RO_ComponentTypeHandle,
        m_Results = this.m_Results
      }.Schedule<TourismInfoviewUISystem.CalculateAverageHotelPriceJob>(this.m_HotelQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      int result = this.m_Results[1];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AverageHotelPrice.Update(result > 0 ? (float) (this.m_Results[0] / result) : 0.0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1647950437_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<AttractivenessParameterData>()
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
    public TourismInfoviewUISystem()
    {
    }

    private enum Result
    {
      Price,
      HotelCount,
      ResultCount,
    }

    [BurstCompile]
    private struct CalculateAverageHotelPriceJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<LodgingProvider> m_LodgingProviderHandle;
      public NativeArray<int> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<LodgingProvider> nativeArray = chunk.GetNativeArray<LodgingProvider>(ref this.m_LodgingProviderHandle);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          LodgingProvider lodgingProvider = nativeArray[index];
          if (lodgingProvider.m_Price > 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Results[0] += lodgingProvider.m_Price;
            // ISSUE: reference to a compiler-generated field
            ++this.m_Results[1];
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
      public ComponentTypeHandle<LodgingProvider> __Game_Companies_LodgingProvider_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_LodgingProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LodgingProvider>(true);
      }
    }
  }
}
