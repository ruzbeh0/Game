// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.HierarchyItem`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.UI.Localization;

#nullable disable
namespace Game.UI.Editor
{
  public struct HierarchyItem<T> : IJsonWritable
  {
    public T m_Data;
    public LocalizedString m_DisplayName;
    public LocalizedString m_Tooltip;
    public int m_Level;
    public string m_Icon;
    public bool m_Selectable;
    public bool m_Selected;
    public bool m_Expandable;
    public bool m_Expanded;

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(nameof (HierarchyItem<T>));
      writer.PropertyName("displayName");
      writer.Write<LocalizedString>(this.m_DisplayName);
      writer.PropertyName("tooltip");
      if (this.m_Tooltip.isEmpty)
        writer.WriteNull();
      else
        writer.Write<LocalizedString>(this.m_Tooltip);
      writer.PropertyName("level");
      writer.Write(this.m_Level);
      writer.PropertyName("icon");
      writer.Write(this.m_Icon);
      writer.PropertyName("expandable");
      writer.Write(this.m_Expandable);
      writer.PropertyName("expanded");
      writer.Write(this.m_Expanded);
      writer.PropertyName("selectable");
      writer.Write(this.m_Selectable);
      writer.PropertyName("selected");
      writer.Write(this.m_Selected);
      writer.TypeEnd();
    }
  }
}
