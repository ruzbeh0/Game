// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.PopupValueField`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Reflection;
using Game.UI.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  public class PopupValueField<T> : NamedWidgetWithTooltip, IExpandable, IDisableCallback
  {
    private bool m_LastExpanded;
    private T m_Value;
    private LocalizedString m_DisplayValue;

    public bool expanded { get; set; }

    public ITypedValueAccessor<T> accessor { get; set; }

    public IValueFieldPopup<T> popup { get; set; }

    public override IList<IWidget> visibleChildren
    {
      get => !this.expanded ? (IList<IWidget>) Array.Empty<IWidget>() : this.popup.children;
    }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.expanded != this.m_LastExpanded)
      {
        widgetChanges |= WidgetChanges.Properties | WidgetChanges.Children;
        this.m_LastExpanded = this.expanded;
        if (this.expanded)
          this.popup.Attach(this.accessor);
        else
          this.popup.Detach();
      }
      T typedValue = this.accessor.GetTypedValue();
      LocalizedString displayValue = this.popup.GetDisplayValue(typedValue);
      if (!object.Equals((object) typedValue, (object) this.m_Value) || !this.m_DisplayValue.Equals((object) typedValue))
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Value = typedValue;
        this.m_DisplayValue = displayValue;
      }
      if (this.expanded && this.popup.Update())
        widgetChanges |= WidgetChanges.Children;
      return widgetChanges;
    }

    public override string propertiesTypeName => "Game.UI.Widgets.PopupValueField";

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("expanded");
      writer.Write(this.m_LastExpanded);
      writer.PropertyName("displayValue");
      writer.Write<LocalizedString>(this.m_DisplayValue);
    }
  }
}
