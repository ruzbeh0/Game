// Decompiled with JetBrains decompiler
// Type: Game.Events.ImpactSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class ImpactSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_ImpactQuery;
    private ComponentTypeSet m_ParkedToMovingCarRemoveTypes;
    private ComponentTypeSet m_ParkedToMovingPersonalCarAddTypes;
    private ComponentTypeSet m_ParkedToMovingTaxiAddTypes;
    private ComponentTypeSet m_ParkedToMovingServiceCarAddTypes;
    private ComponentTypeSet m_ParkedToMovingTrailerAddTypes;
    private ImpactSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImpactQuery = this.GetEntityQuery(ComponentType.ReadOnly<Impact>(), ComponentType.ReadOnly<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingCarRemoveTypes = new ComponentTypeSet(ComponentType.ReadWrite<ParkedCar>(), ComponentType.ReadWrite<Stopped>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingPersonalCarAddTypes = new ComponentTypeSet(new ComponentType[12]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingTaxiAddTypes = new ComponentTypeSet(new ComponentType[13]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<ServiceDispatch>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingServiceCarAddTypes = new ComponentTypeSet(new ComponentType[14]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<PathInformation>(),
        ComponentType.ReadWrite<ServiceDispatch>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingTrailerAddTypes = new ComponentTypeSet(new ComponentType[6]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<CarTrailerLane>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ImpactQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_ImpactQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Impact_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle = new ImpactSystem.AddImpactJob()
      {
        m_ImpactType = this.__TypeHandle.__Game_Events_Impact_RO_ComponentTypeHandle,
        m_StoppedData = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup,
        m_CarTrailerData = this.__TypeHandle.__Game_Vehicles_CarTrailer_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_CarCurrentLaneData = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup,
        m_CarTrailerLaneData = this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RO_ComponentLookup,
        m_PersonalCarData = this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup,
        m_TaxiData = this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentLookup,
        m_CreatureData = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RW_ComponentLookup,
        m_InvolvedInAccidentData = this.__TypeHandle.__Game_Events_InvolvedInAccident_RW_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RW_BufferLookup,
        m_Chunks = archetypeChunkListAsync,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_ParkedToMovingCarRemoveTypes = this.m_ParkedToMovingCarRemoveTypes,
        m_ParkedToMovingPersonalCarAddTypes = this.m_ParkedToMovingPersonalCarAddTypes,
        m_ParkedToMovingTaxiAddTypes = this.m_ParkedToMovingTaxiAddTypes,
        m_ParkedToMovingServiceCarAddTypes = this.m_ParkedToMovingServiceCarAddTypes,
        m_ParkedToMovingTrailerAddTypes = this.m_ParkedToMovingTrailerAddTypes,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ImpactSystem.AddImpactJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
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
    public ImpactSystem()
    {
    }

    [BurstCompile]
    private struct AddImpactJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<Impact> m_ImpactType;
      [ReadOnly]
      public ComponentLookup<Stopped> m_StoppedData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Car> m_CarData;
      [ReadOnly]
      public ComponentLookup<CarTrailer> m_CarTrailerData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> m_CarCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<CarTrailerLane> m_CarTrailerLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> m_PersonalCarData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Taxi> m_TaxiData;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<Moving> m_MovingData;
      public ComponentLookup<Controller> m_ControllerData;
      public ComponentLookup<InvolvedInAccident> m_InvolvedInAccidentData;
      public BufferLookup<TargetElement> m_TargetElements;
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingPersonalCarAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingTaxiAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingServiceCarAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingTrailerAddTypes;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        int capacity = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          capacity += this.m_Chunks[index].Count;
        }
        NativeParallelHashMap<Entity, InvolvedInAccident> nativeParallelHashMap = new NativeParallelHashMap<Entity, InvolvedInAccident>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Impact> nativeArray = this.m_Chunks[index1].GetNativeArray<Impact>(ref this.m_ImpactType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Impact impact = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(impact.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              InvolvedInAccident involvedInAccident1 = new InvolvedInAccident(impact.m_Event, impact.m_Severity, this.m_SimulationFrame);
              InvolvedInAccident involvedInAccident2;
              if (nativeParallelHashMap.TryGetValue(impact.m_Target, out involvedInAccident2))
              {
                if ((double) involvedInAccident1.m_Severity > (double) involvedInAccident2.m_Severity)
                  nativeParallelHashMap[impact.m_Target] = involvedInAccident1;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_InvolvedInAccidentData.HasComponent(impact.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  involvedInAccident2 = this.m_InvolvedInAccidentData[impact.m_Target];
                  if ((double) involvedInAccident1.m_Severity > (double) involvedInAccident2.m_Severity)
                    nativeParallelHashMap.TryAdd(impact.m_Target, involvedInAccident1);
                }
                else
                  nativeParallelHashMap.TryAdd(impact.m_Target, involvedInAccident1);
              }
              Moving moving = new Moving();
              if (!impact.m_VelocityDelta.Equals(new float3()) || !impact.m_AngularVelocityDelta.Equals(new float3()))
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_MovingData.HasComponent(impact.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  moving = this.m_MovingData[impact.m_Target];
                  moving.m_Velocity += impact.m_VelocityDelta;
                  moving.m_AngularVelocity += impact.m_AngularVelocityDelta;
                  // ISSUE: reference to a compiler-generated field
                  this.m_MovingData[impact.m_Target] = moving;
                }
                else
                {
                  moving.m_Velocity += impact.m_VelocityDelta;
                  moving.m_AngularVelocity += impact.m_AngularVelocityDelta;
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_VehicleData.HasComponent(impact.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_CarData.HasComponent(impact.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ParkedCarData.HasComponent(impact.m_Target))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.ActivateParkedCar(impact.m_Target, moving);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<OutOfControl>(impact.m_Target, new OutOfControl());
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_CarCurrentLaneData.HasComponent(impact.m_Target))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_StoppedData.HasComponent(impact.m_Target))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.ActivateStoppedCar(impact.m_Target, moving);
                      }
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<OutOfControl>(impact.m_Target, new OutOfControl());
                    }
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CarTrailerData.HasComponent(impact.m_Target))
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_ParkedCarData.HasComponent(impact.m_Target))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.ActivateParkedCarTrailer(impact.m_Target, moving, new ParkedCar());
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_CarTrailerLaneData.HasComponent(impact.m_Target))
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_StoppedData.HasComponent(impact.m_Target))
                        {
                          // ISSUE: reference to a compiler-generated method
                          this.ActivateStoppedTrailer(impact.m_Target, moving);
                        }
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_ControllerData.HasComponent(impact.m_Target))
                        {
                          // ISSUE: reference to a compiler-generated method
                          this.DetachVehicle(impact.m_Target);
                        }
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.AddComponent<OutOfControl>(impact.m_Target, new OutOfControl());
                      }
                    }
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_CreatureData.HasComponent(impact.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_StoppedData.HasComponent(impact.m_Target))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.ActivateStoppedCreature(impact.m_Target, moving);
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Stumbling>(impact.m_Target, new Stumbling());
                }
              }
            }
          }
        }
        if (nativeParallelHashMap.Count() == 0)
          return;
        NativeArray<Entity> keyArray = nativeParallelHashMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < keyArray.Length; ++index)
        {
          Entity entity = keyArray[index];
          InvolvedInAccident component = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_InvolvedInAccidentData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_InvolvedInAccidentData[entity].m_Event != component.m_Event && this.m_TargetElements.HasBuffer(component.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[component.m_Event], new TargetElement(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_InvolvedInAccidentData[entity] = component;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TargetElements.HasBuffer(component.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[component.m_Event], new TargetElement(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InvolvedInAccident>(entity, component);
          }
        }
      }

      private void DetachVehicle(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        Controller controller = this.m_ControllerData[entity];
        if (!(controller.m_Controller != Entity.Null) || !(controller.m_Controller != entity))
          return;
        DynamicBuffer<LayoutElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LayoutElements.TryGetBuffer(controller.m_Controller, out bufferData))
          CollectionUtils.RemoveValue<LayoutElement>(bufferData, new LayoutElement(entity));
        controller.m_Controller = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_ControllerData[entity] = controller;
      }

      private void ActivateParkedCarTrailer(Entity entity, Moving moving, ParkedCar parkedCar)
      {
        // ISSUE: reference to a compiler-generated field
        ParkedCar parkedCar1 = this.m_ParkedCarData[entity];
        if (parkedCar1.m_Lane == Entity.Null)
        {
          parkedCar1.m_Lane = parkedCar.m_Lane;
          parkedCar1.m_CurvePosition = parkedCar.m_CurvePosition;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(entity, in this.m_ParkedToMovingCarRemoveTypes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(entity, in this.m_ParkedToMovingTrailerAddTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Moving>(entity, moving);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<CarTrailerLane>(entity, new CarTrailerLane(parkedCar1));
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarLaneData.HasComponent(parkedCar1.m_Lane))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathfindUpdated>(parkedCar1.m_Lane, new PathfindUpdated());
      }

      private void ActivateParkedCar(Entity entity, Moving moving)
      {
        // ISSUE: reference to a compiler-generated field
        ParkedCar parkedCar = this.m_ParkedCarData[entity];
        Game.Vehicles.CarLaneFlags flags = Game.Vehicles.CarLaneFlags.EndReached | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.FixedLane;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(entity, in this.m_ParkedToMovingCarRemoveTypes);
        // ISSUE: reference to a compiler-generated field
        if (this.m_PersonalCarData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent(entity, in this.m_ParkedToMovingPersonalCarAddTypes);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TaxiData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(entity, in this.m_ParkedToMovingTaxiAddTypes);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent(entity, in this.m_ParkedToMovingServiceCarAddTypes);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Moving>(entity, moving);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<CarCurrentLane>(entity, new CarCurrentLane(parkedCar, flags));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ParkingLaneData.HasComponent(parkedCar.m_Lane) || this.m_GarageLaneData.HasComponent(parkedCar.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(parkedCar.m_Lane);
        }
        DynamicBuffer<LayoutElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LayoutElements.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 1; index < bufferData.Length; ++index)
        {
          Entity vehicle = bufferData[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkedCarData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated method
            this.ActivateParkedCarTrailer(vehicle, new Moving(), parkedCar);
          }
        }
      }

      private void ActivateStoppedCar(Entity entity, Moving moving)
      {
        // ISSUE: reference to a compiler-generated field
        CarCurrentLane carCurrentLane = this.m_CarCurrentLaneData[entity];
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Stopped>(entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Moving>(entity, moving);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<TransformFrame>(entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<InterpolatedTransform>(entity, new InterpolatedTransform());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Swaying>(entity, new Swaying());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(carCurrentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(carCurrentLane.m_Lane, new PathfindUpdated());
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(carCurrentLane.m_ChangeLane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(carCurrentLane.m_ChangeLane, new PathfindUpdated());
        }
        DynamicBuffer<LayoutElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LayoutElements.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 1; index < bufferData.Length; ++index)
        {
          Entity vehicle = bufferData[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          if (this.m_StoppedData.HasComponent(vehicle))
          {
            // ISSUE: reference to a compiler-generated method
            this.ActivateStoppedTrailer(vehicle, new Moving());
          }
        }
      }

      private void ActivateStoppedTrailer(Entity entity, Moving moving)
      {
        // ISSUE: reference to a compiler-generated field
        CarTrailerLane carTrailerLane = this.m_CarTrailerLaneData[entity];
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Stopped>(entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Moving>(entity, moving);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<TransformFrame>(entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<InterpolatedTransform>(entity, new InterpolatedTransform());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Swaying>(entity, new Swaying());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarLaneData.HasComponent(carTrailerLane.m_Lane))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathfindUpdated>(carTrailerLane.m_Lane, new PathfindUpdated());
      }

      private void ActivateStoppedCreature(Entity entity, Moving moving)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Stopped>(entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<TransformFrame>(entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<InterpolatedTransform>(entity, new InterpolatedTransform());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Moving>(entity, moving);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Impact> __Game_Events_Impact_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Stopped> __Game_Objects_Stopped_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Car> __Game_Vehicles_Car_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTrailer> __Game_Vehicles_CarTrailer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTrailerLane> __Game_Vehicles_CarTrailerLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> __Game_Vehicles_PersonalCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public ComponentLookup<Moving> __Game_Objects_Moving_RW_ComponentLookup;
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RW_ComponentLookup;
      public ComponentLookup<InvolvedInAccident> __Game_Events_InvolvedInAccident_RW_ComponentLookup;
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RW_BufferLookup;
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Impact_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Impact>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentLookup = state.GetComponentLookup<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentLookup = state.GetComponentLookup<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailer_RO_ComponentLookup = state.GetComponentLookup<CarTrailer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup = state.GetComponentLookup<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailerLane_RO_ComponentLookup = state.GetComponentLookup<CarTrailerLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PersonalCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Taxi>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentLookup = state.GetComponentLookup<Moving>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RW_ComponentLookup = state.GetComponentLookup<Controller>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RW_ComponentLookup = state.GetComponentLookup<InvolvedInAccident>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RW_BufferLookup = state.GetBufferLookup<TargetElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RW_BufferLookup = state.GetBufferLookup<LayoutElement>();
      }
    }
  }
}
