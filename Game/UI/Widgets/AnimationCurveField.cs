// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.AnimationCurveField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class AnimationCurveField : 
    Field<AnimationCurve>,
    IMutable<AnimationCurve>,
    IWidget,
    IJsonWritable
  {
    private List<Keyframe> m_Keys = new List<Keyframe>();
    private WrapMode m_PreWrapMode;
    private WrapMode m_PostWrapMode;

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      Keyframe[] keys = this.m_Value.keys;
      if (!this.m_Keys.SequenceEqual<Keyframe>((IEnumerable<Keyframe>) keys))
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Keys.Clear();
        this.m_Keys.AddRange((IEnumerable<Keyframe>) keys);
      }
      if (this.m_Value.preWrapMode != this.m_PreWrapMode)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_PreWrapMode = this.m_Value.preWrapMode;
      }
      if (this.m_Value.postWrapMode != this.m_PostWrapMode)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_PostWrapMode = this.m_Value.postWrapMode;
      }
      return widgetChanges;
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new CallBinding<IWidget, int, Keyframe, bool, int, int>(group, "moveKeyframe", (Func<IWidget, int, Keyframe, bool, int, int>) ((widget, index, key, smooth, curveIndex) =>
        {
          bool flag = true;
          int bindings = index;
          if (widget is IMutable<AnimationCurve> mutable2)
          {
            if (index > 0 && (double) mutable2.GetValue()[index - 1].time == (double) key.time)
              flag = false;
            if (index < mutable2.GetValue().length - 1 && (double) mutable2.GetValue()[index + 1].time == (double) key.time)
              flag = false;
            if (flag)
              bindings = mutable2.GetValue().MoveKey(index, key);
            if (smooth)
              mutable2.GetValue().SmoothTangents(index, (float) (((double) key.inWeight + (double) key.outWeight) / 2.0));
            onValueChanged(widget);
            return bindings;
          }
          Debug.LogError(widget != null ? (object) "Widget does not implement IMutable<AnimationCurve>" : (object) "Invalid widget path");
          return bindings;
        }), pathResolver);
        yield return (IBinding) new CallBinding<IWidget, float, float, int, int>(group, "addKeyframe", (Func<IWidget, float, float, int, int>) ((widget, time, value, curveIndex) =>
        {
          int bindings1 = -1;
          if (widget is IMutable<AnimationCurve> mutable4)
          {
            int bindings2 = mutable4.GetValue().AddKey(time, value);
            onValueChanged(widget);
            return bindings2;
          }
          Debug.LogError(widget != null ? (object) "Widget does not implement IMutable<AnimationCurve>" : (object) "Invalid widget path");
          return bindings1;
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, Keyframe[], int>(group, "setKeyframes", (Action<IWidget, Keyframe[], int>) ((widget, keys, curveIndex) =>
        {
          if (widget is IMutable<AnimationCurve> mutable6)
          {
            while (mutable6.GetValue().length > 0)
              mutable6.GetValue().RemoveKey(0);
            for (int index = 0; index < ((IEnumerable<Keyframe>) keys).Count<Keyframe>(); ++index)
            {
              mutable6.GetValue().AddKey(keys[index].time, keys[index].value);
              mutable6.GetValue().MoveKey(index, keys[index]);
            }
            onValueChanged(widget);
          }
          else
            Debug.LogError(widget != null ? (object) "Widget does not implement IMutable<AnimationCurve>" : (object) "Invalid widget path");
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, int, int>(group, "removeKeyframe", (Action<IWidget, int, int>) ((widget, index, curveIndex) =>
        {
          if (widget is IMutable<AnimationCurve> mutable8)
          {
            mutable8.GetValue().RemoveKey(index);
            onValueChanged(widget);
          }
          else
            Debug.LogError(widget != null ? (object) "Widget does not implement IMutable<AnimationCurve>" : (object) "Invalid widget path");
        }), pathResolver);
      }
    }
  }
}
