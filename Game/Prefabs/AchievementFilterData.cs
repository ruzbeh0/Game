// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AchievementFilterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Game.UI.Widgets;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct AchievementFilterData : IBufferElementData
  {
    [InputField]
    [Range(0.0f, 37f)]
    public AchievementId m_AchievementID;
    public bool m_Allow;
  }
}
