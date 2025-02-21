// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ArrayAdapter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Assertions;

#nullable disable
namespace Game.UI.Widgets
{
  public class ArrayAdapter : ListAdapterBase<Array>
  {
    public override void InsertElement(int index)
    {
      Assert.IsTrue(index >= 0);
      Assert.IsTrue(index <= this.length);
      Array typedValue = this.accessor.GetTypedValue();
      object instance1 = ListAdapterBase<Array>.CreateInstance(this.elementType);
      Array instance2 = Array.CreateInstance(this.elementType, this.length + 1);
      instance2.SetValue(instance1, index);
      if (typedValue != null)
      {
        Array.Copy(typedValue, instance2, index);
        Array.Copy(typedValue, index, instance2, index + 1, typedValue.Length - index);
      }
      this.accessor.SetTypedValue(instance2);
    }

    public override void DeleteElement(int index)
    {
      Array typedValue = this.accessor.GetTypedValue();
      Array instance = Array.CreateInstance(this.elementType, typedValue.Length - 1);
      Array.Copy(typedValue, 0, instance, 0, index);
      Array.Copy(typedValue, index + 1, instance, index, typedValue.Length - index - 1);
      this.accessor.SetTypedValue(instance);
    }

    public override void Clear()
    {
      this.accessor.SetTypedValue(Array.CreateInstance(this.elementType, 0));
    }
  }
}
