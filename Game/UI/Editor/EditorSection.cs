// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Reflection;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorSection : ExpandableGroup, IDisableCallback
  {
    public static readonly Color kPrefabColor = new Color(0.31764707f, 0.2509804f, 0.20784314f);
    private bool m_Active;
    private Color? m_Color;

    [CanBeNull]
    public Action onDelete { get; set; }

    [CanBeNull]
    public ITypedValueAccessor<bool> active { get; set; }

    public bool primary { get; set; }

    [CanBeNull]
    public Color? color
    {
      get => this.m_Color;
      set
      {
        Color? nullable = value;
        Color? color = this.m_Color;
        if ((nullable.HasValue == color.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == color.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
          return;
        this.m_Color = value;
        this.SetPropertiesChanged();
      }
    }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      ITypedValueAccessor<bool> active = this.active;
      bool flag = active == null || active.GetTypedValue();
      if (flag != this.m_Active)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Active = flag;
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("expandable");
      writer.Write(this.children.Count != 0);
      writer.PropertyName("deletable");
      writer.Write(this.onDelete != null);
      writer.PropertyName("activatable");
      writer.Write(this.active != null);
      writer.PropertyName("active");
      writer.Write(this.m_Active);
      writer.PropertyName("primary");
      writer.Write(this.primary);
      writer.PropertyName("color");
      if (this.color.HasValue)
        writer.Write(this.color.Value);
      else
        writer.WriteNull();
    }

    public EditorSection()
      : base()
    {
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget>(group, "deleteEditorSection", (Action<IWidget>) (widget =>
        {
          if (!(widget is EditorSection editorSection2))
            return;
          Action onDelete = editorSection2.onDelete;
          if (onDelete != null)
            onDelete();
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, bool>(group, "setEditorSectionActive", (Action<IWidget, bool>) ((widget, active) =>
        {
          if (!(widget is EditorSection editorSection4))
            return;
          editorSection4.active?.SetTypedValue(active);
          onValueChanged(widget);
        }), pathResolver);
      }
    }
  }
}
