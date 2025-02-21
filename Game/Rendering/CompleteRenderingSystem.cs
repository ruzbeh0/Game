// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CompleteRenderingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Rendering;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  public class CompleteRenderingSystem : GameSystemBase
  {
    private BatchManagerSystem m_BatchManagerSystem;
    private ManagedBatchSystem m_ManagedBatchSystem;
    private ProceduralSkeletonSystem m_ProceduralSkeletonSystem;
    private ProceduralEmissiveSystem m_ProceduralEmissiveSystem;
    private WindTextureSystem m_WindTextureSystem;
    private BatchMeshSystem m_BatchMeshSystem;
    private UpdateSystem m_UpdateSystem;
    private OverlayInfomodeSystem m_OverlayInfomodeSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      this.m_ManagedBatchSystem = this.World.GetOrCreateSystemManaged<ManagedBatchSystem>();
      this.m_ProceduralSkeletonSystem = this.World.GetOrCreateSystemManaged<ProceduralSkeletonSystem>();
      this.m_ProceduralEmissiveSystem = this.World.GetOrCreateSystemManaged<ProceduralEmissiveSystem>();
      this.m_WindTextureSystem = this.World.GetOrCreateSystemManaged<WindTextureSystem>();
      this.m_BatchMeshSystem = this.World.GetOrCreateSystemManaged<BatchMeshSystem>();
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      this.m_OverlayInfomodeSystem = this.World.GetOrCreateSystemManaged<OverlayInfomodeSystem>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances1 = this.m_BatchManagerSystem.GetNativeBatchInstances(false, out dependencies);
      // ISSUE: reference to a compiler-generated method
      ManagedBatches<OptionalProperties> managedBatches = this.m_BatchManagerSystem.GetManagedBatches();
      dependencies.Complete();
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances2 = nativeBatchInstances1;
      managedBatches.EndUpload<CullingData, GroupData, BatchData, InstanceData>(nativeBatchInstances2);
      this.m_ProceduralSkeletonSystem.CompleteUpload();
      this.m_ProceduralEmissiveSystem.CompleteUpload();
      this.m_WindTextureSystem.CompleteUpdate();
      // ISSUE: reference to a compiler-generated method
      this.m_ManagedBatchSystem.CompleteVTRequests();
      // ISSUE: reference to a compiler-generated method
      this.m_BatchMeshSystem.CompleteMeshes();
      // ISSUE: reference to a compiler-generated method
      this.m_OverlayInfomodeSystem.ApplyOverlay();
      this.m_UpdateSystem.Update(SystemUpdatePhase.CompleteRendering);
    }

    [Preserve]
    public CompleteRenderingSystem()
    {
    }
  }
}
