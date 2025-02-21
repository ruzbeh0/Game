// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.Item
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;
using System.IO;

#nullable disable
namespace Game.UI.Editor
{
  public class Item : IItemPicker.Item, IComparable<Item>
  {
    [CanBeNull]
    public string parentDir { get; set; }

    public string name { get; set; }

    public Type type { get; set; }

    public string fullName { get; set; }

    [CanBeNull]
    public string relativePath
    {
      get => this.parentDir != null ? Path.Combine(this.parentDir, this.name) : this.name;
    }

    public int CompareTo(Item other)
    {
      return this.favorite == other.favorite ? string.CompareOrdinal(this.name, other.name) : -this.favorite.CompareTo(other.favorite);
    }
  }
}
