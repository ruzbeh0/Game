// Decompiled with JetBrains decompiler
// Type: Game.Creatures.ReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Citizens;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Creatures
{
  [CompilerGenerated]
  public class ReferencesSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private Game.Objects.SearchSystem m_SearchSystem;
    private EntityQuery m_CreatureQuery;
    private ReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Creature>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatureQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_OwnedCreature_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Pet_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new ReferencesSystem.UpdateCreatureReferencesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_ResidentType = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle,
        m_PetType = this.__TypeHandle.__Game_Creatures_Pet_RO_ComponentTypeHandle,
        m_HumanCurrentLaneType = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle,
        m_AnimalCurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_CurrentTransports = this.__TypeHandle.__Game_Citizens_CurrentTransport_RW_ComponentLookup,
        m_OwnedCreatures = this.__TypeHandle.__Game_Creatures_OwnedCreature_RW_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RW_BufferLookup,
        m_SearchTree = this.m_SearchSystem.GetMovingSearchTree(false, out dependencies),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ReferencesSystem.UpdateCreatureReferencesJob>(this.m_CreatureQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SearchSystem.AddMovingSearchTreeWriter(jobHandle);
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
    public ReferencesSystem()
    {
    }

    [BurstCompile]
    private struct UpdateCreatureReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Resident> m_ResidentType;
      [ReadOnly]
      public ComponentTypeHandle<Pet> m_PetType;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> m_HumanCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<AnimalCurrentLane> m_AnimalCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      public ComponentLookup<CurrentTransport> m_CurrentTransports;
      public BufferLookup<OwnedCreature> m_OwnedCreatures;
      public BufferLookup<LaneObject> m_LaneObjects;
      public BufferLookup<Passenger> m_Passengers;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Created>(ref this.m_CreatedType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray4 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          for (int index = 0; index < nativeArray4.Length; ++index)
          {
            Entity creature = nativeArray1[index];
            Owner owner = nativeArray4[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnedCreatures.HasBuffer(owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_OwnedCreatures[owner.m_Owner].Add(new OwnedCreature(creature));
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<Resident> nativeArray5 = chunk.GetNativeArray<Resident>(ref this.m_ResidentType);
          for (int index = 0; index < nativeArray5.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Resident resident = nativeArray5[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentTransports.HasComponent(resident.m_Citizen))
            {
              // ISSUE: reference to a compiler-generated field
              CurrentTransport currentTransport = this.m_CurrentTransports[resident.m_Citizen] with
              {
                m_CurrentTransport = entity
              };
              // ISSUE: reference to a compiler-generated field
              this.m_CurrentTransports[resident.m_Citizen] = currentTransport;
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<Pet> nativeArray6 = chunk.GetNativeArray<Pet>(ref this.m_PetType);
          for (int index = 0; index < nativeArray6.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Pet pet = nativeArray6[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentTransports.HasComponent(pet.m_HouseholdPet))
            {
              // ISSUE: reference to a compiler-generated field
              CurrentTransport currentTransport = this.m_CurrentTransports[pet.m_HouseholdPet] with
              {
                m_CurrentTransport = entity
              };
              // ISSUE: reference to a compiler-generated field
              this.m_CurrentTransports[pet.m_HouseholdPet] = currentTransport;
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<CurrentVehicle> nativeArray7 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
          for (int index = 0; index < nativeArray7.Length; ++index)
          {
            Entity passenger = nativeArray1[index];
            CurrentVehicle currentVehicle = nativeArray7[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_Passengers.HasBuffer(currentVehicle.m_Vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Passengers[currentVehicle.m_Vehicle].Add(new Passenger(passenger));
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<HumanCurrentLane> nativeArray8 = chunk.GetNativeArray<HumanCurrentLane>(ref this.m_HumanCurrentLaneType);
          for (int index = 0; index < nativeArray8.Length; ++index)
          {
            Entity laneObject = nativeArray1[index];
            HumanCurrentLane humanCurrentLane = nativeArray8[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneObjects.HasBuffer(humanCurrentLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              NetUtils.AddLaneObject(this.m_LaneObjects[humanCurrentLane.m_Lane], laneObject, humanCurrentLane.m_CurvePosition.xx);
            }
            else
            {
              Transform transform = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
              Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
              // ISSUE: reference to a compiler-generated field
              this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<AnimalCurrentLane> nativeArray9 = chunk.GetNativeArray<AnimalCurrentLane>(ref this.m_AnimalCurrentLaneType);
          for (int index = 0; index < nativeArray9.Length; ++index)
          {
            Entity laneObject = nativeArray1[index];
            AnimalCurrentLane animalCurrentLane = nativeArray9[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneObjects.HasBuffer(animalCurrentLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              NetUtils.AddLaneObject(this.m_LaneObjects[animalCurrentLane.m_Lane], laneObject, animalCurrentLane.m_CurvePosition.xx);
            }
            else
            {
              Transform transform = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
              Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
              // ISSUE: reference to a compiler-generated field
              this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray10 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          if (nativeArray10.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index = 0; index < nativeArray10.Length; ++index)
            {
              Entity creature = nativeArray1[index];
              Owner owner = nativeArray10[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_OwnedCreatures.HasBuffer(owner.m_Owner))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<OwnedCreature>(this.m_OwnedCreatures[owner.m_Owner], new OwnedCreature(creature));
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<Resident> nativeArray11 = chunk.GetNativeArray<Resident>(ref this.m_ResidentType);
          for (int index = 0; index < nativeArray11.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Resident resident = nativeArray11[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentTransports.HasComponent(resident.m_Citizen) && this.m_CurrentTransports[resident.m_Citizen].m_CurrentTransport == entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<CurrentTransport>(resident.m_Citizen);
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<Pet> nativeArray12 = chunk.GetNativeArray<Pet>(ref this.m_PetType);
          for (int index = 0; index < nativeArray12.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Pet pet = nativeArray12[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentTransports.HasComponent(pet.m_HouseholdPet) && this.m_CurrentTransports[pet.m_HouseholdPet].m_CurrentTransport == entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<CurrentTransport>(pet.m_HouseholdPet);
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<CurrentVehicle> nativeArray13 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
          if (nativeArray13.Length != 0)
          {
            for (int index = 0; index < nativeArray13.Length; ++index)
            {
              Entity passenger = nativeArray1[index];
              CurrentVehicle currentVehicle = nativeArray13[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_Passengers.HasBuffer(currentVehicle.m_Vehicle))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<Passenger>(this.m_Passengers[currentVehicle.m_Vehicle], new Passenger(passenger));
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<HumanCurrentLane> nativeArray14 = chunk.GetNativeArray<HumanCurrentLane>(ref this.m_HumanCurrentLaneType);
          for (int index = 0; index < nativeArray14.Length; ++index)
          {
            Entity laneObject = nativeArray1[index];
            HumanCurrentLane humanCurrentLane = nativeArray14[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneObjects.HasBuffer(humanCurrentLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              NetUtils.RemoveLaneObject(this.m_LaneObjects[humanCurrentLane.m_Lane], laneObject);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SearchTree.TryRemove(laneObject);
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<AnimalCurrentLane> nativeArray15 = chunk.GetNativeArray<AnimalCurrentLane>(ref this.m_AnimalCurrentLaneType);
          for (int index = 0; index < nativeArray15.Length; ++index)
          {
            Entity laneObject = nativeArray1[index];
            AnimalCurrentLane animalCurrentLane = nativeArray15[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneObjects.HasBuffer(animalCurrentLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              NetUtils.RemoveLaneObject(this.m_LaneObjects[animalCurrentLane.m_Lane], laneObject);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SearchTree.TryRemove(laneObject);
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Resident> __Game_Creatures_Resident_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Pet> __Game_Creatures_Pet_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RW_ComponentLookup;
      public BufferLookup<OwnedCreature> __Game_Creatures_OwnedCreature_RW_BufferLookup;
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RW_BufferLookup;
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Pet_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Pet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RW_ComponentLookup = state.GetComponentLookup<CurrentTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_OwnedCreature_RW_BufferLookup = state.GetBufferLookup<OwnedCreature>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RW_BufferLookup = state.GetBufferLookup<LaneObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RW_BufferLookup = state.GetBufferLookup<Passenger>();
      }
    }
  }
}
