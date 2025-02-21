// Decompiled with JetBrains decompiler
// Type: Game.Zones.LoadSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Game.Serialization;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Zones
{
  public class LoadSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private EntityQuery m_EntityQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      this.m_EntityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Block>());
      this.RequireForUpdate(this.m_EntityQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.m_LoadGameSystem.context.purpose != Purpose.NewGame)
        return;
      this.EntityManager.AddComponent<Updated>(this.m_EntityQuery);
    }

    [Preserve]
    public LoadSystem()
    {
    }
  }
}
