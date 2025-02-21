// Decompiled with JetBrains decompiler
// Type: Game.Simulation.XPSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class XPSystem : GameSystemBase, IXPSystem
  {
    private NativeQueue<XPMessage> m_XPMessages;
    private NativeQueue<XPGain> m_XPQueue;
    private JobHandle m_QueueWriters;
    private CitySystem m_CitySystem;
    private SimulationSystem m_SimulationSystem;
    private XPSystem.TypeHandle __TypeHandle;

    public void TransferMessages(IXPMessageHandler handler)
    {
      this.Dependency.Complete();
      // ISSUE: reference to a compiler-generated field
      while (this.m_XPMessages.Count > 0)
      {
        // ISSUE: reference to a compiler-generated field
        XPMessage message = this.m_XPMessages.Dequeue();
        handler.AddMessage(message);
      }
    }

    public NativeQueue<XPGain> GetQueue(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_QueueWriters;
      // ISSUE: reference to a compiler-generated field
      return this.m_XPQueue;
    }

    public void AddQueueWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_QueueWriters = JobHandle.CombineDependencies(this.m_QueueWriters, handle);
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_XPMessages = new NativeQueue<XPMessage>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_XPQueue = new NativeQueue<XPGain>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_XPQueue.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_XPMessages.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_CitySystem.City == Entity.Null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_XP_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      XPSystem.XPQueueProcessJob jobData = new XPSystem.XPQueueProcessJob()
      {
        m_City = this.m_CitySystem.City,
        m_FrameIndex = this.m_SimulationSystem.frameIndex,
        m_XPMessages = this.m_XPMessages,
        m_XPQueue = this.m_XPQueue,
        m_CityXPs = this.__TypeHandle.__Game_City_XP_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<XPSystem.XPQueueProcessJob>(JobHandle.CombineDependencies(this.m_QueueWriters, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_QueueWriters = this.Dependency;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public XPSystem()
    {
    }

    private struct XPQueueProcessJob : IJob
    {
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public uint m_FrameIndex;
      public ComponentLookup<XP> m_CityXPs;
      public NativeQueue<XPGain> m_XPQueue;
      public NativeQueue<XPMessage> m_XPMessages;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        XP cityXp = this.m_CityXPs[this.m_City];
        XPGain xpGain;
        // ISSUE: reference to a compiler-generated field
        while (this.m_XPQueue.TryDequeue(out xpGain))
        {
          if (xpGain.amount != 0)
          {
            cityXp.m_XP += xpGain.amount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_XPMessages.Enqueue(new XPMessage(this.m_FrameIndex, xpGain.amount, xpGain.reason));
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityXPs[this.m_City] = cityXp;
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<XP> __Game_City_XP_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_XP_RW_ComponentLookup = state.GetComponentLookup<XP>();
      }
    }
  }
}
