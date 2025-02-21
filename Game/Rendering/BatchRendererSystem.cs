// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchRendererSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Rendering;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  public class BatchRendererSystem : GameSystemBase
  {
    private BatchManagerSystem m_BatchManagerSystem;
    private BatchMeshSystem m_BatchMeshSystem;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      this.m_BatchMeshSystem = this.World.GetOrCreateSystemManaged<BatchMeshSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated method
      NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> nativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> nativeSubBatches = this.m_BatchManagerSystem.GetNativeSubBatches(false, out dependencies2);
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies1.Complete();
      dependencies2.Complete();
      ObsoleteManagedBatchEnumerator obsoleteManagedBatches = nativeBatchGroups.GetObsoleteManagedBatches();
      int managedBatchIndex;
      while (obsoleteManagedBatches.GetNextObsoleteBatch(out managedBatchIndex))
      {
        CustomBatch batch = (CustomBatch) managedBatches.GetBatch(managedBatchIndex);
        // ISSUE: reference to a compiler-generated method
        this.m_BatchMeshSystem.RemoveBatch(batch, managedBatchIndex);
        managedBatches.RemoveBatch(managedBatchIndex);
        batch.Dispose();
      }
      UpdatedMetaDataEnumerator updatedMetaDatas = nativeBatchGroups.GetUpdatedMetaDatas();
      int groupIndex1;
      while (updatedMetaDatas.GetNextUpdatedGroup(out groupIndex1))
        nativeSubBatches.RecreateRenderers(groupIndex1);
      ObsoleteBatchRendererEnumerator obsoleteBatchRenderers = nativeSubBatches.GetObsoleteBatchRenderers();
      BatchID rendererIndex;
      while (obsoleteBatchRenderers.GetNextObsoleteRenderer(out rendererIndex))
        managedBatches.RemoveRenderer(rendererIndex);
      nativeSubBatches.ClearObsoleteBatchRenderers();
      UpdatedBatchRendererEnumerator updatedBatchRenderers = nativeSubBatches.GetUpdatedBatchRenderers();
      int groupIndex2;
      while (updatedBatchRenderers.GetNextUpdatedGroup(out groupIndex2))
      {
        NativeSubBatchAccessor<BatchData> subBatchAccessor = nativeSubBatches.GetSubBatchAccessor(groupIndex2);
        for (int index = 0; index < subBatchAccessor.Length; ++index)
        {
          NativeBatchPropertyAccessor propertyAccessor = nativeBatchGroups.GetBatchPropertyAccessor(groupIndex2, index);
          if (subBatchAccessor.GetBatchID(index) == BatchID.Null)
          {
            BatchID batchID = managedBatches.AddBatchRenderer(propertyAccessor);
            nativeSubBatches.SetBatchID(groupIndex2, index, batchID);
          }
        }
      }
      nativeSubBatches.ClearUpdatedBatchRenderers();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchGroupsWriter(new BatchRendererSystem.ClearUpdatedMetaDatasJob()
      {
        m_NativeBatchGroups = this.m_BatchManagerSystem.GetNativeBatchGroups(false, out dependencies1)
      }.Schedule<BatchRendererSystem.ClearUpdatedMetaDatasJob>(dependencies1));
    }

    [UnityEngine.Scripting.Preserve]
    public BatchRendererSystem()
    {
    }

    [BurstCompile]
    private struct ClearUpdatedMetaDatasJob : IJob
    {
      public NativeBatchGroups<CullingData, GroupData, BatchData, InstanceData> m_NativeBatchGroups;

      public void Execute()
      {
        this.m_NativeBatchGroups.ClearObsoleteManagedBatches();
        this.m_NativeBatchGroups.ClearUpdatedMetaDatas();
      }
    }
  }
}
