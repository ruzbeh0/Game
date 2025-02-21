// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CitizenSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class CitizenSection : InfoSectionBase
  {
    private EntityQuery m_HappinessParameterQuery;

    protected override string group => nameof (CitizenSection);

    private CitizenKey citizenKey { get; set; }

    private CitizenStateKey stateKey { get; set; }

    private Entity householdEntity { get; set; }

    private Entity residenceEntity { get; set; }

    private CitizenResidenceKey residenceKey { get; set; }

    private Entity workplaceEntity { get; set; }

    private Entity companyEntity { get; set; }

    private CitizenWorkplaceKey workplaceKey { get; set; }

    private CitizenOccupationKey occupationKey { get; set; }

    private CitizenJobLevelKey jobLevelKey { get; set; }

    private Entity schoolEntity { get; set; }

    private int schoolLevel { get; set; }

    private CitizenEducationKey educationKey { get; set; }

    private CitizenAgeKey ageKey { get; set; }

    private HouseholdWealthKey wealthKey { get; set; }

    private Entity destinationEntity { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HappinessParameterQuery);
    }

    protected override void Reset()
    {
      this.householdEntity = Entity.Null;
      this.residenceEntity = Entity.Null;
      this.workplaceEntity = Entity.Null;
      this.schoolEntity = Entity.Null;
      this.destinationEntity = Entity.Null;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<HouseholdMember>(this.selectedEntity) && this.EntityManager.HasComponent<Citizen>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Citizen componentData = this.EntityManager.GetComponentData<Citizen>(this.selectedEntity);
      HouseholdMember component;
      if (this.EntityManager.TryGetComponent<HouseholdMember>(this.selectedEntity, out component))
      {
        this.householdEntity = component.m_Household;
        this.citizenKey = CitizenKey.Citizen;
        if (this.EntityManager.HasComponent<CommuterHousehold>(component.m_Household))
          this.citizenKey = CitizenKey.Commuter;
        else if (this.EntityManager.HasComponent<TouristHousehold>(component.m_Household))
          this.citizenKey = CitizenKey.Tourist;
        // ISSUE: reference to a compiler-generated field
        this.wealthKey = CitizenUIUtils.GetHouseholdWealth(this.EntityManager, this.householdEntity, this.m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>());
      }
      this.stateKey = CitizenUIUtils.GetStateKey(this.EntityManager, this.selectedEntity);
      this.residenceEntity = CitizenUIUtils.GetResidenceEntity(this.EntityManager, this.selectedEntity);
      this.residenceKey = CitizenUIUtils.GetResidenceType(componentData);
      this.workplaceEntity = CitizenUIUtils.GetWorkplaceEntity(this.EntityManager, this.selectedEntity);
      this.companyEntity = CitizenUIUtils.GetCompanyEntity(this.EntityManager, this.selectedEntity);
      this.workplaceKey = CitizenUIUtils.GetWorkplaceType(this.EntityManager, this.selectedEntity);
      int level;
      this.schoolEntity = CitizenUIUtils.GetSchoolEntity(this.EntityManager, this.selectedEntity, out level);
      this.schoolLevel = level;
      this.occupationKey = CitizenUIUtils.GetOccupation(this.EntityManager, this.selectedEntity);
      this.jobLevelKey = CitizenUIUtils.GetJobLevel(this.EntityManager, this.selectedEntity);
      this.ageKey = CitizenUIUtils.GetAge(this.EntityManager, this.selectedEntity);
      this.educationKey = CitizenUIUtils.GetEducation(componentData);
      // ISSUE: reference to a compiler-generated method
      this.destinationEntity = this.GetDestination();
      if ((componentData.m_State & CitizenFlags.Male) == CitizenFlags.None)
        return;
      this.tooltipTags.Add(TooltipTags.Male.ToString());
    }

    private Entity GetDestination()
    {
      CurrentTransport component1;
      if (!this.EntityManager.TryGetComponent<CurrentTransport>(this.selectedEntity, out component1))
        return Entity.Null;
      Entity target = Entity.Null;
      Divert component2;
      if (this.EntityManager.TryGetComponent<Divert>(component1.m_CurrentTransport, out component2))
      {
        switch (component2.m_Purpose)
        {
          case Game.Citizens.Purpose.Shopping:
          case Game.Citizens.Purpose.Safety:
          case Game.Citizens.Purpose.SendMail:
            target = component2.m_Target;
            break;
        }
      }
      Target component3;
      if (target == Entity.Null && this.EntityManager.TryGetComponent<Target>(component1.m_CurrentTransport, out component3))
        target = component3.m_Target;
      Owner component4;
      return this.EntityManager.HasComponent<Game.Objects.OutsideConnection>(target) || !this.EntityManager.TryGetComponent<Owner>(target, out component4) ? target : component4.m_Owner;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("citizenKey");
      writer.Write(Enum.GetName(typeof (CitizenKey), (object) this.citizenKey));
      writer.PropertyName("stateKey");
      writer.Write(Enum.GetName(typeof (CitizenStateKey), (object) this.stateKey));
      writer.PropertyName("household");
      if (this.householdEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.householdEntity);
      }
      writer.PropertyName("householdEntity");
      if (this.householdEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.householdEntity);
      writer.PropertyName("residence");
      if (this.residenceEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.residenceEntity);
      }
      writer.PropertyName("residenceEntity");
      if (this.residenceEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.residenceEntity);
      writer.PropertyName("residenceKey");
      writer.Write(Enum.GetName(typeof (CitizenResidenceKey), (object) this.residenceKey));
      writer.PropertyName("workplace");
      if (this.companyEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.companyEntity);
      }
      writer.PropertyName("workplaceEntity");
      if (this.workplaceEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.workplaceEntity);
      writer.PropertyName("workplaceKey");
      writer.Write(Enum.GetName(typeof (CitizenWorkplaceKey), (object) this.workplaceKey));
      writer.PropertyName("occupationKey");
      writer.Write(Enum.GetName(typeof (CitizenOccupationKey), (object) this.occupationKey));
      writer.PropertyName("jobLevelKey");
      writer.Write(Enum.GetName(typeof (CitizenJobLevelKey), (object) this.jobLevelKey));
      writer.PropertyName("school");
      if (this.schoolEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.schoolEntity);
      }
      writer.PropertyName("schoolEntity");
      if (this.schoolEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.schoolEntity);
      writer.PropertyName("schoolLevel");
      writer.Write(this.schoolLevel);
      writer.PropertyName("educationKey");
      writer.Write(Enum.GetName(typeof (CitizenEducationKey), (object) this.educationKey));
      writer.PropertyName("ageKey");
      writer.Write(Enum.GetName(typeof (CitizenAgeKey), (object) this.ageKey));
      writer.PropertyName("wealthKey");
      writer.Write(Enum.GetName(typeof (HouseholdWealthKey), (object) this.wealthKey));
      writer.PropertyName("destination");
      if (this.destinationEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.destinationEntity);
      }
      writer.PropertyName("destinationEntity");
      if (this.destinationEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.destinationEntity);
    }

    [Preserve]
    public CitizenSection()
    {
    }
  }
}
