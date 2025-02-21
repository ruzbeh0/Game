// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WeatherHazardSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.City;
using Game.Common;
using Game.Prefabs;
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
  public class WeatherHazardSystem : GameSystemBase
  {
    private const int UPDATES_PER_DAY = 128;
    private ClimateSystem m_ClimateSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_PhenomenonQuery;
    private WeatherHazardSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 2048;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PhenomenonQuery = this.GetEntityQuery(ComponentType.ReadOnly<EventData>(), ComponentType.ReadOnly<WeatherPhenomenonData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PhenomenonQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WeatherHazardSystem.WeatherHazardJob jobData = new WeatherHazardSystem.WeatherHazardJob()
      {
        m_RandomSeed = RandomSeed.Next(),
        m_TimeDelta = 34.1333351f,
        m_Temperature = (float) this.m_ClimateSystem.temperature,
        m_Rain = (float) this.m_ClimateSystem.thunder,
        m_NaturalDisasters = this.m_CityConfigurationSystem.naturalDisasters,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabEventType = this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle,
        m_PrefabWeatherPhenomenonType = this.__TypeHandle.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentTypeHandle,
        m_LockedType = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<WeatherHazardSystem.WeatherHazardJob>(this.m_PhenomenonQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public WeatherHazardSystem()
    {
    }

    [BurstCompile]
    private struct WeatherHazardJob : IJobChunk
    {
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public float m_TimeDelta;
      [ReadOnly]
      public float m_Temperature;
      [ReadOnly]
      public float m_Rain;
      [ReadOnly]
      public bool m_NaturalDisasters;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<EventData> m_PrefabEventType;
      [ReadOnly]
      public ComponentTypeHandle<WeatherPhenomenonData> m_PrefabWeatherPhenomenonType;
      [ReadOnly]
      public ComponentTypeHandle<Locked> m_LockedType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EventData> nativeArray2 = chunk.GetNativeArray<EventData>(ref this.m_PrefabEventType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WeatherPhenomenonData> nativeArray3 = chunk.GetNativeArray<WeatherPhenomenonData>(ref this.m_PrefabWeatherPhenomenonType);
        // ISSUE: reference to a compiler-generated field
        EnabledMask enabledMask = chunk.GetEnabledMask<Locked>(ref this.m_LockedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          if (!enabledMask.EnableBit.IsValid || !enabledMask[index])
          {
            WeatherPhenomenonData weatherPhenomenonData = nativeArray3[index];
            // ISSUE: reference to a compiler-generated field
            if ((double) weatherPhenomenonData.m_DamageSeverity == 0.0 || this.m_NaturalDisasters)
            {
              // ISSUE: reference to a compiler-generated field
              float num1 = (this.m_Temperature - MathUtils.Center(weatherPhenomenonData.m_OccurenceTemperature)) / math.max(0.5f, MathUtils.Extents(weatherPhenomenonData.m_OccurenceTemperature));
              float num2 = math.max(0.0f, (float) (1.0 - (double) num1 * (double) num1));
              float num3 = 1f;
              if ((double) weatherPhenomenonData.m_OccurenceRain.max > 0.99900001287460327)
              {
                if ((double) weatherPhenomenonData.m_OccurenceRain.min >= 1.0 / 1000.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  num3 = math.saturate((this.m_Rain - weatherPhenomenonData.m_OccurenceRain.min) / math.max(1f / 1000f, weatherPhenomenonData.m_OccurenceRain.max - weatherPhenomenonData.m_OccurenceRain.min));
                }
              }
              else if ((double) weatherPhenomenonData.m_OccurenceRain.min < 1.0 / 1000.0)
              {
                // ISSUE: reference to a compiler-generated field
                num3 = math.saturate((weatherPhenomenonData.m_OccurenceRain.max - this.m_Rain) / math.max(1f / 1000f, weatherPhenomenonData.m_OccurenceRain.max - weatherPhenomenonData.m_OccurenceRain.min));
              }
              // ISSUE: reference to a compiler-generated field
              for (float num4 = weatherPhenomenonData.m_OccurenceProbability * num2 * num3 * this.m_TimeDelta; (double) random.NextFloat(100f) < (double) num4; num4 -= 100f)
              {
                Entity eventPrefab = nativeArray1[index];
                EventData eventData = nativeArray2[index];
                // ISSUE: reference to a compiler-generated method
                this.CreateWeatherEvent(unfilteredChunkIndex, eventPrefab, eventData);
              }
            }
          }
        }
      }

      private void CreateWeatherEvent(int jobIndex, Entity eventPrefab, EventData eventData)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, eventData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(eventPrefab));
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
      public ComponentTypeHandle<EventData> __Game_Prefabs_EventData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WeatherPhenomenonData> __Game_Prefabs_WeatherPhenomenonData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WeatherPhenomenonData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>(true);
      }
    }
  }
}
