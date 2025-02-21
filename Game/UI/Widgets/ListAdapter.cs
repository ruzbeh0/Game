// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ListAdapter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Assertions;

#nullable disable
namespace Game.UI.Widgets
{
  public class ListAdapter : ListAdapterBase<IList>
  {
    public Type listType { get; set; }

    public override void InsertElement(int index)
    {
      Assert.IsTrue(index >= 0);
      Assert.IsTrue(index <= this.length);
      IList list = this.accessor.GetTypedValue();
      if (list == null)
      {
        list = (IList) ListAdapterBase<IList>.CreateInstance(this.listType);
        this.accessor.SetTypedValue(list);
      }
      object instance = ListAdapterBase<IList>.CreateInstance(this.elementType);
      list.Insert(index, instance);
    }

    public override void DeleteElement(int index) => this.accessor.GetTypedValue().RemoveAt(index);

    public override void Clear() => this.accessor.GetTypedValue()?.Clear();

    public static ListAdapter FromList<T>([NotNull] List<T> list, IEditorGenerator generator)
    {
      ListAdapter listAdapter = new ListAdapter();
      listAdapter.accessor = (ITypedValueAccessor<IList>) new DelegateAccessor<IList>((Func<IList>) (() => (IList) list));
      listAdapter.generator = generator;
      listAdapter.elementType = typeof (T);
      listAdapter.listType = typeof (List<T>);
      listAdapter.level = 0;
      return listAdapter;
    }
  }
}
