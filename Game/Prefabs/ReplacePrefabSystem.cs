// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ReplacePrefabSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Rendering;
using Game.City;
using Game.Common;
using Game.Rendering;
using Game.Tools;
using Game.Tutorials;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class ReplacePrefabSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ReplacePrefabSystem.Finalize m_FinalizeSystem;
    private BatchManagerSystem m_BatchManagerSystem;
    private ManagedBatchSystem m_ManagedBatchSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_PrefabRefQuery;
    private Entity m_OldPrefab;
    private Entity m_NewPrefab;
    private Entity m_SourceInstance;
    private NativeList<ReplacePrefabSystem.ReplaceMesh> m_MeshReplaces;
    private NativeQueue<Entity> m_UpdateInstances;
    private NativeHashMap<Entity, ReplacePrefabSystem.ReplacePrefabData> m_ReplacePrefabData;
    private ReplacePrefabSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FinalizeSystem = this.World.GetOrCreateSystemManaged<ReplacePrefabSystem.Finalize>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ManagedBatchSystem = this.World.GetOrCreateSystemManaged<ManagedBatchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabRefQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[13]
        {
          ComponentType.ReadWrite<PrefabRef>(),
          ComponentType.ReadWrite<SubObject>(),
          ComponentType.ReadWrite<SubNet>(),
          ComponentType.ReadWrite<SubArea>(),
          ComponentType.ReadWrite<PlaceholderObjectElement>(),
          ComponentType.ReadWrite<ServiceUpgradeBuilding>(),
          ComponentType.ReadWrite<BuildingUpgradeElement>(),
          ComponentType.ReadWrite<Effect>(),
          ComponentType.ReadWrite<ActivityLocationElement>(),
          ComponentType.ReadWrite<SubMesh>(),
          ComponentType.ReadWrite<LodMesh>(),
          ComponentType.ReadWrite<UIGroupElement>(),
          ComponentType.ReadWrite<TutorialPhaseRef>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MeshReplaces = new NativeList<ReplacePrefabSystem.ReplaceMesh>(1, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateInstances = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ReplacePrefabData = new NativeHashMap<Entity, ReplacePrefabSystem.ReplacePrefabData>(1, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabRefQuery);
      this.Enabled = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MeshReplaces.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateInstances.Dispose();
      base.OnDestroy();
    }

    public void ReplacePrefab(Entity oldPrefab, Entity newPrefab, Entity sourceInstance)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_OldPrefab = oldPrefab;
      // ISSUE: reference to a compiler-generated field
      this.m_NewPrefab = newPrefab;
      // ISSUE: reference to a compiler-generated field
      this.m_SourceInstance = sourceInstance;
      try
      {
        this.Enabled = true;
        this.Update();
      }
      finally
      {
        this.Enabled = false;
      }
    }

    public void FinalizeReplaces() => this.m_FinalizeSystem.Update();

    private void CheckInstanceComponents(
      Entity instance,
      HashSet<ComponentType> checkedComponents,
      HashSet<ComponentType> archetypeComponents)
    {
      NativeArray<ComponentType> componentTypes = this.EntityManager.GetChunk(instance).Archetype.GetComponentTypes();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabSystem.GetPrefab<PrefabBase>(this.EntityManager.GetComponentData<PrefabRef>(instance)).GetArchetypeComponents(archetypeComponents);
      foreach (ComponentType componentType in componentTypes)
      {
        if (checkedComponents.Contains(componentType))
        {
          if (archetypeComponents.Contains(componentType))
            archetypeComponents.Remove(componentType);
          else
            this.EntityManager.RemoveComponent(instance, componentType);
        }
      }
      foreach (ComponentType archetypeComponent in archetypeComponents)
      {
        if (checkedComponents.Contains(archetypeComponent))
          this.EntityManager.AddComponent(instance, archetypeComponent);
      }
      archetypeComponents.Clear();
      componentTypes.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      EntityCommandBuffer entityCommandBuffer1 = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.SinglePlayback);
      EntityCommandBuffer entityCommandBuffer2 = new EntityCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.RequireFullUpdate();
      JobHandle jobHandle = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.HasComponent<MeshData>(this.m_OldPrefab))
      {
        entityCommandBuffer2 = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.SinglePlayback);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ReplacePrefabSystem.RemoveBatchGroupsJob jobData = new ReplacePrefabSystem.RemoveBatchGroupsJob()
        {
          m_OldMeshEntity = this.m_OldPrefab,
          m_NewMeshEntity = this.m_NewPrefab,
          m_MeshBatches = this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferLookup,
          m_FadeBatches = this.__TypeHandle.__Game_Rendering_FadeBatch_RW_BufferLookup,
          m_BatchGroups = this.__TypeHandle.__Game_Prefabs_BatchGroup_RW_BufferLookup,
          m_NativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies1),
          m_NativeBatchInstances = this.m_BatchManagerSystem.GetNativeBatchInstances(false, out dependencies2),
          m_NativeSubBatches = this.m_BatchManagerSystem.GetNativeSubBatches(false, out dependencies3),
          m_EntityCommandBuffer = entityCommandBuffer2,
          m_ReplaceMeshes = this.m_MeshReplaces
        };
        jobHandle = JobHandle.CombineDependencies(jobHandle, jobData.Schedule<ReplacePrefabSystem.RemoveBatchGroupsJob>(JobUtils.CombineDependencies(this.Dependency, dependencies1, dependencies2, dependencies3)));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_BatchManagerSystem.AddNativeBatchGroupsWriter(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_BatchManagerSystem.AddNativeBatchInstancesWriter(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_BatchManagerSystem.AddNativeSubBatchesWriter(jobHandle);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ReplacePrefabData[this.m_NewPrefab] = new ReplacePrefabSystem.ReplacePrefabData()
        {
          m_OldPrefab = this.m_OldPrefab,
          m_SourceInstance = this.m_SourceInstance
        };
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_SourceInstance != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateInstances.Enqueue(this.m_SourceInstance);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_TutorialPhaseRef_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UIGroupElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LodMesh_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingUpgradeElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpgradeBuilding_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubNet_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ReplacePrefabSystem.ReplacePrefabJob jobData1 = new ReplacePrefabSystem.ReplacePrefabJob()
      {
        m_OldPrefab = this.m_OldPrefab,
        m_NewPrefab = this.m_NewPrefab,
        m_SourceInstance = this.m_SourceInstance,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle,
        m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RW_ComponentTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Prefabs_SubObject_RW_BufferTypeHandle,
        m_SubNetType = this.__TypeHandle.__Game_Prefabs_SubNet_RW_BufferTypeHandle,
        m_SubAreaType = this.__TypeHandle.__Game_Prefabs_SubArea_RW_BufferTypeHandle,
        m_PlaceholderObjectElementType = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle,
        m_ServiceUpgradeBuildingType = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeBuilding_RW_BufferTypeHandle,
        m_BuildingUpgradeElementType = this.__TypeHandle.__Game_Prefabs_BuildingUpgradeElement_RW_BufferTypeHandle,
        m_EffectType = this.__TypeHandle.__Game_Prefabs_Effect_RW_BufferTypeHandle,
        m_ActivityLocationElementType = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RW_BufferTypeHandle,
        m_SubMeshType = this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferTypeHandle,
        m_LodMeshType = this.__TypeHandle.__Game_Prefabs_LodMesh_RW_BufferTypeHandle,
        m_UIGroupElementType = this.__TypeHandle.__Game_Prefabs_UIGroupElement_RW_BufferTypeHandle,
        m_TutorialPhaseType = this.__TypeHandle.__Game_Tutorials_TutorialPhaseRef_RW_BufferTypeHandle,
        m_CommandBuffer = entityCommandBuffer1.AsParallelWriter(),
        m_UpdateInstances = this.m_UpdateInstances.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle.CombineDependencies(jobHandle, jobData1.ScheduleParallel<ReplacePrefabSystem.ReplacePrefabJob>(this.m_PrefabRefQuery, this.Dependency)).Complete();
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.HasComponent<BuildingUpgradeElement>(this.m_OldPrefab))
      {
        EntityManager entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BuildingUpgradeElement> dynamicBuffer = entityManager.AddBuffer<BuildingUpgradeElement>(this.m_NewPrefab);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BuildingUpgradeElement> buffer = entityManager.GetBuffer<BuildingUpgradeElement>(this.m_OldPrefab, true);
        dynamicBuffer.CopyFrom(buffer);
      }
      entityCommandBuffer1.Playback(this.EntityManager);
      entityCommandBuffer1.Dispose();
      if (!entityCommandBuffer2.IsCreated)
        return;
      entityCommandBuffer2.Playback(this.EntityManager);
      entityCommandBuffer2.Dispose();
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
    public ReplacePrefabSystem()
    {
    }

    private struct ReplaceMesh
    {
      public Entity m_OldMesh;
      public Entity m_NewMesh;
    }

    public struct ReplacePrefabData
    {
      public Entity m_OldPrefab;
      public Entity m_SourceInstance;
      public bool m_AreasUpdated;
      public bool m_NetsUpdated;
      public bool m_LanesUpdated;
    }

    [BurstCompile]
    private struct RemoveBatchGroupsJob : IJob
    {
      [ReadOnly]
      public Entity m_OldMeshEntity;
      [ReadOnly]
      public Entity m_NewMeshEntity;
      public BufferLookup<MeshBatch> m_MeshBatches;
      public BufferLookup<FadeBatch> m_FadeBatches;
      public BufferLookup<BatchGroup> m_BatchGroups;
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchInstances;
      public NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> m_NativeSubBatches;
      public EntityCommandBuffer m_EntityCommandBuffer;
      public NativeList<ReplacePrefabSystem.ReplaceMesh> m_ReplaceMeshes;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BatchGroup> batchGroup = this.m_BatchGroups[this.m_OldMeshEntity];
        NativeHashSet<Entity> updateBatches = new NativeHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < batchGroup.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.RemoveBatchGroup(batchGroup[index].m_GroupIndex, batchGroup[index].m_MergeIndex, updateBatches);
        }
        batchGroup.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ReplaceMeshes.Add(new ReplacePrefabSystem.ReplaceMesh()
        {
          m_OldMesh = this.m_OldMeshEntity,
          m_NewMesh = this.m_NewMeshEntity
        });
        NativeHashSet<Entity>.Enumerator enumerator = updateBatches.GetEnumerator();
        while (enumerator.MoveNext())
        {
          // ISSUE: reference to a compiler-generated field
          this.m_EntityCommandBuffer.AddComponent<BatchesUpdated>(enumerator.Current, new BatchesUpdated());
        }
        enumerator.Dispose();
        updateBatches.Dispose();
      }

      private void RemoveBatchGroup(
        int groupIndex,
        int mergeIndex,
        NativeHashSet<Entity> updateBatches)
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
                updateBatches.Add(instanceData.m_Entity);
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
    }

    [BurstCompile]
    private struct ReplacePrefabJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_OldPrefab;
      [ReadOnly]
      public Entity m_NewPrefab;
      [ReadOnly]
      public Entity m_SourceInstance;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Game.Tools.EditorContainer> m_EditorContainerType;
      public BufferTypeHandle<SubObject> m_SubObjectType;
      public BufferTypeHandle<SubNet> m_SubNetType;
      public BufferTypeHandle<SubArea> m_SubAreaType;
      public BufferTypeHandle<PlaceholderObjectElement> m_PlaceholderObjectElementType;
      public BufferTypeHandle<ServiceUpgradeBuilding> m_ServiceUpgradeBuildingType;
      public BufferTypeHandle<BuildingUpgradeElement> m_BuildingUpgradeElementType;
      public BufferTypeHandle<Effect> m_EffectType;
      public BufferTypeHandle<ActivityLocationElement> m_ActivityLocationElementType;
      public BufferTypeHandle<SubMesh> m_SubMeshType;
      public BufferTypeHandle<LodMesh> m_LodMeshType;
      public BufferTypeHandle<UIGroupElement> m_UIGroupElementType;
      public BufferTypeHandle<TutorialPhaseRef> m_TutorialPhaseType;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<Entity>.ParallelWriter m_UpdateInstances;

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
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Tools.EditorContainer> nativeArray3 = chunk.GetNativeArray<Game.Tools.EditorContainer>(ref this.m_EditorContainerType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            PrefabRef prefabRef = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            if (prefabRef.m_Prefab == this.m_OldPrefab)
            {
              // ISSUE: reference to a compiler-generated field
              prefabRef.m_Prefab = this.m_NewPrefab;
              nativeArray2[index] = prefabRef;
              Entity e = nativeArray1[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_SourceInstance != e)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, e, new Updated());
                // ISSUE: reference to a compiler-generated field
                this.m_UpdateInstances.Enqueue(e);
              }
            }
          }
          for (int index = 0; index < nativeArray3.Length; ++index)
          {
            Game.Tools.EditorContainer editorContainer = nativeArray3[index];
            // ISSUE: reference to a compiler-generated field
            if (editorContainer.m_Prefab == this.m_OldPrefab)
            {
              // ISSUE: reference to a compiler-generated field
              editorContainer.m_Prefab = this.m_NewPrefab;
              nativeArray3[index] = editorContainer;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<SubObject> bufferAccessor1 = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<SubNet> bufferAccessor2 = chunk.GetBufferAccessor<SubNet>(ref this.m_SubNetType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<SubArea> bufferAccessor3 = chunk.GetBufferAccessor<SubArea>(ref this.m_SubAreaType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<PlaceholderObjectElement> bufferAccessor4 = chunk.GetBufferAccessor<PlaceholderObjectElement>(ref this.m_PlaceholderObjectElementType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<ServiceUpgradeBuilding> bufferAccessor5 = chunk.GetBufferAccessor<ServiceUpgradeBuilding>(ref this.m_ServiceUpgradeBuildingType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<BuildingUpgradeElement> bufferAccessor6 = chunk.GetBufferAccessor<BuildingUpgradeElement>(ref this.m_BuildingUpgradeElementType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Effect> bufferAccessor7 = chunk.GetBufferAccessor<Effect>(ref this.m_EffectType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<ActivityLocationElement> bufferAccessor8 = chunk.GetBufferAccessor<ActivityLocationElement>(ref this.m_ActivityLocationElementType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<SubMesh> bufferAccessor9 = chunk.GetBufferAccessor<SubMesh>(ref this.m_SubMeshType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LodMesh> bufferAccessor10 = chunk.GetBufferAccessor<LodMesh>(ref this.m_LodMeshType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<UIGroupElement> bufferAccessor11 = chunk.GetBufferAccessor<UIGroupElement>(ref this.m_UIGroupElementType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<TutorialPhaseRef> bufferAccessor12 = chunk.GetBufferAccessor<TutorialPhaseRef>(ref this.m_TutorialPhaseType);
          for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
          {
            DynamicBuffer<SubObject> dynamicBuffer = bufferAccessor1[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              SubObject subObject = dynamicBuffer[index2];
              // ISSUE: reference to a compiler-generated field
              if (subObject.m_Prefab == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                subObject.m_Prefab = this.m_NewPrefab;
                dynamicBuffer[index2] = subObject;
              }
            }
          }
          for (int index3 = 0; index3 < bufferAccessor2.Length; ++index3)
          {
            DynamicBuffer<SubNet> dynamicBuffer = bufferAccessor2[index3];
            for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
            {
              SubNet subNet = dynamicBuffer[index4];
              // ISSUE: reference to a compiler-generated field
              if (subNet.m_Prefab == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                subNet.m_Prefab = this.m_NewPrefab;
                dynamicBuffer[index4] = subNet;
              }
            }
          }
          for (int index5 = 0; index5 < bufferAccessor3.Length; ++index5)
          {
            DynamicBuffer<SubArea> dynamicBuffer = bufferAccessor3[index5];
            for (int index6 = 0; index6 < dynamicBuffer.Length; ++index6)
            {
              SubArea subArea = dynamicBuffer[index6];
              // ISSUE: reference to a compiler-generated field
              if (subArea.m_Prefab == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                subArea.m_Prefab = this.m_NewPrefab;
                dynamicBuffer[index6] = subArea;
              }
            }
          }
          for (int index7 = 0; index7 < bufferAccessor4.Length; ++index7)
          {
            DynamicBuffer<PlaceholderObjectElement> dynamicBuffer = bufferAccessor4[index7];
            for (int index8 = 0; index8 < dynamicBuffer.Length; ++index8)
            {
              // ISSUE: reference to a compiler-generated field
              if (dynamicBuffer[index8].m_Object == this.m_OldPrefab)
              {
                dynamicBuffer.RemoveAtSwapBack(index8);
                --index8;
              }
            }
          }
          for (int index9 = 0; index9 < bufferAccessor5.Length; ++index9)
          {
            DynamicBuffer<ServiceUpgradeBuilding> dynamicBuffer = bufferAccessor5[index9];
            for (int index10 = 0; index10 < dynamicBuffer.Length; ++index10)
            {
              ServiceUpgradeBuilding serviceUpgradeBuilding = dynamicBuffer[index10];
              // ISSUE: reference to a compiler-generated field
              if (serviceUpgradeBuilding.m_Building == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                serviceUpgradeBuilding.m_Building = this.m_NewPrefab;
                dynamicBuffer[index10] = serviceUpgradeBuilding;
              }
            }
          }
          for (int index11 = 0; index11 < bufferAccessor6.Length; ++index11)
          {
            DynamicBuffer<BuildingUpgradeElement> dynamicBuffer = bufferAccessor6[index11];
            for (int index12 = 0; index12 < dynamicBuffer.Length; ++index12)
            {
              // ISSUE: reference to a compiler-generated field
              if (dynamicBuffer[index12].m_Upgrade == this.m_OldPrefab)
              {
                dynamicBuffer.RemoveAtSwapBack(index12);
                --index12;
              }
            }
          }
          for (int index13 = 0; index13 < bufferAccessor7.Length; ++index13)
          {
            DynamicBuffer<Effect> dynamicBuffer = bufferAccessor7[index13];
            for (int index14 = 0; index14 < dynamicBuffer.Length; ++index14)
            {
              Effect effect = dynamicBuffer[index14];
              // ISSUE: reference to a compiler-generated field
              if (effect.m_Effect == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                effect.m_Effect = this.m_NewPrefab;
                dynamicBuffer[index14] = effect;
              }
            }
          }
          for (int index15 = 0; index15 < bufferAccessor8.Length; ++index15)
          {
            DynamicBuffer<ActivityLocationElement> dynamicBuffer = bufferAccessor8[index15];
            for (int index16 = 0; index16 < dynamicBuffer.Length; ++index16)
            {
              ActivityLocationElement activityLocationElement = dynamicBuffer[index16];
              // ISSUE: reference to a compiler-generated field
              if (activityLocationElement.m_Prefab == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                activityLocationElement.m_Prefab = this.m_NewPrefab;
                dynamicBuffer[index16] = activityLocationElement;
              }
            }
          }
          for (int index17 = 0; index17 < bufferAccessor9.Length; ++index17)
          {
            DynamicBuffer<SubMesh> dynamicBuffer = bufferAccessor9[index17];
            for (int index18 = 0; index18 < dynamicBuffer.Length; ++index18)
            {
              SubMesh subMesh = dynamicBuffer[index18];
              // ISSUE: reference to a compiler-generated field
              if (subMesh.m_SubMesh == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                subMesh.m_SubMesh = this.m_NewPrefab;
                dynamicBuffer[index18] = subMesh;
              }
            }
          }
          for (int index19 = 0; index19 < bufferAccessor10.Length; ++index19)
          {
            DynamicBuffer<LodMesh> dynamicBuffer = bufferAccessor10[index19];
            for (int index20 = 0; index20 < dynamicBuffer.Length; ++index20)
            {
              LodMesh lodMesh = dynamicBuffer[index20];
              // ISSUE: reference to a compiler-generated field
              if (lodMesh.m_LodMesh == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                lodMesh.m_LodMesh = this.m_NewPrefab;
                dynamicBuffer[index20] = lodMesh;
              }
            }
          }
          for (int index21 = 0; index21 < bufferAccessor11.Length; ++index21)
          {
            DynamicBuffer<UIGroupElement> dynamicBuffer = bufferAccessor11[index21];
            for (int index22 = 0; index22 < dynamicBuffer.Length; ++index22)
            {
              // ISSUE: reference to a compiler-generated field
              if (dynamicBuffer[index22].m_Prefab == this.m_OldPrefab)
              {
                dynamicBuffer.RemoveAtSwapBack(index22);
                --index22;
              }
            }
          }
          for (int index23 = 0; index23 < bufferAccessor12.Length; ++index23)
          {
            DynamicBuffer<TutorialPhaseRef> dynamicBuffer = bufferAccessor12[index23];
            for (int index24 = 0; index24 < dynamicBuffer.Length; ++index24)
            {
              TutorialPhaseRef tutorialPhaseRef = dynamicBuffer[index24];
              // ISSUE: reference to a compiler-generated field
              if (tutorialPhaseRef.m_Phase == this.m_OldPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                tutorialPhaseRef.m_Phase = this.m_NewPrefab;
                dynamicBuffer[index24] = tutorialPhaseRef;
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
      public BufferLookup<MeshBatch> __Game_Rendering_MeshBatch_RW_BufferLookup;
      public BufferLookup<FadeBatch> __Game_Rendering_FadeBatch_RW_BufferLookup;
      public BufferLookup<BatchGroup> __Game_Prefabs_BatchGroup_RW_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RW_ComponentTypeHandle;
      public BufferTypeHandle<SubObject> __Game_Prefabs_SubObject_RW_BufferTypeHandle;
      public BufferTypeHandle<SubNet> __Game_Prefabs_SubNet_RW_BufferTypeHandle;
      public BufferTypeHandle<SubArea> __Game_Prefabs_SubArea_RW_BufferTypeHandle;
      public BufferTypeHandle<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceUpgradeBuilding> __Game_Prefabs_ServiceUpgradeBuilding_RW_BufferTypeHandle;
      public BufferTypeHandle<BuildingUpgradeElement> __Game_Prefabs_BuildingUpgradeElement_RW_BufferTypeHandle;
      public BufferTypeHandle<Effect> __Game_Prefabs_Effect_RW_BufferTypeHandle;
      public BufferTypeHandle<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RW_BufferTypeHandle;
      public BufferTypeHandle<SubMesh> __Game_Prefabs_SubMesh_RW_BufferTypeHandle;
      public BufferTypeHandle<LodMesh> __Game_Prefabs_LodMesh_RW_BufferTypeHandle;
      public BufferTypeHandle<UIGroupElement> __Game_Prefabs_UIGroupElement_RW_BufferTypeHandle;
      public BufferTypeHandle<TutorialPhaseRef> __Game_Tutorials_TutorialPhaseRef_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshBatch_RW_BufferLookup = state.GetBufferLookup<MeshBatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_FadeBatch_RW_BufferLookup = state.GetBufferLookup<FadeBatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BatchGroup_RW_BufferLookup = state.GetBufferLookup<BatchGroup>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Tools.EditorContainer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubNet>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubArea>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PlaceholderObjectElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeBuilding_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceUpgradeBuilding>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingUpgradeElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<BuildingUpgradeElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RW_BufferTypeHandle = state.GetBufferTypeHandle<Effect>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<ActivityLocationElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubMesh>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LodMesh_RW_BufferTypeHandle = state.GetBufferTypeHandle<LodMesh>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UIGroupElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<UIGroupElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_TutorialPhaseRef_RW_BufferTypeHandle = state.GetBufferTypeHandle<TutorialPhaseRef>();
      }
    }
  }
}
