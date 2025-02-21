// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.EnumFieldBuilders
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Reflection;
using Game.UI.Editor;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Game.UI.Widgets
{
  public class EnumFieldBuilders : IFieldBuilderFactory
  {
    public static readonly Dictionary<Type, EnumMember[]> kMemberCache = new Dictionary<Type, EnumMember[]>();

    public FieldBuilder TryCreate(Type memberType, object[] attributes)
    {
      Converter<object, ulong> fromObject;
      Converter<ulong, object> toObject;
      if (!memberType.IsEnum || !EnumFieldBuilders.GetConverters(memberType, out fromObject, out toObject))
        return (FieldBuilder) null;
      EnumMember[] enumMembers;
      if (!EnumFieldBuilders.kMemberCache.TryGetValue(memberType, out enumMembers))
      {
        enumMembers = EnumFieldBuilders.BuildMembers(memberType, fromObject);
        EnumFieldBuilders.kMemberCache[memberType] = enumMembers;
      }
      return memberType.GetCustomAttribute(typeof (FlagsAttribute)) != null ? (FieldBuilder) (accessor =>
      {
        return (IWidget) new FlagsField()
        {
          enumMembers = enumMembers,
          accessor = (ITypedValueAccessor<ulong>) new CastAccessor<ulong>(accessor, fromObject, toObject)
        };
      }) : (FieldBuilder) (accessor =>
      {
        return (IWidget) new EnumField()
        {
          enumMembers = enumMembers,
          accessor = (ITypedValueAccessor<ulong>) new CastAccessor<ulong>(accessor, fromObject, toObject)
        };
      });
    }

    private static EnumMember[] BuildMembers(Type memberType, Converter<object, ulong> fromObject)
    {
      bool flag = memberType.GetCustomAttribute(typeof (FlagsAttribute)) != null;
      FieldInfo[] fields = memberType.GetFields(BindingFlags.Static | BindingFlags.Public);
      List<EnumMember> enumMemberList = new List<EnumMember>(fields.Length);
      for (int index = 0; index < fields.Length; ++index)
      {
        FieldInfo element = fields[index];
        if (element.GetCustomAttribute<HideInEditorAttribute>() == null)
        {
          object input = element.GetValue((object) null);
          ulong num = fromObject(input);
          if (!flag || num != 0UL)
            enumMemberList.Add(new EnumMember(fromObject(input), LocalizedString.Value(element.Name)));
        }
      }
      return enumMemberList.ToArray();
    }

    public static bool GetConverters(
      Type memberType,
      out Converter<object, ulong> fromObject,
      out Converter<ulong, object> toObject)
    {
      Type underlyingType = Enum.GetUnderlyingType(memberType);
      if (underlyingType == typeof (sbyte))
      {
        fromObject = (Converter<object, ulong>) (value => (ulong) (sbyte) value);
        toObject = (Converter<ulong, object>) (value => (object) (sbyte) value);
      }
      else if (underlyingType == typeof (byte))
      {
        fromObject = (Converter<object, ulong>) (value => (ulong) (byte) value);
        toObject = (Converter<ulong, object>) (value => (object) (byte) value);
      }
      else if (underlyingType == typeof (short))
      {
        fromObject = (Converter<object, ulong>) (value => (ulong) (short) value);
        toObject = (Converter<ulong, object>) (value => (object) (short) value);
      }
      else if (underlyingType == typeof (ushort))
      {
        fromObject = (Converter<object, ulong>) (value => (ulong) (ushort) value);
        toObject = (Converter<ulong, object>) (value => (object) (ushort) value);
      }
      else if (underlyingType == typeof (int))
      {
        fromObject = (Converter<object, ulong>) (value => (ulong) (int) value);
        toObject = (Converter<ulong, object>) (value => (object) (int) value);
      }
      else if (underlyingType == typeof (uint))
      {
        fromObject = (Converter<object, ulong>) (value => (ulong) (uint) value);
        toObject = (Converter<ulong, object>) (value => (object) (uint) value);
      }
      else if (underlyingType == typeof (long))
      {
        fromObject = (Converter<object, ulong>) (value => (ulong) (long) value);
        toObject = (Converter<ulong, object>) (value => (object) (long) value);
      }
      else if (underlyingType == typeof (ulong))
      {
        fromObject = (Converter<object, ulong>) (value => (ulong) value);
        toObject = (Converter<ulong, object>) (value => (object) value);
      }
      else
      {
        fromObject = (Converter<object, ulong>) null;
        toObject = (Converter<ulong, object>) null;
        return false;
      }
      return true;
    }
  }
}
