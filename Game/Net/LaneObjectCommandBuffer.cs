// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneObjectCommandBuffer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct LaneObjectCommandBuffer
  {
    private NativeParallelQueue<LaneObjectAction>.Writer m_LaneActionQueue;
    private NativeQueue<TreeObjectAction>.ParallelWriter m_TreeActionQueue;

    public LaneObjectCommandBuffer(
      NativeParallelQueue<LaneObjectAction>.Writer laneActionQueue,
      NativeQueue<TreeObjectAction>.ParallelWriter treeActionQueue)
    {
      this.m_LaneActionQueue = laneActionQueue;
      this.m_TreeActionQueue = treeActionQueue;
    }

    public void Remove(Entity lane, Entity entity)
    {
      this.m_LaneActionQueue.Enqueue(new LaneObjectAction(lane, entity));
    }

    public void Remove(Entity entity)
    {
      this.m_TreeActionQueue.Enqueue(new TreeObjectAction(entity));
    }

    public void Add(Entity lane, Entity entity, float2 curvePosition)
    {
      this.m_LaneActionQueue.Enqueue(new LaneObjectAction(lane, entity, curvePosition));
    }

    public void Add(Entity entity, Bounds3 bounds)
    {
      this.m_TreeActionQueue.Enqueue(new TreeObjectAction(entity, bounds));
    }

    public void Update(Entity lane, Entity entity, float2 curvePosition)
    {
      this.m_LaneActionQueue.Enqueue(new LaneObjectAction(lane, entity, entity, curvePosition));
    }

    public void Update(Entity entity, Bounds3 bounds)
    {
      this.m_TreeActionQueue.Enqueue(new TreeObjectAction(entity, entity, bounds));
    }
  }
}
