// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WeatherPhenomenonSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using Game.Vehicles;
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
  public class WeatherPhenomenonSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private WindSystem m_WindSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private CitySystem m_CitySystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private ClimateRenderSystem m_ClimateRenderSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_PhenomenonQuery;
    private EntityArchetype m_FaceWeatherArchetype;
    private EntityArchetype m_ImpactArchetype;
    private EntityArchetype m_EndangerArchetype;
    private EntityArchetype m_EventIgniteArchetype;
    private EntityQuery m_EDWSBuildingQuery;
    private WeatherPhenomenonSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem = this.World.GetOrCreateSystemManaged<WindSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateRenderSystem = this.World.GetExistingSystemManaged<ClimateRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PhenomenonQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Events.WeatherPhenomenon>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_FaceWeatherArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<FaceWeather>());
      // ISSUE: reference to a compiler-generated field
      this.m_ImpactArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Impact>());
      // ISSUE: reference to a compiler-generated field
      this.m_EndangerArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Endanger>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventIgniteArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Ignite>());
      // ISSUE: reference to a compiler-generated field
      this.m_EDWSBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.EarlyDisasterWarningSystem>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PhenomenonQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DestructibleObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_FacingWeather_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_DangerLevel_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_HotspotFrame_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WeatherPhenomenon_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle deps;
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle dependencies4;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WeatherPhenomenonSystem.WeatherPhenomenonJob jobData = new WeatherPhenomenonSystem.WeatherPhenomenonJob()
      {
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_FaceWeatherArchetype = this.m_FaceWeatherArchetype,
        m_ImpactArchetype = this.m_ImpactArchetype,
        m_EndangerArchetype = this.m_EndangerArchetype,
        m_EventIgniteArchetype = this.m_EventIgniteArchetype,
        m_WindData = this.m_WindSystem.GetData(true, out dependencies1),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_City = this.m_CitySystem.City,
        m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies2),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies3),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_LightningStrikes = this.m_ClimateRenderSystem.GetLightningStrikeQueue(out dependencies4).AsParallelWriter(),
        m_EarlyDisasterWarningSystems = this.m_EDWSBuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DurationType = this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_WeatherPhenomenonType = this.__TypeHandle.__Game_Events_WeatherPhenomenon_RW_ComponentTypeHandle,
        m_HotspotFrameType = this.__TypeHandle.__Game_Events_HotspotFrame_RW_BufferTypeHandle,
        m_DangerLevelType = this.__TypeHandle.__Game_Events_DangerLevel_RW_ComponentTypeHandle,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_EmergencyShelterData = this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_InvolvedInAccidentData = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup,
        m_FacingWeatherData = this.__TypeHandle.__Game_Events_FacingWeather_RO_ComponentLookup,
        m_InDangerData = this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_WeatherPhenomenonData = this.__TypeHandle.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentLookup,
        m_TrafficAccidentData = this.__TypeHandle.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup,
        m_PrefabFireData = this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentLookup,
        m_PrefabDestructibleObjectData = this.__TypeHandle.__Game_Prefabs_DestructibleObjectData_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData.ScheduleParallel<WeatherPhenomenonSystem.WeatherPhenomenonJob>(this.m_PhenomenonQuery, JobUtils.CombineDependencies(this.Dependency, dependencies1, deps, dependencies2, dependencies3, dependencies4));
      // ISSUE: reference to a compiler-generated field
      jobData.m_EarlyDisasterWarningSystems.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ClimateRenderSystem.AddLightningStrikeWriter(jobHandle);
      this.Dependency = jobHandle;
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
    public WeatherPhenomenonSystem()
    {
    }

    [BurstCompile]
    private struct WeatherPhenomenonJob : IJobChunk
    {
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_FaceWeatherArchetype;
      [ReadOnly]
      public EntityArchetype m_ImpactArchetype;
      [ReadOnly]
      public EntityArchetype m_EndangerArchetype;
      [ReadOnly]
      public EntityArchetype m_EventIgniteArchetype;
      [ReadOnly]
      public CellMapData<Wind> m_WindData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<LightningStrike>.ParallelWriter m_LightningStrikes;
      [ReadOnly]
      public NativeArray<Entity> m_EarlyDisasterWarningSystems;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Duration> m_DurationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Game.Events.WeatherPhenomenon> m_WeatherPhenomenonType;
      public BufferTypeHandle<HotspotFrame> m_HotspotFrameType;
      public ComponentTypeHandle<Game.Events.DangerLevel> m_DangerLevelType;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.EmergencyShelter> m_EmergencyShelterData;
      [ReadOnly]
      public ComponentLookup<Car> m_CarData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> m_InvolvedInAccidentData;
      [ReadOnly]
      public ComponentLookup<FacingWeather> m_FacingWeatherData;
      [ReadOnly]
      public ComponentLookup<InDanger> m_InDangerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<WeatherPhenomenonData> m_WeatherPhenomenonData;
      [ReadOnly]
      public ComponentLookup<TrafficAccidentData> m_TrafficAccidentData;
      [ReadOnly]
      public ComponentLookup<FireData> m_PrefabFireData;
      [ReadOnly]
      public ComponentLookup<DestructibleObjectData> m_PrefabDestructibleObjectData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        float y = 0.266666681f;
        float s = math.pow(0.9f, y);
        // ISSUE: reference to a compiler-generated field
        int index1 = (int) (this.m_SimulationFrame / 16U) & 3;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Duration> nativeArray2 = chunk.GetNativeArray<Duration>(ref this.m_DurationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Events.WeatherPhenomenon> nativeArray3 = chunk.GetNativeArray<Game.Events.WeatherPhenomenon>(ref this.m_WeatherPhenomenonType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<HotspotFrame> bufferAccessor = chunk.GetBufferAccessor<HotspotFrame>(ref this.m_HotspotFrameType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Events.DangerLevel> nativeArray5 = chunk.GetNativeArray<Game.Events.DangerLevel>(ref this.m_DangerLevelType);
        for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
        {
          Entity entity = nativeArray1[index2];
          Duration duration = nativeArray2[index2];
          Game.Events.WeatherPhenomenon weatherPhenomenon = nativeArray3[index2];
          PrefabRef eventPrefabRef = nativeArray4[index2];
          // ISSUE: reference to a compiler-generated field
          WeatherPhenomenonData weatherPhenomenonData = this.m_WeatherPhenomenonData[eventPrefabRef.m_Prefab];
          float intensity = weatherPhenomenon.m_Intensity;
          // ISSUE: reference to a compiler-generated field
          if (duration.m_EndFrame <= this.m_SimulationFrame)
          {
            weatherPhenomenon.m_Intensity = math.max(0.0f, weatherPhenomenon.m_Intensity - y * 0.2f);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (duration.m_StartFrame <= this.m_SimulationFrame)
              weatherPhenomenon.m_Intensity = math.min(1f, weatherPhenomenon.m_Intensity + y * 0.2f);
          }
          // ISSUE: reference to a compiler-generated field
          float2 wind = Wind.SampleWind(this.m_WindData, weatherPhenomenon.m_PhenomenonPosition) * 20f;
          if ((double) weatherPhenomenon.m_Intensity != 0.0)
          {
            weatherPhenomenon.m_PhenomenonPosition.xz += wind * y;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            weatherPhenomenon.m_PhenomenonPosition.y = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, weatherPhenomenon.m_PhenomenonPosition);
            float max = weatherPhenomenon.m_PhenomenonRadius - weatherPhenomenon.m_HotspotRadius;
            float2 x1 = weatherPhenomenon.m_PhenomenonPosition.xz - weatherPhenomenon.m_HotspotPosition.xz;
            float2 x2 = wind + (x1 + random.NextFloat2((float2) -max, (float2) max)) * weatherPhenomenonData.m_HotspotInstability;
            weatherPhenomenon.m_HotspotVelocity.xz = math.lerp(x2, weatherPhenomenon.m_HotspotVelocity.xz, s);
            float num1 = math.length(x1);
            if ((double) num1 >= 1.0 / 1000.0)
            {
              float num2 = (max - num1) * weatherPhenomenonData.m_HotspotInstability;
              float num3 = math.dot(x1, wind - weatherPhenomenon.m_HotspotVelocity.xz) / num1;
              weatherPhenomenon.m_HotspotVelocity.xz += x1 * (math.max(0.0f, num3 - num2) / num1);
            }
            weatherPhenomenon.m_HotspotPosition += weatherPhenomenon.m_HotspotVelocity * y;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            weatherPhenomenon.m_HotspotPosition.y = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, weatherPhenomenon.m_HotspotPosition);
            if ((double) weatherPhenomenonData.m_DamageSeverity != 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.FindAffectedObjects(unfilteredChunkIndex, entity, weatherPhenomenon, weatherPhenomenonData);
            }
            float num4;
            if ((double) weatherPhenomenon.m_LightningTimer != 0.0)
            {
              for (weatherPhenomenon.m_LightningTimer -= y; (double) weatherPhenomenon.m_LightningTimer <= 0.0; weatherPhenomenon.m_LightningTimer += num4)
              {
                // ISSUE: reference to a compiler-generated method
                this.LightningStrike(ref random, unfilteredChunkIndex, entity, weatherPhenomenon, eventPrefabRef);
                num4 = random.NextFloat(weatherPhenomenonData.m_LightningInterval.min, weatherPhenomenonData.m_LightningInterval.max);
                if ((double) num4 <= 0.0)
                {
                  weatherPhenomenon.m_LightningTimer = 0.0f;
                  break;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrafficAccidentData.HasComponent(eventPrefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              TrafficAccidentData trafficAccidentData = this.m_TrafficAccidentData[eventPrefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated method
              this.FindAffectedEdges(unfilteredChunkIndex, ref random, entity, weatherPhenomenon, trafficAccidentData);
            }
          }
          else
            weatherPhenomenon.m_HotspotVelocity = (float3) 0.0f;
          if (bufferAccessor.Length != 0)
            bufferAccessor[index2][index1] = new HotspotFrame()
            {
              m_Position = weatherPhenomenon.m_HotspotPosition - weatherPhenomenon.m_HotspotVelocity * y * 0.5f,
              m_Velocity = weatherPhenomenon.m_HotspotVelocity
            };
          // ISSUE: reference to a compiler-generated field
          if (this.m_SimulationFrame < duration.m_EndFrame && weatherPhenomenonData.m_DangerFlags != (DangerFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindEndangeredObjects(unfilteredChunkIndex, entity, duration, weatherPhenomenon, wind, weatherPhenomenonData);
          }
          if ((double) intensity != 0.0 != ((double) weatherPhenomenon.m_Intensity != 0.0))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(unfilteredChunkIndex, entity, new EffectsUpdated());
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = this.m_SimulationFrame > duration.m_StartFrame && this.m_SimulationFrame < duration.m_EndFrame;
          nativeArray5[index2] = new Game.Events.DangerLevel(flag ? weatherPhenomenonData.m_DangerLevel : 0.0f);
          nativeArray3[index2] = weatherPhenomenon;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SimulationFrame > duration.m_EndFrame)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity, new Deleted());
            // ISSUE: reference to a compiler-generated field
            foreach (Entity disasterWarningSystem in this.m_EarlyDisasterWarningSystems)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<EffectsUpdated>(unfilteredChunkIndex, disasterWarningSystem, new EffectsUpdated());
            }
          }
        }
      }

      private void LightningStrike(
        ref Random random,
        int jobIndex,
        Entity eventEntity,
        Game.Events.WeatherPhenomenon weatherPhenomenon,
        PrefabRef eventPrefabRef)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WeatherPhenomenonSystem.WeatherPhenomenonJob.LightningTargetIterator iterator = new WeatherPhenomenonSystem.WeatherPhenomenonJob.LightningTargetIterator()
        {
          m_WeatherPhenomenon = weatherPhenomenon,
          m_BestDistance = float.MaxValue,
          m_BuildingData = this.m_BuildingData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<WeatherPhenomenonSystem.WeatherPhenomenonJob.LightningTargetIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        if (iterator.m_SelectedEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_LightningStrikes.Enqueue(new LightningStrike()
          {
            m_HitEntity = iterator.m_SelectedEntity,
            m_Position = iterator.m_SelectedPosition
          });
        }
        PrefabRef componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.TryGetComponent(iterator.m_SelectedEntity, out componentData1))
          return;
        bool flag = false;
        FireData componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabFireData.TryGetComponent(eventPrefabRef.m_Prefab, out componentData2))
        {
          float startProbability = componentData2.m_StartProbability;
          if ((double) startProbability > 0.0099999997764825821)
            flag = (double) random.NextFloat(100f) < (double) startProbability;
        }
        DestructibleObjectData componentData3;
        // ISSUE: reference to a compiler-generated field
        if (flag && this.m_PrefabDestructibleObjectData.TryGetComponent((Entity) componentData1, out componentData3) && (double) componentData3.m_FireHazard == 0.0)
          flag = false;
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Ignite component = new Ignite()
        {
          m_Target = iterator.m_SelectedEntity,
          m_Event = eventEntity,
          m_Intensity = componentData2.m_StartIntensity,
          m_RequestFrame = this.m_SimulationFrame
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_EventIgniteArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Ignite>(jobIndex, entity, component);
      }

      private void FindEndangeredObjects(
        int jobIndex,
        Entity eventEntity,
        Duration duration,
        Game.Events.WeatherPhenomenon weatherPhenomenon,
        float2 wind,
        WeatherPhenomenonData weatherPhenomenonData)
      {
        float y = 0.0f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        CityUtils.ApplyModifier(ref y, cityModifier, CityModifierType.DisasterWarningTime);
        // ISSUE: reference to a compiler-generated field
        if (duration.m_StartFrame > this.m_SimulationFrame)
        {
          // ISSUE: reference to a compiler-generated field
          y -= (float) (duration.m_StartFrame - this.m_SimulationFrame) / 60f;
        }
        float num = math.max(0.0f, y);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WeatherPhenomenonSystem.WeatherPhenomenonJob.EndangeredStaticObjectIterator iterator = new WeatherPhenomenonSystem.WeatherPhenomenonJob.EndangeredStaticObjectIterator()
        {
          m_JobIndex = jobIndex,
          m_DangerSpeed = math.length(wind),
          m_SimulationFrame = this.m_SimulationFrame,
          m_Event = eventEntity,
          m_Line = new Line2.Segment(weatherPhenomenon.m_PhenomenonPosition.xz, weatherPhenomenon.m_PhenomenonPosition.xz + wind * num),
          m_Radius = weatherPhenomenon.m_PhenomenonRadius,
          m_WeatherPhenomenonData = weatherPhenomenonData,
          m_BuildingData = this.m_BuildingData,
          m_EmergencyShelterData = this.m_EmergencyShelterData,
          m_InDangerData = this.m_InDangerData,
          m_EndangerArchetype = this.m_EndangerArchetype,
          m_CommandBuffer = this.m_CommandBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<WeatherPhenomenonSystem.WeatherPhenomenonJob.EndangeredStaticObjectIterator>(ref iterator);
      }

      private void FindAffectedObjects(
        int jobIndex,
        Entity eventEntity,
        Game.Events.WeatherPhenomenon weatherPhenomenon,
        WeatherPhenomenonData weatherPhenomenonData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WeatherPhenomenonSystem.WeatherPhenomenonJob.AffectedStaticObjectIterator iterator = new WeatherPhenomenonSystem.WeatherPhenomenonJob.AffectedStaticObjectIterator()
        {
          m_JobIndex = jobIndex,
          m_Event = eventEntity,
          m_Circle = new Circle2(weatherPhenomenon.m_HotspotRadius, weatherPhenomenon.m_HotspotPosition.xz),
          m_WeatherPhenomenon = weatherPhenomenon,
          m_WeatherPhenomenonData = weatherPhenomenonData,
          m_BuildingData = this.m_BuildingData,
          m_TransformData = this.m_TransformData,
          m_DestroyedData = this.m_DestroyedData,
          m_FacingWeatherData = this.m_FacingWeatherData,
          m_FaceWeatherArchetype = this.m_FaceWeatherArchetype,
          m_CommandBuffer = this.m_CommandBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<WeatherPhenomenonSystem.WeatherPhenomenonJob.AffectedStaticObjectIterator>(ref iterator);
      }

      private void FindAffectedEdges(
        int jobIndex,
        ref Random random,
        Entity eventEntity,
        Game.Events.WeatherPhenomenon weatherPhenomenon,
        TrafficAccidentData trafficAccidentData)
      {
        float num = math.sqrt(trafficAccidentData.m_OccurenceProbability * 0.01f);
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
        WeatherPhenomenonSystem.WeatherPhenomenonJob.AffectedNetIterator iterator = new WeatherPhenomenonSystem.WeatherPhenomenonJob.AffectedNetIterator()
        {
          m_JobIndex = jobIndex,
          m_Event = eventEntity,
          m_Circle = new Circle2(weatherPhenomenon.m_HotspotRadius, weatherPhenomenon.m_HotspotPosition.xz),
          m_WeatherPhenomenon = weatherPhenomenon,
          m_TrafficAccidentData = trafficAccidentData,
          m_Random = random,
          m_DividedProbability = num,
          m_InvolvedInAccidentData = this.m_InvolvedInAccidentData,
          m_CarData = this.m_CarData,
          m_MovingData = this.m_MovingData,
          m_TransformData = this.m_TransformData,
          m_SubLanes = this.m_SubLanes,
          m_LaneObjects = this.m_LaneObjects,
          m_EventImpactArchetype = this.m_ImpactArchetype,
          m_CommandBuffer = this.m_CommandBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<WeatherPhenomenonSystem.WeatherPhenomenonJob.AffectedNetIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        random = iterator.m_Random;
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

      private struct LightningTargetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Game.Events.WeatherPhenomenon m_WeatherPhenomenon;
        public Entity m_SelectedEntity;
        public float3 m_SelectedPosition;
        public float m_BestDistance;
        public ComponentLookup<Building> m_BuildingData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          float num = MathUtils.Distance(bounds.m_Bounds.xz, this.m_WeatherPhenomenon.m_HotspotPosition.xz);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return (double) num < (double) this.m_WeatherPhenomenon.m_HotspotRadius && (double) num * 0.5 - (double) bounds.m_Bounds.max.y < (double) this.m_BestDistance;
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          float2 x = MathUtils.Center(bounds.m_Bounds.xz);
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(x, this.m_WeatherPhenomenon.m_HotspotPosition.xz);
          // ISSUE: reference to a compiler-generated field
          if ((double) num1 >= (double) this.m_WeatherPhenomenon.m_HotspotRadius)
            return;
          float num2 = num1 * 0.5f - bounds.m_Bounds.max.y;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) num2 >= (double) this.m_BestDistance || (bounds.m_Mask & BoundsMask.IsTree) == (BoundsMask) 0 && !this.m_BuildingData.HasComponent(item))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedEntity = item;
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedPosition = new float3(x.x, bounds.m_Bounds.max.y, x.y);
          // ISSUE: reference to a compiler-generated field
          this.m_BestDistance = num2;
        }
      }

      private struct EndangeredStaticObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public int m_JobIndex;
        public uint m_SimulationFrame;
        public float m_DangerSpeed;
        public Entity m_Event;
        public Line2.Segment m_Line;
        public float m_Radius;
        public WeatherPhenomenonData m_WeatherPhenomenonData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<Game.Buildings.EmergencyShelter> m_EmergencyShelterData;
        public ComponentLookup<InDanger> m_InDangerData;
        public EntityArchetype m_EndangerArchetype;
        public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds.xz, (float2) this.m_Radius), this.m_Line, out float2 _);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds.xz, (float2) this.m_Radius), this.m_Line, out float2 _))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, new Circle2(this.m_Radius, this.m_Line.a)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2 forward = this.m_Line.b - this.m_Line.a;
            if (!MathUtils.TryNormalize(ref forward))
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(bounds.m_Bounds.xz, new Circle2(this.m_Radius, this.m_Line.b)))
            {
              float2 float2 = MathUtils.Right(forward);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Quad2 quad = new Quad2(this.m_Line.a - float2, this.m_Line.a + float2, this.m_Line.b + float2, this.m_Line.b - float2);
              if (!MathUtils.Intersect(bounds.m_Bounds.xz, quad))
                return;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_BuildingData.HasComponent(item))
            return;
          // ISSUE: reference to a compiler-generated field
          DangerFlags flags = this.m_WeatherPhenomenonData.m_DangerFlags;
          // ISSUE: reference to a compiler-generated field
          if ((flags & DangerFlags.Evacuate) != (DangerFlags) 0 && this.m_EmergencyShelterData.HasComponent(item))
            flags = flags & ~DangerFlags.Evacuate | DangerFlags.StayIndoors;
          // ISSUE: reference to a compiler-generated field
          if (this.m_InDangerData.HasComponent(item))
          {
            // ISSUE: reference to a compiler-generated field
            InDanger inDanger = this.m_InDangerData[item];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (inDanger.m_EndFrame >= this.m_SimulationFrame + 64U && (inDanger.m_Event == this.m_Event || !EventUtils.IsWorse(flags, inDanger.m_Flags)))
              return;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num = (float) (30.0 + (double) math.max(this.m_Radius, MathUtils.Distance(bounds.m_Bounds.xz, this.m_Line.a)) / (double) this.m_DangerSpeed);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Endanger>(this.m_JobIndex, this.m_CommandBuffer.CreateEntity(this.m_JobIndex, this.m_EndangerArchetype), new Endanger()
          {
            m_Event = this.m_Event,
            m_Target = item,
            m_Flags = flags,
            m_EndFrame = this.m_SimulationFrame + 64U + (uint) ((double) num * 60.0)
          });
        }
      }

      private struct AffectedStaticObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public int m_JobIndex;
        public Entity m_Event;
        public Circle2 m_Circle;
        public Game.Events.WeatherPhenomenon m_WeatherPhenomenon;
        public WeatherPhenomenonData m_WeatherPhenomenonData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<Destroyed> m_DestroyedData;
        public ComponentLookup<FacingWeather> m_FacingWeatherData;
        public EntityArchetype m_FaceWeatherArchetype;
        public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Circle);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Circle) || !this.m_BuildingData.HasComponent(item))
            return;
          float num = 0.0f;
          // ISSUE: reference to a compiler-generated field
          if (this.m_FacingWeatherData.HasComponent(item))
          {
            // ISSUE: reference to a compiler-generated field
            FacingWeather facingWeather = this.m_FacingWeatherData[item];
            // ISSUE: reference to a compiler-generated field
            if (facingWeather.m_Event == this.m_Event)
              return;
            num = facingWeather.m_Severity;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_DestroyedData.HasComponent(item))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float severity = EventUtils.GetSeverity(this.m_TransformData[item].m_Position, this.m_WeatherPhenomenon, this.m_WeatherPhenomenonData);
          if ((double) severity <= (double) num)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<FaceWeather>(this.m_JobIndex, this.m_CommandBuffer.CreateEntity(this.m_JobIndex, this.m_FaceWeatherArchetype), new FaceWeather()
          {
            m_Event = this.m_Event,
            m_Target = item,
            m_Severity = severity
          });
        }
      }

      private struct AffectedNetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public int m_JobIndex;
        public Entity m_Event;
        public Circle2 m_Circle;
        public Game.Events.WeatherPhenomenon m_WeatherPhenomenon;
        public TrafficAccidentData m_TrafficAccidentData;
        public Random m_Random;
        public float m_DividedProbability;
        public ComponentLookup<InvolvedInAccident> m_InvolvedInAccidentData;
        public ComponentLookup<Car> m_CarData;
        public ComponentLookup<Moving> m_MovingData;
        public ComponentLookup<Transform> m_TransformData;
        public BufferLookup<Game.Net.SubLane> m_SubLanes;
        public BufferLookup<LaneObject> m_LaneObjects;
        public EntityArchetype m_EventImpactArchetype;
        public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Circle);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Circle) || (double) this.m_Random.NextFloat(1f) >= (double) this.m_DividedProbability)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          Entity subject = this.TryFindSubject(item, ref this.m_Random, this.m_TrafficAccidentData);
          if (!(subject != Entity.Null))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num = this.m_DividedProbability * (this.m_WeatherPhenomenon.m_HotspotRadius - math.distance(this.m_TransformData[subject].m_Position.xz, this.m_WeatherPhenomenon.m_HotspotPosition.xz)) * 0.266666681f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_Random.NextFloat(this.m_WeatherPhenomenon.m_HotspotRadius) >= (double) num)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddImpact(this.m_JobIndex, this.m_Event, ref this.m_Random, subject, this.m_TrafficAccidentData);
        }

        private Entity TryFindSubject(
          Entity entity,
          ref Random random,
          TrafficAccidentData trafficAccidentData)
        {
          Entity subject = Entity.Null;
          int max = 0;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubLanes.HasBuffer(entity))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[entity];
            for (int index1 = 0; index1 < subLane1.Length; ++index1)
            {
              Entity subLane2 = subLane1[index1].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneObjects.HasBuffer(subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<LaneObject> laneObject1 = this.m_LaneObjects[subLane2];
                for (int index2 = 0; index2 < laneObject1.Length; ++index2)
                {
                  Entity laneObject2 = laneObject1[index2].m_LaneObject;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (trafficAccidentData.m_SubjectType == EventTargetType.MovingCar && this.m_CarData.HasComponent(laneObject2) && this.m_MovingData.HasComponent(laneObject2) && !this.m_InvolvedInAccidentData.HasComponent(laneObject2))
                  {
                    ++max;
                    if (random.NextInt(max) == max - 1)
                      subject = laneObject2;
                  }
                }
              }
            }
          }
          return subject;
        }

        private void AddImpact(
          int jobIndex,
          Entity eventEntity,
          ref Random random,
          Entity target,
          TrafficAccidentData trafficAccidentData)
        {
          Impact component = new Impact()
          {
            m_Event = eventEntity,
            m_Target = target
          };
          // ISSUE: reference to a compiler-generated field
          if (trafficAccidentData.m_AccidentType == TrafficAccidentType.LoseControl && this.m_MovingData.HasComponent(target))
          {
            // ISSUE: reference to a compiler-generated field
            Moving moving = this.m_MovingData[target];
            component.m_Severity = 5f;
            if (random.NextBool())
            {
              component.m_AngularVelocityDelta.y = -2f;
              component.m_VelocityDelta.xz = component.m_Severity * MathUtils.Left(math.normalizesafe(moving.m_Velocity.xz));
            }
            else
            {
              component.m_AngularVelocityDelta.y = 2f;
              component.m_VelocityDelta.xz = component.m_Severity * MathUtils.Right(math.normalizesafe(moving.m_Velocity.xz));
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_EventImpactArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Impact>(jobIndex, entity, component);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Duration> __Game_Events_Duration_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Events.WeatherPhenomenon> __Game_Events_WeatherPhenomenon_RW_ComponentTypeHandle;
      public BufferTypeHandle<HotspotFrame> __Game_Events_HotspotFrame_RW_BufferTypeHandle;
      public ComponentTypeHandle<Game.Events.DangerLevel> __Game_Events_DangerLevel_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.EmergencyShelter> __Game_Buildings_EmergencyShelter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Car> __Game_Vehicles_Car_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FacingWeather> __Game_Events_FacingWeather_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InDanger> __Game_Events_InDanger_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WeatherPhenomenonData> __Game_Prefabs_WeatherPhenomenonData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrafficAccidentData> __Game_Prefabs_TrafficAccidentData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FireData> __Game_Prefabs_FireData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DestructibleObjectData> __Game_Prefabs_DestructibleObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WeatherPhenomenon_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Events.WeatherPhenomenon>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_HotspotFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<HotspotFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_DangerLevel_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Events.DangerLevel>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_EmergencyShelter_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.EmergencyShelter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentLookup = state.GetComponentLookup<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentLookup = state.GetComponentLookup<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_FacingWeather_RO_ComponentLookup = state.GetComponentLookup<FacingWeather>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InDanger_RO_ComponentLookup = state.GetComponentLookup<InDanger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WeatherPhenomenonData_RO_ComponentLookup = state.GetComponentLookup<WeatherPhenomenonData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrafficAccidentData_RO_ComponentLookup = state.GetComponentLookup<TrafficAccidentData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FireData_RO_ComponentLookup = state.GetComponentLookup<FireData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DestructibleObjectData_RO_ComponentLookup = state.GetComponentLookup<DestructibleObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
