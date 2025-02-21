// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AircraftMoveSystem
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
  public class AircraftMoveSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private LightingSystem m_LightingSystem;
    private EntityQuery m_AircraftQuery;
    private EndFrameBarrier m_EndFrameBarrier;
    private AircraftMoveSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 10;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LightingSystem = this.World.GetOrCreateSystemManaged<LightingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_AircraftQuery = this.GetEntityQuery(ComponentType.ReadOnly<Aircraft>(), ComponentType.ReadOnly<AircraftNavigation>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Transform>(), ComponentType.ReadWrite<Moving>(), ComponentType.ReadWrite<TransformFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AirplaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new AircraftMoveSystem.AircraftMoveJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AircraftType = this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentTypeHandle,
        m_HelicopterType = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Vehicles_AircraftNavigation_RO_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle,
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle,
        m_PrefabHelicopterData = this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup,
        m_PrefabAirplaneData = this.__TypeHandle.__Game_Prefabs_AirplaneData_RO_ComponentLookup,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle,
        m_TransformFrameIndex = (this.m_SimulationSystem.frameIndex / 16U % 4U),
        m_DayLightBrightness = this.m_LightingSystem.dayLightBrightness,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<AircraftMoveSystem.AircraftMoveJob>(this.m_AircraftQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
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
    public AircraftMoveSystem()
    {
    }

    [BurstCompile]
    private struct AircraftMoveJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Aircraft> m_AircraftType;
      [ReadOnly]
      public ComponentTypeHandle<Helicopter> m_HelicopterType;
      [ReadOnly]
      public ComponentTypeHandle<AircraftNavigation> m_NavigationType;
      [ReadOnly]
      public ComponentTypeHandle<AircraftCurrentLane> m_CurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Moving> m_MovingType;
      public ComponentTypeHandle<Transform> m_TransformType;
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      [ReadOnly]
      public uint m_TransformFrameIndex;
      [ReadOnly]
      public float m_DayLightBrightness;
      [ReadOnly]
      public ComponentLookup<HelicopterData> m_PrefabHelicopterData;
      [ReadOnly]
      public ComponentLookup<AirplaneData> m_PrefabAirplaneData;
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
        NativeArray<Aircraft> nativeArray3 = chunk.GetNativeArray<Aircraft>(ref this.m_AircraftType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AircraftNavigation> nativeArray4 = chunk.GetNativeArray<AircraftNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AircraftCurrentLane> nativeArray5 = chunk.GetNativeArray<AircraftCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PseudoRandomSeed> nativeArray6 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray7 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray8 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        float num1 = 0.266666681f;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Helicopter>(ref this.m_HelicopterType))
        {
          for (int index1 = 0; index1 < chunk.Count; ++index1)
          {
            PrefabRef prefabRef = nativeArray2[index1];
            Aircraft aircraft = nativeArray3[index1];
            AircraftNavigation aircraftNavigation = nativeArray4[index1];
            AircraftCurrentLane aircraftCurrentLane = nativeArray5[index1];
            PseudoRandomSeed pseudoRandomSeed = nativeArray6[index1];
            Moving moving = nativeArray7[index1];
            Transform transform = nativeArray8[index1];
            Random random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kLightState);
            // ISSUE: reference to a compiler-generated field
            HelicopterData helicopterData = this.m_PrefabHelicopterData[prefabRef.m_Prefab];
            TransformFrame transformFrame = new TransformFrame();
            if ((aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.Flying) != (AircraftLaneFlags) 0)
            {
              float3 x1 = aircraftNavigation.m_TargetPosition - transform.m_Position;
              float3 float3_1 = new float3();
              float3 defaultvalue = float3_1;
              float3 float3_2 = math.normalizesafe(x1, defaultvalue);
              float num2 = math.asin(math.saturate(float3_2.y));
              if ((double) aircraftNavigation.m_MinClimbAngle > (double) num2)
              {
                float3_2.y = math.sin(aircraftNavigation.m_MinClimbAngle);
                float3_2.xz = math.normalizesafe(float3_2.xz) * math.cos(aircraftNavigation.m_MinClimbAngle);
              }
              float3 float3_3 = MathUtils.ClampLength(float3_2 * aircraftNavigation.m_MaxSpeed - moving.m_Velocity, helicopterData.m_FlyingAcceleration * num1);
              moving.m_Velocity += float3_3;
              float3 float3_4 = new float3(0.0f, 0.0f, 1f);
              if ((double) math.lengthsq(moving.m_Velocity.xz) >= 1.0)
              {
                float3_4.xz = moving.m_Velocity.xz;
              }
              else
              {
                ref float3 local = ref float3_4;
                float3_1 = math.forward(transform.m_Rotation);
                float2 xz = float3_1.xz;
                local.xz = xz;
              }
              float3 x2 = moving.m_Velocity * helicopterData.m_VelocitySwayFactor * num1 + float3_3 * helicopterData.m_AccelerationSwayFactor;
              x2.y = math.max(x2.y, 0.0f) + 9.81f * num1;
              float3 float3_5 = math.normalize(x2);
              float3 y1 = math.cross(float3_4, float3_5);
              float3_4 = math.normalizesafe(math.cross(float3_5, y1), new float3(0.0f, 0.0f, 1f));
              quaternion b1 = quaternion.LookRotationSafe(float3_4, float3_5);
              float3 axis;
              float angle;
              MathUtils.AxisAngle(math.mul(math.inverse(transform.m_Rotation), b1), out axis, out angle);
              float3 float3_6 = math.clamp(axis * angle * helicopterData.m_FlyingAngularAcceleration - moving.m_AngularVelocity, (float3) (-helicopterData.m_FlyingAngularAcceleration * num1), (float3) (helicopterData.m_FlyingAngularAcceleration * num1));
              moving.m_AngularVelocity += float3_6;
              float num3 = math.length(moving.m_AngularVelocity);
              if ((double) num3 > 9.9999997473787516E-06)
              {
                quaternion b2 = quaternion.AxisAngle(moving.m_AngularVelocity / num3, num3 * num1);
                transform.m_Rotation = math.normalize(math.mul(transform.m_Rotation, b2));
              }
              float3 y2 = transform.m_Position + moving.m_Velocity * num1;
              transformFrame.m_Position = math.lerp(transform.m_Position, y2, 0.5f);
              transformFrame.m_Velocity = moving.m_Velocity;
              transformFrame.m_Rotation = transform.m_Rotation;
              // ISSUE: reference to a compiler-generated field
              float num4 = this.m_DayLightBrightness + random.NextFloat(-0.05f, 0.05f);
              transformFrame.m_Flags = TransformFlags.InteriorLights | TransformFlags.Flying;
              if ((aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.Landing) != (AircraftLaneFlags) 0)
              {
                transformFrame.m_Flags |= TransformFlags.WarningLights | TransformFlags.Landing;
                if ((double) num4 < 0.5)
                  transformFrame.m_Flags |= TransformFlags.ExtraLights;
              }
              else if ((aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.TakingOff) != (AircraftLaneFlags) 0)
                transformFrame.m_Flags |= TransformFlags.WarningLights | TransformFlags.TakingOff;
              if ((double) num4 < 0.5)
                transformFrame.m_Flags |= TransformFlags.MainLights;
              if ((aircraft.m_Flags & AircraftFlags.Working) != (AircraftFlags) 0)
                transformFrame.m_Flags |= TransformFlags.WorkLights;
              transform.m_Position = y2;
            }
            else
            {
              float3 float3_7 = aircraftNavigation.m_TargetPosition - transform.m_Position;
              MathUtils.TryNormalize(ref float3_7, aircraftNavigation.m_MaxSpeed);
              float3 float3_8 = float3_7 * 8f + moving.m_Velocity;
              MathUtils.TryNormalize(ref float3_8, aircraftNavigation.m_MaxSpeed);
              moving.m_Velocity = float3_8;
              float3 float3_9 = moving.m_Velocity * (num1 * 0.5f);
              float3 float3_10 = transform.m_Position + float3_9;
              float3 targetDirection = aircraftNavigation.m_TargetDirection;
              quaternion quaternion = transform.m_Rotation;
              if (MathUtils.TryNormalize(ref targetDirection))
              {
                quaternion = quaternion.LookRotationSafe(targetDirection, math.up());
              }
              else
              {
                float3 x = aircraftNavigation.m_TargetPosition - transform.m_Position;
                float num5 = math.length(x);
                if ((double) num5 >= 1.0)
                  quaternion = quaternion.LookRotationSafe(x / num5, math.up());
              }
              transform.m_Rotation = quaternion;
              moving.m_AngularVelocity = new float3();
              transformFrame.m_Position = float3_10;
              transformFrame.m_Velocity = moving.m_Velocity;
              transformFrame.m_Rotation = transform.m_Rotation;
              transformFrame.m_Flags = TransformFlags.InteriorLights | TransformFlags.WarningLights;
              transform.m_Position = float3_10 + float3_9;
            }
            DynamicBuffer<TransformFrame> dynamicBuffer = bufferAccessor[index1];
            // ISSUE: reference to a compiler-generated field
            if (((dynamicBuffer[(int) this.m_TransformFrameIndex].m_Flags ^ transformFrame.m_Flags) & (TransformFlags.MainLights | TransformFlags.ExtraLights | TransformFlags.WorkLights | TransformFlags.TakingOff | TransformFlags.Landing | TransformFlags.Flying)) != (TransformFlags) 0)
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
              if (((transformFlags1 ^ transformFlags2) & (TransformFlags.MainLights | TransformFlags.ExtraLights | TransformFlags.WorkLights | TransformFlags.TakingOff | TransformFlags.Landing | TransformFlags.Flying)) != (TransformFlags) 0)
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
        else
        {
          for (int index3 = 0; index3 < chunk.Count; ++index3)
          {
            PrefabRef prefabRef = nativeArray2[index3];
            AircraftNavigation aircraftNavigation = nativeArray4[index3];
            AircraftCurrentLane aircraftCurrentLane = nativeArray5[index3];
            PseudoRandomSeed pseudoRandomSeed = nativeArray6[index3];
            Moving moving = nativeArray7[index3];
            Transform transform = nativeArray8[index3];
            Random random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kLightState);
            // ISSUE: reference to a compiler-generated field
            AirplaneData airplaneData = this.m_PrefabAirplaneData[prefabRef.m_Prefab];
            TransformFrame transformFrame = new TransformFrame();
            if ((aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.Flying) != (AircraftLaneFlags) 0)
            {
              float3 float3_11 = math.normalizesafe(moving.m_Velocity, new float3(0.0f, 0.0f, 1f));
              float2 x3 = math.normalizesafe(moving.m_Velocity.xz, new float2(0.0f, 1f));
              float2 xz = aircraftNavigation.m_TargetDirection.xz;
              float3 float3_12 = aircraftNavigation.m_TargetPosition - transform.m_Position;
              float num6 = aircraftNavigation.m_MaxSpeed / airplaneData.m_FlyingTurning;
              if (MathUtils.TryNormalize(ref xz))
              {
                float x4 = math.dot(x3, xz);
                float2 float2_1 = MathUtils.Right(xz);
                float x5 = math.dot(float2_1, float3_12.xz);
                if ((double) math.dot(float3_12.xz, xz) >= 0.0)
                {
                  x4 = math.max(x4, 0.0f);
                  if ((double) math.abs(x5) <= (double) num6)
                    x4 = math.max(x4, 1f);
                }
                float2 float2_2 = math.select(float2_1, -float2_1, (double) x5 > 0.0);
                float3_12.xz += float2_2 * (num6 * (1f - x4));
                float3_12.xz -= xz * (num6 * (1f - math.abs(x4)));
              }
              if (MathUtils.TryNormalize(ref float3_12))
              {
                float num7 = math.asin(math.saturate(float3_12.y));
                if ((double) aircraftNavigation.m_MinClimbAngle > (double) num7)
                {
                  float3_12.y = math.sin(aircraftNavigation.m_MinClimbAngle);
                  float3_12.xz = math.normalizesafe(float3_12.xz) * math.cos(aircraftNavigation.m_MinClimbAngle);
                }
              }
              float x6 = math.min(math.acos(math.clamp(math.dot(float3_12, float3_11), -1f, 1f)), airplaneData.m_FlyingTurning * num1);
              float3 float3_13 = math.normalizesafe(float3_12 - float3_11 * math.dot(float3_11, float3_12), new float3(float3_11.zy, -float3_11.x));
              float3 float3_14 = float3_11 * math.cos(x6) + float3_13 * math.sin(x6);
              float3 velocity = moving.m_Velocity;
              moving.m_Velocity = float3_14 * aircraftNavigation.m_MaxSpeed;
              float num8 = math.saturate(1f - (float) (((double) aircraftNavigation.m_MaxSpeed - (double) airplaneData.m_FlyingSpeed.x) / ((double) airplaneData.m_FlyingSpeed.y - (double) airplaneData.m_FlyingSpeed.x)));
              float s = num8 * num8;
              float x7 = math.lerp(-airplaneData.m_ClimbAngle, airplaneData.m_SlowPitchAngle, s);
              float num9 = math.sin(x7);
              float3 float3_15 = float3_14;
              if ((double) float3_15.y < (double) num9)
              {
                float3_15.y = num9;
                float3_15.xz = math.normalizesafe(float3_15.xz, new float2(0.0f, 1f)) * math.cos(x7);
              }
              float3 y3 = new float3();
              y3.xz = math.normalizesafe(MathUtils.Right(float3_15.xz));
              float3 x8 = y3 * (math.dot(moving.m_Velocity - velocity, y3) * airplaneData.m_TurningRollFactor);
              x8.y = math.max(x8.y, 0.0f) + 9.81f * num1;
              float3 y4 = math.normalize(x8);
              float3 up = math.normalizesafe(math.cross(math.cross(float3_15, y4), float3_15), new float3(0.0f, 1f, 0.0f));
              quaternion b3 = quaternion.LookRotationSafe(float3_15, up);
              float3 axis;
              float angle;
              MathUtils.AxisAngle(math.mul(math.inverse(transform.m_Rotation), b3), out axis, out angle);
              float3 float3_16 = math.clamp(axis * angle * airplaneData.m_FlyingAngularAcceleration - moving.m_AngularVelocity, (float3) (-airplaneData.m_FlyingAngularAcceleration * num1), (float3) (airplaneData.m_FlyingAngularAcceleration * num1));
              moving.m_AngularVelocity += float3_16;
              float num10 = math.length(moving.m_AngularVelocity);
              if ((double) num10 > 9.9999997473787516E-06)
              {
                quaternion b4 = quaternion.AxisAngle(moving.m_AngularVelocity / num10, num10 * num1);
                transform.m_Rotation = math.normalize(math.mul(transform.m_Rotation, b4));
              }
              float3 y5 = transform.m_Position + moving.m_Velocity * num1;
              transformFrame.m_Position = math.lerp(transform.m_Position, y5, 0.5f);
              transformFrame.m_Velocity = moving.m_Velocity;
              transformFrame.m_Rotation = transform.m_Rotation;
              transformFrame.m_Flags = TransformFlags.InteriorLights | TransformFlags.Flying;
              if ((aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.Landing) != (AircraftLaneFlags) 0)
              {
                transformFrame.m_Flags |= TransformFlags.WarningLights | TransformFlags.Landing;
                // ISSUE: reference to a compiler-generated field
                if ((double) this.m_DayLightBrightness + (double) random.NextFloat(-0.05f, 0.05f) < 0.5)
                  transformFrame.m_Flags |= TransformFlags.ExtraLights;
              }
              else if ((aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.TakingOff) != (AircraftLaneFlags) 0)
                transformFrame.m_Flags |= TransformFlags.WarningLights | TransformFlags.TakingOff;
              transform.m_Position = y5;
            }
            else
            {
              float3 float3_17 = aircraftNavigation.m_TargetPosition - transform.m_Position;
              MathUtils.TryNormalize(ref float3_17, aircraftNavigation.m_MaxSpeed);
              float3 float3_18 = float3_17 * 8f + moving.m_Velocity;
              MathUtils.TryNormalize(ref float3_18, aircraftNavigation.m_MaxSpeed);
              moving.m_Velocity = float3_18;
              float3 float3_19 = moving.m_Velocity * (num1 * 0.5f);
              float3 float3_20 = transform.m_Position + float3_19;
              float3 targetDirection = aircraftNavigation.m_TargetDirection;
              quaternion b5 = transform.m_Rotation;
              if (MathUtils.TryNormalize(ref targetDirection))
              {
                b5 = quaternion.LookRotationSafe(targetDirection, math.up());
              }
              else
              {
                float3 forward = aircraftNavigation.m_TargetPosition - transform.m_Position;
                if (MathUtils.TryNormalize(ref forward))
                  b5 = quaternion.LookRotationSafe(forward, math.up());
              }
              if ((double) aircraftNavigation.m_MaxSpeed > (double) airplaneData.m_FlyingSpeed.x * 0.89999997615814209)
              {
                float3 axis;
                float angle;
                MathUtils.AxisAngle(math.mul(math.inverse(transform.m_Rotation), b5), out axis, out angle);
                float3 float3_21 = math.clamp(axis * angle * airplaneData.m_FlyingAngularAcceleration - moving.m_AngularVelocity, (float3) (-airplaneData.m_FlyingAngularAcceleration * num1), (float3) (airplaneData.m_FlyingAngularAcceleration * num1));
                moving.m_AngularVelocity += float3_21;
                float num11 = math.length(moving.m_AngularVelocity);
                if ((double) num11 > 9.9999997473787516E-06)
                {
                  quaternion b6 = quaternion.AxisAngle(moving.m_AngularVelocity / num11, num11 * num1);
                  transform.m_Rotation = math.normalize(math.mul(transform.m_Rotation, b6));
                }
              }
              else
              {
                transform.m_Rotation = b5;
                moving.m_AngularVelocity = new float3();
              }
              transformFrame.m_Position = float3_20;
              transformFrame.m_Velocity = moving.m_Velocity;
              transformFrame.m_Rotation = transform.m_Rotation;
              transformFrame.m_Flags = TransformFlags.InteriorLights | TransformFlags.WarningLights;
              if ((aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.Landing) != (AircraftLaneFlags) 0)
                transformFrame.m_Flags |= TransformFlags.Landing;
              else if ((aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.TakingOff) != (AircraftLaneFlags) 0)
                transformFrame.m_Flags |= TransformFlags.TakingOff;
              // ISSUE: reference to a compiler-generated field
              if ((double) this.m_DayLightBrightness + (double) random.NextFloat(-0.05f, 0.05f) < 0.5)
                transformFrame.m_Flags |= TransformFlags.MainLights;
              transform.m_Position = float3_20 + float3_19;
            }
            DynamicBuffer<TransformFrame> dynamicBuffer = bufferAccessor[index3];
            // ISSUE: reference to a compiler-generated field
            if (((dynamicBuffer[(int) this.m_TransformFrameIndex].m_Flags ^ transformFrame.m_Flags) & (TransformFlags.MainLights | TransformFlags.ExtraLights | TransformFlags.TakingOff | TransformFlags.Landing | TransformFlags.Flying)) != (TransformFlags) 0)
            {
              TransformFlags transformFlags3 = (TransformFlags) 0;
              TransformFlags transformFlags4 = (TransformFlags) 0;
              for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
              {
                TransformFlags flags = dynamicBuffer[index4].m_Flags;
                transformFlags3 |= flags;
                // ISSUE: reference to a compiler-generated field
                transformFlags4 |= index4 == (int) this.m_TransformFrameIndex ? transformFrame.m_Flags : flags;
              }
              if (((transformFlags3 ^ transformFlags4) & (TransformFlags.MainLights | TransformFlags.ExtraLights | TransformFlags.TakingOff | TransformFlags.Landing | TransformFlags.Flying)) != (TransformFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<EffectsUpdated>(unfilteredChunkIndex, nativeArray1[index3], new EffectsUpdated());
              }
            }
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer[(int) this.m_TransformFrameIndex] = transformFrame;
            nativeArray7[index3] = moving;
            nativeArray8[index3] = transform;
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
      public ComponentTypeHandle<Aircraft> __Game_Vehicles_Aircraft_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AircraftNavigation> __Game_Vehicles_AircraftNavigation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RW_ComponentTypeHandle;
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<HelicopterData> __Game_Prefabs_HelicopterData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AirplaneData> __Game_Prefabs_AirplaneData_RO_ComponentLookup;
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Aircraft_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Aircraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HelicopterData_RO_ComponentLookup = state.GetComponentLookup<HelicopterData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AirplaneData_RO_ComponentLookup = state.GetComponentLookup<AirplaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>();
      }
    }
  }
}
