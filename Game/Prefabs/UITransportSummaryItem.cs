// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UITransportSummaryItem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using System;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class UITransportSummaryItem : UITransportItem
  {
    public StatisticType m_Statistic;
    public bool m_ShowLines = true;
  }
}
