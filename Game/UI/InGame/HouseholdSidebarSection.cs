// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.HouseholdSidebarSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Citizens;
using Game.Economy;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class HouseholdSidebarSection : InfoSectionBase
  {
    private const string kHouseholdIcon = "Media/Game/Icons/Household.svg";
    private const string kResidenceIcon = "Media/Glyphs/Residence.svg";
    private const string kPetIcon = "Media/Game/Icons/Pet.svg";
    private const string kItemType = "Game.UI.InGame.HouseholdSidebarSection+HouseholdSidebarItem";
    private NativeArray<int> m_Results;
    private NativeArray<Entity> m_ResidenceResult;
    private NativeArray<HouseholdSidebarSection.HouseholdResult> m_HouseholdResult;
    private NativeList<HouseholdSidebarSection.ResidentResult> m_ResidentsResult;
    private NativeList<Entity> m_PetsResult;
    private NativeList<HouseholdSidebarSection.HouseholdResult> m_HouseholdsResult;
    private RawMapBinding<int> m_HouseholdMap;
    private RawMapBinding<int> m_ResidentMap;
    private RawMapBinding<int> m_PetMap;
    private HouseholdSidebarSection.TypeHandle __TypeHandle;

    protected override string group => nameof (HouseholdSidebarSection);

    protected override bool displayForDestroyedObjects => true;

    protected override bool displayForUnderConstruction => true;

    private Entity residenceEntity { get; set; }

    private HouseholdSidebarSection.HouseholdResult household { get; set; }

    private HouseholdSidebarSection.HouseholdSidebarVariant variant { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentsResult = new NativeList<HouseholdSidebarSection.ResidentResult>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_PetsResult = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdsResult = new NativeList<HouseholdSidebarSection.HouseholdResult>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(3, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ResidenceResult = new NativeArray<Entity>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdResult = new NativeArray<HouseholdSidebarSection.HouseholdResult>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_HouseholdMap = new RawMapBinding<int>(this.group, "householdMap", (Action<IJsonWriter, int>) ((writer, index) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_HouseholdsResult[index].m_Entity;
        DynamicBuffer<HouseholdCitizen> buffer = this.EntityManager.GetBuffer<HouseholdCitizen>(entity);
        // ISSUE: reference to a compiler-generated method
        this.WriteItem(writer, entity, "Media/Game/Icons/Household.svg", buffer.Length);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResidentMap = new RawMapBinding<int>(this.group, "residentMap", (Action<IJsonWriter, int>) ((writer, index) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_ResidentsResult[index].m_Entity;
        // ISSUE: reference to a compiler-generated method
        this.WriteItem(writer, entity, (string) null);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PetMap = new RawMapBinding<int>(this.group, "petMap", (Action<IJsonWriter, int>) ((writer, index) =>
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_PetsResult[index];
        // ISSUE: reference to a compiler-generated method
        this.WriteItem(writer, entity, "Media/Game/Icons/Pet.svg");
      }))));
    }

    private void BindHousehold(IJsonWriter writer, int index)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Entity entity = this.m_HouseholdsResult[index].m_Entity;
      DynamicBuffer<HouseholdCitizen> buffer = this.EntityManager.GetBuffer<HouseholdCitizen>(entity);
      // ISSUE: reference to a compiler-generated method
      this.WriteItem(writer, entity, "Media/Game/Icons/Household.svg", buffer.Length);
    }

    private void BindResident(IJsonWriter writer, int index)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Entity entity = this.m_ResidentsResult[index].m_Entity;
      // ISSUE: reference to a compiler-generated method
      this.WriteItem(writer, entity, (string) null);
    }

    private void BindPet(IJsonWriter writer, int index)
    {
      // ISSUE: reference to a compiler-generated field
      Entity entity = this.m_PetsResult[index];
      // ISSUE: reference to a compiler-generated method
      this.WriteItem(writer, entity, "Media/Game/Icons/Pet.svg");
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentsResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_PetsResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdsResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidenceResult.Dispose();
      base.OnDestroy();
    }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentsResult.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_PetsResult.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdsResult.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidenceResult[0] = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      ref NativeArray<HouseholdSidebarSection.HouseholdResult> local = ref this.m_HouseholdResult;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HouseholdSidebarSection.HouseholdResult householdResult1 = new HouseholdSidebarSection.HouseholdResult();
      // ISSUE: variable of a compiler-generated type
      HouseholdSidebarSection.HouseholdResult householdResult2 = householdResult1;
      local[0] = householdResult2;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[0] = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[1] = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[2] = 0;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      new HouseholdSidebarSection.CheckVisibilityJob()
      {
        m_SelectedEntity = this.selectedEntity,
        m_SelectedPrefab = this.selectedPrefab,
        m_BuildingFromEntity = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_AbandonedFromEntity = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
        m_HouseholdFromEntity = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_CitizenFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HouseholdPetFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup,
        m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_TravelPurposeFromEntity = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_PropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_HouseholdCitizenFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_Results = this.m_Results
      }.Schedule<HouseholdSidebarSection.CheckVisibilityJob>(this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.visible = this.m_Results[0] == 1 && this.m_Results[1] > 0;
      if (!this.visible)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      new HouseholdSidebarSection.CollectDataJob()
      {
        m_SelectedEntity = this.selectedEntity,
        m_BuildingFromEntity = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_HouseholdFromEntity = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_CitizenFromEntity = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HouseholdMemberFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_HouseholdPetFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdPet_RO_ComponentLookup,
        m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_TravelPurposeFromEntity = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_PropertyRenterFromEntity = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_HouseholdCitizenFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_ResourcesFromEntity = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_HouseholdAnimalsFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup,
        m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_ResidenceResult = this.m_ResidenceResult,
        m_HouseholdResult = this.m_HouseholdResult,
        m_HouseholdsResult = this.m_HouseholdsResult,
        m_ResidentsResult = this.m_ResidentsResult,
        m_PetsResult = this.m_PetsResult
      }.Schedule<HouseholdSidebarSection.CollectDataJob>(this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdsResult.Sort<HouseholdSidebarSection.HouseholdResult>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentsResult.Sort<HouseholdSidebarSection.ResidentResult>();
      // ISSUE: reference to a compiler-generated field
      this.m_PetsResult.Sort<Entity>();
      // ISSUE: reference to a compiler-generated field
      for (int key = 0; key < this.m_HouseholdsResult.Length; ++key)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdMap.Update(key);
      }
      // ISSUE: reference to a compiler-generated field
      for (int key = 0; key < this.m_ResidentsResult.Length; ++key)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ResidentMap.Update(key);
      }
      // ISSUE: reference to a compiler-generated field
      for (int key = 0; key < this.m_PetsResult.Length; ++key)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PetMap.Update(key);
      }
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      this.residenceEntity = this.m_ResidenceResult[0];
      // ISSUE: reference to a compiler-generated field
      this.household = this.m_HouseholdResult[0];
      // ISSUE: reference to a compiler-generated field
      this.variant = (HouseholdSidebarSection.HouseholdSidebarVariant) this.m_Results[2];
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("variant");
      IJsonWriter jsonWriter = writer;
      // ISSUE: variable of a compiler-generated type
      HouseholdSidebarSection.HouseholdSidebarVariant variant = this.variant;
      string str = variant.ToString();
      jsonWriter.Write(str);
      writer.PropertyName("residence");
      // ISSUE: reference to a compiler-generated method
      this.WriteItem(writer, this.residenceEntity, "Media/Glyphs/Residence.svg");
      writer.PropertyName("household");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.WriteItem(writer, this.household.m_Entity, "Media/Game/Icons/Household.svg", this.household.m_Members);
      writer.PropertyName("households");
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_HouseholdsResult.Length);
      writer.PropertyName("residents");
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ResidentsResult.Length);
      writer.PropertyName("pets");
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_PetsResult.Length);
    }

    private void WriteItem(IJsonWriter writer, Entity entity, string iconPath, int memberCount = 0)
    {
      writer.TypeBegin("Game.UI.InGame.HouseholdSidebarSection+HouseholdSidebarItem");
      writer.PropertyName(nameof (entity));
      writer.Write(entity);
      writer.PropertyName("name");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.BindName(writer, entity);
      writer.PropertyName("familyName");
      if (entity == Entity.Null || !this.EntityManager.HasComponent<Household>(entity))
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindFamilyName(writer, entity);
      }
      writer.PropertyName("icon");
      writer.Write(iconPath);
      writer.PropertyName("selected");
      writer.Write(entity == this.selectedEntity);
      writer.PropertyName("count");
      if (memberCount == 0)
        writer.WriteNull();
      else
        writer.Write(memberCount);
      writer.TypeEnd();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public HouseholdSidebarSection()
    {
    }

    public enum Result
    {
      Visible,
      ResidentCount,
      Type,
      ResultCount,
    }

    private enum HouseholdSidebarVariant
    {
      Citizen,
      Household,
      Building,
    }

    [BurstCompile]
    public struct CheckVisibilityJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public Entity m_SelectedPrefab;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_AbandonedFromEntity;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdFromEntity;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenFromEntity;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> m_HouseholdPetFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposeFromEntity;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_PropertyDataFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      public NativeArray<int> m_Results;

      public void Execute()
      {
        int residentCount = 0;
        int householdCount = 0;
        HouseholdPet componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CitizenFromEntity.HasComponent(this.m_SelectedEntity) || this.m_HouseholdPetFromEntity.TryGetComponent(this.m_SelectedEntity, out componentData) && componentData.m_Household != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Results[0] = 1;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[1] = 1;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[2] = 0;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_BuildingFromEntity.HasComponent(this.m_SelectedEntity) && this.HasResidentialProperties(ref residentCount, ref householdCount, this.m_SelectedEntity, this.m_SelectedPrefab))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Results[0] = 1;
            // ISSUE: reference to a compiler-generated field
            this.m_Results[1] = residentCount;
            // ISSUE: reference to a compiler-generated field
            this.m_Results[2] = 2;
          }
          else
          {
            DynamicBuffer<HouseholdCitizen> bufferData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_HouseholdFromEntity.HasComponent(this.m_SelectedEntity) || !this.m_HouseholdCitizenFromEntity.TryGetBuffer(this.m_SelectedEntity, out bufferData))
              return;
            for (int index = 0; index < bufferData.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!CitizenUtils.IsCorpsePickedByHearse(bufferData[index].m_Citizen, ref this.m_HealthProblemFromEntity, ref this.m_TravelPurposeFromEntity))
                ++residentCount;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_Results[0] = 1;
            // ISSUE: reference to a compiler-generated field
            this.m_Results[1] = residentCount;
            // ISSUE: reference to a compiler-generated field
            this.m_Results[2] = 1;
          }
        }
      }

      private bool HasResidentialProperties(
        ref int residentCount,
        ref int householdCount,
        Entity entity,
        Entity prefab)
      {
        bool flag = false;
        BuildingPropertyData componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AbandonedFromEntity.HasComponent(entity) && this.m_PropertyDataFromEntity.TryGetComponent(prefab, out componentData) && componentData.m_ResidentialProperties > 0)
        {
          flag = true;
          DynamicBuffer<Renter> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RenterFromEntity.TryGetBuffer(entity, out bufferData1))
          {
            for (int index1 = 0; index1 < bufferData1.Length; ++index1)
            {
              DynamicBuffer<HouseholdCitizen> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_HouseholdCitizenFromEntity.TryGetBuffer(bufferData1[index1].m_Renter, out bufferData2))
              {
                ++householdCount;
                for (int index2 = 0; index2 < bufferData2.Length; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!CitizenUtils.IsCorpsePickedByHearse(bufferData2[index2].m_Citizen, ref this.m_HealthProblemFromEntity, ref this.m_TravelPurposeFromEntity))
                    ++residentCount;
                }
              }
            }
          }
        }
        return flag;
      }
    }

    [BurstCompile]
    private struct CollectDataJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdFromEntity;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMemberFromEntity;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenFromEntity;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> m_HouseholdPetFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposeFromEntity;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenFromEntity;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_ResourcesFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimalsFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      public NativeArray<Entity> m_ResidenceResult;
      public NativeArray<HouseholdSidebarSection.HouseholdResult> m_HouseholdResult;
      public NativeList<HouseholdSidebarSection.HouseholdResult> m_HouseholdsResult;
      public NativeList<HouseholdSidebarSection.ResidentResult> m_ResidentsResult;
      public NativeList<Entity> m_PetsResult;

      public void Execute()
      {
        HouseholdPet componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_HouseholdPetFromEntity.TryGetComponent(this.m_SelectedEntity, out componentData1))
        {
          Entity household = componentData1.m_Household;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_HouseholdResult[0] = this.AddHousehold(household);
          PropertyRenter componentData2;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PropertyRenterFromEntity.TryGetComponent(household, out componentData2))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_ResidenceResult[0] = componentData2.m_Property;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CitizenFromEntity.HasComponent(this.m_SelectedEntity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity household = this.m_HouseholdMemberFromEntity[this.m_SelectedEntity].m_Household;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_HouseholdResult[0] = this.AddHousehold(household);
            PropertyRenter componentData3;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PropertyRenterFromEntity.TryGetComponent(household, out componentData3))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_ResidenceResult[0] = componentData3.m_Property;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingFromEntity.HasComponent(this.m_SelectedEntity))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Renter> dynamicBuffer = this.m_RenterFromEntity[this.m_SelectedEntity];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ResidenceResult[0] = this.m_SelectedEntity;
              for (int index = 0; index < dynamicBuffer.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated method
                // ISSUE: variable of a compiler-generated type
                HouseholdSidebarSection.HouseholdResult householdResult = this.AddHousehold(dynamicBuffer[index].m_Renter);
                if (index == 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_HouseholdResult[0] = householdResult;
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_HouseholdResult[0] = this.AddHousehold(this.m_SelectedEntity);
              PropertyRenter componentData4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PropertyRenterFromEntity.TryGetComponent(this.m_SelectedEntity, out componentData4))
                return;
              // ISSUE: reference to a compiler-generated field
              this.m_ResidenceResult[0] = componentData4.m_Property;
            }
          }
        }
      }

      private HouseholdSidebarSection.HouseholdResult AddHousehold(Entity householdEntity)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        HouseholdSidebarSection.HouseholdResult householdResult = new HouseholdSidebarSection.HouseholdResult();
        Household componentData1;
        DynamicBuffer<HouseholdCitizen> bufferData1;
        DynamicBuffer<Game.Economy.Resources> bufferData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_HouseholdFromEntity.TryGetComponent(householdEntity, out componentData1) && this.m_HouseholdCitizenFromEntity.TryGetBuffer(householdEntity, out bufferData1) && this.m_ResourcesFromEntity.TryGetBuffer(householdEntity, out bufferData2))
        {
          int x1 = 0;
          int x2 = 0;
          int num1 = 0;
          int num2 = 0;
          int num3 = 0;
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            Entity citizen = bufferData1[index].m_Citizen;
            Citizen componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CitizenFromEntity.TryGetComponent(citizen, out componentData2) && !CitizenUtils.IsCorpsePickedByHearse(citizen, ref this.m_HealthProblemFromEntity, ref this.m_TravelPurposeFromEntity))
            {
              int happiness = componentData2.Happiness;
              CitizenAge age = componentData2.GetAge();
              int educationLevel = componentData2.GetEducationLevel();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_ResidentsResult.Add(new HouseholdSidebarSection.ResidentResult()
              {
                m_Entity = citizen,
                m_Age = age,
                m_Happiness = happiness,
                m_Education = educationLevel
              });
              ++x1;
              num1 += happiness;
              num2 += (int) age;
              if (age == CitizenAge.Adult || age == CitizenAge.Teen)
              {
                ++x2;
                num3 += componentData2.GetEducationLevel();
              }
            }
          }
          DynamicBuffer<HouseholdAnimal> bufferData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_HouseholdAnimalsFromEntity.TryGetBuffer(householdEntity, out bufferData3))
          {
            for (int index = 0; index < bufferData3.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_PetsResult.Add(in bufferData3[index].m_HouseholdPet);
            }
          }
          // ISSUE: object of a compiler-generated type is created
          householdResult = new HouseholdSidebarSection.HouseholdResult()
          {
            m_Entity = householdEntity,
            m_Members = x1,
            m_Age = num2 / math.max(x1, 1),
            m_Happiness = num1 / math.max(x1, 1),
            m_Education = num3 / math.max(x2, 1),
            m_Wealth = EconomyUtils.GetHouseholdTotalWealth(componentData1, bufferData2)
          };
          // ISSUE: reference to a compiler-generated field
          if (householdResult.m_Members > 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_HouseholdsResult.Add(in householdResult);
          }
        }
        return householdResult;
      }
    }

    public struct HouseholdResult : 
      IComparable<HouseholdSidebarSection.HouseholdResult>,
      IEquatable<HouseholdSidebarSection.HouseholdResult>
    {
      public Entity m_Entity;
      public int m_Members;
      public int m_Age;
      public int m_Education;
      public int m_Happiness;
      public int m_Wealth;

      public int CompareTo(HouseholdSidebarSection.HouseholdResult other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = other.m_Members.CompareTo(this.m_Members);
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = other.m_Entity.CompareTo(this.m_Entity);
        }
        return num;
      }

      public bool Equals(HouseholdSidebarSection.HouseholdResult other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Entity.Equals(other.m_Entity) && this.m_Members == other.m_Members;
      }

      public override bool Equals(object obj)
      {
        // ISSUE: reference to a compiler-generated method
        return obj is HouseholdSidebarSection.HouseholdResult other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return HashCode.Combine<Entity, int>(this.m_Entity, this.m_Members);
      }
    }

    public class HouseholdComparer : IComparer<HouseholdSidebarSection.HouseholdResult>
    {
      private readonly Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int> m_Compare;

      public HouseholdComparer(
        HouseholdSidebarSection.HouseholdComparer.CompareBy compareBy)
      {
        Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int> func;
        switch (compareBy)
        {
          case HouseholdSidebarSection.HouseholdComparer.CompareBy.Members:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            func = (Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int>) ((x, y) => y.m_Members.CompareTo(x.m_Members));
            break;
          case HouseholdSidebarSection.HouseholdComparer.CompareBy.Age:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            func = (Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int>) ((x, y) => y.m_Age.CompareTo(x.m_Age));
            break;
          case HouseholdSidebarSection.HouseholdComparer.CompareBy.Education:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            func = (Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int>) ((x, y) => y.m_Education.CompareTo(x.m_Education));
            break;
          case HouseholdSidebarSection.HouseholdComparer.CompareBy.Happiness:
            func = (Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int>) ((x, y) =>
            {
              // ISSUE: reference to a compiler-generated field
              Game.Citizens.CitizenHappiness happinessKey = CitizenUtils.GetHappinessKey(x.m_Happiness);
              // ISSUE: reference to a compiler-generated field
              return CitizenUtils.GetHappinessKey(y.m_Happiness).CompareTo((object) happinessKey);
            });
            break;
          case HouseholdSidebarSection.HouseholdComparer.CompareBy.Wealth:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            func = (Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int>) ((x, y) => y.m_Wealth.CompareTo(x.m_Wealth));
            break;
          default:
            func = (Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int>) ((x, y) => 0);
            break;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Compare = func;
      }

      public int Compare(
        HouseholdSidebarSection.HouseholdResult x,
        HouseholdSidebarSection.HouseholdResult y)
      {
        // ISSUE: reference to a compiler-generated field
        Func<HouseholdSidebarSection.HouseholdResult, HouseholdSidebarSection.HouseholdResult, int> compare = this.m_Compare;
        int num = compare != null ? compare(x, y) : 0;
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = y.m_Members.CompareTo(x.m_Members);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = y.m_Age.CompareTo(x.m_Age);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = y.m_Education.CompareTo(x.m_Education);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Citizens.CitizenHappiness happinessKey = CitizenUtils.GetHappinessKey(x.m_Happiness);
          // ISSUE: reference to a compiler-generated field
          num = CitizenUtils.GetHappinessKey(y.m_Happiness).CompareTo((object) happinessKey);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = y.m_Wealth.CompareTo(x.m_Wealth);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = y.m_Entity.CompareTo(x.m_Entity);
        }
        return num;
      }

      public enum CompareBy
      {
        Members,
        Age,
        Education,
        Happiness,
        Wealth,
      }
    }

    public struct ResidentResult : 
      IComparable<HouseholdSidebarSection.ResidentResult>,
      IEquatable<HouseholdSidebarSection.ResidentResult>
    {
      public Entity m_Entity;
      public CitizenAge m_Age;
      public int m_Education;
      public int m_Happiness;

      public int CompareTo(HouseholdSidebarSection.ResidentResult other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = other.m_Age.CompareTo((object) this.m_Age);
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = other.m_Education.CompareTo(this.m_Education);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Citizens.CitizenHappiness happinessKey = CitizenUtils.GetHappinessKey(this.m_Happiness);
          // ISSUE: reference to a compiler-generated field
          num = CitizenUtils.GetHappinessKey(other.m_Happiness).CompareTo((object) happinessKey);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = other.m_Entity.CompareTo(this.m_Entity);
        }
        return num;
      }

      public bool Equals(HouseholdSidebarSection.ResidentResult other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Entity.Equals(other.m_Entity) && this.m_Age == other.m_Age;
      }

      public override bool Equals(object obj)
      {
        // ISSUE: reference to a compiler-generated method
        return obj is HouseholdSidebarSection.ResidentResult other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return HashCode.Combine<Entity, int>(this.m_Entity, (int) this.m_Age);
      }
    }

    public class ResidentComparer : IComparer<HouseholdSidebarSection.ResidentResult>
    {
      private readonly Func<HouseholdSidebarSection.ResidentResult, HouseholdSidebarSection.ResidentResult, int> m_Compare;

      public ResidentComparer(
        HouseholdSidebarSection.ResidentComparer.CompareBy compareBy)
      {
        Func<HouseholdSidebarSection.ResidentResult, HouseholdSidebarSection.ResidentResult, int> func;
        switch (compareBy)
        {
          case HouseholdSidebarSection.ResidentComparer.CompareBy.Age:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            func = (Func<HouseholdSidebarSection.ResidentResult, HouseholdSidebarSection.ResidentResult, int>) ((x, y) => y.m_Age.CompareTo((object) x.m_Age));
            break;
          case HouseholdSidebarSection.ResidentComparer.CompareBy.Education:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            func = (Func<HouseholdSidebarSection.ResidentResult, HouseholdSidebarSection.ResidentResult, int>) ((x, y) => y.m_Education.CompareTo(x.m_Education));
            break;
          case HouseholdSidebarSection.ResidentComparer.CompareBy.Happiness:
            func = (Func<HouseholdSidebarSection.ResidentResult, HouseholdSidebarSection.ResidentResult, int>) ((x, y) =>
            {
              // ISSUE: reference to a compiler-generated field
              Game.Citizens.CitizenHappiness happinessKey = CitizenUtils.GetHappinessKey(x.m_Happiness);
              // ISSUE: reference to a compiler-generated field
              return CitizenUtils.GetHappinessKey(y.m_Happiness).CompareTo((object) happinessKey);
            });
            break;
          default:
            func = (Func<HouseholdSidebarSection.ResidentResult, HouseholdSidebarSection.ResidentResult, int>) ((x, y) => 0);
            break;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Compare = func;
      }

      public int Compare(
        HouseholdSidebarSection.ResidentResult x,
        HouseholdSidebarSection.ResidentResult y)
      {
        // ISSUE: reference to a compiler-generated field
        Func<HouseholdSidebarSection.ResidentResult, HouseholdSidebarSection.ResidentResult, int> compare = this.m_Compare;
        int num = compare != null ? compare(x, y) : 0;
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = y.m_Age.CompareTo((object) x.m_Age);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = y.m_Education.CompareTo(x.m_Education);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Citizens.CitizenHappiness happinessKey = CitizenUtils.GetHappinessKey(x.m_Happiness);
          // ISSUE: reference to a compiler-generated field
          num = CitizenUtils.GetHappinessKey(y.m_Happiness).CompareTo((object) happinessKey);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = y.m_Entity.CompareTo(x.m_Entity);
        }
        return num;
      }

      public enum CompareBy
      {
        Age,
        Education,
        Happiness,
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdPet> __Game_Citizens_HouseholdPet_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdPet_RO_ComponentLookup = state.GetComponentLookup<HouseholdPet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RO_BufferLookup = state.GetBufferLookup<HouseholdAnimal>(true);
      }
    }
  }
}
