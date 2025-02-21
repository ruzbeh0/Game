// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.IValueFieldPopup`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Reflection;
using Game.UI.Localization;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  public interface IValueFieldPopup<TValue>
  {
    IList<IWidget> children { get; }

    void Attach(ITypedValueAccessor<TValue> accessor);

    void Detach();

    bool Update();

    [NotNull]
    LocalizedString GetDisplayValue(TValue value);
  }
}
