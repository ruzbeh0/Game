// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PrefabUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Entities;
using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.Agents;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PrefabUISystem : UISystemBase
  {
    private const string kGroup = "prefabs";
    private PrefabSystem m_PrefabSystem;
    private UniqueAssetTrackingSystem m_UniqueAssetTrackingSystem;
    private ImageSystem m_ImageSystem;
    private Entity m_RequirementEntity;
    private Entity m_TutorialRequirementEntity;
    private EntityQuery m_ThemeQuery;
    private EntityQuery m_ModifiedThemeQuery;
    private EntityQuery m_UnlockedPrefabQuery;
    private EntityQuery m_PollutionConfigQuery;
    private EntityQuery m_ManualUITagsConfigQuery;
    private GetterValueBinding<Dictionary<string, string>> m_UITagsBinding;
    private RawValueBinding m_ThemesBinding;
    private RawMapBinding<Entity> m_PrefabDetailsBinding;
    private int m_UnlockRequirementVersion;
    private int m_UITagVersion;
    private bool m_Initialized;

    public override GameMode gameMode => GameMode.Game;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueAssetTrackingSystem = this.World.GetOrCreateSystemManaged<UniqueAssetTrackingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RequirementEntity = this.EntityManager.CreateEntity();
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.AddBuffer<UnlockRequirement>(this.m_RequirementEntity);
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialRequirementEntity = this.EntityManager.CreateEntity();
      // ISSUE: reference to a compiler-generated field
      this.m_ThemeQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<UIObjectData>(), ComponentType.ReadOnly<ThemeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedThemeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<ThemeData>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIPollutionConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ManualUITagsConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<ManualUITagsConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ThemesBinding = new RawValueBinding("prefabs", "themes", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.m_ThemeQuery, Allocator.TempJob);
        writer.ArrayBegin(sortedObjects.Length);
        for (int index = 0; index < sortedObjects.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PrefabSystem prefabSystem = this.m_PrefabSystem;
          UIObjectInfo uiObjectInfo = sortedObjects[index];
          PrefabData prefabData = uiObjectInfo.prefabData;
          // ISSUE: reference to a compiler-generated method
          ThemePrefab prefab = prefabSystem.GetPrefab<ThemePrefab>(prefabData);
          writer.TypeBegin("prefabs.Theme");
          writer.PropertyName("entity");
          IJsonWriter writer1 = writer;
          uiObjectInfo = sortedObjects[index];
          Entity entity = uiObjectInfo.entity;
          writer1.Write(entity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        sortedObjects.Dispose();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PrefabDetailsBinding = new RawMapBinding<Entity>("prefabs", "prefabDetails", (Action<IJsonWriter, Entity>) ((writer, entity) =>
      {
        // ISSUE: reference to a compiler-generated field
        bool uniquePlaced = this.m_UniqueAssetTrackingSystem.IsPlacedUniqueAsset(entity);
        // ISSUE: reference to a compiler-generated method
        this.BindPrefabDetails(writer, entity, uniquePlaced);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UITagsBinding = new GetterValueBinding<Dictionary<string, string>>("prefabs", "manualUITags", (Func<Dictionary<string, string>>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ManualUITagsConfigQuery.IsEmptyIgnoreFilter)
          return (Dictionary<string, string>) null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ManualUITagsConfiguration prefab = this.m_PrefabSystem.GetPrefab<ManualUITagsConfiguration>(this.m_ManualUITagsConfigQuery.GetSingletonEntity());
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (FieldInfo field in typeof (ManualUITagsConfiguration).GetFields())
        {
          UITagPrefab uiTagPrefab = field.GetValue((object) prefab) as UITagPrefab;
          if ((UnityEngine.Object) uiTagPrefab != (UnityEngine.Object) null)
          {
            string key = field.Name[2].ToString().ToLower() + field.Name.Remove(0, 3);
            dictionary[key] = uiTagPrefab.uiTag;
          }
        }
        return dictionary;
      }), (IWriter<Dictionary<string, string>>) new DictionaryWriter<string, string>().Nullable<IDictionary<string, string>>())));
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      if (!this.Enabled)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Initialized = true;
      // ISSUE: reference to a compiler-generated method
      this.constructionCostBinders = PrefabUISystem.BuildDefaultConstructionCostBinders();
      // ISSUE: reference to a compiler-generated method
      this.propertyBinders = this.BuildDefaultPropertyBinders();
      // ISSUE: reference to a compiler-generated method
      this.effectBinders = PrefabUISystem.BuildDefaultEffectBinders();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ModifiedThemeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ThemesBinding.Update();
      }
      EntityManager entityManager = this.EntityManager;
      int componentOrderVersion1 = entityManager.GetComponentOrderVersion<UnlockRequirementData>();
      entityManager = this.EntityManager;
      int componentOrderVersion2 = entityManager.GetComponentOrderVersion<ManualUITagsConfigurationData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (PrefabUtils.HasUnlockedPrefab<UIObjectData>(this.EntityManager, this.m_UnlockedPrefabQuery) || this.m_UnlockRequirementVersion != componentOrderVersion1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabDetailsBinding.UpdateAll();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ManualUITagsConfigQuery.IsEmptyIgnoreFilter && componentOrderVersion2 != this.m_UITagVersion)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UITagsBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockRequirementVersion = componentOrderVersion1;
      // ISSUE: reference to a compiler-generated field
      this.m_UITagVersion = componentOrderVersion2;
    }

    private Dictionary<string, string> BindManualUITags()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ManualUITagsConfigQuery.IsEmptyIgnoreFilter)
        return (Dictionary<string, string>) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ManualUITagsConfiguration prefab = this.m_PrefabSystem.GetPrefab<ManualUITagsConfiguration>(this.m_ManualUITagsConfigQuery.GetSingletonEntity());
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (FieldInfo field in typeof (ManualUITagsConfiguration).GetFields())
      {
        UITagPrefab uiTagPrefab = field.GetValue((object) prefab) as UITagPrefab;
        if ((UnityEngine.Object) uiTagPrefab != (UnityEngine.Object) null)
        {
          string key = field.Name[2].ToString().ToLower() + field.Name.Remove(0, 3);
          dictionary[key] = uiTagPrefab.uiTag;
        }
      }
      return dictionary;
    }

    private void BindThemes(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.m_ThemeQuery, Allocator.TempJob);
      writer.ArrayBegin(sortedObjects.Length);
      for (int index = 0; index < sortedObjects.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PrefabSystem prefabSystem = this.m_PrefabSystem;
        UIObjectInfo uiObjectInfo = sortedObjects[index];
        PrefabData prefabData = uiObjectInfo.prefabData;
        // ISSUE: reference to a compiler-generated method
        ThemePrefab prefab = prefabSystem.GetPrefab<ThemePrefab>(prefabData);
        writer.TypeBegin("prefabs.Theme");
        writer.PropertyName("entity");
        IJsonWriter writer1 = writer;
        uiObjectInfo = sortedObjects[index];
        Entity entity = uiObjectInfo.entity;
        writer1.Write(entity);
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
      sortedObjects.Dispose();
    }

    private void BindPrefabDetails(IJsonWriter writer, Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      bool uniquePlaced = this.m_UniqueAssetTrackingSystem.IsPlacedUniqueAsset(entity);
      // ISSUE: reference to a compiler-generated method
      this.BindPrefabDetails(writer, entity, uniquePlaced);
    }

    public void BindPrefabDetails(IJsonWriter writer, Entity entity, bool uniquePlaced)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Initialized)
      {
        writer.WriteNull();
      }
      else
      {
        Entity entity1 = entity;
        EntityManager entityManager = this.EntityManager;
        DynamicBuffer<SubObject> buffer;
        if (entityManager.HasComponent<NetData>(entity1) && this.EntityManager.TryGetBuffer<SubObject>(entity1, true, out buffer))
        {
          for (int index = 0; index < buffer.Length; ++index)
          {
            SubObject subObject = buffer[index];
            if ((subObject.m_Flags & SubObjectFlags.MakeOwner) != (SubObjectFlags) 0)
            {
              entity1 = subObject.m_Prefab;
              break;
            }
          }
        }
        entityManager = this.EntityManager;
        PrefabData component1;
        PrefabData component2;
        if (entityManager.Exists(entity) && this.EntityManager.TryGetEnabledComponent<PrefabData>(entity1, out component1) && this.EntityManager.TryGetEnabledComponent<PrefabData>(entity, out component2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab1 = this.m_PrefabSystem.GetPrefab<PrefabBase>(component2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab2 = this.m_PrefabSystem.GetPrefab<PrefabBase>(component1);
          string titleId;
          string descriptionId;
          // ISSUE: reference to a compiler-generated method
          this.GetTitleAndDescription(entity1, out titleId, out descriptionId);
          string str = (string) null;
          ContentPrerequisite component3;
          DlcRequirement component4;
          if (prefab1.TryGet<ContentPrerequisite>(out component3) && component3.m_ContentPrerequisite.TryGet<DlcRequirement>(out component4))
            str = PlatformManager.instance.GetDlcName(component4.m_Dlc);
          writer.TypeBegin("prefabs.PrefabDetails");
          writer.PropertyName(nameof (entity));
          writer.Write(entity);
          writer.PropertyName("name");
          writer.Write(prefab1.name);
          writer.PropertyName("uiTag");
          writer.Write(prefab1.uiTag);
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          writer.Write(ImageSystem.GetThumbnail(prefab1) ?? this.m_ImageSystem.placeholderIcon);
          writer.PropertyName("dlc");
          if (str != null)
            writer.Write("Media/DLC/" + str + ".svg");
          else
            writer.WriteNull();
          writer.PropertyName("preview");
          SignatureBuilding component5;
          writer.Write(prefab2.TryGet<SignatureBuilding>(out component5) ? component5.m_UnlockEventImage : (string) null);
          writer.PropertyName("titleId");
          writer.Write(titleId);
          writer.PropertyName("descriptionId");
          writer.Write(descriptionId);
          writer.PropertyName("locked");
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
          writer.PropertyName(nameof (uniquePlaced));
          writer.Write(uniquePlaced);
          writer.PropertyName("constructionCost");
          // ISSUE: reference to a compiler-generated method
          this.BindConstructionCost(writer, entity1);
          writer.PropertyName("effects");
          // ISSUE: reference to a compiler-generated method
          this.BindEffects(writer, entity1);
          writer.PropertyName("properties");
          // ISSUE: reference to a compiler-generated method
          this.BindProperties(writer, entity1);
          writer.PropertyName("requirements");
          // ISSUE: reference to a compiler-generated method
          this.BindPrefabRequirements(writer, entity);
          writer.TypeEnd();
        }
        else
          writer.WriteNull();
      }
    }

    public void GetTitleAndDescription(
      Entity prefabEntity,
      out string titleId,
      [CanBeNull] out string descriptionId)
    {
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_PrefabSystem.TryGetPrefab<PrefabBase>(prefabEntity, out prefab))
      {
        switch (prefab)
        {
          case UIAssetMenuPrefab _:
          case ServicePrefab _:
            titleId = "Services.NAME[" + prefab.name + "]";
            descriptionId = "Services.DESCRIPTION[" + prefab.name + "]";
            break;
          case UIAssetCategoryPrefab _:
            titleId = "SubServices.NAME[" + prefab.name + "]";
            descriptionId = "Assets.SUB_SERVICE_DESCRIPTION[" + prefab.name + "]";
            break;
          default:
            if (prefab.Has<Game.Prefabs.ServiceUpgrade>())
            {
              titleId = "Assets.UPGRADE_NAME[" + prefab.name + "]";
              descriptionId = "Assets.UPGRADE_DESCRIPTION[" + prefab.name + "]";
              break;
            }
            titleId = "Assets.NAME[" + prefab.name + "]";
            descriptionId = "Assets.DESCRIPTION[" + prefab.name + "]";
            break;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        titleId = this.m_PrefabSystem.GetObsoleteID(prefabEntity).GetName();
        descriptionId = "Assets.MISSING_PREFAB_DESCRIPTION";
      }
    }

    public List<PrefabUISystem.IPrefabEffectBinder> effectBinders { get; private set; }

    private static List<PrefabUISystem.IPrefabEffectBinder> BuildDefaultEffectBinders()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      return new List<PrefabUISystem.IPrefabEffectBinder>()
      {
        (PrefabUISystem.IPrefabEffectBinder) new PrefabUISystem.CityModifierBinder(),
        (PrefabUISystem.IPrefabEffectBinder) new PrefabUISystem.LocalModifierBinder(),
        (PrefabUISystem.IPrefabEffectBinder) new PrefabUISystem.LeisureProviderBinder()
      };
    }

    public void BindEffects(IJsonWriter binder, Entity prefabEntity)
    {
      int size = 0;
      foreach (PrefabUISystem.IPrefabEffectBinder effectBinder in this.effectBinders)
      {
        // ISSUE: reference to a compiler-generated method
        if (effectBinder.Matches(this.EntityManager, prefabEntity))
          ++size;
      }
      binder.ArrayBegin(size);
      foreach (PrefabUISystem.IPrefabEffectBinder effectBinder in this.effectBinders)
      {
        // ISSUE: reference to a compiler-generated method
        if (effectBinder.Matches(this.EntityManager, prefabEntity))
        {
          // ISSUE: reference to a compiler-generated method
          effectBinder.Bind(binder, this.EntityManager, prefabEntity);
        }
      }
      binder.ArrayEnd();
    }

    public List<PrefabUISystem.IPrefabPropertyBinder> constructionCostBinders { get; private set; }

    public List<PrefabUISystem.IPrefabPropertyBinder> propertyBinders { get; private set; }

    private static List<PrefabUISystem.IPrefabPropertyBinder> BuildDefaultConstructionCostBinders()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      return new List<PrefabUISystem.IPrefabPropertyBinder>()
      {
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ConstructionCostBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentInt2PropertyBinder<PlaceableNetData>("Common.ASSET_CONSTRUCTION_COST", "moneyPerDistance", (Func<PlaceableNetData, int2>) (data => new int2(Convert.ToInt32(data.m_DefaultConstructionCost), Convert.ToInt32(data.m_DefaultConstructionCost) * 125))),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<ServiceUpgradeData>("Properties.CONSTRUCTION_COST", "money", (Func<ServiceUpgradeData, int>) (data => Convert.ToInt32(data.m_UpgradeCost)))
      };
    }

    public void BindConstructionCost(IJsonWriter binder, Entity prefabEntity)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_Initialized)
      {
        foreach (PrefabUISystem.IPrefabPropertyBinder constructionCostBinder in this.constructionCostBinders)
        {
          // ISSUE: reference to a compiler-generated method
          if (constructionCostBinder.Matches(this.EntityManager, prefabEntity))
          {
            // ISSUE: reference to a compiler-generated method
            constructionCostBinder.Bind(binder, this.EntityManager, prefabEntity);
            return;
          }
        }
      }
      binder.WriteNull();
    }

    private List<PrefabUISystem.IPrefabPropertyBinder> BuildDefaultPropertyBinders()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      return new List<PrefabUISystem.IPrefabPropertyBinder>()
      {
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.RequiredResourceBinder(this.World.GetOrCreateSystemManaged<ResourceSystem>()),
        (PrefabUISystem.IPrefabPropertyBinder) this.World.GetOrCreateSystemManaged<PrefabUISystem.UpkeepPropertyBinderSystem>(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<AssetStampData>("Properties.UPKEEP", "moneyPerMonth", (Func<AssetStampData, int>) (data => (int) data.m_UpKeepCost)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PlaceableNetData>("Properties.UPKEEP", "moneyPerDistancePerMonth", (Func<PlaceableNetData, int>) (data => Convert.ToInt32(data.m_DefaultUpkeepCost) * 125)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.PowerProductionBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<BatteryData>("Properties.BATTERY_CAPACITY", "energy", (Func<BatteryData, int>) (data => data.m_Capacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<BatteryData>("Properties.BATTERY_POWER_OUTPUT", "power", (Func<BatteryData, int>) (data => data.m_PowerOutput)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.TransformerCapacityBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.TransformerInputBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.TransformerOutputBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ElectricityConnectionBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.WaterConnectionBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<SewageOutletData>("Properties.SEWAGE_CAPACITY", "volumePerMonth", (Func<SewageOutletData, int>) (data => data.m_Capacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<SewageOutletData>("Properties.SEWAGE_PURIFICATION_RATE", "percentage", (Func<SewageOutletData, int>) (data => Mathf.RoundToInt(100f * data.m_Purification))),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<WaterPumpingStationData>("Properties.WATER_CAPACITY", "volumePerMonth", (Func<WaterPumpingStationData, int>) (data => data.m_Capacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<WaterPumpingStationData>("Properties.WATER_PURIFICATION_RATE", "percentage", (Func<WaterPumpingStationData, int>) (data => Mathf.RoundToInt(100f * data.m_Purification))),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<HospitalData>("Properties.PATIENT_CAPACITY", "integer", (Func<HospitalData, int>) (data => data.m_PatientCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<HospitalData>("Properties.AMBULANCE_COUNT", "integer", (Func<HospitalData, int>) (data => data.m_AmbulanceCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<HospitalData>("Properties.MEDICAL_HELICOPTER_COUNT", "integer", (Func<HospitalData, int>) (data => data.m_MedicalHelicopterCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<DeathcareFacilityData>("Properties.DECEASED_PROCESSING_CAPACITY", "integerPerMonth", (Func<DeathcareFacilityData, int>) (data => Mathf.CeilToInt(data.m_ProcessingRate))),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<DeathcareFacilityData>("Properties.DECEASED_STORAGE", "integer", (Func<DeathcareFacilityData, int>) (data => data.m_StorageCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<DeathcareFacilityData>("Properties.HEARSE_COUNT", "integer", (Func<DeathcareFacilityData, int>) (data => data.m_HearseCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<GarbageFacilityData>("Properties.GARBAGE_PROCESSING_CAPACITY", "weightPerMonth", (Func<GarbageFacilityData, int>) (data => data.m_ProcessingSpeed)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<GarbageFacilityData>("Properties.GARBAGE_STORAGE", "weight", (Func<GarbageFacilityData, int>) (data => data.m_GarbageCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<StorageAreaData>("Properties.GARBAGE_STORAGE", "weightPerCell", (Func<StorageAreaData, int>) (data => data.m_Capacity * 1000)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<GarbageFacilityData>("Properties.GARBAGE_TRUCK_COUNT", "integer", (Func<GarbageFacilityData, int>) (data => data.m_VehicleCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<FireStationData>("Properties.FIRE_ENGINE_COUNT", "integer", (Func<FireStationData, int>) (data => data.m_FireEngineCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<FireStationData>("Properties.FIRE_HELICOPTER_COUNT", "integer", (Func<FireStationData, int>) (data => data.m_FireHelicopterCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<EmergencyShelterData>("Properties.SHELTER_CAPACITY", "integer", (Func<EmergencyShelterData, int>) (data => data.m_ShelterCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<EmergencyShelterData>("Properties.EVACUATION_BUS_COUNT", "integer", (Func<EmergencyShelterData, int>) (data => data.m_VehicleCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.JailCapacityBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PoliceStationData>("Properties.PATROL_CAR_COUNT", "integer", (Func<PoliceStationData, int>) (data => data.m_PatrolCarCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PoliceStationData>("Properties.POLICE_HELICOPTER_COUNT", "integer", (Func<PoliceStationData, int>) (data => data.m_PoliceHelicopterCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PrisonData>("Properties.PRISON_VAN_COUNT", "integer", (Func<PrisonData, int>) (data => data.m_PrisonVanCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<SchoolData>("Properties.STUDENT_CAPACITY", "integer", (Func<SchoolData, int>) (data => data.m_StudentCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<TransportDepotData>("Properties.TRANSPORT_VEHICLE_COUNT", "integer", (Func<TransportDepotData, int>) (data => data.m_VehicleCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.TransportStopBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<MaintenanceDepotData>("Properties.MAINTENANCE_VEHICLES", "integer", (Func<MaintenanceDepotData, int>) (data => data.m_VehicleCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PostFacilityData>("Properties.MAIL_SORTING_RATE", "integerPerMonth", (Func<PostFacilityData, int>) (data => data.m_SortingRate)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PostFacilityData>("Properties.MAIL_STORAGE_CAPACITY", "integer", (Func<PostFacilityData, int>) (data => data.m_MailCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<MailBoxData>("Properties.MAIL_BOX_CAPACITY", "integer", (Func<MailBoxData, int>) (data => data.m_MailCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PostFacilityData>("Properties.POST_VAN_COUNT", "integer", (Func<PostFacilityData, int>) (data => data.m_PostVanCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PostFacilityData>("Properties.POST_TRUCK_COUNT", "integer", (Func<PostFacilityData, int>) (data => data.m_PostTruckCapacity)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<TelecomFacilityData>("Properties.NETWORK_RANGE", "length", (Func<TelecomFacilityData, int>) (data => Mathf.CeilToInt(data.m_Range))),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<TelecomFacilityData>("Properties.NETWORK_CAPACITY", "dataRate", (Func<TelecomFacilityData, int>) (data => Mathf.CeilToInt(data.m_NetworkCapacity))),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<AttractionData>("Properties.ATTRACTIVENESS", "integer", (Func<AttractionData, int>) (data => data.m_Attractiveness)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.UpkeepModifierBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.StorageLimitBinder(),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.PollutionBinder(this.m_PrefabSystem.GetSingletonPrefab<UIPollutionConfigurationPrefab>(this.m_PollutionConfigQuery)),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PollutionModifierData>("SelectedInfoPanel.POLLUTION_LEVELS_GROUND", "percentage", (Func<PollutionModifierData, int>) (data => Mathf.RoundToInt(data.m_GroundPollutionMultiplier * 100f)), signed: true, valueIcon: "Media/Game/Icons/GroundPollution.svg"),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PollutionModifierData>("SelectedInfoPanel.POLLUTION_LEVELS_AIR", "percentage", (Func<PollutionModifierData, int>) (data => Mathf.RoundToInt(data.m_AirPollutionMultiplier * 100f)), signed: true, valueIcon: "Media/Game/Icons/AirPollution.svg"),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<PollutionModifierData>("SelectedInfoPanel.POLLUTION_LEVELS_NOISE", "percentage", (Func<PollutionModifierData, int>) (data => Mathf.RoundToInt(data.m_NoisePollutionMultiplier * 100f)), signed: true, valueIcon: "Media/Game/Icons/NoisePollution.svg"),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<ParkingFacilityData>("Properties.COMFORT", "integer", (Func<ParkingFacilityData, int>) (data => (int) math.round(100f * data.m_ComfortFactor))),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<TransportStopData>("Properties.COMFORT", "integer", (Func<TransportStopData, int>) (data => (int) math.round(100f * data.m_ComfortFactor))),
        (PrefabUISystem.IPrefabPropertyBinder) new PrefabUISystem.ComponentIntPropertyBinder<TransportStationData>("Properties.COMFORT", "integer", (Func<TransportStationData, int>) (data => (int) math.round(100f * data.m_ComfortFactor)))
      };
    }

    public void BindProperties(IJsonWriter binder, Entity prefabEntity)
    {
      int size = 0;
      foreach (PrefabUISystem.IPrefabPropertyBinder propertyBinder in this.propertyBinders)
      {
        // ISSUE: reference to a compiler-generated method
        if (propertyBinder.Matches(this.EntityManager, prefabEntity))
          ++size;
      }
      binder.ArrayBegin(size);
      foreach (PrefabUISystem.IPrefabPropertyBinder propertyBinder in this.propertyBinders)
      {
        // ISSUE: reference to a compiler-generated method
        if (propertyBinder.Matches(this.EntityManager, prefabEntity))
        {
          // ISSUE: reference to a compiler-generated method
          propertyBinder.Bind(binder, this.EntityManager, prefabEntity);
        }
      }
      binder.ArrayEnd();
    }

    public void BindPrefabRequirements(IJsonWriter writer, Entity prefabEntity)
    {
      if (this.EntityManager.HasComponent<UIGroupElement>(prefabEntity))
      {
        // ISSUE: reference to a compiler-generated method
        this.BindUIGroupRequirements(writer, prefabEntity);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.BindRequirements(writer, prefabEntity);
      }
    }

    private void BindUIGroupRequirements(IJsonWriter writer, Entity prefabEntity)
    {
      ForceUnlockRequirementData component;
      if (this.EntityManager.TryGetComponent<ForceUnlockRequirementData>(prefabEntity, out component))
      {
        // ISSUE: reference to a compiler-generated method
        this.BindRequirements(writer, component.m_Prefab);
      }
      else
      {
        NativeList<Entity> requirements = new NativeList<Entity>(4, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<UnlockRequirement> list = new NativeList<UnlockRequirement>(4, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        this.FindLowestRequirements(prefabEntity, requirements);
        if (requirements.Length > 1)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<UnlockRequirement> buffer1 = this.EntityManager.GetBuffer<UnlockRequirement>(this.m_RequirementEntity);
          for (int index1 = 0; index1 < requirements.Length; ++index1)
          {
            Entity entity = requirements[index1];
            DynamicBuffer<UnlockRequirement> buffer2;
            if (this.EntityManager.TryGetBuffer<UnlockRequirement>(entity, true, out buffer2))
            {
              for (int index2 = 0; index2 < buffer2.Length; ++index2)
              {
                if (buffer2[index2].m_Prefab != entity && !list.Contains<UnlockRequirement, UnlockRequirement>(buffer2[index2]))
                {
                  ref NativeList<UnlockRequirement> local1 = ref list;
                  UnlockRequirement unlockRequirement = buffer2[index2];
                  ref UnlockRequirement local2 = ref unlockRequirement;
                  local1.Add(in local2);
                  ref DynamicBuffer<UnlockRequirement> local3 = ref buffer1;
                  unlockRequirement = new UnlockRequirement();
                  unlockRequirement.m_Prefab = buffer2[index2].m_Prefab;
                  unlockRequirement.m_Flags = UnlockFlags.RequireAny;
                  UnlockRequirement elem = unlockRequirement;
                  local3.Add(elem);
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.BindRequirements(writer, this.m_RequirementEntity);
          buffer1.Clear();
        }
        else if (requirements.Length > 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.BindRequirements(writer, requirements[0]);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.BindRequirements(writer, this.m_RequirementEntity);
        }
        requirements.Dispose();
        list.Dispose();
      }
    }

    private int FindLowestRequirements(
      Entity prefabEntity,
      NativeList<Entity> requirements,
      int score = -1)
    {
      NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.EntityManager.GetBuffer<UIGroupElement>(prefabEntity, true), Allocator.TempJob);
      NativeParallelHashMap<Entity, UnlockFlags> devTreeNodes = new NativeParallelHashMap<Entity, UnlockFlags>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelHashMap<Entity, UnlockFlags> unlockRequirements = new NativeParallelHashMap<Entity, UnlockFlags>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      foreach (UIObjectInfo uiObjectInfo in sortedObjects)
      {
        if (this.EntityManager.HasComponent<UIGroupElement>(uiObjectInfo.entity))
        {
          // ISSUE: reference to a compiler-generated method
          int lowestRequirements = this.FindLowestRequirements(uiObjectInfo.entity, requirements, score);
          if (requirements.Length > 0 && score == -1 || lowestRequirements < score)
            score = lowestRequirements;
        }
        else
        {
          Entity milestone;
          // ISSUE: reference to a compiler-generated method
          this.GetRequirements(uiObjectInfo.entity, out milestone, devTreeNodes, unlockRequirements);
          int num1 = 0;
          if (milestone != Entity.Null)
          {
            MilestoneData componentData = this.EntityManager.GetComponentData<MilestoneData>(milestone);
            num1 += componentData.m_Index * 10000;
          }
          foreach (KeyValue<Entity, UnlockFlags> keyValue in devTreeNodes)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            DevTreeNodePrefab prefab = this.m_PrefabSystem.GetPrefab<DevTreeNodePrefab>(keyValue.Key);
            num1 += prefab.m_HorizontalPosition * 100;
          }
          int num2 = num1 + unlockRequirements.Count() * 10;
          foreach (KeyValue<Entity, UnlockFlags> keyValue in unlockRequirements)
          {
            if (this.EntityManager.HasComponent<UnlockRequirementData>(keyValue.Key))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              switch (this.m_PrefabSystem.GetPrefab<UnlockRequirementPrefab>(keyValue.Key))
              {
                case ZoneBuiltRequirementPrefab requirementPrefab1:
                  num2 += requirementPrefab1.m_MinimumLevel * requirementPrefab1.m_MinimumCount + requirementPrefab1.m_MinimumSquares / 100;
                  continue;
                case CitizenRequirementPrefab requirementPrefab2:
                  num2 += requirementPrefab2.m_MinimumPopulation / 10 + requirementPrefab2.m_MinimumHappiness * 100;
                  continue;
                case ProcessingRequirementPrefab requirementPrefab3:
                  num2 += requirementPrefab3.m_MinimumProducedAmount / 10;
                  continue;
                default:
                  num2 += 100;
                  continue;
              }
            }
            else
              num2 += 10;
          }
          if (uiObjectInfo.entity != Entity.Null && !requirements.Contains<Entity, Entity>(uiObjectInfo.entity) && score == -1 || num2 <= score)
          {
            if (num2 < score)
              requirements.Clear();
            requirements.Add(uiObjectInfo.entity);
            score = num2;
          }
        }
      }
      sortedObjects.Dispose();
      devTreeNodes.Dispose();
      unlockRequirements.Dispose();
      return score;
    }

    private void BindRequirements(IJsonWriter writer, Entity prefabEntity)
    {
      NativeParallelHashMap<Entity, UnlockFlags> devTreeNodes = new NativeParallelHashMap<Entity, UnlockFlags>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelHashMap<Entity, UnlockFlags> unlockRequirements = new NativeParallelHashMap<Entity, UnlockFlags>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      writer.TypeBegin("Game.UI.Game.PrefabUISystem.UnlockingRequirements");
      Entity milestone;
      // ISSUE: reference to a compiler-generated method
      this.GetRequirements(prefabEntity, out milestone, devTreeNodes, unlockRequirements);
      // ISSUE: reference to a compiler-generated method
      this.VerifyRequirements(devTreeNodes, unlockRequirements);
      // ISSUE: reference to a compiler-generated method
      this.BindRequirements(writer, UnlockFlags.RequireAll, milestone, devTreeNodes, unlockRequirements);
      // ISSUE: reference to a compiler-generated method
      this.BindRequirements(writer, UnlockFlags.RequireAny, Entity.Null, devTreeNodes, unlockRequirements);
      writer.TypeEnd();
      devTreeNodes.Dispose();
      unlockRequirements.Dispose();
    }

    private void VerifyRequirements(
      NativeParallelHashMap<Entity, UnlockFlags> devTreeNodes,
      NativeParallelHashMap<Entity, UnlockFlags> unlockRequirements)
    {
      Entity key1 = Entity.Null;
      Entity key2 = Entity.Null;
      int num = 0;
      foreach (KeyValue<Entity, UnlockFlags> devTreeNode in devTreeNodes)
      {
        if ((devTreeNode.Value & UnlockFlags.RequireAny) != (UnlockFlags) 0)
        {
          key1 = devTreeNode.Key;
          ++num;
        }
      }
      foreach (KeyValue<Entity, UnlockFlags> unlockRequirement in unlockRequirements)
      {
        if ((unlockRequirement.Value & UnlockFlags.RequireAny) != (UnlockFlags) 0)
        {
          key2 = unlockRequirement.Key;
          ++num;
        }
      }
      if (num != 1)
        return;
      if (key1 != Entity.Null)
        devTreeNodes[key1] = UnlockFlags.RequireAll;
      if (!(key2 != Entity.Null))
        return;
      unlockRequirements[key2] = UnlockFlags.RequireAll;
    }

    private void BindRequirements(
      IJsonWriter writer,
      UnlockFlags flag,
      Entity milestone,
      NativeParallelHashMap<Entity, UnlockFlags> devTreeNodes,
      NativeParallelHashMap<Entity, UnlockFlags> unlockRequirements)
    {
      NativeList<Entity> nativeList1 = new NativeList<Entity>(2, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> nativeList2 = new NativeList<Entity>(4, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      foreach (KeyValue<Entity, UnlockFlags> devTreeNode in devTreeNodes)
      {
        if ((devTreeNode.Value & flag) != (UnlockFlags) 0)
          nativeList1.Add(devTreeNode.Key);
      }
      for (int index = 0; index < nativeList1.Length; ++index)
        devTreeNodes.Remove(nativeList1[index]);
      foreach (KeyValue<Entity, UnlockFlags> unlockRequirement in unlockRequirements)
      {
        if ((unlockRequirement.Value & flag) != (UnlockFlags) 0)
          nativeList2.Add(unlockRequirement.Key);
      }
      for (int index = 0; index < nativeList2.Length; ++index)
        unlockRequirements.Remove(nativeList2[index]);
      writer.PropertyName(flag == UnlockFlags.RequireAll ? "requireAll" : "requireAny");
      writer.ArrayBegin((milestone != Entity.Null ? 1 : 0) + nativeList1.Length + nativeList2.Length);
      if (milestone != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.BindMilestoneRequirement(writer, milestone);
      }
      for (int index = 0; index < nativeList1.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        this.BindDevTreeNodeRequirement(writer, nativeList1[index]);
      }
      for (int index = 0; index < nativeList2.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        this.BindUnlockRequirement(writer, nativeList2[index]);
      }
      writer.ArrayEnd();
      nativeList1.Dispose();
      nativeList2.Dispose();
    }

    private void GetRequirements(
      Entity prefabEntity,
      out Entity milestone,
      NativeParallelHashMap<Entity, UnlockFlags> devTreeNodes,
      NativeParallelHashMap<Entity, UnlockFlags> unlockRequirements)
    {
      NativeParallelHashMap<Entity, UnlockFlags> requiredPrefabs = new NativeParallelHashMap<Entity, UnlockFlags>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      ProgressionUtils.CollectSubRequirements(this.EntityManager, prefabEntity, requiredPrefabs);
      milestone = Entity.Null;
      int num = -1;
      devTreeNodes.Clear();
      unlockRequirements.Clear();
      foreach (KeyValue<Entity, UnlockFlags> keyValue in requiredPrefabs)
      {
        MilestoneData component;
        if (this.EntityManager.TryGetComponent<MilestoneData>(keyValue.Key, out component) && component.m_Index > num)
        {
          milestone = keyValue.Key;
          num = component.m_Index;
        }
        if (this.EntityManager.HasComponent<DevTreeNodeData>(keyValue.Key))
        {
          if (devTreeNodes.ContainsKey(keyValue.Key))
            devTreeNodes[keyValue.Key] |= keyValue.Value;
          else
            devTreeNodes.Add(keyValue.Key, keyValue.Value);
        }
        if (this.EntityManager.HasComponent<UnlockRequirementData>(keyValue.Key))
        {
          if (unlockRequirements.ContainsKey(keyValue.Key))
            unlockRequirements[keyValue.Key] |= keyValue.Value;
          else
            unlockRequirements.Add(keyValue.Key, keyValue.Value);
        }
        if ((this.EntityManager.HasComponent<TutorialData>(keyValue.Key) || this.EntityManager.HasComponent<TutorialPhaseData>(keyValue.Key) || this.EntityManager.HasComponent<TutorialTriggerData>(keyValue.Key) || this.EntityManager.HasComponent<TutorialListData>(keyValue.Key)) && this.EntityManager.HasEnabledComponent<Locked>(keyValue.Key))
        {
          // ISSUE: reference to a compiler-generated field
          if (unlockRequirements.ContainsKey(this.m_TutorialRequirementEntity))
          {
            // ISSUE: reference to a compiler-generated field
            unlockRequirements[this.m_TutorialRequirementEntity] |= keyValue.Value;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            unlockRequirements.Add(this.m_TutorialRequirementEntity, keyValue.Value);
          }
        }
      }
      requiredPrefabs.Dispose();
    }

    private void BindMilestoneRequirement(IJsonWriter binder, Entity entity)
    {
      MilestoneData componentData = this.EntityManager.GetComponentData<MilestoneData>(entity);
      bool flag = this.EntityManager.HasEnabledComponent<Locked>(entity);
      binder.TypeBegin("prefabs.MilestoneRequirement");
      binder.PropertyName(nameof (entity));
      binder.Write(entity);
      binder.PropertyName("index");
      binder.Write(componentData.m_Index);
      binder.PropertyName("locked");
      binder.Write(flag);
      binder.TypeEnd();
    }

    private void BindDevTreeNodeRequirement(IJsonWriter binder, Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      DevTreeNodePrefab prefab = this.m_PrefabSystem.GetPrefab<DevTreeNodePrefab>(entity);
      bool flag = this.EntityManager.HasEnabledComponent<Locked>(entity);
      binder.TypeBegin("prefabs.DevTreeNodeRequirement");
      binder.PropertyName(nameof (entity));
      binder.Write(entity);
      binder.PropertyName("name");
      binder.Write(prefab.name);
      binder.PropertyName("locked");
      binder.Write(flag);
      binder.TypeEnd();
    }

    private void BindUnlockRequirement(IJsonWriter binder, Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      if (entity == this.m_TutorialRequirementEntity)
      {
        // ISSUE: reference to a compiler-generated method
        this.BindTutorialRequirement(binder, entity);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        switch (this.m_PrefabSystem.GetPrefab<PrefabBase>(entity))
        {
          case StrictObjectBuiltRequirementPrefab prefab1:
            // ISSUE: reference to a compiler-generated method
            this.BindObjectBuiltRequirement(binder, entity, prefab1);
            break;
          case ZoneBuiltRequirementPrefab prefab2:
            // ISSUE: reference to a compiler-generated method
            this.BindZoneBuiltRequirement(binder, entity, prefab2);
            break;
          case CitizenRequirementPrefab cr:
            // ISSUE: reference to a compiler-generated method
            this.BindCitizenRequirement(binder, entity, cr);
            break;
          case ProcessingRequirementPrefab prefab3:
            // ISSUE: reference to a compiler-generated method
            this.BindProcessingRequirement(binder, entity, prefab3);
            break;
          case ObjectBuiltRequirementPrefab prefab4:
            // ISSUE: reference to a compiler-generated method
            this.BindOnBuildRequirement(binder, entity, prefab4);
            break;
          case UnlockRequirementPrefab prefab5:
            // ISSUE: reference to a compiler-generated method
            this.BindUnknownUnlockRequirement(binder, entity, prefab5);
            break;
        }
      }
    }

    private void BindTutorialRequirement(IJsonWriter binder, Entity entity)
    {
      binder.TypeBegin("prefabs.TutorialRequirement");
      binder.PropertyName(nameof (entity));
      binder.Write(entity);
      binder.PropertyName("locked");
      binder.Write(true);
      binder.TypeEnd();
    }

    private void BindObjectBuiltRequirement(
      IJsonWriter binder,
      Entity entity,
      StrictObjectBuiltRequirementPrefab prefab)
    {
      binder.TypeBegin("prefabs.StrictObjectBuiltRequirement");
      // ISSUE: reference to a compiler-generated method
      this.BindUnlockRequirementProperties(binder, entity, (UnlockRequirementPrefab) prefab);
      binder.PropertyName("icon");
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      binder.Write(ImageSystem.GetThumbnail(prefab.m_Requirement) ?? this.m_ImageSystem.placeholderIcon);
      binder.PropertyName("requirement");
      binder.Write(prefab.m_Requirement.name);
      binder.PropertyName("minimumCount");
      binder.Write(prefab.m_MinimumCount);
      binder.TypeEnd();
    }

    private void BindZoneBuiltRequirement(
      IJsonWriter binder,
      Entity entity,
      ZoneBuiltRequirementPrefab prefab)
    {
      binder.TypeBegin("prefabs.ZoneBuiltRequirement");
      // ISSUE: reference to a compiler-generated method
      this.BindUnlockRequirementProperties(binder, entity, (UnlockRequirementPrefab) prefab);
      binder.PropertyName("icon");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      bool flag = this.m_PrefabSystem.HasComponent<UIObjectData>((PrefabBase) prefab.m_RequiredZone);
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      binder.Write(ImageSystem.GetIcon(flag ? (PrefabBase) prefab.m_RequiredZone ?? (PrefabBase) prefab : (PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
      binder.PropertyName("requiredTheme");
      binder.Write(prefab.m_RequiredTheme?.name);
      binder.PropertyName("requiredZone");
      binder.Write(prefab.m_RequiredZone?.name);
      binder.PropertyName("requiredType");
      binder.Write((int) prefab.m_RequiredType);
      binder.PropertyName("minimumSquares");
      binder.Write(prefab.m_MinimumSquares);
      binder.PropertyName("minimumCount");
      binder.Write(prefab.m_MinimumCount);
      binder.PropertyName("minimumLevel");
      binder.Write(prefab.m_MinimumLevel);
      binder.TypeEnd();
    }

    private void BindCitizenRequirement(
      IJsonWriter binder,
      Entity entity,
      CitizenRequirementPrefab cr)
    {
      binder.TypeBegin("prefabs.CitizenRequirement");
      // ISSUE: reference to a compiler-generated method
      this.BindUnlockRequirementProperties(binder, entity, (UnlockRequirementPrefab) cr);
      binder.PropertyName("minimumPopulation");
      binder.Write(cr.m_MinimumPopulation);
      binder.PropertyName("minimumHappiness");
      binder.Write(cr.m_MinimumHappiness);
      binder.TypeEnd();
    }

    private void BindProcessingRequirement(
      IJsonWriter binder,
      Entity entity,
      ProcessingRequirementPrefab prefab)
    {
      binder.TypeBegin("prefabs.ProcessingRequirement");
      // ISSUE: reference to a compiler-generated method
      this.BindUnlockRequirementProperties(binder, entity, (UnlockRequirementPrefab) prefab);
      binder.PropertyName("icon");
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      binder.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
      binder.PropertyName("resourceType");
      binder.Write(Enum.GetName(typeof (ResourceInEditor), (object) prefab.m_ResourceType));
      binder.PropertyName("minimumProducedAmount");
      binder.Write(prefab.m_MinimumProducedAmount);
      binder.TypeEnd();
    }

    private void BindOnBuildRequirement(
      IJsonWriter binder,
      Entity entity,
      ObjectBuiltRequirementPrefab prefab)
    {
      binder.TypeBegin("prefabs.ObjectBuiltRequirement");
      // ISSUE: reference to a compiler-generated method
      this.BindUnlockRequirementProperties(binder, entity, (UnlockRequirementPrefab) prefab);
      binder.PropertyName("name");
      binder.Write(prefab.name);
      binder.PropertyName("minimumCount");
      binder.Write(prefab.m_MinimumCount);
      binder.TypeEnd();
    }

    private void BindUnknownUnlockRequirement(
      IJsonWriter binder,
      Entity entity,
      UnlockRequirementPrefab prefab)
    {
      binder.TypeBegin("prefabs.UnlockRequirement");
      // ISSUE: reference to a compiler-generated method
      this.BindUnlockRequirementProperties(binder, entity, prefab);
      binder.TypeEnd();
    }

    private void BindUnlockRequirementProperties(
      IJsonWriter binder,
      Entity entity,
      UnlockRequirementPrefab prefab)
    {
      UnlockRequirementData componentData = this.EntityManager.GetComponentData<UnlockRequirementData>(entity);
      bool flag = this.EntityManager.HasEnabledComponent<Locked>(entity);
      binder.PropertyName(nameof (entity));
      binder.Write(entity);
      binder.PropertyName("labelId");
      binder.Write(!string.IsNullOrEmpty(prefab.m_LabelID) ? prefab.m_LabelID : (string) null);
      binder.PropertyName("progress");
      binder.Write(componentData.m_Progress);
      binder.PropertyName("locked");
      binder.Write(flag);
    }

    [Preserve]
    public PrefabUISystem()
    {
    }

    public interface IPrefabEffectBinder
    {
      bool Matches(EntityManager entityManager, Entity entity);

      void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity);
    }

    public class CityModifierBinder : PrefabUISystem.IPrefabEffectBinder
    {
      public bool Matches(EntityManager entityManager, Entity entity)
      {
        return entityManager.HasComponent<CityModifierData>(entity);
      }

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        DynamicBuffer<CityModifierData> buffer = entityManager.GetBuffer<CityModifierData>(entity, true);
        // ISSUE: reference to a compiler-generated method
        PrefabUISystem.CityModifierBinder.Bind<DynamicBuffer<CityModifierData>>(binder, buffer);
      }

      public static void Bind<T>(IJsonWriter binder, T cityModifiers) where T : INativeList<CityModifierData>
      {
        int size = 0;
        for (int index = 0; index < cityModifiers.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (PrefabUISystem.CityModifierBinder.IsValidModifierType(cityModifiers[index].m_Type))
            ++size;
        }
        binder.TypeBegin("prefabs.CityModifierEffect");
        binder.PropertyName("modifiers");
        binder.ArrayBegin(size);
        for (int index = 0; index < cityModifiers.Length; ++index)
        {
          CityModifierData cityModifier = cityModifiers[index];
          // ISSUE: reference to a compiler-generated method
          if (PrefabUISystem.CityModifierBinder.IsValidModifierType(cityModifier.m_Type))
          {
            binder.TypeBegin("prefabs.CityModifier");
            binder.PropertyName("type");
            binder.Write(Enum.GetName(typeof (CityModifierType), (object) cityModifier.m_Type));
            binder.PropertyName("delta");
            binder.Write(ModifierUIUtils.GetModifierDelta(cityModifier.m_Mode, cityModifier.m_Range.max));
            binder.PropertyName("unit");
            // ISSUE: reference to a compiler-generated method
            binder.Write(PrefabUISystem.CityModifierBinder.GetModifierUnit(cityModifier));
            binder.TypeEnd();
          }
        }
        binder.ArrayEnd();
        binder.TypeEnd();
      }

      public static string GetModifierUnit(CityModifierData data)
      {
        return data.m_Mode == ModifierValueMode.Absolute && data.m_Type == CityModifierType.CrimeAccumulation || data.m_Type == CityModifierType.LoanInterest ? "percentage" : ModifierUIUtils.GetModifierUnit(data.m_Mode);
      }

      private static bool IsValidModifierType(CityModifierType type)
      {
        return type != CityModifierType.CriminalMonitorProbability;
      }
    }

    public class LocalModifierBinder : PrefabUISystem.IPrefabEffectBinder
    {
      public bool Matches(EntityManager entityManager, Entity entity)
      {
        return entityManager.HasComponent<LocalModifierData>(entity);
      }

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        DynamicBuffer<LocalModifierData> buffer = entityManager.GetBuffer<LocalModifierData>(entity, true);
        // ISSUE: reference to a compiler-generated method
        PrefabUISystem.LocalModifierBinder.Bind<DynamicBuffer<LocalModifierData>>(binder, buffer);
      }

      public static void Bind<T>(IJsonWriter binder, T localModifiers) where T : INativeList<LocalModifierData>
      {
        binder.TypeBegin("prefabs.LocalModifierEffect");
        binder.PropertyName("modifiers");
        binder.ArrayBegin(localModifiers.Length);
        for (int index = 0; index < localModifiers.Length; ++index)
        {
          LocalModifierData localModifier = localModifiers[index];
          binder.TypeBegin("prefabs.LocalModifier");
          binder.PropertyName("type");
          binder.Write(Enum.GetName(typeof (LocalModifierType), (object) localModifier.m_Type));
          binder.PropertyName("delta");
          binder.Write(ModifierUIUtils.GetModifierDelta(localModifier.m_Mode, localModifier.m_Delta.max));
          binder.PropertyName("unit");
          binder.Write(ModifierUIUtils.GetModifierUnit(localModifier.m_Mode));
          binder.PropertyName("radius");
          binder.Write(localModifier.m_Radius.max);
          binder.TypeEnd();
        }
        binder.ArrayEnd();
        binder.TypeEnd();
      }
    }

    public class LeisureProviderBinder : PrefabUISystem.IPrefabEffectBinder
    {
      public bool Matches(EntityManager entityManager, Entity entity)
      {
        LeisureProviderData component;
        return entityManager.TryGetComponent<LeisureProviderData>(entity, out component) && component.m_Efficiency > 0;
      }

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        LeisureProviderData componentData = entityManager.GetComponentData<LeisureProviderData>(entity);
        // ISSUE: reference to a compiler-generated method
        PrefabUISystem.LeisureProviderBinder.Bind<NativeList<LeisureProviderData>>(binder, new NativeList<LeisureProviderData>(1, (AllocatorManager.AllocatorHandle) Allocator.Temp)
        {
          ref componentData
        });
      }

      public static void Bind<T>(IJsonWriter binder, T providers) where T : INativeList<LeisureProviderData>
      {
        binder.TypeBegin("prefabs.LeisureProviderEffect");
        binder.PropertyName(nameof (providers));
        binder.ArrayBegin(providers.Length);
        for (int index = 0; index < providers.Length; ++index)
        {
          LeisureProviderData provider = providers[index];
          binder.TypeBegin("prefabs.LeisureProvider");
          binder.PropertyName("type");
          binder.Write(Enum.GetName(typeof (LeisureType), (object) provider.m_LeisureType));
          binder.PropertyName("efficiency");
          binder.Write(provider.m_Efficiency);
          binder.TypeEnd();
        }
        binder.ArrayEnd();
        binder.TypeEnd();
      }
    }

    public interface IPrefabPropertyBinder
    {
      bool Matches(EntityManager entityManager, Entity entity);

      void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity);
    }

    public abstract class IntPropertyBinder : PrefabUISystem.IPrefabPropertyBinder
    {
      public readonly string m_LabelId;
      public readonly string m_Unit;
      public readonly bool m_Signed;
      public readonly string m_Icon;
      public readonly string m_ValueIcon;

      protected IntPropertyBinder(
        string labelId,
        string unit,
        bool signed = false,
        string icon = null,
        string valueIcon = null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LabelId = labelId;
        // ISSUE: reference to a compiler-generated field
        this.m_Unit = unit;
        // ISSUE: reference to a compiler-generated field
        this.m_Signed = signed;
        // ISSUE: reference to a compiler-generated field
        this.m_Icon = icon;
        // ISSUE: reference to a compiler-generated field
        this.m_ValueIcon = valueIcon;
      }

      public abstract bool Matches(EntityManager entityManager, Entity entity);

      public abstract int GetValue(EntityManager entityManager, Entity entity);

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        binder.Write<IntProperty>(new IntProperty()
        {
          labelId = this.m_LabelId,
          unit = this.m_Unit,
          value = this.GetValue(entityManager, entity),
          signed = this.m_Signed,
          icon = this.m_Icon,
          valueIcon = this.m_ValueIcon
        });
      }
    }

    public abstract class IntRangePropertyBinder : PrefabUISystem.IPrefabPropertyBinder
    {
      public readonly string m_LabelId;
      public readonly string m_Unit;
      public readonly bool m_Signed;
      public readonly string m_Icon;
      public readonly string m_ValueIcon;

      protected IntRangePropertyBinder(
        string labelId,
        string unit,
        bool signed = false,
        string icon = null,
        string valueIcon = null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LabelId = labelId;
        // ISSUE: reference to a compiler-generated field
        this.m_Unit = unit;
        // ISSUE: reference to a compiler-generated field
        this.m_Signed = signed;
        // ISSUE: reference to a compiler-generated field
        this.m_Icon = icon;
        // ISSUE: reference to a compiler-generated field
        this.m_ValueIcon = valueIcon;
      }

      public abstract bool Matches(EntityManager entityManager, Entity entity);

      public abstract int GetMinValue(EntityManager entityManager, Entity entity);

      public abstract int GetMaxValue(EntityManager entityManager, Entity entity);

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        binder.Write<IntRangeProperty>(new IntRangeProperty()
        {
          labelId = this.m_LabelId,
          unit = this.m_Unit,
          minValue = this.GetMinValue(entityManager, entity),
          maxValue = this.GetMaxValue(entityManager, entity),
          signed = this.m_Signed,
          icon = this.m_Icon,
          valueIcon = this.m_ValueIcon
        });
      }
    }

    public abstract class Int2PropertyBinder : PrefabUISystem.IPrefabPropertyBinder
    {
      public readonly string m_LabelId;
      public readonly string m_Unit;
      public readonly bool m_Signed;
      public readonly string m_Icon;
      public readonly string m_ValueIcon;

      protected Int2PropertyBinder(
        string labelId,
        string unit,
        bool signed = false,
        string icon = null,
        string valueIcon = null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LabelId = labelId;
        // ISSUE: reference to a compiler-generated field
        this.m_Unit = unit;
        // ISSUE: reference to a compiler-generated field
        this.m_Signed = signed;
        // ISSUE: reference to a compiler-generated field
        this.m_Icon = icon;
        // ISSUE: reference to a compiler-generated field
        this.m_ValueIcon = valueIcon;
      }

      public abstract bool Matches(EntityManager entityManager, Entity entity);

      public abstract int2 GetValue(EntityManager entityManager, Entity entity);

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        binder.Write<Int2Property>(new Int2Property()
        {
          labelId = this.m_LabelId,
          unit = this.m_Unit,
          value = this.GetValue(entityManager, entity),
          signed = this.m_Signed,
          icon = this.m_Icon,
          valueIcon = this.m_ValueIcon
        });
      }
    }

    public class ComponentIntPropertyBinder<T> : PrefabUISystem.IntPropertyBinder where T : unmanaged, IComponentData
    {
      private readonly Func<T, int> m_Getter;
      private readonly bool m_OmitZero;

      public ComponentIntPropertyBinder(
        string labelId,
        string unit,
        Func<T, int> getter,
        bool omitZero = true,
        bool signed = false,
        string icon = null,
        string valueIcon = null)
        : base(labelId, unit, signed, icon, valueIcon)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Getter = getter;
        // ISSUE: reference to a compiler-generated field
        this.m_OmitZero = omitZero;
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        T component;
        if (!entityManager.TryGetComponent<T>(entity, out component))
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return !this.m_OmitZero || this.m_Getter(component) != 0;
      }

      public override int GetValue(EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_Getter(entityManager.GetComponentData<T>(entity));
      }
    }

    public class ComponentIntRangePropertyBinder<T> : PrefabUISystem.IntRangePropertyBinder where T : unmanaged, IComponentData
    {
      private readonly Func<T, int> m_MinGetter;
      private readonly Func<T, int> m_MaxGetter;

      public ComponentIntRangePropertyBinder(
        string labelId,
        string unit,
        Func<T, int> minGetter,
        Func<T, int> maxGetter,
        bool signed = false,
        string icon = null,
        string valueIcon = null)
        : base(labelId, unit, signed, icon, valueIcon)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MinGetter = minGetter;
        // ISSUE: reference to a compiler-generated field
        this.m_MaxGetter = maxGetter;
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        return entityManager.TryGetComponent<T>(entity, out T _);
      }

      public override int GetMinValue(EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_MinGetter(entityManager.GetComponentData<T>(entity));
      }

      public override int GetMaxValue(EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_MaxGetter(entityManager.GetComponentData<T>(entity));
      }
    }

    public class ComponentInt2PropertyBinder<T> : PrefabUISystem.Int2PropertyBinder where T : unmanaged, IComponentData
    {
      private readonly Func<T, int2> m_Getter;
      private readonly bool m_OmitZero;

      public ComponentInt2PropertyBinder(
        string labelId,
        string unit,
        Func<T, int2> getter,
        bool omitZero = true,
        bool signed = false,
        string icon = null,
        string valueIcon = null)
        : base(labelId, unit, signed, icon, valueIcon)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Getter = getter;
        // ISSUE: reference to a compiler-generated field
        this.m_OmitZero = omitZero;
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        T component;
        if (!entityManager.TryGetComponent<T>(entity, out component))
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return !this.m_OmitZero || math.all(this.m_Getter(component) != int2.zero);
      }

      public override int2 GetValue(EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_Getter(entityManager.GetComponentData<T>(entity));
      }
    }

    public abstract class StringPropertyBinder : PrefabUISystem.IPrefabPropertyBinder
    {
      public readonly string m_LabelId;
      public readonly string m_Icon;
      public readonly string m_ValueIcon;

      protected StringPropertyBinder(string labelId, string icon = null, string valueIcon = null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LabelId = labelId;
        // ISSUE: reference to a compiler-generated field
        this.m_Icon = icon;
        // ISSUE: reference to a compiler-generated field
        this.m_ValueIcon = icon;
      }

      public abstract bool Matches(EntityManager entityManager, Entity entity);

      public abstract string GetValueId(EntityManager entityManager, Entity entity);

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        binder.Write<StringProperty>(new StringProperty()
        {
          labelId = this.m_LabelId,
          valueId = this.GetValueId(entityManager, entity),
          icon = this.m_Icon,
          valueIcon = this.m_ValueIcon
        });
      }
    }

    public class ConstructionCostBinder : 
      PrefabUISystem.ComponentIntRangePropertyBinder<PlaceableObjectData>
    {
      public ConstructionCostBinder()
        : base("Properties.CONSTRUCTION_COST", "money", (Func<PlaceableObjectData, int>) (data => (int) data.m_ConstructionCost), (Func<PlaceableObjectData, int>) (data => (int) data.m_ConstructionCost))
      {
      }

      public override int GetMinValue(EntityManager entityManager, Entity entity)
      {
        if (!entityManager.TryGetComponent<TreeData>(entity, out TreeData _))
        {
          // ISSUE: reference to a compiler-generated method
          return base.GetMinValue(entityManager, entity);
        }
        PlaceableObjectData componentData = entityManager.GetComponentData<PlaceableObjectData>(entity);
        // ISSUE: variable of a compiler-generated type
        ObjectToolSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<ObjectToolSystem>();
        EntityQuery entityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
        EconomyParameterData singleton = entityQuery.GetSingleton<EconomyParameterData>();
        int actualAgeMask = (int) systemManaged.actualAgeMask;
        int val1 = int.MaxValue;
        if ((actualAgeMask & 1) != 0)
          val1 = Math.Min(val1, (int) componentData.m_ConstructionCost);
        if ((actualAgeMask & 2) != 0)
          val1 = Math.Min(val1, (int) ((long) componentData.m_ConstructionCost * (long) singleton.m_TreeCostMultipliers.x));
        if ((actualAgeMask & 4) != 0)
          val1 = Math.Min(val1, (int) ((long) componentData.m_ConstructionCost * (long) singleton.m_TreeCostMultipliers.y));
        if ((actualAgeMask & 8) != 0)
          val1 = Math.Min(val1, (int) ((long) componentData.m_ConstructionCost * (long) singleton.m_TreeCostMultipliers.z));
        entityQuery.Dispose();
        return val1;
      }

      public override int GetMaxValue(EntityManager entityManager, Entity entity)
      {
        if (!entityManager.TryGetComponent<TreeData>(entity, out TreeData _))
        {
          // ISSUE: reference to a compiler-generated method
          return base.GetMaxValue(entityManager, entity);
        }
        PlaceableObjectData componentData = entityManager.GetComponentData<PlaceableObjectData>(entity);
        // ISSUE: variable of a compiler-generated type
        ObjectToolSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<ObjectToolSystem>();
        EntityQuery entityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
        EconomyParameterData singleton = entityQuery.GetSingleton<EconomyParameterData>();
        int actualAgeMask = (int) systemManaged.actualAgeMask;
        int val1 = 0;
        if ((actualAgeMask & 1) != 0)
          val1 = Math.Max(val1, (int) componentData.m_ConstructionCost);
        if ((actualAgeMask & 2) != 0)
          val1 = Math.Max(val1, (int) ((long) componentData.m_ConstructionCost * (long) singleton.m_TreeCostMultipliers.x));
        if ((actualAgeMask & 4) != 0)
          val1 = Math.Max(val1, (int) ((long) componentData.m_ConstructionCost * (long) singleton.m_TreeCostMultipliers.y));
        if ((actualAgeMask & 8) != 0)
          val1 = Math.Max(val1, (int) ((long) componentData.m_ConstructionCost * (long) singleton.m_TreeCostMultipliers.z));
        entityQuery.Dispose();
        return val1;
      }
    }

    public class ConsumptionBinder : PrefabUISystem.IPrefabPropertyBinder
    {
      public bool Matches(EntityManager entityManager, Entity entity)
      {
        ConsumptionData component;
        return entityManager.TryGetComponent<ConsumptionData>(entity, out component) && (double) component.m_WaterConsumption + (double) component.m_ElectricityConsumption + (double) component.m_GarbageAccumulation > 0.0;
      }

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        ConsumptionData componentData = entityManager.GetComponentData<ConsumptionData>(entity);
        binder.TypeBegin("prefabs.ConsumptionProperty");
        binder.PropertyName("electricityConsumption");
        binder.Write(Mathf.CeilToInt(componentData.m_ElectricityConsumption));
        binder.PropertyName("waterConsumption");
        binder.Write(Mathf.CeilToInt(componentData.m_WaterConsumption));
        binder.PropertyName("garbageAccumulation");
        binder.Write(Mathf.CeilToInt(componentData.m_GarbageAccumulation));
        binder.TypeEnd();
      }
    }

    public class PollutionBinder : PrefabUISystem.IPrefabPropertyBinder
    {
      private UIPollutionConfigurationPrefab m_ConfigData;

      public PollutionBinder(UIPollutionConfigurationPrefab data) => this.m_ConfigData = data;

      public bool Matches(EntityManager entityManager, Entity entity)
      {
        PollutionData component;
        return entityManager.TryGetComponent<PollutionData>(entity, out component) && (double) component.m_AirPollution + (double) component.m_GroundPollution + (double) component.m_NoisePollution != 0.0;
      }

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        PollutionData componentData = entityManager.GetComponentData<PollutionData>(entity);
        binder.TypeBegin("prefabs.PollutionProperty");
        binder.PropertyName("groundPollution");
        // ISSUE: reference to a compiler-generated field
        binder.Write((int) PollutionUIUtils.GetPollutionKey(this.m_ConfigData.m_GroundPollution, componentData.m_GroundPollution));
        binder.PropertyName("airPollution");
        // ISSUE: reference to a compiler-generated field
        binder.Write((int) PollutionUIUtils.GetPollutionKey(this.m_ConfigData.m_AirPollution, componentData.m_AirPollution));
        binder.PropertyName("noisePollution");
        // ISSUE: reference to a compiler-generated field
        binder.Write((int) PollutionUIUtils.GetPollutionKey(this.m_ConfigData.m_NoisePollution, componentData.m_NoisePollution));
        binder.TypeEnd();
      }
    }

    public abstract class ElectricityPropertyBinder : PrefabUISystem.IPrefabPropertyBinder
    {
      public readonly string m_LabelId;

      protected ElectricityPropertyBinder(string labelId) => this.m_LabelId = labelId;

      public abstract bool Matches(EntityManager entityManager, Entity entity);

      public abstract void GetValue(
        EntityManager entityManager,
        Entity entity,
        out int minCapacity,
        out int maxCapacity,
        out Layer voltageLayers);

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        int minCapacity;
        int maxCapacity;
        Layer voltageLayers;
        // ISSUE: reference to a compiler-generated method
        this.GetValue(entityManager, entity, out minCapacity, out maxCapacity, out voltageLayers);
        binder.TypeBegin("prefabs.ElectricityProperty");
        binder.PropertyName("labelId");
        // ISSUE: reference to a compiler-generated field
        binder.Write(this.m_LabelId);
        binder.PropertyName("minCapacity");
        binder.Write(minCapacity);
        binder.PropertyName("maxCapacity");
        binder.Write(maxCapacity);
        binder.PropertyName("voltage");
        binder.Write((int) ElectricityUIUtils.GetVoltage(voltageLayers));
        binder.TypeEnd();
      }
    }

    public struct UpkeepIntProperty : IJsonWritable
    {
      public string labelId;
      public int value;
      public string unit;
      public bool signed;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("prefabs.UpkeepIntProperty");
        writer.PropertyName("labelId");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.labelId);
        writer.PropertyName("value");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.value);
        writer.PropertyName("unit");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.unit);
        writer.PropertyName("signed");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.signed);
        writer.TypeEnd();
      }
    }

    public struct UpkeepInt2Property : IJsonWritable
    {
      public string labelId;
      public int2 value;
      public string unit;
      public bool signed;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("prefabs.UpkeepInt2Property");
        writer.PropertyName("labelId");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.labelId);
        writer.PropertyName("value");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.value);
        writer.PropertyName("unit");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.unit);
        writer.PropertyName("signed");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.signed);
        writer.TypeEnd();
      }
    }

    public class StorageLimitBinder : PrefabUISystem.IntPropertyBinder
    {
      public StorageLimitBinder()
        : base("Properties.CARGO_CAPACITY", "weight")
      {
      }

      public override int GetValue(EntityManager entityManager, Entity entity)
      {
        StorageLimitData component1;
        if (!entityManager.TryGetComponent<StorageLimitData>(entity, out component1))
          return 0;
        PrefabRef component2;
        if (entityManager.TryGetComponent<PropertyRenter>(entity, out PropertyRenter _) && entityManager.TryGetComponent<PrefabRef>(entity, out component2))
        {
          Entity prefab = component2.m_Prefab;
          BuildingPropertyData component3;
          SpawnableBuildingData component4;
          BuildingData component5;
          if (entityManager.TryGetComponent<BuildingPropertyData>(prefab, out component3) && component3.m_AllowedStored != Resource.NoResource && entityManager.TryGetComponent<SpawnableBuildingData>(prefab, out component4) && entityManager.TryGetComponent<BuildingData>(prefab, out component5))
            return component1.GetAdjustedLimit(component4, component5);
        }
        return component1.m_Limit;
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        return entityManager.TryGetComponent<StorageLimitData>(entity, out StorageLimitData _);
      }
    }

    public class PowerProductionBinder : PrefabUISystem.ElectricityPropertyBinder
    {
      public PowerProductionBinder()
        : base("Properties.POWER_PLANT_OUTPUT")
      {
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        if (entityManager.HasComponent<PowerPlantData>(entity) || entityManager.HasComponent<EmergencyGeneratorData>(entity))
          return true;
        DynamicBuffer<SubObject> buffer;
        if (entityManager.HasComponent<NetData>(entity) && entityManager.TryGetBuffer<SubObject>(entity, true, out buffer))
        {
          for (int index = 0; index < buffer.Length; ++index)
          {
            if (entityManager.HasComponent<PowerPlantData>(buffer[index].m_Prefab))
              return true;
          }
        }
        return false;
      }

      public override void GetValue(
        EntityManager entityManager,
        Entity entity,
        out int minCapacity,
        out int maxCapacity,
        out Layer voltageLayers)
      {
        minCapacity = 0;
        maxCapacity = 0;
        voltageLayers = Layer.None;
        // ISSUE: reference to a compiler-generated method
        PrefabUISystem.PowerProductionBinder.AddValues(entityManager, entity, ref minCapacity, ref maxCapacity, ref voltageLayers);
        DynamicBuffer<SubObject> buffer;
        if (!entityManager.HasComponent<NetData>(entity) || !entityManager.TryGetBuffer<SubObject>(entity, true, out buffer))
          return;
        for (int index = 0; index < buffer.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          PrefabUISystem.PowerProductionBinder.AddValues(entityManager, buffer[index].m_Prefab, ref minCapacity, ref maxCapacity, ref voltageLayers);
        }
      }

      public static void AddValues(
        EntityManager entityManager,
        Entity entity,
        ref int minCapacity,
        ref int maxCapacity,
        ref Layer voltageLayers)
      {
        PowerPlantData component1;
        if (entityManager.TryGetComponent<PowerPlantData>(entity, out component1))
        {
          minCapacity += component1.m_ElectricityProduction;
          maxCapacity += component1.m_ElectricityProduction;
        }
        WindPoweredData component2;
        if (entityManager.TryGetComponent<WindPoweredData>(entity, out component2))
          maxCapacity += component2.m_Production;
        SolarPoweredData component3;
        if (entityManager.TryGetComponent<SolarPoweredData>(entity, out component3))
          maxCapacity += component3.m_Production;
        GarbagePoweredData component4;
        if (entityManager.TryGetComponent<GarbagePoweredData>(entity, out component4))
          maxCapacity += component4.m_Capacity;
        WaterPoweredData component5;
        if (entityManager.TryGetComponent<WaterPoweredData>(entity, out component5))
          maxCapacity += (int) (1000000.0 * (double) component5.m_CapacityFactor);
        GroundWaterPoweredData component6;
        if (entityManager.TryGetComponent<GroundWaterPoweredData>(entity, out component6))
          maxCapacity += component6.m_Production;
        EmergencyGeneratorData component7;
        if (entityManager.TryGetComponent<EmergencyGeneratorData>(entity, out component7))
          maxCapacity += component7.m_ElectricityProduction;
        voltageLayers |= ElectricityUIUtils.GetPowerLineLayers(entityManager, entity);
      }
    }

    public class TransformerCapacityBinder : PrefabUISystem.IntPropertyBinder
    {
      public TransformerCapacityBinder()
        : base("Properties.TRANSFORMER_CAPACITY", "power")
      {
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        return entityManager.HasComponent<TransformerData>(entity) && !entityManager.HasComponent<PowerPlantData>(entity);
      }

      public override int GetValue(EntityManager entityManager, Entity entity)
      {
        int x = 0;
        int y = 0;
        DynamicBuffer<Game.Prefabs.SubNet> buffer;
        if (entityManager.TryGetBuffer<Game.Prefabs.SubNet>(entity, true, out buffer))
        {
          foreach (Game.Prefabs.SubNet subNet in buffer)
          {
            ElectricityConnectionData component;
            if (subNet.m_NodeIndex.x == subNet.m_NodeIndex.y && entityManager.TryGetComponent<ElectricityConnectionData>(subNet.m_Prefab, out component))
            {
              if (component.m_Voltage == Game.Prefabs.ElectricityConnection.Voltage.Low)
                x += component.m_Capacity;
              else
                y += component.m_Capacity;
            }
          }
        }
        return math.min(x, y);
      }
    }

    public class TransformerInputBinder : PrefabUISystem.StringPropertyBinder
    {
      public TransformerInputBinder()
        : base("Properties.TRANSFORMER_INPUT")
      {
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        return entityManager.HasComponent<TransformerData>(entity) && !entityManager.HasComponent<PowerPlantData>(entity);
      }

      public override string GetValueId(EntityManager entityManager, Entity entity)
      {
        return "Properties.VOLTAGE:1";
      }
    }

    public class TransformerOutputBinder : PrefabUISystem.StringPropertyBinder
    {
      public TransformerOutputBinder()
        : base("Properties.TRANSFORMER_OUTPUT")
      {
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        return entityManager.HasComponent<TransformerData>(entity);
      }

      public override string GetValueId(EntityManager entityManager, Entity entity)
      {
        return "Properties.VOLTAGE:0";
      }
    }

    public class ElectricityConnectionBinder : PrefabUISystem.ElectricityPropertyBinder
    {
      public ElectricityConnectionBinder()
        : base("Properties.POWER_LINE_CAPACITY")
      {
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        ElectricityConnectionData component1;
        NetData component2;
        return entityManager.TryGetComponent<ElectricityConnectionData>(entity, out component1) && component1.m_Capacity > 0 && (component1.m_CompositionAll.m_General & CompositionFlags.General.Lighting) == (CompositionFlags.General) 0 && entityManager.TryGetComponent<NetData>(entity, out component2) && ElectricityUIUtils.HasVoltageLayers(component2.m_LocalConnectLayers);
      }

      public override void GetValue(
        EntityManager entityManager,
        Entity entity,
        out int minCapacity,
        out int maxCapacity,
        out Layer voltageLayers)
      {
        minCapacity = entityManager.GetComponentData<ElectricityConnectionData>(entity).m_Capacity;
        maxCapacity = minCapacity;
        voltageLayers = entityManager.GetComponentData<NetData>(entity).m_LocalConnectLayers;
      }
    }

    public class WaterConnectionBinder : PrefabUISystem.StringPropertyBinder
    {
      public WaterConnectionBinder()
        : base("Properties.WATER_PIPES")
      {
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        WaterPipeConnectionData component1;
        NetData component2;
        return entityManager.TryGetComponent<WaterPipeConnectionData>(entity, out component1) && (component1.m_FreshCapacity > 0 || component1.m_SewageCapacity > 0) && entityManager.TryGetComponent<NetData>(entity, out component2) && !entityManager.HasComponent<PipelineData>(entity) && (component2.m_LocalConnectLayers & (Layer.WaterPipe | Layer.SewagePipe)) > Layer.None;
      }

      public override string GetValueId(EntityManager entityManager, Entity entity)
      {
        WaterPipeConnectionData componentData = entityManager.GetComponentData<WaterPipeConnectionData>(entity);
        if (componentData.m_FreshCapacity > 0 && componentData.m_SewageCapacity > 0)
          return "Properties.WATER_PIPE_TYPE[Combined]";
        return componentData.m_FreshCapacity > 0 ? "Properties.WATER_PIPE_TYPE[Fresh]" : "Properties.WATER_PIPE_TYPE[Sewage]";
      }
    }

    public class JailCapacityBinder : PrefabUISystem.IntPropertyBinder
    {
      public JailCapacityBinder()
        : base("Properties.JAIL_CAPACITY", "integer")
      {
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        PoliceStationData component1;
        if (entityManager.TryGetComponent<PoliceStationData>(entity, out component1) && component1.m_JailCapacity > 0)
          return true;
        PrisonData component2;
        return entityManager.TryGetComponent<PrisonData>(entity, out component2) && component2.m_PrisonerCapacity > 0;
      }

      public override int GetValue(EntityManager entityManager, Entity entity)
      {
        int num = 0;
        PoliceStationData component1;
        if (entityManager.TryGetComponent<PoliceStationData>(entity, out component1))
          num += component1.m_JailCapacity;
        PrisonData component2;
        if (entityManager.TryGetComponent<PrisonData>(entity, out component2))
          num += component2.m_PrisonerCapacity;
        return num;
      }
    }

    public class TransportStopBinder : PrefabUISystem.IPrefabPropertyBinder
    {
      public bool Matches(EntityManager entityManager, Entity entity)
      {
        DynamicBuffer<SubObject> buffer;
        if (entityManager.HasComponent<NetData>(entity) || !entityManager.TryGetBuffer<SubObject>(entity, true, out buffer))
          return false;
        for (int index = 0; index < buffer.Length; ++index)
        {
          TransportStopData component;
          if (entityManager.TryGetComponent<TransportStopData>(buffer[index].m_Prefab, out component))
          {
            // ISSUE: reference to a compiler-generated method
            return component.m_PassengerTransport && PrefabUISystem.TransportStopBinder.IsValidTransportType(component.m_TransportType);
          }
        }
        return false;
      }

      public void Bind(IJsonWriter binder, EntityManager entityManager, Entity entity)
      {
        DynamicBuffer<SubObject> buffer = entityManager.GetBuffer<SubObject>(entity, true);
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        for (int index = 0; index < buffer.Length; ++index)
        {
          TransportStopData component;
          if (entityManager.TryGetComponent<TransportStopData>(buffer[index].m_Prefab, out component) && component.m_PassengerTransport)
          {
            if (component.m_TransportType == TransportType.Bus)
              ++num1;
            else if (component.m_TransportType == TransportType.Train)
              ++num2;
            else if (component.m_TransportType == TransportType.Tram)
              ++num3;
            else if (component.m_TransportType == TransportType.Ship)
              ++num4;
            else if (component.m_TransportType == TransportType.Helicopter)
              ++num5;
            else if (component.m_TransportType == TransportType.Airplane)
              ++num6;
            else if (component.m_TransportType == TransportType.Subway)
              ++num7;
          }
        }
        binder.TypeBegin("prefabs.TransportStopProperty");
        binder.PropertyName("stops");
        binder.MapBegin(7U);
        binder.Write("Airplane");
        binder.Write(num6);
        binder.Write("Helicopter");
        binder.Write(num5);
        binder.Write("Ship");
        binder.Write(num4);
        binder.Write("Subway");
        binder.Write(num7);
        binder.Write("Tram");
        binder.Write(num3);
        binder.Write("Train");
        binder.Write(num2);
        binder.Write("Bus");
        binder.Write(num1);
        binder.MapEnd();
        binder.TypeEnd();
      }

      private static bool IsValidTransportType(TransportType type)
      {
        switch (type)
        {
          case TransportType.Bus:
          case TransportType.Train:
          case TransportType.Tram:
          case TransportType.Ship:
          case TransportType.Helicopter:
          case TransportType.Airplane:
          case TransportType.Subway:
            return true;
          default:
            return false;
        }
      }
    }

    public class RequiredResourceBinder : PrefabUISystem.StringPropertyBinder
    {
      private ResourceSystem m_ResourceSystem;

      public RequiredResourceBinder(ResourceSystem resourceSystem)
        : base("Properties.REQUIRED_RESOURCE")
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceSystem = resourceSystem;
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return this.RequiresWater(entityManager, entity, out AllowedWaterTypes _) || this.GetExtractorType(entityManager, entity) != MapFeature.None;
      }

      public override string GetValueId(EntityManager entityManager, Entity entity)
      {
        AllowedWaterTypes types;
        // ISSUE: reference to a compiler-generated method
        if (!this.RequiresWater(entityManager, entity, out types))
        {
          // ISSUE: reference to a compiler-generated method
          return string.Format("Properties.MAP_RESOURCE[{0:G}]", (object) this.GetExtractorType(entityManager, entity));
        }
        return (types & AllowedWaterTypes.Groundwater) != AllowedWaterTypes.None ? "Properties.MAP_RESOURCE[GroundWater]" : "Properties.MAP_RESOURCE[SurfaceWater]";
      }

      private bool RequiresWater(
        EntityManager entityManager,
        Entity entity,
        out AllowedWaterTypes types)
      {
        if (entityManager.HasComponent<GroundWaterPoweredData>(entity))
        {
          types = AllowedWaterTypes.Groundwater;
          return true;
        }
        WaterPumpingStationData component;
        if (entityManager.TryGetComponent<WaterPumpingStationData>(entity, out component) && component.m_Types != AllowedWaterTypes.None)
        {
          types = component.m_Types;
          return true;
        }
        types = AllowedWaterTypes.None;
        return false;
      }

      private MapFeature GetExtractorType(EntityManager entityManager, Entity entity)
      {
        PlaceholderBuildingData component1;
        BuildingPropertyData component2;
        DynamicBuffer<Game.Prefabs.SubArea> buffer;
        if (entityManager.TryGetComponent<PlaceholderBuildingData>(entity, out component1) && component1.m_Type == BuildingType.ExtractorBuilding && entityManager.TryGetComponent<BuildingPropertyData>(entity, out component2) && entityManager.TryGetBuffer<Game.Prefabs.SubArea>(entity, true, out buffer))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
          Resource allowedManufactured = component2.m_AllowedManufactured;
          ResourceData component3;
          if (entityManager.TryGetComponent<ResourceData>(prefabs[allowedManufactured], out component3) && component3.m_RequireNaturalResource)
          {
            foreach (Game.Prefabs.SubArea subArea in buffer)
            {
              ExtractorAreaData component4;
              if (entityManager.TryGetComponent<ExtractorAreaData>(subArea.m_Prefab, out component4))
                return component4.m_MapFeature;
            }
          }
        }
        return MapFeature.None;
      }
    }

    public class UpkeepModifierBinder : PrefabUISystem.IntPropertyBinder
    {
      public UpkeepModifierBinder()
        : base("Properties.RESOURCE_CONSUMPTION", "percentage", true)
      {
      }

      public override bool Matches(EntityManager entityManager, Entity entity)
      {
        DynamicBuffer<UpkeepModifierData> buffer;
        if (entityManager.TryGetBuffer<UpkeepModifierData>(entity, true, out buffer))
        {
          foreach (UpkeepModifierData upkeepModifierData in buffer)
          {
            if ((double) upkeepModifierData.m_Multiplier != 1.0)
              return true;
          }
        }
        return false;
      }

      public override int GetValue(EntityManager entityManager, Entity entity)
      {
        DynamicBuffer<UpkeepModifierData> buffer = entityManager.GetBuffer<UpkeepModifierData>(entity, true);
        float x = 0.0f;
        foreach (UpkeepModifierData upkeepModifierData in buffer)
          x = math.max(x, upkeepModifierData.m_Multiplier);
        return (int) math.round((float) (100.0 * ((double) x - 1.0)));
      }
    }
  }
}
