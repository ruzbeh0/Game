// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.DropdownField`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Reflection;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public class DropdownField<T> : Field<T>, IWarning
  {
    private int m_ItemsVersion = -1;
    private bool m_Warning;

    public DropdownItem<T>[] items { get; set; } = Array.Empty<DropdownItem<T>>();

    [CanBeNull]
    public Func<int> itemsVersion { get; set; }

    [CanBeNull]
    public Func<bool> warningAction { get; set; }

    public ITypedValueAccessor<DropdownItem<T>[]> itemsAccessor { get; set; }

    public new IWriter<T> valueWriter
    {
      protected get => base.valueWriter;
      set => base.valueWriter = value;
    }

    public new IReader<T> valueReader
    {
      protected get => base.valueReader;
      set => base.valueReader = value;
    }

    public bool warning
    {
      get => this.m_Warning;
      set
      {
        this.warningAction = (Func<bool>) null;
        this.m_Warning = value;
      }
    }

    public override string propertiesTypeName => "Game.UI.Widgets.DropdownField";

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.itemsAccessor != null)
      {
        Func<int> itemsVersion = this.itemsVersion;
        int num = itemsVersion != null ? itemsVersion() : 0;
        if (num != this.m_ItemsVersion)
        {
          widgetChanges |= WidgetChanges.Properties;
          this.m_ItemsVersion = num;
          this.items = this.itemsAccessor.GetTypedValue() ?? Array.Empty<DropdownItem<T>>();
        }
      }
      if (this.warningAction != null)
      {
        bool flag = this.warningAction();
        if (flag != this.m_Warning)
        {
          this.m_Warning = flag;
          widgetChanges |= WidgetChanges.Properties;
        }
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("items");
      writer.ArrayBegin(this.items.Length);
      foreach (DropdownItem<T> dropdownItem in this.items)
        dropdownItem.Write(this.valueWriter, writer);
      writer.ArrayEnd();
      writer.PropertyName("warning");
      writer.Write(this.warning);
    }
  }
}
