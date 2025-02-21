// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MapTilePurchaseSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.PSI.Common;
using Game.Areas;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class MapTilePurchaseSystem : GameSystemBase, IMapTilePurchaseSystem
  {
    private static readonly double kMapTileSizeModifier = 1.0 / Math.Pow(623.304347826087, 2.0);
    private static readonly double kResourceModifier = 8.0718994140625E-07;
    private static readonly int kAutoUnlockedTiles = 9;
    private static readonly double[] kMapFeatureBaselineModifiers = new double[8]
    {
      MapTilePurchaseSystem.kMapTileSizeModifier,
      MapTilePurchaseSystem.kMapTileSizeModifier,
      MapTilePurchaseSystem.kResourceModifier,
      MapTilePurchaseSystem.kResourceModifier,
      MapTilePurchaseSystem.kResourceModifier,
      MapTilePurchaseSystem.kResourceModifier,
      1.0,
      MapTilePurchaseSystem.kResourceModifier
    };
    private SelectionToolSystem m_SelectionToolSystem;
    private DefaultToolSystem m_DefaultToolSystem;
    private ToolSystem m_ToolSystem;
    private CitySystem m_CitySystem;
    private NaturalResourceSystem m_NaturalResourceSystem;
    private MapTileSystem m_MapTileSystem;
    private EntityQuery m_SelectionQuery;
    private EntityQuery m_OwnedTileQuery;
    private EntityQuery m_LockedMapTilesQuery;
    private EntityQuery m_UnlockedMilestoneQuery;
    private EntityQuery m_LockedMilestoneQuery;
    private EntityQuery m_EconomyParameterQuery;
    private NativeArray<float> m_FeatureAmounts;
    private float m_Cost;
    private float m_Upkeep;
    private MapTilePurchaseSystem.TypeHandle __TypeHandle;

    public TilePurchaseErrorFlags status { get; private set; }

    public float GetMapTileUpkeepCostMultiplier(int tileCount)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return tileCount <= MapTilePurchaseSystem.kAutoUnlockedTiles ? 0.0f : this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>().m_MapTileUpkeepCostMultiplier.Evaluate((float) tileCount);
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectionToolSystem = this.World.GetOrCreateSystemManaged<SelectionToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultToolSystem = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NaturalResourceSystem = this.World.GetOrCreateSystemManaged<NaturalResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSystem = this.World.GetOrCreateSystemManaged<MapTileSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<SelectionElement>());
      // ISSUE: reference to a compiler-generated field
      this.m_OwnedTileQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapTile>(), ComponentType.Exclude<Native>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedMapTilesQuery = this.GetEntityQuery(ComponentType.ReadWrite<MapTile>(), ComponentType.ReadOnly<Native>(), ComponentType.ReadOnly<Area>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedMilestoneQuery = this.GetEntityQuery(ComponentType.ReadOnly<MilestoneData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedMilestoneQuery = this.GetEntityQuery(ComponentType.ReadOnly<MilestoneData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FeatureAmounts = new NativeArray<float>(8, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_FeatureAmounts.Dispose();
    }

    [Preserve]
    protected override void OnUpdate() => this.UpdateStatus();

    private void UpdateStatus()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_FeatureAmounts.Fill<float>(0.0f);
      // ISSUE: reference to a compiler-generated field
      this.m_Cost = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_Upkeep = 0.0f;
      // ISSUE: reference to a compiler-generated method
      int availableTiles = this.GetAvailableTiles();
      if (availableTiles == 0)
      {
        // ISSUE: reference to a compiler-generated method
        this.status = this.IsMilestonesLeft() ? TilePurchaseErrorFlags.NoCurrentlyAvailable : TilePurchaseErrorFlags.NoAvailable;
      }
      else
      {
        DynamicBuffer<SelectionElement> selections;
        // ISSUE: reference to a compiler-generated method
        if (!this.TryGetSelections(true, out selections) || selections.Length == 0)
        {
          this.status = TilePurchaseErrorFlags.NoSelection;
        }
        else
        {
          this.status = TilePurchaseErrorFlags.None;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_TilePurchaseCostFactor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<TilePurchaseCostFactor> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_TilePurchaseCostFactor_RO_ComponentLookup;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<Native> roComponentLookup3 = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<MapTile> roComponentLookup4 = this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentLookup;
          NativeList<float> list = new NativeList<float>((AllocatorManager.AllocatorHandle) Allocator.Temp);
          int num1 = 0;
          // ISSUE: reference to a compiler-generated method
          int ownedTiles = this.CalculateOwnedTiles();
          // ISSUE: reference to a compiler-generated method
          double ownedTilesCost = (double) this.CalculateOwnedTilesCost();
          // ISSUE: reference to a compiler-generated method
          float num2 = (float) ownedTilesCost * this.GetMapTileUpkeepCostMultiplier(ownedTiles);
          float num3 = (float) ownedTilesCost;
          for (int index1 = 0; index1 < selections.Length; ++index1)
          {
            Entity entity = selections[index1].m_Entity;
            if (roComponentLookup4.HasComponent(entity) && roComponentLookup3.HasComponent(entity))
            {
              ++num1;
              DynamicBuffer<MapFeatureElement> buffer1;
              if (this.EntityManager.TryGetBuffer<MapFeatureElement>(entity, true, out buffer1))
              {
                Entity prefab = roComponentLookup1[entity].m_Prefab;
                float amount1 = roComponentLookup2[prefab].m_Amount;
                DynamicBuffer<MapFeatureData> buffer2;
                if (this.EntityManager.TryGetBuffer<MapFeatureData>(prefab, true, out buffer2))
                {
                  float num4 = 0.0f;
                  for (int index2 = 0; index2 < buffer1.Length; ++index2)
                  {
                    float amount2 = buffer1[index2].m_Amount;
                    // ISSUE: reference to a compiler-generated field
                    this.m_FeatureAmounts[index2] += amount2;
                    // ISSUE: reference to a compiler-generated method
                    double baselineModifier = this.GetBaselineModifier(index2);
                    num4 += (float) ((double) amount2 * baselineModifier * 10.0) * buffer2[index2].m_Cost * amount1;
                    num3 += (float) ((double) amount2 * baselineModifier * 10.0) * buffer2[index2].m_Cost * amount1;
                  }
                  list.Add(in num4);
                }
              }
            }
          }
          list.Sort<float>();
          for (int index = 0; index < list.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Cost += list[list.Length - index - 1] * (float) (ownedTiles + index);
          }
          list.Dispose();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Upkeep = num3 * this.GetMapTileUpkeepCostMultiplier(ownedTiles + num1) - num2;
          if (num1 > 0 && num1 > availableTiles)
            this.status |= TilePurchaseErrorFlags.InsufficientPermits;
          // ISSUE: reference to a compiler-generated field
          if (this.cost <= this.m_CitySystem.moneyAmount)
            return;
          this.status |= TilePurchaseErrorFlags.InsufficientFunds;
        }
      }
    }

    public int GetAvailableTiles()
    {
      // ISSUE: reference to a compiler-generated method
      int ownedTiles = this.CalculateOwnedTiles();
      // ISSUE: reference to a compiler-generated field
      int autoUnlockedTiles = MapTilePurchaseSystem.kAutoUnlockedTiles;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<MilestoneData> componentDataArray = this.m_UnlockedMilestoneQuery.ToComponentDataArray<MilestoneData>((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        foreach (MilestoneData milestoneData in componentDataArray)
          autoUnlockedTiles += milestoneData.m_MapTiles;
      }
      return Mathf.Max(autoUnlockedTiles - ownedTiles, 0);
    }

    public bool IsMilestonesLeft()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<MilestoneData> componentDataArray = this.m_LockedMilestoneQuery.ToComponentDataArray<MilestoneData>((AllocatorManager.AllocatorHandle) Allocator.Temp))
        return componentDataArray.Length != 0;
    }

    private double GetBaselineModifier(int mapFeature)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return mapFeature >= 0 && mapFeature < MapTilePurchaseSystem.kMapFeatureBaselineModifiers.Length ? MapTilePurchaseSystem.kMapFeatureBaselineModifiers[mapFeature] : 1.0;
    }

    public void UnlockMapTiles()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_LockedMapTilesQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_LockedMapTilesQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        MapTilePurchaseSystem.UnlockTile(this.EntityManager, entityArray[index]);
      }
      entityArray.Dispose();
    }

    public void PurchaseSelection()
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateStatus();
      if (this.status != TilePurchaseErrorFlags.None)
        return;
      // ISSUE: reference to a compiler-generated field
      PlayerMoney componentData = this.EntityManager.GetComponentData<PlayerMoney>(this.m_CitySystem.City);
      componentData.Subtract(this.cost);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.SetComponentData<PlayerMoney>(this.m_CitySystem.City, componentData);
      DynamicBuffer<SelectionElement> selections;
      // ISSUE: reference to a compiler-generated method
      if (!this.TryGetSelections(false, out selections))
        return;
      using (NativeArray<SelectionElement> nativeArray = selections.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        selections.Clear();
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          MapTilePurchaseSystem.UnlockTile(this.EntityManager, nativeArray[index].m_Entity);
        }
        // ISSUE: reference to a compiler-generated method
        PlatformManager.instance.IndicateAchievementProgress(new AchievementId[2]
        {
          Game.Achievements.Achievements.TheExplorer,
          Game.Achievements.Achievements.EverythingTheLightTouches
        }, this.CalculateOwnedTiles(), IndicateType.Absolute);
      }
    }

    public static void UnlockTile(EntityManager entityManager, Entity area)
    {
      if (!entityManager.HasComponent<Native>(area))
        return;
      entityManager.RemoveComponent<Native>(area);
      entityManager.AddComponentData<Updated>(area, new Updated());
    }

    private bool TryGetSelections(bool isReadOnly, out DynamicBuffer<SelectionElement> selections)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.selecting && !this.m_SelectionQuery.IsEmptyIgnoreFilter && this.EntityManager.TryGetBuffer<SelectionElement>(this.m_SelectionQuery.GetSingletonEntity(), isReadOnly, out selections))
        return true;
      selections = new DynamicBuffer<SelectionElement>();
      return false;
    }

    public int GetSelectedTileCount()
    {
      DynamicBuffer<SelectionElement> selections;
      // ISSUE: reference to a compiler-generated method
      return this.TryGetSelections(true, out selections) ? selections.Length : 0;
    }

    private int CalculateOwnedTiles()
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_OwnedTileQuery.CalculateEntityCountWithoutFiltering();
    }

    private float CalculateOwnedTilesCost()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_OwnedTileQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TilePurchaseCostFactor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<TilePurchaseCostFactor> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_TilePurchaseCostFactor_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> startTiles = this.m_MapTileSystem.GetStartTiles();
      float ownedTilesCost = 0.0f;
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        DynamicBuffer<MapFeatureElement> buffer1;
        if (!startTiles.Contains<Entity, Entity>(entityArray[index1]) && this.EntityManager.TryGetBuffer<MapFeatureElement>(entityArray[index1], true, out buffer1))
        {
          Entity prefab = roComponentLookup1[entityArray[index1]].m_Prefab;
          float amount1 = roComponentLookup2[prefab].m_Amount;
          DynamicBuffer<MapFeatureData> buffer2;
          if (this.EntityManager.TryGetBuffer<MapFeatureData>(prefab, true, out buffer2))
          {
            for (int index2 = 0; index2 < buffer1.Length; ++index2)
            {
              float amount2 = buffer1[index2].m_Amount;
              // ISSUE: reference to a compiler-generated field
              this.m_FeatureAmounts[index2] += amount2;
              // ISSUE: reference to a compiler-generated method
              double baselineModifier = this.GetBaselineModifier(index2);
              ownedTilesCost += (float) ((double) amount2 * baselineModifier * 10.0) * buffer2[index2].m_Cost * amount1;
            }
          }
        }
      }
      entityArray.Dispose();
      return ownedTilesCost;
    }

    public int CalculateOwnedTilesUpkeep()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return Mathf.RoundToInt(this.CalculateOwnedTilesCost() * this.GetMapTileUpkeepCostMultiplier(this.CalculateOwnedTiles()));
    }

    public float GetFeatureAmount(MapFeature feature)
    {
      // ISSUE: reference to a compiler-generated field
      float featureAmount = this.m_FeatureAmounts[(int) feature];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return feature != MapFeature.FertileLand ? featureAmount : this.m_NaturalResourceSystem.ResourceAmountToArea(featureAmount);
    }

    public bool selecting
    {
      get
      {
        return this.m_ToolSystem.activeTool == this.m_SelectionToolSystem && this.m_SelectionToolSystem.selectionType == SelectionType.MapTiles;
      }
      set
      {
        if (value)
        {
          this.m_SelectionToolSystem.selectionType = SelectionType.MapTiles;
          this.m_SelectionToolSystem.selectionOwner = Entity.Null;
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_SelectionToolSystem;
        }
        else
        {
          if (this.m_ToolSystem.activeTool != this.m_SelectionToolSystem)
            return;
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
        }
      }
    }

    public int cost => Mathf.RoundToInt(this.m_Cost);

    public int upkeep => Mathf.RoundToInt(this.m_Upkeep);

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
    public MapTilePurchaseSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TilePurchaseCostFactor> __Game_Prefabs_TilePurchaseCostFactor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MapTile> __Game_Areas_MapTile_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TilePurchaseCostFactor_RO_ComponentLookup = state.GetComponentLookup<TilePurchaseCostFactor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapTile_RO_ComponentLookup = state.GetComponentLookup<MapTile>(true);
      }
    }
  }
}
