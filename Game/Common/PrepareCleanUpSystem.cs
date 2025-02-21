// Decompiled with JetBrains decompiler
// Type: Game.Common.PrepareCleanUpSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Common
{
  public class PrepareCleanUpSystem : GameSystemBase
  {
    private CleanUpSystem m_CleanUpSystem;
    private EntityQuery m_DeletedQuery;
    private EntityQuery m_UpdatedQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_CleanUpSystem = this.World.GetOrCreateSystemManaged<CleanUpSystem>();
      this.m_DeletedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Event>()
        }
      });
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[6]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Applied>(),
          ComponentType.ReadOnly<EffectsUpdated>(),
          ComponentType.ReadOnly<BatchesUpdated>(),
          ComponentType.ReadOnly<PathfindUpdated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
    }

    [Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle1;
      NativeList<Entity> entityListAsync1 = this.m_DeletedQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      NativeList<Entity> entityListAsync2 = this.m_UpdatedQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      this.m_CleanUpSystem.AddDeleted(entityListAsync1, outJobHandle1);
      this.m_CleanUpSystem.AddUpdated(entityListAsync2, outJobHandle2);
      this.Dependency = JobHandle.CombineDependencies(outJobHandle1, outJobHandle2);
    }

    [Preserve]
    public PrepareCleanUpSystem()
    {
    }
  }
}
