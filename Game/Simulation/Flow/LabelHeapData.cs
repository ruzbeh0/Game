// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.LabelHeapData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;

#nullable disable
namespace Game.Simulation.Flow
{
  public struct LabelHeapData : ILessThan<LabelHeapData>
  {
    public int m_NodeIndex;
    public int m_Distance;

    public LabelHeapData(int nodeIndex, int distance)
    {
      this.m_NodeIndex = nodeIndex;
      this.m_Distance = distance;
    }

    public bool LessThan(LabelHeapData other) => this.m_Distance < other.m_Distance;

    public override string ToString()
    {
      return string.Format("{0}: {1}, {2}: {3}", (object) "m_NodeIndex", (object) this.m_NodeIndex, (object) "m_Distance", (object) this.m_Distance);
    }
  }
}
