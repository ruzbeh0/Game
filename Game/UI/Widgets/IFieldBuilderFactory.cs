// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.IFieldBuilderFactory
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public interface IFieldBuilderFactory
  {
    [CanBeNull]
    FieldBuilder TryCreate([NotNull] Type memberType, [NotNull] object[] attributes);
  }
}
