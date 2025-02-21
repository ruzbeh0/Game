// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.BoundsFieldBuilders
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Reflection;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public class BoundsFieldBuilders : IFieldBuilderFactory
  {
    public FieldBuilder TryCreate(Type memberType, object[] attributes)
    {
      if (memberType == typeof (Bounds1))
      {
        float min = float.MinValue;
        float max = float.MaxValue;
        float step = WidgetAttributeUtils.GetNumberStep(attributes, 0.01f);
        bool allowMinGreaterMax = WidgetAttributeUtils.AllowsMinGreaterMax(attributes);
        return WidgetAttributeUtils.GetNumberRange(attributes, ref min, ref max) && !WidgetAttributeUtils.RequiresInputField(attributes) ? (FieldBuilder) (accessor =>
        {
          return (IWidget) new Bounds1SliderField()
          {
            min = min,
            max = max,
            step = step,
            allowMinGreaterMax = allowMinGreaterMax,
            accessor = (ITypedValueAccessor<Bounds1>) new CastAccessor<Bounds1>(accessor)
          };
        }) : (FieldBuilder) (accessor =>
        {
          return (IWidget) new Bounds1InputField()
          {
            min = min,
            max = max,
            step = step,
            allowMinGreaterMax = allowMinGreaterMax,
            accessor = (ITypedValueAccessor<Bounds1>) new CastAccessor<Bounds1>(accessor)
          };
        });
      }
      if (memberType == typeof (Bounds2))
      {
        bool allowMinGreaterMax = WidgetAttributeUtils.AllowsMinGreaterMax(attributes);
        return (FieldBuilder) (accessor =>
        {
          return (IWidget) new Bounds2InputField()
          {
            allowMinGreaterMax = allowMinGreaterMax,
            accessor = (ITypedValueAccessor<Bounds2>) new CastAccessor<Bounds2>(accessor)
          };
        });
      }
      if (!(memberType == typeof (Bounds3)))
        return (FieldBuilder) null;
      bool allowMinGreaterMax1 = WidgetAttributeUtils.AllowsMinGreaterMax(attributes);
      return (FieldBuilder) (accessor =>
      {
        return (IWidget) new Bounds3InputField()
        {
          allowMinGreaterMax = allowMinGreaterMax1,
          accessor = (ITypedValueAccessor<Bounds3>) new CastAccessor<Bounds3>(accessor)
        };
      });
    }
  }
}
