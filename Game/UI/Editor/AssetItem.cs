// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.AssetItem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System;

#nullable disable
namespace Game.UI.Editor
{
  public class AssetItem : IItemPicker.Item, IComparable<AssetItem>
  {
    public Hash128 guid { get; set; }

    public string fileName { get; set; }

    public int CompareTo(AssetItem other)
    {
      return this.favorite == other.favorite ? string.CompareOrdinal(this.fileName, other.fileName) : -this.favorite.CompareTo(other.favorite);
    }
  }
}
