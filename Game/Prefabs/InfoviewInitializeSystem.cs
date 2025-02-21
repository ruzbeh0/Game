// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.InfoviewInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using Game.Net;
using Game.Rendering;
using Game.Routes;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class InfoviewInitializeSystem : GameSystemBase
  {
    private EntityQuery m_NewInfoviewQuery;
    private EntityQuery m_AllInfoviewQuery;
    private EntityQuery m_AllInfomodeQuery;
    private EntityQuery m_NewPlaceableQuery;
    private EntityQuery m_AllPlaceableQuery;
    private PrefabSystem m_PrefabSystem;
    private InfoviewInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NewInfoviewQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<InfoviewData>(), ComponentType.ReadOnly<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllInfoviewQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<InfoviewData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllInfomodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<InfomodeData>(), ComponentType.Exclude<InfomodeGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_NewPlaceableQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadWrite<PlaceableInfoviewItem>(), ComponentType.ReadOnly<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllPlaceableQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadWrite<PlaceableInfoviewItem>());
    }

    public IEnumerable<InfoviewPrefab> infoviews
    {
      get
      {
        InfoviewInitializeSystem initializeSystem = this;
        initializeSystem.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref initializeSystem.CheckedStateRef);
        ComponentTypeHandle<PrefabData> prefabType = initializeSystem.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        using (NativeArray<ArchetypeChunk> chunks = initializeSystem.m_AllInfoviewQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          for (int i = 0; i < chunks.Length; ++i)
          {
            NativeArray<PrefabData> prefabs = chunks[i].GetNativeArray<PrefabData>(ref prefabType);
            for (int j = 0; j < prefabs.Length; ++j)
              yield return initializeSystem.m_PrefabSystem.GetPrefab<InfoviewPrefab>(prefabs[j]);
            prefabs = new NativeArray<PrefabData>();
          }
        }
      }
    }

    public IEnumerable<InfomodePrefab> infomodes
    {
      get
      {
        InfoviewInitializeSystem initializeSystem = this;
        initializeSystem.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref initializeSystem.CheckedStateRef);
        ComponentTypeHandle<PrefabData> prefabType = initializeSystem.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        using (NativeArray<ArchetypeChunk> chunks = initializeSystem.m_AllInfomodeQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          for (int i = 0; i < chunks.Length; ++i)
          {
            NativeArray<PrefabData> prefabs = chunks[i].GetNativeArray<PrefabData>(ref prefabType);
            for (int j = 0; j < prefabs.Length; ++j)
              yield return initializeSystem.m_PrefabSystem.GetPrefab<InfomodePrefab>(prefabs[j]);
            prefabs = new NativeArray<PrefabData>();
          }
        }
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NewInfoviewQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Dependency = this.InitializeInfoviews(this.Dependency, this.m_NewInfoviewQuery, this.m_AllInfomodeQuery);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Dependency = this.FindInfoviews(this.Dependency, this.m_AllInfoviewQuery, this.m_AllInfomodeQuery, this.m_AllPlaceableQuery);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_NewPlaceableQuery.IsEmptyIgnoreFilter)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Dependency = this.FindInfoviews(this.Dependency, this.m_AllInfoviewQuery, this.m_AllInfomodeQuery, this.m_NewPlaceableQuery);
      }
    }

    private JobHandle InitializeInfoviews(
      JobHandle inputDeps,
      EntityQuery infoviewGroup,
      EntityQuery infomodeGroup)
    {
      NativeArray<ArchetypeChunk> archetypeChunkArray1 = infoviewGroup.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<ArchetypeChunk> archetypeChunkArray2 = infomodeGroup.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelMultiHashMap<Entity, InfoviewInitializeSystem.InfoModeData> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, InfoviewInitializeSystem.InfoModeData>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabData> componentTypeHandle = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewMode_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<InfoviewMode> bufferTypeHandle = this.__TypeHandle.__Game_Prefabs_InfoviewMode_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfomodeGroup_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<InfomodeGroup> roComponentLookup = this.__TypeHandle.__Game_Prefabs_InfomodeGroup_RO_ComponentLookup;
        inputDeps.Complete();
        for (int index1 = 0; index1 < archetypeChunkArray2.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray2[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity1 = nativeArray1[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            InfomodeBasePrefab prefab = this.m_PrefabSystem.GetPrefab<InfomodeBasePrefab>(nativeArray2[index2]);
            if (prefab.m_IncludeInGroups != null)
            {
              for (int index3 = 0; index3 < prefab.m_IncludeInGroups.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                Entity entity2 = this.m_PrefabSystem.GetEntity((PrefabBase) prefab.m_IncludeInGroups[index3]);
                // ISSUE: object of a compiler-generated type is created
                parallelMultiHashMap.Add(entity2, new InfoviewInitializeSystem.InfoModeData()
                {
                  m_Mode = entity1,
                  m_Priority = prefab.m_Priority
                });
              }
            }
          }
        }
        for (int index4 = 0; index4 < archetypeChunkArray1.Length; ++index4)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray1[index4];
          NativeArray<PrefabData> nativeArray = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle);
          BufferAccessor<InfoviewMode> bufferAccessor = archetypeChunk.GetBufferAccessor<InfoviewMode>(ref bufferTypeHandle);
          for (int index5 = 0; index5 < bufferAccessor.Length; ++index5)
          {
            PrefabData prefabData = nativeArray[index5];
            DynamicBuffer<InfoviewMode> dynamicBuffer = bufferAccessor[index5];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            InfoviewPrefab prefab = this.m_PrefabSystem.GetPrefab<InfoviewPrefab>(prefabData);
            if (prefab.m_Infomodes != null)
            {
              for (int index6 = 0; index6 < prefab.m_Infomodes.Length; ++index6)
              {
                InfomodeInfo infomode = prefab.m_Infomodes[index6];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) infomode.m_Mode);
                if (roComponentLookup.HasComponent(entity))
                {
                  // ISSUE: variable of a compiler-generated type
                  InfoviewInitializeSystem.InfoModeData infoModeData;
                  NativeParallelMultiHashMapIterator<Entity> it;
                  if (parallelMultiHashMap.TryGetFirstValue(entity, out infoModeData, out it))
                  {
                    do
                    {
                      // ISSUE: reference to a compiler-generated field
                      int priority = infomode.m_Priority * 1000000 + infomode.m_Mode.m_Priority * 1000 + infoModeData.m_Priority;
                      // ISSUE: reference to a compiler-generated field
                      dynamicBuffer.Add(new InfoviewMode(infoModeData.m_Mode, priority, infomode.m_Supplemental, infomode.m_Optional));
                    }
                    while (parallelMultiHashMap.TryGetNextValue(out infoModeData, ref it));
                  }
                }
                else
                {
                  int priority = infomode.m_Priority * 1000000 + infomode.m_Mode.m_Priority;
                  dynamicBuffer.Add(new InfoviewMode(entity, priority, infomode.m_Supplemental, infomode.m_Optional));
                }
              }
            }
          }
        }
      }
      finally
      {
        archetypeChunkArray1.Dispose();
        archetypeChunkArray2.Dispose();
        parallelMultiHashMap.Dispose();
      }
      return new JobHandle();
    }

    private JobHandle FindInfoviews(
      JobHandle inputDeps,
      EntityQuery infoviewQuery,
      EntityQuery infomodeQuery,
      EntityQuery objectQuery)
    {
      NativeQueue<InfoviewInitializeSystem.InfoviewBufferData> nativeQueue = new NativeQueue<InfoviewInitializeSystem.InfoviewBufferData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle outJobHandle1;
      NativeList<ArchetypeChunk> archetypeChunkListAsync1 = infoviewQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      NativeList<ArchetypeChunk> archetypeChunkListAsync2 = infomodeQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableInfoviewItem_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewRouteData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewAvailabilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewMode_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPoweredData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WindPoweredData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TerraformingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PipelineData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PowerLineData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResearchFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WelfareOfficeData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AdminBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrisonData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FirewatchTowerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DisasterFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EmergencyShelterData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TelecomFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PostFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PoliceStationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FireStationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportStationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterTowerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BatteryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransformerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PowerPlantData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      InfoviewInitializeSystem.FindInfoviewJob jobData1 = new InfoviewInitializeSystem.FindInfoviewJob()
      {
        m_InfoviewChunks = archetypeChunkListAsync1,
        m_InfomodeChunks = archetypeChunkListAsync2,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CoverageType = this.__TypeHandle.__Game_Prefabs_CoverageData_RO_ComponentTypeHandle,
        m_HospitalType = this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentTypeHandle,
        m_PowerPlantType = this.__TypeHandle.__Game_Prefabs_PowerPlantData_RO_ComponentTypeHandle,
        m_TransformerType = this.__TypeHandle.__Game_Prefabs_TransformerData_RO_ComponentTypeHandle,
        m_BatteryType = this.__TypeHandle.__Game_Prefabs_BatteryData_RO_ComponentTypeHandle,
        m_WaterPumpingStationType = this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentTypeHandle,
        m_WaterTowerType = this.__TypeHandle.__Game_Prefabs_WaterTowerData_RO_ComponentTypeHandle,
        m_SewageOutletType = this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle,
        m_TransportDepotType = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentTypeHandle,
        m_TransportStationType = this.__TypeHandle.__Game_Prefabs_TransportStationData_RO_ComponentTypeHandle,
        m_GarbageFacilityType = this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentTypeHandle,
        m_FireStationType = this.__TypeHandle.__Game_Prefabs_FireStationData_RO_ComponentTypeHandle,
        m_PoliceStationType = this.__TypeHandle.__Game_Prefabs_PoliceStationData_RO_ComponentTypeHandle,
        m_MaintenanceDepotType = this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentTypeHandle,
        m_PostFacilityDataType = this.__TypeHandle.__Game_Prefabs_PostFacilityData_RO_ComponentTypeHandle,
        m_TelecomFacilityDataType = this.__TypeHandle.__Game_Prefabs_TelecomFacilityData_RO_ComponentTypeHandle,
        m_SchoolDataType = this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentTypeHandle,
        m_ParkDataType = this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentTypeHandle,
        m_EmergencyShelterDataType = this.__TypeHandle.__Game_Prefabs_EmergencyShelterData_RO_ComponentTypeHandle,
        m_DisasterFacilityDataType = this.__TypeHandle.__Game_Prefabs_DisasterFacilityData_RO_ComponentTypeHandle,
        m_FirewatchTowerDataType = this.__TypeHandle.__Game_Prefabs_FirewatchTowerData_RO_ComponentTypeHandle,
        m_DeathcareFacilityDataType = this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentTypeHandle,
        m_PrisonDataType = this.__TypeHandle.__Game_Prefabs_PrisonData_RO_ComponentTypeHandle,
        m_AdminBuildingDataType = this.__TypeHandle.__Game_Prefabs_AdminBuildingData_RO_ComponentTypeHandle,
        m_WelfareOfficeDataType = this.__TypeHandle.__Game_Prefabs_WelfareOfficeData_RO_ComponentTypeHandle,
        m_ResearchFacilityDataType = this.__TypeHandle.__Game_Prefabs_ResearchFacilityData_RO_ComponentTypeHandle,
        m_ParkingFacilityDataType = this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentTypeHandle,
        m_PowerLineType = this.__TypeHandle.__Game_Prefabs_PowerLineData_RO_ComponentTypeHandle,
        m_PipelineType = this.__TypeHandle.__Game_Prefabs_PipelineData_RO_ComponentTypeHandle,
        m_ElectricityConnectionType = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentTypeHandle,
        m_WaterPipeConnectionType = this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentTypeHandle,
        m_ZoneType = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle,
        m_TransportStopType = this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentTypeHandle,
        m_RouteType = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentTypeHandle,
        m_TransportLineType = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentTypeHandle,
        m_ExtractorAreaType = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentTypeHandle,
        m_TerraformingType = this.__TypeHandle.__Game_Prefabs_TerraformingData_RO_ComponentTypeHandle,
        m_WindPoweredType = this.__TypeHandle.__Game_Prefabs_WindPoweredData_RO_ComponentTypeHandle,
        m_WaterPoweredType = this.__TypeHandle.__Game_Prefabs_WaterPoweredData_RO_ComponentTypeHandle,
        m_GroundWaterPoweredType = this.__TypeHandle.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentTypeHandle,
        m_PollutionType = this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentTypeHandle,
        m_SpawnableBuildingType = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle,
        m_InfoviewModeType = this.__TypeHandle.__Game_Prefabs_InfoviewMode_RO_BufferTypeHandle,
        m_InfoviewCoverageType = this.__TypeHandle.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle,
        m_InfoviewAvailabilityType = this.__TypeHandle.__Game_Prefabs_InfoviewAvailabilityData_RO_ComponentTypeHandle,
        m_InfoviewBuildingType = this.__TypeHandle.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle,
        m_InfoviewVehicleType = this.__TypeHandle.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle,
        m_InfoviewTransportStopType = this.__TypeHandle.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle,
        m_InfoviewRouteType = this.__TypeHandle.__Game_Prefabs_InfoviewRouteData_RO_ComponentTypeHandle,
        m_InfoviewHeatmapType = this.__TypeHandle.__Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle,
        m_InfoviewObjectStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle,
        m_InfoviewNetStatusType = this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle,
        m_PlaceableInfoviewType = this.__TypeHandle.__Game_Prefabs_PlaceableInfoviewItem_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewMode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableInfoviewItem_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableInfoviewItem_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      InfoviewInitializeSystem.FindSubInfoviewJob jobData2 = new InfoviewInitializeSystem.FindSubInfoviewJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SpawnableBuildingType = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle,
        m_PlaceableInfoviewType = this.__TypeHandle.__Game_Prefabs_PlaceableInfoviewItem_RO_BufferTypeHandle,
        m_SubAreaType = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferTypeHandle,
        m_LotData = this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentLookup,
        m_PlaceableInfoviewData = this.__TypeHandle.__Game_Prefabs_PlaceableInfoviewItem_RO_BufferLookup,
        m_InfoviewModes = this.__TypeHandle.__Game_Prefabs_InfoviewMode_RO_BufferLookup,
        m_InfoViewBuffer = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableInfoviewItem_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      InfoviewInitializeSystem.AssignInfoviewJob jobData3 = new InfoviewInitializeSystem.AssignInfoviewJob()
      {
        m_InfoViewBuffer = nativeQueue,
        m_PlaceableInfoviewData = this.__TypeHandle.__Game_Prefabs_PlaceableInfoviewItem_RW_BufferLookup
      };
      JobHandle jobHandle = jobData1.ScheduleParallel<InfoviewInitializeSystem.FindInfoviewJob>(objectQuery, JobHandle.CombineDependencies(inputDeps, outJobHandle1, outJobHandle2));
      JobHandle dependsOn = jobData2.ScheduleParallel<InfoviewInitializeSystem.FindSubInfoviewJob>(objectQuery, jobHandle);
      JobHandle inputDeps1 = jobData3.Schedule<InfoviewInitializeSystem.AssignInfoviewJob>(dependsOn);
      nativeQueue.Dispose(inputDeps1);
      archetypeChunkListAsync1.Dispose(jobHandle);
      archetypeChunkListAsync2.Dispose(jobHandle);
      return inputDeps1;
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
    public InfoviewInitializeSystem()
    {
    }

    private struct InfoModeData
    {
      public Entity m_Mode;
      public int m_Priority;
    }

    [BurstCompile]
    private struct FindInfoviewJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_InfoviewChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_InfomodeChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CoverageData> m_CoverageType;
      [ReadOnly]
      public ComponentTypeHandle<HospitalData> m_HospitalType;
      [ReadOnly]
      public ComponentTypeHandle<PowerPlantData> m_PowerPlantType;
      [ReadOnly]
      public ComponentTypeHandle<TransformerData> m_TransformerType;
      [ReadOnly]
      public ComponentTypeHandle<BatteryData> m_BatteryType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPumpingStationData> m_WaterPumpingStationType;
      [ReadOnly]
      public ComponentTypeHandle<WaterTowerData> m_WaterTowerType;
      [ReadOnly]
      public ComponentTypeHandle<SewageOutletData> m_SewageOutletType;
      [ReadOnly]
      public ComponentTypeHandle<TransportDepotData> m_TransportDepotType;
      [ReadOnly]
      public ComponentTypeHandle<TransportStationData> m_TransportStationType;
      [ReadOnly]
      public ComponentTypeHandle<GarbageFacilityData> m_GarbageFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<FireStationData> m_FireStationType;
      [ReadOnly]
      public ComponentTypeHandle<PoliceStationData> m_PoliceStationType;
      [ReadOnly]
      public ComponentTypeHandle<MaintenanceDepotData> m_MaintenanceDepotType;
      [ReadOnly]
      public ComponentTypeHandle<PostFacilityData> m_PostFacilityDataType;
      [ReadOnly]
      public ComponentTypeHandle<TelecomFacilityData> m_TelecomFacilityDataType;
      [ReadOnly]
      public ComponentTypeHandle<SchoolData> m_SchoolDataType;
      [ReadOnly]
      public ComponentTypeHandle<ParkData> m_ParkDataType;
      [ReadOnly]
      public ComponentTypeHandle<EmergencyShelterData> m_EmergencyShelterDataType;
      [ReadOnly]
      public ComponentTypeHandle<DisasterFacilityData> m_DisasterFacilityDataType;
      [ReadOnly]
      public ComponentTypeHandle<FirewatchTowerData> m_FirewatchTowerDataType;
      [ReadOnly]
      public ComponentTypeHandle<DeathcareFacilityData> m_DeathcareFacilityDataType;
      [ReadOnly]
      public ComponentTypeHandle<PrisonData> m_PrisonDataType;
      [ReadOnly]
      public ComponentTypeHandle<AdminBuildingData> m_AdminBuildingDataType;
      [ReadOnly]
      public ComponentTypeHandle<WelfareOfficeData> m_WelfareOfficeDataType;
      [ReadOnly]
      public ComponentTypeHandle<ResearchFacilityData> m_ResearchFacilityDataType;
      [ReadOnly]
      public ComponentTypeHandle<ParkingFacilityData> m_ParkingFacilityDataType;
      [ReadOnly]
      public ComponentTypeHandle<PowerLineData> m_PowerLineType;
      [ReadOnly]
      public ComponentTypeHandle<PipelineData> m_PipelineType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConnectionData> m_ElectricityConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeConnectionData> m_WaterPipeConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<ZoneData> m_ZoneType;
      [ReadOnly]
      public ComponentTypeHandle<TransportStopData> m_TransportStopType;
      [ReadOnly]
      public ComponentTypeHandle<RouteData> m_RouteType;
      [ReadOnly]
      public ComponentTypeHandle<TransportLineData> m_TransportLineType;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorAreaData> m_ExtractorAreaType;
      [ReadOnly]
      public ComponentTypeHandle<TerraformingData> m_TerraformingType;
      [ReadOnly]
      public ComponentTypeHandle<WindPoweredData> m_WindPoweredType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPoweredData> m_WaterPoweredType;
      [ReadOnly]
      public ComponentTypeHandle<GroundWaterPoweredData> m_GroundWaterPoweredType;
      [ReadOnly]
      public ComponentTypeHandle<PollutionData> m_PollutionType;
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> m_SpawnableBuildingType;
      [ReadOnly]
      public BufferTypeHandle<InfoviewMode> m_InfoviewModeType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewCoverageData> m_InfoviewCoverageType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewAvailabilityData> m_InfoviewAvailabilityType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingData> m_InfoviewBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewVehicleData> m_InfoviewVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewTransportStopData> m_InfoviewTransportStopType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewRouteData> m_InfoviewRouteType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewHeatmapData> m_InfoviewHeatmapType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewObjectStatusData> m_InfoviewObjectStatusType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> m_InfoviewNetStatusType;
      public BufferTypeHandle<PlaceableInfoviewItem> m_PlaceableInfoviewType;
      private const int TYPE_PRIORITY = 1000000;
      private const int PRIMARY_REQUIREMENT_PRIORITY = 10000;
      private const int SECONDARY_REQUIREMENT_PRIORITY = 10000;
      private const int PRIMARY_EFFECT_PRIORITY = 10000;
      private const int SECONDARY_EFFECT_PRIORITY = 100;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        InfoviewInitializeSystem.FindInfoviewJob.InfoviewSearchData searchData = new InfoviewInitializeSystem.FindInfoviewJob.InfoviewSearchData();
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<SpawnableBuildingData>(ref this.m_SpawnableBuildingType))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<HospitalData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.Hospital, chunk, this.m_HospitalType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<PowerPlantData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.PowerPlant, chunk, this.m_PowerPlantType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<TransformerData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.Transformer, chunk, this.m_TransformerType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<BatteryData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.Battery, chunk, this.m_BatteryType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<WaterPumpingStationData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.FreshWaterBuilding, chunk, this.m_WaterPumpingStationType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<WaterTowerData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.FreshWaterBuilding, chunk, this.m_WaterTowerType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<SewageOutletData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.SewageBuilding, chunk, this.m_SewageOutletType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<TransportDepotData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.TransportDepot, chunk, this.m_TransportDepotType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<TransportStationData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.TransportStation, chunk, this.m_TransportStationType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<GarbageFacilityData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.GarbageFacility, chunk, this.m_GarbageFacilityType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<FireStationData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.FireStation, chunk, this.m_FireStationType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<PoliceStationData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.PoliceStation, chunk, this.m_PoliceStationType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<PostFacilityData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.PostFacility, chunk, this.m_PostFacilityDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<TelecomFacilityData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.TelecomFacility, chunk, this.m_TelecomFacilityDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<SchoolData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.School, chunk, this.m_SchoolDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<ParkData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.Park, chunk, this.m_ParkDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<EmergencyShelterData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.EmergencyShelter, chunk, this.m_EmergencyShelterDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<DisasterFacilityData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.DisasterFacility, chunk, this.m_DisasterFacilityDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<FirewatchTowerData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.FirewatchTower, chunk, this.m_FirewatchTowerDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<DeathcareFacilityData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.DeathcareFacility, chunk, this.m_DeathcareFacilityDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<PrisonData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.Prison, chunk, this.m_PrisonDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<AdminBuildingData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.AdminBuilding, chunk, this.m_AdminBuildingDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<WelfareOfficeData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.WelfareOffice, chunk, this.m_WelfareOfficeDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<ResearchFacilityData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.ResearchFacility, chunk, this.m_ResearchFacilityDataType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckBuildingType<ParkingFacilityData>(ref searchData.m_BuildingTypes, ref searchData.m_BuildingPriority, BuildingType.ParkingFacility, chunk, this.m_ParkingFacilityDataType);
          // ISSUE: reference to a compiler-generated field
          if (((long) searchData.m_BuildingTypes & 67108870L) != 0L)
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_NetStatusPriority = 100;
            // ISSUE: reference to a compiler-generated field
            searchData.m_NetStatusTypes = 96U;
          }
          // ISSUE: reference to a compiler-generated field
          if (((long) searchData.m_BuildingTypes & 24L) != 0L)
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_WaterPriority = 10000;
            flag1 = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (((long) searchData.m_BuildingTypes & 8L) != 0L)
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_NetStatusPriority = 100;
            // ISSUE: reference to a compiler-generated field
            searchData.m_NetStatusTypes = 128U;
          }
          // ISSUE: reference to a compiler-generated field
          if (((long) searchData.m_BuildingTypes & 16L) != 0L)
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_NetStatusPriority = 100;
            // ISSUE: reference to a compiler-generated field
            searchData.m_NetStatusTypes = 256U;
          }
          // ISSUE: reference to a compiler-generated field
          if (((long) searchData.m_BuildingTypes & 128L) != 0L)
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_RouteType = RouteType.TransportLine;
            // ISSUE: reference to a compiler-generated field
            searchData.m_RoutePriority = 10000;
          }
          // ISSUE: reference to a compiler-generated field
          if (((long) searchData.m_BuildingTypes & 256L) != 0L)
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_VehicleTypes |= 256U;
            // ISSUE: reference to a compiler-generated field
            searchData.m_VehiclePriority = 10000;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<TransportStopData>(ref this.m_TransportStopType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_TransportStopPriority = 1000000;
            // ISSUE: reference to a compiler-generated field
            searchData.m_RoutePriority = 10000;
            flag1 = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<RouteData>(ref this.m_RouteType))
            {
              // ISSUE: reference to a compiler-generated field
              searchData.m_RoutePriority = 1000000;
              // ISSUE: reference to a compiler-generated field
              searchData.m_TransportStopPriority = 10000;
              flag1 = true;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<MaintenanceDepotData>(ref this.m_MaintenanceDepotType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_MaintenanceDepotPriority = 1000000;
            flag1 = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<TerraformingData>(ref this.m_TerraformingType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_TerraformingPriority = 1000000;
            flag1 = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<CoverageData>(ref this.m_CoverageType))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            searchData.m_CoverageService = chunk.GetNativeArray<CoverageData>(ref this.m_CoverageType)[0].m_Service;
            // ISSUE: reference to a compiler-generated field
            searchData.m_CoveragePriority = 10000;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<WindPoweredData>(ref this.m_WindPoweredType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_WindPriority = 10000;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<WaterPoweredData>(ref this.m_WaterPoweredType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_WaterPriority = 10000;
            // ISSUE: reference to a compiler-generated field
            searchData.m_WaterTypes |= InfoviewInitializeSystem.WaterType.Flowing;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<GroundWaterPoweredData>(ref this.m_GroundWaterPoweredType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_WaterPriority = 10000;
            // ISSUE: reference to a compiler-generated field
            searchData.m_WaterTypes |= InfoviewInitializeSystem.WaterType.Ground;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<ExtractorAreaData>(ref this.m_ExtractorAreaType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_ExtractorAreaPriority = 10000;
            flag1 = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<PowerLineData>(ref this.m_PowerLineType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_NetStatusPriority = 1000000;
            flag1 = true;
            flag2 = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<PipelineData>(ref this.m_PipelineType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_NetStatusPriority = 1000000;
            flag1 = true;
            flag3 = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<ZoneData>(ref this.m_ZoneType))
          {
            // ISSUE: reference to a compiler-generated field
            searchData.m_ZonePriority = 1000000;
            // ISSUE: reference to a compiler-generated field
            searchData.m_PollutionPriority = 10000;
            flag1 = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<PollutionData>(ref this.m_PollutionType))
            {
              // ISSUE: reference to a compiler-generated field
              searchData.m_PollutionPriority = 100;
              flag1 = true;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PlaceableInfoviewItem> bufferAccessor = chunk.GetBufferAccessor<PlaceableInfoviewItem>(ref this.m_PlaceableInfoviewType);
        NativeParallelHashMap<Entity, int> infomodeScores = new NativeParallelHashMap<Entity, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<InfoviewInitializeSystem.InfoModeData> supplementalModes = new NativeList<InfoviewInitializeSystem.InfoModeData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<ZoneData> nativeArray1 = chunk.GetNativeArray<ZoneData>(ref this.m_ZoneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TransportStopData> nativeArray2 = chunk.GetNativeArray<TransportStopData>(ref this.m_TransportStopType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<RouteData> nativeArray3 = chunk.GetNativeArray<RouteData>(ref this.m_RouteType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TransportLineData> nativeArray4 = chunk.GetNativeArray<TransportLineData>(ref this.m_TransportLineType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ExtractorAreaData> nativeArray5 = chunk.GetNativeArray<ExtractorAreaData>(ref this.m_ExtractorAreaType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<MaintenanceDepotData> nativeArray6 = chunk.GetNativeArray<MaintenanceDepotData>(ref this.m_MaintenanceDepotType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TerraformingData> nativeArray7 = chunk.GetNativeArray<TerraformingData>(ref this.m_TerraformingType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PollutionData> nativeArray8 = chunk.GetNativeArray<PollutionData>(ref this.m_PollutionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<WaterPumpingStationData> nativeArray9 = chunk.GetNativeArray<WaterPumpingStationData>(ref this.m_WaterPumpingStationType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<SewageOutletData> nativeArray10 = chunk.GetNativeArray<SewageOutletData>(ref this.m_SewageOutletType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ElectricityConnectionData> nativeArray11 = chunk.GetNativeArray<ElectricityConnectionData>(ref this.m_ElectricityConnectionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<WaterPipeConnectionData> nativeArray12 = chunk.GetNativeArray<WaterPipeConnectionData>(ref this.m_WaterPipeConnectionType);
          // ISSUE: reference to a compiler-generated field
          bool flag4 = chunk.Has<WaterPoweredData>(ref this.m_WaterPoweredType);
          for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_ZonePriority != 0 && nativeArray1.Length != 0)
            {
              ZoneData zoneData = nativeArray1[index1];
              // ISSUE: reference to a compiler-generated field
              searchData.m_AreaType = zoneData.m_AreaType;
              // ISSUE: reference to a compiler-generated field
              searchData.m_IsOffice = (zoneData.m_ZoneFlags & ZoneFlags.Office) != 0;
            }
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_TransportStopPriority != 0)
            {
              if (nativeArray2.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                searchData.m_TransportType = nativeArray2[index1].m_TransportType;
              }
              else if (nativeArray4.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                searchData.m_TransportType = nativeArray4[index1].m_TransportType;
              }
              else if (nativeArray3.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                searchData.m_TransportType = TransportType.None;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_RoutePriority != 0)
            {
              if (nativeArray2.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                searchData.m_RouteType = RouteType.TransportLine;
              }
              else if (nativeArray3.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                searchData.m_RouteType = nativeArray3[index1].m_Type;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_ExtractorAreaPriority != 0 && nativeArray5.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              searchData.m_MapFeature = nativeArray5[index1].m_MapFeature;
            }
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_MaintenanceDepotPriority != 0 && nativeArray6.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              searchData.m_MaintenanceType = nativeArray6[index1].m_MaintenanceType;
            }
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_TerraformingPriority != 0 && nativeArray7.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              searchData.m_TerraformingTarget = nativeArray7[index1].m_Target;
            }
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_PollutionPriority != 0)
            {
              if (nativeArray1.Length != 0)
              {
                ZoneData zoneData = nativeArray1[index1];
                if (zoneData.m_AreaType == Game.Zones.AreaType.Residential)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_PollutionTypes |= InfoviewInitializeSystem.PollutionType.Ground;
                }
                if (zoneData.m_AreaType == Game.Zones.AreaType.Industrial)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_PollutionTypes |= InfoviewInitializeSystem.PollutionType.Ground | InfoviewInitializeSystem.PollutionType.Air;
                }
              }
              else if (nativeArray8.Length != 0)
              {
                PollutionData pollutionData = nativeArray8[index1];
                // ISSUE: reference to a compiler-generated field
                searchData.m_PollutionTypes = InfoviewInitializeSystem.PollutionType.None;
                if ((double) pollutionData.m_GroundPollution > 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_PollutionTypes |= InfoviewInitializeSystem.PollutionType.Ground;
                }
                if ((double) pollutionData.m_AirPollution > 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_PollutionTypes |= InfoviewInitializeSystem.PollutionType.Air;
                }
                if ((double) pollutionData.m_NoisePollution > 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_PollutionTypes |= InfoviewInitializeSystem.PollutionType.Noise;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_WaterPriority != 0 && ((nativeArray9.Length != 0 ? 1 : (nativeArray10.Length != 0 ? 1 : 0)) | (flag4 ? 1 : 0)) != 0)
            {
              // ISSUE: reference to a compiler-generated field
              searchData.m_WaterTypes = InfoviewInitializeSystem.WaterType.None;
              if (nativeArray9.Length != 0)
              {
                WaterPumpingStationData pumpingStationData = nativeArray9[index1];
                if ((pumpingStationData.m_Types & AllowedWaterTypes.Groundwater) != AllowedWaterTypes.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_WaterTypes |= InfoviewInitializeSystem.WaterType.Ground;
                }
                if ((pumpingStationData.m_Types & AllowedWaterTypes.SurfaceWater) != AllowedWaterTypes.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_WaterTypes |= InfoviewInitializeSystem.WaterType.Flowing;
                }
              }
              if (nativeArray10.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                searchData.m_WaterTypes |= InfoviewInitializeSystem.WaterType.Flowing;
              }
              if (flag4)
              {
                // ISSUE: reference to a compiler-generated field
                searchData.m_WaterTypes |= InfoviewInitializeSystem.WaterType.Flowing;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (searchData.m_NetStatusPriority != 0)
            {
              if (nativeArray11.Length != 0 & flag2)
              {
                switch (nativeArray11[index1].m_Voltage)
                {
                  case ElectricityConnection.Voltage.Low:
                    // ISSUE: reference to a compiler-generated field
                    searchData.m_NetStatusTypes = 32U;
                    break;
                  case ElectricityConnection.Voltage.High:
                    // ISSUE: reference to a compiler-generated field
                    searchData.m_NetStatusTypes = 64U;
                    break;
                }
              }
              if (nativeArray12.Length != 0 & flag3)
              {
                WaterPipeConnectionData pipeConnectionData = nativeArray12[index1];
                if (pipeConnectionData.m_FreshCapacity != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_NetStatusTypes = 128U;
                }
                if (pipeConnectionData.m_SewageCapacity != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  searchData.m_NetStatusTypes = 256U;
                }
              }
            }
            // ISSUE: reference to a compiler-generated method
            this.CalculateInfomodeScores(searchData, infomodeScores);
            supplementalModes.Clear();
            int bestScore;
            // ISSUE: reference to a compiler-generated method
            Entity bestInfoView = this.GetBestInfoView(searchData, infomodeScores, supplementalModes, out bestScore);
            DynamicBuffer<PlaceableInfoviewItem> dynamicBuffer = bufferAccessor[index1];
            dynamicBuffer.Clear();
            if (bestInfoView != Entity.Null)
            {
              dynamicBuffer.Capacity = 1 + supplementalModes.Length;
              ref DynamicBuffer<PlaceableInfoviewItem> local1 = ref dynamicBuffer;
              PlaceableInfoviewItem placeableInfoviewItem = new PlaceableInfoviewItem();
              placeableInfoviewItem.m_Item = bestInfoView;
              placeableInfoviewItem.m_Priority = bestScore;
              PlaceableInfoviewItem elem1 = placeableInfoviewItem;
              local1.Add(elem1);
              for (int index2 = 0; index2 < supplementalModes.Length; ++index2)
              {
                // ISSUE: variable of a compiler-generated type
                InfoviewInitializeSystem.InfoModeData infoModeData = supplementalModes[index2];
                ref DynamicBuffer<PlaceableInfoviewItem> local2 = ref dynamicBuffer;
                placeableInfoviewItem = new PlaceableInfoviewItem();
                // ISSUE: reference to a compiler-generated field
                placeableInfoviewItem.m_Item = infoModeData.m_Mode;
                // ISSUE: reference to a compiler-generated field
                placeableInfoviewItem.m_Priority = infoModeData.m_Priority;
                PlaceableInfoviewItem elem2 = placeableInfoviewItem;
                local2.Add(elem2);
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CalculateInfomodeScores(searchData, infomodeScores);
          supplementalModes.Clear();
          int bestScore;
          // ISSUE: reference to a compiler-generated method
          Entity bestInfoView = this.GetBestInfoView(searchData, infomodeScores, supplementalModes, out bestScore);
          for (int index3 = 0; index3 < bufferAccessor.Length; ++index3)
          {
            DynamicBuffer<PlaceableInfoviewItem> dynamicBuffer = bufferAccessor[index3];
            dynamicBuffer.Clear();
            if (bestInfoView != Entity.Null)
            {
              dynamicBuffer.Capacity = 1 + supplementalModes.Length;
              ref DynamicBuffer<PlaceableInfoviewItem> local3 = ref dynamicBuffer;
              PlaceableInfoviewItem placeableInfoviewItem = new PlaceableInfoviewItem();
              placeableInfoviewItem.m_Item = bestInfoView;
              placeableInfoviewItem.m_Priority = bestScore;
              PlaceableInfoviewItem elem3 = placeableInfoviewItem;
              local3.Add(elem3);
              for (int index4 = 0; index4 < supplementalModes.Length; ++index4)
              {
                // ISSUE: variable of a compiler-generated type
                InfoviewInitializeSystem.InfoModeData infoModeData = supplementalModes[index4];
                ref DynamicBuffer<PlaceableInfoviewItem> local4 = ref dynamicBuffer;
                placeableInfoviewItem = new PlaceableInfoviewItem();
                // ISSUE: reference to a compiler-generated field
                placeableInfoviewItem.m_Item = infoModeData.m_Mode;
                // ISSUE: reference to a compiler-generated field
                placeableInfoviewItem.m_Priority = infoModeData.m_Priority;
                PlaceableInfoviewItem elem4 = placeableInfoviewItem;
                local4.Add(elem4);
              }
            }
          }
        }
        infomodeScores.Dispose();
        supplementalModes.Dispose();
      }

      private void CheckBuildingType<T>(
        ref ulong mask,
        ref int priority,
        BuildingType type,
        ArchetypeChunk chunk,
        ComponentTypeHandle<T> componentType)
        where T : struct, IComponentData
      {
        bool c = chunk.Has<T>(ref componentType);
        mask = math.select(mask, mask | (ulong) (1L << (int) (type & (BuildingType.SignatureResidential | BuildingType.ExtractorBuilding))), c);
        priority = math.select(priority, 1000000, c);
      }

      private void CalculateInfomodeScores(
        InfoviewInitializeSystem.FindInfoviewJob.InfoviewSearchData searchData,
        NativeParallelHashMap<Entity, int> infomodeScores)
      {
        infomodeScores.Clear();
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infomodeChunk = this.m_InfomodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = infomodeChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_BuildingPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewBuildingData> nativeArray2 = infomodeChunk.GetNativeArray<InfoviewBuildingData>(ref this.m_InfoviewBuildingType);
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              InfoviewBuildingData infoviewBuildingData = nativeArray2[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int score = math.select(-1, searchData.m_BuildingPriority, (searchData.m_BuildingTypes & (ulong) (1L << (int) (infoviewBuildingData.m_Type & (BuildingType.SignatureResidential | BuildingType.ExtractorBuilding)))) > 0UL);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index2], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_MaintenanceDepotPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewBuildingData> nativeArray3 = infomodeChunk.GetNativeArray<InfoviewBuildingData>(ref this.m_InfoviewBuildingType);
            for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
            {
              InfoviewBuildingData infoviewBuildingData = nativeArray3[index3];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int score = math.select(-1, searchData.m_MaintenanceDepotPriority, (searchData.m_MaintenanceType & InfoviewInitializeSystem.FindInfoviewJob.GetMaintenanceType(infoviewBuildingData.m_Type)) != 0);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index3], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_ZonePriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewAvailabilityData> nativeArray4 = infomodeChunk.GetNativeArray<InfoviewAvailabilityData>(ref this.m_InfoviewAvailabilityType);
            for (int index4 = 0; index4 < nativeArray4.Length; ++index4)
            {
              InfoviewAvailabilityData availabilityData = nativeArray4[index4];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int score = math.select(-1, searchData.m_ZonePriority, searchData.m_AreaType == availabilityData.m_AreaType && searchData.m_IsOffice == availabilityData.m_Office);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index4], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_TransportStopPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewTransportStopData> nativeArray5 = infomodeChunk.GetNativeArray<InfoviewTransportStopData>(ref this.m_InfoviewTransportStopType);
            for (int index5 = 0; index5 < nativeArray5.Length; ++index5)
            {
              InfoviewTransportStopData transportStopData = nativeArray5[index5];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int score = math.select(-1, searchData.m_TransportStopPriority, searchData.m_TransportType == transportStopData.m_Type);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index5], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_RoutePriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewRouteData> nativeArray6 = infomodeChunk.GetNativeArray<InfoviewRouteData>(ref this.m_InfoviewRouteType);
            for (int index6 = 0; index6 < nativeArray6.Length; ++index6)
            {
              InfoviewRouteData infoviewRouteData = nativeArray6[index6];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int score = math.select(-1, searchData.m_RoutePriority, searchData.m_RouteType == infoviewRouteData.m_Type);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index6], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_TerraformingPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewHeatmapData> nativeArray7 = infomodeChunk.GetNativeArray<InfoviewHeatmapData>(ref this.m_InfoviewHeatmapType);
            for (int index7 = 0; index7 < nativeArray7.Length; ++index7)
            {
              InfoviewHeatmapData infoviewHeatmapData = nativeArray7[index7];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int score = math.select(-1, searchData.m_TerraformingPriority, searchData.m_TerraformingTarget == InfoviewInitializeSystem.FindInfoviewJob.GetTerraformingTarget(infoviewHeatmapData.m_Type));
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index7], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_VehiclePriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewVehicleData> nativeArray8 = infomodeChunk.GetNativeArray<InfoviewVehicleData>(ref this.m_InfoviewVehicleType);
            for (int index8 = 0; index8 < nativeArray8.Length; ++index8)
            {
              InfoviewVehicleData infoviewVehicleData = nativeArray8[index8];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int score = math.select(-1, searchData.m_VehiclePriority, (searchData.m_VehicleTypes & 1U << (int) (infoviewVehicleData.m_Type & (VehicleType) 31)) > 0U);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index8], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_CoveragePriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewCoverageData> nativeArray9 = infomodeChunk.GetNativeArray<InfoviewCoverageData>(ref this.m_InfoviewCoverageType);
            for (int index9 = 0; index9 < nativeArray9.Length; ++index9)
            {
              InfoviewCoverageData infoviewCoverageData = nativeArray9[index9];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int score = math.select(-1, searchData.m_CoveragePriority, searchData.m_CoverageService == infoviewCoverageData.m_Service);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index9], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_WaterPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewHeatmapData> nativeArray10 = infomodeChunk.GetNativeArray<InfoviewHeatmapData>(ref this.m_InfoviewHeatmapType);
            for (int index10 = 0; index10 < nativeArray10.Length; ++index10)
            {
              InfoviewHeatmapData infoviewHeatmapData = nativeArray10[index10];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int score = math.select(-1, searchData.m_WaterPriority, (searchData.m_WaterTypes & InfoviewInitializeSystem.FindInfoviewJob.GetWaterType(infoviewHeatmapData.m_Type)) != 0);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index10], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_WindPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewHeatmapData> nativeArray11 = infomodeChunk.GetNativeArray<InfoviewHeatmapData>(ref this.m_InfoviewHeatmapType);
            for (int index11 = 0; index11 < nativeArray11.Length; ++index11)
            {
              InfoviewHeatmapData infoviewHeatmapData = nativeArray11[index11];
              // ISSUE: reference to a compiler-generated field
              int score = math.select(-1, searchData.m_WindPriority, infoviewHeatmapData.m_Type == HeatmapData.Wind);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index11], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_ExtractorAreaPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewHeatmapData> nativeArray12 = infomodeChunk.GetNativeArray<InfoviewHeatmapData>(ref this.m_InfoviewHeatmapType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewObjectStatusData> nativeArray13 = infomodeChunk.GetNativeArray<InfoviewObjectStatusData>(ref this.m_InfoviewObjectStatusType);
            for (int index12 = 0; index12 < nativeArray12.Length; ++index12)
            {
              InfoviewHeatmapData infoviewHeatmapData = nativeArray12[index12];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int score = math.select(-1, searchData.m_ExtractorAreaPriority, searchData.m_MapFeature == InfoviewInitializeSystem.FindInfoviewJob.GetMapFeature(infoviewHeatmapData.m_Type));
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index12], score);
            }
            for (int index13 = 0; index13 < nativeArray13.Length; ++index13)
            {
              InfoviewObjectStatusData objectStatusData = nativeArray13[index13];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int score = math.select(-1, searchData.m_ExtractorAreaPriority, searchData.m_MapFeature == InfoviewInitializeSystem.FindInfoviewJob.GetMapFeature(objectStatusData.m_Type));
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index13], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_PollutionPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewHeatmapData> nativeArray14 = infomodeChunk.GetNativeArray<InfoviewHeatmapData>(ref this.m_InfoviewHeatmapType);
            for (int index14 = 0; index14 < nativeArray14.Length; ++index14)
            {
              InfoviewHeatmapData infoviewHeatmapData = nativeArray14[index14];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int score = math.select(-1, searchData.m_PollutionPriority, (searchData.m_PollutionTypes & InfoviewInitializeSystem.FindInfoviewJob.GetPollutionType(infoviewHeatmapData.m_Type)) != 0);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index14], score);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (searchData.m_NetStatusPriority != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<InfoviewNetStatusData> nativeArray15 = infomodeChunk.GetNativeArray<InfoviewNetStatusData>(ref this.m_InfoviewNetStatusType);
            for (int index15 = 0; index15 < nativeArray15.Length; ++index15)
            {
              InfoviewNetStatusData infoviewNetStatusData = nativeArray15[index15];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int score = math.select(-1, searchData.m_NetStatusPriority, (searchData.m_NetStatusTypes & 1U << (int) (infoviewNetStatusData.m_Type & (NetStatusType) 31)) > 0U);
              // ISSUE: reference to a compiler-generated method
              this.AddInfomodeScore(infomodeScores, nativeArray1[index15], score);
            }
          }
        }
      }

      private void AddInfomodeScore(
        NativeParallelHashMap<Entity, int> infomodeScores,
        Entity entity,
        int score)
      {
        if (infomodeScores.TryAdd(entity, score))
          return;
        infomodeScores[entity] = math.max(infomodeScores[entity], score);
      }

      private Entity GetBestInfoView(
        InfoviewInitializeSystem.FindInfoviewJob.InfoviewSearchData searchData,
        NativeParallelHashMap<Entity, int> infomodeScores,
        NativeList<InfoviewInitializeSystem.InfoModeData> supplementalModes,
        out int bestScore)
      {
        bestScore = int.MinValue;
        Entity bestInfoView = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfoviewChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk infoviewChunk = this.m_InfoviewChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray = infoviewChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<InfoviewMode> bufferAccessor = infoviewChunk.GetBufferAccessor<InfoviewMode>(ref this.m_InfoviewModeType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            DynamicBuffer<InfoviewMode> dynamicBuffer = bufferAccessor[index2];
            int num1 = 0;
            bool flag1 = false;
            for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
            {
              InfoviewMode infoviewMode = dynamicBuffer[index3];
              int a;
              if (infomodeScores.TryGetValue(infoviewMode.m_Mode, out a))
              {
                bool flag2 = infoviewMode.m_Supplemental | infoviewMode.m_Optional;
                num1 += math.select(a, 0, flag2 & a < 0);
                flag1 |= flag2 & a > 0;
              }
            }
            if (num1 > bestScore || bestScore == 0 && dynamicBuffer.Length == 0)
            {
              bestScore = num1;
              bestInfoView = nativeArray[index2];
              supplementalModes.Clear();
              if (flag1)
              {
                for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
                {
                  InfoviewMode infoviewMode = dynamicBuffer[index4];
                  int num2;
                  if (infoviewMode.m_Supplemental | infoviewMode.m_Optional && infomodeScores.TryGetValue(infoviewMode.m_Mode, out num2) && num2 > 0)
                  {
                    // ISSUE: object of a compiler-generated type is created
                    supplementalModes.Add(new InfoviewInitializeSystem.InfoModeData()
                    {
                      m_Mode = infoviewMode.m_Mode,
                      m_Priority = num2
                    });
                  }
                }
              }
            }
          }
        }
        return bestInfoView;
      }

      public static MapFeature GetMapFeature(HeatmapData heatmapType)
      {
        switch (heatmapType)
        {
          case HeatmapData.Fertility:
            return MapFeature.FertileLand;
          case HeatmapData.Ore:
            return MapFeature.Ore;
          case HeatmapData.Oil:
            return MapFeature.Oil;
          default:
            return MapFeature.None;
        }
      }

      public static TerraformingTarget GetTerraformingTarget(HeatmapData heatmapType)
      {
        switch (heatmapType)
        {
          case HeatmapData.GroundWater:
            return TerraformingTarget.GroundWater;
          case HeatmapData.Fertility:
            return TerraformingTarget.FertileLand;
          case HeatmapData.Ore:
            return TerraformingTarget.Ore;
          case HeatmapData.Oil:
            return TerraformingTarget.Oil;
          default:
            return TerraformingTarget.None;
        }
      }

      public static MapFeature GetMapFeature(ObjectStatusType statusType)
      {
        return statusType == ObjectStatusType.WoodResource ? MapFeature.Forest : MapFeature.None;
      }

      public static MaintenanceType GetMaintenanceType(BuildingType buildingType)
      {
        if (buildingType == BuildingType.RoadMaintenanceDepot)
          return MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle;
        return buildingType == BuildingType.ParkMaintenanceDepot ? MaintenanceType.Park : MaintenanceType.None;
      }

      public static InfoviewInitializeSystem.PollutionType GetPollutionType(HeatmapData heatmapType)
      {
        switch (heatmapType)
        {
          case HeatmapData.GroundWater:
            return InfoviewInitializeSystem.PollutionType.Ground;
          case HeatmapData.GroundPollution:
            return InfoviewInitializeSystem.PollutionType.Ground;
          case HeatmapData.Wind:
            return InfoviewInitializeSystem.PollutionType.Air;
          default:
            return InfoviewInitializeSystem.PollutionType.None;
        }
      }

      public static InfoviewInitializeSystem.WaterType GetWaterType(HeatmapData heatmapType)
      {
        if (heatmapType <= HeatmapData.WaterFlow)
        {
          if (heatmapType == HeatmapData.GroundWater)
            return InfoviewInitializeSystem.WaterType.Ground;
          if (heatmapType == HeatmapData.WaterFlow)
            return InfoviewInitializeSystem.WaterType.Flowing;
        }
        else
        {
          if (heatmapType == HeatmapData.WaterPollution)
            return InfoviewInitializeSystem.WaterType.Flowing;
          if (heatmapType == HeatmapData.GroundWaterPollution)
            return InfoviewInitializeSystem.WaterType.Ground;
        }
        return InfoviewInitializeSystem.WaterType.None;
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

      private struct InfoviewSearchData
      {
        public CoverageService m_CoverageService;
        public Game.Zones.AreaType m_AreaType;
        public bool m_IsOffice;
        public TransportType m_TransportType;
        public RouteType m_RouteType;
        public MapFeature m_MapFeature;
        public MaintenanceType m_MaintenanceType;
        public TerraformingTarget m_TerraformingTarget;
        public InfoviewInitializeSystem.PollutionType m_PollutionTypes;
        public InfoviewInitializeSystem.WaterType m_WaterTypes;
        public ulong m_BuildingTypes;
        public uint m_VehicleTypes;
        public uint m_NetStatusTypes;
        public int m_BuildingPriority;
        public int m_VehiclePriority;
        public int m_ZonePriority;
        public int m_TransportStopPriority;
        public int m_RoutePriority;
        public int m_CoveragePriority;
        public int m_ExtractorAreaPriority;
        public int m_MaintenanceDepotPriority;
        public int m_TerraformingPriority;
        public int m_WindPriority;
        public int m_PollutionPriority;
        public int m_WaterPriority;
        public int m_FlowPriority;
        public int m_NetStatusPriority;
      }
    }

    [Flags]
    private enum PollutionType
    {
      None = 0,
      Ground = 1,
      Air = 2,
      Noise = 4,
    }

    [Flags]
    private enum WaterType
    {
      None = 0,
      Ground = 1,
      Flowing = 2,
    }

    private struct InfoviewBufferData
    {
      public Entity m_Target;
      public Entity m_Source;
    }

    [BurstCompile]
    private struct FindSubInfoviewJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> m_SpawnableBuildingType;
      [ReadOnly]
      public BufferTypeHandle<PlaceableInfoviewItem> m_PlaceableInfoviewType;
      [ReadOnly]
      public BufferTypeHandle<SubArea> m_SubAreaType;
      [ReadOnly]
      public ComponentLookup<LotData> m_LotData;
      [ReadOnly]
      public BufferLookup<PlaceableInfoviewItem> m_PlaceableInfoviewData;
      [ReadOnly]
      public BufferLookup<InfoviewMode> m_InfoviewModes;
      public NativeQueue<InfoviewInitializeSystem.InfoviewBufferData>.ParallelWriter m_InfoViewBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<SpawnableBuildingData> nativeArray2 = chunk.GetNativeArray<SpawnableBuildingData>(ref this.m_SpawnableBuildingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PlaceableInfoviewItem> bufferAccessor1 = chunk.GetBufferAccessor<PlaceableInfoviewItem>(ref this.m_PlaceableInfoviewType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubArea> bufferAccessor2 = chunk.GetBufferAccessor<SubArea>(ref this.m_SubAreaType);
        for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
        {
          DynamicBuffer<PlaceableInfoviewItem> dynamicBuffer1 = bufferAccessor1[index1];
          int num = int.MinValue;
          Entity entity = Entity.Null;
          if (dynamicBuffer1.Length != 0)
          {
            PlaceableInfoviewItem placeableInfoviewItem = dynamicBuffer1[0];
            // ISSUE: reference to a compiler-generated field
            if (this.m_InfoviewModes[placeableInfoviewItem.m_Item].Length != 0)
              num = placeableInfoviewItem.m_Priority;
          }
          if (nativeArray2.Length != 0)
          {
            SpawnableBuildingData spawnableBuildingData = nativeArray2[index1];
            DynamicBuffer<PlaceableInfoviewItem> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PlaceableInfoviewData.TryGetBuffer(spawnableBuildingData.m_ZonePrefab, out bufferData) && bufferData.Length != 0)
            {
              PlaceableInfoviewItem placeableInfoviewItem = bufferData[0];
              // ISSUE: reference to a compiler-generated field
              if (this.m_InfoviewModes[placeableInfoviewItem.m_Item].Length != 0 && placeableInfoviewItem.m_Priority > num)
              {
                num = placeableInfoviewItem.m_Priority;
                entity = spawnableBuildingData.m_ZonePrefab;
              }
            }
          }
          if (bufferAccessor2.Length != 0)
          {
            DynamicBuffer<SubArea> dynamicBuffer2 = bufferAccessor2[index1];
            for (int index2 = 0; index2 < dynamicBuffer2.Length; ++index2)
            {
              Entity prefab = dynamicBuffer2[index2].m_Prefab;
              DynamicBuffer<PlaceableInfoviewItem> bufferData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_LotData.HasComponent(prefab) && this.m_PlaceableInfoviewData.TryGetBuffer(prefab, out bufferData) && bufferData.Length != 0)
              {
                PlaceableInfoviewItem placeableInfoviewItem = bufferData[0];
                // ISSUE: reference to a compiler-generated field
                if (this.m_InfoviewModes[placeableInfoviewItem.m_Item].Length != 0 && placeableInfoviewItem.m_Priority > num)
                {
                  num = placeableInfoviewItem.m_Priority;
                  entity = prefab;
                }
              }
            }
          }
          if (entity != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_InfoViewBuffer.Enqueue(new InfoviewInitializeSystem.InfoviewBufferData()
            {
              m_Target = nativeArray1[index1],
              m_Source = entity
            });
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
    private struct AssignInfoviewJob : IJob
    {
      public NativeQueue<InfoviewInitializeSystem.InfoviewBufferData> m_InfoViewBuffer;
      public BufferLookup<PlaceableInfoviewItem> m_PlaceableInfoviewData;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        InfoviewInitializeSystem.InfoviewBufferData infoviewBufferData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_InfoViewBuffer.TryDequeue(out infoviewBufferData))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_PlaceableInfoviewData[infoviewBufferData.m_Target].CopyFrom(this.m_PlaceableInfoviewData[infoviewBufferData.m_Source]);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public BufferTypeHandle<InfoviewMode> __Game_Prefabs_InfoviewMode_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<InfomodeGroup> __Game_Prefabs_InfomodeGroup_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<CoverageData> __Game_Prefabs_CoverageData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HospitalData> __Game_Prefabs_HospitalData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PowerPlantData> __Game_Prefabs_PowerPlantData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransformerData> __Game_Prefabs_TransformerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BatteryData> __Game_Prefabs_BatteryData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterPumpingStationData> __Game_Prefabs_WaterPumpingStationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterTowerData> __Game_Prefabs_WaterTowerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SewageOutletData> __Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransportDepotData> __Game_Prefabs_TransportDepotData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransportStationData> __Game_Prefabs_TransportStationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GarbageFacilityData> __Game_Prefabs_GarbageFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<FireStationData> __Game_Prefabs_FireStationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PoliceStationData> __Game_Prefabs_PoliceStationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MaintenanceDepotData> __Game_Prefabs_MaintenanceDepotData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PostFacilityData> __Game_Prefabs_PostFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TelecomFacilityData> __Game_Prefabs_TelecomFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SchoolData> __Game_Prefabs_SchoolData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkData> __Game_Prefabs_ParkData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EmergencyShelterData> __Game_Prefabs_EmergencyShelterData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<DisasterFacilityData> __Game_Prefabs_DisasterFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<FirewatchTowerData> __Game_Prefabs_FirewatchTowerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<DeathcareFacilityData> __Game_Prefabs_DeathcareFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrisonData> __Game_Prefabs_PrisonData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AdminBuildingData> __Game_Prefabs_AdminBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WelfareOfficeData> __Game_Prefabs_WelfareOfficeData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResearchFacilityData> __Game_Prefabs_ResearchFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkingFacilityData> __Game_Prefabs_ParkingFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PowerLineData> __Game_Prefabs_PowerLineData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PipelineData> __Game_Prefabs_PipelineData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConnectionData> __Game_Prefabs_ElectricityConnectionData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeConnectionData> __Game_Prefabs_WaterPipeConnectionData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransportStopData> __Game_Prefabs_TransportStopData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RouteData> __Game_Prefabs_RouteData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TerraformingData> __Game_Prefabs_TerraformingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WindPoweredData> __Game_Prefabs_WindPoweredData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterPoweredData> __Game_Prefabs_WaterPoweredData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GroundWaterPoweredData> __Game_Prefabs_GroundWaterPoweredData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PollutionData> __Game_Prefabs_PollutionData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InfoviewMode> __Game_Prefabs_InfoviewMode_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewCoverageData> __Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewAvailabilityData> __Game_Prefabs_InfoviewAvailabilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewBuildingData> __Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewVehicleData> __Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewTransportStopData> __Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewRouteData> __Game_Prefabs_InfoviewRouteData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewHeatmapData> __Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewObjectStatusData> __Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> __Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle;
      public BufferTypeHandle<PlaceableInfoviewItem> __Game_Prefabs_PlaceableInfoviewItem_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PlaceableInfoviewItem> __Game_Prefabs_PlaceableInfoviewItem_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubArea> __Game_Prefabs_SubArea_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<LotData> __Game_Prefabs_LotData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<PlaceableInfoviewItem> __Game_Prefabs_PlaceableInfoviewItem_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InfoviewMode> __Game_Prefabs_InfoviewMode_RO_BufferLookup;
      public BufferLookup<PlaceableInfoviewItem> __Game_Prefabs_PlaceableInfoviewItem_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewMode_RW_BufferTypeHandle = state.GetBufferTypeHandle<InfoviewMode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfomodeGroup_RO_ComponentLookup = state.GetComponentLookup<InfomodeGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CoverageData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HospitalData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HospitalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PowerPlantData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PowerPlantData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransformerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransformerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BatteryData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BatteryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPumpingStationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPumpingStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterTowerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterTowerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SewageOutletData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportDepotData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransportDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransportStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GarbageFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FireStationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<FireStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PoliceStationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PoliceStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MaintenanceDepotData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MaintenanceDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PostFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PostFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TelecomFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TelecomFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SchoolData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SchoolData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EmergencyShelterData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EmergencyShelterData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DisasterFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DisasterFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FirewatchTowerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<FirewatchTowerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeathcareFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DeathcareFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrisonData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrisonData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AdminBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AdminBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WelfareOfficeData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WelfareOfficeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResearchFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResearchFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkingFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PowerLineData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PowerLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PipelineData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PipelineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ElectricityConnectionData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStopData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransportStopData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TerraformingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TerraformingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WindPoweredData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WindPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPoweredData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GroundWaterPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PollutionData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PollutionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewMode_RO_BufferTypeHandle = state.GetBufferTypeHandle<InfoviewMode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewCoverageData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewAvailabilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewAvailabilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewVehicleData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewTransportStopData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewTransportStopData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewRouteData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewRouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewHeatmapData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewHeatmapData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewObjectStatusData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewObjectStatusData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewNetStatusData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableInfoviewItem_RW_BufferTypeHandle = state.GetBufferTypeHandle<PlaceableInfoviewItem>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableInfoviewItem_RO_BufferTypeHandle = state.GetBufferTypeHandle<PlaceableInfoviewItem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LotData_RO_ComponentLookup = state.GetComponentLookup<LotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableInfoviewItem_RO_BufferLookup = state.GetBufferLookup<PlaceableInfoviewItem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewMode_RO_BufferLookup = state.GetBufferLookup<InfoviewMode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableInfoviewItem_RW_BufferLookup = state.GetBufferLookup<PlaceableInfoviewItem>();
      }
    }
  }
}
