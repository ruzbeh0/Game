// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MailAccumulationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class MailAccumulationSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_MailProducerQuery;
    private EntityArchetype m_PostVanRequestArchetype;
    private MailAccumulationSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_890676534_0;
    private EntityQuery __query_890676534_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_MailProducerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<MailProducer>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PostVanRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_MailProducerQuery);
      this.RequireForUpdate<PostConfigurationData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      PostConfigurationData singleton = this.__query_890676534_0.GetSingleton<PostConfigurationData>();
      if (this.EntityManager.HasEnabledComponent<Locked>(singleton.m_PostServicePrefab))
        return;
      // ISSUE: reference to a compiler-generated field
      uint num = this.m_SimulationSystem.frameIndex / 64U & 15U;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PostVanRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle producerJob = new MailAccumulationSystem.MailAccumulationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_MailProducerType = this.__TypeHandle.__Game_Buildings_MailProducer_RW_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_PostVanRequestData = this.__TypeHandle.__Game_Simulation_PostVanRequest_RO_ComponentLookup,
        m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
        m_SpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_MailAccumulationData = this.__TypeHandle.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup,
        m_ServiceObjectData = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_UpdateFrameIndex = num,
        m_RandomSeed = RandomSeed.Next(),
        m_PostVanRequestArchetype = this.m_PostVanRequestArchetype,
        m_PostConfigurationData = singleton,
        m_EfficiencyParameters = this.__query_890676534_1.GetSingleton<BuildingEfficiencyParameterData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<MailAccumulationSystem.MailAccumulationJob>(this.m_MailProducerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_890676534_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PostConfigurationData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_890676534_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<BuildingEfficiencyParameterData>()
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
    public MailAccumulationSystem()
    {
    }

    [BurstCompile]
    private struct MailAccumulationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      public ComponentTypeHandle<MailProducer> m_MailProducerType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> m_PostVanRequestData;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<MailAccumulationData> m_MailAccumulationData;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjectData;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<Employee> m_Employees;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_PostVanRequestArchetype;
      [ReadOnly]
      public PostConfigurationData m_PostConfigurationData;
      [ReadOnly]
      public BuildingEfficiencyParameterData m_EfficiencyParameters;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

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
        float num = 0.284444451f;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor1 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<MailProducer> nativeArray3 = chunk.GetNativeArray<MailProducer>(ref this.m_MailProducerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          bool requireCollect;
          // ISSUE: reference to a compiler-generated method
          float2 accumulationRate = this.GetBaseAccumulationRate(nativeArray2[index].m_Prefab, out requireCollect);
          if (!math.all(accumulationRate == 0.0f))
          {
            float2 float2;
            if (bufferAccessor1.Length != 0)
            {
              int residentCount;
              int workerCount;
              // ISSUE: reference to a compiler-generated method
              this.GetCitizenCounts(bufferAccessor1[index], out residentCount, out workerCount);
              float2 = accumulationRate * (float) (residentCount + workerCount);
            }
            else
            {
              int residentCount;
              int workerCount;
              // ISSUE: reference to a compiler-generated method
              this.GetCitizenCounts(entity, out residentCount, out workerCount);
              float2 = accumulationRate * (float) (residentCount + workerCount);
            }
            int2 int2_1;
            int2_1.x = MathUtils.RoundToIntRandom(ref random, float2.x * num);
            int2_1.y = MathUtils.RoundToIntRandom(ref random, float2.y * num);
            ref MailProducer local = ref nativeArray3.ElementAt<MailProducer>(index);
            int2 int2_2 = new int2((int) local.m_SendingMail, local.receivingMail);
            int2 int2_3 = int2_2;
            // ISSUE: reference to a compiler-generated field
            int2_2 = math.min(int2_2 + int2_1, (int2) this.m_PostConfigurationData.m_MaxMailAccumulation);
            local.m_SendingMail = (ushort) int2_2.x;
            local.receivingMail = int2_2.y;
            // ISSUE: reference to a compiler-generated method
            this.RequestPostVanIfNeeded(unfilteredChunkIndex, entity, ref local, requireCollect);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (int2_3.y >= this.m_PostConfigurationData.m_MailAccumulationTolerance != int2_2.y >= this.m_PostConfigurationData.m_MailAccumulationTolerance)
            {
              // ISSUE: reference to a compiler-generated method
              this.QuantityUpdated(unfilteredChunkIndex, entity);
            }
            if (bufferAccessor2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              BuildingUtils.SetEfficiencyFactor(bufferAccessor2[index], EfficiencyFactor.Mail, this.GetMailEfficiencyFactor(local));
            }
          }
        }
      }

      private void QuantityUpdated(int jobIndex, Entity buildingEntity)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(buildingEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[buildingEntity];
        for (int index = 0; index < subObject1.Length; ++index)
        {
          Entity subObject2 = subObject1[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (this.m_QuantityData.HasComponent(subObject2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, subObject2, new BatchesUpdated());
          }
        }
      }

      private void GetCitizenCounts(
        DynamicBuffer<Renter> renters,
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
          this.GetCitizenCounts(renters[index].m_Renter, out residentCount1, out workerCount1);
          residentCount += residentCount1;
          workerCount += workerCount1;
        }
      }

      private void GetCitizenCounts(Entity entity, out int residentCount, out int workerCount)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        residentCount = !this.m_HouseholdCitizens.HasBuffer(entity) ? 0 : this.m_HouseholdCitizens[entity].Length;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Employees.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          workerCount = this.m_Employees[entity].Length;
        }
        else
          workerCount = 0;
      }

      private float2 GetBaseAccumulationRate(Entity prefab, out bool requireCollect)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnableBuildingData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          SpawnableBuildingData spawnableBuildingData = this.m_SpawnableBuildingData[prefab];
          // ISSUE: reference to a compiler-generated field
          if (this.m_MailAccumulationData.HasComponent(spawnableBuildingData.m_ZonePrefab))
          {
            // ISSUE: reference to a compiler-generated field
            MailAccumulationData accumulationData = this.m_MailAccumulationData[spawnableBuildingData.m_ZonePrefab];
            requireCollect = accumulationData.m_RequireCollect;
            return accumulationData.m_AccumulationRate;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceObjectData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ServiceObjectData serviceObjectData = this.m_ServiceObjectData[prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_MailAccumulationData.HasComponent(serviceObjectData.m_Service))
            {
              // ISSUE: reference to a compiler-generated field
              MailAccumulationData accumulationData = this.m_MailAccumulationData[serviceObjectData.m_Service];
              requireCollect = accumulationData.m_RequireCollect;
              return accumulationData.m_AccumulationRate;
            }
          }
        }
        requireCollect = false;
        return new float2();
      }

      private void RequestPostVanIfNeeded(
        int jobIndex,
        Entity entity,
        ref MailProducer producer,
        bool requireCollect)
      {
        int priority = !requireCollect ? producer.receivingMail : math.max((int) producer.m_SendingMail, producer.receivingMail);
        PostVanRequest componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (priority < this.m_PostConfigurationData.m_MailAccumulationTolerance || this.m_PostVanRequestData.TryGetComponent(producer.m_MailRequest, out componentData) && (componentData.m_Target == entity || (int) componentData.m_DispatchIndex == (int) producer.m_DispatchIndex))
          return;
        producer.m_MailRequest = Entity.Null;
        producer.m_DispatchIndex = (byte) 0;
        PostVanRequestFlags flags = PostVanRequestFlags.Deliver | PostVanRequestFlags.BuildingTarget;
        if (requireCollect)
          flags |= PostVanRequestFlags.Collect;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PostVanRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PostVanRequest>(jobIndex, entity1, new PostVanRequest(entity, flags, (ushort) priority));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      private float GetMailEfficiencyFactor(MailProducer producer)
      {
        // ISSUE: reference to a compiler-generated field
        int num1 = math.max(0, math.max((int) producer.m_SendingMail, producer.receivingMail) - this.m_EfficiencyParameters.m_NegligibleMail);
        float num2 = 0.0f;
        if (num1 > 25)
        {
          int num3 = math.min(50, num1 - 25);
          num2 = (float) (((double) (num3 * num3) + 125.0) / 2625.0);
        }
        // ISSUE: reference to a compiler-generated field
        return (float) (1.0 - (double) this.m_EfficiencyParameters.m_MailEfficiencyPenalty * (double) num2);
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      public ComponentTypeHandle<MailProducer> __Game_Buildings_MailProducer_RW_ComponentTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> __Game_Simulation_PostVanRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailAccumulationData> __Game_Prefabs_MailAccumulationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MailProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PostVanRequest_RO_ComponentLookup = state.GetComponentLookup<PostVanRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup = state.GetComponentLookup<MailAccumulationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
