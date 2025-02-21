// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.PrefabItem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Prefabs;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Editor
{
  public class PrefabItem : IItemPicker.Item, IComparable<PrefabItem>
  {
    [CanBeNull]
    public PrefabBase prefab { get; set; }

    public List<string> tags { get; set; } = new List<string>();

    public int CompareTo(PrefabItem other)
    {
      return this.favorite == other.favorite ? string.CompareOrdinal(this.prefab?.name, other.prefab?.name) : -this.favorite.CompareTo(other.favorite);
    }
  }
}
