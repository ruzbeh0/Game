// Decompiled with JetBrains decompiler
// Type: Game.Common.NativeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Net;
using Game.Objects;
using Game.Serialization;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Common
{
  public class NativeSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private EntityQuery m_EntityQuery;
    private EntityQuery m_NativeQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      this.m_EntityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<Object>(),
          ComponentType.ReadOnly<Area>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Native>()
        }
      });
      this.m_NativeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Native>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      switch (this.m_LoadGameSystem.context.purpose)
      {
        case Purpose.NewGame:
          this.EntityManager.AddComponent<Native>(this.m_EntityQuery);
          break;
        case Purpose.LoadMap:
          this.EntityManager.RemoveComponent<Native>(this.m_NativeQuery);
          break;
      }
    }

    [Preserve]
    public NativeSystem()
    {
    }
  }
}
