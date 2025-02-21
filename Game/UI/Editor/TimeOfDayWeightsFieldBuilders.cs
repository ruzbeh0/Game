// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.TimeOfDayWeightsFieldBuilders
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Reflection;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.Editor
{
  public class TimeOfDayWeightsFieldBuilders : IFieldBuilderFactory
  {
    public FieldBuilder TryCreate(Type memberType, object[] attributes)
    {
      if (!(memberType == typeof (float4)))
        return (FieldBuilder) null;
      float min = 0.0f;
      float max = 1f;
      WidgetAttributeUtils.GetNumberRange(attributes, ref min, ref max);
      float step = WidgetAttributeUtils.GetNumberStep(attributes, 0.1f);
      FieldInfo xField = typeof (float4).GetField("x");
      FieldInfo yField = typeof (float4).GetField("y");
      FieldInfo zField = typeof (float4).GetField("z");
      FieldInfo wField = typeof (float4).GetField("w");
      return (FieldBuilder) (accessor =>
      {
        Group group1 = new Group();
        Group group2 = group1;
        IWidget[] widgetArray = new IWidget[5]
        {
          (IWidget) new FloatSliderField()
          {
            path = (PathSegment) "x",
            displayName = (LocalizedString) "Night",
            min = (double) min,
            max = (double) max,
            fractionDigits = 1,
            step = (double) step,
            accessor = (ITypedValueAccessor<double>) new CastAccessor<double>((IValueAccessor) new FieldAccessor(accessor, xField), new Converter<object, double>(ToFloat), new Converter<double, object>(FromFloat))
          },
          (IWidget) new FloatSliderField()
          {
            path = (PathSegment) "y",
            displayName = (LocalizedString) "Morning",
            min = (double) min,
            max = (double) max,
            fractionDigits = 1,
            step = (double) step,
            accessor = (ITypedValueAccessor<double>) new CastAccessor<double>((IValueAccessor) new FieldAccessor(accessor, yField), new Converter<object, double>(ToFloat), new Converter<double, object>(FromFloat))
          },
          (IWidget) new FloatSliderField()
          {
            path = (PathSegment) "z",
            displayName = (LocalizedString) "Day",
            min = (double) min,
            max = (double) max,
            fractionDigits = 1,
            step = (double) step,
            accessor = (ITypedValueAccessor<double>) new CastAccessor<double>((IValueAccessor) new FieldAccessor(accessor, zField), new Converter<object, double>(ToFloat), new Converter<double, object>(FromFloat))
          },
          (IWidget) new FloatSliderField()
          {
            path = (PathSegment) "w",
            displayName = (LocalizedString) "Evening",
            min = (double) min,
            max = (double) max,
            fractionDigits = 1,
            step = (double) step,
            accessor = (ITypedValueAccessor<double>) new CastAccessor<double>((IValueAccessor) new FieldAccessor(accessor, wField), new Converter<object, double>(ToFloat), new Converter<double, object>(FromFloat))
          },
          (IWidget) new TimeOfDayWeightsChart()
          {
            min = min,
            max = max,
            accessor = (ITypedValueAccessor<float4>) new CastAccessor<float4>(accessor)
          }
        };
        group2.children = (IList<IWidget>) widgetArray;
        return (IWidget) group1;
      });

      static double ToFloat(object value) => (double) (float) value;

      static object FromFloat(double value) => (object) (float) value;
    }
  }
}
