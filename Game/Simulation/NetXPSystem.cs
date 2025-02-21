// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NetXPSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class NetXPSystem : GameSystemBase
  {
    private static readonly float kXPRewardLength = 112f;
    private XPSystem m_XPSystem;
    private EntityQuery m_CreatedNetQuery;
    private EntityQuery m_DeletedNetQuery;
    private NetXPSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedNetQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Curve>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedNetQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Curve>(), ComponentType.ReadOnly<Deleted>(), ComponentType.Exclude<Created>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_XPSystem = this.World.GetOrCreateSystemManaged<XPSystem>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedNetQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<Entity> entityListAsync1 = this.m_CreatedNetQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<Entity> entityListAsync2 = this.m_DeletedNetQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PowerLineData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PipelineData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterwayData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetXPSystem.NetXPJob jobData = new NetXPSystem.NetXPJob()
      {
        m_PlaceableNetDatas = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RW_ComponentLookup,
        m_Elevations = this.__TypeHandle.__Game_Net_Elevation_RW_ComponentLookup,
        m_RoadDatas = this.__TypeHandle.__Game_Prefabs_RoadData_RW_ComponentLookup,
        m_TrackDatas = this.__TypeHandle.__Game_Prefabs_TrackData_RW_ComponentLookup,
        m_WaterwayDatas = this.__TypeHandle.__Game_Prefabs_WaterwayData_RW_ComponentLookup,
        m_PipelineDatas = this.__TypeHandle.__Game_Prefabs_PipelineData_RW_ComponentLookup,
        m_PowerLineDatas = this.__TypeHandle.__Game_Prefabs_PowerLineData_RW_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_Edge_RW_ComponentLookup,
        m_Curves = this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup,
        m_CreatedEntities = entityListAsync1.AsDeferredJobArray(),
        m_DeletedEntities = entityListAsync2.AsDeferredJobArray(),
        m_XPQueue = this.m_XPSystem.GetQueue(out deps)
      };
      this.Dependency = jobData.Schedule<NetXPSystem.NetXPJob>(JobHandle.CombineDependencies(outJobHandle1, outJobHandle2, JobHandle.CombineDependencies(deps, this.Dependency)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_XPSystem.AddQueueWriter(this.Dependency);
      entityListAsync1.Dispose(this.Dependency);
      entityListAsync2.Dispose(this.Dependency);
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
    public NetXPSystem()
    {
    }

    [BurstCompile]
    private struct NetXPJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> m_PlaceableNetDatas;
      [ReadOnly]
      public ComponentLookup<Elevation> m_Elevations;
      [ReadOnly]
      public ComponentLookup<RoadData> m_RoadDatas;
      [ReadOnly]
      public ComponentLookup<TrackData> m_TrackDatas;
      [ReadOnly]
      public ComponentLookup<WaterwayData> m_WaterwayDatas;
      [ReadOnly]
      public ComponentLookup<PipelineData> m_PipelineDatas;
      [ReadOnly]
      public ComponentLookup<PowerLineData> m_PowerLineDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_Edges;
      [ReadOnly]
      public ComponentLookup<Curve> m_Curves;
      [ReadOnly]
      public NativeArray<Entity> m_CreatedEntities;
      [ReadOnly]
      public NativeArray<Entity> m_DeletedEntities;
      public NativeQueue<XPGain> m_XPQueue;

      private float GetElevationBonus(
        Game.Net.Edge edge,
        ComponentLookup<Elevation> elevations,
        bool isRoad)
      {
        Elevation componentData;
        return !isRoad || (!elevations.TryGetComponent(edge.m_Start, out componentData) || (double) componentData.m_Elevation.x <= 0.0 || (double) componentData.m_Elevation.y <= 0.0) && (!elevations.TryGetComponent(edge.m_End, out componentData) || (double) componentData.m_Elevation.x <= 0.0 || (double) componentData.m_Elevation.y <= 0.0) ? 0.0f : 1f;
      }

      private NetXPSystem.NetXPs CountXP(ref NativeArray<Entity> entities)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        NetXPSystem.NetXPs netXps = new NetXPSystem.NetXPs();
        for (int index = 0; index < entities.Length; ++index)
        {
          Entity entity = entities[index];
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_PrefabRefs[entity].m_Prefab;
          PlaceableNetData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceableNetDatas.TryGetComponent(prefab, out componentData) && componentData.m_XPReward > 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num = ((float) componentData.m_XPReward + this.GetElevationBonus(this.m_Edges[entity], this.m_Elevations, this.m_RoadDatas.HasComponent(prefab))) * this.m_Curves[entity].m_Length / NetXPSystem.kXPRewardLength;
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoadDatas.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              netXps.m_Roads += num;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TrackDatas.HasComponent(prefab))
              {
                // ISSUE: reference to a compiler-generated field
                TrackData trackData = this.m_TrackDatas[prefab];
                if (trackData.m_TrackType == TrackTypes.Train)
                {
                  // ISSUE: reference to a compiler-generated field
                  netXps.m_Trains += num;
                }
                else if (trackData.m_TrackType == TrackTypes.Tram)
                {
                  // ISSUE: reference to a compiler-generated field
                  netXps.m_Trams += num;
                }
                else if (trackData.m_TrackType == TrackTypes.Subway)
                {
                  // ISSUE: reference to a compiler-generated field
                  netXps.m_Subways += num;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_WaterwayDatas.HasComponent(prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  netXps.m_Waterways += num;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PipelineDatas.HasComponent(prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    netXps.m_Pipes += num;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PowerLineDatas.HasComponent(prefab))
                    {
                      // ISSUE: reference to a compiler-generated field
                      netXps.m_Powerlines += num;
                    }
                  }
                }
              }
            }
          }
        }
        return netXps;
      }

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        NetXPSystem.NetXPs netXps = this.CountXP(ref this.m_CreatedEntities);
        // ISSUE: reference to a compiler-generated field
        if (this.m_DeletedEntities.Length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          netXps -= this.CountXP(ref this.m_DeletedEntities);
        }
        float max;
        XPReason reason;
        // ISSUE: reference to a compiler-generated method
        netXps.GetMaxValue(out max, out reason);
        int num = Mathf.FloorToInt(max);
        if (num <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_XPQueue.Enqueue(new XPGain()
        {
          amount = num,
          entity = Entity.Null,
          reason = reason
        });
      }
    }

    private struct NetXPs
    {
      public float m_Roads;
      public float m_Trains;
      public float m_Trams;
      public float m_Subways;
      public float m_Waterways;
      public float m_Pipes;
      public float m_Powerlines;

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Roads = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Trains = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Trams = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Subways = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Waterways = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Pipes = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Powerlines = 0.0f;
      }

      public static NetXPSystem.NetXPs operator +(NetXPSystem.NetXPs a, NetXPSystem.NetXPs b)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new NetXPSystem.NetXPs()
        {
          m_Roads = a.m_Roads + b.m_Roads,
          m_Trains = a.m_Trains + b.m_Trains,
          m_Trams = a.m_Trams + b.m_Trams,
          m_Subways = a.m_Subways + b.m_Subways,
          m_Waterways = a.m_Waterways + b.m_Waterways,
          m_Pipes = a.m_Pipes + b.m_Pipes,
          m_Powerlines = a.m_Powerlines + b.m_Powerlines
        };
      }

      public static NetXPSystem.NetXPs operator -(NetXPSystem.NetXPs a, NetXPSystem.NetXPs b)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new NetXPSystem.NetXPs()
        {
          m_Roads = a.m_Roads - b.m_Roads,
          m_Trains = a.m_Trains - b.m_Trains,
          m_Trams = a.m_Trams - b.m_Trams,
          m_Subways = a.m_Subways - b.m_Subways,
          m_Waterways = a.m_Waterways - b.m_Waterways,
          m_Pipes = a.m_Pipes - b.m_Pipes,
          m_Powerlines = a.m_Powerlines - b.m_Powerlines
        };
      }

      public void GetMaxValue(out float max, out XPReason reason)
      {
        reason = XPReason.Unknown;
        max = 0.0f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Check(this.m_Roads, XPReason.Road, ref max, ref reason);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Check(this.m_Trains, XPReason.TrainTrack, ref max, ref reason);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Check(this.m_Trams, XPReason.TramTrack, ref max, ref reason);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Check(this.m_Subways, XPReason.SubwayTrack, ref max, ref reason);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Check(this.m_Waterways, XPReason.Waterway, ref max, ref reason);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Check(this.m_Pipes, XPReason.Pipe, ref max, ref reason);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Check(this.m_Powerlines, XPReason.PowerLine, ref max, ref reason);
      }

      private void Check(float value, XPReason reason, ref float max, ref XPReason maxReason)
      {
        if ((double) value <= (double) max)
          return;
        max = value;
        maxReason = reason;
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<PlaceableNetData> __Game_Prefabs_PlaceableNetData_RW_ComponentLookup;
      public ComponentLookup<Elevation> __Game_Net_Elevation_RW_ComponentLookup;
      public ComponentLookup<RoadData> __Game_Prefabs_RoadData_RW_ComponentLookup;
      public ComponentLookup<TrackData> __Game_Prefabs_TrackData_RW_ComponentLookup;
      public ComponentLookup<WaterwayData> __Game_Prefabs_WaterwayData_RW_ComponentLookup;
      public ComponentLookup<PipelineData> __Game_Prefabs_PipelineData_RW_ComponentLookup;
      public ComponentLookup<PowerLineData> __Game_Prefabs_PowerLineData_RW_ComponentLookup;
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RW_ComponentLookup;
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RW_ComponentLookup;
      public ComponentLookup<Curve> __Game_Net_Curve_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetData_RW_ComponentLookup = state.GetComponentLookup<PlaceableNetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RW_ComponentLookup = state.GetComponentLookup<Elevation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RW_ComponentLookup = state.GetComponentLookup<RoadData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackData_RW_ComponentLookup = state.GetComponentLookup<TrackData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterwayData_RW_ComponentLookup = state.GetComponentLookup<WaterwayData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PipelineData_RW_ComponentLookup = state.GetComponentLookup<PipelineData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PowerLineData_RW_ComponentLookup = state.GetComponentLookup<PowerLineData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RW_ComponentLookup = state.GetComponentLookup<PrefabRef>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RW_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RW_ComponentLookup = state.GetComponentLookup<Curve>();
      }
    }
  }
}
