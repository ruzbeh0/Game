// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ColorFieldBuilders
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Reflection;
using System;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class ColorFieldBuilders : IFieldBuilderFactory
  {
    public FieldBuilder TryCreate(System.Type memberType, object[] attributes)
    {
      if (memberType == typeof (Color))
        return ColorFieldBuilders.CreateColorFieldBuilder(attributes, new Converter<object, Color>(ToColor), new Converter<Color, object>(FromColor));
      return memberType == typeof (Color32) ? ColorFieldBuilders.CreateColorFieldBuilder(attributes, new Converter<object, Color>(ToColor32), new Converter<Color, object>(FromColor32)) : (FieldBuilder) null;

      static Color ToColor(object value) => (Color) value;

      static object FromColor(Color value) => (object) value;

      static Color ToColor32(object value) => (Color) (Color32) value;

      static object FromColor32(Color value) => (object) (Color32) value;
    }

    private static FieldBuilder CreateColorFieldBuilder(
      object[] attributes,
      Converter<object, Color> fromObject,
      Converter<Color, object> toObject)
    {
      bool hdr = false;
      bool showAlpha = false;
      WidgetAttributeUtils.GetColorUsage(attributes, ref hdr, ref showAlpha);
      return (FieldBuilder) (accessor =>
      {
        return (IWidget) new ColorField()
        {
          hdr = hdr,
          showAlpha = showAlpha,
          accessor = (ITypedValueAccessor<Color>) new CastAccessor<Color>(accessor, fromObject, toObject)
        };
      });
    }
  }
}
