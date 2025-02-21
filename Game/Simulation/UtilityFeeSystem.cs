// Decompiled with JetBrains decompiler
// Type: Game.Simulation.UtilityFeeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Tools;
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
  public class UtilityFeeSystem : GameSystemBase
  {
    private const int kUpdatesPerDay = 128;
    private SimulationSystem m_SimulationSystem;
    private CitySystem m_CitySystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ServiceFeeSystem m_ServiceFeeSystem;
    private EntityQuery m_ConsumerGroup;
    private UtilityFeeSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeSystem = this.World.GetOrCreateSystemManaged<ServiceFeeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConsumerGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Renter>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<ElectricityConsumer>(),
          ComponentType.ReadOnly<WaterConsumer>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 128, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps1;
      JobHandle deps2;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UtilityFeeSystem.SellUtilitiesJob jobData = new UtilityFeeSystem.SellUtilitiesJob()
      {
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_ElectricityConsumerType = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle,
        m_WaterConsumerType = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.GetSharedComponentTypeHandle<UpdateFrame>(),
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_Fees = this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup,
        m_City = this.m_CitySystem.City,
        m_StatQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps1).AsParallelWriter(),
        m_UpdateFrameIndex = updateFrame,
        m_FeeQueue = this.m_ServiceFeeSystem.GetFeeQueue(out deps2).AsParallelWriter(),
        m_RandomSeed = RandomSeed.Next()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<UtilityFeeSystem.SellUtilitiesJob>(this.m_ConsumerGroup, JobHandle.CombineDependencies(this.Dependency, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ServiceFeeSystem.AddQueueWriter(this.Dependency);
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
    public UtilityFeeSystem()
    {
    }

    [BurstCompile]
    private struct SellUtilitiesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> m_ElectricityConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> m_WaterConsumerType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Resources> m_Resources;
      [ReadOnly]
      public BufferLookup<ServiceFee> m_Fees;
      public Entity m_City;
      public uint m_UpdateFrameIndex;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatQueue;
      public NativeQueue<ServiceFeeSystem.FeeEvent>.ParallelWriter m_FeeQueue;
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(1 + unfilteredChunkIndex);
        int num1 = (int) random.NextUInt();
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityConsumer> nativeArray1 = chunk.GetNativeArray<ElectricityConsumer>(ref this.m_ElectricityConsumerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterConsumer> nativeArray2 = chunk.GetNativeArray<WaterConsumer>(ref this.m_WaterConsumerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        bool flag1 = nativeArray1.Length != 0;
        bool flag2 = nativeArray2.Length != 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float fee1 = ServiceFeeSystem.GetFee(PlayerResource.Water, this.m_Fees[this.m_City]);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float fee2 = ServiceFeeSystem.GetFee(PlayerResource.Water, this.m_Fees[this.m_City]);
        float3 float3_1 = new float3(fee1, fee2, fee2);
        float3 float3_2 = (float3) 0;
        for (int index = 0; index < chunk.Count; ++index)
        {
          DynamicBuffer<Renter> dynamicBuffer = bufferAccessor[index];
          if (dynamicBuffer.Length > 0)
          {
            float3 float3_3 = new float3(flag1 ? (float) nativeArray1[index].m_FulfilledConsumption : 0.0f, flag2 ? (float) nativeArray2[index].m_FulfilledFresh : 0.0f, flag2 ? (float) nativeArray2[index].m_FulfilledSewage : 0.0f);
            float3_3 /= 128f;
            float3_2 += float3_3;
            float num2 = math.csum(float3_3 * float3_1) / (float) dynamicBuffer.Length;
            foreach (Renter renter in dynamicBuffer)
            {
              DynamicBuffer<Resources> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_Resources.TryGetBuffer((Entity) renter, out bufferData))
                EconomyUtils.AddResources(Resource.Money, -MathUtils.RoundToIntRandom(ref random, num2), bufferData);
            }
          }
        }
        float3 float3_4 = float3_2 * float3_1;
        // ISSUE: reference to a compiler-generated method
        this.EnqueueFeeEvent(PlayerResource.Electricity, float3_2.x, float3_4.x);
        // ISSUE: reference to a compiler-generated method
        this.EnqueueFeeEvent(PlayerResource.Water, float3_2.y, float3_4.y);
        // ISSUE: reference to a compiler-generated method
        this.EnqueueFeeEvent(PlayerResource.Sewage, float3_2.z, float3_4.z);
        // ISSUE: reference to a compiler-generated method
        this.EnqueueStatIncomeEvent(IncomeSource.FeeElectricity, float3_4.x);
        // ISSUE: reference to a compiler-generated method
        this.EnqueueStatIncomeEvent(IncomeSource.FeeWater, float3_4.y + float3_4.z);
      }

      private void EnqueueFeeEvent(PlayerResource resource, float totalSold, float totalIncome)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_FeeQueue.Enqueue(new ServiceFeeSystem.FeeEvent()
        {
          m_Resource = resource,
          m_Amount = totalSold,
          m_Cost = totalIncome,
          m_Outside = false
        });
      }

      private void EnqueueStatIncomeEvent(IncomeSource source, float totalIncome)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StatQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Income,
          m_Change = totalIncome,
          m_Parameter = (int) source
        });
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
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentTypeHandle;
      public BufferLookup<Resources> __Game_Economy_Resources_RW_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceFee> __Game_City_ServiceFee_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_ServiceFee_RO_BufferLookup = state.GetBufferLookup<ServiceFee>(true);
      }
    }
  }
}
