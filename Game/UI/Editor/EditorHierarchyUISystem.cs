// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorHierarchyUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using Game.Settings;
using Game.Tools;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class EditorHierarchyUISystem : UISystemBase
  {
    private const string kGroup = "editorHierarchy";
    public Action<Entity> onSave;
    public Action<Entity> onBulldoze;
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private EditorToolUISystem m_EditorToolUISystem;
    private EditorPanelUISystem m_EditorPanelUISystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private EntityQuery m_ObjectQuery;
    private EntityQuery m_ModifiedQuery;
    private GetterValueBinding<EditorHierarchyUISystem.Viewport> m_ViewportBinding;
    private NativeList<EditorHierarchyUISystem.HierarchyItem> m_Hierarchy;
    private NativeParallelHashSet<EditorHierarchyUISystem.ItemId> m_ExpandedIds;
    private int m_TotalCount;
    private EditorHierarchyUISystem.ItemId m_SelectedId;
    private EditorHierarchyUISystem.Viewport m_Viewport;
    private int m_NextViewportStartIndex;
    private int m_NextViewportEndIndex;
    private bool m_Dirty;
    private ValueBinding<int> m_CameraMode;
    private EditorHierarchyUISystem.TypeHandle __TypeHandle;

    public override GameMode gameMode => GameMode.Editor;

    public List<EditorHierarchyUISystem.PanelItem> panelItems { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EditorToolUISystem = this.World.GetOrCreateSystemManaged<EditorToolUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EditorPanelUISystem = this.World.GetOrCreateSystemManaged<EditorPanelUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Game.Objects.Object>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Game.Objects.Object>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("editorHierarchy", "width", (Func<int>) (() =>
      {
        EditorSettings editor = SharedSettings.instance?.editor;
        return editor == null ? 350 : editor.hierarchyWidth;
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("editorHierarchy", "totalCount", (Func<int>) (() => this.m_TotalCount)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<EditorHierarchyUISystem.ItemId>("editorHierarchy", "selectedId", (Func<EditorHierarchyUISystem.ItemId>) (() => this.m_SelectedId), (IWriter<EditorHierarchyUISystem.ItemId>) new ValueWriter<EditorHierarchyUISystem.ItemId>()));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ViewportBinding = new GetterValueBinding<EditorHierarchyUISystem.Viewport>("editorHierarchy", "viewport", (Func<EditorHierarchyUISystem.Viewport>) (() => this.m_Viewport), (IWriter<EditorHierarchyUISystem.Viewport>) new ValueWriter<EditorHierarchyUISystem.Viewport>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CameraMode = new ValueBinding<int>("editorHierarchy", "cameraMode", 0)));
      this.AddBinding((IBinding) new TriggerBinding<int>("editorHierarchy", "setWidth", (Action<int>) (width =>
      {
        EditorSettings editor = SharedSettings.instance?.editor;
        if (editor == null)
          return;
        editor.hierarchyWidth = width;
      })));
      this.AddBinding((IBinding) new TriggerBinding<int, int>("editorHierarchy", "setViewportRange", (Action<int, int>) ((startIndex, endIndex) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NextViewportStartIndex = startIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_NextViewportEndIndex = endIndex;
      })));
      this.AddBinding((IBinding) new TriggerBinding<EditorHierarchyUISystem.ItemId>("editorHierarchy", "setSelectedId", (Action<EditorHierarchyUISystem.ItemId>) (id =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedId = id;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        EditorHierarchyUISystem.ItemType type = id.type;
        switch (type)
        {
          case EditorHierarchyUISystem.ItemType.Object:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ToolSystem.selected = id.entity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_EditorToolUISystem.SelectEntity(id.entity);
            break;
          case EditorHierarchyUISystem.ItemType.SubMesh:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ToolSystem.selected = id.entity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_EditorToolUISystem.SelectEntitySubMesh(id.entity, id.subIndex);
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            this.m_ToolSystem.selected = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_EditorPanelUISystem.activePanel = this.panelItems.FirstOrDefault<EditorHierarchyUISystem.PanelItem>((Func<EditorHierarchyUISystem.PanelItem, bool>) (p => p.type == id.type))?.panel;
            break;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.RefreshCameraController((EditorHierarchyUISystem.CameraMode) this.m_CameraMode.value);
      }), (IReader<EditorHierarchyUISystem.ItemId>) new ValueReader<EditorHierarchyUISystem.ItemId>()));
      this.AddBinding((IBinding) new TriggerBinding<EditorHierarchyUISystem.ItemId, bool>("editorHierarchy", "setExpanded", (Action<EditorHierarchyUISystem.ItemId, bool>) ((id, expanded) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Dirty = true;
        if (expanded)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ExpandedIds.Add(id);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ExpandedIds.Remove(id);
        }
      }), (IReader<EditorHierarchyUISystem.ItemId>) new ValueReader<EditorHierarchyUISystem.ItemId>()));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<int>("editorHierarchy", "toggleCameraMode", (Action<int>) (mode => this.ToggleCameraMode((EditorHierarchyUISystem.CameraMode) mode))));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("editorHierarchy", "save", (Action<Entity>) (entity =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        EditorPrefabUtils.SavePrefab(this.m_PrefabSystem.GetPrefab<PrefabBase>(this.EntityManager.GetComponentData<PrefabRef>(entity)));
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.IMadeThis);
        // ISSUE: reference to a compiler-generated field
        Action<Entity> onSave = this.onSave;
        if (onSave == null)
          return;
        onSave(entity);
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("editorHierarchy", "bulldoze", (Action<Entity>) (entity =>
      {
        // ISSUE: reference to a compiler-generated field
        Action<Entity> onBulldoze = this.onBulldoze;
        if (onBulldoze == null)
          return;
        onBulldoze(entity);
      })));
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: object of a compiler-generated type is created
      this.panelItems = new List<EditorHierarchyUISystem.PanelItem>()
      {
        new EditorHierarchyUISystem.PanelItem(EditorHierarchyUISystem.ItemType.Map, (byte) 0, (IEditorPanel) this.World.GetOrCreateSystemManaged<MapPanelSystem>()),
        new EditorHierarchyUISystem.PanelItem(EditorHierarchyUISystem.ItemType.Climate, (byte) 1, (IEditorPanel) this.World.GetOrCreateSystemManaged<ClimatePanelSystem>()),
        new EditorHierarchyUISystem.PanelItem(EditorHierarchyUISystem.ItemType.Water, (byte) 1, (IEditorPanel) this.World.GetOrCreateSystemManaged<WaterPanelSystem>()),
        new EditorHierarchyUISystem.PanelItem(EditorHierarchyUISystem.ItemType.Resources, (byte) 1, (IEditorPanel) this.World.GetOrCreateSystemManaged<ResourcePanelSystem>())
      };
      // ISSUE: reference to a compiler-generated field
      this.m_Hierarchy = new NativeList<EditorHierarchyUISystem.HierarchyItem>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ExpandedIds = new NativeParallelHashSet<EditorHierarchyUISystem.ItemId>(128, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_Viewport = new EditorHierarchyUISystem.Viewport();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_ExpandedIds.Clear();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Hierarchy.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ExpandedIds.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool force = this.m_Dirty || !this.m_ModifiedQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TotalCount = this.m_Hierarchy.Length;
      // ISSUE: reference to a compiler-generated method
      this.UpdateSelection();
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated method
      this.UpdateViewport(force);
      if (!force)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateHierarchy(this.m_Hierarchy);
    }

    private void UpdateSelection()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_EditorPanelUISystem.activePanel == null)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedId.isContainer)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_SelectedId = new EditorHierarchyUISystem.ItemId();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.selected != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(this.m_SelectedId.entity != this.m_ToolSystem.selected))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_SelectedId = new EditorHierarchyUISystem.ItemId()
          {
            type = EditorHierarchyUISystem.ItemType.Object,
            entity = this.m_ToolSystem.selected
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RefreshCameraController((EditorHierarchyUISystem.CameraMode) this.m_CameraMode.value);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          EditorHierarchyUISystem.PanelItem panelItem1 = this.panelItems.FirstOrDefault<EditorHierarchyUISystem.PanelItem>((Func<EditorHierarchyUISystem.PanelItem, bool>) (p => p.type == this.m_SelectedId.type));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorPanelUISystem.activePanel == panelItem1?.panel)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          EditorHierarchyUISystem.PanelItem panelItem2 = this.panelItems.FirstOrDefault<EditorHierarchyUISystem.PanelItem>((Func<EditorHierarchyUISystem.PanelItem, bool>) (p => p.panel == this.m_EditorPanelUISystem.activePanel));
          // ISSUE: variable of a compiler-generated type
          EditorHierarchyUISystem.ItemId itemId1;
          if (panelItem2 == null)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            EditorHierarchyUISystem.ItemId itemId2 = new EditorHierarchyUISystem.ItemId();
            itemId1 = itemId2;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            itemId1 = new EditorHierarchyUISystem.ItemId(panelItem2.type);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedId = itemId1;
        }
      }
    }

    private void UpdateViewport(bool force)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NextViewportStartIndex = math.clamp(this.m_NextViewportStartIndex, 0, this.m_Hierarchy.Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_NextViewportEndIndex = math.clamp(this.m_NextViewportEndIndex, 0, this.m_Hierarchy.Length);
      // ISSUE: reference to a compiler-generated method
      if (!force && !this.ViewportChanged())
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Viewport.startIndex = this.m_NextViewportStartIndex;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Viewport.items.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (int viewportStartIndex = this.m_NextViewportStartIndex; viewportStartIndex < this.m_NextViewportEndIndex; ++viewportStartIndex)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_Viewport.items.Add(this.BuildViewportItem(this.m_Hierarchy[viewportStartIndex]));
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ViewportBinding.TriggerUpdate();
    }

    private bool ViewportChanged()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_NextViewportStartIndex != this.m_Viewport.startIndex || this.m_NextViewportEndIndex != this.m_Viewport.startIndex + this.m_Viewport.items.Count)
        return true;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_Viewport.items.Count; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        EditorHierarchyUISystem.ViewportItem viewportItem = this.m_Viewport.items[index1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int index2 = this.m_Viewport.startIndex + index1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (index2 >= this.m_Hierarchy.Length || !viewportItem.EqualsHierarchy(this.m_Hierarchy[index2]))
          return true;
      }
      return false;
    }

    private EditorHierarchyUISystem.ViewportItem BuildViewportItem(
      EditorHierarchyUISystem.HierarchyItem item)
    {
      PrefabRef component;
      PrefabBase prefab;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      return new EditorHierarchyUISystem.ViewportItem()
      {
        id = item.id,
        level = item.level,
        expandable = item.expandable,
        expanded = item.expanded,
        name = this.GetName(item.id),
        tooltip = new LocalizedString?(this.GetTooltip(item.id)),
        selectable = item.selectable,
        saveable = item.id.type == EditorHierarchyUISystem.ItemType.Object && this.EntityManager.TryGetComponent<PrefabRef>(item.id.entity, out component) && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component, out prefab) && !prefab.builtin
      };
    }

    private LocalizedString GetName(EditorHierarchyUISystem.ItemId id)
    {
      // ISSUE: reference to a compiler-generated field
      if (id.type == EditorHierarchyUISystem.ItemType.Object)
      {
        PrefabRef component;
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.EntityManager.TryGetComponent<PrefabRef>(id.entity, out component) && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component, out prefab))
          return LocalizedString.Value(prefab.name);
      }
      else
      {
        PrefabRef component;
        DynamicBuffer<SubMesh> buffer;
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (id.type == EditorHierarchyUISystem.ItemType.SubMesh && this.EntityManager.TryGetComponent<PrefabRef>(id.entity, out component) && this.EntityManager.TryGetBuffer<SubMesh>(component.m_Prefab, true, out buffer) && id.subIndex < buffer.Length && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(buffer[id.subIndex].m_SubMesh, out prefab))
          return LocalizedString.Value(prefab.name);
      }
      // ISSUE: reference to a compiler-generated field
      return (LocalizedString) ("Editor." + id.type.ToString().ToUpper());
    }

    private LocalizedString GetTooltip(EditorHierarchyUISystem.ItemId id)
    {
      // ISSUE: reference to a compiler-generated field
      return (LocalizedString) ("Editor." + id.type.ToString().ToUpper() + "_TOOLTIP");
    }

    private void UpdateHierarchy(
      NativeList<EditorHierarchyUISystem.HierarchyItem> hierarchy)
    {
      hierarchy.Clear();
      foreach (EditorHierarchyUISystem.PanelItem panelItem in this.panelItems)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        hierarchy.Add(new EditorHierarchyUISystem.HierarchyItem()
        {
          id = new EditorHierarchyUISystem.ItemId(panelItem.type),
          level = panelItem.level,
          selectable = true
        });
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ObjectQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EditorHierarchyUISystem.ItemId itemId = new EditorHierarchyUISystem.ItemId(EditorHierarchyUISystem.ItemType.ObjectContainer);
      // ISSUE: reference to a compiler-generated field
      int num = this.m_ExpandedIds.Contains(itemId) ? 1 : 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      hierarchy.Add(new EditorHierarchyUISystem.HierarchyItem()
      {
        id = itemId,
        level = (byte) 0,
        expandable = true,
        expanded = this.m_ExpandedIds.Contains(itemId),
        selectable = false
      });
      if (num == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EditorHierarchyUISystem.ObjectHierarchyJob jobData = new EditorHierarchyUISystem.ObjectHierarchyJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_ExpandedIds = this.m_ExpandedIds,
        m_Hierarchy = hierarchy
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<EditorHierarchyUISystem.ObjectHierarchyJob>(this.m_ObjectQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated method
      this.Dependency = this.EditorHierarchyUISystem_4CF10000_LambdaJob_0_Execute(hierarchy, this.Dependency);
    }

    private void SetViewportRange(int startIndex, int endIndex)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_NextViewportStartIndex = startIndex;
      // ISSUE: reference to a compiler-generated field
      this.m_NextViewportEndIndex = endIndex;
    }

    private void SetSelectedId(EditorHierarchyUISystem.ItemId id)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedId = id;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      EditorHierarchyUISystem.ItemType type = id.type;
      switch (type)
      {
        case EditorHierarchyUISystem.ItemType.Object:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.selected = id.entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_EditorToolUISystem.SelectEntity(id.entity);
          break;
        case EditorHierarchyUISystem.ItemType.SubMesh:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.selected = id.entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_EditorToolUISystem.SelectEntitySubMesh(id.entity, id.subIndex);
          break;
        default:
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.selected = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_EditorPanelUISystem.activePanel = this.panelItems.FirstOrDefault<EditorHierarchyUISystem.PanelItem>((Func<EditorHierarchyUISystem.PanelItem, bool>) (p => p.type == id.type))?.panel;
          break;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.RefreshCameraController((EditorHierarchyUISystem.CameraMode) this.m_CameraMode.value);
    }

    private void SetExpanded(EditorHierarchyUISystem.ItemId id, bool expanded)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = true;
      if (expanded)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ExpandedIds.Add(id);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ExpandedIds.Remove(id);
      }
    }

    private void ToggleCameraMode(EditorHierarchyUISystem.CameraMode cameraMode)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CameraMode.Update((int) cameraMode);
      // ISSUE: reference to a compiler-generated method
      this.RefreshCameraController(cameraMode);
    }

    private void RefreshCameraController(EditorHierarchyUISystem.CameraMode mode)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (mode == EditorHierarchyUISystem.CameraMode.Default || mode == EditorHierarchyUISystem.CameraMode.Orbit && this.m_SelectedId.entity == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CameraUpdateSystem.activeCameraController == this.m_CameraUpdateSystem.gamePlayController)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.gamePlayController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.gamePlayController;
      }
      else if (mode == EditorHierarchyUISystem.CameraMode.Orbit)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.orbitCameraController.followedEntity = this.m_SelectedId.entity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CameraUpdateSystem.activeCameraController == this.m_CameraUpdateSystem.orbitCameraController)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.orbitCameraController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.orbitCameraController;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (mode != EditorHierarchyUISystem.CameraMode.FirstPerson || this.m_CameraUpdateSystem.activeCameraController == this.m_CameraUpdateSystem.cinematicCameraController)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.cinematicCameraController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.cinematicCameraController;
      }
    }

    private int GetWidth()
    {
      EditorSettings editor = SharedSettings.instance?.editor;
      return editor == null ? 350 : editor.hierarchyWidth;
    }

    private void SetWidth(int width)
    {
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor == null)
        return;
      editor.hierarchyWidth = width;
    }

    private void OnSave(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      EditorPrefabUtils.SavePrefab(this.m_PrefabSystem.GetPrefab<PrefabBase>(this.EntityManager.GetComponentData<PrefabRef>(entity)));
      PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.IMadeThis);
      // ISSUE: reference to a compiler-generated field
      Action<Entity> onSave = this.onSave;
      if (onSave == null)
        return;
      onSave(entity);
    }

    private void OnBulldoze(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      Action<Entity> onBulldoze = this.onBulldoze;
      if (onBulldoze == null)
        return;
      onBulldoze(entity);
    }

    private JobHandle EditorHierarchyUISystem_4CF10000_LambdaJob_0_Execute(
      NativeList<EditorHierarchyUISystem.HierarchyItem> hierarchy,
      JobHandle __inputDependency)
    {
      // ISSUE: object of a compiler-generated type is created
      return new EditorHierarchyUISystem.EditorHierarchyUISystem_4CF10000_LambdaJob_0_Job()
      {
        hierarchy = hierarchy
      }.Schedule<EditorHierarchyUISystem.EditorHierarchyUISystem_4CF10000_LambdaJob_0_Job>(__inputDependency);
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
    public EditorHierarchyUISystem()
    {
    }

    private enum CameraMode
    {
      Default,
      FirstPerson,
      Orbit,
    }

    [BurstCompile]
    public struct ObjectHierarchyJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public NativeParallelHashSet<EditorHierarchyUISystem.ItemId> m_ExpandedIds;
      public NativeList<EditorHierarchyUISystem.HierarchyItem> m_Hierarchy;

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
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity prefab = nativeArray2[index1].m_Prefab;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          EditorHierarchyUISystem.ItemId itemId = new EditorHierarchyUISystem.ItemId()
          {
            type = EditorHierarchyUISystem.ItemType.Object,
            entity = nativeArray1[index1]
          };
          // ISSUE: reference to a compiler-generated field
          bool flag1 = this.m_SubMeshes.HasBuffer(prefab);
          // ISSUE: reference to a compiler-generated field
          bool flag2 = flag1 && this.m_ExpandedIds.Contains(itemId);
          // ISSUE: reference to a compiler-generated field
          ref NativeList<EditorHierarchyUISystem.HierarchyItem> local1 = ref this.m_Hierarchy;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          EditorHierarchyUISystem.HierarchyItem hierarchyItem = new EditorHierarchyUISystem.HierarchyItem();
          // ISSUE: reference to a compiler-generated field
          hierarchyItem.id = itemId;
          // ISSUE: reference to a compiler-generated field
          hierarchyItem.level = (byte) 1;
          // ISSUE: reference to a compiler-generated field
          hierarchyItem.expandable = flag1;
          // ISSUE: reference to a compiler-generated field
          hierarchyItem.expanded = flag2;
          // ISSUE: reference to a compiler-generated field
          hierarchyItem.selectable = true;
          ref EditorHierarchyUISystem.HierarchyItem local2 = ref hierarchyItem;
          local1.Add(in local2);
          DynamicBuffer<SubMesh> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (flag2 && this.m_SubMeshes.TryGetBuffer(prefab, out bufferData))
          {
            for (int index2 = 0; index2 < bufferData.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeList<EditorHierarchyUISystem.HierarchyItem> local3 = ref this.m_Hierarchy;
              // ISSUE: object of a compiler-generated type is created
              hierarchyItem = new EditorHierarchyUISystem.HierarchyItem();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              hierarchyItem.id = new EditorHierarchyUISystem.ItemId()
              {
                type = EditorHierarchyUISystem.ItemType.SubMesh,
                entity = nativeArray1[index1],
                subIndex = index2
              };
              // ISSUE: reference to a compiler-generated field
              hierarchyItem.level = (byte) 2;
              // ISSUE: reference to a compiler-generated field
              hierarchyItem.selectable = true;
              ref EditorHierarchyUISystem.HierarchyItem local4 = ref hierarchyItem;
              local3.Add(in local4);
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

    public class PanelItem
    {
      public EditorHierarchyUISystem.ItemType type;
      public byte level;
      public IEditorPanel panel;

      public PanelItem(EditorHierarchyUISystem.ItemType type, byte level, IEditorPanel panel)
      {
        // ISSUE: reference to a compiler-generated field
        this.type = type;
        // ISSUE: reference to a compiler-generated field
        this.level = level;
        // ISSUE: reference to a compiler-generated field
        this.panel = panel;
      }
    }

    public class Viewport : IJsonWritable
    {
      public int startIndex;
      public List<EditorHierarchyUISystem.ViewportItem> items = new List<EditorHierarchyUISystem.ViewportItem>();

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("startIndex");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.startIndex);
        writer.PropertyName("items");
        // ISSUE: reference to a compiler-generated field
        writer.Write<EditorHierarchyUISystem.ViewportItem>((IList<EditorHierarchyUISystem.ViewportItem>) this.items);
        writer.TypeEnd();
      }
    }

    public struct ViewportItem : IJsonWritable
    {
      public EditorHierarchyUISystem.ItemId id;
      public byte level;
      public bool expandable;
      public bool expanded;
      public LocalizedString name;
      public bool selectable;
      public bool saveable;
      public LocalizedString? tooltip;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("id");
        // ISSUE: reference to a compiler-generated field
        writer.Write<EditorHierarchyUISystem.ItemId>(this.id);
        writer.PropertyName("level");
        // ISSUE: reference to a compiler-generated field
        writer.Write((int) this.level);
        writer.PropertyName("expandable");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.expandable);
        writer.PropertyName("expanded");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.expanded);
        writer.PropertyName("name");
        // ISSUE: reference to a compiler-generated field
        writer.Write<LocalizedString>(this.name);
        writer.PropertyName("selectable");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.selectable);
        writer.PropertyName("saveable");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.saveable);
        writer.PropertyName("tooltip");
        // ISSUE: reference to a compiler-generated field
        writer.Write<LocalizedString>(this.tooltip);
        writer.TypeEnd();
      }

      public bool EqualsHierarchy(EditorHierarchyUISystem.HierarchyItem other)
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
        return this.id == other.id && (int) this.level == (int) other.level && this.expandable == other.expandable && this.expanded == other.expanded && this.selectable == other.selectable;
      }
    }

    public struct HierarchyItem : IComparable<EditorHierarchyUISystem.HierarchyItem>
    {
      public EditorHierarchyUISystem.ItemId id;
      public byte level;
      public bool expandable;
      public bool expanded;
      public bool selectable;

      public int CompareTo(EditorHierarchyUISystem.HierarchyItem other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return this.id.CompareTo(other.id);
      }
    }

    public struct ItemId : 
      IJsonWritable,
      IJsonReadable,
      IEquatable<EditorHierarchyUISystem.ItemId>,
      IComparable<EditorHierarchyUISystem.ItemId>
    {
      public EditorHierarchyUISystem.ItemType type;
      public Entity entity;
      public int subIndex;

      public bool isContainer => this.type == EditorHierarchyUISystem.ItemType.ObjectContainer;

      public ItemId(EditorHierarchyUISystem.ItemType type, Entity entity = default (Entity), int subIndex = 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.type = type;
        // ISSUE: reference to a compiler-generated field
        this.entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.subIndex = subIndex;
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("type");
        // ISSUE: reference to a compiler-generated field
        writer.Write((int) this.type);
        writer.PropertyName("entity");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.entity);
        writer.PropertyName("subIndex");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.subIndex);
        writer.TypeEnd();
      }

      public void Read(IJsonReader reader)
      {
        long num1 = (long) reader.ReadMapBegin();
        reader.ReadProperty("type");
        int num2;
        reader.Read(out num2);
        // ISSUE: reference to a compiler-generated field
        this.type = (EditorHierarchyUISystem.ItemType) num2;
        reader.ReadProperty("entity");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.entity);
        reader.ReadProperty("subIndex");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.subIndex);
        reader.ReadMapEnd();
      }

      public bool Equals(EditorHierarchyUISystem.ItemId other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.type == other.type && this.entity.Equals(other.entity) && this.subIndex == other.subIndex;
      }

      public override bool Equals(object obj)
      {
        // ISSUE: reference to a compiler-generated method
        return obj is EditorHierarchyUISystem.ItemId other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return ((int) this.type * 397 ^ this.entity.GetHashCode()) * 397 ^ this.subIndex;
      }

      public static bool operator ==(
        EditorHierarchyUISystem.ItemId left,
        EditorHierarchyUISystem.ItemId right)
      {
        // ISSUE: reference to a compiler-generated method
        return left.Equals(right);
      }

      public static bool operator !=(
        EditorHierarchyUISystem.ItemId left,
        EditorHierarchyUISystem.ItemId right)
      {
        // ISSUE: reference to a compiler-generated method
        return !left.Equals(right);
      }

      public int CompareTo(EditorHierarchyUISystem.ItemId other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num1 = this.entity.CompareTo(other.entity);
        if (num1 != 0)
          return num1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num2 = ((byte) this.type).CompareTo((byte) other.type);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return num2 != 0 ? num2 : this.subIndex.CompareTo(other.subIndex);
      }
    }

    public enum ItemType : byte
    {
      None,
      Map,
      Climate,
      Water,
      Resources,
      ObjectContainer,
      Object,
      SubMesh,
    }

    [NoAlias]
    [BurstCompile]
    private struct EditorHierarchyUISystem_4CF10000_LambdaJob_0_Job : IJob
    {
      public NativeList<EditorHierarchyUISystem.HierarchyItem> hierarchy;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private void OriginalLambdaBody()
      {
        // ISSUE: reference to a compiler-generated field
        this.hierarchy.Sort<EditorHierarchyUISystem.HierarchyItem>();
      }

      public void Execute() => this.OriginalLambdaBody();
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
      }
    }
  }
}
