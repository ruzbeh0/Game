// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorPanelUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Settings;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorPanelUISystem : UISystemBase
  {
    private const string kGroup = "editorPanel";
    [CanBeNull]
    private IEditorPanel m_LastPanel;
    private ValueBinding<bool> m_ActiveBinding;
    private ValueBinding<LocalizedString?> m_TitleBinding;
    private WidgetBindings m_WidgetBindings;

    public override GameMode gameMode => GameMode.GameOrEditor;

    [CanBeNull]
    public IEditorPanel activePanel { get; set; }

    private EditorPanelWidgetRenderer widgetRenderer
    {
      get
      {
        return this.activePanel != null ? this.activePanel.widgetRenderer : EditorPanelWidgetRenderer.Editor;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.AddBinding((IBinding) (this.m_ActiveBinding = new ValueBinding<bool>("editorPanel", "active", false)));
      this.AddBinding((IBinding) (this.m_TitleBinding = new ValueBinding<LocalizedString?>("editorPanel", "title", new LocalizedString?(), (IWriter<LocalizedString?>) new ValueWriter<LocalizedString>().Nullable<LocalizedString>())));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("editorPanel", "width", new Func<int>(this.GetWidth)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("editorPanel", "widgetRenderer", (Func<int>) (() => (int) this.widgetRenderer)));
      this.AddUpdateBinding((IUpdateBinding) (this.m_WidgetBindings = new WidgetBindings("editorPanel")));
      EditorPanelUISystem.AddEditorWidgetBindings(this.m_WidgetBindings);
      this.m_WidgetBindings.EventValueChanged += new ValueChangedCallback(this.OnValueChanged);
      this.AddBinding((IBinding) new TriggerBinding("editorPanel", "cancel", new Action(this.Cancel)));
      this.AddBinding((IBinding) new TriggerBinding("editorPanel", "close", new Action(this.Close)));
      this.AddBinding((IBinding) new TriggerBinding<int>("editorPanel", "setWidth", new Action<int>(this.SetWidth)));
    }

    public static void AddEditorWidgetBindings(WidgetBindings widgetBindings)
    {
      widgetBindings.AddDefaultBindings();
      widgetBindings.AddBindings<EditorSection.Bindings>();
      widgetBindings.AddBindings<SeasonsField.Bindings>();
      widgetBindings.AddBindings<IItemPicker.Bindings>();
      widgetBindings.AddBindings<PopupSearchField.Bindings>();
      widgetBindings.AddBindings<AnimationCurveField.Bindings>();
      widgetBindings.AddBindings<LocalizationField.Bindings>();
      widgetBindings.AddBindings<FilterMenu.Bindings>();
      widgetBindings.AddBindings<HierarchyMenu.Bindings>();
      widgetBindings.AddBindings<ExternalLinkField.Bindings>();
      widgetBindings.AddBindings<ListField.Bindings>();
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      this.activePanel = (IEditorPanel) null;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.activePanel != this.m_LastPanel)
      {
        if (this.m_LastPanel is ComponentSystemBase lastPanel)
        {
          lastPanel.Enabled = false;
          lastPanel.Update();
        }
        this.m_LastPanel = this.activePanel;
        if (this.activePanel is ComponentSystemBase activePanel)
          activePanel.Enabled = true;
      }
      if (this.activePanel != null)
      {
        if (this.activePanel is ComponentSystemBase activePanel)
          activePanel.Update();
        this.m_ActiveBinding.Update(true);
        this.m_TitleBinding.Update(new LocalizedString?(this.activePanel.title));
        this.m_WidgetBindings.children = this.activePanel.children;
      }
      else
      {
        this.m_ActiveBinding.Update(false);
        this.m_TitleBinding.Update(new LocalizedString?());
        this.m_WidgetBindings.children = (IList<IWidget>) Array.Empty<IWidget>();
      }
      base.OnUpdate();
    }

    public void OnValueChanged(IWidget widget) => this.activePanel?.OnValueChanged(widget);

    private int GetWidth()
    {
      EditorSettings editor = SharedSettings.instance?.editor;
      return editor == null ? 450 : editor.inspectorWidth;
    }

    private void SetWidth(int width)
    {
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor == null)
        return;
      editor.inspectorWidth = width;
    }

    private void Cancel()
    {
      if (this.activePanel == null || !this.activePanel.OnCancel())
        return;
      this.activePanel = (IEditorPanel) null;
    }

    private void Close()
    {
      if (this.activePanel == null || !this.activePanel.OnClose())
        return;
      this.activePanel = (IEditorPanel) null;
    }

    [Preserve]
    public EditorPanelUISystem()
    {
    }
  }
}
