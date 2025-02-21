// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TouristFindTargetSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Events;
using Game.Pathfind;
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
  public class TouristFindTargetSystem : GameSystemBase
  {
    private EntityQuery m_SeekerQuery;
    private ComponentTypeSet m_PathfindTypes;
    private EndFrameBarrier m_EndFrameBarrier;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private AddMeetingSystem m_AddMeetingSystem;
    private NativeQueue<TouristFindTargetSystem.HotelReserveAction> m_HotelReserveQueue;
    private TouristFindTargetSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AddMeetingSystem = this.World.GetOrCreateSystemManaged<AddMeetingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SeekerQuery = this.GetEntityQuery(ComponentType.ReadWrite<TouristHousehold>(), ComponentType.ReadWrite<LodgingSeeker>(), ComponentType.Exclude<MovingAway>(), ComponentType.Exclude<Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindTypes = new ComponentTypeSet(ComponentType.ReadWrite<PathInformation>());
      // ISSUE: reference to a compiler-generated field
      this.m_HotelReserveQueue = new NativeQueue<TouristFindTargetSystem.HotelReserveAction>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SeekerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_HotelReserveQueue.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_LodgingProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TouristFindTargetSystem.TouristFindTargetJob jobData1 = new TouristFindTargetSystem.TouristFindTargetJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HouseholdType = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentTypeHandle,
        m_CurrentBuildings = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_RenterBufs = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_HouseholdCitizenBufs = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_OwnedVehicleBufs = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_PathfindTypeSet = this.m_PathfindTypes,
        m_PathInformations = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_LodgingProviders = this.__TypeHandle.__Game_Companies_LodgingProvider_RO_ComponentLookup,
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_MeetingQueue = this.m_AddMeetingSystem.GetMeetingQueue(out deps).AsParallelWriter(),
        m_ReserveQueue = this.m_HotelReserveQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<TouristFindTargetSystem.TouristFindTargetJob>(this.m_SeekerQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_LodgingProvider_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      TouristFindTargetSystem.HotelReserveJob jobData2 = new TouristFindTargetSystem.HotelReserveJob()
      {
        m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RW_ComponentLookup,
        m_LodgingProviders = this.__TypeHandle.__Game_Companies_LodgingProvider_RW_ComponentLookup,
        m_RenterBufs = this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup,
        m_ReserveQueue = this.m_HotelReserveQueue,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      this.Dependency = jobData2.Schedule<TouristFindTargetSystem.HotelReserveJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public TouristFindTargetSystem()
    {
    }

    private struct HotelReserveAction
    {
      public Entity m_Household;
      public Entity m_Target;
    }

    [BurstCompile]
    private struct TouristFindTargetJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<TouristHousehold> m_HouseholdType;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildings;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformations;
      [ReadOnly]
      public ComponentLookup<LodgingProvider> m_LodgingProviders;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterBufs;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenBufs;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicleBufs;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<AddMeetingSystem.AddMeeting>.ParallelWriter m_MeetingQueue;
      public NativeQueue<TouristFindTargetSystem.HotelReserveAction>.ParallelWriter m_ReserveQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public ComponentTypeSet m_PathfindTypeSet;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Entity entity = nativeArray[index1];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PathInformations.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(unfilteredChunkIndex, entity, in this.m_PathfindTypeSet);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PathInformation>(unfilteredChunkIndex, entity, new PathInformation()
            {
              m_State = PathFlags.Pending
            });
            PathfindParameters parameters = new PathfindParameters()
            {
              m_MaxSpeed = (float2) 277.777771f,
              m_WalkSpeed = (float2) 1.66666675f,
              m_Weights = new PathfindWeights(0.1f, 0.1f, 0.1f, 0.2f),
              m_Methods = PathMethod.Track | PathMethod.Taxi | PathMethod.Flying,
              m_SecondaryIgnoredRules = VehicleUtils.GetIgnoredPathfindRulesTaxiDefaults(),
              m_PathfindFlags = PathfindFlags.IgnoreFlow | PathfindFlags.Simplified | PathfindFlags.IgnorePath
            };
            Entity currentBuilding = Entity.Null;
            int index2 = 0;
            while (true)
            {
              int num = index2;
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<HouseholdCitizen> householdCitizenBuf = this.m_HouseholdCitizenBufs[entity];
              int length = householdCitizenBuf.Length;
              if (num < length)
              {
                // ISSUE: reference to a compiler-generated field
                ref ComponentLookup<CurrentBuilding> local1 = ref this.m_CurrentBuildings;
                // ISSUE: reference to a compiler-generated field
                householdCitizenBuf = this.m_HouseholdCitizenBufs[entity];
                Entity citizen1 = householdCitizenBuf[index2].m_Citizen;
                if (local1.HasComponent(citizen1))
                {
                  // ISSUE: reference to a compiler-generated field
                  ref ComponentLookup<CurrentBuilding> local2 = ref this.m_CurrentBuildings;
                  // ISSUE: reference to a compiler-generated field
                  householdCitizenBuf = this.m_HouseholdCitizenBufs[entity];
                  Entity citizen2 = householdCitizenBuf[index2].m_Citizen;
                  currentBuilding = local2[citizen2].m_CurrentBuilding;
                }
                ++index2;
              }
              else
                break;
            }
            SetupQueueTarget origin = new SetupQueueTarget()
            {
              m_Type = SetupTargetType.CurrentLocation,
              m_Methods = PathMethod.Pedestrian | PathMethod.Track | PathMethod.Taxi | PathMethod.Flying,
              m_Entity = currentBuilding
            };
            SetupQueueTarget destination = new SetupQueueTarget()
            {
              m_Type = SetupTargetType.TouristFindTarget,
              m_Methods = PathMethod.Pedestrian | PathMethod.Track | PathMethod.Taxi | PathMethod.Flying,
              m_Entity = entity
            };
            // ISSUE: reference to a compiler-generated field
            PathUtils.UpdateOwnedVehicleMethods(entity, ref this.m_OwnedVehicleBufs, ref parameters, ref origin, ref destination);
            // ISSUE: reference to a compiler-generated field
            this.m_PathfindQueue.Enqueue(new SetupQueueItem(entity, parameters, origin, destination));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            PathInformation pathInformation = this.m_PathInformations[entity];
            if ((pathInformation.m_State & PathFlags.Pending) == (PathFlags) 0)
            {
              Entity destination = pathInformation.m_Destination;
              if (destination != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_RenterBufs.HasBuffer(destination))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Renter> renterBuf = this.m_RenterBufs[destination];
                  if (renterBuf.Length > 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    ref ComponentLookup<LodgingProvider> local3 = ref this.m_LodgingProviders;
                    // ISSUE: reference to a compiler-generated field
                    renterBuf = this.m_RenterBufs[destination];
                    Entity renter1 = renterBuf[0].m_Renter;
                    if (local3.HasComponent(renter1))
                    {
                      // ISSUE: reference to a compiler-generated field
                      ref NativeQueue<TouristFindTargetSystem.HotelReserveAction>.ParallelWriter local4 = ref this.m_ReserveQueue;
                      // ISSUE: object of a compiler-generated type is created
                      // ISSUE: variable of a compiler-generated type
                      TouristFindTargetSystem.HotelReserveAction hotelReserveAction1 = new TouristFindTargetSystem.HotelReserveAction();
                      // ISSUE: reference to a compiler-generated field
                      hotelReserveAction1.m_Household = entity;
                      ref TouristFindTargetSystem.HotelReserveAction local5 = ref hotelReserveAction1;
                      // ISSUE: reference to a compiler-generated field
                      renterBuf = this.m_RenterBufs[pathInformation.m_Destination];
                      Entity renter2 = renterBuf[0].m_Renter;
                      // ISSUE: reference to a compiler-generated field
                      local5.m_Target = renter2;
                      // ISSUE: variable of a compiler-generated type
                      TouristFindTargetSystem.HotelReserveAction hotelReserveAction2 = hotelReserveAction1;
                      local4.Enqueue(hotelReserveAction2);
                      goto label_15;
                    }
                  }
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_MeetingQueue.Enqueue(new AddMeetingSystem.AddMeeting()
                {
                  m_Household = entity,
                  m_Type = LeisureType.Attractions
                });
label_15:
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Target>(unfilteredChunkIndex, entity, new Target(destination));
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<MovingAway>(unfilteredChunkIndex, entity);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<PathInformation>(unfilteredChunkIndex, entity);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<LodgingSeeker>(unfilteredChunkIndex, entity);
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
    private struct HotelReserveJob : IJob
    {
      public ComponentLookup<LodgingProvider> m_LodgingProviders;
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      public BufferLookup<Renter> m_RenterBufs;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<TouristFindTargetSystem.HotelReserveAction> m_ReserveQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        TouristFindTargetSystem.HotelReserveAction hotelReserveAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ReserveQueue.TryDequeue(out hotelReserveAction))
        {
          // ISSUE: reference to a compiler-generated field
          Entity target = hotelReserveAction.m_Target;
          // ISSUE: reference to a compiler-generated field
          Entity household = hotelReserveAction.m_Household;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_RenterBufs.HasBuffer(target) && this.m_LodgingProviders.HasComponent(target) && this.m_TouristHouseholds.HasComponent(household))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Renter> renterBuf = this.m_RenterBufs[target];
            // ISSUE: reference to a compiler-generated field
            LodgingProvider lodgingProvider = this.m_LodgingProviders[target];
            // ISSUE: reference to a compiler-generated field
            TouristHousehold touristHousehold = this.m_TouristHouseholds[household];
            if (lodgingProvider.m_FreeRooms > 0)
            {
              --lodgingProvider.m_FreeRooms;
              // ISSUE: reference to a compiler-generated field
              this.m_LodgingProviders[target] = lodgingProvider;
              renterBuf.Add(new Renter()
              {
                m_Renter = household
              });
              touristHousehold.m_Hotel = target;
              // ISSUE: reference to a compiler-generated field
              this.m_TouristHouseholds[household] = touristHousehold;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<LodgingSeeker>(household);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LodgingSeeker>(household);
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LodgingProvider> __Game_Companies_LodgingProvider_RO_ComponentLookup;
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RW_ComponentLookup;
      public ComponentLookup<LodgingProvider> __Game_Companies_LodgingProvider_RW_ComponentLookup;
      public BufferLookup<Renter> __Game_Buildings_Renter_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_LodgingProvider_RO_ComponentLookup = state.GetComponentLookup<LodgingProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RW_ComponentLookup = state.GetComponentLookup<TouristHousehold>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_LodgingProvider_RW_ComponentLookup = state.GetComponentLookup<LodgingProvider>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RW_BufferLookup = state.GetBufferLookup<Renter>();
      }
    }
  }
}
