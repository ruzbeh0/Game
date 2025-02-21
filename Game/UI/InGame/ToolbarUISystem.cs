// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ToolbarUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.City;
using Game.Input;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Serialization;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ToolbarUISystem : UISystemBase, IPreDeserialize
  {
    private const string kGroup = "toolbar";
    private PrefabSystem m_PrefabSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ToolSystem m_ToolSystem;
    private ObjectToolSystem m_ObjectToolSystem;
    private DefaultToolSystem m_DefaultTool;
    private UniqueAssetTrackingSystem m_UniqueAssetTrackingSystem;
    private PrefabUISystem m_PrefabUISystem;
    private ImageSystem m_ImageSystem;
    private UpgradeMenuUISystem m_UpgradeMenuUISystem;
    private ActionsSection m_ActionsSection;
    private EntityQuery m_ThemeQuery;
    private EntityQuery m_AssetPackQuery;
    private EntityQuery m_ToolbarGroupQuery;
    private EntityQuery m_UnlockedPrefabQuery;
    private RawValueBinding m_ToolbarGroupsBinding;
    private RawMapBinding<Entity> m_AssetMenuCategoriesBinding;
    private RawMapBinding<Entity> m_AssetsBinding;
    private RawValueBinding m_ThemesBinding;
    private RawValueBinding m_AssetPacksBinding;
    private ValueBinding<int> m_AgeMaskBinding;
    private GetterValueBinding<List<Entity>> m_SelectedThemesBinding;
    private GetterValueBinding<List<Entity>> m_SelectedAssetPacksBinding;
    private ValueBinding<Entity> m_SelectedAssetMenuBinding;
    private ValueBinding<Entity> m_SelectedAssetCategoryBinding;
    private ValueBinding<Entity> m_SelectedAssetBinding;
    private Dictionary<Entity, Entity> m_LastSelectedCategories;
    private Dictionary<Entity, Entity> m_LastSelectedAssets;
    private List<Entity> m_SelectedThemes;
    private List<Entity> m_SelectedAssetPacks;
    private bool m_UniqueAssetStatusChanged;
    private bool m_HasUnlockedPrefabLastFrame;

    public bool hasActiveSelection
    {
      get
      {
        return this.m_SelectedAssetMenuBinding.value != Entity.Null || this.m_SelectedAssetBinding.value != Entity.Null;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultTool = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueAssetTrackingSystem = this.World.GetOrCreateSystemManaged<UniqueAssetTrackingSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueAssetTrackingSystem.EventUniqueAssetStatusChanged += (Action<Entity, bool>) ((prefabEntity, placed) => this.m_UniqueAssetStatusChanged = true);
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeMenuUISystem = this.World.GetOrCreateSystemManaged<UpgradeMenuUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ActionsSection = this.World.GetOrCreateSystemManaged<ActionsSection>();
      // ISSUE: reference to a compiler-generated field
      this.m_ThemeQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<ThemeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AssetPackQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<AssetPackData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ToolbarGroupQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<UIGroupElement>(), ComponentType.ReadOnly<UIToolbarGroupData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ToolbarGroupsBinding = new RawValueBinding("toolbar", "toolbarGroups", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated method
        NativeArray<UIObjectInfo> sortedToolbarGroups = this.GetSortedToolbarGroups();
        writer.ArrayBegin(sortedToolbarGroups.Length);
        for (int index1 = 0; index1 < sortedToolbarGroups.Length; ++index1)
        {
          UIObjectInfo uiObjectInfo1 = sortedToolbarGroups[index1];
          EntityManager entityManager1 = this.EntityManager;
          EntityManager entityManager2 = this.EntityManager;
          DynamicBuffer<UIGroupElement> buffer = entityManager2.GetBuffer<UIGroupElement>(uiObjectInfo1.entity, true);
          NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(entityManager1, buffer, Allocator.TempJob);
          objects.Sort<UIObjectInfo>();
          writer.TypeBegin("toolbar.ToolbarGroup");
          writer.PropertyName("entity");
          writer.Write(uiObjectInfo1.entity);
          writer.PropertyName("children");
          writer.ArrayBegin(objects.Length);
          for (int index2 = 0; index2 < objects.Length; ++index2)
          {
            UIObjectInfo uiObjectInfo2 = objects[index2];
            Entity entity = uiObjectInfo2.entity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            PrefabSystem prefabSystem = this.m_PrefabSystem;
            uiObjectInfo2 = objects[index2];
            PrefabData prefabData = uiObjectInfo2.prefabData;
            // ISSUE: reference to a compiler-generated method
            PrefabBase prefab = prefabSystem.GetPrefab<PrefabBase>(prefabData);
            entityManager2 = this.EntityManager;
            bool flag = entityManager2.HasComponent<UIAssetMenuData>(entity);
            writer.TypeBegin("toolbar.ToolbarItem");
            writer.PropertyName("entity");
            writer.Write(entity);
            writer.PropertyName("name");
            writer.Write(prefab.name);
            writer.PropertyName("type");
            writer.Write(flag ? 1 : 0);
            writer.PropertyName("icon");
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            writer.Write(ImageSystem.GetIcon(prefab) ?? this.m_ImageSystem.placeholderIcon);
            writer.PropertyName("locked");
            writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
            writer.PropertyName("uiTag");
            writer.Write(prefab.uiTag);
            writer.PropertyName("requirements");
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PrefabUISystem.BindPrefabRequirements(writer, entity);
            writer.PropertyName("highlight");
            IJsonWriter jsonWriter = writer;
            entityManager2 = this.EntityManager;
            int num = entityManager2.HasComponent<UIHighlight>(entity) ? 1 : 0;
            jsonWriter.Write(num != 0);
            writer.PropertyName("selectSound");
            writer.Write(prefab is BulldozePrefab ? "bulldoze" : (string) null);
            writer.PropertyName("deselectSound");
            writer.Write(prefab is BulldozePrefab ? "bulldoze-end" : (string) null);
            writer.PropertyName("shortcut");
            UIShortcut component;
            writer.Write(prefab.TryGet<UIShortcut>(out component) ? component.m_Action.m_AliasName : (string) null);
            writer.TypeEnd();
          }
          writer.ArrayEnd();
          writer.TypeEnd();
          objects.Dispose();
        }
        writer.ArrayEnd();
        sortedToolbarGroups.Dispose();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AssetMenuCategoriesBinding = new RawMapBinding<Entity>("toolbar", "assetCategories", (Action<IJsonWriter, Entity>) ((writer, assetMenu) =>
      {
        Entity entity1 = assetMenu;
        DynamicBuffer<UIGroupElement> buffer;
        if (this.EntityManager.HasComponent<UIAssetMenuData>(entity1) && this.EntityManager.TryGetBuffer<UIGroupElement>(entity1, true, out buffer))
        {
          // ISSUE: reference to a compiler-generated method
          NativeList<UIObjectInfo> sortedCategories = this.GetSortedCategories(buffer);
          writer.ArrayBegin(sortedCategories.Length);
          for (int index = 0; index < sortedCategories.Length; ++index)
          {
            UIObjectInfo uiObjectInfo = sortedCategories[index];
            Entity entity2 = uiObjectInfo.entity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            PrefabSystem prefabSystem = this.m_PrefabSystem;
            uiObjectInfo = sortedCategories[index];
            PrefabData prefabData = uiObjectInfo.prefabData;
            // ISSUE: reference to a compiler-generated method
            PrefabBase prefab = prefabSystem.GetPrefab<PrefabBase>(prefabData);
            writer.TypeBegin("toolbar.AssetCategory");
            writer.PropertyName("entity");
            writer.Write(entity2);
            writer.PropertyName("name");
            writer.Write(prefab.name);
            writer.PropertyName("icon");
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            writer.Write(ImageSystem.GetIcon(prefab) ?? this.m_ImageSystem.placeholderIcon);
            writer.PropertyName("locked");
            writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity2));
            writer.PropertyName("uiTag");
            writer.Write(prefab.uiTag);
            writer.PropertyName("highlight");
            writer.Write(this.EntityManager.HasComponent<UIHighlight>(entity2));
            writer.TypeEnd();
          }
          writer.ArrayEnd();
          sortedCategories.Dispose();
        }
        else
          writer.WriteEmptyArray();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AssetsBinding = new RawMapBinding<Entity>("toolbar", "assets", (Action<IJsonWriter, Entity>) ((writer, assetCategory) =>
      {
        Entity entity3 = assetCategory;
        DynamicBuffer<UIGroupElement> buffer;
        if (this.EntityManager.HasComponent<UIAssetCategoryData>(entity3) && this.EntityManager.TryGetBuffer<UIGroupElement>(entity3, true, out buffer))
        {
          NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, buffer, Allocator.TempJob);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FilterByThemes(objects, this.m_SelectedThemes);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FilterByPacks(objects, this.m_SelectedAssetPacks);
          objects.Sort<UIObjectInfo>();
          writer.ArrayBegin(objects.Length);
          for (int index = 0; index < objects.Length; ++index)
          {
            Entity entity4 = objects[index].entity;
            // ISSUE: reference to a compiler-generated field
            bool uniquePlaced = this.m_UniqueAssetTrackingSystem.IsPlacedUniqueAsset(entity4);
            // ISSUE: reference to a compiler-generated method
            this.BindAsset(writer, entity4, uniquePlaced);
          }
          writer.ArrayEnd();
          objects.Dispose();
        }
        else
          writer.WriteEmptyArray();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ThemesBinding = new RawValueBinding("toolbar", "themes", (Action<IJsonWriter>) (writer =>
      {
        if (GameManager.instance.gameMode.IsEditor())
        {
          writer.WriteEmptyArray();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity5 = this.m_SelectedAssetCategoryBinding.value;
          DynamicBuffer<UIGroupElement> buffer1;
          if (this.EntityManager.HasComponent<UIAssetCategoryData>(entity5) && this.EntityManager.TryGetBuffer<UIGroupElement>(entity5, true, out buffer1))
          {
            NativeParallelHashMap<Entity, bool> nativeParallelHashMap = new NativeParallelHashMap<Entity, bool>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
            for (int index3 = buffer1.Length - 1; index3 >= 0; --index3)
            {
              EntityManager entityManager = this.EntityManager;
              bool flag1 = entityManager.HasComponent<UIHighlight>(buffer1[index3].m_Prefab);
              DynamicBuffer<ObjectRequirementElement> buffer2;
              if (this.EntityManager.TryGetBuffer<ObjectRequirementElement>(buffer1[index3].m_Prefab, true, out buffer2))
              {
                for (int index4 = 0; index4 < buffer2.Length; ++index4)
                {
                  Entity requirement = buffer2[index4].m_Requirement;
                  entityManager = this.EntityManager;
                  if (entityManager.HasComponent<ThemeData>(requirement))
                  {
                    bool flag2;
                    nativeParallelHashMap[requirement] = !nativeParallelHashMap.TryGetValue(requirement, out flag2) ? flag1 : flag1 | flag2;
                  }
                }
              }
            }
            if (!nativeParallelHashMap.IsEmpty)
            {
              // ISSUE: reference to a compiler-generated field
              NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.m_ThemeQuery, Allocator.TempJob);
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
                writer.TypeBegin("toolbar.Theme");
                writer.PropertyName("entity");
                IJsonWriter writer1 = writer;
                uiObjectInfo = sortedObjects[index];
                Entity entity6 = uiObjectInfo.entity;
                writer1.Write(entity6);
                writer.PropertyName("name");
                writer.Write(prefab.name);
                writer.PropertyName("icon");
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated field
                writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
                writer.PropertyName("highlight");
                IJsonWriter jsonWriter = writer;
                ref NativeParallelHashMap<Entity, bool> local1 = ref nativeParallelHashMap;
                uiObjectInfo = sortedObjects[index];
                Entity entity7 = uiObjectInfo.entity;
                bool flag;
                ref bool local2 = ref flag;
                int num = local1.TryGetValue(entity7, out local2) & flag ? 1 : 0;
                jsonWriter.Write(num != 0);
                writer.TypeEnd();
              }
              writer.ArrayEnd();
              sortedObjects.Dispose();
            }
            else
              writer.WriteEmptyArray();
            nativeParallelHashMap.Dispose();
          }
          else
            writer.WriteEmptyArray();
        }
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AssetPacksBinding = new RawValueBinding("toolbar", "assetPacks", (Action<IJsonWriter>) (writer =>
      {
        if (GameManager.instance.gameMode.IsEditor())
        {
          writer.WriteEmptyArray();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity8 = this.m_SelectedAssetCategoryBinding.value;
          DynamicBuffer<UIGroupElement> buffer3;
          if (this.EntityManager.HasComponent<UIAssetCategoryData>(entity8) && this.EntityManager.TryGetBuffer<UIGroupElement>(entity8, true, out buffer3))
          {
            NativeParallelHashMap<Entity, bool> nativeParallelHashMap = new NativeParallelHashMap<Entity, bool>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
            for (int index5 = buffer3.Length - 1; index5 >= 0; --index5)
            {
              EntityManager entityManager = this.EntityManager;
              bool flag3 = entityManager.HasComponent<UIHighlight>(buffer3[index5].m_Prefab);
              DynamicBuffer<AssetPackElement> buffer4;
              if (this.EntityManager.TryGetBuffer<AssetPackElement>(buffer3[index5].m_Prefab, true, out buffer4))
              {
                for (int index6 = 0; index6 < buffer4.Length; ++index6)
                {
                  Entity pack = buffer4[index6].m_Pack;
                  entityManager = this.EntityManager;
                  if (entityManager.HasComponent<AssetPackData>(pack))
                  {
                    bool flag4;
                    nativeParallelHashMap[pack] = !nativeParallelHashMap.TryGetValue(pack, out flag4) ? flag3 : flag3 | flag4;
                  }
                }
              }
            }
            if (!nativeParallelHashMap.IsEmpty)
            {
              // ISSUE: reference to a compiler-generated field
              NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.m_AssetPackQuery, Allocator.TempJob);
              UIObjectInfo uiObjectInfo;
              for (int index = sortedObjects.Length - 1; index >= 0; --index)
              {
                ref NativeParallelHashMap<Entity, bool> local = ref nativeParallelHashMap;
                uiObjectInfo = sortedObjects[index];
                Entity entity9 = uiObjectInfo.entity;
                if (!local.ContainsKey(entity9))
                  sortedObjects.RemoveAt(index);
              }
              writer.ArrayBegin(sortedObjects.Length);
              for (int index = 0; index < sortedObjects.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                PrefabSystem prefabSystem = this.m_PrefabSystem;
                uiObjectInfo = sortedObjects[index];
                PrefabData prefabData = uiObjectInfo.prefabData;
                // ISSUE: reference to a compiler-generated method
                AssetPackPrefab prefab = prefabSystem.GetPrefab<AssetPackPrefab>(prefabData);
                writer.TypeBegin("toolbar.AssetPack");
                writer.PropertyName("entity");
                IJsonWriter writer2 = writer;
                uiObjectInfo = sortedObjects[index];
                Entity entity10 = uiObjectInfo.entity;
                writer2.Write(entity10);
                writer.PropertyName("name");
                writer.Write(prefab.name);
                writer.PropertyName("icon");
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated field
                writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
                writer.PropertyName("highlight");
                IJsonWriter jsonWriter = writer;
                ref NativeParallelHashMap<Entity, bool> local3 = ref nativeParallelHashMap;
                uiObjectInfo = sortedObjects[index];
                Entity entity11 = uiObjectInfo.entity;
                bool flag;
                ref bool local4 = ref flag;
                int num = local3.TryGetValue(entity11, out local4) & flag ? 1 : 0;
                jsonWriter.Write(num != 0);
                writer.TypeEnd();
              }
              writer.ArrayEnd();
              sortedObjects.Dispose();
            }
            else
              writer.WriteEmptyArray();
            nativeParallelHashMap.Dispose();
          }
          else
            writer.WriteEmptyArray();
        }
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AgeMaskBinding = new ValueBinding<int>("toolbar", "ageMask", !this.m_ObjectToolSystem.allowAge ? 0 : (int) this.m_ObjectToolSystem.actualAgeMask)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedThemesBinding = new GetterValueBinding<List<Entity>>("toolbar", "selectedThemes", (Func<List<Entity>>) (() => this.m_SelectedThemes), (IWriter<List<Entity>>) new ListWriter<Entity>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedAssetPacksBinding = new GetterValueBinding<List<Entity>>("toolbar", "selectedAssetPacks", (Func<List<Entity>>) (() => this.m_SelectedAssetPacks), (IWriter<List<Entity>>) new ListWriter<Entity>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedAssetMenuBinding = new ValueBinding<Entity>("toolbar", "selectedAssetMenu", Entity.Null)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedAssetCategoryBinding = new ValueBinding<Entity>("toolbar", "selectedAssetCategory", Entity.Null)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedAssetBinding = new ValueBinding<Entity>("toolbar", "selectedAsset", Entity.Null)));
      this.AddBinding((IBinding) new TriggerBinding<List<Entity>>("toolbar", "setSelectedThemes", (Action<List<Entity>>) (themes =>
      {
        // ISSUE: reference to a compiler-generated field
        Entity assetMenuEntity = this.m_SelectedAssetMenuBinding.value;
        // ISSUE: reference to a compiler-generated field
        Entity entity12 = this.m_SelectedAssetCategoryBinding.value;
        // ISSUE: reference to a compiler-generated field
        Entity entity13 = this.m_SelectedAssetBinding.value;
        // ISSUE: reference to a compiler-generated field
        List<Entity> selectedAssetPacks = this.m_SelectedAssetPacks;
        if (entity12 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          entity13 = !(entity13 != Entity.Null) ? this.GetFirstUnlockedItem(entity12, themes, selectedAssetPacks) : this.GetClosestAssetInThemes(entity13, entity12, themes);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          if (!this.IsMatchingTheme(entity13, themes))
            entity13 = Entity.Null;
        }
        // ISSUE: reference to a compiler-generated method
        this.Apply(themes, selectedAssetPacks, assetMenuEntity, entity12, entity13, InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad);
      }), (IReader<List<Entity>>) new ListReader<Entity>()));
      this.AddBinding((IBinding) new TriggerBinding<List<Entity>>("toolbar", "setSelectedAssetPacks", (Action<List<Entity>>) (packs =>
      {
        // ISSUE: reference to a compiler-generated field
        Entity assetMenuEntity = this.m_SelectedAssetMenuBinding.value;
        // ISSUE: reference to a compiler-generated field
        Entity entity14 = this.m_SelectedAssetCategoryBinding.value;
        // ISSUE: reference to a compiler-generated field
        Entity entity15 = this.m_SelectedAssetBinding.value;
        // ISSUE: reference to a compiler-generated field
        List<Entity> selectedThemes = this.m_SelectedThemes;
        if (entity14 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          entity15 = !(entity15 != Entity.Null) ? this.GetFirstUnlockedItem(entity14, selectedThemes, packs) : this.GetClosestAssetInPacks(entity15, entity14, packs);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          if (!this.IsMatchingPack(entity15, packs))
            entity15 = Entity.Null;
        }
        // ISSUE: reference to a compiler-generated method
        this.Apply(selectedThemes, packs, assetMenuEntity, entity14, entity15, InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad);
      }), (IReader<List<Entity>>) new ListReader<Entity>()));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("toolbar", "selectAssetMenu", (Action<Entity>) (assetMenu =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedAssetPacks = new List<Entity>();
        Entity entity16 = assetMenu;
        // ISSUE: reference to a compiler-generated field
        List<Entity> selectedThemes = this.m_SelectedThemes;
        // ISSUE: reference to a compiler-generated field
        List<Entity> selectedAssetPacks = this.m_SelectedAssetPacks;
        if (!(entity16 != Entity.Null) || !this.EntityManager.HasComponent<UIAssetMenuData>(entity16))
          return;
        Entity firstItem;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LastSelectedCategories.TryGetValue(entity16, out firstItem))
        {
          // ISSUE: reference to a compiler-generated method
          firstItem = this.GetFirstItem(entity16, selectedThemes, selectedAssetPacks);
        }
        Entity entity17 = Entity.Null;
        if (firstItem != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          entity17 = !this.m_LastSelectedAssets.TryGetValue(firstItem, out entity17) ? this.GetFirstUnlockedItem(firstItem, selectedThemes, selectedAssetPacks) : this.GetClosestAssetInPacks(this.GetClosestAssetInThemes(entity17, firstItem, selectedThemes), firstItem, selectedAssetPacks);
        }
        // ISSUE: reference to a compiler-generated method
        this.Apply(selectedThemes, selectedAssetPacks, entity16, firstItem, entity17, InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad);
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("toolbar", "selectAssetCategory", (Action<Entity>) (assetCategory =>
      {
        Entity entity = assetCategory;
        UIAssetCategoryData component;
        if (!(entity != Entity.Null) || !this.EntityManager.TryGetComponent<UIAssetCategoryData>(entity, out component))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedAssetPacks = new List<Entity>();
        Entity menu = component.m_Menu;
        // ISSUE: reference to a compiler-generated field
        List<Entity> selectedThemes = this.m_SelectedThemes;
        // ISSUE: reference to a compiler-generated field
        List<Entity> selectedAssetPacks = this.m_SelectedAssetPacks;
        Entity oldAssetEntity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        Entity assetEntity = !this.m_LastSelectedAssets.TryGetValue(entity, out oldAssetEntity) ? this.GetFirstUnlockedItem(entity, selectedThemes, selectedAssetPacks) : this.GetClosestAssetInPacks(this.GetClosestAssetInThemes(oldAssetEntity, entity, selectedThemes), entity, selectedAssetPacks);
        if (assetEntity == Entity.Null)
        {
          selectedAssetPacks.Clear();
          // ISSUE: reference to a compiler-generated method
          assetEntity = this.GetFirstUnlockedItem(entity, selectedThemes, selectedAssetPacks);
        }
        // ISSUE: reference to a compiler-generated method
        this.Apply(selectedThemes, selectedAssetPacks, menu, entity, assetEntity, InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad);
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity, bool>("toolbar", "selectAsset", (Action<Entity, bool>) ((assetEntity, updateTool) =>
      {
        // ISSUE: reference to a compiler-generated field
        List<Entity> themes = this.m_SelectedThemes;
        // ISSUE: reference to a compiler-generated field
        List<Entity> packs = this.m_SelectedAssetPacks;
        // ISSUE: reference to a compiler-generated method
        if (!this.IsMatchingTheme(assetEntity, themes))
        {
          // ISSUE: reference to a compiler-generated field
          NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.m_ThemeQuery, Allocator.TempJob);
          // ISSUE: reference to a compiler-generated method
          themes = this.FilterThemesByAsset(sortedObjects, assetEntity);
          sortedObjects.Dispose();
        }
        // ISSUE: reference to a compiler-generated method
        if (!this.IsMatchingPack(assetEntity, packs))
        {
          // ISSUE: reference to a compiler-generated field
          NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.m_AssetPackQuery, Allocator.TempJob);
          // ISSUE: reference to a compiler-generated method
          packs = this.FilterPacksByAsset(sortedObjects, assetEntity);
          sortedObjects.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        Entity menu = this.m_SelectedAssetMenuBinding.value;
        // ISSUE: reference to a compiler-generated field
        Entity group = this.m_SelectedAssetCategoryBinding.value;
        UIObjectData component1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.IsMatchingAssetCategory(assetEntity, this.m_SelectedAssetCategoryBinding.value) && assetEntity != Entity.Null && this.EntityManager.TryGetComponent<UIObjectData>(assetEntity, out component1))
        {
          UIAssetCategoryData component2;
          if (component1.m_Group != Entity.Null && this.EntityManager.TryGetComponent<UIAssetCategoryData>(component1.m_Group, out component2))
          {
            menu = component2.m_Menu;
            group = component1.m_Group;
          }
          else
          {
            menu = Entity.Null;
            group = Entity.Null;
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.Apply(themes, packs, menu, group, assetEntity, updateTool);
      })));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding("toolbar", "clearAssetSelection", (System.Action) (() => this.ClearAssetSelection(true))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<bool>("toolbar", "toggleToolOptions", (Action<bool>) (enabled => this.m_ToolSystem.activeTool?.ToggleToolOptions(enabled))));
      this.AddBinding((IBinding) new TriggerBinding<int>("toolbar", "setAgeMask", (Action<int>) (ageMask =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectToolSystem.ageMask = (Game.Tools.AgeMask) ageMask;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AgeMaskBinding.Update(this.m_ToolSystem.activeTool != this.m_ObjectToolSystem || !this.m_ObjectToolSystem.allowAge ? 0 : (int) this.m_ObjectToolSystem.actualAgeMask);
      })));
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectedCategories = new Dictionary<Entity, Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectedAssets = new Dictionary<Entity, Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedThemes = new List<Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetPacks = new List<Entity>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueAssetTrackingSystem.EventUniqueAssetStatusChanged -= (Action<Entity, bool>) ((prefabEntity, placed) => this.m_UniqueAssetStatusChanged = true);
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      PrefabBase activePrefab = this.m_ToolSystem.activePrefab;
      // ISSUE: reference to a compiler-generated field
      Entity assetEntity = this.m_SelectedAssetBinding.value;
      // ISSUE: reference to a compiler-generated field
      Entity entity1 = this.m_SelectedAssetMenuBinding.value;
      if ((UnityEngine.Object) activePrefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity2 = this.m_PrefabSystem.GetEntity(activePrefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (entity2 != assetEntity && (!(this.m_ToolSystem.activeTool is AreaToolSystem) || !this.m_UpgradeMenuUISystem.upgrading && !this.m_ActionsSection.editingLot))
        {
          // ISSUE: reference to a compiler-generated method
          this.SelectAsset(entity2, false);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool != this.m_DefaultTool || entity1 == Entity.Null && assetEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.ClearAssetSelection(false);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_HasUnlockedPrefabLastFrame)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolbarGroupsBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_HasUnlockedPrefabLastFrame = false;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (PrefabUtils.HasUnlockedPrefab<UIObjectData>(this.EntityManager, this.m_UnlockedPrefabQuery) || this.m_UniqueAssetStatusChanged)
      {
        if (assetEntity != Entity.Null && entity1 != Entity.Null && (UnityEngine.Object) activePrefab == (UnityEngine.Object) null && InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad)
        {
          // ISSUE: reference to a compiler-generated method
          this.ActivatePrefabTool(assetEntity);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ToolbarGroupsBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_AssetMenuCategoriesBinding.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_AssetsBinding.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_ThemesBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_AssetPacksBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_HasUnlockedPrefabLastFrame = true;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueAssetStatusChanged = false;
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedThemes.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetPacks.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedThemes.Add(this.m_CityConfigurationSystem.defaultTheme);
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectedCategories.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectedAssets.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Apply(this.m_SelectedThemes, this.m_SelectedAssetPacks, Entity.Null, Entity.Null, Entity.Null);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolbarGroupsBinding.Update();
    }

    private void OnUniqueAssetStatusChanged(Entity prefabEntity, bool placed)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueAssetStatusChanged = true;
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      this.ClearAssetSelection();
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectedAssets.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectedCategories.Clear();
    }

    private void BindToolbarGroups(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      NativeArray<UIObjectInfo> sortedToolbarGroups = this.GetSortedToolbarGroups();
      writer.ArrayBegin(sortedToolbarGroups.Length);
      for (int index1 = 0; index1 < sortedToolbarGroups.Length; ++index1)
      {
        UIObjectInfo uiObjectInfo1 = sortedToolbarGroups[index1];
        EntityManager entityManager1 = this.EntityManager;
        EntityManager entityManager2 = this.EntityManager;
        DynamicBuffer<UIGroupElement> buffer = entityManager2.GetBuffer<UIGroupElement>(uiObjectInfo1.entity, true);
        NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(entityManager1, buffer, Allocator.TempJob);
        objects.Sort<UIObjectInfo>();
        writer.TypeBegin("toolbar.ToolbarGroup");
        writer.PropertyName("entity");
        writer.Write(uiObjectInfo1.entity);
        writer.PropertyName("children");
        writer.ArrayBegin(objects.Length);
        for (int index2 = 0; index2 < objects.Length; ++index2)
        {
          UIObjectInfo uiObjectInfo2 = objects[index2];
          Entity entity = uiObjectInfo2.entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PrefabSystem prefabSystem = this.m_PrefabSystem;
          uiObjectInfo2 = objects[index2];
          PrefabData prefabData = uiObjectInfo2.prefabData;
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = prefabSystem.GetPrefab<PrefabBase>(prefabData);
          entityManager2 = this.EntityManager;
          bool flag = entityManager2.HasComponent<UIAssetMenuData>(entity);
          writer.TypeBegin("toolbar.ToolbarItem");
          writer.PropertyName("entity");
          writer.Write(entity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("type");
          writer.Write(flag ? 1 : 0);
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          writer.Write(ImageSystem.GetIcon(prefab) ?? this.m_ImageSystem.placeholderIcon);
          writer.PropertyName("locked");
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
          writer.PropertyName("uiTag");
          writer.Write(prefab.uiTag);
          writer.PropertyName("requirements");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PrefabUISystem.BindPrefabRequirements(writer, entity);
          writer.PropertyName("highlight");
          IJsonWriter jsonWriter = writer;
          entityManager2 = this.EntityManager;
          int num = entityManager2.HasComponent<UIHighlight>(entity) ? 1 : 0;
          jsonWriter.Write(num != 0);
          writer.PropertyName("selectSound");
          writer.Write(prefab is BulldozePrefab ? "bulldoze" : (string) null);
          writer.PropertyName("deselectSound");
          writer.Write(prefab is BulldozePrefab ? "bulldoze-end" : (string) null);
          writer.PropertyName("shortcut");
          UIShortcut component;
          writer.Write(prefab.TryGet<UIShortcut>(out component) ? component.m_Action.m_AliasName : (string) null);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        writer.TypeEnd();
        objects.Dispose();
      }
      writer.ArrayEnd();
      sortedToolbarGroups.Dispose();
    }

    private void BindAssetCategories(IJsonWriter writer, Entity assetMenu)
    {
      Entity entity1 = assetMenu;
      DynamicBuffer<UIGroupElement> buffer;
      if (this.EntityManager.HasComponent<UIAssetMenuData>(entity1) && this.EntityManager.TryGetBuffer<UIGroupElement>(entity1, true, out buffer))
      {
        // ISSUE: reference to a compiler-generated method
        NativeList<UIObjectInfo> sortedCategories = this.GetSortedCategories(buffer);
        writer.ArrayBegin(sortedCategories.Length);
        for (int index = 0; index < sortedCategories.Length; ++index)
        {
          UIObjectInfo uiObjectInfo = sortedCategories[index];
          Entity entity2 = uiObjectInfo.entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PrefabSystem prefabSystem = this.m_PrefabSystem;
          uiObjectInfo = sortedCategories[index];
          PrefabData prefabData = uiObjectInfo.prefabData;
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = prefabSystem.GetPrefab<PrefabBase>(prefabData);
          writer.TypeBegin("toolbar.AssetCategory");
          writer.PropertyName("entity");
          writer.Write(entity2);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          writer.Write(ImageSystem.GetIcon(prefab) ?? this.m_ImageSystem.placeholderIcon);
          writer.PropertyName("locked");
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity2));
          writer.PropertyName("uiTag");
          writer.Write(prefab.uiTag);
          writer.PropertyName("highlight");
          writer.Write(this.EntityManager.HasComponent<UIHighlight>(entity2));
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        sortedCategories.Dispose();
      }
      else
        writer.WriteEmptyArray();
    }

    private NativeList<UIObjectInfo> GetSortedCategories(DynamicBuffer<UIGroupElement> elements)
    {
      NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, elements, Allocator.TempJob);
      for (int index = objects.Length - 1; index >= 0; --index)
      {
        DynamicBuffer<UIGroupElement> buffer;
        if (!this.EntityManager.HasComponent<UIAssetCategoryData>(objects[index].entity) || !this.EntityManager.TryGetBuffer<UIGroupElement>(objects[index].entity, true, out buffer) || buffer.Length == 0)
          objects.RemoveAtSwapBack(index);
      }
      objects.Sort<UIObjectInfo>();
      return objects;
    }

    private void BindAssets(IJsonWriter writer, Entity assetCategory)
    {
      Entity entity1 = assetCategory;
      DynamicBuffer<UIGroupElement> buffer;
      if (this.EntityManager.HasComponent<UIAssetCategoryData>(entity1) && this.EntityManager.TryGetBuffer<UIGroupElement>(entity1, true, out buffer))
      {
        NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, buffer, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FilterByThemes(objects, this.m_SelectedThemes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FilterByPacks(objects, this.m_SelectedAssetPacks);
        objects.Sort<UIObjectInfo>();
        writer.ArrayBegin(objects.Length);
        for (int index = 0; index < objects.Length; ++index)
        {
          Entity entity2 = objects[index].entity;
          // ISSUE: reference to a compiler-generated field
          bool uniquePlaced = this.m_UniqueAssetTrackingSystem.IsPlacedUniqueAsset(entity2);
          // ISSUE: reference to a compiler-generated method
          this.BindAsset(writer, entity2, uniquePlaced);
        }
        writer.ArrayEnd();
        objects.Dispose();
      }
      else
        writer.WriteEmptyArray();
    }

    public void BindAsset(IJsonWriter writer, Entity entity, bool uniquePlaced)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(entity);
      string str1 = (string) null;
      ContentPrerequisite component1;
      DlcRequirement component2;
      if (prefab.TryGet<ContentPrerequisite>(out component1) && component1.m_ContentPrerequisite.TryGet<DlcRequirement>(out component2))
        str1 = PlatformManager.instance.GetDlcName(component2.m_Dlc);
      string str2 = (string) null;
      DynamicBuffer<ObjectRequirementElement> buffer;
      if (this.EntityManager.TryGetBuffer<ObjectRequirementElement>(entity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity requirement = buffer[index].m_Requirement;
          if (this.EntityManager.HasComponent<ThemeData>(requirement))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            str2 = ImageSystem.GetIcon(this.m_PrefabSystem.GetPrefab<PrefabBase>(requirement)) ?? this.m_ImageSystem.placeholderIcon;
          }
        }
      }
      writer.TypeBegin("toolbar.Asset");
      writer.PropertyName(nameof (entity));
      writer.Write(entity);
      writer.PropertyName("name");
      writer.Write(prefab.name);
      writer.PropertyName("icon");
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      writer.Write(ImageSystem.GetThumbnail(prefab) ?? this.m_ImageSystem.placeholderIcon);
      writer.PropertyName("dlc");
      if (str1 != null)
        writer.Write("Media/DLC/" + str1 + ".svg");
      else
        writer.WriteNull();
      writer.PropertyName("theme");
      if (str2 != null)
        writer.Write(str2);
      else
        writer.WriteNull();
      writer.PropertyName("locked");
      writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
      writer.PropertyName("uiTag");
      writer.Write(prefab.uiTag);
      writer.PropertyName("highlight");
      writer.Write(this.EntityManager.HasComponent<UIHighlight>(entity));
      writer.PropertyName(nameof (uniquePlaced));
      writer.Write(uniquePlaced);
      writer.PropertyName("constructionCost");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabUISystem.BindConstructionCost(writer, entity);
      writer.TypeEnd();
    }

    private void BindThemes(IJsonWriter writer)
    {
      if (GameManager.instance.gameMode.IsEditor())
      {
        writer.WriteEmptyArray();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_SelectedAssetCategoryBinding.value;
        DynamicBuffer<UIGroupElement> buffer1;
        if (this.EntityManager.HasComponent<UIAssetCategoryData>(entity1) && this.EntityManager.TryGetBuffer<UIGroupElement>(entity1, true, out buffer1))
        {
          NativeParallelHashMap<Entity, bool> nativeParallelHashMap = new NativeParallelHashMap<Entity, bool>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
          for (int index1 = buffer1.Length - 1; index1 >= 0; --index1)
          {
            EntityManager entityManager = this.EntityManager;
            bool flag1 = entityManager.HasComponent<UIHighlight>(buffer1[index1].m_Prefab);
            DynamicBuffer<ObjectRequirementElement> buffer2;
            if (this.EntityManager.TryGetBuffer<ObjectRequirementElement>(buffer1[index1].m_Prefab, true, out buffer2))
            {
              for (int index2 = 0; index2 < buffer2.Length; ++index2)
              {
                Entity requirement = buffer2[index2].m_Requirement;
                entityManager = this.EntityManager;
                if (entityManager.HasComponent<ThemeData>(requirement))
                {
                  bool flag2;
                  nativeParallelHashMap[requirement] = !nativeParallelHashMap.TryGetValue(requirement, out flag2) ? flag1 : flag1 | flag2;
                }
              }
            }
          }
          if (!nativeParallelHashMap.IsEmpty)
          {
            // ISSUE: reference to a compiler-generated field
            NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.m_ThemeQuery, Allocator.TempJob);
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
              writer.TypeBegin("toolbar.Theme");
              writer.PropertyName("entity");
              IJsonWriter writer1 = writer;
              uiObjectInfo = sortedObjects[index];
              Entity entity2 = uiObjectInfo.entity;
              writer1.Write(entity2);
              writer.PropertyName("name");
              writer.Write(prefab.name);
              writer.PropertyName("icon");
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
              writer.PropertyName("highlight");
              IJsonWriter jsonWriter = writer;
              ref NativeParallelHashMap<Entity, bool> local1 = ref nativeParallelHashMap;
              uiObjectInfo = sortedObjects[index];
              Entity entity3 = uiObjectInfo.entity;
              bool flag;
              ref bool local2 = ref flag;
              int num = local1.TryGetValue(entity3, out local2) & flag ? 1 : 0;
              jsonWriter.Write(num != 0);
              writer.TypeEnd();
            }
            writer.ArrayEnd();
            sortedObjects.Dispose();
          }
          else
            writer.WriteEmptyArray();
          nativeParallelHashMap.Dispose();
        }
        else
          writer.WriteEmptyArray();
      }
    }

    private void SetSelectedThemes(List<Entity> themes)
    {
      // ISSUE: reference to a compiler-generated field
      Entity assetMenuEntity = this.m_SelectedAssetMenuBinding.value;
      // ISSUE: reference to a compiler-generated field
      Entity entity1 = this.m_SelectedAssetCategoryBinding.value;
      // ISSUE: reference to a compiler-generated field
      Entity entity2 = this.m_SelectedAssetBinding.value;
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedAssetPacks = this.m_SelectedAssetPacks;
      if (entity1 != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        entity2 = !(entity2 != Entity.Null) ? this.GetFirstUnlockedItem(entity1, themes, selectedAssetPacks) : this.GetClosestAssetInThemes(entity2, entity1, themes);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        if (!this.IsMatchingTheme(entity2, themes))
          entity2 = Entity.Null;
      }
      // ISSUE: reference to a compiler-generated method
      this.Apply(themes, selectedAssetPacks, assetMenuEntity, entity1, entity2, InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad);
    }

    private void SetAgeMask(int ageMask)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem.ageMask = (Game.Tools.AgeMask) ageMask;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AgeMaskBinding.Update(this.m_ToolSystem.activeTool != this.m_ObjectToolSystem || !this.m_ObjectToolSystem.allowAge ? 0 : (int) this.m_ObjectToolSystem.actualAgeMask);
    }

    private void SetSelectedAssetPacks(List<Entity> packs)
    {
      // ISSUE: reference to a compiler-generated field
      Entity assetMenuEntity = this.m_SelectedAssetMenuBinding.value;
      // ISSUE: reference to a compiler-generated field
      Entity entity1 = this.m_SelectedAssetCategoryBinding.value;
      // ISSUE: reference to a compiler-generated field
      Entity entity2 = this.m_SelectedAssetBinding.value;
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedThemes = this.m_SelectedThemes;
      if (entity1 != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        entity2 = !(entity2 != Entity.Null) ? this.GetFirstUnlockedItem(entity1, selectedThemes, packs) : this.GetClosestAssetInPacks(entity2, entity1, packs);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        if (!this.IsMatchingPack(entity2, packs))
          entity2 = Entity.Null;
      }
      // ISSUE: reference to a compiler-generated method
      this.Apply(selectedThemes, packs, assetMenuEntity, entity1, entity2, InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad);
    }

    private void BindPacks(IJsonWriter writer)
    {
      if (GameManager.instance.gameMode.IsEditor())
      {
        writer.WriteEmptyArray();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_SelectedAssetCategoryBinding.value;
        DynamicBuffer<UIGroupElement> buffer1;
        if (this.EntityManager.HasComponent<UIAssetCategoryData>(entity1) && this.EntityManager.TryGetBuffer<UIGroupElement>(entity1, true, out buffer1))
        {
          NativeParallelHashMap<Entity, bool> nativeParallelHashMap = new NativeParallelHashMap<Entity, bool>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
          for (int index1 = buffer1.Length - 1; index1 >= 0; --index1)
          {
            EntityManager entityManager = this.EntityManager;
            bool flag1 = entityManager.HasComponent<UIHighlight>(buffer1[index1].m_Prefab);
            DynamicBuffer<AssetPackElement> buffer2;
            if (this.EntityManager.TryGetBuffer<AssetPackElement>(buffer1[index1].m_Prefab, true, out buffer2))
            {
              for (int index2 = 0; index2 < buffer2.Length; ++index2)
              {
                Entity pack = buffer2[index2].m_Pack;
                entityManager = this.EntityManager;
                if (entityManager.HasComponent<AssetPackData>(pack))
                {
                  bool flag2;
                  nativeParallelHashMap[pack] = !nativeParallelHashMap.TryGetValue(pack, out flag2) ? flag1 : flag1 | flag2;
                }
              }
            }
          }
          if (!nativeParallelHashMap.IsEmpty)
          {
            // ISSUE: reference to a compiler-generated field
            NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.m_AssetPackQuery, Allocator.TempJob);
            UIObjectInfo uiObjectInfo;
            for (int index = sortedObjects.Length - 1; index >= 0; --index)
            {
              ref NativeParallelHashMap<Entity, bool> local = ref nativeParallelHashMap;
              uiObjectInfo = sortedObjects[index];
              Entity entity2 = uiObjectInfo.entity;
              if (!local.ContainsKey(entity2))
                sortedObjects.RemoveAt(index);
            }
            writer.ArrayBegin(sortedObjects.Length);
            for (int index = 0; index < sortedObjects.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              PrefabSystem prefabSystem = this.m_PrefabSystem;
              uiObjectInfo = sortedObjects[index];
              PrefabData prefabData = uiObjectInfo.prefabData;
              // ISSUE: reference to a compiler-generated method
              AssetPackPrefab prefab = prefabSystem.GetPrefab<AssetPackPrefab>(prefabData);
              writer.TypeBegin("toolbar.AssetPack");
              writer.PropertyName("entity");
              IJsonWriter writer1 = writer;
              uiObjectInfo = sortedObjects[index];
              Entity entity3 = uiObjectInfo.entity;
              writer1.Write(entity3);
              writer.PropertyName("name");
              writer.Write(prefab.name);
              writer.PropertyName("icon");
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
              writer.PropertyName("highlight");
              IJsonWriter jsonWriter = writer;
              ref NativeParallelHashMap<Entity, bool> local1 = ref nativeParallelHashMap;
              uiObjectInfo = sortedObjects[index];
              Entity entity4 = uiObjectInfo.entity;
              bool flag;
              ref bool local2 = ref flag;
              int num = local1.TryGetValue(entity4, out local2) & flag ? 1 : 0;
              jsonWriter.Write(num != 0);
              writer.TypeEnd();
            }
            writer.ArrayEnd();
            sortedObjects.Dispose();
          }
          else
            writer.WriteEmptyArray();
          nativeParallelHashMap.Dispose();
        }
        else
          writer.WriteEmptyArray();
      }
    }

    private void SelectAssetMenu(Entity assetMenu)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetPacks = new List<Entity>();
      Entity entity1 = assetMenu;
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedThemes = this.m_SelectedThemes;
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedAssetPacks = this.m_SelectedAssetPacks;
      if (!(entity1 != Entity.Null) || !this.EntityManager.HasComponent<UIAssetMenuData>(entity1))
        return;
      Entity firstItem;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LastSelectedCategories.TryGetValue(entity1, out firstItem))
      {
        // ISSUE: reference to a compiler-generated method
        firstItem = this.GetFirstItem(entity1, selectedThemes, selectedAssetPacks);
      }
      Entity entity2 = Entity.Null;
      if (firstItem != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        entity2 = !this.m_LastSelectedAssets.TryGetValue(firstItem, out entity2) ? this.GetFirstUnlockedItem(firstItem, selectedThemes, selectedAssetPacks) : this.GetClosestAssetInPacks(this.GetClosestAssetInThemes(entity2, firstItem, selectedThemes), firstItem, selectedAssetPacks);
      }
      // ISSUE: reference to a compiler-generated method
      this.Apply(selectedThemes, selectedAssetPacks, entity1, firstItem, entity2, InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad);
    }

    private void SelectAssetCategory(Entity assetCategory)
    {
      Entity entity = assetCategory;
      UIAssetCategoryData component;
      if (!(entity != Entity.Null) || !this.EntityManager.TryGetComponent<UIAssetCategoryData>(entity, out component))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetPacks = new List<Entity>();
      Entity menu = component.m_Menu;
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedThemes = this.m_SelectedThemes;
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedAssetPacks = this.m_SelectedAssetPacks;
      Entity oldAssetEntity;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      Entity assetEntity = !this.m_LastSelectedAssets.TryGetValue(entity, out oldAssetEntity) ? this.GetFirstUnlockedItem(entity, selectedThemes, selectedAssetPacks) : this.GetClosestAssetInPacks(this.GetClosestAssetInThemes(oldAssetEntity, entity, selectedThemes), entity, selectedAssetPacks);
      if (assetEntity == Entity.Null)
      {
        selectedAssetPacks.Clear();
        // ISSUE: reference to a compiler-generated method
        assetEntity = this.GetFirstUnlockedItem(entity, selectedThemes, selectedAssetPacks);
      }
      // ISSUE: reference to a compiler-generated method
      this.Apply(selectedThemes, selectedAssetPacks, menu, entity, assetEntity, InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad);
    }

    private void SelectAsset(Entity assetEntity, bool updateTool)
    {
      // ISSUE: reference to a compiler-generated field
      List<Entity> themes = this.m_SelectedThemes;
      // ISSUE: reference to a compiler-generated field
      List<Entity> packs = this.m_SelectedAssetPacks;
      // ISSUE: reference to a compiler-generated method
      if (!this.IsMatchingTheme(assetEntity, themes))
      {
        // ISSUE: reference to a compiler-generated field
        NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.m_ThemeQuery, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        themes = this.FilterThemesByAsset(sortedObjects, assetEntity);
        sortedObjects.Dispose();
      }
      // ISSUE: reference to a compiler-generated method
      if (!this.IsMatchingPack(assetEntity, packs))
      {
        // ISSUE: reference to a compiler-generated field
        NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, this.m_AssetPackQuery, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        packs = this.FilterPacksByAsset(sortedObjects, assetEntity);
        sortedObjects.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      Entity menu = this.m_SelectedAssetMenuBinding.value;
      // ISSUE: reference to a compiler-generated field
      Entity group = this.m_SelectedAssetCategoryBinding.value;
      UIObjectData component1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.IsMatchingAssetCategory(assetEntity, this.m_SelectedAssetCategoryBinding.value) && assetEntity != Entity.Null && this.EntityManager.TryGetComponent<UIObjectData>(assetEntity, out component1))
      {
        UIAssetCategoryData component2;
        if (component1.m_Group != Entity.Null && this.EntityManager.TryGetComponent<UIAssetCategoryData>(component1.m_Group, out component2))
        {
          menu = component2.m_Menu;
          group = component1.m_Group;
        }
        else
        {
          menu = Entity.Null;
          group = Entity.Null;
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.Apply(themes, packs, menu, group, assetEntity, updateTool);
    }

    public void ClearAssetSelection() => this.ClearAssetSelection(true);

    private void ClearAssetSelection(bool updateTool)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.Apply(this.m_SelectedThemes, this.m_SelectedAssetPacks, Entity.Null, Entity.Null, Entity.Null, updateTool);
    }

    private void Apply(
      List<Entity> themes,
      List<Entity> packs,
      Entity assetMenuEntity,
      Entity assetCategoryEntity,
      Entity assetEntity,
      bool updateTool = false)
    {
      if (updateTool)
      {
        // ISSUE: reference to a compiler-generated method
        this.ActivatePrefabTool(assetEntity);
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateHighlights(themes, packs, assetMenuEntity, assetCategoryEntity, assetEntity);
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedThemes = this.m_SelectedThemes;
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedAssetPacks = this.m_SelectedAssetPacks;
      // ISSUE: reference to a compiler-generated field
      Entity entity = this.m_SelectedAssetCategoryBinding.value;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedThemes = themes;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedThemesBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetPacks = packs;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetPacksBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetMenuBinding.Update(assetMenuEntity);
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetCategoryBinding.Update(assetCategoryEntity);
      if (updateTool)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedAssetBinding.Update(assetEntity);
      }
      if (assetMenuEntity != Entity.Null && assetCategoryEntity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastSelectedCategories[assetMenuEntity] = assetCategoryEntity;
      }
      if (assetCategoryEntity != Entity.Null && assetEntity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastSelectedAssets[assetCategoryEntity] = assetEntity;
      }
      if (!selectedThemes.SequenceEqual<Entity>((IEnumerable<Entity>) themes) || !selectedAssetPacks.SequenceEqual<Entity>((IEnumerable<Entity>) packs))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AssetsBinding.UpdateAll();
      }
      if (!selectedThemes.SequenceEqual<Entity>((IEnumerable<Entity>) themes) || entity != assetCategoryEntity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ThemesBinding.Update();
      }
      if (!selectedAssetPacks.SequenceEqual<Entity>((IEnumerable<Entity>) packs) || entity != assetCategoryEntity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AssetPacksBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem && this.m_ObjectToolSystem.allowAge)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AgeMaskBinding.Update((int) this.m_ObjectToolSystem.actualAgeMask);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AgeMaskBinding.Update(0);
      }
    }

    private void UpdateHighlights(
      List<Entity> themes,
      List<Entity> packs,
      Entity assetMenuEntity,
      Entity assetCategoryEntity,
      Entity assetEntity)
    {
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedThemes = this.m_SelectedThemes;
      // ISSUE: reference to a compiler-generated field
      List<Entity> selectedAssetPacks = this.m_SelectedAssetPacks;
      // ISSUE: reference to a compiler-generated field
      Entity entity1 = this.m_SelectedAssetMenuBinding.value;
      // ISSUE: reference to a compiler-generated field
      Entity entity2 = this.m_SelectedAssetCategoryBinding.value;
      // ISSUE: reference to a compiler-generated field
      Entity entity3 = this.m_SelectedAssetBinding.value;
      if (entity2 != Entity.Null && (selectedThemes.Count > 0 && !selectedThemes.SequenceEqual<Entity>((IEnumerable<Entity>) themes) || selectedAssetPacks.Count > 0 && !selectedAssetPacks.SequenceEqual<Entity>((IEnumerable<Entity>) packs)) || entity2 != assetCategoryEntity)
      {
        bool flag1 = this.EntityManager.HasComponent<UIHighlight>(entity2);
        DynamicBuffer<UIGroupElement> buffer1;
        if (this.EntityManager.TryGetBuffer<UIGroupElement>(entity2, true, out buffer1))
        {
          NativeList<Entity> nativeList = new NativeList<Entity>(buffer1.Length, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
          for (int index = 0; index < buffer1.Length; ++index)
          {
            Entity prefab = buffer1[index].m_Prefab;
            if (this.EntityManager.HasComponent<UIHighlight>(prefab))
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (this.IsMatchingTheme(prefab, selectedThemes) || this.IsMatchingPack(prefab, selectedAssetPacks))
                nativeList.Add(in prefab);
              else
                flag1 = false;
            }
          }
          this.EntityManager.RemoveComponent<UIHighlight>(nativeList.AsArray());
          nativeList.Dispose();
        }
        if (flag1)
        {
          this.EntityManager.RemoveComponent<UIHighlight>(entity2);
          // ISSUE: reference to a compiler-generated field
          this.m_AssetMenuCategoriesBinding.UpdateAll();
        }
        if (this.EntityManager.HasComponent<UIHighlight>(entity1))
        {
          bool flag2 = true;
          DynamicBuffer<UIGroupElement> buffer2;
          if (this.EntityManager.TryGetBuffer<UIGroupElement>(entity1, true, out buffer2))
          {
            for (int index = 0; index < buffer2.Length; ++index)
            {
              if (this.EntityManager.HasComponent<UIHighlight>(buffer2[index].m_Prefab))
              {
                flag2 = false;
                break;
              }
            }
          }
          if (flag2)
          {
            this.EntityManager.RemoveComponent<UIHighlight>(entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_ToolbarGroupsBinding.Update();
          }
        }
      }
      if (!(entity1 == Entity.Null) || !(entity2 == Entity.Null) || !(assetEntity != entity3) || !this.EntityManager.HasComponent<UIHighlight>(entity3))
        return;
      this.EntityManager.RemoveComponent<UIHighlight>(entity3);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolbarGroupsBinding.Update();
    }

    private void ActivatePrefabTool(Entity assetEntity)
    {
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (assetEntity != Entity.Null && !this.EntityManager.HasEnabledComponent<Locked>(assetEntity) && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(assetEntity, out prefab))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.ActivatePrefabTool(prefab);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultTool;
      }
    }

    private bool IsMatchingTheme(Entity assetEntity, List<Entity> themes)
    {
      if (assetEntity == Entity.Null)
        return false;
      DynamicBuffer<ObjectRequirementElement> buffer;
      if (themes.Count <= 0 || !this.EntityManager.TryGetBuffer<ObjectRequirementElement>(assetEntity, true, out buffer))
        return true;
      bool flag = false;
      for (int index1 = 0; index1 < buffer.Length; ++index1)
      {
        Entity requirement = buffer[index1].m_Requirement;
        for (int index2 = 0; index2 < themes.Count; ++index2)
        {
          if (requirement == themes[index2])
            return true;
          if (this.EntityManager.HasComponent<ThemeData>(requirement))
            flag = true;
        }
      }
      return !flag;
    }

    private bool IsMatchingPack(Entity assetEntity, List<Entity> packs)
    {
      if (assetEntity == Entity.Null)
        return false;
      DynamicBuffer<AssetPackElement> buffer;
      if (packs.Count <= 0 || !this.EntityManager.TryGetBuffer<AssetPackElement>(assetEntity, true, out buffer))
        return packs.Count == 0;
      bool flag = false;
      for (int index1 = 0; index1 < buffer.Length; ++index1)
      {
        Entity pack = buffer[index1].m_Pack;
        for (int index2 = 0; index2 < packs.Count; ++index2)
        {
          if (pack == packs[index2])
            return true;
          if (this.EntityManager.HasComponent<AssetPackData>(pack))
            flag = true;
        }
      }
      return !flag;
    }

    private bool IsMatchingAssetCategory(Entity assetEntity, Entity assetCategoryEntity)
    {
      UIObjectData component;
      return assetEntity != Entity.Null && this.EntityManager.TryGetComponent<UIObjectData>(assetEntity, out component) && component.m_Group == assetCategoryEntity;
    }

    private Entity GetFirstTheme(Entity assetEntity)
    {
      DynamicBuffer<ObjectRequirementElement> buffer;
      if (assetEntity != Entity.Null && this.EntityManager.TryGetBuffer<ObjectRequirementElement>(assetEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity requirement = buffer[index].m_Requirement;
          if (this.EntityManager.HasComponent<ThemeData>(requirement))
            return requirement;
        }
      }
      return Entity.Null;
    }

    private Entity GetFirstPack(Entity assetEntity)
    {
      DynamicBuffer<AssetPackElement> buffer;
      if (assetEntity != Entity.Null && this.EntityManager.TryGetBuffer<AssetPackElement>(assetEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity pack = buffer[index].m_Pack;
          if (this.EntityManager.HasComponent<AssetPackData>(pack))
            return pack;
        }
      }
      return Entity.Null;
    }

    private Entity GetClosestAssetInThemes(
      Entity oldAssetEntity,
      Entity groupEntity,
      List<Entity> themes)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.IsMatchingTheme(oldAssetEntity, themes))
        return oldAssetEntity;
      // ISSUE: reference to a compiler-generated method
      Entity firstTheme = this.GetFirstTheme(oldAssetEntity);
      DynamicBuffer<UIGroupElement> buffer;
      if (firstTheme != Entity.Null && this.EntityManager.TryGetBuffer<UIGroupElement>(groupEntity, true, out buffer))
      {
        for (int index1 = 0; index1 < themes.Count; ++index1)
        {
          Entity theme = themes[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab1 = this.m_PrefabSystem.GetPrefab<PrefabBase>(oldAssetEntity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ThemePrefab prefab2 = this.m_PrefabSystem.GetPrefab<ThemePrefab>(firstTheme);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ThemePrefab prefab3 = this.m_PrefabSystem.GetPrefab<ThemePrefab>(theme);
          if (prefab1.name.StartsWith(prefab2.assetPrefix))
          {
            string str1 = prefab1.name.Substring(prefab2.assetPrefix.Length);
            string str2 = prefab3.assetPrefix + str1;
            NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, buffer, Allocator.TempJob);
            // ISSUE: reference to a compiler-generated method
            this.FilterByTheme(objects, theme);
            try
            {
              for (int index2 = 0; index2 < objects.Length; ++index2)
              {
                UIObjectInfo uiObjectInfo = objects[index2];
                Entity entity = uiObjectInfo.entity;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                PrefabSystem prefabSystem = this.m_PrefabSystem;
                uiObjectInfo = objects[index2];
                PrefabData prefabData = uiObjectInfo.prefabData;
                // ISSUE: reference to a compiler-generated method
                if (prefabSystem.GetPrefab<PrefabBase>(prefabData).name == str2)
                  return entity;
              }
              for (int index3 = 0; index3 < objects.Length; ++index3)
              {
                UIObjectInfo uiObjectInfo = objects[index3];
                Entity entity = uiObjectInfo.entity;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                PrefabSystem prefabSystem = this.m_PrefabSystem;
                uiObjectInfo = objects[index3];
                PrefabData prefabData = uiObjectInfo.prefabData;
                // ISSUE: reference to a compiler-generated method
                if (prefabSystem.GetPrefab<PrefabBase>(prefabData).name == str1)
                  return entity;
              }
            }
            finally
            {
              objects.Dispose();
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.GetFirstUnlockedItem(groupEntity, themes, this.m_SelectedAssetPacks);
    }

    private Entity GetClosestAssetInPacks(
      Entity oldAssetEntity,
      Entity groupEntity,
      List<Entity> packs)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.IsMatchingPack(oldAssetEntity, packs))
        return oldAssetEntity;
      DynamicBuffer<UIGroupElement> buffer;
      if (this.EntityManager.TryGetBuffer<UIGroupElement>(groupEntity, true, out buffer))
      {
        NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, buffer, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        this.FilterByPacks(objects, packs);
        try
        {
          for (int index = 0; index < objects.Length; ++index)
          {
            Entity entity = objects[index].entity;
            // ISSUE: reference to a compiler-generated method
            if (this.IsMatchingPack(entity, packs))
              return entity;
          }
        }
        finally
        {
          objects.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.GetFirstUnlockedItem(groupEntity, this.m_SelectedThemes, packs);
    }

    private Entity GetFirstItem(Entity groupEntity, List<Entity> themes, List<Entity> packs)
    {
      DynamicBuffer<UIGroupElement> buffer;
      if (this.EntityManager.TryGetBuffer<UIGroupElement>(groupEntity, true, out buffer))
      {
        NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, buffer, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        this.FilterByThemes(objects, themes);
        // ISSUE: reference to a compiler-generated method
        this.FilterByPacks(objects, packs);
        objects.Sort<UIObjectInfo>();
        try
        {
          if (objects.Length > 0)
            return objects[0].entity;
        }
        finally
        {
          objects.Dispose();
        }
      }
      return Entity.Null;
    }

    private Entity GetFirstUnlockedItem(
      Entity groupEntity,
      List<Entity> themes,
      List<Entity> packs)
    {
      DynamicBuffer<UIGroupElement> buffer;
      if (this.EntityManager.TryGetBuffer<UIGroupElement>(groupEntity, true, out buffer))
      {
        NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, buffer, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        this.FilterByThemes(objects, themes);
        // ISSUE: reference to a compiler-generated method
        this.FilterByPacks(objects, packs);
        objects.Sort<UIObjectInfo>();
        try
        {
          for (int index = 0; index < objects.Length; ++index)
          {
            Entity entity = objects[index].entity;
            if (!this.EntityManager.HasEnabledComponent<Locked>(entity))
              return entity;
          }
          if (objects.Length > 0)
            return objects[0].entity;
        }
        finally
        {
          objects.Dispose();
        }
      }
      return Entity.Null;
    }

    private NativeArray<UIObjectInfo> GetSortedToolbarGroups()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ToolbarGroupQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<UIToolbarGroupData> componentDataArray = this.m_ToolbarGroupQuery.ToComponentDataArray<UIToolbarGroupData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      int length = entityArray.Length;
      NativeArray<UIObjectInfo> array = new NativeArray<UIObjectInfo>(length, Allocator.Temp);
      for (int index = 0; index < length; ++index)
        array[index] = new UIObjectInfo(entityArray[index], componentDataArray[index].m_Priority);
      array.Sort<UIObjectInfo>();
      entityArray.Dispose();
      componentDataArray.Dispose();
      return array;
    }

    private void FilterByTheme(NativeList<UIObjectInfo> elementInfos, Entity themeEntity)
    {
      if (themeEntity == Entity.Null)
        return;
      for (int index1 = elementInfos.Length - 1; index1 >= 0; --index1)
      {
        DynamicBuffer<ObjectRequirementElement> buffer;
        if (this.EntityManager.TryGetBuffer<ObjectRequirementElement>(elementInfos[index1].entity, true, out buffer))
        {
          bool flag = false;
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            Entity requirement = buffer[index2].m_Requirement;
            if (requirement == themeEntity)
            {
              flag = false;
              break;
            }
            if (this.EntityManager.HasComponent<ThemeData>(requirement))
              flag = true;
          }
          if (flag)
            elementInfos.RemoveAtSwapBack(index1);
        }
      }
    }

    private List<Entity> FilterThemesByAsset(NativeList<UIObjectInfo> themes, Entity asset)
    {
      if (asset == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_SelectedThemes;
      }
      for (int index1 = themes.Length - 1; index1 >= 0; --index1)
      {
        Entity entity = themes[index1].entity;
        DynamicBuffer<ObjectRequirementElement> buffer;
        if (this.EntityManager.TryGetBuffer<ObjectRequirementElement>(asset, true, out buffer))
        {
          bool flag = false;
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            Entity requirement = buffer[index2].m_Requirement;
            if (requirement == entity)
            {
              flag = false;
              break;
            }
            if (this.EntityManager.HasComponent<ThemeData>(requirement))
              flag = true;
          }
          if (flag)
            themes.RemoveAtSwapBack(index1);
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedThemes.Clear();
      for (int index = 0; index < themes.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedThemes.Add(themes[index].entity);
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_SelectedThemes;
    }

    private List<Entity> FilterPacksByAsset(NativeList<UIObjectInfo> assetPacks, Entity asset)
    {
      if (asset == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_SelectedAssetPacks;
      }
      for (int index1 = assetPacks.Length - 1; index1 >= 0; --index1)
      {
        Entity entity = assetPacks[index1].entity;
        bool flag = true;
        DynamicBuffer<AssetPackElement> buffer;
        if (this.EntityManager.TryGetBuffer<AssetPackElement>(asset, true, out buffer))
        {
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            if (buffer[index2].m_Pack == entity)
            {
              flag = false;
              break;
            }
          }
        }
        if (flag)
          assetPacks.RemoveAtSwapBack(index1);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedAssetPacks.Clear();
      for (int index = 0; index < assetPacks.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedAssetPacks.Add(assetPacks[index].entity);
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_SelectedAssetPacks;
    }

    private void FilterByPack(NativeList<UIObjectInfo> elementInfos, Entity packEntity)
    {
      if (packEntity == Entity.Null)
        return;
      for (int index1 = elementInfos.Length - 1; index1 >= 0; --index1)
      {
        Entity entity = elementInfos[index1].entity;
        bool flag = true;
        DynamicBuffer<AssetPackElement> buffer;
        if (this.EntityManager.TryGetBuffer<AssetPackElement>(entity, true, out buffer))
        {
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            if (buffer[index2].m_Pack == packEntity)
            {
              flag = false;
              break;
            }
          }
        }
        if (flag)
          elementInfos.RemoveAtSwapBack(index1);
      }
    }

    private void FilterByThemes(NativeList<UIObjectInfo> elementInfos, List<Entity> themes)
    {
      for (int index1 = elementInfos.Length - 1; index1 >= 0; --index1)
      {
        DynamicBuffer<ObjectRequirementElement> buffer;
        if (this.EntityManager.TryGetBuffer<ObjectRequirementElement>(elementInfos[index1].entity, true, out buffer))
        {
          bool flag = false;
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            Entity requirement = buffer[index2].m_Requirement;
            if (this.EntityManager.HasComponent<ThemeData>(requirement))
              flag = true;
            for (int index3 = 0; index3 < themes.Count; ++index3)
            {
              Entity theme = themes[index3];
              if (requirement == theme)
              {
                flag = false;
                break;
              }
            }
            if (!flag)
              break;
          }
          if (flag)
            elementInfos.RemoveAtSwapBack(index1);
        }
      }
    }

    private void FilterByPacks(NativeList<UIObjectInfo> elementInfos, List<Entity> packs)
    {
      if (packs == null || packs.Count == 0)
        return;
      for (int index1 = elementInfos.Length - 1; index1 >= 0; --index1)
      {
        Entity entity = elementInfos[index1].entity;
        bool flag = true;
        DynamicBuffer<AssetPackElement> buffer;
        if (this.EntityManager.TryGetBuffer<AssetPackElement>(entity, true, out buffer))
        {
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            Entity pack1 = buffer[index2].m_Pack;
            for (int index3 = 0; index3 < packs.Count; ++index3)
            {
              Entity pack2 = packs[index3];
              if (pack1 == pack2)
              {
                flag = false;
                break;
              }
            }
            if (!flag)
              break;
          }
        }
        if (flag)
          elementInfos.RemoveAtSwapBack(index1);
      }
    }

    private void ToggleToolOptions(bool enabled)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.activeTool?.ToggleToolOptions(enabled);
    }

    [Preserve]
    public ToolbarUISystem()
    {
    }

    private enum ToolbarItemType
    {
      Asset,
      Menu,
    }
  }
}
