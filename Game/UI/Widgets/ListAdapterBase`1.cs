// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ListAdapterBase`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Reflection;
using Game.UI.Editor;
using Game.UI.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class ListAdapterBase<T> : IListAdapter where T : class, IList
  {
    protected int m_StartIndex;
    protected int m_EndIndex;

    public ITypedValueAccessor<T> accessor { get; set; }

    public System.Type elementType { get; set; }

    public IEditorGenerator generator { get; set; }

    public int level { get; set; }

    public string path { get; set; }

    public object[] attributes { get; set; } = Array.Empty<object>();

    public int length
    {
      get
      {
        T typedValue = this.accessor.GetTypedValue();
        return (object) typedValue == null ? 0 : typedValue.Count;
      }
    }

    public bool resizable { get; set; } = true;

    public bool sortable => true;

    public bool UpdateRange(int startIndex, int endIndex)
    {
      if (startIndex == this.m_StartIndex && endIndex == this.m_EndIndex)
        return false;
      this.m_StartIndex = startIndex;
      this.m_EndIndex = endIndex;
      return true;
    }

    public IEnumerable<IWidget> BuildElementsInRange()
    {
      int rangeLength = this.m_EndIndex - this.m_StartIndex;
      for (int i = 0; i < rangeLength; ++i)
      {
        int index = this.m_StartIndex + i;
        IWidget widget = this.generator.Build((IValueAccessor) new ListElementAccessor<T>(this.accessor, this.elementType, index), this.attributes ?? Array.Empty<object>(), this.level + 1, string.Format("{0}[{1}]", (object) this.path, (object) index));
        if (widget is INamed named)
          named.displayName = LocalizedString.Value(string.Format("Element {0}", (object) index));
        yield return widget;
      }
    }

    public int AddElement()
    {
      int length = this.length;
      this.InsertElement(length);
      if (length > 0)
        this.CopyData(length - 1, length);
      return length;
    }

    public virtual int DuplicateElement(int index)
    {
      int num = index + 1;
      this.InsertElement(num);
      this.CopyData(index, num);
      return num;
    }

    protected virtual void CopyData(int fromIndex, int toIndex)
    {
      T typedValue = this.accessor.GetTypedValue();
      if ((object) typedValue == null)
        return;
      if (this.elementType.IsPrimitive || this.elementType == typeof (string) || typeof (PrefabBase).IsAssignableFrom(this.elementType))
      {
        typedValue[toIndex] = typedValue[fromIndex];
      }
      else
      {
        string json = JsonUtility.ToJson(typedValue[fromIndex]);
        object obj = typedValue[toIndex];
        object objectToOverwrite = obj;
        JsonUtility.FromJsonOverwrite(json, objectToOverwrite);
        typedValue[toIndex] = obj;
      }
    }

    public virtual void MoveElement(int fromIndex, int toIndex)
    {
      T typedValue = this.accessor.GetTypedValue();
      object obj = typedValue[fromIndex];
      if (toIndex < fromIndex)
      {
        for (int index = toIndex; index < fromIndex; ++index)
          typedValue[index + 1] = typedValue[index];
      }
      else if (toIndex > fromIndex)
      {
        for (int index = fromIndex; index < toIndex; ++index)
          typedValue[index] = typedValue[index + 1];
      }
      typedValue[toIndex] = obj;
    }

    public abstract void InsertElement(int index);

    public abstract void DeleteElement(int index);

    public abstract void Clear();

    public static object CreateInstance(System.Type type)
    {
      if (type == typeof (string))
        return (object) string.Empty;
      if (typeof (PrefabBase).IsAssignableFrom(type))
        return (object) null;
      if (type.IsEnum)
      {
        foreach (object instance in Enum.GetValues(type))
        {
          string name = Enum.GetName(type, instance);
          if (type.GetMember(name)[0].GetCustomAttribute<HideInEditorAttribute>() == null)
            return instance;
        }
      }
      return Activator.CreateInstance(type);
    }
  }
}
