﻿// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HouseholdSpawnSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class HouseholdSpawnSystem : GameSystemBase
  {
    private EntityQuery m_HouseholdPrefabQuery;
    private EntityQuery m_OutsideConnectionQuery;
    private EntityQuery m_DemandParameterQuery;
    private ResidentialDemandSystem m_ResidentialDemandSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private CountStudyPositionsSystem m_CountStudyPositionsSystem;
    private CitySystem m_CitySystem;
    private HouseholdSpawnSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialDemandSystem = this.World.GetOrCreateSystemManaged<ResidentialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountStudyPositionsSystem = this.World.GetOrCreateSystemManaged<CountStudyPositionsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<Game.Prefabs.HouseholdData>(), ComponentType.Exclude<DynamicHousehold>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideConnectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Objects.ElectricityOutsideConnection>(), ComponentType.Exclude<Game.Objects.WaterPipeOutsideConnection>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DemandParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdPrefabQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_OutsideConnectionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      int householdDemand = this.m_ResidentialDemandSystem.householdDemand;
      if (householdDemand > 0)
      {
        JobHandle deps1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> densityDemandFactors1 = this.m_ResidentialDemandSystem.GetLowDensityDemandFactors(out deps1);
        JobHandle deps2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> densityDemandFactors2 = this.m_ResidentialDemandSystem.GetMediumDensityDemandFactors(out deps2);
        JobHandle deps3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> densityDemandFactors3 = this.m_ResidentialDemandSystem.GetHighDensityDemandFactors(out deps3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_DynamicHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_HouseholdData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle1;
        JobHandle outJobHandle2;
        JobHandle outJobHandle3;
        JobHandle deps4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        jobHandle = new HouseholdSpawnSystem.SpawnHouseholdJob()
        {
          m_PrefabEntities = this.m_HouseholdPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
          m_Archetypes = this.m_HouseholdPrefabQuery.ToComponentDataListAsync<Game.Prefabs.ArchetypeData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
          m_OutsideConnectionEntities = this.m_OutsideConnectionQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle3),
          m_HouseholdDatas = this.__TypeHandle.__Game_Prefabs_HouseholdData_RO_ComponentLookup,
          m_Dynamics = this.__TypeHandle.__Game_Prefabs_DynamicHousehold_RO_ComponentLookup,
          m_Populations = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
          m_OutsideConnectionDatas = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
          m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_DemandParameterData = this.m_DemandParameterQuery.GetSingleton<DemandParameterData>(),
          m_LowFactors = densityDemandFactors1,
          m_MedFactors = densityDemandFactors2,
          m_HiFactors = densityDemandFactors3,
          m_StudyPositions = this.m_CountStudyPositionsSystem.GetStudyPositionsByEducation(out deps4),
          m_City = this.m_CitySystem.City,
          m_Demand = householdDemand,
          m_Random = RandomSeed.Next().GetRandom((int) this.m_SimulationSystem.frameIndex),
          m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
        }.Schedule<HouseholdSpawnSystem.SpawnHouseholdJob>(JobUtils.CombineDependencies(outJobHandle1, outJobHandle2, jobHandle, outJobHandle3, deps1, deps2, deps3, deps4));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ResidentialDemandSystem.AddReader(jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      }
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
    public HouseholdSpawnSystem()
    {
    }

    [BurstCompile]
    private struct SpawnHouseholdJob : IJob
    {
      [ReadOnly]
      public NativeList<Entity> m_PrefabEntities;
      [ReadOnly]
      public NativeList<Game.Prefabs.ArchetypeData> m_Archetypes;
      [ReadOnly]
      public NativeList<Entity> m_OutsideConnectionEntities;
      [ReadOnly]
      public ComponentLookup<Population> m_Populations;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.HouseholdData> m_HouseholdDatas;
      [ReadOnly]
      public ComponentLookup<DynamicHousehold> m_Dynamics;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public DemandParameterData m_DemandParameterData;
      public Entity m_City;
      public EntityCommandBuffer m_CommandBuffer;
      public int m_Demand;
      public Unity.Mathematics.Random m_Random;
      [ReadOnly]
      public NativeArray<int> m_LowFactors;
      [ReadOnly]
      public NativeArray<int> m_MedFactors;
      [ReadOnly]
      public NativeArray<int> m_HiFactors;
      [ReadOnly]
      public NativeArray<int> m_StudyPositions;

      private bool IsValidStudyPrefab(Entity householdPrefab)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Prefabs.HouseholdData householdData = this.m_HouseholdDatas[householdPrefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((this.m_StudyPositions[1] + this.m_StudyPositions[2] <= 0 ? 0 : (this.m_Random.NextBool() ? 1 : 0)) == 0 && this.m_StudyPositions[3] + this.m_StudyPositions[4] > 0)
          return householdData.m_StudentCount > 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_StudyPositions[1] + this.m_StudyPositions[2] > 0 && householdData.m_ChildCount > 0;
      }

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int max1 = Mathf.RoundToInt(300f / math.clamp(this.m_DemandParameterData.m_HouseholdSpawnSpeedFactor * math.log((float) (1.0 + 1.0 / 1000.0 * (double) this.m_Populations[this.m_City].m_Population)), 0.5f, 20f));
        // ISSUE: reference to a compiler-generated field
        int num1 = this.m_Random.NextInt(max1);
        int num2 = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; num1 < this.m_Demand; num1 = this.m_Random.NextInt(max1))
        {
          ++num2;
          // ISSUE: reference to a compiler-generated field
          this.m_Demand -= num1;
        }
        if (num2 == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int y1 = this.m_LowFactors[6] + this.m_MedFactors[6] + this.m_HiFactors[6];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int y2 = this.m_LowFactors[12] + this.m_MedFactors[12] + this.m_HiFactors[12];
        int num3 = math.max(0, y1);
        int num4 = math.max(0, y2);
        float num5 = (float) num4 / (float) (num4 + num3);
        for (int index1 = 0; index1 < num2; ++index1)
        {
          int max2 = 0;
          // ISSUE: reference to a compiler-generated field
          bool flag = (double) this.m_Random.NextFloat() < (double) num5;
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_PrefabEntities.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.IsValidStudyPrefab(this.m_PrefabEntities[index2]) == flag)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              max2 += this.m_HouseholdDatas[this.m_PrefabEntities[index2]].m_Weight;
            }
          }
          // ISSUE: reference to a compiler-generated field
          int num6 = this.m_Random.NextInt(max2);
          int index3 = 0;
          // ISSUE: reference to a compiler-generated field
          for (int index4 = 0; index4 < this.m_PrefabEntities.Length; ++index4)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.IsValidStudyPrefab(this.m_PrefabEntities[index4]) == flag)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              num6 -= this.m_HouseholdDatas[this.m_PrefabEntities[index4]].m_Weight;
            }
            if (num6 < 0)
            {
              index3 = index4;
              break;
            }
          }
          // ISSUE: reference to a compiler-generated field
          Entity prefabEntity = this.m_PrefabEntities[index3];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(this.m_Archetypes[index3].m_Archetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef()
          {
            m_Prefab = prefabEntity
          });
          Entity result;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OutsideConnectionEntities.Length > 0 && BuildingUtils.GetRandomOutsideConnectionByParameters(ref this.m_OutsideConnectionEntities, ref this.m_OutsideConnectionDatas, ref this.m_PrefabRefs, this.m_Random, this.m_DemandParameterData.m_CitizenOCSpawnParameters, out result))
          {
            CurrentBuilding component = new CurrentBuilding()
            {
              m_CurrentBuilding = result
            };
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CurrentBuilding>(entity, component);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(entity, new Deleted());
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.HouseholdData> __Game_Prefabs_HouseholdData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DynamicHousehold> __Game_Prefabs_DynamicHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HouseholdData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.HouseholdData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DynamicHousehold_RO_ComponentLookup = state.GetComponentLookup<DynamicHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
      }
    }
  }
}
