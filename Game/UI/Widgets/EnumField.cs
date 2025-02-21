// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.EnumField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Reflection;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  public class EnumField : Field<ulong>, IWarning
  {
    private int m_ItemsVersion = -1;
    private bool m_Warning;

    [CanBeNull]
    public Func<bool> warningAction { get; set; }

    public EnumMember[] enumMembers { get; set; } = Array.Empty<EnumMember>();

    [CanBeNull]
    public Func<int> itemsVersion { get; set; }

    public ITypedValueAccessor<EnumMember[]> itemsAccessor { get; set; }

    public bool warning
    {
      get => this.m_Warning;
      set
      {
        this.warningAction = (Func<bool>) null;
        this.m_Warning = value;
      }
    }

    public EnumField()
    {
      this.valueWriter = (IWriter<ulong>) new ULongWriter();
      this.valueReader = (IReader<ulong>) new ULongReader();
    }

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
          this.enumMembers = this.itemsAccessor.GetTypedValue() ?? Array.Empty<EnumMember>();
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
      writer.PropertyName("enumMembers");
      writer.Write<EnumMember>((IList<EnumMember>) this.enumMembers);
      writer.PropertyName("warning");
      writer.Write(this.warning);
    }
  }
}
