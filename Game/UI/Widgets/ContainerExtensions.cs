// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ContainerExtensions
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.UI.Localization;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  public static class ContainerExtensions
  {
    [CanBeNull]
    public static IWidget FindChild(this IWidget widget, PathSegment path)
    {
      return ContainerExtensions.FindChild<IWidget>((IEnumerable<IWidget>) widget.visibleChildren, path);
    }

    [CanBeNull]
    public static T FindChild<T>(IEnumerable<T> children, PathSegment path) where T : IWidget
    {
      if (path != PathSegment.Empty && children != null)
      {
        foreach (T child in children)
        {
          if (child.path == path)
            return child;
        }
      }
      return default (T);
    }

    public static void SetDefaults<T>(IList<T> children) where T : IWidget
    {
      for (int index = 0; index < children.Count; ++index)
      {
        T child = children[index];
        if (child.path.m_Key == null)
          child.path = new PathSegment(index);
        if (child is INamed named && named.displayName.isEmpty)
          named.displayName = LocalizedString.Value(string.Format("<{0}>", (object) index));
      }
    }
  }
}
