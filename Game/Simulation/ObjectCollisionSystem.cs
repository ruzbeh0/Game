// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ObjectCollisionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ObjectCollisionSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_ObjectQuery;
    private EntityQuery m_ConfigQuery;
    private EntityArchetype m_EventImpactArchetype;
    private EntityArchetype m_DamageEventArchetype;
    private EntityArchetype m_DestroyEventArchetype;
    private ObjectCollisionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<UpdateFrame>(),
          ComponentType.ReadWrite<Transform>(),
          ComponentType.ReadWrite<Moving>(),
          ComponentType.ReadWrite<TransformFrame>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<OutOfControl>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Unspawned>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventImpactArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Impact>());
      // ISSUE: reference to a compiler-generated field
      this.m_DamageEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Damage>());
      // ISSUE: reference to a compiler-generated field
      this.m_DestroyEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Destroy>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ObjectQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex % 16U;
      // ISSUE: reference to a compiler-generated field
      uint num2 = this.m_SimulationSystem.frameIndex / 16U % 4U;
      NativeQueue<ObjectCollisionSystem.Collision> nativeQueue = new NativeQueue<ObjectCollisionSystem.Collision>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      FireConfigurationData singleton = this.m_ConfigQuery.GetSingleton<FireConfigurationData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: variable of a compiler-generated type
      ObjectCollisionSystem.FindCollisionsJob jobData1 = new ObjectCollisionSystem.FindCollisionsJob()
      {
        m_PreviousTransformFrameIndex = (num2 - 1U) % 4U,
        m_CurrentTransformFrameIndex = num2,
        m_UpdateFrameIndex = num1,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_CollisionQueue = nativeQueue.AsParallelWriter(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      ObjectCollisionSystem.ResolveCollisionsJob jobData2 = new ObjectCollisionSystem.ResolveCollisionsJob()
      {
        m_InvolvedInAccidentData = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup,
        m_EventImpactArchetype = this.m_EventImpactArchetype,
        m_DamageEventArchetype = this.m_DamageEventArchetype,
        m_DestroyEventArchetype = this.m_DestroyEventArchetype,
        m_StructuralIntegrityData = new EventHelpers.StructuralIntegrityData((SystemBase) this, singleton),
        m_CollisionQueue = nativeQueue,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<ObjectCollisionSystem.FindCollisionsJob>(this.m_ObjectQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle2 = jobData2.Schedule<ObjectCollisionSystem.ResolveCollisionsJob>(dependsOn);
      nativeQueue.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = jobHandle2;
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
    public ObjectCollisionSystem()
    {
    }

    private struct Collision : IComparable<ObjectCollisionSystem.Collision>
    {
      public quaternion m_Rotation1;
      public quaternion m_Rotation2;
      public float3 m_Position1;
      public float3 m_Position2;
      public float3 m_ImpactLocation;
      public Entity m_Entity1;
      public Entity m_Entity2;
      public float m_Time;
      public int m_CurrentIndex1;
      public int m_CurrentIndex2;

      public int CompareTo(ObjectCollisionSystem.Collision other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (int) math.sign(this.m_Time - other.m_Time);
      }
    }

    [BurstCompile]
    private struct FindCollisionsJob : IJobChunk
    {
      [ReadOnly]
      public uint m_PreviousTransformFrameIndex;
      [ReadOnly]
      public uint m_CurrentTransformFrameIndex;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      public NativeQueue<ObjectCollisionSystem.Collision>.ParallelWriter m_CollisionQueue;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        NativeList<Entity> nativeList = new NativeList<Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
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
        // ISSUE: variable of a compiler-generated type
        ObjectCollisionSystem.FindCollisionsJob.NetIterator iterator = new ObjectCollisionSystem.FindCollisionsJob.NetIterator()
        {
          m_ObjectList = nativeList,
          m_UnspawnedData = this.m_UnspawnedData,
          m_TransformData = this.m_TransformData,
          m_ControllerData = this.m_ControllerData,
          m_EdgeData = this.m_EdgeData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabGeometryData = this.m_PrefabGeometryData,
          m_TransformFrames = this.m_TransformFrames,
          m_ConnectedEdges = this.m_ConnectedEdges,
          m_SubLanes = this.m_SubLanes,
          m_LaneObjects = this.m_LaneObjects
        };
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          PrefabRef prefabRef = nativeArray2[index1];
          DynamicBuffer<TransformFrame> dynamicBuffer = bufferAccessor[index1];
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          TransformFrame previousFrame1 = dynamicBuffer[(int) this.m_PreviousTransformFrameIndex];
          // ISSUE: reference to a compiler-generated field
          TransformFrame currentFrame1 = dynamicBuffer[(int) this.m_CurrentTransformFrameIndex];
          float3 position = previousFrame1.m_Position;
          float3 float3 = math.mul(math.inverse(currentFrame1.m_Rotation), currentFrame1.m_Position - position);
          Bounds3 bounds = objectGeometryData.m_Bounds;
          Box3 box3_1 = new Box3(bounds, previousFrame1.m_Rotation);
          Box3 box3_2 = new Box3(bounds + float3, currentFrame1.m_Rotation);
          Bounds3 bounds3 = position + (MathUtils.Bounds(box3_1) | MathUtils.Bounds(box3_2));
          // ISSUE: reference to a compiler-generated field
          iterator.m_Bounds = bounds3;
          // ISSUE: reference to a compiler-generated field
          iterator.m_Ignore = entity1;
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<ObjectCollisionSystem.FindCollisionsJob.NetIterator>(ref iterator);
          for (int index2 = 0; index2 < nativeList.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated method
            this.TestCollision(position, previousFrame1, currentFrame1, box3_1, box3_2, entity1, nativeList[index2]);
          }
          nativeList.Clear();
        }
      }

      private void TestCollision(
        float3 origin,
        TransformFrame previousFrame1,
        TransformFrame currentFrame1,
        Box3 previousBox1,
        Box3 currentBox1,
        Entity entity1,
        Entity entity2)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity2];
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity2];
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformFrames.HasBuffer(entity2))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<TransformFrame> transformFrame1 = this.m_TransformFrames[entity2];
          // ISSUE: reference to a compiler-generated method
          int index1 = ObjectCollisionSystem.FindCollisionsJob.CurrentFrameIndex(transform, transformFrame1);
          int index2 = math.select(index1 - 1, 3, index1 == 0);
          TransformFrame transformFrame2 = transformFrame1[index2];
          TransformFrame transformFrame3 = transformFrame1[index1];
          quaternion q1 = math.inverse(transformFrame2.m_Rotation);
          quaternion q2 = math.inverse(transformFrame3.m_Rotation);
          float3 v = transformFrame2.m_Position - origin;
          float3 float3_1 = math.mul(q1, v);
          float3 float3_2 = math.mul(q2, transformFrame3.m_Position - origin);
          Box3 box1 = new Box3(objectGeometryData.m_Bounds + float3_1, transformFrame2.m_Rotation);
          Box3 box2 = new Box3(objectGeometryData.m_Bounds + float3_2, transformFrame3.m_Rotation);
          for (int index3 = 1; index3 <= 16; ++index3)
          {
            float num = (float) index3 * (1f / 16f);
            Box3 box3_1 = MathUtils.Lerp(previousBox1, currentBox1, num);
            Box3 box3_2 = MathUtils.Lerp(box1, box2, num);
            Bounds3 intersection1;
            Bounds3 intersection2;
            if (MathUtils.Intersect(MathUtils.Bounds(box3_1), MathUtils.Bounds(box3_2)) && MathUtils.Intersect(box3_1, box3_2, out intersection1, out intersection2))
            {
              float3 x = math.rotate(box3_1.rotation, MathUtils.Center(intersection1)) + origin;
              float3 y = math.rotate(box3_2.rotation, MathUtils.Center(intersection2)) + origin;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_CollisionQueue.Enqueue(new ObjectCollisionSystem.Collision()
              {
                m_Rotation1 = box3_1.rotation,
                m_Rotation2 = box3_2.rotation,
                m_Position1 = math.lerp(previousFrame1.m_Position, currentFrame1.m_Position, num),
                m_Position2 = math.lerp(transformFrame2.m_Position, transformFrame3.m_Position, num),
                m_ImpactLocation = math.lerp(x, y, 0.5f),
                m_Entity1 = entity1,
                m_Entity2 = entity2,
                m_Time = num,
                m_CurrentIndex1 = (int) this.m_CurrentTransformFrameIndex,
                m_CurrentIndex2 = index1
              });
              break;
            }
          }
        }
        else
        {
          float3 float3 = math.mul(math.inverse(transform.m_Rotation), transform.m_Position - origin);
          Box3 box3_3 = new Box3(objectGeometryData.m_Bounds + float3, transform.m_Rotation);
          Bounds3 bounds2 = MathUtils.Bounds(box3_3);
          for (int index = 1; index <= 16; ++index)
          {
            float num = (float) index * (1f / 16f);
            Box3 box3_4 = MathUtils.Lerp(previousBox1, currentBox1, num);
            Bounds3 intersection1;
            Bounds3 intersection2;
            if (MathUtils.Intersect(MathUtils.Bounds(box3_4), bounds2) && MathUtils.Intersect(box3_4, box3_3, out intersection1, out intersection2))
            {
              float3 x = math.rotate(box3_4.rotation, MathUtils.Center(intersection1)) + origin;
              float3 y = math.rotate(box3_3.rotation, MathUtils.Center(intersection2)) + origin;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_CollisionQueue.Enqueue(new ObjectCollisionSystem.Collision()
              {
                m_Rotation1 = box3_4.rotation,
                m_Rotation2 = box3_3.rotation,
                m_Position1 = math.lerp(previousFrame1.m_Position, currentFrame1.m_Position, num),
                m_Position2 = transform.m_Position,
                m_ImpactLocation = math.lerp(x, y, 0.5f),
                m_Entity1 = entity1,
                m_Entity2 = entity2,
                m_Time = num,
                m_CurrentIndex1 = (int) this.m_CurrentTransformFrameIndex,
                m_CurrentIndex2 = -1
              });
              break;
            }
          }
        }
      }

      private static int CurrentFrameIndex(
        Transform transform,
        DynamicBuffer<TransformFrame> transformFrames)
      {
        float num1 = float.MaxValue;
        int num2 = -1;
        for (int index = 0; index < transformFrames.Length; ++index)
        {
          float num3 = math.distancesq(transformFrames[index].m_Position, transform.m_Position);
          if ((double) num3 < (double) num1)
          {
            num1 = num3;
            num2 = index;
          }
        }
        return num2;
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

      private struct NetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public Entity m_Ignore;
        public NativeList<Entity> m_ObjectList;
        public ComponentLookup<Unspawned> m_UnspawnedData;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<Controller> m_ControllerData;
        public ComponentLookup<Edge> m_EdgeData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<ObjectGeometryData> m_PrefabGeometryData;
        public BufferLookup<TransformFrame> m_TransformFrames;
        public BufferLookup<ConnectedEdge> m_ConnectedEdges;
        public BufferLookup<Game.Net.SubLane> m_SubLanes;
        public BufferLookup<LaneObject> m_LaneObjects;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(this.m_Bounds, bounds.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_Bounds, bounds.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge = this.m_EdgeData[entity];
            // ISSUE: reference to a compiler-generated method
            this.CheckLanes(entity);
            // ISSUE: reference to a compiler-generated method
            this.CheckLanes(edge.m_Start);
            // ISSUE: reference to a compiler-generated method
            this.CheckLanes(edge.m_End);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_ConnectedEdges.HasBuffer(entity))
              return;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[entity];
            // ISSUE: reference to a compiler-generated method
            this.CheckLanes(entity);
            for (int index = 0; index < connectedEdge.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckLanes(connectedEdge[index].m_Edge);
            }
          }
        }

        private void CheckLanes(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SubLanes.HasBuffer(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[entity];
          for (int index1 = 0; index1 < subLane1.Length; ++index1)
          {
            Entity subLane2 = subLane1[index1].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneObjects.HasBuffer(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<LaneObject> laneObject = this.m_LaneObjects[subLane2];
              for (int index2 = 0; index2 < laneObject.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckObject(laneObject[index2].m_LaneObject);
              }
            }
          }
        }

        private void CheckObject(Entity entity)
        {
          Controller componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (entity == this.m_Ignore || this.m_ControllerData.TryGetComponent(entity, out componentData) && componentData.m_Controller == this.m_Ignore || this.m_UnspawnedData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformFrames.HasBuffer(entity))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[entity];
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[entity];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<TransformFrame> transformFrame1 = this.m_TransformFrames[entity];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
            DynamicBuffer<TransformFrame> transformFrames = transformFrame1;
            // ISSUE: reference to a compiler-generated method
            int index1 = ObjectCollisionSystem.FindCollisionsJob.CurrentFrameIndex(transform, transformFrames);
            int index2 = math.select(index1 - 1, 3, index1 == 0);
            TransformFrame transformFrame2 = transformFrame1[index1];
            TransformFrame transformFrame3 = transformFrame1[index2];
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(this.m_Bounds, ObjectUtils.CalculateBounds(transformFrame2.m_Position, transformFrame2.m_Rotation, geometryData) | ObjectUtils.CalculateBounds(transformFrame3.m_Position, transformFrame3.m_Rotation, geometryData)))
              return;
            // ISSUE: reference to a compiler-generated field
            CollectionUtils.TryAddUniqueValue<Entity>(this.m_ObjectList, entity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[entity];
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[entity];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(this.m_Bounds, ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData)))
              return;
            // ISSUE: reference to a compiler-generated field
            CollectionUtils.TryAddUniqueValue<Entity>(this.m_ObjectList, entity);
          }
        }
      }
    }

    [BurstCompile]
    private struct ResolveCollisionsJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> m_InvolvedInAccidentData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabGeometryData;
      public ComponentLookup<Transform> m_TransformData;
      public ComponentLookup<Moving> m_MovingData;
      public ComponentLookup<Damaged> m_DamagedData;
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public EntityArchetype m_EventImpactArchetype;
      [ReadOnly]
      public EntityArchetype m_DamageEventArchetype;
      [ReadOnly]
      public EntityArchetype m_DestroyEventArchetype;
      [ReadOnly]
      public EventHelpers.StructuralIntegrityData m_StructuralIntegrityData;
      public NativeQueue<ObjectCollisionSystem.Collision> m_CollisionQueue;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CollisionQueue.Count == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<ObjectCollisionSystem.Collision> array = CollectionUtils.ToArray<ObjectCollisionSystem.Collision>(this.m_CollisionQueue, Allocator.Temp);
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(array.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        array.Sort<ObjectCollisionSystem.Collision>();
        for (int index = 0; index < array.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          ObjectCollisionSystem.Collision collision = array[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!nativeParallelHashSet.Contains(collision.m_Entity1) && !nativeParallelHashSet.Contains(collision.m_Entity2))
          {
            Moving componentData1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool component1 = this.m_MovingData.TryGetComponent(collision.m_Entity1, out componentData1);
            Moving componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool component2 = this.m_MovingData.TryGetComponent(collision.m_Entity2, out componentData2);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef1 = this.m_PrefabRefData[collision.m_Entity1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef2 = this.m_PrefabRefData[collision.m_Entity2];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData1 = this.m_PrefabGeometryData[prefabRef1.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData2 = this.m_PrefabGeometryData[prefabRef2.m_Prefab];
            InvolvedInAccident componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_InvolvedInAccidentData.TryGetComponent(collision.m_Entity1, out componentData3);
            InvolvedInAccident componentData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_InvolvedInAccidentData.TryGetComponent(collision.m_Entity2, out componentData4);
            if (component1 && component2 || !(componentData3.m_Event != Entity.Null) || !(componentData3.m_Event == componentData4.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              float3 momentOfInertia1 = ObjectUtils.CalculateMomentOfInertia(collision.m_Rotation1, objectGeometryData1.m_Size);
              // ISSUE: reference to a compiler-generated field
              float3 momentOfInertia2 = ObjectUtils.CalculateMomentOfInertia(collision.m_Rotation2, objectGeometryData2.m_Size);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 pointVelocity1 = ObjectUtils.CalculatePointVelocity(collision.m_ImpactLocation - collision.m_Position1, componentData1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 pointVelocity2 = ObjectUtils.CalculatePointVelocity(collision.m_ImpactLocation - collision.m_Position2, componentData2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_VehicleData.HasComponent(collision.m_Entity2))
              {
                float2 x1;
                x1.x = objectGeometryData1.m_Size.x * objectGeometryData1.m_Size.y * objectGeometryData1.m_Size.z;
                x1.y = objectGeometryData2.m_Size.x * objectGeometryData2.m_Size.y * objectGeometryData2.m_Size.z;
                x1 = x1 / math.csum(x1) * 1.2f;
                float3 x2 = (pointVelocity2 - pointVelocity1) * x1.y;
                float3 float3 = (pointVelocity1 - pointVelocity2) * x1.x;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) math.dot(x2, collision.m_Position1 - collision.m_Position2) >= 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float3 velocityDelta1 = x2 + math.normalizesafe(collision.m_Position1 - collision.m_Position2) * (math.length(pointVelocity1) * 0.1f);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float3 velocityDelta2 = float3 + math.normalizesafe(collision.m_Position2 - collision.m_Position1) * (math.length(pointVelocity2) * 0.1f);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  float3 angularVelocityDelta1 = this.CalculatePointAngularVelocityDelta(collision.m_ImpactLocation, collision.m_Position1, velocityDelta1, momentOfInertia1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  float3 angularVelocityDelta2 = this.CalculatePointAngularVelocityDelta(collision.m_ImpactLocation, collision.m_Position2, velocityDelta2, momentOfInertia2);
                  // ISSUE: reference to a compiler-generated field
                  Impact component3 = new Impact()
                  {
                    m_Event = componentData4.m_Event != Entity.Null ? componentData4.m_Event : componentData3.m_Event,
                    m_Target = collision.m_Entity1,
                    m_Severity = 10f
                  };
                  // ISSUE: reference to a compiler-generated field
                  Impact component4 = new Impact()
                  {
                    m_Event = componentData3.m_Event != Entity.Null ? componentData3.m_Event : componentData4.m_Event,
                    m_Target = collision.m_Entity2,
                    m_Severity = 10f
                  };
                  if (component1)
                  {
                    componentData1.m_Velocity += velocityDelta1;
                    componentData1.m_AngularVelocity += angularVelocityDelta1;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_MovingData[collision.m_Entity1] = componentData1;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_TransformData[collision.m_Entity1] = new Transform(collision.m_Position1, collision.m_Rotation1);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_TransformFrames[collision.m_Entity1][collision.m_CurrentIndex1] = new TransformFrame(collision.m_Position1, collision.m_Rotation1, componentData1.m_Velocity);
                  }
                  else
                  {
                    component3.m_VelocityDelta = velocityDelta1;
                    component3.m_AngularVelocityDelta = angularVelocityDelta1;
                  }
                  if (component2)
                  {
                    componentData2.m_Velocity += velocityDelta2;
                    componentData2.m_AngularVelocity += angularVelocityDelta2;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_MovingData[collision.m_Entity2] = componentData2;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_TransformData[collision.m_Entity2] = new Transform(collision.m_Position2, collision.m_Rotation2);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_TransformFrames[collision.m_Entity2][collision.m_CurrentIndex2] = new TransformFrame(collision.m_Position2, collision.m_Rotation2, componentData2.m_Velocity);
                  }
                  else
                  {
                    component4.m_VelocityDelta = velocityDelta2;
                    component4.m_AngularVelocityDelta = angularVelocityDelta2;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Entity entity1 = this.m_CommandBuffer.CreateEntity(this.m_EventImpactArchetype);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Entity entity2 = this.m_CommandBuffer.CreateEntity(this.m_EventImpactArchetype);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Impact>(entity1, component3);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Impact>(entity2, component4);
                  // ISSUE: reference to a compiler-generated field
                  nativeParallelHashSet.Add(collision.m_Entity1);
                  // ISSUE: reference to a compiler-generated field
                  nativeParallelHashSet.Add(collision.m_Entity2);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.AddDamage(collision.m_Entity1, component3.m_Event, prefabRef1, velocityDelta1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.AddDamage(collision.m_Entity2, component4.m_Event, prefabRef2, velocityDelta2);
                }
              }
              else
              {
                float3 x = (pointVelocity1 - pointVelocity2) * 0.5f;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) math.dot(x, collision.m_Position2 - collision.m_Position1) >= 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float3 velocityDelta = x + math.normalizesafe(collision.m_Position2 - collision.m_Position1) * (math.length(pointVelocity2) * 0.1f);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  float3 angularVelocityDelta = this.CalculatePointAngularVelocityDelta(collision.m_ImpactLocation, collision.m_Position2, velocityDelta, momentOfInertia2);
                  // ISSUE: reference to a compiler-generated field
                  Impact component5 = new Impact()
                  {
                    m_Event = componentData3.m_Event != Entity.Null ? componentData3.m_Event : componentData4.m_Event,
                    m_Target = collision.m_Entity2,
                    m_Severity = 10f
                  };
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_MovingData.HasComponent(collision.m_Entity2))
                  {
                    componentData2.m_Velocity += velocityDelta;
                    componentData2.m_AngularVelocity += angularVelocityDelta;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_MovingData[collision.m_Entity2] = componentData2;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_TransformData[collision.m_Entity2] = new Transform(collision.m_Position2, collision.m_Rotation2);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_TransformFrames[collision.m_Entity2][collision.m_CurrentIndex2] = new TransformFrame(collision.m_Position2, collision.m_Rotation2, componentData2.m_Velocity);
                  }
                  else
                  {
                    component5.m_VelocityDelta = velocityDelta;
                    component5.m_AngularVelocityDelta = angularVelocityDelta;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Impact>(this.m_CommandBuffer.CreateEntity(this.m_EventImpactArchetype), component5);
                  // ISSUE: reference to a compiler-generated field
                  nativeParallelHashSet.Add(collision.m_Entity2);
                }
              }
            }
          }
        }
      }

      private float3 CalculatePointAngularVelocityDelta(
        float3 position,
        float3 origin,
        float3 velocityDelta,
        float3 momentOfInertia)
      {
        return math.cross(position - origin, velocityDelta) / momentOfInertia;
      }

      private void AddDamage(
        Entity entity,
        Entity _event,
        PrefabRef prefabRef,
        float3 velocityDelta)
      {
        // ISSUE: reference to a compiler-generated field
        float structuralIntegrity = this.m_StructuralIntegrityData.GetStructuralIntegrity(prefabRef.m_Prefab, false);
        float x = math.min(1f, math.length(velocityDelta) * 100f / structuralIntegrity);
        if ((double) x < 0.0099999997764825821)
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_DamagedData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Damaged damaged = this.m_DamagedData[entity];
          damaged.m_Damage.x = math.min(1f, damaged.m_Damage.x + x);
          // ISSUE: reference to a compiler-generated field
          this.m_DamagedData[entity] = damaged;
          if ((double) ObjectUtils.GetTotalDamage(damaged) != 1.0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Destroy>(this.m_CommandBuffer.CreateEntity(this.m_DestroyEventArchetype), new Destroy(entity, _event));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Damage>(this.m_CommandBuffer.CreateEntity(this.m_DamageEventArchetype), new Damage(entity, new float3(x, 0.0f, 0.0f)));
          if ((double) x < 1.0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Destroy>(this.m_CommandBuffer.CreateEntity(this.m_DestroyEventArchetype), new Destroy(entity, _event));
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;
      public ComponentLookup<Moving> __Game_Objects_Moving_RW_ComponentLookup;
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RW_ComponentLookup;
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferLookup = state.GetBufferLookup<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentLookup = state.GetComponentLookup<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentLookup = state.GetComponentLookup<Moving>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RW_ComponentLookup = state.GetComponentLookup<Damaged>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferLookup = state.GetBufferLookup<TransformFrame>();
      }
    }
  }
}
