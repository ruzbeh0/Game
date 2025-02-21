// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CarTrailerMoveSystem
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
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CarTrailerMoveSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_VehicleQuery;
    private CarTrailerMoveSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Car>(), ComponentType.ReadOnly<LayoutElement>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Stopped>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
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
      this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTrailerData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTractorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new CarTrailerMoveSystem.CarTrailerMoveJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabTractorData = this.__TypeHandle.__Game_Prefabs_CarTractorData_RO_ComponentLookup,
        m_PrefabTrailerData = this.__TypeHandle.__Game_Prefabs_CarTrailerData_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup,
        m_TransformFrameIndex = (this.m_SimulationSystem.frameIndex / 16U % 4U)
      }.ScheduleParallel<CarTrailerMoveSystem.CarTrailerMoveJob>(this.m_VehicleQuery, this.Dependency);
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
    public CarTrailerMoveSystem()
    {
    }

    [BurstCompile]
    private struct CarTrailerMoveJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<CarTractorData> m_PrefabTractorData;
      [ReadOnly]
      public ComponentLookup<CarTrailerData> m_PrefabTrailerData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Transform> m_TransformData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Moving> m_MovingData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public uint m_TransformFrameIndex;

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
        BufferAccessor<LayoutElement> bufferAccessor = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        float num = 0.266666681f;
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray1[index1];
          PrefabRef prefabRef1 = nativeArray2[index1];
          DynamicBuffer<LayoutElement> dynamicBuffer = bufferAccessor[index1];
          if (dynamicBuffer.Length > 1)
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform1 = this.m_TransformData[entity];
            // ISSUE: reference to a compiler-generated field
            Moving moving1 = this.m_MovingData[entity];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<TransformFrame> transformFrame1 = this.m_TransformFrames[entity];
            // ISSUE: reference to a compiler-generated field
            CarTractorData carTractorData = this.m_PrefabTractorData[prefabRef1.m_Prefab];
            for (int index2 = 1; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity vehicle = dynamicBuffer[index2].m_Vehicle;
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef2 = this.m_PrefabRefData[vehicle];
              // ISSUE: reference to a compiler-generated field
              Transform transform2 = this.m_TransformData[vehicle];
              // ISSUE: reference to a compiler-generated field
              Moving moving2 = this.m_MovingData[vehicle];
              // ISSUE: reference to a compiler-generated field
              CarData carData = this.m_PrefabCarData[prefabRef2.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              CarTrailerData carTrailerData = this.m_PrefabTrailerData[prefabRef2.m_Prefab];
              quaternion quaternion;
              float3 float3_1;
              if (flag)
              {
                quaternion = transform1.m_Rotation;
                float3_1 = transform1.m_Position + math.rotate(transform1.m_Rotation, carTractorData.m_AttachPosition) - math.rotate(transform1.m_Rotation, carTrailerData.m_AttachPosition);
              }
              else
              {
                switch (carTrailerData.m_MovementType)
                {
                  case TrailerMovementType.Free:
                    float3 float3_2 = transform1.m_Position + math.rotate(transform1.m_Rotation, carTractorData.m_AttachPosition);
                    float3 float3_3 = transform2.m_Position + math.rotate(transform2.m_Rotation, new float3(carTrailerData.m_AttachPosition.xy, carData.m_PivotOffset));
                    quaternion = transform1.m_Rotation;
                    float3 forward = float3_2 - float3_3;
                    forward += moving1.m_Velocity * (num * 0.25f);
                    forward -= moving2.m_Velocity * (num * 0.5f);
                    if (MathUtils.TryNormalize(ref forward))
                      quaternion = quaternion.LookRotationSafe(forward, math.up());
                    float3_1 = float3_2 - math.rotate(quaternion, carTrailerData.m_AttachPosition);
                    break;
                  case TrailerMovementType.Locked:
                    quaternion = transform1.m_Rotation;
                    float3_1 = transform1.m_Position + math.rotate(transform1.m_Rotation, carTractorData.m_AttachPosition) - math.rotate(transform2.m_Rotation, carTrailerData.m_AttachPosition);
                    break;
                  default:
                    float3_1 = transform2.m_Position;
                    quaternion = transform2.m_Rotation;
                    break;
                }
              }
              moving2.m_Velocity = (float3_1 - transform2.m_Position) / num;
              TransformFrame transformFrame2 = new TransformFrame();
              transformFrame2.m_Position = (transform2.m_Position + float3_1) * 0.5f;
              transformFrame2.m_Velocity = moving2.m_Velocity;
              transformFrame2.m_Rotation = math.slerp(transform2.m_Rotation, quaternion, 0.5f);
              // ISSUE: reference to a compiler-generated field
              transformFrame2.m_Flags = transformFrame1[(int) this.m_TransformFrameIndex].m_Flags;
              transform2.m_Position = float3_1;
              transform2.m_Rotation = quaternion;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_TransformFrames[vehicle][(int) this.m_TransformFrameIndex] = transformFrame2;
              // ISSUE: reference to a compiler-generated field
              this.m_MovingData[vehicle] = moving2;
              // ISSUE: reference to a compiler-generated field
              this.m_TransformData[vehicle] = transform2;
              if (index2 + 1 < dynamicBuffer.Length)
              {
                transform1 = transform2;
                moving1 = moving2;
                // ISSUE: reference to a compiler-generated field
                carTractorData = this.m_PrefabTractorData[prefabRef2.m_Prefab];
              }
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTractorData> __Game_Prefabs_CarTractorData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarTrailerData> __Game_Prefabs_CarTrailerData_RO_ComponentLookup;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;
      public ComponentLookup<Moving> __Game_Objects_Moving_RW_ComponentLookup;
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTractorData_RO_ComponentLookup = state.GetComponentLookup<CarTractorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTrailerData_RO_ComponentLookup = state.GetComponentLookup<CarTrailerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentLookup = state.GetComponentLookup<Moving>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferLookup = state.GetBufferLookup<TransformFrame>();
      }
    }
  }
}
