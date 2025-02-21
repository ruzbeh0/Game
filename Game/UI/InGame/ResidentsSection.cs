// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ResidentsSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ResidentsSection : InfoSectionBase
  {
    private EntityQuery m_DistrictBuildingQuery;
    private EntityQuery m_HappinessParameterQuery;
    private NativeArray<int> m_Results;
    private NativeValue<Entity> m_ResidenceResult;
    private NativeList<Entity> m_HouseholdsResult;
    private ResidentsSection.TypeHandle __TypeHandle;

    protected override string group => nameof (ResidentsSection);

    private bool isHousehold { get; set; }

    private int householdCount { get; set; }

    private int maxHouseholds { get; set; }

    private HouseholdWealthKey wealthKey { get; set; }

    private int residentCount { get; set; }

    private int petCount { get; set; }

    private Entity residenceEntity { get; set; }

    private CitizenResidenceKey residenceKey { get; set; }

    private EducationData educationData { get; set; }

    private AgeData ageData { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<ResidentialProperty>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Renter>(), ComponentType.ReadOnly<CurrentDistrict>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdsResult = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ResidenceResult = new NativeValue<Entity>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(5, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdsResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidenceResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override void Reset()
    {
      this.isHousehold = false;
      this.householdCount = 0;
      this.maxHouseholds = 0;
      this.residentCount = 0;
      this.petCount = 0;
      this.educationData = new EducationData();
      this.ageData = new AgeData();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdsResult.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidenceResult.value = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[0] = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[1] = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[2] = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[4] = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_Results[3] = 0;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      if (this.EntityManager.HasComponent<District>(this.selectedEntity) && this.EntityManager.HasComponent<Area>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        new ResidentsSection.CountDistrictHouseholdsJob()
        {
          m_SelectedEntity = this.selectedEntity,
          m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CurrentDistrictHandle = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle,
          m_PrefabRefHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_AbandonedFromEntity = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
          m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
          m_TravelPurposeFromEntity = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
          m_PropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
          m_HouseholdFromEntity = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
          m_HouseholdCitizenFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
          m_HouseholdAnimalFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup,
          m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
          m_Results = this.m_Results,
          m_HouseholdsResult = this.m_HouseholdsResult
        }.Schedule<ResidentsSection.CountDistrictHouseholdsJob>(this.m_DistrictBuildingQuery, this.Dependency).Complete();
        // ISSUE: reference to a compiler-generated field
        this.visible = this.m_Results[0] > 0;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        new ResidentsSection.CountHouseholdsJob()
        {
          m_SelectedEntity = this.selectedEntity,
          m_SelectedPrefab = this.selectedPrefab,
          m_BuildingFromEntity = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_AbandonedFromEntity = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
          m_HouseholdFromEntity = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
          m_HealthProblemFromEntity = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
          m_TravelPurposeFromEntity = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
          m_PropertyRenterFromEntity = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
          m_PropertyDataFromEntity = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
          m_HouseholdCitizenFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
          m_HouseholdAnimalFromEntity = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RO_BufferLookup,
          m_RenterFromEntity = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
          m_Results = this.m_Results,
          m_HouseholdsResult = this.m_HouseholdsResult,
          m_ResidenceResult = this.m_ResidenceResult
        }.Schedule<ResidentsSection.CountHouseholdsJob>(this.Dependency).Complete();
        // ISSUE: reference to a compiler-generated field
        this.visible = this.m_Results[0] > 0;
      }
    }

    protected override void OnProcess()
    {
      if (this.EntityManager.HasComponent<Household>(this.selectedEntity))
        this.isHousehold = true;
      // ISSUE: reference to a compiler-generated field
      this.householdCount = this.m_Results[3];
      // ISSUE: reference to a compiler-generated field
      this.residentCount = this.m_Results[1];
      // ISSUE: reference to a compiler-generated field
      this.petCount = this.m_Results[2];
      // ISSUE: reference to a compiler-generated field
      this.maxHouseholds = this.m_Results[4];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.wealthKey = CitizenUIUtils.GetAverageHouseholdWealth(this.EntityManager, this.m_HouseholdsResult, this.m_HappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_HouseholdsResult.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<HouseholdCitizen> buffer = this.EntityManager.GetBuffer<HouseholdCitizen>(this.m_HouseholdsResult[index], true);
        // ISSUE: reference to a compiler-generated method
        this.ageData += this.GetAgeData(buffer);
        // ISSUE: reference to a compiler-generated method
        this.educationData += this.GetEducationData(buffer);
      }
      DynamicBuffer<HouseholdCitizen> buffer1;
      if (this.isHousehold && this.EntityManager.TryGetBuffer<HouseholdCitizen>(this.selectedEntity, true, out buffer1))
      {
        // ISSUE: reference to a compiler-generated method
        this.ageData += this.GetAgeData(buffer1);
        // ISSUE: reference to a compiler-generated method
        this.educationData += this.GetEducationData(buffer1);
      }
      // ISSUE: reference to a compiler-generated field
      this.residenceEntity = this.m_ResidenceResult.value;
      this.residenceKey = this.EntityManager.HasComponent<TouristHousehold>(this.selectedEntity) ? CitizenResidenceKey.Hotel : CitizenResidenceKey.Home;
    }

    private AgeData GetAgeData(DynamicBuffer<HouseholdCitizen> citizens)
    {
      int children = 0;
      int teens = 0;
      int adults = 0;
      int elders = 0;
      for (int index = 0; index < citizens.Length; ++index)
      {
        Entity citizen = citizens[index].m_Citizen;
        Citizen component;
        if (this.EntityManager.TryGetComponent<Citizen>(citizen, out component) && !CitizenUtils.IsCorpsePickedByHearse(this.EntityManager, citizen))
        {
          switch (component.GetAge())
          {
            case CitizenAge.Child:
              ++children;
              continue;
            case CitizenAge.Teen:
              ++teens;
              continue;
            case CitizenAge.Adult:
              ++adults;
              continue;
            case CitizenAge.Elderly:
              ++elders;
              continue;
            default:
              continue;
          }
        }
      }
      return new AgeData(children, teens, adults, elders);
    }

    private EducationData GetEducationData(DynamicBuffer<HouseholdCitizen> citizens)
    {
      int uneducated = 0;
      int poorlyEducated = 0;
      int educated = 0;
      int wellEducated = 0;
      int highlyEducated = 0;
      for (int index = 0; index < citizens.Length; ++index)
      {
        Citizen component;
        if (this.EntityManager.TryGetComponent<Citizen>(citizens[index].m_Citizen, out component) && !CitizenUtils.IsCorpsePickedByHearse(this.EntityManager, citizens[index].m_Citizen))
        {
          switch (component.GetEducationLevel())
          {
            case 0:
              ++uneducated;
              continue;
            case 1:
              ++poorlyEducated;
              continue;
            case 2:
              ++educated;
              continue;
            case 3:
              ++wellEducated;
              continue;
            case 4:
              ++highlyEducated;
              continue;
            default:
              continue;
          }
        }
      }
      return new EducationData(uneducated, poorlyEducated, educated, wellEducated, highlyEducated);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("isHousehold");
      writer.Write(this.isHousehold);
      writer.PropertyName("householdCount");
      writer.Write(this.householdCount);
      writer.PropertyName("maxHouseholds");
      writer.Write(this.maxHouseholds);
      writer.PropertyName("residentCount");
      writer.Write(this.residentCount);
      writer.PropertyName("petCount");
      writer.Write(this.petCount);
      writer.PropertyName("wealthKey");
      writer.Write(this.wealthKey.ToString());
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
      writer.PropertyName("ageData");
      writer.Write<AgeData>(this.ageData);
      writer.PropertyName("educationData");
      writer.Write<EducationData>(this.educationData);
    }

    private static bool TryCountHouseholds(
      ref int residentCount,
      ref int petCount,
      ref int householdCount,
      ref int maxHouseholds,
      Entity entity,
      Entity prefab,
      ref ComponentLookup<Abandoned> abandonedFromEntity,
      ref ComponentLookup<BuildingPropertyData> propertyDataFromEntity,
      ref ComponentLookup<HealthProblem> healthProblemFromEntity,
      ref ComponentLookup<TravelPurpose> travelPurposeFromEntity,
      ref ComponentLookup<Household> householdFromEntity,
      ref BufferLookup<Renter> renterFromEntity,
      ref BufferLookup<HouseholdCitizen> householdCitizenFromEntity,
      ref BufferLookup<HouseholdAnimal> householdAnimalFromEntity,
      NativeList<Entity> householdsResult)
    {
      bool flag = false;
      BuildingPropertyData componentData;
      if (!abandonedFromEntity.HasComponent(entity) && propertyDataFromEntity.TryGetComponent(prefab, out componentData) && componentData.m_ResidentialProperties > 0)
      {
        flag = true;
        maxHouseholds += componentData.m_ResidentialProperties;
        DynamicBuffer<Renter> bufferData1;
        if (renterFromEntity.TryGetBuffer(entity, out bufferData1))
        {
          for (int index1 = 0; index1 < bufferData1.Length; ++index1)
          {
            Entity renter = bufferData1[index1].m_Renter;
            DynamicBuffer<HouseholdCitizen> bufferData2;
            if (householdFromEntity.HasComponent(renter) && householdCitizenFromEntity.TryGetBuffer(renter, out bufferData2))
            {
              ++householdCount;
              householdsResult.Add(in renter);
              for (int index2 = 0; index2 < bufferData2.Length; ++index2)
              {
                if (!CitizenUtils.IsCorpsePickedByHearse(bufferData2[index2].m_Citizen, ref healthProblemFromEntity, ref travelPurposeFromEntity))
                  ++residentCount;
              }
              DynamicBuffer<HouseholdAnimal> bufferData3;
              if (householdAnimalFromEntity.TryGetBuffer(renter, out bufferData3))
                petCount += bufferData3.Length;
            }
          }
        }
      }
      return flag;
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
    public ResidentsSection()
    {
    }

    public enum Result
    {
      Visible,
      ResidentCount,
      PetCount,
      HouseholdCount,
      MaxHouseholds,
      ResultCount,
    }

    [BurstCompile]
    public struct CountHouseholdsJob : IJob
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
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposeFromEntity;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterFromEntity;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_PropertyDataFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimalFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      public NativeArray<int> m_Results;
      public NativeList<Entity> m_HouseholdsResult;
      public NativeValue<Entity> m_ResidenceResult;

      public void Execute()
      {
        int residentCount = 0;
        int petCount = 0;
        int householdCount = 0;
        int maxHouseholds = 0;
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
        // ISSUE: reference to a compiler-generated method
        if (this.m_BuildingFromEntity.HasComponent(this.m_SelectedEntity) && ResidentsSection.TryCountHouseholds(ref residentCount, ref petCount, ref householdCount, ref maxHouseholds, this.m_SelectedEntity, this.m_SelectedPrefab, ref this.m_AbandonedFromEntity, ref this.m_PropertyDataFromEntity, ref this.m_HealthProblemFromEntity, ref this.m_TravelPurposeFromEntity, ref this.m_HouseholdFromEntity, ref this.m_RenterFromEntity, ref this.m_HouseholdCitizenFromEntity, ref this.m_HouseholdAnimalFromEntity, this.m_HouseholdsResult))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Results[0] = 1;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[1] = residentCount;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[2] = petCount;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[3] = householdCount;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[4] = maxHouseholds;
        }
        else
        {
          DynamicBuffer<HouseholdCitizen> bufferData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_HouseholdFromEntity.HasComponent(this.m_SelectedEntity) || !this.m_HouseholdCitizenFromEntity.TryGetBuffer(this.m_SelectedEntity, out bufferData1))
            return;
          PropertyRenter componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PropertyRenterFromEntity.TryGetComponent(this.m_SelectedEntity, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_HouseholdsResult.Add(in this.m_SelectedEntity);
            // ISSUE: reference to a compiler-generated field
            this.m_ResidenceResult.value = componentData.m_Property;
          }
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!CitizenUtils.IsCorpsePickedByHearse(bufferData1[index].m_Citizen, ref this.m_HealthProblemFromEntity, ref this.m_TravelPurposeFromEntity))
              ++residentCount;
          }
          DynamicBuffer<HouseholdAnimal> bufferData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_HouseholdAnimalFromEntity.TryGetBuffer(this.m_SelectedEntity, out bufferData2))
            petCount += bufferData2.Length;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[0] = 1;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[1] = residentCount;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[2] = petCount;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[3] = 1;
          // ISSUE: reference to a compiler-generated field
          this.m_Results[4] = 1;
        }
      }
    }

    [BurstCompile]
    public struct CountDistrictHouseholdsJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> m_CurrentDistrictHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefHandle;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_AbandonedFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemFromEntity;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposeFromEntity;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_PropertyDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenFromEntity;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimalFromEntity;
      [ReadOnly]
      public BufferLookup<Renter> m_RenterFromEntity;
      public NativeArray<int> m_Results;
      public NativeList<Entity> m_HouseholdsResult;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentDistrict> nativeArray2 = chunk.GetNativeArray<CurrentDistrict>(ref this.m_CurrentDistrictHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefHandle);
        int num = 0;
        int residentCount = 0;
        int petCount = 0;
        int householdCount = 0;
        int maxHouseholds = 0;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          CurrentDistrict currentDistrict = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
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
          // ISSUE: reference to a compiler-generated method
          if (!(currentDistrict.m_District != this.m_SelectedEntity) && ResidentsSection.TryCountHouseholds(ref residentCount, ref petCount, ref householdCount, ref maxHouseholds, entity, prefabRef.m_Prefab, ref this.m_AbandonedFromEntity, ref this.m_PropertyDataFromEntity, ref this.m_HealthProblemFromEntity, ref this.m_TravelPurposeFromEntity, ref this.m_HouseholdFromEntity, ref this.m_RenterFromEntity, ref this.m_HouseholdCitizenFromEntity, ref this.m_HouseholdAnimalFromEntity, this.m_HouseholdsResult))
            num = 1;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += num;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += residentCount;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += petCount;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[3] += householdCount;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[4] += maxHouseholds;
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RO_BufferLookup = state.GetBufferLookup<HouseholdAnimal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
      }
    }
  }
}
