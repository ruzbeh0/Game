// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.RaycastNameTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using Game.UI.Widgets;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class RaycastNameTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private DefaultToolSystem m_DefaultTool;
    private NameSystem m_NameSystem;
    private ImageSystem m_ImageSystem;
    private ToolRaycastSystem m_ToolRaycastSystem;
    private NameTooltip m_Tooltip;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultTool = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem = this.World.GetOrCreateSystemManaged<ToolRaycastSystem>();
      NameTooltip nameTooltip = new NameTooltip();
      nameTooltip.path = (PathSegment) "raycastName";
      // ISSUE: reference to a compiler-generated field
      nameTooltip.nameBinder = this.m_NameSystem;
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip = nameTooltip;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      RaycastResult result;
      PrefabRef component;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool != this.m_DefaultTool || !this.m_ToolRaycastSystem.GetRaycastResult(out result) || !this.EntityManager.HasComponent<Building>(result.m_Owner) && !this.EntityManager.HasComponent<Game.Routes.TransportStop>(result.m_Owner) && !this.EntityManager.HasComponent<Game.Objects.OutsideConnection>(result.m_Owner) && !this.EntityManager.HasComponent<Route>(result.m_Owner) && !this.EntityManager.HasComponent<Creature>(result.m_Owner) && !this.EntityManager.HasComponent<Vehicle>(result.m_Owner) && !this.EntityManager.HasComponent<Aggregate>(result.m_Owner) && !this.EntityManager.HasComponent<Game.Objects.NetObject>(result.m_Owner) || !this.EntityManager.TryGetComponent<PrefabRef>(result.m_Owner, out component))
        return;
      Entity owner = result.m_Owner;
      Entity prefab = component.m_Prefab;
      // ISSUE: reference to a compiler-generated method
      this.AdjustTargets(ref owner, ref prefab);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Tooltip.icon = this.m_ImageSystem.GetInstanceIcon(owner, prefab);
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip.entity = owner;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_Tooltip);
    }

    private void AdjustTargets(ref Entity instance, ref Entity prefab)
    {
      Game.Creatures.Resident component1;
      PrefabRef component2;
      if (this.EntityManager.TryGetComponent<Game.Creatures.Resident>(instance, out component1) && this.EntityManager.TryGetComponent<PrefabRef>(component1.m_Citizen, out component2))
      {
        instance = component1.m_Citizen;
        prefab = component2.m_Prefab;
      }
      Controller component3;
      PrefabRef component4;
      if (this.EntityManager.TryGetComponent<Controller>(instance, out component3) && this.EntityManager.TryGetComponent<PrefabRef>(component3.m_Controller, out component4))
      {
        instance = component3.m_Controller;
        prefab = component4.m_Prefab;
      }
      Game.Creatures.Pet component5;
      PrefabRef component6;
      if (!this.EntityManager.TryGetComponent<Game.Creatures.Pet>(instance, out component5) || !this.EntityManager.TryGetComponent<PrefabRef>(component5.m_HouseholdPet, out component6))
        return;
      instance = component5.m_HouseholdPet;
      prefab = component6.m_Prefab;
    }

    [Preserve]
    public RaycastNameTooltipSystem()
    {
    }
  }
}
