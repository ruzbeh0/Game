// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ServiceBudgetUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ServiceBudgetUISystem : UISystemBase
  {
    private const string kGroup = "serviceBudget";
    private PrefabSystem m_PrefabSystem;
    private CitySystem m_CitySystem;
    private ICityServiceBudgetSystem m_CityServiceBudgetSystem;
    private IServiceFeeSystem m_ServiceFeeSystem;
    private EntityQuery m_ServiceQuery;
    private ServiceBudgetUISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_2035132663_0;
    private EntityQuery __query_2035132663_1;
    private EntityQuery __query_2035132663_2;
    private EntityQuery __query_2035132663_3;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceBudgetSystem = (ICityServiceBudgetSystem) this.World.GetOrCreateSystemManaged<CityServiceBudgetSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeSystem = (IServiceFeeSystem) this.World.GetOrCreateSystemManaged<ServiceFeeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<UIObjectData>(), ComponentType.ReadOnly<ServiceData>());
      this.RequireForUpdate<ServiceFeeParameterData>();
      this.RequireForUpdate<OutsideTradeParameterData>();
      this.RequireForUpdate<CitizenHappinessParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
      this.AddUpdateBinding((IUpdateBinding) new RawValueBinding("serviceBudget", "services", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.m_ServiceQuery, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceFee> buffer = this.EntityManager.GetBuffer<ServiceFee>(this.m_CitySystem.City, true);
        writer.ArrayBegin(sortedObjects.Length);
        foreach (UIObjectInfo uiObjectInfo in sortedObjects)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(uiObjectInfo.prefabData);
          // ISSUE: reference to a compiler-generated method
          int totalBudget = this.GetTotalBudget(uiObjectInfo.entity, buffer);
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          writer.Write<ServiceBudgetUISystem.ServiceInfo>(new ServiceBudgetUISystem.ServiceInfo()
          {
            entity = uiObjectInfo.entity,
            name = prefab.name,
            icon = ImageSystem.GetIcon((PrefabBase) prefab),
            locked = this.EntityManager.HasEnabledComponent<Locked>(uiObjectInfo.entity),
            budget = totalBudget
          });
        }
        writer.ArrayEnd();
        sortedObjects.Dispose();
      })));
      this.AddUpdateBinding((IUpdateBinding) new RawMapBinding<Entity>("serviceBudget", "serviceDetails", (Action<IJsonWriter, Entity>) ((writer, serviceEntity) =>
      {
        ServiceData component1;
        PrefabData component2;
        if (this.EntityManager.TryGetComponent<ServiceData>(serviceEntity, out component1) && this.EntityManager.TryGetComponent<PrefabData>(serviceEntity, out component2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(component2);
          int upkeep;
          // ISSUE: reference to a compiler-generated field
          this.m_CityServiceBudgetSystem.GetEstimatedServiceBudget(serviceEntity, out upkeep);
          writer.TypeBegin("serviceBudget.ServiceDetails");
          writer.PropertyName("entity");
          writer.Write(serviceEntity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          writer.Write(ImageSystem.GetIcon((PrefabBase) prefab));
          writer.PropertyName("locked");
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(serviceEntity));
          writer.PropertyName("budgetAdjustable");
          writer.Write(component1.m_BudgetAdjustable);
          // ISSUE: reference to a compiler-generated field
          int serviceBudget = this.m_CityServiceBudgetSystem.GetServiceBudget(serviceEntity);
          writer.PropertyName("budgetPercentage");
          writer.Write(serviceBudget);
          writer.PropertyName("efficiency");
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.m_CityServiceBudgetSystem.GetServiceEfficiency(serviceEntity, serviceBudget));
          writer.PropertyName("upkeep");
          writer.Write(-upkeep);
          writer.PropertyName("fees");
          // ISSUE: reference to a compiler-generated method
          this.WriteServiceFees(writer, serviceEntity);
          writer.TypeEnd();
        }
        else
          writer.WriteNull();
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<Entity, int>("serviceBudget", "setServiceBudget", (Action<Entity, int>) ((service, percentage) => this.m_CityServiceBudgetSystem.SetServiceBudget(service, percentage))));
      // ISSUE: object of a compiler-generated type is created
      this.AddBinding((IBinding) new TriggerBinding<PlayerResource, float>("serviceBudget", "setServiceFee", (Action<PlayerResource, float>) ((resource, amount) =>
      {
        if (resource == PlayerResource.Parking)
          return;
        EntityManager entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        if (!entityManager.HasComponent<ServiceFee>(this.m_CitySystem.City))
          return;
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceFee> buffer = entityManager.GetBuffer<ServiceFee>(this.m_CitySystem.City);
        // ISSUE: reference to a compiler-generated method
        ServiceFeeSystem.SetFee(resource, buffer, amount);
      }), (IReader<PlayerResource>) new ServiceBudgetUISystem.PlayerResourceReader()));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("serviceBudget", "resetService", (Action<Entity>) (service =>
      {
        // ISSUE: reference to a compiler-generated method
        this.SetServiceBudget(service, 100);
        DynamicBuffer<CollectedCityServiceFeeData> buffer;
        if (!this.EntityManager.TryGetBuffer<CollectedCityServiceFeeData>(service, true, out buffer))
          return;
        // ISSUE: reference to a compiler-generated field
        ServiceFeeParameterData singleton = this.__query_2035132663_0.GetSingleton<ServiceFeeParameterData>();
        foreach (CollectedCityServiceFeeData cityServiceFeeData in buffer)
        {
          PlayerResource playerResource = (PlayerResource) cityServiceFeeData.m_PlayerResource;
          // ISSUE: reference to a compiler-generated method
          this.SetServiceFee(playerResource, singleton.GetFeeParameters(playerResource).m_Default);
        }
      })));
    }

    private void WriteServices(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.m_ServiceQuery, Allocator.Temp);
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<ServiceFee> buffer = this.EntityManager.GetBuffer<ServiceFee>(this.m_CitySystem.City, true);
      writer.ArrayBegin(sortedObjects.Length);
      foreach (UIObjectInfo uiObjectInfo in sortedObjects)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(uiObjectInfo.prefabData);
        // ISSUE: reference to a compiler-generated method
        int totalBudget = this.GetTotalBudget(uiObjectInfo.entity, buffer);
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        writer.Write<ServiceBudgetUISystem.ServiceInfo>(new ServiceBudgetUISystem.ServiceInfo()
        {
          entity = uiObjectInfo.entity,
          name = prefab.name,
          icon = ImageSystem.GetIcon((PrefabBase) prefab),
          locked = this.EntityManager.HasEnabledComponent<Locked>(uiObjectInfo.entity),
          budget = totalBudget
        });
      }
      writer.ArrayEnd();
      sortedObjects.Dispose();
    }

    private int GetTotalBudget(Entity service, DynamicBuffer<ServiceFee> fees)
    {
      int upkeep;
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceBudgetSystem.GetEstimatedServiceBudget(service, out upkeep);
      int totalBudget = -upkeep;
      DynamicBuffer<CollectedCityServiceFeeData> buffer;
      if (this.EntityManager.TryGetBuffer<CollectedCityServiceFeeData>(service, true, out buffer))
      {
        foreach (CollectedCityServiceFeeData cityServiceFeeData in buffer)
        {
          PlayerResource playerResource = (PlayerResource) cityServiceFeeData.m_PlayerResource;
          float fee;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num = ServiceFeeSystem.TryGetFee(playerResource, fees, out fee) ? this.m_ServiceFeeSystem.GetServiceFeeIncomeEstimate(playerResource, fee) : this.m_ServiceFeeSystem.GetServiceFees(playerResource).x;
          // ISSUE: reference to a compiler-generated field
          int3 serviceFees = this.m_ServiceFeeSystem.GetServiceFees(playerResource);
          totalBudget += num;
          totalBudget += serviceFees.y;
          totalBudget -= serviceFees.z;
        }
      }
      return totalBudget;
    }

    private void WriteServiceDetails(IJsonWriter writer, Entity serviceEntity)
    {
      ServiceData component1;
      PrefabData component2;
      if (this.EntityManager.TryGetComponent<ServiceData>(serviceEntity, out component1) && this.EntityManager.TryGetComponent<PrefabData>(serviceEntity, out component2))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(component2);
        int upkeep;
        // ISSUE: reference to a compiler-generated field
        this.m_CityServiceBudgetSystem.GetEstimatedServiceBudget(serviceEntity, out upkeep);
        writer.TypeBegin("serviceBudget.ServiceDetails");
        writer.PropertyName("entity");
        writer.Write(serviceEntity);
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        writer.Write(ImageSystem.GetIcon((PrefabBase) prefab));
        writer.PropertyName("locked");
        writer.Write(this.EntityManager.HasEnabledComponent<Locked>(serviceEntity));
        writer.PropertyName("budgetAdjustable");
        writer.Write(component1.m_BudgetAdjustable);
        // ISSUE: reference to a compiler-generated field
        int serviceBudget = this.m_CityServiceBudgetSystem.GetServiceBudget(serviceEntity);
        writer.PropertyName("budgetPercentage");
        writer.Write(serviceBudget);
        writer.PropertyName("efficiency");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_CityServiceBudgetSystem.GetServiceEfficiency(serviceEntity, serviceBudget));
        writer.PropertyName("upkeep");
        writer.Write(-upkeep);
        writer.PropertyName("fees");
        // ISSUE: reference to a compiler-generated method
        this.WriteServiceFees(writer, serviceEntity);
        writer.TypeEnd();
      }
      else
        writer.WriteNull();
    }

    private void WriteServiceFees(IJsonWriter writer, Entity serviceEntity)
    {
      DynamicBuffer<CollectedCityServiceFeeData> buffer1;
      if (this.EntityManager.TryGetBuffer<CollectedCityServiceFeeData>(serviceEntity, true, out buffer1) && buffer1.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        ServiceFeeParameterData feeParameters1 = this.__query_2035132663_0.GetSingleton<ServiceFeeParameterData>();
        // ISSUE: reference to a compiler-generated field
        OutsideTradeParameterData singleton = this.__query_2035132663_1.GetSingleton<OutsideTradeParameterData>();
        // ISSUE: reference to a compiler-generated field
        CitizenHappinessParameterData happinessParameters = this.__query_2035132663_2.GetSingleton<CitizenHappinessParameterData>();
        // ISSUE: reference to a compiler-generated field
        BuildingEfficiencyParameterData efficiencyParameters = this.__query_2035132663_3.GetSingleton<BuildingEfficiencyParameterData>();
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceFee> buffer2 = this.EntityManager.GetBuffer<ServiceFee>(this.m_CitySystem.City, true);
        writer.ArrayBegin(buffer1.Length);
        foreach (CollectedCityServiceFeeData cityServiceFeeData in buffer1)
        {
          PlayerResource playerResource = (PlayerResource) cityServiceFeeData.m_PlayerResource;
          FeeParameters feeParameters2 = feeParameters1.GetFeeParameters(playerResource);
          float fee;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num = ServiceFeeSystem.TryGetFee(playerResource, buffer2, out fee) ? this.m_ServiceFeeSystem.GetServiceFeeIncomeEstimate(playerResource, fee) : this.m_ServiceFeeSystem.GetServiceFees(playerResource).x;
          float relativeFee = fee / feeParameters2.m_Default;
          // ISSUE: reference to a compiler-generated field
          int3 serviceFees = this.m_ServiceFeeSystem.GetServiceFees(playerResource);
          writer.TypeBegin("serviceBudget.ServiceFee");
          writer.PropertyName("resource");
          writer.Write((int) playerResource);
          writer.PropertyName("name");
          writer.Write(Enum.GetName(typeof (PlayerResource), (object) playerResource));
          writer.PropertyName("fee");
          writer.Write(fee);
          writer.PropertyName("min");
          writer.Write(0);
          writer.PropertyName("max");
          writer.Write(feeParameters2.m_Max);
          writer.PropertyName("adjustable");
          writer.Write(feeParameters2.m_Adjustable);
          writer.PropertyName("importable");
          writer.Write(singleton.Importable(playerResource));
          writer.PropertyName("exportable");
          writer.Write(singleton.Exportable(playerResource));
          writer.PropertyName("incomeInternal");
          writer.Write(num);
          writer.PropertyName("incomeExports");
          writer.Write(serviceFees.y);
          writer.PropertyName("expenseImports");
          writer.Write(-serviceFees.z);
          writer.PropertyName("consumptionMultiplier");
          // ISSUE: reference to a compiler-generated method
          writer.Write(ServiceFeeSystem.GetConsumptionMultiplier(playerResource, relativeFee, in feeParameters1));
          writer.PropertyName("efficiencyMultiplier");
          // ISSUE: reference to a compiler-generated method
          writer.Write(ServiceFeeSystem.GetEfficiencyMultiplier(playerResource, relativeFee, in efficiencyParameters));
          writer.PropertyName("happinessEffect");
          // ISSUE: reference to a compiler-generated method
          writer.Write(ServiceFeeSystem.GetHappinessEffect(playerResource, relativeFee, in happinessParameters));
          writer.TypeEnd();
        }
        writer.ArrayEnd();
      }
      else
        writer.WriteEmptyArray();
    }

    private void SetServiceBudget(Entity service, int percentage)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceBudgetSystem.SetServiceBudget(service, percentage);
    }

    private void SetServiceFee(PlayerResource resource, float amount)
    {
      if (resource == PlayerResource.Parking)
        return;
      EntityManager entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      if (!entityManager.HasComponent<ServiceFee>(this.m_CitySystem.City))
        return;
      entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<ServiceFee> buffer = entityManager.GetBuffer<ServiceFee>(this.m_CitySystem.City);
      // ISSUE: reference to a compiler-generated method
      ServiceFeeSystem.SetFee(resource, buffer, amount);
    }

    private void ResetService(Entity service)
    {
      // ISSUE: reference to a compiler-generated method
      this.SetServiceBudget(service, 100);
      DynamicBuffer<CollectedCityServiceFeeData> buffer;
      if (!this.EntityManager.TryGetBuffer<CollectedCityServiceFeeData>(service, true, out buffer))
        return;
      // ISSUE: reference to a compiler-generated field
      ServiceFeeParameterData singleton = this.__query_2035132663_0.GetSingleton<ServiceFeeParameterData>();
      foreach (CollectedCityServiceFeeData cityServiceFeeData in buffer)
      {
        PlayerResource playerResource = (PlayerResource) cityServiceFeeData.m_PlayerResource;
        // ISSUE: reference to a compiler-generated method
        this.SetServiceFee(playerResource, singleton.GetFeeParameters(playerResource).m_Default);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_2035132663_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ServiceFeeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_2035132663_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<OutsideTradeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_2035132663_2 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<CitizenHappinessParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_2035132663_3 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<BuildingEfficiencyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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

    [Preserve]
    public ServiceBudgetUISystem()
    {
    }

    private struct ServiceInfo : IJsonWritable
    {
      public Entity entity;
      public string name;
      public string icon;
      public bool locked;
      public int budget;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("serviceBudget.Service");
        writer.PropertyName("entity");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.entity);
        writer.PropertyName("name");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.name);
        writer.PropertyName("icon");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.icon);
        writer.PropertyName("locked");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.locked);
        writer.PropertyName("budget");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.budget);
        writer.TypeEnd();
      }
    }

    private class PlayerResourceReader : IReader<PlayerResource>
    {
      public void Read(IJsonReader reader, out PlayerResource value)
      {
        int num;
        reader.Read(out num);
        value = (PlayerResource) num;
      }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct TypeHandle
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
      }
    }
  }
}
