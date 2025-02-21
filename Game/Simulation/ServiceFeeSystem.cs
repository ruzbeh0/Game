// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ServiceFeeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Serialization;
using Game.Tools;
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
  public class ServiceFeeSystem : 
    GameSystemBase,
    IServiceFeeSystem,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    private const int kUpdatesPerDay = 128;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CitySystem m_CitySystem;
    private TriggerSystem m_TriggerSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_FeeCollectorGroup;
    private EntityQuery m_CollectedFeeGroup;
    private NativeQueue<ServiceFeeSystem.FeeEvent> m_FeeQueue;
    private NativeList<CollectedCityServiceFeeData> m_CityServiceFees;
    private JobHandle m_Writers;
    private ServiceFeeSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 2048;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CollectedFeeGroup = this.GetEntityQuery(ComponentType.ReadOnly<CollectedCityServiceFeeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FeeCollectorGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.City.ServiceFeeCollector>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Patient>(),
          ComponentType.ReadOnly<Game.Buildings.Student>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CollectedFeeGroup);
      // ISSUE: reference to a compiler-generated field
      this.m_FeeQueue = new NativeQueue<ServiceFeeSystem.FeeEvent>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceFees = new NativeList<CollectedCityServiceFeeData>(13, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (context.purpose == Colossal.Serialization.Entities.Purpose.NewGame)
      {
        // ISSUE: reference to a compiler-generated method
        this.CacheFees(true);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.CacheFees();
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_FeeQueue.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceFees.Dispose();
      base.OnDestroy();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      // ISSUE: reference to a compiler-generated field
      NativeArray<ServiceFeeSystem.FeeEvent> array = this.m_FeeQueue.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      writer.Write(array.Length);
      foreach (ServiceFeeSystem.FeeEvent feeEvent in array)
        writer.Write<ServiceFeeSystem.FeeEvent>(feeEvent);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_FeeQueue.Clear();
      int num;
      reader.Read(out num);
      for (int index = 0; index < num; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        ServiceFeeSystem.FeeEvent feeEvent;
        reader.Read<ServiceFeeSystem.FeeEvent>(out feeEvent);
        // ISSUE: reference to a compiler-generated field
        this.m_FeeQueue.Enqueue(feeEvent);
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_FeeQueue.Clear();
    }

    public NativeList<CollectedCityServiceFeeData> GetServiceFees() => this.m_CityServiceFees;

    public NativeQueue<ServiceFeeSystem.FeeEvent> GetFeeQueue(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_Writers;
      // ISSUE: reference to a compiler-generated field
      return this.m_FeeQueue;
    }

    public void AddQueueWriter(JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Writers = JobHandle.CombineDependencies(this.m_Writers, deps);
    }

    public int3 GetServiceFees(PlayerResource resource)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return ServiceFeeSystem.GetServiceFees(resource, this.m_CityServiceFees);
    }

    public int GetServiceFeeIncomeEstimate(PlayerResource resource, float fee)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return ServiceFeeSystem.GetServiceFeeIncomeEstimate(resource, fee, this.m_CityServiceFees);
    }

    public static PlayerResource GetEducationResource(int level)
    {
      switch (level)
      {
        case 1:
          return PlayerResource.BasicEducation;
        case 2:
          return PlayerResource.SecondaryEducation;
        case 3:
        case 4:
          return PlayerResource.HigherEducation;
        default:
          return PlayerResource.Count;
      }
    }

    public static float GetFee(PlayerResource resource, DynamicBuffer<ServiceFee> fees)
    {
      for (int index = 0; index < fees.Length; ++index)
      {
        ServiceFee fee = fees[index];
        if (fee.m_Resource == resource)
          return fee.m_Fee;
      }
      return 0.0f;
    }

    public static bool TryGetFee(
      PlayerResource resource,
      DynamicBuffer<ServiceFee> fees,
      out float fee)
    {
      for (int index = 0; index < fees.Length; ++index)
      {
        ServiceFee fee1 = fees[index];
        if (fee1.m_Resource == resource)
        {
          fee = fee1.m_Fee;
          return true;
        }
      }
      fee = 0.0f;
      return false;
    }

    public static void SetFee(PlayerResource resource, DynamicBuffer<ServiceFee> fees, float value)
    {
      for (int index = 0; index < fees.Length; ++index)
      {
        ServiceFee fee = fees[index];
        if (fee.m_Resource == resource)
        {
          fee.m_Fee = value;
          fees[index] = fee;
          return;
        }
      }
      fees.Add(new ServiceFee()
      {
        m_Fee = value,
        m_Resource = resource
      });
    }

    public static float GetConsumptionMultiplier(
      PlayerResource resource,
      float relativeFee,
      in ServiceFeeParameterData feeParameters)
    {
      if (resource == PlayerResource.Electricity)
      {
        // ISSUE: reference to a compiler-generated method
        return AdjustElectricityConsumptionSystem.GetFeeConsumptionMultiplier(relativeFee, in feeParameters);
      }
      // ISSUE: reference to a compiler-generated method
      return resource == PlayerResource.Water ? AdjustWaterConsumptionSystem.GetFeeConsumptionMultiplier(relativeFee, in feeParameters) : 1f;
    }

    public static float GetEfficiencyMultiplier(
      PlayerResource resource,
      float relativeFee,
      in BuildingEfficiencyParameterData efficiencyParameters)
    {
      if (resource == PlayerResource.Electricity)
      {
        // ISSUE: reference to a compiler-generated method
        return AdjustElectricityConsumptionSystem.GetFeeEfficiencyFactor(relativeFee, in efficiencyParameters);
      }
      // ISSUE: reference to a compiler-generated method
      return resource == PlayerResource.Water ? AdjustWaterConsumptionSystem.GetFeeEfficiencyFactor(relativeFee, in efficiencyParameters) : 1f;
    }

    public static int GetHappinessEffect(
      PlayerResource resource,
      float relativeFee,
      in CitizenHappinessParameterData happinessParameters)
    {
      if (resource == PlayerResource.Electricity)
      {
        // ISSUE: reference to a compiler-generated method
        return CitizenHappinessSystem.GetElectricityFeeHappinessEffect(relativeFee, in happinessParameters);
      }
      // ISSUE: reference to a compiler-generated method
      return resource == PlayerResource.Water ? CitizenHappinessSystem.GetWaterFeeHappinessEffect(relativeFee, in happinessParameters) : 1;
    }

    public static int3 GetServiceFees(
      PlayerResource resource,
      NativeList<CollectedCityServiceFeeData> fees)
    {
      float3 x = new float3();
      foreach (CollectedCityServiceFeeData fee in fees)
      {
        if ((PlayerResource) fee.m_PlayerResource == resource)
          x += new float3(fee.m_Internal, fee.m_Export, fee.m_Import);
      }
      return new int3(math.round(x));
    }

    public static int GetServiceFeeIncomeEstimate(
      PlayerResource resource,
      float fee,
      NativeList<CollectedCityServiceFeeData> fees)
    {
      float x = 0.0f;
      foreach (CollectedCityServiceFeeData fee1 in fees)
      {
        if ((PlayerResource) fee1.m_PlayerResource == resource)
          x += fee1.m_InternalCount * fee;
      }
      return (int) math.round(x);
    }

    private void CacheFees(bool reset = false)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_CollectedFeeGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceFees.Clear();
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        DynamicBuffer<CollectedCityServiceFeeData> buffer = this.EntityManager.GetBuffer<CollectedCityServiceFeeData>(entityArray[index1], !reset);
        for (int index2 = 0; index2 < buffer.Length; ++index2)
        {
          if (reset)
          {
            CollectedCityServiceFeeData cityServiceFeeData = new CollectedCityServiceFeeData()
            {
              m_PlayerResource = buffer[index2].m_PlayerResource
            };
            buffer[index2] = cityServiceFeeData;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CityServiceFees.Add(buffer[index2]);
        }
      }
      entityArray.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.CacheFees();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Student_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Patient_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ServiceFeeSystem.PayFeeJob jobData1 = new ServiceFeeSystem.PayFeeJob()
      {
        m_PatientType = this.__TypeHandle.__Game_Buildings_Patient_RO_BufferTypeHandle,
        m_StudentType = this.__TypeHandle.__Game_Buildings_Student_RO_BufferTypeHandle,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_Students = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup,
        m_Fees = this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_FeeEvents = this.m_FeeQueue,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter(),
        m_City = this.m_CitySystem.City
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.Schedule<ServiceFeeSystem.PayFeeJob>(this.m_FeeCollectorGroup, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceFeeData_RW_BufferLookup.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ServiceFeeSystem.FeeToCityJob jobData2 = new ServiceFeeSystem.FeeToCityJob()
      {
        m_FeeDatas = this.__TypeHandle.__Game_Simulation_CollectedCityServiceFeeData_RW_BufferLookup,
        m_FeeDataEntities = this.m_CollectedFeeGroup.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_FeeEvents = this.m_FeeQueue,
        m_City = this.m_CitySystem.City
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData2.Schedule<ServiceFeeSystem.FeeToCityJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, this.m_Writers));
      // ISSUE: reference to a compiler-generated field
      this.m_Writers = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceFeeData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ServiceFeeSystem.TriggerJob jobData3 = new ServiceFeeSystem.TriggerJob()
      {
        m_Entities = this.m_CollectedFeeGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_FeeDatas = this.__TypeHandle.__Game_Simulation_CollectedCityServiceFeeData_RO_BufferLookup,
        m_ActionQueue = this.m_TriggerSystem.CreateActionBuffer()
      };
      this.Dependency = jobData3.Schedule<ServiceFeeSystem.TriggerJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
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
    public ServiceFeeSystem()
    {
    }

    public struct FeeEvent : ISerializable
    {
      public PlayerResource m_Resource;
      public float m_Amount;
      public float m_Cost;
      public bool m_Outside;

      public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write((int) this.m_Resource);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_Amount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_Cost);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_Outside);
      }

      public void Deserialize<TReader>(TReader reader) where TReader : IReader
      {
        int num;
        reader.Read(out num);
        // ISSUE: reference to a compiler-generated field
        this.m_Resource = (PlayerResource) num;
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_Amount);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_Cost);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_Outside);
      }
    }

    [BurstCompile]
    private struct PayFeeJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<Patient> m_PatientType;
      [ReadOnly]
      public BufferTypeHandle<Game.Buildings.Student> m_StudentType;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> m_Students;
      [ReadOnly]
      public BufferLookup<ServiceFee> m_Fees;
      public BufferLookup<Resources> m_Resources;
      public NativeQueue<ServiceFeeSystem.FeeEvent> m_FeeEvents;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;
      public Entity m_City;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Patient> bufferAccessor1 = chunk.GetBufferAccessor<Patient>(ref this.m_PatientType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Buildings.Student> bufferAccessor2 = chunk.GetBufferAccessor<Game.Buildings.Student>(ref this.m_StudentType);
        if (bufferAccessor1.Length != 0)
        {
          for (int index1 = 0; index1 < chunk.Count; ++index1)
          {
            DynamicBuffer<Patient> dynamicBuffer = bufferAccessor1[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated method
              this.PayFee(dynamicBuffer[index2].m_Patient, PlayerResource.Healthcare);
            }
          }
        }
        if (bufferAccessor2.Length == 0)
          return;
        for (int index3 = 0; index3 < chunk.Count; ++index3)
        {
          DynamicBuffer<Game.Buildings.Student> dynamicBuffer = bufferAccessor2[index3];
          for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
          {
            Entity student = dynamicBuffer[index4].m_Student;
            Game.Citizens.Student componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Students.TryGetComponent(student, out componentData))
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              this.PayFee(student, ServiceFeeSystem.GetEducationResource((int) componentData.m_Level));
            }
          }
        }
      }

      private void PayFee(Entity citizen, PlayerResource resource)
      {
        HouseholdMember componentData;
        DynamicBuffer<Resources> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HouseholdMembers.TryGetComponent(citizen, out componentData) || !this.m_Resources.TryGetBuffer(componentData.m_Household, out bufferData))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float x = ServiceFeeSystem.GetFee(resource, this.m_Fees[this.m_City]) / 128f;
        float num = 1f / 128f;
        EconomyUtils.AddResources(Resource.Money, (int) -(double) math.round(x), bufferData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_FeeEvents.Enqueue(new ServiceFeeSystem.FeeEvent()
        {
          m_Resource = resource,
          m_Amount = num,
          m_Cost = x,
          m_Outside = false
        });
        IncomeSource incomeSource = EconomyUtils.GetIncomeSource(resource);
        if (incomeSource == IncomeSource.Count)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Income,
          m_Change = x,
          m_Parameter = (int) incomeSource
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

    [BurstCompile]
    private struct FeeToCityJob : IJob
    {
      public BufferLookup<CollectedCityServiceFeeData> m_FeeDatas;
      [ReadOnly]
      public NativeList<Entity> m_FeeDataEntities;
      public NativeQueue<ServiceFeeSystem.FeeEvent> m_FeeEvents;
      public Entity m_City;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_FeeDataEntities.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CollectedCityServiceFeeData> feeData = this.m_FeeDatas[this.m_FeeDataEntities[index1]];
          for (int index2 = 0; index2 < feeData.Length; ++index2)
          {
            CollectedCityServiceFeeData cityServiceFeeData = feeData[index2] with
            {
              m_Export = 0.0f,
              m_ExportCount = 0.0f,
              m_Import = 0.0f,
              m_ImportCount = 0.0f,
              m_Internal = 0.0f,
              m_InternalCount = 0.0f
            };
            feeData[index2] = cityServiceFeeData;
          }
        }
        // ISSUE: variable of a compiler-generated type
        ServiceFeeSystem.FeeEvent feeEvent;
        // ISSUE: reference to a compiler-generated field
        while (this.m_FeeEvents.TryDequeue(out feeEvent))
        {
          // ISSUE: reference to a compiler-generated field
          for (int index3 = 0; index3 < this.m_FeeDataEntities.Length; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CollectedCityServiceFeeData> feeData = this.m_FeeDatas[this.m_FeeDataEntities[index3]];
            for (int index4 = 0; index4 < feeData.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated field
              if ((PlayerResource) feeData[index4].m_PlayerResource == feeEvent.m_Resource)
              {
                CollectedCityServiceFeeData cityServiceFeeData = feeData[index4];
                // ISSUE: reference to a compiler-generated field
                if ((double) feeEvent.m_Amount > 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (feeEvent.m_Outside)
                  {
                    // ISSUE: reference to a compiler-generated field
                    cityServiceFeeData.m_Export += feeEvent.m_Cost * 128f;
                    // ISSUE: reference to a compiler-generated field
                    cityServiceFeeData.m_ExportCount += feeEvent.m_Amount * 128f;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    cityServiceFeeData.m_Internal += feeEvent.m_Cost * 128f;
                    // ISSUE: reference to a compiler-generated field
                    cityServiceFeeData.m_InternalCount += feeEvent.m_Amount * 128f;
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  cityServiceFeeData.m_Import += feeEvent.m_Cost * 128f;
                  // ISSUE: reference to a compiler-generated field
                  cityServiceFeeData.m_ImportCount += (float) (-(double) feeEvent.m_Amount * 128.0);
                }
                feeData[index4] = cityServiceFeeData;
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct TriggerJob : IJob
    {
      [ReadOnly]
      [DeallocateOnJobCompletion]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public BufferLookup<CollectedCityServiceFeeData> m_FeeDatas;
      public NativeQueue<TriggerAction> m_ActionQueue;

      public void Execute()
      {
        float num = 0.0f;
        NativeArray<float> nativeArray = new NativeArray<float>(13, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Entities.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CollectedCityServiceFeeData> feeData = this.m_FeeDatas[this.m_Entities[index1]];
          for (int index2 = 0; index2 < feeData.Length; ++index2)
          {
            num += feeData[index2].m_Export - feeData[index2].m_Import;
            nativeArray[feeData[index2].m_PlayerResource] += feeData[index2].m_Export - feeData[index2].m_Import;
          }
        }
        for (int index = 0; index < 13; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.SendTradeResourceTrigger((PlayerResource) index, nativeArray[index]);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ActionQueue.Enqueue(new TriggerAction(TriggerType.ServiceTradeBalance, Entity.Null, num));
      }

      private void SendTradeResourceTrigger(PlayerResource resource, float total)
      {
        bool flag = true;
        TriggerType triggerType = TriggerType.NewNotification;
        switch (resource)
        {
          case PlayerResource.Electricity:
            triggerType = TriggerType.CityServiceElectricity;
            break;
          case PlayerResource.Healthcare:
            triggerType = TriggerType.CityServiceHealthcare;
            break;
          case PlayerResource.BasicEducation:
          case PlayerResource.SecondaryEducation:
          case PlayerResource.HigherEducation:
            triggerType = TriggerType.CityServiceEducation;
            break;
          case PlayerResource.Garbage:
            triggerType = TriggerType.CityServiceGarbage;
            break;
          case PlayerResource.Water:
          case PlayerResource.Sewage:
            triggerType = TriggerType.CityServiceWaterSewage;
            break;
          case PlayerResource.Mail:
            triggerType = TriggerType.CityServicePost;
            break;
          case PlayerResource.FireResponse:
            triggerType = TriggerType.CityServiceFireAndRescue;
            break;
          case PlayerResource.Police:
            triggerType = TriggerType.CityServicePolice;
            break;
          default:
            flag = false;
            break;
        }
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ActionQueue.Enqueue(new TriggerAction(triggerType, Entity.Null, total));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<Patient> __Game_Buildings_Patient_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Buildings.Student> __Game_Buildings_Student_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceFee> __Game_City_ServiceFee_RO_BufferLookup;
      public BufferLookup<Resources> __Game_Economy_Resources_RW_BufferLookup;
      public BufferLookup<CollectedCityServiceFeeData> __Game_Simulation_CollectedCityServiceFeeData_RW_BufferLookup;
      [ReadOnly]
      public BufferLookup<CollectedCityServiceFeeData> __Game_Simulation_CollectedCityServiceFeeData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Patient_RO_BufferTypeHandle = state.GetBufferTypeHandle<Patient>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Student_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Buildings.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentLookup = state.GetComponentLookup<Game.Citizens.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_ServiceFee_RO_BufferLookup = state.GetBufferLookup<ServiceFee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceFeeData_RW_BufferLookup = state.GetBufferLookup<CollectedCityServiceFeeData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceFeeData_RO_BufferLookup = state.GetBufferLookup<CollectedCityServiceFeeData>(true);
      }
    }
  }
}
