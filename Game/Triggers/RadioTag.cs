// Decompiled with JetBrains decompiler
// Type: Game.Triggers.RadioTag
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;

#nullable disable
namespace Game.Triggers
{
  public struct RadioTag : IEquatable<RadioTag>
  {
    public Entity m_Event;
    public Entity m_Target;
    public Game.Audio.Radio.Radio.SegmentType m_SegmentType;
    public int m_EmergencyFrameDelay;

    public bool Equals(RadioTag other)
    {
      return this.m_Event == other.m_Event && this.m_Target == other.m_Target && this.m_SegmentType == other.m_SegmentType;
    }
  }
}
