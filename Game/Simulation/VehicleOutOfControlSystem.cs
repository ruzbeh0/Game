// Decompiled with JetBrains decompiler
// Type: Game.Simulation.VehicleOutOfControlSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
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
namespace Game.Simulation
{
  [CompilerGenerated]
  public class VehicleOutOfControlSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private EntityQuery m_VehicleQuery;
    private VehicleOutOfControlSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<OutOfControl>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Transform>(), ComponentType.ReadWrite<Moving>(), ComponentType.ReadWrite<TransformFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex & 15U;
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new VehicleOutOfControlSystem.VehicleOutOfControlMoveJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_TransformFrameIndex = (this.m_SimulationSystem.frameIndex / 16U % 4U)
      }.ScheduleParallel<VehicleOutOfControlSystem.VehicleOutOfControlMoveJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
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
    public VehicleOutOfControlSystem()
    {
    }

    [BurstCompile]
    private struct VehicleOutOfControlMoveJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Moving> m_MovingType;
      public ComponentTypeHandle<Transform> m_TransformType;
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public uint m_TransformFrameIndex;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray2 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        int num1 = 4;
        float timeStep = 0.266666681f;
        float num2 = timeStep / (float) num1;
        float gravity = 10f;
        float num3 = math.pow(0.95f, num2);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          PrefabRef prefabRef = nativeArray1[index1];
          Moving moving = nativeArray2[index1];
          Transform transform = nativeArray3[index1];
          // ISSUE: reference to a compiler-generated field
          CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
          for (int index2 = 0; index2 < num1; ++index2)
          {
            float3 momentOfInertia = ObjectUtils.CalculateMomentOfInertia(transform.m_Rotation, objectGeometryData.m_Size);
            float3 forward = math.forward(transform.m_Rotation);
            float3 origin = transform.m_Position + math.mul(transform.m_Rotation, new float3(0.0f, objectGeometryData.m_Bounds.max.y * 0.25f, 0.0f));
            Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, objectGeometryData.m_Bounds);
            Quad3 cornerPositions2 = baseCorners + math.mul(transform.m_Rotation, new float3(0.0f, objectGeometryData.m_Bounds.max.y, 0.0f));
            float4 heights;
            float4 heights2;
            // ISSUE: reference to a compiler-generated method
            this.GetGroundHeight(baseCorners, cornerPositions2, moving.m_Velocity, timeStep, gravity, out heights, out heights2);
            Quad3 quad3_1;
            // ISSUE: reference to a compiler-generated method
            quad3_1.a = this.CalculatePointVelocityDelta(baseCorners.a, origin, moving, forward, carData.m_Braking, num2, heights.x, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_1.b = this.CalculatePointVelocityDelta(baseCorners.b, origin, moving, forward, carData.m_Braking, num2, heights.y, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_1.c = this.CalculatePointVelocityDelta(baseCorners.c, origin, moving, forward, carData.m_Braking, num2, heights.z, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_1.d = this.CalculatePointVelocityDelta(baseCorners.d, origin, moving, forward, carData.m_Braking, num2, heights.w, gravity);
            Quad3 quad3_2;
            // ISSUE: reference to a compiler-generated method
            quad3_2.a = this.CalculatePointVelocityDelta(cornerPositions2.a, origin, moving, carData.m_Braking, num2, heights2.x, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_2.b = this.CalculatePointVelocityDelta(cornerPositions2.b, origin, moving, carData.m_Braking, num2, heights2.y, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_2.c = this.CalculatePointVelocityDelta(cornerPositions2.c, origin, moving, carData.m_Braking, num2, heights2.z, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_2.d = this.CalculatePointVelocityDelta(cornerPositions2.d, origin, moving, carData.m_Braking, num2, heights2.w, gravity);
            Quad3 quad3_3;
            // ISSUE: reference to a compiler-generated method
            quad3_3.a = this.CalculatePointAngularVelocityDelta(baseCorners.a, origin, quad3_1.a, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_3.b = this.CalculatePointAngularVelocityDelta(baseCorners.b, origin, quad3_1.b, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_3.c = this.CalculatePointAngularVelocityDelta(baseCorners.c, origin, quad3_1.c, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_3.d = this.CalculatePointAngularVelocityDelta(baseCorners.d, origin, quad3_1.d, momentOfInertia);
            Quad3 quad3_4;
            // ISSUE: reference to a compiler-generated method
            quad3_4.a = this.CalculatePointAngularVelocityDelta(cornerPositions2.a, origin, quad3_2.a, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_4.b = this.CalculatePointAngularVelocityDelta(cornerPositions2.b, origin, quad3_2.b, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_4.c = this.CalculatePointAngularVelocityDelta(cornerPositions2.c, origin, quad3_2.c, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_4.d = this.CalculatePointAngularVelocityDelta(cornerPositions2.d, origin, quad3_2.d, momentOfInertia);
            float3 float3_1 = (quad3_1.a + quad3_1.b + quad3_1.c + quad3_1.d + quad3_2.a + quad3_2.b + quad3_2.c + quad3_2.d) * 0.125f;
            float3 float3_2 = (quad3_3.a + quad3_3.b + quad3_3.c + quad3_3.d + quad3_4.a + quad3_4.b + quad3_4.c + quad3_4.d) * 0.125f;
            float3_1.y -= gravity * num2;
            moving.m_Velocity *= num3;
            moving.m_AngularVelocity *= num3;
            moving.m_Velocity += float3_1;
            moving.m_AngularVelocity += float3_2;
            float num4 = math.length(moving.m_AngularVelocity);
            if ((double) num4 > 9.9999997473787516E-06)
            {
              quaternion a = quaternion.AxisAngle(moving.m_AngularVelocity / num4, num4 * num2);
              transform.m_Rotation = math.normalize(math.mul(a, transform.m_Rotation));
              float3 float3_3 = transform.m_Position + math.mul(transform.m_Rotation, new float3(0.0f, objectGeometryData.m_Bounds.max.y * 0.25f, 0.0f));
              transform.m_Position += origin - float3_3;
            }
            transform.m_Position += moving.m_Velocity * num2;
          }
          // ISSUE: reference to a compiler-generated field
          bufferAccessor[index1][(int) this.m_TransformFrameIndex] = new TransformFrame()
          {
            m_Position = transform.m_Position - moving.m_Velocity * (timeStep * 0.5f),
            m_Velocity = moving.m_Velocity,
            m_Rotation = transform.m_Rotation
          };
          nativeArray2[index1] = moving;
          nativeArray3[index1] = transform;
        }
      }

      private float3 CalculatePointVelocityDelta(
        float3 position,
        float3 origin,
        Moving moving,
        float3 forward,
        float grip,
        float timeStep,
        float groundHeight,
        float gravity)
      {
        float3 pointVelocity = ObjectUtils.CalculatePointVelocity(position - origin, moving);
        float num1 = math.dot(forward, pointVelocity);
        float3 float3 = (pointVelocity - forward * (num1 * 0.5f)) with
        {
          y = 0.0f
        };
        float num2 = pointVelocity.y - gravity * timeStep;
        position.y += num2 * timeStep;
        float num3 = math.max(0.0f, groundHeight - position.y) / timeStep;
        float3 pointVelocityDelta = MathUtils.ClampLength(-float3, grip * math.min(timeStep, num3 / gravity));
        pointVelocityDelta.y += num3;
        return pointVelocityDelta;
      }

      private float3 CalculatePointVelocityDelta(
        float3 position,
        float3 origin,
        Moving moving,
        float grip,
        float timeStep,
        float groundHeight,
        float gravity)
      {
        float3 pointVelocity = ObjectUtils.CalculatePointVelocity(position - origin, moving);
        float3 float3 = (pointVelocity * 0.5f) with
        {
          y = 0.0f
        };
        float num1 = pointVelocity.y - gravity * timeStep;
        position.y += num1 * timeStep;
        float num2 = math.max(0.0f, groundHeight - position.y) / timeStep;
        float3 pointVelocityDelta = MathUtils.ClampLength(-float3, grip * math.min(timeStep, num2 / gravity));
        pointVelocityDelta.y += num2;
        return pointVelocityDelta;
      }

      private float3 CalculatePointAngularVelocityDelta(
        float3 position,
        float3 origin,
        float3 velocityDelta,
        float3 momentOfInertia)
      {
        return math.cross(position - origin, velocityDelta) / momentOfInertia;
      }

      private void GetGroundHeight(
        Quad3 cornerPositions,
        Quad3 cornerPositions2,
        float3 velocity,
        float timeStep,
        float gravity,
        out float4 heights,
        out float4 heights2)
      {
        float4 b1;
        // ISSUE: reference to a compiler-generated field
        b1.x = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions.a);
        // ISSUE: reference to a compiler-generated field
        b1.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions.b);
        // ISSUE: reference to a compiler-generated field
        b1.z = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions.c);
        // ISSUE: reference to a compiler-generated field
        b1.w = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions.d);
        float4 b2;
        // ISSUE: reference to a compiler-generated field
        b2.x = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions2.a);
        // ISSUE: reference to a compiler-generated field
        b2.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions2.b);
        // ISSUE: reference to a compiler-generated field
        b2.z = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions2.c);
        // ISSUE: reference to a compiler-generated field
        b2.w = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions2.d);
        float4 float4_1 = new float4(cornerPositions.a.y, cornerPositions.b.y, cornerPositions.c.y, cornerPositions.d.y);
        float4 float4_2 = new float4(cornerPositions2.a.y, cornerPositions2.b.y, cornerPositions2.c.y, cornerPositions2.d.y);
        heights = math.select((float4) float.MinValue, b1, b1 < float4_1 + 4f);
        heights2 = math.select((float4) float.MinValue, b2, b2 < float4_2 + 4f);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        VehicleOutOfControlSystem.VehicleOutOfControlMoveJob.NetIterator iterator = new VehicleOutOfControlSystem.VehicleOutOfControlMoveJob.NetIterator();
        // ISSUE: reference to a compiler-generated field
        iterator.m_Bounds = MathUtils.Bounds(cornerPositions) | MathUtils.Bounds(cornerPositions2);
        // ISSUE: reference to a compiler-generated field
        iterator.m_Bounds.min.y += (math.min(0.0f, velocity.y) - gravity * timeStep) * timeStep;
        // ISSUE: reference to a compiler-generated field
        iterator.m_Bounds.max.y += math.max(0.0f, velocity.y) * timeStep;
        // ISSUE: reference to a compiler-generated field
        iterator.m_CornerPositions = cornerPositions;
        // ISSUE: reference to a compiler-generated field
        iterator.m_CornerPositions2 = cornerPositions2;
        // ISSUE: reference to a compiler-generated field
        iterator.m_GroundHeights = heights;
        // ISSUE: reference to a compiler-generated field
        iterator.m_GroundHeights2 = heights2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_CompositionData = this.m_CompositionData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_OrphanData = this.m_OrphanData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_NodeData = this.m_NodeData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_NodeGeometryData = this.m_NodeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_EdgeGeometryData = this.m_EdgeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_StartNodeGeometryData = this.m_StartNodeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_EndNodeGeometryData = this.m_EndNodeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_PrefabCompositionData = this.m_PrefabCompositionData;
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<VehicleOutOfControlSystem.VehicleOutOfControlMoveJob.NetIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        heights = iterator.m_GroundHeights;
        // ISSUE: reference to a compiler-generated field
        heights2 = iterator.m_GroundHeights2;
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
        public Quad3 m_CornerPositions;
        public Quad3 m_CornerPositions2;
        public float4 m_GroundHeights;
        public float4 m_GroundHeights2;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<Orphan> m_OrphanData;
        public ComponentLookup<Game.Net.Node> m_NodeData;
        public ComponentLookup<NodeGeometry> m_NodeGeometryData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
        public ComponentLookup<NetCompositionData> m_PrefabCompositionData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompositionData.HasComponent(item))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckEdge(item);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OrphanData.HasComponent(item))
              return;
            // ISSUE: reference to a compiler-generated method
            this.CheckNode(item);
          }
        }

        private void CheckNode(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_NodeGeometryData[entity].m_Bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_NodeData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabCompositionData[this.m_OrphanData[entity].m_Composition];
          float3 position = node.m_Position;
          position.y += netCompositionData.m_SurfaceHeight.max;
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(position, netCompositionData.m_Width * 0.5f);
        }

        private void CheckEdge(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[entity];
          // ISSUE: reference to a compiler-generated field
          EdgeNodeGeometry geometry1 = this.m_StartNodeGeometryData[entity].m_Geometry;
          // ISSUE: reference to a compiler-generated field
          EdgeNodeGeometry geometry2 = this.m_EndNodeGeometryData[entity].m_Geometry;
          bool3 x;
          // ISSUE: reference to a compiler-generated field
          x.x = MathUtils.Intersect(edgeGeometry.m_Bounds, this.m_Bounds);
          // ISSUE: reference to a compiler-generated field
          x.y = MathUtils.Intersect(geometry1.m_Bounds, this.m_Bounds);
          // ISSUE: reference to a compiler-generated field
          x.z = MathUtils.Intersect(geometry2.m_Bounds, this.m_Bounds);
          if (!math.any(x))
            return;
          // ISSUE: reference to a compiler-generated field
          Composition composition = this.m_CompositionData[entity];
          if (x.x)
          {
            // ISSUE: reference to a compiler-generated field
            NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(edgeGeometry.m_Start, prefabCompositionData);
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(edgeGeometry.m_End, prefabCompositionData);
          }
          if (x.y)
          {
            // ISSUE: reference to a compiler-generated field
            NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_StartNode];
            if ((double) geometry1.m_MiddleRadius > 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(geometry1.m_Left, prefabCompositionData);
              Game.Net.Segment right1 = geometry1.m_Right;
              Game.Net.Segment right2 = geometry1.m_Right;
              right1.m_Right = MathUtils.Lerp(geometry1.m_Right.m_Left, geometry1.m_Right.m_Right, 0.5f);
              right2.m_Left = MathUtils.Lerp(geometry1.m_Right.m_Left, geometry1.m_Right.m_Right, 0.5f);
              right1.m_Right.d = geometry1.m_Middle.d;
              right2.m_Left.d = geometry1.m_Middle.d;
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(right1, prefabCompositionData);
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(right2, prefabCompositionData);
            }
            else
            {
              Game.Net.Segment left = geometry1.m_Left;
              Game.Net.Segment right = geometry1.m_Right;
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(left, prefabCompositionData);
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(right, prefabCompositionData);
              left.m_Right = geometry1.m_Middle;
              right.m_Left = geometry1.m_Middle;
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(left, prefabCompositionData);
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(right, prefabCompositionData);
            }
          }
          if (!x.z)
            return;
          // ISSUE: reference to a compiler-generated field
          NetCompositionData prefabCompositionData1 = this.m_PrefabCompositionData[composition.m_EndNode];
          if ((double) geometry2.m_MiddleRadius > 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(geometry2.m_Left, prefabCompositionData1);
            Game.Net.Segment right3 = geometry2.m_Right;
            Game.Net.Segment right4 = geometry2.m_Right;
            right3.m_Right = MathUtils.Lerp(geometry2.m_Right.m_Left, geometry2.m_Right.m_Right, 0.5f);
            right3.m_Right.d = geometry2.m_Middle.d;
            right4.m_Left = right3.m_Right;
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(right3, prefabCompositionData1);
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(right4, prefabCompositionData1);
          }
          else
          {
            Game.Net.Segment left = geometry2.m_Left;
            Game.Net.Segment right = geometry2.m_Right;
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(left, prefabCompositionData1);
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(right, prefabCompositionData1);
            left.m_Right = geometry2.m_Middle;
            right.m_Left = geometry2.m_Middle;
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(left, prefabCompositionData1);
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(right, prefabCompositionData1);
          }
        }

        private void CheckSegment(Game.Net.Segment segment, NetCompositionData prefabCompositionData)
        {
          float3 float3_1 = segment.m_Left.a;
          float3 float3_2 = segment.m_Right.a;
          float3_1.y += prefabCompositionData.m_SurfaceHeight.max;
          float3_2.y += prefabCompositionData.m_SurfaceHeight.max;
          Bounds3 bounds3_1 = MathUtils.Bounds(float3_1, float3_2);
          for (int index = 1; index <= 8; ++index)
          {
            float t = (float) index / 8f;
            float3 float3_3 = MathUtils.Position(segment.m_Left, t);
            float3 float3_4 = MathUtils.Position(segment.m_Right, t);
            float3_3.y += prefabCompositionData.m_SurfaceHeight.max;
            float3_4.y += prefabCompositionData.m_SurfaceHeight.max;
            Bounds3 bounds3_2 = MathUtils.Bounds(float3_3, float3_4);
            // ISSUE: reference to a compiler-generated field
            if (MathUtils.Intersect(bounds3_1 | bounds3_2, this.m_Bounds))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckTriangle(new Triangle3(float3_1, float3_2, float3_3));
              // ISSUE: reference to a compiler-generated method
              this.CheckTriangle(new Triangle3(float3_4, float3_3, float3_2));
            }
            float3_1 = float3_3;
            float3_2 = float3_4;
            bounds3_1 = bounds3_2;
          }
        }

        private void CheckCircle(float3 center, float radius)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions.a, ref this.m_GroundHeights.x);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions.b, ref this.m_GroundHeights.y);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions.c, ref this.m_GroundHeights.z);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions.d, ref this.m_GroundHeights.w);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions2.a, ref this.m_GroundHeights2.x);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions2.b, ref this.m_GroundHeights2.y);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions2.c, ref this.m_GroundHeights2.z);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions2.d, ref this.m_GroundHeights2.w);
        }

        private void CheckTriangle(Triangle3 triangle)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions.a, ref this.m_GroundHeights.x);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions.b, ref this.m_GroundHeights.y);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions.c, ref this.m_GroundHeights.z);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions.d, ref this.m_GroundHeights.w);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions2.a, ref this.m_GroundHeights2.x);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions2.b, ref this.m_GroundHeights2.y);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions2.c, ref this.m_GroundHeights2.z);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions2.d, ref this.m_GroundHeights2.w);
        }

        private void CheckCircle(
          float3 center,
          float radius,
          float3 position,
          ref float groundHeight)
        {
          if ((double) math.distance(center.xz, position.xz) > (double) radius)
            return;
          float y = center.y;
          groundHeight = math.select(groundHeight, y, (double) y < (double) position.y + 4.0 & (double) y > (double) groundHeight);
        }

        private void CheckTriangle(Triangle3 triangle, float3 position, ref float groundHeight)
        {
          float2 t;
          if (!MathUtils.Intersect(triangle.xz, position.xz, out t))
            return;
          float y = MathUtils.Position(triangle, t).y;
          groundHeight = math.select(groundHeight, y, (double) y < (double) position.y + 4.0 & (double) y > (double) groundHeight);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RW_ComponentTypeHandle;
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
      }
    }
  }
}
