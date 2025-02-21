// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UpkeepSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class UpkeepSection : InfoSectionBase
  {
    private ResourceSystem m_ResourceSystem;
    private PrefabUISystem m_PrefabUISystem;
    private EntityQuery m_BudgetDataQuery;
    private EntityQuery m_EconomyParameterQuery;
    private UpkeepSection.TypeHandle __TypeHandle;

    protected override string group => nameof (UpkeepSection);

    private Dictionary<string, UpkeepSection.UIUpkeepItem> moneyUpkeep { get; set; }

    private Dictionary<Resource, UpkeepSection.UIUpkeepItem> resourceUpkeep { get; set; }

    private List<UpkeepSection.UIUpkeepItem> upkeeps { get; set; }

    private int total { get; set; }

    private bool inactive { get; set; }

    protected override bool displayForUpgrades => true;

    protected override void Reset()
    {
      this.moneyUpkeep.Clear();
      this.resourceUpkeep.Clear();
      this.upkeeps.Clear();
      this.total = 0;
      this.inactive = false;
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BudgetDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceBudgetData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      this.resourceUpkeep = new Dictionary<Resource, UpkeepSection.UIUpkeepItem>(5);
      this.moneyUpkeep = new Dictionary<string, UpkeepSection.UIUpkeepItem>(5);
      this.upkeeps = new List<UpkeepSection.UIUpkeepItem>(10);
    }

    private bool Visible()
    {
      if (this.EntityManager.HasComponent<ServiceUpkeepData>(this.selectedPrefab))
        return true;
      return this.EntityManager.HasComponent<ServiceObjectData>(this.selectedPrefab) && this.EntityManager.HasComponent<WorkplaceData>(this.selectedPrefab);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    private void CalculateServiceUpkeepDatas(
      Entity entity,
      Entity prefabEntity,
      Entity buildingOwnerEntity,
      DynamicBuffer<ServiceUpkeepData> serviceUpkeepDatas,
      bool inactiveBuilding,
      bool inactiveUpgrade)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      string prefabName = this.m_PrefabSystem.GetPrefabName(prefabEntity);
      for (int index = 0; index < serviceUpkeepDatas.Length; ++index)
      {
        ServiceUpkeepData serviceUpkeepData = serviceUpkeepDatas[index];
        Resource resource = serviceUpkeepData.m_Upkeep.m_Resource;
        int amount = serviceUpkeepData.m_Upkeep.m_Amount;
        string titleId;
        string descriptionId;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabUISystem.GetTitleAndDescription(prefabEntity, out titleId, out descriptionId);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BudgetDataQuery.IsEmptyIgnoreFilter && resource == Resource.Money)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          int price = CityServiceUpkeepSystem.CalculateUpkeep(amount, this.selectedPrefab, this.m_BudgetDataQuery.GetSingletonEntity(), this.EntityManager);
          if (inactiveBuilding | inactiveUpgrade)
            price = (int) ((double) price * 0.10000000149011612);
          // ISSUE: variable of a compiler-generated type
          UpkeepSection.UIUpkeepItem uiUpkeepItem;
          if (!this.moneyUpkeep.TryGetValue(prefabName, out uiUpkeepItem))
            this.moneyUpkeep[prefabName] = uiUpkeepItem;
          Dictionary<string, UpkeepSection.UIUpkeepItem> moneyUpkeep = this.moneyUpkeep;
          descriptionId = prefabName;
          // ISSUE: object of a compiler-generated type is created
          moneyUpkeep[descriptionId] += new UpkeepSection.UIUpkeepItem(amount, price, Resource.Money, titleId);
        }
        else if (!inactiveUpgrade)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          int price = Mathf.RoundToInt((float) amount * EconomyUtils.GetMarketPrice(resource, this.m_ResourceSystem.GetPrefabs(), this.EntityManager));
          ServiceUsage component;
          if (serviceUpkeepData.m_ScaleWithUsage && this.EntityManager.TryGetComponent<ServiceUsage>(buildingOwnerEntity, out component))
          {
            amount = (int) ((double) amount * (double) component.m_Usage);
            price = (int) ((double) price * (double) component.m_Usage);
          }
          if (amount != 0 && price != 0)
          {
            // ISSUE: variable of a compiler-generated type
            UpkeepSection.UIUpkeepItem uiUpkeepItem;
            if (!this.resourceUpkeep.TryGetValue(resource, out uiUpkeepItem))
              this.resourceUpkeep[resource] = uiUpkeepItem;
            // ISSUE: object of a compiler-generated type is created
            this.resourceUpkeep[resource] += new UpkeepSection.UIUpkeepItem(amount, price, resource, string.Empty);
            if (!this.tooltipKeys.Contains(resource.ToString()))
              this.tooltipKeys.Add(resource.ToString());
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_EconomyParameterQuery.IsEmptyIgnoreFilter || !(entity == buildingOwnerEntity) || !this.EntityManager.HasComponent<WorkplaceData>(this.selectedPrefab))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int upkeepOfEmployeeWage = CityServiceUpkeepSystem.GetUpkeepOfEmployeeWage(this.__TypeHandle.__Game_Companies_Employee_RW_BufferLookup, buildingOwnerEntity, this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(), inactiveBuilding);
      // ISSUE: variable of a compiler-generated type
      UpkeepSection.UIUpkeepItem uiUpkeepItem1;
      if (!this.moneyUpkeep.TryGetValue(Resource.Money.ToString(), out uiUpkeepItem1))
        this.moneyUpkeep[Resource.Money.ToString()] = uiUpkeepItem1;
      // ISSUE: object of a compiler-generated type is created
      this.moneyUpkeep[Resource.Money.ToString()] += new UpkeepSection.UIUpkeepItem(upkeepOfEmployeeWage, upkeepOfEmployeeWage, Resource.Money, string.Empty);
    }

    protected override void OnProcess()
    {
      Building component1;
      Extension component2;
      this.inactive = this.EntityManager.TryGetComponent<Building>(this.selectedEntity, out component1) && BuildingUtils.CheckOption(component1, BuildingOption.Inactive) || this.EntityManager.TryGetComponent<Extension>(this.selectedEntity, out component2) && (component2.m_Flags & ExtensionFlags.Disabled) != 0;
      DynamicBuffer<ServiceUpkeepData> buffer1;
      if (this.EntityManager.TryGetBuffer<ServiceUpkeepData>(this.selectedPrefab, true, out buffer1))
      {
        Entity entity = this.selectedEntity;
        bool inactiveBuilding = this.inactive;
        bool inactiveUpgrade = false;
        Owner component3;
        if (this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(this.selectedEntity) && this.EntityManager.TryGetComponent<Owner>(this.selectedEntity, out component3) && this.EntityManager.TryGetComponent<Building>(component3.m_Owner, out component1))
        {
          entity = component3.m_Owner;
          inactiveUpgrade = this.inactive;
          inactiveBuilding = BuildingUtils.CheckOption(component1, BuildingOption.Inactive);
        }
        // ISSUE: reference to a compiler-generated method
        this.CalculateServiceUpkeepDatas(entity, this.selectedPrefab, entity, buffer1, inactiveBuilding, inactiveUpgrade);
        DynamicBuffer<InstalledUpgrade> buffer2;
        if (this.EntityManager.TryGetBuffer<InstalledUpgrade>(this.selectedEntity, true, out buffer2))
        {
          for (int index = 0; index < buffer2.Length; ++index)
          {
            InstalledUpgrade installedUpgrade = buffer2[index];
            PrefabRef component4;
            DynamicBuffer<ServiceUpkeepData> buffer3;
            if (this.EntityManager.TryGetComponent<PrefabRef>(installedUpgrade.m_Upgrade, out component4) && this.EntityManager.TryGetBuffer<ServiceUpkeepData>(component4.m_Prefab, true, out buffer3))
            {
              // ISSUE: reference to a compiler-generated method
              this.CalculateServiceUpkeepDatas(installedUpgrade.m_Upgrade, component4.m_Prefab, entity, buffer3, inactiveBuilding, BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive));
            }
          }
        }
      }
      foreach (KeyValuePair<string, UpkeepSection.UIUpkeepItem> keyValuePair in this.moneyUpkeep)
      {
        this.upkeeps.Add(keyValuePair.Value);
        int total = this.total;
        // ISSUE: variable of a compiler-generated type
        UpkeepSection.UIUpkeepItem uiUpkeepItem = keyValuePair.Value;
        int price = uiUpkeepItem.price;
        this.total = total + price;
      }
      foreach (KeyValuePair<Resource, UpkeepSection.UIUpkeepItem> keyValuePair in this.resourceUpkeep)
      {
        this.upkeeps.Add(keyValuePair.Value);
        int total = this.total;
        // ISSUE: variable of a compiler-generated type
        UpkeepSection.UIUpkeepItem uiUpkeepItem = keyValuePair.Value;
        int price = uiUpkeepItem.price;
        this.total = total + price;
      }
      this.upkeeps.Sort();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("upkeeps");
      writer.ArrayBegin(this.upkeeps.Count);
      for (int index = 0; index < this.upkeeps.Count; ++index)
        writer.Write<UpkeepSection.UIUpkeepItem>(this.upkeeps[index]);
      writer.ArrayEnd();
      writer.PropertyName("total");
      writer.Write(this.total);
      writer.PropertyName("inactive");
      writer.Write(this.inactive);
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

    [Preserve]
    public UpkeepSection()
    {
    }

    private readonly struct UIUpkeepItem : IJsonWritable, IComparable<UpkeepSection.UIUpkeepItem>
    {
      public int count { get; }

      public int amount { get; }

      public int price { get; }

      public Resource localeKey { get; }

      public string titleId { get; }

      public UIUpkeepItem(int amount, int price, Resource localeKey, string titleId)
      {
        this.count = 1;
        this.amount = amount;
        this.price = price;
        this.localeKey = localeKey;
        this.titleId = titleId;
      }

      private UIUpkeepItem(int count, int amount, int price, Resource localeKey, string titleId)
      {
        this.count = count;
        this.amount = amount;
        this.price = price;
        this.localeKey = localeKey;
        this.titleId = titleId;
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(typeof (UpkeepSection.UIUpkeepItem).FullName);
        writer.PropertyName("count");
        writer.Write(this.count);
        writer.PropertyName("amount");
        writer.Write(this.amount);
        writer.PropertyName("price");
        writer.Write(this.price);
        writer.PropertyName("localeKey");
        writer.Write(Enum.GetName(typeof (Resource), (object) this.localeKey));
        writer.PropertyName("titleId");
        writer.Write(this.titleId);
        writer.PropertyName("localeKey");
        writer.Write(Enum.GetName(typeof (Resource), (object) this.localeKey));
        writer.TypeEnd();
      }

      public int CompareTo(UpkeepSection.UIUpkeepItem other) => this.amount.CompareTo(other.amount);

      public static UpkeepSection.UIUpkeepItem operator +(
        UpkeepSection.UIUpkeepItem a,
        UpkeepSection.UIUpkeepItem b)
      {
        // ISSUE: object of a compiler-generated type is created
        return new UpkeepSection.UIUpkeepItem(a.count + 1, a.amount + b.amount, a.price + b.price, b.localeKey, b.titleId);
      }
    }

    private struct TypeHandle
    {
      public BufferLookup<Employee> __Game_Companies_Employee_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RW_BufferLookup = state.GetBufferLookup<Employee>();
      }
    }
  }
}
