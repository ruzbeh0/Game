// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.IntFieldBuilders
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Reflection;
using System;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class IntFieldBuilders : IFieldBuilderFactory
  {
    private static readonly int kGlobalValueRange = 10000000;

    public FieldBuilder TryCreate(System.Type memberType, object[] attributes)
    {
      if (memberType == typeof (byte))
        return IntFieldBuilders.CreateIntFieldBuilder(attributes, 0, (int) byte.MaxValue, new Converter<object, int>(ToByte), new Converter<int, object>(FromByte));
      if (memberType == typeof (sbyte))
        return IntFieldBuilders.CreateIntFieldBuilder(attributes, (int) sbyte.MinValue, (int) sbyte.MaxValue, new Converter<object, int>(ToSByte), new Converter<int, object>(FromSByte));
      if (memberType == typeof (short))
        return IntFieldBuilders.CreateIntFieldBuilder(attributes, (int) short.MinValue, (int) short.MaxValue, new Converter<object, int>(ToShort), new Converter<int, object>(FromShort));
      if (memberType == typeof (ushort))
        return IntFieldBuilders.CreateIntFieldBuilder(attributes, 0, (int) ushort.MaxValue, new Converter<object, int>(ToUShort), new Converter<int, object>(FromUShort));
      if (memberType == typeof (int))
        return IntFieldBuilders.CreateIntFieldBuilder(attributes, int.MinValue, int.MaxValue, new Converter<object, int>(ToInt), new Converter<int, object>(FromInt));
      if (memberType == typeof (int2))
        return IntFieldBuilders.CreateIntFieldBuilder<Int2InputField, int2>(attributes, new Converter<object, int2>(ToInt2), new Converter<int2, object>(FromInt2));
      if (memberType == typeof (Vector2Int))
        return IntFieldBuilders.CreateIntFieldBuilder<Int2InputField, int2>(attributes, new Converter<object, int2>(ToVector2Int), new Converter<int2, object>(FromVector2Int));
      if (memberType == typeof (int3))
        return IntFieldBuilders.CreateIntFieldBuilder<Int3InputField, int3>(attributes, new Converter<object, int3>(ToInt3), new Converter<int3, object>(FromInt3));
      if (memberType == typeof (Vector3Int))
        return IntFieldBuilders.CreateIntFieldBuilder<Int3InputField, int3>(attributes, new Converter<object, int3>(ToVector3Int), new Converter<int3, object>(FromVector3Int));
      return memberType == typeof (int4) ? IntFieldBuilders.CreateIntFieldBuilder<Int4InputField, int4>(attributes, new Converter<object, int4>(ToInt4), new Converter<int4, object>(FromInt4)) : (FieldBuilder) null;

      static int ToByte(object value) => (int) (byte) value;

      static object FromByte(int value) => (object) (byte) value;

      static int ToSByte(object value) => (int) (sbyte) value;

      static object FromSByte(int value) => (object) (sbyte) value;

      static int ToShort(object value) => (int) (short) value;

      static object FromShort(int value) => (object) (short) value;

      static int ToUShort(object value) => (int) (ushort) value;

      static object FromUShort(int value) => (object) (ushort) value;

      static int ToInt(object value) => (int) value;

      static object FromInt(int value) => (object) value;

      static int2 ToInt2(object value) => (int2) value;

      static object FromInt2(int2 value) => (object) value;

      static int2 ToVector2Int(object value)
      {
        Vector2Int vector2Int = (Vector2Int) value;
        return new int2(vector2Int.x, vector2Int.y);
      }

      static object FromVector2Int(int2 value) => (object) new Vector2Int(value.x, value.y);

      static int3 ToInt3(object value) => (int3) value;

      static object FromInt3(int3 value) => (object) value;

      static int3 ToVector3Int(object value)
      {
        Vector3Int vector3Int = (Vector3Int) value;
        return new int3(vector3Int.x, vector3Int.y, vector3Int.z);
      }

      static object FromVector3Int(int3 value)
      {
        return (object) new Vector3Int(value.x, value.y, value.z);
      }

      static int4 ToInt4(object value) => (int4) value;

      static object FromInt4(int4 value) => (object) value;
    }

    private static FieldBuilder CreateIntFieldBuilder(
      object[] attributes,
      int min,
      int max,
      Converter<object, int> fromObject,
      Converter<int, object> toObject)
    {
      if (!EditorGenerator.sBypassValueLimits)
      {
        min = math.max(min, -IntFieldBuilders.kGlobalValueRange);
        max = math.min(max, IntFieldBuilders.kGlobalValueRange);
      }
      int step = WidgetAttributeUtils.GetNumberStep(attributes, 1);
      if ((EditorGenerator.sBypassValueLimits ? 0 : (WidgetAttributeUtils.GetNumberRange(attributes, ref min, ref max) ? 1 : 0)) == 0 || WidgetAttributeUtils.RequiresInputField(attributes))
        return (FieldBuilder) (accessor =>
        {
          return (IWidget) new IntInputField()
          {
            min = min,
            max = max,
            step = step,
            accessor = (ITypedValueAccessor<int>) new CastAccessor<int>(accessor, fromObject, toObject)
          };
        });
      string unit = WidgetAttributeUtils.GetNumberUnit(attributes);
      return (FieldBuilder) (accessor =>
      {
        return (IWidget) new IntSliderField()
        {
          min = min,
          max = max,
          step = step,
          unit = unit,
          accessor = (ITypedValueAccessor<int>) new CastAccessor<int>(accessor, fromObject, toObject)
        };
      });
    }

    private static FieldBuilder CreateIntFieldBuilder<TWidget, TValue>(
      object[] attributes,
      Converter<object, TValue> fromObject,
      Converter<TValue, object> toObject)
      where TWidget : IntField<TValue>, new()
    {
      int step = WidgetAttributeUtils.GetNumberStep(attributes, 1);
      int min = EditorGenerator.sBypassValueLimits ? int.MinValue : -IntFieldBuilders.kGlobalValueRange;
      int max = EditorGenerator.sBypassValueLimits ? int.MaxValue : IntFieldBuilders.kGlobalValueRange;
      if (!EditorGenerator.sBypassValueLimits)
        WidgetAttributeUtils.GetNumberRange(attributes, ref min, ref max);
      return (FieldBuilder) (accessor =>
      {
        return (IWidget) new TWidget()
        {
          min = min,
          max = max,
          step = step,
          accessor = (fromObject == null || toObject == null ? (ITypedValueAccessor<TValue>) new CastAccessor<TValue>(accessor) : (ITypedValueAccessor<TValue>) new CastAccessor<TValue>(accessor, fromObject, toObject))
        };
      });
    }
  }
}
