// Decompiled with JetBrains decompiler
// Type: Game.Serialization.HouseholdSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Serialization
{
  public class HouseholdSystem : GameSystemBase, IPostDeserialize
  {
    private EntityQuery m_MovingInHouseholdQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_MovingInHouseholdQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Household>()
        },
        None = new ComponentType[5]
        {
          ComponentType.ReadOnly<TouristHousehold>(),
          ComponentType.ReadOnly<CommuterHousehold>(),
          ComponentType.ReadOnly<MovingAway>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.RequireForUpdate(this.m_MovingInHouseholdQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (!(context.version < Version.clearMovingInHousehold))
        return;
      NativeArray<Entity> entityArray = this.m_MovingInHouseholdQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        Household component1;
        PropertyRenter component2;
        if (this.EntityManager.TryGetComponent<Household>(entityArray[index], out component1) && (component1.m_Flags & HouseholdFlags.MovedIn) == HouseholdFlags.None && (!this.EntityManager.TryGetComponent<PropertyRenter>(entityArray[index], out component2) || component2.m_Property == Entity.Null))
          this.EntityManager.AddComponent<Deleted>(entityArray[index]);
      }
      entityArray.Dispose();
    }

    [Preserve]
    public HouseholdSystem()
    {
    }
  }
}
