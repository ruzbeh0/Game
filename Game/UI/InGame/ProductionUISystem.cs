// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ProductionUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ProductionUISystem : UISystemBase
  {
    private const string kGroup = "production";
    private UIUpdateState m_UpdateState;
    private PrefabSystem m_PrefabSystem;
    private ResourceSystem m_ResourceSystem;
    private CommercialDemandSystem m_CommercialDemandSystem;
    private IndustrialDemandSystem m_IndustrialDemandSystem;
    private CountCompanyDataSystem m_CountCompanyDataSystem;
    private EntityQuery m_ResourceCategoryQuery;
    private EntityQuery m_IndustrialCompanyQuery;
    private EntityQuery m_CommercialCompanyQuery;
    private EntityQuery m_ServiceUpkeepQuery;
    private NativeParallelMultiHashMap<Entity, (Entity, Entity)> m_ProductionChain;
    private GetterValueBinding<int> m_MaxProgressBinding;
    private RawValueBinding m_ResourceCategoriesBinding;
    private RawMapBinding<Entity> m_ResourceDetailsBinding;
    private RawMapBinding<Entity> m_ResourceBinding;
    private RawMapBinding<Entity> m_ServiceBinding;
    private RawMapBinding<Entity> m_DataBinding;
    private NativeList<int> m_ProductionCache;
    private NativeList<int> m_CommercialConsumptionCache;
    private NativeList<int> m_IndustrialConsumptionCache;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateState = UIUpdateState.Create(this.World, 256);
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialDemandSystem = this.World.GetOrCreateSystemManaged<CommercialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandSystem = this.World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountCompanyDataSystem = this.World.GetOrCreateSystemManaged<CountCompanyDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceCategoryQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<UIGroupElement>(), ComponentType.ReadOnly<UIResourceCategoryData>(), ComponentType.ReadOnly<UIObjectData>());
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.Exclude<ServiceCompanyData>(), ComponentType.Exclude<StorageCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialCompanyQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.ReadOnly<ServiceCompanyData>(), ComponentType.Exclude<StorageCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceUpkeepQuery = this.GetEntityQuery(ComponentType.ReadWrite<ServiceUpkeepData>(), ComponentType.ReadOnly<ServiceObjectData>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MaxProgressBinding = new GetterValueBinding<int>("production", "maxProgress", (Func<int>) (() =>
      {
        int x2 = 0;
        ResourceIterator iterator = ResourceIterator.GetIterator();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
        while (iterator.Next())
        {
          Entity entity = prefabs[iterator.resource];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ResourcePrefab prefab = this.m_PrefabSystem.GetPrefab<ResourcePrefab>(entity);
          if (prefab.m_IsLeisure || prefab.m_IsMaterial || prefab.m_IsProduceable)
          {
            // ISSUE: reference to a compiler-generated method
            (int x3, int num3, int num4) = this.GetData(entity);
            x2 = math.max(x2, math.max(x3, num3 + num4));
          }
        }
        return x2;
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResourceCategoriesBinding = new RawValueBinding("production", "resourceCategories", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        using (NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.m_ResourceCategoryQuery, Allocator.TempJob))
        {
          writer.ArrayBegin(sortedObjects.Length);
          for (int index1 = 0; index1 < sortedObjects.Length; ++index1)
          {
            UIObjectInfo uiObjectInfo = sortedObjects[index1];
            Entity entity1 = uiObjectInfo.entity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            PrefabSystem prefabSystem = this.m_PrefabSystem;
            uiObjectInfo = sortedObjects[index1];
            PrefabData prefabData = uiObjectInfo.prefabData;
            // ISSUE: reference to a compiler-generated method
            UIResourceCategoryPrefab prefab = prefabSystem.GetPrefab<UIResourceCategoryPrefab>(prefabData);
            NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, this.EntityManager.GetBuffer<UIGroupElement>(entity1, true), Allocator.TempJob);
            objects.Sort<UIObjectInfo>();
            try
            {
              writer.TypeBegin("production.ResourceCategory");
              writer.PropertyName("entity");
              writer.Write(entity1);
              writer.PropertyName("name");
              writer.Write(prefab.name);
              writer.PropertyName("resources");
              writer.ArrayBegin(objects.Length);
              for (int index2 = 0; index2 < objects.Length; ++index2)
              {
                IJsonWriter writer1 = writer;
                uiObjectInfo = objects[index2];
                Entity entity2 = uiObjectInfo.entity;
                // ISSUE: reference to a compiler-generated method
                this.WriteResource(writer1, entity2);
              }
              writer.ArrayEnd();
              writer.TypeEnd();
            }
            finally
            {
              objects.Dispose();
            }
          }
          writer.ArrayEnd();
        }
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResourceBinding = new RawMapBinding<Entity>("production", "resources", (Action<IJsonWriter, Entity>) ((writer, entity) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefab prefab = this.m_PrefabSystem.GetPrefab<ResourcePrefab>(entity);
        UIProductionLinks component = prefab.GetComponent<UIProductionLinks>();
        Resource resource = EconomyUtils.GetResource(prefab.m_Resource);
        try
        {
          writer.TypeBegin("production.Resource");
          writer.PropertyName(nameof (entity));
          writer.Write(entity);
          writer.PropertyName("name");
          writer.Write(resource.ToString());
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          writer.Write(ImageSystem.GetIcon((PrefabBase) prefab));
          writer.PropertyName("tradable");
          writer.Write(prefab.m_IsTradable);
          writer.PropertyName("producer");
          // ISSUE: reference to a compiler-generated method
          this.WriteProductionLink(writer, component.m_Producer);
          writer.PropertyName("consumers");
          if (component.m_FinalConsumers != null)
          {
            writer.ArrayBegin(component.m_FinalConsumers.Length);
            for (int index = 0; index < component.m_FinalConsumers.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.WriteProductionLink(writer, component.m_FinalConsumers[index]);
            }
            writer.ArrayEnd();
          }
          else
            writer.WriteEmptyArray();
          writer.TypeEnd();
        }
        catch (Exception ex)
        {
          writer.WriteNull();
          Debug.LogError((object) ex);
        }
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResourceDetailsBinding = new RawMapBinding<Entity>("production", "resourceDetails", (Action<IJsonWriter, Entity>) ((writer, entity) =>
      {
        NativeList<Entity> outputs1 = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<Entity> outputs2 = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeKeyValueArrays<Entity, (Entity, Entity)> keyValueArrays = this.m_ProductionChain.GetKeyValueArrays((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_ServiceUpkeepQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        try
        {
          writer.TypeBegin("production.ResourceDetails");
          writer.PropertyName("inputs");
          // ISSUE: reference to a compiler-generated field
          writer.ArrayBegin(this.m_ProductionChain.CountValuesForKey(entity));
          // ISSUE: reference to a compiler-generated field
          foreach ((Entity, Entity) valueTuple in this.m_ProductionChain.GetValuesForKey(entity))
          {
            int size = 0;
            if (valueTuple.Item1 != Entity.Null)
              ++size;
            if (valueTuple.Item2 != Entity.Null)
              ++size;
            writer.ArrayBegin(size);
            if (valueTuple.Item1 != Entity.Null)
              writer.Write(valueTuple.Item1);
            if (valueTuple.Item2 != Entity.Null)
              writer.Write(valueTuple.Item2);
            writer.ArrayEnd();
          }
          writer.ArrayEnd();
          // ISSUE: reference to a compiler-generated method
          this.FindOutputs(entity, outputs1, keyValueArrays);
          writer.PropertyName("outputs");
          writer.ArrayBegin(outputs1.Length);
          for (int index = 0; index < outputs1.Length; ++index)
            writer.Write(outputs1[index]);
          writer.ArrayEnd();
          // ISSUE: reference to a compiler-generated method
          this.FindServiceOutputs(entity, outputs2, entityArray);
          writer.PropertyName("serviceOutputs");
          writer.ArrayBegin(outputs2.Length);
          for (int index = 0; index < outputs2.Length; ++index)
            writer.Write(outputs2[index]);
          writer.ArrayEnd();
          writer.TypeEnd();
        }
        finally
        {
          keyValueArrays.Dispose();
          outputs1.Dispose();
          outputs2.Dispose();
          entityArray.Dispose();
        }
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ServiceBinding = new RawMapBinding<Entity>("production", "services", (Action<IJsonWriter, Entity>) ((writer, entity) =>
      {
        PrefabData component;
        if (this.EntityManager.TryGetComponent<PrefabData>(entity, out component))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(component);
          writer.TypeBegin("production.Service");
          writer.PropertyName(nameof (entity));
          writer.Write(entity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          writer.Write(ImageSystem.GetIcon((PrefabBase) prefab));
          writer.TypeEnd();
        }
        else
          writer.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_DataBinding = new RawMapBinding<Entity>("production", "data", (Action<IJsonWriter, Entity>) ((writer, entity) =>
      {
        // ISSUE: reference to a compiler-generated method
        (int num8, int num9, int num10) = this.GetData(entity);
        writer.TypeBegin("production.ResourceData");
        writer.PropertyName("production");
        writer.Write(num8);
        writer.PropertyName("surplus");
        writer.Write(num9);
        writer.PropertyName("deficit");
        writer.Write(num10);
        writer.TypeEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionChain = new NativeParallelMultiHashMap<Entity, (Entity, Entity)>(50, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCache = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialConsumptionCache = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialConsumptionCache = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      if (GameManager.instance.gameMode != GameMode.Game)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.BuildProductionChain(this.m_ProductionChain);
      // ISSUE: reference to a compiler-generated method
      this.UpdateCache();
      // ISSUE: reference to a compiler-generated field
      this.m_MaxProgressBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceCategoriesBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceDetailsBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceBinding.Update();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionChain.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCache.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialConsumptionCache.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialConsumptionCache.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated field
      if (GameManager.instance.gameMode != GameMode.Game || !this.m_UpdateState.Advance())
        return;
      // ISSUE: reference to a compiler-generated method
      this.UpdateCache();
      // ISSUE: reference to a compiler-generated field
      this.m_DataBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_MaxProgressBinding.Update();
    }

    private void UpdateCache()
    {
      JobHandle deps1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> other1 = this.m_CountCompanyDataSystem.GetProduction(out deps1);
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      CountCompanyDataSystem.CommercialCompanyDatas commercialCompanyDatas = this.m_CountCompanyDataSystem.GetCommercialCompanyDatas(out deps2);
      JobHandle deps3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> other2 = this.m_IndustrialDemandSystem.GetConsumption(out deps3);
      JobHandle deps4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> other3 = this.m_CommercialDemandSystem.GetConsumption(out deps4);
      JobHandle.CompleteAll(ref deps1, ref deps3, ref deps4);
      deps2.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCache.CopyFrom(in other1);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialConsumptionCache.CopyFrom(in other2);
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialConsumptionCache.CopyFrom(in other3);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ProductionCache.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.EntityManager.GetComponentData<ResourceData>(this.m_ResourceSystem.GetPrefabs()[EconomyUtils.GetResource(index)]).m_IsProduceable)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ProductionCache[index] = commercialCompanyDatas.m_ProduceCapacity[index];
        }
      }
    }

    private int GetMaxProgress()
    {
      int x1 = 0;
      ResourceIterator iterator = ResourceIterator.GetIterator();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      while (iterator.Next())
      {
        Entity entity = prefabs[iterator.resource];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefab prefab = this.m_PrefabSystem.GetPrefab<ResourcePrefab>(entity);
        if (prefab.m_IsLeisure || prefab.m_IsMaterial || prefab.m_IsProduceable)
        {
          // ISSUE: reference to a compiler-generated method
          (int x2, int num1, int num2) = this.GetData(entity);
          x1 = math.max(x1, math.max(x2, num1 + num2));
        }
      }
      return x1;
    }

    private void WriteResourceCategories(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.m_ResourceCategoryQuery, Allocator.TempJob))
      {
        writer.ArrayBegin(sortedObjects.Length);
        for (int index1 = 0; index1 < sortedObjects.Length; ++index1)
        {
          UIObjectInfo uiObjectInfo = sortedObjects[index1];
          Entity entity1 = uiObjectInfo.entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PrefabSystem prefabSystem = this.m_PrefabSystem;
          uiObjectInfo = sortedObjects[index1];
          PrefabData prefabData = uiObjectInfo.prefabData;
          // ISSUE: reference to a compiler-generated method
          UIResourceCategoryPrefab prefab = prefabSystem.GetPrefab<UIResourceCategoryPrefab>(prefabData);
          NativeList<UIObjectInfo> objects = UIObjectInfo.GetObjects(this.EntityManager, this.EntityManager.GetBuffer<UIGroupElement>(entity1, true), Allocator.TempJob);
          objects.Sort<UIObjectInfo>();
          try
          {
            writer.TypeBegin("production.ResourceCategory");
            writer.PropertyName("entity");
            writer.Write(entity1);
            writer.PropertyName("name");
            writer.Write(prefab.name);
            writer.PropertyName("resources");
            writer.ArrayBegin(objects.Length);
            for (int index2 = 0; index2 < objects.Length; ++index2)
            {
              IJsonWriter writer1 = writer;
              uiObjectInfo = objects[index2];
              Entity entity2 = uiObjectInfo.entity;
              // ISSUE: reference to a compiler-generated method
              this.WriteResource(writer1, entity2);
            }
            writer.ArrayEnd();
            writer.TypeEnd();
          }
          finally
          {
            objects.Dispose();
          }
        }
        writer.ArrayEnd();
      }
    }

    public void WriteResource(IJsonWriter writer, Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefab prefab = this.m_PrefabSystem.GetPrefab<ResourcePrefab>(entity);
      UIProductionLinks component = prefab.GetComponent<UIProductionLinks>();
      Resource resource = EconomyUtils.GetResource(prefab.m_Resource);
      try
      {
        writer.TypeBegin("production.Resource");
        writer.PropertyName(nameof (entity));
        writer.Write(entity);
        writer.PropertyName("name");
        writer.Write(resource.ToString());
        writer.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        writer.Write(ImageSystem.GetIcon((PrefabBase) prefab));
        writer.PropertyName("tradable");
        writer.Write(prefab.m_IsTradable);
        writer.PropertyName("producer");
        // ISSUE: reference to a compiler-generated method
        this.WriteProductionLink(writer, component.m_Producer);
        writer.PropertyName("consumers");
        if (component.m_FinalConsumers != null)
        {
          writer.ArrayBegin(component.m_FinalConsumers.Length);
          for (int index = 0; index < component.m_FinalConsumers.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.WriteProductionLink(writer, component.m_FinalConsumers[index]);
          }
          writer.ArrayEnd();
        }
        else
          writer.WriteEmptyArray();
        writer.TypeEnd();
      }
      catch (Exception ex)
      {
        writer.WriteNull();
        Debug.LogError((object) ex);
      }
    }

    private void WriteProductionLink(IJsonWriter writer, UIProductionLinkPrefab prefab)
    {
      writer.TypeBegin("ProductionLink");
      writer.PropertyName("name");
      writer.Write(prefab.m_Type.ToString());
      writer.PropertyName("icon");
      writer.Write(prefab.m_Icon);
      writer.TypeEnd();
    }

    private void WriteResourceDetails(IJsonWriter writer, Entity entity)
    {
      NativeList<Entity> outputs1 = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> outputs2 = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeKeyValueArrays<Entity, (Entity, Entity)> keyValueArrays = this.m_ProductionChain.GetKeyValueArrays((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ServiceUpkeepQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        writer.TypeBegin("production.ResourceDetails");
        writer.PropertyName("inputs");
        // ISSUE: reference to a compiler-generated field
        writer.ArrayBegin(this.m_ProductionChain.CountValuesForKey(entity));
        // ISSUE: reference to a compiler-generated field
        foreach ((Entity, Entity) tuple in this.m_ProductionChain.GetValuesForKey(entity))
        {
          int size = 0;
          if (tuple.Item1 != Entity.Null)
            ++size;
          if (tuple.Item2 != Entity.Null)
            ++size;
          writer.ArrayBegin(size);
          if (tuple.Item1 != Entity.Null)
            writer.Write(tuple.Item1);
          if (tuple.Item2 != Entity.Null)
            writer.Write(tuple.Item2);
          writer.ArrayEnd();
        }
        writer.ArrayEnd();
        // ISSUE: reference to a compiler-generated method
        this.FindOutputs(entity, outputs1, keyValueArrays);
        writer.PropertyName("outputs");
        writer.ArrayBegin(outputs1.Length);
        for (int index = 0; index < outputs1.Length; ++index)
          writer.Write(outputs1[index]);
        writer.ArrayEnd();
        // ISSUE: reference to a compiler-generated method
        this.FindServiceOutputs(entity, outputs2, entityArray);
        writer.PropertyName("serviceOutputs");
        writer.ArrayBegin(outputs2.Length);
        for (int index = 0; index < outputs2.Length; ++index)
          writer.Write(outputs2[index]);
        writer.ArrayEnd();
        writer.TypeEnd();
      }
      finally
      {
        keyValueArrays.Dispose();
        outputs1.Dispose();
        outputs2.Dispose();
        entityArray.Dispose();
      }
    }

    private void FindServiceOutputs(
      Entity entity,
      NativeList<Entity> outputs,
      NativeArray<Entity> serviceUpkeeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      for (int index1 = 0; index1 < serviceUpkeeps.Length; ++index1)
      {
        Entity serviceUpkeep = serviceUpkeeps[index1];
        EntityManager entityManager = this.EntityManager;
        DynamicBuffer<ServiceUpkeepData> buffer = entityManager.GetBuffer<ServiceUpkeepData>(serviceUpkeep, true);
        for (int index2 = 0; index2 < buffer.Length; ++index2)
        {
          Resource resource = buffer[index2].m_Upkeep.m_Resource;
          if (resource != Resource.NoResource)
          {
            Entity entity1 = prefabs[resource];
            if (entity == entity1)
            {
              entityManager = this.EntityManager;
              Entity service = entityManager.GetComponentData<ServiceObjectData>(serviceUpkeep).m_Service;
              if (!outputs.Contains<Entity, Entity>(service))
                outputs.Add(in service);
            }
          }
        }
      }
    }

    private void FindOutputs(
      Entity entity,
      NativeList<Entity> outputs,
      NativeKeyValueArrays<Entity, (Entity, Entity)> keyValueArrays)
    {
      for (int index = 0; index < keyValueArrays.Length; ++index)
      {
        if (keyValueArrays.Values[index].Item1 == entity || keyValueArrays.Values[index].Item2 == entity)
          outputs.Add(keyValueArrays.Keys[index]);
      }
    }

    private void WriteService(IJsonWriter writer, Entity entity)
    {
      PrefabData component;
      if (this.EntityManager.TryGetComponent<PrefabData>(entity, out component))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(component);
        writer.TypeBegin("production.Service");
        writer.PropertyName(nameof (entity));
        writer.Write(entity);
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        writer.Write(ImageSystem.GetIcon((PrefabBase) prefab));
        writer.TypeEnd();
      }
      else
        writer.WriteNull();
    }

    private void WriteData(IJsonWriter writer, Entity entity)
    {
      // ISSUE: reference to a compiler-generated method
      (int num1, int num2, int num3) = this.GetData(entity);
      writer.TypeBegin("production.ResourceData");
      writer.PropertyName("production");
      writer.Write(num1);
      writer.PropertyName("surplus");
      writer.Write(num2);
      writer.PropertyName("deficit");
      writer.Write(num3);
      writer.TypeEnd();
    }

    private (int, int, int) GetData(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int resourceIndex = EconomyUtils.GetResourceIndex(EconomyUtils.GetResource(this.m_PrefabSystem.GetPrefab<ResourcePrefab>(entity).m_Resource));
      // ISSUE: reference to a compiler-generated field
      int y1 = this.m_ProductionCache[resourceIndex];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int x = this.m_CommercialConsumptionCache[resourceIndex] + this.m_IndustrialConsumptionCache[resourceIndex];
      int y2 = math.min(x, y1);
      int num1 = math.min(x, y2);
      int num2 = y1 - y2;
      int num3 = x - num1;
      return (y1, num2, num3);
    }

    private void BuildProductionChain(
      NativeParallelMultiHashMap<Entity, (Entity, Entity)> multiHashMap)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      NativeArray<IndustrialProcessData> componentDataArray1 = this.m_IndustrialCompanyQuery.ToComponentDataArray<IndustrialProcessData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<IndustrialProcessData> componentDataArray2 = this.m_CommercialCompanyQuery.ToComponentDataArray<IndustrialProcessData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      this.ProcessProductionChainDatas(componentDataArray1, prefabs, multiHashMap);
      // ISSUE: reference to a compiler-generated method
      this.ProcessProductionChainDatas(componentDataArray2, prefabs, multiHashMap);
      componentDataArray1.Dispose();
      componentDataArray2.Dispose();
    }

    private void ProcessProductionChainDatas(
      NativeArray<IndustrialProcessData> datas,
      ResourcePrefabs resourcePrefabs,
      NativeParallelMultiHashMap<Entity, (Entity, Entity)> multiHashMap)
    {
      for (int index = 0; index < datas.Length; ++index)
      {
        IndustrialProcessData data = datas[index];
        Entity resourcePrefab = resourcePrefabs[data.m_Output.m_Resource];
        if (resourcePrefab != Entity.Null)
        {
          (Entity, Entity) valueTuple = (Entity.Null, Entity.Null);
          if (data.m_Input1.m_Resource != Resource.NoResource && data.m_Input1.m_Resource != data.m_Output.m_Resource)
            valueTuple.Item1 = resourcePrefabs[data.m_Input1.m_Resource];
          if (data.m_Input2.m_Resource != Resource.NoResource && data.m_Input2.m_Resource != data.m_Output.m_Resource)
            valueTuple.Item2 = resourcePrefabs[data.m_Input2.m_Resource];
          if (valueTuple.Item1 != Entity.Null || valueTuple.Item2 != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated method
            ProductionUISystem.TryAddUniqueValue(multiHashMap, resourcePrefab, valueTuple);
          }
        }
      }
    }

    private static void TryAddUniqueValue(
      NativeParallelMultiHashMap<Entity, (Entity, Entity)> multiHashMap,
      Entity key,
      (Entity, Entity) value)
    {
      foreach ((Entity, Entity) tuple in multiHashMap.GetValuesForKey(key))
      {
        if (tuple.Item1 == value.Item1 && tuple.Item2 == value.Item2)
          return;
      }
      multiHashMap.Add(key, value);
    }

    [Preserve]
    public ProductionUISystem()
    {
    }
  }
}
