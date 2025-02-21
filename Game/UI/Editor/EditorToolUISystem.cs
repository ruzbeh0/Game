// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorToolUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorToolUISystem : UISystemBase
  {
    private const string kGroup = "editorTool";
    private ToolSystem m_ToolSystem;
    private EditorPanelUISystem m_EditorPanelUISystem;
    private InspectorPanelSystem m_InspectorPanelSystem;
    private GetterValueBinding<IEditorTool[]> m_ToolsBinding;
    private IEditorTool[] m_Tools;
    private bool[] m_Disabled;
    private IEditorTool m_ActiveTool;
    private Entity m_LastSelectedEntity;

    public override GameMode gameMode => GameMode.Editor;

    public IEditorTool[] tools
    {
      get => this.m_Tools;
      set
      {
        this.m_Tools = value;
        this.m_Disabled = ((IEnumerable<IEditorTool>) value).Select<IEditorTool, bool>((Func<IEditorTool, bool>) (t => t.disabled)).ToArray<bool>();
      }
    }

    [CanBeNull]
    public IEditorTool activeTool
    {
      get => this.m_ActiveTool;
      set
      {
        if (value == this.m_ActiveTool)
          return;
        if (this.m_ActiveTool != null)
          this.m_ActiveTool.active = false;
        this.m_ActiveTool = value;
        if (this.m_ActiveTool == null)
          return;
        this.m_ActiveTool.active = true;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_EditorPanelUISystem = this.World.GetOrCreateSystemManaged<EditorPanelUISystem>();
      this.m_InspectorPanelSystem = this.World.GetOrCreateSystemManaged<InspectorPanelSystem>();
      this.tools = new IEditorTool[6]
      {
        (IEditorTool) new EditorAssetImportTool(this.World),
        (IEditorTool) new EditorTerrainTool(this.World),
        (IEditorTool) new EditorPrefabTool(this.World),
        (IEditorTool) new EditorPrefabEditorTool(this.World),
        (IEditorTool) new EditorPhotoTool(this.World),
        (IEditorTool) new EditorBulldozeTool(this.World)
      };
      this.AddUpdateBinding((IUpdateBinding) (this.m_ToolsBinding = new GetterValueBinding<IEditorTool[]>("editorTool", "tools", (Func<IEditorTool[]>) (() => this.tools), (IWriter<IEditorTool[]>) new ArrayWriter<IEditorTool>((IWriter<IEditorTool>) new ValueWriter<IEditorTool>()))));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<string>("editorTool", "activeTool", (Func<string>) (() => this.activeTool?.id), (IWriter<string>) new StringWriter().Nullable<string>()));
      this.AddBinding((IBinding) new TriggerBinding<string>("editorTool", "selectTool", new Action<string>(this.SelectTool)));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.activeTool != null && !this.activeTool.active)
        this.activeTool = (IEditorTool) null;
      if (this.m_ToolSystem.selected != this.m_LastSelectedEntity)
        this.SelectEntity(this.m_ToolSystem.selected);
      if (!this.UpdateToolState())
        return;
      this.m_ToolsBinding.TriggerUpdate();
    }

    public void SelectEntity(Entity entity)
    {
      this.m_LastSelectedEntity = entity;
      // ISSUE: reference to a compiler-generated method
      if (this.m_InspectorPanelSystem.SelectEntity(entity))
      {
        this.activeTool = (IEditorTool) null;
        this.m_EditorPanelUISystem.activePanel = (IEditorPanel) this.m_InspectorPanelSystem;
      }
      else
      {
        if (this.m_EditorPanelUISystem.activePanel != this.m_InspectorPanelSystem)
          return;
        this.m_EditorPanelUISystem.activePanel = (IEditorPanel) null;
      }
    }

    public void SelectEntitySubMesh(Entity entity, int subMeshIndex)
    {
      this.m_LastSelectedEntity = entity;
      // ISSUE: reference to a compiler-generated method
      if (this.m_InspectorPanelSystem.SelectMesh(entity, subMeshIndex))
      {
        this.activeTool = (IEditorTool) null;
        this.m_EditorPanelUISystem.activePanel = (IEditorPanel) this.m_InspectorPanelSystem;
      }
      else
      {
        if (this.m_EditorPanelUISystem.activePanel != this.m_InspectorPanelSystem)
          return;
        this.m_EditorPanelUISystem.activePanel = (IEditorPanel) null;
      }
    }

    private void SelectTool([CanBeNull] string id)
    {
      this.activeTool = ((IEnumerable<IEditorTool>) this.tools).FirstOrDefault<IEditorTool>((Func<IEditorTool, bool>) (t => t.id == id));
    }

    private bool UpdateToolState()
    {
      bool flag = false;
      for (int index = 0; index < this.m_Tools.Length; ++index)
      {
        bool disabled = this.m_Tools[index].disabled;
        if (disabled != this.m_Disabled[index])
        {
          this.m_Disabled[index] = disabled;
          flag = true;
        }
      }
      return flag;
    }

    [Preserve]
    public EditorToolUISystem()
    {
    }
  }
}
