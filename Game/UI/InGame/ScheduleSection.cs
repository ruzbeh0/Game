// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ScheduleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Prefabs;
using Game.Routes;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ScheduleSection : InfoSectionBase
  {
    private PoliciesUISystem m_PoliciesUISystem;
    private Entity m_NightRoutePolicy;
    private Entity m_DayRoutePolicy;
    private EntityQuery m_ConfigQuery;

    protected override string group => nameof (ScheduleSection);

    private RouteSchedule schedule { get; set; }

    protected override void Reset() => this.schedule = RouteSchedule.DayAndNight;

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PoliciesUISystem = this.World.GetOrCreateSystemManaged<PoliciesUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());
      this.AddBinding((IBinding) new TriggerBinding<int>(this.group, "setSchedule", (Action<int>) (newSchedule =>
      {
        switch ((RouteSchedule) newSchedule)
        {
          case RouteSchedule.Day:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_NightRoutePolicy, false);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_DayRoutePolicy, true);
            break;
          case RouteSchedule.Night:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_NightRoutePolicy, true);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_DayRoutePolicy, false);
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_NightRoutePolicy, false);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_DayRoutePolicy, false);
            break;
        }
      })));
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ConfigQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      UITransportConfigurationPrefab singletonPrefab = this.m_PrefabSystem.GetSingletonPrefab<UITransportConfigurationPrefab>(this.m_ConfigQuery);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DayRoutePolicy = this.m_PrefabSystem.GetEntity((PrefabBase) singletonPrefab.m_DayRoutePolicy);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NightRoutePolicy = this.m_PrefabSystem.GetEntity((PrefabBase) singletonPrefab.m_NightRoutePolicy);
    }

    private void OnSetSchedule(int newSchedule)
    {
      switch ((RouteSchedule) newSchedule)
      {
        case RouteSchedule.Day:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_NightRoutePolicy, false);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_DayRoutePolicy, true);
          break;
        case RouteSchedule.Night:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_NightRoutePolicy, true);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_DayRoutePolicy, false);
          break;
        default:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_NightRoutePolicy, false);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_DayRoutePolicy, false);
          break;
      }
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Route>(this.selectedEntity) && this.EntityManager.HasComponent<TransportLine>(this.selectedEntity) && this.EntityManager.HasComponent<RouteWaypoint>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Route componentData = this.EntityManager.GetComponentData<Route>(this.selectedEntity);
      this.schedule = RouteUtils.CheckOption(componentData, RouteOption.Day) ? RouteSchedule.Day : (RouteUtils.CheckOption(componentData, RouteOption.Night) ? RouteSchedule.Night : RouteSchedule.DayAndNight);
      this.tooltipTags.Add("TransportLine");
      this.tooltipTags.Add("CargoRoute");
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("schedule");
      writer.Write((int) this.schedule);
    }

    [Preserve]
    public ScheduleSection()
    {
    }
  }
}
