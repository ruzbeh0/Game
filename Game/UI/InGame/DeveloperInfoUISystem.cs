// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DeveloperInfoUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Rendering;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Creatures;
using Game.Economy;
using Game.Events;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.Simulation;
using Game.Vehicles;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class DeveloperInfoUISystem : UISystemBase
  {
    protected CitySystem m_CitySystem;
    protected NameSystem m_NameSystem;
    protected PrefabSystem m_PrefabSystem;
    protected ResourceSystem m_ResourceSystem;
    protected SimulationSystem m_SimulationSystem;
    protected SelectedInfoUISystem m_InfoUISystem;
    protected GroundPollutionSystem m_GroundPollutionSystem;
    protected AirPollutionSystem m_AirPollutionSystem;
    protected NoisePollutionSystem m_NoisePollutionSystem;
    protected TelecomCoverageSystem m_TelecomCoverageSystem;
    protected TaxSystem m_TaxSystem;
    protected BatchManagerSystem m_BatchManagerSystem;
    protected EntityQuery m_CitizenHappinessParameterQuery;
    protected EntityQuery m_HealthcareParameterQuery;
    protected EntityQuery m_ParkParameterQuery;
    protected EntityQuery m_EducationParameterQuery;
    protected EntityQuery m_TelecomParameterQuery;
    protected EntityQuery m_HappinessFactorParameterQuery;
    protected EntityQuery m_EconomyParameterQuery;
    protected EntityQuery m_ProcessQuery;
    protected EntityQuery m_TimeDataQuery;
    protected EntityQuery m_GarbageParameterQuery;
    private DeveloperInfoUISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_746694603_0;
    private EntityQuery __query_746694603_1;
    private EntityQuery __query_746694603_2;
    private EntityQuery __query_746694603_3;
    private EntityQuery __query_746694603_4;
    private EntityQuery __query_746694603_5;
    private EntityQuery __query_746694603_6;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenHappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ParkParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EducationParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EducationParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<TelecomParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessFactorParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<HappinessFactorParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<GarbageParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfoUISystem = this.World.GetOrCreateSystemManaged<SelectedInfoUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => entity != InfoList.Item.kNullEntity), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        info.label = "Entity";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(entity);
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        if (this.EntityManager.HasComponent<MeshGroup>(entity))
          return true;
        CurrentTransport component;
        return this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component) && this.EntityManager.HasComponent<MeshGroup>(component.m_CurrentTransport);
      }), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        CurrentTransport component1;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component1))
        {
          entity = component1.m_CurrentTransport;
          PrefabRef component2;
          if (this.EntityManager.TryGetComponent<PrefabRef>(entity, out component2))
            prefab = component2.m_Prefab;
        }
        DynamicBuffer<MeshGroup> buffer;
        ObjectGeometryPrefab prefab1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.EntityManager.TryGetBuffer<MeshGroup>(entity, true, out buffer) || !this.m_PrefabSystem.TryGetPrefab<ObjectGeometryPrefab>(prefab, out prefab1) || prefab1.m_Meshes == null)
          return;
        CreatureData component3;
        this.EntityManager.TryGetComponent<CreatureData>(prefab, out component3);
        info.label = string.Format("Mesh groups ({0})", (object) buffer.Length);
