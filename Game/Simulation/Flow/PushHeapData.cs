// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.PushHeapData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;

#nullable disable
namespace Game.Simulation.Flow
{
  public struct PushHeapData : ILessThan<PushHeapData>
  {
    public int m_NodeIndex;
    public int m_Height;

    public PushHeapData(int nodeIndex, int height)
    {
      this.m_NodeIndex = nodeIndex;
      this.m_Height = height;
    }

    public bool LessThan(PushHeapData other) => this.m_Height > other.m_Height;

    public override string ToString()
    {
      return string.Format("{0}: {1}, {2}: {3}", (object) "m_NodeIndex", (object) this.m_NodeIndex, (object) "m_Height", (object) this.m_Height);
    }
  }
}
