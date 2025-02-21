// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TaxationUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.City;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using Game.Zones;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class TaxationUISystem : UISystemBase
  {
    private static readonly string kGroup = "taxation";
    private ITaxSystem m_TaxSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_ResourceQuery;
    private EntityQuery m_UnlockedZoneQuery;
    private GetterValueBinding<int> m_TaxRate;
    private GetterValueBinding<int> m_TaxIncome;
    private GetterValueBinding<int> m_TaxEffect;
    private GetterValueBinding<int> m_MinTaxRate;
    private GetterValueBinding<int> m_MaxTaxRate;
    private RawValueBinding m_AreaTypes;
    private GetterMapBinding<int, int> m_AreaTaxRates;
    private GetterMapBinding<int, Bounds1> m_AreaResourceTaxRanges;
    private GetterMapBinding<int, int> m_AreaTaxIncomes;
    private GetterMapBinding<int, int> m_AreaTaxEffects;
    private GetterMapBinding<TaxResource, int> m_ResourceTaxRates;
    private GetterMapBinding<TaxResource, int> m_ResourceTaxIncomes;
    private TaxParameterData m_CachedTaxParameterData;
    private int m_CachedLockedOrderVersion = -1;
    private Dictionary<int, string> m_ResourceIcons = new Dictionary<int, string>();
    private TaxationUISystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceQuery = this.GetEntityQuery(ComponentType.ReadOnly<ResourceData>(), ComponentType.ReadOnly<TaxableResourceData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedZoneQuery = this.GetEntityQuery(ComponentType.ReadOnly<ZoneData>(), ComponentType.Exclude<Game.Prefabs.Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = (ITaxSystem) this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TaxRate = new GetterValueBinding<int>(TaxationUISystem.kGroup, "taxRate", (Func<int>) (() => this.m_TaxSystem.TaxRate))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TaxIncome = new GetterValueBinding<int>(TaxationUISystem.kGroup, "taxIncome", (Func<int>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> lookup = this.m_CityStatisticsSystem.GetLookup();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<CityStatistic> statisticRoBufferLookup = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Residential, TaxResultType.Any, lookup, statisticRoBufferLookup) + this.m_TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Commercial, TaxResultType.Any, lookup, statisticRoBufferLookup) + this.m_TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Industrial, TaxResultType.Any, lookup, statisticRoBufferLookup) + this.m_TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Office, TaxResultType.Any, lookup, statisticRoBufferLookup);
      }))));
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
      this.AddBinding((IBinding) (this.m_TaxEffect = new GetterValueBinding<int>(TaxationUISystem.kGroup, "taxEffect", (Func<int>) (() => this.m_TaxSystem.GetTaxRateEffect(TaxAreaType.Residential, this.m_TaxSystem.GetTaxRate(TaxAreaType.Residential)) + this.m_TaxSystem.GetTaxRateEffect(TaxAreaType.Commercial, this.m_TaxSystem.GetTaxRate(TaxAreaType.Commercial)) + this.m_TaxSystem.GetTaxRateEffect(TaxAreaType.Industrial, this.m_TaxSystem.GetTaxRate(TaxAreaType.Industrial)) + this.m_TaxSystem.GetTaxRateEffect(TaxAreaType.Office, this.m_TaxSystem.GetTaxRate(TaxAreaType.Office))))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MinTaxRate = new GetterValueBinding<int>(TaxationUISystem.kGroup, "minTaxRate", (Func<int>) (() => this.m_TaxSystem.GetTaxParameterData().m_TotalTaxLimits.x))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MaxTaxRate = new GetterValueBinding<int>(TaxationUISystem.kGroup, "maxTaxRate", (Func<int>) (() => this.m_TaxSystem.GetTaxParameterData().m_TotalTaxLimits.y))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AreaTypes = new RawValueBinding(TaxationUISystem.kGroup, "areaTypes", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        TaxParameterData taxParameterData = this.m_TaxSystem.GetTaxParameterData();
        // ISSUE: reference to a compiler-generated field
        this.m_CachedTaxParameterData = taxParameterData;
        binder.ArrayBegin(4U);
        for (TaxAreaType taxAreaType = TaxAreaType.Residential; taxAreaType <= TaxAreaType.Office; ++taxAreaType)
        {
          binder.TypeBegin("taxation.TaxAreaType");
          binder.PropertyName("index");
          binder.Write((int) taxAreaType);
          binder.PropertyName("id");
          binder.Write(taxAreaType.ToString());
          binder.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          binder.Write(this.GetIcon(taxAreaType));
          // ISSUE: reference to a compiler-generated method
          int2 limits = this.GetLimits(taxAreaType, taxParameterData);
          binder.PropertyName("taxRateMin");
          binder.Write(limits.x);
          binder.PropertyName("taxRateMax");
          binder.Write(limits.y);
          // ISSUE: reference to a compiler-generated method
          int2 resourceLimits = this.GetResourceLimits(taxAreaType, taxParameterData);
          binder.PropertyName("resourceTaxRateMin");
          binder.Write(resourceLimits.x);
          binder.PropertyName("resourceTaxRateMax");
          binder.Write(resourceLimits.y);
          binder.PropertyName("locked");
          // ISSUE: reference to a compiler-generated method
          binder.Write(this.Locked(taxAreaType));
          binder.TypeEnd();
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AreaTaxRates = new GetterMapBinding<int, int>(TaxationUISystem.kGroup, "areaTaxRates", (Func<int, int>) (areaType => this.m_TaxSystem.GetTaxRate((TaxAreaType) areaType)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AreaResourceTaxRanges = new GetterMapBinding<int, Bounds1>(TaxationUISystem.kGroup, "areaResourceTaxRanges", (Func<int, Bounds1>) (area => new Bounds1((float2) this.m_TaxSystem.GetTaxRateRange((TaxAreaType) area))))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AreaTaxIncomes = new GetterMapBinding<int, int>(TaxationUISystem.kGroup, "areaTaxIncomes", (Func<int, int>) (areaType =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> lookup = this.m_CityStatisticsSystem.GetLookup();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<CityStatistic> statisticRoBufferLookup = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        return this.m_TaxSystem.GetEstimatedTaxAmount((TaxAreaType) areaType, TaxResultType.Any, lookup, statisticRoBufferLookup);
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AreaTaxEffects = new GetterMapBinding<int, int>(TaxationUISystem.kGroup, "areaTaxEffects", (Func<int, int>) (areaType => this.m_TaxSystem.GetTaxRateEffect((TaxAreaType) areaType, this.m_TaxSystem.GetTaxRate((TaxAreaType) areaType))))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new RawMapBinding<int>(TaxationUISystem.kGroup, "areaResources", (Action<IJsonWriter, int>) ((binder, area) =>
      {
        if ((byte) area == (byte) 1)
        {
          binder.ArrayBegin(5U);
          for (int index = 0; index < 5; ++index)
            binder.Write<TaxResource>(new TaxResource()
            {
              m_Resource = index,
              m_AreaType = 1
            });
          binder.ArrayEnd();
        }
        else
        {
          int size = 0;
          // ISSUE: reference to a compiler-generated method
          foreach (ResourcePrefab resource in this.GetResources(area))
            ++size;
          binder.ArrayBegin(size);
          // ISSUE: reference to a compiler-generated method
          foreach (ResourcePrefab resource in this.GetResources(area))
            binder.Write<TaxResource>(new TaxResource()
            {
              m_Resource = (int) (resource.m_Resource - 1),
              m_AreaType = area
            });
          binder.ArrayEnd();
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ResourceTaxRates = new GetterMapBinding<TaxResource, int>(TaxationUISystem.kGroup, "resourceTaxRates", (Func<TaxResource, int>) (taxResource => this.GetResourceTaxRate((TaxAreaType) taxResource.m_AreaType, taxResource.m_Resource)), (IReader<TaxResource>) new ValueReader<TaxResource>(), (IWriter<TaxResource>) new ValueWriter<TaxResource>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ResourceTaxIncomes = new GetterMapBinding<TaxResource, int>(TaxationUISystem.kGroup, "resourceTaxIncomes", (Func<TaxResource, int>) (taxResource => this.GetEstimatedResourceTaxIncome((TaxAreaType) taxResource.m_AreaType, taxResource.m_Resource)), (IReader<TaxResource>) new ValueReader<TaxResource>(), (IWriter<TaxResource>) new ValueWriter<TaxResource>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new GetterMapBinding<TaxResource, TaxResourceInfo>(TaxationUISystem.kGroup, "taxResourceInfos", (Func<TaxResource, TaxResourceInfo>) (resource =>
      {
        if (resource.m_AreaType == 1)
          return new TaxResourceInfo()
          {
            m_ID = string.Empty,
            m_Icon = "Media/Game/Icons/ZoneResidential.svg"
          };
        // ISSUE: reference to a compiler-generated field
        return new TaxResourceInfo()
        {
          m_ID = EconomyUtils.GetResource(resource.m_Resource).ToString(),
          m_Icon = this.m_ResourceIcons[resource.m_Resource]
        };
      }), (IReader<TaxResource>) new ValueReader<TaxResource>(), (IWriter<TaxResource>) new ValueWriter<TaxResource>(), (IWriter<TaxResourceInfo>) new ValueWriter<TaxResourceInfo>()));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<int>(TaxationUISystem.kGroup, "setTaxRate", (Action<int>) (rate =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.Readers.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.TaxRate = rate;
        // ISSUE: reference to a compiler-generated field
        this.m_TaxRate.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTaxRates.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceTaxRates.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_AreaResourceTaxRanges.UpdateAll();
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<int, int>(TaxationUISystem.kGroup, "setAreaTaxRate", (Action<int, int>) ((areaType, rate) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.Readers.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.SetTaxRate((TaxAreaType) areaType, rate);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTaxRates.Update(areaType);
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceTaxRates.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_AreaResourceTaxRanges.UpdateAll();
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<int, int, int>(TaxationUISystem.kGroup, "setResourceTaxRate", (Action<int, int, int>) ((resource, areaType, rate) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.Readers.Complete();
        if ((byte) areaType == (byte) 1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TaxSystem.SetResidentialTaxRate(resource, rate);
        }
        if ((byte) areaType == (byte) 2)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TaxSystem.SetCommercialTaxRate(EconomyUtils.GetResource(resource), rate);
        }
        else if ((byte) areaType == (byte) 3)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TaxSystem.SetIndustrialTaxRate(EconomyUtils.GetResource(resource), rate);
        }
        else if ((byte) areaType == (byte) 4)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TaxSystem.SetOfficeTaxRate(EconomyUtils.GetResource(resource), rate);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTaxRates.Update(areaType);
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceTaxRates.Update(new TaxResource()
        {
          m_AreaType = areaType,
          m_Resource = resource
        });
        // ISSUE: reference to a compiler-generated field
        this.m_AreaResourceTaxRanges.UpdateAll();
      })));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTaxIncomes.UpdateAll();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTaxEffects.UpdateAll();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxIncome.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxEffect.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceTaxIncomes.UpdateAll();
      // ISSUE: reference to a compiler-generated field
      TaxParameterData taxParameterData = this.m_TaxSystem.GetTaxParameterData();
      int componentOrderVersion = this.EntityManager.GetComponentOrderVersion<Game.Prefabs.Locked>();
      // ISSUE: reference to a compiler-generated field
      bool flag = componentOrderVersion != this.m_CachedLockedOrderVersion;
      // ISSUE: reference to a compiler-generated field
      this.m_CachedLockedOrderVersion = componentOrderVersion;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CachedTaxParameterData.Equals(taxParameterData))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTypes.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_MinTaxRate.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_MaxTaxRate.Update();
      }
      else
      {
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTypes.Update();
      }
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceIcons.Clear();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ResourceQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefab prefab = this.m_PrefabSystem.GetPrefab<ResourcePrefab>(entityArray[index]);
        UIObject component = prefab.GetComponent<UIObject>();
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceIcons[(int) (prefab.m_Resource - 1)] = (bool) (UnityEngine.Object) component ? component.m_Icon : string.Empty;
      }
      entityArray.Dispose();
    }

    private int2 GetLimits(TaxAreaType type, TaxParameterData limits)
    {
      switch (type)
      {
        case TaxAreaType.Residential:
          return limits.m_ResidentialTaxLimits;
        case TaxAreaType.Commercial:
          return limits.m_CommercialTaxLimits;
        case TaxAreaType.Industrial:
          return limits.m_IndustrialTaxLimits;
        case TaxAreaType.Office:
          return limits.m_OfficeTaxLimits;
        default:
          return new int2();
      }
    }

    private int2 GetResourceLimits(TaxAreaType type, TaxParameterData limits)
    {
      return type == TaxAreaType.Residential ? limits.m_JobLevelTaxLimits : limits.m_ResourceTaxLimits;
    }

    private int GetResourceTaxRate(TaxAreaType type, int resource)
    {
      switch (type)
      {
        case TaxAreaType.Residential:
          // ISSUE: reference to a compiler-generated field
          return this.m_TaxSystem.GetResidentialTaxRate(resource);
        case TaxAreaType.Commercial:
          // ISSUE: reference to a compiler-generated field
          return this.m_TaxSystem.GetCommercialTaxRate(EconomyUtils.GetResource(resource));
        case TaxAreaType.Industrial:
          // ISSUE: reference to a compiler-generated field
          return this.m_TaxSystem.GetIndustrialTaxRate(EconomyUtils.GetResource(resource));
        case TaxAreaType.Office:
          // ISSUE: reference to a compiler-generated field
          return this.m_TaxSystem.GetOfficeTaxRate(EconomyUtils.GetResource(resource));
        default:
          return 0;
      }
    }

    private int GetEstimatedResourceTaxIncome(TaxAreaType type, int resource)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> lookup = this.m_CityStatisticsSystem.GetLookup();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<CityStatistic> statisticRoBufferLookup = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup;
      switch (type)
      {
        case TaxAreaType.Residential:
          // ISSUE: reference to a compiler-generated field
          return this.m_TaxSystem.GetEstimatedResidentialTaxIncome(resource, lookup, statisticRoBufferLookup);
        case TaxAreaType.Commercial:
          // ISSUE: reference to a compiler-generated field
          return this.m_TaxSystem.GetEstimatedCommercialTaxIncome(EconomyUtils.GetResource(resource), lookup, statisticRoBufferLookup);
        case TaxAreaType.Industrial:
          // ISSUE: reference to a compiler-generated field
          return this.m_TaxSystem.GetEstimatedIndustrialTaxIncome(EconomyUtils.GetResource(resource), lookup, statisticRoBufferLookup);
        case TaxAreaType.Office:
          // ISSUE: reference to a compiler-generated field
          return this.m_TaxSystem.GetEstimatedOfficeTaxIncome(EconomyUtils.GetResource(resource), lookup, statisticRoBufferLookup);
        default:
          return 0;
      }
    }

    private int UpdateTaxRate() => this.m_TaxSystem.TaxRate;

    private int UpdateTaxIncome()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> lookup = this.m_CityStatisticsSystem.GetLookup();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<CityStatistic> statisticRoBufferLookup = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Residential, TaxResultType.Any, lookup, statisticRoBufferLookup) + this.m_TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Commercial, TaxResultType.Any, lookup, statisticRoBufferLookup) + this.m_TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Industrial, TaxResultType.Any, lookup, statisticRoBufferLookup) + this.m_TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Office, TaxResultType.Any, lookup, statisticRoBufferLookup);
    }

    private int UpdateTaxEffect()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_TaxSystem.GetTaxRateEffect(TaxAreaType.Residential, this.m_TaxSystem.GetTaxRate(TaxAreaType.Residential)) + this.m_TaxSystem.GetTaxRateEffect(TaxAreaType.Commercial, this.m_TaxSystem.GetTaxRate(TaxAreaType.Commercial)) + this.m_TaxSystem.GetTaxRateEffect(TaxAreaType.Industrial, this.m_TaxSystem.GetTaxRate(TaxAreaType.Industrial)) + this.m_TaxSystem.GetTaxRateEffect(TaxAreaType.Office, this.m_TaxSystem.GetTaxRate(TaxAreaType.Office));
    }

    private int UpdateMinTaxRate() => this.m_TaxSystem.GetTaxParameterData().m_TotalTaxLimits.x;

    private int UpdateMaxTaxRate() => this.m_TaxSystem.GetTaxParameterData().m_TotalTaxLimits.y;

    private void SetTaxRate(int rate)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem.Readers.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem.TaxRate = rate;
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRate.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTaxRates.UpdateAll();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceTaxRates.UpdateAll();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaResourceTaxRanges.UpdateAll();
    }

    private void SetAreaTaxRate(int areaType, int rate)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem.Readers.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem.SetTaxRate((TaxAreaType) areaType, rate);
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTaxRates.Update(areaType);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceTaxRates.UpdateAll();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaResourceTaxRanges.UpdateAll();
    }

    private void SetResourceTaxRate(int resource, int areaType, int rate)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem.Readers.Complete();
      if ((byte) areaType == (byte) 1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.SetResidentialTaxRate(resource, rate);
      }
      if ((byte) areaType == (byte) 2)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.SetCommercialTaxRate(EconomyUtils.GetResource(resource), rate);
      }
      else if ((byte) areaType == (byte) 3)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.SetIndustrialTaxRate(EconomyUtils.GetResource(resource), rate);
      }
      else if ((byte) areaType == (byte) 4)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxSystem.SetOfficeTaxRate(EconomyUtils.GetResource(resource), rate);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTaxRates.Update(areaType);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceTaxRates.Update(new TaxResource()
      {
        m_AreaType = areaType,
        m_Resource = resource
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AreaResourceTaxRanges.UpdateAll();
    }

    private int UpdateAreaTaxRate(int areaType)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_TaxSystem.GetTaxRate((TaxAreaType) areaType);
    }

    private Bounds1 UpdateAreaResourceTaxRange(int area)
    {
      // ISSUE: reference to a compiler-generated field
      return new Bounds1((float2) this.m_TaxSystem.GetTaxRateRange((TaxAreaType) area));
    }

    private int UpdateAreaTaxIncome(int areaType)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> lookup = this.m_CityStatisticsSystem.GetLookup();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<CityStatistic> statisticRoBufferLookup = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      return this.m_TaxSystem.GetEstimatedTaxAmount((TaxAreaType) areaType, TaxResultType.Any, lookup, statisticRoBufferLookup);
    }

    private int UpdateAreaTaxEffect(int areaType)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_TaxSystem.GetTaxRateEffect((TaxAreaType) areaType, this.m_TaxSystem.GetTaxRate((TaxAreaType) areaType));
    }

    private int UpdateResourceTaxRate(TaxResource taxResource)
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetResourceTaxRate((TaxAreaType) taxResource.m_AreaType, taxResource.m_Resource);
    }

    private int UpdateResourceTaxIncome(TaxResource taxResource)
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetEstimatedResourceTaxIncome((TaxAreaType) taxResource.m_AreaType, taxResource.m_Resource);
    }

    private void UpdateAreaTypes(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      TaxParameterData taxParameterData = this.m_TaxSystem.GetTaxParameterData();
      // ISSUE: reference to a compiler-generated field
      this.m_CachedTaxParameterData = taxParameterData;
      binder.ArrayBegin(4U);
      for (TaxAreaType taxAreaType = TaxAreaType.Residential; taxAreaType <= TaxAreaType.Office; ++taxAreaType)
      {
        binder.TypeBegin("taxation.TaxAreaType");
        binder.PropertyName("index");
        binder.Write((int) taxAreaType);
        binder.PropertyName("id");
        binder.Write(taxAreaType.ToString());
        binder.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        binder.Write(this.GetIcon(taxAreaType));
        // ISSUE: reference to a compiler-generated method
        int2 limits = this.GetLimits(taxAreaType, taxParameterData);
        binder.PropertyName("taxRateMin");
        binder.Write(limits.x);
        binder.PropertyName("taxRateMax");
        binder.Write(limits.y);
        // ISSUE: reference to a compiler-generated method
        int2 resourceLimits = this.GetResourceLimits(taxAreaType, taxParameterData);
        binder.PropertyName("resourceTaxRateMin");
        binder.Write(resourceLimits.x);
        binder.PropertyName("resourceTaxRateMax");
        binder.Write(resourceLimits.y);
        binder.PropertyName("locked");
        // ISSUE: reference to a compiler-generated method
        binder.Write(this.Locked(taxAreaType));
        binder.TypeEnd();
      }
      binder.ArrayEnd();
    }

    private bool Locked(TaxAreaType areaType)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ZoneData> componentDataArray = this.m_UnlockedZoneQuery.ToComponentDataArray<ZoneData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        if (areaType == TaxAreaType.Residential && componentDataArray[index].m_AreaType == AreaType.Residential || areaType == TaxAreaType.Commercial && componentDataArray[index].m_AreaType == AreaType.Commercial || areaType == TaxAreaType.Industrial && componentDataArray[index].m_AreaType == AreaType.Industrial || areaType == TaxAreaType.Office && (componentDataArray[index].m_ZoneFlags & ZoneFlags.Office) != (ZoneFlags) 0)
        {
          componentDataArray.Dispose();
          return false;
        }
      }
      componentDataArray.Dispose();
      return true;
    }

    private void UpdateAreaResources(IJsonWriter binder, int area)
    {
      if ((byte) area == (byte) 1)
      {
        binder.ArrayBegin(5U);
        for (int index = 0; index < 5; ++index)
          binder.Write<TaxResource>(new TaxResource()
          {
            m_Resource = index,
            m_AreaType = 1
          });
        binder.ArrayEnd();
      }
      else
      {
        int size = 0;
        // ISSUE: reference to a compiler-generated method
        foreach (ResourcePrefab resource in this.GetResources(area))
          ++size;
        binder.ArrayBegin(size);
        // ISSUE: reference to a compiler-generated method
        foreach (ResourcePrefab resource in this.GetResources(area))
          binder.Write<TaxResource>(new TaxResource()
          {
            m_Resource = (int) (resource.m_Resource - 1),
            m_AreaType = area
          });
        binder.ArrayEnd();
      }
    }

    private TaxResourceInfo UpdateResourceInfo(TaxResource resource)
    {
      if (resource.m_AreaType == 1)
        return new TaxResourceInfo()
        {
          m_ID = string.Empty,
          m_Icon = "Media/Game/Icons/ZoneResidential.svg"
        };
      // ISSUE: reference to a compiler-generated field
      return new TaxResourceInfo()
      {
        m_ID = EconomyUtils.GetResource(resource.m_Resource).ToString(),
        m_Icon = this.m_ResourceIcons[resource.m_Resource]
      };
    }

    private IEnumerable<ResourcePrefab> GetResources(int areaType)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entities = this.m_ResourceQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int i = 0; i < entities.Length; ++i)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefab prefab = this.m_PrefabSystem.GetPrefab<ResourcePrefab>(entities[i]);
        // ISSUE: reference to a compiler-generated method
        if (this.MatchArea(prefab.GetComponent<TaxableResource>(), areaType))
          yield return prefab;
      }
      entities.Dispose();
    }

    private bool MatchArea(TaxableResource data, int areaType)
    {
      if (data.m_TaxAreas == null || data.m_TaxAreas.Length == 0)
        return true;
      for (int index = 0; index < data.m_TaxAreas.Length; ++index)
      {
        if (data.m_TaxAreas[index] == (TaxAreaType) areaType)
          return true;
      }
      return false;
    }

    private string GetIcon(TaxAreaType type) => "Media/Game/Icons/Zone" + type.ToString() + ".svg";

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
    public TaxationUISystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<CityStatistic> __Game_City_CityStatistic_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityStatistic_RO_BufferLookup = state.GetBufferLookup<CityStatistic>(true);
      }
    }
  }
}
