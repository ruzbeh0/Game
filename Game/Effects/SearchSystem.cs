// Decompiled with JetBrains decompiler
// Type: Game.Effects.SearchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Serialization;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Effects
{
  [CompilerGenerated]
  public class SearchSystem : GameSystemBase, IPreDeserialize
  {
    private EffectFlagSystem m_EffectFlagSystem;
    private SimulationSystem m_SimulationSystem;
    private ToolSystem m_ToolSystem;
    private EffectControlData m_EffectControlData;
    private NativeQuadTree<SourceInfo, QuadTreeBoundsXZ> m_SearchTree;
    private NativeParallelMultiHashMap<Entity, SearchSystem.AddedSource> m_AddedSources;
    private EntityQuery m_UpdatedEffectsQuery;
    private EntityQuery m_AllEffectsQuery;
    private JobHandle m_ReadDependencies;
    private JobHandle m_WriteDependencies;
    private bool m_Loaded;
    private SearchSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectFlagSystem = this.World.GetOrCreateSystemManaged<EffectFlagSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlData = new EffectControlData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_SearchTree = new NativeQuadTree<SourceInfo, QuadTreeBoundsXZ>(1f, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AddedSources = new NativeParallelMultiHashMap<Entity, SearchSystem.AddedSource>(1000, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedEffectsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EnabledEffect>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<EffectsUpdated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Game.Events.Event>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<EnabledEffect>(),
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Static>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<EffectsUpdated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllEffectsQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EnabledEffect>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Game.Events.Event>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<EnabledEffect>(),
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Static>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SearchTree.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AddedSources.Dispose();
      base.OnDestroy();
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = loaded ? this.m_AllEffectsQuery : this.m_UpdatedEffectsQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlData.Update((SystemBase) this, this.m_EffectFlagSystem.GetData(), this.m_SimulationSystem.frameIndex, this.m_ToolSystem.selected);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LightEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new SearchSystem.UpdateSearchTreeJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_InterpolatedTransformType = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle,
        m_PrefabEffectData = this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup,
        m_PrefabLightEffectData = this.__TypeHandle.__Game_Prefabs_LightEffectData_RO_ComponentLookup,
        m_PrefabAudioEffectData = this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup,
        m_PrefabEffects = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup,
        m_Loaded = loaded,
        m_SearchTree = this.GetSearchTree(false, out dependencies),
        m_AddedSources = this.m_AddedSources,
        m_EffectControlData = this.m_EffectControlData
      }.Schedule<SearchSystem.UpdateSearchTreeJob>(query, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.AddSearchTreeWriter(jobHandle);
      this.Dependency = jobHandle;
    }

    public NativeQuadTree<SourceInfo, QuadTreeBoundsXZ> GetSearchTree(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_SearchTree;
    }

    public void AddSearchTreeReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, jobHandle);
    }

    public void AddSearchTreeWriter(JobHandle jobHandle) => this.m_WriteDependencies = jobHandle;

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<SourceInfo, QuadTreeBoundsXZ> searchTree = this.GetSearchTree(false, out dependencies);
      dependencies.Complete();
      searchTree.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_AddedSources.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    public static QuadTreeBoundsXZ GetBounds(
      NativeArray<Transform> transforms,
      NativeArray<Curve> curves,
      int index,
      Effect effect,
      ref ComponentLookup<LightEffectData> prefabLightEffectData,
      ref ComponentLookup<AudioEffectData> prefabAudioEffectData)
    {
      float3 position = effect.m_Position;
      quaternion rotation = effect.m_Rotation;
      Curve curve;
      if (CollectionUtils.TryGet<Curve>(curves, index, out curve))
      {
        position = MathUtils.Position(curve.m_Bezier, 0.5f);
      }
      else
      {
        Transform transform;
        if (CollectionUtils.TryGet<Transform>(transforms, index, out transform))
        {
          Transform world = ObjectUtils.LocalToWorld(transform, position, rotation);
          position = world.m_Position;
          rotation = world.m_Rotation;
        }
      }
      Bounds3 bounds1 = new Bounds3(position - 1f, position + 1f);
      int num = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(1f)));
      LightEffectData componentData1;
      if (prefabLightEffectData.TryGetComponent(effect.m_Effect, out componentData1))
      {
        bounds1 |= new Bounds3(position - componentData1.m_Range, position + componentData1.m_Range);
        num = math.min(num, componentData1.m_MinLod);
      }
      AudioEffectData componentData2;
      if (prefabAudioEffectData.TryGetComponent(effect.m_Effect, out componentData2) && math.any(componentData2.m_SourceSize > 0.0f))
      {
        Bounds3 bounds2 = new Bounds3(-componentData2.m_SourceSize, componentData2.m_SourceSize);
        bounds1 |= ObjectUtils.CalculateBounds(position, rotation, bounds2);
        num = math.min(num, RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(componentData2.m_SourceSize)));
      }
      return new QuadTreeBoundsXZ(bounds1, (BoundsMask) 0, num);
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
    public SearchSystem()
    {
    }

    private struct AddedSource
    {
      public Entity m_Prefab;
      public int m_EffectIndex;
    }

    [BurstCompile]
    private struct UpdateSearchTreeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Tools.EditorContainer> m_EditorContainerType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<InterpolatedTransform> m_InterpolatedTransformType;
      [ReadOnly]
      public ComponentLookup<EffectData> m_PrefabEffectData;
      [ReadOnly]
      public ComponentLookup<LightEffectData> m_PrefabLightEffectData;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> m_PrefabAudioEffectData;
      [ReadOnly]
      public BufferLookup<Effect> m_PrefabEffects;
      [ReadOnly]
      public bool m_Loaded;
      public NativeQuadTree<SourceInfo, QuadTreeBoundsXZ> m_SearchTree;
      public NativeParallelMultiHashMap<Entity, SearchSystem.AddedSource> m_AddedSources;
      public EffectControlData m_EffectControlData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Tools.EditorContainer> nativeArray2 = chunk.GetNativeArray<Game.Tools.EditorContainer>(ref this.m_EditorContainerType);
        NativeArray<PrefabRef> nativeArray3 = new NativeArray<PrefabRef>();
        if (nativeArray2.Length == 0)
        {
          // ISSUE: reference to a compiler-generated field
          nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType) || chunk.Has<InterpolatedTransform>(ref this.m_InterpolatedTransformType))
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            // ISSUE: variable of a compiler-generated type
            SearchSystem.AddedSource addedSource;
            NativeParallelMultiHashMapIterator<Entity> it;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AddedSources.TryGetFirstValue(entity, out addedSource, out it))
            {
              // ISSUE: reference to a compiler-generated field
              do
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.TryRemove(new SourceInfo(entity, addedSource.m_EffectIndex));
              }
              while (this.m_AddedSources.TryGetNextValue(out addedSource, ref it));
              // ISSUE: reference to a compiler-generated field
              this.m_AddedSources.Remove(entity);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray5 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = this.m_Loaded || chunk.Has<Created>(ref this.m_CreatedType);
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Entity entity = nativeArray1[index1];
            Game.Tools.EditorContainer editorContainer;
            // ISSUE: variable of a compiler-generated type
            SearchSystem.AddedSource addedSource1;
            if (CollectionUtils.TryGet<Game.Tools.EditorContainer>(nativeArray2, index1, out editorContainer))
            {
              // ISSUE: variable of a compiler-generated type
              SearchSystem.AddedSource addedSource2;
              NativeParallelMultiHashMapIterator<Entity> it;
              // ISSUE: reference to a compiler-generated field
              if (this.m_AddedSources.TryGetFirstValue(entity, out addedSource2, out it))
              {
                // ISSUE: reference to a compiler-generated field
                do
                {
                  // ISSUE: reference to a compiler-generated field
                  if (editorContainer.m_Prefab != addedSource2.m_Prefab)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_SearchTree.TryRemove(new SourceInfo(entity, addedSource2.m_EffectIndex));
                  }
                }
                while (this.m_AddedSources.TryGetNextValue(out addedSource2, ref it));
                // ISSUE: reference to a compiler-generated field
                this.m_AddedSources.Remove(entity);
              }
              EffectData componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabEffectData.TryGetComponent(editorContainer.m_Prefab, out componentData) && !componentData.m_OwnerCulling)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_EffectControlData.ShouldBeEnabled(entity, editorContainer.m_Prefab, true, true))
                {
                  Effect effect = new Effect()
                  {
                    m_Effect = editorContainer.m_Prefab
                  };
                  // ISSUE: reference to a compiler-generated method
                  QuadTreeBoundsXZ bounds = this.GetBounds(nativeArray4, nativeArray5, index1, effect);
                  // ISSUE: reference to a compiler-generated field
                  this.m_SearchTree.AddOrUpdate(new SourceInfo(entity, 0), bounds);
                  // ISSUE: reference to a compiler-generated field
                  ref NativeParallelMultiHashMap<Entity, SearchSystem.AddedSource> local = ref this.m_AddedSources;
                  Entity key = entity;
                  // ISSUE: object of a compiler-generated type is created
                  addedSource1 = new SearchSystem.AddedSource();
                  // ISSUE: reference to a compiler-generated field
                  addedSource1.m_Prefab = editorContainer.m_Prefab;
                  // ISSUE: reference to a compiler-generated field
                  addedSource1.m_EffectIndex = 0;
                  // ISSUE: variable of a compiler-generated type
                  SearchSystem.AddedSource addedSource3 = addedSource1;
                  local.Add(key, addedSource3);
                }
                else if (!flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_SearchTree.TryRemove(new SourceInfo(entity, 0));
                }
              }
            }
            else
            {
              DynamicBuffer<Effect> bufferData;
              // ISSUE: reference to a compiler-generated field
              this.m_PrefabEffects.TryGetBuffer(nativeArray3[index1].m_Prefab, out bufferData);
              // ISSUE: variable of a compiler-generated type
              SearchSystem.AddedSource addedSource4;
              NativeParallelMultiHashMapIterator<Entity> it;
              // ISSUE: reference to a compiler-generated field
              if (this.m_AddedSources.TryGetFirstValue(entity, out addedSource4, out it))
              {
                // ISSUE: reference to a compiler-generated field
                do
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!bufferData.IsCreated || bufferData.Length <= addedSource4.m_EffectIndex || bufferData[addedSource4.m_EffectIndex].m_Effect != addedSource4.m_Prefab)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_SearchTree.TryRemove(new SourceInfo(entity, addedSource4.m_EffectIndex));
                  }
                }
                while (this.m_AddedSources.TryGetNextValue(out addedSource4, ref it));
                // ISSUE: reference to a compiler-generated field
                this.m_AddedSources.Remove(entity);
              }
              if (bufferData.IsCreated)
              {
                for (int index2 = 0; index2 < bufferData.Length; ++index2)
                {
                  Effect effect = bufferData[index2];
                  EffectData componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabEffectData.TryGetComponent(effect.m_Effect, out componentData) && !componentData.m_OwnerCulling)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_EffectControlData.ShouldBeEnabled(entity, effect.m_Effect, true, false))
                    {
                      // ISSUE: reference to a compiler-generated method
                      QuadTreeBoundsXZ bounds = this.GetBounds(nativeArray4, nativeArray5, index1, effect);
                      // ISSUE: reference to a compiler-generated field
                      this.m_SearchTree.AddOrUpdate(new SourceInfo(entity, index2), bounds);
                      // ISSUE: reference to a compiler-generated field
                      ref NativeParallelMultiHashMap<Entity, SearchSystem.AddedSource> local = ref this.m_AddedSources;
                      Entity key = entity;
                      // ISSUE: object of a compiler-generated type is created
                      addedSource1 = new SearchSystem.AddedSource();
                      // ISSUE: reference to a compiler-generated field
                      addedSource1.m_Prefab = effect.m_Effect;
                      // ISSUE: reference to a compiler-generated field
                      addedSource1.m_EffectIndex = index2;
                      // ISSUE: variable of a compiler-generated type
                      SearchSystem.AddedSource addedSource5 = addedSource1;
                      local.Add(key, addedSource5);
                    }
                    else if (!flag)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_SearchTree.TryRemove(new SourceInfo(entity, index2));
                    }
                  }
                }
              }
            }
          }
        }
      }

      private QuadTreeBoundsXZ GetBounds(
        NativeArray<Transform> transforms,
        NativeArray<Curve> curves,
        int index,
        Effect effect)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return SearchSystem.GetBounds(transforms, curves, index, effect, ref this.m_PrefabLightEffectData, ref this.m_PrefabAudioEffectData);
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<EffectData> __Game_Prefabs_EffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LightEffectData> __Game_Prefabs_LightEffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> __Game_Prefabs_AudioEffectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Effect> __Game_Prefabs_Effect_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Tools.EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectData_RO_ComponentLookup = state.GetComponentLookup<EffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LightEffectData_RO_ComponentLookup = state.GetComponentLookup<LightEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioEffectData_RO_ComponentLookup = state.GetComponentLookup<AudioEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RO_BufferLookup = state.GetBufferLookup<Effect>(true);
      }
    }
  }
}
