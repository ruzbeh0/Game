// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ProductionCompanyUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ProductionCompanyUISystem : UISystemBase
  {
    private static readonly string kGroup = "production";
    private static readonly int kLevels = 5;
    private IBudgetSystem m_BudgetSystem;
    private ResourceSystem m_ResourceSystem;
    private PrefabSystem m_PrefabSystem;
    private Dictionary<string, Resource> m_ResourceIDMap;
    private RawValueBinding m_ProductionCompanyInfoBinding;
    private ValueBinding<int> m_IndustrialCompanyWealthBinding;
    private ValueBinding<int> m_CommercialCompanyWealthBinding;
    private NativeArray<ProductionCompanyUISystem.ProductionLevelInfo> m_CachedValues;
    private NativeArray<ProductionCompanyUISystem.ProductionLevelInfo> m_Values;
    private Resource m_SelectedResource;
    private NativeQueue<ProductionCompanyUISystem.ProductionCompanyInfo> m_ProductionCompanyInfoQueue;
    private EntityQuery m_CompanyQuery;
    private ProductionCompanyUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BudgetSystem = (IBudgetSystem) this.World.GetOrCreateSystemManaged<BudgetSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<PropertyRenter>(),
          ComponentType.ReadOnly<Employee>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<IndustrialCompany>(),
          ComponentType.ReadOnly<CommercialCompany>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedResource = Resource.NoResource;
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceIDMap = new Dictionary<string, Resource>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CachedValues = new NativeArray<ProductionCompanyUISystem.ProductionLevelInfo>(ProductionCompanyUISystem.kLevels, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Values = new NativeArray<ProductionCompanyUISystem.ProductionLevelInfo>(ProductionCompanyUISystem.kLevels, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCompanyInfoQueue = new NativeQueue<ProductionCompanyUISystem.ProductionCompanyInfo>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ProductionCompanyInfoBinding = new RawValueBinding(ProductionCompanyUISystem.kGroup, "productionCompanyInfo", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        binder.ArrayBegin(ProductionCompanyUISystem.kLevels);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < ProductionCompanyUISystem.kLevels; ++index)
        {
          binder.TypeBegin("production.ProductionCompanyInfo");
          binder.PropertyName("industrialCompanies");
          binder.Write(0);
          binder.PropertyName("industrialWorkers");
          binder.Write(0);
          binder.PropertyName("commercialCompanies");
          binder.Write(0);
          binder.PropertyName("commercialWorkers");
          binder.Write(0);
          binder.TypeEnd();
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_IndustrialCompanyWealthBinding = new ValueBinding<int>(ProductionCompanyUISystem.kGroup, "industrialCompanyWealth", 0)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CommercialCompanyWealthBinding = new ValueBinding<int>(ProductionCompanyUISystem.kGroup, "commercialCompanyWealth", 0)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string>(ProductionCompanyUISystem.kGroup, "selectResource", (Action<string>) (resourceID =>
      {
        Resource resource;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ResourceIDMap.TryGetValue(resourceID, out resource))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedResource = resource;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedResource = Resource.NoResource;
        }
      })));
      // ISSUE: reference to a compiler-generated method
      this.RebuildResourceIDMap();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_CachedValues.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Values.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCompanyInfoQueue.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ProductionCompanyInfoBinding.active)
      {
        // ISSUE: reference to a compiler-generated method
        this.PatchProductionCompanyInfo();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_IndustrialCompanyWealthBinding.active)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndustrialCompanyWealthBinding.Update(this.m_SelectedResource != Resource.NoResource ? this.m_BudgetSystem.GetCompanyWealth(false, this.m_SelectedResource) : 0);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CommercialCompanyWealthBinding.active)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialCompanyWealthBinding.Update(this.m_SelectedResource != Resource.NoResource ? this.m_BudgetSystem.GetCompanyWealth(true, this.m_SelectedResource) : 0);
    }

    private void UpdateProductionCompanyInfo(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      binder.ArrayBegin(ProductionCompanyUISystem.kLevels);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < ProductionCompanyUISystem.kLevels; ++index)
      {
        binder.TypeBegin("production.ProductionCompanyInfo");
        binder.PropertyName("industrialCompanies");
        binder.Write(0);
        binder.PropertyName("industrialWorkers");
        binder.Write(0);
        binder.PropertyName("commercialCompanies");
        binder.Write(0);
        binder.PropertyName("commercialWorkers");
        binder.Write(0);
        binder.TypeEnd();
      }
      binder.ArrayEnd();
    }

    private void PatchProductionCompanyInfo()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCompanyInfoQueue.Clear();
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_Values.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        ref NativeArray<ProductionCompanyUISystem.ProductionLevelInfo> local = ref this.m_Values;
        int index2 = index1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionLevelInfo productionLevelInfo1 = new ProductionCompanyUISystem.ProductionLevelInfo();
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionLevelInfo productionLevelInfo2 = productionLevelInfo1;
        local[index2] = productionLevelInfo2;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedResource != Resource.NoResource)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_IndustrialCompany_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.MapCompanyStatisticsJob jobData = new ProductionCompanyUISystem.MapCompanyStatisticsJob()
        {
          m_IndustrialCompanyType = this.__TypeHandle.__Game_Companies_IndustrialCompany_RO_ComponentTypeHandle,
          m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_EmployeeType = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
          m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
          m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
          m_Resource = this.m_SelectedResource,
          m_Queue = this.m_ProductionCompanyInfoQueue.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<ProductionCompanyUISystem.MapCompanyStatisticsJob>(this.m_CompanyQuery, this.Dependency);
        this.Dependency.Complete();
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionCompanyInfo productionCompanyInfo;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ProductionCompanyInfoQueue.TryDequeue(out productionCompanyInfo))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ProductionCompanyUISystem.ProductionLevelInfo productionLevelInfo = this.m_Values[productionCompanyInfo.m_Level - 1];
          // ISSUE: reference to a compiler-generated field
          if (productionCompanyInfo.m_Industrial)
          {
            // ISSUE: reference to a compiler-generated field
            ++productionLevelInfo.m_IndustrialCompanies;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            productionLevelInfo.m_IndustrialWorkers += productionCompanyInfo.m_Workers;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            ++productionLevelInfo.m_CommercialCompanies;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            productionLevelInfo.m_CommercialWorkers += productionCompanyInfo.m_Workers;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Values[productionCompanyInfo.m_Level - 1] = productionLevelInfo;
        }
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_CachedValues.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionLevelInfo cachedValue = this.m_CachedValues[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionLevelInfo productionLevelInfo = this.m_Values[index];
        // ISSUE: reference to a compiler-generated field
        this.m_CachedValues[index] = productionLevelInfo;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (cachedValue.m_IndustrialCompanies != productionLevelInfo.m_IndustrialCompanies)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.Patch(index, "industrialCompanies", productionLevelInfo.m_IndustrialCompanies);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (cachedValue.m_IndustrialWorkers != productionLevelInfo.m_IndustrialWorkers)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.Patch(index, "industrialWorkers", productionLevelInfo.m_IndustrialWorkers);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (cachedValue.m_CommercialCompanies != productionLevelInfo.m_CommercialCompanies)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.Patch(index, "commercialCompanies", productionLevelInfo.m_CommercialCompanies);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (cachedValue.m_CommercialWorkers != productionLevelInfo.m_CommercialWorkers)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.Patch(index, "commercialWorkers", productionLevelInfo.m_CommercialWorkers);
        }
      }
    }

    private void Patch(int index, string fieldName, int value)
    {
      // ISSUE: reference to a compiler-generated field
      IJsonWriter jsonWriter = this.m_ProductionCompanyInfoBinding.PatchBegin();
      jsonWriter.ArrayBegin(2U);
      jsonWriter.Write(index);
      jsonWriter.Write(fieldName);
      jsonWriter.ArrayEnd();
      jsonWriter.Write(value);
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCompanyInfoBinding.PatchEnd();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated method
      this.RebuildResourceIDMap();
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < this.m_CachedValues.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        ref NativeArray<ProductionCompanyUISystem.ProductionLevelInfo> local = ref this.m_CachedValues;
        int index2 = index1;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionLevelInfo productionLevelInfo1 = new ProductionCompanyUISystem.ProductionLevelInfo();
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionLevelInfo productionLevelInfo2 = productionLevelInfo1;
        local[index2] = productionLevelInfo2;
      }
      // ISSUE: reference to a compiler-generated field
      for (int index3 = 0; index3 < this.m_Values.Length; ++index3)
      {
        // ISSUE: reference to a compiler-generated field
        ref NativeArray<ProductionCompanyUISystem.ProductionLevelInfo> local = ref this.m_Values;
        int index4 = index3;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionLevelInfo productionLevelInfo3 = new ProductionCompanyUISystem.ProductionLevelInfo();
        // ISSUE: variable of a compiler-generated type
        ProductionCompanyUISystem.ProductionLevelInfo productionLevelInfo4 = productionLevelInfo3;
        local[index4] = productionLevelInfo4;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCompanyInfoQueue.Clear();
    }

    private void RebuildResourceIDMap()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceIDMap.Clear();
      ResourceIterator iterator = ResourceIterator.GetIterator();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      while (iterator.Next())
      {
        Entity entity = prefabs[iterator.resource];
        if (entity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ResourceIDMap[this.m_PrefabSystem.GetPrefab<ResourcePrefab>(entity).name] = iterator.resource;
        }
      }
    }

    private void OnSelectResource(string resourceID)
    {
      Resource resource;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ResourceIDMap.TryGetValue(resourceID, out resource))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedResource = resource;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedResource = Resource.NoResource;
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
    public ProductionCompanyUISystem()
    {
    }

    [BurstCompile]
    private struct MapCompanyStatisticsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<IndustrialCompany> m_IndustrialCompanyType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public Resource m_Resource;
      public NativeQueue<ProductionCompanyUISystem.ProductionCompanyInfo>.ParallelWriter m_Queue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<IndustrialCompany>(ref this.m_IndustrialCompanyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray1 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_IndustrialProcessDatas[nativeArray2[index].m_Prefab].m_Output.m_Resource == this.m_Resource && this.m_PrefabRefs.HasComponent(nativeArray1[index].m_Property))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            SpawnableBuildingData spawnableBuildingData = this.m_SpawnableBuildingDatas[this.m_PrefabRefs[nativeArray1[index].m_Property].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_Queue.Enqueue(new ProductionCompanyUISystem.ProductionCompanyInfo()
            {
              m_Industrial = flag,
              m_Level = (int) spawnableBuildingData.m_Level,
              m_Workers = bufferAccessor[index].Length
            });
          }
        }
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

    private struct ProductionCompanyInfo
    {
      public bool m_Industrial;
      public int m_Level;
      public int m_Workers;
    }

    private struct ProductionLevelInfo : IEquatable<ProductionCompanyUISystem.ProductionLevelInfo>
    {
      public int m_IndustrialCompanies;
      public int m_IndustrialWorkers;
      public int m_CommercialCompanies;
      public int m_CommercialWorkers;

      public bool Equals(
        ProductionCompanyUISystem.ProductionLevelInfo other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return other.m_IndustrialCompanies == this.m_IndustrialCompanies && other.m_IndustrialWorkers == this.m_IndustrialWorkers && other.m_CommercialCompanies == this.m_CommercialCompanies && other.m_CommercialWorkers == this.m_CommercialWorkers;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<IndustrialCompany> __Game_Companies_IndustrialCompany_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_IndustrialCompany_RO_ComponentTypeHandle = state.GetComponentTypeHandle<IndustrialCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
      }
    }
  }
}
