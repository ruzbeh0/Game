// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ResidentPurposeCounterSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Debug;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Reflection;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [DebugWatchOnly]
  [CompilerGenerated]
  public class ResidentPurposeCounterSystem : GameSystemBase
  {
    private EntityQuery m_CreatureQuery;
    [EnumArray(typeof (ResidentPurposeCounterSystem.CountPurpose))]
    [DebugWatchValue(historyLength = 1024)]
    private NativeArray<int> m_Results;
    private ResidentPurposeCounterSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Creatures.Resident>(), ComponentType.ReadOnly<Human>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<PathOwner>(), ComponentType.ReadOnly<Target>(), ComponentType.Exclude<Unspawned>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Stumbling>());
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(12, Allocator.Persistent);
      this.Enabled = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Divert_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      ResidentPurposeCounterSystem.PurposeCountJob jobData = new ResidentPurposeCounterSystem.PurposeCountJob()
      {
        m_Chunks = this.m_CreatureQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ResidentType = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle,
        m_DivertType = this.__TypeHandle.__Game_Creatures_Divert_RO_ComponentTypeHandle,
        m_TravelPurposeData = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_CurrentBuildings = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_MovingAways = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
        m_Results = this.m_Results
      };
      this.Dependency = jobData.Schedule<ResidentPurposeCounterSystem.PurposeCountJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
    public ResidentPurposeCounterSystem()
    {
    }

    public enum CountPurpose
    {
      GoingHome,
      GoingToSchool,
      GoingToWork,
      Leisure,
      MovingAway,
      Shopping,
      Travel,
      None,
      Other,
      TouristLeaving,
      Mail,
      MovingIn,
      Count,
    }

    [BurstCompile]
    private struct PurposeCountJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Creatures.Resident> m_ResidentType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public ComponentTypeHandle<Divert> m_DivertType;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposeData;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildings;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<MovingAway> m_MovingAways;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      public NativeArray<int> m_Results;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Results.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Results[index] = 0;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Creatures.Resident> nativeArray2 = chunk.GetNativeArray<Game.Creatures.Resident>(ref this.m_ResidentType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Divert> nativeArray3 = chunk.GetNativeArray<Divert>(ref this.m_DivertType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            if (bufferAccessor[index2].Length != 0)
            {
              Entity citizen = nativeArray2[index2].m_Citizen;
              // ISSUE: reference to a compiler-generated field
              Entity household = this.m_HouseholdMembers[citizen].m_Household;
              // ISSUE: reference to a compiler-generated field
              if (this.m_MovingAways.HasComponent(household))
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TouristHouseholds.HasComponent(household))
                {
                  // ISSUE: reference to a compiler-generated field
                  ++this.m_Results[9];
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  ++this.m_Results[4];
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Households.HasComponent(household) && this.m_TravelPurposeData.HasComponent(citizen) && !this.m_CurrentBuildings.HasComponent(citizen))
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_Households[household].m_Flags & HouseholdFlags.MovedIn) == HouseholdFlags.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    ++this.m_Results[11];
                  }
                  // ISSUE: reference to a compiler-generated field
                  Game.Citizens.Purpose purpose = this.m_TravelPurposeData[citizen].m_Purpose;
                  if (nativeArray3.IsCreated)
                    purpose = nativeArray3[index2].m_Purpose;
                  if ((uint) purpose <= 10U)
                  {
                    switch (purpose - (byte) 1)
                    {
                      case Game.Citizens.Purpose.None:
                        // ISSUE: reference to a compiler-generated field
                        ++this.m_Results[5];
                        continue;
                      case Game.Citizens.Purpose.Shopping:
                        // ISSUE: reference to a compiler-generated field
                        ++this.m_Results[3];
                        continue;
                      case Game.Citizens.Purpose.Leisure:
                        // ISSUE: reference to a compiler-generated field
                        ++this.m_Results[0];
                        continue;
                      case Game.Citizens.Purpose.GoingHome:
                        // ISSUE: reference to a compiler-generated field
                        ++this.m_Results[2];
                        continue;
                      default:
                        if (purpose == Game.Citizens.Purpose.GoingToSchool)
                        {
                          // ISSUE: reference to a compiler-generated field
                          ++this.m_Results[1];
                          continue;
                        }
                        break;
                    }
                  }
                  else if (purpose != Game.Citizens.Purpose.Traveling)
                  {
                    if (purpose == Game.Citizens.Purpose.SendMail)
                    {
                      // ISSUE: reference to a compiler-generated field
                      ++this.m_Results[10];
                      continue;
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    ++this.m_Results[6];
                    continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  ++this.m_Results[8];
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  ++this.m_Results[7];
                }
              }
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
      public ComponentTypeHandle<Game.Creatures.Resident> __Game_Creatures_Resident_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Divert> __Game_Creatures_Divert_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingAway> __Game_Agents_MovingAway_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Creatures.Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Divert_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Divert>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_MovingAway_RO_ComponentLookup = state.GetComponentLookup<MovingAway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
      }
    }
  }
}
