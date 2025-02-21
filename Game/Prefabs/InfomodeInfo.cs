// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.InfomodeInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class InfomodeInfo : IComparable<InfomodeInfo>
  {
    public InfomodePrefab m_Mode;
    public int m_Priority;
    public bool m_Supplemental;
    public bool m_Optional;

    public int CompareTo(InfomodeInfo other) => this.m_Priority - other.m_Priority;
  }
}
