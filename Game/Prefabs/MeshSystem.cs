// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Mathematics;
using Colossal.Rendering;
using Game.Common;
using Game.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class MeshSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private BatchManagerSystem m_BatchManagerSystem;
    private ManagedBatchSystem m_ManagedBatchSystem;
    private EntityQuery m_PrefabQuery;
    private System.Collections.Generic.Dictionary<MeshSystem.MaterialKey, int> m_MaterialIndex;
    private MeshSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatchSystem = this.World.GetOrCreateSystemManaged<ManagedBatchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadWrite<MeshData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MaterialIndex = new System.Collections.Generic.Dictionary<MeshSystem.MaterialKey, int>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    public int GetMaterialIndex(SurfaceAsset surface)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MeshSystem.MaterialKey key = new MeshSystem.MaterialKey(surface);
      int materialIndex;
      // ISSUE: reference to a compiler-generated field
      if (this.m_MaterialIndex.TryGetValue(key, out materialIndex))
        return materialIndex;
      // ISSUE: reference to a compiler-generated field
      int count = this.m_MaterialIndex.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_MaterialIndex.Add(key, count);
      return count;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Deleted> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<MeshData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_MeshData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LodMesh_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<LodMesh> bufferTypeHandle1 = this.__TypeHandle.__Game_Prefabs_LodMesh_RW_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<ProceduralBone> bufferTypeHandle2 = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RW_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralLight_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<ProceduralLight> bufferTypeHandle3 = this.__TypeHandle.__Game_Prefabs_ProceduralLight_RW_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LightAnimation_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<LightAnimation> bufferTypeHandle4 = this.__TypeHandle.__Game_Prefabs_LightAnimation_RW_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshMaterial_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<MeshMaterial> bufferTypeHandle5 = this.__TypeHandle.__Game_Prefabs_MeshMaterial_RW_BufferTypeHandle;
      bool flag = false;
      this.CompleteDependency();
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle2);
        if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
        {
          NativeArray<Entity> nativeArray2 = archetypeChunk.GetNativeArray(entityTypeHandle);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            if (nativeArray1[index2].m_Index < 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_ManagedBatchSystem.RemoveMesh(nativeArray2[index2]);
              flag = true;
            }
          }
        }
        else
        {
          NativeArray<MeshData> nativeArray3 = archetypeChunk.GetNativeArray<MeshData>(ref componentTypeHandle3);
          BufferAccessor<LodMesh> bufferAccessor1 = archetypeChunk.GetBufferAccessor<LodMesh>(ref bufferTypeHandle1);
          BufferAccessor<ProceduralBone> bufferAccessor2 = archetypeChunk.GetBufferAccessor<ProceduralBone>(ref bufferTypeHandle2);
          BufferAccessor<ProceduralLight> bufferAccessor3 = archetypeChunk.GetBufferAccessor<ProceduralLight>(ref bufferTypeHandle3);
          BufferAccessor<LightAnimation> bufferAccessor4 = archetypeChunk.GetBufferAccessor<LightAnimation>(ref bufferTypeHandle4);
          BufferAccessor<MeshMaterial> bufferAccessor5 = archetypeChunk.GetBufferAccessor<MeshMaterial>(ref bufferTypeHandle5);
          for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            RenderPrefab prefab = this.m_PrefabSystem.GetPrefab<RenderPrefab>(nativeArray1[index3]);
            MeshData meshData = nativeArray3[index3] with
            {
              m_Bounds = prefab.bounds,
              m_SubMeshCount = prefab.meshCount,
              m_IndexCount = prefab.indexCount,
              m_SmoothingDistance = 1f / 1000f,
              m_ShadowBias = 0.5f
            };
            if (prefab.meshCount != prefab.materialCount)
              COSystemBase.baseLog.WarnFormat((UnityEngine.Object) prefab, "{0}: subMeshCount ({1}) != materialCount ({2})", (object) prefab.name, (object) prefab.meshCount, (object) prefab.materialCount);
            if (bufferAccessor5.Length != 0)
            {
              int materialCount = prefab.materialCount;
              DynamicBuffer<MeshMaterial> dynamicBuffer = bufferAccessor5[index3];
              dynamicBuffer.ResizeUninitialized(materialCount);
              int num = 0;
              foreach (SurfaceAsset surfaceAsset in prefab.surfaceAssets)
              {
                // ISSUE: reference to a compiler-generated method
                dynamicBuffer[num++] = new MeshMaterial()
                {
                  m_MaterialIndex = this.GetMaterialIndex(surfaceAsset)
                };
              }
            }
            if (prefab.isImpostor)
              meshData.m_State |= MeshFlags.Impostor;
            if (bufferAccessor1.Length != 0)
            {
              LodProperties component = prefab.GetComponent<LodProperties>();
              DynamicBuffer<LodMesh> dynamicBuffer = bufferAccessor1[index3];
              meshData.m_LodBias = component.m_Bias;
              meshData.m_ShadowBias += component.m_Bias + component.m_ShadowBias;
              if (component.m_LodMeshes != null)
              {
                dynamicBuffer.ResizeUninitialized(component.m_LodMeshes.Length);
                for (int index4 = 0; index4 < component.m_LodMeshes.Length; ++index4)
                {
                  RenderPrefab lodMesh1 = component.m_LodMeshes[index4];
                  int index5 = index4;
                  for (int index6 = index4 - 1; index6 >= 0; --index6)
                  {
                    RenderPrefab lodMesh2 = component.m_LodMeshes[index6];
                    if (lodMesh1.indexCount > lodMesh2.indexCount)
                    {
                      dynamicBuffer[index5] = dynamicBuffer[index6];
                      index5 = index6;
                    }
                    else
                      break;
                  }
                  LodMesh lodMesh3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  lodMesh3.m_LodMesh = this.m_PrefabSystem.GetEntity((PrefabBase) lodMesh1);
                  dynamicBuffer[index5] = lodMesh3;
                }
              }
            }
            if ((double) prefab.surfaceArea > 0.0)
            {
              float3 x = meshData.m_Bounds.max - meshData.m_Bounds.min;
              float num1 = math.log2(math.sqrt(math.clamp(prefab.surfaceArea / (math.csum(x * x.yzx) * 2f), 1E-06f, 1f)));
              float num2 = math.log2(math.sqrt(math.clamp(math.cmax(math.min(x, x.yzx)) * 3f / math.csum(x), 1E-06f, 1f)));
              meshData.m_LodBias -= num1;
              meshData.m_ShadowBias -= 1.5f * num1 + num2;
            }
            if (bufferAccessor2.Length != 0)
            {
              ProceduralAnimationProperties component = prefab.GetComponent<ProceduralAnimationProperties>();
              if (component.m_Bones != null)
              {
                DynamicBuffer<ProceduralBone> dynamicBuffer = bufferAccessor2[index3];
                dynamicBuffer.ResizeUninitialized(component.m_Bones.Length);
                for (int index7 = 0; index7 < component.m_Bones.Length; ++index7)
                {
                  ProceduralAnimationProperties.BoneInfo bone = component.m_Bones[index7];
                  float num3;
                  float num4;
                  switch (bone.m_Type)
                  {
                    case BoneType.LookAtDirection:
                    case BoneType.WindTurbineRotation:
                    case BoneType.WindSpeedRotation:
                    case BoneType.PoweredRotation:
                    case BoneType.TrafficBarrierDirection:
                    case BoneType.RollingRotation:
                    case BoneType.PropellerRotation:
                    case BoneType.LookAtRotation:
                    case BoneType.LookAtAim:
                    case BoneType.PropellerAngle:
                    case BoneType.PantographRotation:
                    case BoneType.WorkingRotation:
                    case BoneType.OperatingRotation:
                    case BoneType.TimeRotation:
                    case BoneType.LookAtRotationSide:
                      num3 = bone.m_Speed * 6.28318548f;
                      num4 = bone.m_Acceleration * 6.28318548f;
                      break;
                    default:
                      num3 = bone.m_Speed;
                      num4 = bone.m_Acceleration;
                      break;
                  }
                  int p3 = bone.m_ConnectionID;
                  if (p3 < 0 || p3 > 900)
                  {
                    COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, "{0}: boneInfo[{1}].ConnectionID ({2}) != 0->900", (object) prefab.name, (object) index7, (object) p3);
                    p3 = 0;
                  }
                  dynamicBuffer[index7] = new ProceduralBone()
                  {
                    m_Position = (float3) bone.position,
                    m_Rotation = (quaternion) bone.rotation,
                    m_Scale = (float3) bone.scale,
                    m_BindPose = (float4x4) bone.bindPose,
                    m_ParentIndex = bone.parentId,
                    m_BindIndex = index7,
                    m_Type = bone.m_Type,
                    m_ConnectionID = p3,
                    m_Speed = num3,
                    m_Acceleration = num4
                  };
                }
              }
            }
            if (bufferAccessor3.Length != 0)
            {
              EmissiveProperties component = prefab.GetComponent<EmissiveProperties>();
              if (component.hasAnyLights)
              {
                DynamicBuffer<ProceduralLight> dynamicBuffer1 = bufferAccessor3[index3];
                dynamicBuffer1.ResizeUninitialized(component.lightsCount);
                int num5 = 0;
                int num6 = 0;
                if (bufferAccessor4.Length != 0)
                {
                  DynamicBuffer<LightAnimation> dynamicBuffer2 = bufferAccessor4[index3];
                  int length = 0;
                  if (component.m_SignalGroupAnimations != null)
                    length += component.m_SignalGroupAnimations.Count;
                  num5 = length;
                  if (component.m_AnimationCurves != null)
                  {
                    length += component.m_AnimationCurves.Count;
                    num6 = component.m_AnimationCurves.Count;
                  }
                  dynamicBuffer2.ResizeUninitialized(length);
                  LightAnimation lightAnimation1;
                  if (component.m_SignalGroupAnimations != null)
                  {
                    for (int index8 = 0; index8 < component.m_SignalGroupAnimations.Count; ++index8)
                    {
                      EmissiveProperties.SignalGroupAnimation signalGroupAnimation = component.m_SignalGroupAnimations[index8];
                      ref DynamicBuffer<LightAnimation> local = ref dynamicBuffer2;
                      int index9 = index8;
                      lightAnimation1 = new LightAnimation();
                      lightAnimation1.m_DurationFrames = (uint) math.max(1, Mathf.RoundToInt(signalGroupAnimation.m_Duration * 60f));
                      lightAnimation1.m_SignalAnimation = new SignalAnimation(signalGroupAnimation.m_SignalGroupMasks);
                      LightAnimation lightAnimation2 = lightAnimation1;
                      local[index9] = lightAnimation2;
                    }
                  }
                  if (component.m_AnimationCurves != null)
                  {
                    for (int index10 = 0; index10 < component.m_AnimationCurves.Count; ++index10)
                    {
                      EmissiveProperties.AnimationProperties animationCurve = component.m_AnimationCurves[index10];
                      ref DynamicBuffer<LightAnimation> local = ref dynamicBuffer2;
                      int index11 = num5 + index10;
                      lightAnimation1 = new LightAnimation();
                      lightAnimation1.m_DurationFrames = (uint) math.max(1, Mathf.RoundToInt(animationCurve.m_Duration * 60f));
                      lightAnimation1.m_AnimationCurve = new AnimationCurve1(animationCurve.m_Curve);
                      LightAnimation lightAnimation3 = lightAnimation1;
                      local[index11] = lightAnimation3;
                    }
                  }
                }
                int num7 = 0;
                ProceduralLight proceduralLight1;
                if (component.hasMultiLights)
                {
                  num7 = component.m_MultiLights.Count;
                  for (int index12 = 0; index12 < component.m_MultiLights.Count; ++index12)
                  {
                    EmissiveProperties.MultiLightMapping multiLight = component.m_MultiLights[index12];
                    Color linear1 = multiLight.color.linear;
                    Color linear2 = multiLight.colorOff.linear;
                    ref DynamicBuffer<ProceduralLight> local = ref dynamicBuffer1;
                    int index13 = index12;
                    proceduralLight1 = new ProceduralLight();
                    proceduralLight1.m_Color = new float4(linear1.r, linear1.g, linear1.b, multiLight.intensity * 100f);
                    proceduralLight1.m_Color2 = new float4(linear2.r, linear2.g, linear2.b, multiLight.intensity * 100f);
                    proceduralLight1.m_Purpose = multiLight.purpose;
                    proceduralLight1.m_ResponseSpeed = 1f / math.max(1f / 1000f, multiLight.responseTime);
                    proceduralLight1.m_AnimationIndex = math.select(-1, num5 + multiLight.animationIndex, multiLight.animationIndex >= 0 && multiLight.animationIndex < num6);
                    ProceduralLight proceduralLight2 = proceduralLight1;
                    local[index13] = proceduralLight2;
                  }
                }
                if (component.hasSingleLights)
                {
                  for (int index14 = 0; index14 < component.m_SingleLights.Count; ++index14)
                  {
                    EmissiveProperties.SingleLightMapping singleLight = component.m_SingleLights[index14];
                    Color linear3 = singleLight.color.linear;
                    Color linear4 = singleLight.colorOff.linear;
                    ref DynamicBuffer<ProceduralLight> local = ref dynamicBuffer1;
                    int index15 = num7 + index14;
                    proceduralLight1 = new ProceduralLight();
                    proceduralLight1.m_Color = new float4(linear3.r, linear3.g, linear3.b, singleLight.intensity * 100f);
                    proceduralLight1.m_Color2 = new float4(linear4.r, linear4.g, linear4.b, singleLight.intensity * 100f);
                    proceduralLight1.m_Purpose = singleLight.purpose;
                    proceduralLight1.m_ResponseSpeed = 1f / math.max(1f / 1000f, singleLight.responseTime);
                    proceduralLight1.m_AnimationIndex = math.select(-1, num5 + singleLight.animationIndex, singleLight.animationIndex >= 0 && singleLight.animationIndex < num6);
                    ProceduralLight proceduralLight3 = proceduralLight1;
                    local[index15] = proceduralLight3;
                  }
                }
              }
            }
            UndergroundMesh component1 = prefab.GetComponent<UndergroundMesh>();
            if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
            {
              if (component1.m_IsTunnel)
                meshData.m_DefaultLayers |= MeshLayer.Tunnel;
              if (component1.m_IsPipeline)
                meshData.m_DefaultLayers |= MeshLayer.Pipeline;
              if (component1.m_IsSubPipeline)
                meshData.m_DefaultLayers |= MeshLayer.SubPipeline;
            }
            OverlayProperties component2 = prefab.GetComponent<OverlayProperties>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.m_IsWaterway)
              meshData.m_DefaultLayers |= MeshLayer.Waterway;
            if ((UnityEngine.Object) prefab.GetComponent<DecalProperties>() != (UnityEngine.Object) null)
              meshData.m_State |= MeshFlags.Decal;
            StackProperties component3 = prefab.GetComponent<StackProperties>();
            if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
            {
              switch (component3.m_Direction)
              {
                case StackDirection.Right:
                  meshData.m_State |= MeshFlags.StackX;
                  break;
                case StackDirection.Up:
                  meshData.m_State |= MeshFlags.StackY;
                  break;
                case StackDirection.Forward:
                  meshData.m_State |= MeshFlags.StackZ;
                  break;
              }
            }
            if ((UnityEngine.Object) prefab.GetComponent<AnimationProperties>() != (UnityEngine.Object) null)
              meshData.m_State |= MeshFlags.Animated;
            if ((UnityEngine.Object) prefab.GetComponent<ProceduralAnimationProperties>() != (UnityEngine.Object) null)
              meshData.m_State |= MeshFlags.Skeleton;
            CurveProperties component4 = prefab.GetComponent<CurveProperties>();
            if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
            {
              meshData.m_TilingCount = component4.m_TilingCount;
              if ((double) component4.m_OverrideLength != 0.0)
              {
                meshData.m_Bounds.min.z = component4.m_OverrideLength * -0.5f;
                meshData.m_Bounds.max.z = component4.m_OverrideLength * 0.5f;
              }
              if ((double) component4.m_SmoothingDistance > (double) meshData.m_SmoothingDistance)
                meshData.m_SmoothingDistance = component4.m_SmoothingDistance;
              if (component4.m_GeometryTiling)
                meshData.m_State |= MeshFlags.Tiling;
              if (component4.m_InvertCurve)
                meshData.m_State |= MeshFlags.Invert;
            }
            BaseProperties component5 = prefab.GetComponent<BaseProperties>();
            if ((UnityEngine.Object) component5 != (UnityEngine.Object) null && (UnityEngine.Object) component5.m_BaseType != (UnityEngine.Object) null)
            {
              meshData.m_State |= MeshFlags.Base;
              if (component5.m_UseMinBounds)
                meshData.m_State |= MeshFlags.MinBounds;
            }
            if (prefab.Has<DefaultMesh>())
            {
              float renderingSize = RenderingUtils.GetRenderingSize(MathUtils.Size(meshData.m_Bounds));
              meshData.m_State |= MeshFlags.Default;
              meshData.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(renderingSize, meshData.m_LodBias);
              meshData.m_ShadowLod = (byte) RenderingUtils.CalculateLodLimit(renderingSize, meshData.m_ShadowBias);
            }
            nativeArray3[index3] = meshData;
          }
        }
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MeshSystem.InitializeMeshJob jobData = new MeshSystem.InitializeMeshJob()
      {
        m_Chunks = archetypeChunkArray,
        m_DeletedType = componentTypeHandle1,
        m_ProceduralBoneType = bufferTypeHandle2
      };
      this.Dependency = jobData.Schedule<MeshSystem.InitializeMeshJob>(archetypeChunkArray.Length, 1, this.Dependency);
      if (!flag)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BatchGroup_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_FadeBatch_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new MeshSystem.RemoveBatchGroupsJob()
      {
        m_EntityType = entityTypeHandle,
        m_DeletedType = componentTypeHandle1,
        m_PrefabDataType = componentTypeHandle2,
        m_MeshBatches = this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferLookup,
        m_FadeBatches = this.__TypeHandle.__Game_Rendering_FadeBatch_RW_BufferLookup,
        m_BatchGroups = this.__TypeHandle.__Game_Prefabs_BatchGroup_RW_BufferLookup,
        m_NativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies1),
        m_NativeBatchInstances = this.m_BatchManagerSystem.GetNativeBatchInstances(false, out dependencies2),
        m_NativeSubBatches = this.m_BatchManagerSystem.GetNativeSubBatches(false, out dependencies3)
      }.Schedule<MeshSystem.RemoveBatchGroupsJob>(this.m_PrefabQuery, JobUtils.CombineDependencies(this.Dependency, dependencies1, dependencies2, dependencies3));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchGroupsWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchInstancesWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeSubBatchesWriter(jobHandle);
      this.Dependency = jobHandle;
    }

    public int GetMaterialIndex(Material material)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MeshSystem.MaterialKey key = new MeshSystem.MaterialKey(material);
      int materialIndex;
      // ISSUE: reference to a compiler-generated field
      if (this.m_MaterialIndex.TryGetValue(key, out materialIndex))
        return materialIndex;
      // ISSUE: reference to a compiler-generated field
      int count = this.m_MaterialIndex.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_MaterialIndex.Add(key, count);
      return count;
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
    public MeshSystem()
    {
    }

    [BurstCompile]
    private struct RemoveBatchGroupsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> m_PrefabDataType;
      public BufferLookup<MeshBatch> m_MeshBatches;
      public BufferLookup<FadeBatch> m_FadeBatches;
      public BufferLookup<BatchGroup> m_BatchGroups;
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchInstances;
      public NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> m_NativeSubBatches;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<Deleted>(ref this.m_DeletedType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabData> nativeArray2 = chunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          if (nativeArray2[index1].m_Index < 0)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<BatchGroup> batchGroup = this.m_BatchGroups[nativeArray1[index1]];
            for (int index2 = 0; index2 < batchGroup.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated method
              this.RemoveBatchGroup(batchGroup[index2].m_GroupIndex, batchGroup[index2].m_MergeIndex);
            }
            batchGroup.Clear();
          }
        }
      }

      private void RemoveBatchGroup(int groupIndex, int mergeIndex)
      {
        int groupIndex1 = groupIndex;
        if (mergeIndex != -1)
        {
          // ISSUE: reference to a compiler-generated field
          groupIndex1 = this.m_NativeBatchGroups.GetMergedGroupIndex(groupIndex, mergeIndex);
          // ISSUE: reference to a compiler-generated field
          this.m_NativeBatchGroups.RemoveMergedGroup(groupIndex, mergeIndex);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          int mergedGroupCount = this.m_NativeBatchGroups.GetMergedGroupCount(groupIndex);
          if (mergedGroupCount != 0)
          {
            // ISSUE: reference to a compiler-generated field
            int mergedGroupIndex1 = this.m_NativeBatchGroups.GetMergedGroupIndex(groupIndex, 0);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<BatchGroup> batchGroup1 = this.m_BatchGroups[this.m_NativeBatchGroups.GetGroupData(mergedGroupIndex1).m_Mesh];
            for (int index = 0; index < batchGroup1.Length; ++index)
            {
              BatchGroup batchGroup2 = batchGroup1[index];
              if (batchGroup2.m_GroupIndex == mergedGroupIndex1)
              {
                batchGroup2.m_MergeIndex = -1;
                batchGroup1[index] = batchGroup2;
                break;
              }
            }
            for (int index1 = 1; index1 < mergedGroupCount; ++index1)
            {
              // ISSUE: reference to a compiler-generated field
              int mergedGroupIndex2 = this.m_NativeBatchGroups.GetMergedGroupIndex(groupIndex, index1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              batchGroup1 = this.m_BatchGroups[this.m_NativeBatchGroups.GetGroupData(mergedGroupIndex2).m_Mesh];
              // ISSUE: reference to a compiler-generated field
              this.m_NativeBatchGroups.AddMergedGroup(mergedGroupIndex1, mergedGroupIndex2);
              for (int index2 = 0; index2 < batchGroup1.Length; ++index2)
              {
                BatchGroup batchGroup3 = batchGroup1[index2];
                if (batchGroup3.m_GroupIndex == mergedGroupIndex2)
                {
                  batchGroup3.m_MergeIndex = mergedGroupIndex1;
                  batchGroup1[index1] = batchGroup3;
                  break;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        int instanceCount = this.m_NativeBatchInstances.GetInstanceCount(groupIndex);
        for (int instanceIndex = 0; instanceIndex < instanceCount; ++instanceIndex)
        {
          // ISSUE: reference to a compiler-generated field
          InstanceData instanceData = this.m_NativeBatchInstances.GetInstanceData(groupIndex, instanceIndex);
          DynamicBuffer<MeshBatch> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MeshBatches.TryGetBuffer(instanceData.m_Entity, out bufferData1))
          {
            for (int index = 0; index < bufferData1.Length; ++index)
            {
              MeshBatch meshBatch = bufferData1[index];
              if (meshBatch.m_GroupIndex == groupIndex && meshBatch.m_InstanceIndex == instanceIndex)
              {
                DynamicBuffer<FadeBatch> bufferData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_FadeBatches.TryGetBuffer(instanceData.m_Entity, out bufferData2))
                {
                  bufferData1.RemoveAtSwapBack(index);
                  bufferData2.RemoveAtSwapBack(index);
                  break;
                }
                meshBatch.m_GroupIndex = -1;
                meshBatch.m_InstanceIndex = -1;
                bufferData1[index] = meshBatch;
                break;
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NativeBatchInstances.RemoveInstances(groupIndex, this.m_NativeSubBatches);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NativeBatchGroups.DestroyGroup(groupIndex1, this.m_NativeBatchInstances, this.m_NativeSubBatches);
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
    private struct InitializeMeshJob : IJobParallelFor
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      public BufferTypeHandle<ProceduralBone> m_ProceduralBoneType;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
          return;
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ProceduralBone> bufferAccessor = chunk.GetBufferAccessor<ProceduralBone>(ref this.m_ProceduralBoneType);
        if (bufferAccessor.Length == 0)
          return;
        NativeList<int> nativeList = new NativeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<ProceduralBone> dynamicBuffer = bufferAccessor[index1];
          if (dynamicBuffer.Length > 1)
          {
            nativeList.ResizeUninitialized(dynamicBuffer.Length);
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              ProceduralBone proceduralBone = dynamicBuffer[index2];
              if (proceduralBone.m_ParentIndex >= 0 && proceduralBone.m_HierarchyDepth == 0)
              {
                int index3 = index2;
                int num = 0;
                do
                {
                  proceduralBone.m_HierarchyDepth = 1;
                  dynamicBuffer[index3] = proceduralBone;
                  nativeList[num++] = index3;
                  index3 = proceduralBone.m_ParentIndex;
                  proceduralBone = dynamicBuffer[index3];
                }
                while (proceduralBone.m_ParentIndex >= 0 && proceduralBone.m_HierarchyDepth == 0);
                while (num != 0)
                {
                  int hierarchyDepth = proceduralBone.m_HierarchyDepth;
                  int index4 = nativeList[--num];
                  proceduralBone = dynamicBuffer[index4];
                  proceduralBone.m_HierarchyDepth += hierarchyDepth;
                  dynamicBuffer[index4] = proceduralBone;
                }
              }
            }
            NativeArray<ProceduralBone> array = dynamicBuffer.AsNativeArray();
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            MeshSystem.InitializeMeshJob.BoneParentComparer boneParentComparer = new MeshSystem.InitializeMeshJob.BoneParentComparer();
            // ISSUE: variable of a compiler-generated type
            MeshSystem.InitializeMeshJob.BoneParentComparer comp = boneParentComparer;
            array.Sort<ProceduralBone, MeshSystem.InitializeMeshJob.BoneParentComparer>(comp);
            for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
            {
              ProceduralBone proceduralBone = dynamicBuffer[index5];
              nativeList[proceduralBone.m_BindIndex] = index5;
            }
            for (int index6 = 0; index6 < dynamicBuffer.Length; ++index6)
            {
              ProceduralBone proceduralBone1 = dynamicBuffer[index6];
              if (proceduralBone1.m_ParentIndex >= 0)
              {
                proceduralBone1.m_ParentIndex = nativeList[proceduralBone1.m_ParentIndex];
                ProceduralBone proceduralBone2 = dynamicBuffer[proceduralBone1.m_ParentIndex];
                proceduralBone1.m_ObjectPosition = proceduralBone2.m_ObjectPosition + math.mul(proceduralBone2.m_ObjectRotation, proceduralBone1.m_Position);
                proceduralBone1.m_ObjectRotation = math.mul(proceduralBone2.m_ObjectRotation, proceduralBone1.m_Rotation);
              }
              else
              {
                proceduralBone1.m_ObjectPosition = proceduralBone1.m_Position;
                proceduralBone1.m_ObjectRotation = proceduralBone1.m_Rotation;
              }
              dynamicBuffer[index6] = proceduralBone1;
            }
          }
        }
        nativeList.Dispose();
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct BoneParentComparer : IComparer<ProceduralBone>
      {
        public int Compare(ProceduralBone x, ProceduralBone y)
        {
          return math.select(x.m_ParentIndex - y.m_ParentIndex, x.m_HierarchyDepth - y.m_HierarchyDepth, x.m_HierarchyDepth != y.m_HierarchyDepth);
        }
      }
    }

    private struct MaterialKey : IEquatable<MeshSystem.MaterialKey>
    {
      private Material m_Material;
      private SurfaceAsset m_Surface;

      public MaterialKey(Material material)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Material = material;
        // ISSUE: reference to a compiler-generated field
        this.m_Surface = (SurfaceAsset) null;
      }

      public MaterialKey(SurfaceAsset surface)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Material = (Material) null;
        // ISSUE: reference to a compiler-generated field
        this.m_Surface = surface;
      }

      public bool Equals(MeshSystem.MaterialKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (UnityEngine.Object) this.m_Material == (UnityEngine.Object) other.m_Material && this.m_Surface.Equals((IAssetData) other.m_Surface);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (UnityEngine.Object) this.m_Material != (UnityEngine.Object) null ? this.m_Material.GetHashCode() : this.m_Surface.GetHashCode();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<MeshData> __Game_Prefabs_MeshData_RW_ComponentTypeHandle;
      public BufferTypeHandle<LodMesh> __Game_Prefabs_LodMesh_RW_BufferTypeHandle;
      public BufferTypeHandle<ProceduralBone> __Game_Prefabs_ProceduralBone_RW_BufferTypeHandle;
      public BufferTypeHandle<ProceduralLight> __Game_Prefabs_ProceduralLight_RW_BufferTypeHandle;
      public BufferTypeHandle<LightAnimation> __Game_Prefabs_LightAnimation_RW_BufferTypeHandle;
      public BufferTypeHandle<MeshMaterial> __Game_Prefabs_MeshMaterial_RW_BufferTypeHandle;
      public BufferLookup<MeshBatch> __Game_Rendering_MeshBatch_RW_BufferLookup;
      public BufferLookup<FadeBatch> __Game_Rendering_FadeBatch_RW_BufferLookup;
      public BufferLookup<BatchGroup> __Game_Prefabs_BatchGroup_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MeshData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LodMesh_RW_BufferTypeHandle = state.GetBufferTypeHandle<LodMesh>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RW_BufferTypeHandle = state.GetBufferTypeHandle<ProceduralBone>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralLight_RW_BufferTypeHandle = state.GetBufferTypeHandle<ProceduralLight>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LightAnimation_RW_BufferTypeHandle = state.GetBufferTypeHandle<LightAnimation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshMaterial_RW_BufferTypeHandle = state.GetBufferTypeHandle<MeshMaterial>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshBatch_RW_BufferLookup = state.GetBufferLookup<MeshBatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_FadeBatch_RW_BufferLookup = state.GetBufferLookup<FadeBatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BatchGroup_RW_BufferLookup = state.GetBufferLookup<BatchGroup>();
      }
    }
  }
}
