// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.PrefabToolPanelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
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
  public class PrefabToolPanelSystem : EditorPanelSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private EditorAssetCategorySystem m_CategorySystem;
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_ModifiedPrefabQuery;
    private PrefabPickerAdapter m_Adapter;
    private HierarchyMenu<EditorAssetCategory> m_CategoryMenu;
    private EditorAssetCategory m_AllCategory;
    private PrefabToolPanelSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CategorySystem = this.World.GetOrCreateSystemManaged<EditorAssetCategorySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedPrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_Adapter = new PrefabPickerAdapter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Adapter.EventPrefabSelected += (Action<PrefabBase>) (prefab => this.m_ToolSystem.ActivatePrefabTool(this.m_Adapter.selectedPrefab));
      this.title = (LocalizedString) "Editor.TOOL[PrefabTool]";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      IWidget[] widgetArray1 = new IWidget[4]
      {
        (IWidget) new PopupSearchField()
        {
          adapter = (PopupSearchField.IAdapter) this.m_Adapter
        },
        (IWidget) new FilterMenu()
        {
          adapter = (FilterMenu.IAdapter) this.m_Adapter
        },
        null,
        null
      };
      Row row1 = new Row();
      row1.flex = FlexLayout.Fill;
      Row row2 = row1;
      IWidget[] widgetArray2 = new IWidget[2];
      HierarchyMenu<EditorAssetCategory> hierarchyMenu1 = new HierarchyMenu<EditorAssetCategory>();
      hierarchyMenu1.selectionType = HierarchyMenu.SelectionType.singleSelection;
      hierarchyMenu1.onSelectionChange = (System.Action) (() =>
      {
        EditorAssetCategory selection;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CategoryMenu.GetSelectedItem(out selection))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Adapter.SetPrefabs((ICollection<PrefabBase>) selection.GetPrefabs(this.EntityManager, this.m_PrefabSystem, this.__TypeHandle.__Unity_Entities_Entity_TypeHandle));
      });
      hierarchyMenu1.flex = new FlexLayout(1f, 0.0f, 0);
      hierarchyMenu1.path = (PathSegment) "PrefabToolCategories";
      HierarchyMenu<EditorAssetCategory> hierarchyMenu2 = hierarchyMenu1;
      // ISSUE: reference to a compiler-generated field
      this.m_CategoryMenu = hierarchyMenu1;
      widgetArray2[0] = (IWidget) hierarchyMenu2;
      // ISSUE: reference to a compiler-generated field
      widgetArray2[1] = (IWidget) new ItemPicker<PrefabItem>()
      {
        adapter = (ItemPicker<PrefabItem>.IAdapter) this.m_Adapter,
        hasFavorites = true,
        flex = new FlexLayout(2f, 0.0f, 0),
        selectOnFocus = true
      };
      row2.children = (IList<IWidget>) widgetArray2;
      widgetArray1[2] = (IWidget) row1;
      // ISSUE: reference to a compiler-generated field
      widgetArray1[3] = (IWidget) new ItemPickerFooter()
      {
        adapter = (ItemPickerFooter.IAdapter) this.m_Adapter
      };
      this.children = (IList<IWidget>) widgetArray1;
      // ISSUE: reference to a compiler-generated method
      this.GenerateCategories();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated field
      this.m_Adapter.searchQuery = string.Empty;
      // ISSUE: reference to a compiler-generated field
      this.m_Adapter.selectedPrefab = (PrefabBase) null;
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_Adapter.LoadSettings();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CategoryMenu.items = this.GetHierarchy();
      // ISSUE: reference to a compiler-generated method
      this.UpdatePrefabs();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool(this.m_Adapter.selectedPrefab);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ModifiedPrefabQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdatePrefabs();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Adapter.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Adapter.selectedPrefab = this.m_ToolSystem.activePrefab;
    }

    protected override bool OnCancel()
    {
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_Adapter.selectedPrefab != (UnityEngine.Object) null))
        return base.OnCancel();
      // ISSUE: reference to a compiler-generated field
      this.m_Adapter.selectedPrefab = (PrefabBase) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool((PrefabBase) null);
      return false;
    }

    private void OnPrefabSelected(PrefabBase prefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool(this.m_Adapter.selectedPrefab);
    }

    private void UpdatePrefabs()
    {
      EditorAssetCategory selection;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CategoryMenu.GetSelectedItem(out selection))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Adapter.SetPrefabs((ICollection<PrefabBase>) selection.GetPrefabs(this.EntityManager, this.m_PrefabSystem, this.__TypeHandle.__Unity_Entities_Entity_TypeHandle));
    }

    private void GenerateCategories()
    {
      EditorAssetCategory editorAssetCategory = new EditorAssetCategory();
      editorAssetCategory.id = "All";
      editorAssetCategory.path = "All";
      editorAssetCategory.entityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[6]
        {
          ComponentType.ReadOnly<ObjectData>(),
          ComponentType.ReadOnly<EffectData>(),
          ComponentType.ReadOnly<ActivityLocationData>(),
          ComponentType.ReadOnly<NetData>(),
          ComponentType.ReadOnly<NetLaneData>(),
          ComponentType.ReadOnly<AreaData>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<BrandObjectData>(),
          ComponentType.ReadOnly<CarLaneData>(),
          ComponentType.ReadOnly<TrackLaneData>(),
          ComponentType.ReadOnly<ConnectionLaneData>()
        }
      });
      editorAssetCategory.defaultSelection = true;
      editorAssetCategory.includeChildCategories = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AllCategory = editorAssetCategory;
    }

    private IEnumerable<HierarchyItem<EditorAssetCategory>> GetHierarchy()
    {
      // ISSUE: reference to a compiler-generated field
      yield return this.m_AllCategory.ToHierarchyItem();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      foreach (HierarchyItem<EditorAssetCategory> hierarchyItem in this.m_CategorySystem.GetHierarchy())
        yield return hierarchyItem;
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
    public PrefabToolPanelSystem()
    {
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
