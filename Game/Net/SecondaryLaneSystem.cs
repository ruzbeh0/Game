// Decompiled with JetBrains decompiler
// Type: Game.Net.SecondaryLaneSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class SecondaryLaneSystem : GameSystemBase
  {
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ModificationBarrier4B m_ModificationBarrier;
    private EntityQuery m_OwnerQuery;
    private ComponentTypeSet m_AppliedTypes;
    private ComponentTypeSet m_DeletedTempTypes;
    private SecondaryLaneSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_OwnerQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<SubLane>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<OutsideConnection>(),
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Area>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedTempTypes = new ComponentTypeSet(ComponentType.ReadWrite<Deleted>(), ComponentType.ReadWrite<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_OwnerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SecondaryNetLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SecondaryLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_HangingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new SecondaryLaneSystem.UpdateLanesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_NodeLaneData = this.__TypeHandle.__Game_Net_NodeLane_RO_ComponentLookup,
        m_HangingLaneData = this.__TypeHandle.__Game_Net_HangingLane_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabLaneArchetypeData = this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup,
        m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabSecondaryLaneData = this.__TypeHandle.__Game_Prefabs_SecondaryLaneData_RO_ComponentLookup,
        m_PrefabLaneGeometryData = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_PrefabSecondaryLanes = this.__TypeHandle.__Game_Prefabs_SecondaryNetLane_RO_BufferLookup,
        m_LaneRequirements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup,
        m_DefaultTheme = this.m_CityConfigurationSystem.defaultTheme,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_AppliedTypes = this.m_AppliedTypes,
        m_DeletedTempTypes = this.m_DeletedTempTypes,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<SecondaryLaneSystem.UpdateLanesJob>(this.m_OwnerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public SecondaryLaneSystem()
    {
    }

    private struct LaneKey : IEquatable<SecondaryLaneSystem.LaneKey>
    {
      private Lane m_Lane;
      private Entity m_Prefab;

      public LaneKey(Lane lane, Entity prefab)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Lane = lane;
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = prefab;
      }

      public void ReplaceOwner(Entity oldOwner, Entity newOwner)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Lane.m_StartNode.ReplaceOwner(oldOwner, newOwner);
        // ISSUE: reference to a compiler-generated field
        this.m_Lane.m_MiddleNode.ReplaceOwner(oldOwner, newOwner);
        // ISSUE: reference to a compiler-generated field
        this.m_Lane.m_EndNode.ReplaceOwner(oldOwner, newOwner);
      }

      public bool Equals(SecondaryLaneSystem.LaneKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Lane.Equals(other.m_Lane) && this.m_Prefab.Equals(other.m_Prefab);
      }

      public override int GetHashCode() => this.m_Lane.GetHashCode();
    }

    private struct LaneBuffer
    {
      public NativeParallelHashMap<SecondaryLaneSystem.LaneKey, Entity> m_OldLanes;
      public NativeParallelHashMap<SecondaryLaneSystem.LaneKey, Entity> m_OriginalLanes;
      public NativeParallelHashSet<Entity> m_Requirements;
      public NativeList<SecondaryLaneSystem.LaneCorner> m_LaneCorners;
      public NativeList<SecondaryLaneSystem.CutRange> m_CutRanges;
      public NativeList<SecondaryLaneSystem.CrossingLane> m_CrossingLanes;
      public bool m_RequirementsSearched;

      public LaneBuffer(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OldLanes = new NativeParallelHashMap<SecondaryLaneSystem.LaneKey, Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalLanes = new NativeParallelHashMap<SecondaryLaneSystem.LaneKey, Entity>(32, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_Requirements = new NativeParallelHashSet<Entity>();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneCorners = new NativeList<SecondaryLaneSystem.LaneCorner>(32, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_CutRanges = new NativeList<SecondaryLaneSystem.CutRange>(32, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_CrossingLanes = new NativeList<SecondaryLaneSystem.CrossingLane>(32, (AllocatorManager.AllocatorHandle) allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_RequirementsSearched = false;
      }

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OldLanes.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalLanes.Clear();
        // ISSUE: reference to a compiler-generated field
        if (this.m_Requirements.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Requirements.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LaneCorners.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_CutRanges.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_CrossingLanes.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_RequirementsSearched = false;
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OldLanes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalLanes.Dispose();
        // ISSUE: reference to a compiler-generated field
        if (this.m_Requirements.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Requirements.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LaneCorners.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_CutRanges.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_CrossingLanes.Dispose();
      }
    }

    private struct LaneCorner
    {
      public float3 m_StartPosition;
      public float3 m_EndPosition;
      public float4 m_Tangents;
      public float2 m_Width;
      public Entity m_Lane;
      public PathNode m_StartNode;
      public PathNode m_EndNode;
      public LaneFlags m_Flags;
      public bool m_Inverted;
      public bool m_Duplicates;
    }

    private struct CutRange : IComparable<SecondaryLaneSystem.CutRange>
    {
      public Bounds1 m_Bounds;
      public uint m_Group;

      public int CompareTo(SecondaryLaneSystem.CutRange other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(0, math.select(-1, 1, (double) this.m_Bounds.min > (double) other.m_Bounds.min), (double) this.m_Bounds.min != (double) other.m_Bounds.min);
      }
    }

    private struct CrossingLane
    {
      public Entity m_Prefab;
      public float3 m_StartPos;
      public float2 m_StartTangent;
      public float3 m_EndPos;
      public float2 m_EndTangent;
      public bool m_Optional;
    }

    [BurstCompile]
    private struct UpdateLanesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLane> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<NodeLane> m_NodeLaneData;
      [ReadOnly]
      public ComponentLookup<HangingLane> m_HangingLaneData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> m_PrefabLaneArchetypeData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public ComponentLookup<SecondaryLaneData> m_PrefabSecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> m_PrefabLaneGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<SecondaryNetLane> m_PrefabSecondaryLanes;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_LaneRequirements;
      [ReadOnly]
      public Entity m_DefaultTheme;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      [ReadOnly]
      public ComponentTypeSet m_DeletedTempTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          // ISSUE: reference to a compiler-generated method
          this.DeleteLanes(chunk, unfilteredChunkIndex);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateLanes(chunk, unfilteredChunkIndex);
        }
      }

      private void DeleteLanes(ArchetypeChunk chunk, int chunkIndex)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<SubLane> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity subLane = dynamicBuffer[index2].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SecondaryLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, subLane, new Deleted());
            }
          }
        }
      }

      private void UpdateLanes(ArchetypeChunk chunk, int chunkIndex)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SecondaryLaneSystem.LaneBuffer laneBuffer = new SecondaryLaneSystem.LaneBuffer(Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeGeometry> nativeArray2 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Composition> nativeArray3 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        // ISSUE: reference to a compiler-generated field
        bool isNode = chunk.Has<Node>(ref this.m_NodeType);
        bool isTemp = nativeArray4.Length != 0;
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity owner = nativeArray1[index1];
          DynamicBuffer<SubLane> lanes = bufferAccessor[index1];
          int laneIndex = 0;
          Temp ownerTemp = new Temp();
          if (isTemp)
          {
            ownerTemp = nativeArray4[index1];
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubLanes.HasBuffer(ownerTemp.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.FillOldLaneBuffer(this.m_SubLanes[ownerTemp.m_Original], laneBuffer.m_OriginalLanes);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FillOldLaneBuffer(lanes, laneBuffer.m_OldLanes);
          EdgeGeometry edgeGeometry1 = new EdgeGeometry();
          Line3 line3_1 = new Line3();
          Line3 line3_2 = new Line3();
          float2 forward1 = new float2();
          float2 forward2 = new float2();
          if (nativeArray2.Length != 0)
          {
            EdgeGeometry edgeGeometry2 = nativeArray2[index1];
            line3_1 = new Line3(edgeGeometry2.m_Start.m_Right.a, edgeGeometry2.m_Start.m_Left.a);
            line3_2 = new Line3(edgeGeometry2.m_End.m_Left.d, edgeGeometry2.m_End.m_Right.d);
            forward1 = MathUtils.Right(math.normalizesafe(line3_1.b.xz - line3_1.a.xz));
            forward2 = MathUtils.Right(math.normalizesafe(line3_2.b.xz - line3_2.a.xz));
          }
          NetCompositionData netCompositionData1 = new NetCompositionData();
          NetCompositionData netCompositionData2 = new NetCompositionData();
          if (nativeArray3.Length != 0)
          {
            Composition composition = nativeArray3[index1];
            // ISSUE: reference to a compiler-generated field
            netCompositionData1 = this.m_PrefabCompositionData[composition.m_StartNode];
            // ISSUE: reference to a compiler-generated field
            netCompositionData2 = this.m_PrefabCompositionData[composition.m_EndNode];
          }
          float num1;
          for (int index2 = 0; index2 < lanes.Length; ++index2)
          {
            Entity subLane1 = lanes[index2].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_MasterLaneData.HasComponent(subLane1) && !this.m_SecondaryLaneData.HasComponent(subLane1))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve1 = this.m_CurveData[subLane1];
              // ISSUE: reference to a compiler-generated field
              Lane lane = this.m_LaneData[subLane1];
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef1 = this.m_PrefabRefData[subLane1];
              // ISSUE: reference to a compiler-generated field
              NetLaneData netLaneData = this.m_PrefabLaneData[prefabRef1.m_Prefab];
              DynamicBuffer<SecondaryNetLane> bufferData;
              // ISSUE: reference to a compiler-generated field
              if ((netLaneData.m_Flags & LaneFlags.Secondary) == (LaneFlags) 0 && this.m_PrefabSecondaryLanes.TryGetBuffer(prefabRef1.m_Prefab, out bufferData) && bufferData.Length != 0)
              {
                float2 float2_1 = math.normalizesafe(MathUtils.StartTangent(curve1.m_Bezier).xz);
                float2 float2_2 = math.normalizesafe(MathUtils.EndTangent(curve1.m_Bezier).xz);
                float3 a1 = curve1.m_Bezier.a;
                float3 d1 = curve1.m_Bezier.d;
                float3 d2 = curve1.m_Bezier.d;
                float3 a2 = curve1.m_Bezier.a;
                float2 width = (float2) netLaneData.m_Width;
                NodeLane componentData1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_NodeLaneData.TryGetComponent(subLane1, out componentData1))
                  width += componentData1.m_WidthOffset;
                a1.xz += MathUtils.Right(float2_1) * (width.x * 0.5f);
                d1.xz += MathUtils.Left(float2_2) * (width.x * 0.5f);
                d2.xz += MathUtils.Right(float2_2) * (width.y * 0.5f);
                a2.xz += MathUtils.Left(float2_1) * (width.y * 0.5f);
                bool flag = false;
                for (int index3 = 0; index3 < bufferData.Length; ++index3)
                {
                  SecondaryNetLane secondaryNetLane = bufferData[index3];
                  flag |= (secondaryNetLane.m_Flags & SecondaryNetLaneFlags.DuplicateSides) != 0;
                }
                // ISSUE: reference to a compiler-generated field
                ref NativeList<SecondaryLaneSystem.LaneCorner> local1 = ref laneBuffer.m_LaneCorners;
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                SecondaryLaneSystem.LaneCorner laneCorner = new SecondaryLaneSystem.LaneCorner();
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_StartPosition = a1;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_EndPosition = d2;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Tangents = new float4(float2_1, float2_2);
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Lane = subLane1;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_StartNode = lane.m_StartNode;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_EndNode = lane.m_EndNode;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Width = width;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Inverted = false;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Duplicates = flag;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Flags = netLaneData.m_Flags;
                ref SecondaryLaneSystem.LaneCorner local2 = ref laneCorner;
                local1.Add(in local2);
                // ISSUE: reference to a compiler-generated field
                ref NativeList<SecondaryLaneSystem.LaneCorner> local3 = ref laneBuffer.m_LaneCorners;
                // ISSUE: object of a compiler-generated type is created
                laneCorner = new SecondaryLaneSystem.LaneCorner();
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_StartPosition = d1;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_EndPosition = a2;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Tangents = new float4(float2_2, float2_1);
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Lane = subLane1;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_StartNode = lane.m_EndNode;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_EndNode = lane.m_StartNode;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Width = width;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Inverted = true;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Duplicates = flag;
                // ISSUE: reference to a compiler-generated field
                laneCorner.m_Flags = netLaneData.m_Flags;
                ref SecondaryLaneSystem.LaneCorner local4 = ref laneCorner;
                local3.Add(in local4);
                EdgeLane componentData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_EdgeLaneData.TryGetComponent(subLane1, out componentData2))
                {
                  bool4 x1 = componentData2.m_EdgeDelta.xxyy == new float4(0.0f, 1f, 0.0f, 1f);
                  if (math.any(x1))
                  {
                    CarLaneFlags carLaneFlags = ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_CarLaneData.HasComponent(subLane1))
                    {
                      // ISSUE: reference to a compiler-generated field
                      carLaneFlags = this.m_CarLaneData[subLane1].m_Flags;
                    }
                    for (int index4 = 0; index4 < bufferData.Length; ++index4)
                    {
                      SecondaryNetLane secondaryNetLane = bufferData[index4];
                      if ((secondaryNetLane.m_Flags & SecondaryNetLaneFlags.Crossing) != (SecondaryNetLaneFlags) 0)
                      {
                        bool isOptional = false;
                        bool2 x2 = new bool2(math.any(x1.xy), math.any(x1.zw));
                        if ((secondaryNetLane.m_Flags & SecondaryNetLaneFlags.RequireStop) != (SecondaryNetLaneFlags) 0)
                        {
                          isOptional |= (carLaneFlags & (CarLaneFlags.LevelCrossing | CarLaneFlags.Stop | CarLaneFlags.TrafficLights)) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                          x2.x = false;
                        }
                        if ((secondaryNetLane.m_Flags & SecondaryNetLaneFlags.RequireYield) != (SecondaryNetLaneFlags) 0)
                        {
                          isOptional |= (carLaneFlags & CarLaneFlags.Yield) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter);
                          x2.x = false;
                        }
                        if ((secondaryNetLane.m_Flags & SecondaryNetLaneFlags.RequirePavement) != (SecondaryNetLaneFlags) 0)
                          x2 &= x1.xz & (netCompositionData1.m_Flags.m_General & CompositionFlags.General.Pavement) > (CompositionFlags.General) 0 | x1.yw & (netCompositionData2.m_Flags.m_General & CompositionFlags.General.Pavement) > (CompositionFlags.General) 0;
                        // ISSUE: reference to a compiler-generated method
                        if (math.any(x2) && this.CheckRequirements(ref laneBuffer, secondaryNetLane.m_Lane))
                        {
                          float3 float3_1 = (float3) 0.0f;
                          SecondaryLaneData componentData3;
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_PrefabSecondaryLaneData.TryGetComponent(secondaryNetLane.m_Lane, out componentData3))
                          {
                            float3_1 = componentData3.m_PositionOffset;
                            if ((componentData3.m_Flags & SecondaryLaneDataFlags.FitToParkingSpaces) != (SecondaryLaneDataFlags) 0)
                            {
                              ParkingLane componentData4;
                              // ISSUE: reference to a compiler-generated field
                              this.m_ParkingLaneData.TryGetComponent(subLane1, out componentData4);
                              Bounds1 curveBounds;
                              ulong blockedMask1;
                              int slotCount;
                              float slotAngle;
                              bool2 skipStartEnd;
                              // ISSUE: reference to a compiler-generated method
                              this.FitToParkingLane(subLane1, curve1, prefabRef1, (float2) 0.0f, out curveBounds, out blockedMask1, out slotCount, out slotAngle, out skipStartEnd);
                              curveBounds = new Bounds1(math.max(0.0f, curveBounds.min), math.min(1f, curveBounds.max));
                              if ((secondaryNetLane.m_Flags & SecondaryNetLaneFlags.RequireContinue) != (SecondaryNetLaneFlags) 0)
                              {
                                bool2 bool2 = new bool2((componentData4.m_Flags & ParkingLaneFlags.StartingLane) != 0, (componentData4.m_Flags & ParkingLaneFlags.EndingLane) != 0);
                                float2 float2_3 = new float2(curveBounds.min - 0.01f, 0.99f - curveBounds.max) * curve1.m_Length;
                                if ((double) math.abs(slotAngle) > 0.25)
                                  float2_3 -= netLaneData.m_Width * 0.5f / math.tan(slotAngle);
                                skipStartEnd |= bool2 & float2_3 < 0.2f;
                              }
                              float num2 = 1f / (float) math.max(1, slotCount);
                              int2 int2 = math.select(new int2(0, slotCount), new int2(1, slotCount - 1), skipStartEnd);
                              float newLength = (double) math.abs(slotAngle) > 0.25 ? netLaneData.m_Width * 0.5f / math.cos(1.57079637f - math.abs(slotAngle)) : netLaneData.m_Width * 0.5f;
                              for (int x3 = int2.x; x3 <= int2.y; ++x3)
                              {
                                if (x3 == 0)
                                {
                                  if (((int) (uint) blockedMask1 & 1) == 1)
                                    continue;
                                }
                                else if (x3 == slotCount)
                                {
                                  if (((int) (uint) (blockedMask1 >> x3 - 1) & 1) == 1)
                                  {
                                    if ((componentData4.m_Flags & ParkingLaneFlags.EndingLane) == (ParkingLaneFlags) 0)
                                    {
                                      ulong blockedMask2 = 0;
                                      for (int index5 = 0; index5 < lanes.Length; ++index5)
                                      {
                                        SubLane subLane2 = lanes[index5];
                                        if ((subLane2.m_PathMethods & PathMethod.Parking) != (PathMethod) 0)
                                        {
                                          // ISSUE: reference to a compiler-generated field
                                          Curve curve2 = this.m_CurveData[subLane2.m_SubLane];
                                          if ((double) math.distancesq(curve2.m_Bezier.a, curve1.m_Bezier.d) <= 9.9999997473787516E-05)
                                          {
                                            // ISSUE: reference to a compiler-generated field
                                            PrefabRef prefabRef2 = this.m_PrefabRefData[subLane2.m_SubLane];
                                            // ISSUE: reference to a compiler-generated method
                                            this.FitToParkingLane(subLane2.m_SubLane, curve2, prefabRef2, (float2) 0.0f, out Bounds1 _, out blockedMask2, out int _, out num1, out bool2 _);
                                            break;
                                          }
                                        }
                                      }
                                      if (((int) (uint) blockedMask2 & 1) == 1)
                                        continue;
                                    }
                                    else
                                      continue;
                                  }
                                }
                                else if (((int) (uint) (blockedMask1 >> x3 - 1) & 3) == 3)
                                  continue;
                                float t = math.lerp(curveBounds.min, curveBounds.max, (float) x3 * num2);
                                float2 xz = MathUtils.Tangent(curve1.m_Bezier, t).xz;
                                float2 float2_4 = (double) math.abs(slotAngle) > 0.25 ? MathUtils.RotateRight(xz, slotAngle) : ((double) slotAngle < 0.0 ? MathUtils.Left(xz) : MathUtils.Right(xz));
                                if (MathUtils.TryNormalize(ref float2_4, newLength))
                                {
                                  float3 float3_2 = MathUtils.Position(curve1.m_Bezier, t);
                                  // ISSUE: reference to a compiler-generated method
                                  this.AddCrossingLane(laneBuffer, secondaryNetLane.m_Lane, float3_2 - new float3(float2_4.x, 0.0f, float2_4.y), float3_2 + new float3(float2_4.x, 0.0f, float2_4.y), math.normalizesafe(xz), isOptional);
                                }
                              }
                              continue;
                            }
                          }
                          Line3 line3_3 = line3_1;
                          Line3 line3_4 = line3_2;
                          line3_3.xz += forward1 * float3_1.z + MathUtils.Right(forward1) * float3_1.x;
                          line3_4.xz += forward2 * float3_1.z + MathUtils.Right(forward2) * float3_1.x;
                          line3_3.y += float3_1.y;
                          line3_4.y += float3_1.y;
                          if (x2.x)
                          {
                            Line2 line2_1 = new Line2(a2.xz, a2.xz + float2_1);
                            Line2 line2_2 = new Line2(a1.xz, a1.xz + float2_1);
                            Line3 line = x1.x ? line3_3 : line3_4;
                            float2 t1;
                            float2 t2;
                            if (MathUtils.Intersect(line.xz, line2_1, out t1) && MathUtils.Intersect(line.xz, line2_2, out t2))
                            {
                              // ISSUE: reference to a compiler-generated method
                              this.AddCrossingLane(laneBuffer, secondaryNetLane.m_Lane, MathUtils.Position(line, t1.x), MathUtils.Position(line, t2.x), float2_1, isOptional);
                            }
                          }
                          if (x2.y)
                          {
                            Line2 line2_3 = new Line2(d2.xz, d2.xz + float2_2);
                            Line2 line2_4 = new Line2(d1.xz, d1.xz + float2_2);
                            Line3 line = x1.z ? line3_3 : line3_4;
                            float2 t3;
                            float2 t4;
                            if (MathUtils.Intersect(line.xz, line2_3, out t3) && MathUtils.Intersect(line.xz, line2_4, out t4))
                            {
                              // ISSUE: reference to a compiler-generated method
                              this.AddCrossingLane(laneBuffer, secondaryNetLane.m_Lane, MathUtils.Position(line, t3.x), MathUtils.Position(line, t4.x), float2_2, isOptional);
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          for (int index6 = 0; index6 < laneBuffer.m_LaneCorners.Length; ++index6)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            SecondaryLaneSystem.LaneCorner laneCorner1 = laneBuffer.m_LaneCorners[index6];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            SecondaryLaneSystem.LaneCorner laneCorner2 = new SecondaryLaneSystem.LaneCorner();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num3 = math.distance(laneCorner1.m_StartPosition.xz, laneCorner1.m_EndPosition.xz) * 0.5f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Line3.Segment line1 = new Line3.Segment(laneCorner1.m_StartPosition, laneCorner1.m_StartPosition);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Line3.Segment line2 = new Line3.Segment(laneCorner1.m_EndPosition, laneCorner1.m_EndPosition);
            // ISSUE: reference to a compiler-generated field
            line1.a.xz -= laneCorner1.m_Tangents.xy * num3;
            // ISSUE: reference to a compiler-generated field
            line1.b.xz += laneCorner1.m_Tangents.xy * num3;
            // ISSUE: reference to a compiler-generated field
            line2.a.xz -= laneCorner1.m_Tangents.zw * num3;
            // ISSUE: reference to a compiler-generated field
            line2.b.xz += laneCorner1.m_Tangents.zw * num3;
            float num4 = float.MaxValue;
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2 float2_5 = math.select(laneCorner1.m_Width, laneCorner1.m_Width.yx, laneCorner1.m_Inverted);
            // ISSUE: reference to a compiler-generated field
            for (int index7 = 0; index7 < laneBuffer.m_LaneCorners.Length; ++index7)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              SecondaryLaneSystem.LaneCorner laneCorner3 = laneBuffer.m_LaneCorners[index7];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (((laneCorner1.m_Flags ^ laneCorner3.m_Flags) & (LaneFlags.Utility | LaneFlags.Underground)) == (LaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                bool flag4 = laneCorner1.m_StartNode.Equals(laneCorner3.m_EndNode);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                bool flag5 = laneCorner1.m_EndNode.Equals(laneCorner3.m_StartNode);
                // ISSUE: reference to a compiler-generated field
                if ((laneCorner1.m_Flags & LaneFlags.Utility) == (LaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float2 float2_6 = math.select(laneCorner3.m_Width.yx, laneCorner3.m_Width, laneCorner3.m_Inverted);
                  float2 float2_7 = (float2_5 + float2_6) * 0.25f;
                  float2 float2_8 = float2_7 * float2_7;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((flag4 ? 0.0 : (double) MathUtils.DistanceSquared(line1, laneCorner3.m_EndPosition, out num1)) > (double) float2_8.x || (flag5 ? 0.0 : (double) MathUtils.DistanceSquared(line2, laneCorner3.m_StartPosition, out num1)) > (double) float2_8.y)
                    continue;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!(laneCorner1.m_Lane == laneCorner3.m_Lane))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int num5 = (double) math.distancesq(laneCorner1.m_Tangents, laneCorner3.m_Tangents.zwxy) < 0.0099999997764825821 ? 1 : 0;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  bool flag6 = (double) math.distancesq(laneCorner1.m_Tangents, -laneCorner3.m_Tangents.zwxy) < 0.0099999997764825821;
                  int num6 = flag6 ? 1 : 0;
                  if ((num5 | num6) != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    float num7 = math.max(math.distancesq(laneCorner1.m_StartPosition, laneCorner3.m_EndPosition), math.distancesq(laneCorner1.m_EndPosition, laneCorner3.m_StartPosition));
                    if ((double) num7 < (double) num4)
                    {
                      num4 = num7;
                      laneCorner2 = laneCorner3;
                      flag1 = flag6;
                      flag2 = flag4;
                      flag3 = flag5;
                    }
                  }
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!(laneCorner2.m_Lane != Entity.Null) || laneCorner1.m_Duplicates || laneCorner1.m_Lane.Index <= laneCorner2.m_Lane.Index)
            {
              SecondaryNetLaneFlags secondaryNetLaneFlags1 = (SecondaryNetLaneFlags) 0;
              SecondaryNetLaneFlags secondaryNetLaneFlags2 = (SecondaryNetLaneFlags) 0;
              SecondaryNetLaneFlags secondaryNetLaneFlags3 = (SecondaryNetLaneFlags) 0;
              SecondaryNetLaneFlags secondaryNetLaneFlags4 = (SecondaryNetLaneFlags) 0;
              CarLane carLane1 = new CarLane();
              CarLane carLane2 = new CarLane();
              PedestrianLane pedestrianLane1 = new PedestrianLane();
              PedestrianLane pedestrianLane2 = new PedestrianLane();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarLaneData.HasComponent(laneCorner1.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                carLane1 = this.m_CarLaneData[laneCorner1.m_Lane];
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_TrackLaneData.HasComponent(laneCorner1.m_Lane))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_TrackLaneData[laneCorner1.m_Lane].m_Flags & TrackLaneFlags.Switch) != (TrackLaneFlags) 0)
                    secondaryNetLaneFlags1 |= SecondaryNetLaneFlags.RequireContinue;
                  else
                    secondaryNetLaneFlags1 |= SecondaryNetLaneFlags.RequireMerge;
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarLaneData.HasComponent(laneCorner2.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                carLane2 = this.m_CarLaneData[laneCorner2.m_Lane];
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_TrackLaneData.HasComponent(laneCorner2.m_Lane))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_TrackLaneData[laneCorner2.m_Lane].m_Flags & TrackLaneFlags.Switch) != (TrackLaneFlags) 0)
                    secondaryNetLaneFlags2 |= SecondaryNetLaneFlags.RequireContinue;
                  else
                    secondaryNetLaneFlags2 |= SecondaryNetLaneFlags.RequireMerge;
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PedestrianLaneData.HasComponent(laneCorner1.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                pedestrianLane1 = this.m_PedestrianLaneData[laneCorner1.m_Lane];
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PedestrianLaneData.HasComponent(laneCorner2.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                pedestrianLane2 = this.m_PedestrianLaneData[laneCorner2.m_Lane];
              }
              SecondaryNetLaneFlags secondaryNetLaneFlags5 = (carLane1.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) || (pedestrianLane1.m_Flags & PedestrianLaneFlags.Unsafe) != (PedestrianLaneFlags) 0 ? secondaryNetLaneFlags1 | SecondaryNetLaneFlags.RequireSafe : secondaryNetLaneFlags1 | SecondaryNetLaneFlags.RequireUnsafe;
              SecondaryNetLaneFlags secondaryNetLaneFlags6 = (carLane2.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) || (pedestrianLane2.m_Flags & PedestrianLaneFlags.Unsafe) != (PedestrianLaneFlags) 0 ? secondaryNetLaneFlags2 | SecondaryNetLaneFlags.RequireSafe : secondaryNetLaneFlags2 | SecondaryNetLaneFlags.RequireUnsafe;
              SecondaryNetLaneFlags secondaryNetLaneFlags7 = (carLane1.m_Flags & CarLaneFlags.ForbidPassing) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) ? secondaryNetLaneFlags5 | SecondaryNetLaneFlags.RequireForbidPassing : secondaryNetLaneFlags5 | SecondaryNetLaneFlags.RequireAllowPassing;
              SecondaryNetLaneFlags secondaryNetLaneFlags8 = (carLane2.m_Flags & CarLaneFlags.ForbidPassing) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) ? secondaryNetLaneFlags6 | SecondaryNetLaneFlags.RequireForbidPassing : secondaryNetLaneFlags6 | SecondaryNetLaneFlags.RequireAllowPassing;
              SecondaryNetLaneFlags secondaryNetLaneFlags9;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_SlaveLaneData.HasComponent(laneCorner1.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                SlaveLane slaveLane = this.m_SlaveLaneData[laneCorner1.m_Lane];
                CarLane componentData;
                // ISSUE: reference to a compiler-generated field
                if (lanes.Length > (int) slaveLane.m_MasterIndex && this.m_CarLaneData.TryGetComponent(lanes[(int) slaveLane.m_MasterIndex].m_SubLane, out componentData) && (componentData.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                  secondaryNetLaneFlags7 |= SecondaryNetLaneFlags.RequireSafeMaster;
                SecondaryNetLaneFlags secondaryNetLaneFlags10 = (slaveLane.m_Flags & SlaveLaneFlags.MultipleLanes) == (SlaveLaneFlags) 0 ? secondaryNetLaneFlags7 | SecondaryNetLaneFlags.RequireMultiple : secondaryNetLaneFlags7 | SecondaryNetLaneFlags.RequireSingle;
                secondaryNetLaneFlags9 = (slaveLane.m_Flags & SlaveLaneFlags.MergingLane) == (SlaveLaneFlags) 0 ? secondaryNetLaneFlags10 | SecondaryNetLaneFlags.RequireMerge : secondaryNetLaneFlags10 | SecondaryNetLaneFlags.RequireContinue;
              }
              else
                secondaryNetLaneFlags9 = secondaryNetLaneFlags7 | SecondaryNetLaneFlags.RequireMultiple | SecondaryNetLaneFlags.RequireMerge;
              SecondaryNetLaneFlags secondaryNetLaneFlags11;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_SlaveLaneData.HasComponent(laneCorner2.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                SlaveLane slaveLane = this.m_SlaveLaneData[laneCorner2.m_Lane];
                CarLane componentData;
                // ISSUE: reference to a compiler-generated field
                if (lanes.Length > (int) slaveLane.m_MasterIndex && this.m_CarLaneData.TryGetComponent(lanes[(int) slaveLane.m_MasterIndex].m_SubLane, out componentData) && (componentData.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                  secondaryNetLaneFlags8 |= SecondaryNetLaneFlags.RequireSafeMaster;
                SecondaryNetLaneFlags secondaryNetLaneFlags12 = (slaveLane.m_Flags & SlaveLaneFlags.MultipleLanes) == (SlaveLaneFlags) 0 ? secondaryNetLaneFlags8 | SecondaryNetLaneFlags.RequireMultiple : secondaryNetLaneFlags8 | SecondaryNetLaneFlags.RequireSingle;
                // ISSUE: reference to a compiler-generated field
                secondaryNetLaneFlags11 = (slaveLane.m_Flags & (laneCorner2.m_Inverted ? SlaveLaneFlags.SplitLeft : SlaveLaneFlags.SplitRight)) == (SlaveLaneFlags) 0 ? ((slaveLane.m_Flags & SlaveLaneFlags.MergingLane) == (SlaveLaneFlags) 0 ? secondaryNetLaneFlags12 | SecondaryNetLaneFlags.RequireMerge : secondaryNetLaneFlags12 | SecondaryNetLaneFlags.RequireContinue) : ((secondaryNetLaneFlags9 & SecondaryNetLaneFlags.RequireContinue) == (SecondaryNetLaneFlags) 0 ? secondaryNetLaneFlags12 | SecondaryNetLaneFlags.RequireContinue : secondaryNetLaneFlags12 | SecondaryNetLaneFlags.RequireMerge);
              }
              else
                secondaryNetLaneFlags11 = secondaryNetLaneFlags8 | SecondaryNetLaneFlags.RequireMultiple | SecondaryNetLaneFlags.RequireMerge;
              bool flag7 = false;
              if (flag2 | flag3)
              {
                int num8 = 0;
                int num9 = 0;
                int num10 = num8 + math.select(0, 1, (secondaryNetLaneFlags9 & SecondaryNetLaneFlags.RequireSafe) != 0) + math.select(0, 2, (secondaryNetLaneFlags9 & SecondaryNetLaneFlags.RequireContinue) != 0);
                int num11 = num9 + math.select(0, 1, (secondaryNetLaneFlags11 & SecondaryNetLaneFlags.RequireSafe) != 0) + math.select(0, 2, (secondaryNetLaneFlags11 & SecondaryNetLaneFlags.RequireContinue) != 0);
                if (num10 != 0 || num11 != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  flag7 = num10 > num11 ^ laneCorner1.m_Inverted;
                }
                else
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<SecondaryNetLane> prefabSecondaryLane = this.m_PrefabSecondaryLanes[this.m_PrefabRefData[laneCorner1.m_Lane].m_Prefab];
              DynamicBuffer<SecondaryNetLane> dynamicBuffer = new DynamicBuffer<SecondaryNetLane>();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              SecondaryNetLaneFlags secondaryNetLaneFlags13 = secondaryNetLaneFlags3 | (laneCorner1.m_Inverted != this.m_LeftHandTraffic ? SecondaryNetLaneFlags.Right : SecondaryNetLaneFlags.Left);
              SecondaryNetLaneFlags secondaryNetLaneFlags14;
              // ISSUE: reference to a compiler-generated field
              if (laneCorner2.m_Lane != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                secondaryNetLaneFlags4 |= laneCorner1.m_Inverted != this.m_LeftHandTraffic ? SecondaryNetLaneFlags.Left : SecondaryNetLaneFlags.Right;
                SecondaryNetLaneFlags secondaryNetLaneFlags15 = secondaryNetLaneFlags9 | SecondaryNetLaneFlags.OneSided;
                SecondaryNetLaneFlags secondaryNetLaneFlags16 = secondaryNetLaneFlags11 | SecondaryNetLaneFlags.OneSided;
                if (flag1)
                {
                  secondaryNetLaneFlags9 = secondaryNetLaneFlags15 | SecondaryNetLaneFlags.RequireParallel;
                  secondaryNetLaneFlags14 = secondaryNetLaneFlags16 | SecondaryNetLaneFlags.RequireParallel;
                }
                else
                {
                  secondaryNetLaneFlags9 = secondaryNetLaneFlags15 | SecondaryNetLaneFlags.RequireOpposite;
                  secondaryNetLaneFlags14 = secondaryNetLaneFlags16 | SecondaryNetLaneFlags.RequireOpposite;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                dynamicBuffer = this.m_PrefabSecondaryLanes[this.m_PrefabRefData[laneCorner2.m_Lane].m_Prefab];
              }
              else
              {
                secondaryNetLaneFlags13 |= SecondaryNetLaneFlags.OneSided;
                secondaryNetLaneFlags14 = secondaryNetLaneFlags11 | SecondaryNetLaneFlags.Left | SecondaryNetLaneFlags.Right;
              }
              SecondaryNetLaneFlags secondaryNetLaneFlags17 = secondaryNetLaneFlags13 ^ (SecondaryNetLaneFlags.Left | SecondaryNetLaneFlags.Right) | SecondaryNetLaneFlags.CanFlipSides;
              SecondaryNetLaneFlags secondaryNetLaneFlags18 = secondaryNetLaneFlags4 ^ (SecondaryNetLaneFlags.Left | SecondaryNetLaneFlags.Right) | SecondaryNetLaneFlags.CanFlipSides;
              for (int index8 = 0; index8 < prefabSecondaryLane.Length; ++index8)
              {
                SecondaryNetLane secondaryNetLane1 = prefabSecondaryLane[index8];
                bool2 x = new bool2((secondaryNetLane1.m_Flags & secondaryNetLaneFlags13) == secondaryNetLaneFlags13, (secondaryNetLane1.m_Flags & secondaryNetLaneFlags17) == secondaryNetLaneFlags17);
                // ISSUE: reference to a compiler-generated method
                if (!((secondaryNetLane1.m_Flags & secondaryNetLaneFlags9) != 0 | !math.any(x)) && this.CheckRequirements(ref laneBuffer, secondaryNetLane1.m_Lane))
                {
                  bool flag8 = !x.x;
                  // ISSUE: reference to a compiler-generated field
                  if (laneCorner2.m_Lane != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (laneCorner1.m_Lane.Index <= laneCorner2.m_Lane.Index || (secondaryNetLane1.m_Flags & SecondaryNetLaneFlags.DuplicateSides) != (SecondaryNetLaneFlags) 0)
                    {
                      for (int index9 = 0; index9 < dynamicBuffer.Length; ++index9)
                      {
                        SecondaryNetLane secondaryNetLane2 = dynamicBuffer[index9];
                        bool2 bool2 = new bool2((secondaryNetLane2.m_Flags & secondaryNetLaneFlags4) == secondaryNetLaneFlags4, (secondaryNetLane2.m_Flags & secondaryNetLaneFlags18) == secondaryNetLaneFlags18);
                        if ((secondaryNetLane2.m_Flags & secondaryNetLaneFlags14) == (SecondaryNetLaneFlags) 0 & math.any(x & bool2) & secondaryNetLane1.m_Lane == secondaryNetLane2.m_Lane)
                        {
                          flag8 = !(x.x & bool2.x);
                          goto label_119;
                        }
                      }
                      continue;
                    }
                    continue;
                  }
label_119:
                  // ISSUE: reference to a compiler-generated field
                  bool flag9 = flag8 ^ this.m_LeftHandTraffic;
                  if ((secondaryNetLane1.m_Flags & SecondaryNetLaneFlags.DuplicateSides) != (SecondaryNetLaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.CreateSecondaryLane(chunkIndex, ref laneIndex, owner, Entity.Null, laneCorner1.m_Lane, secondaryNetLane1.m_Lane, lanes, laneBuffer, (float2) 0.0f, laneCorner1.m_Width, MathUtils.Left(forward1), MathUtils.Left(forward2), isNode, false, !laneCorner1.m_Inverted, false, false, false, isTemp, ownerTemp);
                    break;
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (laneCorner1.m_Inverted ^ flag9)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.CreateSecondaryLane(chunkIndex, ref laneIndex, owner, laneCorner2.m_Lane, laneCorner1.m_Lane, secondaryNetLane1.m_Lane, lanes, laneBuffer, laneCorner2.m_Width, laneCorner1.m_Width, MathUtils.Left(forward1), MathUtils.Left(forward2), isNode, flag1 ^ flag9, flag9, flag3, flag2, flag7 ^ flag9, isTemp, ownerTemp);
                    break;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.CreateSecondaryLane(chunkIndex, ref laneIndex, owner, laneCorner1.m_Lane, laneCorner2.m_Lane, secondaryNetLane1.m_Lane, lanes, laneBuffer, laneCorner1.m_Width, laneCorner2.m_Width, MathUtils.Left(forward1), MathUtils.Left(forward2), isNode, flag9, flag1 ^ flag9, flag2, flag3, flag7 ^ flag9, isTemp, ownerTemp);
                  break;
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          for (int index10 = 0; index10 < laneBuffer.m_CrossingLanes.Length; ++index10)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            SecondaryLaneSystem.CrossingLane crossingLane = laneBuffer.m_CrossingLanes[index10];
            // ISSUE: reference to a compiler-generated field
            if (!crossingLane.m_Optional)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Curve curveData = new Curve()
              {
                m_Bezier = NetUtils.StraightCurve(crossingLane.m_StartPos, crossingLane.m_EndPos)
              };
              curveData.m_Length = MathUtils.Length(curveData.m_Bezier);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateSecondaryLane(chunkIndex, ref laneIndex, owner, crossingLane.m_Prefab, laneBuffer, curveData, crossingLane.m_StartTangent, crossingLane.m_EndTangent, (float2) 0.0f, isTemp, ownerTemp);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RemoveUnusedOldLanes(chunkIndex, lanes, laneBuffer.m_OldLanes);
          // ISSUE: reference to a compiler-generated method
          laneBuffer.Clear();
        }
        // ISSUE: reference to a compiler-generated method
        laneBuffer.Dispose();
      }

      private void FitToParkingLane(
        Entity lane,
        Curve curve,
        PrefabRef prefabRef,
        float2 sideOffset,
        out Bounds1 curveBounds,
        out ulong blockedMask,
        out int slotCount,
        out float slotAngle,
        out bool2 skipStartEnd)
      {
        curveBounds = new Bounds1(0.0f, 1f);
        blockedMask = 0UL;
        slotCount = 1;
        slotAngle = 0.0f;
        skipStartEnd = (bool2) false;
        ParkingLane componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ParkingLaneData.TryGetComponent(lane, out componentData) || (componentData.m_Flags & ParkingLaneFlags.VirtualLane) != (ParkingLaneFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        ParkingLaneData prefabParkingLane = this.m_PrefabParkingLaneData[prefabRef.m_Prefab];
        if ((double) prefabParkingLane.m_SlotInterval != 0.0)
        {
          slotCount = NetUtils.GetParkingSlotCount(curve, componentData, prefabParkingLane);
          float parkingSlotInterval = NetUtils.GetParkingSlotInterval(curve, componentData, prefabParkingLane, slotCount);
          slotAngle = prefabParkingLane.m_SlotAngle;
          float4 float4_1 = (float4) 0.0f with
          {
            y = (float) slotCount * parkingSlotInterval
          };
          float4_1.x = -float4_1.y;
          float4_1.zw = math.select((float2) 0.0f, sideOffset * math.tan(1.57079637f - slotAngle), (double) slotAngle > 0.25);
          float4 float4_2 = float4_1 / curve.m_Length;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<LaneOverlap> laneOverlap1 = this.m_LaneOverlaps[lane];
          float x1;
          switch (componentData.m_Flags & (ParkingLaneFlags.StartingLane | ParkingLaneFlags.EndingLane))
          {
            case ParkingLaneFlags.StartingLane:
              x1 = curve.m_Length - (float) slotCount * parkingSlotInterval;
              curveBounds.min = 1f + float4_2.x + float4_2.z;
              curveBounds.max = 1f + float4_2.w;
              break;
            case ParkingLaneFlags.EndingLane:
              x1 = 0.0f;
              curveBounds.min = float4_2.z;
              curveBounds.max = float4_2.y + float4_2.w;
              skipStartEnd.x = true;
              break;
            default:
              x1 = (float) (((double) curve.m_Length - (double) slotCount * (double) parkingSlotInterval) * 0.5);
              curveBounds.min = (float) (0.5 + (double) float4_2.x * 0.5) + float4_2.z;
              curveBounds.max = (float) (0.5 + (double) float4_2.y * 0.5) + float4_2.w;
              break;
          }
          float3 x2 = curve.m_Bezier.a;
          float num1 = 0.0f;
          int num2 = -1;
          float4 float4_3 = (float4) 0.0f;
          float num3 = math.max(x1, 0.0f);
          float2 float2_1 = (float2) 2f;
          int num4 = 0;
          if (num4 < laneOverlap1.Length)
          {
            LaneOverlap laneOverlap2 = laneOverlap1[num4++];
            float2_1 = new float2((float) laneOverlap2.m_ThisStart, (float) laneOverlap2.m_ThisEnd) * 0.003921569f;
          }
          for (int index = 1; index <= 16; ++index)
          {
            float num5 = (float) index * (1f / 16f);
            float3 y = MathUtils.Position(curve.m_Bezier, num5);
            for (num1 += math.distance(x2, y); (double) num1 >= (double) num3 || index == 16 && num2 < slotCount; ++num2)
            {
              float4_3.y = math.select(num5, math.lerp(float4_3.x, num5, num3 / num1), (double) num3 < (double) num1);
              bool flag = false;
              if ((double) float2_1.x < (double) float4_3.y)
              {
                flag = true;
                if ((double) float2_1.y <= (double) float4_3.y)
                {
                  float2_1 = (float2) 2f;
                  while (num4 < laneOverlap1.Length)
                  {
                    LaneOverlap laneOverlap3 = laneOverlap1[num4++];
                    float2 float2_2 = new float2((float) laneOverlap3.m_ThisStart, (float) laneOverlap3.m_ThisEnd) * 0.003921569f;
                    if ((double) float2_2.y > (double) float4_3.y)
                    {
                      float2_1 = float2_2;
                      break;
                    }
                  }
                }
              }
              if (flag && num2 >= 0 && num2 < slotCount)
                blockedMask |= (ulong) (1L << num2);
              num1 -= num3;
              float4_3.x = float4_3.y;
              num3 = parkingSlotInterval;
            }
            x2 = y;
          }
        }
        else
        {
          skipStartEnd.x = (componentData.m_Flags & ParkingLaneFlags.StartingLane) == (ParkingLaneFlags) 0;
          skipStartEnd.y = (componentData.m_Flags & ParkingLaneFlags.EndingLane) == (ParkingLaneFlags) 0;
        }
        slotAngle = math.select(slotAngle, -slotAngle, (componentData.m_Flags & ParkingLaneFlags.ParkingLeft) != 0);
      }

      private void FillOldLaneBuffer(
        DynamicBuffer<SubLane> lanes,
        NativeParallelHashMap<SecondaryLaneSystem.LaneKey, Entity> laneBuffer)
      {
        for (int index = 0; index < lanes.Length; ++index)
        {
          Entity subLane = lanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SecondaryLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            SecondaryLaneSystem.LaneKey key = new SecondaryLaneSystem.LaneKey(this.m_LaneData[subLane], this.m_PrefabRefData[subLane].m_Prefab);
            laneBuffer.TryAdd(key, subLane);
          }
        }
      }

      private void RemoveUnusedOldLanes(
        int jobIndex,
        DynamicBuffer<SubLane> lanes,
        NativeParallelHashMap<SecondaryLaneSystem.LaneKey, Entity> laneBuffer)
      {
        for (int index = 0; index < lanes.Length; ++index)
        {
          Entity subLane = lanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SecondaryLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            SecondaryLaneSystem.LaneKey key = new SecondaryLaneSystem.LaneKey(this.m_LaneData[subLane], this.m_PrefabRefData[subLane].m_Prefab);
            if (laneBuffer.TryGetValue(key, out Entity _))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, subLane, in this.m_AppliedTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, subLane, new Deleted());
              laneBuffer.Remove(key);
            }
          }
        }
      }

      private void AddCrossingLane(
        SecondaryLaneSystem.LaneBuffer laneBuffer,
        Entity prefab,
        float3 startPos,
        float3 endPos,
        float2 tangent,
        bool isOptional)
      {
        float2 float2_1 = tangent;
        float2 float2_2 = tangent;
        bool flag = true;
        while (flag)
        {
          flag = false;
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < laneBuffer.m_CrossingLanes.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            SecondaryLaneSystem.CrossingLane crossingLane = laneBuffer.m_CrossingLanes[index];
            // ISSUE: reference to a compiler-generated field
            if (!(crossingLane.m_Prefab != prefab))
            {
              // ISSUE: reference to a compiler-generated field
              if ((double) math.distancesq(crossingLane.m_EndPos, startPos) < 1.0)
              {
                // ISSUE: reference to a compiler-generated field
                startPos = crossingLane.m_StartPos;
                // ISSUE: reference to a compiler-generated field
                float2_1 = crossingLane.m_StartTangent;
                // ISSUE: reference to a compiler-generated field
                isOptional &= crossingLane.m_Optional;
                // ISSUE: reference to a compiler-generated field
                laneBuffer.m_CrossingLanes.RemoveAtSwapBack(index);
                flag = true;
                break;
              }
              // ISSUE: reference to a compiler-generated field
              if ((double) math.distancesq(crossingLane.m_StartPos, endPos) < 1.0)
              {
                // ISSUE: reference to a compiler-generated field
                endPos = crossingLane.m_EndPos;
                // ISSUE: reference to a compiler-generated field
                float2_2 = crossingLane.m_EndTangent;
                // ISSUE: reference to a compiler-generated field
                isOptional &= crossingLane.m_Optional;
                // ISSUE: reference to a compiler-generated field
                laneBuffer.m_CrossingLanes.RemoveAtSwapBack(index);
                flag = true;
                break;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        laneBuffer.m_CrossingLanes.Add(new SecondaryLaneSystem.CrossingLane()
        {
          m_Prefab = prefab,
          m_StartPos = startPos,
          m_StartTangent = float2_1,
          m_EndPos = endPos,
          m_EndTangent = float2_2,
          m_Optional = isOptional
        });
      }

      private void CreateSecondaryLane(
        int jobIndex,
        ref int laneIndex,
        Entity owner,
        Entity leftLane,
        Entity rightLane,
        Entity prefab,
        DynamicBuffer<SubLane> lanes,
        SecondaryLaneSystem.LaneBuffer laneBuffer,
        float2 leftWidth,
        float2 rightWidth,
        float2 startTangent,
        float2 endTangent,
        bool isNode,
        bool invertLeft,
        bool invertRight,
        bool mergeStart,
        bool mergeEnd,
        bool mergeLeft,
        bool isTemp,
        Temp ownerTemp)
      {
        // ISSUE: reference to a compiler-generated field
        SecondaryLaneData secondaryLaneData = this.m_PrefabSecondaryLaneData[prefab];
        NetLaneGeometryData laneGeometryData = new NetLaneGeometryData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabLaneGeometryData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          laneGeometryData = this.m_PrefabLaneGeometryData[prefab];
        }
        Bezier4x3 curve1 = new Bezier4x3();
        float x = math.max(0.01f, laneGeometryData.m_Size.x);
        // ISSUE: reference to a compiler-generated field
        laneBuffer.m_CutRanges.Clear();
        Curve curve2 = new Curve();
        Curve curve3 = new Curve();
        float slotAngle1;
        bool2 skipStartEnd1;
        if (leftLane != Entity.Null && rightLane != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          curve2 = this.m_CurveData[leftLane];
          // ISSUE: reference to a compiler-generated field
          curve3 = this.m_CurveData[rightLane];
          if ((secondaryLaneData.m_Flags & SecondaryLaneDataFlags.FitToParkingSpaces) != (SecondaryLaneDataFlags) 0)
          {
            Bounds1 curveBounds1;
            ulong blockedMask1;
            int slotCount1;
            float slotAngle2;
            bool2 skipStartEnd2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FitToParkingLane(leftLane, curve2, this.m_PrefabRefData[leftLane], leftWidth * 0.5f, out curveBounds1, out blockedMask1, out slotCount1, out slotAngle2, out skipStartEnd2);
            Bounds1 curveBounds2;
            ulong blockedMask2;
            int slotCount2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FitToParkingLane(rightLane, curve3, this.m_PrefabRefData[rightLane], rightWidth * 0.5f, out curveBounds2, out blockedMask2, out slotCount2, out slotAngle2, out skipStartEnd2);
            // ISSUE: reference to a compiler-generated method
            this.GetCutRanges(leftLane, lanes, laneBuffer, curveBounds1, blockedMask1, slotCount1, invertLeft);
            // ISSUE: reference to a compiler-generated method
            this.GetCutRanges(rightLane, lanes, laneBuffer, curveBounds2, blockedMask2, slotCount2, invertRight);
          }
          if (invertLeft)
          {
            curve2.m_Bezier = MathUtils.Invert(curve2.m_Bezier);
            leftWidth = leftWidth.yx;
          }
          if (invertRight)
          {
            curve3.m_Bezier = MathUtils.Invert(curve3.m_Bezier);
            rightWidth = rightWidth.yx;
          }
          float2 float2 = math.select(leftWidth / (leftWidth + rightWidth), (float2) 0.5f, leftWidth == 0.0f & rightWidth == 0.0f);
          Bezier4x1 t = new Bezier4x1(float2.x, float2.x, float2.y, float2.y);
          curve1 = MathUtils.Lerp(curve2.m_Bezier, curve3.m_Bezier, t);
          if (mergeStart | mergeEnd)
          {
            Bezier4x3 curve4 = !mergeLeft ? NetUtils.OffsetCurveLeftSmooth(curve2.m_Bezier, leftWidth * -0.5f - secondaryLaneData.m_CutOffset) : NetUtils.OffsetCurveLeftSmooth(curve3.m_Bezier, rightWidth * 0.5f - secondaryLaneData.m_CutOffset);
            // ISSUE: reference to a compiler-generated method
            if (!this.ValidateCurve(curve4))
              return;
            if (mergeStart)
            {
              curve1.a = curve4.a;
              curve1.b = curve4.b;
            }
            if (mergeEnd)
            {
              curve1.c = curve4.c;
              curve1.d = curve4.d;
            }
          }
          // ISSUE: reference to a compiler-generated method
          this.GetCutRanges(leftLane, secondaryLaneData.m_Flags, laneBuffer, curve1, leftWidth, secondaryLaneData.m_CutOverlap, invertLeft, false, rightLane);
          // ISSUE: reference to a compiler-generated method
          this.GetCutRanges(rightLane, secondaryLaneData.m_Flags, laneBuffer, curve1, rightWidth, secondaryLaneData.m_CutOverlap, invertRight, true, leftLane);
          x = math.min(x, math.min(curve2.m_Length, curve3.m_Length) * 0.5f);
        }
        else if (leftLane != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          curve2 = this.m_CurveData[leftLane];
          if ((secondaryLaneData.m_Flags & SecondaryLaneDataFlags.FitToParkingSpaces) != (SecondaryLaneDataFlags) 0)
          {
            Bounds1 curveBounds;
            ulong blockedMask;
            int slotCount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FitToParkingLane(leftLane, curve2, this.m_PrefabRefData[leftLane], leftWidth * 0.5f, out curveBounds, out blockedMask, out slotCount, out slotAngle1, out skipStartEnd1);
            // ISSUE: reference to a compiler-generated method
            this.GetCutRanges(leftLane, lanes, laneBuffer, curveBounds, blockedMask, slotCount, invertLeft);
          }
          if (invertLeft)
          {
            curve2.m_Bezier = MathUtils.Invert(curve2.m_Bezier);
            leftWidth = leftWidth.yx;
          }
          curve1 = NetUtils.OffsetCurveLeftSmooth(curve2.m_Bezier, leftWidth * -0.5f - secondaryLaneData.m_CutOffset);
          // ISSUE: reference to a compiler-generated method
          if (!this.ValidateCurve(curve1))
            return;
          // ISSUE: reference to a compiler-generated method
          this.GetCutRanges(leftLane, secondaryLaneData.m_Flags, laneBuffer, curve1, leftWidth, secondaryLaneData.m_CutOverlap, invertLeft, false, Entity.Null);
          x = math.min(x, curve2.m_Length * 0.5f);
        }
        else if (rightLane != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          curve3 = this.m_CurveData[rightLane];
          if ((secondaryLaneData.m_Flags & SecondaryLaneDataFlags.FitToParkingSpaces) != (SecondaryLaneDataFlags) 0)
          {
            Bounds1 curveBounds;
            ulong blockedMask;
            int slotCount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FitToParkingLane(rightLane, curve3, this.m_PrefabRefData[rightLane], rightWidth * 0.5f, out curveBounds, out blockedMask, out slotCount, out slotAngle1, out skipStartEnd1);
            // ISSUE: reference to a compiler-generated method
            this.GetCutRanges(rightLane, lanes, laneBuffer, curveBounds, blockedMask, slotCount, invertRight);
          }
          if (invertRight)
          {
            curve3.m_Bezier = MathUtils.Invert(curve3.m_Bezier);
            rightWidth = rightWidth.yx;
          }
          curve1 = NetUtils.OffsetCurveLeftSmooth(curve3.m_Bezier, rightWidth * 0.5f - secondaryLaneData.m_CutOffset);
          // ISSUE: reference to a compiler-generated method
          if (!this.ValidateCurve(curve1))
            return;
          // ISSUE: reference to a compiler-generated method
          this.GetCutRanges(rightLane, secondaryLaneData.m_Flags, laneBuffer, curve1, rightWidth, secondaryLaneData.m_CutOverlap, invertRight, true, Entity.Null);
          x = math.min(x, curve3.m_Length * 0.5f);
        }
        if ((double) secondaryLaneData.m_PositionOffset.x != (double) secondaryLaneData.m_CutOffset)
          curve1 = NetUtils.OffsetCurveLeftSmooth(curve1, (float2) (secondaryLaneData.m_CutOffset - secondaryLaneData.m_PositionOffset.x));
        if ((double) secondaryLaneData.m_PositionOffset.y != 0.0)
        {
          curve1.a.y += secondaryLaneData.m_PositionOffset.y;
          curve1.b.y += secondaryLaneData.m_PositionOffset.y;
          curve1.c.y += secondaryLaneData.m_PositionOffset.y;
          curve1.d.y += secondaryLaneData.m_PositionOffset.y;
        }
        Bounds1 bounds1 = new Bounds1(0.0f, 1f);
        // ISSUE: reference to a compiler-generated field
        if (laneBuffer.m_CutRanges.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_CutRanges.Sort<SecondaryLaneSystem.CutRange>();
        }
        Bezier4x2 curve5 = new Bezier4x2();
        PrefabRef componentData1;
        UtilityLaneData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.TryGetComponent(leftLane, out componentData1) && this.m_PrefabUtilityLaneData.TryGetComponent((Entity) componentData1, out componentData2) && (double) componentData2.m_Hanging != 0.0)
        {
          HangingLane componentData3;
          // ISSUE: reference to a compiler-generated field
          this.m_HangingLaneData.TryGetComponent(leftLane, out componentData3);
          curve5.a.x = componentData3.m_Distances.x;
          curve5.b.x = (float) (((double) componentData3.m_Distances.x + (double) componentData2.m_Hanging * (double) curve2.m_Length) * 0.66666668653488159);
          curve5.c.x = (float) (((double) componentData3.m_Distances.y + (double) componentData2.m_Hanging * (double) curve2.m_Length) * 0.66666668653488159);
          curve5.d.x = componentData3.m_Distances.y;
        }
        PrefabRef componentData4;
        UtilityLaneData componentData5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.TryGetComponent(rightLane, out componentData4) && this.m_PrefabUtilityLaneData.TryGetComponent((Entity) componentData4, out componentData5) && (double) componentData5.m_Hanging != 0.0)
        {
          HangingLane componentData6;
          // ISSUE: reference to a compiler-generated field
          this.m_HangingLaneData.TryGetComponent(rightLane, out componentData6);
          curve5.a.y = componentData6.m_Distances.x;
          curve5.b.y = (float) (((double) componentData6.m_Distances.x + (double) componentData5.m_Hanging * (double) curve3.m_Length) * 0.66666668653488159);
          curve5.c.y = (float) (((double) componentData6.m_Distances.y + (double) componentData5.m_Hanging * (double) curve3.m_Length) * 0.66666668653488159);
          curve5.d.y = componentData6.m_Distances.y;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < laneBuffer.m_CutRanges.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          SecondaryLaneSystem.CutRange cutRange = laneBuffer.m_CutRanges[index1];
          // ISSUE: reference to a compiler-generated field
          if ((double) cutRange.m_Bounds.min > (double) bounds1.min)
          {
            // ISSUE: reference to a compiler-generated field
            Bounds1 bounds2 = new Bounds1(bounds1.min, math.min(bounds1.max, cutRange.m_Bounds.min));
            if ((double) bounds2.max > (double) bounds2.min)
            {
              if ((double) secondaryLaneData.m_CutMargin > 1.0 / 1000.0)
              {
                if ((double) bounds2.min > 1.0 / 1000.0)
                {
                  Bounds1 t = bounds2;
                  MathUtils.ClampLength(curve1, ref t, secondaryLaneData.m_CutMargin);
                  bounds2.min = t.max;
                }
                if ((double) bounds2.max < 0.99900001287460327)
                {
                  Bounds1 t = bounds2;
                  MathUtils.ClampLengthInverse(curve1, ref t, secondaryLaneData.m_CutMargin);
                  bounds2.max = t.min;
                }
              }
              Curve curve6 = new Curve()
              {
                m_Bezier = MathUtils.Cut(curve1, bounds2)
              };
              curve6.m_Length = MathUtils.Length(curve6.m_Bezier);
              if ((double) curve6.m_Length >= (double) x)
              {
                if ((double) secondaryLaneData.m_Spacing > 0.10000000149011612)
                {
                  int count;
                  float offset;
                  float factor;
                  // ISSUE: reference to a compiler-generated method
                  this.CalculateSpacing(secondaryLaneData, curve6, out count, out offset, out factor);
                  for (int index2 = 0; index2 < count; ++index2)
                  {
                    float t = math.lerp(bounds2.min, bounds2.max, ((float) index2 + offset) * factor);
                    float2 hangingDistances = MathUtils.Position(curve5, t);
                    curve6.m_Bezier = NetUtils.StraightCurve(MathUtils.Position(curve2.m_Bezier, t), MathUtils.Position(curve3.m_Bezier, t));
                    curve6.m_Length = math.distance(curve6.m_Bezier.a, curve6.m_Bezier.d);
                    // ISSUE: reference to a compiler-generated method
                    this.CreateSecondaryLane(jobIndex, ref laneIndex, owner, prefab, laneBuffer, curve6, startTangent, endTangent, hangingDistances, isTemp, ownerTemp);
                  }
                }
                else
                {
                  float2 hangingDistances = math.lerp(new float2(curve5.a.x, curve5.d.x), new float2(curve5.a.y, curve5.d.y), 0.5f);
                  // ISSUE: reference to a compiler-generated method
                  this.CreateSecondaryLane(jobIndex, ref laneIndex, owner, prefab, laneBuffer, curve6, startTangent, endTangent, hangingDistances, isTemp, ownerTemp);
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          bounds1.min = math.max(bounds1.min, cutRange.m_Bounds.max);
          if ((double) bounds1.min >= (double) bounds1.max)
            break;
        }
        if ((double) bounds1.max <= (double) bounds1.min)
          return;
        if ((double) secondaryLaneData.m_CutMargin > 1.0 / 1000.0)
        {
          if ((double) bounds1.min > 1.0 / 1000.0)
          {
            Bounds1 t = bounds1;
            MathUtils.ClampLength(curve1, ref t, secondaryLaneData.m_CutMargin);
            bounds1.min = t.max;
          }
          if ((double) bounds1.max < 0.99900001287460327)
          {
            Bounds1 t = bounds1;
            MathUtils.ClampLengthInverse(curve1, ref t, secondaryLaneData.m_CutMargin);
            bounds1.max = t.min;
          }
        }
        Curve curve7 = new Curve()
        {
          m_Bezier = MathUtils.Cut(curve1, bounds1)
        };
        curve7.m_Length = MathUtils.Length(curve7.m_Bezier);
        if ((double) curve7.m_Length < (double) x)
          return;
        if ((double) secondaryLaneData.m_Spacing > 0.10000000149011612)
        {
          int count;
          float offset;
          float factor;
          // ISSUE: reference to a compiler-generated method
          this.CalculateSpacing(secondaryLaneData, curve7, out count, out offset, out factor);
          for (int index = 0; index < count; ++index)
          {
            float t = math.lerp(bounds1.min, bounds1.max, ((float) index + offset) * factor);
            float2 hangingDistances = MathUtils.Position(curve5, t);
            curve7.m_Bezier = NetUtils.StraightCurve(MathUtils.Position(curve2.m_Bezier, t), MathUtils.Position(curve3.m_Bezier, t));
            curve7.m_Length = math.distance(curve7.m_Bezier.a, curve7.m_Bezier.d);
            // ISSUE: reference to a compiler-generated method
            this.CreateSecondaryLane(jobIndex, ref laneIndex, owner, prefab, laneBuffer, curve7, startTangent, endTangent, hangingDistances, isTemp, ownerTemp);
          }
        }
        else
        {
          float2 hangingDistances = math.lerp(new float2(curve5.a.x, curve5.d.x), new float2(curve5.a.y, curve5.d.y), 0.5f);
          // ISSUE: reference to a compiler-generated method
          this.CreateSecondaryLane(jobIndex, ref laneIndex, owner, prefab, laneBuffer, curve7, startTangent, endTangent, hangingDistances, isTemp, ownerTemp);
        }
      }

      private void CalculateSpacing(
        SecondaryLaneData secondaryLaneData,
        Curve curve,
        out int count,
        out float offset,
        out float factor)
      {
        count = Mathf.RoundToInt(curve.m_Length / secondaryLaneData.m_Spacing);
        factor = 1f / (float) count;
        if ((secondaryLaneData.m_Flags & SecondaryLaneDataFlags.EvenSpacing) != (SecondaryLaneDataFlags) 0)
        {
          --count;
          offset = 1f;
        }
        else
          offset = 0.5f;
      }

      private bool ValidateCurve(Bezier4x3 curve)
      {
        float2 xz1 = MathUtils.StartTangent(curve).xz;
        float2 xz2 = MathUtils.EndTangent(curve).xz;
        float2 y = curve.d.xz - curve.a.xz;
        if (!MathUtils.TryNormalize(ref xz1) || !MathUtils.TryNormalize(ref xz2) || !MathUtils.TryNormalize(ref y))
          return false;
        float2 x = new float2(math.dot(xz1, y), math.dot(xz2, y));
        return (double) math.dot(xz1, xz2) >= -0.99000000953674316 || (double) math.cmax(math.abs(x)) <= 0.99000000953674316;
      }

      private void CreateSecondaryLane(
        int jobIndex,
        ref int laneIndex,
        Entity owner,
        Entity prefab,
        SecondaryLaneSystem.LaneBuffer laneBuffer,
        Curve curveData,
        float2 startTangent,
        float2 endTangent,
        float2 hangingDistances,
        bool isTemp,
        Temp ownerTemp)
      {
        PrefabRef component1 = new PrefabRef(prefab);
        // ISSUE: reference to a compiler-generated field
        SecondaryLaneData secondaryLaneData = this.m_PrefabSecondaryLaneData[prefab];
        float2 x1 = (float2) secondaryLaneData.m_LengthOffset.x;
        if ((double) secondaryLaneData.m_LengthOffset.y != 0.0)
        {
          float2 y1 = math.normalizesafe(MathUtils.StartTangent(curveData.m_Bezier).xz);
          float2 y2 = math.normalizesafe(MathUtils.StartTangent(curveData.m_Bezier).xz);
          float2 float2 = math.tan(1.57079637f - math.acos(math.saturate(math.abs(new float2(math.dot(startTangent, y1), math.dot(endTangent, y2))))));
          x1 += float2 * secondaryLaneData.m_LengthOffset.y * 0.5f;
        }
        if (math.any(x1 < 0.0f))
        {
          float2 x2 = math.min((float2) 0.5f, -x1 / math.max(1f / 1000f, curveData.m_Length));
          curveData.m_Length -= curveData.m_Length * math.csum(x2);
          curveData.m_Bezier = MathUtils.Cut(curveData.m_Bezier, new float2(x2.x, 1f - x2.y));
        }
        Owner component2 = new Owner();
        component2.m_Owner = owner;
        Elevation component3 = new Elevation();
        Lane lane = new Lane();
        lane.m_StartNode = new PathNode(new PathNode(owner, (ushort) laneIndex++), true);
        lane.m_MiddleNode = new PathNode(new PathNode(owner, (ushort) laneIndex++), true);
        lane.m_EndNode = new PathNode(new PathNode(owner, (ushort) laneIndex++), true);
        Temp temp = new Temp();
        if (isTemp)
        {
          temp.m_Flags = ownerTemp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
          if ((ownerTemp.m_Flags & TempFlags.Replace) != (TempFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
        }
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SecondaryLaneSystem.LaneKey key = new SecondaryLaneSystem.LaneKey(lane, component1.m_Prefab);
        // ISSUE: variable of a compiler-generated type
        SecondaryLaneSystem.LaneKey laneKey = key;
        if (isTemp)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReplaceTempOwner(ref laneKey, owner);
          // ISSUE: reference to a compiler-generated method
          this.GetOriginalLane(laneBuffer, laneKey, ref temp);
        }
        HangingLane component4 = new HangingLane();
        bool flag = false;
        UtilityLaneData componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabUtilityLaneData.TryGetComponent(prefab, out componentData) && (double) componentData.m_Hanging != 0.0)
        {
          component4.m_Distances = hangingDistances;
          flag = true;
        }
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (laneBuffer.m_OldLanes.TryGetValue(key, out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_OldLanes.Remove(key);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, curveData);
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<HangingLane>(jobIndex, entity1, component4);
          }
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, entity1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity1, new Updated());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(jobIndex, entity1, temp);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_DeletedTempTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_AppliedTypes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, entity1);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity1, new Updated());
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabLaneArchetypeData[component1.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, laneArchetypeData.m_LaneArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, lane);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, curveData);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, entity2, component3);
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<HangingLane>(jobIndex, entity2, component4);
          }
          if (!isTemp)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity2, temp);
        }
      }

      private void GetCutRanges(
        Entity lane,
        DynamicBuffer<SubLane> lanes,
        SecondaryLaneSystem.LaneBuffer laneBuffer,
        Bounds1 bounds,
        ulong blockedMask,
        int slotCount,
        bool invert)
      {
        Bounds1 bounds1 = new Bounds1(0.0f, 1f);
        ParkingLane componentData;
        // ISSUE: reference to a compiler-generated field
        if ((double) bounds.min > 9.9999997473787516E-05 && this.m_ParkingLaneData.TryGetComponent(lane, out componentData) && (componentData.m_Flags & ParkingLaneFlags.StartingLane) == (ParkingLaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve1 = this.m_CurveData[lane];
          for (int index = 0; index < lanes.Length; ++index)
          {
            SubLane lane1 = lanes[index];
            if ((lane1.m_PathMethods & PathMethod.Parking) != (PathMethod) 0)
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve2 = this.m_CurveData[lane1.m_SubLane];
              if ((double) math.distancesq(curve2.m_Bezier.d, curve1.m_Bezier.a) <= 9.9999997473787516E-05)
              {
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef = this.m_PrefabRefData[lane1.m_SubLane];
                ulong blockedMask1;
                int slotCount1;
                // ISSUE: reference to a compiler-generated method
                this.FitToParkingLane(lane1.m_SubLane, curve2, prefabRef, (float2) 0.0f, out Bounds1 _, out blockedMask1, out slotCount1, out float _, out bool2 _);
                if (slotCount1 != 0 && ((long) (blockedMask1 >> slotCount1 - 1) & 1L) == 0L)
                {
                  bounds1.min = bounds.min;
                  break;
                }
                break;
              }
            }
          }
        }
        if (invert)
        {
          blockedMask = math.reversebits(blockedMask) >> 64 - slotCount;
          bounds = 1f - MathUtils.Invert(bounds);
          bounds1 = 1f - MathUtils.Invert(bounds1);
        }
        Bounds1 bounds1_1 = new Bounds1(bounds1.min, bounds.min);
        float num = MathUtils.Size(bounds) / (float) math.max(1, slotCount);
        // ISSUE: variable of a compiler-generated type
        SecondaryLaneSystem.CutRange cutRange;
        for (int index = 0; index < slotCount; ++index)
        {
          if (((long) (blockedMask >> index) & 1L) != 0L)
          {
            bounds1_1.min = math.min(bounds1_1.min, bounds.min + (float) index * num);
            bounds1_1.max = bounds.min + (float) (index + 1) * num;
          }
          else
          {
            if ((double) bounds1_1.max > (double) bounds1_1.min)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeList<SecondaryLaneSystem.CutRange> local1 = ref laneBuffer.m_CutRanges;
              // ISSUE: object of a compiler-generated type is created
              cutRange = new SecondaryLaneSystem.CutRange();
              // ISSUE: reference to a compiler-generated field
              cutRange.m_Bounds = bounds1_1;
              // ISSUE: reference to a compiler-generated field
              cutRange.m_Group = uint.MaxValue;
              ref SecondaryLaneSystem.CutRange local2 = ref cutRange;
              local1.Add(in local2);
            }
            bounds1_1 = new Bounds1(bounds.max, 0.0f);
          }
        }
        bounds1_1.max = bounds1.max;
        if ((double) bounds1_1.max <= (double) bounds1_1.min)
          return;
        // ISSUE: reference to a compiler-generated field
        ref NativeList<SecondaryLaneSystem.CutRange> local3 = ref laneBuffer.m_CutRanges;
        // ISSUE: object of a compiler-generated type is created
        cutRange = new SecondaryLaneSystem.CutRange();
        // ISSUE: reference to a compiler-generated field
        cutRange.m_Bounds = bounds1_1;
        // ISSUE: reference to a compiler-generated field
        cutRange.m_Group = uint.MaxValue;
        ref SecondaryLaneSystem.CutRange local4 = ref cutRange;
        local3.Add(in local4);
      }

      private void GetCutRanges(
        Entity lane,
        SecondaryLaneDataFlags flags,
        SecondaryLaneSystem.LaneBuffer laneBuffer,
        Bezier4x3 curve,
        float2 width,
        float cutOverlap,
        bool invert,
        bool isRight,
        Entity ignore)
      {
        bool flag1 = (flags & SecondaryLaneDataFlags.SkipSafePedestrianOverlap) != 0;
        bool flag2 = (flags & (SecondaryLaneDataFlags.SkipSafeCarOverlap | SecondaryLaneDataFlags.SkipUnsafeCarOverlap)) != 0;
        bool flag3 = (flags & SecondaryLaneDataFlags.SkipTrackOverlap) != 0;
        bool flag4 = (flags & SecondaryLaneDataFlags.SkipMergeOverlap) != 0;
        // ISSUE: reference to a compiler-generated field
        if (flag2 && this.m_CarLaneData.HasComponent(lane))
        {
          // ISSUE: reference to a compiler-generated field
          CarLane carLane = this.m_CarLaneData[lane];
          CarLaneFlags carLaneFlags = isRight != invert ? CarLaneFlags.Roundabout | CarLaneFlags.LeftLimit : CarLaneFlags.Roundabout | CarLaneFlags.RightLimit;
          flag2 = (carLane.m_Flags & (carLaneFlags | CarLaneFlags.Approach)) != carLaneFlags;
        }
        // ISSUE: reference to a compiler-generated field
        if (!flag1 && !flag2 && !flag3 || !this.m_LaneOverlaps.HasBuffer(lane))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LaneOverlap> laneOverlap1 = this.m_LaneOverlaps[lane];
        // ISSUE: reference to a compiler-generated field
        int length = laneBuffer.m_CutRanges.Length;
label_39:
        for (int index1 = 0; index1 < laneOverlap1.Length; ++index1)
        {
          LaneOverlap laneOverlap2 = laneOverlap1[index1];
          if ((laneOverlap2.m_Flags & (isRight != invert ? OverlapFlags.OverlapLeft : OverlapFlags.OverlapRight)) != (OverlapFlags) 0 && !(laneOverlap2.m_Other == ignore))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!flag1 || !this.m_PedestrianLaneData.HasComponent(laneOverlap2.m_Other) || (this.m_PedestrianLaneData[laneOverlap2.m_Other].m_Flags & PedestrianLaneFlags.Unsafe) != (PedestrianLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (flag2 && this.m_CarLaneData.HasComponent(laneOverlap2.m_Other))
              {
                // ISSUE: reference to a compiler-generated field
                CarLane carLane = this.m_CarLaneData[laneOverlap2.m_Other];
                if ((flags & SecondaryLaneDataFlags.SkipSafeCarOverlap) != (SecondaryLaneDataFlags) 0 && (carLane.m_Flags & CarLaneFlags.Unsafe) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) || (flags & SecondaryLaneDataFlags.SkipUnsafeCarOverlap) != (SecondaryLaneDataFlags) 0 && (carLane.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                {
                  if ((carLane.m_Flags & (CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.LeftLimit)) == (CarLaneFlags.Roundabout | CarLaneFlags.LeftLimit))
                    laneOverlap2.m_Flags &= ~OverlapFlags.OverlapRight;
                  if ((carLane.m_Flags & (CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit)) == (CarLaneFlags.Roundabout | CarLaneFlags.RightLimit))
                  {
                    laneOverlap2.m_Flags &= ~OverlapFlags.OverlapLeft;
                    goto label_16;
                  }
                  else
                    goto label_16;
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (!flag3 || !this.m_TrackLaneData.HasComponent(laneOverlap2.m_Other))
              {
                // ISSUE: reference to a compiler-generated field
                if (flag4 && this.m_SlaveLaneData.HasComponent(laneOverlap2.m_Other))
                {
                  // ISSUE: reference to a compiler-generated field
                  SlaveLane slaveLane = this.m_SlaveLaneData[laneOverlap2.m_Other];
                  Owner componentData1;
                  DynamicBuffer<SubLane> bufferData;
                  CarLane componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((slaveLane.m_Flags & SlaveLaneFlags.MergingLane) == (SlaveLaneFlags) 0 || (flags & (SecondaryLaneDataFlags.SkipSafeCarOverlap | SecondaryLaneDataFlags.SkipUnsafeCarOverlap)) == SecondaryLaneDataFlags.SkipSafeCarOverlap && (!this.m_OwnerData.TryGetComponent(laneOverlap2.m_Other, out componentData1) || !this.m_SubLanes.TryGetBuffer(componentData1.m_Owner, out bufferData) || bufferData.Length <= (int) slaveLane.m_MasterIndex || !this.m_CarLaneData.TryGetComponent(bufferData[(int) slaveLane.m_MasterIndex].m_SubLane, out componentData2) || (componentData2.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter)))
                    continue;
                }
                else
                  continue;
              }
            }
label_16:
            // ISSUE: reference to a compiler-generated field
            Curve curve1 = this.m_CurveData[laneOverlap2.m_Other];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData = this.m_PrefabLaneData[this.m_PrefabRefData[laneOverlap2.m_Other].m_Prefab];
            float2 a1 = new float2((float) laneOverlap2.m_ThisStart, (float) laneOverlap2.m_ThisEnd) * 0.003921569f;
            float2 float2 = math.select(a1, 1f - a1, invert);
            Bounds1 bounds1 = new Bounds1(1f, 0.0f);
            int num1 = 0;
            if ((laneOverlap2.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeMiddleStart)) != (OverlapFlags) 0)
            {
              bounds1 |= float2.x;
              ++num1;
            }
            if ((laneOverlap2.m_Flags & (OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleEnd)) != (OverlapFlags) 0)
            {
              bounds1 |= float2.y;
              ++num1;
            }
            float2 width1 = (float2) netLaneData.m_Width;
            NodeLane componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeLaneData.TryGetComponent(laneOverlap2.m_Other, out componentData))
              width1 += componentData.m_WidthOffset;
            float4 a2 = new float4(math.min((float2) 0.0f, cutOverlap - width1 * 0.5f), math.max((float2) 0.0f, width1 * 0.5f - cutOverlap));
            a2 = math.select(a2, a2.zwxy, (laneOverlap2.m_Flags & OverlapFlags.MergeFlip) != 0);
            if ((laneOverlap2.m_Flags & OverlapFlags.OverlapLeft) != (OverlapFlags) 0)
            {
              Bezier4x3 curve2 = NetUtils.OffsetCurveLeftSmooth(curve1.m_Bezier, a2.xy);
              float2 t;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (this.ValidateCurve(curve2) && this.ExtendedIntersect(curve.xz, curve2.xz, width, width1, out t))
              {
                bounds1 |= t.x;
                ++num1;
              }
            }
            if ((laneOverlap2.m_Flags & OverlapFlags.OverlapRight) != (OverlapFlags) 0)
            {
              Bezier4x3 curve3 = NetUtils.OffsetCurveLeftSmooth(curve1.m_Bezier, a2.zw);
              float2 t;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (this.ValidateCurve(curve3) && this.ExtendedIntersect(curve.xz, curve3.xz, width, width1, out t))
              {
                bounds1 |= t.x;
                ++num1;
              }
            }
            if (num1 == 1)
            {
              bounds1 |= float2.x;
              bounds1 |= float2.y;
            }
            if ((double) bounds1.max > (double) bounds1.min)
            {
              uint num2 = uint.MaxValue;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SlaveLaneData.HasComponent(laneOverlap2.m_Other))
              {
                // ISSUE: reference to a compiler-generated field
                num2 = this.m_SlaveLaneData[laneOverlap2.m_Other].m_Group;
                // ISSUE: reference to a compiler-generated field
                for (int index2 = length; index2 < laneBuffer.m_CutRanges.Length; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  SecondaryLaneSystem.CutRange cutRange = laneBuffer.m_CutRanges[index2];
                  // ISSUE: reference to a compiler-generated field
                  if ((int) cutRange.m_Group == (int) num2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    cutRange.m_Bounds |= bounds1;
                    // ISSUE: reference to a compiler-generated field
                    laneBuffer.m_CutRanges[index2] = cutRange;
                    goto label_39;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              laneBuffer.m_CutRanges.Add(new SecondaryLaneSystem.CutRange()
              {
                m_Bounds = bounds1,
                m_Group = num2
              });
            }
          }
        }
      }

      private bool ExtendedIntersect(
        Bezier4x2 curve1,
        Bezier4x2 curve2,
        float2 width1,
        float2 width2,
        out float2 t)
      {
        float2 float2 = math.max(new float2(width1.x, width2.x), new float2(width1.y, width2.y));
        if (MathUtils.Intersect(curve1, curve2, out t, 4))
          return true;
        if (MathUtils.Intersect(curve1, new Line2.Segment(curve2.a, curve2.a - math.normalizesafe(MathUtils.StartTangent(curve2)) * (float2.x * 0.5f)), out t, 4) && (double) t.y * (double) float2.x <= (double) math.lerp(width1.x, width1.y, t.x))
        {
          t.y = 0.0f;
          return true;
        }
        if (MathUtils.Intersect(curve2, new Line2.Segment(curve1.a, curve1.a - math.normalizesafe(MathUtils.StartTangent(curve1)) * (float2.y * 0.5f)), out t, 4) && (double) t.y * (double) float2.y <= (double) math.lerp(width2.x, width2.y, t.x))
        {
          t = new float2(0.0f, t.x);
          return true;
        }
        if (MathUtils.Intersect(curve1, new Line2.Segment(curve2.d, curve2.d + math.normalizesafe(MathUtils.EndTangent(curve2)) * (float2.x * 0.5f)), out t, 4) && (double) t.y * (double) float2.x <= (double) math.lerp(width1.x, width1.y, t.x))
        {
          t.y = 1f;
          return true;
        }
        if (!MathUtils.Intersect(curve2, new Line2.Segment(curve1.d, curve1.d + math.normalizesafe(MathUtils.EndTangent(curve1)) * (float2.y * 0.5f)), out t, 4) || (double) t.y * (double) float2.y > (double) math.lerp(width2.x, width2.y, t.x))
          return false;
        t = new float2(1f, t.x);
        return true;
      }

      private void ReplaceTempOwner(ref SecondaryLaneSystem.LaneKey laneKey, Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TempData.HasComponent(owner))
          return;
        // ISSUE: reference to a compiler-generated field
        Temp temp = this.m_TempData[owner];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!(temp.m_Original != Entity.Null) || this.m_EdgeData.HasComponent(temp.m_Original) && !this.m_EdgeData.HasComponent(owner))
          return;
        // ISSUE: reference to a compiler-generated method
        laneKey.ReplaceOwner(owner, temp.m_Original);
      }

      private void GetOriginalLane(
        SecondaryLaneSystem.LaneBuffer laneBuffer,
        SecondaryLaneSystem.LaneKey laneKey,
        ref Temp temp)
      {
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        if (!laneBuffer.m_OriginalLanes.TryGetValue(laneKey, out entity))
          return;
        temp.m_Original = entity;
        // ISSUE: reference to a compiler-generated field
        laneBuffer.m_OriginalLanes.Remove(laneKey);
      }

      private bool CheckRequirements(
        ref SecondaryLaneSystem.LaneBuffer laneBuffer,
        Entity lanePrefab)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LaneRequirements.HasBuffer(lanePrefab))
          return true;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ObjectRequirementElement> laneRequirement = this.m_LaneRequirements[lanePrefab];
        // ISSUE: reference to a compiler-generated field
        if (!laneBuffer.m_RequirementsSearched)
        {
          // ISSUE: reference to a compiler-generated field
          if (!laneBuffer.m_Requirements.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_Requirements = new NativeParallelHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_DefaultTheme != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            laneBuffer.m_Requirements.Add(this.m_DefaultTheme);
          }
          // ISSUE: reference to a compiler-generated field
          laneBuffer.m_RequirementsSearched = true;
        }
        int num = -1;
        bool flag = true;
        for (int index = 0; index < laneRequirement.Length; ++index)
        {
          ObjectRequirementElement requirementElement = laneRequirement[index];
          if ((int) requirementElement.m_Group != num)
          {
            if (flag)
            {
              num = (int) requirementElement.m_Group;
              flag = false;
            }
            else
              break;
          }
          // ISSUE: reference to a compiler-generated field
          flag |= laneBuffer.m_Requirements.Contains(requirementElement.m_Requirement);
        }
        return flag;
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLane> __Game_Net_TrackLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeLane> __Game_Net_NodeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HangingLane> __Game_Net_HangingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> __Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SecondaryLaneData> __Game_Prefabs_SecondaryLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> __Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SecondaryNetLane> __Game_Prefabs_SecondaryNetLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentLookup = state.GetComponentLookup<TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeLane_RO_ComponentLookup = state.GetComponentLookup<NodeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_HangingLane_RO_ComponentLookup = state.GetComponentLookup<HangingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup = state.GetComponentLookup<NetLaneArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SecondaryLaneData_RO_ComponentLookup = state.GetComponentLookup<SecondaryLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetLaneGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SecondaryNetLane_RO_BufferLookup = state.GetBufferLookup<SecondaryNetLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
      }
    }
  }
}
