// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.StatisticsUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class StatisticsUISystem : UISystemBase
  {
    private const string kGroup = "statistics";
    private PrefabUISystem m_PrefabUISystem;
    private PrefabSystem m_PrefabSystem;
    private ResourceSystem m_ResourceSystem;
    private ICityStatisticsSystem m_CityStatisticsSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private TimeUISystem m_TimeUISystem;
    private EntityQuery m_StatisticsCategoryQuery;
    private EntityQuery m_TimeDataQuery;
    private EntityQuery m_UnlockedPrefabQuery;
    private List<StatisticsUISystem.StatItem> m_GroupCache;
    private List<StatisticsUISystem.StatItem> m_SubGroupCache;
    private List<StatisticsUISystem.StatItem> m_SelectedStatistics;
    private List<StatisticsUISystem.StatItem> m_SelectedStatisticsTracker;
    private Entity m_ActiveCategory;
    private Entity m_ActiveGroup;
    private int m_SampleRange;
    private bool m_Stacked;
    private RawMapBinding<Entity> m_GroupsMapBinding;
    private ValueBinding<int> m_SampleRangeBinding;
    private ValueBinding<int> m_SampleCountBinding;
    private GetterValueBinding<Entity> m_ActiveGroupBinding;
    private GetterValueBinding<Entity> m_ActiveCategoryBinding;
    private GetterValueBinding<bool> m_StackedBinding;
    private RawValueBinding m_SelectedStatisticsBinding;
    private RawValueBinding m_CategoriesBinding;
    private RawValueBinding m_DataBinding;
    private RawMapBinding<Entity> m_UnlockingRequirementsBinding;
    private bool m_ClearActive = true;
    private int m_UnlockRequirementVersion;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsCategoryQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIObjectData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<UIStatisticsCategoryData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_GroupCache = new List<StatisticsUISystem.StatItem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SubGroupCache = new List<StatisticsUISystem.StatItem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatistics = new List<StatisticsUISystem.StatItem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatisticsTracker = new List<StatisticsUISystem.StatItem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = (ICityStatisticsSystem) this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem.eventStatisticsUpdated += (System.Action) (() => this.m_DataBinding.Update());
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeUISystem = this.World.GetOrCreateSystemManaged<TimeUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_GroupsMapBinding = new RawMapBinding<Entity>("statistics", "groups", (Action<IJsonWriter, Entity>) ((binder, parent) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CacheChildren(parent, this.m_GroupCache);
        // ISSUE: reference to a compiler-generated field
        binder.ArrayBegin(this.m_GroupCache.Count);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_GroupCache.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          binder.Write<StatisticsUISystem.StatItem>(this.m_GroupCache[index]);
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SampleRangeBinding = new ValueBinding<int>("statistics", "sampleRange", this.m_SampleRange)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SampleCountBinding = new ValueBinding<int>("statistics", "sampleCount", this.m_CityStatisticsSystem.sampleCount)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ActiveGroupBinding = new GetterValueBinding<Entity>("statistics", "activeGroup", (Func<Entity>) (() => this.m_ActiveGroup))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ActiveCategoryBinding = new GetterValueBinding<Entity>("statistics", "activeCategory", (Func<Entity>) (() => this.m_ActiveCategory))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_StackedBinding = new GetterValueBinding<bool>("statistics", "stacked", (Func<bool>) (() => this.m_Stacked))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CategoriesBinding = new RawValueBinding("statistics", "categories", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated method
        NativeList<StatisticsUISystem.StatCategory> sortedCategories = this.GetSortedCategories();
        binder.ArrayBegin(sortedCategories.Length);
        for (int index = 0; index < sortedCategories.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          StatisticsUISystem.StatCategory statCategory = sortedCategories[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(statCategory.m_PrefabData);
          // ISSUE: reference to a compiler-generated field
          bool flag = this.EntityManager.HasEnabledComponent<Locked>(statCategory.m_Entity);
          binder.TypeBegin("statistics.StatCategory");
          binder.PropertyName("entity");
          // ISSUE: reference to a compiler-generated field
          binder.Write(statCategory.m_Entity);
          binder.PropertyName("key");
          binder.Write(prefab.name);
          binder.PropertyName("locked");
          binder.Write(flag);
          binder.TypeEnd();
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_DataBinding = new RawValueBinding("statistics", "data", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        binder.ArrayBegin(this.m_SelectedStatistics.Count);
        // ISSUE: reference to a compiler-generated field
        for (int index = this.m_SelectedStatistics.Count - 1; index >= 0; --index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.BindData(binder, this.m_SelectedStatistics[index]);
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedStatisticsBinding = new RawValueBinding("statistics", "selectedStatistics", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        binder.ArrayBegin(this.m_SelectedStatistics.Count);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SelectedStatistics.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          StatisticsUISystem.StatItem selectedStatistic = this.m_SelectedStatistics[index];
          binder.Write<StatisticsUISystem.StatItem>(selectedStatistic);
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_UnlockingRequirementsBinding = new RawMapBinding<Entity>("statistics", "unlockingRequirements", (Action<IJsonWriter, Entity>) ((writer, prefabEntity) => this.m_PrefabUISystem.BindPrefabRequirements(writer, prefabEntity)))));
      this.AddBinding((IBinding) new GetterValueBinding<int>("statistics", "updatesPerDay", (Func<int>) (() => 32)));
      this.AddBinding((IBinding) new TriggerBinding<StatisticsUISystem.StatItem>("statistics", "addStat", (Action<StatisticsUISystem.StatItem>) (stat =>
      {
        // ISSUE: reference to a compiler-generated field
        if (stat.locked)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (stat.category != this.m_ActiveCategory)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedStatistics.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedStatisticsTracker.Clear();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ActiveCategory = stat.category;
          // ISSUE: reference to a compiler-generated field
          this.m_ActiveCategoryBinding.Update();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ActiveGroup == Entity.Null || stat.isGroup || stat.group != this.m_ActiveGroup)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedStatistics.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedStatisticsTracker.Clear();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ActiveGroup = stat.isGroup ? stat.entity : stat.group;
          // ISSUE: reference to a compiler-generated field
          this.m_ActiveGroupBinding.Update();
        }
        // ISSUE: reference to a compiler-generated field
        if (stat.isSubgroup)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num = this.m_SelectedStatisticsTracker.Count<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
          if (num == 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            StatisticsUISystem.StatItem stat1 = this.m_SelectedStatisticsTracker.First<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
            // ISSUE: reference to a compiler-generated field
            this.m_ClearActive = false;
            // ISSUE: reference to a compiler-generated method
            this.DeepRemoveStat(stat1);
            // ISSUE: reference to a compiler-generated method
            this.AddStat(stat1);
          }
          if (num == 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedStatisticsTracker.Add(stat);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.TryAddChildren(stat, this.m_SubGroupCache);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddStat(stat);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (stat.isGroup)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedStatisticsTracker.Add(stat);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (!this.TryAddChildren(stat, this.m_GroupCache))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SelectedStatistics.Add(stat);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddStat(stat);
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.UpdateStackedStatus();
        // ISSUE: reference to a compiler-generated method
        this.UpdateStats();
      }), (IReader<StatisticsUISystem.StatItem>) new ValueReader<StatisticsUISystem.StatItem>()));
      this.AddBinding((IBinding) new TriggerBinding<StatisticsUISystem.StatItem>("statistics", "removeStat", (Action<StatisticsUISystem.StatItem>) (stat =>
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SelectedStatisticsTracker.Contains(stat))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int index1 = this.m_SelectedStatisticsTracker.FindIndex((Predicate<StatisticsUISystem.StatItem>) (s => s.entity == stat.group));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int index2 = this.m_SelectedStatisticsTracker.FindIndex((Predicate<StatisticsUISystem.StatItem>) (s => s.entity == stat.entity && s.isSubgroup));
          if (index1 >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            StatisticsUISystem.StatItem stat2 = this.m_SelectedStatisticsTracker[index1];
            if (index2 >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              StatisticsUISystem.StatItem stat3 = this.m_SelectedStatisticsTracker[index2];
              // ISSUE: reference to a compiler-generated method
              this.DeepRemoveStat(stat2);
              // ISSUE: reference to a compiler-generated method
              this.ProcessAddStat(stat3);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.DeepRemoveStat(stat2);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num1 = this.m_SelectedStatisticsTracker.Count<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
        // ISSUE: reference to a compiler-generated method
        this.RemoveStat(stat);
        // ISSUE: reference to a compiler-generated field
        if (stat.isGroup)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = this.m_SelectedStatistics.Count - 1; index >= 0; --index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SelectedStatistics[index].group == stat.entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SelectedStatistics.RemoveAt(index);
            }
          }
          // ISSUE: reference to a compiler-generated field
          for (int index = this.m_SelectedStatisticsTracker.Count - 1; index >= 0; --index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SelectedStatisticsTracker[index].group == stat.entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SelectedStatisticsTracker.RemoveAt(index);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (stat.isSubgroup)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = this.m_SelectedStatistics.Count - 1; index >= 0; --index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SelectedStatistics[index].entity == stat.entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SelectedStatistics.RemoveAt(index);
            }
          }
          // ISSUE: reference to a compiler-generated field
          for (int index = this.m_SelectedStatisticsTracker.Count - 1; index >= 0; --index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SelectedStatisticsTracker[index].entity == stat.entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SelectedStatisticsTracker.RemoveAt(index);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num2 = this.m_SelectedStatisticsTracker.Count<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
        if (num1 > 1 && num2 == 1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          StatisticsUISystem.StatItem stat4 = this.m_SelectedStatisticsTracker.First<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
          // ISSUE: reference to a compiler-generated method
          this.RemoveStat(stat4);
          // ISSUE: reference to a compiler-generated method
          this.ProcessAddStat(stat4);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ClearActive && this.m_SelectedStatistics.Count == 0 && this.m_SelectedStatisticsTracker.Count <= 1)
        {
          // ISSUE: reference to a compiler-generated method
          this.ClearStats();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateStats();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ClearActive = true;
        // ISSUE: reference to a compiler-generated method
        this.UpdateStackedStatus();
      }), (IReader<StatisticsUISystem.StatItem>) new ValueReader<StatisticsUISystem.StatItem>()));
      this.AddBinding((IBinding) new TriggerBinding("statistics", "clearStats", (System.Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedStatistics.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedStatisticsTracker.Clear();
        // ISSUE: reference to a compiler-generated method
        this.UpdateStats();
        // ISSUE: reference to a compiler-generated method
        this.ClearActive();
      })));
      this.AddBinding((IBinding) new TriggerBinding<int>("statistics", "setSampleRange", (Action<int>) (range =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SampleRange = range;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SampleRangeBinding.Update(this.m_SampleRange);
        // ISSUE: reference to a compiler-generated method
        this.UpdateStats();
      })));
    }

    private void BindUnlockingRequirements(IJsonWriter writer, Entity prefabEntity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabUISystem.BindPrefabRequirements(writer, prefabEntity);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatistics.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_SampleRange = 32;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SampleCountBinding.Update(this.m_CityStatisticsSystem.sampleCount);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SampleRangeBinding.Update(this.m_SampleRange);
      int componentOrderVersion = this.EntityManager.GetComponentOrderVersion<UnlockRequirementData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (PrefabUtils.HasUnlockedPrefab<UIObjectData>(this.EntityManager, this.m_UnlockedPrefabQuery) || this.m_UnlockRequirementVersion != componentOrderVersion)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UnlockingRequirementsBinding.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_GroupsMapBinding.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_CategoriesBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockRequirementVersion = componentOrderVersion;
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem.eventStatisticsUpdated -= (System.Action) (() => this.m_DataBinding.Update());
      base.OnDestroy();
    }

    private void OnStatisticsUpdated() => this.m_DataBinding.Update();

    private void BindSelectedStatistics(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      binder.ArrayBegin(this.m_SelectedStatistics.Count);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_SelectedStatistics.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        StatisticsUISystem.StatItem selectedStatistic = this.m_SelectedStatistics[index];
        binder.Write<StatisticsUISystem.StatItem>(selectedStatistic);
      }
      binder.ArrayEnd();
    }

    private void BindCategories(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated method
      NativeList<StatisticsUISystem.StatCategory> sortedCategories = this.GetSortedCategories();
      binder.ArrayBegin(sortedCategories.Length);
      for (int index = 0; index < sortedCategories.Length; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        StatisticsUISystem.StatCategory statCategory = sortedCategories[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(statCategory.m_PrefabData);
        // ISSUE: reference to a compiler-generated field
        bool flag = this.EntityManager.HasEnabledComponent<Locked>(statCategory.m_Entity);
        binder.TypeBegin("statistics.StatCategory");
        binder.PropertyName("entity");
        // ISSUE: reference to a compiler-generated field
        binder.Write(statCategory.m_Entity);
        binder.PropertyName("key");
        binder.Write(prefab.name);
        binder.PropertyName("locked");
        binder.Write(flag);
        binder.TypeEnd();
      }
      binder.ArrayEnd();
    }

    private NativeList<StatisticsUISystem.StatCategory> GetSortedCategories()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_StatisticsCategoryQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<UIObjectData> componentDataArray1 = this.m_StatisticsCategoryQuery.ToComponentDataArray<UIObjectData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<PrefabData> componentDataArray2 = this.m_StatisticsCategoryQuery.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<StatisticsUISystem.StatCategory> list = new NativeList<StatisticsUISystem.StatCategory>(entityArray.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        ref NativeList<StatisticsUISystem.StatCategory> local1 = ref list;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        StatisticsUISystem.StatCategory statCategory = new StatisticsUISystem.StatCategory(entityArray[index], componentDataArray1[index], componentDataArray2[index]);
        ref StatisticsUISystem.StatCategory local2 = ref statCategory;
        local1.Add(in local2);
      }
      entityArray.Dispose();
      componentDataArray1.Dispose();
      componentDataArray2.Dispose();
      list.Sort<StatisticsUISystem.StatCategory>();
      return list;
    }

    private void CacheChildren(Entity parentEntity, List<StatisticsUISystem.StatItem> cache)
    {
      cache.Clear();
      bool isGroup = this.EntityManager.HasComponent<UIStatisticsCategoryData>(parentEntity);
      DynamicBuffer<UIGroupElement> buffer;
      if (this.EntityManager.TryGetBuffer<UIGroupElement>(parentEntity, true, out buffer))
      {
        NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, buffer, Allocator.TempJob);
        for (int index = 0; index < sortedObjects.Length; ++index)
        {
          Entity category = Entity.Null;
          Entity group = Entity.Null;
          Entity entity = sortedObjects[index].entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(entity);
          StatisticUnitType unitType = StatisticUnitType.None;
          StatisticType statisticType = StatisticType.Invalid;
          bool locked = this.EntityManager.HasEnabledComponent<Locked>(entity);
          bool isSubgroup = !isGroup && this.EntityManager.HasComponent<UIStatisticsGroupData>(entity) || prefab is ParametricStatistic parametricStatistic && parametricStatistic.GetParameters().Count<StatisticParameterData>() > 1;
          bool stacked = true;
          Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          StatisticsData component1;
          if (this.EntityManager.TryGetComponent<StatisticsData>(entity, out component1))
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_CityConfigurationSystem.unlimitedMoney || component1.m_StatisticType != StatisticType.Money)
            {
              unitType = component1.m_UnitType;
              statisticType = component1.m_StatisticType;
              group = component1.m_Group;
              category = component1.m_Category;
              color = component1.m_Color;
              stacked = component1.m_Stacked;
            }
            else
              continue;
          }
          UIStatisticsGroupData component2;
          UIObjectData component3;
          if (this.EntityManager.TryGetComponent<UIStatisticsGroupData>(entity, out component2) && this.EntityManager.TryGetComponent<UIObjectData>(entity, out component3))
          {
            group = component3.m_Group == component2.m_Category ? entity : component3.m_Group;
            unitType = component2.m_UnitType;
            category = component2.m_Category;
            color = component2.m_Color;
            stacked = component2.m_Stacked;
          }
          // ISSUE: object of a compiler-generated type is created
          cache.Add(new StatisticsUISystem.StatItem(index, category, group, entity, (int) statisticType, unitType, 0, prefab.name, color, locked, isGroup, isSubgroup, stacked));
        }
        sortedObjects.Dispose();
      }
      else
      {
        StatisticsData component4;
        PrefabData component5;
        if (!this.EntityManager.TryGetComponent<StatisticsData>(parentEntity, out component4) || !this.EntityManager.TryGetComponent<PrefabData>(parentEntity, out component5))
          return;
        bool locked = this.EntityManager.HasEnabledComponent<Locked>(parentEntity);
        // ISSUE: reference to a compiler-generated method
        this.CacheParameterChildren(parentEntity, locked, component4, component5, cache);
      }
    }

    private void CacheParameterChildren(
      Entity parent,
      bool locked,
      StatisticsData statisticsData,
      PrefabData prefabData,
      List<StatisticsUISystem.StatItem> cache)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ParametricStatistic prefab = this.m_PrefabSystem.GetPrefab<ParametricStatistic>(prefabData);
      DynamicBuffer<StatisticParameterData> buffer;
      if (!this.EntityManager.TryGetBuffer<StatisticParameterData>(parent, true, out buffer))
        return;
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        cache.Add(new StatisticsUISystem.StatItem(index, statisticsData.m_Category, statisticsData.m_Group == Entity.Null ? parent : statisticsData.m_Group, parent, (int) prefab.m_StatisticsType, prefab.m_UnitType, index, prefab.name + prefab.GetParameterName(buffer[index].m_Value), buffer[index].m_Color, locked, stacked: statisticsData.m_Stacked));
      }
    }

    private void BindGroups(IJsonWriter binder, Entity parent)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.CacheChildren(parent, this.m_GroupCache);
      // ISSUE: reference to a compiler-generated field
      binder.ArrayBegin(this.m_GroupCache.Count);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_GroupCache.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        binder.Write<StatisticsUISystem.StatItem>(this.m_GroupCache[index]);
      }
      binder.ArrayEnd();
    }

    private void BindData(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      binder.ArrayBegin(this.m_SelectedStatistics.Count);
      // ISSUE: reference to a compiler-generated field
      for (int index = this.m_SelectedStatistics.Count - 1; index >= 0; --index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.BindData(binder, this.m_SelectedStatistics[index]);
      }
      binder.ArrayEnd();
    }

    private void BindData(IJsonWriter binder, StatisticsUISystem.StatItem stat)
    {
      binder.TypeBegin("statistics.ChartDataSets");
      binder.PropertyName("label");
      // ISSUE: reference to a compiler-generated field
      binder.Write(stat.key);
      binder.PropertyName("data");
      // ISSUE: reference to a compiler-generated method
      NativeList<StatisticsUISystem.DataPoint> statisticData = this.GetStatisticData(stat);
      binder.ArrayBegin(statisticData.Length);
      for (int index = 0; index < statisticData.Length; ++index)
        binder.Write<StatisticsUISystem.DataPoint>(statisticData[index]);
      binder.ArrayEnd();
      binder.PropertyName("borderColor");
      // ISSUE: reference to a compiler-generated field
      binder.Write(stat.color.ToHexCode());
      binder.PropertyName("backgroundColor");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      binder.Write(string.Format("rgba({0}, {1}, {2}, 0.5)", (object) Mathf.RoundToInt(stat.color.r * (float) byte.MaxValue), (object) Mathf.RoundToInt(stat.color.g * (float) byte.MaxValue), (object) Mathf.RoundToInt(stat.color.b * (float) byte.MaxValue)));
      binder.PropertyName("fill");
      // ISSUE: reference to a compiler-generated field
      if (this.m_Stacked)
        binder.Write("origin");
      else
        binder.Write("false");
      binder.TypeEnd();
    }

    private NativeList<StatisticsUISystem.DataPoint> GetStatisticData(
      StatisticsUISystem.StatItem stat)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem.CompleteWriters();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      StatisticsPrefab prefab = this.m_PrefabSystem.GetPrefab<StatisticsPrefab>(stat.entity);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      TimeData singleton = TimeData.GetSingleton(this.m_TimeDataQuery);
      // ISSUE: reference to a compiler-generated field
      int sampleCount = this.m_CityStatisticsSystem.sampleCount;
      // ISSUE: reference to a compiler-generated field
      int num1 = math.min(this.m_SampleRange + 1, sampleCount);
      if (sampleCount <= 1)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: explicit reference operation
        return new NativeList<StatisticsUISystem.DataPoint>(1, (AllocatorManager.AllocatorHandle) Allocator.Temp)
        {
          @new StatisticsUISystem.DataPoint()
          {
            x = (long) singleton.m_FirstFrame,
            y = 0L
          }
        };
      }
      NativeArray<long> nativeArray1 = CollectionHelper.CreateNativeArray<long>(num1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      StatisticParameterData[] statisticParameterDataArray1;
      if (!(prefab is ParametricStatistic parametricStatistic))
        statisticParameterDataArray1 = new StatisticParameterData[1]
        {
          new StatisticParameterData() { m_Value = 0 }
        };
      else
        statisticParameterDataArray1 = parametricStatistic.GetParameters().ToArray<StatisticParameterData>();
      StatisticParameterData[] statisticParameterDataArray2 = statisticParameterDataArray1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (stat.isSubgroup && this.m_SelectedStatistics.Count<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup)) > 1)
      {
        for (int index1 = 0; index1 < statisticParameterDataArray2.Length; ++index1)
        {
          int parameter = statisticParameterDataArray2[index1].m_Value;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          NativeArray<long> nativeArray2 = this.EnsureDataSize(this.m_CityStatisticsSystem.GetStatisticDataArrayLong((StatisticType) stat.statisticType, parameter));
          for (int index2 = 0; index2 < num1; ++index2)
          {
            long num2 = nativeArray2[nativeArray2.Length - num1 + index2];
            // ISSUE: reference to a compiler-generated field
            if (stat.statisticType == 4 && prefab is ResourceStatistic resourceStatistic)
            {
              Resource resource = EconomyUtils.GetResource(resourceStatistic.m_Resources[index1].m_Resource);
              ResourceData componentData = this.EntityManager.GetComponentData<ResourceData>(prefabs[resource]);
              num2 *= (long) (int) EconomyUtils.GetMarketPrice(componentData);
            }
            nativeArray1[index2] += num2;
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        int parameter = statisticParameterDataArray2[stat.parameterIndex].m_Value;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<long> statisticDataArrayLong = this.m_CityStatisticsSystem.GetStatisticDataArrayLong((StatisticType) stat.statisticType, parameter);
        NativeArray<long> nativeArray3 = CollectionHelper.CreateNativeArray<long>(0, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (stat.statisticType == 16 || stat.statisticType == 15)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          nativeArray3 = this.EnsureDataSize(this.m_CityStatisticsSystem.GetStatisticDataArrayLong(StatisticType.Population));
        }
        // ISSUE: reference to a compiler-generated method
        NativeArray<long> nativeArray4 = this.EnsureDataSize(statisticDataArrayLong);
        for (int index = 0; index < num1; ++index)
        {
          long num3 = nativeArray4[nativeArray4.Length - num1 + index];
          // ISSUE: reference to a compiler-generated field
          if (stat.statisticType == 4 && prefab is ResourceStatistic resourceStatistic)
          {
            // ISSUE: reference to a compiler-generated field
            Resource resource = EconomyUtils.GetResource(resourceStatistic.m_Resources[stat.parameterIndex].m_Resource);
            ResourceData componentData = this.EntityManager.GetComponentData<ResourceData>(prefabs[resource]);
            num3 *= (long) (int) EconomyUtils.GetMarketPrice(componentData);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (nativeArray3.Length > 0 && (stat.statisticType == 16 || stat.statisticType == 15))
          {
            long num4 = nativeArray3[nativeArray3.Length - num1 + index];
            if (num4 > 0L)
              num3 /= num4;
          }
          nativeArray1[index] += num3;
        }
      }
      // ISSUE: reference to a compiler-generated method
      return this.GetDataPoints(num1, sampleCount, nativeArray1, singleton);
    }

    private NativeArray<long> EnsureDataSize(NativeArray<long> data)
    {
      // ISSUE: reference to a compiler-generated field
      if (data.Length >= this.m_CityStatisticsSystem.sampleCount)
        return data;
      // ISSUE: reference to a compiler-generated field
      NativeArray<long> nativeArray = CollectionHelper.CreateNativeArray<long>(this.m_CityStatisticsSystem.sampleCount, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      int num = 0;
      for (int index = 0; index < nativeArray.Length; ++index)
        nativeArray[index] = index >= nativeArray.Length - data.Length ? data[num++] : 0L;
      return nativeArray;
    }

    private NativeList<StatisticsUISystem.DataPoint> GetDataPoints(
      int range,
      int samples,
      NativeArray<long> data,
      TimeData timeData)
    {
      // ISSUE: reference to a compiler-generated method
      int sampleInterval = this.GetSampleInterval(range);
      NativeList<StatisticsUISystem.DataPoint> dataPoints = new NativeList<StatisticsUISystem.DataPoint>(data.Length / sampleInterval, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      int num1 = 0;
      // ISSUE: reference to a compiler-generated field
      uint x = (uint) math.max((int) this.m_CityStatisticsSystem.GetSampleFrameIndex(samples - range) - (int) timeData.m_FirstFrame, 0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      dataPoints.Add(new StatisticsUISystem.DataPoint()
      {
        x = (long) (uint) math.max((long) x, (long) (this.m_TimeUISystem.GetTicks() - 8192 * this.m_SampleRange)),
        y = data[0]
      });
      if (data.Length > 2)
      {
        for (int index = 1; index < data.Length - 1; ++index)
        {
          if (num1 % sampleInterval == 0)
          {
            // ISSUE: reference to a compiler-generated field
            uint sampleFrameIndex = this.m_CityStatisticsSystem.GetSampleFrameIndex(samples - range + index);
            // ISSUE: object of a compiler-generated type is created
            dataPoints.Add(new StatisticsUISystem.DataPoint()
            {
              x = (long) (sampleFrameIndex - timeData.m_FirstFrame),
              y = data[index]
            });
          }
          ++num1;
        }
      }
      // ISSUE: reference to a compiler-generated field
      int sampleFrameIndex1 = (int) this.m_CityStatisticsSystem.GetSampleFrameIndex(samples);
      ref NativeList<StatisticsUISystem.DataPoint> local1 = ref dataPoints;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StatisticsUISystem.DataPoint dataPoint = new StatisticsUISystem.DataPoint();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      dataPoint.x = (long) (uint) (this.m_TimeUISystem.GetTicks() + 182 + 1);
      ref StatisticsUISystem.DataPoint local2 = ref dataPoint;
      ref NativeArray<long> local3 = ref data;
      long num2 = local3[local3.Length - 1];
      // ISSUE: reference to a compiler-generated field
      local2.y = num2;
      ref StatisticsUISystem.DataPoint local4 = ref dataPoint;
      local1.Add(in local4);
      return dataPoints;
    }

    private int GetSampleInterval(int range)
    {
      int num1 = 32;
      int num2 = range;
      if (num2 <= num1)
        return 1;
      int num3 = num1 - 2;
      return Math.Max(1, (num2 - 2) / num3);
    }

    private void ProcessAddStat(StatisticsUISystem.StatItem stat)
    {
      // ISSUE: reference to a compiler-generated field
      if (stat.locked)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (stat.category != this.m_ActiveCategory)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedStatistics.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedStatisticsTracker.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveCategory = stat.category;
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveCategoryBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActiveGroup == Entity.Null || stat.isGroup || stat.group != this.m_ActiveGroup)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedStatistics.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedStatisticsTracker.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveGroup = stat.isGroup ? stat.entity : stat.group;
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveGroupBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      if (stat.isSubgroup)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = this.m_SelectedStatisticsTracker.Count<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
        if (num == 1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          StatisticsUISystem.StatItem stat1 = this.m_SelectedStatisticsTracker.First<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
          // ISSUE: reference to a compiler-generated field
          this.m_ClearActive = false;
          // ISSUE: reference to a compiler-generated method
          this.DeepRemoveStat(stat1);
          // ISSUE: reference to a compiler-generated method
          this.AddStat(stat1);
        }
        if (num == 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedStatisticsTracker.Add(stat);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.TryAddChildren(stat, this.m_SubGroupCache);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.AddStat(stat);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (stat.isGroup)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedStatisticsTracker.Add(stat);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.TryAddChildren(stat, this.m_GroupCache))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedStatistics.Add(stat);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.AddStat(stat);
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateStackedStatus();
      // ISSUE: reference to a compiler-generated method
      this.UpdateStats();
    }

    private void UpdateStackedStatus()
    {
      UIStatisticsGroupData component;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedStatisticsTracker.Count<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (stat => stat.isSubgroup && stat.group == this.m_ActiveGroup)) > 1 && this.EntityManager.TryGetComponent<UIStatisticsGroupData>(this.m_ActiveGroup, out component))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Stacked = component.m_Stacked;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedStatisticsTracker.Count > 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Stacked = false;
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_SelectedStatisticsTracker.Count; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SelectedStatisticsTracker[index].stacked)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Stacked = true;
              break;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Stacked = false;
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_StackedBinding.Update();
    }

    private void AddStat(StatisticsUISystem.StatItem stat)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SelectedStatisticsTracker.Contains(stat))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedStatisticsTracker.Add(stat);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedStatistics.Contains(stat))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatistics.Add(stat);
    }

    private bool TryAddChildren(
      StatisticsUISystem.StatItem stat,
      List<StatisticsUISystem.StatItem> cache)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.CacheChildren(stat.entity, cache);
      for (int index = 0; index < cache.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        this.ProcessAddStat(cache[index]);
      }
      return cache.Count > 0;
    }

    private void DeepRemoveStat(StatisticsUISystem.StatItem stat)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SelectedStatisticsTracker.Contains(stat))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int index1 = this.m_SelectedStatisticsTracker.FindIndex((Predicate<StatisticsUISystem.StatItem>) (s => s.entity == stat.group));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int index2 = this.m_SelectedStatisticsTracker.FindIndex((Predicate<StatisticsUISystem.StatItem>) (s => s.entity == stat.entity && s.isSubgroup));
        if (index1 >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          StatisticsUISystem.StatItem stat1 = this.m_SelectedStatisticsTracker[index1];
          if (index2 >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            StatisticsUISystem.StatItem stat2 = this.m_SelectedStatisticsTracker[index2];
            // ISSUE: reference to a compiler-generated method
            this.DeepRemoveStat(stat1);
            // ISSUE: reference to a compiler-generated method
            this.ProcessAddStat(stat2);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.DeepRemoveStat(stat1);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num1 = this.m_SelectedStatisticsTracker.Count<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
      // ISSUE: reference to a compiler-generated method
      this.RemoveStat(stat);
      // ISSUE: reference to a compiler-generated field
      if (stat.isGroup)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = this.m_SelectedStatistics.Count - 1; index >= 0; --index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedStatistics[index].group == stat.entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedStatistics.RemoveAt(index);
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = this.m_SelectedStatisticsTracker.Count - 1; index >= 0; --index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedStatisticsTracker[index].group == stat.entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedStatisticsTracker.RemoveAt(index);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (stat.isSubgroup)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = this.m_SelectedStatistics.Count - 1; index >= 0; --index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedStatistics[index].entity == stat.entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedStatistics.RemoveAt(index);
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = this.m_SelectedStatisticsTracker.Count - 1; index >= 0; --index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedStatisticsTracker[index].entity == stat.entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedStatisticsTracker.RemoveAt(index);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num2 = this.m_SelectedStatisticsTracker.Count<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
      if (num1 > 1 && num2 == 1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        StatisticsUISystem.StatItem stat3 = this.m_SelectedStatisticsTracker.First<StatisticsUISystem.StatItem>((Func<StatisticsUISystem.StatItem, bool>) (s => s.isSubgroup));
        // ISSUE: reference to a compiler-generated method
        this.RemoveStat(stat3);
        // ISSUE: reference to a compiler-generated method
        this.ProcessAddStat(stat3);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ClearActive && this.m_SelectedStatistics.Count == 0 && this.m_SelectedStatisticsTracker.Count <= 1)
      {
        // ISSUE: reference to a compiler-generated method
        this.ClearStats();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateStats();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ClearActive = true;
      // ISSUE: reference to a compiler-generated method
      this.UpdateStackedStatus();
    }

    private void RemoveStat(StatisticsUISystem.StatItem stat)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatistics.Remove(stat);
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatisticsTracker.Remove(stat);
    }

    private void ClearStats()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatistics.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatisticsTracker.Clear();
      // ISSUE: reference to a compiler-generated method
      this.UpdateStats();
      // ISSUE: reference to a compiler-generated method
      this.ClearActive();
    }

    private void UpdateStats()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatistics.Sort();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedStatisticsBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_DataBinding.Update();
    }

    private void ClearActive()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveGroup = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveGroupBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveCategory = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveCategoryBinding.Update();
    }

    private void SetSampleRange(int range)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SampleRange = range;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SampleRangeBinding.Update(this.m_SampleRange);
      // ISSUE: reference to a compiler-generated method
      this.UpdateStats();
    }

    [Preserve]
    public StatisticsUISystem()
    {
    }

    public struct StatCategory : IComparable<StatisticsUISystem.StatCategory>
    {
      public Entity m_Entity;
      public PrefabData m_PrefabData;
      public UIObjectData m_ObjectData;

      public StatCategory(Entity entity, UIObjectData objectData, PrefabData prefabData)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabData = prefabData;
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectData = objectData;
      }

      public int CompareTo(StatisticsUISystem.StatCategory other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_ObjectData.m_Priority.CompareTo(other.m_ObjectData.m_Priority);
      }
    }

    public struct DataPoint : IJsonWritable
    {
      public long x;
      public long y;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("x");
        // ISSUE: reference to a compiler-generated field
        writer.Write((float) this.x);
        writer.PropertyName("y");
        // ISSUE: reference to a compiler-generated field
        writer.Write((float) this.y);
        writer.TypeEnd();
      }
    }

    public struct StatItem : IJsonReadable, IJsonWritable, IComparable<StatisticsUISystem.StatItem>
    {
      public Entity category;
      public Entity group;
      public Entity entity;
      public int statisticType;
      public int unitType;
      public int parameterIndex;
      public string key;
      public Color color;
      public bool locked;
      public bool isGroup;
      public bool isSubgroup;
      public bool stacked;
      public int priority;

      public StatItem(
        int priority,
        Entity category,
        Entity group,
        Entity entity,
        int statisticType,
        StatisticUnitType unitType,
        int parameterIndex,
        string key,
        Color color,
        bool locked,
        bool isGroup = false,
        bool isSubgroup = false,
        bool stacked = true)
      {
        // ISSUE: reference to a compiler-generated field
        this.category = category;
        // ISSUE: reference to a compiler-generated field
        this.group = group;
        // ISSUE: reference to a compiler-generated field
        this.entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.statisticType = statisticType;
        // ISSUE: reference to a compiler-generated field
        this.unitType = (int) unitType;
        // ISSUE: reference to a compiler-generated field
        this.parameterIndex = parameterIndex;
        // ISSUE: reference to a compiler-generated field
        this.key = key;
        // ISSUE: reference to a compiler-generated field
        this.color = color;
        // ISSUE: reference to a compiler-generated field
        this.locked = locked;
        // ISSUE: reference to a compiler-generated field
        this.isGroup = isGroup;
        // ISSUE: reference to a compiler-generated field
        this.isSubgroup = isSubgroup;
        // ISSUE: reference to a compiler-generated field
        this.stacked = stacked;
        // ISSUE: reference to a compiler-generated field
        this.priority = priority;
      }

      public void Read(IJsonReader reader)
      {
        long num = (long) reader.ReadMapBegin();
        reader.ReadProperty("category");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.category);
        reader.ReadProperty("group");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.group);
        reader.ReadProperty("entity");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.entity);
        reader.ReadProperty("statisticType");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.statisticType);
        reader.ReadProperty("unitType");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.unitType);
        reader.ReadProperty("parameterIndex");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.parameterIndex);
        reader.ReadProperty("key");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.key);
        reader.ReadProperty("color");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.color);
        reader.ReadProperty("locked");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.locked);
        reader.ReadProperty("isGroup");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.isGroup);
        reader.ReadProperty("isSubgroup");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.isSubgroup);
        reader.ReadProperty("stacked");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.stacked);
        reader.ReadProperty("priority");
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.priority);
        reader.ReadMapEnd();
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("statistics.StatItem");
        writer.PropertyName("category");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.category);
        writer.PropertyName("group");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.group);
        writer.PropertyName("entity");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.entity);
        writer.PropertyName("statisticType");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.statisticType);
        writer.PropertyName("unitType");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.unitType);
        writer.PropertyName("parameterIndex");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.parameterIndex);
        writer.PropertyName("key");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.key);
        writer.PropertyName("color");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.color);
        writer.PropertyName("locked");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.locked);
        writer.PropertyName("isGroup");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.isGroup);
        writer.PropertyName("isSubgroup");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.isSubgroup);
        writer.PropertyName("stacked");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.stacked);
        writer.PropertyName("priority");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.priority);
        writer.TypeEnd();
      }

      public int CompareTo(StatisticsUISystem.StatItem other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.priority.CompareTo(other.priority);
      }
    }
  }
}
