// Decompiled with JetBrains decompiler
// Type: Game.Areas.AreaConnectionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [CompilerGenerated]
  public class AreaConnectionSystem : GameSystemBase
  {
    private ModificationBarrier4B m_ModificationBarrier;
    private EntityQuery m_ModificationQuery;
    private EntityQuery m_ConnectionQuery;
    private ComponentTypeSet m_AppliedTypes;
    private AreaConnectionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Area>(),
          ComponentType.ReadOnly<Game.Net.SubLane>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ConnectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<ConnectionLaneData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ModificationQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<Entity> entityListAsync = this.m_ConnectionQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CutRange_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EnclosedAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NavigationAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_AreaLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new AreaConnectionSystem.UpdateSecondaryLanesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AreaType = this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle,
        m_LotType = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentTypeHandle,
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_OverriddenData = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_SecondaryLaneData = this.__TypeHandle.__Game_Net_SecondaryLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_AreaLaneData = this.__TypeHandle.__Game_Net_AreaLane_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_TakeoffLocationData = this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentLookup,
        m_AccessLaneData = this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup,
        m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabNavigationAreaData = this.__TypeHandle.__Game_Prefabs_NavigationAreaData_RO_ComponentLookup,
        m_PrefabEnclosedAreaData = this.__TypeHandle.__Game_Prefabs_EnclosedAreaData_RO_ComponentLookup,
        m_PrefabNetLaneArchetypeData = this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_CutRanges = this.__TypeHandle.__Game_Net_CutRange_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_ConnectionPrefabs = entityListAsync,
        m_AppliedTypes = this.m_AppliedTypes,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<AreaConnectionSystem.UpdateSecondaryLanesJob>(this.m_ModificationQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      entityListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public AreaConnectionSystem()
    {
    }

    private enum LaneType
    {
      Road,
      Pedestrian,
      Border,
    }

    private struct AreaLaneKey : IEquatable<AreaConnectionSystem.AreaLaneKey>
    {
      private AreaConnectionSystem.LaneType m_LaneType;
      private float2 m_Position1;
      private float2 m_Position2;

      public AreaLaneKey(
        AreaConnectionSystem.LaneType laneType,
        float2 position1,
        float2 position2)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LaneType = laneType;
        bool2 bool2 = position1 < position2;
        bool flag = (double) position1.x == (double) position2.x;
        if (bool2.x | flag & bool2.y)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Position1 = position1;
          // ISSUE: reference to a compiler-generated field
          this.m_Position2 = position2;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Position1 = position2;
          // ISSUE: reference to a compiler-generated field
          this.m_Position2 = position1;
        }
      }

      public bool Equals(AreaConnectionSystem.AreaLaneKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_LaneType == other.m_LaneType && this.m_Position1.Equals(other.m_Position1) && this.m_Position2.Equals(other.m_Position2);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return ((17 * 31 + ((int) this.m_LaneType).GetHashCode()) * 31 + this.m_Position1.GetHashCode()) * 31 + this.m_Position2.GetHashCode();
      }
    }

    private struct AreaLaneValue
    {
      public Entity m_Lane;
      public float2 m_Heights;

      public AreaLaneValue(Entity lane, float a, float b)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Lane = lane;
        // ISSUE: reference to a compiler-generated field
        this.m_Heights = new float2(a, b);
      }
    }

    private struct TriangleSideKey : IEquatable<AreaConnectionSystem.TriangleSideKey>
    {
      private float3 m_Position1;
      private float3 m_Position2;

      public TriangleSideKey(float3 position1, float3 position2)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Position1 = position1;
        // ISSUE: reference to a compiler-generated field
        this.m_Position2 = position2;
      }

      public bool Equals(AreaConnectionSystem.TriangleSideKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Position1.Equals(other.m_Position1) && this.m_Position2.Equals(other.m_Position2);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (17 * 31 + this.m_Position1.GetHashCode()) * 31 + this.m_Position2.GetHashCode();
      }
    }

    [BurstCompile]
    private struct UpdateSecondaryLanesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Area> m_AreaType;
      [ReadOnly]
      public ComponentTypeHandle<Lot> m_LotType;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Overridden> m_OverriddenData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.SecondaryLane> m_SecondaryLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<AreaLane> m_AreaLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TakeoffLocation> m_TakeoffLocationData;
      [ReadOnly]
      public ComponentLookup<AccessLane> m_AccessLaneData;
      [ReadOnly]
      public ComponentLookup<RouteLane> m_RouteLaneData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public ComponentLookup<NavigationAreaData> m_PrefabNavigationAreaData;
      [ReadOnly]
      public ComponentLookup<EnclosedAreaData> m_PrefabEnclosedAreaData;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> m_PrefabNetLaneArchetypeData;
      [ReadOnly]
      public BufferLookup<Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<CutRange> m_CutRanges;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativeList<Entity> m_ConnectionPrefabs;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Area> nativeArray2 = chunk.GetNativeArray<Area>(ref this.m_AreaType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PseudoRandomSeed> nativeArray3 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
        // ISSUE: reference to a compiler-generated field
        bool isLot = chunk.Has<Lot>(ref this.m_LotType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Area area = nativeArray2[index];
          Temp temp = new Temp();
          Temp subTemp = new Temp();
          bool isTemp = false;
          // ISSUE: reference to a compiler-generated field
          bool isDeleted = this.m_DeletedData.HasComponent(entity);
          bool isCounterClockwise = (area.m_Flags & AreaFlags.CounterClockwise) != 0;
          PseudoRandomSeed pseudoRandomSeed;
          if (!CollectionUtils.TryGet<PseudoRandomSeed>(nativeArray3, index, out pseudoRandomSeed))
            pseudoRandomSeed = new PseudoRandomSeed((ushort) random.NextUInt(65536U));
          // ISSUE: reference to a compiler-generated field
          if (this.m_TempData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            temp = this.m_TempData[entity];
            subTemp.m_Flags = temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden);
            if ((temp.m_Flags & (TempFlags.Replace | TempFlags.Upgrade)) != (TempFlags) 0)
              subTemp.m_Flags |= TempFlags.Modify;
            isTemp = true;
          }
          NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> originalConnections;
          // ISSUE: reference to a compiler-generated method
          this.FindOriginalLanes(temp.m_Original, out originalConnections);
          // ISSUE: reference to a compiler-generated method
          this.UpdateLanes(unfilteredChunkIndex, entity, pseudoRandomSeed, isCounterClockwise, isLot, isDeleted, isTemp, subTemp, originalConnections);
          if (originalConnections.IsCreated)
            originalConnections.Dispose();
        }
      }

      private void GetLaneFlags(
        RouteConnectionType connectionType,
        RoadTypes areaRoadTypes,
        ref ConnectionLaneFlags laneFlags,
        ref RoadTypes roadTypes,
        ref int indexOffset)
      {
        if (connectionType != RouteConnectionType.Road)
        {
          if (connectionType != RouteConnectionType.Pedestrian)
            return;
          laneFlags |= ConnectionLaneFlags.Pedestrian;
          ++indexOffset;
        }
        else
        {
          laneFlags |= ConnectionLaneFlags.Road;
          roadTypes = areaRoadTypes;
          ++indexOffset;
        }
      }

      private void UpdateLanes(
        int jobIndex,
        Entity area,
        PseudoRandomSeed pseudoRandomSeed,
        bool isCounterClockwise,
        bool isLot,
        bool isDeleted,
        bool isTemp,
        Temp subTemp,
        NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> originalLanes)
      {
        NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> oldLanes = new NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue>();
        NativeParallelHashSet<Entity> updatedSet = new NativeParallelHashSet<Entity>();
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[area];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SecondaryLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[subLane2];
            if (!oldLanes.IsCreated)
              oldLanes = new NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue>(subLane1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            Game.Net.ConnectionLane componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            AreaConnectionSystem.LaneType laneType = !this.m_ConnectionLaneData.TryGetComponent(subLane2, out componentData) ? AreaConnectionSystem.LaneType.Border : ((componentData.m_Flags & ConnectionLaneFlags.Road) == (ConnectionLaneFlags) 0 ? AreaConnectionSystem.LaneType.Pedestrian : AreaConnectionSystem.LaneType.Road);
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: object of a compiler-generated type is created
            oldLanes.Add(new AreaConnectionSystem.AreaLaneKey(laneType, curve.m_Bezier.a.xz, curve.m_Bezier.d.xz), new AreaConnectionSystem.AreaLaneValue(subLane2, curve.m_Bezier.a.y, curve.m_Bezier.d.y));
          }
        }
        if (!isDeleted)
        {
          ConnectionLaneFlags laneFlags = (ConnectionLaneFlags) 0;
          RoadTypes roadTypes = RoadTypes.None;
          Entity borderLanePrefab = Entity.Null;
          int num1 = 0;
          int indexOffset = 0;
          bool c = false;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[area];
          Unity.Mathematics.Random random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kAreaBorder);
          NavigationAreaData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabNavigationAreaData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
          {
            // ISSUE: reference to a compiler-generated method
            this.GetLaneFlags(componentData1.m_ConnectionType, componentData1.m_RoadTypes, ref laneFlags, ref roadTypes, ref indexOffset);
            // ISSUE: reference to a compiler-generated method
            this.GetLaneFlags(componentData1.m_SecondaryType, componentData1.m_RoadTypes, ref laneFlags, ref roadTypes, ref indexOffset);
          }
          EnclosedAreaData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabEnclosedAreaData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          {
            borderLanePrefab = componentData2.m_BorderLanePrefab;
            c = componentData2.m_CounterClockWise != isCounterClockwise;
          }
          if (laneFlags != (ConnectionLaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Node> node = this.m_Nodes[area];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Triangle> triangle1 = this.m_Triangles[area];
            if (triangle1.Length == 1)
            {
              Triangle triangle2 = triangle1[0];
              // ISSUE: reference to a compiler-generated method
              float3 trianglePosition1 = this.GetTrianglePosition(node, triangle2);
              // ISSUE: reference to a compiler-generated method
              float3 trianglePosition2 = this.GetTrianglePosition(node, triangle2);
              float3 startPosition = trianglePosition1 + (node[triangle2.m_Indices.x].m_Position - trianglePosition1) * 0.5f;
              float3 endPosition = trianglePosition2 + ((node[triangle2.m_Indices.y].m_Position - trianglePosition2) * 0.25f + (node[triangle2.m_Indices.z].m_Position - trianglePosition2) * 0.25f);
              float3 middlePosition = (startPosition + endPosition) * 0.5f;
              int4 xyyz = triangle2.m_Indices.xyyz;
              // ISSUE: reference to a compiler-generated method
              this.CheckConnection(jobIndex, area, isTemp, subTemp, 0, 2 * indexOffset, indexOffset, startPosition, middlePosition, endPosition, xyyz, laneFlags, roadTypes, originalLanes, oldLanes, ref updatedSet);
              num1 = 3 * indexOffset;
            }
            else if (triangle1.Length >= 2)
            {
              NativeParallelHashMap<AreaConnectionSystem.TriangleSideKey, int2> nativeParallelHashMap = new NativeParallelHashMap<AreaConnectionSystem.TriangleSideKey, int2>(triangle1.Length * 3, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              for (int index = 0; index < triangle1.Length; ++index)
              {
                Triangle triangle3 = triangle1[index];
                // ISSUE: object of a compiler-generated type is created
                nativeParallelHashMap.TryAdd(new AreaConnectionSystem.TriangleSideKey(node[triangle3.m_Indices.y].m_Position, node[triangle3.m_Indices.z].m_Position), new int2(index, triangle3.m_Indices.x));
                // ISSUE: object of a compiler-generated type is created
                nativeParallelHashMap.TryAdd(new AreaConnectionSystem.TriangleSideKey(node[triangle3.m_Indices.z].m_Position, node[triangle3.m_Indices.x].m_Position), new int2(index, triangle3.m_Indices.y));
                // ISSUE: object of a compiler-generated type is created
                nativeParallelHashMap.TryAdd(new AreaConnectionSystem.TriangleSideKey(node[triangle3.m_Indices.x].m_Position, node[triangle3.m_Indices.y].m_Position), new int2(index, triangle3.m_Indices.z));
              }
              int middleIndex = triangle1.Length * indexOffset;
              for (int index = 0; index < triangle1.Length; ++index)
              {
                Triangle triangle4 = triangle1[index];
                int2 int2_1;
                // ISSUE: object of a compiler-generated type is created
                if (nativeParallelHashMap.TryGetValue(new AreaConnectionSystem.TriangleSideKey(node[triangle4.m_Indices.z].m_Position, node[triangle4.m_Indices.y].m_Position), out int2_1) && int2_1.x > index)
                {
                  // ISSUE: reference to a compiler-generated method
                  float3 trianglePosition3 = this.GetTrianglePosition(node, triangle4);
                  // ISSUE: reference to a compiler-generated method
                  float3 edgePosition = this.GetEdgePosition(node, triangle4.m_Indices.zy);
                  // ISSUE: reference to a compiler-generated method
                  float3 trianglePosition4 = this.GetTrianglePosition(node, triangle1[int2_1.x]);
                  int4 nodeIndex = new int4(math.select(triangle4.m_Indices.xyz, triangle4.m_Indices.xzy, isCounterClockwise), int2_1.y);
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, area, isTemp, subTemp, index * indexOffset, middleIndex, int2_1.x * indexOffset, trianglePosition3, edgePosition, trianglePosition4, nodeIndex, laneFlags, roadTypes, originalLanes, oldLanes, ref updatedSet);
                  middleIndex += indexOffset;
                }
                int2 int2_2;
                // ISSUE: object of a compiler-generated type is created
                if (nativeParallelHashMap.TryGetValue(new AreaConnectionSystem.TriangleSideKey(node[triangle4.m_Indices.x].m_Position, node[triangle4.m_Indices.z].m_Position), out int2_2) && int2_2.x > index)
                {
                  // ISSUE: reference to a compiler-generated method
                  float3 trianglePosition5 = this.GetTrianglePosition(node, triangle4);
                  // ISSUE: reference to a compiler-generated method
                  float3 edgePosition = this.GetEdgePosition(node, triangle4.m_Indices.xz);
                  // ISSUE: reference to a compiler-generated method
                  float3 trianglePosition6 = this.GetTrianglePosition(node, triangle1[int2_2.x]);
                  int4 nodeIndex = new int4(math.select(triangle4.m_Indices.yzx, triangle4.m_Indices.yxz, isCounterClockwise), int2_2.y);
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, area, isTemp, subTemp, index * indexOffset, middleIndex, int2_2.x * indexOffset, trianglePosition5, edgePosition, trianglePosition6, nodeIndex, laneFlags, roadTypes, originalLanes, oldLanes, ref updatedSet);
                  middleIndex += indexOffset;
                }
                int2 int2_3;
                // ISSUE: object of a compiler-generated type is created
                if (nativeParallelHashMap.TryGetValue(new AreaConnectionSystem.TriangleSideKey(node[triangle4.m_Indices.y].m_Position, node[triangle4.m_Indices.x].m_Position), out int2_3) && int2_3.x > index)
                {
                  // ISSUE: reference to a compiler-generated method
                  float3 trianglePosition7 = this.GetTrianglePosition(node, triangle4);
                  // ISSUE: reference to a compiler-generated method
                  float3 edgePosition = this.GetEdgePosition(node, triangle4.m_Indices.yx);
                  // ISSUE: reference to a compiler-generated method
                  float3 trianglePosition8 = this.GetTrianglePosition(node, triangle1[int2_3.x]);
                  int4 nodeIndex = new int4(math.select(triangle4.m_Indices.zxy, triangle4.m_Indices.zyx, isCounterClockwise), int2_3.y);
                  // ISSUE: reference to a compiler-generated method
                  this.CheckConnection(jobIndex, area, isTemp, subTemp, index * indexOffset, middleIndex, int2_3.x * indexOffset, trianglePosition7, edgePosition, trianglePosition8, nodeIndex, laneFlags, roadTypes, originalLanes, oldLanes, ref updatedSet);
                  middleIndex += indexOffset;
                }
              }
              nativeParallelHashMap.Dispose();
              num1 = middleIndex;
            }
          }
          if (borderLanePrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Node> node1 = this.m_Nodes[area];
            int num2 = num1 + node1.Length;
            int num3 = math.select(0, 1, isLot);
            PseudoRandomSeed pseudoRandomSeed1 = new PseudoRandomSeed((ushort) random.NextUInt(65536U));
            NativeParallelHashSet<AreaConnectionSystem.TriangleSideKey> nativeParallelHashSet = new NativeParallelHashSet<AreaConnectionSystem.TriangleSideKey>(node1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            for (int x = num3; x < node1.Length; ++x)
            {
              int2 a = new int2(x, x + 1);
              a.y = math.select(a.y, 0, a.y == node1.Length);
              a = math.select(a, a.yx, c);
              Node node2 = node1[a.x];
              Node node3 = node1[a.y];
              // ISSUE: object of a compiler-generated type is created
              nativeParallelHashSet.Add(new AreaConnectionSystem.TriangleSideKey(node2.m_Position, node3.m_Position));
            }
            for (int x = num3; x < node1.Length; ++x)
            {
              int2 a = new int2(x, x + 1);
              a.y = math.select(a.y, 0, a.y == node1.Length);
              a = math.select(a, a.yx, c);
              Node node4 = node1[a.x];
              Node node5 = node1[a.y];
              // ISSUE: object of a compiler-generated type is created
              if (!nativeParallelHashSet.Contains(new AreaConnectionSystem.TriangleSideKey(node5.m_Position, node4.m_Position)))
              {
                float3 middlePosition = (node4.m_Position + node5.m_Position) * 0.5f;
                a += num1;
                // ISSUE: reference to a compiler-generated method
                this.CheckBorder(jobIndex, area, borderLanePrefab, isTemp, subTemp, pseudoRandomSeed1, a.x, num2++, a.y, node4.m_Position, middlePosition, node5.m_Position, a.xxyy, originalLanes, oldLanes);
              }
            }
            nativeParallelHashSet.Dispose();
          }
        }
        if (oldLanes.IsCreated)
        {
          int capacity = oldLanes.Count();
          if (capacity != 0)
          {
            NativeArray<AreaConnectionSystem.AreaLaneValue> valueArray = oldLanes.GetValueArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
            for (int index = 0; index < valueArray.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              Entity lane = valueArray[index].m_Lane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, lane, in this.m_AppliedTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, lane);
              if (!updatedSet.IsCreated)
                updatedSet = new NativeParallelHashSet<Entity>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              updatedSet.Add(lane);
            }
            valueArray.Dispose();
          }
          oldLanes.Dispose();
        }
        if (!updatedSet.IsCreated)
          return;
        Entity entity = area;
        Owner componentData3;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity, out componentData3))
        {
          entity = componentData3.m_Owner;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_UpdatedData.HasComponent(entity) || this.m_DeletedData.HasComponent(entity))
          {
            entity = Entity.Null;
            break;
          }
        }
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.TryGetBuffer(entity, out bufferData))
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateConnections(jobIndex, updatedSet, bufferData);
        }
        updatedSet.Dispose();
      }

      private void UpdateConnections(
        int jobIndex,
        NativeParallelHashSet<Entity> updatedSet,
        DynamicBuffer<Game.Objects.SubObject> subObjects)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_UpdatedData.HasComponent(subObject) && !this.m_DeletedData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnLocationData.HasComponent(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Objects.SpawnLocation spawnLocation = this.m_SpawnLocationData[subObject];
              if (updatedSet.Contains(spawnLocation.m_ConnectedLane1) || updatedSet.Contains(spawnLocation.m_ConnectedLane2))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(jobIndex, subObject, new Updated());
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TakeoffLocationData.HasComponent(subObject))
              {
                // ISSUE: reference to a compiler-generated field
                AccessLane accessLane = this.m_AccessLaneData[subObject];
                // ISSUE: reference to a compiler-generated field
                RouteLane routeLane = this.m_RouteLaneData[subObject];
                if (updatedSet.Contains(accessLane.m_Lane) || updatedSet.Contains(routeLane.m_StartLane) || updatedSet.Contains(routeLane.m_EndLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(jobIndex, subObject, new Updated());
                }
              }
            }
            DynamicBuffer<Game.Objects.SubObject> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.TryGetBuffer(subObject, out bufferData))
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateConnections(jobIndex, updatedSet, bufferData);
            }
          }
        }
      }

      private float3 GetEdgePosition(DynamicBuffer<Node> nodes, int2 indices)
      {
        return (nodes[indices.x].m_Position + nodes[indices.y].m_Position) / 2f;
      }

      private float3 GetTrianglePosition(DynamicBuffer<Node> nodes, Triangle triangle)
      {
        return (nodes[triangle.m_Indices.x].m_Position + nodes[triangle.m_Indices.y].m_Position + nodes[triangle.m_Indices.z].m_Position) / 3f;
      }

      private void CheckConnection(
        int jobIndex,
        Entity area,
        bool isTemp,
        Temp temp,
        int startIndex,
        int middleIndex,
        int endIndex,
        float3 startPosition,
        float3 middlePosition,
        float3 endPosition,
        int4 nodeIndex,
        ConnectionLaneFlags laneFlags,
        RoadTypes roadTypes,
        NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> originalLanes,
        NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> oldLanes,
        ref NativeParallelHashSet<Entity> updatedSet)
      {
        Lane component1;
        component1.m_StartNode = new PathNode(area, (ushort) startIndex);
        component1.m_MiddleNode = new PathNode(area, (ushort) middleIndex);
        component1.m_EndNode = new PathNode(area, (ushort) endIndex);
        float3 float3_1 = middlePosition - startPosition;
        float3 float3_2 = endPosition - middlePosition;
        float3 startTangent = MathUtils.Normalize(float3_1, float3_1.xz);
        float3 endTangent = MathUtils.Normalize(float3_2, float3_2.xz);
        Curve component2;
        component2.m_Bezier = NetUtils.FitCurve(startPosition, startTangent, endTangent, endPosition);
        component2.m_Length = MathUtils.Length(component2.m_Bezier);
        AreaLane component3;
        component3.m_Nodes = nodeIndex;
        float2 heights = new float2(startPosition.y, endPosition.y);
        while (laneFlags != (ConnectionLaneFlags) 0)
        {
          ConnectionLaneFlags connectionLaneFlags;
          // ISSUE: variable of a compiler-generated type
          AreaConnectionSystem.AreaLaneKey laneKey;
          if ((laneFlags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0)
          {
            connectionLaneFlags = ConnectionLaneFlags.Road;
            laneFlags &= ~ConnectionLaneFlags.Road;
            // ISSUE: object of a compiler-generated type is created
            laneKey = new AreaConnectionSystem.AreaLaneKey(AreaConnectionSystem.LaneType.Road, startPosition.xz, endPosition.xz);
          }
          else
          {
            connectionLaneFlags = ConnectionLaneFlags.Pedestrian;
            laneFlags = (ConnectionLaneFlags) 0;
            // ISSUE: object of a compiler-generated type is created
            laneKey = new AreaConnectionSystem.AreaLaneKey(AreaConnectionSystem.LaneType.Pedestrian, startPosition.xz, endPosition.xz);
          }
          // ISSUE: reference to a compiler-generated method
          Entity entity1 = this.SelectOld(oldLanes, laneKey, heights);
          if (entity1 != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeletedData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, entity1);
            }
            // ISSUE: reference to a compiler-generated field
            Lane other1 = this.m_LaneData[entity1];
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[entity1];
            AreaLane areaLane = new AreaLane();
            // ISSUE: reference to a compiler-generated field
            bool flag = this.m_AreaLaneData.HasComponent(entity1);
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              areaLane = this.m_AreaLaneData[entity1];
            }
            Lane other2 = other1;
            CommonUtils.Swap<PathNode>(ref other2.m_StartNode, ref other2.m_EndNode);
            if ((!component1.Equals(other1) || !component2.m_Bezier.Equals(curve.m_Bezier) || !component3.m_Nodes.Equals(areaLane.m_Nodes)) && (!component1.Equals(other2) || !MathUtils.Invert(component2.m_Bezier).Equals(curve.m_Bezier) || !component3.m_Nodes.wzyx.Equals(areaLane.m_Nodes)))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity1, component1);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, component2);
              if (flag)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<AreaLane>(jobIndex, entity1, component3);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<AreaLane>(jobIndex, entity1, component3);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity1, new Updated());
              if (!updatedSet.IsCreated)
                updatedSet = new NativeParallelHashSet<Entity>(oldLanes.Count() + 1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              updatedSet.Add(entity1);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Entity connectionPrefab = this.m_ConnectionPrefabs[0];
            // ISSUE: reference to a compiler-generated field
            NetLaneArchetypeData laneArchetypeData = this.m_PrefabNetLaneArchetypeData[connectionPrefab];
            Owner component4 = new Owner(area);
            PrefabRef component5 = new PrefabRef(connectionPrefab);
            Game.Net.SecondaryLane component6 = new Game.Net.SecondaryLane();
            Game.Net.ConnectionLane component7 = new Game.Net.ConnectionLane();
            component7.m_Flags = connectionLaneFlags | ConnectionLaneFlags.AllowMiddle | ConnectionLaneFlags.Area;
            component7.m_RoadTypes = roadTypes;
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, laneArchetypeData.m_AreaLaneArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component5);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, component2);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<AreaLane>(jobIndex, entity2, component3);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Game.Net.ConnectionLane>(jobIndex, entity2, component7);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component4);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Game.Net.SecondaryLane>(jobIndex, entity2, component6);
            if (isTemp)
            {
              // ISSUE: reference to a compiler-generated method
              temp.m_Original = this.SelectOriginal(originalLanes, laneKey, heights);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity2, temp);
            }
          }
          component1.m_StartNode = new PathNode(area, (ushort) ++startIndex);
          component1.m_MiddleNode = new PathNode(area, (ushort) ++middleIndex);
          component1.m_EndNode = new PathNode(area, (ushort) ++endIndex);
          component2.m_Bezier.y += 0.5f;
          heights += 0.5f;
        }
      }

      private void CheckBorder(
        int jobIndex,
        Entity area,
        Entity lanePrefab,
        bool isTemp,
        Temp temp,
        PseudoRandomSeed pseudoRandomSeed,
        int startIndex,
        int middleIndex,
        int endIndex,
        float3 startPosition,
        float3 middlePosition,
        float3 endPosition,
        int4 nodeIndex,
        NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> originalLanes,
        NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> oldLanes)
      {
        Lane component1;
        component1.m_StartNode = new PathNode(area, (ushort) startIndex);
        component1.m_MiddleNode = new PathNode(area, (ushort) middleIndex);
        component1.m_EndNode = new PathNode(area, (ushort) endIndex);
        float3 float3_1 = middlePosition - startPosition;
        float3 float3_2 = endPosition - middlePosition;
        float3 startTangent = MathUtils.Normalize(float3_1, float3_1.xz);
        float3 endTangent = MathUtils.Normalize(float3_2, float3_2.xz);
        Curve component2;
        component2.m_Bezier = NetUtils.FitCurve(startPosition, startTangent, endTangent, endPosition);
        component2.m_Length = MathUtils.Length(component2.m_Bezier);
        AreaLane component3;
        component3.m_Nodes = nodeIndex;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AreaConnectionSystem.AreaLaneKey laneKey = new AreaConnectionSystem.AreaLaneKey(AreaConnectionSystem.LaneType.Border, startPosition.xz, endPosition.xz);
        float2 heights = new float2(startPosition.y, endPosition.y);
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = this.SelectOld(oldLanes, laneKey, heights);
        if (entity1 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_DeletedData.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, entity1);
          }
          // ISSUE: reference to a compiler-generated field
          Lane other1 = this.m_LaneData[entity1];
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[entity1];
          AreaLane areaLane = new AreaLane();
          // ISSUE: reference to a compiler-generated field
          bool flag = this.m_AreaLaneData.HasComponent(entity1);
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            areaLane = this.m_AreaLaneData[entity1];
          }
          Lane other2 = other1;
          CommonUtils.Swap<PathNode>(ref other2.m_StartNode, ref other2.m_EndNode);
          if (component1.Equals(other1) && component2.m_Bezier.Equals(curve.m_Bezier) && component3.m_Nodes.Equals(areaLane.m_Nodes) || component1.Equals(other2) && MathUtils.Invert(component2.m_Bezier).Equals(curve.m_Bezier) && component3.m_Nodes.wzyx.Equals(areaLane.m_Nodes))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity1, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity1, component2);
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<AreaLane>(jobIndex, entity1, component3);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<AreaLane>(jobIndex, entity1, component3);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity1, new Updated());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NetLaneArchetypeData laneArchetypeData = this.m_PrefabNetLaneArchetypeData[lanePrefab];
          Owner component4 = new Owner(area);
          PrefabRef component5 = new PrefabRef(lanePrefab);
          Game.Net.SecondaryLane component6 = new Game.Net.SecondaryLane();
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, laneArchetypeData.m_AreaLaneArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, component5);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Lane>(jobIndex, entity2, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Curve>(jobIndex, entity2, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<AreaLane>(jobIndex, entity2, component3);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, component4);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Game.Net.SecondaryLane>(jobIndex, entity2, component6);
          NetLaneData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabNetLaneData.TryGetComponent(lanePrefab, out componentData) && (componentData.m_Flags & LaneFlags.PseudoRandom) != (LaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, pseudoRandomSeed);
          }
          if (!isTemp)
            return;
          // ISSUE: reference to a compiler-generated method
          temp.m_Original = this.SelectOriginal(originalLanes, laneKey, heights);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Temp>(jobIndex, entity2, temp);
          if (!(temp.m_Original != Entity.Null))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OverriddenData.HasComponent(temp.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Overridden>(jobIndex, entity2, new Overridden());
          }
          DynamicBuffer<CutRange> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CutRanges.TryGetBuffer(temp.m_Original, out bufferData))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddBuffer<CutRange>(jobIndex, entity2).CopyFrom(bufferData);
        }
      }

      private Entity SelectOld(
        NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> oldLanes,
        AreaConnectionSystem.AreaLaneKey laneKey,
        float2 heights)
      {
        Entity lane = Entity.Null;
        // ISSUE: variable of a compiler-generated type
        AreaConnectionSystem.AreaLaneValue areaLaneValue;
        NativeParallelMultiHashMapIterator<AreaConnectionSystem.AreaLaneKey> it1;
        if (oldLanes.IsCreated && oldLanes.TryGetFirstValue(laneKey, out areaLaneValue, out it1))
        {
          NativeParallelMultiHashMapIterator<AreaConnectionSystem.AreaLaneKey> it2 = it1;
          float num1 = float.MaxValue;
          // ISSUE: reference to a compiler-generated field
          lane = areaLaneValue.m_Lane;
          do
          {
            // ISSUE: reference to a compiler-generated field
            float num2 = math.csum(math.abs(heights - areaLaneValue.m_Heights));
            if ((double) num2 < (double) num1)
            {
              it2 = it1;
              num1 = num2;
              // ISSUE: reference to a compiler-generated field
              lane = areaLaneValue.m_Lane;
            }
          }
          while (oldLanes.TryGetNextValue(out areaLaneValue, ref it1));
          oldLanes.Remove(it2);
        }
        return lane;
      }

      private Entity SelectOriginal(
        NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> originalLanes,
        AreaConnectionSystem.AreaLaneKey laneKey,
        float2 heights)
      {
        Entity lane = Entity.Null;
        // ISSUE: variable of a compiler-generated type
        AreaConnectionSystem.AreaLaneValue areaLaneValue;
        NativeParallelMultiHashMapIterator<AreaConnectionSystem.AreaLaneKey> it1;
        if (originalLanes.IsCreated && originalLanes.TryGetFirstValue(laneKey, out areaLaneValue, out it1))
        {
          NativeParallelMultiHashMapIterator<AreaConnectionSystem.AreaLaneKey> it2 = it1;
          float num1 = float.MaxValue;
          do
          {
            // ISSUE: reference to a compiler-generated field
            float num2 = math.csum(math.abs(heights - areaLaneValue.m_Heights));
            if ((double) num2 < (double) num1)
            {
              it2 = it1;
              num1 = num2;
              // ISSUE: reference to a compiler-generated field
              lane = areaLaneValue.m_Lane;
            }
          }
          while (originalLanes.TryGetNextValue(out areaLaneValue, ref it1));
          originalLanes.Remove(it2);
        }
        return lane;
      }

      private void FindOriginalLanes(
        Entity originalArea,
        out NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue> originalConnections)
      {
        originalConnections = new NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue>();
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.HasBuffer(originalArea))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[originalArea];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SecondaryLaneData.HasComponent(subLane2))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[subLane2];
            if (!originalConnections.IsCreated)
              originalConnections = new NativeParallelMultiHashMap<AreaConnectionSystem.AreaLaneKey, AreaConnectionSystem.AreaLaneValue>(subLane1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            Game.Net.ConnectionLane componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            AreaConnectionSystem.LaneType laneType = !this.m_ConnectionLaneData.TryGetComponent(subLane2, out componentData) ? AreaConnectionSystem.LaneType.Border : ((componentData.m_Flags & ConnectionLaneFlags.Road) == (ConnectionLaneFlags) 0 ? AreaConnectionSystem.LaneType.Pedestrian : AreaConnectionSystem.LaneType.Road);
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: object of a compiler-generated type is created
            originalConnections.Add(new AreaConnectionSystem.AreaLaneKey(laneType, curve.m_Bezier.a.xz, curve.m_Bezier.d.xz), new AreaConnectionSystem.AreaLaneValue(subLane2, curve.m_Bezier.a.y, curve.m_Bezier.d.y));
          }
        }
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
      public ComponentTypeHandle<Area> __Game_Areas_Area_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lot> __Game_Areas_Lot_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Overridden> __Game_Common_Overridden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.SecondaryLane> __Game_Net_SecondaryLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaLane> __Game_Net_AreaLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TakeoffLocation> __Game_Routes_TakeoffLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccessLane> __Game_Routes_AccessLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NavigationAreaData> __Game_Prefabs_NavigationAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EnclosedAreaData> __Game_Prefabs_EnclosedAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneArchetypeData> __Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CutRange> __Game_Net_CutRange_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentLookup = state.GetComponentLookup<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SecondaryLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.SecondaryLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_AreaLane_RO_ComponentLookup = state.GetComponentLookup<AreaLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TakeoffLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.TakeoffLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_AccessLane_RO_ComponentLookup = state.GetComponentLookup<AccessLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NavigationAreaData_RO_ComponentLookup = state.GetComponentLookup<NavigationAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EnclosedAreaData_RO_ComponentLookup = state.GetComponentLookup<EnclosedAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneArchetypeData_RO_ComponentLookup = state.GetComponentLookup<NetLaneArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CutRange_RO_BufferLookup = state.GetBufferLookup<CutRange>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
