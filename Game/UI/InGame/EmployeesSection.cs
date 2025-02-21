// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.EmployeesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class EmployeesSection : InfoSectionBase
  {
    private EntityQuery m_DistrictBuildingQuery;

    protected override string group => nameof (EmployeesSection);

    private int employeeCount { get; set; }

    private int maxEmployees { get; set; }

    private EmploymentData educationDataEmployees { get; set; }

    private EmploymentData educationDataWorkplaces { get; set; }

    private NativeList<Entity> districtBuildings { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictBuildingQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<CurrentDistrict>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Renter>(),
          ComponentType.ReadOnly<Employee>()
        },
        None = new ComponentType[2]
        {
          ComponentType.Exclude<Temp>(),
          ComponentType.Exclude<Deleted>()
        }
      });
      this.districtBuildings = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.districtBuildings.Dispose();
      base.OnDestroy();
    }

    protected override void Reset()
    {
      this.employeeCount = 0;
      this.maxEmployees = 0;
      this.educationDataEmployees = new EmploymentData();
      this.educationDataWorkplaces = new EmploymentData();
      this.districtBuildings.Clear();
    }

    private bool Visible()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return this.EntityManager.HasComponent<District>(this.selectedEntity) && this.EntityManager.HasComponent<Area>(this.selectedEntity) ? this.DisplayForDistrict() : this.HasEmployees(this.selectedEntity, this.selectedPrefab);
    }

    private bool HasEmployees(Entity entity, Entity prefab)
    {
      DynamicBuffer<Renter> buffer;
      if (!this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer) || this.EntityManager.HasComponent<Game.Buildings.Park>(entity))
        return (!this.EntityManager.HasComponent<Employee>(entity) ? 0 : (this.EntityManager.HasComponent<WorkProvider>(entity) ? 1 : 0)) != 0 && this.Enabled;
      SpawnableBuildingData component;
      if (buffer.Length == 0 && this.EntityManager.TryGetComponent<SpawnableBuildingData>(prefab, out component))
      {
        ZonePrefab prefab1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.TryGetPrefab<ZonePrefab>(component.m_ZonePrefab, out prefab1);
        if (!((Object) prefab1 != (Object) null))
          return false;
        return prefab1.m_AreaType == Game.Zones.AreaType.Commercial || prefab1.m_AreaType == Game.Zones.AreaType.Industrial;
      }
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity renter = buffer[index].m_Renter;
        if (this.EntityManager.HasComponent<CompanyData>(renter))
          return this.EntityManager.HasComponent<Employee>(renter) && this.EntityManager.HasComponent<WorkProvider>(renter);
      }
      return false;
    }

    private bool DisplayForDistrict()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_DistrictBuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<CurrentDistrict> componentDataArray1 = this.m_DistrictBuildingQuery.ToComponentDataArray<CurrentDistrict>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<PrefabRef> componentDataArray2 = this.m_DistrictBuildingQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        for (int index = 0; index < entityArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (!(componentDataArray1[index].m_District != this.selectedEntity) && this.HasEmployees(entityArray[index], componentDataArray2[index].m_Prefab))
            return true;
        }
      }
      finally
      {
        entityArray.Dispose();
        componentDataArray1.Dispose();
        componentDataArray2.Dispose();
      }
      return false;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.visible = this.Visible();
      if (!this.visible)
        return;
      // ISSUE: reference to a compiler-generated method
      this.AddEmployees();
      this.visible = this.maxEmployees > 0;
    }

    protected override void OnProcess()
    {
    }

    private void AddEmployees()
    {
      if (this.EntityManager.HasComponent<ServiceUsage>(this.selectedEntity))
        this.tooltipKeys.Add("ServiceUsage");
      if (this.EntityManager.HasComponent<District>(this.selectedEntity) && this.EntityManager.HasComponent<Area>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateForDistricts();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.AddEmployees(this.selectedEntity);
      }
    }

    private void UpdateForDistricts()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_DistrictBuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<CurrentDistrict> componentDataArray1 = this.m_DistrictBuildingQuery.ToComponentDataArray<CurrentDistrict>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<PrefabRef> componentDataArray2 = this.m_DistrictBuildingQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        for (int index = 0; index < entityArray.Length; ++index)
        {
          if (!(componentDataArray1[index].m_District != this.selectedEntity))
          {
            Entity entity = entityArray[index];
            // ISSUE: reference to a compiler-generated method
            if (this.HasEmployees(entity, componentDataArray2[index].m_Prefab))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddEmployees(entity);
            }
          }
        }
      }
      finally
      {
        entityArray.Dispose();
        componentDataArray1.Dispose();
        componentDataArray2.Dispose();
      }
    }

    private void AddEmployees(Entity entity)
    {
      Entity prefab1 = this.EntityManager.GetComponentData<PrefabRef>(entity).m_Prefab;
      // ISSUE: reference to a compiler-generated method
      Entity entity1 = this.GetEntity(entity);
      Entity prefab2 = this.EntityManager.GetComponentData<PrefabRef>(entity1).m_Prefab;
      int buildingLevel = 1;
      SpawnableBuildingData component1;
      if (this.EntityManager.TryGetComponent<SpawnableBuildingData>(prefab1, out component1))
      {
        buildingLevel = (int) component1.m_Level;
      }
      else
      {
        PropertyRenter component2;
        PrefabRef component3;
        SpawnableBuildingData component4;
        if (this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component2) && this.EntityManager.TryGetComponent<PrefabRef>(component2.m_Property, out component3) && this.EntityManager.TryGetComponent<SpawnableBuildingData>(component3.m_Prefab, out component4))
          buildingLevel = (int) component4.m_Level;
      }
      DynamicBuffer<Employee> buffer;
      WorkProvider component5;
      if (!this.EntityManager.TryGetBuffer<Employee>(entity1, true, out buffer) || !this.EntityManager.TryGetComponent<WorkProvider>(entity1, out component5))
        return;
      this.employeeCount += buffer.Length;
      WorkplaceComplexity complexity = this.EntityManager.GetComponentData<WorkplaceData>(prefab2).m_Complexity;
      EmploymentData workplacesData = EmploymentData.GetWorkplacesData(component5.m_MaxWorkers, buildingLevel, complexity);
      this.maxEmployees += workplacesData.total;
      this.educationDataWorkplaces += workplacesData;
      this.educationDataEmployees += EmploymentData.GetEmployeesData(buffer, workplacesData.total - buffer.Length);
    }

    private Entity GetEntity(Entity entity)
    {
      DynamicBuffer<Renter> buffer;
      if (!this.EntityManager.HasComponent<Game.Buildings.Park>(entity) && this.EntityManager.TryGetBuffer<Renter>(entity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity renter = buffer[index].m_Renter;
          if (this.EntityManager.HasComponent<CompanyData>(renter))
            return renter;
        }
      }
      return entity;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("employeeCount");
      writer.Write(this.employeeCount);
      writer.PropertyName("maxEmployees");
      writer.Write(this.maxEmployees);
      writer.PropertyName("educationDataEmployees");
      writer.Write<EmploymentData>(this.educationDataEmployees);
      writer.PropertyName("educationDataWorkplaces");
      writer.Write<EmploymentData>(this.educationDataWorkplaces);
    }

    [Preserve]
    public EmployeesSection()
    {
    }
  }
}
