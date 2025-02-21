// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UnlockAllSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Simulation;
using Game.UI.InGame;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class UnlockAllSystem : GameSystemBase
  {
    private MilestoneSystem m_MilestoneSystem;
    private ModificationBarrier1 m_ModificationBarrier;
    private UIHighlightSystem m_UIHighlightSystem;
    private SignatureBuildingUISystem m_SignatureBuildingUISystem;
    private EntityQuery m_LockedQuery;
    private EntityArchetype m_UnlockEventArchetype;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneSystem = this.World.GetOrCreateSystemManaged<MilestoneSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_UIHighlightSystem = this.World.GetOrCreateSystemManaged<UIHighlightSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SignatureBuildingUISystem = this.World.GetOrCreateSystemManaged<SignatureBuildingUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LockedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Locked>(), ComponentType.Exclude<MilestoneData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LockedQuery);
      this.Enabled = false;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.UnlockAllImpl();
      this.Enabled = false;
    }

    private void UnlockAllImpl()
    {
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_ModificationBarrier.CreateCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_LockedQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        Entity prefab = entityArray[index];
        // ISSUE: reference to a compiler-generated field
        Entity entity = commandBuffer.CreateEntity(this.m_UnlockEventArchetype);
        commandBuffer.SetComponent<Unlock>(entity, new Unlock(prefab));
      }
      entityArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_MilestoneSystem.UnlockAllMilestones();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_UIHighlightSystem.SkipUpdate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SignatureBuildingUISystem.SkipUpdate();
    }

    [Preserve]
    public UnlockAllSystem()
    {
    }
  }
}
