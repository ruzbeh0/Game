// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchUploadSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Rendering;
using Unity.Burst;
using Unity.Jobs;

#nullable disable
namespace Game.Rendering
{
  public class BatchUploadSystem : GameSystemBase
  {
    private BatchManagerSystem m_BatchManagerSystem;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances1 = this.m_BatchManagerSystem.GetNativeBatchInstances(false, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> nativeSubBatches1 = this.m_BatchManagerSystem.GetNativeSubBatches(true, out dependencies2);
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies1.Complete();
      dependencies2.Complete();
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances2 = nativeBatchInstances1;
      NativeSubBatches<CullingData, GroupData, BatchData, InstanceData> nativeSubBatches2 = nativeSubBatches1;
      managedBatches.StartUpload<CullingData, GroupData, BatchData, InstanceData>(nativeBatchInstances2, nativeSubBatches2);
      int activeGroupCount = nativeBatchInstances1.GetActiveGroupCount();
      BatchUploadSystem.BatchUploadJob jobData = new BatchUploadSystem.BatchUploadJob()
      {
        m_NativeBatchInstances = nativeBatchInstances1.BeginParallelUpload()
      };
      JobHandle jobHandle1 = jobData.Schedule<BatchUploadSystem.BatchUploadJob>(activeGroupCount, 1);
      JobHandle jobHandle2 = nativeBatchInstances1.EndParallelUpload(jobData.m_NativeBatchInstances, jobHandle1);
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeSubBatchesReader(jobHandle1);
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchInstancesWriter(jobHandle2);
    }

    [UnityEngine.Scripting.Preserve]
    public BatchUploadSystem()
    {
    }

    [BurstCompile]
    private struct BatchUploadJob : IJobParallelFor
    {
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>.ParallelUploadWriter m_NativeBatchInstances;

      public void Execute(int index) => this.m_NativeBatchInstances.UploadInstances(index);
    }
  }
}
