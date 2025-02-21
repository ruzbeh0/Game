// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UITrendThresholds
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class UITrendThresholds
  {
    [Tooltip("Proportion of the actual value over which the medium trend arrows will be shown.")]
    public float m_Medium;
    [Tooltip("Proportion of the actual value over which the high trend arrows will be shown.")]
    public float m_High;
  }
}
