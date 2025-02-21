// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CinematicCamera.PhotoModeUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.UI.InGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering.CinematicCamera
{
  public static class PhotoModeUtils
  {
    private static T ExtractMember<T>(Expression<Func<T>> expression, out string name)
    {
      MemberExpression body = (MemberExpression) expression.Body;
      if (!(body.Expression is MemberExpression expression2))
      {
        ConstantExpression expression1 = (ConstantExpression) body.Expression;
        name = body.Expression.Type.Name + "." + body.Member.Name;
        object obj = expression1.Value;
        return (T) ((FieldInfo) body.Member).GetValue(obj);
      }
      if (!(expression2.Expression is MemberExpression expression4))
      {
        ConstantExpression expression3 = (ConstantExpression) expression2.Expression;
        name = expression2.Type.Name + "." + body.Member.Name;
        object obj1 = expression3.Value;
        object obj2 = ((FieldInfo) expression2.Member).GetValue(obj1);
        return (T) ((FieldInfo) body.Member).GetValue(obj2);
      }
      ConstantExpression expression5 = (ConstantExpression) expression4.Expression;
      name = expression4.Type.Name + "." + expression2.Member.Name + "." + body.Member.Name;
      object obj3 = expression5.Value;
      object obj4 = ((FieldInfo) expression4.Member).GetValue(obj3);
      object obj5 = ((FieldInfo) expression2.Member).GetValue(obj4);
      return (T) ((FieldInfo) body.Member).GetValue(obj5);
    }

    private static T ExtractMember<T>(
      Expression<Func<T>> expression,
      out string name,
      out GameSystemBase systemBase)
    {
      MemberExpression body = (MemberExpression) expression.Body;
      MemberExpression expression1 = (MemberExpression) body.Expression;
      ConstantExpression expression2 = (ConstantExpression) expression1.Expression;
      name = expression1.Type.Name + "." + body.Member.Name;
      object obj = expression2.Value;
      systemBase = (GameSystemBase) ((FieldInfo) expression1.Member).GetValue(obj);
      return (T) ((PropertyInfo) body.Member).GetValue((object) systemBase);
    }

    public static PhotoModeProperty BindPropertyW(
      string tab,
      Expression<Func<Vector4Parameter>> expression,
      float min,
      float max,
      Func<bool> isAvailable = null)
    {
      return PhotoModeUtils.BindProperty(tab, expression, (Func<Vector4, float>) (v => v.w), (Func<Vector4, float, Vector4>) ((i, o) => new Vector4(i.x, i.y, i.z, o)), min, max, isAvailable);
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<FloatParameter>> expression,
      Func<bool> isAvailable = null)
    {
      string name;
      FloatParameter parameter = PhotoModeUtils.ExtractMember<FloatParameter>(expression, out name);
      float def = parameter.value;
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = new Action<float>(((VolumeParameter<float>) parameter).Override),
        getValue = (Func<float>) (() => parameter.value),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.value = def),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<FloatParameter>> expression,
      float min,
      float max,
      Func<bool> isAvailable = null)
    {
      string name;
      FloatParameter parameter = PhotoModeUtils.ExtractMember<FloatParameter>(expression, out name);
      float def = parameter.value;
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = new Action<float>(((VolumeParameter<float>) parameter).Override),
        getValue = (Func<float>) (() => parameter.value),
        min = (Func<float>) (() => min),
        max = (Func<float>) (() => max),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.value = def),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<MinFloatParameter>> expression,
      Func<bool> isAvailable = null)
    {
      string name;
      MinFloatParameter parameter = PhotoModeUtils.ExtractMember<MinFloatParameter>(expression, out name);
      float def = parameter.value;
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = new Action<float>(((VolumeParameter<float>) parameter).Override),
        getValue = (Func<float>) (() => parameter.value),
        min = (Func<float>) (() => parameter.min),
        isEnabled = (Func<bool>) (() => (isAvailable == null || isAvailable()) && parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.value = def),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<Vector4Parameter>> expression,
      Func<Vector4, float> getter,
      Func<Vector4, float, Vector4> setter,
      float min,
      float max,
      Func<bool> isAvailable = null)
    {
      string name;
      Vector4Parameter parameter = PhotoModeUtils.ExtractMember<Vector4Parameter>(expression, out name);
      Vector4 def = parameter.value;
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = (Action<float>) (v => parameter.Override(setter(parameter.value, v))),
        getValue = (Func<float>) (() => getter(parameter.value)),
        min = (Func<float>) (() => min),
        max = (Func<float>) (() => max),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.value = def),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<OverridableProperty<float>>> expression,
      Func<bool> isAvailable = null)
    {
      string name;
      GameSystemBase system;
      OverridableProperty<float> parameter = PhotoModeUtils.ExtractMember<OverridableProperty<float>>(expression, out name, out system);
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = (Action<float>) (x =>
        {
          parameter.overrideValue = x;
          system.Update();
        }),
        getValue = (Func<float>) (() => parameter.overrideValue),
        min = (Func<float>) (() => 0.0f),
        max = (Func<float>) (() => 1f),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled =>
        {
          parameter.overrideState = enabled;
          system.Update();
        }),
        reset = (Action) (() => parameter.overrideValue = parameter.value),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<ClampedFloatParameter>> expression,
      Func<bool> isAvailable = null)
    {
      string name;
      ClampedFloatParameter parameter = PhotoModeUtils.ExtractMember<ClampedFloatParameter>(expression, out name);
      float def = parameter.value;
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = new Action<float>(((VolumeParameter<float>) parameter).Override),
        getValue = (Func<float>) (() => parameter.value),
        min = (Func<float>) (() => parameter.min),
        max = (Func<float>) (() => parameter.max),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.value = def),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<ClampedIntParameter>> expression,
      Func<bool> isAvailable = null)
    {
      string name;
      ClampedIntParameter parameter = PhotoModeUtils.ExtractMember<ClampedIntParameter>(expression, out name);
      int def = parameter.value;
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = (Action<float>) (x => parameter.Override((int) x)),
        getValue = (Func<float>) (() => (float) parameter.value),
        min = (Func<float>) (() => (float) parameter.min),
        max = (Func<float>) (() => (float) parameter.max),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.value = def),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<OverridableLensProperty<float>>> expression,
      float min,
      float max,
      Func<float, float> from = null,
      Func<float, float> to = null,
      Func<bool> isAvailable = null)
    {
      string name;
      OverridableLensProperty<float> parameter = PhotoModeUtils.ExtractMember<OverridableLensProperty<float>>(expression, out name);
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = to != null ? (Action<float>) (x => parameter.Override(to(x))) : new Action<float>(parameter.Override),
        getValue = from != null ? (Func<float>) (() => from(parameter.value)) : (Func<float>) (() => parameter.value),
        min = (Func<float>) (() => min),
        max = (Func<float>) (() => max),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = new Action(parameter.Sync),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty GroupTitle(string tab, string name)
    {
      return new PhotoModeProperty()
      {
        id = name,
        group = tab
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<OverridableLensProperty<float>>> expression,
      Func<float> min,
      Func<float> max,
      Func<float, float> from = null,
      Func<float, float> to = null,
      Func<bool> isAvailable = null)
    {
      string name;
      OverridableLensProperty<float> parameter = PhotoModeUtils.ExtractMember<OverridableLensProperty<float>>(expression, out name);
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = to != null ? (Action<float>) (x => parameter.Override(to(x))) : new Action<float>(parameter.Override),
        getValue = from != null ? (Func<float>) (() => from(parameter.value)) : (Func<float>) (() => parameter.value),
        min = min,
        max = max,
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = new Action(parameter.Sync),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<OverridableLensProperty<int>>> expression,
      int min,
      int max,
      Func<bool> isAvailable = null)
    {
      string name;
      OverridableLensProperty<int> parameter = PhotoModeUtils.ExtractMember<OverridableLensProperty<int>>(expression, out name);
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = (Action<float>) (x => parameter.Override((int) x)),
        getValue = (Func<float>) (() => (float) parameter.value),
        min = (Func<float>) (() => (float) min),
        max = (Func<float>) (() => (float) max),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        fractionDigits = 0,
        reset = new Action(parameter.Sync),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<OverridableLensProperty<int>>> expression,
      int min,
      Func<bool> isAvailable = null)
    {
      string name;
      OverridableLensProperty<int> parameter = PhotoModeUtils.ExtractMember<OverridableLensProperty<int>>(expression, out name);
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = (Action<float>) (x => parameter.Override((int) x)),
        getValue = (Func<float>) (() => (float) parameter.value),
        min = (Func<float>) (() => (float) min),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        fractionDigits = 0,
        reset = new Action(parameter.Sync),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty<T>(
      string tab,
      Expression<Func<VolumeParameter<T>>> expression,
      Func<bool> isAvailable = null)
      where T : Enum
    {
      string name;
      VolumeParameter<T> parameter = PhotoModeUtils.ExtractMember<VolumeParameter<T>>(expression, out name);
      T def = parameter.value;
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = (Action<float>) (value => parameter.Override(PhotoModeUtils.FindClosestEnumValue<T>(value))),
        getValue = (Func<float>) (() => (float) Convert.ToInt32((object) parameter.value)),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.value = def),
        enumType = typeof (T),
        isAvailable = isAvailable
      };
    }

    public static PhotoModeProperty BindProperty<T>(
      string tab,
      Expression<Func<OverridableLensProperty<T>>> expression,
      Func<bool> isAvailable = null)
      where T : Enum
    {
      string name;
      OverridableLensProperty<T> parameter = PhotoModeUtils.ExtractMember<OverridableLensProperty<T>>(expression, out name);
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = (Action<float>) (value => parameter.Override(PhotoModeUtils.FindClosestEnumValue<T>(value))),
        getValue = (Func<float>) (() => (float) Convert.ToInt32((object) parameter.value)),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.Sync()),
        enumType = typeof (T),
        isAvailable = isAvailable
      };
    }

    public static T FindClosestEnumValue<T>(float value) where T : Enum
    {
      T closestEnumValue = default (T);
      float num1 = (float) int.MaxValue;
      foreach (T obj in Enum.GetValues(typeof (T)))
      {
        float int32 = (float) Convert.ToInt32((object) obj);
        float num2 = Mathf.Abs(value - int32);
        if ((double) num2 < (double) num1)
        {
          closestEnumValue = obj;
          num1 = num2;
        }
      }
      return closestEnumValue;
    }

    public static PhotoModeProperty BindProperty(
      string tab,
      Expression<Func<BoolParameter>> expression,
      Func<bool> isAvailable = null)
    {
      string name;
      BoolParameter parameter = PhotoModeUtils.ExtractMember<BoolParameter>(expression, out name);
      bool def = parameter.value;
      return new PhotoModeProperty()
      {
        id = name,
        group = tab,
        setValue = (Action<float>) (value => parameter.Override(PhotoModeUtils.FloatToBoolean(value))),
        getValue = (Func<float>) (() => PhotoModeUtils.BooleanToFloat(parameter.value)),
        isEnabled = (Func<bool>) (() => parameter.overrideState),
        setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
        reset = (Action) (() => parameter.value = def),
        overrideControl = PhotoModeProperty.OverrideControl.Checkbox,
        isAvailable = isAvailable
      };
    }

    public static bool FloatToBoolean(float value) => Mathf.RoundToInt(value) != 0;

    public static float BooleanToFloat(bool value) => !value ? 0.0f : 1f;

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<ColorParameter>> expression)
    {
      string name;
      ColorParameter parameter = PhotoModeUtils.ExtractMember<ColorParameter>(expression, out name);
      Color def = parameter.value;
      List<PhotoModeProperty> photoModePropertyList = new List<PhotoModeProperty>()
      {
        new PhotoModeProperty()
        {
          id = name + "/r",
          group = tab,
          setValue = (Action<float>) (r =>
          {
            Color x = parameter.value with { r = r };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.r),
          min = (Func<float>) (() => 0.0f),
          max = parameter.hdr ? (Func<float>) (() => float.PositiveInfinity) : (Func<float>) (() => 1f),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def),
          overrideControl = PhotoModeProperty.OverrideControl.ColorField
        },
        new PhotoModeProperty()
        {
          id = name + "/g",
          group = tab,
          setValue = (Action<float>) (g =>
          {
            Color x = parameter.value with { g = g };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.g),
          min = (Func<float>) (() => 0.0f),
          max = parameter.hdr ? (Func<float>) (() => float.PositiveInfinity) : (Func<float>) (() => 1f),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def),
          overrideControl = PhotoModeProperty.OverrideControl.ColorField
        },
        new PhotoModeProperty()
        {
          id = name + "/b",
          group = tab,
          setValue = (Action<float>) (b =>
          {
            Color x = parameter.value with { b = b };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.b),
          min = (Func<float>) (() => 0.0f),
          max = parameter.hdr ? (Func<float>) (() => float.PositiveInfinity) : (Func<float>) (() => 1f),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def),
          overrideControl = PhotoModeProperty.OverrideControl.ColorField
        }
      };
      if (parameter.showAlpha)
        photoModePropertyList.Add(new PhotoModeProperty()
        {
          id = name + "/a",
          group = tab,
          setValue = (Action<float>) (a =>
          {
            Color x = parameter.value with { a = a };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.a),
          min = (Func<float>) (() => 0.0f),
          max = parameter.hdr ? (Func<float>) (() => float.PositiveInfinity) : (Func<float>) (() => 1f),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def),
          overrideControl = PhotoModeProperty.OverrideControl.ColorField
        });
      return photoModePropertyList.ToArray();
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<Vector2Parameter>> expression,
      float min,
      float max)
    {
      return PhotoModeUtils.BindProperty(tab, expression, new Bounds1(min, max));
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<Vector2Parameter>> expression,
      Bounds1 bounds)
    {
      return PhotoModeUtils.BindProperty(tab, expression, bounds, bounds);
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<Vector2Parameter>> expression,
      Bounds1 xBounds,
      Bounds1 yBounds)
    {
      string name;
      Vector2Parameter parameter = PhotoModeUtils.ExtractMember<Vector2Parameter>(expression, out name);
      Vector2 def = parameter.value;
      return new PhotoModeProperty[2]
      {
        new PhotoModeProperty()
        {
          id = name + "/x",
          group = tab,
          setValue = (Action<float>) (x =>
          {
            Vector2 x1 = parameter.value with { x = x };
            parameter.Override(x1);
          }),
          getValue = (Func<float>) (() => parameter.value.x),
          min = (Func<float>) (() => xBounds.min),
          max = (Func<float>) (() => xBounds.max),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        },
        new PhotoModeProperty()
        {
          id = name + "/y",
          group = tab,
          setValue = (Action<float>) (y =>
          {
            Vector2 x = parameter.value with { y = y };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.y),
          min = (Func<float>) (() => yBounds.min),
          max = (Func<float>) (() => yBounds.max),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        }
      };
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<Vector2Parameter>> expression)
    {
      string name;
      Vector2Parameter parameter = PhotoModeUtils.ExtractMember<Vector2Parameter>(expression, out name);
      Vector2 def = parameter.value;
      return new PhotoModeProperty[2]
      {
        new PhotoModeProperty()
        {
          id = name + "/x",
          group = tab,
          setValue = (Action<float>) (x =>
          {
            Vector2 x1 = parameter.value with { x = x };
            parameter.Override(x1);
          }),
          getValue = (Func<float>) (() => parameter.value.x),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        },
        new PhotoModeProperty()
        {
          id = name + "/y",
          group = tab,
          setValue = (Action<float>) (y =>
          {
            Vector2 x = parameter.value with { y = y };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.y),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        }
      };
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<Vector3Parameter>> expression,
      float min,
      float max)
    {
      return PhotoModeUtils.BindProperty(tab, expression, new Bounds1(min, max));
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<Vector3Parameter>> expression,
      Bounds1 bounds)
    {
      return PhotoModeUtils.BindProperty(tab, expression, bounds, bounds, bounds);
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<Vector3Parameter>> expression,
      Bounds1 xBounds,
      Bounds1 yBounds,
      Bounds1 zBounds)
    {
      string name;
      Vector3Parameter parameter = PhotoModeUtils.ExtractMember<Vector3Parameter>(expression, out name);
      Vector3 def = parameter.value;
      return new PhotoModeProperty[3]
      {
        new PhotoModeProperty()
        {
          id = name + "/x",
          group = tab,
          setValue = (Action<float>) (x =>
          {
            Vector3 x1 = parameter.value with { x = x };
            parameter.Override(x1);
          }),
          getValue = (Func<float>) (() => parameter.value.x),
          min = (Func<float>) (() => xBounds.min),
          max = (Func<float>) (() => xBounds.max),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        },
        new PhotoModeProperty()
        {
          id = name + "/y",
          group = tab,
          setValue = (Action<float>) (y =>
          {
            Vector3 x = parameter.value with { y = y };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.y),
          min = (Func<float>) (() => yBounds.min),
          max = (Func<float>) (() => yBounds.max),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        },
        new PhotoModeProperty()
        {
          id = name + "/z",
          group = tab,
          setValue = (Action<float>) (z =>
          {
            Vector3 x = parameter.value with { z = z };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.z),
          min = (Func<float>) (() => zBounds.min),
          max = (Func<float>) (() => zBounds.max),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        }
      };
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<Vector3Parameter>> expression)
    {
      string name;
      Vector3Parameter parameter = PhotoModeUtils.ExtractMember<Vector3Parameter>(expression, out name);
      Vector3 def = parameter.value;
      return new PhotoModeProperty[3]
      {
        new PhotoModeProperty()
        {
          id = name + "/x",
          group = tab,
          setValue = (Action<float>) (x =>
          {
            Vector3 x1 = parameter.value with { x = x };
            parameter.Override(x1);
          }),
          getValue = (Func<float>) (() => parameter.value.x),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        },
        new PhotoModeProperty()
        {
          id = name + "/y",
          group = tab,
          setValue = (Action<float>) (y =>
          {
            Vector3 x = parameter.value with { y = y };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.y),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        },
        new PhotoModeProperty()
        {
          id = name + "/z",
          group = tab,
          setValue = (Action<float>) (z =>
          {
            Vector3 x = parameter.value with { z = z };
            parameter.Override(x);
          }),
          getValue = (Func<float>) (() => parameter.value.z),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.value = def)
        }
      };
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<OverridableLensProperty<Vector2>>> expression)
    {
      string name;
      OverridableLensProperty<Vector2> parameter = PhotoModeUtils.ExtractMember<OverridableLensProperty<Vector2>>(expression, out name);
      return new PhotoModeProperty[2]
      {
        new PhotoModeProperty()
        {
          id = name + "/x",
          group = tab,
          setValue = (Action<float>) (x =>
          {
            Vector2 v = parameter.value with { x = x };
            parameter.Override(v);
          }),
          getValue = (Func<float>) (() => parameter.value.x),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.Sync())
        },
        new PhotoModeProperty()
        {
          id = name + "/y",
          group = tab,
          setValue = (Action<float>) (y =>
          {
            Vector2 v = parameter.value with { y = y };
            parameter.Override(v);
          }),
          getValue = (Func<float>) (() => parameter.value.y),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.Sync())
        }
      };
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<OverridableLensProperty<Vector2>>> expression,
      float min = 0.0f,
      float max = 0.0f)
    {
      return PhotoModeUtils.BindProperty(tab, expression, new Bounds1(min, max));
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<OverridableLensProperty<Vector2>>> expression,
      Bounds1 bounds)
    {
      return PhotoModeUtils.BindProperty(tab, expression, bounds, bounds);
    }

    public static PhotoModeProperty[] BindProperty(
      string tab,
      Expression<Func<OverridableLensProperty<Vector2>>> expression,
      Bounds1 xBounds,
      Bounds1 yBounds)
    {
      string name;
      OverridableLensProperty<Vector2> parameter = PhotoModeUtils.ExtractMember<OverridableLensProperty<Vector2>>(expression, out name);
      return new PhotoModeProperty[2]
      {
        new PhotoModeProperty()
        {
          id = name + "/x",
          group = tab,
          setValue = (Action<float>) (x =>
          {
            Vector2 v = parameter.value with { x = x };
            parameter.Override(v);
          }),
          getValue = (Func<float>) (() => parameter.value.x),
          min = (Func<float>) (() => xBounds.min),
          max = (Func<float>) (() => xBounds.max),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.Sync())
        },
        new PhotoModeProperty()
        {
          id = name + "/y",
          group = tab,
          setValue = (Action<float>) (y =>
          {
            Vector2 v = parameter.value with { y = y };
            parameter.Override(v);
          }),
          getValue = (Func<float>) (() => parameter.value.y),
          min = (Func<float>) (() => yBounds.min),
          max = (Func<float>) (() => yBounds.max),
          isEnabled = (Func<bool>) (() => parameter.overrideState),
          setEnabled = (Action<bool>) (enabled => parameter.overrideState = enabled),
          reset = (Action) (() => parameter.Sync())
        }
      };
    }

    public static IEnumerable<PhotoModeProperty> ExtractMultiPropertyComponents(
      PhotoModeProperty property,
      IDictionary<string, PhotoModeProperty> allProperties)
    {
      int num = property.id.IndexOf("/");
      if (num < 0)
      {
        yield return property;
      }
      else
      {
        string name = property.id.Substring(0, num + 1);
        foreach (KeyValuePair<string, PhotoModeProperty> allProperty in (IEnumerable<KeyValuePair<string, PhotoModeProperty>>) allProperties)
        {
          if (allProperty.Key.StartsWith(name))
            yield return allProperty.Value;
        }
        name = (string) null;
      }
    }

    public static string ExtractPropertyID(PhotoModeProperty property)
    {
      int length = property.id.IndexOf("/");
      return length < 0 ? property.id : property.id.Substring(0, length);
    }

    public static PhotoModeUIPreset CreatePreset(
      string name,
      PhotoModeProperty injectionProperty,
      PhotoModeProperty[] targetProperties,
      string[] options,
      Vector2[] values)
    {
      if (targetProperties.Length != 2)
        throw new ArgumentException("targetProperties must be of length 2 with Vector2 values");
      PresetDescriptor presetDescriptor = new PresetDescriptor();
      presetDescriptor.AddOptions((IEnumerable<string>) options);
      foreach (PhotoModeProperty targetProperty in targetProperties)
      {
        if (targetProperty.id.EndsWith("x"))
          presetDescriptor.AddValues(targetProperty, ((IEnumerable<Vector2>) values).Select<Vector2, float>((Func<Vector2, float>) (v => v.x)).ToArray<float>());
        else if (targetProperty.id.EndsWith("y"))
          presetDescriptor.AddValues(targetProperty, ((IEnumerable<Vector2>) values).Select<Vector2, float>((Func<Vector2, float>) (v => v.y)).ToArray<float>());
      }
      if (!presetDescriptor.Validate())
        throw new ArgumentException("Preset descriptor is invalid");
      return new PhotoModeUIPreset()
      {
        id = name,
        injectionProperty = injectionProperty,
        descriptor = presetDescriptor
      };
    }
  }
}
