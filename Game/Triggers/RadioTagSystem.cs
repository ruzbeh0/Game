// Decompiled with JetBrains decompiler
// Type: Game.Triggers.RadioTagSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Triggers
{
  public class RadioTagSystem : GameSystemBase
  {
    private static readonly int kFrameDelay = 50000;
    private static readonly int kMaxBufferSize = 10;
    private NativeParallelHashMap<Entity, uint> m_RecentTags;
    private NativeQueue<RadioTag> m_InputQueue;
    private NativeQueue<RadioTag> m_EmergencyInputQueue;
    private NativeQueue<RadioTag> m_EmergencyQueue;
    private Dictionary<Game.Audio.Radio.Radio.SegmentType, List<RadioTag>> m_Events;
    private JobHandle m_InputDependencies;
    private JobHandle m_EmergencyInputDependencies;
    private JobHandle m_EmergencyDependencies;
    private SimulationSystem m_SimulationSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_InputQueue = new NativeQueue<RadioTag>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_EmergencyInputQueue = new NativeQueue<RadioTag>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_EmergencyQueue = new NativeQueue<RadioTag>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_RecentTags = new NativeParallelHashMap<Entity, uint>(0, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      this.m_Events = new Dictionary<Game.Audio.Radio.Radio.SegmentType, List<RadioTag>>();
      this.Enabled = false;
    }

    protected override void OnGamePreload(Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_InputDependencies.Complete();
      this.m_EmergencyInputDependencies.Complete();
      this.m_EmergencyDependencies.Complete();
      RadioTag radioTag1;
      while (this.m_InputQueue.TryDequeue(out radioTag1))
      {
        List<RadioTag> radioTagList = this.EnsureList(radioTag1.m_SegmentType);
        uint num;
        if (!this.m_RecentTags.TryGetValue(radioTag1.m_Event, out num) || (long) this.m_SimulationSystem.frameIndex >= (long) num + (long) RadioTagSystem.kFrameDelay)
        {
          while (radioTagList.Contains(radioTag1))
            radioTagList.Remove(radioTag1);
          radioTagList.Add(radioTag1);
          radioTagList.RemoveRange(0, math.max(radioTagList.Count - RadioTagSystem.kMaxBufferSize, 0));
          this.m_RecentTags[radioTag1.m_Event] = this.m_SimulationSystem.frameIndex;
        }
      }
      RadioTag radioTag2;
      while (this.m_EmergencyInputQueue.TryDequeue(out radioTag2))
      {
        uint num;
        if (!this.m_RecentTags.TryGetValue(radioTag2.m_Event, out num) || (long) this.m_SimulationSystem.frameIndex >= (long) num + (long) radioTag2.m_EmergencyFrameDelay)
        {
          this.m_EmergencyQueue.Enqueue(radioTag2);
          this.m_RecentTags[radioTag2.m_Event] = this.m_SimulationSystem.frameIndex;
        }
      }
    }

    private List<RadioTag> EnsureList(Game.Audio.Radio.Radio.SegmentType segmentType)
    {
      if (!this.m_Events.ContainsKey(segmentType))
        this.m_Events.Add(segmentType, new List<RadioTag>());
      return this.m_Events[segmentType];
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      base.OnStopRunning();
      this.Clear();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.m_InputQueue.Dispose();
      this.m_EmergencyInputQueue.Dispose();
      this.m_EmergencyQueue.Dispose();
      this.m_RecentTags.Dispose();
      base.OnDestroy();
    }

    public bool TryPopEvent(Game.Audio.Radio.Radio.SegmentType segmentType, bool newestFirst, out RadioTag radioTag)
    {
      List<RadioTag> radioTagList = this.EnsureList(segmentType);
      if (radioTagList.Count > 0)
      {
        int index = newestFirst ? radioTagList.Count - 1 : 0;
        radioTag = radioTagList[index];
        radioTagList.RemoveAt(index);
        return true;
      }
      radioTag = new RadioTag();
      return false;
    }

    public void FlushEvents(Game.Audio.Radio.Radio.SegmentType segmentType)
    {
      this.EnsureList(segmentType).Clear();
    }

    public NativeQueue<RadioTag> GetInputQueue(out JobHandle deps)
    {
      Assert.IsTrue(this.Enabled, "Can not write to queue when system isn't running");
      deps = this.m_InputDependencies;
      return this.m_InputQueue;
    }

    public NativeQueue<RadioTag> GetEmergencyInputQueue(out JobHandle deps)
    {
      Assert.IsTrue(this.Enabled, "Can not write to queue when system isn't running");
      deps = this.m_EmergencyInputDependencies;
      return this.m_EmergencyInputQueue;
    }

    public NativeQueue<RadioTag> GetEmergencyQueue(out JobHandle deps)
    {
      Assert.IsTrue(this.Enabled, "Can not write to queue when system isn't running");
      deps = this.m_EmergencyDependencies;
      return this.m_EmergencyQueue;
    }

    public void AddInputQueueWriter(JobHandle handle)
    {
      this.m_InputDependencies = JobHandle.CombineDependencies(this.m_InputDependencies, handle);
    }

    public void AddEmergencyInputQueueWriter(JobHandle handle)
    {
      this.m_EmergencyInputDependencies = JobHandle.CombineDependencies(this.m_EmergencyInputDependencies, handle);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      this.Clear();
    }

    private void Clear()
    {
      this.m_InputDependencies.Complete();
      this.m_EmergencyInputDependencies.Complete();
      this.m_EmergencyDependencies.Complete();
      this.m_InputQueue.Clear();
      this.m_EmergencyInputQueue.Clear();
      this.m_EmergencyQueue.Clear();
      this.m_RecentTags.Clear();
      this.m_Events.Clear();
    }

    [Preserve]
    public RadioTagSystem()
    {
    }
  }
}
