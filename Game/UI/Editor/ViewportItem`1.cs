// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.ViewportItem`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Editor
{
  public struct ViewportItem<T> : IJsonWritable
  {
    public HierarchyItem<T> m_Item;
    public int m_ItemIndex;

    public void Write(IJsonWriter writer) => writer.Write<HierarchyItem<T>>(this.m_Item);
  }
}
