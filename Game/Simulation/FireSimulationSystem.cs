// Decompiled with JetBrains decompiler
// Type: Game.Simulation.FireSimulationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Economy;
using Game.Events;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class FireSimulationSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private Game.Objects.SearchSystem m_SearchSystem;
    private IconCommandSystem m_IconCommandSystem;
    private LocalEffectSystem m_LocalEffectSystem;
    private PrefabSystem m_PrefabSystem;
    private TelecomCoverageSystem m_TelecomCoverageSystem;
    private TimeSystem m_TimeSystem;
    private ClimateSystem m_ClimateSystem;
    private FireHazardSystem m_FireHazardSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_FireQuery;
    private EntityQuery m_ConfigQuery;
    private EntityArchetype m_FireRescueRequestArchetype;
    private EntityArchetype m_DamageEventArchetype;
    private EntityArchetype m_DestroyEventArchetype;
    private EntityArchetype m_IgniteEventArchetype;
    private FireSimulationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalEffectSystem = this.World.GetOrCreateSystemManaged<LocalEffectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FireHazardSystem = this.World.GetOrCreateSystemManaged<FireHazardSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_FireQuery = this.GetEntityQuery(ComponentType.ReadWrite<OnFire>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FireRescueRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<FireRescueRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_DamageEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Damage>());
      // ISSUE: reference to a compiler-generated field
      this.m_DestroyEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Destroy>());
      // ISSUE: reference to a compiler-generated field
      this.m_IgniteEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Ignite>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FireQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      FireConfigurationData singleton = this.m_ConfigQuery.GetSingleton<FireConfigurationData>();
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      LocalEffectSystem.ReadData readData = this.m_LocalEffectSystem.GetReadData(out dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      FireConfigurationPrefab prefab = this.m_PrefabSystem.GetPrefab<FireConfigurationPrefab>(this.m_ConfigQuery.GetSingletonEntity());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_FireRescueRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FireSimulationSystem.FireSimulationJob jobData1 = new FireSimulationSystem.FireSimulationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_CurrentDistrictType = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_OnFireType = this.__TypeHandle.__Game_Events_OnFire_RW_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_FireRescueRequestData = this.__TypeHandle.__Game_Simulation_FireRescueRequest_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabFireData = this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_RenterData = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_ResourcesData = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_FireRescueRequestArchetype = this.m_FireRescueRequestArchetype,
        m_DamageEventArchetype = this.m_DamageEventArchetype,
        m_DestroyEventArchetype = this.m_DestroyEventArchetype,
        m_FireConfigurationData = singleton,
        m_StructuralIntegrityData = new EventHelpers.StructuralIntegrityData((SystemBase) this, singleton),
        m_TelecomCoverageData = this.m_TelecomCoverageSystem.GetData(true, out dependencies2),
        m_TimeOfDay = this.m_TimeSystem.normalizedTime,
        m_LocalEffectData = readData,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies3;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FireSimulationSystem.FireSpreadCheckJob jobData2 = new FireSimulationSystem.FireSpreadCheckJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_OnFireType = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabFireData = this.__TypeHandle.__Game_Prefabs_FireData_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup,
        m_UnderConstructionData = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_FireHazardData = new EventHelpers.FireHazardData((SystemBase) this, readData, prefab, (float) this.m_ClimateSystem.temperature, this.m_FireHazardSystem.noRainDays),
        m_ObjectSearchTree = this.m_SearchSystem.GetStaticSearchTree(true, out dependencies3),
        m_IgniteEventArchetype = this.m_IgniteEventArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<FireSimulationSystem.FireSimulationJob>(this.m_FireQuery, JobHandle.CombineDependencies(this.Dependency, dependencies2, dependencies1));
      // ISSUE: reference to a compiler-generated field
      EntityQuery fireQuery = this.m_FireQuery;
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle1, dependencies3);
      JobHandle jobHandle2 = jobData2.ScheduleParallel<FireSimulationSystem.FireSpreadCheckJob>(fireQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem.AddReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LocalEffectSystem.AddLocalEffectReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SearchSystem.AddStaticSearchTreeReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = jobHandle2;
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
    public FireSimulationSystem()
    {
    }

    [BurstCompile]
    private struct FireSimulationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Tree> m_TreeType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public ComponentTypeHandle<OnFire> m_OnFireType;
      public ComponentTypeHandle<Damaged> m_DamagedType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> m_FireRescueRequestData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<FireData> m_PrefabFireData;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Resources> m_ResourcesData;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public EntityArchetype m_FireRescueRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_DamageEventArchetype;
      [ReadOnly]
      public EntityArchetype m_DestroyEventArchetype;
      [ReadOnly]
      public FireConfigurationData m_FireConfigurationData;
      [ReadOnly]
      public EventHelpers.StructuralIntegrityData m_StructuralIntegrityData;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverageData;
      [ReadOnly]
      public float m_TimeOfDay;
      [ReadOnly]
      public LocalEffectSystem.ReadData m_LocalEffectData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        float num = 1.06666672f;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<OnFire> nativeArray3 = chunk.GetNativeArray<OnFire>(ref this.m_OnFireType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Damaged> nativeArray4 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray5 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray6 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        bool isBuilding = chunk.Has<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        bool isTree = chunk.Has<Tree>(ref this.m_TreeType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Destroyed>(ref this.m_DestroyedType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray3.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          PrefabRef prefabRef = nativeArray2[index1];
          ref OnFire local = ref nativeArray3.ElementAt<OnFire>(index1);
          bool flag2 = false;
          if (nativeArray4.Length != 0)
            flag2 = (double) nativeArray4[index1].m_Damage.y >= 1.0;
          if ((double) local.m_Intensity > 0.0)
          {
            PrefabRef componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.TryGetComponent(local.m_Event, out componentData))
            {
              // ISSUE: reference to a compiler-generated field
              FireData fireData = this.m_PrefabFireData[componentData.m_Prefab];
              local.m_Intensity = !flag2 ? math.min(100f, local.m_Intensity + fireData.m_EscalationRate * num) : math.max(0.0f, local.m_Intensity - 2f * fireData.m_EscalationRate * num);
            }
            else
              local.m_Intensity = 0.0f;
          }
          if ((double) local.m_Intensity > 0.0 && !flag2)
          {
            // ISSUE: reference to a compiler-generated field
            float structuralIntegrity = this.m_StructuralIntegrityData.GetStructuralIntegrity(prefabRef.m_Prefab, isBuilding);
            float y = math.min(0.5f, local.m_Intensity * num / structuralIntegrity);
            if (nativeArray4.Length != 0)
            {
              Damaged damaged = nativeArray4[index1];
              damaged.m_Damage.y = math.min(1f, damaged.m_Damage.y + y);
              flag2 |= (double) damaged.m_Damage.y >= 1.0;
              if (!flag1 && (double) ObjectUtils.GetTotalDamage(damaged) == 1.0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_DestroyEventArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Destroy>(unfilteredChunkIndex, entity2, new Destroy(entity1, local.m_Event));
                if (!isTree)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity1, this.m_FireConfigurationData.m_FireNotificationPrefab);
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity1, IconPriority.Problem);
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(entity1, IconPriority.FatalProblem);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(entity1, this.m_FireConfigurationData.m_BurnedDownNotificationPrefab, IconPriority.FatalProblem, flags: IconFlags.IgnoreTarget, target: local.m_Event);
                }
              }
              nativeArray4[index1] = damaged;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity3 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_DamageEventArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Damage>(unfilteredChunkIndex, entity3, new Damage(entity1, new float3(0.0f, y, 0.0f)));
            }
          }
          if ((double) local.m_Intensity > 0.0)
          {
            if (!flag2)
            {
              if (local.m_RequestFrame == 0U)
              {
                Transform transform = nativeArray5[index1];
                CurrentDistrict currentDistrict = new CurrentDistrict();
                if (nativeArray6.Length != 0)
                  currentDistrict = nativeArray6[index1];
                // ISSUE: reference to a compiler-generated method
                this.InitializeRequestFrame(isBuilding, isTree, transform, currentDistrict, ref local, ref random);
              }
              // ISSUE: reference to a compiler-generated method
              this.RequestFireRescueIfNeeded(unfilteredChunkIndex, entity1, ref local);
            }
          }
          else
          {
            if (isBuilding && nativeArray4.Length > 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ObjectUtils.UpdateResourcesDamage(entity1, ObjectUtils.GetTotalDamage(nativeArray4[index1]), ref this.m_RenterData, ref this.m_ResourcesData);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<OnFire>(unfilteredChunkIndex, entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, entity1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(entity1, this.m_FireConfigurationData.m_FireNotificationPrefab);
            DynamicBuffer<InstalledUpgrade> dynamicBuffer;
            if (CollectionUtils.TryGet<InstalledUpgrade>(bufferAccessor2, index1, out dynamicBuffer))
            {
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Entity upgrade = dynamicBuffer[index2].m_Upgrade;
                // ISSUE: reference to a compiler-generated field
                if (!this.m_BuildingData.HasComponent(upgrade))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, upgrade);
                }
              }
            }
          }
          if (bufferAccessor1.Length != 0)
            BuildingUtils.SetEfficiencyFactor(bufferAccessor1[index1], EfficiencyFactor.Fire, (double) local.m_Intensity > 0.0099999997764825821 ? 0.0f : 1f);
        }
      }

      private void InitializeRequestFrame(
        bool isBuilding,
        bool isTree,
        Transform transform,
        CurrentDistrict currentDistrict,
        ref OnFire onFire,
        ref Random random)
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = math.saturate((float) ((double) math.abs(this.m_TimeOfDay - 0.5f) * 4.0 - 1.0));
        // ISSUE: reference to a compiler-generated field
        float num2 = TelecomCoverage.SampleNetworkQuality(this.m_TelecomCoverageData, transform.m_Position);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num3 = random.NextFloat(this.m_FireConfigurationData.m_ResponseTimeRange.min, this.m_FireConfigurationData.m_ResponseTimeRange.max);
        // ISSUE: reference to a compiler-generated field
        float num4 = num3 + num3 * (this.m_FireConfigurationData.m_DarknessResponseTimeModifier * num1);
        // ISSUE: reference to a compiler-generated field
        float num5 = num4 + num4 * (this.m_FireConfigurationData.m_TelecomResponseTimeModifier * num2);
        // ISSUE: reference to a compiler-generated field
        if (isBuilding && this.m_DistrictModifiers.HasBuffer(currentDistrict.m_District))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<DistrictModifier> districtModifier = this.m_DistrictModifiers[currentDistrict.m_District];
          AreaUtils.ApplyModifier(ref num5, districtModifier, DistrictModifierType.BuildingFireResponseTime);
        }
        if (isTree)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_LocalEffectData.ApplyModifier(ref num5, transform.m_Position, LocalModifierType.ForestFireResponseTime);
        }
        int y = (int) ((double) num5 * 60.0) - 32 - 128;
        // ISSUE: reference to a compiler-generated field
        onFire.m_RequestFrame = this.m_SimulationFrame + (uint) math.max(0, y);
      }

      private void RequestFireRescueIfNeeded(int jobIndex, Entity entity, ref OnFire onFire)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (onFire.m_RequestFrame > this.m_SimulationFrame || this.m_FireRescueRequestData.HasComponent(onFire.m_RescueRequest))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_FireRescueRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<FireRescueRequest>(jobIndex, entity1, new FireRescueRequest(entity, onFire.m_Intensity, FireRescueRequestType.Fire));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(4U));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IconCommandBuffer.Add(entity, this.m_FireConfigurationData.m_FireNotificationPrefab, IconPriority.MajorProblem, flags: IconFlags.IgnoreTarget, target: onFire.m_Event);
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
    private struct FireSpreadCheckJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<OnFire> m_OnFireType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<FireData> m_PrefabFireData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Damaged> m_DamagedData;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> m_UnderConstructionData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EventHelpers.FireHazardData m_FireHazardData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public EntityArchetype m_IgniteEventArchetype;
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
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<OnFire> nativeArray3 = chunk.GetNativeArray<OnFire>(ref this.m_OnFireType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          OnFire onFire = nativeArray3[index];
          Transform transform = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          if ((double) onFire.m_Intensity > 0.0 && this.m_PrefabRefData.HasComponent(onFire.m_Event))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            FireData prefabFireData = this.m_PrefabFireData[this.m_PrefabRefData[onFire.m_Event].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            Random random = this.m_RandomSeed.GetRandom(entity.Index);
            // ISSUE: reference to a compiler-generated method
            this.TrySpreadFire(unfilteredChunkIndex, entity, onFire, ref random, prefabRef, transform, prefabFireData);
          }
        }
      }

      private void TrySpreadFire(
        int jobIndex,
        Entity entity,
        OnFire onFire,
        ref Random random,
        PrefabRef prefabRef,
        Transform transform,
        FireData prefabFireData)
      {
        float num1 = 1.06666672f;
        float num2 = math.sqrt(prefabFireData.m_SpreadProbability * 0.01f);
        if ((double) random.NextFloat(100f) >= (double) onFire.m_Intensity * (double) num2 * (double) num1)
          return;
        // ISSUE: reference to a compiler-generated field
        float num3 = math.cmin(this.m_ObjectGeometryData[prefabRef.m_Prefab].m_Size.xz) * 0.5f;
        float num4 = prefabFireData.m_SpreadRange + num3;
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
        FireSimulationSystem.FireSpreadCheckJob.ObjectSpreadIterator iterator = new FireSimulationSystem.FireSpreadCheckJob.ObjectSpreadIterator()
        {
          m_Event = onFire.m_Event,
          m_Random = random,
          m_Position = transform.m_Position,
          m_Bounds = new Bounds3(transform.m_Position - num4, transform.m_Position + num4),
          m_Range = prefabFireData.m_SpreadRange,
          m_Size = num3,
          m_StartIntensity = prefabFireData.m_StartIntensity,
          m_Probability = num2,
          m_RequestFrame = onFire.m_RequestFrame,
          m_JobIndex = jobIndex,
          m_FireHazardData = this.m_FireHazardData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_BuildingData = this.m_BuildingData,
          m_CurrentDistrictData = this.m_CurrentDistrictData,
          m_TreeData = this.m_TreeData,
          m_TransformData = this.m_TransformData,
          m_ObjectGeometryData = this.m_ObjectGeometryData,
          m_DamagedData = this.m_DamagedData,
          m_UnderConstructionData = this.m_UnderConstructionData,
          m_IgniteEventArchetype = this.m_IgniteEventArchetype,
          m_CommandBuffer = this.m_CommandBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<FireSimulationSystem.FireSpreadCheckJob.ObjectSpreadIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        random = iterator.m_Random;
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

      private struct ObjectSpreadIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_Event;
        public Random m_Random;
        public float3 m_Position;
        public Bounds3 m_Bounds;
        public float m_Range;
        public float m_Size;
        public float m_StartIntensity;
        public float m_Probability;
        public uint m_RequestFrame;
        public int m_JobIndex;
        public EventHelpers.FireHazardData m_FireHazardData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
        public ComponentLookup<Tree> m_TreeData;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
        public ComponentLookup<Damaged> m_DamagedData;
        public ComponentLookup<UnderConstruction> m_UnderConstructionData;
        public EntityArchetype m_IgniteEventArchetype;
        public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return (bounds.m_Mask & BoundsMask.NotOverridden) != (BoundsMask) 0 && MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          if ((bounds.m_Mask & BoundsMask.NotOverridden) == (BoundsMask) 0 || !MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
            return;
          float riskFactor;
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingData.HasComponent(item))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[item];
            // ISSUE: reference to a compiler-generated field
            Building building = this.m_BuildingData[item];
            // ISSUE: reference to a compiler-generated field
            CurrentDistrict currentDistrict = this.m_CurrentDistrictData[item];
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[item];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float distance = math.distance(transform.m_Position, this.m_Position) - math.cmin(objectGeometryData.m_Size.xz) * 0.5f - this.m_Size;
            // ISSUE: reference to a compiler-generated field
            if ((double) distance >= (double) this.m_Range)
              return;
            Damaged componentData1;
            // ISSUE: reference to a compiler-generated field
            this.m_DamagedData.TryGetComponent(item, out componentData1);
            UnderConstruction componentData2;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_UnderConstructionData.TryGetComponent(item, out componentData2))
              componentData2 = new UnderConstruction()
              {
                m_Progress = byte.MaxValue
              };
            float fireHazard;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_FireHazardData.GetFireHazard(prefabRef, building, currentDistrict, componentData1, componentData2, out fireHazard, out riskFactor))
              return;
            // ISSUE: reference to a compiler-generated method
            this.TrySpreadFire(item, fireHazard, distance);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_TreeData.HasComponent(item))
              return;
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[item];
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[item];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
            Damaged damaged = new Damaged();
            // ISSUE: reference to a compiler-generated field
            if (this.m_DamagedData.HasComponent(item))
            {
              // ISSUE: reference to a compiler-generated field
              damaged = this.m_DamagedData[item];
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float y = math.distance(transform.m_Position, this.m_Position) - math.cmin(objectGeometryData.m_Size.xz) * 0.5f - this.m_Size;
            float fireHazard;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) y >= (double) this.m_Range || !this.m_FireHazardData.GetFireHazard(prefabRef, new Tree(), transform, damaged, out fireHazard, out riskFactor))
              return;
            // ISSUE: reference to a compiler-generated method
            this.TrySpreadFire(item, fireHazard, math.max(0.0f, y));
          }
        }

        private void TrySpreadFire(Entity entity, float fireHazard, float distance)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_Random.NextFloat(100f) * (double) this.m_Range >= (double) fireHazard * ((double) this.m_Range - (double) distance) * (double) this.m_Probability)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Ignite component = new Ignite()
          {
            m_Target = entity,
            m_Event = this.m_Event,
            m_Intensity = this.m_StartIntensity,
            m_RequestFrame = this.m_RequestFrame + 64U
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Ignite>(this.m_JobIndex, this.m_CommandBuffer.CreateEntity(this.m_JobIndex, this.m_IgniteEventArchetype), component);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Tree> __Game_Objects_Tree_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      public ComponentTypeHandle<OnFire> __Game_Events_OnFire_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RW_ComponentTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> __Game_Simulation_FireRescueRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FireData> __Game_Prefabs_FireData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      public BufferLookup<Resources> __Game_Economy_Resources_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<OnFire> __Game_Events_OnFire_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RW_ComponentTypeHandle = state.GetComponentTypeHandle<OnFire>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_FireRescueRequest_RO_ComponentLookup = state.GetComponentLookup<FireRescueRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FireData_RO_ComponentLookup = state.GetComponentLookup<FireData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentLookup = state.GetComponentLookup<UnderConstruction>(true);
      }
    }
  }
}
