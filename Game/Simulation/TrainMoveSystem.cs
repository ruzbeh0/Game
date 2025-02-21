// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TrainMoveSystem
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
  public class TrainMoveSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private LightingSystem m_LightingSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_TrainQuery;
    private EntityQuery m_LayoutQuery;
    private TrainMoveSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 3;

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
      this.m_TrainQuery = this.GetEntityQuery(ComponentType.ReadOnly<Train>(), ComponentType.ReadOnly<TrainNavigation>(), ComponentType.ReadWrite<Transform>(), ComponentType.ReadWrite<Moving>(), ComponentType.ReadWrite<TransformFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>());
      // ISSUE: reference to a compiler-generated field
      this.m_LayoutQuery = this.GetEntityQuery(ComponentType.ReadOnly<Train>(), ComponentType.ReadOnly<TrainNavigation>(), ComponentType.ReadOnly<LayoutElement>(), ComponentType.ReadWrite<TransformFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TrainQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num = this.m_SimulationSystem.frameIndex / 16U % 4U;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainBogieFrame_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_TrainNavigation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      TrainMoveSystem.UpdateTransformDataJob jobData1 = new TrainMoveSystem.UpdateTransformDataJob()
      {
        m_TrainType = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Vehicles_TrainNavigation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle,
        m_BogieFrameType = this.__TypeHandle.__Game_Vehicles_TrainBogieFrame_RW_BufferTypeHandle,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_TransformFrameIndex = num
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      TrainMoveSystem.UpdateLayoutDataJob jobData2 = new TrainMoveSystem.UpdateLayoutDataJob()
      {
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_CurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup,
        m_TransformFrameIndex = num,
        m_DayLightBrightness = this.m_LightingSystem.dayLightBrightness,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<TrainMoveSystem.UpdateTransformDataJob>(this.m_TrainQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery layoutQuery = this.m_LayoutQuery;
      JobHandle dependsOn = jobHandle;
      JobHandle producerJob = jobData2.ScheduleParallel<TrainMoveSystem.UpdateLayoutDataJob>(layoutQuery, dependsOn);
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
    public TrainMoveSystem()
    {
    }

    [BurstCompile]
    private struct UpdateTransformDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Train> m_TrainType;
      [ReadOnly]
      public ComponentTypeHandle<TrainCurrentLane> m_CurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<TrainNavigation> m_NavigationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Moving> m_MovingType;
      public ComponentTypeHandle<Transform> m_TransformType;
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      public BufferTypeHandle<TrainBogieFrame> m_BogieFrameType;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
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
        NativeArray<Train> nativeArray2 = chunk.GetNativeArray<Train>(ref this.m_TrainType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrainCurrentLane> nativeArray3 = chunk.GetNativeArray<TrainCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrainNavigation> nativeArray4 = chunk.GetNativeArray<TrainNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray5 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray6 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor1 = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TrainBogieFrame> bufferAccessor2 = chunk.GetBufferAccessor<TrainBogieFrame>(ref this.m_BogieFrameType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          PrefabRef prefabRef = nativeArray1[index];
          Train train = nativeArray2[index];
          TrainCurrentLane trainCurrentLane = nativeArray3[index];
          TrainNavigation trainNavigation = nativeArray4[index];
          Moving moving = nativeArray5[index];
          Transform transform = nativeArray6[index];
          // ISSUE: reference to a compiler-generated field
          TrainData prefabTrainData = this.m_PrefabTrainData[prefabRef.m_Prefab];
          float3 pivot1;
          float3 pivot2;
          VehicleUtils.CalculateTrainNavigationPivots(transform, prefabTrainData, out pivot1, out pivot2);
          float3 b = trainNavigation.m_Rear.m_Position - trainNavigation.m_Front.m_Position;
          bool c = (train.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0;
          if (c)
          {
            CommonUtils.Swap<float3>(ref pivot1, ref pivot2);
            prefabTrainData.m_BogieOffsets = prefabTrainData.m_BogieOffsets.yx;
          }
          if (!MathUtils.TryNormalize(ref b, prefabTrainData.m_BogieOffsets.x))
            b = transform.m_Position - pivot1;
          transform.m_Position = trainNavigation.m_Front.m_Position + b;
          float3 forward = math.select(-b, b, c);
          if (MathUtils.TryNormalize(ref forward))
            transform.m_Rotation = quaternion.LookRotationSafe(forward, math.up());
          moving.m_Velocity = trainNavigation.m_Front.m_Direction + trainNavigation.m_Rear.m_Direction;
          MathUtils.TryNormalize(ref moving.m_Velocity, trainNavigation.m_Speed);
          TransformFrame transformFrame = new TransformFrame();
          transformFrame.m_Position = transform.m_Position;
          transformFrame.m_Velocity = moving.m_Velocity;
          transformFrame.m_Rotation = transform.m_Rotation;
          TrainBogieFrame trainBogieFrame = new TrainBogieFrame();
          trainBogieFrame.m_FrontLane = trainCurrentLane.m_Front.m_Lane;
          trainBogieFrame.m_RearLane = trainCurrentLane.m_Rear.m_Lane;
          // ISSUE: reference to a compiler-generated field
          bufferAccessor1[index][(int) this.m_TransformFrameIndex] = transformFrame;
          // ISSUE: reference to a compiler-generated field
          bufferAccessor2[index][(int) this.m_TransformFrameIndex] = trainBogieFrame;
          nativeArray5[index] = moving;
          nativeArray6[index] = transform;
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

    [BurstCompile]
    private struct UpdateLayoutDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> m_CurrentLaneData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public uint m_TransformFrameIndex;
      [ReadOnly]
      public float m_DayLightBrightness;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PseudoRandomSeed> nativeArray = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          PseudoRandomSeed pseudoRandomSeed = nativeArray[index1];
          DynamicBuffer<LayoutElement> dynamicBuffer = bufferAccessor[index1];
          if (dynamicBuffer.Length != 0)
          {
            Entity vehicle1 = dynamicBuffer[0].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            Train train1 = this.m_TrainData[vehicle1];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<TransformFrame> transformFrame1 = this.m_TransformFrames[vehicle1];
            // ISSUE: reference to a compiler-generated field
            TrainCurrentLane trainCurrentLane = this.m_CurrentLaneData[vehicle1];
            TransformFlags transformFlags1 = TransformFlags.InteriorLights;
            TransformFlags transformFlags2 = TransformFlags.InteriorLights;
            TransformFlags transformFlags3;
            TransformFlags transformFlags4;
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_DayLightBrightness + (double) pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kLightState).NextFloat(-0.05f, 0.05f) < 0.25 && (trainCurrentLane.m_Front.m_LaneFlags & TrainLaneFlags.HighBeams) != (TrainLaneFlags) 0)
            {
              transformFlags3 = transformFlags1 | TransformFlags.MainLights | TransformFlags.ExtraLights;
              transformFlags4 = transformFlags2 | TransformFlags.MainLights | TransformFlags.ExtraLights;
            }
            else
            {
              transformFlags3 = transformFlags1 | TransformFlags.MainLights;
              transformFlags4 = transformFlags2 | TransformFlags.MainLights;
            }
            if ((trainCurrentLane.m_Front.m_LaneFlags & TrainLaneFlags.TurnLeft) != (TrainLaneFlags) 0)
            {
              transformFlags3 |= TransformFlags.TurningLeft;
              transformFlags4 |= TransformFlags.TurningRight;
            }
            if ((trainCurrentLane.m_Front.m_LaneFlags & TrainLaneFlags.TurnRight) != (TrainLaneFlags) 0)
            {
              transformFlags3 |= TransformFlags.TurningRight;
              transformFlags4 |= TransformFlags.TurningLeft;
            }
            if ((train1.m_Flags & Game.Vehicles.TrainFlags.BoardingLeft) != (Game.Vehicles.TrainFlags) 0)
            {
              transformFlags3 |= TransformFlags.BoardingLeft;
              transformFlags4 |= TransformFlags.BoardingRight;
            }
            if ((train1.m_Flags & Game.Vehicles.TrainFlags.BoardingRight) != (Game.Vehicles.TrainFlags) 0)
            {
              transformFlags3 |= TransformFlags.BoardingRight;
              transformFlags4 |= TransformFlags.BoardingLeft;
            }
            // ISSUE: reference to a compiler-generated field
            TransformFrame transformFrame2 = transformFrame1[(int) this.m_TransformFrameIndex];
            TransformFlags flags1 = transformFrame2.m_Flags;
            transformFrame2.m_Flags = (train1.m_Flags & Game.Vehicles.TrainFlags.Reversed) == (Game.Vehicles.TrainFlags) 0 ? transformFlags3 : transformFlags4;
            if ((train1.m_Flags & Game.Vehicles.TrainFlags.Pantograph) != (Game.Vehicles.TrainFlags) 0)
              transformFrame2.m_Flags |= TransformFlags.Pantograph;
            if (((flags1 ^ transformFrame2.m_Flags) & (TransformFlags.MainLights | TransformFlags.ExtraLights)) != (TransformFlags) 0)
            {
              TransformFlags transformFlags5 = (TransformFlags) 0;
              TransformFlags transformFlags6 = (TransformFlags) 0;
              for (int index2 = 0; index2 < transformFrame1.Length; ++index2)
              {
                TransformFlags flags2 = transformFrame1[index2].m_Flags;
                transformFlags5 |= flags2;
                // ISSUE: reference to a compiler-generated field
                transformFlags6 |= index2 == (int) this.m_TransformFrameIndex ? transformFrame2.m_Flags : flags2;
              }
              if (((transformFlags5 ^ transformFlags6) & (TransformFlags.MainLights | TransformFlags.ExtraLights)) != (TransformFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<EffectsUpdated>(unfilteredChunkIndex, vehicle1, new EffectsUpdated());
              }
            }
            // ISSUE: reference to a compiler-generated field
            transformFrame1[(int) this.m_TransformFrameIndex] = transformFrame2;
            TransformFlags transformFlags7 = transformFlags3 & ~(TransformFlags.MainLights | TransformFlags.ExtraLights);
            TransformFlags transformFlags8 = transformFlags4 & ~(TransformFlags.MainLights | TransformFlags.ExtraLights);
            for (int index3 = 1; index3 < dynamicBuffer.Length; ++index3)
            {
              Entity vehicle2 = dynamicBuffer[index3].m_Vehicle;
              if (index3 == dynamicBuffer.Length - 1)
              {
                transformFlags7 |= TransformFlags.RearLights;
                transformFlags8 |= TransformFlags.RearLights;
              }
              // ISSUE: reference to a compiler-generated field
              Train train2 = this.m_TrainData[vehicle2];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TransformFrame> transformFrame3 = this.m_TransformFrames[vehicle2];
              // ISSUE: reference to a compiler-generated field
              TransformFrame transformFrame4 = transformFrame3[(int) this.m_TransformFrameIndex];
              TransformFlags flags3 = transformFrame4.m_Flags;
              transformFrame4.m_Flags = (train2.m_Flags & Game.Vehicles.TrainFlags.Reversed) == (Game.Vehicles.TrainFlags) 0 ? transformFlags7 : transformFlags8;
              if ((train2.m_Flags & Game.Vehicles.TrainFlags.Pantograph) != (Game.Vehicles.TrainFlags) 0)
                transformFrame4.m_Flags |= TransformFlags.Pantograph;
              if (((flags3 ^ transformFrame4.m_Flags) & (TransformFlags.MainLights | TransformFlags.ExtraLights)) != (TransformFlags) 0)
              {
                TransformFlags transformFlags9 = (TransformFlags) 0;
                TransformFlags transformFlags10 = (TransformFlags) 0;
                for (int index4 = 0; index4 < transformFrame3.Length; ++index4)
                {
                  TransformFlags flags4 = transformFrame3[index4].m_Flags;
                  transformFlags9 |= flags4;
                  // ISSUE: reference to a compiler-generated field
                  transformFlags10 |= index4 == (int) this.m_TransformFrameIndex ? transformFrame4.m_Flags : flags4;
                }
                if (((transformFlags9 ^ transformFlags10) & (TransformFlags.MainLights | TransformFlags.ExtraLights)) != (TransformFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<EffectsUpdated>(unfilteredChunkIndex, vehicle2, new EffectsUpdated());
                }
              }
              // ISSUE: reference to a compiler-generated field
              transformFrame3[(int) this.m_TransformFrameIndex] = transformFrame4;
            }
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
      public ComponentTypeHandle<Train> __Game_Vehicles_Train_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrainNavigation> __Game_Vehicles_TrainNavigation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RW_ComponentTypeHandle;
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RW_BufferTypeHandle;
      public BufferTypeHandle<TrainBogieFrame> __Game_Vehicles_TrainBogieFrame_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentLookup;
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrainNavigation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainBogieFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TrainBogieFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferLookup = state.GetBufferLookup<TransformFrame>();
      }
    }
  }
}
