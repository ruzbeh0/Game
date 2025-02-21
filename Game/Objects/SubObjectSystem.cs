// Decompiled with JetBrains decompiler
// Type: Game.Objects.SubObjectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Creatures;
using Game.Economy;
using Game.Effects;
using Game.Net;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class SubObjectSystem : GameSystemBase
  {
    private const int kMaxSubObjectDepth = 7;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private ToolSystem m_ToolSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ModificationBarrier2B m_ModificationBarrier;
    private EntityQuery m_UpdateQuery;
    private EntityQuery m_TempQuery;
    private EntityQuery m_ContainerQuery;
    private EntityQuery m_BuildingSettingsQuery;
    private EntityQuery m_HappinessParameterQuery;
    private ComponentTypeSet m_AppliedTypes;
    private NativeQueue<Entity> m_LoopErrorPrefabs;
    private SubObjectSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2B>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<SubObject>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Common.Event>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<RentersUpdated>(),
          ComponentType.ReadOnly<SubObjectsUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ContainerQuery = this.GetEntityQuery(ComponentType.ReadOnly<EditorContainerData>(), ComponentType.ReadOnly<ObjectData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdateQuery);
      // ISSUE: reference to a compiler-generated field
      this.m_LoopErrorPrefabs = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LoopErrorPrefabs.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.ShowLoopErrors();
      NativeQueue<SubObjectSystem.SubObjectOwnerData> nativeQueue = new NativeQueue<SubObjectSystem.SubObjectOwnerData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<SubObjectSystem.SubObjectOwnerData> nativeList = new NativeList<SubObjectSystem.SubObjectOwnerData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelHashMap<Entity, SubObjectSystem.SubObjectOwnerData> nativeParallelHashMap = new NativeParallelHashMap<Entity, SubObjectSystem.SubObjectOwnerData>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Object_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObjectsUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Object_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      SubObjectSystem.CheckSubObjectOwnersJob jobData1 = new SubObjectSystem.CheckSubObjectOwnersJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_ObjectType = this.__TypeHandle.__Game_Objects_Object_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_SubObjectsUpdatedType = this.__TypeHandle.__Game_Objects_SubObjectsUpdated_RO_ComponentTypeHandle,
        m_RentersUpdatedType = this.__TypeHandle.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle,
        m_ServiceUpgradeType = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_VehicleType = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle,
        m_CreatureType = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentTypeHandle,
        m_CreatedData = this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_SecondaryData = this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup,
        m_ObjectData = this.__TypeHandle.__Game_Objects_Object_RO_ComponentLookup,
        m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_AppliedTypes = this.m_AppliedTypes,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_OwnerQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SubObjectSystem.CollectSubObjectOwnersJob jobData2 = new SubObjectSystem.CollectSubObjectOwnersJob()
      {
        m_OwnerQueue = nativeQueue,
        m_OwnerList = nativeList,
        m_OwnerMap = nativeParallelHashMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SubObjectSystem.FillIgnoreSetJob jobData3 = new SubObjectSystem.FillIgnoreSetJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_IgnoreSet = nativeParallelHashSet
      };
      Entity singletonEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor() && !this.m_ContainerQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        singletonEntity = this.m_ContainerQuery.GetSingletonEntity();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AffiliatedBrandElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CompanyBrandElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Watercraft_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Area_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_StreetLight_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Aligned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StreetLightData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MovingObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ThemeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PillarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SubObjectSystem.UpdateSubObjectsJob jobData4 = new SubObjectSystem.UpdateSubObjectsJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabNetObjectData = this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup,
        m_PrefabPillarData = this.__TypeHandle.__Game_Prefabs_PillarData_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_PrefabThemeData = this.__TypeHandle.__Game_Prefabs_ThemeData_RO_ComponentLookup,
        m_PrefabMovingObjectData = this.__TypeHandle.__Game_Prefabs_MovingObjectData_RO_ComponentLookup,
        m_PrefabQuantityObjectData = this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup,
        m_PrefabWorkVehicleData = this.__TypeHandle.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup,
        m_PrefabEffectData = this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup,
        m_PrefabStreetLightData = this.__TypeHandle.__Game_Prefabs_StreetLightData_RO_ComponentLookup,
        m_PrefabPlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_PrefabCargoTransportVehicleData = this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_PrefabPlaceholderObjectData = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabBuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_PrefabTreeData = this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabMeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
        m_StorageCompanyData = this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_AlignedData = this.__TypeHandle.__Game_Objects_Aligned_RO_ComponentLookup,
        m_SecondaryData = this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_StreetLightData = this.__TypeHandle.__Game_Objects_StreetLight_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_RelativeData = this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_OverriddenData = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup,
        m_SurfaceData = this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup,
        m_StackData = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup,
        m_UnderConstructionData = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
        m_MailProducerData = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup,
        m_GarbageProducerData = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup,
        m_GarbageFacilityData = this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ResidentialPropertyData = this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentLookup,
        m_CityServiceUpkeepData = this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentLookup,
        m_NetNodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_NetEdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NetCurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup,
        m_FixedData = this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
        m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
        m_CompanyData = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup,
        m_CitizenData = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HouseholdData = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_HomelessHousehold = this.__TypeHandle.__Game_Citizens_HomelessHousehold_RO_ComponentLookup,
        m_AreaData = this.__TypeHandle.__Game_Areas_Area_RO_ComponentLookup,
        m_AreaGeometryData = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup,
        m_AreaClearData = this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup,
        m_DeliveryTruckData = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_WatercraftData = this.__TypeHandle.__Game_Vehicles_Watercraft_RO_ComponentLookup,
        m_BuildingRenters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_PrefabSubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup,
        m_PrefabSubLanes = this.__TypeHandle.__Game_Prefabs_SubLane_RO_BufferLookup,
        m_PlaceholderObjects = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_ObjectRequirements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup,
        m_PrefabEffects = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup,
        m_PrefabActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_CompanyBrands = this.__TypeHandle.__Game_Prefabs_CompanyBrandElement_RO_BufferLookup,
        m_AffiliatedBrands = this.__TypeHandle.__Game_Prefabs_AffiliatedBrandElement_RO_BufferLookup,
        m_PrefabSubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_PrefabProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_PrefabServiceUpkeepDatas = this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_HouseholdAnimals = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_RandomSeed = RandomSeed.Next(),
        m_DefaultTheme = this.m_CityConfigurationSystem.defaultTheme,
        m_TransformEditor = singletonEntity,
        m_BuildingConfigurationData = this.m_BuildingSettingsQuery.GetSingleton<BuildingConfigurationData>(),
        m_HappinessParameterData = this.m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>(),
        m_AppliedTypes = this.m_AppliedTypes,
        m_OwnerList = nativeList.AsDeferredJobArray(),
        m_IgnoreSet = nativeParallelHashSet,
        m_OwnerMap = nativeParallelHashMap,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_LoopErrorPrefabs = this.m_LoopErrorPrefabs.AsParallelWriter(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn1 = jobData1.ScheduleParallel<SubObjectSystem.CheckSubObjectOwnersJob>(this.m_UpdateQuery, this.Dependency);
      JobHandle jobHandle1 = jobData2.Schedule<SubObjectSystem.CollectSubObjectOwnersJob>(dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle job1 = jobData3.Schedule<SubObjectSystem.FillIgnoreSetJob>(this.m_TempQuery, this.Dependency);
      NativeList<SubObjectSystem.SubObjectOwnerData> list = nativeList;
      JobHandle dependsOn2 = JobHandle.CombineDependencies(jobHandle1, job1, deps);
      JobHandle jobHandle2 = jobData4.Schedule<SubObjectSystem.UpdateSubObjectsJob, SubObjectSystem.SubObjectOwnerData>(list, 1, dependsOn2);
      nativeQueue.Dispose(jobHandle1);
      nativeList.Dispose(jobHandle2);
      nativeParallelHashSet.Dispose(jobHandle2);
      nativeParallelHashMap.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = jobHandle2;
    }

    private void ShowLoopErrors()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoopErrorPrefabs.IsEmpty())
        return;
      // ISSUE: reference to a compiler-generated field
      NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(this.m_LoopErrorPrefabs.Count, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      Entity entity1;
      // ISSUE: reference to a compiler-generated field
      while (this.m_LoopErrorPrefabs.TryDequeue(out entity1))
        nativeParallelHashSet.Add(entity1);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = this.World.GetExistingSystemManaged<PrefabSystem>();
      NativeArray<Entity> nativeArray = nativeParallelHashSet.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      foreach (Entity entity2 in nativeArray)
      {
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab = existingSystemManaged.GetPrefab<PrefabBase>(entity2);
        COSystemBase.baseLog.ErrorFormat("Sub objects are nested too deep in '{0}'. Are you using a parent object as a sub object?", (object) prefab.name);
      }
      nativeParallelHashSet.Dispose();
      nativeArray.Dispose();
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
    public SubObjectSystem()
    {
    }

    private struct SubObjectOwnerData
    {
      public Entity m_Owner;
      public Entity m_Original;
      public bool m_Temp;
      public bool m_Created;
      public bool m_Deleted;

      public SubObjectOwnerData(
        Entity owner,
        Entity original,
        bool temp,
        bool created,
        bool deleted)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        this.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        this.m_Temp = temp;
        // ISSUE: reference to a compiler-generated field
        this.m_Created = created;
        // ISSUE: reference to a compiler-generated field
        this.m_Deleted = deleted;
      }
    }

    public struct SubObjectData : IComparable<SubObjectSystem.SubObjectData>
    {
      public Entity m_SubObject;
      public float m_Radius;

      public SubObjectData(Entity subObject, float radius)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SubObject = subObject;
        // ISSUE: reference to a compiler-generated field
        this.m_Radius = radius;
      }

      public int CompareTo(SubObjectSystem.SubObjectData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(0, math.select(1, -1, (double) this.m_Radius > (double) other.m_Radius), (double) this.m_Radius != (double) other.m_Radius);
      }
    }

    private struct DeepSubObjectOwnerData
    {
      public Transform m_Transform;
      public Temp m_Temp;
      public Entity m_Entity;
      public Entity m_Prefab;
      public float m_Elevation;
      public PseudoRandomSeed m_RandomSeed;
      public bool m_Deleted;
      public bool m_New;
      public bool m_HasRandomSeed;
      public bool m_UnderConstruction;
      public bool m_Destroyed;
      public bool m_Overridden;
      public int m_Depth;
    }

    private struct PlaceholderKey : IEquatable<SubObjectSystem.PlaceholderKey>
    {
      public Entity m_GroupPrefab;
      public int m_GroupIndex;

      public PlaceholderKey(Entity groupPrefab, int groupIndex)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_GroupPrefab = groupPrefab;
        // ISSUE: reference to a compiler-generated field
        this.m_GroupIndex = groupIndex;
      }

      public bool Equals(SubObjectSystem.PlaceholderKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_GroupPrefab.Equals(other.m_GroupPrefab) && this.m_GroupIndex == other.m_GroupIndex;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (17 * 31 + this.m_GroupPrefab.GetHashCode()) * 31 + this.m_GroupIndex.GetHashCode();
      }
    }

    private struct UpdateSubObjectsData
    {
      public NativeParallelMultiHashMap<Entity, Entity> m_OldEntities;
      public NativeParallelMultiHashMap<Entity, Entity> m_OriginalEntities;
      public NativeParallelHashMap<Entity, int2> m_PlaceholderRequirements;
      public NativeParallelHashMap<SubObjectSystem.PlaceholderKey, Unity.Mathematics.Random> m_SelectedSpawnabled;
      public NativeList<AreaUtils.ObjectItem> m_ObjectBuffer;
      public NativeList<SubObjectSystem.DeepSubObjectOwnerData> m_DeepOwners;
      public NativeList<ClearAreaData> m_ClearAreas;
      public ObjectRequirementFlags m_PlaceholderRequirementFlags;
      public Resource m_StoredResources;
      public bool m_RequirementsSearched;

      public void EnsureOldEntities(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OldEntities.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_OldEntities = new NativeParallelMultiHashMap<Entity, Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureOriginalEntities(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OriginalEntities.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalEntities = new NativeParallelMultiHashMap<Entity, Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsurePlaceholderRequirements(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceholderRequirements.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_PlaceholderRequirements = new NativeParallelHashMap<Entity, int2>(10, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureSelectedSpawnables(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedSpawnabled.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedSpawnabled = new NativeParallelHashMap<SubObjectSystem.PlaceholderKey, Unity.Mathematics.Random>(10, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureObjectBuffer(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectBuffer.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectBuffer = new NativeList<AreaUtils.ObjectItem>(32, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void EnsureDeepOwners(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_DeepOwners.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_DeepOwners = new NativeList<SubObjectSystem.DeepSubObjectOwnerData>(16, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void Clear(bool deepOwners)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OldEntities.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OldEntities.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OriginalEntities.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OriginalEntities.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (deepOwners && this.m_PlaceholderRequirements.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PlaceholderRequirements.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (deepOwners && this.m_SelectedSpawnabled.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedSpawnabled.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectBuffer.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ObjectBuffer.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (deepOwners && this.m_DeepOwners.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_DeepOwners.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ClearAreas.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ClearAreas.Clear();
        }
        if (!deepOwners)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_PlaceholderRequirementFlags = (ObjectRequirementFlags) 0;
        // ISSUE: reference to a compiler-generated field
        this.m_StoredResources = Resource.NoResource;
        // ISSUE: reference to a compiler-generated field
        this.m_RequirementsSearched = false;
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OldEntities.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OldEntities.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OriginalEntities.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OriginalEntities.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceholderRequirements.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PlaceholderRequirements.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedSpawnabled.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedSpawnabled.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectBuffer.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ObjectBuffer.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_DeepOwners.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_DeepOwners.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ClearAreas.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ClearAreas.Dispose();
      }
    }

    [BurstCompile]
    private struct CheckSubObjectOwnersJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Object> m_ObjectType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<SubObjectsUpdated> m_SubObjectsUpdatedType;
      [ReadOnly]
      public ComponentTypeHandle<RentersUpdated> m_RentersUpdatedType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> m_VehicleType;
      [ReadOnly]
      public ComponentTypeHandle<Creature> m_CreatureType;
      [ReadOnly]
      public ComponentLookup<Created> m_CreatedData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Secondary> m_SecondaryData;
      [ReadOnly]
      public ComponentLookup<Object> m_ObjectData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SubObjectSystem.SubObjectOwnerData>.ParallelWriter m_OwnerQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<RentersUpdated> nativeArray1 = chunk.GetNativeArray<RentersUpdated>(ref this.m_RentersUpdatedType);
        if (nativeArray1.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity property = nativeArray1[index].m_Property;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.HasBuffer(property))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_OwnerQueue.Enqueue(new SubObjectSystem.SubObjectOwnerData(property, Entity.Null, false, this.m_CreatedData.HasComponent(property), this.m_DeletedData.HasComponent(property)));
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<SubObjectsUpdated> nativeArray2 = chunk.GetNativeArray<SubObjectsUpdated>(ref this.m_SubObjectsUpdatedType);
          if (nativeArray2.Length != 0)
          {
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Entity owner = nativeArray2[index].m_Owner;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubObjects.HasBuffer(owner))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_OwnerQueue.Enqueue(new SubObjectSystem.SubObjectOwnerData(owner, Entity.Null, false, this.m_CreatedData.HasComponent(owner), this.m_DeletedData.HasComponent(owner)));
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            bool deleted = chunk.Has<Deleted>(ref this.m_DeletedType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Object>(ref this.m_ObjectType) && chunk.Has<Owner>(ref this.m_OwnerType) && !chunk.Has<Game.Buildings.ServiceUpgrade>(ref this.m_ServiceUpgradeType) && !chunk.Has<Building>(ref this.m_BuildingType) && !chunk.Has<Vehicle>(ref this.m_VehicleType) && !chunk.Has<Creature>(ref this.m_CreatureType) && nativeArray3.Length == 0 | deleted)
              return;
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray4 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<SubObject> bufferAccessor = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
            if (deleted)
            {
              for (int index1 = 0; index1 < nativeArray4.Length; ++index1)
              {
                Entity entity = nativeArray4[index1];
                DynamicBuffer<SubObject> dynamicBuffer = bufferAccessor[index1];
                for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
                {
                  Entity subObject = dynamicBuffer[index2].m_SubObject;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_DeletedData.HasComponent(subObject) && !this.m_SecondaryData.HasComponent(subObject))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_OwnerData.HasComponent(subObject) && this.m_OwnerData[subObject].m_Owner == entity && (this.m_ServiceUpgradeData.HasComponent(subObject) || this.m_BuildingData.HasComponent(subObject)))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.RemoveComponent(unfilteredChunkIndex, subObject, in this.m_AppliedTypes);
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, subObject, new Deleted());
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_SubObjects.HasBuffer(subObject))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: object of a compiler-generated type is created
                        this.m_OwnerQueue.Enqueue(new SubObjectSystem.SubObjectOwnerData(subObject, Entity.Null, false, false, true));
                      }
                    }
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_AttachedData.HasComponent(subObject) && this.m_AttachedData[subObject].m_Parent == entity)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.SetComponent<Attached>(unfilteredChunkIndex, subObject, new Attached());
                    }
                  }
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            bool created = chunk.Has<Created>(ref this.m_CreatedType);
            for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
            {
              Entity owner1 = nativeArray4[index3];
              if (nativeArray3.Length != 0)
              {
                Temp temp = nativeArray3[index3];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_OwnerQueue.Enqueue(new SubObjectSystem.SubObjectOwnerData(owner1, temp.m_Original, true, created, deleted));
                // ISSUE: reference to a compiler-generated field
                if (!deleted && this.m_SubObjects.HasBuffer(temp.m_Original))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<SubObject> subObject1 = this.m_SubObjects[temp.m_Original];
                  for (int index4 = 0; index4 < subObject1.Length; ++index4)
                  {
                    Entity subObject2 = subObject1[index4].m_SubObject;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_OwnerData.HasComponent(subObject2) && this.m_AttachedData.HasComponent(subObject2) && !this.m_SecondaryData.HasComponent(subObject2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Owner owner2 = this.m_OwnerData[subObject2];
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (owner2.m_Owner != temp.m_Original && this.m_AttachedData[subObject2].m_Parent == temp.m_Original && !this.m_HiddenData.HasComponent(owner2.m_Owner))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        while (this.m_OwnerData.HasComponent(owner2.m_Owner) && this.m_ObjectData.HasComponent(owner2.m_Owner) && !this.m_ServiceUpgradeData.HasComponent(owner2.m_Owner) && !this.m_BuildingData.HasComponent(owner2.m_Owner))
                        {
                          // ISSUE: reference to a compiler-generated field
                          owner2 = this.m_OwnerData[owner2.m_Owner];
                        }
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: object of a compiler-generated type is created
                        this.m_OwnerQueue.Enqueue(new SubObjectSystem.SubObjectOwnerData(owner2.m_Owner, Entity.Null, true, this.m_CreatedData.HasComponent(owner2.m_Owner), this.m_DeletedData.HasComponent(owner2.m_Owner)));
                      }
                    }
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_OwnerQueue.Enqueue(new SubObjectSystem.SubObjectOwnerData(owner1, Entity.Null, false, created, deleted));
                DynamicBuffer<SubObject> dynamicBuffer = bufferAccessor[index3];
                for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
                {
                  Entity subObject = dynamicBuffer[index5].m_SubObject;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.HasComponent(subObject) && this.m_AttachedData.HasComponent(subObject) && !this.m_SecondaryData.HasComponent(subObject))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Owner owner3 = this.m_OwnerData[subObject];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (owner3.m_Owner != owner1 && this.m_AttachedData[subObject].m_Parent == owner1 && !this.m_HiddenData.HasComponent(owner3.m_Owner))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      while (this.m_OwnerData.HasComponent(owner3.m_Owner) && this.m_ObjectData.HasComponent(owner3.m_Owner) && !this.m_ServiceUpgradeData.HasComponent(owner3.m_Owner) && !this.m_BuildingData.HasComponent(owner3.m_Owner))
                      {
                        // ISSUE: reference to a compiler-generated field
                        owner3 = this.m_OwnerData[owner3.m_Owner];
                      }
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: object of a compiler-generated type is created
                      this.m_OwnerQueue.Enqueue(new SubObjectSystem.SubObjectOwnerData(owner3.m_Owner, Entity.Null, false, this.m_CreatedData.HasComponent(owner3.m_Owner), this.m_DeletedData.HasComponent(owner3.m_Owner)));
                    }
                  }
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
    private struct CollectSubObjectOwnersJob : IJob
    {
      public NativeQueue<SubObjectSystem.SubObjectOwnerData> m_OwnerQueue;
      public NativeList<SubObjectSystem.SubObjectOwnerData> m_OwnerList;
      public NativeParallelHashMap<Entity, SubObjectSystem.SubObjectOwnerData> m_OwnerMap;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        SubObjectSystem.SubObjectOwnerData subObjectOwnerData1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerQueue.TryDequeue(out subObjectOwnerData1))
        {
          // ISSUE: variable of a compiler-generated type
          SubObjectSystem.SubObjectOwnerData subObjectOwnerData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerMap.TryGetValue(subObjectOwnerData1.m_Owner, out subObjectOwnerData2))
          {
            // ISSUE: reference to a compiler-generated field
            if (subObjectOwnerData1.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              subObjectOwnerData1.m_Created |= subObjectOwnerData2.m_Created;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              subObjectOwnerData1.m_Deleted |= subObjectOwnerData2.m_Deleted;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OwnerMap[subObjectOwnerData1.m_Owner] = subObjectOwnerData1;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              subObjectOwnerData2.m_Created |= subObjectOwnerData1.m_Created;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              subObjectOwnerData2.m_Deleted |= subObjectOwnerData1.m_Deleted;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OwnerMap[subObjectOwnerData1.m_Owner] = subObjectOwnerData2;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OwnerMap.Add(subObjectOwnerData1.m_Owner, subObjectOwnerData1);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerList.SetCapacity(this.m_OwnerMap.Count());
        // ISSUE: reference to a compiler-generated field
        NativeParallelHashMap<Entity, SubObjectSystem.SubObjectOwnerData>.Enumerator enumerator = this.m_OwnerMap.GetEnumerator();
        while (enumerator.MoveNext())
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OwnerList.Add(in enumerator.Current.Value);
        }
        enumerator.Dispose();
      }
    }

    [BurstCompile]
    private struct FillIgnoreSetJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      public NativeParallelHashSet<Entity> m_IgnoreSet;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Temp temp = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          this.m_IgnoreSet.Add(entity);
          if (temp.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_IgnoreSet.Add(temp.m_Original);
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
    private struct UpdateSubObjectsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_PrefabObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<NetObjectData> m_PrefabNetObjectData;
      [ReadOnly]
      public ComponentLookup<PillarData> m_PrefabPillarData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public ComponentLookup<ThemeData> m_PrefabThemeData;
      [ReadOnly]
      public ComponentLookup<MovingObjectData> m_PrefabMovingObjectData;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> m_PrefabQuantityObjectData;
      [ReadOnly]
      public ComponentLookup<WorkVehicleData> m_PrefabWorkVehicleData;
      [ReadOnly]
      public ComponentLookup<EffectData> m_PrefabEffectData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.StreetLightData> m_PrefabStreetLightData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_PrefabCargoTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceholderObjectData> m_PrefabPlaceholderObjectData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public ComponentLookup<TreeData> m_PrefabTreeData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> m_PrefabMeshData;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageCompanyData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Aligned> m_AlignedData;
      [ReadOnly]
      public ComponentLookup<Secondary> m_SecondaryData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<StreetLight> m_StreetLightData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Relative> m_RelativeData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Overridden> m_OverriddenData;
      [ReadOnly]
      public ComponentLookup<Surface> m_SurfaceData;
      [ReadOnly]
      public ComponentLookup<Stack> m_StackData;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> m_UnderConstructionData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducerData;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_GarbageProducerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.GarbageFacility> m_GarbageFacilityData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<ResidentialProperty> m_ResidentialPropertyData;
      [ReadOnly]
      public ComponentLookup<CityServiceUpkeep> m_CityServiceUpkeepData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NetNodeData;
      [ReadOnly]
      public ComponentLookup<Edge> m_NetEdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_NetCurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<Game.Net.OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Fixed> m_FixedData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_CompanyData;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenData;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdData;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> m_HomelessHousehold;
      [ReadOnly]
      public ComponentLookup<Area> m_AreaData;
      [ReadOnly]
      public ComponentLookup<Geometry> m_AreaGeometryData;
      [ReadOnly]
      public ComponentLookup<Clear> m_AreaClearData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTruckData;
      [ReadOnly]
      public ComponentLookup<Watercraft> m_WatercraftData;
      [ReadOnly]
      public BufferLookup<Renter> m_BuildingRenters;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> m_PrefabSubObjects;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubLane> m_PrefabSubLanes;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PlaceholderObjects;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_ObjectRequirements;
      [ReadOnly]
      public BufferLookup<Effect> m_PrefabEffects;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_PrefabActivityLocations;
      [ReadOnly]
      public BufferLookup<CompanyBrandElement> m_CompanyBrands;
      [ReadOnly]
      public BufferLookup<AffiliatedBrandElement> m_AffiliatedBrands;
      [ReadOnly]
      public BufferLookup<SubMesh> m_PrefabSubMeshes;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_PrefabProceduralBones;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> m_PrefabServiceUpkeepDatas;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Resources> m_Resources;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimals;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public Entity m_DefaultTheme;
      [ReadOnly]
      public Entity m_TransformEditor;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      [ReadOnly]
      public CitizenHappinessParameterData m_HappinessParameterData;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      [ReadOnly]
      public NativeArray<SubObjectSystem.SubObjectOwnerData> m_OwnerList;
      [ReadOnly]
      public NativeParallelHashSet<Entity> m_IgnoreSet;
      [ReadOnly]
      public NativeParallelHashMap<Entity, SubObjectSystem.SubObjectOwnerData> m_OwnerMap;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public NativeQueue<Entity>.ParallelWriter m_LoopErrorPrefabs;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        SubObjectSystem.SubObjectOwnerData owner = this.m_OwnerList[index];
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!owner.m_Deleted && this.m_ServiceUpgradeData.HasComponent(owner.m_Owner) && this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData1) && this.m_OwnerMap.ContainsKey(componentData1.m_Owner))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[owner.m_Owner];
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool relative = false;
        bool interpolated = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool native = this.m_NativeData.HasComponent(owner.m_Owner);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(owner.m_Owner))
        {
          flag1 = true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EditorMode && this.m_PrefabMovingObjectData.HasComponent(prefabRef.m_Prefab))
          {
            relative = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            interpolated = this.m_InterpolatedTransformData.HasComponent(owner.m_Owner);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetNodeData.HasComponent(owner.m_Owner))
          {
            flag2 = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetEdgeData.HasComponent(owner.m_Owner))
            {
              flag3 = true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_AreaData.HasComponent(owner.m_Owner))
                flag4 = true;
            }
          }
        }
        bool flag5 = false;
        Temp ownerTemp = new Temp();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.HasComponent(owner.m_Owner))
        {
          flag5 = true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ownerTemp = this.m_TempData[owner.m_Owner];
        }
        bool useIgnoreSet = false;
        float ownerElevation = 0.0f;
        DynamicBuffer<InstalledUpgrade> bufferData = new DynamicBuffer<InstalledUpgrade>();
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_InstalledUpgrades.TryGetBuffer(owner.m_Owner, out bufferData);
        }
        // ISSUE: reference to a compiler-generated field
        if (!owner.m_Deleted)
        {
          if (!flag2 && !flag3)
          {
            // ISSUE: reference to a compiler-generated field
            if (owner.m_Temp)
            {
              // ISSUE: reference to a compiler-generated field
              if ((ownerTemp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) != (TempFlags) 0 || !flag5 || this.m_EditorMode && ownerTemp.m_Original != Entity.Null && (!bufferData.IsCreated || bufferData.Length == 0))
                useIgnoreSet = true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!owner.m_Created && this.m_EditorMode && !this.m_UpdatedData.HasComponent(prefabRef.m_Prefab) && (!bufferData.IsCreated || bufferData.Length == 0))
                return;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.HasComponent(owner.m_Owner))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ownerElevation = this.m_ElevationData[owner.m_Owner].m_Elevation;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetElevationData.HasComponent(owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ownerElevation = math.cmin(this.m_NetElevationData[owner.m_Owner].m_Elevation);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (owner.m_Temp && !flag5)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          owner.m_Original = owner.m_Owner;
        }
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SubObjectSystem.UpdateSubObjectsData updateData = new SubObjectSystem.UpdateSubObjectsData();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SubObject> subObject1 = this.m_SubObjects[owner.m_Owner];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FillOldSubObjectsBuffer(owner.m_Owner, subObject1, ref updateData, owner.m_Temp, useIgnoreSet);
        // ISSUE: reference to a compiler-generated field
        if (!owner.m_Deleted)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.HasBuffer(owner.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubObject> subObject2 = this.m_SubObjects[owner.m_Original];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FillOriginalSubObjectsBuffer(owner.m_Original, subObject2, ref updateData, useIgnoreSet);
          }
          if (bufferData.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FillClearAreas(owner.m_Owner, bufferData, ref updateData);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = !this.m_PseudoRandomSeedData.HasComponent(owner.m_Owner) ? this.m_RandomSeed.GetRandom(index) : this.m_PseudoRandomSeedData[owner.m_Owner].GetRandom((uint) PseudoRandomSeed.kSubObject);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.EnsurePlaceholderRequirements(owner.m_Owner, prefabRef.m_Prefab, ref updateData, ref random, flag1);
          if (useIgnoreSet)
          {
            Transform ownerTransform = new Transform();
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ownerTransform = this.m_TransformData[owner.m_Owner];
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.DuplicateSubObjects(index, ref random, owner.m_Owner, owner.m_Owner, owner.m_Original, ownerTransform, ref updateData, prefabRef.m_Prefab, owner.m_Temp, ownerTemp, ownerElevation, flag1, native, relative, interpolated, 0);
          }
          else if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[owner.m_Owner];
            bool isUnderConstruction = false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool isDestroyed = this.m_DestroyedData.HasComponent(owner.m_Owner);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool isOverridden = this.m_OverriddenData.HasComponent(owner.m_Owner);
            UnderConstruction componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_UnderConstructionData.TryGetComponent(owner.m_Owner, out componentData2))
              isUnderConstruction = componentData2.m_NewPrefab == Entity.Null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CreateSubObjects(index, ref random, owner.m_Owner, owner.m_Owner, transform, transform, transform, ref updateData, prefabRef.m_Prefab, true, false, false, owner.m_Temp, ownerTemp, ownerElevation, native, relative, interpolated, isUnderConstruction, isDestroyed, isOverridden, 0);
          }
          else if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node = this.m_NetNodeData[owner.m_Owner];
            Transform transform = new Transform(node.m_Position, node.m_Rotation);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CreateSubObjects(index, ref random, owner.m_Owner, owner.m_Owner, transform, transform, transform, ref updateData, prefabRef.m_Prefab, false, false, true, owner.m_Temp, ownerTemp, ownerElevation, native, false, false, false, false, false, 0);
          }
          else if (flag3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_NetCurveData[owner.m_Owner];
            Transform transform1 = new Transform(curve.m_Bezier.a, NetUtils.GetNodeRotation(MathUtils.StartTangent(curve.m_Bezier)));
            Transform transform2 = new Transform(MathUtils.Position(curve.m_Bezier, 0.5f), NetUtils.GetNodeRotation(MathUtils.Tangent(curve.m_Bezier, 0.5f)));
            Transform transform3 = new Transform(curve.m_Bezier.d, NetUtils.GetNodeRotation(-MathUtils.EndTangent(curve.m_Bezier)));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CreateSubObjects(index, ref random, owner.m_Owner, owner.m_Owner, transform1, transform2, transform3, ref updateData, prefabRef.m_Prefab, false, true, false, owner.m_Temp, ownerTemp, ownerElevation, native, false, false, false, false, false, 0);
          }
          else if (flag4)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Area area = this.m_AreaData[owner.m_Owner];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Geometry geometry = this.m_AreaGeometryData[owner.m_Owner];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[owner.m_Owner];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Triangle> areaTriangle = this.m_AreaTriangles[owner.m_Owner];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.RelocateSubObjects(index, ref random, owner.m_Owner, owner.m_Owner, owner.m_Original, area, geometry, areaNode, areaTriangle, ref updateData, prefabRef.m_Prefab, owner.m_Temp, ownerTemp, ownerElevation);
          }
          if (bufferData.IsCreated)
          {
            for (int index1 = 0; index1 < bufferData.Length; ++index1)
            {
              // ISSUE: variable of a compiler-generated type
              SubObjectSystem.SubObjectOwnerData subObjectOwnerData1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_OwnerMap.TryGetValue(bufferData[index1].m_Upgrade, out subObjectOwnerData1) && !subObjectOwnerData1.m_Deleted)
              {
                // ISSUE: reference to a compiler-generated method
                updateData.EnsureDeepOwners(Allocator.Temp);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                SubObjectSystem.DeepSubObjectOwnerData subObjectOwnerData2 = new SubObjectSystem.DeepSubObjectOwnerData()
                {
                  m_Transform = this.m_TransformData[subObjectOwnerData1.m_Owner],
                  m_Entity = subObjectOwnerData1.m_Owner,
                  m_Prefab = (Entity) this.m_PrefabRefData[subObjectOwnerData1.m_Owner],
                  m_RandomSeed = this.m_PseudoRandomSeedData[subObjectOwnerData1.m_Owner],
                  m_HasRandomSeed = true,
                  m_Depth = 1
                };
                Elevation componentData3;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ElevationData.TryGetComponent(subObjectOwnerData1.m_Owner, out componentData3))
                {
                  // ISSUE: reference to a compiler-generated field
                  subObjectOwnerData2.m_Elevation = componentData3.m_Elevation;
                }
                UnderConstruction componentData4;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_UnderConstructionData.TryGetComponent(subObjectOwnerData1.m_Owner, out componentData4))
                {
                  // ISSUE: reference to a compiler-generated field
                  subObjectOwnerData2.m_UnderConstruction = componentData4.m_NewPrefab == Entity.Null;
                }
                Temp componentData5;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.TryGetComponent(subObjectOwnerData1.m_Owner, out componentData5))
                {
                  // ISSUE: reference to a compiler-generated field
                  subObjectOwnerData2.m_Temp = componentData5;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                subObjectOwnerData2.m_Destroyed = this.m_DestroyedData.HasComponent(subObjectOwnerData1.m_Owner);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                subObjectOwnerData2.m_Overridden = this.m_OverriddenData.HasComponent(subObjectOwnerData1.m_Owner);
                // ISSUE: reference to a compiler-generated field
                updateData.m_DeepOwners.Add(in subObjectOwnerData2);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.RemoveUnusedOldSubObjects(index, owner.m_Owner, subObject1, ref updateData, owner.m_Temp, useIgnoreSet);
        // ISSUE: reference to a compiler-generated field
        if (updateData.m_DeepOwners.IsCreated)
        {
          int num1 = 0;
          // ISSUE: reference to a compiler-generated field
          while (num1 < updateData.m_DeepOwners.Length)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            SubObjectSystem.DeepSubObjectOwnerData deepOwner = updateData.m_DeepOwners[num1++];
            // ISSUE: reference to a compiler-generated method
            updateData.Clear(false);
            // ISSUE: reference to a compiler-generated field
            if (!deepOwner.m_New)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              subObject1 = this.m_SubObjects[deepOwner.m_Entity];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.FillOldSubObjectsBuffer(deepOwner.m_Entity, subObject1, ref updateData, owner.m_Temp, useIgnoreSet);
            }
            // ISSUE: reference to a compiler-generated field
            if (!deepOwner.m_Deleted)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Unity.Mathematics.Random random = !deepOwner.m_HasRandomSeed ? this.m_RandomSeed.GetRandom(index + num1 * 137209) : deepOwner.m_RandomSeed.GetRandom((uint) PseudoRandomSeed.kSubObject);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int num2 = this.HasSubRequirements(deepOwner.m_Prefab) ? 1 : 0;
              if (num2 != 0)
              {
                // ISSUE: reference to a compiler-generated field
                updateData.m_PlaceholderRequirements.Clear();
                // ISSUE: reference to a compiler-generated field
                updateData.m_PlaceholderRequirementFlags = (ObjectRequirementFlags) 0;
                // ISSUE: reference to a compiler-generated field
                updateData.m_StoredResources = Resource.NoResource;
                // ISSUE: reference to a compiler-generated field
                updateData.m_RequirementsSearched = false;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.EnsurePlaceholderRequirements(Entity.Null, deepOwner.m_Prefab, ref updateData, ref random, true);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.EnsurePlaceholderRequirements(owner.m_Owner, prefabRef.m_Prefab, ref updateData, ref random, flag1);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubObjects.HasBuffer(deepOwner.m_Temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<SubObject> subObject3 = this.m_SubObjects[deepOwner.m_Temp.m_Original];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.FillOriginalSubObjectsBuffer(deepOwner.m_Temp.m_Original, subObject3, ref updateData, useIgnoreSet);
              }
              if (useIgnoreSet)
              {
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
                this.DuplicateSubObjects(index, ref random, owner.m_Owner, deepOwner.m_Entity, deepOwner.m_Temp.m_Original, deepOwner.m_Transform, ref updateData, deepOwner.m_Prefab, owner.m_Temp, deepOwner.m_Temp, deepOwner.m_Elevation, true, native, relative, interpolated, deepOwner.m_Depth);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
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
                this.CreateSubObjects(index, ref random, owner.m_Owner, deepOwner.m_Entity, deepOwner.m_Transform, deepOwner.m_Transform, deepOwner.m_Transform, ref updateData, deepOwner.m_Prefab, true, false, false, owner.m_Temp, deepOwner.m_Temp, deepOwner.m_Elevation, native, relative, interpolated, deepOwner.m_UnderConstruction, deepOwner.m_Destroyed, deepOwner.m_Overridden, deepOwner.m_Depth);
              }
              if (num2 != 0)
              {
                // ISSUE: reference to a compiler-generated field
                updateData.m_PlaceholderRequirements.Clear();
                // ISSUE: reference to a compiler-generated field
                updateData.m_PlaceholderRequirementFlags = (ObjectRequirementFlags) 0;
                // ISSUE: reference to a compiler-generated field
                updateData.m_StoredResources = Resource.NoResource;
                // ISSUE: reference to a compiler-generated field
                updateData.m_RequirementsSearched = false;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (!deepOwner.m_New)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.RemoveUnusedOldSubObjects(index, deepOwner.m_Entity, subObject1, ref updateData, owner.m_Temp, useIgnoreSet);
            }
          }
        }
        // ISSUE: reference to a compiler-generated method
        updateData.Dispose();
      }

      private bool HasSubRequirements(Entity ownerPrefab)
      {
        DynamicBuffer<ObjectRequirementElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectRequirements.TryGetBuffer(ownerPrefab, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            if ((bufferData[index].m_Type & ObjectRequirementType.SelectOnly) != (ObjectRequirementType) 0)
              return true;
          }
        }
        return false;
      }

      private void FillOldSubObjectsBuffer(
        Entity owner,
        DynamicBuffer<SubObject> subObjects,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        bool isTemp,
        bool useIgnoreSet)
      {
        if (subObjects.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated method
        updateData.EnsureOldEntities(Allocator.Temp);
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(subObject) && !this.m_ServiceUpgradeData.HasComponent(subObject) && !this.m_BuildingData.HasComponent(subObject) && !this.m_SecondaryData.HasComponent(subObject) && this.m_OwnerData[subObject].m_Owner == owner && isTemp == this.m_TempData.HasComponent(subObject) && (!useIgnoreSet || !this.m_IgnoreSet.Contains(subObject)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode && this.m_EditorContainerData.HasComponent(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Tools.EditorContainer editorContainer = this.m_EditorContainerData[subObject];
              // ISSUE: reference to a compiler-generated field
              updateData.m_OldEntities.Add(editorContainer.m_Prefab, subObject);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[subObject];
              // ISSUE: reference to a compiler-generated field
              updateData.m_OldEntities.Add(prefabRef.m_Prefab, subObject);
            }
          }
        }
      }

      private void FillOriginalSubObjectsBuffer(
        Entity owner,
        DynamicBuffer<SubObject> subObjects,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        bool useIgnoreSet)
      {
        if (subObjects.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated method
        updateData.EnsureOriginalEntities(Allocator.Temp);
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(subObject) && !this.m_ServiceUpgradeData.HasComponent(subObject) && !this.m_BuildingData.HasComponent(subObject) && !this.m_SecondaryData.HasComponent(subObject) && this.m_OwnerData[subObject].m_Owner == owner && !this.m_TempData.HasComponent(subObject) && (!useIgnoreSet || !this.m_IgnoreSet.Contains(subObject)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode && this.m_EditorContainerData.HasComponent(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Tools.EditorContainer editorContainer = this.m_EditorContainerData[subObject];
              // ISSUE: reference to a compiler-generated field
              updateData.m_OriginalEntities.Add(editorContainer.m_Prefab, subObject);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[subObject];
              // ISSUE: reference to a compiler-generated field
              updateData.m_OriginalEntities.Add(prefabRef.m_Prefab, subObject);
            }
          }
        }
      }

      private void FillClearAreas(
        Entity owner,
        DynamicBuffer<InstalledUpgrade> installedUpgrades,
        ref SubObjectSystem.UpdateSubObjectsData updateData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ClearAreaHelpers.FillClearAreas(installedUpgrades, Entity.Null, this.m_TransformData, this.m_AreaClearData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_SubAreas, this.m_AreaNodes, this.m_AreaTriangles, ref updateData.m_ClearAreas);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ClearAreaHelpers.InitClearAreas(updateData.m_ClearAreas, this.m_TransformData[owner]);
      }

      private void RemoveUnusedOldSubObjects(
        int jobIndex,
        Entity owner,
        DynamicBuffer<SubObject> subObjects,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        bool isTemp,
        bool useIgnoreSet)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(subObject) && !this.m_ServiceUpgradeData.HasComponent(subObject) && !this.m_BuildingData.HasComponent(subObject) && !this.m_SecondaryData.HasComponent(subObject) && this.m_OwnerData[subObject].m_Owner == owner && (!useIgnoreSet || !this.m_IgnoreSet.Contains(subObject)))
          {
            // ISSUE: reference to a compiler-generated field
            if (isTemp == this.m_TempData.HasComponent(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity key = !this.m_EditorMode || !this.m_EditorContainerData.HasComponent(subObject) ? this.m_PrefabRefData[subObject].m_Prefab : this.m_EditorContainerData[subObject].m_Prefab;
              Entity entity;
              NativeParallelMultiHashMapIterator<Entity> it;
              // ISSUE: reference to a compiler-generated field
              if (updateData.m_OldEntities.TryGetFirstValue(key, out entity, out it))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent(jobIndex, entity, in this.m_AppliedTypes);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
                // ISSUE: reference to a compiler-generated field
                updateData.m_OldEntities.Remove(it);
                // ISSUE: reference to a compiler-generated field
                if (this.m_SubObjects.HasBuffer(entity))
                {
                  // ISSUE: reference to a compiler-generated method
                  updateData.EnsureDeepOwners(Allocator.Temp);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  updateData.m_DeepOwners.Add(new SubObjectSystem.DeepSubObjectOwnerData()
                  {
                    m_Entity = entity,
                    m_Deleted = true
                  });
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (isTemp && updateData.m_OriginalEntities.IsCreated)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity key = !this.m_EditorMode || !this.m_EditorContainerData.HasComponent(subObject) ? this.m_PrefabRefData[subObject].m_Prefab : this.m_EditorContainerData[subObject].m_Prefab;
                Entity e;
                NativeParallelMultiHashMapIterator<Entity> it;
                // ISSUE: reference to a compiler-generated field
                if (updateData.m_OriginalEntities.TryGetFirstValue(key, out e, out it))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Hidden>(jobIndex, e, new Hidden());
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, e, new BatchesUpdated());
                  // ISSUE: reference to a compiler-generated field
                  updateData.m_OriginalEntities.Remove(it);
                }
              }
            }
          }
        }
      }

      private void DuplicateSubObjects(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity topOwner,
        Entity owner,
        Entity original,
        Transform ownerTransform,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        Entity prefab,
        bool isTemp,
        Temp ownerTemp,
        float ownerElevation,
        bool hasTransform,
        bool native,
        bool relative,
        bool interpolated,
        int depth)
      {
        Transform transform1 = ownerTransform;
        Transform componentData1 = new Transform();
        // ISSUE: reference to a compiler-generated field
        if (hasTransform && this.m_TransformData.TryGetComponent(original, out componentData1))
          componentData1 = ObjectUtils.InverseTransform(componentData1);
        DynamicBuffer<SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(original, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ServiceUpgradeData.HasComponent(subObject) && !this.m_BuildingData.HasComponent(subObject) && !this.m_SecondaryData.HasComponent(subObject) && !this.m_IgnoreSet.Contains(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            Entity prefab1 = this.m_PrefabRefData[subObject].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            Transform world = this.m_TransformData[subObject];
            Transform transform2 = world;
            int parentMesh = 0;
            int groupIndex = 0;
            int probability = 100;
            int prefabSubIndex = -1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.HasComponent(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              LocalTransformCache localTransformCache = this.m_LocalTransformCacheData[subObject];
              transform2.m_Position = localTransformCache.m_Position;
              transform2.m_Rotation = localTransformCache.m_Rotation;
              parentMesh = localTransformCache.m_ParentMesh;
              groupIndex = localTransformCache.m_GroupIndex;
              probability = localTransformCache.m_Probability;
              prefabSubIndex = localTransformCache.m_PrefabSubIndex;
              world = ObjectUtils.LocalToWorld(transform1, transform2);
            }
            else if (hasTransform)
            {
              transform2 = ObjectUtils.WorldToLocal(componentData1, world);
              Elevation componentData2;
              // ISSUE: reference to a compiler-generated field
              parentMesh = !this.m_ElevationData.TryGetComponent(subObject, out componentData2) ? -1 : ObjectUtils.GetSubParentMesh(componentData2.m_Flags);
            }
            SubObjectFlags flags = (SubObjectFlags) 0;
            if (parentMesh == -1)
              flags |= SubObjectFlags.OnGround;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode && this.m_EditorContainerData.HasComponent(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Tools.EditorContainer editorContainer = this.m_EditorContainerData[subObject];
              // ISSUE: reference to a compiler-generated method
              this.CreateContainerObject(jobIndex, owner, isTemp, ownerTemp, ownerElevation, Entity.Null, world, transform2, ref updateData, editorContainer.m_Prefab, editorContainer.m_Scale, editorContainer.m_Intensity, parentMesh, editorContainer.m_GroupIndex, prefabSubIndex);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateSubObject(jobIndex, ref random, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, Entity.Null, transform1, world, transform2, flags, ref updateData, prefab1, this.m_EditorMode, native, relative, interpolated, false, false, false, false, -1, parentMesh, groupIndex, probability, prefabSubIndex, depth);
            }
          }
        }
      }

      private void RelocateSubObjects(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity topOwner,
        Entity owner,
        Entity original,
        Area area,
        Geometry geometry,
        DynamicBuffer<Game.Areas.Node> nodes,
        DynamicBuffer<Triangle> triangles,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        Entity prefab,
        bool isTemp,
        Temp ownerTemp,
        float ownerElevation)
      {
        DynamicBuffer<SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.TryGetBuffer(original, out bufferData))
        {
          ownerTemp.m_Flags &= ~TempFlags.Modify;
          NativeArray<SubObjectSystem.SubObjectData> array = new NativeArray<SubObjectSystem.SubObjectData>(bufferData.Length, Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          AreaGeometryData areaData = this.m_PrefabAreaGeometryData[prefab];
          int num1 = 0;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity subObject = bufferData[index].m_SubObject;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SecondaryData.HasComponent(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              Entity prefab1 = this.m_PrefabRefData[subObject].m_Prefab;
              ObjectGeometryData objectGeometryData = new ObjectGeometryData();
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabObjectGeometryData.HasComponent(prefab1))
              {
                // ISSUE: reference to a compiler-generated field
                objectGeometryData = this.m_PrefabObjectGeometryData[prefab1];
              }
              float num2 = (objectGeometryData.m_Flags & GeometryFlags.Circular) == GeometryFlags.None ? math.length(MathUtils.Size(objectGeometryData.m_Bounds.xz)) * 0.5f : objectGeometryData.m_Size.x * 0.5f;
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingData.HasComponent(subObject))
              {
                // ISSUE: reference to a compiler-generated field
                Transform transform = this.m_TransformData[subObject];
                float minNodeDistance = AreaUtils.GetMinNodeDistance(areaData);
                // ISSUE: reference to a compiler-generated method
                updateData.EnsureObjectBuffer(Allocator.Temp);
                // ISSUE: reference to a compiler-generated field
                updateData.m_ObjectBuffer.Add(new AreaUtils.ObjectItem(num2 + minNodeDistance, transform.m_Position.xz, Entity.Null));
              }
              else
              {
                // ISSUE: object of a compiler-generated type is created
                array[num1++] = new SubObjectSystem.SubObjectData()
                {
                  m_SubObject = subObject,
                  m_Radius = num2
                };
              }
            }
          }
          array.Sort<SubObjectSystem.SubObjectData>();
          for (int index = 0; index < num1; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Entity subObject = array[index].m_SubObject;
            // ISSUE: reference to a compiler-generated field
            Entity prefab2 = this.m_PrefabRefData[subObject].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[subObject];
            ObjectGeometryData objectGeometryData = new ObjectGeometryData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.HasComponent(prefab2))
            {
              // ISSUE: reference to a compiler-generated field
              objectGeometryData = this.m_PrefabObjectGeometryData[prefab2];
            }
            float radius;
            float3 float3;
            if ((objectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              radius = objectGeometryData.m_Size.x * 0.5f;
              float3 = new float3();
            }
            else
            {
              radius = math.length(MathUtils.Size(objectGeometryData.m_Bounds.xz)) * 0.5f;
              float3 = math.rotate(transform.m_Rotation, MathUtils.Center(objectGeometryData.m_Bounds)) with
              {
                y = 0.0f
              };
            }
            float extraRadius = 0.0f;
            float3 position = transform.m_Position + float3;
            // ISSUE: reference to a compiler-generated field
            if (AreaUtils.IntersectArea(position, radius, nodes, triangles) && !AreaUtils.IntersectEdges(position, radius, extraRadius, nodes) && !AreaUtils.IntersectObjects(position, radius, extraRadius, updateData.m_ObjectBuffer))
            {
              // ISSUE: reference to a compiler-generated method
              Entity oldSubObject = this.FindOldSubObject(prefab2, subObject, ref updateData);
              SubObjectFlags flags = (SubObjectFlags) 0;
              // ISSUE: reference to a compiler-generated field
              if (!this.m_ElevationData.HasComponent(oldSubObject))
                flags |= SubObjectFlags.OnGround;
              if (oldSubObject == Entity.Null)
                oldSubObject.Index = -1;
              // ISSUE: reference to a compiler-generated method
              updateData.EnsureObjectBuffer(Allocator.Temp);
              // ISSUE: reference to a compiler-generated field
              updateData.m_ObjectBuffer.Add(new AreaUtils.ObjectItem(radius + extraRadius, position.xz, Entity.Null));
              // ISSUE: reference to a compiler-generated method
              this.CreateSubObject(jobIndex, ref random, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, oldSubObject, transform, transform, new Transform(), flags, ref updateData, prefab2, false, false, false, false, false, false, false, false, -1, -1, 0, 100, -1, 0);
            }
          }
          array.Dispose();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SubObjects.HasBuffer(owner))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubObject> subObject1 = this.m_SubObjects[owner];
          NativeArray<SubObjectSystem.SubObjectData> array = new NativeArray<SubObjectSystem.SubObjectData>(subObject1.Length, Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          AreaGeometryData areaData = this.m_PrefabAreaGeometryData[prefab];
          int num3 = 0;
          for (int index = 0; index < subObject1.Length; ++index)
          {
            Entity subObject2 = subObject1[index].m_SubObject;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SecondaryData.HasComponent(subObject2))
            {
              // ISSUE: reference to a compiler-generated field
              Entity prefab3 = this.m_PrefabRefData[subObject2].m_Prefab;
              ObjectGeometryData objectGeometryData = new ObjectGeometryData();
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabObjectGeometryData.HasComponent(prefab3))
              {
                // ISSUE: reference to a compiler-generated field
                objectGeometryData = this.m_PrefabObjectGeometryData[prefab3];
              }
              float num4 = (objectGeometryData.m_Flags & GeometryFlags.Circular) == GeometryFlags.None ? math.length(MathUtils.Size(objectGeometryData.m_Bounds.xz)) * 0.5f : objectGeometryData.m_Size.x * 0.5f;
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingData.HasComponent(subObject2))
              {
                // ISSUE: reference to a compiler-generated field
                Transform transform = this.m_TransformData[subObject2];
                float minNodeDistance = AreaUtils.GetMinNodeDistance(areaData);
                // ISSUE: reference to a compiler-generated method
                updateData.EnsureObjectBuffer(Allocator.Temp);
                // ISSUE: reference to a compiler-generated field
                updateData.m_ObjectBuffer.Add(new AreaUtils.ObjectItem(num4 + minNodeDistance, transform.m_Position.xz, Entity.Null));
              }
              else
              {
                // ISSUE: object of a compiler-generated type is created
                array[num3++] = new SubObjectSystem.SubObjectData()
                {
                  m_SubObject = subObject2,
                  m_Radius = num4
                };
              }
            }
          }
          array.Sort<SubObjectSystem.SubObjectData>();
          for (int index = 0; index < num3; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Entity subObject3 = array[index].m_SubObject;
            // ISSUE: reference to a compiler-generated field
            Entity prefab4 = this.m_PrefabRefData[subObject3].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[subObject3];
            ObjectGeometryData objectGeometryData = new ObjectGeometryData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.HasComponent(prefab4))
            {
              // ISSUE: reference to a compiler-generated field
              objectGeometryData = this.m_PrefabObjectGeometryData[prefab4];
            }
            float radius;
            float3 float3;
            if ((objectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              radius = objectGeometryData.m_Size.x * 0.5f;
              float3 = new float3();
            }
            else
            {
              radius = math.length(MathUtils.Size(objectGeometryData.m_Bounds.xz)) * 0.5f;
              float3 = math.rotate(transform.m_Rotation, MathUtils.Center(objectGeometryData.m_Bounds)) with
              {
                y = 0.0f
              };
            }
            float extraRadius = 0.0f;
            float3 position = transform.m_Position + float3;
            // ISSUE: reference to a compiler-generated field
            if (!AreaUtils.IntersectArea(position, radius, nodes, triangles) || AreaUtils.IntersectEdges(position, radius, extraRadius, nodes) || AreaUtils.IntersectObjects(position, radius, extraRadius, updateData.m_ObjectBuffer))
            {
              position = AreaUtils.GetRandomPosition(ref random, geometry, nodes, triangles);
              // ISSUE: reference to a compiler-generated field
              if (AreaUtils.TryFitInside(ref position, radius, extraRadius, area, nodes, updateData.m_ObjectBuffer))
              {
                transform.m_Rotation = AreaUtils.GetRandomRotation(ref random, position, nodes);
                if ((objectGeometryData.m_Flags & GeometryFlags.Circular) == GeometryFlags.None)
                  float3 = math.rotate(transform.m_Rotation, MathUtils.Center(objectGeometryData.m_Bounds)) with
                  {
                    y = 0.0f
                  };
                transform.m_Position = position - float3;
              }
              else
                continue;
            }
            SubObjectFlags flags = (SubObjectFlags) 0;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_ElevationData.HasComponent(subObject3))
              flags |= SubObjectFlags.OnGround;
            // ISSUE: reference to a compiler-generated method
            updateData.EnsureObjectBuffer(Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            updateData.m_ObjectBuffer.Add(new AreaUtils.ObjectItem(radius + extraRadius, position.xz, Entity.Null));
            // ISSUE: reference to a compiler-generated method
            this.CreateSubObject(jobIndex, ref random, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, Entity.Null, transform, transform, new Transform(), flags, ref updateData, prefab4, false, false, false, false, false, false, false, false, -1, -1, 0, 100, -1, 0);
          }
          array.Dispose();
        }
      }

      private void CreateSubObjects(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity topOwner,
        Entity owner,
        Transform transform1,
        Transform transform2,
        Transform transform3,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        Entity prefab,
        bool isTransform,
        bool isEdge,
        bool isNode,
        bool isTemp,
        Temp ownerTemp,
        float ownerElevation,
        bool native,
        bool relative,
        bool interpolated,
        bool isUnderConstruction,
        bool isDestroyed,
        bool isOverridden,
        int depth)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorMode & isTransform)
        {
          DynamicBuffer<Game.Prefabs.SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSubObjects.TryGetBuffer(prefab, out bufferData))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              Game.Prefabs.SubObject subObject = bufferData[index];
              if ((subObject.m_Flags & SubObjectFlags.CoursePlacement) == (SubObjectFlags) 0 && isEdge == ((subObject.m_Flags & SubObjectFlags.EdgePlacement) != 0))
              {
                Transform transform = new Transform(subObject.m_Position, subObject.m_Rotation);
                Transform world = ObjectUtils.LocalToWorld(transform2, transform);
                int alignIndex = math.select(-1, index, isEdge);
                // ISSUE: reference to a compiler-generated method
                this.CreateSubObject(jobIndex, ref random, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, Entity.Null, transform2, world, transform, subObject.m_Flags, ref updateData, subObject.m_Prefab, true, native, relative, interpolated, false, false, isOverridden, false, alignIndex, subObject.m_ParentIndex, subObject.m_GroupIndex, subObject.m_Probability, index, depth);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabEffects.HasBuffer(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Effect> prefabEffect = this.m_PrefabEffects[prefab];
            for (int index = 0; index < prefabEffect.Length; ++index)
            {
              Effect effect = prefabEffect[index];
              if (!effect.m_Procedural)
              {
                Transform transform = new Transform(effect.m_Position, effect.m_Rotation);
                Transform world = ObjectUtils.LocalToWorld(transform2, transform);
                // ISSUE: reference to a compiler-generated method
                this.CreateContainerObject(jobIndex, owner, isTemp, ownerTemp, ownerElevation, Entity.Null, world, transform, ref updateData, effect.m_Effect, effect.m_Scale, effect.m_Intensity, effect.m_ParentMesh, effect.m_AnimationIndex, index);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabActivityLocations.HasBuffer(prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ActivityLocationElement> activityLocation = this.m_PrefabActivityLocations[prefab];
          for (int index = 0; index < activityLocation.Length; ++index)
          {
            ActivityLocationElement activityLocationElement = activityLocation[index];
            Transform transform = new Transform(activityLocationElement.m_Position, activityLocationElement.m_Rotation);
            Transform world = ObjectUtils.LocalToWorld(transform2, transform);
            // ISSUE: reference to a compiler-generated method
            this.CreateContainerObject(jobIndex, owner, isTemp, ownerTemp, ownerElevation, Entity.Null, world, transform, ref updateData, activityLocationElement.m_Prefab, (float3) 1f, 1f, 0, -1, index);
          }
        }
        else
        {
          Unity.Mathematics.Random random1 = random;
          DynamicBuffer<Game.Prefabs.SubObject> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSubObjects.TryGetBuffer(prefab, out bufferData1))
          {
            for (int index = 0; index < bufferData1.Length; ++index)
            {
              Game.Prefabs.SubObject subObject = bufferData1[index];
              if ((subObject.m_Flags & SubObjectFlags.CoursePlacement) == (SubObjectFlags) 0)
              {
                if ((subObject.m_Flags & SubObjectFlags.EdgePlacement) != (SubObjectFlags) 0)
                {
                  if (!isEdge)
                  {
                    // ISSUE: reference to a compiler-generated method
                    if ((subObject.m_Flags & SubObjectFlags.AllowCombine) != (SubObjectFlags) 0 && this.IsContinuous(topOwner, Entity.Null))
                      subObject.m_Position.z = 0.0f;
                    else
                      continue;
                  }
                }
                else if (isEdge)
                  continue;
                bool flag1 = true;
                bool flag2 = true;
                if ((subObject.m_Flags & SubObjectFlags.RequireElevated) != (SubObjectFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabNetGeometryData.HasComponent(prefab))
                  {
                    if (isEdge && (subObject.m_Flags & (SubObjectFlags.EdgePlacement | SubObjectFlags.MiddlePlacement)) == SubObjectFlags.EdgePlacement)
                    {
                      // ISSUE: reference to a compiler-generated field
                      NetGeometryData netGeometryData = this.m_PrefabNetGeometryData[prefab];
                      // ISSUE: reference to a compiler-generated field
                      Edge edge = this.m_NetEdgeData[topOwner];
                      Game.Net.Elevation elevation1 = new Game.Net.Elevation();
                      Game.Net.Elevation elevation2 = new Game.Net.Elevation();
                      Game.Net.Elevation elevation3 = new Game.Net.Elevation();
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_NetElevationData.HasComponent(topOwner))
                      {
                        // ISSUE: reference to a compiler-generated field
                        elevation1 = this.m_NetElevationData[topOwner];
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_NetElevationData.HasComponent(edge.m_Start))
                      {
                        // ISSUE: reference to a compiler-generated field
                        elevation2 = this.m_NetElevationData[edge.m_Start];
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_NetElevationData.HasComponent(edge.m_End))
                      {
                        // ISSUE: reference to a compiler-generated field
                        elevation3 = this.m_NetElevationData[edge.m_End];
                      }
                      flag1 = math.all(elevation1.m_Elevation >= netGeometryData.m_ElevationLimit * 2f) | math.all(elevation2.m_Elevation >= netGeometryData.m_ElevationLimit * 2f);
                      flag2 = math.all(elevation1.m_Elevation >= netGeometryData.m_ElevationLimit * 2f) | math.all(elevation3.m_Elevation >= netGeometryData.m_ElevationLimit * 2f);
                      if (!flag1 & !flag2)
                        continue;
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_NetElevationData.HasComponent(topOwner))
                      {
                        // ISSUE: reference to a compiler-generated field
                        NetGeometryData netGeometryData = this.m_PrefabNetGeometryData[prefab];
                        // ISSUE: reference to a compiler-generated field
                        if (!math.all(this.m_NetElevationData[topOwner].m_Elevation >= netGeometryData.m_ElevationLimit * 2f))
                          continue;
                      }
                      else
                        continue;
                    }
                  }
                  else
                    continue;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                if (((subObject.m_Flags & SubObjectFlags.RequireOutsideConnection) == (SubObjectFlags) 0 || this.m_OutsideConnectionData.HasComponent(topOwner)) && ((subObject.m_Flags & SubObjectFlags.RequireDeadEnd) == (SubObjectFlags) 0 || this.IsDeadEnd(topOwner)) && ((subObject.m_Flags & SubObjectFlags.RequireOrphan) == (SubObjectFlags) 0 || this.IsOrphan(topOwner)))
                {
                  Transform transform = new Transform(subObject.m_Position, subObject.m_Rotation);
                  int parentMesh = 0;
                  if (isTransform)
                    parentMesh = subObject.m_ParentIndex;
                  else if ((subObject.m_Flags & SubObjectFlags.FixedPlacement) != (SubObjectFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    int2 fixedRange = this.GetFixedRange(owner);
                    if ((subObject.m_Flags & SubObjectFlags.StartPlacement) != (SubObjectFlags) 0)
                    {
                      if (fixedRange.x == subObject.m_ParentIndex && (isEdge || fixedRange.x != fixedRange.y))
                        flag2 = false;
                      else
                        continue;
                    }
                    else if ((subObject.m_Flags & SubObjectFlags.EndPlacement) != (SubObjectFlags) 0)
                    {
                      if (fixedRange.y == subObject.m_ParentIndex && (isEdge || fixedRange.x != fixedRange.y))
                        flag1 = false;
                      else
                        continue;
                    }
                    else if (fixedRange.x != subObject.m_ParentIndex || !isEdge && fixedRange.x != fixedRange.y)
                      continue;
                  }
                  if (isEdge && (subObject.m_Flags & SubObjectFlags.MiddlePlacement) == (SubObjectFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    Edge edge = this.m_NetEdgeData[topOwner];
                    if (flag1)
                    {
                      Transform world = ObjectUtils.LocalToWorld(transform1, transform);
                      // ISSUE: reference to a compiler-generated method
                      if ((subObject.m_Flags & SubObjectFlags.AllowCombine) == (SubObjectFlags) 0 || !this.IsContinuous(edge.m_Start, topOwner))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.CreateSubObject(jobIndex, ref random, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, transform1, world, transform, subObject.m_Flags, ref updateData, subObject.m_Prefab, native, relative, interpolated, isUnderConstruction, isDestroyed, isOverridden, index, parentMesh, subObject.m_GroupIndex, subObject.m_Probability, index, depth);
                      }
                    }
                    if (flag2)
                    {
                      Transform world = ObjectUtils.LocalToWorld(transform3, transform);
                      // ISSUE: reference to a compiler-generated method
                      if ((subObject.m_Flags & SubObjectFlags.AllowCombine) == (SubObjectFlags) 0 || !this.IsContinuous(edge.m_End, topOwner))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.CreateSubObject(jobIndex, ref random, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, transform3, world, transform, subObject.m_Flags, ref updateData, subObject.m_Prefab, native, relative, interpolated, isUnderConstruction, isDestroyed, isOverridden, index, parentMesh, subObject.m_GroupIndex, subObject.m_Probability, index, depth);
                      }
                    }
                  }
                  else
                  {
                    Transform world = ObjectUtils.LocalToWorld(transform2, transform);
                    int alignIndex = math.select(-1, index, isEdge | isNode);
                    // ISSUE: reference to a compiler-generated method
                    this.CreateSubObject(jobIndex, ref random, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, transform2, world, transform, subObject.m_Flags, ref updateData, subObject.m_Prefab, native, relative, interpolated, isUnderConstruction, isDestroyed, isOverridden, alignIndex, parentMesh, subObject.m_GroupIndex, subObject.m_Probability, index, depth);
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (isUnderConstruction && this.m_PrefabBuildingData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
            Transform transform = new Transform()
            {
              m_Rotation = quaternion.identity
            };
            DynamicBuffer<SubMesh> bufferData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSubMeshes.TryGetBuffer(prefab, out bufferData2) && bufferData2.Length != 0)
            {
              SubMesh subMesh = bufferData2[random1.NextInt(bufferData2.Length)];
              transform.m_Position = subMesh.m_Position;
              transform.m_Rotation = subMesh.m_Rotation;
            }
            transform.m_Position.y += math.max(objectGeometryData.m_Bounds.max.y, 15f);
            transform.m_Position.y += math.csum(math.frac(transform2.m_Position.xz / 60f)) * 5f;
            Transform world = ObjectUtils.LocalToWorld(transform2, transform);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CreateSubObject(jobIndex, ref random1, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, transform2, world, transform, (SubObjectFlags) 0, ref updateData, this.m_BuildingConfigurationData.m_ConstructionObject, native, relative, interpolated, false, isDestroyed, isOverridden, -1, 0, 0, 100, -1, depth);
          }
          ObjectGeometryData componentData1;
          DynamicBuffer<SubMesh> bufferData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!isDestroyed || !this.m_PrefabObjectGeometryData.TryGetComponent(prefab, out componentData1) || (componentData1.m_Flags & (GeometryFlags.Physical | GeometryFlags.HasLot)) != (GeometryFlags.Physical | GeometryFlags.HasLot) || !this.m_PrefabSubMeshes.TryGetBuffer(prefab, out bufferData3))
            return;
          int num = 0;
          for (int index = 0; index < bufferData3.Length; ++index)
          {
            SubMesh subMesh = bufferData3[index];
            Game.Prefabs.MeshData componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabMeshData.TryGetComponent(subMesh.m_SubMesh, out componentData2))
            {
              float2 float2_1 = MathUtils.Center(componentData2.m_Bounds.xz);
              float2 x1 = MathUtils.Size(componentData2.m_Bounds.xz);
              int2 int2 = math.max((int2) 1, (int2) math.sqrt(x1));
              float2 float2_2 = x1 / (float2) int2;
              float3 float3_1 = math.rotate(subMesh.m_Rotation, new float3(float2_2.x, 0.0f, 0.0f));
              float3 float3_2 = math.rotate(subMesh.m_Rotation, new float3(0.0f, 0.0f, float2_2.y));
              float3 float3_3 = subMesh.m_Position + math.rotate(subMesh.m_Rotation, new float3(float2_1.x, 0.0f, float2_1.y)) - (float3_1 * (float) ((double) int2.x * 0.5 - 0.5) + float3_2 * (float) ((double) int2.y * 0.5 - 0.5));
              for (int y = 0; y < int2.y; ++y)
              {
                for (int x2 = 0; x2 < int2.x; ++x2)
                {
                  float2 float2_3 = new float2((float) x2, (float) y) + random.NextFloat2((float2) -0.5f, (float2) 0.5f);
                  Transform transform = new Transform()
                  {
                    m_Position = float3_3 + float3_1 * float2_3.x + float3_2 * float2_3.y,
                    m_Rotation = quaternion.RotateY(random1.NextFloat(-3.14159274f, 3.14159274f))
                  };
                  Transform world = ObjectUtils.LocalToWorld(transform2, transform);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.CreateSubObject(jobIndex, ref random1, topOwner, owner, prefab, isTemp, ownerTemp, ownerElevation, transform2, world, transform, SubObjectFlags.OnGround, ref updateData, this.m_BuildingConfigurationData.m_CollapsedObject, native, relative, interpolated, false, false, isOverridden, -1, 0, num++, 100, -1, depth);
                }
              }
            }
          }
        }
      }

      private int2 GetFixedRange(Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Edges.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef1 = this.m_PrefabRefData[owner];
          int2 fixedRange = new int2(int.MaxValue, int.MinValue);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.Net.EdgeIterator edgeIterator = new Game.Net.EdgeIterator(Entity.Null, owner, this.m_Edges, this.m_NetEdgeData, this.m_TempData, this.m_HiddenData);
          EdgeIteratorValue edgeIteratorValue;
          while (edgeIterator.GetNext(out edgeIteratorValue))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef2 = this.m_PrefabRefData[edgeIteratorValue.m_Edge];
            // ISSUE: reference to a compiler-generated field
            if (prefabRef1.m_Prefab == prefabRef2.m_Prefab && this.m_FixedData.HasComponent(edgeIteratorValue.m_Edge))
            {
              // ISSUE: reference to a compiler-generated field
              Fixed @fixed = this.m_FixedData[edgeIteratorValue.m_Edge];
              if (edgeIteratorValue.m_End)
                fixedRange.y = math.max(fixedRange.y, @fixed.m_Index);
              else
                fixedRange.x = math.min(fixedRange.x, @fixed.m_Index);
            }
          }
          return fixedRange;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_FixedData.HasComponent(owner))
          return new int2(int.MaxValue, int.MinValue);
        // ISSUE: reference to a compiler-generated field
        Fixed fixed1 = this.m_FixedData[owner];
        return new int2(fixed1.m_Index, fixed1.m_Index);
      }

      private bool IsDeadEnd(Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[owner];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Edges.HasBuffer(owner) || !this.m_PrefabNetGeometryData.HasComponent(prefabRef.m_Prefab))
          return false;
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData = this.m_PrefabNetGeometryData[prefabRef.m_Prefab];
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Net.EdgeIterator edgeIterator = new Game.Net.EdgeIterator(Entity.Null, owner, this.m_Edges, this.m_NetEdgeData, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_PrefabNetGeometryData[this.m_PrefabRefData[edgeIteratorValue.m_Edge].m_Prefab].m_MergeLayers & netGeometryData.m_MergeLayers) != Layer.None)
            ++num;
        }
        return num <= 1;
      }

      private bool IsOrphan(Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[owner];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Edges.HasBuffer(owner) && this.m_PrefabNetGeometryData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_PrefabNetGeometryData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.Net.EdgeIterator edgeIterator = new Game.Net.EdgeIterator(Entity.Null, owner, this.m_Edges, this.m_NetEdgeData, this.m_TempData, this.m_HiddenData);
          EdgeIteratorValue edgeIteratorValue;
          while (edgeIterator.GetNext(out edgeIteratorValue))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabNetGeometryData[this.m_PrefabRefData[edgeIteratorValue.m_Edge].m_Prefab].m_MergeLayers & netGeometryData.m_MergeLayers) != Layer.None)
              return false;
          }
        }
        return true;
      }

      private bool IsContinuous(Entity node, Entity edge)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[node];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Edges.HasBuffer(node) || !this.m_PrefabNetGeometryData.HasComponent(prefabRef1.m_Prefab))
          return false;
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData1 = this.m_PrefabNetGeometryData[prefabRef1.m_Prefab];
        int num = 0;
        Curve curve1 = new Curve();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Net.EdgeIterator edgeIterator = new Game.Net.EdgeIterator(edge, node, this.m_Edges, this.m_NetEdgeData, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef2 = this.m_PrefabRefData[edgeIteratorValue.m_Edge];
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData2 = this.m_PrefabNetGeometryData[prefabRef2.m_Prefab];
          if ((netGeometryData2.m_MergeLayers & netGeometryData1.m_MergeLayers) != Layer.None)
          {
            if (prefabRef2.m_Prefab != prefabRef1.m_Prefab)
              return false;
            if (++num == 1)
            {
              // ISSUE: reference to a compiler-generated field
              curve1 = this.m_NetCurveData[edgeIteratorValue.m_Edge];
              if (edgeIteratorValue.m_End)
                curve1.m_Bezier = MathUtils.Invert(curve1.m_Bezier);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve2 = this.m_NetCurveData[edgeIteratorValue.m_Edge];
              if (edgeIteratorValue.m_End)
                curve2.m_Bezier = MathUtils.Invert(curve2.m_Bezier);
              float3 x1 = MathUtils.StartTangent(curve1.m_Bezier);
              float3 y1 = MathUtils.StartTangent(curve2.m_Bezier);
              if (MathUtils.TryNormalize(ref x1) && MathUtils.TryNormalize(ref y1))
              {
                if ((double) math.dot(x1, y1) > -0.99000000953674316)
                  return false;
                float3 y2 = (x1 - y1) * 0.5f;
                float3 x2 = curve2.m_Bezier.a - curve1.m_Bezier.a;
                if ((double) math.lengthsq(x2 - y2 * math.dot(x2, y2)) > 0.0099999997764825821)
                  return false;
              }
            }
          }
          else if ((netGeometryData2.m_IntersectLayers & netGeometryData1.m_IntersectLayers) != Layer.None)
            return false;
        }
        return num == 2;
      }

      private bool CheckRequirements(
        Entity prefab,
        int groupIndex,
        bool isExplicit,
        ref SubObjectSystem.UpdateSubObjectsData updateData)
      {
        DynamicBuffer<ObjectRequirementElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectRequirements.TryGetBuffer(prefab, out bufferData))
        {
          int num1 = -1;
          bool flag = true;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            ObjectRequirementElement requirementElement = bufferData[index];
            if ((requirementElement.m_Type & ObjectRequirementType.SelectOnly) == (ObjectRequirementType) 0)
            {
              if ((int) requirementElement.m_Group != num1)
              {
                if (flag)
                {
                  num1 = (int) requirementElement.m_Group;
                  flag = false;
                }
                else
                  break;
              }
              if (requirementElement.m_Requirement != Entity.Null)
              {
                int2 int2;
                // ISSUE: reference to a compiler-generated field
                if (updateData.m_PlaceholderRequirements.TryGetValue(requirementElement.m_Requirement, out int2))
                {
                  if (int2.y == 0)
                  {
                    flag = true;
                  }
                  else
                  {
                    int a = groupIndex % int2.y;
                    int num2 = math.select(a, -int2.y, a == 0 && groupIndex < 0);
                    flag |= num2 == int2.x;
                  }
                }
                else if (isExplicit && (requirementElement.m_Type & ObjectRequirementType.IgnoreExplicit) != (ObjectRequirementType) 0)
                  flag = true;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                flag |= (updateData.m_PlaceholderRequirementFlags & (requirementElement.m_RequireFlags | requirementElement.m_ForbidFlags)) == requirementElement.m_RequireFlags;
              }
            }
          }
          if (!flag)
            return false;
        }
        return true;
      }

      private void CreateSubObject(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity topOwner,
        Entity owner,
        Entity ownerPrefab,
        bool isTemp,
        Temp ownerTemp,
        float ownerElevation,
        Transform ownerTransform,
        Transform transformData,
        Transform localTransformData,
        SubObjectFlags flags,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        Entity prefab,
        bool native,
        bool relative,
        bool interpolated,
        bool underConstruction,
        bool destroyed,
        bool overridden,
        int alignIndex,
        int parentMesh,
        int groupIndex,
        int probability,
        int prefabSubIndex,
        int depth)
      {
        PlaceholderObjectData componentData1;
        DynamicBuffer<PlaceholderObjectElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabPlaceholderObjectData.TryGetComponent(prefab, out componentData1) || !this.m_PlaceholderObjects.TryGetBuffer(prefab, out bufferData))
        {
          Entity groupPrefab = prefab;
          SpawnableObjectData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSpawnableObjectData.TryGetComponent(prefab, out componentData2) && componentData2.m_RandomizationGroup != Entity.Null)
            groupPrefab = componentData2.m_RandomizationGroup;
          // ISSUE: reference to a compiler-generated method
          if (!this.CheckRequirements(prefab, groupIndex, true, ref updateData))
            return;
          Unity.Mathematics.Random random1 = random;
          random.NextInt();
          random.NextInt();
          Unity.Mathematics.Random random2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          if (updateData.m_SelectedSpawnabled.IsCreated && updateData.m_SelectedSpawnabled.TryGetValue(new SubObjectSystem.PlaceholderKey(groupPrefab, groupIndex), out random2))
          {
            random1 = random2;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            updateData.EnsureSelectedSpawnables(Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            updateData.m_SelectedSpawnabled.TryAdd(new SubObjectSystem.PlaceholderKey(groupPrefab, groupIndex), random1);
          }
          if (random1.NextInt(100) >= probability)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CreateSubObject(jobIndex, ref random1, topOwner, owner, ownerPrefab, isTemp, ownerTemp, ownerElevation, Entity.Null, ownerTransform, transformData, localTransformData, flags, ref updateData, prefab, false, native, relative, interpolated, underConstruction, destroyed, overridden, false, alignIndex, parentMesh, groupIndex, probability, prefabSubIndex, depth);
        }
        else
        {
          float num1 = 0.0f;
          bool updated1 = false;
          bool updated2 = false;
          float num2 = -1f;
          float num3 = -1f;
          float num4 = -1f;
          Entity prefab1 = Entity.Null;
          Entity prefab2 = Entity.Null;
          Entity prefab3 = Entity.Null;
          Entity groupPrefab1 = Entity.Null;
          Entity groupPrefab2 = Entity.Null;
          Entity groupPrefab3 = Entity.Null;
          SubObjectFlags flags1 = (SubObjectFlags) 0;
          SubObjectFlags flags2 = (SubObjectFlags) 0;
          SubObjectFlags flags3 = (SubObjectFlags) 0;
          Unity.Mathematics.Random random3 = new Unity.Mathematics.Random();
          Unity.Mathematics.Random random4 = new Unity.Mathematics.Random();
          Unity.Mathematics.Random random5 = new Unity.Mathematics.Random();
          int max1 = 0;
          int max2 = 0;
          int max3 = 0;
          if (componentData1.m_RandomizeGroupIndex)
          {
            int a = random.NextInt() & int.MaxValue;
            groupIndex = math.select(a, -1 - a, groupIndex < 0);
          }
          for (int index1 = 0; index1 < bufferData.Length; ++index1)
          {
            Entity entity = bufferData[index1].m_Object;
            // ISSUE: reference to a compiler-generated method
            if (this.CheckRequirements(entity, groupIndex, false, ref updateData))
            {
              SubObjectFlags subObjectFlags = flags;
              float num5 = 0.0f;
              float num6 = 0.0f;
              int num7 = 0;
              bool flag = false;
              PillarData componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabPillarData.TryGetComponent(entity, out componentData3))
              {
                switch (componentData3.m_Type)
                {
                  case PillarType.Vertical:
                    subObjectFlags = subObjectFlags | SubObjectFlags.AnchorTop | SubObjectFlags.OnGround;
                    flag = true;
                    NetGeometryData componentData4;
                    ObjectGeometryData componentData5;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabNetGeometryData.TryGetComponent(ownerPrefab, out componentData4) && this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData5))
                    {
                      PlaceableObjectData componentData6;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PrefabPlaceableObjectData.TryGetComponent(entity, out componentData6))
                        componentData5.m_Size.y -= componentData6.m_PlacementOffset.y;
                      float num8 = ownerElevation + transformData.m_Position.y - ownerTransform.m_Position.y;
                      float num9 = componentData5.m_Size.y - num8;
                      num6 += (float) (1.0 / (1.0 + (double) math.select(2f * num9, -num9, (double) num9 < 0.0)));
                      num5 = math.max(0.0f, (float) ((double) componentData4.m_ElevatedWidth * 0.5 - (double) componentData5.m_Size.x * 0.5));
                      break;
                    }
                    break;
                  case PillarType.Horizontal:
                    num7 = 1;
                    flag = true;
                    NetGeometryData componentData7;
                    ObjectGeometryData componentData8;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabNetGeometryData.TryGetComponent(ownerPrefab, out componentData7) && this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData8))
                    {
                      float num10 = componentData7.m_ElevatedWidth - 1f;
                      float max4 = componentData3.m_OffsetRange.max;
                      num6 = num6 + (float) (1.0 / (1.0 + (double) math.abs(componentData8.m_Size.x - num10))) + (float) (0.0099999997764825821 / (1.0 + (double) math.max(0.0f, max4)));
                      break;
                    }
                    break;
                  case PillarType.Standalone:
                    subObjectFlags = subObjectFlags | SubObjectFlags.AnchorTop | SubObjectFlags.OnGround;
                    ObjectGeometryData componentData9;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData9))
                    {
                      PlaceableObjectData componentData10;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PrefabPlaceableObjectData.TryGetComponent(entity, out componentData10))
                        componentData9.m_Size.y -= componentData10.m_PlacementOffset.y;
                      float num11 = ownerElevation + transformData.m_Position.y - ownerTransform.m_Position.y;
                      float num12 = componentData9.m_Size.y - num11;
                      num6 += (float) (1.0 / (1.0 + (double) math.select(2f * num12, -num12, (double) num12 < 0.0)));
                      break;
                    }
                    break;
                  case PillarType.Base:
                    num7 = 2;
                    subObjectFlags |= SubObjectFlags.OnGround;
                    ObjectGeometryData componentData11;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData11))
                    {
                      PlaceableObjectData componentData12;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PrefabPlaceableObjectData.TryGetComponent(entity, out componentData12))
                        componentData11.m_Size.y -= componentData12.m_PlacementOffset.y;
                      float num13 = ownerElevation + transformData.m_Position.y - ownerTransform.m_Position.y;
                      float num14 = componentData11.m_Size.y - num13;
                      num6 += (float) (1.0 / (1.0 + (double) math.select(2f * num14, -num14, (double) num14 < 0.0)));
                      break;
                    }
                    break;
                }
              }
              QuantityObjectData componentData13;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabQuantityObjectData.TryGetComponent(entity, out componentData13))
              {
                // ISSUE: reference to a compiler-generated field
                if ((componentData13.m_Resources & updateData.m_StoredResources) != Resource.NoResource)
                {
                  ++num6;
                  componentData13.m_Resources = Resource.NoResource;
                }
                Game.Vehicles.DeliveryTruck componentData14;
                // ISSUE: reference to a compiler-generated field
                if (componentData13.m_Resources != Resource.NoResource && this.m_DeliveryTruckData.TryGetComponent(topOwner, out componentData14) && (componentData13.m_Resources & componentData14.m_Resource) != Resource.NoResource)
                {
                  ++num6;
                  componentData13.m_Resources = Resource.NoResource;
                }
                // ISSUE: reference to a compiler-generated field
                if ((componentData13.m_Resources & Resource.LocalMail) != Resource.NoResource && this.m_MailProducerData.HasComponent(topOwner))
                {
                  num6 += 0.9f;
                  componentData13.m_Resources = Resource.NoResource;
                }
                // ISSUE: reference to a compiler-generated field
                if ((componentData13.m_Resources & Resource.Garbage) != Resource.NoResource && this.m_GarbageProducerData.HasComponent(topOwner))
                {
                  num6 += 0.9f;
                  componentData13.m_Resources = Resource.NoResource;
                }
                // ISSUE: reference to a compiler-generated field
                if (componentData13.m_Resources != Resource.NoResource && this.m_Resources.HasBuffer(topOwner))
                {
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[topOwner];
                  Resource resources = componentData13.m_Resources;
                  CargoTransportVehicleData componentData15;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabCargoTransportVehicleData.TryGetComponent(prefabRef.m_Prefab, out componentData15))
                    resources &= componentData15.m_Resources;
                  if (resources != Resource.NoResource)
                  {
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<Resources> resource = this.m_Resources[topOwner];
                    for (int index2 = 0; index2 < resource.Length; ++index2)
                    {
                      if ((resource[index2].m_Resource & resources) != Resource.NoResource)
                      {
                        ++num6;
                        componentData13.m_Resources = Resource.NoResource;
                        break;
                      }
                    }
                  }
                }
                WorkVehicleData componentData16;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (componentData13.m_MapFeature != MapFeature.None && this.m_PrefabWorkVehicleData.TryGetComponent(this.m_PrefabRefData[topOwner].m_Prefab, out componentData16) && componentData13.m_MapFeature == componentData16.m_MapFeature)
                {
                  ++num6;
                  componentData13.m_MapFeature = MapFeature.None;
                }
                WorkVehicleData componentData17;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (componentData13.m_Resources != Resource.NoResource && this.m_PrefabWorkVehicleData.TryGetComponent(this.m_PrefabRefData[topOwner].m_Prefab, out componentData17) && (componentData13.m_Resources & componentData17.m_Resources) != Resource.NoResource)
                {
                  ++num6;
                  componentData13.m_Resources = Resource.NoResource;
                }
                if (componentData13.m_Resources != Resource.NoResource || componentData13.m_MapFeature != MapFeature.None)
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              SpawnableObjectData spawnableObjectData = this.m_PrefabSpawnableObjectData[entity];
              Entity groupPrefab4 = spawnableObjectData.m_RandomizationGroup != Entity.Null ? spawnableObjectData.m_RandomizationGroup : entity;
              Unity.Mathematics.Random random6 = random;
              random.NextInt();
              random.NextInt();
              Unity.Mathematics.Random random7;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              if (updateData.m_SelectedSpawnabled.IsCreated && updateData.m_SelectedSpawnabled.TryGetValue(new SubObjectSystem.PlaceholderKey(groupPrefab4, groupIndex), out random7))
              {
                num6 += 0.5f;
                random6 = random7;
              }
              switch (num7)
              {
                case 0:
                  if ((double) num6 > (double) num2)
                  {
                    num1 = num5;
                    updated1 = flag;
                    num2 = num6;
                    prefab1 = entity;
                    groupPrefab1 = groupPrefab4;
                    flags1 = subObjectFlags;
                    random3 = random6;
                    max1 = spawnableObjectData.m_Probability;
                    continue;
                  }
                  if ((double) num6 == (double) num2)
                  {
                    int probability1 = spawnableObjectData.m_Probability;
                    max1 += probability1;
                    if (random.NextInt(max1) < probability1)
                    {
                      num1 = num5;
                      updated1 = flag;
                      prefab1 = entity;
                      groupPrefab1 = groupPrefab4;
                      flags1 = subObjectFlags;
                      random3 = random6;
                      continue;
                    }
                    continue;
                  }
                  continue;
                case 1:
                  if ((double) num6 > (double) num3)
                  {
                    updated2 = flag;
                    num3 = num6;
                    prefab2 = entity;
                    groupPrefab2 = groupPrefab4;
                    flags2 = subObjectFlags;
                    random4 = random6;
                    max2 = spawnableObjectData.m_Probability;
                    continue;
                  }
                  if ((double) num6 == (double) num3)
                  {
                    int probability2 = spawnableObjectData.m_Probability;
                    max2 += probability2;
                    if (random.NextInt(max2) < probability2)
                    {
                      updated2 = flag;
                      prefab2 = entity;
                      groupPrefab2 = groupPrefab4;
                      flags2 = subObjectFlags;
                      random4 = random6;
                      continue;
                    }
                    continue;
                  }
                  continue;
                case 2:
                  if ((double) num6 > (double) num4)
                  {
                    num4 = num6;
                    prefab3 = entity;
                    groupPrefab3 = groupPrefab4;
                    flags3 = subObjectFlags;
                    random5 = random6;
                    max3 = spawnableObjectData.m_Probability;
                    continue;
                  }
                  if ((double) num6 == (double) num4)
                  {
                    int probability3 = spawnableObjectData.m_Probability;
                    max3 += probability3;
                    if (random.NextInt(max3) < probability3)
                    {
                      prefab3 = entity;
                      groupPrefab3 = groupPrefab4;
                      flags3 = subObjectFlags;
                      random5 = random6;
                      continue;
                    }
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
          if (max1 > 0)
          {
            // ISSUE: reference to a compiler-generated method
            updateData.EnsureSelectedSpawnables(Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            updateData.m_SelectedSpawnabled.TryAdd(new SubObjectSystem.PlaceholderKey(groupPrefab1, groupIndex), random3);
            if (random3.NextInt(100) < probability)
            {
              if ((double) num1 != 0.0)
              {
                Transform transform1 = localTransformData;
                Transform transform2 = localTransformData;
                transform1.m_Position.x -= num1;
                transform2.m_Position.x += num1;
                Transform world1 = ObjectUtils.LocalToWorld(ownerTransform, transform1);
                Transform world2 = ObjectUtils.LocalToWorld(ownerTransform, transform2);
                Unity.Mathematics.Random random8 = random3;
                // ISSUE: reference to a compiler-generated method
                this.CreateSubObject(jobIndex, ref random3, topOwner, owner, ownerPrefab, isTemp, ownerTemp, ownerElevation, Entity.Null, ownerTransform, world1, transform1, flags1, ref updateData, prefab1, false, native, relative, interpolated, underConstruction, destroyed, overridden, updated1, alignIndex, parentMesh, groupIndex, probability, prefabSubIndex, depth);
                // ISSUE: reference to a compiler-generated method
                this.CreateSubObject(jobIndex, ref random8, topOwner, owner, ownerPrefab, isTemp, ownerTemp, ownerElevation, Entity.Null, ownerTransform, world2, transform2, flags1, ref updateData, prefab1, false, native, relative, interpolated, underConstruction, destroyed, overridden, updated1, alignIndex, parentMesh, groupIndex, probability, prefabSubIndex, depth);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.CreateSubObject(jobIndex, ref random3, topOwner, owner, ownerPrefab, isTemp, ownerTemp, ownerElevation, Entity.Null, ownerTransform, transformData, localTransformData, flags1, ref updateData, prefab1, false, native, relative, interpolated, underConstruction, destroyed, overridden, updated1, alignIndex, parentMesh, groupIndex, probability, prefabSubIndex, depth);
              }
            }
          }
          if (max2 > 0)
          {
            // ISSUE: reference to a compiler-generated method
            updateData.EnsureSelectedSpawnables(Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            updateData.m_SelectedSpawnabled.TryAdd(new SubObjectSystem.PlaceholderKey(groupPrefab2, groupIndex), random4);
            if (random4.NextInt(100) < probability)
            {
              // ISSUE: reference to a compiler-generated method
              this.CreateSubObject(jobIndex, ref random4, topOwner, owner, ownerPrefab, isTemp, ownerTemp, ownerElevation, Entity.Null, ownerTransform, transformData, localTransformData, flags2, ref updateData, prefab2, false, native, relative, interpolated, underConstruction, destroyed, overridden, updated2, alignIndex, parentMesh, groupIndex, probability, prefabSubIndex, depth);
            }
          }
          if (max3 <= 0)
            return;
          // ISSUE: reference to a compiler-generated method
          updateData.EnsureSelectedSpawnables(Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          updateData.m_SelectedSpawnabled.TryAdd(new SubObjectSystem.PlaceholderKey(groupPrefab3, groupIndex), random5);
          if (random5.NextInt(100) >= probability)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CreateSubObject(jobIndex, ref random5, topOwner, owner, ownerPrefab, isTemp, ownerTemp, ownerElevation, Entity.Null, ownerTransform, transformData, localTransformData, flags3, ref updateData, prefab3, false, native, relative, interpolated, underConstruction, destroyed, overridden, false, alignIndex, parentMesh, groupIndex, probability, prefabSubIndex, depth);
        }
      }

      private void CreateSubObject(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity topOwner,
        Entity owner,
        Entity ownerPrefab,
        bool isTemp,
        Temp ownerTemp,
        float ownerElevation,
        Entity oldSubObject,
        Transform ownerTransform,
        Transform transformData,
        Transform localTransformData,
        SubObjectFlags flags,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        Entity prefab,
        bool cacheTransform,
        bool native,
        bool relative,
        bool interpolated,
        bool underConstruction,
        bool isDestroyed,
        bool isOverridden,
        bool updated,
        int alignIndex,
        int parentMesh,
        int groupIndex,
        int probability,
        int prefabSubIndex,
        int depth)
      {
        ObjectGeometryData componentData1;
        // ISSUE: reference to a compiler-generated field
        bool component1 = this.m_PrefabObjectGeometryData.TryGetComponent(prefab, out componentData1);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = this.m_PrefabData.IsComponentEnabled(prefab);
        if ((flags & SubObjectFlags.AnchorTop) != (SubObjectFlags) 0)
        {
          PlaceableObjectData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabPlaceableObjectData.TryGetComponent(prefab, out componentData2))
            componentData1.m_Bounds.max.y -= componentData2.m_PlacementOffset.y;
          transformData.m_Position.y -= componentData1.m_Bounds.max.y;
          localTransformData.m_Position.y -= componentData1.m_Bounds.max.y;
        }
        else if ((flags & SubObjectFlags.AnchorCenter) != (SubObjectFlags) 0)
        {
          float num = (float) (((double) componentData1.m_Bounds.max.y - (double) componentData1.m_Bounds.min.y) * 0.5);
          transformData.m_Position.y -= num;
          localTransformData.m_Position.y -= num;
        }
        PseudoRandomSeed pseudoRandomSeed = new PseudoRandomSeed();
        if (component1)
          pseudoRandomSeed = new PseudoRandomSeed(ref random);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (underConstruction & component1 && (componentData1.m_Flags & GeometryFlags.Marker) == GeometryFlags.None && !this.m_PrefabBuildingExtensionData.HasComponent(prefab) && !this.m_PrefabSubLanes.HasBuffer(prefab) && !this.m_PrefabSpawnLocationData.HasComponent(prefab))
          return;
        Elevation elevation = new Elevation(ownerElevation, math.abs(parentMesh) >= 1000 ? ElevationFlags.Stacked : (ElevationFlags) 0);
        if ((flags & SubObjectFlags.OnGround) == (SubObjectFlags) 0)
        {
          elevation.m_Elevation += localTransformData.m_Position.y;
          if ((double) ownerElevation >= 0.0 && (double) elevation.m_Elevation >= -0.5 && (double) elevation.m_Elevation < 0.0)
            elevation.m_Elevation = 0.0f;
          if (parentMesh < 0)
            elevation.m_Flags |= ElevationFlags.OnGround;
        }
        else
        {
          if ((flags & (SubObjectFlags.AnchorTop | SubObjectFlags.AnchorCenter)) == (SubObjectFlags) 0)
          {
            transformData.m_Position.y = ownerTransform.m_Position.y - ownerElevation;
            localTransformData.m_Position.y = -ownerElevation;
          }
          elevation.m_Elevation = 0.0f;
          elevation.m_Flags |= ElevationFlags.OnGround;
        }
        if ((elevation.m_Flags & ElevationFlags.OnGround) != (ElevationFlags) 0)
        {
          bool flag2 = true;
          if (component1)
            flag2 = (componentData1.m_Flags & GeometryFlags.DeleteOverridden) == GeometryFlags.None && (componentData1.m_Flags & (GeometryFlags.Overridable | GeometryFlags.Marker)) != 0;
          if (flag2)
          {
            bool angledSample;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Transform transform = ObjectUtils.AdjustPosition(transformData, elevation, prefab, out angledSample, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData, ref this.m_PrefabPlaceableObjectData, ref this.m_PrefabObjectGeometryData);
            if ((double) math.abs(transform.m_Position.y - transformData.m_Position.y) >= 0.0099999997764825821 || angledSample && (double) MathUtils.RotationAngle(transform.m_Rotation, transformData.m_Rotation) >= (double) math.radians(0.1f))
              transformData = transform;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (isDestroyed && (elevation.m_Flags & (ElevationFlags.Stacked | ElevationFlags.OnGround)) != ElevationFlags.OnGround && !this.m_PrefabBuildingExtensionData.HasComponent(prefab) || ClearAreaHelpers.ShouldClear(updateData.m_ClearAreas, transformData.m_Position, (flags & SubObjectFlags.OnGround) != 0))
          return;
        if (oldSubObject == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          oldSubObject = this.FindOldSubObject(prefab, transformData, ref updateData);
        }
        else if (oldSubObject.Index < 0)
          oldSubObject = Entity.Null;
        int3 boneIndex = new int3(0, -1, -1);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EditorMode)
        {
          int num = parentMesh % 1000;
          if (num > 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            boneIndex.yz = RenderingUtils.FindBoneIndex(ownerPrefab, ref localTransformData.m_Position, ref localTransformData.m_Rotation, num, ref this.m_PrefabSubMeshes, ref this.m_PrefabProceduralBones);
            boneIndex.x = math.select(0, num, boneIndex.y >= 0);
          }
        }
        if (oldSubObject != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, oldSubObject);
          Temp component2 = new Temp();
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              component2 = this.m_TempData[oldSubObject] with
              {
                m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
              };
              if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
                component2.m_Flags |= TempFlags.Modify;
              // ISSUE: reference to a compiler-generated method
              component2.m_Original = this.FindOriginalSubObject(prefab, component2.m_Original, transformData, ref updateData);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Temp>(jobIndex, oldSubObject, component2);
              Tree componentData3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (component2.m_Original != Entity.Null && flag1 && this.m_PrefabTreeData.HasComponent(prefab) && this.m_TreeData.TryGetComponent(component2.m_Original, out componentData3))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Tree>(jobIndex, oldSubObject, componentData3);
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.HasComponent(prefab))
              interpolated = true;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabNetObjectData.HasComponent(prefab))
            {
              Attached componentData4;
              // ISSUE: reference to a compiler-generated field
              this.m_AttachedData.TryGetComponent(oldSubObject, out componentData4);
              componentData4.m_OldParent = componentData4.m_Parent;
              componentData4.m_Parent = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Attached>(jobIndex, oldSubObject, componentData4);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Transform>(jobIndex, oldSubObject, transformData);
            updated = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!transformData.Equals(this.m_TransformData[oldSubObject]))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Transform>(jobIndex, oldSubObject, transformData);
              updated = true;
            }
          }
          if (cacheTransform)
          {
            LocalTransformCache component3;
            component3.m_Position = localTransformData.m_Position;
            component3.m_Rotation = localTransformData.m_Rotation;
            component3.m_ParentMesh = parentMesh;
            component3.m_GroupIndex = groupIndex;
            component3.m_Probability = probability;
            component3.m_PrefabSubIndex = prefabSubIndex;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<LocalTransformCache>(jobIndex, oldSubObject, component3);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LocalTransformCache>(jobIndex, oldSubObject, component3);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<LocalTransformCache>(jobIndex, oldSubObject);
            }
          }
          PseudoRandomSeed componentData5 = new PseudoRandomSeed();
          if (component1)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PseudoRandomSeedData.TryGetComponent(component2.m_Original, out componentData5))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldSubObject, componentData5);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PseudoRandomSeedData.TryGetComponent(oldSubObject, out componentData5))
              {
                componentData5 = pseudoRandomSeed;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PseudoRandomSeed>(jobIndex, oldSubObject, componentData5);
              }
            }
          }
          if ((flags & SubObjectFlags.OnGround) == (SubObjectFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElevationData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Elevation>(jobIndex, oldSubObject, elevation);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, oldSubObject, elevation);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElevationData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Elevation>(jobIndex, oldSubObject);
            }
          }
          if (alignIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_AlignedData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Aligned>(jobIndex, oldSubObject, new Aligned((ushort) alignIndex));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Aligned>(jobIndex, oldSubObject, new Aligned((ushort) alignIndex));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_AlignedData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Aligned>(jobIndex, oldSubObject);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_RelativeData.HasComponent(oldSubObject))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Relative>(jobIndex, oldSubObject, new Relative(localTransformData, boneIndex));
          }
          if (interpolated || boneIndex.y >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_InterpolatedTransformData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<InterpolatedTransform>(jobIndex, oldSubObject, new InterpolatedTransform(transformData));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<InterpolatedTransform>(jobIndex, oldSubObject, new InterpolatedTransform(transformData));
              updated = true;
            }
          }
          else
          {
            Destroyed componentData6;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_InterpolatedTransformData.HasComponent(oldSubObject) && (!this.m_PrefabBuildingExtensionData.HasComponent(prefab) || !this.m_DestroyedData.TryGetComponent(oldSubObject, out componentData6) || (double) componentData6.m_Cleared >= 0.0))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(jobIndex, oldSubObject);
              updated = true;
            }
          }
          UnderConstruction componentData7 = new UnderConstruction();
          if (component2.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            underConstruction = this.m_UnderConstructionData.TryGetComponent(component2.m_Original, out componentData7);
          }
          if (underConstruction)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_UnderConstructionData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<UnderConstruction>(jobIndex, oldSubObject, componentData7);
              updated = true;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_UnderConstructionData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<UnderConstruction>(jobIndex, oldSubObject);
              updated = true;
            }
          }
          Destroyed componentData8 = new Destroyed();
          if (component2.m_Original != Entity.Null && (ownerTemp.m_Flags & TempFlags.Upgrade) == (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            isDestroyed = this.m_DestroyedData.TryGetComponent(component2.m_Original, out componentData8);
          }
          if (isDestroyed)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DestroyedData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Destroyed>(jobIndex, oldSubObject, componentData8);
              updated = true;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_DestroyedData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Destroyed>(jobIndex, oldSubObject);
              updated = true;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_OverriddenData.HasComponent(oldSubObject))
            isOverridden = true;
          else if (isOverridden)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Overridden>(jobIndex, oldSubObject, new Overridden());
            updated = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (updated && !this.m_UpdatedData.HasComponent(oldSubObject))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldSubObject, new Updated());
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabStreetLightData.HasComponent(prefab))
          {
            StreetLight streetLight = new StreetLight();
            bool flag3 = false;
            StreetLight componentData9;
            // ISSUE: reference to a compiler-generated field
            if (this.m_StreetLightData.TryGetComponent(oldSubObject, out componentData9))
            {
              streetLight = componentData9;
              flag3 = true;
            }
            Building componentData10;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingData.TryGetComponent(topOwner, out componentData10))
            {
              // ISSUE: reference to a compiler-generated method
              StreetLightSystem.UpdateStreetLightState(ref streetLight, componentData10);
            }
            else
            {
              Watercraft componentData11;
              // ISSUE: reference to a compiler-generated field
              if (this.m_WatercraftData.TryGetComponent(topOwner, out componentData11))
              {
                // ISSUE: reference to a compiler-generated method
                StreetLightSystem.UpdateStreetLightState(ref streetLight, componentData11);
              }
            }
            if (flag3)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<StreetLight>(jobIndex, oldSubObject, streetLight);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<StreetLight>(jobIndex, oldSubObject, streetLight);
            }
          }
          StackData componentData12;
          // ISSUE: reference to a compiler-generated field
          if (flag1 && this.m_PrefabStackData.TryGetComponent(prefab, out componentData12))
          {
            Stack componentData13;
            // ISSUE: reference to a compiler-generated field
            if (this.m_StackData.TryGetComponent(component2.m_Original, out componentData13))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Stack>(jobIndex, oldSubObject, componentData13);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (updated || !this.m_StackData.HasComponent(oldSubObject))
              {
                if (componentData12.m_Direction == StackDirection.Up)
                {
                  componentData13.m_Range.min = componentData12.m_FirstBounds.min - elevation.m_Elevation;
                  componentData13.m_Range.max = componentData12.m_LastBounds.max;
                }
                else
                {
                  componentData13.m_Range.min = componentData12.m_FirstBounds.min;
                  componentData13.m_Range.max = componentData12.m_FirstBounds.max + MathUtils.Size(componentData12.m_MiddleBounds) * 2f + MathUtils.Size(componentData12.m_LastBounds);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Stack>(jobIndex, oldSubObject, componentData13);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SubObjects.HasBuffer(oldSubObject))
            return;
          if (depth < 7)
          {
            // ISSUE: reference to a compiler-generated method
            updateData.EnsureDeepOwners(Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            updateData.m_DeepOwners.Add(new SubObjectSystem.DeepSubObjectOwnerData()
            {
              m_Transform = transformData,
              m_Temp = component2,
              m_Entity = oldSubObject,
              m_Prefab = prefab,
              m_Elevation = elevation.m_Elevation,
              m_RandomSeed = componentData5,
              m_HasRandomSeed = component1,
              m_UnderConstruction = underConstruction,
              m_Destroyed = isDestroyed,
              m_Overridden = isOverridden,
              m_Depth = depth + 1
            });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LoopErrorPrefabs.Enqueue(prefab);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          ObjectData objectData = this.m_PrefabObjectData[prefab];
          if (!objectData.m_Archetype.Valid)
            return;
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, objectData.m_Archetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity, new Owner(owner));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(prefab));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(jobIndex, entity, transformData);
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabNetObjectData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Attached>(jobIndex, entity, new Attached());
          }
          Temp component4 = new Temp();
          if (isTemp)
          {
            component4.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate);
            if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
              component4.m_Flags |= TempFlags.Modify;
            // ISSUE: reference to a compiler-generated method
            component4.m_Original = this.FindOriginalSubObject(prefab, Entity.Null, transformData, ref updateData);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity, component4);
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Animation>(jobIndex, entity, new Animation());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<InterpolatedTransform>(jobIndex, entity, new InterpolatedTransform());
            }
            if (component4.m_Original != Entity.Null)
            {
              Tree componentData14;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (flag1 && this.m_PrefabTreeData.HasComponent(prefab) && this.m_TreeData.TryGetComponent(component4.m_Original, out componentData14))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Tree>(jobIndex, entity, componentData14);
              }
              // ISSUE: reference to a compiler-generated field
              if ((component4.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) != (TempFlags) 0 && this.m_OverriddenData.HasComponent(component4.m_Original))
                isOverridden = true;
              // ISSUE: reference to a compiler-generated field
              if (owner.Index >= 0 && !this.m_TempData.HasComponent(owner))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Hidden>(jobIndex, component4.m_Original, new Hidden());
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, component4.m_Original, new BatchesUpdated());
              }
            }
          }
          if (cacheTransform)
          {
            LocalTransformCache component5;
            component5.m_Position = localTransformData.m_Position;
            component5.m_Rotation = localTransformData.m_Rotation;
            component5.m_ParentMesh = parentMesh;
            component5.m_GroupIndex = groupIndex;
            component5.m_Probability = probability;
            component5.m_PrefabSubIndex = prefabSubIndex;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalTransformCache>(jobIndex, entity, component5);
          }
          PseudoRandomSeed componentData15 = new PseudoRandomSeed();
          if (component1)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PseudoRandomSeedData.TryGetComponent(component4.m_Original, out componentData15))
              componentData15 = pseudoRandomSeed;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, componentData15);
          }
          if ((flags & SubObjectFlags.OnGround) == (SubObjectFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, entity, elevation);
          }
          if (alignIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Aligned>(jobIndex, entity, new Aligned((ushort) alignIndex));
          }
          if (relative || boneIndex.y >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Static>(jobIndex, entity);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Relative>(jobIndex, entity, new Relative(localTransformData, boneIndex));
          }
          if (interpolated || boneIndex.y >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InterpolatedTransform>(jobIndex, entity, new InterpolatedTransform(transformData));
          }
          UnderConstruction componentData16 = new UnderConstruction();
          if (component4.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            underConstruction = this.m_UnderConstructionData.TryGetComponent(component4.m_Original, out componentData16);
          }
          if (underConstruction)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<UnderConstruction>(jobIndex, entity, componentData16);
          }
          Destroyed componentData17 = new Destroyed();
          if (component4.m_Original != Entity.Null && (ownerTemp.m_Flags & TempFlags.Upgrade) == (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            isDestroyed = this.m_DestroyedData.TryGetComponent(component4.m_Original, out componentData17);
          }
          if (isDestroyed)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Destroyed>(jobIndex, entity, componentData17);
          }
          if (isOverridden)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Overridden>(jobIndex, entity, new Overridden());
          }
          if (native)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Native>(jobIndex, entity, new Native());
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode && this.m_EditorContainerData.HasComponent(component4.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Tools.EditorContainer component6 = this.m_EditorContainerData[component4.m_Original];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Game.Tools.EditorContainer>(jobIndex, entity, component6);
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabEffectData.HasComponent(component6.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddBuffer<EnabledEffect>(jobIndex, entity);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabStreetLightData.HasComponent(prefab))
          {
            StreetLight streetLight = new StreetLight();
            StreetLight componentData18;
            // ISSUE: reference to a compiler-generated field
            if (this.m_StreetLightData.TryGetComponent(component4.m_Original, out componentData18))
              streetLight = componentData18;
            Building componentData19;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingData.TryGetComponent(topOwner, out componentData19))
            {
              // ISSUE: reference to a compiler-generated method
              StreetLightSystem.UpdateStreetLightState(ref streetLight, componentData19);
            }
            else
            {
              Watercraft componentData20;
              // ISSUE: reference to a compiler-generated field
              if (this.m_WatercraftData.TryGetComponent(topOwner, out componentData20))
              {
                // ISSUE: reference to a compiler-generated method
                StreetLightSystem.UpdateStreetLightState(ref streetLight, componentData20);
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<StreetLight>(jobIndex, entity, streetLight);
          }
          StackData componentData21;
          // ISSUE: reference to a compiler-generated field
          if (flag1 && this.m_PrefabStackData.TryGetComponent(prefab, out componentData21))
          {
            Stack componentData22;
            // ISSUE: reference to a compiler-generated field
            if (this.m_StackData.TryGetComponent(component4.m_Original, out componentData22))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Stack>(jobIndex, entity, componentData22);
            }
            else
            {
              if (componentData21.m_Direction == StackDirection.Up)
              {
                componentData22.m_Range.min = componentData21.m_FirstBounds.min - elevation.m_Elevation;
                componentData22.m_Range.max = componentData21.m_LastBounds.max;
              }
              else
              {
                componentData22.m_Range.min = componentData21.m_FirstBounds.min;
                componentData22.m_Range.max = componentData21.m_FirstBounds.max + MathUtils.Size(componentData21.m_MiddleBounds) * 2f + MathUtils.Size(componentData21.m_LastBounds);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Stack>(jobIndex, entity, componentData22);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSpawnLocationData.HasComponent(prefab))
          {
            SpawnLocation component7 = new SpawnLocation()
            {
              m_GroupIndex = groupIndex
            };
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<SpawnLocation>(jobIndex, entity, component7);
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabSubObjects.HasBuffer(prefab))
            return;
          if (depth < 7)
          {
            // ISSUE: reference to a compiler-generated method
            updateData.EnsureDeepOwners(Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            updateData.m_DeepOwners.Add(new SubObjectSystem.DeepSubObjectOwnerData()
            {
              m_Transform = transformData,
              m_Temp = component4,
              m_Entity = entity,
              m_Prefab = prefab,
              m_Elevation = elevation.m_Elevation,
              m_RandomSeed = componentData15,
              m_New = true,
              m_HasRandomSeed = component1,
              m_UnderConstruction = underConstruction,
              m_Destroyed = isDestroyed,
              m_Overridden = isOverridden,
              m_Depth = depth + 1
            });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LoopErrorPrefabs.Enqueue(prefab);
          }
        }
      }

      private void CreateContainerObject(
        int jobIndex,
        Entity owner,
        bool isTemp,
        Temp ownerTemp,
        float ownerElevation,
        Entity oldSubObject,
        Transform transformData,
        Transform localTransformData,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        Entity prefab,
        float3 scale,
        float intensity,
        int parentMesh,
        int groupIndex,
        int prefabSubIndex)
      {
        Elevation component1 = new Elevation(ownerElevation, (ElevationFlags) 0);
        component1.m_Elevation += localTransformData.m_Position.y;
        if (oldSubObject == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          oldSubObject = this.FindOldSubObject(prefab, transformData, ref updateData);
        }
        if (oldSubObject != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, oldSubObject);
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              Temp component2 = this.m_TempData[oldSubObject] with
              {
                m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
              };
              if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
                component2.m_Flags |= TempFlags.Modify;
              // ISSUE: reference to a compiler-generated method
              component2.m_Original = this.FindOriginalSubObject(prefab, component2.m_Original, transformData, ref updateData);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Temp>(jobIndex, oldSubObject, component2);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Transform>(jobIndex, oldSubObject, transformData);
            // ISSUE: reference to a compiler-generated field
            if (!this.m_UpdatedData.HasComponent(oldSubObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldSubObject, new Updated());
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!transformData.Equals(this.m_TransformData[oldSubObject]))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Transform>(jobIndex, oldSubObject, transformData);
              // ISSUE: reference to a compiler-generated field
              if (!this.m_UpdatedData.HasComponent(oldSubObject))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldSubObject, new Updated());
              }
            }
          }
          LocalTransformCache component3;
          component3.m_Position = localTransformData.m_Position;
          component3.m_Rotation = localTransformData.m_Rotation;
          component3.m_ParentMesh = parentMesh;
          component3.m_GroupIndex = groupIndex;
          component3.m_Probability = 100;
          component3.m_PrefabSubIndex = prefabSubIndex;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<LocalTransformCache>(jobIndex, oldSubObject, component3);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Elevation>(jobIndex, oldSubObject, component1);
          Game.Tools.EditorContainer component4 = new Game.Tools.EditorContainer()
          {
            m_Prefab = prefab,
            m_Scale = scale,
            m_Intensity = intensity,
            m_GroupIndex = groupIndex
          };
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Game.Tools.EditorContainer>(jobIndex, oldSubObject, component4);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ObjectData objectData = this.m_PrefabObjectData[this.m_TransformEditor];
          if (!objectData.m_Archetype.Valid)
            return;
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, objectData.m_Archetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity, new Owner(owner));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(this.m_TransformEditor));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(jobIndex, entity, transformData);
          Temp component5 = new Temp();
          if (isTemp)
          {
            component5.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate);
            if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
              component5.m_Flags |= TempFlags.Modify;
            // ISSUE: reference to a compiler-generated method
            component5.m_Original = this.FindOriginalSubObject(prefab, Entity.Null, transformData, ref updateData);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity, component5);
          }
          LocalTransformCache component6;
          component6.m_Position = localTransformData.m_Position;
          component6.m_Rotation = localTransformData.m_Rotation;
          component6.m_ParentMesh = parentMesh;
          component6.m_GroupIndex = groupIndex;
          component6.m_Probability = 100;
          component6.m_PrefabSubIndex = prefabSubIndex;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<LocalTransformCache>(jobIndex, entity, component6);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, entity, component1);
          Game.Tools.EditorContainer component7 = new Game.Tools.EditorContainer()
          {
            m_Prefab = prefab,
            m_Scale = scale,
            m_Intensity = intensity,
            m_GroupIndex = groupIndex
          };
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Game.Tools.EditorContainer>(jobIndex, entity, component7);
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabEffectData.HasComponent(component7.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddBuffer<EnabledEffect>(jobIndex, entity);
        }
      }

      private void EnsurePlaceholderRequirements(
        Entity owner,
        Entity ownerPrefab,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        ref Unity.Mathematics.Random random,
        bool isObject)
      {
        // ISSUE: reference to a compiler-generated field
        if (updateData.m_RequirementsSearched)
          return;
        // ISSUE: reference to a compiler-generated method
        updateData.EnsurePlaceholderRequirements(Allocator.Temp);
        bool flag1 = false;
        bool flag2 = false;
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        if (!isObject && this.m_OwnerData.TryGetComponent(owner, out componentData1))
        {
          owner = componentData1.m_Owner;
          Attachment componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachmentData.TryGetComponent(componentData1.m_Owner, out componentData2))
            owner = componentData2.m_Attached;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_CityServiceUpkeepData.HasComponent(owner))
        {
          DynamicBuffer<ServiceUpkeepData> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabServiceUpkeepDatas.TryGetBuffer(ownerPrefab, out bufferData))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              updateData.m_StoredResources |= bufferData[index].m_Upkeep.m_Resource;
            }
          }
          StorageCompanyData componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_StorageCompanyData.TryGetComponent(ownerPrefab, out componentData3))
          {
            // ISSUE: reference to a compiler-generated field
            updateData.m_StoredResources |= componentData3.m_StoredResources;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageFacilityData.HasComponent(owner))
          {
            // ISSUE: reference to a compiler-generated field
            updateData.m_StoredResources |= Resource.Garbage;
          }
        }
        DynamicBuffer<Renter> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingRenters.TryGetBuffer(owner, out bufferData1))
        {
          for (int index1 = 0; index1 < bufferData1.Length; ++index1)
          {
            Entity renter = bufferData1[index1].m_Renter;
            CompanyData componentData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CompanyData.TryGetComponent(renter, out componentData4))
            {
              // ISSUE: reference to a compiler-generated field
              updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.Renter;
              // ISSUE: reference to a compiler-generated field
              Entity prefab = this.m_PrefabRefData[renter].m_Prefab;
              if (componentData4.m_Brand != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                updateData.m_PlaceholderRequirements.TryAdd(componentData4.m_Brand, new int2(0, 1));
                // ISSUE: reference to a compiler-generated method
                this.AddAffiliatedBrands(prefab, ref updateData, ref random);
                flag2 = true;
              }
              StorageCompanyData componentData5;
              // ISSUE: reference to a compiler-generated field
              if (this.m_StorageCompanyData.TryGetComponent(prefab, out componentData5))
              {
                // ISSUE: reference to a compiler-generated field
                updateData.m_StoredResources |= componentData5.m_StoredResources;
              }
              // ISSUE: reference to a compiler-generated field
              updateData.m_PlaceholderRequirements.TryAdd(prefab, (int2) 0);
            }
            else
            {
              DynamicBuffer<HouseholdCitizen> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_HouseholdCitizens.TryGetBuffer(renter, out bufferData2))
              {
                for (int index2 = 0; index2 < bufferData2.Length; ++index2)
                {
                  Citizen componentData6;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CitizenData.TryGetComponent(bufferData2[index2].m_Citizen, out componentData6))
                  {
                    switch (componentData6.GetAge())
                    {
                      case CitizenAge.Child:
                        // ISSUE: reference to a compiler-generated field
                        updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.Children;
                        continue;
                      case CitizenAge.Teen:
                        // ISSUE: reference to a compiler-generated field
                        updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.Teens;
                        continue;
                      default:
                        continue;
                    }
                  }
                }
                Household componentData7;
                // ISSUE: reference to a compiler-generated field
                if (this.m_HouseholdData.TryGetComponent(renter, out componentData7))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  int2 consumptionBonuses = CitizenHappinessSystem.GetConsumptionBonuses((float) componentData7.m_ConsumptionPerDay, bufferData2.Length, in this.m_HappinessParameterData);
                  if (consumptionBonuses.x + consumptionBonuses.y > 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.GoodWealth;
                  }
                }
                DynamicBuffer<HouseholdAnimal> bufferData3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_HouseholdAnimals.TryGetBuffer(renter, out bufferData3) && bufferData3.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.Dogs;
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_HomelessHousehold.HasComponent(renter))
                {
                  // ISSUE: reference to a compiler-generated field
                  updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.Homeless;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.Renter;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ResidentialPropertyData.HasComponent(owner))
        {
          // ISSUE: reference to a compiler-generated field
          updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.Children | ObjectRequirementFlags.Teens | ObjectRequirementFlags.GoodWealth | ObjectRequirementFlags.Dogs;
        }
        Surface componentData8;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SurfaceData.TryGetComponent(owner, out componentData8) && componentData8.m_AccumulatedSnow >= (byte) 15)
        {
          // ISSUE: reference to a compiler-generated field
          updateData.m_PlaceholderRequirementFlags |= ObjectRequirementFlags.Snow;
        }
        DynamicBuffer<ObjectRequirementElement> bufferData4;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectRequirements.TryGetBuffer(ownerPrefab, out bufferData4))
        {
          int num1 = 0;
          while (num1 < bufferData4.Length)
          {
            ObjectRequirementElement requirementElement = bufferData4[num1];
            if ((requirementElement.m_Type & ObjectRequirementType.SelectOnly) != (ObjectRequirementType) 0)
            {
              int num2 = num1;
              do
                ;
              while (++num2 < bufferData4.Length && (int) bufferData4[num2].m_Group == (int) requirementElement.m_Group);
              Entity requirement = bufferData4[random.NextInt(num1, num2)].m_Requirement;
              // ISSUE: reference to a compiler-generated field
              updateData.m_PlaceholderRequirements.TryAdd(requirement, (int2) 0);
              DynamicBuffer<CompanyBrandElement> bufferData5;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CompanyBrands.TryGetBuffer(requirement, out bufferData5))
              {
                if (bufferData5.Length != 0)
                {
                  Entity brand = bufferData5[random.NextInt(bufferData5.Length)].m_Brand;
                  // ISSUE: reference to a compiler-generated field
                  updateData.m_PlaceholderRequirements.TryAdd(brand, new int2(0, 1));
                  // ISSUE: reference to a compiler-generated method
                  this.AddAffiliatedBrands(requirement, ref updateData, ref random);
                  flag2 = true;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabThemeData.HasComponent(requirement))
                  flag1 = true;
              }
              num1 = num2;
            }
            else
              ++num1;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!flag1 && this.m_DefaultTheme != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          updateData.m_PlaceholderRequirements.TryAdd(this.m_DefaultTheme, (int2) 0);
        }
        // ISSUE: reference to a compiler-generated field
        if (!flag2 && this.m_BuildingConfigurationData.m_DefaultRenterBrand != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          updateData.m_PlaceholderRequirements.TryAdd(this.m_BuildingConfigurationData.m_DefaultRenterBrand, (int2) 0);
        }
        // ISSUE: reference to a compiler-generated field
        updateData.m_RequirementsSearched = true;
      }

      private void AddAffiliatedBrands(
        Entity entity,
        ref SubObjectSystem.UpdateSubObjectsData updateData,
        ref Unity.Mathematics.Random random)
      {
        DynamicBuffer<AffiliatedBrandElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AffiliatedBrands.TryGetBuffer(entity, out bufferData) || bufferData.Length == 0)
          return;
        int max = bufferData.Length + 3 >> 1;
        int num = 0;
        int y = 0;
        Unity.Mathematics.Random random1 = random;
        for (int index = random.NextInt(max >> 1); index < bufferData.Length; index += 1 + random.NextInt(max))
          ++y;
        for (int index = random1.NextInt(max >> 1); index < bufferData.Length; index += 1 + random1.NextInt(max))
        {
          // ISSUE: reference to a compiler-generated field
          updateData.m_PlaceholderRequirements.TryAdd(bufferData[index].m_Brand, new int2(--num, y));
        }
      }

      private Entity FindOldSubObject(
        Entity prefab,
        Entity original,
        ref SubObjectSystem.UpdateSubObjectsData updateData)
      {
        Entity entity;
        NativeParallelMultiHashMapIterator<Entity> it;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (updateData.m_OldEntities.IsCreated && updateData.m_OldEntities.TryGetFirstValue(prefab, out entity, out it))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (!this.m_TempData.HasComponent(entity) || !(this.m_TempData[entity].m_Original == original))
          {
            // ISSUE: reference to a compiler-generated field
            if (!updateData.m_OldEntities.TryGetNextValue(out entity, ref it))
              goto label_4;
          }
          // ISSUE: reference to a compiler-generated field
          updateData.m_OldEntities.Remove(it);
          return entity;
        }
