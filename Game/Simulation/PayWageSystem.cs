// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PayWageSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PayWageSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 32;
    private SimulationSystem m_SimulationSystem;
    private TaxSystem m_TaxSystem;
    private EntityQuery m_EconomyParameterGroup;
    private EntityQuery m_HouseholdGroup;
    private NativeQueue<PayWageSystem.Payment> m_PaymentQueue;
    private PayWageSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (PayWageSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PaymentQueue = new NativeQueue<PayWageSystem.Payment>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdGroup = this.GetEntityQuery(ComponentType.ReadOnly<Household>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<HouseholdCitizen>(), ComponentType.Exclude<TouristHousehold>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PaymentQueue.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, PayWageSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new PayWageSystem.PayWageJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HouseholdCitizenType = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle,
        m_ResourcesType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_TaxPayerType = this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle,
        m_CommuterHouseholdType = this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.GetSharedComponentTypeHandle<UpdateFrame>(),
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup,
        m_Companies = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup,
        m_EmployeeBufs = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_EconomyParameters = this.m_EconomyParameterGroup.GetSingleton<EconomyParameterData>(),
        m_UpdateFrameIndex = updateFrame,
        m_PaymentQueue = this.m_PaymentQueue.AsParallelWriter(),
        m_TaxRates = this.m_TaxSystem.GetTaxRates()
      }.ScheduleParallel<PayWageSystem.PayWageJob>(this.m_HouseholdGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PayWageSystem.PayJob jobData = new PayWageSystem.PayJob()
      {
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_PaymentQueue = this.m_PaymentQueue
      };
      this.Dependency = jobData.Schedule<PayWageSystem.PayJob>(jobHandle);
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
    public PayWageSystem()
    {
    }

    private struct Payment
    {
      public Entity m_Target;
      public int m_Amount;
    }

    [BurstCompile]
    private struct PayJob : IJob
    {
      public NativeQueue<PayWageSystem.Payment> m_PaymentQueue;
      public BufferLookup<Game.Economy.Resources> m_Resources;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        PayWageSystem.Payment payment;
        // ISSUE: reference to a compiler-generated field
        while (this.m_PaymentQueue.TryDequeue(out payment))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Resources.HasBuffer(payment.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EconomyUtils.AddResources(Resource.Money, payment.m_Amount, this.m_Resources[payment.m_Target]);
          }
        }
      }
    }

    [BurstCompile]
    private struct PayWageJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<TaxPayer> m_TaxPayerType;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> m_HouseholdCitizenType;
      public BufferTypeHandle<Game.Economy.Resources> m_ResourcesType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<CommuterHousehold> m_CommuterHouseholdType;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_Companies;
      [ReadOnly]
      public BufferLookup<Employee> m_EmployeeBufs;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      public EconomyParameterData m_EconomyParameters;
      public uint m_UpdateFrameIndex;
      public NativeQueue<PayWageSystem.Payment>.ParallelWriter m_PaymentQueue;

      private void PayWage(
        Entity workplace,
        Entity worker,
        Entity household,
        Worker workerData,
        ref TaxPayer taxPayer,
        DynamicBuffer<Game.Economy.Resources> resources,
        CitizenAge age,
        bool isCommuter,
        ref EconomyParameterData economyParameters)
      {
        int amount = 0;
        // ISSUE: reference to a compiler-generated field
        Citizen citizen = this.m_Citizens[worker];
        // ISSUE: reference to a compiler-generated field
        if (workplace != Entity.Null && this.m_EmployeeBufs.HasBuffer(workplace))
        {
          bool flag = false;
          int index = 0;
          while (true)
          {
            int num = index;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Employee> employeeBuf = this.m_EmployeeBufs[workplace];
            int length = employeeBuf.Length;
            if (num < length)
            {
              // ISSUE: reference to a compiler-generated field
              employeeBuf = this.m_EmployeeBufs[workplace];
              if (!(employeeBuf[index].m_Worker == worker))
                ++index;
              else
                break;
            }
            else
              goto label_6;
          }
          flag = true;
label_6:
          if (!flag)
            return;
          int wage = economyParameters.GetWage((int) workerData.m_Level);
          if (isCommuter)
            wage = Mathf.RoundToInt((float) wage * economyParameters.m_CommuterWageMultiplier);
          // ISSUE: reference to a compiler-generated field
          amount = wage / PayWageSystem.kUpdatesPerDay;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Companies.HasComponent(workplace))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_PaymentQueue.Enqueue(new PayWageSystem.Payment()
            {
              m_Target = workplace,
              m_Amount = -amount
            });
          }
          if (citizen.m_UnemploymentCounter > 0)
          {
            citizen.m_UnemploymentCounter = 0;
            // ISSUE: reference to a compiler-generated field
            this.m_Citizens[worker] = citizen;
          }
        }
        else
        {
          switch (age)
          {
            case CitizenAge.Child:
              // ISSUE: reference to a compiler-generated field
              amount = economyParameters.m_FamilyAllowance / PayWageSystem.kUpdatesPerDay;
              break;
            case CitizenAge.Elderly:
              // ISSUE: reference to a compiler-generated field
              amount = economyParameters.m_Pension / PayWageSystem.kUpdatesPerDay;
              break;
            default:
              // ISSUE: reference to a compiler-generated field
              if ((double) citizen.m_UnemploymentCounter < (double) economyParameters.m_UnemploymentAllowanceMaxDays * (double) PayWageSystem.kUpdatesPerDay)
              {
                ++citizen.m_UnemploymentCounter;
                // ISSUE: reference to a compiler-generated field
                this.m_Citizens[worker] = citizen;
                // ISSUE: reference to a compiler-generated field
                amount = economyParameters.m_UnemploymentBenefit / PayWageSystem.kUpdatesPerDay;
                break;
              }
              break;
          }
        }
        EconomyUtils.AddResources(Resource.Money, amount, resources);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        int residentialTaxRate = TaxSystem.GetResidentialTaxRate((int) workerData.m_Level, this.m_TaxRates);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = this.m_OutsideConnections.HasComponent(workplace);
        // ISSUE: reference to a compiler-generated field
        int num1 = amount - economyParameters.m_ResidentialMinimumEarnings / PayWageSystem.kUpdatesPerDay;
        if (isCommuter || flag1 || residentialTaxRate == 0 || num1 <= 0)
          return;
        taxPayer.m_AverageTaxRate = Mathf.RoundToInt(math.lerp((float) taxPayer.m_AverageTaxRate, (float) residentialTaxRate, (float) num1 / (float) (num1 + taxPayer.m_UntaxedIncome)));
        taxPayer.m_UntaxedIncome += num1;
      }

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) this.m_UpdateFrameIndex != (int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index)
          return;
        // ISSUE: reference to a compiler-generated field
        bool isCommuter = chunk.Has<CommuterHousehold>(ref this.m_CommuterHouseholdType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TaxPayer> nativeArray2 = chunk.GetNativeArray<TaxPayer>(ref this.m_TaxPayerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<HouseholdCitizen> bufferAccessor1 = chunk.GetBufferAccessor<HouseholdCitizen>(ref this.m_HouseholdCitizenType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Economy.Resources> bufferAccessor2 = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourcesType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity household = nativeArray1[index1];
          DynamicBuffer<HouseholdCitizen> dynamicBuffer = bufferAccessor1[index1];
          DynamicBuffer<Game.Economy.Resources> resources = bufferAccessor2[index1];
          TaxPayer taxPayer = nativeArray2[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity citizen = dynamicBuffer[index2].m_Citizen;
            Entity workplace = Entity.Null;
            Worker workerData = new Worker();
            // ISSUE: reference to a compiler-generated field
            if (this.m_Workers.HasComponent(citizen))
            {
              // ISSUE: reference to a compiler-generated field
              workplace = this.m_Workers[citizen].m_Workplace;
              // ISSUE: reference to a compiler-generated field
              workerData = this.m_Workers[citizen];
            }
            // ISSUE: reference to a compiler-generated field
            CitizenAge age = this.m_Citizens[citizen].GetAge();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.PayWage(workplace, citizen, household, workerData, ref taxPayer, resources, age, isCommuter, ref this.m_EconomyParameters);
          }
          nativeArray2[index1] = taxPayer;
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle;
      public BufferTypeHandle<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public ComponentTypeHandle<TaxPayer> __Game_Agents_TaxPayer_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CommuterHousehold> __Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CompanyData> __Game_Companies_CompanyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle = state.GetBufferTypeHandle<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_TaxPayer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TaxPayer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CommuterHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentLookup = state.GetComponentLookup<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RO_ComponentLookup = state.GetComponentLookup<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>();
      }
    }
  }
}
