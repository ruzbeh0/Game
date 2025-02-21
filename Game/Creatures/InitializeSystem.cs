// Decompiled with JetBrains decompiler
// Type: Game.Creatures.InitializeSystem
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
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Creatures
{
  [CompilerGenerated]
  public class InitializeSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ClimateSystem m_ClimateSystem;
    private EntityQuery m_CreatureQuery;
    private ComponentTypeSet m_TripSourceRemoveTypes;
    private InitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadOnly<Creature>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_TripSourceRemoveTypes = new ComponentTypeSet(ComponentType.ReadWrite<TripSource>(), ComponentType.ReadWrite<Unspawned>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatureQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      InitializeSystem.InitializeCreaturesJob jobData = new InitializeSystem.InitializeCreaturesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TripSourceType = this.__TypeHandle.__Game_Objects_TripSource_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle,
        m_HumanType = this.__TypeHandle.__Game_Creatures_Human_RW_ComponentTypeHandle,
        m_AnimalType = this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentTypeHandle,
        m_HumanCurrentLaneType = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RW_ComponentTypeHandle,
        m_AnimalCurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle,
        m_HumanNavigationType = this.__TypeHandle.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle,
        m_AnimalNavigationType = this.__TypeHandle.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_HouseholdMemberData = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_HomelessHousehold = this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup,
        m_WorkerData = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_AnimalData = this.__TypeHandle.__Game_Prefabs_AnimalData_RO_ComponentLookup,
        m_SpawnLocations = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_TripSourceRemoveTypes = this.m_TripSourceRemoveTypes,
        m_Temperature = (float) this.m_ClimateSystem.temperature,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<InitializeSystem.InitializeCreaturesJob>(this.m_CreatureQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
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
    public InitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeCreaturesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<TripSource> m_TripSourceType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      public ComponentTypeHandle<Human> m_HumanType;
      public ComponentTypeHandle<Animal> m_AnimalType;
      public ComponentTypeHandle<HumanCurrentLane> m_HumanCurrentLaneType;
      public ComponentTypeHandle<AnimalCurrentLane> m_AnimalCurrentLaneType;
      public ComponentTypeHandle<HumanNavigation> m_HumanNavigationType;
      public ComponentTypeHandle<AnimalNavigation> m_AnimalNavigationType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Resident> m_ResidentData;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMemberData;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> m_HomelessHousehold;
      [ReadOnly]
      public ComponentLookup<Worker> m_WorkerData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<AnimalData> m_AnimalData;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> m_SpawnLocations;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public ComponentTypeSet m_TripSourceRemoveTypes;
      [ReadOnly]
      public float m_Temperature;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Human> nativeArray2 = chunk.GetNativeArray<Human>(ref this.m_HumanType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanCurrentLane> nativeArray3 = chunk.GetNativeArray<HumanCurrentLane>(ref this.m_HumanCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalCurrentLane> nativeArray4 = chunk.GetNativeArray<AnimalCurrentLane>(ref this.m_AnimalCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanNavigation> nativeArray5 = chunk.GetNativeArray<HumanNavigation>(ref this.m_HumanNavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalNavigation> nativeArray6 = chunk.GetNativeArray<AnimalNavigation>(ref this.m_AnimalNavigationType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        if (nativeArray5.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<CurrentVehicle> nativeArray7 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TripSource> nativeArray8 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathOwner> nativeArray9 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
          for (int index = 0; index < nativeArray5.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            if (flag && nativeArray8.Length != 0)
            {
              TripSource tripSource = nativeArray8[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_DeletedData.HasComponent(tripSource.m_Source))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, entity, in this.m_TripSourceRemoveTypes);
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnLocations.HasBuffer(tripSource.m_Source))
              {
                PathOwner pathOwner = nativeArray9[index];
                DynamicBuffer<PathElement> path = bufferAccessor[index];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<SpawnLocationElement> spawnLocation = this.m_SpawnLocations[tripSource.m_Source];
                // ISSUE: reference to a compiler-generated method
                Transform pathTransform = this.CalculatePathTransform(entity, pathOwner, path);
                float3 spawnPosition;
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                if (!this.FindClosestSpawnLocation(pathTransform.m_Position, out spawnPosition, spawnLocation, path.Length == 0, ref random, this.HasAuthorization(entity, tripSource.m_Source)))
                {
                  // ISSUE: reference to a compiler-generated field
                  Transform transform = this.m_TransformData[tripSource.m_Source];
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[tripSource.m_Source];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_BuildingData.HasComponent(prefabRef.m_Prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Game.Prefabs.BuildingData buildingData = this.m_BuildingData[prefabRef.m_Prefab];
                    spawnPosition = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y);
                  }
                  else
                    spawnPosition = transform.m_Position;
                }
                float3 forward = pathTransform.m_Position - spawnPosition;
                if (MathUtils.TryNormalize(ref forward))
                {
                  pathTransform.m_Position = spawnPosition;
                  pathTransform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
                }
                // ISSUE: reference to a compiler-generated field
                this.m_TransformData[entity] = pathTransform;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.HasComponent(tripSource.m_Source))
                {
                  PathOwner pathOwner = nativeArray9[index];
                  DynamicBuffer<PathElement> path = bufferAccessor[index];
                  // ISSUE: reference to a compiler-generated field
                  Transform transform = this.m_TransformData[tripSource.m_Source];
                  // ISSUE: reference to a compiler-generated method
                  Transform pathTransform = this.CalculatePathTransform(entity, pathOwner, path);
                  float3 forward = pathTransform.m_Position - transform.m_Position;
                  if (MathUtils.TryNormalize(ref forward))
                  {
                    pathTransform.m_Position = transform.m_Position;
                    pathTransform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_TransformData[entity] = pathTransform;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            Transform transform1 = this.m_TransformData[entity];
            HumanNavigation humanNavigation = new HumanNavigation();
            humanNavigation.m_TargetPosition = transform1.m_Position;
            humanNavigation.m_TargetDirection = math.normalizesafe(math.forward(transform1.m_Rotation).xz);
            CurrentVehicle currentVehicle;
            HumanCurrentLane humanCurrentLane;
            if (CollectionUtils.TryGet<CurrentVehicle>(nativeArray7, index, out currentVehicle) && CollectionUtils.TryGet<HumanCurrentLane>(nativeArray3, index, out humanCurrentLane) && (currentVehicle.m_Flags & CreatureVehicleFlags.Exiting) != (CreatureVehicleFlags) 0 && (humanCurrentLane.m_Flags & CreatureLaneFlags.EndOfPath) != (CreatureLaneFlags) 0)
            {
              humanNavigation.m_TransformState = TransformState.Action;
              humanNavigation.m_LastActivity = (byte) 11;
              humanNavigation.m_TargetActivity = (byte) 11;
            }
            nativeArray5[index] = humanNavigation;
          }
        }
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PseudoRandomSeed> nativeArray10 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Human human = nativeArray2[index];
            human.m_Flags &= ~(HumanFlags.Cold | HumanFlags.Homeless);
            PseudoRandomSeed pseudoRandomSeed;
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_Temperature < (!CollectionUtils.TryGet<PseudoRandomSeed>(nativeArray10, index, out pseudoRandomSeed) ? (double) random.NextFloat(15f, 20f) : (double) pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kTemperatureLimit).NextFloat(15f, 20f)))
              human.m_Flags |= HumanFlags.Cold;
            Resident componentData1;
            HouseholdMember componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ResidentData.TryGetComponent(nativeArray1[index], out componentData1) && this.m_HouseholdMemberData.TryGetComponent(componentData1.m_Citizen, out componentData2) && this.m_HomelessHousehold.HasComponent(componentData2.m_Household))
              human.m_Flags |= HumanFlags.Homeless;
            nativeArray2[index] = human;
          }
        }
        if (nativeArray3.Length != 0)
        {
          for (int index = 0; index < nativeArray3.Length; ++index)
          {
            HumanCurrentLane humanCurrentLane = nativeArray3[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(humanCurrentLane.m_Lane))
            {
              humanCurrentLane.m_Flags |= CreatureLaneFlags.TransformTarget;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectionLaneData.HasComponent(humanCurrentLane.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                if ((this.m_ConnectionLaneData[humanCurrentLane.m_Lane].m_Flags & ConnectionLaneFlags.Area) != (ConnectionLaneFlags) 0)
                  humanCurrentLane.m_Flags |= CreatureLaneFlags.Area;
                else
                  humanCurrentLane.m_Flags |= CreatureLaneFlags.Connection;
              }
            }
            humanCurrentLane.m_LanePosition = random.NextFloat(0.0f, 1f);
            humanCurrentLane.m_LanePosition *= humanCurrentLane.m_LanePosition;
            // ISSUE: reference to a compiler-generated field
            humanCurrentLane.m_LanePosition = math.select(0.5f - humanCurrentLane.m_LanePosition, humanCurrentLane.m_LanePosition - 0.5f, this.m_LeftHandTraffic != (humanCurrentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0);
            nativeArray3[index] = humanCurrentLane;
          }
        }
        if (nativeArray4.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Animal> nativeArray11 = chunk.GetNativeArray<Animal>(ref this.m_AnimalType);
          for (int index = 0; index < nativeArray4.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Animal animal = nativeArray11[index];
            AnimalCurrentLane animalCurrentLane = nativeArray4[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            AnimalData animalData = this.m_AnimalData[this.m_PrefabRefData[entity].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(animalCurrentLane.m_Lane))
            {
              animalCurrentLane.m_Flags |= CreatureLaneFlags.TransformTarget;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectionLaneData.HasComponent(animalCurrentLane.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                if ((this.m_ConnectionLaneData[animalCurrentLane.m_Lane].m_Flags & ConnectionLaneFlags.Area) != (ConnectionLaneFlags) 0)
                  animalCurrentLane.m_Flags |= CreatureLaneFlags.Area;
                else
                  animalCurrentLane.m_Flags |= CreatureLaneFlags.Connection;
              }
            }
            if ((double) animalData.m_MoveSpeed == 0.0 && (double) animalData.m_SwimSpeed > 0.0)
            {
              animal.m_Flags |= AnimalFlags.SwimmingTarget;
              animalCurrentLane.m_Flags |= CreatureLaneFlags.Swimming;
            }
            if ((animal.m_Flags & AnimalFlags.Roaming) != (AnimalFlags) 0)
            {
              animalCurrentLane.m_LanePosition = random.NextFloat(-0.5f, 0.5f);
            }
            else
            {
              animalCurrentLane.m_LanePosition = random.NextFloat(0.0f, 1f);
              animalCurrentLane.m_LanePosition *= animalCurrentLane.m_LanePosition;
              // ISSUE: reference to a compiler-generated field
              animalCurrentLane.m_LanePosition = math.select(0.5f - animalCurrentLane.m_LanePosition, animalCurrentLane.m_LanePosition - 0.5f, this.m_LeftHandTraffic != (animalCurrentLane.m_Flags & CreatureLaneFlags.Backward) > (CreatureLaneFlags) 0);
            }
            nativeArray11[index] = animal;
            nativeArray4[index] = animalCurrentLane;
          }
        }
        if (nativeArray6.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<TripSource> nativeArray12 = chunk.GetNativeArray<TripSource>(ref this.m_TripSourceType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray6.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          if (flag1 && nativeArray12.Length != 0)
          {
            TripSource tripSource = nativeArray12[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeletedData.HasComponent(tripSource.m_Source))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, entity, in this.m_TripSourceRemoveTypes);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnLocations.HasBuffer(tripSource.m_Source))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<SpawnLocationElement> spawnLocation = this.m_SpawnLocations[tripSource.m_Source];
              // ISSUE: reference to a compiler-generated field
              Transform transform2 = this.m_TransformData[entity];
              float3 spawnPosition;
              // ISSUE: reference to a compiler-generated method
              if (!this.FindClosestSpawnLocation(transform2.m_Position, out spawnPosition, spawnLocation, false, ref random, false))
              {
                // ISSUE: reference to a compiler-generated field
                Transform transform3 = this.m_TransformData[tripSource.m_Source];
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef = this.m_PrefabRefData[tripSource.m_Source];
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingData.HasComponent(prefabRef.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Prefabs.BuildingData buildingData = this.m_BuildingData[prefabRef.m_Prefab];
                  spawnPosition = BuildingUtils.CalculateFrontPosition(transform3, buildingData.m_LotSize.y);
                }
                else
                  spawnPosition = transform3.m_Position;
              }
              float3 forward = transform2.m_Position - spawnPosition;
              if (MathUtils.TryNormalize(ref forward))
              {
                transform2.m_Position = spawnPosition;
                transform2.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
              }
              // ISSUE: reference to a compiler-generated field
              this.m_TransformData[entity] = transform2;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(tripSource.m_Source))
              {
                // ISSUE: reference to a compiler-generated field
                Transform transform4 = this.m_TransformData[tripSource.m_Source];
                // ISSUE: reference to a compiler-generated field
                Transform transform5 = this.m_TransformData[entity];
                float3 forward = transform5.m_Position - transform4.m_Position;
                if (MathUtils.TryNormalize(ref forward))
                {
                  transform5.m_Position = transform4.m_Position;
                  transform5.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
                }
                if (nativeArray4.Length != 0 && (nativeArray4[index].m_Flags & CreatureLaneFlags.Swimming) != (CreatureLaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  AnimalData animalData = this.m_AnimalData[this.m_PrefabRefData[entity].m_Prefab];
                  transform5.m_Position.y -= animalData.m_SwimDepth.min;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_TransformData[entity] = transform5;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          Transform transform = this.m_TransformData[entity];
          nativeArray6[index] = new AnimalNavigation()
          {
            m_TargetPosition = transform.m_Position,
            m_TargetDirection = math.normalizesafe(math.forward(transform.m_Rotation))
          };
        }
      }

      private Transform CalculatePathTransform(
        Entity creature,
        PathOwner pathOwner,
        DynamicBuffer<PathElement> path)
      {
        // ISSUE: reference to a compiler-generated field
        Transform pathTransform = this.m_TransformData[creature];
        if (path.Length > pathOwner.m_ElementIndex)
        {
          PathElement pathElement = path[pathOwner.m_ElementIndex];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.HasComponent(pathElement.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[pathElement.m_Target];
            pathTransform.m_Position = MathUtils.Position(curve.m_Bezier, pathElement.m_TargetDelta.x);
            float3 forward = MathUtils.Tangent(curve.m_Bezier, pathElement.m_TargetDelta.x);
            if (MathUtils.TryNormalize(ref forward))
              pathTransform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
          }
        }
        return pathTransform;
      }

      private bool HasAuthorization(Entity entity, Entity building)
      {
        Resident componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ResidentData.TryGetComponent(entity, out componentData1))
        {
          HouseholdMember componentData2;
          PropertyRenter componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_HouseholdMemberData.TryGetComponent(componentData1.m_Citizen, out componentData2) && this.m_PropertyRenterData.TryGetComponent(componentData2.m_Household, out componentData3) && componentData3.m_Property == building)
            return true;
          Worker componentData4;
          // ISSUE: reference to a compiler-generated field
          if (this.m_WorkerData.TryGetComponent(componentData1.m_Citizen, out componentData4))
          {
            PropertyRenter componentData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PropertyRenterData.TryGetComponent(componentData4.m_Workplace, out componentData5))
            {
              if (componentData5.m_Property == building)
                return true;
            }
            else if (componentData4.m_Workplace == building)
              return true;
          }
        }
        return false;
      }

      private bool FindClosestSpawnLocation(
        float3 comparePosition,
        out float3 spawnPosition,
        DynamicBuffer<SpawnLocationElement> spawnLocations,
        bool randomLocation,
        ref Random random,
        bool hasAuthorization)
      {
        spawnPosition = comparePosition;
        float num1 = float.MaxValue;
        bool flag1 = true;
        bool closestSpawnLocation = false;
        for (int index1 = 0; index1 < spawnLocations.Length; ++index1)
        {
          if (spawnLocations[index1].m_Type == SpawnLocationType.SpawnLocation || spawnLocations[index1].m_Type == SpawnLocationType.HangaroundLocation)
          {
            Entity spawnLocation = spawnLocations[index1].m_SpawnLocation;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.SpawnLocationData spawnLocationData = this.m_SpawnLocationData[this.m_PrefabRefData[spawnLocation].m_Prefab];
            if (spawnLocationData.m_ConnectionType == RouteConnectionType.Pedestrian)
            {
              bool flag2 = spawnLocationData.m_ActivityMask.m_Mask > 0U;
              if (!flag2 || flag1)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.HasComponent(spawnLocation))
                {
                  // ISSUE: reference to a compiler-generated field
                  Transform transform = this.m_TransformData[spawnLocation];
                  float num2 = !randomLocation ? math.distance(transform.m_Position, comparePosition) : random.NextFloat() + math.select(0.0f, 1f, hasAuthorization != spawnLocationData.m_RequireAuthorization);
                  if (!flag2 & flag1 || (double) num2 < (double) num1)
                  {
                    spawnPosition = transform.m_Position;
                    num1 = num2;
                    flag1 = flag2;
                    closestSpawnLocation = true;
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_AreaNodes.HasBuffer(spawnLocation))
                  {
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[spawnLocation];
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<Triangle> areaTriangle = this.m_AreaTriangles[spawnLocation];
                    for (int index2 = 0; index2 < areaTriangle.Length; ++index2)
                    {
                      Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, areaTriangle[index2]);
                      float num3;
                      float2 t;
                      if (randomLocation)
                      {
                        num3 = random.NextFloat() + math.select(0.0f, 1f, hasAuthorization != spawnLocationData.m_RequireAuthorization);
                        t = random.NextFloat2();
                        t = math.select(t, 1f - t, (double) math.csum(t) > 1.0);
                      }
                      else
                        num3 = MathUtils.Distance(triangle3, comparePosition, out t);
                      if (!flag2 & flag1 || (double) num3 < (double) num1)
                      {
                        spawnPosition = MathUtils.Position(triangle3, t);
                        num1 = num3;
                        flag1 = flag2;
                        closestSpawnLocation = true;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        return closestSpawnLocation;
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
      public ComponentTypeHandle<TripSource> __Game_Objects_TripSource_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<Human> __Game_Creatures_Human_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Animal> __Game_Creatures_Animal_RW_ComponentTypeHandle;
      public ComponentTypeHandle<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<HumanNavigation> __Game_Creatures_HumanNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AnimalNavigation> __Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Resident> __Game_Creatures_Resident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> __Game_Citizens_HomelessHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalData> __Game_Prefabs_AnimalData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> __Game_Buildings_SpawnLocationElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TripSource_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TripSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Human>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Animal_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Animal>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<HumanCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<HumanNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentLookup = state.GetComponentLookup<Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HomelessHousehold_RO_ComponentLookup = state.GetComponentLookup<HomelessHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimalData_RO_ComponentLookup = state.GetComponentLookup<AnimalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SpawnLocationElement_RO_BufferLookup = state.GetBufferLookup<SpawnLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
      }
    }
  }
}
