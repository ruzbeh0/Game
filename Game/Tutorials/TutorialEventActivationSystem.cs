// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialEventActivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public class TutorialEventActivationSystem : GameSystemBase
  {
    protected EntityCommandBufferSystem m_BarrierSystem;
    private NativeQueue<Entity> m_ActivationQueue;
    private JobHandle m_InputDependencies;

    public NativeQueue<Entity> GetQueue(out JobHandle dependency)
    {
      dependency = this.m_InputDependencies;
      return this.m_ActivationQueue;
    }

    public void AddQueueWriter(JobHandle dependency)
    {
      this.m_InputDependencies = JobHandle.CombineDependencies(this.m_InputDependencies, dependency);
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_BarrierSystem = (EntityCommandBufferSystem) this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      this.m_ActivationQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.m_ActivationQueue.Dispose();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_InputDependencies.Complete();
      EntityCommandBuffer commandBuffer = this.m_BarrierSystem.CreateCommandBuffer();
      Entity e;
      while (this.m_ActivationQueue.TryDequeue(out e))
        commandBuffer.AddComponent<TutorialActivated>(e);
    }

    [Preserve]
    public TutorialEventActivationSystem()
    {
    }
  }
}
