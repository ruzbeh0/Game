// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.FloatFieldBuilders
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
  public class FloatFieldBuilders : IFieldBuilderFactory
  {
    private const double kGlobalValueRange = 10000000.0;

    public FieldBuilder TryCreate(System.Type memberType, object[] attributes)
    {
      if (memberType == typeof (float))
        return FloatFieldBuilders.CreateFloatFieldBuilder(attributes, -3.4028234663852886E+38, 3.4028234663852886E+38, new Converter<object, double>(ToFloat), new Converter<double, object>(FromFloat));
      if (memberType == typeof (double))
        return FloatFieldBuilders.CreateFloatFieldBuilder(attributes, double.MinValue, double.MaxValue, new Converter<object, double>(ToDouble), new Converter<double, object>(FromDouble));
      if (memberType == typeof (float2))
        return FloatFieldBuilders.CreateFloatFieldBuilder<Float2InputField, float2>(attributes);
      if (memberType == typeof (Vector2))
        return FloatFieldBuilders.CreateFloatFieldBuilder<Float2InputField, float2>(attributes, new Converter<object, float2>(ToVector2), new Converter<float2, object>(FromVector2));
      if (memberType == typeof (float3))
        return FloatFieldBuilders.CreateFloatFieldBuilder<Float3InputField, float3>(attributes);
      if (memberType == typeof (Vector3))
        return FloatFieldBuilders.CreateFloatFieldBuilder<Float3InputField, float3>(attributes, new Converter<object, float3>(ToVector3), new Converter<float3, object>(FromVector3));
      if (memberType == typeof (quaternion))
      {
        return FloatFieldBuilders.CreateFloatFieldBuilder<EulerAnglesField, float3>(attributes, new Converter<object, float3>(ToEulerAngles), new Converter<float3, object>(FromEulerAngles));

        static float3 ToEulerAngles(object value)
        {
          return (float3) ((Quaternion) (quaternion) value).eulerAngles;
        }

        static object FromEulerAngles(float3 value)
        {
          return (object) (quaternion) Quaternion.Euler((Vector3) value);
        }
      }
      if (memberType == typeof (Quaternion))
      {
        return FloatFieldBuilders.CreateFloatFieldBuilder<EulerAnglesField, float3>(attributes, new Converter<object, float3>(ToEulerAngles), new Converter<float3, object>(FromEulerAngles));

        static float3 ToEulerAngles(object value) => (float3) ((Quaternion) value).eulerAngles;

        static object FromEulerAngles(float3 value) => (object) Quaternion.Euler((Vector3) value);
      }
      if (memberType == typeof (float4))
        return FloatFieldBuilders.CreateFloatFieldBuilder<Float4InputField, float4>(attributes);
      return memberType == typeof (Vector4) ? FloatFieldBuilders.CreateFloatFieldBuilder<Float4InputField, float4>(attributes, new Converter<object, float4>(ToVector4), new Converter<float4, object>(FromVector4)) : (FieldBuilder) null;

      static double ToFloat(object value) => (double) (float) value;

      static object FromFloat(double value) => (object) (float) value;

      static double ToDouble(object value) => (double) value;

      static object FromDouble(double value) => (object) value;

      static float2 ToVector2(object value) => (float2) (Vector2) value;

      static object FromVector2(float2 value) => (object) (Vector2) value;

      static float3 ToVector3(object value) => (float3) (Vector3) value;

      static object FromVector3(float3 value) => (object) (Vector3) value;

      static float4 ToVector4(object value) => (float4) (Vector4) value;

      static object FromVector4(float4 value) => (object) (Vector4) value;
    }

    private static FieldBuilder CreateFloatFieldBuilder(
      object[] attributes,
      double min,
      double max,
      Converter<object, double> fromObject,
      Converter<double, object> toObject)
    {
      if (!EditorGenerator.sBypassValueLimits)
      {
        min = math.max(min, -10000000.0);
        max = math.min(max, 10000000.0);
      }
      double step = WidgetAttributeUtils.GetNumberStep(attributes, 0.01);
      if ((EditorGenerator.sBypassValueLimits ? 0 : (WidgetAttributeUtils.GetNumberRange(attributes, ref min, ref max) ? 1 : 0)) == 0 || WidgetAttributeUtils.RequiresInputField(attributes))
        return (FieldBuilder) (accessor =>
        {
          return (IWidget) new FloatInputField()
          {
            min = min,
            max = max,
            step = step,
            accessor = (ITypedValueAccessor<double>) new CastAccessor<double>(accessor, fromObject, toObject)
          };
        });
      string unit = WidgetAttributeUtils.GetNumberUnit(attributes);
      return (FieldBuilder) (accessor =>
      {
        return (IWidget) new FloatSliderField()
        {
          min = min,
          max = max,
          step = step,
          unit = unit,
          accessor = (ITypedValueAccessor<double>) new CastAccessor<double>(accessor, fromObject, toObject)
        };
      });
    }

    private static FieldBuilder CreateFloatFieldBuilder<TWidget, TValue>(
      object[] attributes,
      Converter<object, TValue> fromObject = null,
      Converter<TValue, object> toObject = null)
      where TWidget : FloatField<TValue>, new()
    {
      float4 min = new float4(-10000000.0);
      float4 max = new float4(10000000.0);
      double step = WidgetAttributeUtils.GetNumberStep(attributes, 0.01);
      WidgetAttributeUtils.GetNumberRange(attributes, ref min, ref max);
      return (FieldBuilder) (accessor =>
      {
        TWidget floatFieldBuilder = new TWidget()
        {
          step = step,
          accessor = fromObject == null || toObject == null ? (ITypedValueAccessor<TValue>) new CastAccessor<TValue>(accessor) : (ITypedValueAccessor<TValue>) new CastAccessor<TValue>(accessor, fromObject, toObject)
        };
        if (!EditorGenerator.sBypassValueLimits)
        {
          floatFieldBuilder.min = floatFieldBuilder.ToFieldType((double4) min);
          floatFieldBuilder.max = floatFieldBuilder.ToFieldType((double4) max);
        }
        return (IWidget) floatFieldBuilder;
      });
    }
  }
}
