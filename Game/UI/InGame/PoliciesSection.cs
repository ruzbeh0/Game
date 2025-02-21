// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PoliciesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Policies;
using Game.Routes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PoliciesSection : InfoSectionBase
  {
    private PoliciesUISystem m_PoliciesUISystem;

    protected override string group => nameof (PoliciesSection);

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PoliciesUISystem = this.World.GetOrCreateSystemManaged<PoliciesUISystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PoliciesUISystem.EventPolicyUnlocked += new Action(this.m_InfoUISystem.RequestUpdate);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PoliciesUISystem.EventPolicyUnlocked -= new Action(this.m_InfoUISystem.RequestUpdate);
    }

    protected override void Reset()
    {
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.visible = this.EntityManager.HasComponent<Policy>(this.selectedEntity) && this.m_PoliciesUISystem.GatherSelectedInfoPolicies(this.selectedEntity);
    }

    protected override void OnProcess()
    {
      if (this.EntityManager.HasComponent<Building>(this.selectedEntity))
      {
        List<string> tooltipKeys = this.tooltipKeys;
        // ISSUE: variable of a compiler-generated type
        PoliciesSection.PoliciesKey policiesKey = PoliciesSection.PoliciesKey.Building;
        string str = policiesKey.ToString();
        tooltipKeys.Add(str);
      }
      else if (this.EntityManager.HasComponent<District>(this.selectedEntity))
      {
        List<string> tooltipKeys = this.tooltipKeys;
        // ISSUE: variable of a compiler-generated type
        PoliciesSection.PoliciesKey policiesKey = PoliciesSection.PoliciesKey.District;
        string str = policiesKey.ToString();
        tooltipKeys.Add(str);
      }
      else
      {
        if (!this.EntityManager.HasComponent<Route>(this.selectedEntity))
          return;
        this.tooltipTags.Add(TooltipTags.CargoRoute.ToString());
        this.tooltipTags.Add(TooltipTags.TransportLine.ToString());
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("policies");
      if (this.EntityManager.HasComponent<Building>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PoliciesUISystem.BindBuildingPolicies(writer);
      }
      else if (this.EntityManager.HasComponent<District>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PoliciesUISystem.BindDistrictPolicies(writer);
      }
      else if (this.EntityManager.HasComponent<Route>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PoliciesUISystem.BindRoutePolicies(writer);
      }
      else
        writer.WriteNull();
    }

    [Preserve]
    public PoliciesSection()
    {
    }

    private enum PoliciesKey
    {
      Building,
      District,
    }
  }
}
