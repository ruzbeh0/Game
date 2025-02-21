// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.EditorGenerator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.OdinSerializer;
using Colossal.OdinSerializer.Utilities;
using Colossal.Reflection;
using Game.Prefabs;
using Game.Reflection;
using Game.UI.Editor;
using Game.UI.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class EditorGenerator : IEditorGenerator
  {
    public static readonly List<IFieldBuilderFactory> kFactories = new List<IFieldBuilderFactory>()
    {
      (IFieldBuilderFactory) new CustomFieldBuilders(),
      (IFieldBuilderFactory) new ToggleFieldBuilders(),
      (IFieldBuilderFactory) new IntFieldBuilders(),
      (IFieldBuilderFactory) new UIntFieldBuilders(),
      (IFieldBuilderFactory) new TimeFieldBuilders(),
      (IFieldBuilderFactory) new FloatFieldBuilders(),
      (IFieldBuilderFactory) new BoundsFieldBuilders(),
      (IFieldBuilderFactory) new BezierFieldBuilders(),
      (IFieldBuilderFactory) new StringFieldBuilders(),
      (IFieldBuilderFactory) new ColorFieldBuilders(),
      (IFieldBuilderFactory) new AnimationCurveFieldBuilders(),
      (IFieldBuilderFactory) new EnumFieldBuilders(),
      (IFieldBuilderFactory) new PopupValueFieldBuilders()
    };
    private static readonly CustomSerializationPolicy kMemberFilter = new CustomSerializationPolicy(nameof (EditorGenerator), true, (Func<MemberInfo, bool>) (member =>
    {
      FieldInfo member1 = member as FieldInfo;
      if ((object) member1 == null || !member1.IsPublic || member1.IsDefined<NonSerializedAttribute>(true) || member1.IsDefined<HideInInspector>() || member1.IsDefined<HideInEditorAttribute>() || !(member1.Name != "active") || !(member1.Name != "dirty") || !(member1.Name != "m_Dirty"))
        return false;
      return !typeof (PrefabBase).IsAssignableFrom(member1.DeclaringType) || member1.Name != "components";
    }));

    public int maxLevel { get; set; } = 5;

    public static bool sBypassValueLimits { get; set; }

    public IWidget Build(IValueAccessor accessor, object[] attributes, int level, string path)
    {
      if (level <= this.maxLevel)
        return this.BuildMemberImpl(accessor, attributes, level, path);
      ValueField valueField = new ValueField();
      valueField.accessor = (ITypedValueAccessor<string>) new ObjectAccessor<string>(string.Empty);
      return (IWidget) valueField;
    }

    [CanBeNull]
    private IWidget TryBuildField(IValueAccessor accessor, object[] attributes, string path)
    {
      FieldBuilder fieldBuilder = this.TryCreateFieldBuilder(accessor.valueType, attributes);
      if (fieldBuilder == null)
        return (IWidget) null;
      IWidget widget = fieldBuilder(accessor);
      widget.path = (PathSegment) path;
      return widget;
    }

    [CanBeNull]
    private FieldBuilder TryCreateFieldBuilder(System.Type memberType, object[] attributes)
    {
      foreach (IFieldBuilderFactory kFactory in EditorGenerator.kFactories)
      {
        FieldBuilder fieldBuilder = kFactory.TryCreate(memberType, attributes);
        if (fieldBuilder != null)
          return fieldBuilder;
      }
      return (FieldBuilder) null;
    }

    [CanBeNull]
    public PagedList TryBuildList(
      IValueAccessor accessor,
      int level,
      string path,
      object[] attributes)
    {
      IListAdapter listAdapter = this.TryBuildListAdapter(accessor, level, path, attributes);
      if (listAdapter == null)
        return (PagedList) null;
      PagedList pagedList = new PagedList();
      pagedList.adapter = listAdapter;
      pagedList.level = level;
      pagedList.path = (PathSegment) path;
      return pagedList;
    }

    public IListAdapter TryBuildListAdapter(
      IValueAccessor accessor,
      int level,
      string path,
      object[] attributes)
    {
      if (WidgetReflectionUtils.IsListType(accessor.valueType))
      {
        System.Type listElementType = WidgetReflectionUtils.GetListElementType(accessor.valueType);
        if (listElementType != (System.Type) null)
        {
          bool flag = ((IEnumerable<object>) attributes).Any<object>((Func<object, bool>) (attr => attr is FixedLengthAttribute));
          if (accessor.valueType.IsArray)
          {
            ArrayAdapter arrayAdapter = new ArrayAdapter();
            arrayAdapter.accessor = (ITypedValueAccessor<Array>) new CastAccessor<Array>(accessor);
            arrayAdapter.elementType = listElementType;
            arrayAdapter.generator = (IEditorGenerator) this;
            arrayAdapter.level = level;
            arrayAdapter.path = path;
            arrayAdapter.resizable = !flag;
            arrayAdapter.attributes = attributes;
            return (IListAdapter) arrayAdapter;
          }
          ListAdapter listAdapter = new ListAdapter();
          listAdapter.accessor = (ITypedValueAccessor<IList>) new CastAccessor<IList>(accessor);
          listAdapter.listType = accessor.valueType;
          listAdapter.elementType = listElementType;
          listAdapter.generator = (IEditorGenerator) this;
          listAdapter.level = level;
          listAdapter.path = path;
          listAdapter.resizable = !flag;
          listAdapter.attributes = attributes;
          return (IListAdapter) listAdapter;
        }
      }
      return (IListAdapter) null;
    }

    [CanBeNull]
    private ExpandableGroup TryBuildGroup(IValueAccessor accessor, int level, string path)
    {
      if (!accessor.valueType.IsSerializable || typeof (ComponentBase).IsAssignableFrom(accessor.valueType))
        return (ExpandableGroup) null;
      ExpandableGroup expandableGroup = new ExpandableGroup();
      expandableGroup.path = (PathSegment) path;
      expandableGroup.children = (IList<IWidget>) this.BuildMembers(accessor, level, path).ToArray<IWidget>();
      return expandableGroup;
    }

    [NotNull]
    public IEnumerable<IWidget> BuildMembers(IValueAccessor accessor, int level, string parentPath)
    {
      List<MemberInfo> source = new List<MemberInfo>();
      source.AddRange((IEnumerable<MemberInfo>) this.GetSpecialMembers(accessor.valueType));
      source.AddRange((IEnumerable<MemberInfo>) FormatterUtilities.GetSerializableMembers(accessor.valueType, (ISerializationPolicy) EditorGenerator.kMemberFilter));
      return source.Select<MemberInfo, IWidget>((Func<MemberInfo, IWidget>) (member => this.BuildMember(accessor, member, level, parentPath)));
    }

    private MemberInfo[] GetSpecialMembers(System.Type type)
    {
      return type.InheritsFrom(typeof (PrefabBase)) ? typeof (UnityEngine.Object).GetMember("name", MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public) : Array.Empty<MemberInfo>();
    }

    [NotNull]
    private IWidget BuildMember(
      IValueAccessor parent,
      MemberInfo member,
      int level,
      string parentPath)
    {
      IWidget widget = this.BuildMemberImpl(ValueAccessorUtils.CreateMemberAccessor(parent, member), member.GetCustomAttributes(false), level, parentPath + "." + member.Name);
      if (widget is INamed named)
      {
        InspectorNameAttribute attribute1 = member.GetAttribute<InspectorNameAttribute>();
        EditorNameAttribute attribute2 = member.GetAttribute<EditorNameAttribute>();
        string id = attribute2 == null ? (attribute1 == null ? WidgetReflectionUtils.NicifyVariableName(member.Name) : attribute1.displayName) : attribute2.displayName;
        named.displayName = LocalizedString.IdWithFallback(id, id);
      }
      if (!(widget is ITooltipTarget tooltipTarget))
        return widget;
      TooltipAttribute attribute;
      LocalizedString? nullable = new LocalizedString?(member.TryGetAttribute<TooltipAttribute>(out attribute) ? LocalizedString.IdWithFallback(EditorGenerator.GetMemberTooltipLocaleId(member), attribute.tooltip) : LocalizedString.Id(EditorGenerator.GetMemberTooltipLocaleId(member)));
      tooltipTarget.tooltip = nullable;
      return widget;
    }

    [NotNull]
    private IWidget BuildMemberImpl(
      IValueAccessor accessor,
      object[] attributes,
      int level,
      string path)
    {
      return ((this.TryBuildField(accessor, attributes, path) ?? (IWidget) this.TryBuildList(accessor, level, path, attributes)) ?? (IWidget) this.TryBuildGroup(accessor, level, path)) ?? (IWidget) this.BuildUnknownMember(accessor.valueType);
    }

    private ValueField BuildUnknownMember(System.Type memberType)
    {
      string name = memberType.Name;
      ValueField valueField = new ValueField();
      valueField.accessor = (ITypedValueAccessor<string>) new ObjectAccessor<string>(name);
      return valueField;
    }

    private static string GetMemberTooltipLocaleId(MemberInfo member)
    {
      return "Editor.TOOLTIP[" + (member.DeclaringType != (System.Type) null ? member.DeclaringType.FullName + "." + member.Name : member.Name) + "]";
    }
  }
}