label_4:
        return Entity.Null;
      }

      private Entity FindOldSubObject(
        Entity prefab,
        Transform transform,
        ref SubObjectSystem.UpdateSubObjectsData updateData)
      {
        Entity oldSubObject = Entity.Null;
        Entity entity;
        NativeParallelMultiHashMapIterator<Entity> it1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (updateData.m_OldEntities.IsCreated && updateData.m_OldEntities.TryGetFirstValue(prefab, out entity, out it1))
        {
          oldSubObject = entity;
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(this.m_TransformData[entity].m_Position, transform.m_Position);
          NativeParallelMultiHashMapIterator<Entity> it2 = it1;
          // ISSUE: reference to a compiler-generated field
          while (updateData.m_OldEntities.TryGetNextValue(out entity, ref it1))
          {
            // ISSUE: reference to a compiler-generated field
            float num2 = math.distance(this.m_TransformData[entity].m_Position, transform.m_Position);
            if ((double) num2 < (double) num1)
            {
              oldSubObject = entity;
              num1 = num2;
              it2 = it1;
            }
          }
          // ISSUE: reference to a compiler-generated field
          updateData.m_OldEntities.Remove(it2);
        }
        return oldSubObject;
      }

      private Entity FindOriginalSubObject(
        Entity prefab,
        Entity original,
        Transform transform,
        ref SubObjectSystem.UpdateSubObjectsData updateData)
      {
        Entity originalSubObject = Entity.Null;
        Entity entity;
        NativeParallelMultiHashMapIterator<Entity> it1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (updateData.m_OriginalEntities.IsCreated && updateData.m_OriginalEntities.TryGetFirstValue(prefab, out entity, out it1))
        {
          if (entity == original)
          {
            // ISSUE: reference to a compiler-generated field
            updateData.m_OriginalEntities.Remove(it1);
            return original;
          }
          originalSubObject = entity;
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(this.m_TransformData[entity].m_Position, transform.m_Position);
          NativeParallelMultiHashMapIterator<Entity> it2 = it1;
          // ISSUE: reference to a compiler-generated field
          while (updateData.m_OriginalEntities.TryGetNextValue(out entity, ref it1))
          {
            if (entity == original)
            {
              // ISSUE: reference to a compiler-generated field
              updateData.m_OriginalEntities.Remove(it1);
              return original;
            }
            // ISSUE: reference to a compiler-generated field
            float num2 = math.distance(this.m_TransformData[entity].m_Position, transform.m_Position);
            if ((double) num2 < (double) num1)
            {
              originalSubObject = entity;
              num1 = num2;
              it2 = it1;
            }
          }
          // ISSUE: reference to a compiler-generated field
          updateData.m_OriginalEntities.Remove(it2);
        }
        return originalSubObject;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Object> __Game_Objects_Object_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SubObjectsUpdated> __Game_Objects_SubObjectsUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RentersUpdated> __Game_Buildings_RentersUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Creature> __Game_Creatures_Creature_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Created> __Game_Common_Created_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Secondary> __Game_Objects_Secondary_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Object> __Game_Objects_Object_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetObjectData> __Game_Prefabs_NetObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PillarData> __Game_Prefabs_PillarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ThemeData> __Game_Prefabs_ThemeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingObjectData> __Game_Prefabs_MovingObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> __Game_Prefabs_QuantityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkVehicleData> __Game_Prefabs_WorkVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EffectData> __Game_Prefabs_EffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.StreetLightData> __Game_Prefabs_StreetLightData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> __Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceholderObjectData> __Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TreeData> __Game_Prefabs_TreeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> __Game_Prefabs_StorageCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Aligned> __Game_Objects_Aligned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StreetLight> __Game_Objects_StreetLight_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Relative> __Game_Objects_Relative_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Overridden> __Game_Common_Overridden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Surface> __Game_Objects_Surface_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stack> __Game_Objects_Stack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.GarbageFacility> __Game_Buildings_GarbageFacility_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResidentialProperty> __Game_Buildings_ResidentialProperty_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CityServiceUpkeep> __Game_City_CityServiceUpkeep_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Fixed> __Game_Net_Fixed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CompanyData> __Game_Companies_CompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HomelessHousehold> __Game_Citizens_HomelessHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Area> __Game_Areas_Area_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Clear> __Game_Areas_Clear_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Watercraft> __Game_Vehicles_Watercraft_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> __Game_Prefabs_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubLane> __Game_Prefabs_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Effect> __Game_Prefabs_Effect_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CompanyBrandElement> __Game_Prefabs_CompanyBrandElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AffiliatedBrandElement> __Game_Prefabs_AffiliatedBrandElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> __Game_Prefabs_ServiceUpkeepData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Object_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Object>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObjectsUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SubObjectsUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RentersUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentLookup = state.GetComponentLookup<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Secondary_RO_ComponentLookup = state.GetComponentLookup<Secondary>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Object_RO_ComponentLookup = state.GetComponentLookup<Object>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetObjectData_RO_ComponentLookup = state.GetComponentLookup<NetObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PillarData_RO_ComponentLookup = state.GetComponentLookup<PillarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ThemeData_RO_ComponentLookup = state.GetComponentLookup<ThemeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MovingObjectData_RO_ComponentLookup = state.GetComponentLookup<MovingObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup = state.GetComponentLookup<QuantityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkVehicleData_RO_ComponentLookup = state.GetComponentLookup<WorkVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectData_RO_ComponentLookup = state.GetComponentLookup<EffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StreetLightData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.StreetLightData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TreeData_RO_ComponentLookup = state.GetComponentLookup<TreeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup = state.GetComponentLookup<StorageCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Aligned_RO_ComponentLookup = state.GetComponentLookup<Aligned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_StreetLight_RO_ComponentLookup = state.GetComponentLookup<StreetLight>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Relative_RO_ComponentLookup = state.GetComponentLookup<Relative>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentLookup = state.GetComponentLookup<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Surface_RO_ComponentLookup = state.GetComponentLookup<Surface>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentLookup = state.GetComponentLookup<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentLookup = state.GetComponentLookup<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentLookup = state.GetComponentLookup<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentLookup = state.GetComponentLookup<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.GarbageFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResidentialProperty_RO_ComponentLookup = state.GetComponentLookup<ResidentialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityServiceUpkeep_RO_ComponentLookup = state.GetComponentLookup<CityServiceUpkeep>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Net.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Fixed_RO_ComponentLookup = state.GetComponentLookup<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<Game.Tools.EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RO_ComponentLookup = state.GetComponentLookup<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HomelessHousehold_RO_ComponentLookup = state.GetComponentLookup<HomelessHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RO_ComponentLookup = state.GetComponentLookup<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Clear_RO_ComponentLookup = state.GetComponentLookup<Clear>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Watercraft_RO_ComponentLookup = state.GetComponentLookup<Watercraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RO_BufferLookup = state.GetBufferLookup<Effect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CompanyBrandElement_RO_BufferLookup = state.GetBufferLookup<CompanyBrandElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AffiliatedBrandElement_RO_BufferLookup = state.GetBufferLookup<AffiliatedBrandElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup = state.GetBufferLookup<ServiceUpkeepData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RO_BufferLookup = state.GetBufferLookup<HouseholdAnimal>(true);
      }
    }
  }
}
