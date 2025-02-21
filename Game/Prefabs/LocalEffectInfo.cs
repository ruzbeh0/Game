// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LocalEffectInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using System;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class LocalEffectInfo
  {
    public LocalModifierType m_Type;
    public ModifierValueMode m_Mode;
    public float m_Delta;
    public ModifierRadiusCombineMode m_RadiusCombineMode;
    public float m_Radius;
  }
}
