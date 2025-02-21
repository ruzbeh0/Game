// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.AnimationCurveFieldBuilders
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Reflection;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class AnimationCurveFieldBuilders : IFieldBuilderFactory
  {
    public FieldBuilder TryCreate(System.Type memberType, object[] attributes)
    {
      return memberType == typeof (AnimationCurve) ? (FieldBuilder) (accessor =>
      {
        if (accessor.GetValue() == null)
          accessor.SetValue((object) new AnimationCurve());
        return (IWidget) new AnimationCurveField()
        {
          accessor = (ITypedValueAccessor<AnimationCurve>) new CastAccessor<AnimationCurve>(accessor)
        };
      }) : (FieldBuilder) null;
    }
  }
}
