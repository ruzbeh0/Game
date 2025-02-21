// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PostInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Citizens;
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
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PostInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "postInfo";
    private const float kAccumulationFactor = 72.81778f;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_PostFacilityModifiedQuery;
    private EntityQuery m_MailProducerQuery;
    private EntityQuery m_MailProducerModifiedQuery;
    private ValueBinding<int> m_CollectedMail;
    private ValueBinding<int> m_DeliveredMail;
    private ValueBinding<float> m_MailProductionRate;
    private ValueBinding<IndicatorValue> m_PostServiceAvailability;
    private NativeArray<float2> m_Result;
    private PostInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PostFacilityModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Buildings.PostFacility>()
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
      // ISSUE: reference to a compiler-generated field
      this.m_MailProducerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<MailProducer>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_MailProducerModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<MailProducer>()
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
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CollectedMail = new ValueBinding<int>("postInfo", "collectedMail", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_DeliveredMail = new ValueBinding<int>("postInfo", "deliveredMail", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MailProductionRate = new ValueBinding<float>("postInfo", "mailProductionRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PostServiceAvailability = new ValueBinding<IndicatorValue>("postInfo", "postServiceAvailability", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.m_Result = new NativeArray<float2>(1, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Result.Dispose();
      base.OnDestroy();
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_CollectedMail.active || this.m_DeliveredMail.active || this.m_MailProductionRate.active || this.m_PostServiceAvailability.active;
      }
    }

    protected override bool Modified
    {
      get
      {
        return !this.m_MailProducerModifiedQuery.IsEmptyIgnoreFilter || !this.m_PostFacilityModifiedQuery.IsEmptyIgnoreFilter;
      }
    }

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateMailRate();
      // ISSUE: reference to a compiler-generated method
      this.UpdateProcessingRate();
      // ISSUE: reference to a compiler-generated method
      this.UpdateAvailability();
    }

    private void ResetResults()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Result.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Result[index] = new float2();
      }
    }

    private void UpdateMailRate()
    {
      // ISSUE: reference to a compiler-generated method
      this.ResetResults();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new PostInfoviewUISystem.UpdateMailRateJob()
      {
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ServiceObjectFromEntity = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_SpawnableDataFromEntity = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_MailAccumulationFromEntity = this.__TypeHandle.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup,
        m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_EmployeeFromEntity = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_HouseholdCitizenFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Result = this.m_Result
      }.Schedule<PostInfoviewUISystem.UpdateMailRateJob>(this.m_MailProducerQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      float2 float2 = this.m_Result[0] * 72.81778f;
      // ISSUE: reference to a compiler-generated field
      this.m_MailProductionRate.Update(float2.x + float2.y);
    }

    private void UpdateProcessingRate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DeliveredMail.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.DeliveredMail, 0));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CollectedMail.Update(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.CollectedMail, 0));
    }

    private void UpdateAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PostServiceAvailability.Update(IndicatorValue.Calculate((float) (this.m_DeliveredMail.value + this.m_CollectedMail.value), this.m_MailProductionRate.value));
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
    public PostInfoviewUISystem()
    {
    }

    [BurstCompile]
    private struct UpdateMailRateJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefHandle;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjectFromEntity;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableDataFromEntity;
      [ReadOnly]
      public ComponentLookup<MailAccumulationData> m_MailAccumulationFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      [ReadOnly]
      public BufferLookup<Employee> m_EmployeeFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenFromEntity;
      public NativeArray<float2> m_Result;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefHandle);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          float2 float2 = new float2(0.0f, 0.0f);
          SpawnableBuildingData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpawnableDataFromEntity.TryGetComponent(prefabRef.m_Prefab, out componentData1))
          {
            MailAccumulationData componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MailAccumulationFromEntity.TryGetComponent(componentData1.m_ZonePrefab, out componentData2))
              float2 = componentData2.m_AccumulationRate;
          }
          else
          {
            ServiceObjectData componentData3;
            MailAccumulationData componentData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ServiceObjectFromEntity.TryGetComponent(prefabRef.m_Prefab, out componentData3) && this.m_MailAccumulationFromEntity.TryGetComponent(componentData3.m_Service, out componentData4))
              float2 = componentData4.m_AccumulationRate;
          }
          int num = 0;
          DynamicBuffer<Renter> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RenterFromEntity.TryGetBuffer(entity, out bufferData))
          {
            if (bufferData.Length > 0)
            {
              int residentCount;
              int workerCount;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.GetCitizenCounts(bufferData, this.m_HouseholdCitizenFromEntity, this.m_EmployeeFromEntity, out residentCount, out workerCount);
              num += residentCount + workerCount;
            }
          }
          else
          {
            int residentCount;
            int workerCount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.GetCitizenCounts(entity, this.m_HouseholdCitizenFromEntity, this.m_EmployeeFromEntity, out residentCount, out workerCount);
            num += residentCount + workerCount;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_Result[0] += float2 * (float) num;
        }
      }

      private void GetCitizenCounts(
        DynamicBuffer<Renter> renters,
        BufferLookup<HouseholdCitizen> citizens,
        BufferLookup<Employee> employees,
        out int residentCount,
        out int workerCount)
      {
        residentCount = 0;
        workerCount = 0;
        for (int index = 0; index < renters.Length; ++index)
        {
          int residentCount1;
          int workerCount1;
          // ISSUE: reference to a compiler-generated method
          this.GetCitizenCounts(renters[index].m_Renter, citizens, employees, out residentCount1, out workerCount1);
          residentCount += residentCount1;
          workerCount += workerCount1;
        }
      }

      private void GetCitizenCounts(
        Entity entity,
        BufferLookup<HouseholdCitizen> citizens,
        BufferLookup<Employee> employees,
        out int residentCount,
        out int workerCount)
      {
        residentCount = citizens.HasBuffer(entity) ? citizens[entity].Length : 0;
        workerCount = employees.HasBuffer(entity) ? employees[entity].Length : 0;
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailAccumulationData> __Game_Prefabs_MailAccumulationData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup = state.GetComponentLookup<MailAccumulationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
      }
    }
  }
}
