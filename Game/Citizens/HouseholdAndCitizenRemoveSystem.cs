// Decompiled with JetBrains decompiler
// Type: Game.Citizens.HouseholdAndCitizenRemoveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Agents;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Citizens
{
  [CompilerGenerated]
  public class HouseholdAndCitizenRemoveSystem : GameSystemBase
  {
    private EntityQuery m_DeletedQuery;
    private EntityArchetype m_RentEventArchetype;
    private ModificationBarrier2 m_ModificationBarrier;
    private HouseholdAndCitizenRemoveSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Household>(),
          ComponentType.ReadOnly<Citizen>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RentEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<RentersUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DeletedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Occupant_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Patient_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Student_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_HasJobSeeker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HasSchoolSeeker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new HouseholdAndCitizenRemoveSystem.HouseholdAndCitizenRemoveJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
        m_HouseholdPets = this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup,
        m_HasSchoolSeekers = this.__TypeHandle.__Game_Citizens_HasSchoolSeeker_RO_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_HomelessHouseholds = this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup,
        m_HasJobSeekers = this.__TypeHandle.__Game_Agents_HasJobSeeker_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_Students = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup,
        m_Vehicles = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_CurrentBuildings = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_CurrentTransports = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_Creatures = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
        m_Deleteds = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_SchoolStudents = this.__TypeHandle.__Game_Buildings_Student_RW_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferLookup,
        m_HouseholdAnimals = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RW_BufferLookup,
        m_Patients = this.__TypeHandle.__Game_Buildings_Patient_RW_BufferLookup,
        m_Occupants = this.__TypeHandle.__Game_Buildings_Occupant_RW_BufferLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup,
        m_RentEventArchetype = this.m_RentEventArchetype,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<HouseholdAndCitizenRemoveSystem.HouseholdAndCitizenRemoveJob>(this.m_DeletedQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public HouseholdAndCitizenRemoveSystem()
    {
    }

    [BurstCompile]
    private struct HouseholdAndCitizenRemoveJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> m_HouseholdPets;
      [ReadOnly]
      public ComponentLookup<HasSchoolSeeker> m_HasSchoolSeekers;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> m_HomelessHouseholds;
      [ReadOnly]
      public ComponentLookup<HasJobSeeker> m_HasJobSeekers;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<Student> m_Students;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildings;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransports;
      [ReadOnly]
      public ComponentLookup<Creature> m_Creatures;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_Vehicles;
      [ReadOnly]
      public ComponentLookup<Deleted> m_Deleteds;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      public BufferLookup<Game.Buildings.Student> m_SchoolStudents;
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimals;
      public BufferLookup<Patient> m_Patients;
      public BufferLookup<Occupant> m_Occupants;
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public EntityArchetype m_RentEventArchetype;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Citizen>(ref this.m_CitizenType))
        {
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            Entity entity = nativeArray[index];
            // ISSUE: reference to a compiler-generated method
            this.RemoveCitizen(entity);
            // ISSUE: reference to a compiler-generated field
            HouseholdMember householdMember = this.m_HouseholdMembers[entity];
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdCitizens.HasBuffer(householdMember.m_Household))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[householdMember.m_Household];
              CollectionUtils.RemoveValue<HouseholdCitizen>(householdCitizen, new HouseholdCitizen(entity));
              if (householdCitizen.Length == 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.RemoveHousehold(householdMember.m_Household);
              }
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.RemoveHousehold(nativeArray[index]);
          }
        }
      }

      private void RemoveCitizen(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Deleteds.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(entity, new Deleted());
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_HasJobSeekers.IsComponentEnabled(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Entity seeker = this.m_HasJobSeekers[entity].m_Seeker;
          // ISSUE: reference to a compiler-generated field
          if (Entity.Null != seeker && !this.m_Deleteds.HasComponent(seeker))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(seeker, new Deleted());
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_HasSchoolSeekers.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Entity seeker = this.m_HasSchoolSeekers[entity].m_Seeker;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Deleteds.HasComponent(seeker))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(seeker, new Deleted());
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Students.HasComponent(entity) && this.m_SchoolStudents.HasBuffer(this.m_Students[entity].m_School))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CollectionUtils.RemoveValue<Game.Buildings.Student>(this.m_SchoolStudents[this.m_Students[entity].m_School], new Game.Buildings.Student(entity));
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentBuildings.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          CurrentBuilding currentBuilding = this.m_CurrentBuildings[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Patients.HasBuffer(currentBuilding.m_CurrentBuilding))
          {
            // ISSUE: reference to a compiler-generated field
            CollectionUtils.RemoveValue<Patient>(this.m_Patients[currentBuilding.m_CurrentBuilding], new Patient(entity));
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_Occupants.HasBuffer(currentBuilding.m_CurrentBuilding))
          {
            // ISSUE: reference to a compiler-generated field
            CollectionUtils.RemoveValue<Occupant>(this.m_Occupants[currentBuilding.m_CurrentBuilding], new Occupant(entity));
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CurrentTransports.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        CurrentTransport currentTransport = this.m_CurrentTransports[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Creatures.HasComponent(currentTransport.m_CurrentTransport) || this.m_Deleteds.HasComponent(currentTransport.m_CurrentTransport))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(currentTransport.m_CurrentTransport, new Deleted());
      }

      private void RemoveHousehold(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HouseholdAnimals.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<HouseholdAnimal> householdAnimal = this.m_HouseholdAnimals[entity];
          for (int index = 0; index < householdAnimal.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdPets.HasComponent(householdAnimal[index].m_HouseholdPet))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(householdAnimal[index].m_HouseholdPet, new Deleted());
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_HouseholdCitizens.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[entity];
          for (int index = 0; index < householdCitizen.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_Citizens.HasComponent(householdCitizen[index].m_Citizen))
            {
              // ISSUE: reference to a compiler-generated method
              this.RemoveCitizen(householdCitizen[index].m_Citizen);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnedVehicles.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<OwnedVehicle> ownedVehicle = this.m_OwnedVehicles[entity];
          for (int index = 0; index < ownedVehicle.Length; ++index)
          {
            Entity vehicle = ownedVehicle[index].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Vehicles.HasComponent(vehicle))
            {
              DynamicBuffer<LayoutElement> layout = new DynamicBuffer<LayoutElement>();
              // ISSUE: reference to a compiler-generated field
              if (this.m_LayoutElements.HasBuffer(vehicle))
              {
                // ISSUE: reference to a compiler-generated field
                layout = this.m_LayoutElements[vehicle];
              }
              // ISSUE: reference to a compiler-generated field
              VehicleUtils.DeleteVehicle(this.m_CommandBuffer, vehicle, layout);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PropertyRenters.HasComponent(entity) && !this.m_HomelessHouseholds.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity propertyFromRenter = BuildingUtils.GetPropertyFromRenter(entity, ref this.m_HomelessHouseholds, ref this.m_PropertyRenters);
        DynamicBuffer<Renter> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Renters.TryGetBuffer(propertyFromRenter, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          if (bufferData[index].m_Renter == entity)
          {
            bufferData.RemoveAt(index);
            break;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RentersUpdated>(this.m_CommandBuffer.CreateEntity(this.m_RentEventArchetype), new RentersUpdated(propertyFromRenter));
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
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> __Game_Citizens_HouseholdPet_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HasSchoolSeeker> __Game_Citizens_HasSchoolSeeker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> __Game_Citizens_HomelessHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HasJobSeeker> __Game_Agents_HasJobSeeker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Student> __Game_Citizens_Student_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      public BufferLookup<Game.Buildings.Student> __Game_Buildings_Student_RW_BufferLookup;
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RW_BufferLookup;
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RW_BufferLookup;
      public BufferLookup<Patient> __Game_Buildings_Patient_RW_BufferLookup;
      public BufferLookup<Occupant> __Game_Buildings_Occupant_RW_BufferLookup;
      public BufferLookup<Renter> __Game_Buildings_Renter_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdPet_RO_ComponentLookup = state.GetComponentLookup<HouseholdPet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HasSchoolSeeker_RO_ComponentLookup = state.GetComponentLookup<HasSchoolSeeker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HomelessHousehold_RO_ComponentLookup = state.GetComponentLookup<HomelessHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_HasJobSeeker_RO_ComponentLookup = state.GetComponentLookup<HasJobSeeker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentLookup = state.GetComponentLookup<Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Student_RW_BufferLookup = state.GetBufferLookup<Game.Buildings.Student>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RW_BufferLookup = state.GetBufferLookup<HouseholdCitizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RW_BufferLookup = state.GetBufferLookup<HouseholdAnimal>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Patient_RW_BufferLookup = state.GetBufferLookup<Patient>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Occupant_RW_BufferLookup = state.GetBufferLookup<Occupant>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RW_BufferLookup = state.GetBufferLookup<Renter>();
      }
    }
  }
}
