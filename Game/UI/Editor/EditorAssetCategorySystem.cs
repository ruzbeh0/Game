// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorAssetCategorySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Zones;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class EditorAssetCategorySystem : GameSystemBase
  {
    private List<EditorAssetCategory> m_Categories = new List<EditorAssetCategory>();
    private Dictionary<string, EditorAssetCategory> m_PathMap = new Dictionary<string, EditorAssetCategory>();
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_ServiceQuery;
    private EntityQuery m_ZoneQuery;
    private EntityQuery m_ThemeQuery;
    private EntityQuery m_Overrides;
    private EntityQuery m_PrefabModificationQuery;
    private bool m_Dirty = true;
    private EditorAssetCategorySystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneQuery = this.GetEntityQuery(ComponentType.ReadOnly<ZoneData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ThemeQuery = this.GetEntityQuery(ComponentType.ReadOnly<ThemeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_Overrides = this.GetEntityQuery(ComponentType.ReadOnly<EditorAssetCategoryOverrideData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabModificationQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_PrefabModificationQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = true;
    }

    public IEnumerable<EditorAssetCategory> GetCategories(bool ignoreEmpty = true)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_Dirty)
      {
        // ISSUE: reference to a compiler-generated method
        this.GenerateCategories();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      foreach ((EditorAssetCategory, int) valueTuple in this.GetCategoriesImpl((IEnumerable<EditorAssetCategory>) this.m_Categories, 0, ignoreEmpty))
        yield return valueTuple.Item1;
    }

    public IEnumerable<HierarchyItem<EditorAssetCategory>> GetHierarchy(bool ignoreEmpty = true)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_Dirty)
      {
        // ISSUE: reference to a compiler-generated method
        this.GenerateCategories();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      foreach ((EditorAssetCategory, int) valueTuple in this.GetCategoriesImpl((IEnumerable<EditorAssetCategory>) this.m_Categories, 0, ignoreEmpty))
        yield return valueTuple.Item1.ToHierarchyItem(valueTuple.Item2);
    }

    private void AddCategory(EditorAssetCategory category, EditorAssetCategory parent = null)
    {
      string str = category.id.Trim('/');
      category.path = parent != null ? parent.path + "/" + str : str;
      if (parent == null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Categories.Add(category);
      }
      else
        parent.AddSubCategory(category);
      // ISSUE: reference to a compiler-generated field
      this.m_PathMap[category.path] = category;
    }

    private void ClearCategories()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Categories.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_PathMap.Clear();
    }

    private IEnumerable<(EditorAssetCategory, int)> GetCategoriesImpl(
      IEnumerable<EditorAssetCategory> categories,
      int level,
      bool ignoreEmpty)
    {
      // ISSUE: variable of a compiler-generated type
      EditorAssetCategorySystem assetCategorySystem = this;
      foreach (EditorAssetCategory category1 in categories)
      {
        EditorAssetCategory category = category1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        assetCategorySystem.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref assetCategorySystem.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!ignoreEmpty || !category.IsEmpty(assetCategorySystem.EntityManager, assetCategorySystem.m_PrefabSystem, assetCategorySystem.__TypeHandle.__Unity_Entities_Entity_TypeHandle))
        {
          yield return (category, level);
          // ISSUE: reference to a compiler-generated method
          foreach ((EditorAssetCategory, int) valueTuple in assetCategorySystem.GetCategoriesImpl((IEnumerable<EditorAssetCategory>) category.subCategories, level + 1, ignoreEmpty))
            yield return (valueTuple.Item1, valueTuple.Item2);
          category = (EditorAssetCategory) null;
        }
      }
    }

    private void GenerateCategories()
    {
      // ISSUE: reference to a compiler-generated method
      this.ClearCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateBuildingCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateVehicleCategories();
      // ISSUE: reference to a compiler-generated method
      this.GeneratePropCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateFoliageCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateCharacterCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateAreaCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateSurfaceCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateBridgeCategory();
      // ISSUE: reference to a compiler-generated method
      this.GenerateRoadCategory();
      // ISSUE: reference to a compiler-generated method
      this.GenerateTrackCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateEffectCategories();
      // ISSUE: reference to a compiler-generated method
      this.GenerateLocationCategories();
      // ISSUE: reference to a compiler-generated method
      this.AddOverrides();
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = false;
    }

    private void GenerateBuildingCategories()
    {
      EditorAssetCategory editorAssetCategory1 = new EditorAssetCategory();
      editorAssetCategory1.id = "Buildings";
      editorAssetCategory1.entityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Prefabs.BuildingData>(),
          ComponentType.ReadOnly<BuildingExtensionData>()
        }
      });
      editorAssetCategory1.includeChildCategories = false;
      EditorAssetCategory editorAssetCategory2 = editorAssetCategory1;
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory2);
      // ISSUE: reference to a compiler-generated method
      this.GenerateServiceBuildingCategories(editorAssetCategory2);
      // ISSUE: reference to a compiler-generated method
      this.GenerateSpawnableBuildingCategories(editorAssetCategory2);
      // ISSUE: reference to a compiler-generated method
      this.GenerateMiscBuildingCategory(editorAssetCategory2);
    }

    private void GenerateServiceBuildingCategories(EditorAssetCategory parent)
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Services"
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory, parent);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ServiceQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<PrefabBase>(entityArray[index], out prefab))
        {
          EditorAssetCategory category = new EditorAssetCategory();
          category.id = prefab.name;
          category.entityQuery = this.GetEntityQuery(new EntityQueryDesc()
          {
            All = new ComponentType[2]
            {
              ComponentType.ReadOnly<Game.Prefabs.BuildingData>(),
              ComponentType.ReadOnly<ServiceObjectData>()
            },
            None = new ComponentType[1]
            {
              ComponentType.ReadOnly<TrafficSpawnerData>()
            }
          }, new EntityQueryDesc()
          {
            All = new ComponentType[1]
            {
              ComponentType.ReadOnly<ServiceUpgradeBuilding>()
            },
            Any = new ComponentType[2]
            {
              ComponentType.ReadOnly<Game.Prefabs.BuildingData>(),
              ComponentType.ReadOnly<BuildingExtensionData>()
            },
            None = new ComponentType[1]
            {
              ComponentType.ReadOnly<TrafficSpawnerData>()
            }
          });
          // ISSUE: object of a compiler-generated type is created
          category.filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.ServiceTypeFilter(entityArray[index]);
          // ISSUE: reference to a compiler-generated method
          category.icon = ImageSystem.GetIcon(prefab);
          // ISSUE: reference to a compiler-generated method
          this.AddCategory(category, editorAssetCategory);
        }
      }
    }

    private void GenerateSpawnableBuildingCategories(EditorAssetCategory parent)
    {
      // ISSUE: reference to a compiler-generated method
      this.GenerateZoneCategories(AreaType.Residential, false, parent);
      // ISSUE: reference to a compiler-generated method
      this.GenerateZoneCategories(AreaType.Commercial, false, parent);
      // ISSUE: reference to a compiler-generated method
      this.GenerateZoneCategories(AreaType.Industrial, false, parent);
      // ISSUE: reference to a compiler-generated method
      this.GenerateZoneCategories(AreaType.Industrial, true, parent);
    }

    private void GenerateZoneCategories(AreaType areaType, bool office, EditorAssetCategory parent)
    {
      string str = office ? "Office" : areaType.ToString();
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = str
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory, parent);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_ZoneQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < entityArray1.Length; ++index)
      {
        ZonePrefab prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<ZonePrefab>(entityArray1[index], out prefab) && prefab.m_AreaType == areaType && prefab.m_Office == office)
        {
          EditorAssetCategory category = new EditorAssetCategory();
          category.id = prefab.name;
          category.entityQuery = this.GetEntityQuery(new EntityQueryDesc()
          {
            All = new ComponentType[1]
            {
              ComponentType.ReadOnly<Game.Prefabs.BuildingData>()
            },
            Any = new ComponentType[2]
            {
              ComponentType.ReadOnly<SpawnableBuildingData>(),
              ComponentType.ReadOnly<PlaceholderBuildingData>()
            }
          });
          // ISSUE: object of a compiler-generated type is created
          category.filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.ZoneTypeFilter(entityArray1[index]);
          // ISSUE: reference to a compiler-generated method
          category.icon = ImageSystem.GetIcon((PrefabBase) prefab);
          // ISSUE: reference to a compiler-generated method
          this.AddCategory(category, editorAssetCategory);
        }
      }
      if (areaType == AreaType.Industrial && !office)
      {
        // ISSUE: reference to a compiler-generated method
        this.AddCategory(new EditorAssetCategory()
        {
          id = "Extractors",
          entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.BuildingData>(), ComponentType.ReadOnly<ExtractorFacilityData>())
        }, editorAssetCategory);
      }
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray2 = this.m_ThemeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < entityArray2.Length; ++index)
      {
        ThemePrefab prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<ThemePrefab>(entityArray2[index], out prefab))
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated method
          this.AddCategory(new EditorAssetCategory()
          {
            id = prefab.assetPrefix + " Signature",
            entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<SignatureBuildingData>()),
            filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.SignatureBuildingFilter()
            {
              m_AreaType = areaType,
              m_Office = office,
              m_Theme = entityArray2[index]
            }
          }, editorAssetCategory);
        }
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Signature",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<SignatureBuildingData>()),
        filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.SignatureBuildingFilter()
        {
          m_AreaType = areaType,
          m_Office = office,
          m_Theme = Entity.Null
        }
      }, editorAssetCategory);
      entityArray1.Dispose();
    }

    private void GenerateMiscBuildingCategory(EditorAssetCategory parent)
    {
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Misc",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.BuildingData>(), ComponentType.Exclude<ServiceObjectData>(), ComponentType.Exclude<SpawnableBuildingData>(), ComponentType.Exclude<SignatureBuildingData>(), ComponentType.Exclude<ServiceUpgradeBuilding>(), ComponentType.Exclude<ExtractorFacilityData>())
      }, parent);
    }

    private void GenerateVehicleCategories()
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Vehicles",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<VehicleData>()),
        includeChildCategories = false
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GenerateResidentialVehicleCategory(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GenerateIndustrialVehicleCategory(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GenerateServiceVehicleCategories(editorAssetCategory);
    }

    private void GenerateResidentialVehicleCategory(EditorAssetCategory parent)
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Residential"
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory, parent);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Cars",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<VehicleData>(), ComponentType.ReadOnly<PersonalCarData>()),
        filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.PassengerCountFilter()
        {
          m_Type = EditorAssetCategorySystem.PassengerCountFilter.FilterType.NotEquals,
          m_Count = 1
        },
        icon = "Media/Game/Icons/GenericVehicle.svg"
      }, editorAssetCategory);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Bikes",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<VehicleData>(), ComponentType.ReadOnly<PersonalCarData>()),
        filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.PassengerCountFilter()
        {
          m_Type = EditorAssetCategorySystem.PassengerCountFilter.FilterType.Equals,
          m_Count = 1
        },
        icon = "Media/Game/Icons/Bicycle.svg"
      }, editorAssetCategory);
    }

    private void GenerateServiceVehicleCategories(EditorAssetCategory parent)
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Services"
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory, parent);
      // ISSUE: reference to a compiler-generated method
      this.GeneratePublicTransportVehicleCategory(TransportType.Bus, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GeneratePublicTransportVehicleCategory(TransportType.Taxi, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GeneratePublicTransportVehicleCategory(TransportType.Tram, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GeneratePublicTransportVehicleCategory(TransportType.Train, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GeneratePublicTransportVehicleCategory(TransportType.Subway, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GeneratePublicTransportVehicleCategory(TransportType.Ship, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Aircraft",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<AircraftData>(), ComponentType.Exclude<CargoTransportVehicleData>()),
        icon = "Media/Game/Icons/airplane.svg"
      }, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Healthcare",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<AmbulanceData>()),
        icon = "Media/Game/Icons/Healthcare.svg"
      }, editorAssetCategory);
      EditorAssetCategory category = new EditorAssetCategory();
      category.id = "Police";
      category.entityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<VehicleData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<PoliceCarData>(),
          ComponentType.ReadOnly<PublicTransportVehicleData>()
        }
      });
      category.icon = "Media/Game/Icons/Police.svg";
      // ISSUE: object of a compiler-generated type is created
      category.filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.PublicTransportTypeFilter()
      {
        m_TransportType = TransportType.None,
        m_Purpose = PublicTransportPurpose.PrisonerTransport
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(category, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Deathcare",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<HearseData>()),
        icon = "Media/Game/Icons/Deathcare.svg"
      }, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "FireRescue",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireEngineData>()),
        icon = "Media/Game/Icons/FireSafety.svg"
      }, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Garbage",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<GarbageTruckData>()),
        icon = "Media/Game/Icons/Garbage.svg"
      }, editorAssetCategory);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Parks",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<MaintenanceVehicleData>()),
        icon = "Media/Game/Icons/ParksAndRecreation.svg",
        filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.MaintenanceTypeFilter()
        {
          m_Type = MaintenanceType.Park
        }
      }, editorAssetCategory);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Roads",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<MaintenanceVehicleData>()),
        icon = "Media/Game/Icons/Roads.svg",
        filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.MaintenanceTypeFilter()
        {
          m_Type = (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)
        }
      }, editorAssetCategory);
    }

    private void GeneratePublicTransportVehicleCategory(
      TransportType transportType,
      EditorAssetCategory parent)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = transportType.ToString(),
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<VehicleData>(), ComponentType.ReadOnly<PublicTransportVehicleData>()),
        icon = string.Format("Media/Game/Icons/{0}.svg", (object) transportType),
        filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.PublicTransportTypeFilter()
        {
          m_TransportType = transportType,
          m_Purpose = PublicTransportPurpose.TransportLine
        }
      }, parent);
    }

    private void GenerateIndustrialVehicleCategory(EditorAssetCategory parent)
    {
      EditorAssetCategory category = new EditorAssetCategory();
      category.id = "Industrial";
      category.entityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<VehicleData>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<CargoTransportVehicleData>(),
          ComponentType.ReadOnly<DeliveryTruckData>(),
          ComponentType.ReadOnly<WorkVehicleData>()
        }
      });
      category.icon = "Media/Game/Icons/ZoneIndustrial.svg";
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(category, parent);
    }

    private void GeneratePropCategories()
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Props",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<StaticObjectData>(), ComponentType.Exclude<Game.Prefabs.BuildingData>(), ComponentType.Exclude<NetObjectData>(), ComponentType.Exclude<BuildingExtensionData>(), ComponentType.Exclude<PillarData>(), ComponentType.Exclude<PlantData>(), ComponentType.Exclude<BrandObjectData>()),
        includeChildCategories = false
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Brand Graphics",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<StaticObjectData>(), ComponentType.ReadOnly<BrandObjectData>(), ComponentType.Exclude<Game.Prefabs.BuildingData>(), ComponentType.Exclude<NetObjectData>(), ComponentType.Exclude<BuildingExtensionData>(), ComponentType.Exclude<PillarData>(), ComponentType.Exclude<PlantData>())
      }, editorAssetCategory);
    }

    private void GenerateFoliageCategories()
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Foliage",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<PlantData>()),
        icon = "Media/Game/Icons/Vegetation.svg",
        includeChildCategories = false
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GenerateTreeCategories(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GenerateBushCategories(editorAssetCategory);
    }

    private void GenerateTreeCategories(EditorAssetCategory parent)
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Trees"
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory, parent);
      // ISSUE: reference to a compiler-generated field
      foreach (Entity entity in this.m_ThemeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        ThemePrefab prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<ThemePrefab>(entity, out prefab))
        {
          string icon = prefab.GetComponent<UIObject>()?.m_Icon;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated method
          this.AddCategory(new EditorAssetCategory()
          {
            id = prefab.assetPrefix,
            entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<TreeData>()),
            icon = icon,
            filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.ThemeFilter()
            {
              m_Theme = entity,
              m_DefaultResult = false
            }
          }, editorAssetCategory);
        }
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Shared",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<TreeData>()),
        filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.ThemeFilter()
        {
          m_Theme = Entity.Null,
          m_DefaultResult = true
        }
      }, editorAssetCategory);
    }

    private void GenerateBushCategories(EditorAssetCategory parent)
    {
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Bushes",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<PlantData>(), ComponentType.Exclude<TreeData>())
      }, parent);
    }

    private void GenerateRoadCategory()
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Roads"
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Roads",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<RoadData>(), ComponentType.Exclude<BridgeData>()),
        icon = "Media/Game/Icons/Roads.svg"
      }, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Intersections",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<AssetStampData>(), ComponentType.ReadOnly<Game.Prefabs.SubNet>())
      }, editorAssetCategory);
    }

    private void GenerateTrackCategories()
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Tracks",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<TrackData>()),
        includeChildCategories = false
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GenerateTrackTypeCategory(TrackTypes.Train, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GenerateTrackTypeCategory(TrackTypes.Tram, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.GenerateTrackTypeCategory(TrackTypes.Subway, editorAssetCategory);
    }

    private void GenerateTrackTypeCategory(TrackTypes trackTypes, EditorAssetCategory parent)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = trackTypes.ToString(),
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<TrackData>()),
        filter = (EditorAssetCategorySystem.IEditorAssetCategoryFilter) new EditorAssetCategorySystem.TrackTypeFilter()
        {
          m_TrackType = trackTypes
        }
      }, parent);
    }

    private void GenerateEffectCategories()
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory()
      {
        id = "Effects",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<EffectData>())
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "VFX",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<EffectData>(), ComponentType.ReadOnly<VFXData>())
      }, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Audio",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<EffectData>(), ComponentType.ReadOnly<AudioEffectData>())
      }, editorAssetCategory);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Lights",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<EffectData>(), ComponentType.ReadOnly<LightEffectData>())
      }, editorAssetCategory);
    }

    private void GenerateLocationCategories()
    {
      EditorAssetCategory editorAssetCategory1 = new EditorAssetCategory()
      {
        id = "Locations"
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory1);
      EditorAssetCategory editorAssetCategory2 = new EditorAssetCategory()
      {
        id = "Spawners"
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory2, editorAssetCategory1);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Animals",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreatureSpawnData>())
      }, editorAssetCategory2);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Vehicles",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<TrafficSpawnerData>())
      }, editorAssetCategory2);
    }

    private void GenerateBridgeCategory()
    {
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Bridges",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<BridgeData>()),
        icon = "Media/Game/Icons/CableStayed.svg"
      });
    }

    private void GenerateCharacterCategories()
    {
      EditorAssetCategory editorAssetCategory1 = new EditorAssetCategory()
      {
        id = "Characters",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreatureData>()),
        includeChildCategories = false
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory1);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "People",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<HumanData>())
      }, editorAssetCategory1);
      EditorAssetCategory editorAssetCategory2 = new EditorAssetCategory()
      {
        id = "Animals",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<AnimalData>())
      };
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(editorAssetCategory2, editorAssetCategory1);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Pets",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<PetData>())
      }, editorAssetCategory2);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Livestock",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<DomesticatedData>())
      }, editorAssetCategory2);
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Wildlife",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<WildlifeData>())
      }, editorAssetCategory2);
    }

    private void GenerateAreaCategories()
    {
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Areas",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<AreaData>(), ComponentType.Exclude<SurfaceData>())
      });
    }

    private void GenerateSurfaceCategories()
    {
      // ISSUE: reference to a compiler-generated method
      this.AddCategory(new EditorAssetCategory()
      {
        id = "Surfaces",
        entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<SurfaceData>())
      });
    }

    private void AddOverrides()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_Overrides.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<PrefabBase>(entityArray[index1], out prefab))
        {
          EditorAssetCategoryOverride component = prefab.GetComponent<EditorAssetCategoryOverride>();
          if (component.m_IncludeCategories != null)
          {
            for (int index2 = 0; index2 < component.m_IncludeCategories.Length; ++index2)
            {
              string includeCategory = component.m_IncludeCategories[index2];
              EditorAssetCategory editorAssetCategory;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathMap.TryGetValue(includeCategory, out editorAssetCategory))
              {
                editorAssetCategory.AddEntity(entityArray[index1]);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.CreateCategory(includeCategory).AddEntity(entityArray[index1]);
              }
            }
          }
          if (component.m_ExcludeCategories != null)
          {
            for (int index3 = 0; index3 < component.m_ExcludeCategories.Length; ++index3)
            {
              EditorAssetCategory editorAssetCategory;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathMap.TryGetValue(component.m_ExcludeCategories[index3], out editorAssetCategory))
                editorAssetCategory.AddExclusion(entityArray[index1]);
            }
          }
        }
      }
    }

    private EditorAssetCategory CreateCategory(string path)
    {
      string[] strArray = path.Split("/", StringSplitOptions.None);
      EditorAssetCategory parent = (EditorAssetCategory) null;
      string key = (string) null;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str;
        if (key == null)
          str = strArray[index];
        else
          str = string.Join("/", new string[2]
          {
            key,
            strArray[index]
          });
        key = str;
        EditorAssetCategory editorAssetCategory;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathMap.TryGetValue(key, out editorAssetCategory))
        {
          parent = editorAssetCategory;
        }
        else
        {
          EditorAssetCategory category = new EditorAssetCategory()
          {
            id = strArray[index]
          };
          // ISSUE: reference to a compiler-generated method
          this.AddCategory(category, parent);
          parent = category;
        }
      }
      return parent;
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

    [Preserve]
    public EditorAssetCategorySystem()
    {
    }

    public interface IEditorAssetCategoryFilter
    {
      bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem prefabSystem);
    }

    public class ServiceTypeFilter : EditorAssetCategorySystem.IEditorAssetCategoryFilter
    {
      private Entity m_Service;

      public ServiceTypeFilter(Entity service) => this.m_Service = service;

      public bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem _)
      {
        ServiceObjectData component;
        if (entityManager.TryGetComponent<ServiceObjectData>(prefab, out component))
        {
          // ISSUE: reference to a compiler-generated field
          return component.m_Service == this.m_Service;
        }
        DynamicBuffer<ServiceUpgradeBuilding> buffer;
        if (!entityManager.TryGetBuffer<ServiceUpgradeBuilding>(prefab, true, out buffer))
          return true;
        foreach (ServiceUpgradeBuilding serviceUpgradeBuilding in buffer)
        {
          // ISSUE: reference to a compiler-generated field
          if (entityManager.TryGetComponent<ServiceObjectData>(serviceUpgradeBuilding.m_Building, out component) && component.m_Service == this.m_Service)
            return true;
        }
        return false;
      }
    }

    public class ZoneTypeFilter : EditorAssetCategorySystem.IEditorAssetCategoryFilter
    {
      private Entity m_Zone;

      public ZoneTypeFilter(Entity zone) => this.m_Zone = zone;

      public bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem _)
      {
        SpawnableBuildingData component1;
        if (entityManager.TryGetComponent<SpawnableBuildingData>(prefab, out component1))
        {
          // ISSUE: reference to a compiler-generated field
          return component1.m_ZonePrefab == this.m_Zone;
        }
        PlaceholderBuildingData component2;
        // ISSUE: reference to a compiler-generated field
        return !entityManager.TryGetComponent<PlaceholderBuildingData>(prefab, out component2) || component2.m_ZonePrefab == this.m_Zone;
      }
    }

    public class PassengerCountFilter : EditorAssetCategorySystem.IEditorAssetCategoryFilter
    {
      public EditorAssetCategorySystem.PassengerCountFilter.FilterType m_Type;
      public int m_Count;

      public bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem _)
      {
        PersonalCarData component;
        // ISSUE: reference to a compiler-generated method
        return !entityManager.TryGetComponent<PersonalCarData>(prefab, out component) || this.Check(component);
      }

      private bool Check(PersonalCarData data)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        EditorAssetCategorySystem.PassengerCountFilter.FilterType type = this.m_Type;
        switch (type)
        {
          case EditorAssetCategorySystem.PassengerCountFilter.FilterType.Equals:
            // ISSUE: reference to a compiler-generated field
            return data.m_PassengerCapacity == this.m_Count;
          case EditorAssetCategorySystem.PassengerCountFilter.FilterType.NotEquals:
            // ISSUE: reference to a compiler-generated field
            return data.m_PassengerCapacity != this.m_Count;
          case EditorAssetCategorySystem.PassengerCountFilter.FilterType.MoreThan:
            // ISSUE: reference to a compiler-generated field
            return data.m_PassengerCapacity > this.m_Count;
          case EditorAssetCategorySystem.PassengerCountFilter.FilterType.LessThan:
            // ISSUE: reference to a compiler-generated field
            return data.m_PassengerCapacity < this.m_Count;
          default:
            return false;
        }
      }

      public enum FilterType
      {
        Equals,
        NotEquals,
        MoreThan,
        LessThan,
      }
    }

    public class PublicTransportTypeFilter : EditorAssetCategorySystem.IEditorAssetCategoryFilter
    {
      public TransportType m_TransportType;
      public PublicTransportPurpose m_Purpose;

      public bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem prefabSystem)
      {
        PublicTransportVehicleData component;
        if (!entityManager.TryGetComponent<PublicTransportVehicleData>(prefab, out component))
          return true;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransportType != TransportType.None && component.m_TransportType != this.m_TransportType)
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Purpose == (PublicTransportPurpose) 0 || (component.m_PurposeMask & this.m_Purpose) != 0;
      }
    }

    public class MaintenanceTypeFilter : EditorAssetCategorySystem.IEditorAssetCategoryFilter
    {
      public MaintenanceType m_Type;

      public bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem prefabSystem)
      {
        MaintenanceVehicleData component;
        // ISSUE: reference to a compiler-generated field
        return !entityManager.TryGetComponent<MaintenanceVehicleData>(prefab, out component) || (component.m_MaintenanceType & this.m_Type) != 0;
      }
    }

    public class ThemeFilter : EditorAssetCategorySystem.IEditorAssetCategoryFilter
    {
      public Entity m_Theme;
      public bool m_DefaultResult = true;

      public bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem prefabSystem)
      {
        PrefabBase prefab1;
        // ISSUE: reference to a compiler-generated method
        if (!prefabSystem.TryGetPrefab<PrefabBase>(prefab, out prefab1))
          return false;
        ThemeObject component = prefab1.GetComponent<ThemeObject>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        return (UnityEngine.Object) component == (UnityEngine.Object) null ? this.m_DefaultResult : prefabSystem.GetEntity((PrefabBase) component.m_Theme) == this.m_Theme;
      }
    }

    public class TrackTypeFilter : EditorAssetCategorySystem.IEditorAssetCategoryFilter
    {
      public TrackTypes m_TrackType;

      public bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem prefabSystem)
      {
        TrackPrefab prefab1;
        // ISSUE: reference to a compiler-generated method
        if (!prefabSystem.TryGetPrefab<TrackPrefab>(prefab, out prefab1))
          return false;
        // ISSUE: reference to a compiler-generated field
        return !((UnityEngine.Object) prefab1 != (UnityEngine.Object) null) || (prefab1.m_TrackType & this.m_TrackType) != 0;
      }
    }

    public class SignatureBuildingFilter : EditorAssetCategorySystem.IEditorAssetCategoryFilter
    {
      public AreaType m_AreaType;
      public bool m_Office;
      public Entity m_Theme;

      public bool Contains(Entity prefab, EntityManager entityManager, PrefabSystem prefabSystem)
      {
        PrefabBase prefab1;
        // ISSUE: reference to a compiler-generated method
        if (!prefabSystem.TryGetPrefab<PrefabBase>(prefab, out prefab1))
          return false;
        SignatureBuilding component1 = prefab1.GetComponent<SignatureBuilding>();
        if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
          return true;
        ThemeObject component2 = prefab1.GetComponent<ThemeObject>();
        Entity entity = Entity.Null;
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          entity = prefabSystem.GetEntity((PrefabBase) component2.m_Theme);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Theme == entity && component1.m_ZoneType.m_AreaType == this.m_AreaType && component1.m_ZoneType.m_Office == this.m_Office;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
