// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.WidgetReflectionUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace Game.UI.Widgets
{
  public static class WidgetReflectionUtils
  {
    public static bool IsListType(Type memberType)
    {
      if (memberType.IsArray)
        return true;
      return typeof (IList).IsAssignableFrom(memberType) && ((IEnumerable<Type>) memberType.GetInterfaces()).Any<Type>(new Func<Type, bool>(WidgetReflectionUtils.IsGenericListInterface));
    }

    [CanBeNull]
    public static Type GetListElementType(Type memberType)
    {
      if (memberType.IsArray)
        return memberType.GetElementType();
      Type type = ((IEnumerable<Type>) memberType.GetInterfaces()).FirstOrDefault<Type>(new Func<Type, bool>(WidgetReflectionUtils.IsGenericListInterface));
      return type != (Type) null ? type.GenericTypeArguments[0] : (Type) null;
    }

    private static bool IsGenericListInterface(Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IList<>);
    }

    public static FieldBuilder CreateFieldBuilder<T, U>() where T : Field<U>, new()
    {
      return (FieldBuilder) (accessor =>
      {
        return (IWidget) new T()
        {
          accessor = (ITypedValueAccessor<U>) new CastAccessor<U>(accessor)
        };
      });
    }

    public static FieldBuilder CreateFieldBuilder<T, U>(
      Converter<object, U> fromObject,
      Converter<U, object> toObject)
      where T : Field<U>, new()
    {
      return (FieldBuilder) (accessor =>
      {
        return (IWidget) new T()
        {
          accessor = (ITypedValueAccessor<U>) new CastAccessor<U>(accessor, fromObject, toObject)
        };
      });
    }

    public static string NicifyVariableName(string name)
    {
      if (name == null)
        return string.Empty;
      if (name.StartsWith("m_"))
        name = name.Substring(2);
      else if (name.StartsWith("Get"))
        name = name.Substring(3);
      name = Regex.Replace(name, "\\B([A-Z][a-z])", " $1");
      name = Regex.Replace(name, "([^A-Z\\s])([A-Z])", "$1 $2");
      name = Regex.Replace(name, "(?<![\\d\\s]|\\dx|\\d-)(\\d)", " $1");
      name = Regex.Replace(name, "^([a-z])", (MatchEvaluator) (match => match.Value.ToUpperInvariant()));
      return name;
    }
  }
}
