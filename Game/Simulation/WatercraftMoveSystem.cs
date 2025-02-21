// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WatercraftMoveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
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
  public class WatercraftMoveSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_WatercraftQuery;
    private WatercraftMoveSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 8;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WatercraftQuery = this.GetEntityQuery(ComponentType.ReadOnly<Watercraft>(), ComponentType.ReadOnly<WatercraftNavigation>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Transform>(), ComponentType.ReadWrite<Moving>(), ComponentType.ReadWrite<TransformFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_WatercraftQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
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
      this.__TypeHandle.__Game_Prefabs_WatercraftData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WatercraftMoveSystem.UpdateTransformDataJob jobData = new WatercraftMoveSystem.UpdateTransformDataJob()
      {
        m_TransformFrameIndex = this.m_SimulationSystem.frameIndex / 16U % 4U,
        m_NavigationType = this.__TypeHandle.__Game_Vehicles_WatercraftNavigation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabWatercraftData = this.__TypeHandle.__Game_Prefabs_WatercraftData_RO_ComponentLookup,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<WatercraftMoveSystem.UpdateTransformDataJob>(this.m_WatercraftQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(this.Dependency);
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
    public WatercraftMoveSystem()
    {
    }

    [BurstCompile]
    public struct UpdateTransformDataJob : IJobChunk
    {
      [ReadOnly]
      public uint m_TransformFrameIndex;
      [ReadOnly]
      public ComponentTypeHandle<WatercraftNavigation> m_NavigationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<WatercraftData> m_PrefabWatercraftData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public ComponentTypeHandle<Moving> m_MovingType;
      public ComponentTypeHandle<Transform> m_TransformType;
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WatercraftNavigation> nativeArray2 = chunk.GetNativeArray<WatercraftNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray3 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        float num1 = 0.266666681f;
        for (int index = 0; index < chunk.Count; ++index)
        {
          PrefabRef prefabRef = nativeArray1[index];
          WatercraftNavigation watercraftNavigation = nativeArray2[index];
          Moving moving = nativeArray3[index];
          Transform transform = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData prefabGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          WatercraftData watercraftData = this.m_PrefabWatercraftData[prefabRef.m_Prefab];
          float3 pivot1;
          float3 pivot2;
          VehicleUtils.CalculateShipNavigationPivots(transform, prefabGeometryData, out pivot1, out pivot2);
          float3 float3 = pivot1 - pivot2;
          float num2 = math.length(float3.xz);
          float num3 = 1.5f * math.lerp(watercraftData.m_Turning.x, watercraftData.m_Turning.y, watercraftNavigation.m_MaxSpeed / watercraftData.m_MaxSpeed);
          float2 xz1 = watercraftNavigation.m_TargetDirection.xz;
          float2 float2_1;
          float2 float2_2;
          if (MathUtils.TryNormalize(ref xz1, num2 * 0.5f))
          {
            float2_1 = watercraftNavigation.m_TargetPosition.xz + xz1 - pivot1.xz;
            float2_2 = watercraftNavigation.m_TargetPosition.xz - xz1 - pivot2.xz;
          }
          else
          {
            float2_1 = watercraftNavigation.m_TargetPosition.xz - pivot1.xz;
            float2_2 = watercraftNavigation.m_TargetPosition.xz - pivot2.xz;
            xz1 = moving.m_Velocity.xz;
            num3 = math.min(num3, watercraftNavigation.m_MaxSpeed / (num2 * 0.5f));
          }
          float2 float2_3 = float2_1 + float2_2;
          MathUtils.TryNormalize(ref float2_3, watercraftNavigation.m_MaxSpeed);
          float2 float2_4 = float2_3 * 8f + moving.m_Velocity.xz;
          MathUtils.TryNormalize(ref float2_4, watercraftNavigation.m_MaxSpeed);
          moving.m_Velocity.xz = float2_4;
          float num4 = 0.0f;
          float2 xz2 = float3.xz;
          if (MathUtils.TryNormalize(ref xz2) && MathUtils.TryNormalize(ref xz1))
          {
            num4 = math.min(math.acos(math.saturate(math.dot(xz2, xz1))) * watercraftData.m_AngularAcceleration, num3);
            if ((double) math.dot(MathUtils.Left(xz2), xz1) > 0.0)
              num4 = -num4;
          }
          float num5 = math.clamp(num4 - moving.m_AngularVelocity.y, -watercraftData.m_AngularAcceleration * num1, watercraftData.m_AngularAcceleration * num1);
          moving.m_AngularVelocity.y += num5;
          quaternion a = quaternion.LookRotationSafe(new float3(xz2.x, 0.0f, xz2.y), math.up());
          transform.m_Rotation = math.mul(a, quaternion.RotateY(moving.m_AngularVelocity.y * num1));
          float3 position = transform.m_Position + moving.m_Velocity * num1;
          // ISSUE: reference to a compiler-generated method
          this.SampleWater(ref position, ref transform.m_Rotation, prefabGeometryData);
          TransformFrame transformFrame = new TransformFrame();
          transformFrame.m_Position = math.lerp(transform.m_Position, position, 0.5f);
          transformFrame.m_Velocity = moving.m_Velocity;
          transformFrame.m_Rotation = transform.m_Rotation;
          transform.m_Position = position;
          // ISSUE: reference to a compiler-generated field
          bufferAccessor[index][(int) this.m_TransformFrameIndex] = transformFrame;
          nativeArray3[index] = moving;
          nativeArray4[index] = transform;
        }
      }

      private void SampleWater(
        ref float3 position,
        ref quaternion rotation,
        ObjectGeometryData prefabGeometryData)
      {
        float2 float2 = prefabGeometryData.m_Size.xz * 0.4f;
        float3 float3_1 = position + math.rotate(rotation, new float3(-float2.x, 0.0f, -float2.y));
        float3 float3_2 = position + math.rotate(rotation, new float3(float2.x, 0.0f, -float2.y));
        float3 float3_3 = position + math.rotate(rotation, new float3(-float2.x, 0.0f, float2.y));
        float3 float3_4 = position + math.rotate(rotation, new float3(float2.x, 0.0f, float2.y));
        float4 float4_1;
        float4 x;
        float4 float4_2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, float3_1, out float4_1.x, out x.x, out float4_2.x);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, float3_2, out float4_1.y, out x.y, out float4_2.y);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, float3_3, out float4_1.z, out x.z, out float4_2.z);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, float3_4, out float4_1.w, out x.w, out float4_2.w);
        float num = math.max(0.0f, prefabGeometryData.m_Bounds.min.y * -0.75f);
        float4 float4_3 = math.max(x, float4_1 + num);
        float3_1.y = float4_3.x;
        float3_2.y = float4_3.y;
        float3_3.y = float4_3.z;
        float3_4.y = float4_3.w;
        float3 float3_5 = math.lerp(float3_3, float3_4, 0.5f);
        float3 float3_6 = math.lerp(float3_1, float3_2, 0.5f);
        float3 float3_7 = math.lerp(float3_3, float3_1, 0.5f);
        float3 float3_8 = math.lerp(float3_4, float3_2, 0.5f);
        position.y = math.lerp(float3_5.y, float3_6.y, 0.5f);
        float3 float3_9 = math.normalizesafe(float3_5 - float3_6, new float3(0.0f, 0.0f, 1f));
        float3 float3_10 = float3_7;
        float3 y = float3_8 - float3_10;
        float3 up = math.normalizesafe(math.cross(float3_9, y), new float3(0.0f, 1f, 0.0f));
        rotation = quaternion.LookRotationSafe(float3_9, up);
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
      public ComponentTypeHandle<WatercraftNavigation> __Game_Vehicles_WatercraftNavigation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftData> __Game_Prefabs_WatercraftData_RO_ComponentLookup;
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RW_ComponentTypeHandle;
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WatercraftNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WatercraftData_RO_ComponentLookup = state.GetComponentLookup<WatercraftData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>();
      }
    }
  }
}
