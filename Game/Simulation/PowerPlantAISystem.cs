// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PowerPlantAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PowerPlantAISystem : GameSystemBase
  {
    public const int MAX_WATERPOWERED_SIZE = 1000000;
    private PlanetarySystem m_PlanetarySystem;
    private WindSystem m_WindSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private ClimateSystem m_ClimateSystem;
    private EntityQuery m_PowerPlantQuery;
    private PowerPlantAISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_833752410_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PlanetarySystem = this.World.GetOrCreateSystemManaged<PlanetarySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem = this.World.GetOrCreateSystemManaged<WindSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetExistingSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PowerPlantQuery = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityProducer>(), ComponentType.ReadOnly<ElectricityBuildingConnection>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PowerPlantQuery);
      this.RequireForUpdate<ElectricityParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      ElectricityParameterData singleton = this.__query_833752410_0.GetSingleton<ElectricityParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      PlanetarySystem.LightData sunLight = this.m_PlanetarySystem.SunLight;
      float num1 = 0.0f;
      if (sunLight.isValid)
        num1 = (float) ((double) math.max(0.0f, -sunLight.transform.forward.y) * (double) sunLight.additionalData.intensity / 110000.0);
      // ISSUE: reference to a compiler-generated field
      float num2 = num1 * math.lerp(1f, 1f - singleton.m_CloudinessSolarPenalty, this.m_ClimateSystem.cloudiness.value);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUsage_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResourceConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SolarPoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WindPoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbagePoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PowerPlantData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PointOfInterest_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUsage_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityProducer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterPowered_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResourceConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle deps;
      JobHandle dependencies2;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PowerPlantAISystem.PowerPlantTickJob jobData = new PowerPlantAISystem.PowerPlantTickJob()
      {
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_GarbageFacilityType = this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle,
        m_ResourceConsumerType = this.__TypeHandle.__Game_Buildings_ResourceConsumer_RO_ComponentTypeHandle,
        m_WaterPoweredType = this.__TypeHandle.__Game_Buildings_WaterPowered_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_SubNetType = this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle,
        m_ElectricityProducerType = this.__TypeHandle.__Game_Buildings_ElectricityProducer_RW_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_ServiceUsageType = this.__TypeHandle.__Game_Buildings_ServiceUsage_RW_ComponentTypeHandle,
        m_PointOfInterestType = this.__TypeHandle.__Game_Common_PointOfInterest_RW_ComponentTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PowerPlantDatas = this.__TypeHandle.__Game_Prefabs_PowerPlantData_RO_ComponentLookup,
        m_GarbagePoweredData = this.__TypeHandle.__Game_Prefabs_GarbagePoweredData_RO_ComponentLookup,
        m_WindPoweredData = this.__TypeHandle.__Game_Prefabs_WindPoweredData_RO_ComponentLookup,
        m_WaterPoweredData = this.__TypeHandle.__Game_Prefabs_WaterPoweredData_RO_ComponentLookup,
        m_SolarPoweredData = this.__TypeHandle.__Game_Prefabs_SolarPoweredData_RO_ComponentLookup,
        m_GroundWaterPoweredData = this.__TypeHandle.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentLookup,
        m_PlaceableNetData = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup,
        m_NetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_ResourceConsumers = this.__TypeHandle.__Game_Buildings_ResourceConsumer_RO_ComponentLookup,
        m_Curves = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_Compositions = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_ServiceUsages = this.__TypeHandle.__Game_Buildings_ServiceUsage_RW_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup,
        m_WindMap = this.m_WindSystem.GetMap(true, out dependencies1),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetVelocitiesSurfaceData(out deps),
        m_GroundWaterMap = this.m_GroundWaterSystem.GetMap(true, out dependencies2),
        m_SunLight = num2
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<PowerPlantAISystem.PowerPlantTickJob>(this.m_PowerPlantQuery, JobUtils.CombineDependencies(this.Dependency, dependencies1, deps, dependencies2));
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddVelocitySurfaceReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem.AddReader(this.Dependency);
    }

    public static float2 GetWindProduction(WindPoweredData windData, Wind wind, float efficiency)
    {
      float y = efficiency * (float) windData.m_Production;
      float x = math.lengthsq(wind.m_Wind) / (windData.m_MaximumWind * windData.m_MaximumWind);
      return new float2(y * math.saturate(math.pow(x, 1.5f)), y);
    }

    public static float GetWaterCapacity(Game.Buildings.WaterPowered waterPowered, WaterPoweredData waterData)
    {
      return math.min(waterPowered.m_Length * waterPowered.m_Height, 1000000f) * waterData.m_CapacityFactor;
    }

    public static float2 GetGroundWaterProduction(
      GroundWaterPoweredData groundWaterData,
      float3 position,
      float efficiency,
      NativeArray<GroundWater> groundWaterMap)
    {
      // ISSUE: reference to a compiler-generated method
      float num1 = (float) GroundWaterSystem.GetGroundWater(position, groundWaterMap).m_Amount / (float) groundWaterData.m_MaximumGroundWater;
      float num2 = efficiency * (float) groundWaterData.m_Production;
      return new float2(math.clamp(num2 * num1, 0.0f, num2), num2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_833752410_0 = state.GetEntityQuery(new EntityQueryDesc()
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
    public PowerPlantAISystem()
    {
    }

    [BurstCompile]
    private struct PowerPlantTickJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.GarbageFacility> m_GarbageFacilityType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ResourceConsumer> m_ResourceConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterPowered> m_WaterPoweredType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> m_SubNetType;
      public ComponentTypeHandle<ElectricityProducer> m_ElectricityProducerType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      public ComponentTypeHandle<ServiceUsage> m_ServiceUsageType;
      public ComponentTypeHandle<PointOfInterest> m_PointOfInterestType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<PowerPlantData> m_PowerPlantDatas;
      [ReadOnly]
      public ComponentLookup<GarbagePoweredData> m_GarbagePoweredData;
      [ReadOnly]
      public ComponentLookup<WindPoweredData> m_WindPoweredData;
      [ReadOnly]
      public ComponentLookup<WaterPoweredData> m_WaterPoweredData;
      [ReadOnly]
      public ComponentLookup<SolarPoweredData> m_SolarPoweredData;
      [ReadOnly]
      public ComponentLookup<GroundWaterPoweredData> m_GroundWaterPoweredData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> m_PlaceableNetData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ResourceConsumer> m_ResourceConsumers;
      [ReadOnly]
      public ComponentLookup<Curve> m_Curves;
      [ReadOnly]
      public ComponentLookup<Composition> m_Compositions;
      [NativeDisableContainerSafetyRestriction]
      public ComponentLookup<ServiceUsage> m_ServiceUsages;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      [ReadOnly]
      public NativeArray<Wind> m_WindMap;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public NativeArray<GroundWater> m_GroundWaterMap;
      public float m_SunLight;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.GarbageFacility> nativeArray2 = chunk.GetNativeArray<Game.Buildings.GarbageFacility>(ref this.m_GarbageFacilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor1 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityBuildingConnection> nativeArray3 = chunk.GetNativeArray<ElectricityBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityProducer> nativeArray4 = chunk.GetNativeArray<ElectricityProducer>(ref this.m_ElectricityProducerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.WaterPowered> nativeArray5 = chunk.GetNativeArray<Game.Buildings.WaterPowered>(ref this.m_WaterPoweredType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray6 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubNet> bufferAccessor2 = chunk.GetBufferAccessor<Game.Net.SubNet>(ref this.m_SubNetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.ResourceConsumer> nativeArray7 = chunk.GetNativeArray<Game.Buildings.ResourceConsumer>(ref this.m_ResourceConsumerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor3 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceUsage> nativeArray8 = chunk.GetNativeArray<ServiceUsage>(ref this.m_ServiceUsageType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PointOfInterest> nativeArray9 = chunk.GetNativeArray<PointOfInterest>(ref this.m_PointOfInterestType);
        Span<float> factors = stackalloc float[28];
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity prefab = nativeArray1[index1].m_Prefab;
          ref ElectricityProducer local1 = ref nativeArray4.ElementAt<ElectricityProducer>(index1);
          ElectricityBuildingConnection buildingConnection = nativeArray3[index1];
          byte resourceAvailability1 = nativeArray7.Length != 0 ? nativeArray7[index1].m_ResourceAvailability : byte.MaxValue;
          Game.Objects.Transform transform = nativeArray6[index1];
          if (bufferAccessor3.Length != 0)
          {
            BuildingUtils.GetEfficiencyFactors(bufferAccessor3[index1], factors);
            factors[17] = 1f;
            factors[18] = 1f;
            factors[19] = 1f;
            factors[20] = 1f;
          }
          else
            factors.Fill(1f);
          float efficiency = BuildingUtils.GetEfficiency(factors);
          if (buildingConnection.m_ProducerEdge == Entity.Null)
          {
            UnityEngine.Debug.LogError((object) "PowerPlant is missing producer edge!");
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            ElectricityFlowEdge flowEdge = this.m_FlowEdges[buildingConnection.m_ProducerEdge];
            local1.m_LastProduction = flowEdge.m_Flow;
            float num1 = local1.m_Capacity > 0 ? (float) local1.m_LastProduction / (float) local1.m_Capacity : 0.0f;
            ServiceUsage serviceUsage1;
            if (nativeArray8.Length != 0)
            {
              ref NativeArray<ServiceUsage> local2 = ref nativeArray8;
              int index2 = index1;
              serviceUsage1 = new ServiceUsage();
              serviceUsage1.m_Usage = resourceAvailability1 > (byte) 0 ? num1 : 0.0f;
              ServiceUsage serviceUsage2 = serviceUsage1;
              local2[index2] = serviceUsage2;
            }
            if (bufferAccessor1.Length != 0)
            {
              foreach (InstalledUpgrade installedUpgrade in bufferAccessor1[index1])
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive) && this.m_PowerPlantDatas.HasComponent((Entity) installedUpgrade) && this.m_ServiceUsages.HasComponent((Entity) installedUpgrade))
                {
                  Game.Buildings.ResourceConsumer componentData;
                  // ISSUE: reference to a compiler-generated field
                  byte num2 = this.m_ResourceConsumers.TryGetComponent(installedUpgrade.m_Upgrade, out componentData) ? componentData.m_ResourceAvailability : resourceAvailability1;
                  // ISSUE: reference to a compiler-generated field
                  ref ComponentLookup<ServiceUsage> local3 = ref this.m_ServiceUsages;
                  Entity entity = (Entity) installedUpgrade;
                  serviceUsage1 = new ServiceUsage();
                  serviceUsage1.m_Usage = num2 > (byte) 0 ? num1 : 0.0f;
                  ServiceUsage serviceUsage3 = serviceUsage1;
                  local3[entity] = serviceUsage3;
                }
              }
            }
            float2 zero1 = float2.zero;
            PowerPlantData componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PowerPlantDatas.TryGetComponent(prefab, out componentData1))
            {
              // ISSUE: reference to a compiler-generated method
              zero1 += PowerPlantAISystem.PowerPlantTickJob.GetPowerPlantProduction(componentData1, resourceAvailability1, efficiency);
            }
            if (bufferAccessor1.Length != 0)
            {
              foreach (InstalledUpgrade installedUpgrade in bufferAccessor1[index1])
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive) && this.m_PowerPlantDatas.TryGetComponent((Entity) this.m_Prefabs[installedUpgrade.m_Upgrade], out componentData1))
                {
                  Game.Buildings.ResourceConsumer componentData2;
                  // ISSUE: reference to a compiler-generated field
                  byte resourceAvailability2 = this.m_ResourceConsumers.TryGetComponent(installedUpgrade.m_Upgrade, out componentData2) ? componentData2.m_ResourceAvailability : resourceAvailability1;
                  // ISSUE: reference to a compiler-generated method
                  zero1 += PowerPlantAISystem.PowerPlantTickJob.GetPowerPlantProduction(componentData1, resourceAvailability2, efficiency);
                }
              }
            }
            GarbagePoweredData componentData3;
            // ISSUE: reference to a compiler-generated field
            this.m_GarbagePoweredData.TryGetComponent(prefab, out componentData3);
            WindPoweredData componentData4;
            // ISSUE: reference to a compiler-generated field
            this.m_WindPoweredData.TryGetComponent(prefab, out componentData4);
            WaterPoweredData componentData5;
            // ISSUE: reference to a compiler-generated field
            this.m_WaterPoweredData.TryGetComponent(prefab, out componentData5);
            SolarPoweredData componentData6;
            // ISSUE: reference to a compiler-generated field
            this.m_SolarPoweredData.TryGetComponent(prefab, out componentData6);
            GroundWaterPoweredData componentData7;
            // ISSUE: reference to a compiler-generated field
            this.m_GroundWaterPoweredData.TryGetComponent(prefab, out componentData7);
            if (bufferAccessor1.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<GarbagePoweredData>(ref componentData3, bufferAccessor1[index1], ref this.m_Prefabs, ref this.m_GarbagePoweredData);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<WindPoweredData>(ref componentData4, bufferAccessor1[index1], ref this.m_Prefabs, ref this.m_WindPoweredData);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<WaterPoweredData>(ref componentData5, bufferAccessor1[index1], ref this.m_Prefabs, ref this.m_WaterPoweredData);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<SolarPoweredData>(ref componentData6, bufferAccessor1[index1], ref this.m_Prefabs, ref this.m_SolarPoweredData);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<GroundWaterPoweredData>(ref componentData7, bufferAccessor1[index1], ref this.m_Prefabs, ref this.m_GroundWaterPoweredData);
            }
            float2 float2_1 = float2.zero;
            if (componentData3.m_Capacity > 0 && nativeArray2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              float2_1 = (float2) PowerPlantAISystem.PowerPlantTickJob.GetGarbageProduction(componentData3, nativeArray2[index1]);
            }
            float2 float2_2 = float2.zero;
            if (componentData4.m_Production > 0)
            {
              // ISSUE: reference to a compiler-generated field
              Wind wind = WindSystem.GetWind(nativeArray6[index1].m_Position, this.m_WindMap);
              // ISSUE: reference to a compiler-generated method
              float2_2 = PowerPlantAISystem.GetWindProduction(componentData4, wind, efficiency);
              if ((double) float2_2.x > 0.0 && nativeArray9.Length != 0 && math.any(wind.m_Wind))
              {
                ref PointOfInterest local4 = ref nativeArray9.ElementAt<PointOfInterest>(index1);
                local4.m_Position = transform.m_Position;
                local4.m_Position.xz -= wind.m_Wind;
                local4.m_IsValid = true;
              }
            }
            float2 zero2 = float2.zero;
            if (nativeArray5.Length != 0 && bufferAccessor2.Length != 0 && (double) componentData5.m_ProductionFactor > 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              zero2 += this.GetWaterProduction(componentData5, nativeArray5[index1], bufferAccessor2[index1], efficiency);
            }
            if (componentData7.m_Production > 0 && componentData7.m_MaximumGroundWater > 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              zero2 += PowerPlantAISystem.GetGroundWaterProduction(componentData7, nativeArray6[index1].m_Position, efficiency, this.m_GroundWaterMap);
            }
            float2 float2_3 = float2.zero;
            if (componentData6.m_Production > 0)
            {
              // ISSUE: reference to a compiler-generated method
              float2_3 = this.GetSolarProduction(componentData6, efficiency);
            }
            float2 float2_4 = math.round(zero1 + float2_1 + float2_2 + zero2 + float2_3);
            flowEdge.m_Capacity = local1.m_Capacity = (int) float2_4.x;
            // ISSUE: reference to a compiler-generated field
            this.m_FlowEdges[buildingConnection.m_ProducerEdge] = flowEdge;
            if (bufferAccessor3.Length != 0)
            {
              if ((double) float2_4.y > 0.0)
              {
                float4 float4 = BuildingUtils.ApproximateEfficiencyFactors(float2_4.x / float2_4.y, new float4(zero1.y - zero1.x, float2_2.y - float2_2.x, zero2.y - zero2.x, float2_3.y - float2_3.x));
                factors[17] = float4.x;
                factors[18] = float4.y;
                factors[19] = float4.z;
                factors[20] = float4.w;
              }
              BuildingUtils.SetEfficiencyFactors(bufferAccessor3[index1], factors);
            }
          }
        }
      }

      private static float2 GetPowerPlantProduction(
        PowerPlantData powerPlantData,
        byte resourceAvailability,
        float efficiency)
      {
        float y = efficiency * (float) powerPlantData.m_ElectricityProduction;
        return new float2(resourceAvailability > (byte) 0 ? y : 0.0f, y);
      }

      private static float GetGarbageProduction(
        GarbagePoweredData garbageData,
        Game.Buildings.GarbageFacility garbageFacility)
      {
        return math.clamp((float) garbageFacility.m_ProcessingRate / garbageData.m_ProductionPerUnit, 0.0f, (float) garbageData.m_Capacity);
      }

      private float2 GetWaterProduction(
        WaterPoweredData waterData,
        Game.Buildings.WaterPowered waterPowered,
        DynamicBuffer<Game.Net.SubNet> subNets,
        float efficiency)
      {
        float num1 = 0.0f;
        for (int index = 0; index < subNets.Length; ++index)
        {
          Entity subNet = subNets[index].m_SubNet;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefab = this.m_Prefabs[subNet];
          Curve componentData1;
          Composition componentData2;
          PlaceableNetData componentData3;
          NetCompositionData componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Curves.TryGetComponent(subNet, out componentData1) && this.m_Compositions.TryGetComponent(subNet, out componentData2) && this.m_PlaceableNetData.TryGetComponent(prefab.m_Prefab, out componentData3) && this.m_NetCompositionData.TryGetComponent(componentData2.m_Edge, out componentData4) && (componentData3.m_PlacementFlags & (Game.Net.PlacementFlags.FlowLeft | Game.Net.PlacementFlags.FlowRight)) != Game.Net.PlacementFlags.None && (componentData4.m_Flags.m_General & (CompositionFlags.General.Spillway | CompositionFlags.General.Front | CompositionFlags.General.Back)) == (CompositionFlags.General) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            num1 += this.GetWaterProduction(waterData, componentData1, componentData3, componentData4, this.m_TerrainHeightData, this.m_WaterSurfaceData);
          }
        }
        // ISSUE: reference to a compiler-generated method
        float num2 = efficiency * PowerPlantAISystem.GetWaterCapacity(waterPowered, waterData);
        return new float2(math.clamp(efficiency * num1, 0.0f, num2), num2);
      }

      private float GetWaterProduction(
        WaterPoweredData waterData,
        Curve curve,
        PlaceableNetData placeableData,
        NetCompositionData compositionData,
        TerrainHeightData terrainHeightData,
        WaterSurfaceData waterSurfaceData)
      {
        int num1 = math.max(1, (int) math.round(curve.m_Length * waterSurfaceData.scale.x));
        bool c = (placeableData.m_PlacementFlags & Game.Net.PlacementFlags.FlowLeft) != 0;
        float num2 = 0.0f;
        for (int index = 0; index < num1; ++index)
        {
          float t = ((float) index + 0.5f) / (float) num1;
          float3 float3_1 = MathUtils.Position(curve.m_Bezier, t);
          float3 float3_2 = MathUtils.Tangent(curve.m_Bezier, t);
          float2 y = math.normalizesafe(math.select(MathUtils.Right(float3_2.xz), MathUtils.Left(float3_2.xz), c));
          float3 worldPosition1 = float3_1;
          float3 worldPosition2 = float3_1;
          worldPosition1.xz -= y * (compositionData.m_Width * 0.5f);
          worldPosition2.xz += y * (compositionData.m_Width * 0.5f);
          float waterDepth1;
          float num3 = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, worldPosition1, out waterDepth1);
          float waterDepth2;
          float num4 = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, worldPosition2, out waterDepth2);
          float2 x1 = WaterUtils.SampleVelocity(ref waterSurfaceData, worldPosition1);
          float2 x2 = WaterUtils.SampleVelocity(ref waterSurfaceData, worldPosition2);
          if ((double) num3 > (double) worldPosition1.y)
          {
            waterDepth1 = math.max(0.0f, waterDepth1 - (num3 - worldPosition1.y));
            num3 = worldPosition1.y;
          }
          num2 += (float) (((double) math.dot(x1, y) * (double) waterDepth1 + (double) math.dot(x2, y) * (double) waterDepth2) * 0.5) * math.max(0.0f, num3 - num4);
        }
        return num2 * waterData.m_ProductionFactor * curve.m_Length / (float) num1;
      }

      private float2 GetSolarProduction(SolarPoweredData solarData, float efficiency)
      {
        float num = efficiency * (float) solarData.m_Production;
        // ISSUE: reference to a compiler-generated field
        return new float2(math.clamp(num * this.m_SunLight, 0.0f, num), num);
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.GarbageFacility> __Game_Buildings_GarbageFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ResourceConsumer> __Game_Buildings_ResourceConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterPowered> __Game_Buildings_WaterPowered_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferTypeHandle;
      public ComponentTypeHandle<ElectricityProducer> __Game_Buildings_ElectricityProducer_RW_ComponentTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      public ComponentTypeHandle<ServiceUsage> __Game_Buildings_ServiceUsage_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PointOfInterest> __Game_Common_PointOfInterest_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PowerPlantData> __Game_Prefabs_PowerPlantData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbagePoweredData> __Game_Prefabs_GarbagePoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WindPoweredData> __Game_Prefabs_WindPoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPoweredData> __Game_Prefabs_WaterPoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SolarPoweredData> __Game_Prefabs_SolarPoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GroundWaterPoweredData> __Game_Prefabs_GroundWaterPoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> __Game_Prefabs_PlaceableNetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ResourceConsumer> __Game_Buildings_ResourceConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      public ComponentLookup<ServiceUsage> __Game_Buildings_ServiceUsage_RW_ComponentLookup;
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.GarbageFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResourceConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ResourceConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterPowered_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.WaterPowered>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityProducer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUsage_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceUsage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PointOfInterest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PointOfInterest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PowerPlantData_RO_ComponentLookup = state.GetComponentLookup<PowerPlantData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbagePoweredData_RO_ComponentLookup = state.GetComponentLookup<GarbagePoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WindPoweredData_RO_ComponentLookup = state.GetComponentLookup<WindPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPoweredData_RO_ComponentLookup = state.GetComponentLookup<WaterPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SolarPoweredData_RO_ComponentLookup = state.GetComponentLookup<SolarPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentLookup = state.GetComponentLookup<GroundWaterPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResourceConsumer_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ResourceConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUsage_RW_ComponentLookup = state.GetComponentLookup<ServiceUsage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>();
      }
    }
  }
}
