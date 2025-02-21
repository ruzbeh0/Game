// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.Widget
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Game.UI.Widgets
{
  [DebuggerDisplay("{m_Path}, hidden={m_Hidden}, disabled={m_Disabled}")]
  public abstract class Widget : IWidget, IJsonWritable, IVisibleWidget
  {
    protected const string kProperties = "props";
    protected const string kChildren = "children";
    protected const string kTutorialTag = "tutorialTag";
    private WidgetChanges m_Changes;
    private PathSegment m_Path = PathSegment.Empty;
    private string m_TutorialTag;
    protected bool m_Disabled;
    protected bool m_Hidden;
    private bool m_IsInitialUpdate = true;

    public PathSegment path
    {
      get => this.m_Path;
      set
      {
        if (!(value != this.m_Path))
          return;
        this.m_Path = value;
        this.m_Changes |= WidgetChanges.Path;
      }
    }

    public string tutorialTag
    {
      get => this.m_TutorialTag;
      set
      {
        if (!(value != this.m_TutorialTag))
          return;
        this.m_TutorialTag = value;
        this.SetPropertiesChanged();
      }
    }

    public virtual IList<IWidget> visibleChildren => (IList<IWidget>) Array.Empty<IWidget>();

    [CanBeNull]
    public Func<bool> disabled { get; set; }

    [CanBeNull]
    public Func<bool> hidden { get; set; }

    public virtual bool isVisible => !this.m_Hidden;

    public virtual bool isActive => !this.m_Disabled;

    public void SetPropertiesChanged() => this.m_Changes |= WidgetChanges.Properties;

    public void SetChildrenChanged() => this.m_Changes |= WidgetChanges.Children;

    WidgetChanges IWidget.Update() => this.UpdateBase();

    private WidgetChanges UpdateBase()
    {
      int num = (int) this.UpdateVisibility();
      WidgetChanges changes = this.m_Changes;
      this.m_Changes = WidgetChanges.None;
      if (this.m_Hidden && !this.m_IsInitialUpdate)
        return changes;
      this.m_IsInitialUpdate = false;
      bool flag = this.disabled != null && this.disabled();
      if (flag != this.m_Disabled)
      {
        changes |= WidgetChanges.Activity;
        this.m_Disabled = flag;
      }
      return changes | this.Update();
    }

    protected virtual WidgetChanges Update() => WidgetChanges.None;

    public virtual WidgetChanges UpdateVisibility()
    {
      bool flag = this.hidden != null && this.hidden();
      if (flag == this.m_Hidden)
        return WidgetChanges.None;
      this.m_Hidden = flag;
      this.m_Changes |= WidgetChanges.Visibility;
      return WidgetChanges.Visibility;
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(typeof (Widget).FullName);
      writer.PropertyName("path");
      writer.Write<PathSegment>(this.path);
      writer.PropertyName("props");
      writer.TypeBegin(this.propertiesTypeName);
      this.WriteBaseProperties(writer);
      writer.TypeEnd();
      writer.PropertyName("children");
      writer.Write<IWidget>(this.visibleChildren);
      writer.TypeEnd();
    }

    public virtual string propertiesTypeName => this.GetType().FullName;

    void IWidget.WriteProperties(IJsonWriter writer) => this.WriteBaseProperties(writer);

    private void WriteBaseProperties(IJsonWriter writer)
    {
      writer.PropertyName("disabled");
      writer.Write(this.m_Disabled);
      writer.PropertyName("hidden");
      writer.Write(this.m_Hidden);
      this.WriteProperties(writer);
    }

    protected virtual void WriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("tutorialTag");
      writer.Write(this.tutorialTag);
    }

    public static void PatchWidget(
      RawValueBinding binding,
      IList<int> path,
      IWidget widget,
      WidgetChanges changes)
    {
      IJsonWriter writer = binding.PatchBegin();
      if (changes == WidgetChanges.Path)
      {
        Widget.WritePatchPath(writer, path, nameof (path));
        writer.Write<PathSegment>(widget.path);
      }
      else if ((changes & WidgetChanges.TotalProperties) != WidgetChanges.None && (changes & ~WidgetChanges.TotalProperties) == WidgetChanges.None)
      {
        Widget.WritePatchPath(writer, path, "props");
        writer.TypeBegin(widget.propertiesTypeName);
        widget.WriteProperties(writer);
        writer.TypeEnd();
      }
      else if (changes == WidgetChanges.Children)
      {
        Widget.WritePatchPath(writer, path, "children");
        writer.Write<IWidget>(widget.visibleChildren);
      }
      else
      {
        Widget.WritePatchPath(writer, path);
        writer.Write<IWidget>(widget);
      }
      binding.PatchEnd();
    }

    public static void WritePatchPath(IJsonWriter writer, IList<int> path)
    {
      writer.ArrayBegin(2 * path.Count - 1);
      for (int index = 0; index < path.Count - 1; ++index)
      {
        writer.Write(path[index]);
        writer.Write("children");
      }
      writer.Write(path[path.Count - 1]);
      writer.ArrayEnd();
    }

    public static void WritePatchPath(IJsonWriter writer, IList<int> path, string propertyName)
    {
      writer.ArrayBegin(2 * path.Count);
      for (int index = 0; index < path.Count - 1; ++index)
      {
        writer.Write(path[index]);
        writer.Write("children");
      }
      writer.Write(path[path.Count - 1]);
      writer.Write(propertyName);
      writer.ArrayEnd();
    }

    public static void WritePatchPath(
      IJsonWriter writer,
      IList<int> path,
      string propertyName1,
      string propertyName2,
      string propertyName3,
      int propertyName4)
    {
      writer.ArrayBegin(2 * path.Count + 3);
      for (int index = 0; index < path.Count - 1; ++index)
      {
        writer.Write(path[index]);
        writer.Write("children");
      }
      writer.Write(path[path.Count - 1]);
      writer.Write(propertyName1);
      writer.Write(propertyName2);
      writer.Write(propertyName3);
      writer.Write(propertyName4);
      writer.ArrayEnd();
    }
  }
}