label_25:
        for (int index1 = 0; index1 < buffer.Length; ++index1)
        {
          int subMeshGroup = (int) buffer[index1].m_SubMeshGroup;
          for (int index2 = 0; index2 < prefab1.m_Meshes.Length; ++index2)
          {
            if (prefab1.m_Meshes[index2].m_Mesh is CharacterGroup mesh2 && mesh2.m_Characters != null)
            {
              for (int index3 = 0; index3 < mesh2.m_Characters.Length; ++index3)
              {
                if ((mesh2.m_Characters[index3].m_Style.m_Gender & component3.m_Gender) == component3.m_Gender && subMeshGroup-- == 0)
                {
                  info.Add(new InfoList.Item(string.Format("{0} #{1}", (object) mesh2.name, (object) index3)));
                  goto label_25;
                }
              }
              if (mesh2.m_Overrides != null)
              {
                for (int index4 = 0; index4 < mesh2.m_Overrides.Length; ++index4)
                {
                  CharacterGroup.OverrideInfo overrideInfo = mesh2.m_Overrides[index4];
                  for (int index5 = 0; index5 < mesh2.m_Characters.Length; ++index5)
                  {
                    if ((mesh2.m_Characters[index5].m_Style.m_Gender & component3.m_Gender) == component3.m_Gender && subMeshGroup-- == 0)
                    {
                      info.Add(new InfoList.Item(string.Format("{0} #{1} ({2})", (object) mesh2.name, (object) index5, (object) overrideInfo.m_Group.name)));
                      goto label_25;
                    }
                  }
                }
              }
            }
          }
          info.Add(new InfoList.Item(string.Format("Unknown group #{0}", (object) buffer[index1].m_SubMeshGroup)));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<MeshBatch>(entity) || this.EntityManager.HasComponent<CurrentTransport>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        CurrentTransport component;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component))
          entity = component.m_CurrentTransport;
        DynamicBuffer<MeshBatch> buffer;
        if (!this.EntityManager.TryGetBuffer<MeshBatch>(entity, true, out buffer))
          return;
        JobHandle dependencies1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeBatchGroups<CullingData, Game.Rendering.GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(true, out dependencies1);
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeBatchInstances<CullingData, Game.Rendering.GroupData, BatchData, InstanceData> nativeBatchInstances = this.m_BatchManagerSystem.GetNativeBatchInstances(true, out dependencies2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
        dependencies1.Complete();
        dependencies2.Complete();
        int num1 = 0;
        for (int index = 0; index < buffer.Length; ++index)
        {
          MeshBatch meshBatch = buffer[index];
          num1 += nativeBatchGroups.GetBatchCount(meshBatch.m_GroupIndex);
        }
        info.label = string.Format("Batches ({0})", (object) num1);
        for (int index = 0; index < buffer.Length; ++index)
        {
          MeshBatch meshBatch = buffer[index];
          Game.Rendering.GroupData groupData = nativeBatchGroups.GetGroupData(meshBatch.m_GroupIndex);
          int batchCount = nativeBatchGroups.GetBatchCount(meshBatch.m_GroupIndex);
          int instanceGroupIndex = nativeBatchInstances.GetMergedInstanceGroupIndex(meshBatch.m_GroupIndex, meshBatch.m_InstanceIndex);
          int num2 = -1;
          for (int batchIndex1 = 0; batchIndex1 < batchCount; ++batchIndex1)
          {
            BatchData batchData = nativeBatchGroups.GetBatchData(meshBatch.m_GroupIndex, batchIndex1);
            int managedBatchIndex = nativeBatchGroups.GetManagedBatchIndex(meshBatch.m_GroupIndex, batchIndex1);
            int batchIndex2 = -1;
            if (instanceGroupIndex >= 0)
              batchIndex2 = nativeBatchGroups.GetManagedBatchIndex(instanceGroupIndex, batchIndex1);
            if ((int) batchData.m_LodIndex != num2)
            {
              num2 = (int) batchData.m_LodIndex;
              info.Add(new InfoList.Item(string.Format("--- Mesh {0}, Tile {1}, Layer {2}, Lod {3} ---", (object) meshBatch.m_MeshIndex, (object) meshBatch.m_TileIndex, (object) groupData.m_Layer, (object) batchData.m_LodIndex)));
            }
            if (managedBatchIndex >= 0)
            {
              CustomBatch batch1 = (CustomBatch) managedBatches.GetBatch(managedBatchIndex);
              if (batchIndex2 >= 0)
              {
                CustomBatch batch2 = (CustomBatch) managedBatches.GetBatch(batchIndex2);
                RenderPrefab prefab2;
                RenderPrefab prefab3;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(batch1.sourceMeshEntity, out prefab2) && this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(batch2.sourceMeshEntity, out prefab3))
                {
                  if (batch1.generatedType != GeneratedType.None)
                    info.Add(new InfoList.Item(string.Format("{0} {1} -> {2} {3}", (object) prefab3.name, (object) batch2.generatedType, (object) prefab2.name, (object) batch1.generatedType)));
                  else
                    info.Add(new InfoList.Item(string.Format("{0}[{1}] -> {2}[{3}]", (object) prefab3.name, (object) batch2.sourceSubMeshIndex, (object) prefab2.name, (object) batch1.sourceSubMeshIndex)));
                }
                else
                  info.Add(new InfoList.Item(batch2.mesh.name + " -> " + batch1.mesh.name));
              }
              else
              {
                RenderPrefab prefab4;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(batch1.sourceMeshEntity, out prefab4))
                {
                  if (batch1.generatedType != GeneratedType.None)
                    info.Add(new InfoList.Item(string.Format("{0} {1}", (object) prefab4.name, (object) batch1.generatedType)));
                  else
                    info.Add(new InfoList.Item(string.Format("{0}[{1}]", (object) prefab4.name, (object) batch1.sourceSubMeshIndex)));
                }
                else
                  info.Add(new InfoList.Item(batch1.mesh.name));
              }
            }
          }
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Building>(entity) || this.EntityManager.HasComponent<Attached>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        info.label = "Address";
        Entity road;
        int number;
        if (BuildingUtils.GetAddress(this.EntityManager, entity, out road, out number))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.value = this.m_NameSystem.GetDebugName(road) + " " + (object) number;
          info.target = road;
        }
        else
        {
          info.value = "Unknown";
          info.target = InfoList.Item.kNullEntity;
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Building>(entity) && this.EntityManager.HasComponent<Renter>(entity) ? this.EntityManager.HasComponent<SpawnableBuildingData>(prefab) && this.EntityManager.HasComponent<Game.Prefabs.BuildingData>(prefab) : (this.EntityManager.HasComponent<Household>(entity) || this.EntityManager.HasComponent<CompanyData>(entity)) && this.EntityManager.HasComponent<PropertyRenter>(entity) && this.EntityManager.HasComponent<SpawnableBuildingData>(this.EntityManager.GetComponentData<PrefabRef>(this.EntityManager.GetComponentData<PropertyRenter>(entity).m_Property).m_Prefab)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        if (!this.EntityManager.HasComponent<Building>(entity))
        {
          entity = this.EntityManager.GetComponentData<PropertyRenter>(entity).m_Property;
          prefab = this.EntityManager.GetComponentData<PrefabRef>(entity).m_Prefab;
        }
        SpawnableBuildingData componentData1 = this.EntityManager.GetComponentData<SpawnableBuildingData>(prefab);
        Game.Prefabs.BuildingData componentData2 = this.EntityManager.GetComponentData<Game.Prefabs.BuildingData>(prefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        string prefabName = this.m_PrefabSystem.GetPrefabName(componentData1.m_ZonePrefab);
        info.label = "Zone Info";
        info.value = prefabName + " " + (object) componentData2.m_LotSize.x + "x" + (object) componentData2.m_LotSize.y;
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Building>(entity) && this.EntityManager.HasComponent<Renter>(entity) ? this.EntityManager.HasComponent<SpawnableBuildingData>(prefab) && this.EntityManager.HasComponent<Game.Prefabs.BuildingData>(prefab) : (this.EntityManager.HasComponent<Household>(entity) || this.EntityManager.HasComponent<CompanyData>(entity)) && this.EntityManager.HasComponent<PropertyRenter>(entity) && this.EntityManager.HasComponent<SpawnableBuildingData>(this.EntityManager.GetComponentData<PrefabRef>(this.EntityManager.GetComponentData<PropertyRenter>(entity).m_Property).m_Prefab)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_ProcessQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<SpawnableBuildingData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<BuildingPropertyData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ConsumptionData> roComponentLookup4 = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<CityModifier> modifierRoBufferLookup = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Building> roComponentLookup5 = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ElectricityConsumer> roComponentLookup6 = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<WaterConsumer> roComponentLookup7 = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<Game.Net.ServiceCoverage> coverageRoBufferLookup = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Locked> roComponentLookup8 = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Game.Objects.Transform> roComponentLookup9 = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<GarbageProducer> roComponentLookup10 = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<CrimeProducer> roComponentLookup11 = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<MailProducer> roComponentLookup12 = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<OfficeBuilding> roComponentLookup13 = this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<Renter> renterRoBufferLookup = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Citizen> roComponentLookup14 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<HouseholdCitizen> citizenRoBufferLookup = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Game.Prefabs.BuildingData> roComponentLookup15 = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<CompanyData> roComponentLookup16 = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<IndustrialProcessData> roComponentLookup17 = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<WorkProvider> roComponentLookup18 = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<Employee> employeeRoBufferLookup = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<WorkplaceData> roComponentLookup19 = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Citizen> roComponentLookup20 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<HealthProblem> roComponentLookup21 = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ServiceAvailable> roComponentLookup22 = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ResourceData> roComponentLookup23 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ZonePropertiesData> roComponentLookup24 = this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<Efficiency> efficiencyRoBufferLookup = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ServiceCompanyData> roComponentLookup25 = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<ResourceAvailability> availabilityRoBufferLookup = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<TradeCost> costRoBufferLookup = this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        CitizenHappinessParameterData singleton1 = this.m_CitizenHappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
        // ISSUE: reference to a compiler-generated field
        HealthcareParameterData singleton2 = this.m_HealthcareParameterQuery.GetSingleton<HealthcareParameterData>();
        // ISSUE: reference to a compiler-generated field
        ParkParameterData singleton3 = this.m_ParkParameterQuery.GetSingleton<ParkParameterData>();
        // ISSUE: reference to a compiler-generated field
        EducationParameterData singleton4 = this.m_EducationParameterQuery.GetSingleton<EducationParameterData>();
        // ISSUE: reference to a compiler-generated field
        TelecomParameterData singleton5 = this.m_TelecomParameterQuery.GetSingleton<TelecomParameterData>();
        this.CheckedStateRef.EntityManager.CompleteDependencyBeforeRW<HappinessFactorParameterData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<HappinessFactorParameterData> happinessFactorParameters = this.__TypeHandle.__Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup[this.m_HappinessFactorParameterQuery.GetSingletonEntity()];
        // ISSUE: reference to a compiler-generated field
        GarbageParameterData singleton6 = this.m_GarbageParameterQuery.GetSingleton<GarbageParameterData>();
        // ISSUE: reference to a compiler-generated field
        EconomyParameterData singleton7 = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>();
        // ISSUE: reference to a compiler-generated field
        ServiceFeeParameterData singleton8 = this.__query_746694603_0.GetSingleton<ServiceFeeParameterData>();
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        NativeArray<GroundPollution> buffer1 = this.m_GroundPollutionSystem.GetData(true, out dependencies3).m_Buffer;
        JobHandle dependencies4;
        // ISSUE: reference to a compiler-generated field
        NativeArray<NoisePollution> buffer2 = this.m_NoisePollutionSystem.GetData(true, out dependencies4).m_Buffer;
        JobHandle dependencies5;
        // ISSUE: reference to a compiler-generated field
        NativeArray<AirPollution> buffer3 = this.m_AirPollutionSystem.GetData(true, out dependencies5).m_Buffer;
        JobHandle dependencies6;
        // ISSUE: reference to a compiler-generated field
        CellMapData<TelecomCoverage> data = this.m_TelecomCoverageSystem.GetData(true, out dependencies6);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> taxRates = this.m_TaxSystem.GetTaxRates();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceFee> buffer4 = this.EntityManager.GetBuffer<ServiceFee>(this.m_CitySystem.City);
        // ISSUE: reference to a compiler-generated method
        float relativeElectricityFee = ServiceFeeSystem.GetFee(PlayerResource.Electricity, buffer4) / singleton8.m_ElectricityFee.m_Default;
        // ISSUE: reference to a compiler-generated method
        float relativeWaterFee = ServiceFeeSystem.GetFee(PlayerResource.Water, buffer4) / singleton8.m_WaterFee.m_Default;
        NativeArray<int> factors = new NativeArray<int>(28, Allocator.Temp);
        dependencies3.Complete();
        dependencies4.Complete();
        dependencies5.Complete();
        dependencies6.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        CitizenHappinessSystem.GetBuildingHappinessFactors(entity, factors, ref roComponentLookup1, ref roComponentLookup2, ref roComponentLookup3, ref roComponentLookup4, ref modifierRoBufferLookup, ref roComponentLookup5, ref roComponentLookup6, ref roComponentLookup7, ref coverageRoBufferLookup, ref roComponentLookup8, ref roComponentLookup9, ref roComponentLookup10, ref roComponentLookup11, ref roComponentLookup12, ref roComponentLookup13, ref renterRoBufferLookup, ref roComponentLookup14, ref citizenRoBufferLookup, ref roComponentLookup15, ref roComponentLookup16, ref roComponentLookup17, ref roComponentLookup18, ref employeeRoBufferLookup, ref roComponentLookup19, ref roComponentLookup20, ref roComponentLookup21, ref roComponentLookup22, ref roComponentLookup23, ref roComponentLookup24, ref efficiencyRoBufferLookup, ref roComponentLookup25, ref availabilityRoBufferLookup, ref costRoBufferLookup, singleton1, singleton6, singleton2, singleton3, singleton4, singleton5, ref singleton7, happinessFactorParameters, buffer1, buffer2, buffer3, data, this.m_CitySystem.City, taxRates, entityArray, prefabs, relativeElectricityFee, relativeWaterFee);
        entityArray.Dispose();
        NativeList<DeveloperInfoUISystem.BuildingHappinessFactorValue> list = new NativeList<DeveloperInfoUISystem.BuildingHappinessFactorValue>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: variable of a compiler-generated type
        DeveloperInfoUISystem.BuildingHappinessFactorValue happinessFactorValue;
        for (int index = 0; index < factors.Length; ++index)
        {
          if (factors[index] != 0)
          {
            ref NativeList<DeveloperInfoUISystem.BuildingHappinessFactorValue> local1 = ref list;
            // ISSUE: object of a compiler-generated type is created
            happinessFactorValue = new DeveloperInfoUISystem.BuildingHappinessFactorValue();
            // ISSUE: reference to a compiler-generated field
            happinessFactorValue.m_Factor = (BuildingHappinessFactor) index;
            // ISSUE: reference to a compiler-generated field
            happinessFactorValue.m_Value = factors[index];
            ref DeveloperInfoUISystem.BuildingHappinessFactorValue local2 = ref happinessFactorValue;
            local1.Add(in local2);
          }
        }
        list.Sort<DeveloperInfoUISystem.BuildingHappinessFactorValue>();
        string str = "";
        for (int index = 0; index < math.min(10, list.Length); ++index)
        {
          object[] objArray = new object[5]
          {
            (object) str,
            null,
            null,
            null,
            null
          };
          happinessFactorValue = list[index];
          // ISSUE: reference to a compiler-generated field
          objArray[1] = (object) happinessFactorValue.m_Factor.ToString();
          objArray[2] = (object) ": ";
          // ISSUE: reference to a compiler-generated field
          objArray[3] = (object) list[index].m_Value;
          objArray[4] = (object) "  ";
          str = string.Concat(objArray);
        }
        info.label = "Building happiness factors";
        info.value = str;
        info.target = InfoList.Item.kNullEntity;
        factors.Dispose();
        list.Dispose();
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        TelecomFacilityData component;
        if (!this.EntityManager.HasComponent<Building>(entity) || !this.EntityManager.HasComponent<Game.Buildings.TelecomFacility>(entity) || !this.EntityManager.TryGetComponent<TelecomFacilityData>(prefab, out component))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<TelecomFacilityData>(entity, ref component);
        return (double) component.m_Range >= 1.0;
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        TelecomFacilityData componentData = this.EntityManager.GetComponentData<TelecomFacilityData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<TelecomFacilityData>(entity, ref componentData);
        float x = 1f;
        DynamicBuffer<Efficiency> buffer;
        if (this.EntityManager.TryGetBuffer<Efficiency>(entity, true, out buffer))
          x = BuildingUtils.GetEfficiency(buffer);
        float f = componentData.m_Range * math.sqrt(x);
        info.label = "Range";
        info.value = Mathf.RoundToInt(f).ToString() + "/" + (object) Mathf.RoundToInt(componentData.m_Range);
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        if (!this.EntityManager.HasComponent<Building>(entity))
          return false;
        return this.EntityManager.HasComponent<PollutionData>(prefab) || this.EntityManager.HasComponent<Abandoned>(entity) || this.EntityManager.HasComponent<Destroyed>(entity) || this.EntityManager.HasComponent<Game.Buildings.Park>(entity);
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        this.CompleteDependency();
        bool destroyed = this.EntityManager.HasComponent<Destroyed>(entity);
        bool abandoned = this.EntityManager.HasComponent<Abandoned>(entity);
        bool isPark = this.EntityManager.HasComponent<Game.Buildings.Park>(entity);
        DynamicBuffer<Efficiency> buffer5;
        float efficiency = this.EntityManager.TryGetBuffer<Efficiency>(entity, true, out buffer5) ? BuildingUtils.GetEfficiency(buffer5) : 1f;
        DynamicBuffer<Renter> buffer6;
        this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer6);
        DynamicBuffer<InstalledUpgrade> buffer7;
        this.EntityManager.TryGetBuffer<InstalledUpgrade>(entity, true, out buffer7);
        // ISSUE: reference to a compiler-generated field
        PollutionParameterData singleton = this.__query_746694603_1.GetSingleton<PollutionParameterData>();
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> singletonBuffer = this.__query_746694603_2.GetSingletonBuffer<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PrefabRef> roComponentLookup26 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Game.Prefabs.BuildingData> roComponentLookup27 = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<SpawnableBuildingData> roComponentLookup28 = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PollutionData> roComponentLookup29 = this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PollutionModifierData> roComponentLookup30 = this.__TypeHandle.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ZoneData> roComponentLookup31 = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<Employee> employeeRoBufferLookup = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<HouseholdCitizen> citizenRoBufferLookup = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Citizen> roComponentLookup32 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated method
        PollutionData buildingPollution = BuildingPollutionAddSystem.GetBuildingPollution(prefab, destroyed, abandoned, isPark, efficiency, buffer6, buffer7, singleton, singletonBuffer, ref roComponentLookup26, ref roComponentLookup27, ref roComponentLookup28, ref roComponentLookup29, ref roComponentLookup30, ref roComponentLookup31, ref employeeRoBufferLookup, ref citizenRoBufferLookup, ref roComponentLookup32);
        info.label = "Pollution";
        info.value = "Ground: " + (object) buildingPollution.m_GroundPollution + ". " + "Air: " + (object) buildingPollution.m_AirPollution + ". " + "Noise: " + (object) buildingPollution.m_NoisePollution + ".";
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<GarbageProducer>(entity) && this.EntityManager.HasComponent<ConsumptionData>(prefab)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        ConsumptionData componentData3 = this.EntityManager.GetComponentData<ConsumptionData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<ConsumptionData>(entity, ref componentData3);
        GarbageProducer componentData4 = this.EntityManager.GetComponentData<GarbageProducer>(entity);
        // ISSUE: reference to a compiler-generated field
        GarbageParameterData singleton = this.__query_746694603_3.GetSingleton<GarbageParameterData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        GarbageAccumulationSystem.GetGarbage(ref componentData3, entity, prefab, this.GetBufferLookup<Renter>(true), this.GetBufferLookup<Game.Buildings.Student>(true), this.GetBufferLookup<Occupant>(true), this.GetComponentLookup<HomelessHousehold>(true), this.GetBufferLookup<HouseholdCitizen>(true), this.GetComponentLookup<Citizen>(true), this.GetBufferLookup<Employee>(true), this.GetBufferLookup<Patient>(true), this.GetComponentLookup<SpawnableBuildingData>(true), this.GetComponentLookup<CurrentDistrict>(true), this.GetBufferLookup<DistrictModifier>(true), this.GetComponentLookup<ZoneData>(true), this.GetBufferLookup<CityModifier>(true)[this.m_CitySystem.City], ref singleton);
        int homeless = 0;
        DynamicBuffer<Renter> buffer8;
        if (this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer8))
        {
          for (int index = 0; index < buffer8.Length; ++index)
          {
            DynamicBuffer<HouseholdCitizen> buffer9;
            if (this.EntityManager.HasComponent<HomelessHousehold>(buffer8[index].m_Renter) && this.EntityManager.TryGetBuffer<HouseholdCitizen>(buffer8[index].m_Renter, true, out buffer9))
              homeless += buffer9.Length;
          }
        }
        // ISSUE: reference to a compiler-generated method
        string garbageStatus = this.GetGarbageStatus(Mathf.RoundToInt(componentData3.m_GarbageAccumulation), componentData4.m_Garbage, homeless, singleton.m_HomelessGarbageProduce);
        info.label = "Garbage";
        info.value = garbageStatus;
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<WaterConsumer>(entity) && this.EntityManager.HasComponent<ConsumptionData>(prefab)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        ConsumptionData componentData5 = this.EntityManager.GetComponentData<ConsumptionData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<ConsumptionData>(entity, ref componentData5);
        WaterConsumer componentData6 = this.EntityManager.GetComponentData<WaterConsumer>(entity);
        info.label = "Water Consuming";
        info.value = string.Format("consuming: {0}  fill: {1}", (object) componentData6.m_WantedConsumption, (object) componentData6.m_FulfilledFresh);
        info.target = InfoList.Item.kNullEntity;
      })));
      SpectatorSite component4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, _) => this.EntityManager.TryGetComponent<SpectatorSite>(entity, out component4) && this.EntityManager.HasComponent<Duration>(component4.m_Event)), (Action<Entity, Entity, GenericInfo>) ((entity, _, info) =>
      {
        SpectatorSite componentData7 = this.EntityManager.GetComponentData<SpectatorSite>(entity);
        Duration componentData8 = this.EntityManager.GetComponentData<Duration>(componentData7.m_Event);
        // ISSUE: reference to a compiler-generated field
        if (this.m_SimulationSystem.frameIndex < componentData8.m_StartFrame)
        {
          info.label = "Preparing";
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_SimulationSystem.frameIndex < componentData8.m_EndFrame)
            info.label = "Event";
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(componentData7.m_Event);
        info.target = componentData7.m_Event;
      })));
      InDanger component5;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.TryGetComponent<InDanger>(entity, out component5) && this.EntityManager.Exists(component5.m_Event)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        InDanger componentData = this.EntityManager.GetComponentData<InDanger>(entity);
        if ((componentData.m_Flags & DangerFlags.Evacuate) != (DangerFlags) 0)
          info.label = "Evacuating";
        else if ((componentData.m_Flags & DangerFlags.StayIndoors) != (DangerFlags) 0)
          info.label = "In danger";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(componentData.m_Event);
        info.target = componentData.m_Event;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Game.Buildings.Park>(entity) && this.EntityManager.HasComponent<ParkData>(prefab)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Game.Buildings.Park componentData9 = this.EntityManager.GetComponentData<Game.Buildings.Park>(entity);
        ParkData componentData10 = this.EntityManager.GetComponentData<ParkData>(prefab);
        info.label = "Maintenance";
        info.value = componentData9.m_Maintenance.ToString() + "/" + (object) componentData10.m_MaintenancePool;
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.HasCompany(entity, prefab, out Entity _)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Renter> buffer = this.EntityManager.GetBuffer<Renter>(entity, true);
        info.label = "Company";
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity renter = buffer[index].m_Renter;
          PropertyRenter component6;
          if (this.EntityManager.HasComponent<CompanyData>(renter) && this.EntityManager.TryGetComponent<PropertyRenter>(renter, out component6))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.value = string.Format("Name:{0} Rent:{1}", (object) this.m_NameSystem.GetDebugName(renter), (object) component6.m_Rent);
            info.target = renter;
          }
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        if (this.EntityManager.HasComponent<Building>(entity))
          return this.EntityManager.HasComponent<Efficiency>(entity);
        PropertyRenter component7;
        return this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component7) && this.EntityManager.HasComponent<Efficiency>(component7.m_Property);
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        PropertyRenter component8;
        float efficiency = BuildingUtils.GetEfficiency(this.EntityManager.GetBuffer<Efficiency>(!this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component8) ? entity : component8.m_Property, true));
        info.label = "Efficiency";
        info.value = Mathf.RoundToInt(100f * efficiency).ToString() + " %";
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<ElectricityProducer>(entity) && this.EntityManager.HasComponent<PowerPlantData>(prefab)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        PowerPlantData componentData = this.EntityManager.GetComponentData<PowerPlantData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PowerPlantData>(entity, ref componentData);
        int electricityProduction = componentData.m_ElectricityProduction;
        info.label = "Electricity Production";
        info.value = electricityProduction.ToString();
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        BatteryData component9;
        if (!this.EntityManager.HasComponent<Game.Buildings.Battery>(entity) || !this.EntityManager.HasComponent<PowerPlantData>(prefab) || !this.EntityManager.TryGetComponent<BatteryData>(prefab, out component9))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<BatteryData>(entity, ref component9);
        return component9.m_Capacity > 0;
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Game.Buildings.Battery componentData11 = this.EntityManager.GetComponentData<Game.Buildings.Battery>(entity);
        BatteryData componentData12 = this.EntityManager.GetComponentData<BatteryData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<BatteryData>(entity, ref componentData12);
        info.label = "Batteries";
        info.value = Mathf.RoundToInt(100f * (float) (componentData11.m_StoredEnergy / componentData12.capacityTicks)).ToString() + "%";
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        GarbageFacilityData component10;
        if (!this.EntityManager.HasComponent<Game.Buildings.GarbageFacility>(entity) || !this.EntityManager.TryGetComponent<GarbageFacilityData>(prefab, out component10))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<GarbageFacilityData>(entity, ref component10);
        return component10.m_ProcessingSpeed > 0;
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Game.Buildings.GarbageFacility componentData13 = this.EntityManager.GetComponentData<Game.Buildings.GarbageFacility>(entity);
        GarbageFacilityData componentData14 = this.EntityManager.GetComponentData<GarbageFacilityData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<GarbageFacilityData>(entity, ref componentData14);
        info.label = "Garbage Processing Speed";
        info.value = componentData13.m_ProcessingRate.ToString() + "/" + (object) componentData14.m_ProcessingSpeed;
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        PostFacilityData component11;
        if (!this.EntityManager.HasComponent<Game.Buildings.PostFacility>(entity) || !this.EntityManager.TryGetComponent<PostFacilityData>(prefab, out component11))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PostFacilityData>(entity, ref component11);
        return component11.m_SortingRate > 0;
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Game.Buildings.PostFacility componentData15 = this.EntityManager.GetComponentData<Game.Buildings.PostFacility>(entity);
        PostFacilityData componentData16 = this.EntityManager.GetComponentData<PostFacilityData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PostFacilityData>(entity, ref componentData16);
        int num = (componentData16.m_SortingRate * (int) componentData15.m_ProcessingFactor + 50) / 100;
        info.label = "Mail Processing Speed";
        info.value = num.ToString() + "/" + (object) componentData16.m_SortingRate;
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        PostFacilityData component12;
        if (!this.EntityManager.HasComponent<Game.Buildings.PostFacility>(entity) || !this.EntityManager.TryGetComponent<PostFacilityData>(prefab, out component12))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PostFacilityData>(entity, ref component12);
        return component12.m_MailCapacity > 0;
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        PostFacilityData componentData = this.EntityManager.GetComponentData<PostFacilityData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PostFacilityData>(entity, ref componentData);
        DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        int resources1 = EconomyUtils.GetResources(Resource.UnsortedMail, buffer);
        int resources2 = EconomyUtils.GetResources(Resource.LocalMail, buffer);
        int resources3 = EconomyUtils.GetResources(Resource.OutgoingMail, buffer);
        string str1;
        if (componentData.m_PostVanCapacity <= 0)
          str1 = "Unsorted mail: " + (object) resources1 + ". Local mail: " + (object) resources2 + ".";
        else
          str1 = "Mail to deliver: " + (object) resources2 + ". Collected mail: " + (object) resources1 + ".";
        string str2 = str1;
        if (componentData.m_SortingRate > 0 || resources3 > 0)
          str2 = str2 + " Outgoing mail: " + (object) resources3;
        info.label = "Post Facility";
        info.value = str2;
        info.target = InfoList.Item.kNullEntity;
      })));
      PropertyRenter component13;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component13) && component13.m_Property != InfoList.Item.kNullEntity), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        PropertyRenter componentData = this.EntityManager.GetComponentData<PropertyRenter>(entity);
        DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity);
        info.label = "Rent";
        info.value = string.Format("Rent: {0} Money:{1}", (object) componentData.m_Rent, (object) EconomyUtils.GetResources(Resource.Money, buffer));
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.TryGetBuffer<TradeCost>(entity, true, out DynamicBuffer<TradeCost> _)), (Action<Entity, Entity, InfoList>) ((entity, prefab, infos) =>
      {
        infos.label = "Trade Costs";
        DynamicBuffer<TradeCost> buffer;
        if (!this.EntityManager.TryGetBuffer<TradeCost>(entity, true, out buffer))
          return;
        for (int index = 0; index < buffer.Length; ++index)
        {
          TradeCost tradeCost = buffer[index];
          infos.Add(new InfoList.Item(string.Format("{0} buy {1} sell {2}", (object) EconomyUtils.GetName(tradeCost.m_Resource), (object) tradeCost.m_BuyCost, (object) tradeCost.m_SellCost)));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        Game.Companies.StorageCompany component14;
        if (this.EntityManager.TryGetComponent<Game.Companies.StorageCompany>(entity, out component14) && component14.m_LastTradePartner != InfoList.Item.kNullEntity)
          return true;
        BuyingCompany component15;
        return this.EntityManager.TryGetComponent<BuyingCompany>(entity, out component15) && component15.m_LastTradePartner != InfoList.Item.kNullEntity;
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Game.Companies.StorageCompany component16;
        string debugName;
        Entity lastTradePartner;
        if (this.EntityManager.TryGetComponent<Game.Companies.StorageCompany>(entity, out component16) && component16.m_LastTradePartner != InfoList.Item.kNullEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          debugName = this.m_NameSystem.GetDebugName(component16.m_LastTradePartner);
          lastTradePartner = component16.m_LastTradePartner;
        }
        else
        {
          BuyingCompany componentData = this.EntityManager.GetComponentData<BuyingCompany>(entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          debugName = this.m_NameSystem.GetDebugName(componentData.m_LastTradePartner);
          lastTradePartner = componentData.m_LastTradePartner;
        }
        info.label = "Trade Partner";
        info.value = debugName;
        info.target = lastTradePartner;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Game.Companies.StorageCompany>(entity) && this.EntityManager.HasComponent<StorageCompanyData>(prefab)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        StorageCompanyData componentData = this.EntityManager.GetComponentData<StorageCompanyData>(prefab);
        DynamicBuffer<TradeCost> buffer10 = this.EntityManager.GetBuffer<TradeCost>(entity, true);
        DynamicBuffer<Game.Economy.Resources> buffer11 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        TradeCost tradeCost = EconomyUtils.GetTradeCost(componentData.m_StoredResources, buffer10);
        int resources = EconomyUtils.GetResources(componentData.m_StoredResources, buffer11);
        info.label = "Warehouse - ";
        info.value = "Stores: " + EconomyUtils.GetName(componentData.m_StoredResources) + " (" + (object) resources + "). Buy Cost: " + tradeCost.m_BuyCost.ToString("F1") + ". Sell Cost: " + tradeCost.m_SellCost.ToString("F1");
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<CompanyData>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
        this.EntityManager.HasComponent<ServiceAvailable>(entity);
        // ISSUE: reference to a compiler-generated field
        this.__query_746694603_4.GetSingleton<EconomyParameterData>();
        DynamicBuffer<Game.Economy.Resources> buffer12 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        this.EntityManager.GetComponentData<IndustrialProcessData>(prefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ResourceData> roComponentLookup33 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Game.Vehicles.DeliveryTruck> roComponentLookup34 = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<LayoutElement> elementRoBufferLookup = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup;
        DynamicBuffer<OwnedVehicle> buffer13;
        int worth = !this.EntityManager.TryGetBuffer<OwnedVehicle>(entity, true, out buffer13) ? EconomyUtils.GetCompanyTotalWorth(buffer12, prefabs, roComponentLookup33) : EconomyUtils.GetCompanyTotalWorth(buffer12, buffer13, elementRoBufferLookup, roComponentLookup34, prefabs, roComponentLookup33);
        info.label = "Company Economy - ";
        // ISSUE: reference to a compiler-generated method
        info.value = this.WorthToString(worth) + " (" + (object) worth + ").";
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.HasComponent<PropertyRenter>(entity) && this.EntityManager.HasComponent<Employee>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
        // ISSUE: reference to a compiler-generated field
        EconomyParameterData singleton = this.__query_746694603_4.GetSingleton<EconomyParameterData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ResourceData> roComponentLookup35 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Citizen> roComponentLookup36 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
        bool isIndustrial = !this.EntityManager.HasComponent<ServiceAvailable>(entity);
        bool flag = this.EntityManager.HasComponent<Game.Companies.ExtractorCompany>(entity);
        IndustrialProcessData component17;
        if (!this.EntityManager.TryGetComponent<IndustrialProcessData>(prefab, out component17))
          return;
        float buildingEfficiency = 0.0f;
        float concentration = 0.0f;
        PropertyRenter component18;
        if (this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component18))
        {
          DynamicBuffer<Efficiency> buffer14;
          buildingEfficiency = this.EntityManager.TryGetBuffer<Efficiency>(component18.m_Property, true, out buffer14) ? BuildingUtils.GetEfficiency(buffer14) : 1f;
          Attached component19;
          DynamicBuffer<Game.Areas.SubArea> buffer15;
          if (this.EntityManager.TryGetComponent<Attached>(component18.m_Property, out component19) && this.EntityManager.TryGetBuffer<Game.Areas.SubArea>(component19.m_Parent, true, out buffer15))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
            // ISSUE: reference to a compiler-generated method
            ExtractorCompanySystem.GetBestConcentration(component17.m_Output.m_Resource, buffer15, this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup, this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup, this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup, this.__query_746694603_5.GetSingleton<ExtractorParameterData>(), this.m_ResourceSystem.GetPrefabs(), this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup, out concentration, out int _);
          }
        }
        DynamicBuffer<Employee> buffer = this.EntityManager.GetBuffer<Employee>(entity, true);
        // ISSUE: reference to a compiler-generated method
        int totalWage = WorkProviderSystem.CalculateTotalWage(buffer, ref singleton);
        int productionPerDay = EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, isIndustrial, buffer, component17, prefabs, roComponentLookup35, roComponentLookup36, ref singleton);
        float companyProfitPerDay = (float) EconomyUtils.GetCompanyProfitPerDay(buildingEfficiency, isIndustrial, buffer, component17, prefabs, roComponentLookup35, roComponentLookup36, ref singleton);
        float num = (isIndustrial ? EconomyUtils.GetIndustrialPrice(component17.m_Output.m_Resource, prefabs, ref roComponentLookup35) : EconomyUtils.GetMarketPrice(component17.m_Output.m_Resource, prefabs, ref roComponentLookup35)) * (float) component17.m_Output.m_Amount;
        info.label = (isIndustrial ? (flag ? "Extractor" : "Industrial") : "Commercial") + " Company Profit";
        info.Add(flag ? new InfoList.Item(string.Format("efficiency:{0}% concentration:{1}", (object) (float) ((double) buildingEfficiency * 100.0), (object) concentration)) : new InfoList.Item(string.Format("efficiency:{0}%", (object) (float) ((double) buildingEfficiency * 100.0))));
        info.Add(new InfoList.Item("Wages: " + (object) totalWage));
        info.Add(new InfoList.Item(string.Format("Production Per Day: {0} * {1}({2})={3}", (object) productionPerDay, (object) EconomyUtils.GetNameFixed(component17.m_Output.m_Resource), (object) num, (object) (float) ((double) productionPerDay * (double) num))));
        if (component17.m_Input1.m_Resource != Resource.NoResource)
          info.Add(new InfoList.Item(string.Format("Input1: {0}*{1}({2})", (object) component17.m_Input1.m_Amount, (object) EconomyUtils.GetNameFixed(component17.m_Input1.m_Resource), (object) EconomyUtils.GetIndustrialPrice(component17.m_Input1.m_Resource, prefabs, ref roComponentLookup35))));
        if (component17.m_Input2.m_Resource != Resource.NoResource)
          info.Add(new InfoList.Item(string.Format("Input2: {0}*{1}({2})", (object) component17.m_Input2.m_Amount, (object) EconomyUtils.GetNameFixed(component17.m_Input2.m_Resource), (object) EconomyUtils.GetIndustrialPrice(component17.m_Input2.m_Resource, prefabs, ref roComponentLookup35))));
        if (component17.m_Output.m_Resource != Resource.NoResource)
          info.Add(new InfoList.Item(string.Format("Output: {0}*{1}({2})", (object) component17.m_Output.m_Amount, (object) EconomyUtils.GetNameFixed(component17.m_Output.m_Resource), (object) num)));
        info.Add(new InfoList.Item("Profit Per Day(wages deducted): " + (object) companyProfitPerDay));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Renter>(entity) && this.EntityManager.HasComponent<Building>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Building componentData = this.EntityManager.GetComponentData<Building>(entity);
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        float landValueBase = 0.0f;
        // ISSUE: reference to a compiler-generated field
        EconomyParameterData singleton = this.__query_746694603_4.GetSingleton<EconomyParameterData>();
        BuildingPropertyData component20;
        if (this.EntityManager.TryGetComponent<BuildingPropertyData>(prefab, out component20))
        {
          LandValue component21;
          if (this.EntityManager.TryGetComponent<LandValue>(componentData.m_RoadEdge, out component21))
            landValueBase = component21.m_LandValue;
          ConsumptionData component22;
          if (this.EntityManager.TryGetComponent<ConsumptionData>(prefab, out component22))
            num4 = component22.m_Upkeep;
          int lotSize = 0;
          Game.Prefabs.BuildingData component23;
          if (this.EntityManager.TryGetComponent<Game.Prefabs.BuildingData>(prefab, out component23))
            lotSize = component23.m_LotSize.x * component23.m_LotSize.y;
          Attached component24;
          DynamicBuffer<Game.Areas.SubArea> buffer16;
          if (this.EntityManager.TryGetComponent<Attached>(entity, out component24) && this.EntityManager.TryGetBuffer<Game.Areas.SubArea>(component24.m_Parent, true, out buffer16))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            lotSize += Mathf.CeilToInt(ExtractorAISystem.GetArea(buffer16, this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup, this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup));
          }
          Game.Zones.AreaType areaType = Game.Zones.AreaType.None;
          int buildingLevel = 1;
          SpawnableBuildingData component25;
          if (this.EntityManager.TryGetComponent<SpawnableBuildingData>(prefab, out component25))
          {
            buildingLevel = (int) component25.m_Level;
            EntityManager entityManager = this.EntityManager;
            areaType = entityManager.GetComponentData<ZoneData>(component25.m_ZonePrefab).m_AreaType;
            BuildingCondition component26;
            if (this.EntityManager.TryGetComponent<BuildingCondition>(entity, out component26))
            {
              entityManager = this.EntityManager;
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CityModifier> buffer17 = entityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true);
              num6 = BuildingUtils.GetLevelingCost(areaType, component20, (int) component25.m_Level, buffer17);
              num5 = component26.m_Condition;
            }
          }
          num3 = PropertyUtils.GetRentPricePerRenter(component22, component20, buildingLevel, lotSize, landValueBase, areaType, ref singleton);
        }
        info.label = "Rent/Upkeep/Land value/Leveling";
        info.value = string.Format("Rent per renter: {0} Upkeep: {1} LV: {2} Leveling {3} / {4}", (object) num3, (object) num4, (object) landValueBase, (object) num5, (object) num6);
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<ElectricityConsumer>(entity) && this.EntityManager.HasComponent<ConsumptionData>(prefab)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        ConsumptionData componentData17 = this.EntityManager.GetComponentData<ConsumptionData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<ConsumptionData>(entity, ref componentData17);
        ElectricityConsumer componentData18 = this.EntityManager.GetComponentData<ElectricityConsumer>(entity);
        info.label = "Electricity Consuming";
        info.value = string.Format("consuming: {0}  fill: {1}", (object) componentData18.m_WantedConsumption, (object) componentData18.m_FulfilledConsumption);
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<MailProducer>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        MailProducer componentData = this.EntityManager.GetComponentData<MailProducer>(entity);
        info.label = "Send Receive Mail";
        info.value = string.Format("Send: {0} Receive: {1}", (object) componentData.m_SendingMail, (object) componentData.receivingMail);
        info.target = InfoList.Item.kNullEntity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        TelecomFacilityData component27;
        if (!this.EntityManager.HasComponent<Building>(entity) || !this.EntityManager.HasComponent<Game.Buildings.TelecomFacility>(entity) || !this.EntityManager.TryGetComponent<TelecomFacilityData>(prefab, out component27))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<TelecomFacilityData>(entity, ref component27);
        return (double) component27.m_NetworkCapacity >= 1.0;
      }), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        TelecomFacilityData componentData = this.EntityManager.GetComponentData<TelecomFacilityData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<TelecomFacilityData>(entity, ref componentData);
        float num = 1f;
        DynamicBuffer<Efficiency> buffer18;
        if (this.EntityManager.TryGetBuffer<Efficiency>(entity, true, out buffer18))
          num = BuildingUtils.GetEfficiency(buffer18);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> buffer19 = this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true);
        CityUtils.ApplyModifier(ref componentData.m_NetworkCapacity, buffer19, CityModifierType.TelecomCapacity);
        float f = componentData.m_NetworkCapacity * num;
        info.label = "Network Capacity";
        info.value = Mathf.RoundToInt(f);
        info.max = info.value;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        EmergencyShelterData component28;
        if (!this.EntityManager.HasComponent<Game.Buildings.EmergencyShelter>(entity) || !this.EntityManager.TryGetComponent<EmergencyShelterData>(prefab, out component28) || !this.EntityManager.HasComponent<Occupant>(entity))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<EmergencyShelterData>(entity, ref component28);
        return component28.m_ShelterCapacity > 0;
      }), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        EmergencyShelterData componentData = this.EntityManager.GetComponentData<EmergencyShelterData>(prefab);
        DynamicBuffer<Occupant> buffer = this.EntityManager.GetBuffer<Occupant>(entity, true);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<EmergencyShelterData>(entity, ref componentData);
        info.label = "Occupants";
        info.value = buffer.Length;
        info.max = componentData.m_ShelterCapacity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        PoliceStationData component29;
        if (!this.EntityManager.HasComponent<Game.Buildings.PoliceStation>(entity) || !this.EntityManager.HasComponent<Occupant>(entity) || !this.EntityManager.TryGetComponent<PoliceStationData>(prefab, out component29))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PoliceStationData>(entity, ref component29);
        return component29.m_JailCapacity > 0;
      }), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        PoliceStationData componentData = this.EntityManager.GetComponentData<PoliceStationData>(prefab);
        DynamicBuffer<Occupant> buffer = this.EntityManager.GetBuffer<Occupant>(entity, true);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PoliceStationData>(entity, ref componentData);
        info.label = "Arrested criminals";
        info.value = buffer.Length;
        info.max = componentData.m_JailCapacity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        DeathcareFacilityData component30;
        if (!this.EntityManager.HasComponent<Game.Buildings.DeathcareFacility>(entity) || !this.EntityManager.HasComponent<Patient>(entity) || !this.EntityManager.TryGetComponent<DeathcareFacilityData>(prefab, out component30))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<DeathcareFacilityData>(entity, ref component30);
        return component30.m_StorageCapacity > 0;
      }), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Patient> buffer = this.EntityManager.GetBuffer<Patient>(entity, true);
        EntityManager entityManager = this.EntityManager;
        Game.Buildings.DeathcareFacility componentData19 = entityManager.GetComponentData<Game.Buildings.DeathcareFacility>(entity);
        entityManager = this.EntityManager;
        DeathcareFacilityData componentData20 = entityManager.GetComponentData<DeathcareFacilityData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<DeathcareFacilityData>(entity, ref componentData20);
        int num = componentData19.m_LongTermStoredCount + buffer.Length;
        info.label = "Bodies";
        info.value = num;
        info.max = componentData20.m_StorageCapacity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        GarbageFacilityData component31;
        if (!this.EntityManager.HasComponent<Game.Buildings.GarbageFacility>(entity) || !this.EntityManager.TryGetComponent<GarbageFacilityData>(prefab, out component31))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<GarbageFacilityData>(entity, ref component31);
        return component31.m_GarbageCapacity > 0;
      }), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        GarbageFacilityData componentData21 = this.EntityManager.GetComponentData<GarbageFacilityData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<GarbageFacilityData>(entity, ref componentData21);
        PowerPlantData component32;
        if (this.EntityManager.TryGetComponent<PowerPlantData>(prefab, out component32))
        {
          // ISSUE: reference to a compiler-generated method
          this.AddUpgradeData<PowerPlantData>(entity, ref component32);
        }
        int num = 0;
        DynamicBuffer<Game.Economy.Resources> buffer20;
        if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer20))
          num = EconomyUtils.GetResources(Resource.Garbage, buffer20);
        DynamicBuffer<Game.Areas.SubArea> buffer21;
        if (this.EntityManager.TryGetBuffer<Game.Areas.SubArea>(entity, true, out buffer21))
        {
          for (int index = 0; index < buffer21.Length; ++index)
          {
            Entity area = buffer21[index].m_Area;
            Storage component33;
            if (this.EntityManager.TryGetComponent<Storage>(area, out component33))
            {
              EntityManager entityManager = this.EntityManager;
              PrefabRef componentData22 = entityManager.GetComponentData<PrefabRef>(area);
              entityManager = this.EntityManager;
              Geometry componentData23 = entityManager.GetComponentData<Geometry>(area);
              StorageAreaData component34;
              if (this.EntityManager.TryGetComponent<StorageAreaData>(componentData22.m_Prefab, out component34))
              {
                componentData21.m_GarbageCapacity += AreaUtils.CalculateStorageCapacity(componentData23, component34);
                num += component33.m_Amount;
              }
            }
          }
        }
        info.label = "Stored Garbage";
        info.value = num;
        info.max = componentData21.m_GarbageCapacity;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        PostFacilityData component35;
        if (!this.EntityManager.HasComponent<Game.Buildings.PostFacility>(entity) || !this.EntityManager.TryGetComponent<PostFacilityData>(prefab, out component35) || !this.EntityManager.HasComponent<Game.Economy.Resources>(entity))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PostFacilityData>(entity, ref component35);
        return component35.m_MailCapacity > 0;
      }), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        PostFacilityData componentData = this.EntityManager.GetComponentData<PostFacilityData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PostFacilityData>(entity, ref componentData);
        DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        int resources4 = EconomyUtils.GetResources(Resource.UnsortedMail, buffer);
        int resources5 = EconomyUtils.GetResources(Resource.LocalMail, buffer);
        int resources6 = EconomyUtils.GetResources(Resource.OutgoingMail, buffer);
        int num7 = resources5;
        int num8 = resources4 + num7 + resources6;
        info.label = "Stored Mail";
        info.value = num8;
        info.max = componentData.m_MailCapacity;
      })));
      SpawnableBuildingData component36;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.TryGetComponent<SpawnableBuildingData>(prefab, out component36) && this.EntityManager.HasComponent<ZoneData>(component36.m_ZonePrefab) && this.EntityManager.HasComponent<BuildingPropertyData>(prefab) && this.EntityManager.HasComponent<BuildingCondition>(entity)), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        SpawnableBuildingData componentData24 = this.EntityManager.GetComponentData<SpawnableBuildingData>(prefab);
        ZoneData componentData25 = this.EntityManager.GetComponentData<ZoneData>(componentData24.m_ZonePrefab);
        BuildingPropertyData componentData26 = this.EntityManager.GetComponentData<BuildingPropertyData>(prefab);
        info.label = string.Format("Level Progression (level {0})", (object) componentData24.m_Level);
        info.value = this.EntityManager.GetComponentData<BuildingCondition>(entity).m_Condition;
        // ISSUE: reference to a compiler-generated field
        info.max = BuildingUtils.GetLevelingCost(componentData25.m_AreaType, componentData26, (int) componentData24.m_Level, this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        PrisonData component37;
        if (!this.EntityManager.HasComponent<Game.Buildings.Prison>(entity) || !this.EntityManager.TryGetComponent<PrisonData>(prefab, out component37) || !this.EntityManager.HasComponent<Occupant>(entity))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PrisonData>(entity, ref component37);
        return component37.m_PrisonerCapacity > 0;
      }), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        PrisonData componentData = this.EntityManager.GetComponentData<PrisonData>(prefab);
        DynamicBuffer<Occupant> buffer = this.EntityManager.GetBuffer<Occupant>(entity, true);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PrisonData>(entity, ref componentData);
        info.label = string.Format("Prisoners ({0})", (object) buffer.Length);
        info.Add(new InfoList.Item(buffer.Length.ToString() + "/" + (object) componentData.m_PrisonerCapacity));
        for (int index = 0; index < buffer.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer[index].m_Occupant), buffer[index].m_Occupant));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Building>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        int parkedCars = 0;
        int slotCapacity = 0;
        int parkingFee = 0;
        int laneCount = 0;
        string empty = string.Empty;
        NativeList<Entity> parkedCarList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        DynamicBuffer<Game.Net.SubLane> buffer22;
        if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(entity, true, out buffer22))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer22, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
        }
        DynamicBuffer<Game.Net.SubNet> buffer23;
        if (this.EntityManager.TryGetBuffer<Game.Net.SubNet>(entity, true, out buffer23))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer23, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
        }
        DynamicBuffer<Game.Objects.SubObject> buffer24;
        if (this.EntityManager.TryGetBuffer<Game.Objects.SubObject>(entity, true, out buffer24))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer24, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
        }
        info.label = string.Format("Parking ({0})", (object) parkedCarList.Length);
        Game.Prefabs.BuildingData component38;
        if (laneCount != 0 && this.EntityManager.TryGetComponent<Game.Prefabs.BuildingData>(prefab, out component38) && (component38.m_Flags & (Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar)) == (Game.Prefabs.BuildingFlags) 0)
        {
          int num = parkingFee / laneCount;
          info.Add(new InfoList.Item("Parking Fee: " + (object) num));
        }
        info.Add(new InfoList.Item(empty + " Parked Cars: " + (object) parkedCars + "/" + (object) slotCapacity + "."));
        for (int index = 0; index < parkedCarList.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(parkedCarList[index]), parkedCarList[index]));
        }
        parkedCarList.Dispose();
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => (this.EntityManager.HasComponent<Building>(entity) || this.EntityManager.HasComponent<CompanyData>(entity) || this.EntityManager.HasComponent<Household>(entity)) && this.EntityManager.HasComponent<OwnedVehicle>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<OwnedVehicle> buffer = this.EntityManager.GetBuffer<OwnedVehicle>(entity, true);
        int availableVehicles = VehicleUIUtils.GetAvailableVehicles(entity, this.EntityManager);
        info.label = string.Format("Vehicles availableVehicles:{0}", (object) availableVehicles);
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity vehicle = buffer[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(vehicle), vehicle));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Game.Economy.Resources>(entity) && this.EntityManager.HasComponent<Game.Buildings.ResourceProducer>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Game.Economy.Resources> buffer25 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        NativeList<ResourceProductionData> resources7 = new NativeList<ResourceProductionData>();
        DynamicBuffer<ResourceProductionData> buffer26;
        if (this.EntityManager.TryGetBuffer<ResourceProductionData>(prefab, true, out buffer26))
        {
          resources7 = new NativeList<ResourceProductionData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
          resources7.AddRange(buffer26.AsNativeArray());
        }
        DynamicBuffer<InstalledUpgrade> buffer27;
        if (this.EntityManager.TryGetBuffer<InstalledUpgrade>(entity, true, out buffer27))
        {
          for (int index = 0; index < buffer27.Length; ++index)
          {
            DynamicBuffer<ResourceProductionData> buffer28;
            if (this.EntityManager.TryGetBuffer<ResourceProductionData>(this.EntityManager.GetComponentData<PrefabRef>(buffer27[index].m_Upgrade).m_Prefab, true, out buffer28))
            {
              if (!resources7.IsCreated)
                resources7 = new NativeList<ResourceProductionData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
              ResourceProductionData.Combine(resources7, buffer28);
            }
          }
        }
        info.label = "Resource Production";
        if (!resources7.IsCreated)
          return;
        for (int index = 0; index < resources7.Length; ++index)
        {
          ResourceProductionData resourceProductionData = resources7[index];
          int resources8 = EconomyUtils.GetResources(resourceProductionData.m_Type, buffer25);
          info.Add(new InfoList.Item(string.Concat((object) (EconomyUtils.GetName(resourceProductionData.m_Type), " ", resources8, "/", resourceProductionData.m_StorageCapacity))));
        }
        resources7.Dispose();
      })));
      BuildingPropertyData component39;
      DynamicBuffer<Renter> buffer29;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Renter>(entity) && this.EntityManager.TryGetComponent<BuildingPropertyData>(prefab, out component39) && component39.m_ResidentialProperties > 0 && this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer29) && buffer29.Length > 0), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Renter> buffer30 = this.EntityManager.GetBuffer<Renter>(entity, true);
        info.label = string.Format("Households ({0})", (object) buffer30.Length);
        for (int index = 0; index < buffer30.Length; ++index)
        {
          Entity renter = buffer30[index].m_Renter;
          Household component40;
          PropertyRenter component41;
          DynamicBuffer<Game.Economy.Resources> buffer31;
          if (this.EntityManager.TryGetComponent<Household>(renter, out component40) && this.EntityManager.TryGetComponent<PropertyRenter>(renter, out component41) && this.EntityManager.TryGetBuffer<Game.Economy.Resources>(renter, true, out buffer31))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item(string.Format("Name:{0} Rent:{1} Wealth:{2}", (object) this.m_NameSystem.GetDebugName(renter), (object) component41.m_Rent, (object) EconomyUtils.GetHouseholdTotalWealth(component40, buffer31)), renter));
          }
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Household>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        info.label = "Household info";
        DynamicBuffer<HouseholdCitizen> buffer32 = this.EntityManager.GetBuffer<HouseholdCitizen>(entity, true);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Worker> roComponentLookup37 = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Citizen> roComponentLookup38 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<HealthProblem> roComponentLookup39 = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        EconomyParameterData singleton = this.__query_746694603_4.GetSingleton<EconomyParameterData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> taxRates = this.m_TaxSystem.GetTaxRates();
        Household componentData = this.EntityManager.GetComponentData<Household>(entity);
        DynamicBuffer<Game.Economy.Resources> buffer33 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        info.Add(new InfoList.Item("Wealth: " + (object) EconomyUtils.GetHouseholdTotalWealth(componentData, buffer33)));
        info.Add(new InfoList.Item("Income: " + (object) EconomyUtils.GetHouseholdIncome(buffer32, ref roComponentLookup37, ref roComponentLookup38, ref roComponentLookup39, ref singleton, taxRates)));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        if (!this.EntityManager.HasComponent<Building>(entity) || !this.EntityManager.HasComponent<Renter>(entity))
          return false;
        return this.EntityManager.HasComponent<Game.Buildings.Park>(entity) || this.EntityManager.HasComponent<Abandoned>(entity);
      }), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        info.label = "Homeless";
        DynamicBuffer<Renter> buffer34 = this.EntityManager.GetBuffer<Renter>(entity, true);
        info.label = string.Format("Homeless Count: ({0})", (object) buffer34.Length);
        for (int index = 0; index < buffer34.Length; ++index)
        {
          Entity renter = buffer34[index].m_Renter;
          if (this.EntityManager.HasComponent<Household>(renter))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(renter), renter));
          }
        }
      })));
      DynamicBuffer<HouseholdCitizen> buffer35;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Household>(entity) && this.EntityManager.TryGetBuffer<HouseholdCitizen>(entity, true, out buffer35) && buffer35.Length > 0), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<HouseholdCitizen> buffer36 = this.EntityManager.GetBuffer<HouseholdCitizen>(entity, true);
        info.label = string.Format("Residents ({0})", (object) buffer36.Length);
        for (int index = 0; index < buffer36.Length; ++index)
        {
          Entity citizen = buffer36[index].m_Citizen;
          if (this.EntityManager.HasComponent<Citizen>(citizen))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(citizen), citizen));
          }
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        if (this.EntityManager.HasComponent<Employee>(entity))
          return true;
        Entity company;
        // ISSUE: reference to a compiler-generated method
        return this.HasCompany(entity, prefab, out company) && this.EntityManager.HasComponent<Employee>(company);
      }), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Employee> buffer37;
        Entity company;
        // ISSUE: reference to a compiler-generated method
        if (!this.EntityManager.TryGetBuffer<Employee>(entity, true, out buffer37) && (!this.HasCompany(entity, prefab, out company) || !this.EntityManager.TryGetBuffer<Employee>(company, true, out buffer37)))
          return;
        info.label = string.Format("Employees ({0})", (object) buffer37.Length);
        for (int index = 0; index < buffer37.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer37[index].m_Worker), buffer37[index].m_Worker));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        HospitalData component42;
        if (!this.EntityManager.HasComponent<Game.Buildings.Hospital>(entity) || !this.EntityManager.TryGetComponent<HospitalData>(prefab, out component42))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<HospitalData>(entity, ref component42);
        return component42.m_PatientCapacity > 0;
      }), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Patient> buffer38 = this.EntityManager.GetBuffer<Patient>(entity, true);
        HospitalData componentData = this.EntityManager.GetComponentData<HospitalData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<HospitalData>(entity, ref componentData);
        info.label = string.Format("Patients ({0})", (object) buffer38.Length);
        for (int index = 0; index < buffer38.Length; ++index)
        {
          Entity patient = buffer38[index].m_Patient;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(patient), patient));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        SchoolData component43;
        if (!this.EntityManager.HasComponent<Game.Buildings.School>(entity) || !this.EntityManager.HasBuffer<Game.Buildings.Student>(entity) || !this.EntityManager.TryGetComponent<SchoolData>(prefab, out component43))
          return false;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<SchoolData>(entity, ref component43);
        return component43.m_StudentCapacity > 0;
      }), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Game.Buildings.Student> buffer39 = this.EntityManager.GetBuffer<Game.Buildings.Student>(entity, true);
        SchoolData componentData27 = this.EntityManager.GetComponentData<SchoolData>(prefab);
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<SchoolData>(entity, ref componentData27);
        info.label = string.Format("Students ({0})", (object) buffer39.Length);
        for (int index = 0; index < buffer39.Length; ++index)
        {
          Entity student = buffer39[index].m_Student;
          Citizen componentData28 = this.EntityManager.GetComponentData<Citizen>(student);
          float studyWillingness = componentData28.GetPseudoRandom(CitizenPseudoRandom.StudyWillingness).NextFloat();
          DynamicBuffer<Efficiency> buffer40;
          float efficiency = this.EntityManager.TryGetBuffer<Efficiency>(entity, true, out buffer40) ? BuildingUtils.GetEfficiency(buffer40) : 1f;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CityModifier> buffer41 = this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(student) + string.Format("Graduation: {0}", (object) GraduationSystem.GetGraduationProbability((int) componentData27.m_EducationLevel, (int) componentData28.m_WellBeing, componentData27, buffer41, studyWillingness, efficiency)), student));
        }
      })));
      DynamicBuffer<Game.Economy.Resources> buffer42;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<StorageCompanyData>(prefab) && this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer42) && buffer42.Length > 0), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        StorageCompanyData componentData = this.EntityManager.GetComponentData<StorageCompanyData>(prefab);
        DynamicBuffer<Game.Economy.Resources> buffer43 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        ResourceIterator iterator = ResourceIterator.GetIterator();
        info.label = string.Format("Stored Resources ({0})", (object) buffer43.Length);
        while (iterator.Next())
        {
          if ((componentData.m_StoredResources & iterator.resource) != Resource.NoResource)
          {
            int resources = EconomyUtils.GetResources(iterator.resource, buffer43);
            info.Add(new InfoList.Item(EconomyUtils.GetName(iterator.resource) + (object) resources));
          }
        }
      })));
      DynamicBuffer<HouseholdAnimal> buffer44;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Household>(entity) && this.EntityManager.HasComponent<Game.Prefabs.HouseholdData>(prefab) && this.EntityManager.TryGetBuffer<HouseholdAnimal>(entity, true, out buffer44) && buffer44.Length > 0), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<HouseholdAnimal> buffer45 = this.EntityManager.GetBuffer<HouseholdAnimal>(entity, true);
        info.label = "Household Pets";
        for (int index = 0; index < buffer45.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer45[index].m_HouseholdPet), buffer45[index].m_HouseholdPet));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.HasComponent<ServiceAvailable>(entity) && this.EntityManager.HasComponent<Game.Economy.Resources>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        ServiceAvailable componentData29 = this.EntityManager.GetComponentData<ServiceAvailable>(entity);
        ServiceCompanyData componentData30 = this.EntityManager.GetComponentData<ServiceCompanyData>(prefab);
        DynamicBuffer<Game.Economy.Resources> buffer46 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        info.label = "Service Company";
        IndustrialProcessData component44;
        if (this.EntityManager.HasComponent<Game.Companies.ProcessingCompany>(entity) && this.EntityManager.TryGetComponent<IndustrialProcessData>(prefab, out component44))
        {
          Resource resource = component44.m_Output.m_Resource;
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Service: " + (componentData29.m_ServiceAvailable.ToString() + "/" + componentData30.m_MaxService.ToString() + "(" + this.ServicesToString(componentData29.m_ServiceAvailable, componentData30.m_MaxService) + ")")));
          info.Add(new InfoList.Item("Provide Resource Storage: " + EconomyUtils.GetName(resource) + " (" + (object) EconomyUtils.GetResources(resource, buffer46) + ")"));
          PropertyRenter component45;
          LodgingProvider component46;
          if (this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component45) && component45.m_Property != InfoList.Item.kNullEntity && this.EntityManager.TryGetComponent<LodgingProvider>(entity, out component46) && resource == Resource.Lodging)
          {
            Entity property = component45.m_Property;
            EntityManager entityManager = this.EntityManager;
            Entity prefab5 = entityManager.GetComponentData<PrefabRef>(property).m_Prefab;
            entityManager = this.EntityManager;
            SpawnableBuildingData componentData31 = entityManager.GetComponentData<SpawnableBuildingData>(prefab5);
            entityManager = this.EntityManager;
            BuildingPropertyData componentData32 = entityManager.GetComponentData<BuildingPropertyData>(prefab5);
            entityManager = this.EntityManager;
            // ISSUE: reference to a compiler-generated method
            int roomCount = LodgingProviderSystem.GetRoomCount(entityManager.GetComponentData<Game.Prefabs.BuildingData>(prefab5).m_LotSize, (int) componentData31.m_Level, componentData32);
            info.Add(new InfoList.Item("Lodging rooms free: " + (object) component46.m_FreeRooms + "/" + (object) roomCount));
          }
        }
        BuyingCompany component47;
        if (!this.EntityManager.TryGetComponent<BuyingCompany>(entity, out component47))
          return;
        info.Add(new InfoList.Item("Trip Length: " + (object) component47.m_MeanInputTripLength));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.HasComponent<Game.Companies.ExtractorCompany>(entity) && this.EntityManager.HasComponent<IndustrialProcessData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Game.Economy.Resources> buffer47 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        IndustrialProcessData componentData = this.EntityManager.GetComponentData<IndustrialProcessData>(prefab);
        info.label = "Extractor Company";
        Resource resource = componentData.m_Output.m_Resource;
        info.Add(new InfoList.Item("Produces: " + EconomyUtils.GetName(resource) + " (" + (object) EconomyUtils.GetResources(resource, buffer47) + ")"));
        PropertyRenter component48;
        Attached component49;
        IndustrialProcessData component50;
        if (!this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component48) || !(component48.m_Property != InfoList.Item.kNullEntity) || !this.EntityManager.TryGetComponent<Attached>(component48.m_Property, out component49) || !this.EntityManager.TryGetComponent<WorkplaceData>(prefab, out WorkplaceData _) || !this.EntityManager.TryGetComponent<PrefabRef>(component48.m_Property, out PrefabRef _) || !this.EntityManager.TryGetComponent<IndustrialProcessData>(prefab, out component50))
          return;
        DynamicBuffer<Game.Areas.SubArea> buffer48 = this.EntityManager.GetBuffer<Game.Areas.SubArea>(component49.m_Parent, true);
        // ISSUE: reference to a compiler-generated field
        ExtractorParameterData singleton = this.__query_746694603_5.GetSingleton<ExtractorParameterData>();
        // ISSUE: reference to a compiler-generated field
        this.__query_746694603_4.GetSingleton<EconomyParameterData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Game.Areas.Lot> roComponentLookup40 = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Geometry> roComponentLookup41 = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Extractor> roComponentLookup42 = this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated method
        ResourcePrefabs prefabs = this.World.GetOrCreateSystemManaged<ResourceSystem>().GetPrefabs();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ResourceData> roComponentLookup43 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PrefabRef> roComponentLookup44 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ExtractorAreaData> roComponentLookup45 = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated method
        double area = (double) ExtractorAISystem.GetArea(buffer48, roComponentLookup40, roComponentLookup41);
        // ISSUE: reference to a compiler-generated method
        double resourcesInArea = (double) ExtractorAISystem.GetResourcesInArea(entity, buffer48, roComponentLookup42);
        // ISSUE: reference to a compiler-generated method
        ExtractorCompanySystem.GetBestConcentration(component50.m_Output.m_Resource, buffer48, roComponentLookup42, roComponentLookup44, roComponentLookup45, singleton, prefabs, roComponentLookup43, out float _, out int _);
        IndustrialProcessData processData = componentData;
        CompanyUtils.GetExtractorFittingWorkers((float) area, 1f, processData);
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.HasComponent<Game.Companies.ProcessingCompany>(entity) && this.EntityManager.HasComponent<IndustrialProcessData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<Game.Economy.Resources> buffer49 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
        IndustrialProcessData componentData = this.EntityManager.GetComponentData<IndustrialProcessData>(prefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Citizen> roComponentLookup = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
        info.label = "Processing Company";
        Resource resource1 = componentData.m_Input1.m_Resource;
        Resource resource2 = componentData.m_Input2.m_Resource;
        Resource resource3 = componentData.m_Output.m_Resource;
        info.Add(new InfoList.Item("In: " + EconomyUtils.GetName(resource1) + " (" + (object) EconomyUtils.GetResources(resource1, buffer49) + ")"));
        if (resource2 != Resource.NoResource)
          info.Add(new InfoList.Item("In: " + EconomyUtils.GetName(resource2) + " (" + (object) EconomyUtils.GetResources(resource2, buffer49) + ")"));
        info.Add(new InfoList.Item("Out: " + EconomyUtils.GetName(resource3) + " (" + (object) EconomyUtils.GetResources(resource3, buffer49) + ")"));
        // ISSUE: reference to a compiler-generated field
        this.__query_746694603_4.GetSingleton<EconomyParameterData>();
        EntityManager entityManager = this.EntityManager;
        if (!entityManager.HasComponent<ServiceAvailable>(entity))
        {
          entityManager = this.EntityManager;
          entityManager.HasComponent<Game.Companies.ExtractorCompany>(entity);
        }
        DynamicBuffer<Employee> buffer50;
        PropertyRenter component51;
        PrefabRef component52;
        if (this.EntityManager.TryGetBuffer<Employee>(entity, true, out buffer50) && this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component51) && this.EntityManager.TryGetComponent<PrefabRef>(component51.m_Property, out component52) && this.EntityManager.TryGetComponent<WorkplaceData>(prefab, out WorkplaceData _) && this.EntityManager.TryGetComponent<SpawnableBuildingData>((Entity) component52, out SpawnableBuildingData _))
        {
          // ISSUE: reference to a compiler-generated method
          float workforce = WorkProviderSystem.GetWorkforce(buffer50, roComponentLookup);
          info.Add(new InfoList.Item(string.Format("Workforce:{0}", (object) workforce)));
          DynamicBuffer<Efficiency> buffer51;
          float num = this.EntityManager.TryGetBuffer<Efficiency>(component51.m_Property, true, out buffer51) ? BuildingUtils.GetEfficiency(buffer51) : 1f;
          info.Add(new InfoList.Item(string.Format("Building Efficiency:{0}", (object) num)));
        }
        BuyingCompany component53;
        if (!this.EntityManager.TryGetComponent<BuyingCompany>(entity, out component53))
          return;
        info.Add(new InfoList.Item("Trip Length: " + (object) component53.m_MeanInputTripLength));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<StorageLimitData>(prefab) && this.EntityManager.HasComponent<Game.Economy.Resources>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        info.label = "Resource Storage";
        StorageLimitData component54;
        DynamicBuffer<Game.Economy.Resources> buffer52;
        if (!this.EntityManager.TryGetComponent<StorageLimitData>(prefab, out component54) || !this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer52))
          return;
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<StorageLimitData>(entity, ref component54);
        info.Add(new InfoList.Item(string.Format("Storage Limit: {0}", (object) component54.m_Limit)));
        for (int index = 0; index < buffer52.Length; ++index)
          info.Add(new InfoList.Item(string.Format("{0}({1})", (object) EconomyUtils.GetName(buffer52[index].m_Resource), (object) buffer52[index].m_Amount)));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<StorageTransferRequest>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<StorageTransferRequest> buffer53 = this.EntityManager.GetBuffer<StorageTransferRequest>(entity, true);
        info.label = "Transfer requests";
        for (int index = 0; index < buffer53.Length; ++index)
        {
          StorageTransferRequest storageTransferRequest = buffer53[index];
          info.Add(new InfoList.Item(string.Format("{0} {1} {2} {3} {4}", (object) storageTransferRequest.m_Amount, (object) EconomyUtils.GetName(storageTransferRequest.m_Resource), (storageTransferRequest.m_Flags & StorageTransferFlags.Incoming) != (StorageTransferFlags) 0 ? (object) " from " : (object) " to ", (object) storageTransferRequest.m_Target.Index, (storageTransferRequest.m_Flags & StorageTransferFlags.Car) != (StorageTransferFlags) 0 ? (object) "(C)" : (object) "")));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Owner>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Owner componentData = this.EntityManager.GetComponentData<Owner>(entity);
        info.label = "Owner";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(componentData.m_Owner);
        info.target = componentData.m_Owner;
      })));
      Game.Vehicles.PersonalCar component55;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.TryGetComponent<Game.Vehicles.PersonalCar>(entity, out component55) && component55.m_Keeper != InfoList.Item.kNullEntity), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Game.Vehicles.PersonalCar componentData = this.EntityManager.GetComponentData<Game.Vehicles.PersonalCar>(entity);
        info.label = "Keeper";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(componentData.m_Keeper);
        info.target = componentData.m_Keeper;
      })));
      Controller component56;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.TryGetComponent<Controller>(entity, out component56) && component56.m_Controller != InfoList.Item.kNullEntity), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Controller componentData = this.EntityManager.GetComponentData<Controller>(entity);
        info.label = "Controller";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(componentData.m_Controller);
        info.target = componentData.m_Controller;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        if (!this.EntityManager.HasComponent<Vehicle>(entity))
          return false;
        int num = 0;
        PersonalCarData component57;
        if (this.EntityManager.TryGetComponent<PersonalCarData>(prefab, out component57))
        {
          num = component57.m_PassengerCapacity;
        }
        else
        {
          PublicTransportVehicleData component58;
          if (this.EntityManager.TryGetComponent<PublicTransportVehicleData>(prefab, out component58))
          {
            num = component58.m_PassengerCapacity;
          }
          else
          {
            AmbulanceData component59;
            if (this.EntityManager.TryGetComponent<AmbulanceData>(prefab, out component59))
            {
              num = component59.m_PatientCapacity;
            }
            else
            {
              HearseData component60;
              if (this.EntityManager.TryGetComponent<HearseData>(prefab, out component60))
              {
                num = component60.m_CorpseCapacity;
              }
              else
              {
                PoliceCarData component61;
                if (this.EntityManager.TryGetComponent<PoliceCarData>(prefab, out component61))
                {
                  num = component61.m_CriminalCapacity;
                }
                else
                {
                  TaxiData component62;
                  if (this.EntityManager.TryGetComponent<TaxiData>(prefab, out component62))
                    num = component62.m_PassengerCapacity;
                }
              }
            }
          }
        }
        return num > 0;
      }), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        int num9 = 0;
        int num10 = 0;
        PersonalCarData component63;
        if (this.EntityManager.TryGetComponent<PersonalCarData>(prefab, out component63))
        {
          num9 = component63.m_PassengerCapacity;
        }
        else
        {
          PublicTransportVehicleData component64;
          if (this.EntityManager.TryGetComponent<PublicTransportVehicleData>(prefab, out component64))
          {
            num9 = component64.m_PassengerCapacity;
          }
          else
          {
            AmbulanceData component65;
            if (this.EntityManager.TryGetComponent<AmbulanceData>(prefab, out component65))
            {
              num9 = component65.m_PatientCapacity;
            }
            else
            {
              HearseData component66;
              if (this.EntityManager.TryGetComponent<HearseData>(prefab, out component66))
              {
                num9 = component66.m_CorpseCapacity;
              }
              else
              {
                PoliceCarData component67;
                if (this.EntityManager.TryGetComponent<PoliceCarData>(prefab, out component67))
                {
                  num9 = component67.m_CriminalCapacity;
                }
                else
                {
                  TaxiData component68;
                  if (this.EntityManager.TryGetComponent<TaxiData>(prefab, out component68))
                    num9 = component68.m_PassengerCapacity;
                }
              }
            }
          }
        }
        DynamicBuffer<Passenger> buffer54;
        if (this.EntityManager.TryGetBuffer<Passenger>(entity, true, out buffer54))
          num10 = buffer54.Length;
        info.label = "Passengers";
        info.value = num10;
        info.max = num9;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.PersonalCar>(entity) && this.EntityManager.HasComponent<PersonalCarData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.PersonalCar componentData = this.EntityManager.GetComponentData<Game.Vehicles.PersonalCar>(entity);
        info.label = "Personal Car";
        if (this.EntityManager.HasComponent<ParkedCar>(entity))
          info.Add(new InfoList.Item("Parked"));
        if ((componentData.m_State & PersonalCarFlags.Boarding) != (PersonalCarFlags) 0)
          info.Add(new InfoList.Item("Boarding"));
        else if ((componentData.m_State & PersonalCarFlags.Disembarking) != (PersonalCarFlags) 0)
          info.Add(new InfoList.Item("Disembarking"));
        else if ((componentData.m_State & PersonalCarFlags.Transporting) != (PersonalCarFlags) 0)
          info.Add(new InfoList.Item("Transporting"));
        if ((componentData.m_State & PersonalCarFlags.DummyTraffic) != (PersonalCarFlags) 0)
          info.Add(new InfoList.Item("Dummy Traffic"));
        if ((componentData.m_State & PersonalCarFlags.HomeTarget) == (PersonalCarFlags) 0)
          return;
        info.Add(new InfoList.Item("Home Target"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.DeliveryTruck>(entity) && this.EntityManager.HasComponent<DeliveryTruckData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.DeliveryTruck componentData33 = this.EntityManager.GetComponentData<Game.Vehicles.DeliveryTruck>(entity);
        DeliveryTruckData componentData34 = this.EntityManager.GetComponentData<DeliveryTruckData>(prefab);
        Resource resource = Resource.NoResource;
        int num11 = 0;
        int num12 = 0;
        DynamicBuffer<LayoutElement> buffer55;
        if (this.EntityManager.TryGetBuffer<LayoutElement>(entity, true, out buffer55) && buffer55.Length != 0)
        {
          for (int index = 0; index < buffer55.Length; ++index)
          {
            Entity vehicle = buffer55[index].m_Vehicle;
            Game.Vehicles.DeliveryTruck component69;
            if (this.EntityManager.TryGetComponent<Game.Vehicles.DeliveryTruck>(vehicle, out component69))
            {
              resource |= component69.m_Resource;
              if ((component69.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0)
                num11 += component69.m_Amount;
              DeliveryTruckData component70;
              if (this.EntityManager.TryGetComponent<DeliveryTruckData>(this.EntityManager.GetComponentData<PrefabRef>(vehicle).m_Prefab, out component70))
                num12 += component70.m_CargoCapacity;
            }
          }
        }
        else
        {
          resource = componentData33.m_Resource;
          if ((componentData33.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0)
            num11 = componentData33.m_Amount;
          num12 = componentData34.m_CargoCapacity;
        }
        bool flag1 = (componentData33.m_State & DeliveryTruckFlags.StorageTransfer) > (DeliveryTruckFlags) 0;
        bool flag2 = (componentData33.m_State & DeliveryTruckFlags.Buying) > (DeliveryTruckFlags) 0;
        bool flag3 = (componentData33.m_State & DeliveryTruckFlags.Returning) > (DeliveryTruckFlags) 0;
        bool flag4 = (componentData33.m_State & DeliveryTruckFlags.Delivering) > (DeliveryTruckFlags) 0;
        info.label = "Delivery Truck";
        if ((componentData33.m_State & DeliveryTruckFlags.DummyTraffic) != (DeliveryTruckFlags) 0)
          info.Add(new InfoList.Item("Dummy Traffic"));
        info.Add(new InfoList.Item("Cargo: " + (object) num11 + "/" + (object) num12));
        Game.Common.Target component71;
        if (this.EntityManager.TryGetComponent<Game.Common.Target>(entity, out component71))
        {
          if (flag2)
          {
            string str3 = flag3 ? "Bought " : "Buying ";
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            string str4 = flag3 ? string.Empty : "from " + this.m_NameSystem.GetDebugName(component71.m_Target);
            Entity entity1 = flag3 ? InfoList.Item.kNullEntity : component71.m_Target;
            info.Add(new InfoList.Item(str3 + (object) resource + str4, entity1));
          }
          else if (flag1)
          {
            string str5 = flag3 ? "Exported " : "Exporting ";
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            string str6 = flag3 ? string.Empty : "to " + this.m_NameSystem.GetDebugName(component71.m_Target);
            Entity entity2 = flag3 ? InfoList.Item.kNullEntity : component71.m_Target;
            info.Add(new InfoList.Item(str5 + (object) resource + str6, entity2));
          }
          else if (flag4)
          {
            string str7 = flag3 ? "Delivered " : "Delivering ";
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            string str8 = flag3 ? string.Empty : "to " + this.m_NameSystem.GetDebugName(component71.m_Target);
            Entity entity3 = flag3 ? InfoList.Item.kNullEntity : component71.m_Target;
            info.Add(new InfoList.Item(str7 + (object) resource + str8, entity3));
          }
          else
          {
            string str9 = flag3 ? "Transported " : "Transporting ";
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            string str10 = flag3 ? string.Empty : "to " + this.m_NameSystem.GetDebugName(component71.m_Target);
            Entity entity4 = flag3 ? InfoList.Item.kNullEntity : component71.m_Target;
            info.Add(new InfoList.Item(str9 + (object) resource + str10, entity4));
          }
        }
        if (!flag3)
          return;
        info.Add(new InfoList.Item("Returning"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.Ambulance>(entity) && this.EntityManager.HasComponent<AmbulanceData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.Ambulance componentData = this.EntityManager.GetComponentData<Game.Vehicles.Ambulance>(entity);
        info.label = "Ambulance";
        Game.Common.Target component72;
        if (!this.EntityManager.TryGetComponent<Game.Common.Target>(entity, out component72))
          return;
        if (componentData.m_TargetPatient != InfoList.Item.kNullEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Patient" + this.m_NameSystem.GetDebugName(componentData.m_TargetPatient), componentData.m_TargetPatient));
        }
        if ((componentData.m_State & AmbulanceFlags.Returning) != (AmbulanceFlags) 0)
          info.Add(new InfoList.Item("Returning"));
        else if ((componentData.m_State & AmbulanceFlags.Transporting) != (AmbulanceFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Transporting to: " + this.m_NameSystem.GetDebugName(component72.m_Target), component72.m_Target));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Picking up from: " + this.m_NameSystem.GetDebugName(component72.m_Target), component72.m_Target));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.Hearse>(entity) && this.EntityManager.HasComponent<HearseData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.Hearse componentData = this.EntityManager.GetComponentData<Game.Vehicles.Hearse>(entity);
        info.label = "Hearse";
        Game.Common.Target component73;
        if (!this.EntityManager.TryGetComponent<Game.Common.Target>(entity, out component73))
          return;
        if (componentData.m_TargetCorpse != InfoList.Item.kNullEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Body" + this.m_NameSystem.GetDebugName(componentData.m_TargetCorpse), componentData.m_TargetCorpse));
        }
        if ((componentData.m_State & HearseFlags.Returning) != (HearseFlags) 0)
          info.Add(new InfoList.Item("Returning"));
        else if ((componentData.m_State & HearseFlags.Transporting) != (HearseFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Transporting to" + this.m_NameSystem.GetDebugName(component73.m_Target), component73.m_Target));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Picking up from" + this.m_NameSystem.GetDebugName(component73.m_Target), component73.m_Target));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.GarbageTruck>(entity) && this.EntityManager.HasComponent<GarbageTruckData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.GarbageTruck componentData35 = this.EntityManager.GetComponentData<Game.Vehicles.GarbageTruck>(entity);
        GarbageTruckData componentData36 = this.EntityManager.GetComponentData<GarbageTruckData>(prefab);
        info.label = "Garbage Truck";
        info.Add(new InfoList.Item("Capacity: " + (object) componentData35.m_Garbage + "/" + (object) componentData36.m_GarbageCapacity));
        if ((componentData35.m_State & GarbageTruckFlags.Unloading) != (GarbageTruckFlags) 0)
        {
          info.Add(new InfoList.Item("Unloading"));
        }
        else
        {
          if ((componentData35.m_State & GarbageTruckFlags.Returning) == (GarbageTruckFlags) 0)
            return;
          info.Add(new InfoList.Item("Returning"));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.PublicTransport>(entity) && this.EntityManager.HasComponent<PublicTransportVehicleData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.PublicTransport componentData = this.EntityManager.GetComponentData<Game.Vehicles.PublicTransport>(entity);
        info.label = "Public Transport";
        if ((componentData.m_State & PublicTransportFlags.DummyTraffic) != (PublicTransportFlags) 0)
          info.Add(new InfoList.Item("Dummy Traffic"));
        CurrentRoute component74;
        if (this.EntityManager.TryGetComponent<CurrentRoute>(entity, out component74))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Line: " + this.m_NameSystem.GetDebugName(component74.m_Route), component74.m_Route));
        }
        if ((componentData.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0)
          info.Add(new InfoList.Item("Returning"));
        else if ((componentData.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        {
          info.Add(new InfoList.Item("Boarding"));
          // ISSUE: reference to a compiler-generated field
          if (this.m_SimulationSystem.frameIndex < componentData.m_DepartureFrame)
          {
            // ISSUE: reference to a compiler-generated field
            int num = Mathf.CeilToInt((float) (componentData.m_DepartureFrame - this.m_SimulationSystem.frameIndex) / 60f);
            info.Add(new InfoList.Item("Departure: " + (object) num + "s"));
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            Entity passengerWaiting = this.GetPassengerWaiting(entity);
            if (passengerWaiting != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Waiting for: " + this.m_NameSystem.GetDebugName(passengerWaiting), passengerWaiting));
            }
          }
        }
        else if ((componentData.m_State & PublicTransportFlags.EnRoute) != (PublicTransportFlags) 0)
          info.Add(new InfoList.Item("En route"));
        PublicTransportVehicleData component75;
        if (this.EntityManager.TryGetComponent<PublicTransportVehicleData>(prefab, out component75))
        {
          if ((componentData.m_State & PublicTransportFlags.RequiresMaintenance) != (PublicTransportFlags) 0)
          {
            info.Add(new InfoList.Item("Maintenance scheduled"));
          }
          else
          {
            Odometer component76;
            if ((double) component75.m_MaintenanceRange > 0.10000000149011612 && this.EntityManager.TryGetComponent<Odometer>(entity, out component76))
            {
              int num13 = Mathf.RoundToInt(component75.m_MaintenanceRange * (1f / 1000f));
              int num14 = math.max(0, Mathf.RoundToInt((float) (((double) component75.m_MaintenanceRange - (double) component76.m_Distance) * (1.0 / 1000.0))));
              info.Add(new InfoList.Item("Remaining range: " + (object) num14 + "/" + (object) num13));
            }
          }
        }
        int nextWaypointIndex;
        float segmentPosition;
        // ISSUE: reference to a compiler-generated method
        if (!this.GetRoutePosition(entity, out nextWaypointIndex, out segmentPosition))
          return;
        info.Add(new InfoList.Item("Route waypoint index: " + (object) nextWaypointIndex));
        info.Add(new InfoList.Item("Route segment position: " + (object) Mathf.RoundToInt(segmentPosition * 100f) + "%"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.CargoTransport>(entity) && this.EntityManager.HasComponent<CargoTransportVehicleData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.CargoTransport componentData37 = this.EntityManager.GetComponentData<Game.Vehicles.CargoTransport>(entity);
        CargoTransportVehicleData componentData38 = this.EntityManager.GetComponentData<CargoTransportVehicleData>(prefab);
        info.label = "Cargo Transport";
        if ((componentData37.m_State & CargoTransportFlags.DummyTraffic) != (CargoTransportFlags) 0)
          info.Add(new InfoList.Item("Dummy Traffic"));
        CurrentRoute component77;
        if (this.EntityManager.TryGetComponent<CurrentRoute>(entity, out component77))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Route: " + this.m_NameSystem.GetDebugName(component77.m_Route), component77.m_Route));
        }
        if ((componentData37.m_State & CargoTransportFlags.Returning) != (CargoTransportFlags) 0)
          info.Add(new InfoList.Item("Returning"));
        else if ((componentData37.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0)
        {
          info.Add(new InfoList.Item("Loading"));
          // ISSUE: reference to a compiler-generated field
          if (this.m_SimulationSystem.frameIndex < componentData37.m_DepartureFrame)
          {
            // ISSUE: reference to a compiler-generated field
            int num = Mathf.CeilToInt((float) (componentData37.m_DepartureFrame - this.m_SimulationSystem.frameIndex) / 60f);
            info.Add(new InfoList.Item("Departure: " + (object) num + "s"));
          }
        }
        else if ((componentData37.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0)
          info.Add(new InfoList.Item("En route"));
        CargoTransportVehicleData component78;
        if (this.EntityManager.TryGetComponent<CargoTransportVehicleData>(prefab, out component78))
        {
          if ((componentData37.m_State & CargoTransportFlags.RequiresMaintenance) != (CargoTransportFlags) 0)
          {
            info.Add(new InfoList.Item("Maintenance scheduled"));
          }
          else
          {
            Odometer component79;
            if ((double) component78.m_MaintenanceRange > 0.10000000149011612 && this.EntityManager.TryGetComponent<Odometer>(entity, out component79))
            {
              int num15 = Mathf.RoundToInt(component78.m_MaintenanceRange * (1f / 1000f));
              int num16 = math.max(0, Mathf.RoundToInt((float) (((double) component78.m_MaintenanceRange - (double) component79.m_Distance) * (1.0 / 1000.0))));
              info.Add(new InfoList.Item("Remaining range: " + (object) num16 + "/" + (object) num15));
            }
          }
        }
        NativeList<Game.Economy.Resources> target = new NativeList<Game.Economy.Resources>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        int num17 = 0;
        DynamicBuffer<LayoutElement> buffer56;
        if (this.EntityManager.TryGetBuffer<LayoutElement>(entity, true, out buffer56))
        {
          for (int index = 0; index < buffer56.Length; ++index)
          {
            Entity vehicle = buffer56[index].m_Vehicle;
            DynamicBuffer<Game.Economy.Resources> buffer57;
            if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(vehicle, true, out buffer57))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddResources(buffer57, target);
            }
            PrefabRef component80;
            CargoTransportVehicleData component81;
            if (this.EntityManager.TryGetComponent<PrefabRef>(vehicle, out component80) && this.EntityManager.TryGetComponent<CargoTransportVehicleData>(component80.m_Prefab, out component81))
              num17 += component81.m_CargoCapacity;
          }
        }
        else
        {
          DynamicBuffer<Game.Economy.Resources> buffer58;
          if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer58))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddResources(buffer58, target);
          }
          num17 += componentData38.m_CargoCapacity;
        }
        info.Add(new InfoList.Item("Cargo: "));
        int num18 = 0;
        for (int index = 0; index < target.Length; ++index)
        {
          Game.Economy.Resources resources = target[index];
          info.Add(new InfoList.Item(resources.m_Resource.ToString() + " " + (object) resources.m_Amount));
          num18 += resources.m_Amount;
        }
        info.Add(new InfoList.Item("Capacity " + (object) num18 + "/" + (object) num17));
        target.Dispose();
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.MaintenanceVehicle>(entity) && this.EntityManager.HasComponent<MaintenanceVehicleData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.MaintenanceVehicle componentData39 = this.EntityManager.GetComponentData<Game.Vehicles.MaintenanceVehicle>(entity);
        MaintenanceVehicleData componentData40 = this.EntityManager.GetComponentData<MaintenanceVehicleData>(prefab);
        componentData40.m_MaintenanceCapacity = Mathf.CeilToInt((float) componentData40.m_MaintenanceCapacity * componentData39.m_Efficiency);
        info.label = "Maintenance Vehicle";
        info.Add(new InfoList.Item("Work shift: " + string.Format("{0}%", (object) Mathf.CeilToInt(math.select((float) componentData39.m_Maintained / (float) componentData40.m_MaintenanceCapacity, 0.0f, componentData40.m_MaintenanceCapacity == 0) * 100f))));
        if ((componentData39.m_State & MaintenanceVehicleFlags.ClearingDebris) != (MaintenanceVehicleFlags) 0)
        {
          info.Add(new InfoList.Item("Clearing debris"));
        }
        else
        {
          if ((componentData39.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0)
            return;
          info.Add(new InfoList.Item("Returning"));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.PostVan>(entity) && this.EntityManager.HasComponent<PostVanData>(prefab)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.PostVan componentData41 = this.EntityManager.GetComponentData<Game.Vehicles.PostVan>(entity);
        PostVanData componentData42 = this.EntityManager.GetComponentData<PostVanData>(prefab);
        info.label = "Post Van";
        info.Add(new InfoList.Item("Mail to deliver: " + (object) componentData41.m_DeliveringMail + "/" + (object) componentData42.m_MailCapacity));
        info.Add(new InfoList.Item("Collected mail: " + (object) componentData41.m_CollectedMail + "/" + (object) componentData42.m_MailCapacity));
        if ((componentData41.m_State & PostVanFlags.Returning) == (PostVanFlags) 0)
          return;
        info.Add(new InfoList.Item("Returning"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.FireEngine>(entity) && this.EntityManager.HasComponent<FireEngineData>(prefab) && this.EntityManager.HasComponent<ServiceDispatch>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.FireEngine componentData43 = this.EntityManager.GetComponentData<Game.Vehicles.FireEngine>(entity);
        EntityManager entityManager = this.EntityManager;
        FireEngineData componentData44 = entityManager.GetComponentData<FireEngineData>(prefab);
        entityManager = this.EntityManager;
        DynamicBuffer<ServiceDispatch> buffer59 = entityManager.GetBuffer<ServiceDispatch>(entity, true);
        info.label = "Fire Engine";
        int num19 = Mathf.CeilToInt(componentData43.m_ExtinguishingAmount);
        int num20 = Mathf.CeilToInt(componentData44.m_ExtinguishingCapacity);
        if (num20 > 0)
          info.Add(new InfoList.Item("Load: " + (object) num19 + "/" + (object) num20));
        if ((componentData43.m_State & FireEngineFlags.Extinguishing) != (FireEngineFlags) 0)
          info.Add(new InfoList.Item("Extinguishing"));
        else if ((componentData43.m_State & FireEngineFlags.Rescueing) != (FireEngineFlags) 0)
          info.Add(new InfoList.Item("Searching for survivors"));
        else if ((componentData43.m_State & FireEngineFlags.Returning) != (FireEngineFlags) 0)
        {
          info.Add(new InfoList.Item("Returning"));
        }
        else
        {
          FireRescueRequest component82;
          if (componentData43.m_RequestCount <= 0 || buffer59.Length <= 0 || !this.EntityManager.TryGetComponent<FireRescueRequest>(buffer59[0].m_Request, out component82))
            return;
          OnFire component83;
          if (this.EntityManager.TryGetComponent<OnFire>(component82.m_Target, out component83) && component83.m_Event != InfoList.Item.kNullEntity)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component83.m_Event), component83.m_Event));
          }
          else
          {
            Destroyed component84;
            if (this.EntityManager.TryGetComponent<Destroyed>(component82.m_Target, out component84) && component84.m_Event != InfoList.Item.kNullEntity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component84.m_Event), component84.m_Event));
            }
            else
            {
              if (!(component82.m_Target != InfoList.Item.kNullEntity))
                return;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component82.m_Target), component82.m_Target));
            }
          }
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.PoliceCar>(entity) && this.EntityManager.HasComponent<PoliceCarData>(prefab) && this.EntityManager.HasComponent<ServiceDispatch>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Game.Vehicles.PoliceCar componentData45 = this.EntityManager.GetComponentData<Game.Vehicles.PoliceCar>(entity);
        PoliceCarData componentData46 = this.EntityManager.GetComponentData<PoliceCarData>(prefab);
        DynamicBuffer<ServiceDispatch> buffer60 = this.EntityManager.GetBuffer<ServiceDispatch>(entity, true);
        info.label = "Police Car";
        if (componentData46.m_ShiftDuration > 0U)
        {
          uint num = math.min(componentData45.m_ShiftTime, componentData46.m_ShiftDuration);
          info.Add(new InfoList.Item("Work shift: " + (object) num + "/" + (object) componentData46.m_ShiftDuration));
        }
        DynamicBuffer<Passenger> buffer61;
        if (this.EntityManager.TryGetBuffer<Passenger>(entity, true, out buffer61))
        {
          for (int index = 0; index < buffer61.Length; ++index)
          {
            Entity entity5 = buffer61[index].m_Passenger;
            Game.Creatures.Resident component85;
            if (this.EntityManager.TryGetComponent<Game.Creatures.Resident>(entity5, out component85))
              entity5 = component85.m_Citizen;
            if (this.EntityManager.HasComponent<Citizen>(entity5))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Arrested criminal" + this.m_NameSystem.GetDebugName(entity5), entity5));
            }
          }
        }
        if ((componentData45.m_State & PoliceCarFlags.Returning) != (PoliceCarFlags) 0)
          info.Add(new InfoList.Item("Returning"));
        else if ((componentData45.m_State & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0)
        {
          if ((componentData45.m_State & PoliceCarFlags.AtTarget) != (PoliceCarFlags) 0)
          {
            PoliceEmergencyRequest component86;
            AccidentSite component87;
            if (componentData45.m_RequestCount <= 0 || buffer60.Length <= 0 || !this.EntityManager.TryGetComponent<PoliceEmergencyRequest>(buffer60[0].m_Request, out component86) || !this.EntityManager.TryGetComponent<AccidentSite>(component86.m_Site, out component87))
              return;
            if ((component87.m_Flags & AccidentSiteFlags.TrafficAccident) != (AccidentSiteFlags) 0)
            {
              info.Add(new InfoList.Item("Securing accident site"));
            }
            else
            {
              if ((component87.m_Flags & AccidentSiteFlags.CrimeScene) == (AccidentSiteFlags) 0)
                return;
              info.Add(new InfoList.Item("Securing crime scene"));
            }
          }
          else
          {
            PoliceEmergencyRequest component88;
            if (componentData45.m_RequestCount <= 0 || buffer60.Length <= 0 || !this.EntityManager.TryGetComponent<PoliceEmergencyRequest>(buffer60[0].m_Request, out component88))
              return;
            AccidentSite component89;
            if (this.EntityManager.TryGetComponent<AccidentSite>(component88.m_Site, out component89) && component89.m_Event != InfoList.Item.kNullEntity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component89.m_Event), component89.m_Event));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component88.m_Site), component88.m_Site));
            }
          }
        }
        else
          info.Add(new InfoList.Item("Patrolling"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<HouseholdMember>(entity) && this.EntityManager.HasComponent<Citizen>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Citizen componentData = this.EntityManager.GetComponentData<Citizen>(entity);
        Entity household = this.EntityManager.GetComponentData<HouseholdMember>(entity).m_Household;
        Household householdData = new Household();
        if (this.EntityManager.HasComponent<Household>(household))
        {
          householdData = this.EntityManager.GetComponentData<Household>(household);
          this.EntityManager.GetBuffer<HouseholdCitizen>(household, true);
        }
        // ISSUE: reference to a compiler-generated field
        EconomyParameterData singleton = this.__query_746694603_4.GetSingleton<EconomyParameterData>();
        // ISSUE: reference to a compiler-generated field
        this.__query_746694603_6.GetSingleton<CitizenHappinessParameterData>();
        bool tourist = (componentData.m_State & CitizenFlags.Tourist) != 0;
        bool flag = (componentData.m_State & CitizenFlags.Commuter) != 0;
        info.label = "Citizen";
        if (!flag)
        {
          Entity entity6 = InfoList.Item.kNullEntity;
          CurrentBuilding component90;
          if (this.EntityManager.TryGetComponent<CurrentBuilding>(entity, out component90))
            entity6 = component90.m_CurrentBuilding;
          info.Add(new InfoList.Item("Current Building: " + (object) entity6, entity6));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Household: " + this.m_NameSystem.GetDebugName(household), household));
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Wellbeing: " + DeveloperInfoUISystem.WellbeingToString((int) componentData.m_WellBeing) + "(" + componentData.m_WellBeing.ToString() + ")"));
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Health: " + DeveloperInfoUISystem.HealthToString((int) componentData.m_Health) + "(" + componentData.m_Health.ToString() + ")"));
          if (this.EntityManager.HasComponent<Game.Economy.Resources>(household))
          {
            int householdTotalWealth = EconomyUtils.GetHouseholdTotalWealth(householdData, this.EntityManager.GetBuffer<Game.Economy.Resources>(household, true));
            int resources = EconomyUtils.GetResources(Resource.Money, this.EntityManager.GetBuffer<Game.Economy.Resources>(household, true));
            info.Add(new InfoList.Item("Household total Wealth Value: " + householdTotalWealth.ToString()));
            info.Add(new InfoList.Item("Household Money: " + resources.ToString()));
            PropertyRenter component91;
            if (this.EntityManager.TryGetComponent<PropertyRenter>(household, out component91))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              BufferLookup<Renter> renterRoBufferLookup = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<ConsumptionData> roComponentLookup46 = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<PrefabRef> roComponentLookup47 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
              int householdSpendableMoney = EconomyUtils.GetHouseholdSpendableMoney(householdData, this.EntityManager.GetBuffer<Game.Economy.Resources>(household, true), ref renterRoBufferLookup, ref roComponentLookup46, ref roComponentLookup47, component91);
              info.Add(new InfoList.Item("Household spendable Money: " + householdSpendableMoney.ToString()));
            }
          }
        }
        CarKeeper component92;
        if (this.EntityManager.IsComponentEnabled<CarKeeper>(entity) && this.EntityManager.TryGetComponent<CarKeeper>(entity, out component92))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Car: " + this.m_NameSystem.GetDebugName(component92.m_Car), component92.m_Car));
        }
        if (!flag)
        {
          info.Add(new InfoList.Item("Household total Resources: " + householdData.m_Resources.ToString()));
          PropertyRenter component93;
          if (this.EntityManager.TryGetComponent<PropertyRenter>(household, out component93))
          {
            info.Add(new InfoList.Item("Property: " + component93.m_Property.ToString(), component93.m_Property));
            info.Add(new InfoList.Item("Rent: " + component93.m_Rent.ToString()));
          }
          else if (!tourist)
            info.Add(new InfoList.Item("Homeless"));
        }
        else
          info.Add(new InfoList.Item("From outside the city"));
        Criminal component94;
        bool component95 = this.EntityManager.TryGetComponent<Criminal>(entity, out component94);
        TravelPurpose component96;
        if (this.EntityManager.TryGetComponent<TravelPurpose>(entity, out component96))
        {
          Entity kNullEntity = InfoList.Item.kNullEntity;
          // ISSUE: reference to a compiler-generated method
          string purposeText = this.GetPurposeText(component96, tourist, component94, ref kNullEntity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(purposeText + " " + this.m_NameSystem.GetDebugName(kNullEntity), kNullEntity));
        }
        string str = " female";
        if ((componentData.m_State & CitizenFlags.Male) != CitizenFlags.None)
          str = " male";
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        info.Add(new InfoList.Item(this.GetAgeString(entity) + "(" + componentData.GetAgeInDays(this.m_SimulationSystem.frameIndex, TimeData.GetSingleton(this.m_TimeDataQuery)).ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")" + str));
        info.Add(new InfoList.Item("Leisure: " + componentData.m_LeisureCounter.ToString()));
        HealthProblem component97;
        if (this.EntityManager.TryGetComponent<HealthProblem>(entity, out component97))
        {
          if ((component97.m_Flags & HealthProblemFlags.Sick) != HealthProblemFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Sick " + this.m_NameSystem.GetDebugName(component97.m_Event), component97.m_Event));
          }
          else if ((component97.m_Flags & HealthProblemFlags.Injured) != HealthProblemFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Injured " + this.m_NameSystem.GetDebugName(component97.m_Event), component97.m_Event));
          }
          else if ((component97.m_Flags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Dead " + this.m_NameSystem.GetDebugName(component97.m_Event), component97.m_Event));
          }
          else if ((component97.m_Flags & HealthProblemFlags.Trapped) != HealthProblemFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Trapped " + this.m_NameSystem.GetDebugName(component97.m_Event), component97.m_Event));
          }
          else if ((component97.m_Flags & HealthProblemFlags.InDanger) != HealthProblemFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("In danger " + this.m_NameSystem.GetDebugName(component97.m_Event), component97.m_Event));
          }
        }
        if (component95)
        {
          string text = "Criminal";
          if ((component94.m_Flags & CriminalFlags.Robber) != (CriminalFlags) 0)
            text += " Robber ";
          if ((component94.m_Flags & CriminalFlags.Prisoner) != (CriminalFlags) 0)
            text = text + "Jail Time: " + ((uint) ((int) component94.m_JailTime * 16 * 16) / 262144U).ToString();
          info.Add(new InfoList.Item(text));
        }
        if (tourist)
        {
          info.Add(new InfoList.Item("Tourist"));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<TouristHousehold> roComponentLookup = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup;
          if (roComponentLookup.HasComponent(household) && roComponentLookup.HasComponent(household))
          {
            TouristHousehold touristHousehold = roComponentLookup[household];
            if (this.EntityManager.Exists(touristHousehold.m_Hotel))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Staying at: " + this.m_NameSystem.GetDebugName(touristHousehold.m_Hotel), touristHousehold.m_Hotel));
            }
          }
        }
        if (!tourist)
        {
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.GetEducationString(componentData.GetEducationLevel())));
          Citizen component98;
          if (this.EntityManager.TryGetComponent<Citizen>(entity, out component98))
          {
            CitizenAge age = component98.GetAge();
            Worker component99;
            if (this.EntityManager.TryGetComponent<Worker>(entity, out component99))
            {
              Entity workplace = component99.m_Workplace;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Works at: " + this.m_NameSystem.GetDebugName(workplace), workplace));
              // ISSUE: reference to a compiler-generated method
              float2 timeToWork1 = WorkerSystem.GetTimeToWork(componentData, component99, ref singleton, false);
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Work Shift: " + (this.GetTimeString(timeToWork1.x) + " to " + this.GetTimeString(timeToWork1.y))));
              // ISSUE: reference to a compiler-generated method
              float2 timeToWork2 = WorkerSystem.GetTimeToWork(componentData, component99, ref singleton, true);
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Work Shift(Commute): " + (this.GetTimeString(timeToWork2.x) + " to " + this.GetTimeString(timeToWork2.y))));
            }
            else
            {
              Game.Citizens.Student component100;
              if (this.EntityManager.TryGetComponent<Game.Citizens.Student>(entity, out component100))
              {
                Entity school = component100.m_School;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                info.Add(new InfoList.Item("Studies at: " + this.m_NameSystem.GetDebugName(school), school));
                // ISSUE: reference to a compiler-generated method
                float2 timeToStudy = StudentSystem.GetTimeToStudy(componentData, component100, ref singleton);
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                info.Add(new InfoList.Item("Study Time: " + (this.GetTimeString(timeToStudy.x) + " to " + this.GetTimeString(timeToStudy.y))));
              }
              else
              {
                switch (age)
                {
                  case CitizenAge.Child:
                  case CitizenAge.Teen:
                    info.Add(new InfoList.Item("Not in school!"));
                    break;
                  case CitizenAge.Adult:
                    info.Add(new InfoList.Item("Unemployed"));
                    break;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentLookup<Worker> roComponentLookup48 = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentLookup<Game.Citizens.Student> roComponentLookup49 = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup;
            // ISSUE: reference to a compiler-generated method
            float2 sleepTime = CitizenBehaviorSystem.GetSleepTime(entity, componentData, ref singleton, ref roComponentLookup48, ref roComponentLookup49);
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Sleep Time: " + (this.GetTimeString(sleepTime.x) + " to " + this.GetTimeString(sleepTime.y))));
          }
        }
        AttendingMeeting component101;
        CoordinatedMeeting component102;
        if (this.EntityManager.TryGetComponent<AttendingMeeting>(entity, out component101) && this.EntityManager.TryGetComponent<CoordinatedMeeting>(component101.m_Meeting, out component102))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          InfoList.Item obj = component102.m_Target != InfoList.Item.kNullEntity ? new InfoList.Item("Meeting at: " + this.m_NameSystem.GetDebugName(component102.m_Target), component102.m_Target) : new InfoList.Item("Planning a meeting");
          info.Add(obj);
        }
        AttendingEvent component103;
        if (!this.EntityManager.TryGetComponent<AttendingEvent>(household, out component103) || !this.EntityManager.HasComponent<Game.Events.CalendarEvent>(component103.m_Event))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Participating in " + this.m_NameSystem.GetDebugName(component103.m_Event)));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasEnabledComponent<MailSender>(entity)), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        MailSender componentData = this.EntityManager.GetComponentData<MailSender>(entity);
        info.label = "Mail sender";
        info.value = (int) componentData.m_Amount;
        info.max = 100;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<HouseholdPet>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        HouseholdPet componentData = this.EntityManager.GetComponentData<HouseholdPet>(entity);
        info.label = "Household";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(componentData.m_Household);
        info.target = componentData.m_Household;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        CurrentTransport component104;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component104))
          entity = component104.m_CurrentTransport;
        return this.EntityManager.HasComponent<Creature>(entity);
      }), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        info.label = "Creature";
        Citizen component105;
        CurrentTransport component106;
        if (!this.EntityManager.TryGetComponent<Citizen>(entity, out component105) || !this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component106))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Entity: " + this.m_NameSystem.GetDebugName(component106.m_CurrentTransport)));
        bool tourist = (component105.m_State & CitizenFlags.Tourist) != 0;
        Criminal component107;
        this.EntityManager.TryGetComponent<Criminal>(entity, out component107);
        Divert component108;
        if (this.EntityManager.TryGetComponent<Divert>(component106.m_CurrentTransport, out component108) && component108.m_Purpose != Game.Citizens.Purpose.None)
        {
          Entity kNullEntity = InfoList.Item.kNullEntity;
          // ISSUE: reference to a compiler-generated method
          string purposeText = this.GetPurposeText(new TravelPurpose()
          {
            m_Purpose = component108.m_Purpose,
            m_Resource = component108.m_Resource
          }, tourist, component107, ref kNullEntity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(purposeText + " " + this.m_NameSystem.GetDebugName(kNullEntity), kNullEntity));
        }
        RideNeeder component109;
        if (this.EntityManager.TryGetComponent<RideNeeder>(component106.m_CurrentTransport, out component109))
        {
          Dispatched component110;
          DynamicBuffer<ServiceDispatch> buffer62;
          if (this.EntityManager.TryGetComponent<Dispatched>(component109.m_RideRequest, out component110) && this.EntityManager.TryGetBuffer<ServiceDispatch>(component110.m_Handler, true, out buffer62) && buffer62.Length > 0 && buffer62[0].m_Request == component109.m_RideRequest)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Waiting for ride: " + this.m_NameSystem.GetDebugName(component110.m_Handler), component110.m_Handler));
          }
          else
            info.Add(new InfoList.Item("Taking a taxi"));
        }
        HumanCurrentLane component111;
        if (!this.EntityManager.TryGetComponent<HumanCurrentLane>(component106.m_CurrentTransport, out component111))
          return;
        Creature componentData = this.EntityManager.GetComponentData<Creature>(component106.m_CurrentTransport);
        if ((component111.m_Flags & CreatureLaneFlags.EndReached) != (CreatureLaneFlags) 0)
        {
          if ((component111.m_Flags & CreatureLaneFlags.Transport) == (CreatureLaneFlags) 0)
            return;
          PathOwner component112;
          DynamicBuffer<PathElement> buffer63;
          if (this.EntityManager.TryGetComponent<PathOwner>(component106.m_CurrentTransport, out component112) && this.EntityManager.TryGetBuffer<PathElement>(component106.m_CurrentTransport, true, out buffer63) && buffer63.Length > component112.m_ElementIndex)
          {
            Entity entity7 = buffer63[component112.m_ElementIndex].m_Target;
            Owner component113;
            if (this.EntityManager.TryGetComponent<Owner>(entity7, out component113))
              entity7 = component113.m_Owner;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Waiting for transport: " + this.m_NameSystem.GetDebugName(entity7), entity7));
          }
          else
            info.Add(new InfoList.Item("Waiting for transport"));
        }
        else
        {
          if ((double) componentData.m_QueueArea.radius <= 0.0)
            return;
          if (componentData.m_QueueEntity != Entity.Null)
          {
            Entity entity8 = componentData.m_QueueEntity;
            Owner component114;
            if (this.EntityManager.HasComponent<Waypoint>(entity8) && this.EntityManager.TryGetComponent<Owner>(entity8, out component114))
              entity8 = component114.m_Owner;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Queueing for: " + this.m_NameSystem.GetDebugName(entity8), entity8));
          }
          else
            info.Add(new InfoList.Item("Queueing"));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        CurrentTransport component115;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component115))
          entity = component115.m_CurrentTransport;
        return this.EntityManager.HasComponent<GroupCreature>(entity);
      }), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        CurrentTransport component116;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component116))
          entity = component116.m_CurrentTransport;
        DynamicBuffer<GroupCreature> buffer64 = this.EntityManager.GetBuffer<GroupCreature>(entity, true);
        info.label = string.Format("Group members ({0})", (object) buffer64.Length);
        for (int index = 0; index < buffer64.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer64[index].m_Creature), buffer64[index].m_Creature));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        CurrentTransport component117;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component117))
          entity = component117.m_CurrentTransport;
        return this.EntityManager.HasComponent<GroupMember>(entity);
      }), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        CurrentTransport component118;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component118))
          entity = component118.m_CurrentTransport;
        GroupMember componentData = this.EntityManager.GetComponentData<GroupMember>(entity);
        info.label = "Group leader";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(componentData.m_Leader);
        info.target = componentData.m_Leader;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Area>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        info.label = "Area Info";
        info.Add(new InfoList.Item("Nothing to see here, move along! (TBD)"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Game.Objects.Object>(entity) && this.EntityManager.HasComponent<Tree>(entity)), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        Tree componentData47 = this.EntityManager.GetComponentData<Tree>(entity);
        Plant componentData48 = this.EntityManager.GetComponentData<Plant>(entity);
        Damaged component119;
        this.EntityManager.TryGetComponent<Damaged>(entity, out component119);
        int num = 0;
        TreeData component120;
        if (this.EntityManager.TryGetComponent<TreeData>(prefab, out component120))
          num = Mathf.RoundToInt(ObjectUtils.CalculateWoodAmount(componentData47, componentData48, component119, component120));
        info.label = string.Format("Wood: {0}", (object) num);
        info.value = num;
        info.max = Mathf.RoundToInt(component120.m_WoodAmount);
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Game.Objects.Object>(entity) && this.EntityManager.HasComponent<Game.Routes.MailBox>(entity) && this.EntityManager.HasComponent<MailBoxData>(prefab)), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        Game.Routes.MailBox componentData49 = this.EntityManager.GetComponentData<Game.Routes.MailBox>(entity);
        MailBoxData componentData50 = this.EntityManager.GetComponentData<MailBoxData>(prefab);
        info.label = "Stored Mail in Mailbox";
        info.value = componentData49.m_MailAmount;
        info.max = componentData50.m_MailCapacity;
      })));
      BoardingVehicle component121;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.TryGetComponent<BoardingVehicle>(entity, out component121) && component121.m_Vehicle != InfoList.Item.kNullEntity), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        BoardingVehicle componentData = this.EntityManager.GetComponentData<BoardingVehicle>(entity);
        info.label = "Boarding";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(componentData.m_Vehicle);
        info.target = componentData.m_Vehicle;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<WaitingPassengers>(entity) || this.EntityManager.HasBuffer<ConnectedRoute>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        WaitingPassengers component122;
        this.EntityManager.TryGetComponent<WaitingPassengers>(entity, out component122);
        DynamicBuffer<ConnectedRoute> buffer65;
        if (this.EntityManager.TryGetBuffer<ConnectedRoute>(entity, true, out buffer65))
        {
          int num21 = 0;
          for (int index = 0; index < buffer65.Length; ++index)
          {
            WaitingPassengers component123;
            if (this.EntityManager.TryGetComponent<WaitingPassengers>(buffer65[index].m_Waypoint, out component123))
            {
              component122.m_Count += component123.m_Count;
              num21 += (int) component123.m_AverageWaitingTime;
            }
          }
          int num22 = num21 / math.max(1, buffer65.Length);
          int num23 = num22 - num22 % 5;
          component122.m_AverageWaitingTime = (ushort) num23;
        }
        info.label = "Waiting passengers";
        info.Add(new InfoList.Item("Passenger count: " + (object) component122.m_Count));
        info.Add(new InfoList.Item("Waiting time: " + (component122.m_AverageWaitingTime.ToString() + "s")));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) =>
      {
        CurrentTransport component124;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component124))
          entity = component124.m_CurrentTransport;
        return this.EntityManager.HasComponent<Moving>(entity);
      }), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        CurrentTransport component125;
        if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component125))
        {
          entity = component125.m_CurrentTransport;
          prefab = this.EntityManager.GetComponentData<PrefabRef>(entity).m_Prefab;
        }
        int num24 = Mathf.RoundToInt(math.length(this.EntityManager.GetComponentData<Moving>(entity).m_Velocity) * 3.6f);
        int num25 = Mathf.RoundToInt(999.999939f);
        CarData component126;
        if (this.EntityManager.TryGetComponent<CarData>(prefab, out component126))
        {
          num25 = Mathf.RoundToInt(component126.m_MaxSpeed * 3.6f);
        }
        else
        {
          WatercraftData component127;
          if (this.EntityManager.TryGetComponent<WatercraftData>(prefab, out component127))
          {
            num25 = Mathf.RoundToInt(component127.m_MaxSpeed * 3.6f);
          }
          else
          {
            AirplaneData component128;
            if (this.EntityManager.TryGetComponent<AirplaneData>(prefab, out component128))
            {
              num25 = Mathf.RoundToInt(component128.m_FlyingSpeed.y * 3.6f);
            }
            else
            {
              HelicopterData component129;
              if (this.EntityManager.TryGetComponent<HelicopterData>(prefab, out component129))
              {
                num25 = Mathf.RoundToInt(component129.m_FlyingMaxSpeed * 3.6f);
              }
              else
              {
                TrainData component130;
                if (this.EntityManager.TryGetComponent<TrainData>(prefab, out component130))
                {
                  num25 = Mathf.RoundToInt(component130.m_MaxSpeed * 3.6f);
                }
                else
                {
                  HumanData component131;
                  if (this.EntityManager.TryGetComponent<HumanData>(prefab, out component131))
                  {
                    num25 = Mathf.RoundToInt(component131.m_RunSpeed * 3.6f);
                  }
                  else
                  {
                    AnimalData component132;
                    if (this.EntityManager.TryGetComponent<AnimalData>(prefab, out component132))
                      num25 = Mathf.RoundToInt(component132.m_MoveSpeed * 3.6f);
                  }
                }
              }
            }
          }
        }
        info.label = string.Format("Moving: {0} km/h", (object) num24);
        info.value = num24;
        info.max = num25;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Damaged>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Damaged componentData = this.EntityManager.GetComponentData<Damaged>(entity);
        float4 a1 = math.clamp(new float4(componentData.m_Damage, ObjectUtils.GetTotalDamage(componentData)) * 100f, (float4) 0.0f, (float4) 100f);
        float4 a2 = math.select(a1, (float4) 1f, a1 > 0.0f & a1 < 1f);
        float4 float4 = math.select(a2, (float4) 99f, a2 > 99f & a2 < 100f);
        info.label = "Damaged";
        info.Add(new InfoList.Item("Physical: " + (object) Mathf.RoundToInt(float4.x) + "%"));
        info.Add(new InfoList.Item("Fire: " + (object) Mathf.RoundToInt(float4.y) + "%"));
        info.Add(new InfoList.Item("Water: " + (object) Mathf.RoundToInt(float4.z) + "%"));
        info.Add(new InfoList.Item("Total: " + (object) Mathf.RoundToInt(float4.w) + "%"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Destroyed>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        Destroyed componentData = this.EntityManager.GetComponentData<Destroyed>(entity);
        info.label = componentData.m_Event == InfoList.Item.kNullEntity ? "Destroyed" : "Destroyed By";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = componentData.m_Event == InfoList.Item.kNullEntity ? string.Empty : this.m_NameSystem.GetDebugName(componentData.m_Event);
        info.target = componentData.m_Event;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new CapacityInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Destroyed>(entity) && this.EntityManager.HasComponent<Building>(entity)), (Action<Entity, Entity, CapacityInfo>) ((entity, prefab, info) =>
      {
        Destroyed componentData = this.EntityManager.GetComponentData<Destroyed>(entity);
        info.label = string.Format("Searching for survivors: {0}%)", (object) Mathf.RoundToInt(componentData.m_Cleared * 100f));
        info.value = Mathf.RoundToInt(componentData.m_Cleared * 100f);
        info.max = 100;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<OnFire>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        OnFire componentData = this.EntityManager.GetComponentData<OnFire>(entity);
        info.label = "On fire";
        if (componentData.m_Event != InfoList.Item.kNullEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Ignited by: " + this.m_NameSystem.GetDebugName(componentData.m_Event), componentData.m_Event));
        }
        info.Add(new InfoList.Item("Intensity: " + (object) Mathf.RoundToInt(componentData.m_Intensity) + "%"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<FacingWeather>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        FacingWeather componentData = this.EntityManager.GetComponentData<FacingWeather>(entity);
        info.label = componentData.m_Event == InfoList.Item.kNullEntity ? "Suffering from weather" : "Weather phenomenon";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = componentData.m_Event == InfoList.Item.kNullEntity ? string.Empty : this.m_NameSystem.GetDebugName(componentData.m_Event);
        info.target = componentData.m_Event;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<AccidentSite>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        AccidentSite componentData = this.EntityManager.GetComponentData<AccidentSite>(entity);
        info.label = componentData.m_Event == InfoList.Item.kNullEntity ? "Accident site" : "Incident";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = componentData.m_Event == InfoList.Item.kNullEntity ? string.Empty : this.m_NameSystem.GetDebugName(componentData.m_Event);
        info.target = componentData.m_Event;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new GenericInfo((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<InvolvedInAccident>(entity)), (Action<Entity, Entity, GenericInfo>) ((entity, prefab, info) =>
      {
        InvolvedInAccident componentData = this.EntityManager.GetComponentData<InvolvedInAccident>(entity);
        info.label = componentData.m_Event == InfoList.Item.kNullEntity ? "Involved in accident" : "Involved in";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = componentData.m_Event == InfoList.Item.kNullEntity ? string.Empty : this.m_NameSystem.GetDebugName(componentData.m_Event);
        info.target = componentData.m_Event;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Flooded>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        Flooded componentData = this.EntityManager.GetComponentData<Flooded>(entity);
        info.label = "Flooded";
        if (componentData.m_Event != InfoList.Item.kNullEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Caused by: " + this.m_NameSystem.GetDebugName(componentData.m_Event), componentData.m_Event));
        }
        info.Add(new InfoList.Item("Depth: " + (object) Mathf.RoundToInt(componentData.m_Depth) + "m"));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Game.Events.Event>(entity) && this.EntityManager.HasComponent<TargetElement>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        DynamicBuffer<TargetElement> buffer66 = this.EntityManager.GetBuffer<TargetElement>(entity, true);
        info.label = string.Format("Affected Objects: {0})", (object) buffer66.Length);
        for (int index = 0; index < buffer66.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer66[index].m_Entity), buffer66[index].m_Entity));
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<Icon>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NotificationIconPrefab prefab6 = this.m_PrefabSystem.GetPrefab<NotificationIconPrefab>(prefab);
        info.label = "Notification Info";
        Owner component133;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(this.EntityManager.TryGetComponent<Owner>(entity, out component133) ? new InfoList.Item(prefab6.m_Description + this.m_NameSystem.GetDebugName(component133.m_Owner), component133.m_Owner) : new InfoList.Item(prefab6.m_Description));
        Game.Common.Target component134;
        if (!this.EntityManager.TryGetComponent<Game.Common.Target>(entity, out component134))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(prefab6.m_TargetDescription + this.m_NameSystem.GetDebugName(component134.m_Target), component134.m_Target));
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.AddDeveloperInfo((ISubsectionSource) new InfoList((Func<Entity, Entity, bool>) ((entity, prefab) => this.EntityManager.HasComponent<VehicleModel>(entity)), (Action<Entity, Entity, InfoList>) ((entity, prefab, info) =>
      {
        VehicleModel componentData = this.EntityManager.GetComponentData<VehicleModel>(entity);
        info.label = "Vehicle Model";
        if (this.EntityManager.HasComponent<PrefabData>(componentData.m_PrimaryPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_PrefabSystem.GetPrefabName(componentData.m_PrimaryPrefab)));
        }
        if (!this.EntityManager.HasComponent<PrefabData>(componentData.m_SecondaryPrefab))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_PrefabSystem.GetPrefabName(componentData.m_SecondaryPrefab)));
      })));
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    protected void AddUpgradeData<T>(Entity entity, ref T data) where T : unmanaged, IComponentData, ICombineData<T>
    {
      DynamicBuffer<InstalledUpgrade> buffer;
      if (!this.EntityManager.TryGetBuffer<InstalledUpgrade>(entity, true, out buffer))
        return;
      UpgradeUtils.CombineStats<T>(this.EntityManager, ref data, buffer);
    }

    private bool HasEntityInfo(Entity entity, Entity prefab) => entity != InfoList.Item.kNullEntity;

    private void UpdateEntityInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      info.label = "Entity";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(entity);
    }

    private bool HasMeshPrefabInfo(Entity entity, Entity prefab)
    {
      if (prefab != InfoList.Item.kNullEntity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ObjectGeometryPrefab prefab1 = this.m_PrefabSystem.GetPrefab<ObjectGeometryPrefab>(prefab);
        if ((UnityEngine.Object) prefab1 != (UnityEngine.Object) null)
          return prefab1.m_Meshes.Length != 0;
      }
      return false;
    }

    private void UpdateMeshPrefabInfo(Entity entity, Entity prefab, InfoList info)
    {
      info.label = "MeshPrefabs";
      SubMeshFlags subMeshFlags = (SubMeshFlags) 0;
      Tree component1;
      if (this.EntityManager.TryGetComponent<Tree>(entity, out component1))
      {
        GrowthScaleData component2;
        if (this.EntityManager.TryGetComponent<GrowthScaleData>(prefab, out component2))
          subMeshFlags |= BatchDataHelpers.CalculateTreeSubMeshData(component1, component2, out float3 _);
        else
          subMeshFlags |= SubMeshFlags.RequireAdult;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ObjectGeometryPrefab prefab1 = this.m_PrefabSystem.GetPrefab<ObjectGeometryPrefab>(prefab);
      for (int index = 0; index < prefab1.m_Meshes.Length; ++index)
      {
        ObjectState requireState = prefab1.m_Meshes[index].m_RequireState;
        string name = prefab1.m_Meshes[index].m_Mesh.name;
        if (requireState == ObjectState.Child && (subMeshFlags & SubMeshFlags.RequireChild) == SubMeshFlags.RequireChild || requireState == ObjectState.Teen && (subMeshFlags & SubMeshFlags.RequireTeen) == SubMeshFlags.RequireTeen || requireState == ObjectState.Adult && (subMeshFlags & SubMeshFlags.RequireAdult) == SubMeshFlags.RequireAdult || requireState == ObjectState.Elderly && (subMeshFlags & SubMeshFlags.RequireElderly) == SubMeshFlags.RequireElderly || requireState == ObjectState.Dead && (subMeshFlags & SubMeshFlags.RequireDead) == SubMeshFlags.RequireDead || requireState == ObjectState.Stump && (subMeshFlags & SubMeshFlags.RequireStump) == SubMeshFlags.RequireStump)
          name += " [X]";
        info.Add(new InfoList.Item(name, InfoList.Item.kNullEntity));
      }
    }

    private bool HasMeshGroupInfo(Entity entity, Entity prefab)
    {
      if (this.EntityManager.HasComponent<MeshGroup>(entity))
        return true;
      CurrentTransport component;
      return this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component) && this.EntityManager.HasComponent<MeshGroup>(component.m_CurrentTransport);
    }

    private void UpdateMeshGroupInfo(Entity entity, Entity prefab, InfoList info)
    {
      CurrentTransport component1;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component1))
      {
        entity = component1.m_CurrentTransport;
        PrefabRef component2;
        if (this.EntityManager.TryGetComponent<PrefabRef>(entity, out component2))
          prefab = component2.m_Prefab;
      }
      DynamicBuffer<MeshGroup> buffer;
      ObjectGeometryPrefab prefab1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.EntityManager.TryGetBuffer<MeshGroup>(entity, true, out buffer) || !this.m_PrefabSystem.TryGetPrefab<ObjectGeometryPrefab>(prefab, out prefab1) || prefab1.m_Meshes == null)
        return;
      CreatureData component3;
      this.EntityManager.TryGetComponent<CreatureData>(prefab, out component3);
      info.label = string.Format("Mesh groups ({0})", (object) buffer.Length);
