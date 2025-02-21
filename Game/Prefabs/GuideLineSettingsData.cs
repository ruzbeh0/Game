// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GuideLineSettingsData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct GuideLineSettingsData : IComponentData, IQueryTypeParameter
  {
    public Color m_VeryLowPriorityColor;
    public Color m_LowPriorityColor;
    public Color m_MediumPriorityColor;
    public Color m_HighPriorityColor;
  }
}
