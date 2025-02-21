// Decompiled with JetBrains decompiler
// Type: Game.Creatures.TripResetSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Common;
using Game.Objects;
using Game.Pathfind;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  [CompilerGenerated]
  public class TripResetSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_ResetQuery;
    private TripResetSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResetQuery = this.GetEntityQuery(ComponentType.ReadOnly<ResetTrip>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ResetQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Divert_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Pet_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_ResetTrip_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TripResetSystem.CreatureTripResetJob jobData = new TripResetSystem.CreatureTripResetJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ResetTripType = this.__TypeHandle.__Game_Creatures_ResetTrip_RO_ComponentTypeHandle,
        m_Deleted = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_TravelPurpose = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_TripNeeded = this.__TypeHandle.__Game_Citizens_TripNeeded_RO_BufferLookup,
        m_GroupCreatures = this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferLookup,
        m_HumanCurrentLane = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RW_ComponentLookup,
        m_AnimalCurrentLane = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup,
        m_Resident = this.__TypeHandle.__Game_Creatures_Resident_RW_ComponentLookup,
        m_Pet = this.__TypeHandle.__Game_Creatures_Pet_RW_ComponentLookup,
        m_Target = this.__TypeHandle.__Game_Common_Target_RW_ComponentLookup,
        m_Divert = this.__TypeHandle.__Game_Creatures_Divert_RW_ComponentLookup,
        m_PathOwner = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<TripResetSystem.CreatureTripResetJob>(this.m_ResetQuery, this.Dependency);
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
    public TripResetSystem()
    {
    }

    [BurstCompile]
    private struct CreatureTripResetJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ResetTrip> m_ResetTripType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_Deleted;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurpose;
      [ReadOnly]
      public BufferLookup<TripNeeded> m_TripNeeded;
      [ReadOnly]
      public BufferLookup<GroupCreature> m_GroupCreatures;
      public ComponentLookup<HumanCurrentLane> m_HumanCurrentLane;
      public ComponentLookup<AnimalCurrentLane> m_AnimalCurrentLane;
      public ComponentLookup<Resident> m_Resident;
      public ComponentLookup<Pet> m_Pet;
      public ComponentLookup<Target> m_Target;
      public ComponentLookup<Divert> m_Divert;
      public ComponentLookup<PathOwner> m_PathOwner;
      public BufferLookup<PathElement> m_PathElements;
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
        NativeArray<ResetTrip> nativeArray2 = chunk.GetNativeArray<ResetTrip>(ref this.m_ResetTripType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          ResetTrip resetTrip = nativeArray2[index1];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Deleted.HasComponent(resetTrip.m_Creature))
          {
            HumanCurrentLane componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HumanCurrentLane.TryGetComponent(resetTrip.m_Creature, out componentData1))
            {
              componentData1.m_Flags &= ~CreatureLaneFlags.EndOfPath;
              // ISSUE: reference to a compiler-generated field
              this.m_HumanCurrentLane[resetTrip.m_Creature] = componentData1;
            }
            AnimalCurrentLane componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AnimalCurrentLane.TryGetComponent(resetTrip.m_Creature, out componentData2))
            {
              componentData2.m_Flags &= ~CreatureLaneFlags.EndOfPath;
              // ISSUE: reference to a compiler-generated field
              this.m_AnimalCurrentLane[resetTrip.m_Creature] = componentData2;
            }
            Target componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Target.TryGetComponent(resetTrip.m_Creature, out componentData3))
            {
              bool flag1 = false;
              bool flag2 = false;
              if (resetTrip.m_DivertPurpose != Purpose.None)
              {
                Divert componentData4;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Divert.TryGetComponent(resetTrip.m_Creature, out componentData4))
                {
                  if (componentData4.m_Purpose != resetTrip.m_DivertPurpose || componentData4.m_Target != resetTrip.m_DivertTarget)
                  {
                    componentData4.m_Purpose = resetTrip.m_DivertPurpose;
                    componentData4.m_Target = resetTrip.m_DivertTarget;
                    componentData4.m_Data = resetTrip.m_DivertData;
                    componentData4.m_Resource = resetTrip.m_DivertResource;
                    // ISSUE: reference to a compiler-generated field
                    this.m_Divert[resetTrip.m_Creature] = componentData4;
                    flag1 = true;
                  }
                  else
                    flag2 = true;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Divert>(unfilteredChunkIndex, resetTrip.m_Creature, new Divert()
                  {
                    m_Target = resetTrip.m_DivertTarget,
                    m_Purpose = resetTrip.m_DivertPurpose,
                    m_Data = resetTrip.m_DivertData,
                    m_Resource = resetTrip.m_DivertResource
                  });
                  flag1 = true;
                }
                PathOwner componentData5;
                // ISSUE: reference to a compiler-generated field
                if (flag1 && this.m_PathOwner.TryGetComponent(resetTrip.m_Creature, out componentData5))
                {
                  componentData5.m_State &= ~PathFlags.Failed;
                  DynamicBuffer<PathElement> bufferData1;
                  DynamicBuffer<PathElement> bufferData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (resetTrip.m_HasDivertPath && (componentData5.m_State & PathFlags.Pending) == (PathFlags) 0 && this.m_PathElements.TryGetBuffer(entity, out bufferData1) && this.m_PathElements.TryGetBuffer(resetTrip.m_Creature, out bufferData2))
                  {
                    PathUtils.CopyPath(bufferData1, new PathOwner(), 0, bufferData2);
                    componentData5.m_ElementIndex = 0;
                    componentData5.m_State &= ~PathFlags.DivertObsolete;
                    componentData5.m_State |= PathFlags.Updated | PathFlags.CachedObsolete;
                  }
                  else
                    componentData5.m_State |= PathFlags.DivertObsolete;
                  // ISSUE: reference to a compiler-generated field
                  this.m_PathOwner[resetTrip.m_Creature] = componentData5;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_Divert.HasComponent(resetTrip.m_Creature))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Divert>(unfilteredChunkIndex, resetTrip.m_Creature);
                }
              }
              if (resetTrip.m_Target != componentData3.m_Target)
              {
                Resident componentData6;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Resident.TryGetComponent(resetTrip.m_Creature, out componentData6))
                {
                  componentData6.m_Flags &= ~(ResidentFlags.Arrived | ResidentFlags.Hangaround | ResidentFlags.PreferredLeader | ResidentFlags.IgnoreBenches | ResidentFlags.IgnoreAreas | ResidentFlags.CannotIgnore);
                  componentData6.m_Flags |= resetTrip.m_ResidentFlags;
                  componentData6.m_Timer = 0;
                  // ISSUE: reference to a compiler-generated field
                  this.m_Resident[resetTrip.m_Creature] = componentData6;
                }
                Pet componentData7;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Pet.TryGetComponent(resetTrip.m_Creature, out componentData7))
                {
                  componentData7.m_Flags &= ~(PetFlags.Hangaround | PetFlags.Arrived | PetFlags.LeaderArrived);
                  // ISSUE: reference to a compiler-generated field
                  this.m_Pet[resetTrip.m_Creature] = componentData7;
                }
                PathOwner componentData8;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PathOwner.TryGetComponent(resetTrip.m_Creature, out componentData8))
                {
                  componentData8.m_State &= ~PathFlags.Failed;
                  DynamicBuffer<PathElement> bufferData3;
                  DynamicBuffer<PathElement> bufferData4;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!resetTrip.m_HasDivertPath && (componentData8.m_State & PathFlags.Pending) == (PathFlags) 0 && this.m_PathElements.TryGetBuffer(entity, out bufferData3) && this.m_PathElements.TryGetBuffer(resetTrip.m_Creature, out bufferData4))
                  {
                    PathUtils.CopyPath(bufferData3, new PathOwner(), 0, bufferData4);
                    componentData8.m_ElementIndex = 0;
                    if (flag1 | flag2)
                      componentData8.m_State &= ~PathFlags.CachedObsolete;
                    else
                      componentData8.m_State &= ~PathFlags.Obsolete;
                    componentData8.m_State |= PathFlags.Updated;
                  }
                  else if (flag1 | flag2)
                    componentData8.m_State |= PathFlags.CachedObsolete;
                  else
                    componentData8.m_State |= PathFlags.Obsolete;
                  // ISSUE: reference to a compiler-generated field
                  this.m_PathOwner[resetTrip.m_Creature] = componentData8;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_Target[resetTrip.m_Creature] = new Target(resetTrip.m_Target);
              }
            }
            Resident componentData9;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Resident.TryGetComponent(resetTrip.m_Creature, out componentData9))
            {
              if (resetTrip.m_Arrived != Entity.Null && componentData9.m_Citizen != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TripNeeded.HasBuffer(componentData9.m_Citizen))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<CurrentBuilding>(unfilteredChunkIndex, componentData9.m_Citizen, new CurrentBuilding(resetTrip.m_Arrived));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponentEnabled<Arrived>(unfilteredChunkIndex, componentData9.m_Citizen, true);
                }
                DynamicBuffer<GroupCreature> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_GroupCreatures.TryGetBuffer(resetTrip.m_Creature, out bufferData))
                {
                  for (int index2 = 0; index2 < bufferData.Length; ++index2)
                  {
                    Entity creature = bufferData[index2].m_Creature;
                    Pet componentData10;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Pet.TryGetComponent(creature, out componentData10))
                    {
                      componentData10.m_Flags |= PetFlags.LeaderArrived;
                      // ISSUE: reference to a compiler-generated field
                      this.m_Pet[creature] = componentData10;
                    }
                  }
                }
              }
              if (resetTrip.m_TravelPurpose != Purpose.None && componentData9.m_Citizen != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TravelPurpose.HasComponent(componentData9.m_Citizen))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TravelPurpose[componentData9.m_Citizen].m_Purpose != resetTrip.m_TravelPurpose)
                  {
                    TravelPurpose component = new TravelPurpose()
                    {
                      m_Purpose = resetTrip.m_TravelPurpose,
                      m_Data = resetTrip.m_TravelData,
                      m_Resource = resetTrip.m_TravelResource
                    };
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<TravelPurpose>(unfilteredChunkIndex, componentData9.m_Citizen, component);
                  }
                }
                else
                {
                  TravelPurpose component = new TravelPurpose()
                  {
                    m_Purpose = resetTrip.m_TravelPurpose
                  };
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<TravelPurpose>(unfilteredChunkIndex, componentData9.m_Citizen, component);
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (resetTrip.m_NextPurpose != Purpose.None && componentData9.m_Citizen != Entity.Null && this.m_TripNeeded.HasBuffer(componentData9.m_Citizen))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<TripNeeded> dynamicBuffer1 = this.m_TripNeeded[componentData9.m_Citizen];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<TripNeeded> dynamicBuffer2 = this.m_CommandBuffer.SetBuffer<TripNeeded>(unfilteredChunkIndex, componentData9.m_Citizen);
                dynamicBuffer2.ResizeUninitialized(1 + dynamicBuffer1.Length);
                dynamicBuffer2[0] = new TripNeeded()
                {
                  m_Purpose = resetTrip.m_NextPurpose,
                  m_TargetAgent = resetTrip.m_NextTarget,
                  m_Data = resetTrip.m_NextData,
                  m_Resource = resetTrip.m_NextResource
                };
                for (int index3 = 0; index3 < dynamicBuffer1.Length; ++index3)
                  dynamicBuffer2[index3 + 1] = dynamicBuffer1[index3];
              }
            }
            if (resetTrip.m_Source != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<TripSource>(unfilteredChunkIndex, resetTrip.m_Creature, new TripSource(resetTrip.m_Source, resetTrip.m_Delay));
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
      public ComponentTypeHandle<ResetTrip> __Game_Creatures_ResetTrip_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TripNeeded> __Game_Citizens_TripNeeded_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<GroupCreature> __Game_Creatures_GroupCreature_RO_BufferLookup;
      public ComponentLookup<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RW_ComponentLookup;
      public ComponentLookup<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RW_ComponentLookup;
      public ComponentLookup<Resident> __Game_Creatures_Resident_RW_ComponentLookup;
      public ComponentLookup<Pet> __Game_Creatures_Pet_RW_ComponentLookup;
      public ComponentLookup<Target> __Game_Common_Target_RW_ComponentLookup;
      public ComponentLookup<Divert> __Game_Creatures_Divert_RW_ComponentLookup;
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_ResetTrip_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResetTrip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RO_BufferLookup = state.GetBufferLookup<TripNeeded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupCreature_RO_BufferLookup = state.GetBufferLookup<GroupCreature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RW_ComponentLookup = state.GetComponentLookup<HumanCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RW_ComponentLookup = state.GetComponentLookup<AnimalCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RW_ComponentLookup = state.GetComponentLookup<Resident>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Pet_RW_ComponentLookup = state.GetComponentLookup<Pet>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentLookup = state.GetComponentLookup<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Divert_RW_ComponentLookup = state.GetComponentLookup<Divert>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentLookup = state.GetComponentLookup<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
      }
    }
  }
}
