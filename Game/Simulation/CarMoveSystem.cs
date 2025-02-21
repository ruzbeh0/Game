// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CarMoveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
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
  public class CarMoveSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private LightingSystem m_LightingSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_VehicleQuery;
    private CarMoveSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LightingSystem = this.World.GetOrCreateSystemManaged<LightingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Car>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<TransformFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex & 15U;
      // ISSUE: reference to a compiler-generated field
      uint num = this.m_SimulationSystem.frameIndex >> 4 & 3U;
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new CarMoveSystem.CarMoveJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Vehicles_CarNavigation_RO_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle,
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_TransformFrameIndex = num,
        m_DayLightBrightness = this.m_LightingSystem.dayLightBrightness,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<CarMoveSystem.CarMoveJob>(this.m_VehicleQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
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
    public CarMoveSystem()
    {
    }

    [BurstCompile]
    private struct CarMoveJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Car> m_CarType;
      [ReadOnly]
      public ComponentTypeHandle<CarNavigation> m_NavigationType;
      [ReadOnly]
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Moving> m_MovingType;
      public ComponentTypeHandle<Transform> m_TransformType;
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public uint m_TransformFrameIndex;
      [ReadOnly]
      public float m_DayLightBrightness;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
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
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray3 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarNavigation> nativeArray4 = chunk.GetNativeArray<CarNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray5 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PseudoRandomSeed> nativeArray6 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray7 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray8 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        float num1 = 0.266666681f;
        float num2 = num1 * 0.5f;
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          PrefabRef prefabRef = nativeArray2[index1];
          Car car = nativeArray3[index1];
          CarNavigation carNavigation = nativeArray4[index1];
          CarCurrentLane carCurrentLane = nativeArray5[index1];
          PseudoRandomSeed pseudoRandomSeed = nativeArray6[index1];
          Moving moving = nativeArray7[index1];
          Transform transform = nativeArray8[index1];
          // ISSUE: reference to a compiler-generated field
          CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
          Random random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kLightState);
          float3 float3_1 = carNavigation.m_TargetPosition - transform.m_Position;
          bool c = math.asuint(carNavigation.m_MaxSpeed) >> 31 > 0U;
          bool flag = !carNavigation.m_TargetRotation.Equals(new quaternion());
          float num3 = math.abs(carNavigation.m_MaxSpeed);
          if ((carCurrentLane.m_LaneFlags & CarLaneFlags.Area) != (CarLaneFlags) 0)
          {
            if (!c)
            {
              float y = math.select(carData.m_PivotOffset, -carData.m_PivotOffset, c);
              float3_1.xz = MathUtils.ClampLength(float3_1.xz, math.max(1f, num3) + math.max(0.0f, y));
              carNavigation.m_TargetPosition.xz = transform.m_Position.xz + float3_1.xz;
            }
            // ISSUE: reference to a compiler-generated field
            carNavigation.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, carNavigation.m_TargetPosition);
            float3_1.y = carNavigation.m_TargetPosition.y - transform.m_Position.y;
            MathUtils.TryNormalize(ref float3_1, num3);
          }
          else
            MathUtils.TryNormalize(ref float3_1, num3);
          float3 x1;
          if ((carCurrentLane.m_LaneFlags & (CarLaneFlags.Connection | CarLaneFlags.ResetSpeed)) > (CarLaneFlags) 0 | flag)
          {
            x1 = float3_1;
          }
          else
          {
            float3 x2 = math.rotate(transform.m_Rotation, math.right());
            float3 x3 = math.rotate(transform.m_Rotation, math.up());
            float3 float3_2 = math.forward(transform.m_Rotation);
            float3 float3_3 = new float3(math.dot(x2, float3_1), math.dot(x3, float3_1), math.dot(float3_2, float3_1));
            float num4 = math.saturate(math.distance(carNavigation.m_TargetPosition, transform.m_Position) - 1f);
            if (c)
            {
              float num5 = math.select(math.smoothstep(-1f, -0.9f, math.dot(float3_2, math.normalizesafe(float3_1))), 1f, (carCurrentLane.m_LaneFlags & CarLaneFlags.CanReverse) == (CarLaneFlags) 0);
              float3 float3_4 = MathUtils.Normalize(float3_3, float3_3.xz);
              float3_3.x = 1.5f * num5 * num4 * float3_3.x;
              float3_3.z = num4 * (math.max(0.0f, float3_3.z) + 1f + math.max(1f, carData.m_PivotOffset * -4f));
              float3_3.y = float3_4.y * math.dot(float3_3.xz, float3_4.xz);
              float3_1 -= x2 * float3_3.x + x3 * float3_3.y + float3_2 * float3_3.z;
              if ((carCurrentLane.m_LaneFlags & CarLaneFlags.Area) != (CarLaneFlags) 0)
              {
                carNavigation.m_TargetPosition.xz = transform.m_Position.xz + float3_1.xz;
                // ISSUE: reference to a compiler-generated field
                carNavigation.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, carNavigation.m_TargetPosition);
                float3_1.y = carNavigation.m_TargetPosition.y - transform.m_Position.y;
              }
              x1 = float3_1 * 8f + moving.m_Velocity;
              if ((double) math.dot(float3_1, float3_2) < 0.0)
                float3_1 = -float3_1;
              carNavigation.m_TargetPosition = transform.m_Position + float3_1;
            }
            else
            {
              double num6 = (double) math.acos(math.normalizesafe(float3_3.xz).y);
              float x4 = num3 * num1 / math.max(1f, -carData.m_PivotOffset);
              double num7 = (double) x4;
              if (num6 > num7)
              {
                float3 float3_5 = MathUtils.Normalize(float3_3, float3_3.xz);
                float3_3.xz = new float2(math.sin(x4 * math.sign(float3_3.x)), math.cos(x4)) - float3_3.xz;
                float3_3.xz *= num4;
                float3_3.y = float3_5.y * math.dot(float3_3.xz, float3_5.xz);
                float3_1 += x2 * float3_3.x + x3 * float3_3.y + float3_2 * float3_3.z;
                if ((carCurrentLane.m_LaneFlags & CarLaneFlags.Area) != (CarLaneFlags) 0)
                {
                  carNavigation.m_TargetPosition.xz = transform.m_Position.xz + float3_1.xz;
                  // ISSUE: reference to a compiler-generated field
                  carNavigation.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, carNavigation.m_TargetPosition);
                  float3_1.y = carNavigation.m_TargetPosition.y - transform.m_Position.y;
                }
              }
              x1 = float3_1 * 8f + moving.m_Velocity;
              carNavigation.m_TargetPosition = transform.m_Position + float3_1;
            }
          }
          MathUtils.TryNormalize(ref x1, num3);
          float num8 = math.length(moving.m_Velocity);
          float num9 = math.length(x1);
          moving.m_Velocity = x1;
          float3 float3_6 = moving.m_Velocity * num2;
          float3 float3_7 = transform.m_Position + float3_6;
          quaternion q = transform.m_Rotation;
          quaternion quaternion = transform.m_Rotation;
          if (flag)
          {
            q = carNavigation.m_TargetRotation;
            quaternion = q;
          }
          else if ((double) num9 > 0.0099999997764825821)
          {
            float z = math.min(0.0f, carData.m_PivotOffset);
            float3 float3_8 = transform.m_Position + math.rotate(transform.m_Rotation, new float3(0.0f, 0.0f, z));
            float3 forward = carNavigation.m_TargetPosition - float3_8;
            if (MathUtils.TryNormalize(ref forward))
            {
              q = quaternion.LookRotationSafe(forward, math.up());
              quaternion = q;
            }
            float3 float3_9 = float3_7 + math.rotate(q, new float3(0.0f, 0.0f, z));
            forward = carNavigation.m_TargetPosition - float3_9;
            if (MathUtils.TryNormalize(ref forward))
              quaternion = quaternion.LookRotationSafe(forward, math.up());
          }
          TransformFrame transformFrame = new TransformFrame();
          transformFrame.m_Position = float3_7;
          transformFrame.m_Velocity = moving.m_Velocity;
          transformFrame.m_Rotation = q;
          transformFrame.m_Flags |= TransformFlags.RearLights;
          // ISSUE: reference to a compiler-generated field
          float num10 = this.m_DayLightBrightness + random.NextFloat(-0.05f, 0.05f);
          if ((double) num10 < 0.5)
          {
            if ((double) num10 < 0.25 && (carCurrentLane.m_LaneFlags & CarLaneFlags.HighBeams) != (CarLaneFlags) 0)
              transformFrame.m_Flags |= TransformFlags.ExtraLights;
            else
              transformFrame.m_Flags |= TransformFlags.MainLights;
          }
          if ((double) num9 <= (double) num8 * (1.0 - (double) num1 / (1.0 + (double) num8)) + 0.10000000149011612 * (double) num1)
            transformFrame.m_Flags |= TransformFlags.Braking;
          if (c)
          {
            transformFrame.m_Flags |= TransformFlags.Reversing;
          }
          else
          {
            if ((carCurrentLane.m_LaneFlags & CarLaneFlags.TurnLeft) != (CarLaneFlags) 0)
              transformFrame.m_Flags |= TransformFlags.TurningLeft;
            if ((carCurrentLane.m_LaneFlags & CarLaneFlags.TurnRight) != (CarLaneFlags) 0)
              transformFrame.m_Flags |= TransformFlags.TurningRight;
          }
          if ((car.m_Flags & (CarFlags.Emergency | CarFlags.Warning)) != (CarFlags) 0)
            transformFrame.m_Flags |= TransformFlags.WarningLights;
          if ((car.m_Flags & (CarFlags.Sign | CarFlags.Working)) != (CarFlags) 0)
            transformFrame.m_Flags |= TransformFlags.WorkLights;
          if ((car.m_Flags & CarFlags.Interior) != (CarFlags) 0)
            transformFrame.m_Flags |= TransformFlags.InteriorLights;
          if ((car.m_Flags & (CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2)) != (CarFlags) 0)
          {
            transformFrame.m_Flags |= (car.m_Flags & CarFlags.SignalAnimation1) != (CarFlags) 0 ? TransformFlags.SignalAnimation1 : (TransformFlags) 0;
            transformFrame.m_Flags |= (car.m_Flags & CarFlags.SignalAnimation2) != (CarFlags) 0 ? TransformFlags.SignalAnimation2 : (TransformFlags) 0;
          }
          transform.m_Position = float3_7 + float3_6;
          transform.m_Rotation = quaternion;
          DynamicBuffer<TransformFrame> dynamicBuffer = bufferAccessor[index1];
          // ISSUE: reference to a compiler-generated field
          if (((dynamicBuffer[(int) this.m_TransformFrameIndex].m_Flags ^ transformFrame.m_Flags) & (TransformFlags.MainLights | TransformFlags.ExtraLights | TransformFlags.WarningLights | TransformFlags.WorkLights)) != (TransformFlags) 0)
          {
            TransformFlags transformFlags1 = (TransformFlags) 0;
            TransformFlags transformFlags2 = (TransformFlags) 0;
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              TransformFlags flags = dynamicBuffer[index2].m_Flags;
              transformFlags1 |= flags;
              // ISSUE: reference to a compiler-generated field
              transformFlags2 |= index2 == (int) this.m_TransformFrameIndex ? transformFrame.m_Flags : flags;
            }
            if (((transformFlags1 ^ transformFlags2) & (TransformFlags.MainLights | TransformFlags.ExtraLights | TransformFlags.WarningLights | TransformFlags.WorkLights)) != (TransformFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<EffectsUpdated>(unfilteredChunkIndex, nativeArray1[index1], new EffectsUpdated());
            }
          }
          // ISSUE: reference to a compiler-generated field
          dynamicBuffer[(int) this.m_TransformFrameIndex] = transformFrame;
          nativeArray7[index1] = moving;
          nativeArray8[index1] = transform;
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
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarNavigation> __Game_Vehicles_CarNavigation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RW_ComponentTypeHandle;
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
      }
    }
  }
}
