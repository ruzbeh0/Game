// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.WidgetBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Assertions;

#nullable disable
namespace Game.UI.Widgets
{
  public class WidgetBindings : CompositeBinding, IReader<IWidget>
  {
    private string m_Group;
    private List<IWidget> m_LastChildren = new List<IWidget>();
    private List<int> m_CurrentPath = new List<int>();
    private RawValueBinding m_ChildrenBinding;

    public IList<IWidget> children { get; set; } = (IList<IWidget>) new List<IWidget>();

    public event ValueChangedCallback EventValueChanged;

    public bool active => this.m_ChildrenBinding.active;

    public WidgetBindings(string group, string name = "children")
    {
      this.m_Group = group;
      this.AddBinding((IBinding) (this.m_ChildrenBinding = new RawValueBinding(group, name, new Action<IJsonWriter>(this.WriteChildren))));
    }

    public void AddDefaultBindings()
    {
      this.AddBindings<InvokableBindings>();
      this.AddBindings<SettableBindings>();
      this.AddBindings<ExpandableBindings>();
      this.AddBindings<ListBindings>();
      this.AddBindings<PagedBindings>();
    }

    public void AddBindings<U>() where U : IWidgetBindingFactory, new()
    {
      this.AddBindings((IWidgetBindingFactory) new U());
    }

    public void AddBindings(IWidgetBindingFactory bindingFactory)
    {
      foreach (IBinding binding in bindingFactory.CreateBindings(this.m_Group, (IReader<IWidget>) this, new ValueChangedCallback(this.OnValueChanged)))
        this.AddBinding(binding);
    }

    private void OnValueChanged(IWidget widget)
    {
      this.UpdateChildrenBinding();
      ValueChangedCallback eventValueChanged = this.EventValueChanged;
      if (eventValueChanged == null)
        return;
      eventValueChanged(widget);
    }

    public override bool Update()
    {
      this.UpdateChildrenBinding();
      return base.Update();
    }

    private void UpdateChildrenBinding()
    {
      if (!this.active)
        return;
      bool flag1 = !this.children.SequenceEqual<IWidget>((IEnumerable<IWidget>) this.m_LastChildren);
      if (flag1)
      {
        this.m_LastChildren.Clear();
        this.m_LastChildren.AddRange((IEnumerable<IWidget>) this.children);
        ContainerExtensions.SetDefaults<IWidget>((IList<IWidget>) this.m_LastChildren);
      }
      this.m_CurrentPath.Clear();
      bool flag2 = flag1 | this.UpdateSubTree(this.children, !flag1);
      Assert.AreEqual(0, this.m_CurrentPath.Count);
      if (!flag2)
        return;
      this.m_ChildrenBinding.Update();
    }

    private bool UpdateSubTree(IList<IWidget> widgets, bool patch)
    {
      bool flag = false;
      for (int index = 0; index < widgets.Count; ++index)
      {
        IWidget widget = widgets[index];
        WidgetChanges changes = widget.Update();
        this.m_CurrentPath.Add(index);
        if (this.UpdateSubTree(widget.visibleChildren, patch && (changes & WidgetChanges.Children) == WidgetChanges.None))
          changes |= WidgetChanges.Children;
        if (changes != WidgetChanges.None)
        {
          if (patch)
            Widget.PatchWidget(this.m_ChildrenBinding, (IList<int>) this.m_CurrentPath, widget, changes);
          else
            flag = true;
        }
        this.m_CurrentPath.RemoveAt(this.m_CurrentPath.Count - 1);
      }
      return flag;
    }

    private void WriteChildren(IJsonWriter writer)
    {
      writer.Write<IWidget>((IList<IWidget>) this.m_LastChildren);
    }

    void IReader<IWidget>.Read(IJsonReader reader, out IWidget value)
    {
      value = (IWidget) null;
      ulong num = reader.ReadArrayBegin();
      if (num > 0UL)
      {
        reader.ReadArrayElement(0UL);
        PathSegment path = new PathSegment();
        path.Read(reader);
        value = ContainerExtensions.FindChild<IWidget>((IEnumerable<IWidget>) this.m_LastChildren, path);
        for (ulong index = 1; index < num; ++index)
        {
          reader.ReadArrayElement(index);
          path.Read(reader);
          value = value.FindChild(path);
        }
      }
      reader.ReadArrayEnd();
    }
  }
}
