// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LifePathUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Citizens;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class LifePathUISystem : UISystemBase
  {
    private const string kGroup = "lifePath";
    private LifePathEventSystem m_LifePathEventSystem;
    private NameSystem m_NameSystem;
    private SelectedInfoUISystem m_SelectedInfoUISystem;
    private ChirperUISystem m_ChirperUISystem;
    private EntityQuery m_FollowedQuery;
    private EntityQuery m_HappinessParameterQuery;
    private int m_FollowedVersion;
    private int m_LifePathEntryVersion;
    private int m_ChirpVersion;
    private RawValueBinding m_FollowedCitizensBinding;
    private RawMapBinding<Entity> m_LifePathDetailsBinding;
    private RawMapBinding<Entity> m_LifePathItemsBinding;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LifePathEventSystem = this.World.GetOrCreateSystemManaged<LifePathEventSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoUISystem = this.World.GetOrCreateSystemManaged<SelectedInfoUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ChirperUISystem = this.World.GetOrCreateSystemManaged<ChirperUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FollowedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<Followed>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_FollowedCitizensBinding = new RawValueBinding("lifePath", "followedCitizens", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated method
        NativeArray<Entity> followedCitizens = this.GetSortedFollowedCitizens();
        binder.ArrayBegin(followedCitizens.Length);
        for (int index = 0; index < followedCitizens.Length; ++index)
        {
          Entity entity = followedCitizens[index];
          binder.TypeBegin("lifePath.FollowedCitizen");
          binder.PropertyName("entity");
          binder.Write(entity);
          binder.PropertyName("name");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_NameSystem.BindName(binder, entity);
          binder.PropertyName("age");
          binder.Write(Enum.GetName(typeof (CitizenAgeKey), (object) CitizenUIUtils.GetAge(this.EntityManager, entity)));
          binder.PropertyName("dead");
          binder.Write(CitizenUtils.IsDead(this.EntityManager, entity));
          binder.TypeEnd();
        }
        binder.ArrayEnd();
        followedCitizens.Dispose();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_LifePathDetailsBinding = new RawMapBinding<Entity>("lifePath", "lifePathDetails", (Action<IJsonWriter, Entity>) ((binder, citizen) => this.BindLifePathDetails(binder, citizen)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_LifePathItemsBinding = new RawMapBinding<Entity>("lifePath", "lifePathItems", (Action<IJsonWriter, Entity>) ((binder, citizen) => this.BindLifePathItems(binder, citizen)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<Entity>("lifePath", "followCitizen", (Action<Entity>) (citizen => this.m_LifePathEventSystem.FollowCitizen(citizen))));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("lifePath", "unfollowCitizen", (Action<Entity>) (citizen =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_LifePathEventSystem.UnfollowCitizen(citizen);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SelectedInfoUISystem.SetDirty();
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new ValueBinding<int>("lifePath", "maxFollowedCitizens", LifePathEventSystem.kMaxFollowed));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      int componentOrderVersion1 = this.EntityManager.GetComponentOrderVersion<Followed>();
      // ISSUE: reference to a compiler-generated field
      if (this.m_FollowedVersion != componentOrderVersion1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_FollowedCitizensBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_LifePathDetailsBinding.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_FollowedVersion = componentOrderVersion1;
      }
      int componentOrderVersion2 = this.EntityManager.GetComponentOrderVersion<LifePathEntry>();
      int componentOrderVersion3 = this.EntityManager.GetComponentOrderVersion<Game.Triggers.Chirp>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LifePathEntryVersion == componentOrderVersion2 && this.m_ChirpVersion == componentOrderVersion3)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_LifePathItemsBinding.UpdateAll();
      // ISSUE: reference to a compiler-generated field
      this.m_LifePathEntryVersion = componentOrderVersion2;
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpVersion = componentOrderVersion3;
    }

    private void FollowCitizen(Entity citizen) => this.m_LifePathEventSystem.FollowCitizen(citizen);

    private void UnfollowCitizen(Entity citizen)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LifePathEventSystem.UnfollowCitizen(citizen);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SelectedInfoUISystem.SetDirty();
    }

    private void BindFollowedCitizens(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated method
      NativeArray<Entity> followedCitizens = this.GetSortedFollowedCitizens();
      binder.ArrayBegin(followedCitizens.Length);
      for (int index = 0; index < followedCitizens.Length; ++index)
      {
        Entity entity = followedCitizens[index];
        binder.TypeBegin("lifePath.FollowedCitizen");
        binder.PropertyName("entity");
        binder.Write(entity);
        binder.PropertyName("name");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(binder, entity);
        binder.PropertyName("age");
        binder.Write(Enum.GetName(typeof (CitizenAgeKey), (object) CitizenUIUtils.GetAge(this.EntityManager, entity)));
        binder.PropertyName("dead");
        binder.Write(CitizenUtils.IsDead(this.EntityManager, entity));
        binder.TypeEnd();
      }
      binder.ArrayEnd();
      followedCitizens.Dispose();
    }

    private NativeArray<Entity> GetSortedFollowedCitizens()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_FollowedQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: object of a compiler-generated type is created
      entityArray.Sort<Entity, LifePathUISystem.FollowedCitizenComparer>(new LifePathUISystem.FollowedCitizenComparer(this.EntityManager));
      return entityArray;
    }

    private void BindLifePathDetails(IJsonWriter binder, Entity entity)
    {
      Citizen component;
      if (this.EntityManager.TryGetComponent<Citizen>(entity, out component) && this.EntityManager.HasComponent<Followed>(entity))
      {
        Entity residenceEntity = CitizenUIUtils.GetResidenceEntity(this.EntityManager, entity);
        CitizenResidenceKey residenceType = CitizenUIUtils.GetResidenceType(component);
        Entity workplaceEntity = CitizenUIUtils.GetWorkplaceEntity(this.EntityManager, entity);
        Entity companyEntity = CitizenUIUtils.GetCompanyEntity(this.EntityManager, entity);
        CitizenWorkplaceKey workplaceType = CitizenUIUtils.GetWorkplaceType(this.EntityManager, entity);
        Entity schoolEntity = CitizenUIUtils.GetSchoolEntity(this.EntityManager, entity, out int _);
        CitizenOccupationKey occupation = CitizenUIUtils.GetOccupation(this.EntityManager, entity);
        CitizenJobLevelKey jobLevel = CitizenUIUtils.GetJobLevel(this.EntityManager, entity);
        CitizenAgeKey age = CitizenUIUtils.GetAge(this.EntityManager, entity);
        CitizenEducationKey education = CitizenUIUtils.GetEducation(component);
        HouseholdMember componentData = this.EntityManager.GetComponentData<HouseholdMember>(entity);
        NativeList<CitizenCondition> citizenConditions = CitizenUIUtils.GetCitizenConditions(this.EntityManager, entity, component, componentData, new NativeList<CitizenCondition>((AllocatorManager.AllocatorHandle) Allocator.TempJob));
        // ISSUE: reference to a compiler-generated field
        HouseholdWealthKey householdWealth = CitizenUIUtils.GetHouseholdWealth(this.EntityManager, componentData.m_Household, this.m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>());
        bool flag = CitizenUtils.IsDead(this.EntityManager, entity);
        binder.TypeBegin("lifePath.LifePathDetails");
        binder.PropertyName(nameof (entity));
        binder.Write(entity);
        binder.PropertyName("name");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(binder, entity);
        binder.PropertyName("avatar");
        binder.WriteNull();
        binder.PropertyName("randomIndex");
        // ISSUE: reference to a compiler-generated method
        binder.Write(this.GetRandomIndex(entity));
        binder.PropertyName("birthDay");
        binder.Write((int) component.m_BirthDay);
        binder.PropertyName("age");
        binder.Write(Enum.GetName(typeof (CitizenAgeKey), (object) age));
        binder.PropertyName("education");
        binder.Write(Enum.GetName(typeof (CitizenEducationKey), (object) education));
        binder.PropertyName("wealth");
        binder.Write(Enum.GetName(typeof (HouseholdWealthKey), (object) householdWealth));
        binder.PropertyName("occupation");
        binder.Write(Enum.GetName(typeof (CitizenOccupationKey), (object) occupation));
        binder.PropertyName("jobLevel");
        binder.Write(Enum.GetName(typeof (CitizenJobLevelKey), (object) jobLevel));
        binder.PropertyName("residenceName");
        if (residenceEntity == Entity.Null)
        {
          binder.WriteNull();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_NameSystem.BindName(binder, residenceEntity);
        }
        binder.PropertyName("residenceEntity");
        if (residenceEntity == Entity.Null)
          binder.WriteNull();
        else
          binder.Write(residenceEntity);
        binder.PropertyName("residenceKey");
        binder.Write(Enum.GetName(typeof (CitizenResidenceKey), (object) residenceType));
        binder.PropertyName("workplaceName");
        if (companyEntity == Entity.Null)
        {
          binder.WriteNull();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_NameSystem.BindName(binder, companyEntity);
        }
        binder.PropertyName("workplaceEntity");
        if (workplaceEntity == Entity.Null)
          binder.WriteNull();
        else
          binder.Write(workplaceEntity);
        binder.PropertyName("workplaceKey");
        binder.Write(Enum.GetName(typeof (CitizenWorkplaceKey), (object) workplaceType));
        binder.PropertyName("schoolName");
        if (schoolEntity == Entity.Null)
        {
          binder.WriteNull();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_NameSystem.BindName(binder, schoolEntity);
        }
        binder.PropertyName("schoolEntity");
        if (schoolEntity == Entity.Null)
          binder.WriteNull();
        else
          binder.Write(schoolEntity);
        binder.PropertyName("conditions");
        if (flag)
        {
          binder.WriteEmptyArray();
        }
        else
        {
          binder.ArrayBegin(citizenConditions.Length);
          for (int index = 0; index < citizenConditions.Length; ++index)
            binder.Write<CitizenCondition>(citizenConditions[index]);
          binder.ArrayEnd();
        }
        binder.PropertyName("happiness");
        if (flag)
          binder.WriteNull();
        else
          binder.Write<CitizenHappiness>(CitizenUIUtils.GetCitizenHappiness(component));
        binder.PropertyName("state");
        binder.Write(Enum.GetName(typeof (CitizenStateKey), (object) CitizenUIUtils.GetStateKey(this.EntityManager, entity)));
        binder.TypeEnd();
        citizenConditions.Dispose();
      }
      else
        binder.WriteNull();
    }

    private void BindLifePathItems(IJsonWriter binder, Entity citizen)
    {
      DynamicBuffer<LifePathEntry> buffer;
      if (this.EntityManager.TryGetBuffer<LifePathEntry>(citizen, true, out buffer))
      {
        binder.ArrayBegin(buffer.Length);
        for (int index = buffer.Length - 1; index >= 0; --index)
        {
          Entity entity = buffer[index].m_Entity;
          if (!this.EntityManager.HasComponent<Deleted>(entity))
          {
            if (this.EntityManager.HasComponent<Game.Triggers.Chirp>(entity))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_ChirperUISystem.BindChirp(binder, entity);
            }
            else if (this.EntityManager.HasComponent<Game.Triggers.LifePathEvent>(entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.BindLifePathEvent(binder, entity);
            }
            else
              binder.WriteNull();
          }
          else
            binder.WriteNull();
        }
        binder.ArrayEnd();
      }
      else
        binder.WriteEmptyArray();
    }

    private void BindLifePathEvent(IJsonWriter binder, Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      string messageId = this.m_ChirperUISystem.GetMessageID(entity);
      Game.Triggers.LifePathEvent componentData = this.EntityManager.GetComponentData<Game.Triggers.LifePathEvent>(entity);
      binder.TypeBegin("lifePath.LifePathEvent");
      binder.PropertyName(nameof (entity));
      binder.Write(entity);
      binder.PropertyName("date");
      binder.Write(componentData.m_Date);
      binder.PropertyName("messageId");
      binder.Write(messageId);
      binder.TypeEnd();
    }

    private int GetRandomIndex(Entity entity)
    {
      DynamicBuffer<RandomLocalizationIndex> buffer;
      return this.EntityManager.TryGetBuffer<RandomLocalizationIndex>(entity, true, out buffer) && buffer.Length > 0 ? buffer[0].m_Index : 0;
    }

    [Preserve]
    public LifePathUISystem()
    {
    }

    private struct FollowedCitizenComparer : IComparer<Entity>
    {
      private EntityManager m_EntityManager;

      public FollowedCitizenComparer(EntityManager entityManager)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_EntityManager = entityManager;
      }

      public int Compare(Entity a, Entity b)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = this.m_EntityManager.GetComponentData<Followed>(a).m_Priority.CompareTo(this.m_EntityManager.GetComponentData<Followed>(b).m_Priority);
        return num == 0 ? a.CompareTo(b) : num;
      }
    }
  }
}
