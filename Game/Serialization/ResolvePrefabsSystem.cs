// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ResolvePrefabsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Simulation;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class ResolvePrefabsSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private PrefabSystem m_PrefabSystem;
    private UpdateSystem m_UpdateSystem;
    private CheckPrefabReferencesSystem m_CheckPrefabReferencesSystem;
    private EntityQuery m_ActualPrefabQuery;
    private EntityQuery m_EnabledLoadedPrefabQuery;
    private EntityQuery m_AllLoadedPrefabQuery;
    private EntityQuery m_LoadedZonePrefabQuery;
    private EntityQuery m_LoadedZoneCellQuery;
    private EntityQuery m_ActualBudgetQuery;
    private ResolvePrefabsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CheckPrefabReferencesSystem = this.World.GetOrCreateSystemManaged<CheckPrefabReferencesSystem>();
      EntityQueryBuilder entityQueryBuilder1 = new EntityQueryBuilder((AllocatorManager.AllocatorHandle) Allocator.Temp);
      entityQueryBuilder1 = entityQueryBuilder1.WithAll<PrefabData, LoadedIndex>();
      entityQueryBuilder1 = entityQueryBuilder1.WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
      // ISSUE: reference to a compiler-generated field
      this.m_ActualPrefabQuery = entityQueryBuilder1.Build((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_EnabledLoadedPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<LoadedIndex>());
      EntityQueryBuilder entityQueryBuilder2 = new EntityQueryBuilder((AllocatorManager.AllocatorHandle) Allocator.Temp);
      entityQueryBuilder2 = entityQueryBuilder2.WithAll<PrefabData>();
      entityQueryBuilder2 = entityQueryBuilder2.WithNone<LoadedIndex>();
      entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
      // ISSUE: reference to a compiler-generated field
      this.m_AllLoadedPrefabQuery = entityQueryBuilder2.Build((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedZonePrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<ZoneData>(), ComponentType.Exclude<LoadedIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedZoneCellQuery = this.GetEntityQuery(ComponentType.ReadOnly<Cell>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActualBudgetQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<LoadedIndex>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<CollectedCityServiceBudgetData>(),
          ComponentType.ReadOnly<CollectedCityServiceFeeData>(),
          ComponentType.ReadOnly<CollectedCityServiceUpkeepData>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      int entityCount = this.m_EnabledLoadedPrefabQuery.CalculateEntityCount();
      NativeArray<Entity> array = new NativeArray<Entity>(entityCount, Allocator.TempJob);
      NativeArray<PrefabComponents> nativeArray1 = new NativeArray<PrefabComponents>(entityCount, Allocator.TempJob);
      NativeArray<ZoneType> nativeArray2 = new NativeArray<ZoneType>(340, Allocator.TempJob);
      NativeQueue<ResolvePrefabsSystem.ComponentModification> nativeQueue = new NativeQueue<ResolvePrefabsSystem.ComponentModification>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResolvePrefabsSystem.FillLoadedPrefabsJob jobData1 = new ResolvePrefabsSystem.FillLoadedPrefabsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabDataType = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle,
        m_LockedType = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle,
        m_PlacedSignatureType = this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle,
        m_PrefabArray = array,
        m_PrefabComponents = nativeArray1
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LoadedIndex_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResolvePrefabsSystem.CheckActualPrefabsJob jobData2 = new ResolvePrefabsSystem.CheckActualPrefabsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabDataType = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle,
        m_SignatureBuildingType = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle,
        m_PlacedSignatureBuildingType = this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle,
        m_LoadedIndexType = this.__TypeHandle.__Game_Prefabs_LoadedIndex_RO_BufferTypeHandle,
        m_Context = this.m_LoadGameSystem.context,
        m_PrefabComponents = nativeArray1,
        m_LockedType = this.__TypeHandle.__Game_Prefabs_Locked_RW_ComponentTypeHandle,
        m_PrefabArray = array,
        m_ComponentModifications = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceFeeData_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LoadedIndex_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      ResolvePrefabsSystem.CopyBudgetDataJob jobData3 = new ResolvePrefabsSystem.CopyBudgetDataJob()
      {
        m_PrefabDataType = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle,
        m_LoadedIndexType = this.__TypeHandle.__Game_Prefabs_LoadedIndex_RO_BufferTypeHandle,
        m_PrefabArray = array,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_Budgets = this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup,
        m_Fees = this.__TypeHandle.__Game_Simulation_CollectedCityServiceFeeData_RW_BufferLookup,
        m_Upkeeps = this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResolvePrefabsSystem.FillZoneTypeArrayJob jobData4 = new ResolvePrefabsSystem.FillZoneTypeArrayJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabDataType = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle,
        m_ZoneDataType = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle,
        m_ZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_PrefabArray = array,
        m_ZoneTypeArray = nativeArray2
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_VacantLot_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResolvePrefabsSystem.FixZoneTypeJob jobData5 = new ResolvePrefabsSystem.FixZoneTypeJob()
      {
        m_ZoneTypeArray = nativeArray2,
        m_CellType = this.__TypeHandle.__Game_Zones_Cell_RW_BufferTypeHandle,
        m_VacantLotType = this.__TypeHandle.__Game_Zones_VacantLot_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn1 = jobData1.ScheduleParallel<ResolvePrefabsSystem.FillLoadedPrefabsJob>(this.m_EnabledLoadedPrefabQuery, this.Dependency);
      JobHandle.ScheduleBatchedJobs();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabSystem.UpdateLoadedIndices();
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn2 = jobData3.ScheduleParallel<ResolvePrefabsSystem.CopyBudgetDataJob>(this.m_ActualBudgetQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn3 = jobData2.ScheduleParallel<ResolvePrefabsSystem.CheckActualPrefabsJob>(this.m_ActualPrefabQuery, dependsOn2);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData4.ScheduleParallel<ResolvePrefabsSystem.FillZoneTypeArrayJob>(this.m_LoadedZonePrefabQuery, dependsOn3);
      // ISSUE: reference to a compiler-generated field
      EntityQuery loadedZoneCellQuery = this.m_LoadedZoneCellQuery;
      JobHandle dependsOn4 = jobHandle;
      JobHandle dependencies1 = jobData5.ScheduleParallel<ResolvePrefabsSystem.FixZoneTypeJob>(loadedZoneCellQuery, dependsOn4);
      jobHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.SetComponentEnabled<PrefabData>(this.m_ActualPrefabQuery, false);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.SetComponentEnabled<PrefabData>(this.m_EnabledLoadedPrefabQuery, false);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CheckPrefabReferencesSystem.BeginPrefabCheck(array, true, dependencies1);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.PrefabReferences);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CheckPrefabReferencesSystem.EndPrefabCheck(out dependencies2);
      dependencies2.Complete();
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.SetComponentEnabled<PrefabData>(this.m_ActualPrefabQuery, true);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_EnabledLoadedPrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      ComponentType type = ComponentType.ReadWrite<PlacedSignatureBuildingData>();
      // ISSUE: variable of a compiler-generated type
      ResolvePrefabsSystem.ComponentModification componentModification;
      while (nativeQueue.TryDequeue(out componentModification))
      {
        // ISSUE: reference to a compiler-generated method
        this.AddOrRemoveComponent(componentModification, PrefabComponents.PlacedSignatureBuilding, type);
      }
      EntityManager entityManager;
      if (entityArray.Length != 0)
      {
        entityManager = this.EntityManager;
        entityManager.AddComponent<LoadedIndex>(entityArray);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Entity entity = entityArray[index];
          entityManager = this.EntityManager;
          PrefabData componentData = entityManager.GetComponentData<PrefabData>(entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabID loadedObsoleteId = this.m_PrefabSystem.GetLoadedObsoleteID(componentData.m_Index);
          componentData.m_Index = -1 - index;
          entityManager = this.EntityManager;
          entityManager.SetComponentData<PrefabData>(entity, componentData);
          entityManager = this.EntityManager;
          entityManager.SetComponentEnabled<PrefabData>(entity, false);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PrefabSystem.AddObsoleteID(entity, loadedObsoleteId);
        }
      }
      entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      entityManager.DestroyEntity(this.m_AllLoadedPrefabQuery);
      array.Dispose();
      nativeArray1.Dispose();
      nativeArray2.Dispose();
      nativeQueue.Dispose();
      entityArray.Dispose();
    }

    private void AddOrRemoveComponent(
      ResolvePrefabsSystem.ComponentModification componentModification,
      PrefabComponents mask,
      ComponentType type)
    {
      // ISSUE: reference to a compiler-generated field
      if ((componentModification.m_Remove & mask) != (PrefabComponents) 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.RemoveComponent(componentModification.m_Entity, type);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if ((componentModification.m_Add & mask) == (PrefabComponents) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.AddComponent(componentModification.m_Entity, type);
      }
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
    public ResolvePrefabsSystem()
    {
    }

    private struct ComponentModification
    {
      public Entity m_Entity;
      public PrefabComponents m_Add;
      public PrefabComponents m_Remove;

      public ComponentModification(Entity entity, PrefabComponents add, PrefabComponents remove)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Add = add;
        // ISSUE: reference to a compiler-generated field
        this.m_Remove = remove;
      }
    }

    [BurstCompile]
    private struct FillLoadedPrefabsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> m_PrefabDataType;
      [ReadOnly]
      public ComponentTypeHandle<Locked> m_LockedType;
      [ReadOnly]
      public ComponentTypeHandle<PlacedSignatureBuildingData> m_PlacedSignatureType;
      [NativeDisableParallelForRestriction]
      public NativeArray<Entity> m_PrefabArray;
      [NativeDisableParallelForRestriction]
      public NativeArray<PrefabComponents> m_PrefabComponents;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabData> nativeArray2 = chunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
        PrefabComponents prefabComponents1 = (PrefabComponents) 0;
        // ISSUE: reference to a compiler-generated field
        EnabledMask enabledMask = chunk.GetEnabledMask<Locked>(ref this.m_LockedType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<PlacedSignatureBuildingData>(ref this.m_PlacedSignatureType))
          prefabComponents1 |= PrefabComponents.PlacedSignatureBuilding;
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          int index2 = nativeArray2[index1].m_Index;
          // ISSUE: reference to a compiler-generated field
          int index3 = math.select(index2, this.m_PrefabArray.Length + index2, index2 < 0);
          // ISSUE: reference to a compiler-generated field
          this.m_PrefabArray[index3] = nativeArray1[index1];
          PrefabComponents prefabComponents2 = prefabComponents1;
          if (enabledMask.EnableBit.IsValid && enabledMask[index1])
            prefabComponents2 |= PrefabComponents.Locked;
          // ISSUE: reference to a compiler-generated field
          this.m_PrefabComponents[index3] = prefabComponents2;
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
    private struct CheckActualPrefabsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> m_PrefabDataType;
      [ReadOnly]
      public ComponentTypeHandle<SignatureBuildingData> m_SignatureBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<PlacedSignatureBuildingData> m_PlacedSignatureBuildingType;
      [ReadOnly]
      public BufferTypeHandle<LoadedIndex> m_LoadedIndexType;
      [ReadOnly]
      public Colossal.Serialization.Entities.Context m_Context;
      [ReadOnly]
      public NativeArray<PrefabComponents> m_PrefabComponents;
      public ComponentTypeHandle<Locked> m_LockedType;
      [NativeDisableParallelForRestriction]
      public NativeArray<Entity> m_PrefabArray;
      public NativeQueue<ResolvePrefabsSystem.ComponentModification>.ParallelWriter m_ComponentModifications;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabData> nativeArray2 = chunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LoadedIndex> bufferAccessor = chunk.GetBufferAccessor<LoadedIndex>(ref this.m_LoadedIndexType);
        PrefabComponents prefabComponents1 = (PrefabComponents) 0;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<SignatureBuildingData>(ref this.m_SignatureBuildingType))
          prefabComponents1 |= PrefabComponents.PlacedSignatureBuilding;
        PrefabComponents prefabComponents2 = (PrefabComponents) 0;
        // ISSUE: reference to a compiler-generated field
        EnabledMask enabledMask = chunk.GetEnabledMask<Locked>(ref this.m_LockedType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<PlacedSignatureBuildingData>(ref this.m_PlacedSignatureBuildingType))
          prefabComponents2 |= PrefabComponents.PlacedSignatureBuilding;
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          DynamicBuffer<LoadedIndex> dynamicBuffer = bufferAccessor[index1];
          PrefabComponents prefabComponents3 = PrefabComponents.Locked;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            int index3 = dynamicBuffer[index2].m_Index;
            // ISSUE: reference to a compiler-generated field
            int index4 = math.select(index3, this.m_PrefabArray.Length + index3, index3 < 0);
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabArray[index4] = entity;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Context.purpose == Colossal.Serialization.Entities.Purpose.LoadGame)
            {
              // ISSUE: reference to a compiler-generated field
              prefabComponents3 = this.m_PrefabComponents[index4];
            }
          }
          if (enabledMask.EnableBit.IsValid)
          {
            enabledMask[index1] = (prefabComponents3 & PrefabComponents.Locked) > (PrefabComponents) 0;
            prefabComponents3 &= ~PrefabComponents.Locked;
          }
          PrefabComponents prefabComponents4 = prefabComponents3 & prefabComponents1;
          if (prefabComponents4 != prefabComponents2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ComponentModifications.Enqueue(new ResolvePrefabsSystem.ComponentModification(entity, prefabComponents4 & ~prefabComponents2, prefabComponents2 & ~prefabComponents4));
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
    private struct CopyBudgetDataJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> m_PrefabDataType;
      [ReadOnly]
      public BufferTypeHandle<LoadedIndex> m_LoadedIndexType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CollectedCityServiceBudgetData> m_Budgets;
      [NativeDisableParallelForRestriction]
      public BufferLookup<CollectedCityServiceFeeData> m_Fees;
      [NativeDisableParallelForRestriction]
      public BufferLookup<CollectedCityServiceUpkeepData> m_Upkeeps;
      [ReadOnly]
      public NativeArray<Entity> m_PrefabArray;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabData> nativeArray2 = chunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LoadedIndex> bufferAccessor = chunk.GetBufferAccessor<LoadedIndex>(ref this.m_LoadedIndexType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          DynamicBuffer<LoadedIndex> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            int index3 = dynamicBuffer[index2].m_Index;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_PrefabArray[math.select(index3, this.m_PrefabArray.Length + index3, index3 < 0)];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Budgets.HasComponent(entity) && this.m_Budgets.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Budgets[entity] = this.m_Budgets[prefab];
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Fees.HasBuffer(entity) && this.m_Fees.HasBuffer(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CollectedCityServiceFeeData> fee1 = this.m_Fees[entity];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CollectedCityServiceFeeData> fee2 = this.m_Fees[prefab];
              for (int index4 = 0; index4 < fee2.Length; ++index4)
              {
                for (int index5 = 0; index5 < fee1.Length; ++index5)
                {
                  if (fee1[index5].m_PlayerResource == fee2[index4].m_PlayerResource)
                    fee1[index5] = fee2[index4];
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Upkeeps.HasBuffer(entity) && this.m_Upkeeps.HasBuffer(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CollectedCityServiceUpkeepData> upkeep1 = this.m_Upkeeps[entity];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CollectedCityServiceUpkeepData> upkeep2 = this.m_Upkeeps[prefab];
              for (int index6 = 0; index6 < upkeep2.Length; ++index6)
              {
                for (int index7 = 0; index7 < upkeep1.Length; ++index7)
                {
                  if (upkeep1[index7].m_Resource == upkeep2[index6].m_Resource)
                    upkeep1[index7] = upkeep2[index6];
                }
              }
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
    private struct FillZoneTypeArrayJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> m_PrefabDataType;
      [ReadOnly]
      public ComponentTypeHandle<ZoneData> m_ZoneDataType;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneData;
      [ReadOnly]
      public NativeArray<Entity> m_PrefabArray;
      [NativeDisableParallelForRestriction]
      public NativeArray<ZoneType> m_ZoneTypeArray;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabData> nativeArray2 = chunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ZoneData> nativeArray3 = chunk.GetNativeArray<ZoneData>(ref this.m_ZoneDataType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          int index2 = nativeArray2[index1].m_Index;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_PrefabArray[math.select(index2, this.m_PrefabArray.Length + index2, index2 < 0)];
          ZoneType zoneType1 = nativeArray3[index1].m_ZoneType;
          // ISSUE: reference to a compiler-generated field
          ZoneType zoneType2 = this.m_ZoneData[prefab].m_ZoneType;
          Entity entity2 = prefab;
          if (entity1 == entity2)
            zoneType2 = ZoneType.None;
          // ISSUE: reference to a compiler-generated field
          this.m_ZoneTypeArray[(int) zoneType1.m_Index] = zoneType2;
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
    private struct FixZoneTypeJob : IJobChunk
    {
      [ReadOnly]
      public NativeArray<ZoneType> m_ZoneTypeArray;
      public BufferTypeHandle<Cell> m_CellType;
      public BufferTypeHandle<VacantLot> m_VacantLotType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Cell> bufferAccessor1 = chunk.GetBufferAccessor<Cell>(ref this.m_CellType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<VacantLot> bufferAccessor2 = chunk.GetBufferAccessor<VacantLot>(ref this.m_VacantLotType);
        for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
        {
          DynamicBuffer<Cell> dynamicBuffer = bufferAccessor1[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Cell cell = dynamicBuffer[index2];
            // ISSUE: reference to a compiler-generated field
            cell.m_Zone = this.m_ZoneTypeArray[(int) cell.m_Zone.m_Index];
            dynamicBuffer[index2] = cell;
          }
        }
        for (int index3 = 0; index3 < bufferAccessor2.Length; ++index3)
        {
          DynamicBuffer<VacantLot> dynamicBuffer = bufferAccessor2[index3];
          for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
          {
            VacantLot vacantLot = dynamicBuffer[index4];
            // ISSUE: reference to a compiler-generated field
            vacantLot.m_Type = this.m_ZoneTypeArray[(int) vacantLot.m_Type.m_Index];
            dynamicBuffer[index4] = vacantLot;
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
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PlacedSignatureBuildingData> __Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LoadedIndex> __Game_Prefabs_LoadedIndex_RO_BufferTypeHandle;
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RW_ComponentTypeHandle;
      public ComponentLookup<CollectedCityServiceBudgetData> __Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup;
      public BufferLookup<CollectedCityServiceFeeData> __Game_Simulation_CollectedCityServiceFeeData_RW_BufferLookup;
      public BufferLookup<CollectedCityServiceUpkeepData> __Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      public BufferTypeHandle<Cell> __Game_Zones_Cell_RW_BufferTypeHandle;
      public BufferTypeHandle<VacantLot> __Game_Zones_VacantLot_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PlacedSignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LoadedIndex_RO_BufferTypeHandle = state.GetBufferTypeHandle<LoadedIndex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup = state.GetComponentLookup<CollectedCityServiceBudgetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceFeeData_RW_BufferLookup = state.GetBufferLookup<CollectedCityServiceFeeData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferLookup = state.GetBufferLookup<CollectedCityServiceUpkeepData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RW_BufferTypeHandle = state.GetBufferTypeHandle<Cell>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_VacantLot_RW_BufferTypeHandle = state.GetBufferTypeHandle<VacantLot>();
      }
    }
  }
}
