// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TelecomInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class TelecomInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "telecomInfo";
    private TerrainSystem m_TerrainSystem;
    private CitySystem m_CitySystem;
    private EntityQuery m_TelecomQuery;
    private EntityQuery m_TelecomModifiedQuery;
    private EntityQuery m_DensityQuery;
    private NativeArray<TelecomCoverage> m_Coverage;
    private NativeArray<TelecomStatus> m_Status;
    private ValueBinding<IndicatorValue> m_NetworkAvailability;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      this.m_TelecomQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Buildings.TelecomFacility>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>()
        }
      });
      this.m_TelecomModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Buildings.TelecomFacility>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.m_DensityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<HouseholdCitizen>(),
          ComponentType.ReadOnly<Employee>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      this.AddBinding((IBinding) (this.m_NetworkAvailability = new ValueBinding<IndicatorValue>("telecomInfo", "networkAvailability", new IndicatorValue(), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      this.m_Coverage = new NativeArray<TelecomCoverage>(0, Allocator.Persistent);
      this.m_Status = new NativeArray<TelecomStatus>(1, Allocator.Persistent);
    }

    protected override bool Active => base.Active || this.m_NetworkAvailability.active;

    protected override bool Modified => !this.m_TelecomModifiedQuery.IsEmptyIgnoreFilter;

    [Preserve]
    protected override void OnDestroy()
    {
      this.m_Coverage.Dispose();
      this.m_Status.Dispose();
      base.OnDestroy();
    }

    protected override void PerformUpdate()
    {
    }

    [Preserve]
    public TelecomInfoviewUISystem()
    {
    }
  }
}
