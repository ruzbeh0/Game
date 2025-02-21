// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.VehicleInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class VehicleInitializeSystem : GameSystemBase
  {
    private EntityQuery m_PrefabQuery;
    private PrefabSystem m_PrefabSystem;
    private VehicleInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadWrite<CarData>(),
          ComponentType.ReadWrite<TrainData>(),
          ComponentType.ReadWrite<CarTractorData>(),
          ComponentType.ReadWrite<CarTrailerData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ObjectGeometryData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MultipleUnitTrainData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<MultipleUnitTrainData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_MultipleUnitTrainData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<TrainData> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_TrainData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<CarData> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_CarData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SwayingData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<SwayingData> componentTypeHandle6 = this.__TypeHandle.__Game_Prefabs_SwayingData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VehicleData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<VehicleData> componentTypeHandle7 = this.__TypeHandle.__Game_Prefabs_VehicleData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTractorData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<CarTractorData> componentTypeHandle8 = this.__TypeHandle.__Game_Prefabs_CarTractorData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTrailerData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<CarTrailerData> componentTypeHandle9 = this.__TypeHandle.__Game_Prefabs_CarTrailerData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<SubMesh> bufferTypeHandle = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferTypeHandle;
      this.CompleteDependency();
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
        NativeArray<ObjectGeometryData> nativeArray2 = archetypeChunk.GetNativeArray<ObjectGeometryData>(ref componentTypeHandle2);
        NativeArray<TrainData> nativeArray3 = archetypeChunk.GetNativeArray<TrainData>(ref componentTypeHandle4);
        NativeArray<CarData> nativeArray4 = archetypeChunk.GetNativeArray<CarData>(ref componentTypeHandle5);
        NativeArray<SwayingData> nativeArray5 = archetypeChunk.GetNativeArray<SwayingData>(ref componentTypeHandle6);
        NativeArray<CarTractorData> nativeArray6 = archetypeChunk.GetNativeArray<CarTractorData>(ref componentTypeHandle8);
        NativeArray<CarTrailerData> nativeArray7 = archetypeChunk.GetNativeArray<CarTrailerData>(ref componentTypeHandle9);
        for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          TrainPrefab prefab = this.m_PrefabSystem.GetPrefab<TrainPrefab>(nativeArray1[index2]);
          ObjectGeometryData objectGeometryData = nativeArray2[index2];
          TrainData trainData = nativeArray3[index2];
          float2 float2 = new float2(objectGeometryData.m_Bounds.max.z, -objectGeometryData.m_Bounds.min.z);
          trainData.m_TrackType = prefab.m_TrackType;
          trainData.m_EnergyType = prefab.m_EnergyType;
          trainData.m_MaxSpeed = prefab.m_MaxSpeed / 3.6f;
          trainData.m_Acceleration = prefab.m_Acceleration;
          trainData.m_Braking = prefab.m_Braking;
          trainData.m_Turning = math.radians(prefab.m_Turning);
          trainData.m_BogieOffsets = prefab.m_BogieOffset;
          trainData.m_AttachOffsets = float2 - prefab.m_AttachOffset;
          nativeArray3[index2] = trainData;
        }
        for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          CarBasePrefab prefab = this.m_PrefabSystem.GetPrefab<CarBasePrefab>(nativeArray1[index3]);
          CarData carData = nativeArray4[index3];
          SwayingData swayingData = nativeArray5[index3];
          carData.m_SizeClass = prefab.m_SizeClass;
          carData.m_EnergyType = prefab.m_EnergyType;
          carData.m_MaxSpeed = prefab.m_MaxSpeed / 3.6f;
          carData.m_Acceleration = prefab.m_Acceleration;
          carData.m_Braking = prefab.m_Braking;
          carData.m_Turning = math.radians(prefab.m_Turning);
          swayingData.m_SpringFactors = (float3) prefab.m_Stiffness;
          nativeArray4[index3] = carData;
          nativeArray5[index3] = swayingData;
        }
        for (int index4 = 0; index4 < nativeArray6.Length; ++index4)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          CarTractor component = this.m_PrefabSystem.GetPrefab<VehiclePrefab>(nativeArray1[index4]).GetComponent<CarTractor>();
          ObjectGeometryData objectGeometryData = nativeArray2[index4];
          CarTractorData carTractorData = nativeArray6[index4] with
          {
            m_TrailerType = component.m_TrailerType
          };
          carTractorData.m_AttachPosition.xy = component.m_AttachOffset.xy;
          carTractorData.m_AttachPosition.z = objectGeometryData.m_Bounds.min.z + component.m_AttachOffset.z;
          if ((Object) component.m_FixedTrailer != (Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            carTractorData.m_FixedTrailer = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_FixedTrailer);
          }
          nativeArray6[index4] = carTractorData;
        }
        for (int index5 = 0; index5 < nativeArray7.Length; ++index5)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          CarTrailerPrefab prefab = this.m_PrefabSystem.GetPrefab<CarTrailerPrefab>(nativeArray1[index5]);
          ObjectGeometryData objectGeometryData = nativeArray2[index5];
          CarTrailerData carTrailerData = nativeArray7[index5] with
          {
            m_TrailerType = prefab.m_TrailerType,
            m_MovementType = prefab.m_MovementType
          };
          carTrailerData.m_AttachPosition.xy = prefab.m_AttachOffset.xy;
          carTrailerData.m_AttachPosition.z = objectGeometryData.m_Bounds.max.z - prefab.m_AttachOffset.z;
          if ((Object) prefab.m_FixedTractor != (Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            carTrailerData.m_FixedTractor = this.m_PrefabSystem.GetEntity((PrefabBase) prefab.m_FixedTractor);
          }
          nativeArray7[index5] = carTrailerData;
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.Dependency = new VehicleInitializeSystem.InitializeVehiclesJob()
      {
        m_ObjectGeometryType = componentTypeHandle2,
        m_CarTrailerType = componentTypeHandle9,
        m_MultipleUnitTrainType = componentTypeHandle3,
        m_SubmeshType = bufferTypeHandle,
        m_CarType = componentTypeHandle5,
        m_TrainType = componentTypeHandle4,
        m_SwayingType = componentTypeHandle6,
        m_VehicleType = componentTypeHandle7,
        m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_Chunks = archetypeChunkArray
      }.Schedule<VehicleInitializeSystem.InitializeVehiclesJob>(archetypeChunkArray.Length, 1, this.Dependency);
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
    public VehicleInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeVehiclesJob : IJobParallelFor
    {
      [ReadOnly]
      public ComponentTypeHandle<ObjectGeometryData> m_ObjectGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<CarTrailerData> m_CarTrailerType;
      [ReadOnly]
      public ComponentTypeHandle<MultipleUnitTrainData> m_MultipleUnitTrainType;
      [ReadOnly]
      public BufferTypeHandle<SubMesh> m_SubmeshType;
      public ComponentTypeHandle<CarData> m_CarType;
      public ComponentTypeHandle<TrainData> m_TrainType;
      public ComponentTypeHandle<SwayingData> m_SwayingType;
      public ComponentTypeHandle<VehicleData> m_VehicleType;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        NativeArray<ObjectGeometryData> nativeArray1 = chunk.GetNativeArray<ObjectGeometryData>(ref this.m_ObjectGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarTrailerData> nativeArray2 = chunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarData> nativeArray3 = chunk.GetNativeArray<CarData>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrainData> nativeArray4 = chunk.GetNativeArray<TrainData>(ref this.m_TrainType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<SwayingData> nativeArray5 = chunk.GetNativeArray<SwayingData>(ref this.m_SwayingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<VehicleData> nativeArray6 = chunk.GetNativeArray<VehicleData>(ref this.m_VehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubMesh> bufferAccessor = chunk.GetBufferAccessor<SubMesh>(ref this.m_SubmeshType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<MultipleUnitTrainData>(ref this.m_MultipleUnitTrainType);
        for (int index1 = 0; index1 < nativeArray6.Length; ++index1)
        {
          VehicleData vehicleData = nativeArray6[index1] with
          {
            m_SteeringBoneIndex = -1
          };
          if (bufferAccessor.Length != 0)
          {
            DynamicBuffer<SubMesh> dynamicBuffer = bufferAccessor[index1];
            int num = 0;
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              DynamicBuffer<ProceduralBone> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ProceduralBones.TryGetBuffer(dynamicBuffer[index2].m_SubMesh, out bufferData))
              {
                for (int index3 = 0; index3 < bufferData.Length; ++index3)
                {
                  if (bufferData[index3].m_Type == BoneType.SteeringRotation)
                    vehicleData.m_SteeringBoneIndex = num + index3;
                }
                num += bufferData.Length;
              }
            }
          }
          nativeArray6[index1] = vehicleData;
        }
        for (int index4 = 0; index4 < nativeArray3.Length; ++index4)
        {
          ObjectGeometryData objectGeometryData = nativeArray1[index4];
          CarData carData = nativeArray3[index4];
          SwayingData swayingData = nativeArray5[index4];
          Bounds3 bounds3 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
          float3 float3_1 = (float3) 0.0f;
          float3 float3_2 = (float3) 0.0f;
          int3 int3 = (int3) 0;
          if (bufferAccessor.Length != 0)
          {
            DynamicBuffer<SubMesh> dynamicBuffer = bufferAccessor[index4];
            for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
            {
              SubMesh subMesh = dynamicBuffer[index5];
              DynamicBuffer<ProceduralBone> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ProceduralBones.TryGetBuffer(subMesh.m_SubMesh, out bufferData))
              {
                for (int index6 = 0; index6 < bufferData.Length; ++index6)
                {
                  ProceduralBone bone = bufferData[index6];
                  switch (bone.m_Type)
                  {
                    case BoneType.RollingTire:
                    case BoneType.SteeringTire:
                    case BoneType.FixedTire:
                      float3 v = bone.m_ObjectPosition;
                      if ((subMesh.m_Flags & SubMeshFlags.HasTransform) != (SubMeshFlags) 0)
                        v = subMesh.m_Position + math.rotate(subMesh.m_Rotation, v);
                      bounds3 |= v;
                      // ISSUE: reference to a compiler-generated method
                      if (this.HasSteering(bufferData, bone))
                      {
                        float3_2 += v;
                        ++int3.yz;
                        break;
                      }
                      float3_1 += v;
                      ++int3.xz;
                      break;
                  }
                }
              }
            }
          }
          carData.m_PivotOffset = int3.x == 0 ? (int3.y == 0 ? objectGeometryData.m_Size.z * -0.2f : float3_2.z / (float) int3.y) : float3_1.z / (float) int3.x;
          if (nativeArray2.Length != 0)
          {
            CarTrailerData carTrailerData = nativeArray2[index4];
            bounds3 |= carTrailerData.m_AttachPosition;
          }
          float2 float2;
          float num;
          if (int3.z != 0)
          {
            float2 = math.max((float2) 0.5f, MathUtils.Size(bounds3.xz) * 0.5f);
            num = (float3_1.y + float3_2.y) / (float) int3.z;
          }
          else
          {
            float2 = objectGeometryData.m_Size.xz * new float2(0.45f, 0.3f);
            num = objectGeometryData.m_Size.y * 0.25f;
          }
          float3 float3_3 = math.max((float3) 1f, objectGeometryData.m_Size * objectGeometryData.m_Size);
          float3 float3_4 = math.max((float3) 1f, swayingData.m_SpringFactors);
          swayingData.m_SpringFactors.x *= (float) (1.0 + (double) objectGeometryData.m_Size.y * 0.3333333432674408);
          float3_4.x *= (float) (1.0 + (double) objectGeometryData.m_Size.y * 0.1666666716337204);
          swayingData.m_VelocityFactors.xz = (float) (((double) objectGeometryData.m_Size.y * 0.5 - (double) num) * 12.0) / (float3_3.yy + float3_3.xz);
          swayingData.m_VelocityFactors.y = 1f;
          swayingData.m_DampingFactors = 1f / float3_4;
          swayingData.m_MaxPosition = math.length(objectGeometryData.m_Size) * 3f / (new float3(float2.x, 1f, float2.y) * float3_4);
          swayingData.m_SpringFactors.xz *= float2 * 12f / (float3_3.yy + float3_3.xz);
          if (int3.z != 0 && (double) bounds3.max.x - (double) bounds3.min.x < (double) objectGeometryData.m_Size.x * 0.10000000149011612)
          {
            swayingData.m_VelocityFactors.x *= -0.4f;
            swayingData.m_SpringFactors.x *= 0.1f;
            swayingData.m_MaxPosition.x *= 5f;
          }
          nativeArray3[index4] = carData;
          nativeArray5[index4] = swayingData;
        }
        for (int index7 = 0; index7 < nativeArray4.Length; ++index7)
        {
          ObjectGeometryData objectGeometryData = nativeArray1[index7];
          TrainData trainData = nativeArray4[index7];
          Bounds3 bounds3 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
          int num1 = 0;
          if (flag)
            trainData.m_TrainFlags |= TrainFlags.MultiUnit;
          if (bufferAccessor.Length != 0)
          {
            DynamicBuffer<SubMesh> dynamicBuffer = bufferAccessor[index7];
            for (int index8 = 0; index8 < dynamicBuffer.Length; ++index8)
            {
              SubMesh subMesh = dynamicBuffer[index8];
              DynamicBuffer<ProceduralBone> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ProceduralBones.TryGetBuffer(subMesh.m_SubMesh, out bufferData))
              {
                for (int index9 = 0; index9 < bufferData.Length; ++index9)
                {
                  ProceduralBone proceduralBone = bufferData[index9];
                  switch (proceduralBone.m_Type)
                  {
                    case BoneType.TrainBogie:
                      float3 v = proceduralBone.m_ObjectPosition;
                      if ((subMesh.m_Flags & SubMeshFlags.HasTransform) != (SubMeshFlags) 0)
                        v = subMesh.m_Position + math.rotate(subMesh.m_Rotation, v);
                      bounds3 |= v;
                      ++num1;
                      break;
                    case BoneType.PantographRotation:
                      trainData.m_TrainFlags |= TrainFlags.Pantograph;
                      break;
                  }
                }
              }
            }
          }
          if (num1 >= 2)
            trainData.m_BogieOffsets = new float2(bounds3.max.z, -bounds3.min.z) - trainData.m_BogieOffsets;
          else if (num1 == 1)
          {
            float num2 = MathUtils.Size(objectGeometryData.m_Bounds.x) * 0.5f;
            trainData.m_BogieOffsets = MathUtils.Center(bounds3.z) + num2 - trainData.m_BogieOffsets;
          }
          else
          {
            float2 float2 = new float2(objectGeometryData.m_Bounds.max.z, -objectGeometryData.m_Bounds.min.z);
            trainData.m_BogieOffsets = float2 - MathUtils.Size(objectGeometryData.m_Bounds.z) * 0.15f - trainData.m_BogieOffsets;
          }
          nativeArray4[index7] = trainData;
        }
      }

      private bool HasSteering(DynamicBuffer<ProceduralBone> bones, ProceduralBone bone)
      {
        if (bone.m_Type == BoneType.SteeringTire || bone.m_Type == BoneType.SteeringRotation || bone.m_Type == BoneType.SteeringSuspension)
          return true;
        while (bone.m_ParentIndex >= 0)
        {
          bone = bones[bone.m_ParentIndex];
          if (bone.m_Type == BoneType.SteeringTire || bone.m_Type == BoneType.SteeringRotation || bone.m_Type == BoneType.SteeringSuspension)
            return true;
        }
        return false;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MultipleUnitTrainData> __Game_Prefabs_MultipleUnitTrainData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<TrainData> __Game_Prefabs_TrainData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarData> __Game_Prefabs_CarData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<SwayingData> __Game_Prefabs_SwayingData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<VehicleData> __Game_Prefabs_VehicleData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarTractorData> __Game_Prefabs_CarTractorData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarTrailerData> __Game_Prefabs_CarTrailerData_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubMesh> __Game_Prefabs_SubMesh_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MultipleUnitTrainData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MultipleUnitTrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TrainData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SwayingData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SwayingData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<VehicleData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTractorData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarTractorData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTrailerData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarTrailerData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
      }
    }
  }
}
