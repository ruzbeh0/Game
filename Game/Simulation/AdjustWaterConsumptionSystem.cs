// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AdjustWaterConsumptionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
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
  public class AdjustWaterConsumptionSystem : GameSystemBase
  {
    private const int kFullUpdatesPerDay = 128;
    private SimulationSystem m_SimulationSystem;
    private CitySystem m_CitySystem;
    private WaterPipeFlowSystem m_WaterPipeFlowSystem;
    private EntityQuery m_ConsumerQuery;
    private AdjustWaterConsumptionSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1300465010_0;
    private EntityQuery __query_1300465010_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      Assert.IsTrue(this.GetUpdateInterval(SystemUpdatePhase.GameSimulation) >= 128);
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeFlowSystem = this.World.GetOrCreateSystemManaged<WaterPipeFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConsumerQuery = this.GetEntityQuery(ComponentType.ReadWrite<WaterConsumer>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ConsumerQuery);
      this.RequireForUpdate<ServiceFeeParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
    }

    public static float GetFeeConsumptionMultiplier(
      float relativeFee,
      in ServiceFeeParameterData feeParameters)
    {
      return feeParameters.m_WaterFeeConsumptionMultiplier.Evaluate(relativeFee);
    }

    public static float GetFeeEfficiencyFactor(
      float relativeFee,
      in BuildingEfficiencyParameterData efficiencyParameters)
    {
      return efficiencyParameters.m_WaterFeeFactor.Evaluate(relativeFee);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 128, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = new AdjustWaterConsumptionSystem.AdjustWaterConsumptionJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_CityServiceType = this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentTypeHandle,
        m_UpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_ParkType = this.__TypeHandle.__Game_Buildings_Park_RO_ComponentTypeHandle,
        m_StoragePropertyType = this.__TypeHandle.__Game_Buildings_StorageProperty_RO_ComponentTypeHandle,
        m_ConsumerType = this.__TypeHandle.__Game_Buildings_WaterConsumer_RW_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceConsumption = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup,
        m_Fees = this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_SpawnableDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup,
        m_UpdatedEdges = nativeQueue.AsParallelWriter(),
        m_FeeParameters = this.__query_1300465010_0.GetSingleton<ServiceFeeParameterData>(),
        m_EfficiencyParameters = this.__query_1300465010_1.GetSingleton<BuildingEfficiencyParameterData>(),
        m_RandomSeed = RandomSeed.Next(),
        m_City = this.m_CitySystem.City,
        m_UpdateFrameIndex = updateFrame
      }.ScheduleParallel<AdjustWaterConsumptionSystem.AdjustWaterConsumptionJob>(this.m_ConsumerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      AdjustWaterConsumptionSystem.UpdateEdgesJob jobData = new AdjustWaterConsumptionSystem.UpdateEdgesJob()
      {
        m_NodeConnections = this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup,
        m_BuildingConnections = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup,
        m_Consumers = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup,
        m_UpdatedEdges = nativeQueue,
        m_SinkNode = this.m_WaterPipeFlowSystem.sinkNode
      };
      this.Dependency = jobData.Schedule<AdjustWaterConsumptionSystem.UpdateEdgesJob>(dependsOn);
      nativeQueue.Dispose(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1300465010_0 = state.GetEntityQuery(new EntityQueryDesc()
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
      this.__query_1300465010_1 = state.GetEntityQuery(new EntityQueryDesc()
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
    public AdjustWaterConsumptionSystem()
    {
    }

    [BurstCompile]
    public struct AdjustWaterConsumptionJob : IJobChunk
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
      public ComponentTypeHandle<WaterPipeBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Park> m_ParkType;
      [ReadOnly]
      public ComponentTypeHandle<StorageProperty> m_StoragePropertyType;
      public ComponentTypeHandle<WaterConsumer> m_ConsumerType;
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
      [NativeDisableParallelForRestriction]
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      public NativeQueue<Entity>.ParallelWriter m_UpdatedEdges;
      public ServiceFeeParameterData m_FeeParameters;
      public BuildingEfficiencyParameterData m_EfficiencyParameters;
      public RandomSeed m_RandomSeed;
      public Entity m_City;
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
        BufferAccessor<InstalledUpgrade> bufferAccessor1 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_UpgradeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPipeBuildingConnection> nativeArray3 = chunk.GetNativeArray<WaterPipeBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor2 = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterConsumer> nativeArray4 = chunk.GetNativeArray<WaterConsumer>(ref this.m_ConsumerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor3 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Game.Buildings.Park>(ref this.m_ParkType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<StorageProperty>(ref this.m_StoragePropertyType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(1 + unfilteredChunkIndex);
        float num1;
        float efficiency;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<CityServiceUpkeep>(ref this.m_CityServiceType))
        {
          num1 = 1f;
          efficiency = 1f;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          double relativeFee = (double) ServiceFeeSystem.GetFee(PlayerResource.Water, this.m_Fees[this.m_City]) / (double) this.m_FeeParameters.m_WaterFee.m_Default;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          num1 = AdjustWaterConsumptionSystem.GetFeeConsumptionMultiplier((float) relativeFee, in this.m_FeeParameters);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          efficiency = AdjustWaterConsumptionSystem.GetFeeEfficiencyFactor((float) relativeFee, in this.m_EfficiencyParameters);
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
          float a1 = (BuildingUtils.CheckOption(nativeArray2[index], BuildingOption.Inactive) ? componentData.m_WaterConsumption * 0.1f : componentData.m_WaterConsumption) * num1;
          float num2;
          if (!flag1 && !flag2 && bufferAccessor2.Length != 0)
          {
            bool flag3 = (double) a1 > 0.0;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float a2 = a1 * FlowUtils.GetRenterConsumptionMultiplier(prefab, bufferAccessor2[index], ref this.m_HouseholdCitizens, ref this.m_Employees, ref this.m_Citizens, ref this.m_SpawnableDatas);
            num2 = math.select(a2, 1f, flag3 && (double) a2 < 1.0);
          }
          else
            num2 = math.select(a1, 1f, (double) a1 > 0.0 && (double) a1 < 1.0);
          ref WaterConsumer local = ref nativeArray4.ElementAt<WaterConsumer>(index);
          int wantedConsumption = local.m_WantedConsumption;
          int intRandom = (double) num2 > 0.0 ? MathUtils.RoundToIntRandom(ref random, num2) : 0;
          int num3 = intRandom;
          if (wantedConsumption != num3)
          {
            local.m_WantedConsumption = intRandom;
            if (nativeArray2[index].m_RoadEdge != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedEdges.Enqueue(nativeArray2[index].m_RoadEdge);
            }
            if (nativeArray3.Length != 0)
            {
              Entity consumerEdge = nativeArray3[index].m_ConsumerEdge;
              if (consumerEdge != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                WaterPipeEdge flowEdge = this.m_FlowEdges[consumerEdge] with
                {
                  m_FreshCapacity = intRandom,
                  m_SewageCapacity = intRandom
                };
                // ISSUE: reference to a compiler-generated field
                this.m_FlowEdges[consumerEdge] = flowEdge;
              }
            }
          }
          if (bufferAccessor3.Length != 0)
            BuildingUtils.SetEfficiencyFactor(bufferAccessor3[index], EfficiencyFactor.WaterFee, efficiency);
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
      public ComponentLookup<WaterPipeNodeConnection> m_NodeConnections;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_Consumers;
      [ReadOnly]
      public ComponentLookup<WaterPipeBuildingConnection> m_BuildingConnections;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
      public NativeQueue<Entity> m_UpdatedEdges;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      public Entity m_SinkNode;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(this.m_UpdatedEdges.Count, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_UpdatedEdges.TryDequeue(out entity1))
        {
          WaterPipeNodeConnection componentData;
          Entity entity2;
          WaterPipeEdge edge;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (nativeParallelHashSet.Add(entity1) && this.m_NodeConnections.TryGetComponent(entity1, out componentData) && WaterPipeGraphUtils.TryGetFlowEdge(componentData.m_WaterPipeNode, this.m_SinkNode, ref this.m_FlowConnections, ref this.m_FlowEdges, out entity2, out edge))
          {
            // ISSUE: reference to a compiler-generated method
            int buildingConsumption = this.GetNonConnectedBuildingConsumption(entity1);
            edge.m_FreshCapacity = buildingConsumption;
            edge.m_SewageCapacity = buildingConsumption;
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
            WaterConsumer componentData;
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
      public ComponentTypeHandle<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Park> __Game_Buildings_Park_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StorageProperty> __Game_Buildings_StorageProperty_RO_ComponentTypeHandle;
      public ComponentTypeHandle<WaterConsumer> __Game_Buildings_WaterConsumer_RW_ComponentTypeHandle;
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
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> __Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
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
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Park>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_StorageProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StorageProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterConsumer>();
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
        this.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferLookup = state.GetBufferLookup<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
      }
    }
  }
}