label_25:
      for (int index1 = 0; index1 < buffer.Length; ++index1)
      {
        int subMeshGroup = (int) buffer[index1].m_SubMeshGroup;
        for (int index2 = 0; index2 < prefab1.m_Meshes.Length; ++index2)
        {
          if (prefab1.m_Meshes[index2].m_Mesh is CharacterGroup mesh && mesh.m_Characters != null)
          {
            for (int index3 = 0; index3 < mesh.m_Characters.Length; ++index3)
            {
              if ((mesh.m_Characters[index3].m_Style.m_Gender & component3.m_Gender) == component3.m_Gender && subMeshGroup-- == 0)
              {
                info.Add(new InfoList.Item(string.Format("{0} #{1}", (object) mesh.name, (object) index3)));
                goto label_25;
              }
            }
            if (mesh.m_Overrides != null)
            {
              for (int index4 = 0; index4 < mesh.m_Overrides.Length; ++index4)
              {
                CharacterGroup.OverrideInfo overrideInfo = mesh.m_Overrides[index4];
                for (int index5 = 0; index5 < mesh.m_Characters.Length; ++index5)
                {
                  if ((mesh.m_Characters[index5].m_Style.m_Gender & component3.m_Gender) == component3.m_Gender && subMeshGroup-- == 0)
                  {
                    info.Add(new InfoList.Item(string.Format("{0} #{1} ({2})", (object) mesh.name, (object) index5, (object) overrideInfo.m_Group.name)));
                    goto label_25;
                  }
                }
              }
            }
          }
        }
        info.Add(new InfoList.Item(string.Format("Unknown group #{0}", (object) buffer[index1].m_SubMeshGroup)));
      }
    }

    private bool HasBatchInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<MeshBatch>(entity) || this.EntityManager.HasComponent<CurrentTransport>(entity);
    }

    private void UpdateBatchInfo(Entity entity, Entity prefab, InfoList info)
    {
      CurrentTransport component;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component))
        entity = component.m_CurrentTransport;
      DynamicBuffer<MeshBatch> buffer;
      if (!this.EntityManager.TryGetBuffer<MeshBatch>(entity, true, out buffer))
        return;
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, Game.Rendering.GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, Game.Rendering.GroupData, BatchData, InstanceData> nativeBatchInstances = this.m_BatchManagerSystem.GetNativeBatchInstances(true, out dependencies2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies1.Complete();
      dependencies2.Complete();
      int num1 = 0;
      for (int index = 0; index < buffer.Length; ++index)
      {
        MeshBatch meshBatch = buffer[index];
        num1 += nativeBatchGroups.GetBatchCount(meshBatch.m_GroupIndex);
      }
      info.label = string.Format("Batches ({0})", (object) num1);
      for (int index = 0; index < buffer.Length; ++index)
      {
        MeshBatch meshBatch = buffer[index];
        Game.Rendering.GroupData groupData = nativeBatchGroups.GetGroupData(meshBatch.m_GroupIndex);
        int batchCount = nativeBatchGroups.GetBatchCount(meshBatch.m_GroupIndex);
        int instanceGroupIndex = nativeBatchInstances.GetMergedInstanceGroupIndex(meshBatch.m_GroupIndex, meshBatch.m_InstanceIndex);
        int num2 = -1;
        for (int batchIndex1 = 0; batchIndex1 < batchCount; ++batchIndex1)
        {
          BatchData batchData = nativeBatchGroups.GetBatchData(meshBatch.m_GroupIndex, batchIndex1);
          int managedBatchIndex = nativeBatchGroups.GetManagedBatchIndex(meshBatch.m_GroupIndex, batchIndex1);
          int batchIndex2 = -1;
          if (instanceGroupIndex >= 0)
            batchIndex2 = nativeBatchGroups.GetManagedBatchIndex(instanceGroupIndex, batchIndex1);
          if ((int) batchData.m_LodIndex != num2)
          {
            num2 = (int) batchData.m_LodIndex;
            info.Add(new InfoList.Item(string.Format("--- Mesh {0}, Tile {1}, Layer {2}, Lod {3} ---", (object) meshBatch.m_MeshIndex, (object) meshBatch.m_TileIndex, (object) groupData.m_Layer, (object) batchData.m_LodIndex)));
          }
          if (managedBatchIndex >= 0)
          {
            CustomBatch batch1 = (CustomBatch) managedBatches.GetBatch(managedBatchIndex);
            if (batchIndex2 >= 0)
            {
              CustomBatch batch2 = (CustomBatch) managedBatches.GetBatch(batchIndex2);
              RenderPrefab prefab1;
              RenderPrefab prefab2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(batch1.sourceMeshEntity, out prefab1) && this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(batch2.sourceMeshEntity, out prefab2))
              {
                if (batch1.generatedType != GeneratedType.None)
                  info.Add(new InfoList.Item(string.Format("{0} {1} -> {2} {3}", (object) prefab2.name, (object) batch2.generatedType, (object) prefab1.name, (object) batch1.generatedType)));
                else
                  info.Add(new InfoList.Item(string.Format("{0}[{1}] -> {2}[{3}]", (object) prefab2.name, (object) batch2.sourceSubMeshIndex, (object) prefab1.name, (object) batch1.sourceSubMeshIndex)));
              }
              else
                info.Add(new InfoList.Item(batch2.mesh.name + " -> " + batch1.mesh.name));
            }
            else
            {
              RenderPrefab prefab3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_PrefabSystem.TryGetPrefab<RenderPrefab>(batch1.sourceMeshEntity, out prefab3))
              {
                if (batch1.generatedType != GeneratedType.None)
                  info.Add(new InfoList.Item(string.Format("{0} {1}", (object) prefab3.name, (object) batch1.generatedType)));
                else
                  info.Add(new InfoList.Item(string.Format("{0}[{1}]", (object) prefab3.name, (object) batch1.sourceSubMeshIndex)));
              }
              else
                info.Add(new InfoList.Item(batch1.mesh.name));
            }
          }
        }
      }
    }

    private bool HasAddressInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Building>(entity) || this.EntityManager.HasComponent<Attached>(entity);
    }

    private void UpdateAddressInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      info.label = "Address";
      Entity road;
      int number;
      if (BuildingUtils.GetAddress(this.EntityManager, entity, out road, out number))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.value = this.m_NameSystem.GetDebugName(road) + " " + (object) number;
        info.target = road;
      }
      else
      {
        info.value = "Unknown";
        info.target = InfoList.Item.kNullEntity;
      }
    }

    private bool HasHomelessInfo(Entity entity, Entity prefab)
    {
      if (!this.EntityManager.HasComponent<Building>(entity) || !this.EntityManager.HasComponent<Renter>(entity))
        return false;
      return this.EntityManager.HasComponent<Game.Buildings.Park>(entity) || this.EntityManager.HasComponent<Abandoned>(entity);
    }

    private void UpdateHomelessInfo(Entity entity, Entity prefab, InfoList info)
    {
      info.label = "Homeless";
      DynamicBuffer<Renter> buffer = this.EntityManager.GetBuffer<Renter>(entity, true);
      info.label = string.Format("Homeless Count: ({0})", (object) buffer.Length);
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity renter = buffer[index].m_Renter;
        if (this.EntityManager.HasComponent<Household>(renter))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(renter), renter));
        }
      }
    }

    private bool HasTelecomRangeInfo(Entity entity, Entity prefab)
    {
      TelecomFacilityData component;
      if (!this.EntityManager.HasComponent<Building>(entity) || !this.EntityManager.HasComponent<Game.Buildings.TelecomFacility>(entity) || !this.EntityManager.TryGetComponent<TelecomFacilityData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<TelecomFacilityData>(entity, ref component);
      return (double) component.m_Range >= 1.0;
    }

    private void UpdateTelecomRangeInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      TelecomFacilityData componentData = this.EntityManager.GetComponentData<TelecomFacilityData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<TelecomFacilityData>(entity, ref componentData);
      float x = 1f;
      DynamicBuffer<Efficiency> buffer;
      if (this.EntityManager.TryGetBuffer<Efficiency>(entity, true, out buffer))
        x = BuildingUtils.GetEfficiency(buffer);
      float f = componentData.m_Range * math.sqrt(x);
      info.label = "Range";
      info.value = Mathf.RoundToInt(f).ToString() + "/" + (object) Mathf.RoundToInt(componentData.m_Range);
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasNetworkCapacityInfo(Entity entity, Entity prefab)
    {
      TelecomFacilityData component;
      if (!this.EntityManager.HasComponent<Building>(entity) || !this.EntityManager.HasComponent<Game.Buildings.TelecomFacility>(entity) || !this.EntityManager.TryGetComponent<TelecomFacilityData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<TelecomFacilityData>(entity, ref component);
      return (double) component.m_NetworkCapacity >= 1.0;
    }

    private void UpdateNetworkCapacityInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      TelecomFacilityData componentData = this.EntityManager.GetComponentData<TelecomFacilityData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<TelecomFacilityData>(entity, ref componentData);
      float num = 1f;
      DynamicBuffer<Efficiency> buffer1;
      if (this.EntityManager.TryGetBuffer<Efficiency>(entity, true, out buffer1))
        num = BuildingUtils.GetEfficiency(buffer1);
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<CityModifier> buffer2 = this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true);
      CityUtils.ApplyModifier(ref componentData.m_NetworkCapacity, buffer2, CityModifierType.TelecomCapacity);
      float f = componentData.m_NetworkCapacity * num;
      info.label = "Network Capacity";
      info.value = Mathf.RoundToInt(f);
      info.max = info.value;
    }

    private bool HasZoneInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Building>(entity) && this.EntityManager.HasComponent<Renter>(entity) ? this.EntityManager.HasComponent<SpawnableBuildingData>(prefab) && this.EntityManager.HasComponent<Game.Prefabs.BuildingData>(prefab) : (this.EntityManager.HasComponent<Household>(entity) || this.EntityManager.HasComponent<CompanyData>(entity)) && this.EntityManager.HasComponent<PropertyRenter>(entity) && this.EntityManager.HasComponent<SpawnableBuildingData>(this.EntityManager.GetComponentData<PrefabRef>(this.EntityManager.GetComponentData<PropertyRenter>(entity).m_Property).m_Prefab);
    }

    private void UpdateZoneInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      if (!this.EntityManager.HasComponent<Building>(entity))
      {
        entity = this.EntityManager.GetComponentData<PropertyRenter>(entity).m_Property;
        prefab = this.EntityManager.GetComponentData<PrefabRef>(entity).m_Prefab;
      }
      SpawnableBuildingData componentData1 = this.EntityManager.GetComponentData<SpawnableBuildingData>(prefab);
      Game.Prefabs.BuildingData componentData2 = this.EntityManager.GetComponentData<Game.Prefabs.BuildingData>(prefab);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      string prefabName = this.m_PrefabSystem.GetPrefabName(componentData1.m_ZonePrefab);
      info.label = "Zone Info";
      info.value = prefabName + " " + (object) componentData2.m_LotSize.x + "x" + (object) componentData2.m_LotSize.y;
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasZoneHappinessInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Building>(entity) && this.EntityManager.HasComponent<SpawnableBuildingData>(prefab) && this.EntityManager.HasComponent<Game.Prefabs.BuildingData>(prefab);
    }

    private void UpdateZoneHappinessInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ProcessQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<SpawnableBuildingData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<BuildingPropertyData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ConsumptionData> roComponentLookup4 = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<CityModifier> modifierRoBufferLookup = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Building> roComponentLookup5 = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ElectricityConsumer> roComponentLookup6 = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<WaterConsumer> roComponentLookup7 = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Game.Net.ServiceCoverage> coverageRoBufferLookup = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Locked> roComponentLookup8 = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Game.Objects.Transform> roComponentLookup9 = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<GarbageProducer> roComponentLookup10 = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<CrimeProducer> roComponentLookup11 = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<MailProducer> roComponentLookup12 = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<OfficeBuilding> roComponentLookup13 = this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Renter> renterRoBufferLookup = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Citizen> roComponentLookup14 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<HouseholdCitizen> citizenRoBufferLookup = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Game.Prefabs.BuildingData> roComponentLookup15 = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<CompanyData> roComponentLookup16 = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<IndustrialProcessData> roComponentLookup17 = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<WorkProvider> roComponentLookup18 = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Employee> employeeRoBufferLookup = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<WorkplaceData> roComponentLookup19 = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Citizen> roComponentLookup20 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<HealthProblem> roComponentLookup21 = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ServiceAvailable> roComponentLookup22 = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup23 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ZonePropertiesData> roComponentLookup24 = this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Efficiency> efficiencyRoBufferLookup = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ServiceCompanyData> roComponentLookup25 = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<ResourceAvailability> availabilityRoBufferLookup = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<TradeCost> costRoBufferLookup = this.__TypeHandle.__Game_Companies_TradeCost_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      CitizenHappinessParameterData singleton1 = this.m_CitizenHappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>();
      // ISSUE: reference to a compiler-generated field
      HealthcareParameterData singleton2 = this.m_HealthcareParameterQuery.GetSingleton<HealthcareParameterData>();
      // ISSUE: reference to a compiler-generated field
      ParkParameterData singleton3 = this.m_ParkParameterQuery.GetSingleton<ParkParameterData>();
      // ISSUE: reference to a compiler-generated field
      EducationParameterData singleton4 = this.m_EducationParameterQuery.GetSingleton<EducationParameterData>();
      // ISSUE: reference to a compiler-generated field
      TelecomParameterData singleton5 = this.m_TelecomParameterQuery.GetSingleton<TelecomParameterData>();
      this.CheckedStateRef.EntityManager.CompleteDependencyBeforeRW<HappinessFactorParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<HappinessFactorParameterData> happinessFactorParameters = this.__TypeHandle.__Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup[this.m_HappinessFactorParameterQuery.GetSingletonEntity()];
      // ISSUE: reference to a compiler-generated field
      GarbageParameterData singleton6 = this.m_GarbageParameterQuery.GetSingleton<GarbageParameterData>();
      // ISSUE: reference to a compiler-generated field
      EconomyParameterData singleton7 = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>();
      // ISSUE: reference to a compiler-generated field
      ServiceFeeParameterData singleton8 = this.__query_746694603_0.GetSingleton<ServiceFeeParameterData>();
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      NativeArray<GroundPollution> buffer1 = this.m_GroundPollutionSystem.GetData(true, out dependencies1).m_Buffer;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      NativeArray<NoisePollution> buffer2 = this.m_NoisePollutionSystem.GetData(true, out dependencies2).m_Buffer;
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      NativeArray<AirPollution> buffer3 = this.m_AirPollutionSystem.GetData(true, out dependencies3).m_Buffer;
      JobHandle dependencies4;
      // ISSUE: reference to a compiler-generated field
      CellMapData<TelecomCoverage> data = this.m_TelecomCoverageSystem.GetData(true, out dependencies4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> taxRates = this.m_TaxSystem.GetTaxRates();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<ServiceFee> buffer4 = this.EntityManager.GetBuffer<ServiceFee>(this.m_CitySystem.City);
      // ISSUE: reference to a compiler-generated method
      float relativeElectricityFee = ServiceFeeSystem.GetFee(PlayerResource.Electricity, buffer4) / singleton8.m_ElectricityFee.m_Default;
      // ISSUE: reference to a compiler-generated method
      float relativeWaterFee = ServiceFeeSystem.GetFee(PlayerResource.Water, buffer4) / singleton8.m_WaterFee.m_Default;
      NativeArray<int> factors = new NativeArray<int>(28, Allocator.Temp);
      dependencies1.Complete();
      dependencies2.Complete();
      dependencies3.Complete();
      dependencies4.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      CitizenHappinessSystem.GetBuildingHappinessFactors(entity, factors, ref roComponentLookup1, ref roComponentLookup2, ref roComponentLookup3, ref roComponentLookup4, ref modifierRoBufferLookup, ref roComponentLookup5, ref roComponentLookup6, ref roComponentLookup7, ref coverageRoBufferLookup, ref roComponentLookup8, ref roComponentLookup9, ref roComponentLookup10, ref roComponentLookup11, ref roComponentLookup12, ref roComponentLookup13, ref renterRoBufferLookup, ref roComponentLookup14, ref citizenRoBufferLookup, ref roComponentLookup15, ref roComponentLookup16, ref roComponentLookup17, ref roComponentLookup18, ref employeeRoBufferLookup, ref roComponentLookup19, ref roComponentLookup20, ref roComponentLookup21, ref roComponentLookup22, ref roComponentLookup23, ref roComponentLookup24, ref efficiencyRoBufferLookup, ref roComponentLookup25, ref availabilityRoBufferLookup, ref costRoBufferLookup, singleton1, singleton6, singleton2, singleton3, singleton4, singleton5, ref singleton7, happinessFactorParameters, buffer1, buffer2, buffer3, data, this.m_CitySystem.City, taxRates, entityArray, prefabs, relativeElectricityFee, relativeWaterFee);
      entityArray.Dispose();
      NativeList<DeveloperInfoUISystem.BuildingHappinessFactorValue> list = new NativeList<DeveloperInfoUISystem.BuildingHappinessFactorValue>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: variable of a compiler-generated type
      DeveloperInfoUISystem.BuildingHappinessFactorValue happinessFactorValue;
      for (int index = 0; index < factors.Length; ++index)
      {
        if (factors[index] != 0)
        {
          ref NativeList<DeveloperInfoUISystem.BuildingHappinessFactorValue> local1 = ref list;
          // ISSUE: object of a compiler-generated type is created
          happinessFactorValue = new DeveloperInfoUISystem.BuildingHappinessFactorValue();
          // ISSUE: reference to a compiler-generated field
          happinessFactorValue.m_Factor = (BuildingHappinessFactor) index;
          // ISSUE: reference to a compiler-generated field
          happinessFactorValue.m_Value = factors[index];
          ref DeveloperInfoUISystem.BuildingHappinessFactorValue local2 = ref happinessFactorValue;
          local1.Add(in local2);
        }
      }
      list.Sort<DeveloperInfoUISystem.BuildingHappinessFactorValue>();
      string str = "";
      for (int index = 0; index < math.min(10, list.Length); ++index)
      {
        object[] objArray = new object[5]
        {
          (object) str,
          null,
          null,
          null,
          null
        };
        happinessFactorValue = list[index];
        // ISSUE: reference to a compiler-generated field
        objArray[1] = (object) happinessFactorValue.m_Factor.ToString();
        objArray[2] = (object) ": ";
        // ISSUE: reference to a compiler-generated field
        objArray[3] = (object) list[index].m_Value;
        objArray[4] = (object) "  ";
        str = string.Concat(objArray);
      }
      info.label = "Building happiness factors";
      info.value = str;
      info.target = InfoList.Item.kNullEntity;
      factors.Dispose();
      list.Dispose();
    }

    private bool HasZoneLevelInfo(Entity entity, Entity prefab)
    {
      SpawnableBuildingData component;
      return this.EntityManager.TryGetComponent<SpawnableBuildingData>(prefab, out component) && this.EntityManager.HasComponent<ZoneData>(component.m_ZonePrefab) && this.EntityManager.HasComponent<BuildingPropertyData>(prefab) && this.EntityManager.HasComponent<BuildingCondition>(entity);
    }

    private void UpdateZoneLevelInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      SpawnableBuildingData componentData1 = this.EntityManager.GetComponentData<SpawnableBuildingData>(prefab);
      ZoneData componentData2 = this.EntityManager.GetComponentData<ZoneData>(componentData1.m_ZonePrefab);
      BuildingPropertyData componentData3 = this.EntityManager.GetComponentData<BuildingPropertyData>(prefab);
      info.label = string.Format("Level Progression (level {0})", (object) componentData1.m_Level);
      info.value = this.EntityManager.GetComponentData<BuildingCondition>(entity).m_Condition;
      // ISSUE: reference to a compiler-generated field
      info.max = BuildingUtils.GetLevelingCost(componentData2.m_AreaType, componentData3, (int) componentData1.m_Level, this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true));
    }

    private bool HasPollutionInfo(Entity entity, Entity prefab)
    {
      if (!this.EntityManager.HasComponent<Building>(entity))
        return false;
      return this.EntityManager.HasComponent<PollutionData>(prefab) || this.EntityManager.HasComponent<Abandoned>(entity) || this.EntityManager.HasComponent<Destroyed>(entity) || this.EntityManager.HasComponent<Game.Buildings.Park>(entity);
    }

    private void UpdatePollutionInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      this.CompleteDependency();
      bool destroyed = this.EntityManager.HasComponent<Destroyed>(entity);
      bool abandoned = this.EntityManager.HasComponent<Abandoned>(entity);
      bool isPark = this.EntityManager.HasComponent<Game.Buildings.Park>(entity);
      DynamicBuffer<Efficiency> buffer1;
      float efficiency = this.EntityManager.TryGetBuffer<Efficiency>(entity, true, out buffer1) ? BuildingUtils.GetEfficiency(buffer1) : 1f;
      DynamicBuffer<Renter> buffer2;
      this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer2);
      DynamicBuffer<InstalledUpgrade> buffer3;
      this.EntityManager.TryGetBuffer<InstalledUpgrade>(entity, true, out buffer3);
      // ISSUE: reference to a compiler-generated field
      PollutionParameterData singleton = this.__query_746694603_1.GetSingleton<PollutionParameterData>();
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<CityModifier> singletonBuffer = this.__query_746694603_2.GetSingletonBuffer<CityModifier>(true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Game.Prefabs.BuildingData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<SpawnableBuildingData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PollutionData> roComponentLookup4 = this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PollutionModifierData> roComponentLookup5 = this.__TypeHandle.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ZoneData> roComponentLookup6 = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Employee> employeeRoBufferLookup = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<HouseholdCitizen> citizenRoBufferLookup = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Citizen> roComponentLookup7 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated method
      PollutionData buildingPollution = BuildingPollutionAddSystem.GetBuildingPollution(prefab, destroyed, abandoned, isPark, efficiency, buffer2, buffer3, singleton, singletonBuffer, ref roComponentLookup1, ref roComponentLookup2, ref roComponentLookup3, ref roComponentLookup4, ref roComponentLookup5, ref roComponentLookup6, ref employeeRoBufferLookup, ref citizenRoBufferLookup, ref roComponentLookup7);
      info.label = "Pollution";
      info.value = "Ground: " + (object) buildingPollution.m_GroundPollution + ". " + "Air: " + (object) buildingPollution.m_AirPollution + ". " + "Noise: " + (object) buildingPollution.m_NoisePollution + ".";
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasElectricityConsumeInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<ElectricityConsumer>(entity) && this.EntityManager.HasComponent<ConsumptionData>(prefab);
    }

    private void UpdateElectricityConsumeInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      ConsumptionData componentData1 = this.EntityManager.GetComponentData<ConsumptionData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<ConsumptionData>(entity, ref componentData1);
      ElectricityConsumer componentData2 = this.EntityManager.GetComponentData<ElectricityConsumer>(entity);
      info.label = "Electricity Consuming";
      info.value = string.Format("consuming: {0}  fill: {1}", (object) componentData2.m_WantedConsumption, (object) componentData2.m_FulfilledConsumption);
    }

    private bool HasStorageInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<StorageLimitData>(prefab) && this.EntityManager.HasComponent<Game.Economy.Resources>(entity);
    }

    private void UpdateStorageInfo(Entity entity, Entity prefab, InfoList info)
    {
      info.label = "Resource Storage";
      StorageLimitData component;
      DynamicBuffer<Game.Economy.Resources> buffer;
      if (!this.EntityManager.TryGetComponent<StorageLimitData>(prefab, out component) || !this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer))
        return;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<StorageLimitData>(entity, ref component);
      info.Add(new InfoList.Item(string.Format("Storage Limit: {0}", (object) component.m_Limit)));
      for (int index = 0; index < buffer.Length; ++index)
        info.Add(new InfoList.Item(string.Format("{0}({1})", (object) EconomyUtils.GetName(buffer[index].m_Resource), (object) buffer[index].m_Amount)));
    }

    private bool HasWaterConsumeInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<WaterConsumer>(entity) && this.EntityManager.HasComponent<ConsumptionData>(prefab);
    }

    private void UpdateWaterConsumeInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      ConsumptionData componentData1 = this.EntityManager.GetComponentData<ConsumptionData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<ConsumptionData>(entity, ref componentData1);
      WaterConsumer componentData2 = this.EntityManager.GetComponentData<WaterConsumer>(entity);
      info.label = "Water Consuming";
      info.value = string.Format("consuming: {0}  fill: {1}", (object) componentData2.m_WantedConsumption, (object) componentData2.m_FulfilledFresh);
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasGarbageInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<GarbageProducer>(entity) && this.EntityManager.HasComponent<ConsumptionData>(prefab);
    }

    private void UpdateGarbageInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      ConsumptionData componentData1 = this.EntityManager.GetComponentData<ConsumptionData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<ConsumptionData>(entity, ref componentData1);
      GarbageProducer componentData2 = this.EntityManager.GetComponentData<GarbageProducer>(entity);
      // ISSUE: reference to a compiler-generated field
      GarbageParameterData singleton = this.__query_746694603_3.GetSingleton<GarbageParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      GarbageAccumulationSystem.GetGarbage(ref componentData1, entity, prefab, this.GetBufferLookup<Renter>(true), this.GetBufferLookup<Game.Buildings.Student>(true), this.GetBufferLookup<Occupant>(true), this.GetComponentLookup<HomelessHousehold>(true), this.GetBufferLookup<HouseholdCitizen>(true), this.GetComponentLookup<Citizen>(true), this.GetBufferLookup<Employee>(true), this.GetBufferLookup<Patient>(true), this.GetComponentLookup<SpawnableBuildingData>(true), this.GetComponentLookup<CurrentDistrict>(true), this.GetBufferLookup<DistrictModifier>(true), this.GetComponentLookup<ZoneData>(true), this.GetBufferLookup<CityModifier>(true)[this.m_CitySystem.City], ref singleton);
      int homeless = 0;
      DynamicBuffer<Renter> buffer1;
      if (this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer1))
      {
        for (int index = 0; index < buffer1.Length; ++index)
        {
          DynamicBuffer<HouseholdCitizen> buffer2;
          if (this.EntityManager.HasComponent<HomelessHousehold>(buffer1[index].m_Renter) && this.EntityManager.TryGetBuffer<HouseholdCitizen>(buffer1[index].m_Renter, true, out buffer2))
            homeless += buffer2.Length;
        }
      }
      // ISSUE: reference to a compiler-generated method
      string garbageStatus = this.GetGarbageStatus(Mathf.RoundToInt(componentData1.m_GarbageAccumulation), componentData2.m_Garbage, homeless, singleton.m_HomelessGarbageProduce);
      info.label = "Garbage";
      info.value = garbageStatus;
      info.target = InfoList.Item.kNullEntity;
    }

    private string GetGarbageStatus(
      int accumulation,
      int garbage,
      int homeless,
      int homelessProduce)
    {
      return garbage.ToString() + " (+" + (object) accumulation + " / day) homeless(" + (object) homeless + " * " + (object) homelessProduce + "=" + (object) (homeless * homelessProduce) + ")";
    }

    private bool HasSpectatorSiteInfo(Entity entity, Entity _)
    {
      SpectatorSite component;
      return this.EntityManager.TryGetComponent<SpectatorSite>(entity, out component) && this.EntityManager.HasComponent<Duration>(component.m_Event);
    }

    private void UpdateSpectatorSiteInfo(Entity entity, Entity _, GenericInfo info)
    {
      SpectatorSite componentData1 = this.EntityManager.GetComponentData<SpectatorSite>(entity);
      Duration componentData2 = this.EntityManager.GetComponentData<Duration>(componentData1.m_Event);
      // ISSUE: reference to a compiler-generated field
      if (this.m_SimulationSystem.frameIndex < componentData2.m_StartFrame)
      {
        info.label = "Preparing";
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SimulationSystem.frameIndex < componentData2.m_EndFrame)
          info.label = "Event";
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(componentData1.m_Event);
      info.target = componentData1.m_Event;
    }

    private bool HasInDangerInfo(Entity entity, Entity prefab)
    {
      InDanger component;
      return this.EntityManager.TryGetComponent<InDanger>(entity, out component) && this.EntityManager.Exists(component.m_Event);
    }

    private void UpdateInDangerInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      InDanger componentData = this.EntityManager.GetComponentData<InDanger>(entity);
      if ((componentData.m_Flags & DangerFlags.Evacuate) != (DangerFlags) 0)
        info.label = "Evacuating";
      else if ((componentData.m_Flags & DangerFlags.StayIndoors) != (DangerFlags) 0)
        info.label = "In danger";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(componentData.m_Event);
      info.target = componentData.m_Event;
    }

    private bool HasVehicleInfo(Entity entity, Entity prefab)
    {
      return (this.EntityManager.HasComponent<Building>(entity) || this.EntityManager.HasComponent<CompanyData>(entity) || this.EntityManager.HasComponent<Household>(entity)) && this.EntityManager.HasComponent<OwnedVehicle>(entity);
    }

    private void UpdateVehicleInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<OwnedVehicle> buffer = this.EntityManager.GetBuffer<OwnedVehicle>(entity, true);
      int availableVehicles = VehicleUIUtils.GetAvailableVehicles(entity, this.EntityManager);
      info.label = string.Format("Vehicles availableVehicles:{0}", (object) availableVehicles);
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity vehicle = buffer[index].m_Vehicle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(vehicle), vehicle));
      }
    }

    private bool HasPoliceInfo(Entity entity, Entity prefab)
    {
      PoliceStationData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.PoliceStation>(entity) || !this.EntityManager.HasComponent<Occupant>(entity) || !this.EntityManager.TryGetComponent<PoliceStationData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PoliceStationData>(entity, ref component);
      return component.m_JailCapacity > 0;
    }

    private void UpdatePoliceInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      PoliceStationData componentData = this.EntityManager.GetComponentData<PoliceStationData>(prefab);
      DynamicBuffer<Occupant> buffer = this.EntityManager.GetBuffer<Occupant>(entity, true);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PoliceStationData>(entity, ref componentData);
      info.label = "Arrested criminals";
      info.value = buffer.Length;
      info.max = componentData.m_JailCapacity;
    }

    private bool HasPrisonInfo(Entity entity, Entity prefab)
    {
      PrisonData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.Prison>(entity) || !this.EntityManager.TryGetComponent<PrisonData>(prefab, out component) || !this.EntityManager.HasComponent<Occupant>(entity))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PrisonData>(entity, ref component);
      return component.m_PrisonerCapacity > 0;
    }

    private void UpdatePrisonInfo(Entity entity, Entity prefab, InfoList info)
    {
      PrisonData componentData = this.EntityManager.GetComponentData<PrisonData>(prefab);
      DynamicBuffer<Occupant> buffer = this.EntityManager.GetBuffer<Occupant>(entity, true);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PrisonData>(entity, ref componentData);
      info.label = string.Format("Prisoners ({0})", (object) buffer.Length);
      info.Add(new InfoList.Item(buffer.Length.ToString() + "/" + (object) componentData.m_PrisonerCapacity));
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer[index].m_Occupant), buffer[index].m_Occupant));
      }
    }

    private bool HasResourceProductionInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Game.Economy.Resources>(entity) && this.EntityManager.HasComponent<Game.Buildings.ResourceProducer>(entity);
    }

    private void UpdateResourceProductionInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<Game.Economy.Resources> buffer1 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      NativeList<ResourceProductionData> resources1 = new NativeList<ResourceProductionData>();
      DynamicBuffer<ResourceProductionData> buffer2;
      if (this.EntityManager.TryGetBuffer<ResourceProductionData>(prefab, true, out buffer2))
      {
        resources1 = new NativeList<ResourceProductionData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        resources1.AddRange(buffer2.AsNativeArray());
      }
      DynamicBuffer<InstalledUpgrade> buffer3;
      if (this.EntityManager.TryGetBuffer<InstalledUpgrade>(entity, true, out buffer3))
      {
        for (int index = 0; index < buffer3.Length; ++index)
        {
          DynamicBuffer<ResourceProductionData> buffer4;
          if (this.EntityManager.TryGetBuffer<ResourceProductionData>(this.EntityManager.GetComponentData<PrefabRef>(buffer3[index].m_Upgrade).m_Prefab, true, out buffer4))
          {
            if (!resources1.IsCreated)
              resources1 = new NativeList<ResourceProductionData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
            ResourceProductionData.Combine(resources1, buffer4);
          }
        }
      }
      info.label = "Resource Production";
      if (!resources1.IsCreated)
        return;
      for (int index = 0; index < resources1.Length; ++index)
      {
        ResourceProductionData resourceProductionData = resources1[index];
        int resources2 = EconomyUtils.GetResources(resourceProductionData.m_Type, buffer1);
        info.Add(new InfoList.Item(string.Concat((object) (EconomyUtils.GetName(resourceProductionData.m_Type), " ", resources2, "/", resourceProductionData.m_StorageCapacity))));
      }
      resources1.Dispose();
    }

    private bool HasShelterInfo(Entity entity, Entity prefab)
    {
      EmergencyShelterData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.EmergencyShelter>(entity) || !this.EntityManager.TryGetComponent<EmergencyShelterData>(prefab, out component) || !this.EntityManager.HasComponent<Occupant>(entity))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<EmergencyShelterData>(entity, ref component);
      return component.m_ShelterCapacity > 0;
    }

    private void UpdateShelterInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      EmergencyShelterData componentData = this.EntityManager.GetComponentData<EmergencyShelterData>(prefab);
      DynamicBuffer<Occupant> buffer = this.EntityManager.GetBuffer<Occupant>(entity, true);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<EmergencyShelterData>(entity, ref componentData);
      info.label = "Occupants";
      info.value = buffer.Length;
      info.max = componentData.m_ShelterCapacity;
    }

    private bool HasParkInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Game.Buildings.Park>(entity) && this.EntityManager.HasComponent<ParkData>(prefab);
    }

    private void UpdateParkInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Game.Buildings.Park componentData1 = this.EntityManager.GetComponentData<Game.Buildings.Park>(entity);
      ParkData componentData2 = this.EntityManager.GetComponentData<ParkData>(prefab);
      info.label = "Maintenance";
      info.value = componentData1.m_Maintenance.ToString() + "/" + (object) componentData2.m_MaintenancePool;
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasHouseholdInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Household>(entity);
    }

    private void UpdateHouseholdInfo(Entity entity, Entity prefab, InfoList info)
    {
      info.label = "Household info";
      DynamicBuffer<HouseholdCitizen> buffer1 = this.EntityManager.GetBuffer<HouseholdCitizen>(entity, true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Worker> roComponentLookup1 = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Citizen> roComponentLookup2 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<HealthProblem> roComponentLookup3 = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      EconomyParameterData singleton = this.__query_746694603_4.GetSingleton<EconomyParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> taxRates = this.m_TaxSystem.GetTaxRates();
      Household componentData = this.EntityManager.GetComponentData<Household>(entity);
      DynamicBuffer<Game.Economy.Resources> buffer2 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      info.Add(new InfoList.Item("Wealth: " + (object) EconomyUtils.GetHouseholdTotalWealth(componentData, buffer2)));
      info.Add(new InfoList.Item("Income: " + (object) EconomyUtils.GetHouseholdIncome(buffer1, ref roComponentLookup1, ref roComponentLookup2, ref roComponentLookup3, ref singleton, taxRates)));
    }

    private bool HasHouseholdsInfo(Entity entity, Entity prefab)
    {
      BuildingPropertyData component;
      DynamicBuffer<Renter> buffer;
      return this.EntityManager.HasComponent<Renter>(entity) && this.EntityManager.TryGetComponent<BuildingPropertyData>(prefab, out component) && component.m_ResidentialProperties > 0 && this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer) && buffer.Length > 0;
    }

    private void UpdateHouseholdsInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<Renter> buffer1 = this.EntityManager.GetBuffer<Renter>(entity, true);
      info.label = string.Format("Households ({0})", (object) buffer1.Length);
      for (int index = 0; index < buffer1.Length; ++index)
      {
        Entity renter = buffer1[index].m_Renter;
        Household component1;
        PropertyRenter component2;
        DynamicBuffer<Game.Economy.Resources> buffer2;
        if (this.EntityManager.TryGetComponent<Household>(renter, out component1) && this.EntityManager.TryGetComponent<PropertyRenter>(renter, out component2) && this.EntityManager.TryGetBuffer<Game.Economy.Resources>(renter, true, out buffer2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(string.Format("Name:{0} Rent:{1} Wealth:{2}", (object) this.m_NameSystem.GetDebugName(renter), (object) component2.m_Rent, (object) EconomyUtils.GetHouseholdTotalWealth(component1, buffer2)), renter));
        }
      }
    }

    private bool HasResidentsInfo(Entity entity, Entity prefab)
    {
      DynamicBuffer<HouseholdCitizen> buffer;
      return this.EntityManager.HasComponent<Household>(entity) && this.EntityManager.TryGetBuffer<HouseholdCitizen>(entity, true, out buffer) && buffer.Length > 0;
    }

    private void UpdateResidentInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<HouseholdCitizen> buffer = this.EntityManager.GetBuffer<HouseholdCitizen>(entity, true);
      info.label = string.Format("Residents ({0})", (object) buffer.Length);
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity citizen = buffer[index].m_Citizen;
        if (this.EntityManager.HasComponent<Citizen>(citizen))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(citizen), citizen));
        }
      }
    }

    private bool HasCompanyInfo(Entity entity, Entity prefab)
    {
      // ISSUE: reference to a compiler-generated method
      return this.HasCompany(entity, prefab, out Entity _);
    }

    private bool HasCompany(Entity entity, Entity prefab, out Entity company)
    {
      DynamicBuffer<Renter> buffer;
      if (this.EntityManager.HasComponent<Renter>(entity) && this.EntityManager.HasComponent<BuildingPropertyData>(prefab) && this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer) && buffer.Length > 0)
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          if (this.EntityManager.HasComponent<CompanyData>(buffer[index].m_Renter))
          {
            company = buffer[index].m_Renter;
            return true;
          }
        }
      }
      company = InfoList.Item.kNullEntity;
      return false;
    }

    private void UpdateCompanyInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      DynamicBuffer<Renter> buffer = this.EntityManager.GetBuffer<Renter>(entity, true);
      info.label = "Company";
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity renter = buffer[index].m_Renter;
        PropertyRenter component;
        if (this.EntityManager.HasComponent<CompanyData>(renter) && this.EntityManager.TryGetComponent<PropertyRenter>(renter, out component))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.value = string.Format("Name:{0} Rent:{1}", (object) this.m_NameSystem.GetDebugName(renter), (object) component.m_Rent);
          info.target = renter;
        }
      }
    }

    private bool HasEmployeesInfo(Entity entity, Entity prefab)
    {
      if (this.EntityManager.HasComponent<Employee>(entity))
        return true;
      Entity company;
      // ISSUE: reference to a compiler-generated method
      return this.HasCompany(entity, prefab, out company) && this.EntityManager.HasComponent<Employee>(company);
    }

    private void UpdateEmployeesInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<Employee> buffer;
      Entity company;
      // ISSUE: reference to a compiler-generated method
      if (!this.EntityManager.TryGetBuffer<Employee>(entity, true, out buffer) && (!this.HasCompany(entity, prefab, out company) || !this.EntityManager.TryGetBuffer<Employee>(company, true, out buffer)))
        return;
      info.label = string.Format("Employees ({0})", (object) buffer.Length);
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer[index].m_Worker), buffer[index].m_Worker));
      }
    }

    private bool HasPatientsInfo(Entity entity, Entity prefab)
    {
      HospitalData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.Hospital>(entity) || !this.EntityManager.TryGetComponent<HospitalData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<HospitalData>(entity, ref component);
      return component.m_PatientCapacity > 0;
    }

    private void UpdatePatientsInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<Patient> buffer = this.EntityManager.GetBuffer<Patient>(entity, true);
      HospitalData componentData = this.EntityManager.GetComponentData<HospitalData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<HospitalData>(entity, ref componentData);
      info.label = string.Format("Patients ({0})", (object) buffer.Length);
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity patient = buffer[index].m_Patient;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(patient), patient));
      }
    }

    private bool HasStudentsInfo(Entity entity, Entity prefab)
    {
      SchoolData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.School>(entity) || !this.EntityManager.HasBuffer<Game.Buildings.Student>(entity) || !this.EntityManager.TryGetComponent<SchoolData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<SchoolData>(entity, ref component);
      return component.m_StudentCapacity > 0;
    }

    private void UpdateStudentsInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<Game.Buildings.Student> buffer1 = this.EntityManager.GetBuffer<Game.Buildings.Student>(entity, true);
      SchoolData componentData1 = this.EntityManager.GetComponentData<SchoolData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<SchoolData>(entity, ref componentData1);
      info.label = string.Format("Students ({0})", (object) buffer1.Length);
      for (int index = 0; index < buffer1.Length; ++index)
      {
        Entity student = buffer1[index].m_Student;
        Citizen componentData2 = this.EntityManager.GetComponentData<Citizen>(student);
        float studyWillingness = componentData2.GetPseudoRandom(CitizenPseudoRandom.StudyWillingness).NextFloat();
        DynamicBuffer<Efficiency> buffer2;
        float efficiency = this.EntityManager.TryGetBuffer<Efficiency>(entity, true, out buffer2) ? BuildingUtils.GetEfficiency(buffer2) : 1f;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> buffer3 = this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(student) + string.Format("Graduation: {0}", (object) GraduationSystem.GetGraduationProbability((int) componentData1.m_EducationLevel, (int) componentData2.m_WellBeing, componentData1, buffer3, studyWillingness, efficiency)), student));
      }
    }

    private bool HasDeathcareInfo(Entity entity, Entity prefab)
    {
      DeathcareFacilityData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.DeathcareFacility>(entity) || !this.EntityManager.HasComponent<Patient>(entity) || !this.EntityManager.TryGetComponent<DeathcareFacilityData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<DeathcareFacilityData>(entity, ref component);
      return component.m_StorageCapacity > 0;
    }

    private void UpdateDeathcareInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      DynamicBuffer<Patient> buffer = this.EntityManager.GetBuffer<Patient>(entity, true);
      EntityManager entityManager = this.EntityManager;
      Game.Buildings.DeathcareFacility componentData1 = entityManager.GetComponentData<Game.Buildings.DeathcareFacility>(entity);
      entityManager = this.EntityManager;
      DeathcareFacilityData componentData2 = entityManager.GetComponentData<DeathcareFacilityData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<DeathcareFacilityData>(entity, ref componentData2);
      int num = componentData1.m_LongTermStoredCount + buffer.Length;
      info.label = "Bodies";
      info.value = num;
      info.max = componentData2.m_StorageCapacity;
    }

    private bool HasEfficiencyInfo(Entity entity, Entity prefab)
    {
      if (this.EntityManager.HasComponent<Building>(entity))
        return this.EntityManager.HasComponent<Efficiency>(entity);
      PropertyRenter component;
      return this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component) && this.EntityManager.HasComponent<Efficiency>(component.m_Property);
    }

    private void UpdateEfficiencyInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      PropertyRenter component;
      float efficiency = BuildingUtils.GetEfficiency(this.EntityManager.GetBuffer<Efficiency>(!this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component) ? entity : component.m_Property, true));
      info.label = "Efficiency";
      info.value = Mathf.RoundToInt(100f * efficiency).ToString() + " %";
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasStoredResourcesInfo(Entity entity, Entity prefab)
    {
      DynamicBuffer<Game.Economy.Resources> buffer;
      return this.EntityManager.HasComponent<StorageCompanyData>(prefab) && this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer) && buffer.Length > 0;
    }

    private void UpdateStoredResourcesInfo(Entity entity, Entity prefab, InfoList info)
    {
      StorageCompanyData componentData = this.EntityManager.GetComponentData<StorageCompanyData>(prefab);
      DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      ResourceIterator iterator = ResourceIterator.GetIterator();
      info.label = string.Format("Stored Resources ({0})", (object) buffer.Length);
      while (iterator.Next())
      {
        if ((componentData.m_StoredResources & iterator.resource) != Resource.NoResource)
        {
          int resources = EconomyUtils.GetResources(iterator.resource, buffer);
          info.Add(new InfoList.Item(EconomyUtils.GetName(iterator.resource) + (object) resources));
        }
      }
    }

    private bool HasElectricityProductionInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<ElectricityProducer>(entity) && this.EntityManager.HasComponent<PowerPlantData>(prefab);
    }

    private void UpdateElectricityProductionInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      PowerPlantData componentData = this.EntityManager.GetComponentData<PowerPlantData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PowerPlantData>(entity, ref componentData);
      int electricityProduction = componentData.m_ElectricityProduction;
      info.label = "Electricity Production";
      info.value = electricityProduction.ToString();
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasBatteriesInfo(Entity entity, Entity prefab)
    {
      BatteryData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.Battery>(entity) || !this.EntityManager.HasComponent<PowerPlantData>(prefab) || !this.EntityManager.TryGetComponent<BatteryData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<BatteryData>(entity, ref component);
      return component.m_Capacity > 0;
    }

    private void UpdateBatteriesInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Game.Buildings.Battery componentData1 = this.EntityManager.GetComponentData<Game.Buildings.Battery>(entity);
      BatteryData componentData2 = this.EntityManager.GetComponentData<BatteryData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<BatteryData>(entity, ref componentData2);
      info.label = "Batteries";
      info.value = Mathf.RoundToInt(100f * (float) (componentData1.m_StoredEnergy / componentData2.capacityTicks)).ToString() + "%";
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasStoredGarbageInfo(Entity entity, Entity prefab)
    {
      GarbageFacilityData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.GarbageFacility>(entity) || !this.EntityManager.TryGetComponent<GarbageFacilityData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<GarbageFacilityData>(entity, ref component);
      return component.m_GarbageCapacity > 0;
    }

    private void UpdateStoredGarbageInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      GarbageFacilityData componentData1 = this.EntityManager.GetComponentData<GarbageFacilityData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<GarbageFacilityData>(entity, ref componentData1);
      PowerPlantData component1;
      if (this.EntityManager.TryGetComponent<PowerPlantData>(prefab, out component1))
      {
        // ISSUE: reference to a compiler-generated method
        this.AddUpgradeData<PowerPlantData>(entity, ref component1);
      }
      int num = 0;
      DynamicBuffer<Game.Economy.Resources> buffer1;
      if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer1))
        num = EconomyUtils.GetResources(Resource.Garbage, buffer1);
      DynamicBuffer<Game.Areas.SubArea> buffer2;
      if (this.EntityManager.TryGetBuffer<Game.Areas.SubArea>(entity, true, out buffer2))
      {
        for (int index = 0; index < buffer2.Length; ++index)
        {
          Entity area = buffer2[index].m_Area;
          Storage component2;
          if (this.EntityManager.TryGetComponent<Storage>(area, out component2))
          {
            EntityManager entityManager = this.EntityManager;
            PrefabRef componentData2 = entityManager.GetComponentData<PrefabRef>(area);
            entityManager = this.EntityManager;
            Geometry componentData3 = entityManager.GetComponentData<Geometry>(area);
            StorageAreaData component3;
            if (this.EntityManager.TryGetComponent<StorageAreaData>(componentData2.m_Prefab, out component3))
            {
              componentData1.m_GarbageCapacity += AreaUtils.CalculateStorageCapacity(componentData3, component3);
              num += component2.m_Amount;
            }
          }
        }
      }
      info.label = "Stored Garbage";
      info.value = num;
      info.max = componentData1.m_GarbageCapacity;
    }

    private bool HasGarbageProcessingInfo(Entity entity, Entity prefab)
    {
      GarbageFacilityData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.GarbageFacility>(entity) || !this.EntityManager.TryGetComponent<GarbageFacilityData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<GarbageFacilityData>(entity, ref component);
      return component.m_ProcessingSpeed > 0;
    }

    private void UpdateGarbageProcessingInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Game.Buildings.GarbageFacility componentData1 = this.EntityManager.GetComponentData<Game.Buildings.GarbageFacility>(entity);
      GarbageFacilityData componentData2 = this.EntityManager.GetComponentData<GarbageFacilityData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<GarbageFacilityData>(entity, ref componentData2);
      info.label = "Garbage Processing Speed";
      info.value = componentData1.m_ProcessingRate.ToString() + "/" + (object) componentData2.m_ProcessingSpeed;
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasMailProcessingSpeedInfo(Entity entity, Entity prefab)
    {
      PostFacilityData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.PostFacility>(entity) || !this.EntityManager.TryGetComponent<PostFacilityData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PostFacilityData>(entity, ref component);
      return component.m_SortingRate > 0;
    }

    private void UpdateMailProcessingSpeedInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Game.Buildings.PostFacility componentData1 = this.EntityManager.GetComponentData<Game.Buildings.PostFacility>(entity);
      PostFacilityData componentData2 = this.EntityManager.GetComponentData<PostFacilityData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PostFacilityData>(entity, ref componentData2);
      int num = (componentData2.m_SortingRate * (int) componentData1.m_ProcessingFactor + 50) / 100;
      info.label = "Mail Processing Speed";
      info.value = num.ToString() + "/" + (object) componentData2.m_SortingRate;
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasMailInfo(Entity entity, Entity prefab)
    {
      PostFacilityData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.PostFacility>(entity) || !this.EntityManager.TryGetComponent<PostFacilityData>(prefab, out component))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PostFacilityData>(entity, ref component);
      return component.m_MailCapacity > 0;
    }

    private void UpdateMailInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      PostFacilityData componentData = this.EntityManager.GetComponentData<PostFacilityData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PostFacilityData>(entity, ref componentData);
      DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      int resources1 = EconomyUtils.GetResources(Resource.UnsortedMail, buffer);
      int resources2 = EconomyUtils.GetResources(Resource.LocalMail, buffer);
      int resources3 = EconomyUtils.GetResources(Resource.OutgoingMail, buffer);
      string str1;
      if (componentData.m_PostVanCapacity <= 0)
        str1 = "Unsorted mail: " + (object) resources1 + ". Local mail: " + (object) resources2 + ".";
      else
        str1 = "Mail to deliver: " + (object) resources2 + ". Collected mail: " + (object) resources1 + ".";
      string str2 = str1;
      if (componentData.m_SortingRate > 0 || resources3 > 0)
        str2 = str2 + " Outgoing mail: " + (object) resources3;
      info.label = "Post Facility";
      info.value = str2;
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasStoredMailInfo(Entity entity, Entity prefab)
    {
      PostFacilityData component;
      if (!this.EntityManager.HasComponent<Game.Buildings.PostFacility>(entity) || !this.EntityManager.TryGetComponent<PostFacilityData>(prefab, out component) || !this.EntityManager.HasComponent<Game.Economy.Resources>(entity))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PostFacilityData>(entity, ref component);
      return component.m_MailCapacity > 0;
    }

    private void UpdateStoredMailInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      PostFacilityData componentData = this.EntityManager.GetComponentData<PostFacilityData>(prefab);
      // ISSUE: reference to a compiler-generated method
      this.AddUpgradeData<PostFacilityData>(entity, ref componentData);
      DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      int resources1 = EconomyUtils.GetResources(Resource.UnsortedMail, buffer);
      int resources2 = EconomyUtils.GetResources(Resource.LocalMail, buffer);
      int resources3 = EconomyUtils.GetResources(Resource.OutgoingMail, buffer);
      int num1 = resources2;
      int num2 = resources1 + num1 + resources3;
      info.label = "Stored Mail";
      info.value = num2;
      info.max = componentData.m_MailCapacity;
    }

    private bool HasSendReceiveMailInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<MailProducer>(entity);
    }

    private void UpdateSendReceiveMailInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      MailProducer componentData = this.EntityManager.GetComponentData<MailProducer>(entity);
      info.label = "Send Receive Mail";
      info.value = string.Format("Send: {0} Receive: {1}", (object) componentData.m_SendingMail, (object) componentData.receivingMail);
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasParkingInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Building>(entity);
    }

    private void UpdateParkingInfo(Entity entity, Entity prefab, InfoList info)
    {
      int parkedCars = 0;
      int slotCapacity = 0;
      int parkingFee = 0;
      int laneCount = 0;
      string empty = string.Empty;
      NativeList<Entity> parkedCarList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      DynamicBuffer<Game.Net.SubLane> buffer1;
      if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(entity, true, out buffer1))
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckParkingLanes(buffer1, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
      }
      DynamicBuffer<Game.Net.SubNet> buffer2;
      if (this.EntityManager.TryGetBuffer<Game.Net.SubNet>(entity, true, out buffer2))
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckParkingLanes(buffer2, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
      }
      DynamicBuffer<Game.Objects.SubObject> buffer3;
      if (this.EntityManager.TryGetBuffer<Game.Objects.SubObject>(entity, true, out buffer3))
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckParkingLanes(buffer3, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
      }
      info.label = string.Format("Parking ({0})", (object) parkedCarList.Length);
      Game.Prefabs.BuildingData component;
      if (laneCount != 0 && this.EntityManager.TryGetComponent<Game.Prefabs.BuildingData>(prefab, out component) && (component.m_Flags & (Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar)) == (Game.Prefabs.BuildingFlags) 0)
      {
        int num = parkingFee / laneCount;
        info.Add(new InfoList.Item("Parking Fee: " + (object) num));
      }
      info.Add(new InfoList.Item(empty + " Parked Cars: " + (object) parkedCars + "/" + (object) slotCapacity + "."));
      for (int index = 0; index < parkedCarList.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(parkedCarList[index]), parkedCarList[index]));
      }
      parkedCarList.Dispose();
    }

    private void CheckParkingLanes(
      DynamicBuffer<Game.Objects.SubObject> subObjects,
      ref int slotCapacity,
      ref int parkedCars,
      ref int parkingFee,
      ref int laneCount,
      ref NativeList<Entity> parkedCarList)
    {
      for (int index = 0; index < subObjects.Length; ++index)
      {
        Entity subObject = subObjects[index].m_SubObject;
        DynamicBuffer<Game.Net.SubLane> buffer1;
        if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(subObject, true, out buffer1))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer1, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
        }
        DynamicBuffer<Game.Objects.SubObject> buffer2;
        if (this.EntityManager.TryGetBuffer<Game.Objects.SubObject>(subObject, true, out buffer2))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer2, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
        }
      }
    }

    private void CheckParkingLanes(
      DynamicBuffer<Game.Net.SubNet> subNets,
      ref int slotCapacity,
      ref int parkedCars,
      ref int parkingFee,
      ref int laneCount,
      ref NativeList<Entity> parkedCarList)
    {
      for (int index = 0; index < subNets.Length; ++index)
      {
        DynamicBuffer<Game.Net.SubLane> buffer;
        if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(subNets[index].m_SubNet, true, out buffer))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer, ref slotCapacity, ref parkedCars, ref parkingFee, ref laneCount, ref parkedCarList);
        }
      }
    }

    private void CheckParkingLanes(
      DynamicBuffer<Game.Net.SubLane> subLanes,
      ref int slotCapacity,
      ref int parkedCars,
      ref int parkingFee,
      ref int laneCount,
      ref NativeList<Entity> parkedCarList)
    {
      for (int index1 = 0; index1 < subLanes.Length; ++index1)
      {
        Entity subLane = subLanes[index1].m_SubLane;
        Game.Net.ParkingLane component1;
        if (this.EntityManager.TryGetComponent<Game.Net.ParkingLane>(subLane, out component1))
        {
          if ((component1.m_Flags & ParkingLaneFlags.VirtualLane) == (ParkingLaneFlags) 0)
          {
            EntityManager entityManager = this.EntityManager;
            Entity prefab = entityManager.GetComponentData<PrefabRef>(subLane).m_Prefab;
            entityManager = this.EntityManager;
            Curve componentData1 = entityManager.GetComponentData<Curve>(subLane);
            entityManager = this.EntityManager;
            DynamicBuffer<LaneObject> buffer = entityManager.GetBuffer<LaneObject>(subLane, true);
            entityManager = this.EntityManager;
            ParkingLaneData componentData2 = entityManager.GetComponentData<ParkingLaneData>(prefab);
            if ((double) componentData2.m_SlotInterval != 0.0)
            {
              int parkingSlotCount = NetUtils.GetParkingSlotCount(componentData1, component1, componentData2);
              slotCapacity += parkingSlotCount;
            }
            else
              slotCapacity = -1000000;
            for (int index2 = 0; index2 < buffer.Length; ++index2)
            {
              entityManager = this.EntityManager;
              if (entityManager.HasComponent<ParkedCar>(buffer[index2].m_LaneObject))
              {
                parkedCarList.Add(in buffer[index2].m_LaneObject);
                ++parkedCars;
              }
            }
            parkingFee += (int) component1.m_ParkingFee;
            ++laneCount;
          }
        }
        else
        {
          GarageLane component2;
          if (this.EntityManager.TryGetComponent<GarageLane>(subLane, out component2))
          {
            slotCapacity += (int) component2.m_VehicleCapacity;
            parkedCars += (int) component2.m_VehicleCount;
            parkingFee += (int) component2.m_ParkingFee;
            ++laneCount;
          }
        }
      }
    }

    private bool HasHouseholdPetsInfo(Entity entity, Entity prefab)
    {
      DynamicBuffer<HouseholdAnimal> buffer;
      return this.EntityManager.HasComponent<Household>(entity) && this.EntityManager.HasComponent<Game.Prefabs.HouseholdData>(prefab) && this.EntityManager.TryGetBuffer<HouseholdAnimal>(entity, true, out buffer) && buffer.Length > 0;
    }

    private void UpdateHouseholdPetsInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<HouseholdAnimal> buffer = this.EntityManager.GetBuffer<HouseholdAnimal>(entity, true);
      info.label = "Household Pets";
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer[index].m_HouseholdPet), buffer[index].m_HouseholdPet));
      }
    }

    private bool HasRentInfo(Entity entity, Entity prefab)
    {
      PropertyRenter component;
      return this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component) && component.m_Property != InfoList.Item.kNullEntity;
    }

    private void UpdateRentInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      PropertyRenter componentData = this.EntityManager.GetComponentData<PropertyRenter>(entity);
      DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity);
      info.label = "Rent";
      info.value = string.Format("Rent: {0} Money:{1}", (object) componentData.m_Rent, (object) EconomyUtils.GetResources(Resource.Money, buffer));
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasLandValueInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Renter>(entity) && this.EntityManager.HasComponent<Building>(entity);
    }

    private void UpdateLandValueInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Building componentData = this.EntityManager.GetComponentData<Building>(entity);
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      float landValueBase = 0.0f;
      // ISSUE: reference to a compiler-generated field
      EconomyParameterData singleton = this.__query_746694603_4.GetSingleton<EconomyParameterData>();
      BuildingPropertyData component1;
      if (this.EntityManager.TryGetComponent<BuildingPropertyData>(prefab, out component1))
      {
        LandValue component2;
        if (this.EntityManager.TryGetComponent<LandValue>(componentData.m_RoadEdge, out component2))
          landValueBase = component2.m_LandValue;
        ConsumptionData component3;
        if (this.EntityManager.TryGetComponent<ConsumptionData>(prefab, out component3))
          num2 = component3.m_Upkeep;
        int lotSize = 0;
        Game.Prefabs.BuildingData component4;
        if (this.EntityManager.TryGetComponent<Game.Prefabs.BuildingData>(prefab, out component4))
          lotSize = component4.m_LotSize.x * component4.m_LotSize.y;
        Attached component5;
        DynamicBuffer<Game.Areas.SubArea> buffer1;
        if (this.EntityManager.TryGetComponent<Attached>(entity, out component5) && this.EntityManager.TryGetBuffer<Game.Areas.SubArea>(component5.m_Parent, true, out buffer1))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          lotSize += Mathf.CeilToInt(ExtractorAISystem.GetArea(buffer1, this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup, this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup));
        }
        Game.Zones.AreaType areaType = Game.Zones.AreaType.None;
        int buildingLevel = 1;
        SpawnableBuildingData component6;
        if (this.EntityManager.TryGetComponent<SpawnableBuildingData>(prefab, out component6))
        {
          buildingLevel = (int) component6.m_Level;
          EntityManager entityManager = this.EntityManager;
          areaType = entityManager.GetComponentData<ZoneData>(component6.m_ZonePrefab).m_AreaType;
          BuildingCondition component7;
          if (this.EntityManager.TryGetComponent<BuildingCondition>(entity, out component7))
          {
            entityManager = this.EntityManager;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CityModifier> buffer2 = entityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true);
            num4 = BuildingUtils.GetLevelingCost(areaType, component1, (int) component6.m_Level, buffer2);
            num3 = component7.m_Condition;
          }
        }
        num1 = PropertyUtils.GetRentPricePerRenter(component3, component1, buildingLevel, lotSize, landValueBase, areaType, ref singleton);
      }
      info.label = "Rent/Upkeep/Land value/Leveling";
      info.value = string.Format("Rent per renter: {0} Upkeep: {1} LV: {2} Leveling {3} / {4}", (object) num1, (object) num2, (object) landValueBase, (object) num3, (object) num4);
    }

    private bool HasTradeCostInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.TryGetBuffer<TradeCost>(entity, true, out DynamicBuffer<TradeCost> _);
    }

    private void UpdateTradeCostInfo(Entity entity, Entity prefab, InfoList infos)
    {
      infos.label = "Trade Costs";
      DynamicBuffer<TradeCost> buffer;
      if (!this.EntityManager.TryGetBuffer<TradeCost>(entity, true, out buffer))
        return;
      for (int index = 0; index < buffer.Length; ++index)
      {
        TradeCost tradeCost = buffer[index];
        infos.Add(new InfoList.Item(string.Format("{0} buy {1} sell {2}", (object) EconomyUtils.GetName(tradeCost.m_Resource), (object) tradeCost.m_BuyCost, (object) tradeCost.m_SellCost)));
      }
    }

    private bool HasTradePartnerInfo(Entity entity, Entity prefab)
    {
      Game.Companies.StorageCompany component1;
      if (this.EntityManager.TryGetComponent<Game.Companies.StorageCompany>(entity, out component1) && component1.m_LastTradePartner != InfoList.Item.kNullEntity)
        return true;
      BuyingCompany component2;
      return this.EntityManager.TryGetComponent<BuyingCompany>(entity, out component2) && component2.m_LastTradePartner != InfoList.Item.kNullEntity;
    }

    private void UpdateTradePartnerInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Game.Companies.StorageCompany component;
      string debugName;
      Entity lastTradePartner;
      if (this.EntityManager.TryGetComponent<Game.Companies.StorageCompany>(entity, out component) && component.m_LastTradePartner != InfoList.Item.kNullEntity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        debugName = this.m_NameSystem.GetDebugName(component.m_LastTradePartner);
        lastTradePartner = component.m_LastTradePartner;
      }
      else
      {
        BuyingCompany componentData = this.EntityManager.GetComponentData<BuyingCompany>(entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        debugName = this.m_NameSystem.GetDebugName(componentData.m_LastTradePartner);
        lastTradePartner = componentData.m_LastTradePartner;
      }
      info.label = "Trade Partner";
      info.value = debugName;
      info.target = lastTradePartner;
    }

    private bool HasWarehouseInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Game.Companies.StorageCompany>(entity) && this.EntityManager.HasComponent<StorageCompanyData>(prefab);
    }

    private void UpdateWarehouseInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      StorageCompanyData componentData = this.EntityManager.GetComponentData<StorageCompanyData>(prefab);
      DynamicBuffer<TradeCost> buffer1 = this.EntityManager.GetBuffer<TradeCost>(entity, true);
      DynamicBuffer<Game.Economy.Resources> buffer2 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      TradeCost tradeCost = EconomyUtils.GetTradeCost(componentData.m_StoredResources, buffer1);
      int resources = EconomyUtils.GetResources(componentData.m_StoredResources, buffer2);
      info.label = "Warehouse - ";
      info.value = "Stores: " + EconomyUtils.GetName(componentData.m_StoredResources) + " (" + (object) resources + "). Buy Cost: " + tradeCost.m_BuyCost.ToString("F1") + ". Sell Cost: " + tradeCost.m_SellCost.ToString("F1");
      info.target = InfoList.Item.kNullEntity;
    }

    private bool HasCompanyEconomyInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<CompanyData>(entity);
    }

    private void UpdateCompanyEconomyInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      this.EntityManager.HasComponent<ServiceAvailable>(entity);
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_4.GetSingleton<EconomyParameterData>();
      DynamicBuffer<Game.Economy.Resources> buffer1 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      this.EntityManager.GetComponentData<IndustrialProcessData>(prefab);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Game.Vehicles.DeliveryTruck> roComponentLookup2 = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<LayoutElement> elementRoBufferLookup = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup;
      DynamicBuffer<OwnedVehicle> buffer2;
      int worth = !this.EntityManager.TryGetBuffer<OwnedVehicle>(entity, true, out buffer2) ? EconomyUtils.GetCompanyTotalWorth(buffer1, prefabs, roComponentLookup1) : EconomyUtils.GetCompanyTotalWorth(buffer1, buffer2, elementRoBufferLookup, roComponentLookup2, prefabs, roComponentLookup1);
      info.label = "Company Economy - ";
      // ISSUE: reference to a compiler-generated method
      info.value = this.WorthToString(worth) + " (" + (object) worth + ").";
      info.target = InfoList.Item.kNullEntity;
    }

    private string WorthToString(int worth)
    {
      if (worth < -7500)
        return "Bankrupt ";
      if (worth < -1000)
        return "Poor ";
      return worth < 1000 ? "Stable " : "Wealthy ";
    }

    private bool HasCompanyProfitInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.HasComponent<PropertyRenter>(entity) && this.EntityManager.HasComponent<Employee>(entity);
    }

    private void UpdateCompanyProfitInfo(Entity entity, Entity prefab, InfoList info)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      EconomyParameterData singleton = this.__query_746694603_4.GetSingleton<EconomyParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Citizen> roComponentLookup2 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
      bool isIndustrial = !this.EntityManager.HasComponent<ServiceAvailable>(entity);
      bool flag = this.EntityManager.HasComponent<Game.Companies.ExtractorCompany>(entity);
      IndustrialProcessData component1;
      if (!this.EntityManager.TryGetComponent<IndustrialProcessData>(prefab, out component1))
        return;
      float buildingEfficiency = 0.0f;
      float concentration = 0.0f;
      PropertyRenter component2;
      if (this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component2))
      {
        DynamicBuffer<Efficiency> buffer1;
        buildingEfficiency = this.EntityManager.TryGetBuffer<Efficiency>(component2.m_Property, true, out buffer1) ? BuildingUtils.GetEfficiency(buffer1) : 1f;
        Attached component3;
        DynamicBuffer<Game.Areas.SubArea> buffer2;
        if (this.EntityManager.TryGetComponent<Attached>(component2.m_Property, out component3) && this.EntityManager.TryGetBuffer<Game.Areas.SubArea>(component3.m_Parent, true, out buffer2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
          // ISSUE: reference to a compiler-generated method
          ExtractorCompanySystem.GetBestConcentration(component1.m_Output.m_Resource, buffer2, this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup, this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup, this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup, this.__query_746694603_5.GetSingleton<ExtractorParameterData>(), this.m_ResourceSystem.GetPrefabs(), this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup, out concentration, out int _);
        }
      }
      DynamicBuffer<Employee> buffer = this.EntityManager.GetBuffer<Employee>(entity, true);
      // ISSUE: reference to a compiler-generated method
      int totalWage = WorkProviderSystem.CalculateTotalWage(buffer, ref singleton);
      int productionPerDay = EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, isIndustrial, buffer, component1, prefabs, roComponentLookup1, roComponentLookup2, ref singleton);
      float companyProfitPerDay = (float) EconomyUtils.GetCompanyProfitPerDay(buildingEfficiency, isIndustrial, buffer, component1, prefabs, roComponentLookup1, roComponentLookup2, ref singleton);
      float num = (isIndustrial ? EconomyUtils.GetIndustrialPrice(component1.m_Output.m_Resource, prefabs, ref roComponentLookup1) : EconomyUtils.GetMarketPrice(component1.m_Output.m_Resource, prefabs, ref roComponentLookup1)) * (float) component1.m_Output.m_Amount;
      info.label = (isIndustrial ? (flag ? "Extractor" : "Industrial") : "Commercial") + " Company Profit";
      info.Add(flag ? new InfoList.Item(string.Format("efficiency:{0}% concentration:{1}", (object) (float) ((double) buildingEfficiency * 100.0), (object) concentration)) : new InfoList.Item(string.Format("efficiency:{0}%", (object) (float) ((double) buildingEfficiency * 100.0))));
      info.Add(new InfoList.Item("Wages: " + (object) totalWage));
      info.Add(new InfoList.Item(string.Format("Production Per Day: {0} * {1}({2})={3}", (object) productionPerDay, (object) EconomyUtils.GetNameFixed(component1.m_Output.m_Resource), (object) num, (object) (float) ((double) productionPerDay * (double) num))));
      if (component1.m_Input1.m_Resource != Resource.NoResource)
        info.Add(new InfoList.Item(string.Format("Input1: {0}*{1}({2})", (object) component1.m_Input1.m_Amount, (object) EconomyUtils.GetNameFixed(component1.m_Input1.m_Resource), (object) EconomyUtils.GetIndustrialPrice(component1.m_Input1.m_Resource, prefabs, ref roComponentLookup1))));
      if (component1.m_Input2.m_Resource != Resource.NoResource)
        info.Add(new InfoList.Item(string.Format("Input2: {0}*{1}({2})", (object) component1.m_Input2.m_Amount, (object) EconomyUtils.GetNameFixed(component1.m_Input2.m_Resource), (object) EconomyUtils.GetIndustrialPrice(component1.m_Input2.m_Resource, prefabs, ref roComponentLookup1))));
      if (component1.m_Output.m_Resource != Resource.NoResource)
        info.Add(new InfoList.Item(string.Format("Output: {0}*{1}({2})", (object) component1.m_Output.m_Amount, (object) EconomyUtils.GetNameFixed(component1.m_Output.m_Resource), (object) num)));
      info.Add(new InfoList.Item("Profit Per Day(wages deducted): " + (object) companyProfitPerDay));
    }

    private bool HasServiceCompanyInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.HasComponent<ServiceAvailable>(entity) && this.EntityManager.HasComponent<Game.Economy.Resources>(entity);
    }

    private void UpdateServiceCompanyInfo(Entity entity, Entity prefab, InfoList info)
    {
      ServiceAvailable componentData1 = this.EntityManager.GetComponentData<ServiceAvailable>(entity);
      ServiceCompanyData componentData2 = this.EntityManager.GetComponentData<ServiceCompanyData>(prefab);
      DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      info.label = "Service Company";
      IndustrialProcessData component1;
      if (this.EntityManager.HasComponent<Game.Companies.ProcessingCompany>(entity) && this.EntityManager.TryGetComponent<IndustrialProcessData>(prefab, out component1))
      {
        Resource resource = component1.m_Output.m_Resource;
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Service: " + (componentData1.m_ServiceAvailable.ToString() + "/" + componentData2.m_MaxService.ToString() + "(" + this.ServicesToString(componentData1.m_ServiceAvailable, componentData2.m_MaxService) + ")")));
        info.Add(new InfoList.Item("Provide Resource Storage: " + EconomyUtils.GetName(resource) + " (" + (object) EconomyUtils.GetResources(resource, buffer) + ")"));
        PropertyRenter component2;
        LodgingProvider component3;
        if (this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component2) && component2.m_Property != InfoList.Item.kNullEntity && this.EntityManager.TryGetComponent<LodgingProvider>(entity, out component3) && resource == Resource.Lodging)
        {
          Entity property = component2.m_Property;
          EntityManager entityManager = this.EntityManager;
          Entity prefab1 = entityManager.GetComponentData<PrefabRef>(property).m_Prefab;
          entityManager = this.EntityManager;
          SpawnableBuildingData componentData3 = entityManager.GetComponentData<SpawnableBuildingData>(prefab1);
          entityManager = this.EntityManager;
          BuildingPropertyData componentData4 = entityManager.GetComponentData<BuildingPropertyData>(prefab1);
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated method
          int roomCount = LodgingProviderSystem.GetRoomCount(entityManager.GetComponentData<Game.Prefabs.BuildingData>(prefab1).m_LotSize, (int) componentData3.m_Level, componentData4);
          info.Add(new InfoList.Item("Lodging rooms free: " + (object) component3.m_FreeRooms + "/" + (object) roomCount));
        }
      }
      BuyingCompany component4;
      if (!this.EntityManager.TryGetComponent<BuyingCompany>(entity, out component4))
        return;
      info.Add(new InfoList.Item("Trip Length: " + (object) component4.m_MeanInputTripLength));
    }

    private string ServicesToString(int services, int maxServices)
    {
      float num = (float) services / (float) maxServices;
      if (services <= 0)
        return "Overworked";
      if ((double) num < 0.20000000298023224)
        return "Busy";
      return (double) num < 0.800000011920929 ? "Operational" : "Low on customers";
    }

    private bool HasExtractorCompanyInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.HasComponent<Game.Companies.ExtractorCompany>(entity) && this.EntityManager.HasComponent<IndustrialProcessData>(prefab);
    }

    private void UpdateExtractorCompanyInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<Game.Economy.Resources> buffer1 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      IndustrialProcessData componentData = this.EntityManager.GetComponentData<IndustrialProcessData>(prefab);
      info.label = "Extractor Company";
      Resource resource = componentData.m_Output.m_Resource;
      info.Add(new InfoList.Item("Produces: " + EconomyUtils.GetName(resource) + " (" + (object) EconomyUtils.GetResources(resource, buffer1) + ")"));
      PropertyRenter component1;
      Attached component2;
      IndustrialProcessData component3;
      if (!this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component1) || !(component1.m_Property != InfoList.Item.kNullEntity) || !this.EntityManager.TryGetComponent<Attached>(component1.m_Property, out component2) || !this.EntityManager.TryGetComponent<WorkplaceData>(prefab, out WorkplaceData _) || !this.EntityManager.TryGetComponent<PrefabRef>(component1.m_Property, out PrefabRef _) || !this.EntityManager.TryGetComponent<IndustrialProcessData>(prefab, out component3))
        return;
      DynamicBuffer<Game.Areas.SubArea> buffer2 = this.EntityManager.GetBuffer<Game.Areas.SubArea>(component2.m_Parent, true);
      // ISSUE: reference to a compiler-generated field
      ExtractorParameterData singleton = this.__query_746694603_5.GetSingleton<ExtractorParameterData>();
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_4.GetSingleton<EconomyParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Game.Areas.Lot> roComponentLookup1 = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Geometry> roComponentLookup2 = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Extractor> roComponentLookup3 = this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.World.GetOrCreateSystemManaged<ResourceSystem>().GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup4 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup5 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ExtractorAreaData> roComponentLookup6 = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated method
      double area = (double) ExtractorAISystem.GetArea(buffer2, roComponentLookup1, roComponentLookup2);
      // ISSUE: reference to a compiler-generated method
      double resourcesInArea = (double) ExtractorAISystem.GetResourcesInArea(entity, buffer2, roComponentLookup3);
      // ISSUE: reference to a compiler-generated method
      ExtractorCompanySystem.GetBestConcentration(component3.m_Output.m_Resource, buffer2, roComponentLookup3, roComponentLookup5, roComponentLookup6, singleton, prefabs, roComponentLookup4, out float _, out int _);
      IndustrialProcessData processData = componentData;
      CompanyUtils.GetExtractorFittingWorkers((float) area, 1f, processData);
    }

    private bool HasProcessingCompanyInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.HasComponent<Game.Companies.ProcessingCompany>(entity) && this.EntityManager.HasComponent<IndustrialProcessData>(prefab);
    }

    private void UpdateProcessingCompanyInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<Game.Economy.Resources> buffer1 = this.EntityManager.GetBuffer<Game.Economy.Resources>(entity, true);
      IndustrialProcessData componentData = this.EntityManager.GetComponentData<IndustrialProcessData>(prefab);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Citizen> roComponentLookup = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
      info.label = "Processing Company";
      Resource resource1 = componentData.m_Input1.m_Resource;
      Resource resource2 = componentData.m_Input2.m_Resource;
      Resource resource3 = componentData.m_Output.m_Resource;
      info.Add(new InfoList.Item("In: " + EconomyUtils.GetName(resource1) + " (" + (object) EconomyUtils.GetResources(resource1, buffer1) + ")"));
      if (resource2 != Resource.NoResource)
        info.Add(new InfoList.Item("In: " + EconomyUtils.GetName(resource2) + " (" + (object) EconomyUtils.GetResources(resource2, buffer1) + ")"));
      info.Add(new InfoList.Item("Out: " + EconomyUtils.GetName(resource3) + " (" + (object) EconomyUtils.GetResources(resource3, buffer1) + ")"));
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_4.GetSingleton<EconomyParameterData>();
      EntityManager entityManager = this.EntityManager;
      if (!entityManager.HasComponent<ServiceAvailable>(entity))
      {
        entityManager = this.EntityManager;
        entityManager.HasComponent<Game.Companies.ExtractorCompany>(entity);
      }
      DynamicBuffer<Employee> buffer2;
      PropertyRenter component1;
      PrefabRef component2;
      if (this.EntityManager.TryGetBuffer<Employee>(entity, true, out buffer2) && this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component1) && this.EntityManager.TryGetComponent<PrefabRef>(component1.m_Property, out component2) && this.EntityManager.TryGetComponent<WorkplaceData>(prefab, out WorkplaceData _) && this.EntityManager.TryGetComponent<SpawnableBuildingData>((Entity) component2, out SpawnableBuildingData _))
      {
        // ISSUE: reference to a compiler-generated method
        float workforce = WorkProviderSystem.GetWorkforce(buffer2, roComponentLookup);
        info.Add(new InfoList.Item(string.Format("Workforce:{0}", (object) workforce)));
        DynamicBuffer<Efficiency> buffer3;
        float num = this.EntityManager.TryGetBuffer<Efficiency>(component1.m_Property, true, out buffer3) ? BuildingUtils.GetEfficiency(buffer3) : 1f;
        info.Add(new InfoList.Item(string.Format("Building Efficiency:{0}", (object) num)));
      }
      BuyingCompany component3;
      if (!this.EntityManager.TryGetComponent<BuyingCompany>(entity, out component3))
        return;
      info.Add(new InfoList.Item("Trip Length: " + (object) component3.m_MeanInputTripLength));
    }

    private bool HasOwnerInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Owner>(entity);
    }

    private void UpdateOwnerInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Owner componentData = this.EntityManager.GetComponentData<Owner>(entity);
      info.label = "Owner";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(componentData.m_Owner);
      info.target = componentData.m_Owner;
    }

    private bool HasKeeperInfo(Entity entity, Entity prefab)
    {
      Game.Vehicles.PersonalCar component;
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.TryGetComponent<Game.Vehicles.PersonalCar>(entity, out component) && component.m_Keeper != InfoList.Item.kNullEntity;
    }

    private void UpdateKeeperInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Game.Vehicles.PersonalCar componentData = this.EntityManager.GetComponentData<Game.Vehicles.PersonalCar>(entity);
      info.label = "Keeper";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(componentData.m_Keeper);
      info.target = componentData.m_Keeper;
    }

    private bool HasControllerInfo(Entity entity, Entity prefab)
    {
      Controller component;
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.TryGetComponent<Controller>(entity, out component) && component.m_Controller != InfoList.Item.kNullEntity;
    }

    private void UpdateControllerInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Controller componentData = this.EntityManager.GetComponentData<Controller>(entity);
      info.label = "Controller";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(componentData.m_Controller);
      info.target = componentData.m_Controller;
    }

    private bool HasTransferRequestInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<StorageTransferRequest>(entity);
    }

    private void UpdateTransferRequestInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<StorageTransferRequest> buffer = this.EntityManager.GetBuffer<StorageTransferRequest>(entity, true);
      info.label = "Transfer requests";
      for (int index = 0; index < buffer.Length; ++index)
      {
        StorageTransferRequest storageTransferRequest = buffer[index];
        info.Add(new InfoList.Item(string.Format("{0} {1} {2} {3} {4}", (object) storageTransferRequest.m_Amount, (object) EconomyUtils.GetName(storageTransferRequest.m_Resource), (storageTransferRequest.m_Flags & StorageTransferFlags.Incoming) != (StorageTransferFlags) 0 ? (object) " from " : (object) " to ", (object) storageTransferRequest.m_Target.Index, (storageTransferRequest.m_Flags & StorageTransferFlags.Car) != (StorageTransferFlags) 0 ? (object) "(C)" : (object) "")));
      }
    }

    private bool HasPassengerInfo(Entity entity, Entity prefab)
    {
      if (!this.EntityManager.HasComponent<Vehicle>(entity))
        return false;
      int num = 0;
      PersonalCarData component1;
      if (this.EntityManager.TryGetComponent<PersonalCarData>(prefab, out component1))
      {
        num = component1.m_PassengerCapacity;
      }
      else
      {
        PublicTransportVehicleData component2;
        if (this.EntityManager.TryGetComponent<PublicTransportVehicleData>(prefab, out component2))
        {
          num = component2.m_PassengerCapacity;
        }
        else
        {
          AmbulanceData component3;
          if (this.EntityManager.TryGetComponent<AmbulanceData>(prefab, out component3))
          {
            num = component3.m_PatientCapacity;
          }
          else
          {
            HearseData component4;
            if (this.EntityManager.TryGetComponent<HearseData>(prefab, out component4))
            {
              num = component4.m_CorpseCapacity;
            }
            else
            {
              PoliceCarData component5;
              if (this.EntityManager.TryGetComponent<PoliceCarData>(prefab, out component5))
              {
                num = component5.m_CriminalCapacity;
              }
              else
              {
                TaxiData component6;
                if (this.EntityManager.TryGetComponent<TaxiData>(prefab, out component6))
                  num = component6.m_PassengerCapacity;
              }
            }
          }
        }
      }
      return num > 0;
    }

    private void UpdatePassengerInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      int num1 = 0;
      int num2 = 0;
      PersonalCarData component1;
      if (this.EntityManager.TryGetComponent<PersonalCarData>(prefab, out component1))
      {
        num1 = component1.m_PassengerCapacity;
      }
      else
      {
        PublicTransportVehicleData component2;
        if (this.EntityManager.TryGetComponent<PublicTransportVehicleData>(prefab, out component2))
        {
          num1 = component2.m_PassengerCapacity;
        }
        else
        {
          AmbulanceData component3;
          if (this.EntityManager.TryGetComponent<AmbulanceData>(prefab, out component3))
          {
            num1 = component3.m_PatientCapacity;
          }
          else
          {
            HearseData component4;
            if (this.EntityManager.TryGetComponent<HearseData>(prefab, out component4))
            {
              num1 = component4.m_CorpseCapacity;
            }
            else
            {
              PoliceCarData component5;
              if (this.EntityManager.TryGetComponent<PoliceCarData>(prefab, out component5))
              {
                num1 = component5.m_CriminalCapacity;
              }
              else
              {
                TaxiData component6;
                if (this.EntityManager.TryGetComponent<TaxiData>(prefab, out component6))
                  num1 = component6.m_PassengerCapacity;
              }
            }
          }
        }
      }
      DynamicBuffer<Passenger> buffer;
      if (this.EntityManager.TryGetBuffer<Passenger>(entity, true, out buffer))
        num2 = buffer.Length;
      info.label = "Passengers";
      info.value = num2;
      info.max = num1;
    }

    private bool HasPersonalCarInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.PersonalCar>(entity) && this.EntityManager.HasComponent<PersonalCarData>(prefab);
    }

    private void UpdatePersonalCarInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.PersonalCar componentData = this.EntityManager.GetComponentData<Game.Vehicles.PersonalCar>(entity);
      info.label = "Personal Car";
      if (this.EntityManager.HasComponent<ParkedCar>(entity))
        info.Add(new InfoList.Item("Parked"));
      if ((componentData.m_State & PersonalCarFlags.Boarding) != (PersonalCarFlags) 0)
        info.Add(new InfoList.Item("Boarding"));
      else if ((componentData.m_State & PersonalCarFlags.Disembarking) != (PersonalCarFlags) 0)
        info.Add(new InfoList.Item("Disembarking"));
      else if ((componentData.m_State & PersonalCarFlags.Transporting) != (PersonalCarFlags) 0)
        info.Add(new InfoList.Item("Transporting"));
      if ((componentData.m_State & PersonalCarFlags.DummyTraffic) != (PersonalCarFlags) 0)
        info.Add(new InfoList.Item("Dummy Traffic"));
      if ((componentData.m_State & PersonalCarFlags.HomeTarget) == (PersonalCarFlags) 0)
        return;
      info.Add(new InfoList.Item("Home Target"));
    }

    private bool HasDeliveryTruckInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.DeliveryTruck>(entity) && this.EntityManager.HasComponent<DeliveryTruckData>(prefab);
    }

    private void UpdateDeliveryTruckInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.DeliveryTruck componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.DeliveryTruck>(entity);
      DeliveryTruckData componentData2 = this.EntityManager.GetComponentData<DeliveryTruckData>(prefab);
      Resource resource = Resource.NoResource;
      int num1 = 0;
      int num2 = 0;
      DynamicBuffer<LayoutElement> buffer;
      if (this.EntityManager.TryGetBuffer<LayoutElement>(entity, true, out buffer) && buffer.Length != 0)
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity vehicle = buffer[index].m_Vehicle;
          Game.Vehicles.DeliveryTruck component1;
          if (this.EntityManager.TryGetComponent<Game.Vehicles.DeliveryTruck>(vehicle, out component1))
          {
            resource |= component1.m_Resource;
            if ((component1.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0)
              num1 += component1.m_Amount;
            DeliveryTruckData component2;
            if (this.EntityManager.TryGetComponent<DeliveryTruckData>(this.EntityManager.GetComponentData<PrefabRef>(vehicle).m_Prefab, out component2))
              num2 += component2.m_CargoCapacity;
          }
        }
      }
      else
      {
        resource = componentData1.m_Resource;
        if ((componentData1.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0)
          num1 = componentData1.m_Amount;
        num2 = componentData2.m_CargoCapacity;
      }
      bool flag1 = (componentData1.m_State & DeliveryTruckFlags.StorageTransfer) > (DeliveryTruckFlags) 0;
      bool flag2 = (componentData1.m_State & DeliveryTruckFlags.Buying) > (DeliveryTruckFlags) 0;
      bool flag3 = (componentData1.m_State & DeliveryTruckFlags.Returning) > (DeliveryTruckFlags) 0;
      bool flag4 = (componentData1.m_State & DeliveryTruckFlags.Delivering) > (DeliveryTruckFlags) 0;
      info.label = "Delivery Truck";
      if ((componentData1.m_State & DeliveryTruckFlags.DummyTraffic) != (DeliveryTruckFlags) 0)
        info.Add(new InfoList.Item("Dummy Traffic"));
      info.Add(new InfoList.Item("Cargo: " + (object) num1 + "/" + (object) num2));
      Game.Common.Target component;
      if (this.EntityManager.TryGetComponent<Game.Common.Target>(entity, out component))
      {
        if (flag2)
        {
          string str1 = flag3 ? "Bought " : "Buying ";
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          string str2 = flag3 ? string.Empty : "from " + this.m_NameSystem.GetDebugName(component.m_Target);
          Entity entity1 = flag3 ? InfoList.Item.kNullEntity : component.m_Target;
          info.Add(new InfoList.Item(str1 + (object) resource + str2, entity1));
        }
        else if (flag1)
        {
          string str3 = flag3 ? "Exported " : "Exporting ";
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          string str4 = flag3 ? string.Empty : "to " + this.m_NameSystem.GetDebugName(component.m_Target);
          Entity entity2 = flag3 ? InfoList.Item.kNullEntity : component.m_Target;
          info.Add(new InfoList.Item(str3 + (object) resource + str4, entity2));
        }
        else if (flag4)
        {
          string str5 = flag3 ? "Delivered " : "Delivering ";
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          string str6 = flag3 ? string.Empty : "to " + this.m_NameSystem.GetDebugName(component.m_Target);
          Entity entity3 = flag3 ? InfoList.Item.kNullEntity : component.m_Target;
          info.Add(new InfoList.Item(str5 + (object) resource + str6, entity3));
        }
        else
        {
          string str7 = flag3 ? "Transported " : "Transporting ";
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          string str8 = flag3 ? string.Empty : "to " + this.m_NameSystem.GetDebugName(component.m_Target);
          Entity entity4 = flag3 ? InfoList.Item.kNullEntity : component.m_Target;
          info.Add(new InfoList.Item(str7 + (object) resource + str8, entity4));
        }
      }
      if (!flag3)
        return;
      info.Add(new InfoList.Item("Returning"));
    }

    private bool HasAmbulanceInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.Ambulance>(entity) && this.EntityManager.HasComponent<AmbulanceData>(prefab);
    }

    private void UpdateAmbulanceInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.Ambulance componentData = this.EntityManager.GetComponentData<Game.Vehicles.Ambulance>(entity);
      info.label = "Ambulance";
      Game.Common.Target component;
      if (!this.EntityManager.TryGetComponent<Game.Common.Target>(entity, out component))
        return;
      if (componentData.m_TargetPatient != InfoList.Item.kNullEntity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Patient" + this.m_NameSystem.GetDebugName(componentData.m_TargetPatient), componentData.m_TargetPatient));
      }
      if ((componentData.m_State & AmbulanceFlags.Returning) != (AmbulanceFlags) 0)
        info.Add(new InfoList.Item("Returning"));
      else if ((componentData.m_State & AmbulanceFlags.Transporting) != (AmbulanceFlags) 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Transporting to: " + this.m_NameSystem.GetDebugName(component.m_Target), component.m_Target));
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Picking up from: " + this.m_NameSystem.GetDebugName(component.m_Target), component.m_Target));
      }
    }

    private bool HasHearseInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.Hearse>(entity) && this.EntityManager.HasComponent<HearseData>(prefab);
    }

    private void UpdateHearseInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.Hearse componentData = this.EntityManager.GetComponentData<Game.Vehicles.Hearse>(entity);
      info.label = "Hearse";
      Game.Common.Target component;
      if (!this.EntityManager.TryGetComponent<Game.Common.Target>(entity, out component))
        return;
      if (componentData.m_TargetCorpse != InfoList.Item.kNullEntity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Body" + this.m_NameSystem.GetDebugName(componentData.m_TargetCorpse), componentData.m_TargetCorpse));
      }
      if ((componentData.m_State & HearseFlags.Returning) != (HearseFlags) 0)
        info.Add(new InfoList.Item("Returning"));
      else if ((componentData.m_State & HearseFlags.Transporting) != (HearseFlags) 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Transporting to" + this.m_NameSystem.GetDebugName(component.m_Target), component.m_Target));
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Picking up from" + this.m_NameSystem.GetDebugName(component.m_Target), component.m_Target));
      }
    }

    private bool HasGarbageTruckInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.GarbageTruck>(entity) && this.EntityManager.HasComponent<GarbageTruckData>(prefab);
    }

    private void UpdateGarbageTruckInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.GarbageTruck componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.GarbageTruck>(entity);
      GarbageTruckData componentData2 = this.EntityManager.GetComponentData<GarbageTruckData>(prefab);
      info.label = "Garbage Truck";
      info.Add(new InfoList.Item("Capacity: " + (object) componentData1.m_Garbage + "/" + (object) componentData2.m_GarbageCapacity));
      if ((componentData1.m_State & GarbageTruckFlags.Unloading) != (GarbageTruckFlags) 0)
      {
        info.Add(new InfoList.Item("Unloading"));
      }
      else
      {
        if ((componentData1.m_State & GarbageTruckFlags.Returning) == (GarbageTruckFlags) 0)
          return;
        info.Add(new InfoList.Item("Returning"));
      }
    }

    private bool HasPublicTransportInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.PublicTransport>(entity) && this.EntityManager.HasComponent<PublicTransportVehicleData>(prefab);
    }

    private void UpdatePublicTransportInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.PublicTransport componentData = this.EntityManager.GetComponentData<Game.Vehicles.PublicTransport>(entity);
      info.label = "Public Transport";
      if ((componentData.m_State & PublicTransportFlags.DummyTraffic) != (PublicTransportFlags) 0)
        info.Add(new InfoList.Item("Dummy Traffic"));
      CurrentRoute component1;
      if (this.EntityManager.TryGetComponent<CurrentRoute>(entity, out component1))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Line: " + this.m_NameSystem.GetDebugName(component1.m_Route), component1.m_Route));
      }
      if ((componentData.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0)
        info.Add(new InfoList.Item("Returning"));
      else if ((componentData.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
      {
        info.Add(new InfoList.Item("Boarding"));
        // ISSUE: reference to a compiler-generated field
        if (this.m_SimulationSystem.frameIndex < componentData.m_DepartureFrame)
        {
          // ISSUE: reference to a compiler-generated field
          int num = Mathf.CeilToInt((float) (componentData.m_DepartureFrame - this.m_SimulationSystem.frameIndex) / 60f);
          info.Add(new InfoList.Item("Departure: " + (object) num + "s"));
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          Entity passengerWaiting = this.GetPassengerWaiting(entity);
          if (passengerWaiting != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Waiting for: " + this.m_NameSystem.GetDebugName(passengerWaiting), passengerWaiting));
          }
        }
      }
      else if ((componentData.m_State & PublicTransportFlags.EnRoute) != (PublicTransportFlags) 0)
        info.Add(new InfoList.Item("En route"));
      PublicTransportVehicleData component2;
      if (this.EntityManager.TryGetComponent<PublicTransportVehicleData>(prefab, out component2))
      {
        if ((componentData.m_State & PublicTransportFlags.RequiresMaintenance) != (PublicTransportFlags) 0)
        {
          info.Add(new InfoList.Item("Maintenance scheduled"));
        }
        else
        {
          Odometer component3;
          if ((double) component2.m_MaintenanceRange > 0.10000000149011612 && this.EntityManager.TryGetComponent<Odometer>(entity, out component3))
          {
            int num1 = Mathf.RoundToInt(component2.m_MaintenanceRange * (1f / 1000f));
            int num2 = math.max(0, Mathf.RoundToInt((float) (((double) component2.m_MaintenanceRange - (double) component3.m_Distance) * (1.0 / 1000.0))));
            info.Add(new InfoList.Item("Remaining range: " + (object) num2 + "/" + (object) num1));
          }
        }
      }
      int nextWaypointIndex;
      float segmentPosition;
      // ISSUE: reference to a compiler-generated method
      if (!this.GetRoutePosition(entity, out nextWaypointIndex, out segmentPosition))
        return;
      info.Add(new InfoList.Item("Route waypoint index: " + (object) nextWaypointIndex));
      info.Add(new InfoList.Item("Route segment position: " + (object) Mathf.RoundToInt(segmentPosition * 100f) + "%"));
    }

    private Entity GetPassengerWaiting(Entity vehicleEntity)
    {
      DynamicBuffer<LayoutElement> buffer;
      if (!this.EntityManager.TryGetBuffer<LayoutElement>(vehicleEntity, true, out buffer))
      {
        // ISSUE: reference to a compiler-generated method
        return this.GetPassengerWaiting2(vehicleEntity);
      }
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        Entity passengerWaiting2 = this.GetPassengerWaiting2(buffer[index].m_Vehicle);
        if (passengerWaiting2 != Entity.Null)
          return passengerWaiting2;
      }
      return Entity.Null;
    }

    private Entity GetPassengerWaiting2(Entity vehicleEntity)
    {
      DynamicBuffer<Passenger> buffer;
      if (this.EntityManager.TryGetBuffer<Passenger>(vehicleEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity passenger = buffer[index].m_Passenger;
          CurrentVehicle component;
          if (this.EntityManager.TryGetComponent<CurrentVehicle>(passenger, out component) && (component.m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
            return passenger;
        }
      }
      return Entity.Null;
    }

    private bool GetRoutePosition(
      Entity transportVehicle,
      out int nextWaypointIndex,
      out float segmentPosition)
    {
      CurrentRoute component1;
      if (this.EntityManager.TryGetComponent<CurrentRoute>(transportVehicle, out component1))
      {
        PathInformation component2;
        Waypoint component3;
        DynamicBuffer<RouteSegment> buffer1;
        if (this.EntityManager.TryGetComponent<PathInformation>(transportVehicle, out component2) && this.EntityManager.TryGetComponent<Waypoint>(component2.m_Destination, out component3) && this.EntityManager.TryGetBuffer<RouteSegment>(component1.m_Route, true, out buffer1))
        {
          nextWaypointIndex = component3.m_Index;
          int index = math.select(nextWaypointIndex - 1, buffer1.Length - 1, nextWaypointIndex == 0);
          DynamicBuffer<PathElement> buffer2;
          if (this.EntityManager.TryGetBuffer<PathElement>(buffer1[index].m_Segment, true, out buffer2) && buffer2.Length != 0)
          {
            int num = 0;
            PathOwner component4;
            DynamicBuffer<PathElement> buffer3;
            if (this.EntityManager.TryGetComponent<PathOwner>(transportVehicle, out component4) && this.EntityManager.TryGetBuffer<PathElement>(transportVehicle, true, out buffer3))
              num += math.max(0, buffer3.Length - component4.m_ElementIndex);
            DynamicBuffer<CarNavigationLane> buffer4;
            if (this.EntityManager.TryGetBuffer<CarNavigationLane>(transportVehicle, true, out buffer4))
            {
              num += buffer4.Length;
            }
            else
            {
              DynamicBuffer<TrainNavigationLane> buffer5;
              if (this.EntityManager.TryGetBuffer<TrainNavigationLane>(transportVehicle, true, out buffer5))
              {
                num += buffer5.Length;
              }
              else
              {
                DynamicBuffer<WatercraftNavigationLane> buffer6;
                if (this.EntityManager.TryGetBuffer<WatercraftNavigationLane>(transportVehicle, true, out buffer6))
                {
                  num += buffer6.Length;
                }
                else
                {
                  DynamicBuffer<AircraftNavigationLane> buffer7;
                  if (this.EntityManager.TryGetBuffer<AircraftNavigationLane>(transportVehicle, true, out buffer7))
                    num += buffer7.Length;
                }
              }
            }
            segmentPosition = math.saturate((float) (buffer2.Length - num) / (float) buffer2.Length);
            return true;
          }
        }
        Game.Common.Target component5;
        DynamicBuffer<RouteWaypoint> buffer8;
        if (this.EntityManager.TryGetComponent<Game.Common.Target>(transportVehicle, out component5) && this.EntityManager.TryGetComponent<Waypoint>(component5.m_Target, out component3) && this.EntityManager.TryGetBuffer<RouteWaypoint>(component1.m_Route, true, out buffer8))
        {
          nextWaypointIndex = component3.m_Index;
          int index = math.select(nextWaypointIndex - 1, buffer8.Length - 1, nextWaypointIndex == 0);
          RouteWaypoint routeWaypoint = buffer8[index];
          Game.Objects.Transform component6;
          Position component7;
          Position component8;
          if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(transportVehicle, out component6) && this.EntityManager.TryGetComponent<Position>(routeWaypoint.m_Waypoint, out component7) && this.EntityManager.TryGetComponent<Position>(component5.m_Target, out component8))
          {
            float num1 = math.distance(component6.m_Position, component8.m_Position);
            float num2 = math.max(1f, math.distance(component7.m_Position, component8.m_Position));
            segmentPosition = math.saturate((num2 - num1) / num2);
            return true;
          }
        }
      }
      nextWaypointIndex = 0;
      segmentPosition = 0.0f;
      return false;
    }

    private bool HasCargoTransportInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.CargoTransport>(entity) && this.EntityManager.HasComponent<CargoTransportVehicleData>(prefab);
    }

    private void UpdateCargoTransportInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.CargoTransport componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.CargoTransport>(entity);
      CargoTransportVehicleData componentData2 = this.EntityManager.GetComponentData<CargoTransportVehicleData>(prefab);
      info.label = "Cargo Transport";
      if ((componentData1.m_State & CargoTransportFlags.DummyTraffic) != (CargoTransportFlags) 0)
        info.Add(new InfoList.Item("Dummy Traffic"));
      CurrentRoute component1;
      if (this.EntityManager.TryGetComponent<CurrentRoute>(entity, out component1))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Route: " + this.m_NameSystem.GetDebugName(component1.m_Route), component1.m_Route));
      }
      if ((componentData1.m_State & CargoTransportFlags.Returning) != (CargoTransportFlags) 0)
        info.Add(new InfoList.Item("Returning"));
      else if ((componentData1.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0)
      {
        info.Add(new InfoList.Item("Loading"));
        // ISSUE: reference to a compiler-generated field
        if (this.m_SimulationSystem.frameIndex < componentData1.m_DepartureFrame)
        {
          // ISSUE: reference to a compiler-generated field
          int num = Mathf.CeilToInt((float) (componentData1.m_DepartureFrame - this.m_SimulationSystem.frameIndex) / 60f);
          info.Add(new InfoList.Item("Departure: " + (object) num + "s"));
        }
      }
      else if ((componentData1.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0)
        info.Add(new InfoList.Item("En route"));
      CargoTransportVehicleData component2;
      if (this.EntityManager.TryGetComponent<CargoTransportVehicleData>(prefab, out component2))
      {
        if ((componentData1.m_State & CargoTransportFlags.RequiresMaintenance) != (CargoTransportFlags) 0)
        {
          info.Add(new InfoList.Item("Maintenance scheduled"));
        }
        else
        {
          Odometer component3;
          if ((double) component2.m_MaintenanceRange > 0.10000000149011612 && this.EntityManager.TryGetComponent<Odometer>(entity, out component3))
          {
            int num1 = Mathf.RoundToInt(component2.m_MaintenanceRange * (1f / 1000f));
            int num2 = math.max(0, Mathf.RoundToInt((float) (((double) component2.m_MaintenanceRange - (double) component3.m_Distance) * (1.0 / 1000.0))));
            info.Add(new InfoList.Item("Remaining range: " + (object) num2 + "/" + (object) num1));
          }
        }
      }
      NativeList<Game.Economy.Resources> target = new NativeList<Game.Economy.Resources>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      int num3 = 0;
      DynamicBuffer<LayoutElement> buffer1;
      if (this.EntityManager.TryGetBuffer<LayoutElement>(entity, true, out buffer1))
      {
        for (int index = 0; index < buffer1.Length; ++index)
        {
          Entity vehicle = buffer1[index].m_Vehicle;
          DynamicBuffer<Game.Economy.Resources> buffer2;
          if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(vehicle, true, out buffer2))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddResources(buffer2, target);
          }
          PrefabRef component4;
          CargoTransportVehicleData component5;
          if (this.EntityManager.TryGetComponent<PrefabRef>(vehicle, out component4) && this.EntityManager.TryGetComponent<CargoTransportVehicleData>(component4.m_Prefab, out component5))
            num3 += component5.m_CargoCapacity;
        }
      }
      else
      {
        DynamicBuffer<Game.Economy.Resources> buffer3;
        if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer3))
        {
          // ISSUE: reference to a compiler-generated method
          this.AddResources(buffer3, target);
        }
        num3 += componentData2.m_CargoCapacity;
      }
      info.Add(new InfoList.Item("Cargo: "));
      int num4 = 0;
      for (int index = 0; index < target.Length; ++index)
      {
        Game.Economy.Resources resources = target[index];
        info.Add(new InfoList.Item(resources.m_Resource.ToString() + " " + (object) resources.m_Amount));
        num4 += resources.m_Amount;
      }
      info.Add(new InfoList.Item("Capacity " + (object) num4 + "/" + (object) num3));
      target.Dispose();
    }

    private void AddResources(DynamicBuffer<Game.Economy.Resources> source, NativeList<Game.Economy.Resources> target)
    {
label_9:
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        Game.Economy.Resources resources1 = source[index1];
        if (resources1.m_Amount != 0)
        {
          for (int index2 = 0; index2 < target.Length; ++index2)
          {
            Game.Economy.Resources resources2 = target[index2];
            if (resources2.m_Resource == resources1.m_Resource)
            {
              resources2.m_Amount += resources1.m_Amount;
              target[index2] = resources2;
              goto label_9;
            }
          }
          target.Add(in resources1);
        }
      }
    }

    private bool HasMaintenanceVehicleInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.MaintenanceVehicle>(entity) && this.EntityManager.HasComponent<MaintenanceVehicleData>(prefab);
    }

    private void UpdateMaintenanceVehicleInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.MaintenanceVehicle componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.MaintenanceVehicle>(entity);
      MaintenanceVehicleData componentData2 = this.EntityManager.GetComponentData<MaintenanceVehicleData>(prefab);
      componentData2.m_MaintenanceCapacity = Mathf.CeilToInt((float) componentData2.m_MaintenanceCapacity * componentData1.m_Efficiency);
      info.label = "Maintenance Vehicle";
      info.Add(new InfoList.Item("Work shift: " + string.Format("{0}%", (object) Mathf.CeilToInt(math.select((float) componentData1.m_Maintained / (float) componentData2.m_MaintenanceCapacity, 0.0f, componentData2.m_MaintenanceCapacity == 0) * 100f))));
      if ((componentData1.m_State & MaintenanceVehicleFlags.ClearingDebris) != (MaintenanceVehicleFlags) 0)
      {
        info.Add(new InfoList.Item("Clearing debris"));
      }
      else
      {
        if ((componentData1.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0)
          return;
        info.Add(new InfoList.Item("Returning"));
      }
    }

    private bool HasPostVanInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.PostVan>(entity) && this.EntityManager.HasComponent<PostVanData>(prefab);
    }

    private void UpdatePostVanInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.PostVan componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.PostVan>(entity);
      PostVanData componentData2 = this.EntityManager.GetComponentData<PostVanData>(prefab);
      info.label = "Post Van";
      info.Add(new InfoList.Item("Mail to deliver: " + (object) componentData1.m_DeliveringMail + "/" + (object) componentData2.m_MailCapacity));
      info.Add(new InfoList.Item("Collected mail: " + (object) componentData1.m_CollectedMail + "/" + (object) componentData2.m_MailCapacity));
      if ((componentData1.m_State & PostVanFlags.Returning) == (PostVanFlags) 0)
        return;
      info.Add(new InfoList.Item("Returning"));
    }

    private bool HasFireEngineInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.FireEngine>(entity) && this.EntityManager.HasComponent<FireEngineData>(prefab) && this.EntityManager.HasComponent<ServiceDispatch>(entity);
    }

    private void UpdateFireEngineInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.FireEngine componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.FireEngine>(entity);
      EntityManager entityManager = this.EntityManager;
      FireEngineData componentData2 = entityManager.GetComponentData<FireEngineData>(prefab);
      entityManager = this.EntityManager;
      DynamicBuffer<ServiceDispatch> buffer = entityManager.GetBuffer<ServiceDispatch>(entity, true);
      info.label = "Fire Engine";
      int num1 = Mathf.CeilToInt(componentData1.m_ExtinguishingAmount);
      int num2 = Mathf.CeilToInt(componentData2.m_ExtinguishingCapacity);
      if (num2 > 0)
        info.Add(new InfoList.Item("Load: " + (object) num1 + "/" + (object) num2));
      if ((componentData1.m_State & FireEngineFlags.Extinguishing) != (FireEngineFlags) 0)
        info.Add(new InfoList.Item("Extinguishing"));
      else if ((componentData1.m_State & FireEngineFlags.Rescueing) != (FireEngineFlags) 0)
        info.Add(new InfoList.Item("Searching for survivors"));
      else if ((componentData1.m_State & FireEngineFlags.Returning) != (FireEngineFlags) 0)
      {
        info.Add(new InfoList.Item("Returning"));
      }
      else
      {
        FireRescueRequest component1;
        if (componentData1.m_RequestCount <= 0 || buffer.Length <= 0 || !this.EntityManager.TryGetComponent<FireRescueRequest>(buffer[0].m_Request, out component1))
          return;
        OnFire component2;
        if (this.EntityManager.TryGetComponent<OnFire>(component1.m_Target, out component2) && component2.m_Event != InfoList.Item.kNullEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component2.m_Event), component2.m_Event));
        }
        else
        {
          Destroyed component3;
          if (this.EntityManager.TryGetComponent<Destroyed>(component1.m_Target, out component3) && component3.m_Event != InfoList.Item.kNullEntity)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component3.m_Event), component3.m_Event));
          }
          else
          {
            if (!(component1.m_Target != InfoList.Item.kNullEntity))
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component1.m_Target), component1.m_Target));
          }
        }
      }
    }

    private bool HasPoliceCarInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Vehicle>(entity) && this.EntityManager.HasComponent<Game.Vehicles.PoliceCar>(entity) && this.EntityManager.HasComponent<PoliceCarData>(prefab) && this.EntityManager.HasComponent<ServiceDispatch>(entity);
    }

    private void UpdatePoliceCarInfo(Entity entity, Entity prefab, InfoList info)
    {
      Game.Vehicles.PoliceCar componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.PoliceCar>(entity);
      PoliceCarData componentData2 = this.EntityManager.GetComponentData<PoliceCarData>(prefab);
      DynamicBuffer<ServiceDispatch> buffer1 = this.EntityManager.GetBuffer<ServiceDispatch>(entity, true);
      info.label = "Police Car";
      if (componentData2.m_ShiftDuration > 0U)
      {
        uint num = math.min(componentData1.m_ShiftTime, componentData2.m_ShiftDuration);
        info.Add(new InfoList.Item("Work shift: " + (object) num + "/" + (object) componentData2.m_ShiftDuration));
      }
      DynamicBuffer<Passenger> buffer2;
      if (this.EntityManager.TryGetBuffer<Passenger>(entity, true, out buffer2))
      {
        for (int index = 0; index < buffer2.Length; ++index)
        {
          Entity entity1 = buffer2[index].m_Passenger;
          Game.Creatures.Resident component;
          if (this.EntityManager.TryGetComponent<Game.Creatures.Resident>(entity1, out component))
            entity1 = component.m_Citizen;
          if (this.EntityManager.HasComponent<Citizen>(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Arrested criminal" + this.m_NameSystem.GetDebugName(entity1), entity1));
          }
        }
      }
      if ((componentData1.m_State & PoliceCarFlags.Returning) != (PoliceCarFlags) 0)
        info.Add(new InfoList.Item("Returning"));
      else if ((componentData1.m_State & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0)
      {
        if ((componentData1.m_State & PoliceCarFlags.AtTarget) != (PoliceCarFlags) 0)
        {
          PoliceEmergencyRequest component1;
          AccidentSite component2;
          if (componentData1.m_RequestCount <= 0 || buffer1.Length <= 0 || !this.EntityManager.TryGetComponent<PoliceEmergencyRequest>(buffer1[0].m_Request, out component1) || !this.EntityManager.TryGetComponent<AccidentSite>(component1.m_Site, out component2))
            return;
          if ((component2.m_Flags & AccidentSiteFlags.TrafficAccident) != (AccidentSiteFlags) 0)
          {
            info.Add(new InfoList.Item("Securing accident site"));
          }
          else
          {
            if ((component2.m_Flags & AccidentSiteFlags.CrimeScene) == (AccidentSiteFlags) 0)
              return;
            info.Add(new InfoList.Item("Securing crime scene"));
          }
        }
        else
        {
          PoliceEmergencyRequest component3;
          if (componentData1.m_RequestCount <= 0 || buffer1.Length <= 0 || !this.EntityManager.TryGetComponent<PoliceEmergencyRequest>(buffer1[0].m_Request, out component3))
            return;
          AccidentSite component4;
          if (this.EntityManager.TryGetComponent<AccidentSite>(component3.m_Site, out component4) && component4.m_Event != InfoList.Item.kNullEntity)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component4.m_Event), component4.m_Event));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Dispatched" + this.m_NameSystem.GetDebugName(component3.m_Site), component3.m_Site));
          }
        }
      }
      else
        info.Add(new InfoList.Item("Patrolling"));
    }

    private bool HasCitizenInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<HouseholdMember>(entity) && this.EntityManager.HasComponent<Citizen>(entity);
    }

    private void UpdateCitizenInfo(Entity entity, Entity prefab, InfoList info)
    {
      Citizen componentData = this.EntityManager.GetComponentData<Citizen>(entity);
      Entity household = this.EntityManager.GetComponentData<HouseholdMember>(entity).m_Household;
      Household householdData = new Household();
      if (this.EntityManager.HasComponent<Household>(household))
      {
        householdData = this.EntityManager.GetComponentData<Household>(household);
        this.EntityManager.GetBuffer<HouseholdCitizen>(household, true);
      }
      // ISSUE: reference to a compiler-generated field
      EconomyParameterData singleton = this.__query_746694603_4.GetSingleton<EconomyParameterData>();
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_6.GetSingleton<CitizenHappinessParameterData>();
      bool tourist = (componentData.m_State & CitizenFlags.Tourist) != 0;
      bool flag = (componentData.m_State & CitizenFlags.Commuter) != 0;
      info.label = "Citizen";
      if (!flag)
      {
        Entity entity1 = InfoList.Item.kNullEntity;
        CurrentBuilding component1;
        if (this.EntityManager.TryGetComponent<CurrentBuilding>(entity, out component1))
          entity1 = component1.m_CurrentBuilding;
        info.Add(new InfoList.Item("Current Building: " + (object) entity1, entity1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Household: " + this.m_NameSystem.GetDebugName(household), household));
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Wellbeing: " + DeveloperInfoUISystem.WellbeingToString((int) componentData.m_WellBeing) + "(" + componentData.m_WellBeing.ToString() + ")"));
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Health: " + DeveloperInfoUISystem.HealthToString((int) componentData.m_Health) + "(" + componentData.m_Health.ToString() + ")"));
        if (this.EntityManager.HasComponent<Game.Economy.Resources>(household))
        {
          int householdTotalWealth = EconomyUtils.GetHouseholdTotalWealth(householdData, this.EntityManager.GetBuffer<Game.Economy.Resources>(household, true));
          int resources = EconomyUtils.GetResources(Resource.Money, this.EntityManager.GetBuffer<Game.Economy.Resources>(household, true));
          info.Add(new InfoList.Item("Household total Wealth Value: " + householdTotalWealth.ToString()));
          info.Add(new InfoList.Item("Household Money: " + resources.ToString()));
          PropertyRenter component2;
          if (this.EntityManager.TryGetComponent<PropertyRenter>(household, out component2))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            BufferLookup<Renter> renterRoBufferLookup = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentLookup<ConsumptionData> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentLookup<PrefabRef> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
            int householdSpendableMoney = EconomyUtils.GetHouseholdSpendableMoney(householdData, this.EntityManager.GetBuffer<Game.Economy.Resources>(household, true), ref renterRoBufferLookup, ref roComponentLookup1, ref roComponentLookup2, component2);
            info.Add(new InfoList.Item("Household spendable Money: " + householdSpendableMoney.ToString()));
          }
        }
      }
      CarKeeper component3;
      if (this.EntityManager.IsComponentEnabled<CarKeeper>(entity) && this.EntityManager.TryGetComponent<CarKeeper>(entity, out component3))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Car: " + this.m_NameSystem.GetDebugName(component3.m_Car), component3.m_Car));
      }
      if (!flag)
      {
        info.Add(new InfoList.Item("Household total Resources: " + householdData.m_Resources.ToString()));
        PropertyRenter component4;
        if (this.EntityManager.TryGetComponent<PropertyRenter>(household, out component4))
        {
          info.Add(new InfoList.Item("Property: " + component4.m_Property.ToString(), component4.m_Property));
          info.Add(new InfoList.Item("Rent: " + component4.m_Rent.ToString()));
        }
        else if (!tourist)
          info.Add(new InfoList.Item("Homeless"));
      }
      else
        info.Add(new InfoList.Item("From outside the city"));
      Criminal component5;
      bool component6 = this.EntityManager.TryGetComponent<Criminal>(entity, out component5);
      TravelPurpose component7;
      if (this.EntityManager.TryGetComponent<TravelPurpose>(entity, out component7))
      {
        Entity kNullEntity = InfoList.Item.kNullEntity;
        // ISSUE: reference to a compiler-generated method
        string purposeText = this.GetPurposeText(component7, tourist, component5, ref kNullEntity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(purposeText + " " + this.m_NameSystem.GetDebugName(kNullEntity), kNullEntity));
      }
      string str = " female";
      if ((componentData.m_State & CitizenFlags.Male) != CitizenFlags.None)
        str = " male";
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      info.Add(new InfoList.Item(this.GetAgeString(entity) + "(" + componentData.GetAgeInDays(this.m_SimulationSystem.frameIndex, TimeData.GetSingleton(this.m_TimeDataQuery)).ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")" + str));
      info.Add(new InfoList.Item("Leisure: " + componentData.m_LeisureCounter.ToString()));
      HealthProblem component8;
      if (this.EntityManager.TryGetComponent<HealthProblem>(entity, out component8))
      {
        if ((component8.m_Flags & HealthProblemFlags.Sick) != HealthProblemFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Sick " + this.m_NameSystem.GetDebugName(component8.m_Event), component8.m_Event));
        }
        else if ((component8.m_Flags & HealthProblemFlags.Injured) != HealthProblemFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Injured " + this.m_NameSystem.GetDebugName(component8.m_Event), component8.m_Event));
        }
        else if ((component8.m_Flags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Dead " + this.m_NameSystem.GetDebugName(component8.m_Event), component8.m_Event));
        }
        else if ((component8.m_Flags & HealthProblemFlags.Trapped) != HealthProblemFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Trapped " + this.m_NameSystem.GetDebugName(component8.m_Event), component8.m_Event));
        }
        else if ((component8.m_Flags & HealthProblemFlags.InDanger) != HealthProblemFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("In danger " + this.m_NameSystem.GetDebugName(component8.m_Event), component8.m_Event));
        }
      }
      if (component6)
      {
        string text = "Criminal";
        if ((component5.m_Flags & CriminalFlags.Robber) != (CriminalFlags) 0)
          text += " Robber ";
        if ((component5.m_Flags & CriminalFlags.Prisoner) != (CriminalFlags) 0)
          text = text + "Jail Time: " + ((uint) ((int) component5.m_JailTime * 16 * 16) / 262144U).ToString();
        info.Add(new InfoList.Item(text));
      }
      if (tourist)
      {
        info.Add(new InfoList.Item("Tourist"));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<TouristHousehold> roComponentLookup = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup;
        if (roComponentLookup.HasComponent(household) && roComponentLookup.HasComponent(household))
        {
          TouristHousehold touristHousehold = roComponentLookup[household];
          if (this.EntityManager.Exists(touristHousehold.m_Hotel))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Staying at: " + this.m_NameSystem.GetDebugName(touristHousehold.m_Hotel), touristHousehold.m_Hotel));
          }
        }
      }
      if (!tourist)
      {
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.GetEducationString(componentData.GetEducationLevel())));
        Citizen component9;
        if (this.EntityManager.TryGetComponent<Citizen>(entity, out component9))
        {
          CitizenAge age = component9.GetAge();
          Worker component10;
          if (this.EntityManager.TryGetComponent<Worker>(entity, out component10))
          {
            Entity workplace = component10.m_Workplace;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Works at: " + this.m_NameSystem.GetDebugName(workplace), workplace));
            // ISSUE: reference to a compiler-generated method
            float2 timeToWork1 = WorkerSystem.GetTimeToWork(componentData, component10, ref singleton, false);
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Work Shift: " + (this.GetTimeString(timeToWork1.x) + " to " + this.GetTimeString(timeToWork1.y))));
            // ISSUE: reference to a compiler-generated method
            float2 timeToWork2 = WorkerSystem.GetTimeToWork(componentData, component10, ref singleton, true);
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            info.Add(new InfoList.Item("Work Shift(Commute): " + (this.GetTimeString(timeToWork2.x) + " to " + this.GetTimeString(timeToWork2.y))));
          }
          else
          {
            Game.Citizens.Student component11;
            if (this.EntityManager.TryGetComponent<Game.Citizens.Student>(entity, out component11))
            {
              Entity school = component11.m_School;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Studies at: " + this.m_NameSystem.GetDebugName(school), school));
              // ISSUE: reference to a compiler-generated method
              float2 timeToStudy = StudentSystem.GetTimeToStudy(componentData, component11, ref singleton);
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              info.Add(new InfoList.Item("Study Time: " + (this.GetTimeString(timeToStudy.x) + " to " + this.GetTimeString(timeToStudy.y))));
            }
            else
            {
              switch (age)
              {
                case CitizenAge.Child:
                case CitizenAge.Teen:
                  info.Add(new InfoList.Item("Not in school!"));
                  break;
                case CitizenAge.Adult:
                  info.Add(new InfoList.Item("Unemployed"));
                  break;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<Worker> roComponentLookup3 = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<Game.Citizens.Student> roComponentLookup4 = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup;
          // ISSUE: reference to a compiler-generated method
          float2 sleepTime = CitizenBehaviorSystem.GetSleepTime(entity, componentData, ref singleton, ref roComponentLookup3, ref roComponentLookup4);
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Sleep Time: " + (this.GetTimeString(sleepTime.x) + " to " + this.GetTimeString(sleepTime.y))));
        }
      }
      AttendingMeeting component12;
      CoordinatedMeeting component13;
      if (this.EntityManager.TryGetComponent<AttendingMeeting>(entity, out component12) && this.EntityManager.TryGetComponent<CoordinatedMeeting>(component12.m_Meeting, out component13))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        InfoList.Item obj = component13.m_Target != InfoList.Item.kNullEntity ? new InfoList.Item("Meeting at: " + this.m_NameSystem.GetDebugName(component13.m_Target), component13.m_Target) : new InfoList.Item("Planning a meeting");
        info.Add(obj);
      }
      AttendingEvent component14;
      if (!this.EntityManager.TryGetComponent<AttendingEvent>(household, out component14) || !this.EntityManager.HasComponent<Game.Events.CalendarEvent>(component14.m_Event))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.Add(new InfoList.Item("Participating in " + this.m_NameSystem.GetDebugName(component14.m_Event)));
    }

    private string GetPurposeText(
      TravelPurpose purpose,
      bool tourist,
      Criminal criminal,
      ref Entity entity)
    {
      string purposeText;
      switch (purpose.m_Purpose)
      {
        case Game.Citizens.Purpose.Shopping:
          purposeText = "Buying " + EconomyUtils.GetName(purpose.m_Resource);
          break;
        case Game.Citizens.Purpose.Leisure:
          purposeText = tourist ? "Sightseeing" : "Spending free time";
          break;
        case Game.Citizens.Purpose.GoingHome:
          purposeText = tourist ? "Going to hotel" : "Going home";
          break;
        case Game.Citizens.Purpose.GoingToWork:
          purposeText = "Going to work";
          break;
        case Game.Citizens.Purpose.Working:
          purposeText = "Working";
          break;
        case Game.Citizens.Purpose.Sleeping:
          purposeText = "Sleeping";
          break;
        case Game.Citizens.Purpose.MovingAway:
          purposeText = "Moving away";
          break;
        case Game.Citizens.Purpose.Studying:
          purposeText = "Studying";
          break;
        case Game.Citizens.Purpose.GoingToSchool:
          purposeText = "Going to school";
          break;
        case Game.Citizens.Purpose.Hospital:
          purposeText = "Seeking medical care";
          break;
        case Game.Citizens.Purpose.Safety:
          purposeText = "Getting safe";
          break;
        case Game.Citizens.Purpose.EmergencyShelter:
          purposeText = "Evacuating";
          break;
        case Game.Citizens.Purpose.Crime:
          purposeText = "Committing crime";
          entity = criminal.m_Event;
          break;
        case Game.Citizens.Purpose.GoingToJail:
          purposeText = "Going to jail";
          entity = criminal.m_Event;
          break;
        case Game.Citizens.Purpose.GoingToPrison:
          purposeText = "Going to prison";
          break;
        case Game.Citizens.Purpose.InJail:
          if ((criminal.m_Flags & CriminalFlags.Sentenced) != (CriminalFlags) 0)
          {
            purposeText = "Sentenced to prison";
            break;
          }
          purposeText = "In jail";
          entity = criminal.m_Event;
          break;
        case Game.Citizens.Purpose.InPrison:
          purposeText = "In prison";
          break;
        case Game.Citizens.Purpose.Escape:
          purposeText = "Escaping";
          break;
        case Game.Citizens.Purpose.InHospital:
          purposeText = "Getting medical care";
          break;
        case Game.Citizens.Purpose.Deathcare:
          purposeText = "Transferring to death care";
          break;
        case Game.Citizens.Purpose.InDeathcare:
          purposeText = "Waiting for processing";
          break;
        case Game.Citizens.Purpose.SendMail:
          purposeText = "Sending mail";
          break;
        case Game.Citizens.Purpose.Disappear:
          purposeText = "Disappearing";
          break;
        case Game.Citizens.Purpose.WaitingHome:
          purposeText = "Waiting for new home";
          break;
        case Game.Citizens.Purpose.PathFailed:
          purposeText = "Can't reach destination";
          break;
        default:
          purposeText = "Idling";
          break;
      }
      return purposeText;
    }

    private static string WellbeingToString(int wellbeing)
    {
      if (wellbeing < 25)
        return "Depressed";
      if (wellbeing < 40)
        return "Sad";
      if (wellbeing < 60)
        return "Neutral";
      return wellbeing < 80 ? "Content" : "Happy";
    }

    private static string HealthToString(int wellbeing)
    {
      if (wellbeing < 25)
        return "Weak";
      if (wellbeing < 40)
        return "Poor";
      if (wellbeing < 60)
        return "OK";
      return wellbeing < 80 ? "Healthy" : "Vigorous";
    }

    private static string ConsumptionToString(
      int dailyConsumption,
      int citizens,
      CitizenHappinessParameterData happinessParameters)
    {
      // ISSUE: reference to a compiler-generated method
      int2 consumptionBonuses = CitizenHappinessSystem.GetConsumptionBonuses((float) dailyConsumption, citizens, in happinessParameters);
      int num = consumptionBonuses.x + consumptionBonuses.y;
      if (num < -15)
        return "Wretched";
      if (num < -5)
        return "Poor";
      if (num < 5)
        return "Modest";
      return num < 15 ? "Comfortable" : "Wealthy";
    }

    private static string GetLevelupTime(int condition, int levelup, int changePerDay)
    {
      return changePerDay <= 0 ? "Decaying" : "In " + Mathf.CeilToInt((float) (levelup - condition) / (float) changePerDay).ToString() + " days";
    }

    private string GetAgeString(Entity entity)
    {
      Citizen component;
      if (!this.EntityManager.TryGetComponent<Citizen>(entity, out component))
        return "Unknown";
      switch (component.GetAge())
      {
        case CitizenAge.Child:
          return "Child";
        case CitizenAge.Teen:
          return "Teenager";
        case CitizenAge.Adult:
          return "Adult";
        default:
          return "Elderly";
      }
    }

    private string GetEducationString(int education)
    {
      switch (education)
      {
        case 0:
          return "Uneducated";
        case 1:
          return "Poorly educated";
        case 2:
          return "Educated";
        case 3:
          return "Well educated";
        case 4:
          return "Highly educated";
        default:
          return "Unknown education";
      }
    }

    private string GetTimeString(float time) => Mathf.RoundToInt(time * 24f).ToString() + ":00";

    private bool HasMailSenderInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasEnabledComponent<MailSender>(entity);
    }

    private void UpdateMailSenderInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      MailSender componentData = this.EntityManager.GetComponentData<MailSender>(entity);
      info.label = "Mail sender";
      info.value = (int) componentData.m_Amount;
      info.max = 100;
    }

    private bool HasAnimalInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<HouseholdPet>(entity);
    }

    private void UpdateAnimalInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      HouseholdPet componentData = this.EntityManager.GetComponentData<HouseholdPet>(entity);
      info.label = "Household";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(componentData.m_Household);
      info.target = componentData.m_Household;
    }

    private bool HasCreatureInfo(Entity entity, Entity prefab)
    {
      CurrentTransport component;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component))
        entity = component.m_CurrentTransport;
      return this.EntityManager.HasComponent<Creature>(entity);
    }

    private void UpdateCreatureInfo(Entity entity, Entity prefab, InfoList info)
    {
      info.label = "Creature";
      Citizen component1;
      CurrentTransport component2;
      if (!this.EntityManager.TryGetComponent<Citizen>(entity, out component1) || !this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component2))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.Add(new InfoList.Item("Entity: " + this.m_NameSystem.GetDebugName(component2.m_CurrentTransport)));
      bool tourist = (component1.m_State & CitizenFlags.Tourist) != 0;
      Criminal component3;
      this.EntityManager.TryGetComponent<Criminal>(entity, out component3);
      Divert component4;
      if (this.EntityManager.TryGetComponent<Divert>(component2.m_CurrentTransport, out component4) && component4.m_Purpose != Game.Citizens.Purpose.None)
      {
        Entity kNullEntity = InfoList.Item.kNullEntity;
        // ISSUE: reference to a compiler-generated method
        string purposeText = this.GetPurposeText(new TravelPurpose()
        {
          m_Purpose = component4.m_Purpose,
          m_Resource = component4.m_Resource
        }, tourist, component3, ref kNullEntity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(purposeText + " " + this.m_NameSystem.GetDebugName(kNullEntity), kNullEntity));
      }
      RideNeeder component5;
      if (this.EntityManager.TryGetComponent<RideNeeder>(component2.m_CurrentTransport, out component5))
      {
        Dispatched component6;
        DynamicBuffer<ServiceDispatch> buffer;
        if (this.EntityManager.TryGetComponent<Dispatched>(component5.m_RideRequest, out component6) && this.EntityManager.TryGetBuffer<ServiceDispatch>(component6.m_Handler, true, out buffer) && buffer.Length > 0 && buffer[0].m_Request == component5.m_RideRequest)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Waiting for ride: " + this.m_NameSystem.GetDebugName(component6.m_Handler), component6.m_Handler));
        }
        else
          info.Add(new InfoList.Item("Taking a taxi"));
      }
      HumanCurrentLane component7;
      if (!this.EntityManager.TryGetComponent<HumanCurrentLane>(component2.m_CurrentTransport, out component7))
        return;
      Creature componentData = this.EntityManager.GetComponentData<Creature>(component2.m_CurrentTransport);
      if ((component7.m_Flags & CreatureLaneFlags.EndReached) != (CreatureLaneFlags) 0)
      {
        if ((component7.m_Flags & CreatureLaneFlags.Transport) == (CreatureLaneFlags) 0)
          return;
        PathOwner component8;
        DynamicBuffer<PathElement> buffer;
        if (this.EntityManager.TryGetComponent<PathOwner>(component2.m_CurrentTransport, out component8) && this.EntityManager.TryGetBuffer<PathElement>(component2.m_CurrentTransport, true, out buffer) && buffer.Length > component8.m_ElementIndex)
        {
          Entity entity1 = buffer[component8.m_ElementIndex].m_Target;
          Owner component9;
          if (this.EntityManager.TryGetComponent<Owner>(entity1, out component9))
            entity1 = component9.m_Owner;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Waiting for transport: " + this.m_NameSystem.GetDebugName(entity1), entity1));
        }
        else
          info.Add(new InfoList.Item("Waiting for transport"));
      }
      else
      {
        if ((double) componentData.m_QueueArea.radius <= 0.0)
          return;
        if (componentData.m_QueueEntity != Entity.Null)
        {
          Entity entity2 = componentData.m_QueueEntity;
          Owner component10;
          if (this.EntityManager.HasComponent<Waypoint>(entity2) && this.EntityManager.TryGetComponent<Owner>(entity2, out component10))
            entity2 = component10.m_Owner;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          info.Add(new InfoList.Item("Queueing for: " + this.m_NameSystem.GetDebugName(entity2), entity2));
        }
        else
          info.Add(new InfoList.Item("Queueing"));
      }
    }

    private bool HasGroupLeaderInfo(Entity entity, Entity prefab)
    {
      CurrentTransport component;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component))
        entity = component.m_CurrentTransport;
      return this.EntityManager.HasComponent<GroupCreature>(entity);
    }

    private bool HasGroupMemberInfo(Entity entity, Entity prefab)
    {
      CurrentTransport component;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component))
        entity = component.m_CurrentTransport;
      return this.EntityManager.HasComponent<GroupMember>(entity);
    }

    private void UpdateGroupLeaderInfo(Entity entity, Entity prefab, InfoList info)
    {
      CurrentTransport component;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component))
        entity = component.m_CurrentTransport;
      DynamicBuffer<GroupCreature> buffer = this.EntityManager.GetBuffer<GroupCreature>(entity, true);
      info.label = string.Format("Group members ({0})", (object) buffer.Length);
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer[index].m_Creature), buffer[index].m_Creature));
      }
    }

    private void UpdateGroupMemberInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      CurrentTransport component;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component))
        entity = component.m_CurrentTransport;
      GroupMember componentData = this.EntityManager.GetComponentData<GroupMember>(entity);
      info.label = "Group leader";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(componentData.m_Leader);
      info.target = componentData.m_Leader;
    }

    private bool HasVehicleModelInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<VehicleModel>(entity);
    }

    private void UpdateVehicleModelInfo(Entity entity, Entity prefab, InfoList info)
    {
      VehicleModel componentData = this.EntityManager.GetComponentData<VehicleModel>(entity);
      info.label = "Vehicle Model";
      if (this.EntityManager.HasComponent<PrefabData>(componentData.m_PrimaryPrefab))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_PrefabSystem.GetPrefabName(componentData.m_PrimaryPrefab)));
      }
      if (!this.EntityManager.HasComponent<PrefabData>(componentData.m_SecondaryPrefab))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.Add(new InfoList.Item(this.m_PrefabSystem.GetPrefabName(componentData.m_SecondaryPrefab)));
    }

    private bool HasAreaInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Area>(entity);
    }

    private void UpdateAreaInfo(Entity entity, Entity prefab, InfoList info)
    {
      info.label = "Area Info";
      info.Add(new InfoList.Item("Nothing to see here, move along! (TBD)"));
    }

    private bool HasTreeInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Game.Objects.Object>(entity) && this.EntityManager.HasComponent<Tree>(entity);
    }

    private void UpdateTreeInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      Tree componentData1 = this.EntityManager.GetComponentData<Tree>(entity);
      Plant componentData2 = this.EntityManager.GetComponentData<Plant>(entity);
      Damaged component1;
      this.EntityManager.TryGetComponent<Damaged>(entity, out component1);
      int num = 0;
      TreeData component2;
      if (this.EntityManager.TryGetComponent<TreeData>(prefab, out component2))
        num = Mathf.RoundToInt(ObjectUtils.CalculateWoodAmount(componentData1, componentData2, component1, component2));
      info.label = string.Format("Wood: {0}", (object) num);
      info.value = num;
      info.max = Mathf.RoundToInt(component2.m_WoodAmount);
    }

    private bool HasMailBoxInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Game.Objects.Object>(entity) && this.EntityManager.HasComponent<Game.Routes.MailBox>(entity) && this.EntityManager.HasComponent<MailBoxData>(prefab);
    }

    private void UpdateMailBoxInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      Game.Routes.MailBox componentData1 = this.EntityManager.GetComponentData<Game.Routes.MailBox>(entity);
      MailBoxData componentData2 = this.EntityManager.GetComponentData<MailBoxData>(prefab);
      info.label = "Stored Mail in Mailbox";
      info.value = componentData1.m_MailAmount;
      info.max = componentData2.m_MailCapacity;
    }

    private bool HasBoardingVehicleInfo(Entity entity, Entity prefab)
    {
      BoardingVehicle component;
      return this.EntityManager.TryGetComponent<BoardingVehicle>(entity, out component) && component.m_Vehicle != InfoList.Item.kNullEntity;
    }

    private void UpdateBoardingVehicleInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      BoardingVehicle componentData = this.EntityManager.GetComponentData<BoardingVehicle>(entity);
      info.label = "Boarding";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = this.m_NameSystem.GetDebugName(componentData.m_Vehicle);
      info.target = componentData.m_Vehicle;
    }

    private bool HasWaitingPassengerInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<WaitingPassengers>(entity) || this.EntityManager.HasBuffer<ConnectedRoute>(entity);
    }

    private void UpdateWaitingPassengerInfo(Entity entity, Entity prefab, InfoList info)
    {
      WaitingPassengers component1;
      this.EntityManager.TryGetComponent<WaitingPassengers>(entity, out component1);
      DynamicBuffer<ConnectedRoute> buffer;
      if (this.EntityManager.TryGetBuffer<ConnectedRoute>(entity, true, out buffer))
      {
        int num1 = 0;
        for (int index = 0; index < buffer.Length; ++index)
        {
          WaitingPassengers component2;
          if (this.EntityManager.TryGetComponent<WaitingPassengers>(buffer[index].m_Waypoint, out component2))
          {
            component1.m_Count += component2.m_Count;
            num1 += (int) component2.m_AverageWaitingTime;
          }
        }
        int num2 = num1 / math.max(1, buffer.Length);
        int num3 = num2 - num2 % 5;
        component1.m_AverageWaitingTime = (ushort) num3;
      }
      info.label = "Waiting passengers";
      info.Add(new InfoList.Item("Passenger count: " + (object) component1.m_Count));
      info.Add(new InfoList.Item("Waiting time: " + (component1.m_AverageWaitingTime.ToString() + "s")));
    }

    private bool HasMovingInfo(Entity entity, Entity prefab)
    {
      CurrentTransport component;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component))
        entity = component.m_CurrentTransport;
      return this.EntityManager.HasComponent<Moving>(entity);
    }

    private void UpdateMovingInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      CurrentTransport component1;
      if (this.EntityManager.TryGetComponent<CurrentTransport>(entity, out component1))
      {
        entity = component1.m_CurrentTransport;
        prefab = this.EntityManager.GetComponentData<PrefabRef>(entity).m_Prefab;
      }
      int num1 = Mathf.RoundToInt(math.length(this.EntityManager.GetComponentData<Moving>(entity).m_Velocity) * 3.6f);
      int num2 = Mathf.RoundToInt(999.999939f);
      CarData component2;
      if (this.EntityManager.TryGetComponent<CarData>(prefab, out component2))
      {
        num2 = Mathf.RoundToInt(component2.m_MaxSpeed * 3.6f);
      }
      else
      {
        WatercraftData component3;
        if (this.EntityManager.TryGetComponent<WatercraftData>(prefab, out component3))
        {
          num2 = Mathf.RoundToInt(component3.m_MaxSpeed * 3.6f);
        }
        else
        {
          AirplaneData component4;
          if (this.EntityManager.TryGetComponent<AirplaneData>(prefab, out component4))
          {
            num2 = Mathf.RoundToInt(component4.m_FlyingSpeed.y * 3.6f);
          }
          else
          {
            HelicopterData component5;
            if (this.EntityManager.TryGetComponent<HelicopterData>(prefab, out component5))
            {
              num2 = Mathf.RoundToInt(component5.m_FlyingMaxSpeed * 3.6f);
            }
            else
            {
              TrainData component6;
              if (this.EntityManager.TryGetComponent<TrainData>(prefab, out component6))
              {
                num2 = Mathf.RoundToInt(component6.m_MaxSpeed * 3.6f);
              }
              else
              {
                HumanData component7;
                if (this.EntityManager.TryGetComponent<HumanData>(prefab, out component7))
                {
                  num2 = Mathf.RoundToInt(component7.m_RunSpeed * 3.6f);
                }
                else
                {
                  AnimalData component8;
                  if (this.EntityManager.TryGetComponent<AnimalData>(prefab, out component8))
                    num2 = Mathf.RoundToInt(component8.m_MoveSpeed * 3.6f);
                }
              }
            }
          }
        }
      }
      info.label = string.Format("Moving: {0} km/h", (object) num1);
      info.value = num1;
      info.max = num2;
    }

    private bool HasDamagedInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Damaged>(entity);
    }

    private void UpdateDamagedInfo(Entity entity, Entity prefab, InfoList info)
    {
      Damaged componentData = this.EntityManager.GetComponentData<Damaged>(entity);
      float4 a1 = math.clamp(new float4(componentData.m_Damage, ObjectUtils.GetTotalDamage(componentData)) * 100f, (float4) 0.0f, (float4) 100f);
      float4 a2 = math.select(a1, (float4) 1f, a1 > 0.0f & a1 < 1f);
      float4 float4 = math.select(a2, (float4) 99f, a2 > 99f & a2 < 100f);
      info.label = "Damaged";
      info.Add(new InfoList.Item("Physical: " + (object) Mathf.RoundToInt(float4.x) + "%"));
      info.Add(new InfoList.Item("Fire: " + (object) Mathf.RoundToInt(float4.y) + "%"));
      info.Add(new InfoList.Item("Water: " + (object) Mathf.RoundToInt(float4.z) + "%"));
      info.Add(new InfoList.Item("Total: " + (object) Mathf.RoundToInt(float4.w) + "%"));
    }

    private bool HasDestroyedInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Destroyed>(entity);
    }

    private void UpdateDestroyedInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      Destroyed componentData = this.EntityManager.GetComponentData<Destroyed>(entity);
      info.label = componentData.m_Event == InfoList.Item.kNullEntity ? "Destroyed" : "Destroyed By";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = componentData.m_Event == InfoList.Item.kNullEntity ? string.Empty : this.m_NameSystem.GetDebugName(componentData.m_Event);
      info.target = componentData.m_Event;
    }

    private bool HasDestroyedBuildingInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Destroyed>(entity) && this.EntityManager.HasComponent<Building>(entity);
    }

    private void UpdateDestroyedBuildingInfo(Entity entity, Entity prefab, CapacityInfo info)
    {
      Destroyed componentData = this.EntityManager.GetComponentData<Destroyed>(entity);
      info.label = string.Format("Searching for survivors: {0}%)", (object) Mathf.RoundToInt(componentData.m_Cleared * 100f));
      info.value = Mathf.RoundToInt(componentData.m_Cleared * 100f);
      info.max = 100;
    }

    private bool HasOnFireInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<OnFire>(entity);
    }

    private void UpdateOnFireInfo(Entity entity, Entity prefab, InfoList info)
    {
      OnFire componentData = this.EntityManager.GetComponentData<OnFire>(entity);
      info.label = "On fire";
      if (componentData.m_Event != InfoList.Item.kNullEntity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Ignited by: " + this.m_NameSystem.GetDebugName(componentData.m_Event), componentData.m_Event));
      }
      info.Add(new InfoList.Item("Intensity: " + (object) Mathf.RoundToInt(componentData.m_Intensity) + "%"));
    }

    private bool HasFacingWeatherInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<FacingWeather>(entity);
    }

    private void UpdateFacingWeatherInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      FacingWeather componentData = this.EntityManager.GetComponentData<FacingWeather>(entity);
      info.label = componentData.m_Event == InfoList.Item.kNullEntity ? "Suffering from weather" : "Weather phenomenon";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = componentData.m_Event == InfoList.Item.kNullEntity ? string.Empty : this.m_NameSystem.GetDebugName(componentData.m_Event);
      info.target = componentData.m_Event;
    }

    private bool HasAccidentSiteInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<AccidentSite>(entity);
    }

    private void UpdateAccidentSiteInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      AccidentSite componentData = this.EntityManager.GetComponentData<AccidentSite>(entity);
      info.label = componentData.m_Event == InfoList.Item.kNullEntity ? "Accident site" : "Incident";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = componentData.m_Event == InfoList.Item.kNullEntity ? string.Empty : this.m_NameSystem.GetDebugName(componentData.m_Event);
      info.target = componentData.m_Event;
    }

    private bool HasInvolvedInAccidentInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<InvolvedInAccident>(entity);
    }

    private void UpdateInvolvedInAccidentInfo(Entity entity, Entity prefab, GenericInfo info)
    {
      InvolvedInAccident componentData = this.EntityManager.GetComponentData<InvolvedInAccident>(entity);
      info.label = componentData.m_Event == InfoList.Item.kNullEntity ? "Involved in accident" : "Involved in";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.value = componentData.m_Event == InfoList.Item.kNullEntity ? string.Empty : this.m_NameSystem.GetDebugName(componentData.m_Event);
      info.target = componentData.m_Event;
    }

    private bool HasFloodedInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Flooded>(entity);
    }

    private void UpdateFloodedInfo(Entity entity, Entity prefab, InfoList info)
    {
      Flooded componentData = this.EntityManager.GetComponentData<Flooded>(entity);
      info.label = "Flooded";
      if (componentData.m_Event != InfoList.Item.kNullEntity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item("Caused by: " + this.m_NameSystem.GetDebugName(componentData.m_Event), componentData.m_Event));
      }
      info.Add(new InfoList.Item("Depth: " + (object) Mathf.RoundToInt(componentData.m_Depth) + "m"));
    }

    private bool HasEventInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Game.Events.Event>(entity) && this.EntityManager.HasComponent<TargetElement>(entity);
    }

    private void UpdateEventInfo(Entity entity, Entity prefab, InfoList info)
    {
      DynamicBuffer<TargetElement> buffer = this.EntityManager.GetBuffer<TargetElement>(entity, true);
      info.label = string.Format("Affected Objects: {0})", (object) buffer.Length);
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        info.Add(new InfoList.Item(this.m_NameSystem.GetDebugName(buffer[index].m_Entity), buffer[index].m_Entity));
      }
    }

    private bool HasNotificationInfo(Entity entity, Entity prefab)
    {
      return this.EntityManager.HasComponent<Icon>(entity);
    }

    private void UpdateNotificationInfo(Entity entity, Entity prefab, InfoList info)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NotificationIconPrefab prefab1 = this.m_PrefabSystem.GetPrefab<NotificationIconPrefab>(prefab);
      info.label = "Notification Info";
      Owner component1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.Add(this.EntityManager.TryGetComponent<Owner>(entity, out component1) ? new InfoList.Item(prefab1.m_Description + this.m_NameSystem.GetDebugName(component1.m_Owner), component1.m_Owner) : new InfoList.Item(prefab1.m_Description));
      Game.Common.Target component2;
      if (!this.EntityManager.TryGetComponent<Game.Common.Target>(entity, out component2))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      info.Add(new InfoList.Item(prefab1.m_TargetDescription + this.m_NameSystem.GetDebugName(component2.m_Target), component2.m_Target));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ServiceFeeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PollutionParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_2 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadWrite<CityModifier>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_3 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<GarbageParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_4 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EconomyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_5 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ExtractorParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_746694603_6 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<CitizenHappinessParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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

    [Preserve]
    public DeveloperInfoUISystem()
    {
    }

    private struct BuildingHappinessFactorValue : 
      IComparable<DeveloperInfoUISystem.BuildingHappinessFactorValue>
    {
      public BuildingHappinessFactor m_Factor;
      public int m_Value;

      public int CompareTo(
        DeveloperInfoUISystem.BuildingHappinessFactorValue other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return -math.abs(this.m_Value).CompareTo(math.abs(other.m_Value));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> __Game_Prefabs_ConsumptionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> __Game_Buildings_CrimeProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> __Game_Prefabs_OfficeBuilding_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CompanyData> __Game_Companies_CompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> __Game_Companies_ServiceAvailable_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZonePropertiesData> __Game_Prefabs_ZonePropertiesData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> __Game_Companies_ServiceCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TradeCost> __Game_Companies_TradeCost_RO_BufferLookup;
      public BufferLookup<HappinessFactorParameterData> __Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PollutionData> __Game_Prefabs_PollutionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PollutionModifierData> __Game_Prefabs_PollutionModifierData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Extractor> __Game_Areas_Extractor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RO_ComponentLookup = state.GetComponentLookup<ConsumptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferLookup = state.GetBufferLookup<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentLookup = state.GetComponentLookup<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RO_ComponentLookup = state.GetComponentLookup<CrimeProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentLookup = state.GetComponentLookup<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup = state.GetComponentLookup<OfficeBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RO_ComponentLookup = state.GetComponentLookup<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentLookup = state.GetComponentLookup<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RO_ComponentLookup = state.GetComponentLookup<ServiceAvailable>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup = state.GetComponentLookup<ZonePropertiesData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceCompanyData_RO_ComponentLookup = state.GetComponentLookup<ServiceCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RO_BufferLookup = state.GetBufferLookup<TradeCost>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HappinessFactorParameterData_RW_BufferLookup = state.GetBufferLookup<HappinessFactorParameterData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PollutionData_RO_ComponentLookup = state.GetComponentLookup<PollutionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup = state.GetComponentLookup<PollutionModifierData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RO_ComponentLookup = state.GetComponentLookup<Extractor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup = state.GetComponentLookup<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentLookup = state.GetComponentLookup<Game.Citizens.Student>(true);
      }
    }
  }
}
