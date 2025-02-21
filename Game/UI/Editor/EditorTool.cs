// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorTool
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Prefabs;
using Game.Tools;
using Game.UI.Widgets;
using Unity.Entities;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorTool : IEditorTool, IJsonWritable, IUITagProvider
  {
    protected ToolSystem m_ToolSystem;
    protected EditorPanelUISystem m_EditorPanelUISystem;

    public string id { get; set; }

    public string icon { get; set; }

    public bool disabled { get; set; }

    public string shortcut { get; set; }

    public string uiTag { get; set; }

    [CanBeNull]
    public IEditorPanel panel { get; set; }

    [CanBeNull]
    public ToolBaseSystem tool { get; set; }

    public bool active
    {
      get => this.IsActive();
      set
      {
        if (value)
          this.OnEnable();
        else
          this.OnDisable();
      }
    }

    public EditorTool(World world)
    {
      this.m_ToolSystem = world.GetOrCreateSystemManaged<ToolSystem>();
      this.m_EditorPanelUISystem = world.GetOrCreateSystemManaged<EditorPanelUISystem>();
    }

    protected virtual bool IsActive()
    {
      if (this.m_EditorPanelUISystem.activePanel != this.panel)
        return false;
      return this.tool == null || this.m_ToolSystem.activeTool == this.tool;
    }

    protected virtual void OnEnable()
    {
      this.m_ToolSystem.selected = Entity.Null;
      this.m_EditorPanelUISystem.activePanel = this.panel;
      if (this.tool == null)
        return;
      this.m_ToolSystem.activeTool = this.tool;
    }

    protected virtual void OnDisable()
    {
      if (this.m_EditorPanelUISystem.activePanel == this.panel)
        this.m_EditorPanelUISystem.activePanel = (IEditorPanel) null;
      if (this.tool == null || this.m_ToolSystem.activeTool != this.tool)
        return;
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool((PrefabBase) null);
    }
  }
}
