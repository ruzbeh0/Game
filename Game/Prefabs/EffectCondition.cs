// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EffectCondition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public struct EffectCondition
  {
    public EffectConditionFlags m_RequiredFlags;
    public EffectConditionFlags m_ForbiddenFlags;
    public EffectConditionFlags m_IntensityFlags;
  }
}
