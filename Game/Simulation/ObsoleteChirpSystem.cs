// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ObsoleteChirpSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ObsoleteChirpSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_ChirpQuery;
    private EntityQuery m_LimitSettingQuery;
    private ObsoleteChirpSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 65536;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Triggers.Chirp>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Game.Triggers.LifePathEvent>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_LimitSettingQuery = this.GetEntityQuery(ComponentType.ReadOnly<LimitSettingData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ChirpQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LimitSettingQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ChirpQuery.CalculateEntityCount() <= this.m_LimitSettingQuery.GetSingleton<LimitSettingData>().m_MaxChirpsLimit)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_Chirp_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ObsoleteChirpSystem.ObsoleteChirpJob jobData = new ObsoleteChirpSystem.ObsoleteChirpJob()
      {
        m_Entities = this.m_ChirpQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_Chirps = this.__TypeHandle.__Game_Triggers_Chirp_RW_ComponentLookup,
        m_LimitSettingData = this.m_LimitSettingQuery.GetSingleton<LimitSettingData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      this.Dependency = jobData.Schedule<ObsoleteChirpSystem.ObsoleteChirpJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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

    [UnityEngine.Scripting.Preserve]
    public ObsoleteChirpSystem()
    {
    }

    [BurstCompile]
    private struct ObsoleteChirpJob : IJob
    {
      [DeallocateOnJobCompletion]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public ComponentLookup<Game.Triggers.Chirp> m_Chirps;
      [ReadOnly]
      public LimitSettingData m_LimitSettingData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_Entities.Sort<Entity, ObsoleteChirpSystem.ChirpComparer>(new ObsoleteChirpSystem.ChirpComparer()
        {
          m_Chirps = this.m_Chirps
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Entities.Length - this.m_LimitSettingData.m_MaxChirpsLimit; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(this.m_Entities[index]);
        }
      }
    }

    private struct ChirpComparer : IComparer<Entity>
    {
      [ReadOnly]
      public ComponentLookup<Game.Triggers.Chirp> m_Chirps;

      public int Compare(Entity x, Entity y)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Chirps[x].m_CreationFrame.CompareTo(this.m_Chirps[y].m_CreationFrame);
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<Game.Triggers.Chirp> __Game_Triggers_Chirp_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_Chirp_RW_ComponentLookup = state.GetComponentLookup<Game.Triggers.Chirp>();
      }
    }
  }
}
