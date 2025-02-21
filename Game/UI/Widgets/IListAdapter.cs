// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.IListAdapter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  public interface IListAdapter
  {
    int length { get; }

    bool resizable { get; }

    bool sortable { get; }

    bool UpdateRange(int startIndex, int endIndex);

    IEnumerable<IWidget> BuildElementsInRange();

    int AddElement();

    void InsertElement(int index);

    int DuplicateElement(int index);

    void MoveElement(int fromIndex, int toIndex);

    void DeleteElement(int index);

    void Clear();
  }
}
