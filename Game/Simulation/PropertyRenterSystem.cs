// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PropertyRenterSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
using Game.Zones;
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
  public class PropertyRenterSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 16;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ServiceFeeSystem m_ServiceFeeSystem;
    private EntityQuery m_BuildingGroup;
    private EntityQuery m_GarbageFacilityGroup;
    private EntityQuery m_MovingAwayHouseholdGroup;
    private EntityArchetype m_RentEventArchetype;
    private PropertyRenterSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_595560377_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (PropertyRenterSystem.kUpdatesPerDay * 16);
    }

    public static int GetUpkeep(
      int level,
      float baseUpkeep,
      int lotSize,
      AreaType areaType,
      ref EconomyParameterData economyParameterData,
      bool isStorage = false)
    {
      double num;
      switch (areaType)
      {
        case AreaType.Residential:
          return Mathf.RoundToInt(math.pow((float) level, economyParameterData.m_ResidentialUpkeepLevelExponent) * baseUpkeep * (float) lotSize);
        case AreaType.Commercial:
          num = (double) economyParameterData.m_CommercialUpkeepLevelExponent;
          break;
        case AreaType.Industrial:
          num = (double) economyParameterData.m_IndustrialUpkeepLevelExponent;
          break;
        default:
          num = 1.0;
          break;
      }
      float y = (float) num;
      return Mathf.RoundToInt((float) ((double) math.pow((float) level, y) * (double) baseUpkeep * (double) lotSize * (isStorage ? 0.5 : 1.0)));
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (!(reader.context.version >= Version.taxRateArrayLength) || !(reader.context.version < Version.economyFix))
        return;
      reader.Read(out Entity _);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeSystem = this.World.GetOrCreateSystemManaged<ServiceFeeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Renter>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadWrite<BuildingCondition>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageFacilityGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.GarbageFacility>(), ComponentType.Exclude<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingAwayHouseholdGroup = this.GetEntityQuery(ComponentType.ReadOnly<Household>(), ComponentType.ReadOnly<MovingAway>(), ComponentType.ReadOnly<PropertyRenter>());
      // ISSUE: reference to a compiler-generated field
      this.m_RentEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<RentersUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy() => base.OnDestroy();

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, PropertyRenterSystem.kUpdatesPerDay, 16);
      bool flag = false;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_GarbageFacilityGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        Building component;
        if (this.EntityManager.TryGetComponent<Building>(entityArray[index], out component) && !BuildingUtils.CheckOption(component, BuildingOption.Inactive))
        {
          flag = true;
          break;
        }
      }
      entityArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = new PropertyRenterSystem.RenterMovingAwayJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.Schedule<PropertyRenterSystem.RenterMovingAwayJob>(this.m_MovingAwayHouseholdGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = new PropertyRenterSystem.PayRentJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RW_BufferTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.GetSharedComponentTypeHandle<UpdateFrame>(),
        m_SpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_ZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_BuildingProperties = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_PropertiesOnMarket = this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup,
        m_Abandoned = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
        m_Destroyed = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_Storages = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_RentEventArchetype = this.m_RentEventArchetype,
        m_RandomSeed = RandomSeed.Next(),
        m_FeeParameters = this.__query_595560377_0.GetSingleton<ServiceFeeParameterData>(),
        m_UpdateFrameIndex = updateFrame,
        m_ProvidedGarbageService = flag,
        m_FeeQueue = this.m_ServiceFeeSystem.GetFeeQueue(out deps1).AsParallelWriter(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps2).AsParallelWriter(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<PropertyRenterSystem.PayRentJob>(this.m_BuildingGroup, JobHandle.CombineDependencies(jobHandle1, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ServiceFeeSystem.AddQueueWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle2);
      this.Dependency = jobHandle2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_595560377_0 = state.GetEntityQuery(new EntityQueryDesc()
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
    public PropertyRenterSystem()
    {
    }

    [BurstCompile]
    private struct PayRentJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingProperties;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> m_PropertiesOnMarket;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_Abandoned;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_Destroyed;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_Storages;
      public RandomSeed m_RandomSeed;
      public NativeQueue<ServiceFeeSystem.FeeEvent>.ParallelWriter m_FeeQueue;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public EntityArchetype m_RentEventArchetype;
      public bool m_ProvidedGarbageService;
      public ServiceFeeParameterData m_FeeParameters;
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
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(1 + unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          DynamicBuffer<Renter> dynamicBuffer = bufferAccessor[index1];
          Entity prefab = nativeArray2[index1].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpawnableBuildingData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            SpawnableBuildingData spawnableBuildingData = this.m_SpawnableBuildingData[prefab];
            // ISSUE: reference to a compiler-generated field
            AreaType areaType = this.m_ZoneData[spawnableBuildingData.m_ZonePrefab].m_AreaType;
            // ISSUE: reference to a compiler-generated field
            bool flag1 = (this.m_ZoneData[spawnableBuildingData.m_ZonePrefab].m_ZoneFlags & ZoneFlags.Office) != 0;
            // ISSUE: reference to a compiler-generated field
            BuildingPropertyData buildingProperty = this.m_BuildingProperties[prefab];
            int num1 = 0;
            switch (areaType)
            {
              case AreaType.Residential:
                // ISSUE: reference to a compiler-generated field
                num1 = this.m_FeeParameters.m_GarbageFeeRCIO.x;
                break;
              case AreaType.Commercial:
                // ISSUE: reference to a compiler-generated field
                num1 = this.m_FeeParameters.m_GarbageFeeRCIO.y;
                break;
              case AreaType.Industrial:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                num1 = !flag1 ? this.m_FeeParameters.m_GarbageFeeRCIO.w : this.m_FeeParameters.m_GarbageFeeRCIO.z;
                break;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ProvidedGarbageService)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
              {
                m_Statistic = StatisticType.Income,
                m_Change = 1f * (float) num1 / (float) PropertyRenterSystem.kUpdatesPerDay,
                m_Parameter = 12
              });
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_FeeQueue.Enqueue(new ServiceFeeSystem.FeeEvent()
              {
                m_Amount = 1f,
                m_Cost = 1f * (float) num1 / (float) PropertyRenterSystem.kUpdatesPerDay,
                m_Outside = false,
                m_Resource = PlayerResource.Garbage
              });
            }
            int intRandom = MathUtils.RoundToIntRandom(ref random, 1f * (float) num1 / (float) dynamicBuffer.Length);
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity renter = dynamicBuffer[index2].m_Renter;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PropertyRenters.HasComponent(renter))
              {
                // ISSUE: reference to a compiler-generated field
                PropertyRenter propertyRenter = this.m_PropertyRenters[renter];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                EconomyUtils.AddResources(Resource.Money, -(!this.m_Storages.HasComponent(renter) ? MathUtils.RoundToIntRandom(ref random, (float) propertyRenter.m_Rent * 1f / (float) PropertyRenterSystem.kUpdatesPerDay) : EconomyUtils.GetResources(Resource.Money, this.m_Resources[renter])), this.m_Resources[renter]);
                // ISSUE: reference to a compiler-generated field
                if (!this.m_Storages.HasComponent(renter))
                {
                  // ISSUE: reference to a compiler-generated field
                  EconomyUtils.AddResources(Resource.Money, -intRandom, this.m_Resources[renter]);
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool flag2 = !this.m_Abandoned.HasComponent(nativeArray1[index1]) && !this.m_Destroyed.HasComponent(nativeArray1[index1]);
            bool flag3 = false;
            for (int index3 = dynamicBuffer.Length - 1; index3 >= 0; --index3)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PropertyRenters.HasComponent(dynamicBuffer[index3].m_Renter))
              {
                dynamicBuffer.RemoveAt(index3);
                flag3 = true;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (((dynamicBuffer.Length >= buildingProperty.CountProperties() ? 0 : (!this.m_PropertiesOnMarket.HasComponent(nativeArray1[index1]) ? 1 : 0)) & (flag2 ? 1 : 0)) != 0 && !chunk.Has<Signature>())
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PropertyToBeOnMarket>(unfilteredChunkIndex, nativeArray1[index1], new PropertyToBeOnMarket());
            }
            int num2 = buildingProperty.CountProperties();
            while (dynamicBuffer.Length > 0 && !flag2 || dynamicBuffer.Length > num2)
            {
              Entity renter = dynamicBuffer[dynamicBuffer.Length - 1].m_Renter;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PropertyRenters.HasComponent(renter))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<PropertyRenter>(unfilteredChunkIndex, renter);
              }
              dynamicBuffer.RemoveAt(dynamicBuffer.Length - 1);
              flag3 = true;
            }
            if (flag3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_RentEventArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RentersUpdated>(unfilteredChunkIndex, entity, new RentersUpdated(nativeArray1[index1]));
            }
          }
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
    private struct RenterMovingAwayJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Entity e = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<PropertyRenter>(unfilteredChunkIndex, e);
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
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> __Game_Buildings_PropertyOnMarket_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RW_BufferTypeHandle = state.GetBufferTypeHandle<Renter>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup = state.GetComponentLookup<PropertyOnMarket>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
      }
    }
  }
}
