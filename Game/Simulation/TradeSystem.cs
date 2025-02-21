// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TradeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Debug;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TradeSystem : GameSystemBase, ITradeSystem, IDefaultSerializable, ISerializable
  {
    private static readonly float kRefreshRate = 0.01f;
    public static readonly int kUpdatesPerDay = 128;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_StorageGroup;
    private EntityQuery m_TradeParameterQuery;
    private EntityQuery m_CityQuery;
    private ResourceSystem m_ResourceSystem;
    [DebugWatchDeps]
    private JobHandle m_DebugTradeBalanceDeps;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_TradeBalances;
    private NativeArray<float> m_CachedCosts;
    private TradeSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / TradeSystem.kUpdatesPerDay;
    }

    public float GetTradePrice(
      Resource resource,
      OutsideConnectionTransferType type,
      bool import,
      DynamicBuffer<CityModifier> cityEffects)
    {
      OutsideConnectionTransferType connectionTransferType = OutsideConnectionTransferType.Road;
      float x = float.MaxValue;
      for (; connectionTransferType != OutsideConnectionTransferType.Last; connectionTransferType = (OutsideConnectionTransferType) ((int) connectionTransferType << 1))
      {
        if ((connectionTransferType & type) != OutsideConnectionTransferType.None)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          x = math.min(x, this.m_CachedCosts[TradeSystem.GetCacheIndex(resource, type, import)]);
        }
      }
      if (import)
        CityUtils.ApplyModifier(ref x, cityEffects, CityModifierType.ImportCost);
      else
        CityUtils.ApplyModifier(ref x, cityEffects, CityModifierType.ExportCost);
      return x;
    }

    private static int GetCacheIndex(
      Resource resource,
      OutsideConnectionTransferType type,
      bool import)
    {
      return Mathf.RoundToInt((float) ((double) math.log2((float) type) * 2.0 * (double) EconomyUtils.ResourceCount + (double) (2 * EconomyUtils.GetResourceIndex(resource)) + (import ? 1.0 : 0.0)));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TradeBalances = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CachedCosts = new NativeArray<float>(2 * EconomyUtils.ResourceCount * Mathf.RoundToInt(math.log2(32f)), Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.StorageCompany>(), ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Game.Economy.Resources>(), ComponentType.ReadOnly<TradeCost>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TradeParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<OutsideTradeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.City.City>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_StorageGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TradeParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CityQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CachedCosts.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TradeBalances.Dispose();
      base.OnDestroy();
    }

    private static TradeCost CalculateTradeCost(
      Resource resource,
      int tradeBalance,
      OutsideConnectionTransferType type,
      float weight,
      ref OutsideTradeParameterData tradeParameters,
      DynamicBuffer<CityModifier> cityEffects)
    {
      float num1 = tradeParameters.GetWeightCost(type) * weight;
      if ((double) tradeBalance < 0.0)
        num1 *= (float) (1.0 + (double) tradeParameters.GetDistanceCost(type) * (double) math.max(50f, math.sqrt((float) -tradeBalance)));
      CityUtils.ApplyModifier(ref num1, cityEffects, CityModifierType.ImportCost);
      float num2 = tradeParameters.GetWeightCost(type) * weight;
      if ((double) tradeBalance > 0.0)
        num2 *= (float) (1.0 + (double) tradeParameters.GetDistanceCost(type) * (double) math.max(50f, math.sqrt((float) tradeBalance)));
      CityUtils.ApplyModifier(ref num2, cityEffects, CityModifierType.ExportCost);
      return new TradeCost()
      {
        m_Resource = resource,
        m_BuyCost = num1,
        m_SellCost = num2
      };
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context context)
    {
      if (context.purpose != Colossal.Serialization.Entities.Purpose.NewGame)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_StorageGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        Entity entity = entityArray[index];
        PrefabRef component1;
        DynamicBuffer<Game.Economy.Resources> buffer;
        StorageCompanyData component2;
        StorageLimitData component3;
        if (this.EntityManager.TryGetComponent<PrefabRef>(entity, out component1) && this.EntityManager.TryGetBuffer<Game.Economy.Resources>(entity, false, out buffer) && this.EntityManager.TryGetComponent<StorageCompanyData>((Entity) component1, out component2) && this.EntityManager.TryGetComponent<StorageLimitData>((Entity) component1, out component3))
        {
          ResourceIterator iterator = ResourceIterator.GetIterator();
          int num = EconomyUtils.CountResources(component2.m_StoredResources);
          while (iterator.Next())
          {
            if ((component2.m_StoredResources & iterator.resource) != Resource.NoResource)
            {
              if (iterator.resource == Resource.OutgoingMail)
              {
                EconomyUtils.SetResources(Resource.OutgoingMail, buffer, 0);
              }
              else
              {
                int resources = EconomyUtils.GetResources(iterator.resource, buffer);
                int amount = component3.m_Limit / num / 2 - resources;
                EconomyUtils.AddResources(iterator.resource, amount, buffer);
              }
            }
          }
        }
      }
      entityArray.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new TradeSystem.TradeJob()
      {
        m_Chunks = this.m_StorageGroup.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_TradeCostType = this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferTypeHandle,
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_Limits = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_StorageDatas = this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup,
        m_OutsideConnectionDatas = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_GarbageFacilityDatas = this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup,
        m_CityEffects = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_City = this.m_CityQuery.GetSingletonEntity(),
        m_TradeBalances = this.m_TradeBalances,
        m_CachedCosts = this.m_CachedCosts,
        m_TradeParameters = this.m_TradeParameterQuery.GetSingleton<OutsideTradeParameterData>(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      }.Schedule<TradeSystem.TradeJob>(JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_DebugTradeBalanceDeps = jobHandle;
      this.Dependency = jobHandle;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_TradeBalances);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (!(reader.context.version >= Version.tradeBalance))
        return;
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_TradeBalances);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_TradeBalances.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TradeBalances[index] = 0;
      }
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
    public TradeSystem()
    {
    }

    [BurstCompile]
    private struct TradeJob : IJob
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public BufferTypeHandle<Game.Economy.Resources> m_ResourceType;
      public BufferTypeHandle<TradeCost> m_TradeCostType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> m_GarbageFacilityDatas;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageDatas;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_Limits;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityEffects;
      public Entity m_City;
      public NativeArray<int> m_TradeBalances;
      public NativeArray<float> m_CachedCosts;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;
      public OutsideTradeParameterData m_TradeParameters;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;

      private TradeCost GetCachedTradeCost(
        Resource resource,
        OutsideConnectionTransferType type,
        NativeArray<float> cache)
      {
        float x1 = float.MaxValue;
        float x2 = float.MaxValue;
        for (OutsideConnectionTransferType connectionTransferType = OutsideConnectionTransferType.Road; connectionTransferType != OutsideConnectionTransferType.Last; connectionTransferType = (OutsideConnectionTransferType) ((int) connectionTransferType << 1))
        {
          if ((connectionTransferType & type) != OutsideConnectionTransferType.None)
          {
            // ISSUE: reference to a compiler-generated method
            x1 = math.min(x1, cache[TradeSystem.GetCacheIndex(resource, type, true)]);
            // ISSUE: reference to a compiler-generated method
            x2 = math.min(x2, cache[TradeSystem.GetCacheIndex(resource, type, false)]);
          }
        }
        return new TradeCost()
        {
          m_Resource = resource,
          m_BuyCost = x1,
          m_SellCost = x2
        };
      }

      public void Execute()
      {
        ResourceIterator iterator1 = ResourceIterator.GetIterator();
        while (iterator1.Next())
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(iterator1.resource);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num = Mathf.RoundToInt((1f - TradeSystem.kRefreshRate) * (float) this.m_TradeBalances[resourceIndex]);
          // ISSUE: reference to a compiler-generated field
          this.m_TradeBalances[resourceIndex] = num;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float weight = this.m_ResourceDatas[this.m_ResourcePrefabs[iterator1.resource]].m_Weight;
          OutsideConnectionTransferType type = OutsideConnectionTransferType.Road;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CityModifier> cityEffect = this.m_CityEffects[this.m_City];
          for (; type != OutsideConnectionTransferType.Last; type = (OutsideConnectionTransferType) ((int) type << 1))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            TradeCost tradeCost = TradeSystem.CalculateTradeCost(iterator1.resource, this.m_TradeBalances[resourceIndex], type, weight, ref this.m_TradeParameters, cityEffect);
            Assert.IsTrue(!float.IsNaN(tradeCost.m_SellCost));
            Assert.IsTrue(!float.IsNaN(tradeCost.m_BuyCost));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_CachedCosts[TradeSystem.GetCacheIndex(iterator1.resource, type, false)] = tradeCost.m_SellCost;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_CachedCosts[TradeSystem.GetCacheIndex(iterator1.resource, type, true)] = tradeCost.m_BuyCost;
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Game.Economy.Resources> bufferAccessor1 = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourceType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<TradeCost> bufferAccessor2 = chunk.GetBufferAccessor<TradeCost>(ref this.m_TradeCostType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<InstalledUpgrade> bufferAccessor3 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            DynamicBuffer<Game.Economy.Resources> resources1 = bufferAccessor1[index2];
            Entity prefab = nativeArray2[index2].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            StorageCompanyData storageData = this.m_StorageDatas[prefab];
            DynamicBuffer<TradeCost> costs = bufferAccessor2[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_Limits.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              StorageLimitData limit = this.m_Limits[prefab];
              if (bufferAccessor3.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                UpgradeUtils.CombineStats<StorageLimitData>(ref limit, bufferAccessor3[index2], ref this.m_PrefabRefData, ref this.m_Limits);
              }
              ResourceIterator iterator2 = ResourceIterator.GetIterator();
              int num1 = EconomyUtils.CountResources(storageData.m_StoredResources);
              while (iterator2.Next())
              {
                if ((storageData.m_StoredResources & iterator2.resource) != Resource.NoResource)
                {
                  if (iterator2.resource == Resource.OutgoingMail)
                  {
                    EconomyUtils.SetResources(Resource.OutgoingMail, resources1, 0);
                  }
                  else
                  {
                    int resources2 = EconomyUtils.GetResources(iterator2.resource, resources1);
                    int num2 = limit.m_Limit / num1;
                    // ISSUE: reference to a compiler-generated field
                    if (iterator2.resource == Resource.Garbage && this.m_GarbageFacilityDatas.HasComponent(prefab))
                    {
                      // ISSUE: reference to a compiler-generated field
                      num2 = this.m_GarbageFacilityDatas[prefab].m_GarbageCapacity;
                    }
                    int amount = num2 / 2 - resources2;
                    // ISSUE: reference to a compiler-generated field
                    this.m_TradeBalances[EconomyUtils.GetResourceIndex(iterator2.resource)] -= amount;
                    EconomyUtils.AddResources(iterator2.resource, amount, resources1);
                    // ISSUE: reference to a compiler-generated field
                    OutsideConnectionTransferType type = this.m_OutsideConnectionDatas[prefab].m_Type;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    EconomyUtils.SetTradeCost(iterator2.resource, this.GetCachedTradeCost(iterator2.resource, type, this.m_CachedCosts), costs, false);
                    if (amount != 0 && (iterator2.resource & (Resource.UnsortedMail | Resource.LocalMail | Resource.OutgoingMail)) == Resource.NoResource)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
                      {
                        m_Statistic = StatisticType.Trade,
                        m_Change = (float) amount,
                        m_Parameter = EconomyUtils.GetResourceIndex(iterator2.resource)
                      });
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      public BufferTypeHandle<TradeCost> __Game_Companies_TradeCost_RW_BufferTypeHandle;
      public BufferTypeHandle<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> __Game_Prefabs_StorageCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> __Game_Prefabs_GarbageFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RW_BufferTypeHandle = state.GetBufferTypeHandle<TradeCost>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup = state.GetComponentLookup<StorageCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup = state.GetComponentLookup<GarbageFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
