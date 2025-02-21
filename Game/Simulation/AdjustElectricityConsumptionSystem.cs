// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AdjustElectricityConsumptionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Assertions;
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
  public class AdjustElectricityConsumptionSystem : GameSystemBase
  {
    private const int kFullUpdatesPerDay = 128;
    private ClimateSystem m_ClimateSystem;
    private SimulationSystem m_SimulationSystem;
    private CitySystem m_CitySystem;
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private EntityQuery m_ConsumerQuery;
    private AdjustElectricityConsumptionSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_653552652_0;
    private EntityQuery __query_653552652_1;
    private EntityQuery __query_653552652_2;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      Assert.IsTrue(this.GetUpdateInterval(SystemUpdatePhase.GameSimulation) >= 128);
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConsumerQuery = this.GetEntityQuery(ComponentType.ReadWrite<ElectricityConsumer>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ConsumerQuery);
      this.RequireForUpdate<ServiceFeeParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 128, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_StorageProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle dependsOn = new AdjustElectricityConsumptionSystem.AdjustElectricityConsumptionJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_CityServiceType = this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentTypeHandle,
        m_UpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle,
        m_CurrentDistrictType = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_ParkType = this.__TypeHandle.__Game_Buildings_Park_RO_ComponentTypeHandle,
        m_StoragePropertyType = this.__TypeHandle.__Game_Buildings_StorageProperty_RO_ComponentTypeHandle,
        m_ConsumerType = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RW_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceConsumption = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup,
        m_Fees = this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_SpawnableDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup,
        m_UpdatedEdges = nativeQueue.AsParallelWriter(),
        m_FeeParameters = this.__query_653552652_0.GetSingleton<ServiceFeeParameterData>(),
        m_EfficiencyParameters = this.__query_653552652_1.GetSingleton<BuildingEfficiencyParameterData>(),
        m_RandomSeed = RandomSeed.Next(),
        m_City = this.m_CitySystem.City,
        m_TemperatureMultiplier = this.GetTemperatureMultiplier((float) this.m_ClimateSystem.temperature),
        m_UpdateFrameIndex = updateFrame
      }.ScheduleParallel<AdjustElectricityConsumptionSystem.AdjustElectricityConsumptionJob>(this.m_ConsumerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      AdjustElectricityConsumptionSystem.UpdateEdgesJob jobData = new AdjustElectricityConsumptionSystem.UpdateEdgesJob()
      {
        m_NodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_BuildingConnections = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup,
        m_Consumers = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup,
        m_UpdatedEdges = nativeQueue,
        m_SinkNode = this.m_ElectricityFlowSystem.sinkNode
      };
      this.Dependency = jobData.Schedule<AdjustElectricityConsumptionSystem.UpdateEdgesJob>(dependsOn);
      nativeQueue.Dispose(this.Dependency);
    }

    public float GetTemperatureMultiplier(float temperature)
    {
      ElectricityParameterData electricityParameterData;
      // ISSUE: reference to a compiler-generated field
      return !this.__query_653552652_2.TryGetSingleton<ElectricityParameterData>(out electricityParameterData) ? 1f : electricityParameterData.m_TemperatureConsumptionMultiplier.Evaluate(temperature);
    }

    public static float GetFeeConsumptionMultiplier(
      float relativeFee,
      in ServiceFeeParameterData feeParameters)
    {
      return feeParameters.m_ElectricityFeeConsumptionMultiplier.Evaluate(relativeFee);
    }

    public static float GetFeeEfficiencyFactor(
      float relativeFee,
      in BuildingEfficiencyParameterData efficiencyParameters)
    {
      return efficiencyParameters.m_ElectricityFeeFactor.Evaluate(relativeFee);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_653552652_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ServiceFeeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_653552652_1 = state.GetEntityQuery(new EntityQueryDesc()
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
      // ISSUE: reference to a compiler-generated field
      this.__query_653552652_2 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ElectricityParameterData>()
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
    public AdjustElectricityConsumptionSystem()
    {
    }

    [BurstCompile]
    public struct AdjustElectricityConsumptionJob : IJobChunk
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<CityServiceUpkeep> m_CityServiceType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_UpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Park> m_ParkType;
      [ReadOnly]
      public ComponentTypeHandle<StorageProperty> m_StoragePropertyType;
      public ComponentTypeHandle<ElectricityConsumer> m_ConsumerType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> m_ServiceConsumption;
      [ReadOnly]
      public BufferLookup<ServiceFee> m_Fees;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<Employee> m_Employees;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableDatas;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public NativeQueue<Entity>.ParallelWriter m_UpdatedEdges;
      public ServiceFeeParameterData m_FeeParameters;
      public BuildingEfficiencyParameterData m_EfficiencyParameters;
      public RandomSeed m_RandomSeed;
      public Entity m_City;
      public float m_TemperatureMultiplier;
      public uint m_UpdateFrameIndex;

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
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray2 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray3 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor1 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_UpgradeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityBuildingConnection> nativeArray4 = chunk.GetNativeArray<ElectricityBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor2 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityConsumer> nativeArray5 = chunk.GetNativeArray<ElectricityConsumer>(ref this.m_ConsumerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor3 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Game.Buildings.Park>(ref this.m_ParkType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<StorageProperty>(ref this.m_StoragePropertyType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(1 + unfilteredChunkIndex);
        float num;
        float efficiency;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<CityServiceUpkeep>(ref this.m_CityServiceType))
        {
          num = 1f;
          efficiency = 1f;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          double relativeFee = (double) ServiceFeeSystem.GetFee(PlayerResource.Electricity, this.m_Fees[this.m_City]) / (double) this.m_FeeParameters.m_ElectricityFee.m_Default;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          num = AdjustElectricityConsumptionSystem.GetFeeConsumptionMultiplier((float) relativeFee, in this.m_FeeParameters);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          efficiency = AdjustElectricityConsumptionSystem.GetFeeEfficiencyFactor((float) relativeFee, in this.m_EfficiencyParameters);
        }
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity prefab = nativeArray1[index].m_Prefab;
          ConsumptionData componentData;
          // ISSUE: reference to a compiler-generated field
          this.m_ServiceConsumption.TryGetComponent(prefab, out componentData);
          if (bufferAccessor1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<ConsumptionData>(ref componentData, bufferAccessor1[index], ref this.m_Prefabs, ref this.m_ServiceConsumption);
          }
          float a = componentData.m_ElectricityConsumption;
          // ISSUE: reference to a compiler-generated field
          a *= this.m_TemperatureMultiplier;
          a *= num;
          DynamicBuffer<DistrictModifier> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (nativeArray3.Length != 0 && this.m_DistrictModifiers.TryGetBuffer(nativeArray3[index].m_District, out bufferData))
            AreaUtils.ApplyModifier(ref a, bufferData, DistrictModifierType.EnergyConsumptionAwareness);
          if (!flag1 && !flag2 && bufferAccessor2.Length != 0)
          {
            bool flag3 = (double) a > 0.0;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            a *= FlowUtils.GetRenterConsumptionMultiplier(prefab, bufferAccessor2[index], ref this.m_HouseholdCitizens, ref this.m_Employees, ref this.m_Citizens, ref this.m_SpawnableDatas);
            a = math.select(a, 1f, flag3 && (double) a < 1.0);
          }
          else
            a = math.select(a, 1f, (double) a > 0.0 && (double) a < 1.0);
          ref ElectricityConsumer local = ref nativeArray5.ElementAt<ElectricityConsumer>(index);
          int intRandom = (double) a > 0.0 ? MathUtils.RoundToIntRandom(ref random, a) : 0;
          if (BuildingUtils.CheckOption(nativeArray2[index], BuildingOption.Inactive))
            intRandom /= 10;
          if (intRandom != local.m_WantedConsumption)
          {
            local.m_WantedConsumption = intRandom;
            if (nativeArray2[index].m_RoadEdge != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedEdges.Enqueue(nativeArray2[index].m_RoadEdge);
            }
            if (nativeArray4.Length != 0)
            {
              Entity consumerEdge = nativeArray4[index].m_ConsumerEdge;
              if (consumerEdge != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                ElectricityFlowEdge flowEdge = this.m_FlowEdges[consumerEdge] with
                {
                  m_Capacity = intRandom
                };
                // ISSUE: reference to a compiler-generated field
                this.m_FlowEdges[consumerEdge] = flowEdge;
              }
            }
          }
          if (bufferAccessor3.Length != 0)
            BuildingUtils.SetEfficiencyFactor(bufferAccessor3[index], EfficiencyFactor.ElectricityFee, efficiency);
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
    private struct UpdateEdgesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_NodeConnections;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_Consumers;
      [ReadOnly]
      public ComponentLookup<ElectricityBuildingConnection> m_BuildingConnections;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
      public NativeQueue<Entity> m_UpdatedEdges;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public Entity m_SinkNode;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(this.m_UpdatedEdges.Count, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_UpdatedEdges.TryDequeue(out entity1))
        {
          ElectricityNodeConnection componentData;
          Entity entity2;
          ElectricityFlowEdge edge;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (nativeParallelHashSet.Add(entity1) && this.m_NodeConnections.TryGetComponent(entity1, out componentData) && ElectricityGraphUtils.TryGetFlowEdge(componentData.m_ElectricityNode, this.m_SinkNode, ref this.m_FlowConnections, ref this.m_FlowEdges, out entity2, out edge))
          {
            // ISSUE: reference to a compiler-generated method
            edge.m_Capacity = this.GetNonConnectedBuildingConsumption(entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_FlowEdges[entity2] = edge;
          }
        }
      }

      private int GetNonConnectedBuildingConsumption(Entity roadEdge)
      {
        int buildingConsumption = 0;
        DynamicBuffer<ConnectedBuilding> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedBuildings.TryGetBuffer(roadEdge, out bufferData))
        {
          foreach (ConnectedBuilding connectedBuilding in bufferData)
          {
            ElectricityConsumer componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Consumers.TryGetComponent(connectedBuilding.m_Building, out componentData) && !this.m_BuildingConnections.HasComponent(connectedBuilding.m_Building))
              buildingConsumption += componentData.m_WantedConsumption;
          }
        }
        return buildingConsumption;
      }
    }

    private struct TypeHandle
    {
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CityServiceUpkeep> __Game_City_CityServiceUpkeep_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Park> __Game_Buildings_Park_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StorageProperty> __Game_Buildings_StorageProperty_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RW_ComponentTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> __Game_Prefabs_ConsumptionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceFee> __Game_City_ServiceFee_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityServiceUpkeep_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CityServiceUpkeep>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Park>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_StorageProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StorageProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RO_ComponentLookup = state.GetComponentLookup<ConsumptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_ServiceFee_RO_BufferLookup = state.GetBufferLookup<ServiceFee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferLookup = state.GetBufferLookup<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
      }
    }
  }
}
