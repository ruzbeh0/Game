// Decompiled with JetBrains decompiler
// Type: Game.Common.CleanUpSystem
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
  public class CleanUpSystem : GameSystemBase
  {
    private NativeList<Entity> m_DeletedEntities;
    private NativeList<Entity> m_UpdatedEntities;
    private JobHandle m_DeletedDeps;
    private JobHandle m_UpdatedDeps;
    private ComponentTypeSet m_UpdateTypes;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_UpdateTypes = new ComponentTypeSet(new ComponentType[6]
      {
        ComponentType.ReadWrite<Created>(),
        ComponentType.ReadWrite<Updated>(),
        ComponentType.ReadWrite<Applied>(),
        ComponentType.ReadWrite<EffectsUpdated>(),
        ComponentType.ReadWrite<BatchesUpdated>(),
        ComponentType.ReadWrite<PathfindUpdated>()
      });
    }

    public void AddDeleted(NativeList<Entity> deletedEntities, JobHandle deletedDeps)
    {
      this.m_DeletedEntities = deletedEntities;
      this.m_DeletedDeps = deletedDeps;
    }

    public void AddUpdated(NativeList<Entity> updatedEntities, JobHandle updatedDeps)
    {
      this.m_UpdatedEntities = updatedEntities;
      this.m_UpdatedDeps = updatedDeps;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_DeletedDeps.Complete();
      this.m_UpdatedDeps.Complete();
      this.EntityManager.DestroyEntity(this.m_DeletedEntities.AsArray());
      this.EntityManager.RemoveComponent(this.m_UpdatedEntities.AsArray(), in this.m_UpdateTypes);
      this.m_DeletedEntities.Dispose();
      this.m_UpdatedEntities.Dispose();
    }

    [Preserve]
    public CleanUpSystem()
    {
    }
  }
}
