// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.StringFieldBuilders
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.UI.Widgets
{
  public class StringFieldBuilders : IFieldBuilderFactory
  {
    public FieldBuilder TryCreate(Type memberType, object[] attributes)
    {
      return memberType == typeof (string) ? WidgetReflectionUtils.CreateFieldBuilder<StringInputField, string>() : (FieldBuilder) null;
    }
  }
}
