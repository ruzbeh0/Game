// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PollutionTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Objects;
using Game.Prefabs;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PollutionTriggerSystem : GameSystemBase
  {
    private AirPollutionSystem m_AirPollutionSystem;
    private TriggerSystem m_TriggerSystem;
    private CitySystem m_CitySystem;
    private EntityQuery m_HouseholdQuery;
    private EntityQuery m_HappinessParameterQuery;
    private NativeArray<float> m_AirPollutionResult;
    private PollutionTriggerSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_380796347_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4096;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdQuery = this.GetEntityQuery(ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<Household>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<CommuterHousehold>(), ComponentType.Exclude<MovingAway>());
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionResult = new NativeArray<float>(1, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionResult.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeAccumulator<AverageFloat> nativeAccumulator = new NativeAccumulator<AverageFloat>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      PollutionTriggerSystem.CalculateAverageAirPollutionJob jobData1 = new PollutionTriggerSystem.CalculateAverageAirPollutionJob()
      {
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_Transforms = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_AirPollutionMap = this.m_AirPollutionSystem.GetMap(true, out dependencies),
        m_HappinessParameters = this.__query_380796347_0.GetSingleton<CitizenHappinessParameterData>(),
        m_City = this.m_CitySystem.City,
        m_Result = nativeAccumulator.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<PollutionTriggerSystem.CalculateAverageAirPollutionJob>(this.m_HouseholdQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PollutionTriggerSystem.SendPollutionTriggerJob jobData2 = new PollutionTriggerSystem.SendPollutionTriggerJob()
      {
        m_Result = nativeAccumulator,
        m_TriggerQueue = this.m_TriggerSystem.CreateActionBuffer()
      };
      this.Dependency = jobData2.Schedule<PollutionTriggerSystem.SendPollutionTriggerJob>(this.Dependency);
      nativeAccumulator.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_380796347_0 = state.GetEntityQuery(new EntityQueryDesc()
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
    public PollutionTriggerSystem()
    {
    }

    [BurstCompile]
    private struct CalculateAverageAirPollutionJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      [ReadOnly]
      public ComponentLookup<Transform> m_Transforms;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      public CitizenHappinessParameterData m_HappinessParameters;
      public Entity m_City;
      public NativeAccumulator<AverageFloat>.ParallelWriter m_Result;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          int2 pollutionBonuses = CitizenHappinessSystem.GetAirPollutionBonuses(nativeArray[index].m_Property, ref this.m_Transforms, this.m_AirPollutionMap, cityModifier, in this.m_HappinessParameters);
          // ISSUE: reference to a compiler-generated field
          this.m_Result.Accumulate(new AverageFloat()
          {
            m_Total = (float) math.csum(pollutionBonuses),
            m_Count = 1
          });
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
    private struct SendPollutionTriggerJob : IJob
    {
      public NativeAccumulator<AverageFloat> m_Result;
      public NativeQueue<TriggerAction> m_TriggerQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TriggerQueue.Enqueue(new TriggerAction(TriggerType.AverageAirPollution, Entity.Null, this.m_Result.GetResult().average));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
