// Decompiled with JetBrains decompiler
// Type: Game.Rendering.InitializeAnimatedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class InitializeAnimatedSystem : GameSystemBase
  {
    private AnimatedSystem m_AnimatedSystem;
    private PreCullingSystem m_PreCullingSystem;
    private InitializeAnimatedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedSystem = this.World.GetOrCreateSystemManaged<AnimatedSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OverlayElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CharacterStyleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new InitializeAnimatedSystem.InitializeAnimatedJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_CharacterStyleData = this.__TypeHandle.__Game_Prefabs_CharacterStyleData_RO_ComponentLookup,
        m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
        m_MeshColors = this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup,
        m_AnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
        m_OverlayElements = this.__TypeHandle.__Game_Prefabs_OverlayElement_RO_BufferLookup,
        m_Animateds = this.__TypeHandle.__Game_Rendering_Animated_RW_BufferLookup,
        m_CullingData = this.m_PreCullingSystem.GetUpdatedData(true, out dependencies1),
        m_AllocationData = this.m_AnimatedSystem.GetAllocationData(out dependencies2)
      }.Schedule<InitializeAnimatedSystem.InitializeAnimatedJob>(JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AnimatedSystem.AddAllocationWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle);
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
    public InitializeAnimatedSystem()
    {
    }

    [BurstCompile]
    private struct InitializeAnimatedJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CharacterStyleData> m_CharacterStyleData;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [ReadOnly]
      public BufferLookup<MeshColor> m_MeshColors;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.AnimationClip> m_AnimationClips;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<CharacterElement> m_CharacterElements;
      [ReadOnly]
      public BufferLookup<OverlayElement> m_OverlayElements;
      public BufferLookup<Animated> m_Animateds;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      public AnimatedSystem.AllocationData m_AllocationData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_CullingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          PreCullingData cullingData = this.m_CullingData[index];
          if ((cullingData.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.BatchesUpdated)) != (PreCullingFlags) 0 && (cullingData.m_Flags & PreCullingFlags.Animated) != (PreCullingFlags) 0)
          {
            if ((cullingData.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Remove(cullingData);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Update(cullingData);
            }
          }
        }
      }

      private void Remove(PreCullingData cullingData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Animated> animated = this.m_Animateds[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated method
        this.Deallocate(animated);
        animated.Clear();
      }

      private unsafe void Update(PreCullingData cullingData)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        DynamicBuffer<SubMesh> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubMeshes.TryGetBuffer(prefabRef.m_Prefab, out bufferData1))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Animated> animated1 = this.m_Animateds[cullingData.m_Entity];
          DynamicBuffer<MeshGroup> bufferData2 = new DynamicBuffer<MeshGroup>();
          DynamicBuffer<MeshColor> bufferData3 = new DynamicBuffer<MeshColor>();
          DynamicBuffer<CharacterElement> bufferData4 = new DynamicBuffer<CharacterElement>();
          int length = bufferData1.Length;
          DynamicBuffer<SubMeshGroup> bufferData5;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData5))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshGroups.TryGetBuffer(cullingData.m_Entity, out bufferData2))
            {
              length = bufferData2.Length;
              // ISSUE: reference to a compiler-generated field
              this.m_MeshColors.TryGetBuffer(cullingData.m_Entity, out bufferData3);
            }
            else
              length = 1;
            // ISSUE: reference to a compiler-generated field
            this.m_CharacterElements.TryGetBuffer(prefabRef.m_Prefab, out bufferData4);
          }
          bool flag = animated1.Length != length;
          if (!flag && (cullingData.m_Flags & (PreCullingFlags.Updated | PreCullingFlags.BatchesUpdated)) == (PreCullingFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.Deallocate(animated1);
          animated1.ResizeUninitialized(length);
          InitializeAnimatedSystem.OverlayIndex* overlayIndexPtr = stackalloc InitializeAnimatedSystem.OverlayIndex[8];
          for (int index1 = 0; index1 < length; ++index1)
          {
            Animated animated2;
            if (flag)
              animated2 = new Animated()
              {
                m_ClipIndexBody0 = (short) -1,
                m_ClipIndexBody0I = (short) -1,
                m_ClipIndexBody1 = (short) -1,
                m_ClipIndexBody1I = (short) -1,
                m_ClipIndexFace0 = (short) -1,
                m_ClipIndexFace1 = (short) -1
              };
            else
              animated2 = animated1[index1];
            Entity entity;
            AnimationLayerMask animationLayerMask;
            if (bufferData4.IsCreated)
            {
              MeshGroup meshGroup;
              CollectionUtils.TryGet<MeshGroup>(bufferData2, index1, out meshGroup);
              SubMeshGroup subMeshGroup = bufferData5[(int) meshGroup.m_SubMeshGroup];
              CharacterElement characterElement = bufferData4[(int) meshGroup.m_SubMeshGroup];
              // ISSUE: reference to a compiler-generated field
              CharacterStyleData characterStyleData = this.m_CharacterStyleData[characterElement.m_Style];
              entity = characterElement.m_Style;
              animationLayerMask = characterStyleData.m_AnimationLayerMask;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              animated2.m_BoneAllocation = this.m_AllocationData.AllocateBones(characterStyleData.m_BoneCount);
              MetaBufferData metaBufferData = new MetaBufferData()
              {
                m_BoneOffset = (int) animated2.m_BoneAllocation.Begin,
                m_BoneCount = characterStyleData.m_BoneCount,
                m_ShapeCount = characterStyleData.m_ShapeCount,
                m_ShapeWeights = characterElement.m_ShapeWeights,
                m_TextureWeights = characterElement.m_TextureWeights,
                m_OverlayWeights = characterElement.m_OverlayWeights,
                m_MaskWeights = characterElement.m_MaskWeights
              };
              DynamicBuffer<OverlayElement> bufferData6 = new DynamicBuffer<OverlayElement>();
              int x = subMeshGroup.m_SubMeshRange.x;
              // ISSUE: reference to a compiler-generated field
              while (x < subMeshGroup.m_SubMeshRange.y && !this.m_OverlayElements.TryGetBuffer(bufferData1[x].m_SubMesh, out bufferData6))
                ++x;
              // ISSUE: reference to a compiler-generated method
              this.AddOverlayIndex(overlayIndexPtr, 0, bufferData6, characterElement.m_OverlayWeights.m_Weight0);
              // ISSUE: reference to a compiler-generated method
              this.AddOverlayIndex(overlayIndexPtr, 1, bufferData6, characterElement.m_OverlayWeights.m_Weight1);
              // ISSUE: reference to a compiler-generated method
              this.AddOverlayIndex(overlayIndexPtr, 2, bufferData6, characterElement.m_OverlayWeights.m_Weight2);
              // ISSUE: reference to a compiler-generated method
              this.AddOverlayIndex(overlayIndexPtr, 3, bufferData6, characterElement.m_OverlayWeights.m_Weight3);
              // ISSUE: reference to a compiler-generated method
              this.AddOverlayIndex(overlayIndexPtr, 4, bufferData6, characterElement.m_OverlayWeights.m_Weight4);
              // ISSUE: reference to a compiler-generated method
              this.AddOverlayIndex(overlayIndexPtr, 5, bufferData6, characterElement.m_OverlayWeights.m_Weight5);
              // ISSUE: reference to a compiler-generated method
              this.AddOverlayIndex(overlayIndexPtr, 6, bufferData6, characterElement.m_OverlayWeights.m_Weight6);
              // ISSUE: reference to a compiler-generated method
              this.AddOverlayIndex(overlayIndexPtr, 7, bufferData6, characterElement.m_OverlayWeights.m_Weight7);
              NativeSortExtension.Sort<InitializeAnimatedSystem.OverlayIndex>(overlayIndexPtr, 8);
              // ISSUE: reference to a compiler-generated field
              metaBufferData.m_OverlayWeights.m_Weight0 = overlayIndexPtr->m_Weight;
              // ISSUE: reference to a compiler-generated field
              metaBufferData.m_OverlayWeights.m_Weight1 = overlayIndexPtr[1].m_Weight;
              // ISSUE: reference to a compiler-generated field
              metaBufferData.m_OverlayWeights.m_Weight2 = overlayIndexPtr[2].m_Weight;
              // ISSUE: reference to a compiler-generated field
              metaBufferData.m_OverlayWeights.m_Weight3 = overlayIndexPtr[3].m_Weight;
              // ISSUE: reference to a compiler-generated field
              metaBufferData.m_OverlayWeights.m_Weight4 = overlayIndexPtr[4].m_Weight;
              // ISSUE: reference to a compiler-generated field
              metaBufferData.m_OverlayWeights.m_Weight5 = overlayIndexPtr[5].m_Weight;
              // ISSUE: reference to a compiler-generated field
              metaBufferData.m_OverlayWeights.m_Weight6 = overlayIndexPtr[6].m_Weight;
              // ISSUE: reference to a compiler-generated field
              metaBufferData.m_OverlayWeights.m_Weight7 = overlayIndexPtr[7].m_Weight;
              int colorOffset = (int) meshGroup.m_ColorOffset + (subMeshGroup.m_SubMeshRange.y - subMeshGroup.m_SubMeshRange.x);
              // ISSUE: reference to a compiler-generated method
              metaBufferData.m_OverlayColors1.m_Color0 = this.GetOverlayColor(overlayIndexPtr, 0, bufferData3, colorOffset);
              // ISSUE: reference to a compiler-generated method
              metaBufferData.m_OverlayColors1.m_Color1 = this.GetOverlayColor(overlayIndexPtr, 1, bufferData3, colorOffset);
              // ISSUE: reference to a compiler-generated method
              metaBufferData.m_OverlayColors1.m_Color2 = this.GetOverlayColor(overlayIndexPtr, 2, bufferData3, colorOffset);
              // ISSUE: reference to a compiler-generated method
              metaBufferData.m_OverlayColors1.m_Color3 = this.GetOverlayColor(overlayIndexPtr, 3, bufferData3, colorOffset);
              // ISSUE: reference to a compiler-generated method
              metaBufferData.m_OverlayColors1.m_Color4 = this.GetOverlayColor(overlayIndexPtr, 4, bufferData3, colorOffset);
              // ISSUE: reference to a compiler-generated method
              metaBufferData.m_OverlayColors1.m_Color5 = this.GetOverlayColor(overlayIndexPtr, 5, bufferData3, colorOffset);
              // ISSUE: reference to a compiler-generated method
              metaBufferData.m_OverlayColors1.m_Color6 = this.GetOverlayColor(overlayIndexPtr, 6, bufferData3, colorOffset);
              // ISSUE: reference to a compiler-generated method
              metaBufferData.m_OverlayColors1.m_Color7 = this.GetOverlayColor(overlayIndexPtr, 7, bufferData3, colorOffset);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              animated2.m_MetaIndex = this.m_AllocationData.AddMetaBufferData(metaBufferData);
            }
            else
            {
              int index2 = index1;
              if (bufferData5.IsCreated)
              {
                MeshGroup meshGroup;
                CollectionUtils.TryGet<MeshGroup>(bufferData2, index1, out meshGroup);
                index2 = bufferData5[(int) meshGroup.m_SubMeshGroup].m_SubMeshRange.x;
              }
              entity = bufferData1[index2].m_SubMesh;
              animationLayerMask = new AnimationLayerMask(AnimationLayer.Body);
            }
            DynamicBuffer<Game.Prefabs.AnimationClip> bufferData7;
            // ISSUE: reference to a compiler-generated field
            if (flag && this.m_AnimationClips.TryGetBuffer(entity, out bufferData7) && bufferData7.Length != 0)
            {
              if (((int) animationLayerMask.m_Mask & (int) new AnimationLayerMask(AnimationLayer.Body).m_Mask) != 0)
                animated2.m_ClipIndexBody0 = (short) 0;
              if (((int) animationLayerMask.m_Mask & (int) new AnimationLayerMask(AnimationLayer.Facial).m_Mask) != 0)
                animated2.m_ClipIndexFace0 = (short) 0;
            }
            animated1[index1] = animated2;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.Remove(cullingData);
        }
      }

      private void Deallocate(DynamicBuffer<Animated> animateds)
      {
        for (int index = 0; index < animateds.Length; ++index)
        {
          Animated animated = animateds[index];
          if (!animated.m_BoneAllocation.Empty)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AllocationData.ReleaseBones(animated.m_BoneAllocation);
          }
          if (animated.m_MetaIndex != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AllocationData.RemoveMetaBufferData(animated.m_MetaIndex);
          }
        }
      }

      private unsafe void AddOverlayIndex(
        InitializeAnimatedSystem.OverlayIndex* overlayIndex,
        int index,
        DynamicBuffer<OverlayElement> overlayElements,
        BlendWeight weight)
      {
        int num = 0;
        if (overlayElements.IsCreated && weight.m_Index >= 0 && weight.m_Index < overlayElements.Length)
          num = overlayElements[weight.m_Index].m_SortOrder;
        // ISSUE: object of a compiler-generated type is created
        *(overlayIndex + index) = new InitializeAnimatedSystem.OverlayIndex()
        {
          m_Weight = weight,
          m_OriginalIndex = index,
          m_SortOrder = num
        };
      }

      private unsafe Color GetOverlayColor(
        InitializeAnimatedSystem.OverlayIndex* overlayIndex,
        int index,
        DynamicBuffer<MeshColor> meshColors,
        int colorOffset)
      {
        // ISSUE: reference to a compiler-generated field
        return meshColors.IsCreated && meshColors.Length >= colorOffset + 8 ? meshColors[colorOffset + overlayIndex[index].m_OriginalIndex].m_ColorSet.m_Channel0.linear : Color.white;
      }
    }

    private struct OverlayIndex : IComparable<InitializeAnimatedSystem.OverlayIndex>
    {
      public BlendWeight m_Weight;
      public int m_OriginalIndex;
      public int m_SortOrder;

      public int CompareTo(InitializeAnimatedSystem.OverlayIndex other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(this.m_OriginalIndex - other.m_OriginalIndex, this.m_SortOrder - other.m_SortOrder, this.m_SortOrder != other.m_SortOrder);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CharacterStyleData> __Game_Prefabs_CharacterStyleData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshColor> __Game_Rendering_MeshColor_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.AnimationClip> __Game_Prefabs_AnimationClip_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CharacterElement> __Game_Prefabs_CharacterElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<OverlayElement> __Game_Prefabs_OverlayElement_RO_BufferLookup;
      public BufferLookup<Animated> __Game_Rendering_Animated_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterStyleData_RO_ComponentLookup = state.GetComponentLookup<CharacterStyleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferLookup = state.GetBufferLookup<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshColor_RO_BufferLookup = state.GetBufferLookup<MeshColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationClip_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.AnimationClip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterElement_RO_BufferLookup = state.GetBufferLookup<CharacterElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OverlayElement_RO_BufferLookup = state.GetBufferLookup<OverlayElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Animated_RW_BufferLookup = state.GetBufferLookup<Animated>();
      }
    }
  }
}
