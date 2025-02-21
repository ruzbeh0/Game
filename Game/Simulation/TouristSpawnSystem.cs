// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TouristSpawnSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TouristSpawnSystem : GameSystemBase
  {
    private EntityQuery m_HouseholdPrefabQuery;
    private EntityQuery m_OutsideConnectionQuery;
    private EntityQuery m_AttractivenessParameterQuery;
    private EntityQuery m_DemandParameterQuery;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private ClimateSystem m_ClimateSystem;
    private CitySystem m_CitySystem;
    private TouristSpawnSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<Game.Prefabs.HouseholdData>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideConnectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Objects.ElectricityOutsideConnection>(), ComponentType.Exclude<Game.Objects.WaterPipeOutsideConnection>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DemandParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AttractivenessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<AttractivenessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdPrefabQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_OutsideConnectionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Tourism_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      JobHandle outJobHandle3;
      JobHandle outJobHandle4;
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
      TouristSpawnSystem.SpawnTouristHouseholdJob jobData = new TouristSpawnSystem.SpawnTouristHouseholdJob()
      {
        m_PrefabEntities = this.m_HouseholdPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_Archetypes = this.m_HouseholdPrefabQuery.ToComponentDataListAsync<Game.Prefabs.ArchetypeData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_HouseholdPrefabs = this.m_HouseholdPrefabQuery.ToComponentDataListAsync<Game.Prefabs.HouseholdData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle3),
        m_OutsideConnectionEntities = this.m_OutsideConnectionQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle4),
        m_Tourisms = this.__TypeHandle.__Game_City_Tourism_RO_ComponentLookup,
        m_OutsideConnectionDatas = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_AttractivenessParameter = this.m_AttractivenessParameterQuery.GetSingleton<AttractivenessParameterData>(),
        m_DemandParameterData = this.m_DemandParameterQuery.GetSingleton<DemandParameterData>(),
        m_WeatherClassification = this.m_ClimateSystem.classification,
        m_Temperature = (float) this.m_ClimateSystem.temperature,
        m_Precipitation = (float) this.m_ClimateSystem.precipitation,
        m_IsRaining = this.m_ClimateSystem.isRaining,
        m_IsSnowing = this.m_ClimateSystem.isSnowing,
        m_City = this.m_CitySystem.City,
        m_Frame = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      this.Dependency = jobData.Schedule<TouristSpawnSystem.SpawnTouristHouseholdJob>(JobHandle.CombineDependencies(outJobHandle1, outJobHandle2, JobHandle.CombineDependencies(outJobHandle3, this.Dependency, outJobHandle4)));
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
    public TouristSpawnSystem()
    {
    }

    [BurstCompile]
    private struct SpawnTouristHouseholdJob : IJob
    {
      [ReadOnly]
      public NativeList<Entity> m_PrefabEntities;
      [ReadOnly]
      public NativeList<Game.Prefabs.ArchetypeData> m_Archetypes;
      [ReadOnly]
      public NativeList<Game.Prefabs.HouseholdData> m_HouseholdPrefabs;
      [ReadOnly]
      public NativeList<Entity> m_OutsideConnectionEntities;
      [ReadOnly]
      public ComponentLookup<Tourism> m_Tourisms;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public AttractivenessParameterData m_AttractivenessParameter;
      [ReadOnly]
      public DemandParameterData m_DemandParameterData;
      public RandomSeed m_RandomSeed;
      public Entity m_City;
      public uint m_Frame;
      public EntityCommandBuffer m_CommandBuffer;
      public ClimateSystem.WeatherClassification m_WeatherClassification;
      public float m_Temperature;
      public float m_Precipitation;
      public bool m_IsRaining;
      public bool m_IsSnowing;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Tourisms.HasComponent(this.m_City))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom((int) this.m_Frame);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int attractiveness = this.m_Tourisms[this.m_City].m_Attractiveness;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if ((double) random.NextFloat() >= (double) TourismSystem.GetTouristProbability(this.m_AttractivenessParameter, attractiveness, this.m_WeatherClassification, this.m_Temperature, this.m_Precipitation, this.m_IsRaining, this.m_IsSnowing))
          return;
        // ISSUE: reference to a compiler-generated field
        int index = random.NextInt(this.m_HouseholdPrefabs.Length);
        // ISSUE: reference to a compiler-generated field
        Entity prefabEntity = this.m_PrefabEntities[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(this.m_Archetypes[index].m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef()
        {
          m_Prefab = prefabEntity
        });
        Household component1 = new Household()
        {
          m_Flags = HouseholdFlags.Tourist
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Household>(entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<TouristHousehold>(entity, new TouristHousehold()
        {
          m_Hotel = Entity.Null,
          m_LeavingTime = 0U
        });
        Entity result;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_OutsideConnectionEntities.Length <= 0 || !BuildingUtils.GetRandomOutsideConnectionByParameters(ref this.m_OutsideConnectionEntities, ref this.m_OutsideConnectionDatas, ref this.m_PrefabRefs, random, this.m_DemandParameterData.m_TouristOCSpawnParameters, out result))
          return;
        CurrentBuilding component2 = new CurrentBuilding()
        {
          m_CurrentBuilding = result
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CurrentBuilding>(entity, component2);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Tourism> __Game_City_Tourism_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Tourism_RO_ComponentLookup = state.GetComponentLookup<Tourism>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
      }
    }
  }
}
