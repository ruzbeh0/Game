// Decompiled with JetBrains decompiler
// Type: Game.Objects.LaneBlockSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class LaneBlockSystem : GameSystemBase
  {
    private Game.Net.SearchSystem m_NetSearchSystem;
    private EntityQuery m_ObjectQuery;
    private LaneObjectUpdater m_LaneObjectUpdater;
    private LaneBlockSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<BlockedLane>(),
          ComponentType.ReadOnly<Transform>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Vehicle>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LaneObjectUpdater = new LaneObjectUpdater((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ObjectQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_BlockedLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new LaneBlockSystem.FindBlockedLanesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_BlockedLaneType = this.__TypeHandle.__Game_Objects_BlockedLane_RW_BufferTypeHandle,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_LaneObjectBuffer = this.m_LaneObjectUpdater.Begin(Allocator.TempJob)
      }.ScheduleParallel<LaneBlockSystem.FindBlockedLanesJob>(this.m_ObjectQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_LaneObjectUpdater.Apply((SystemBase) this, jobHandle);
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
    public LaneBlockSystem()
    {
    }

    [BurstCompile]
    private struct FindBlockedLanesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      public BufferTypeHandle<BlockedLane> m_BlockedLaneType;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      public LaneObjectCommandBuffer m_LaneObjectBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<BlockedLane> bufferAccessor = chunk.GetBufferAccessor<BlockedLane>(ref this.m_BlockedLaneType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Created>(ref this.m_CreatedType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddBlockedLanes(nativeArray1[index], bufferAccessor[index], nativeArray2[index], nativeArray3[index].m_Prefab);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Deleted>(ref this.m_DeletedType))
          {
            for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
            {
              Entity entity = nativeArray1[index1];
              DynamicBuffer<BlockedLane> dynamicBuffer = bufferAccessor[index1];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_LaneObjectBuffer.Remove(dynamicBuffer[index2].m_Lane, entity);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
            {
              Entity entity = nativeArray1[index3];
              DynamicBuffer<BlockedLane> blockedLanes = bufferAccessor[index3];
              Transform transform = nativeArray4[index3];
              PrefabRef prefabRef = nativeArray5[index3];
              for (int index4 = 0; index4 < blockedLanes.Length; ++index4)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_LaneObjectBuffer.Remove(blockedLanes[index4].m_Lane, entity);
              }
              blockedLanes.Clear();
              // ISSUE: reference to a compiler-generated method
              this.AddBlockedLanes(entity, blockedLanes, transform, prefabRef.m_Prefab);
            }
          }
        }
      }

      private void AddBlockedLanes(
        Entity entity,
        DynamicBuffer<BlockedLane> blockedLanes,
        Transform transform,
        Entity prefab)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Bounds3 bounds = !this.m_PrefabGeometryData.HasComponent(prefab) ? new Bounds3() : this.m_PrefabGeometryData[prefab].m_Bounds;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LaneBlockSystem.FindBlockedLanesJob.FindBlockedLanesIterator iterator = new LaneBlockSystem.FindBlockedLanesJob.FindBlockedLanesIterator()
        {
          m_Bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, bounds),
          m_Entity = entity,
          m_Position = transform.m_Position,
          m_Radius = (float) (((double) bounds.max.x - (double) bounds.min.y) * 0.5),
          m_BlockedLanes = blockedLanes,
          m_LaneObjectBuffer = this.m_LaneObjectBuffer,
          m_SubLanes = this.m_SubLanes,
          m_MasterLaneData = this.m_MasterLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabLaneData = this.m_PrefabLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<LaneBlockSystem.FindBlockedLanesJob.FindBlockedLanesIterator>(ref iterator);
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

      private struct FindBlockedLanesIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public Entity m_Entity;
        public float3 m_Position;
        public float m_Radius;
        public DynamicBuffer<BlockedLane> m_BlockedLanes;
        public LaneObjectCommandBuffer m_LaneObjectBuffer;
        public BufferLookup<Game.Net.SubLane> m_SubLanes;
        public ComponentLookup<MasterLane> m_MasterLaneData;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<NetLaneData> m_PrefabLaneData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_SubLanes.HasBuffer(edgeEntity))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[edgeEntity];
          for (int index = 0; index < subLane1.Length; ++index)
          {
            Entity subLane2 = subLane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_MasterLaneData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              Entity prefab = this.m_PrefabRefData[subLane2].m_Prefab;
              // ISSUE: reference to a compiler-generated field
              Bezier4x3 bezier = this.m_CurveData[subLane2].m_Bezier;
              // ISSUE: reference to a compiler-generated field
              NetLaneData netLaneData = this.m_PrefabLaneData[prefab];
              // ISSUE: reference to a compiler-generated field
              float num1 = this.m_Radius + netLaneData.m_Width * 0.5f;
              // ISSUE: reference to a compiler-generated field
              if ((double) MathUtils.Distance(MathUtils.Bounds(bezier), this.m_Position) < (double) num1)
              {
                float t1;
                // ISSUE: reference to a compiler-generated field
                float num2 = MathUtils.Distance(bezier, this.m_Position, out t1);
                if ((double) num2 < (double) num1)
                {
                  float num3 = math.max(0.0f, num2 - netLaneData.m_Width * 0.5f);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float length = math.sqrt(math.max(0.0f, (float) ((double) this.m_Radius * (double) this.m_Radius - (double) num3 * (double) num3)));
                  Bounds1 t2 = new Bounds1(0.0f, t1);
                  Bounds1 t3 = new Bounds1(t1, 1f);
                  MathUtils.ClampLengthInverse(bezier, ref t2, length);
                  MathUtils.ClampLength(bezier, ref t3, length);
                  // ISSUE: reference to a compiler-generated field
                  this.m_BlockedLanes.Add(new BlockedLane(subLane2, new float2(t2.min, t3.max)));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_LaneObjectBuffer.Add(subLane2, this.m_Entity, new float2(t2.min, t3.max));
                }
              }
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      public BufferTypeHandle<BlockedLane> __Game_Objects_BlockedLane_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_BlockedLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<BlockedLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
      }
    }
  }
}
