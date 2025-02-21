// Decompiled with JetBrains decompiler
// Type: Game.Triggers.EarlyGameOutsideConnectionTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Triggers
{
  [CompilerGenerated]
  public class EarlyGameOutsideConnectionTriggerSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private static readonly float kDelaySeconds = 10f;
    private EntityQuery m_BuildingQuery;
    private TriggerSystem m_TriggerSystem;
    private SimulationSystem m_SimulationSystem;
    private ResourceAvailabilitySystem m_ResourceAvailabilitySystem;
    private bool m_Started;
    private double m_StartTime;
    private bool m_Triggered;
    private EarlyGameOutsideConnectionTriggerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<BuildingCondition>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceAvailabilitySystem = this.World.GetOrCreateSystemManaged<ResourceAvailabilitySystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BuildingQuery.IsEmptyIgnoreFilter && !this.m_Triggered)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Started)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_StartTime = (double) this.m_SimulationSystem.frameIndex;
          // ISSUE: reference to a compiler-generated field
          this.m_Started = true;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ResourceAvailabilitySystem.appliedResource == AvailableResource.OutsideConnection && (double) this.m_SimulationSystem.frameIndex - this.m_StartTime > (double) EarlyGameOutsideConnectionTriggerSystem.kDelaySeconds * 60.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          EarlyGameOutsideConnectionTriggerSystem.TriggerJob jobData = new EarlyGameOutsideConnectionTriggerSystem.TriggerJob()
          {
            m_Buildings = this.m_BuildingQuery.ToComponentDataArray<Building>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
            m_AvailabilityDatas = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup,
            m_ActionBuffer = this.m_TriggerSystem.CreateActionBuffer()
          };
          this.Dependency = jobData.Schedule<EarlyGameOutsideConnectionTriggerSystem.TriggerJob>(this.Dependency);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
          // ISSUE: reference to a compiler-generated field
          this.m_Triggered = true;
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BuildingQuery.IsEmptyIgnoreFilter || !this.m_Started || this.m_Triggered)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Started = false;
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      if (serializationContext.purpose == Purpose.NewGame)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Started = false;
        // ISSUE: reference to a compiler-generated field
        this.m_StartTime = 0.0;
        // ISSUE: reference to a compiler-generated field
        this.m_Triggered = false;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Triggered = true;
      }
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
    public EarlyGameOutsideConnectionTriggerSystem()
    {
    }

    [BurstCompile]
    private struct TriggerJob : IJob
    {
      [ReadOnly]
      [DeallocateOnJobCompletion]
      public NativeArray<Building> m_Buildings;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_AvailabilityDatas;
      public NativeQueue<TriggerAction> m_ActionBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Buildings.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Building building = this.m_Buildings[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (building.m_RoadEdge != Entity.Null && this.m_AvailabilityDatas.HasBuffer(building.m_RoadEdge) && (double) NetUtils.GetAvailability(this.m_AvailabilityDatas[building.m_RoadEdge], AvailableResource.OutsideConnection, building.m_CurvePosition) <= 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ActionBuffer.Enqueue(new TriggerAction(TriggerType.NoOutsideConnection, Entity.Null, 0.0f));
            break;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
      }
    }
  }
}
