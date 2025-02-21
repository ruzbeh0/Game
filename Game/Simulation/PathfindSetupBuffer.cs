// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PathfindSetupBuffer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Pathfind;
using Unity.Collections;

#nullable disable
namespace Game.Simulation
{
  public struct PathfindSetupBuffer : IPathfindTargetBuffer
  {
    public NativeQueue<PathfindSetupTarget>.ParallelWriter m_Queue;
    public int m_SetupIndex;

    public void Enqueue(PathTarget pathTarget)
    {
      this.m_Queue.Enqueue(new PathfindSetupTarget()
      {
        m_SetupIndex = this.m_SetupIndex,
        m_PathTarget = pathTarget
      });
    }
  }
}
