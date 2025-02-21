// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.IGradientInfomode
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Localization;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public interface IGradientInfomode
  {
    GradientLegendType legendType { get; }

    LocalizedString? lowLabel { get; }

    LocalizedString? mediumLabel { get; }

    LocalizedString? highLabel { get; }

    Color lowColor { get; }

    Color mediumColor { get; }

    Color highColor { get; }
  }
}
