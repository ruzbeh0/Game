// Decompiled with JetBrains decompiler
// Type: Game.UpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game
{
  public class UpdateSystem : GameSystemBase
  {
    private List<IGPUSystem> m_GPUSystems;
    private List<UpdateSystem.SystemData> m_Systems;
    private List<UpdateSystem.SystemData> m_Updates;
    private List<int2> m_UpdateRanges;
    private Dictionary<ComponentSystemBase, List<UpdateSystem.SystemData>> m_RefMap;
    private int m_AddIndex;
    private bool m_IsDirty;

    public SystemUpdatePhase currentPhase { get; private set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      RenderPipelineManager.beginFrameRendering += new Action<ScriptableRenderContext, Camera[]>(this.OnBeginFrame);
      this.m_GPUSystems = new List<IGPUSystem>();
      this.m_Systems = new List<UpdateSystem.SystemData>(1000);
      this.m_Updates = new List<UpdateSystem.SystemData>(1000);
      this.m_UpdateRanges = new List<int2>(100);
      this.m_RefMap = new Dictionary<ComponentSystemBase, List<UpdateSystem.SystemData>>(100);
      this.currentPhase = SystemUpdatePhase.Invalid;
    }

    protected virtual void OnBeginFrame(ScriptableRenderContext renderContext, Camera[] cameras)
    {
      foreach (IGPUSystem gpuSystem in this.m_GPUSystems)
      {
        if (gpuSystem.Enabled)
        {
          CommandBuffer commandBuffer = CommandBufferPool.Get("");
          if (gpuSystem.IsAsync)
            commandBuffer.SetExecutionFlags(CommandBufferExecutionFlags.AsyncCompute);
          gpuSystem.OnSimulateGPU(commandBuffer);
          if (gpuSystem.IsAsync)
            renderContext.ExecuteCommandBufferAsync(commandBuffer, ComputeQueueType.Default);
          else
            renderContext.ExecuteCommandBuffer(commandBuffer);
          renderContext.Submit();
          commandBuffer.Clear();
          CommandBufferPool.Release(commandBuffer);
        }
      }
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    [Preserve]
    protected override void OnDestroy()
    {
      RenderPipelineManager.beginFrameRendering -= new Action<ScriptableRenderContext, Camera[]>(this.OnBeginFrame);
      this.m_GPUSystems.Clear();
      base.OnDestroy();
    }

    public void RegisterGPUSystem<SystemType>() where SystemType : ComponentSystemBase, IGPUSystem
    {
      this.RegisterGPUSystem((IGPUSystem) this.World.GetOrCreateSystemManaged<SystemType>());
    }

    public void RegisterGPUSystem(IGPUSystem system)
    {
      if (this.m_GPUSystems.Contains(system))
        return;
      this.m_GPUSystems.Add(system);
    }

    public void UpdateAt<SystemType>(SystemUpdatePhase phase) where SystemType : ComponentSystemBase
    {
      this.Register(++this.m_AddIndex, (ComponentSystemBase) this.World.GetOrCreateSystemManaged<SystemType>(), phase);
    }

    public void UpdateBefore<SystemType>(SystemUpdatePhase phase) where SystemType : ComponentSystemBase
    {
      this.Register(++this.m_AddIndex - 1000000, (ComponentSystemBase) this.World.GetOrCreateSystemManaged<SystemType>(), phase);
    }

    public void UpdateAfter<SystemType>(SystemUpdatePhase phase) where SystemType : ComponentSystemBase
    {
      this.Register(++this.m_AddIndex + 1000000, (ComponentSystemBase) this.World.GetOrCreateSystemManaged<SystemType>(), phase);
    }

    public void UpdateBefore<SystemType, OtherType>(SystemUpdatePhase phase)
      where SystemType : ComponentSystemBase
      where OtherType : ComponentSystemBase
    {
      this.Register(++this.m_AddIndex - 1000000, (ComponentSystemBase) this.World.GetOrCreateSystemManaged<SystemType>(), (ComponentSystemBase) this.World.GetOrCreateSystemManaged<OtherType>(), phase);
    }

    public void UpdateAfter<SystemType, OtherType>(SystemUpdatePhase phase)
      where SystemType : ComponentSystemBase
      where OtherType : ComponentSystemBase
    {
      this.Register(++this.m_AddIndex + 1000000, (ComponentSystemBase) this.World.GetOrCreateSystemManaged<SystemType>(), (ComponentSystemBase) this.World.GetOrCreateSystemManaged<OtherType>(), phase);
    }

    public void Update(SystemUpdatePhase phase)
    {
      if (this.m_IsDirty)
        this.Refresh();
      if ((SystemUpdatePhase) this.m_UpdateRanges.Count <= phase)
        return;
      SystemUpdatePhase currentPhase = this.currentPhase;
      try
      {
        this.currentPhase = phase;
        int2 updateRange = this.m_UpdateRanges[(int) phase];
        for (int x = updateRange.x; x < updateRange.y; ++x)
        {
          UpdateSystem.SystemData update = this.m_Updates[x];
          try
          {
            update.m_System.Update();
          }
          catch (Exception ex)
          {
            COSystemBase.baseLog.CriticalFormat(ex, "System update error during {0}->{1}:", (object) phase.ToString(), (object) update.m_System.GetType().Name);
          }
        }
      }
      finally
      {
        this.currentPhase = currentPhase;
      }
    }

    public void Update(SystemUpdatePhase phase, uint updateIndex, int iterationIndex)
    {
      if (this.m_IsDirty)
        this.Refresh();
      if ((SystemUpdatePhase) this.m_UpdateRanges.Count <= phase)
        return;
      SystemUpdatePhase currentPhase = this.currentPhase;
      try
      {
        this.currentPhase = phase;
        int2 updateRange = this.m_UpdateRanges[(int) phase];
        for (int x = updateRange.x; x < updateRange.y; ++x)
        {
          UpdateSystem.SystemData update = this.m_Updates[x];
          if (((int) updateIndex & update.m_Interval - 1) == update.m_Offset)
          {
            try
            {
              if (update.m_ResetInterval <= iterationIndex)
                ((GameSystemBase) update.m_System).ResetDependency();
              update.m_System.Update();
            }
            catch (Exception ex)
            {
              COSystemBase.baseLog.CriticalFormat(ex, "System update error during {0}->{1}:", (object) phase.ToString(), (object) update.m_System.GetType().Name);
            }
          }
        }
      }
      finally
      {
        this.currentPhase = currentPhase;
      }
    }

    private void Register(int addIndex, ComponentSystemBase system, SystemUpdatePhase phase)
    {
      int interval;
      int offset;
      UpdateSystem.GetInterval(system, phase, out interval, out offset);
      this.m_Systems.Add(new UpdateSystem.SystemData(phase, interval, offset, addIndex, system));
      this.m_IsDirty = true;
    }

    private void Register(
      int addIndex,
      ComponentSystemBase system,
      ComponentSystemBase other,
      SystemUpdatePhase phase)
    {
      int interval;
      int offset;
      UpdateSystem.GetInterval(system, phase, out interval, out offset);
      List<UpdateSystem.SystemData> systemDataList;
      if (this.m_RefMap.TryGetValue(other, out systemDataList))
        systemDataList.Add(new UpdateSystem.SystemData(phase, interval, offset, addIndex, system));
      else
        this.m_RefMap.Add(other, new List<UpdateSystem.SystemData>(10)
        {
          new UpdateSystem.SystemData(phase, interval, offset, addIndex, system)
        });
      this.m_IsDirty = true;
    }

    public static void GetInterval(
      ComponentSystemBase system,
      SystemUpdatePhase phase,
      out int interval,
      out int offset)
    {
      interval = 1;
      offset = -1;
      if (system is GameSystemBase gameSystemBase)
      {
        interval = gameSystemBase.GetUpdateInterval(phase);
        offset = gameSystemBase.GetUpdateOffset(phase);
      }
      if (!math.ispow2(interval))
        throw new Exception("System update interval not power of 2");
    }

    private void Refresh()
    {
      this.m_IsDirty = false;
      this.m_Updates.Clear();
      this.m_UpdateRanges.Clear();
      if (this.m_Systems.Count >= 2)
        this.m_Systems.Sort();
      List<UpdateSystem.IntervalData> intervalList = new List<UpdateSystem.IntervalData>(1000);
      int index1;
      for (int index2 = 0; index2 < this.m_Systems.Count; index2 = index1)
      {
        int count = this.m_Updates.Count;
        intervalList.Clear();
        UpdateSystem.SystemData system1 = this.m_Systems[index2];
        SystemUpdatePhase phase = system1.m_Phase;
        this.AddSystemUpdate(intervalList, system1, false, 0);
        for (index1 = index2 + 1; index1 < this.m_Systems.Count; ++index1)
        {
          UpdateSystem.SystemData system2 = this.m_Systems[index1];
          if (system2.m_Phase == system1.m_Phase)
            this.AddSystemUpdate(intervalList, system2, false, 0);
          else
            break;
        }
        if (intervalList.Count != 0)
        {
          if (intervalList.Count >= 2)
            intervalList.Sort();
          int num1 = 0;
          int num2 = 0;
          int num3 = 0;
          for (int index3 = 0; index3 < intervalList.Count; ++index3)
          {
            UpdateSystem.IntervalData intervalData = intervalList[index3];
            if (intervalData.m_Interval != num1)
            {
              num1 = intervalData.m_Interval;
              num3 = 0;
            }
            UpdateSystem.SystemData update = this.m_Updates[intervalData.m_UpdateIndex] with
            {
              m_Offset = num2
            };
            this.PatchSystemOffset(ref intervalData.m_UpdateStart, update, true, 0);
            ++num3;
            int num4 = 0;
            do
              ;
            while ((num3 & 1 << num4++) == 0);
            num2 = num2 + (num1 >> num4) & num1 - 1;
            if (num3 << 1 >= num1)
              num3 = 0;
          }
        }
        if (this.m_Updates.Count > count)
        {
          while ((SystemUpdatePhase) this.m_UpdateRanges.Count < phase)
            this.m_UpdateRanges.Add((int2) count);
          this.m_UpdateRanges.Add(new int2(count, this.m_Updates.Count));
        }
      }
    }

    private void AddSystemUpdate(
      List<UpdateSystem.IntervalData> intervalList,
      UpdateSystem.SystemData systemData,
      bool inheritOffset,
      int safety)
    {
      if (++safety == 100)
        throw new Exception("Too deep system order");
      List<UpdateSystem.SystemData> systemDataList;
      if (this.m_RefMap.TryGetValue(systemData.m_System, out systemDataList))
      {
        if (systemDataList.Count >= 2)
          systemDataList.Sort();
        int count = this.m_Updates.Count;
        int num = 0;
        while (num < systemDataList.Count)
        {
          UpdateSystem.SystemData systemData1 = systemDataList[num++];
          if (systemData1.m_Phase == systemData.m_Phase)
          {
            if (systemData1.m_AddIndex >= 0)
            {
              --num;
              break;
            }
            bool inheritOffset1 = systemData1.m_Interval == systemData.m_Interval && systemData1.m_Offset < 0;
            if (inheritOffset1)
              systemData1.m_Offset = systemData.m_Offset;
            this.AddSystemUpdate(intervalList, systemData1, inheritOffset1, safety);
          }
        }
        if (systemData.m_Offset < 0)
        {
          if (systemData.m_Interval == 1)
            systemData.m_Offset = 0;
          else if (!inheritOffset)
            intervalList.Add(new UpdateSystem.IntervalData(systemData.m_Interval, count, this.m_Updates.Count));
        }
        this.m_Updates.Add(systemData);
        while (num < systemDataList.Count)
        {
          UpdateSystem.SystemData systemData2 = systemDataList[num++];
          if (systemData2.m_Phase != systemData.m_Phase)
            break;
          bool inheritOffset2 = systemData2.m_Interval == systemData.m_Interval && systemData2.m_Offset < 0;
          if (inheritOffset2)
            systemData2.m_Offset = systemData.m_Offset;
          this.AddSystemUpdate(intervalList, systemData2, inheritOffset2, safety);
        }
      }
      else
      {
        if (systemData.m_Offset < 0)
        {
          if (systemData.m_Interval == 1)
            systemData.m_Offset = 0;
          else if (!inheritOffset)
            intervalList.Add(new UpdateSystem.IntervalData(systemData.m_Interval, this.m_Updates.Count, this.m_Updates.Count));
        }
        this.m_Updates.Add(systemData);
      }
    }

    private void PatchSystemOffset(
      ref int updateIndex,
      UpdateSystem.SystemData systemData,
      bool inheritOffset,
      int safety)
    {
      if (++safety == 100)
        throw new Exception("Too deep system order");
      List<UpdateSystem.SystemData> systemDataList;
      if (this.m_RefMap.TryGetValue(systemData.m_System, out systemDataList))
      {
        int num = 0;
        while (num < systemDataList.Count)
        {
          UpdateSystem.SystemData systemData1 = systemDataList[num++];
          if (systemData1.m_Phase == systemData.m_Phase)
          {
            if (systemData1.m_AddIndex >= 0)
            {
              --num;
              break;
            }
            bool inheritOffset1 = systemData1.m_Interval == systemData.m_Interval && systemData1.m_Offset < 0;
            if (inheritOffset1)
              systemData1.m_Offset = systemData.m_Offset;
            this.PatchSystemOffset(ref updateIndex, systemData1, inheritOffset1, safety);
          }
        }
        if (inheritOffset)
        {
          UpdateSystem.SystemData update = this.m_Updates[updateIndex] with
          {
            m_Offset = systemData.m_Offset
          };
          this.m_Updates[updateIndex] = update;
        }
        ++updateIndex;
        while (num < systemDataList.Count)
        {
          UpdateSystem.SystemData systemData2 = systemDataList[num++];
          if (systemData2.m_Phase != systemData.m_Phase)
            break;
          bool inheritOffset2 = systemData2.m_Interval == systemData.m_Interval && systemData2.m_Offset < 0;
          if (inheritOffset2)
            systemData2.m_Offset = systemData.m_Offset;
          this.PatchSystemOffset(ref updateIndex, systemData2, inheritOffset2, safety);
        }
      }
      else
      {
        if (inheritOffset)
        {
          UpdateSystem.SystemData update = this.m_Updates[updateIndex] with
          {
            m_Offset = systemData.m_Offset
          };
          this.m_Updates[updateIndex] = update;
        }
        ++updateIndex;
      }
    }

    [Preserve]
    public UpdateSystem()
    {
    }

    private struct SystemData : IComparable<UpdateSystem.SystemData>
    {
      public SystemUpdatePhase m_Phase;
      public int m_Interval;
      public int m_Offset;
      public int m_AddIndex;
      public int m_ResetInterval;
      public ComponentSystemBase m_System;

      public SystemData(
        SystemUpdatePhase phase,
        int interval,
        int offset,
        int addIndex,
        ComponentSystemBase system)
      {
        this.m_Phase = phase;
        this.m_Interval = interval;
        this.m_Offset = offset;
        this.m_AddIndex = addIndex;
        this.m_System = system;
        this.m_ResetInterval = system is GameSystemBase ? interval : int.MaxValue;
      }

      public int CompareTo(UpdateSystem.SystemData other)
      {
        int num = this.m_Phase - other.m_Phase;
        if (num == 0)
          num = this.m_AddIndex - other.m_AddIndex;
        return num;
      }
    }

    private struct IntervalData : IComparable<UpdateSystem.IntervalData>
    {
      public int m_Interval;
      public int m_UpdateStart;
      public int m_UpdateIndex;

      public IntervalData(int interval, int updateStart, int updateIndex)
      {
        this.m_Interval = interval;
        this.m_UpdateStart = updateStart;
        this.m_UpdateIndex = updateIndex;
      }

      public int CompareTo(UpdateSystem.IntervalData other)
      {
        int num = this.m_Interval - other.m_Interval;
        if (num == 0)
          num = this.m_UpdateIndex - other.m_UpdateIndex;
        return num;
      }
    }
  }
}
