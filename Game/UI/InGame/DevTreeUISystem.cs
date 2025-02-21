// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DevTreeUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System;
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
  public class DevTreeUISystem : UISystemBase
  {
    private const string kGroup = "devTree";
    private PrefabSystem m_PrefabSystem;
    private PrefabUISystem m_PrefabUISystem;
    private DevTreeSystem m_DevTreeSystem;
    private ImageSystem m_ImageSystem;
    private EntityQuery m_DevTreePointsQuery;
    private EntityQuery m_DevTreeNodeQuery;
    private EntityQuery m_UnlockedServiceQuery;
    private EntityQuery m_ModifiedDevTreeNodeQuery;
    private EntityQuery m_LockedDevTreeNodeQuery;
    private GetterValueBinding<int> m_PointsBinding;
    private RawValueBinding m_ServicesBinding;
    private RawMapBinding<Entity> m_ServiceDetailsBinding;
    private RawMapBinding<Entity> m_NodesBinding;
    private RawMapBinding<Entity> m_NodeDetailsBinding;
    private DevTreeUISystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DevTreeSystem = this.World.GetOrCreateSystemManaged<DevTreeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DevTreePointsQuery = this.GetEntityQuery(ComponentType.ReadOnly<DevTreePoints>());
      // ISSUE: reference to a compiler-generated field
      this.m_DevTreeNodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<DevTreeNodeData>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedServiceQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedDevTreeNodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<DevTreeNodeData>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LockedDevTreeNodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<DevTreeNodeData>(),
          ComponentType.ReadOnly<Locked>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_PointsBinding = new GetterValueBinding<int>("devTree", "points", (Func<int>) (() => Mathf.Min(!this.m_DevTreePointsQuery.IsEmptyIgnoreFilter ? this.m_DevTreePointsQuery.GetSingleton<DevTreePoints>().m_Points : 0, this.GetMaxDevTreePoints())))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ServicesBinding = new RawValueBinding("devTree", "services", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated method
        NativeList<UIObjectInfo> sortedDevTreeServices = this.GetSortedDevTreeServices(Allocator.TempJob);
        writer.ArrayBegin(sortedDevTreeServices.Length);
        for (int index = 0; index < sortedDevTreeServices.Length; ++index)
        {
          UIObjectInfo uiObjectInfo = sortedDevTreeServices[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(uiObjectInfo.prefabData);
          writer.TypeBegin("devTree.Service");
          writer.PropertyName("entity");
          writer.Write(uiObjectInfo.entity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
          writer.PropertyName("locked");
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(uiObjectInfo.entity));
          writer.PropertyName("uiTag");
          writer.Write(prefab.uiTag);
          writer.PropertyName("requirements");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PrefabUISystem.BindPrefabRequirements(writer, uiObjectInfo.entity);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        sortedDevTreeServices.Dispose();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ServiceDetailsBinding = new RawMapBinding<Entity>("devTree", "serviceDetails", (Action<IJsonWriter, Entity>) ((binder, service) =>
      {
        Entity entity = service;
        PrefabData component;
        if (entity != Entity.Null && this.EntityManager.HasComponent<ServiceData>(entity) && this.EntityManager.TryGetComponent<PrefabData>(entity, out component))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(component);
          binder.TypeBegin("devTree.ServiceDetails");
          binder.PropertyName("entity");
          binder.Write(service);
          binder.PropertyName("name");
          binder.Write(prefab.name);
          binder.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          binder.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
          binder.PropertyName("locked");
          binder.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
          binder.PropertyName("milestoneRequirement");
          binder.Write(ProgressionUtils.GetRequiredMilestone(this.EntityManager, entity));
          binder.TypeEnd();
        }
        else
          binder.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_NodesBinding = new RawMapBinding<Entity>("devTree", "nodes", (Action<IJsonWriter, Entity>) ((binder, service) =>
      {
        Entity service1 = service;
        if (service1 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          NativeList<DevTreeUISystem.DevTreeNodeInfo> devTreeNodes = this.GetDevTreeNodes(service1, Allocator.TempJob);
          binder.ArrayBegin(devTreeNodes.Length);
          for (int index1 = 0; index1 < devTreeNodes.Length; ++index1)
          {
            // ISSUE: variable of a compiler-generated type
            DevTreeUISystem.DevTreeNodeInfo devTreeNodeInfo = devTreeNodes[index1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            DevTreeNodePrefab prefab = this.m_PrefabSystem.GetPrefab<DevTreeNodePrefab>(devTreeNodeInfo.prefabData);
            binder.TypeBegin("devTree.Node");
            binder.PropertyName("entity");
            // ISSUE: reference to a compiler-generated field
            binder.Write(devTreeNodeInfo.entity);
            binder.PropertyName("name");
            binder.Write(prefab.name);
            binder.PropertyName("icon");
            // ISSUE: reference to a compiler-generated method
            binder.Write(this.GetDevTreeIcon(prefab));
            binder.PropertyName("cost");
            // ISSUE: reference to a compiler-generated field
            binder.Write(devTreeNodeInfo.devTreeNodeData.m_Cost);
            binder.PropertyName("locked");
            // ISSUE: reference to a compiler-generated field
            binder.Write(devTreeNodeInfo.locked);
            binder.PropertyName("position");
            binder.Write(new float2((float) prefab.m_HorizontalPosition, prefab.m_VerticalPosition));
            DynamicBuffer<DevTreeNodeRequirement> buffer;
            // ISSUE: reference to a compiler-generated field
            if (this.EntityManager.TryGetBuffer<DevTreeNodeRequirement>(devTreeNodeInfo.entity, true, out buffer))
            {
              bool flag = buffer.Length > 0;
              binder.PropertyName("requirements");
              binder.ArrayBegin(buffer.Length);
              for (int index2 = 0; index2 < buffer.Length; ++index2)
              {
                binder.Write(buffer[index2].m_Node);
                if (this.EntityManager.HasEnabledComponent<Locked>(buffer[index2].m_Node))
                  flag = false;
              }
              binder.ArrayEnd();
              binder.PropertyName("unlockable");
              binder.Write(flag);
            }
            else
            {
              binder.PropertyName("requirements");
              binder.WriteEmptyArray();
              binder.PropertyName("unlockable");
              // ISSUE: reference to a compiler-generated field
              binder.Write(!devTreeNodeInfo.locked);
            }
            binder.TypeEnd();
          }
          binder.ArrayEnd();
          devTreeNodes.Dispose();
        }
        else
          binder.WriteEmptyArray();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_NodeDetailsBinding = new RawMapBinding<Entity>("devTree", "nodeDetails", (Action<IJsonWriter, Entity>) ((binder, node) =>
      {
        Entity entity = node;
        DevTreeNodeData component1;
        PrefabData component2;
        if (entity != Entity.Null && this.EntityManager.TryGetComponent<DevTreeNodeData>(entity, out component1) && this.EntityManager.TryGetComponent<PrefabData>(entity, out component2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          DevTreeNodePrefab prefab = this.m_PrefabSystem.GetPrefab<DevTreeNodePrefab>(component2);
          bool flag1 = this.EntityManager.HasEnabledComponent<Locked>(entity);
          binder.TypeBegin("devTree.NodeDetails");
          binder.PropertyName("entity");
          binder.Write(node);
          binder.PropertyName("name");
          binder.Write(prefab.name);
          binder.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          binder.Write(this.GetDevTreeIcon(prefab));
          binder.PropertyName("cost");
          binder.Write(component1.m_Cost);
          binder.PropertyName("locked");
          binder.Write(flag1);
          int num = 0;
          bool flag2 = false;
          DynamicBuffer<DevTreeNodeRequirement> buffer;
          if (this.EntityManager.TryGetBuffer<DevTreeNodeRequirement>(entity, true, out buffer))
          {
            num = buffer.Length;
            flag2 = buffer.Length > 0;
            for (int index = 0; index < buffer.Length; ++index)
            {
              if (this.EntityManager.HasEnabledComponent<Locked>(buffer[index].m_Node))
                flag2 = false;
            }
          }
          binder.PropertyName("unlockable");
          binder.Write(flag2);
          binder.PropertyName("requirementCount");
          binder.Write(num);
          binder.PropertyName("milestoneRequirement");
          binder.Write(ProgressionUtils.GetRequiredMilestone(this.EntityManager, component1.m_Service));
          binder.TypeEnd();
        }
        else
          binder.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<Entity>("devTree", "purchaseNode", (Action<Entity>) (node => this.m_DevTreeSystem.Purchase(node))));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PointsBinding.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ModifiedDevTreeNodeQuery.IsEmptyIgnoreFilter || PrefabUtils.HasUnlockedPrefab<DevTreeNodeData>(this.EntityManager, this.m_UnlockedServiceQuery))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NodesBinding.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        this.m_NodeDetailsBinding.UpdateAll();
      }
      // ISSUE: reference to a compiler-generated field
      if (!PrefabUtils.HasUnlockedPrefab<ServiceData>(this.EntityManager, this.m_UnlockedServiceQuery))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ServicesBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceDetailsBinding.UpdateAll();
    }

    private void PurchaseNode(Entity node) => this.m_DevTreeSystem.Purchase(node);

    private int GetDevTreePoints()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return Mathf.Min(!this.m_DevTreePointsQuery.IsEmptyIgnoreFilter ? this.m_DevTreePointsQuery.GetSingleton<DevTreePoints>().m_Points : 0, this.GetMaxDevTreePoints());
    }

    private int GetMaxDevTreePoints()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<DevTreeNodeData> componentDataArray = this.m_LockedDevTreeNodeQuery.ToComponentDataArray<DevTreeNodeData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      int maxDevTreePoints = 0;
      foreach (DevTreeNodeData devTreeNodeData in componentDataArray)
        maxDevTreePoints += devTreeNodeData.m_Cost;
      componentDataArray.Dispose();
      return maxDevTreePoints;
    }

    private void BindServices(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      NativeList<UIObjectInfo> sortedDevTreeServices = this.GetSortedDevTreeServices(Allocator.TempJob);
      writer.ArrayBegin(sortedDevTreeServices.Length);
      for (int index = 0; index < sortedDevTreeServices.Length; ++index)
      {
        UIObjectInfo uiObjectInfo = sortedDevTreeServices[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(uiObjectInfo.prefabData);
        writer.TypeBegin("devTree.Service");
        writer.PropertyName("entity");
        writer.Write(uiObjectInfo.entity);
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        writer.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
        writer.PropertyName("locked");
        writer.Write(this.EntityManager.HasEnabledComponent<Locked>(uiObjectInfo.entity));
        writer.PropertyName("uiTag");
        writer.Write(prefab.uiTag);
        writer.PropertyName("requirements");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabUISystem.BindPrefabRequirements(writer, uiObjectInfo.entity);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
      sortedDevTreeServices.Dispose();
    }

    private NativeList<UIObjectInfo> GetSortedDevTreeServices(Allocator allocator)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<DevTreeNodeData> componentDataArray = this.m_DevTreeNodeQuery.ToComponentDataArray<DevTreeNodeData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<UIObjectInfo> list = new NativeList<UIObjectInfo>(16, (AllocatorManager.AllocatorHandle) allocator);
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        Entity service = componentDataArray[index].m_Service;
        UIObjectData component;
        if (nativeParallelHashSet.Add(service) && this.EntityManager.TryGetComponent<UIObjectData>(service, out component))
        {
          PrefabData componentData = this.EntityManager.GetComponentData<PrefabData>(service);
          list.Add(new UIObjectInfo(service, componentData, component.m_Priority));
        }
      }
      componentDataArray.Dispose();
      nativeParallelHashSet.Dispose();
      list.Sort<UIObjectInfo>();
      return list;
    }

    private void BindServiceDetails(IJsonWriter binder, Entity service)
    {
      Entity entity = service;
      PrefabData component;
      if (entity != Entity.Null && this.EntityManager.HasComponent<ServiceData>(entity) && this.EntityManager.TryGetComponent<PrefabData>(entity, out component))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(component);
        binder.TypeBegin("devTree.ServiceDetails");
        binder.PropertyName("entity");
        binder.Write(service);
        binder.PropertyName("name");
        binder.Write(prefab.name);
        binder.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        binder.Write(ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon);
        binder.PropertyName("locked");
        binder.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
        binder.PropertyName("milestoneRequirement");
        binder.Write(ProgressionUtils.GetRequiredMilestone(this.EntityManager, entity));
        binder.TypeEnd();
      }
      else
        binder.WriteNull();
    }

    private void BindNodes(IJsonWriter binder, Entity service)
    {
      Entity service1 = service;
      if (service1 != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        NativeList<DevTreeUISystem.DevTreeNodeInfo> devTreeNodes = this.GetDevTreeNodes(service1, Allocator.TempJob);
        binder.ArrayBegin(devTreeNodes.Length);
        for (int index1 = 0; index1 < devTreeNodes.Length; ++index1)
        {
          // ISSUE: variable of a compiler-generated type
          DevTreeUISystem.DevTreeNodeInfo devTreeNodeInfo = devTreeNodes[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          DevTreeNodePrefab prefab = this.m_PrefabSystem.GetPrefab<DevTreeNodePrefab>(devTreeNodeInfo.prefabData);
          binder.TypeBegin("devTree.Node");
          binder.PropertyName("entity");
          // ISSUE: reference to a compiler-generated field
          binder.Write(devTreeNodeInfo.entity);
          binder.PropertyName("name");
          binder.Write(prefab.name);
          binder.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          binder.Write(this.GetDevTreeIcon(prefab));
          binder.PropertyName("cost");
          // ISSUE: reference to a compiler-generated field
          binder.Write(devTreeNodeInfo.devTreeNodeData.m_Cost);
          binder.PropertyName("locked");
          // ISSUE: reference to a compiler-generated field
          binder.Write(devTreeNodeInfo.locked);
          binder.PropertyName("position");
          binder.Write(new float2((float) prefab.m_HorizontalPosition, prefab.m_VerticalPosition));
          DynamicBuffer<DevTreeNodeRequirement> buffer;
          // ISSUE: reference to a compiler-generated field
          if (this.EntityManager.TryGetBuffer<DevTreeNodeRequirement>(devTreeNodeInfo.entity, true, out buffer))
          {
            bool flag = buffer.Length > 0;
            binder.PropertyName("requirements");
            binder.ArrayBegin(buffer.Length);
            for (int index2 = 0; index2 < buffer.Length; ++index2)
            {
              binder.Write(buffer[index2].m_Node);
              if (this.EntityManager.HasEnabledComponent<Locked>(buffer[index2].m_Node))
                flag = false;
            }
            binder.ArrayEnd();
            binder.PropertyName("unlockable");
            binder.Write(flag);
          }
          else
          {
            binder.PropertyName("requirements");
            binder.WriteEmptyArray();
            binder.PropertyName("unlockable");
            // ISSUE: reference to a compiler-generated field
            binder.Write(!devTreeNodeInfo.locked);
          }
          binder.TypeEnd();
        }
        binder.ArrayEnd();
        devTreeNodes.Dispose();
      }
      else
        binder.WriteEmptyArray();
    }

    private NativeList<DevTreeUISystem.DevTreeNodeInfo> GetDevTreeNodes(
      Entity service,
      Allocator allocator)
    {
      NativeList<DevTreeUISystem.DevTreeNodeInfo> devTreeNodes = new NativeList<DevTreeUISystem.DevTreeNodeInfo>(16, (AllocatorManager.AllocatorHandle) allocator);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DevTreeNodeData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<DevTreeNodeData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_DevTreeNodeData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Locked> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_DevTreeNodeQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
          NativeArray<DevTreeNodeData> nativeArray3 = archetypeChunk.GetNativeArray<DevTreeNodeData>(ref componentTypeHandle2);
          EnabledMask enabledMask = archetypeChunk.GetEnabledMask<Locked>(ref componentTypeHandle3);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            if (nativeArray3[index2].m_Service == service)
            {
              // ISSUE: object of a compiler-generated type is created
              devTreeNodes.Add(new DevTreeUISystem.DevTreeNodeInfo()
              {
                entity = nativeArray1[index2],
                prefabData = nativeArray2[index2],
                devTreeNodeData = nativeArray3[index2],
                locked = enabledMask.EnableBit.IsValid && enabledMask[index2]
              });
            }
          }
        }
      }
      return devTreeNodes;
    }

    private void BindNodeDetails(IJsonWriter binder, Entity node)
    {
      Entity entity = node;
      DevTreeNodeData component1;
      PrefabData component2;
      if (entity != Entity.Null && this.EntityManager.TryGetComponent<DevTreeNodeData>(entity, out component1) && this.EntityManager.TryGetComponent<PrefabData>(entity, out component2))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        DevTreeNodePrefab prefab = this.m_PrefabSystem.GetPrefab<DevTreeNodePrefab>(component2);
        bool flag1 = this.EntityManager.HasEnabledComponent<Locked>(entity);
        binder.TypeBegin("devTree.NodeDetails");
        binder.PropertyName("entity");
        binder.Write(node);
        binder.PropertyName("name");
        binder.Write(prefab.name);
        binder.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        binder.Write(this.GetDevTreeIcon(prefab));
        binder.PropertyName("cost");
        binder.Write(component1.m_Cost);
        binder.PropertyName("locked");
        binder.Write(flag1);
        int num = 0;
        bool flag2 = false;
        DynamicBuffer<DevTreeNodeRequirement> buffer;
        if (this.EntityManager.TryGetBuffer<DevTreeNodeRequirement>(entity, true, out buffer))
        {
          num = buffer.Length;
          flag2 = buffer.Length > 0;
          for (int index = 0; index < buffer.Length; ++index)
          {
            if (this.EntityManager.HasEnabledComponent<Locked>(buffer[index].m_Node))
              flag2 = false;
          }
        }
        binder.PropertyName("unlockable");
        binder.Write(flag2);
        binder.PropertyName("requirementCount");
        binder.Write(num);
        binder.PropertyName("milestoneRequirement");
        binder.Write(ProgressionUtils.GetRequiredMilestone(this.EntityManager, component1.m_Service));
        binder.TypeEnd();
      }
      else
        binder.WriteNull();
    }

    private string GetDevTreeIcon(DevTreeNodePrefab prefab)
    {
      if (!string.IsNullOrEmpty(prefab.m_IconPath))
        return prefab.m_IconPath;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return (UnityEngine.Object) prefab.m_IconPrefab != (UnityEngine.Object) null ? ImageSystem.GetThumbnail(prefab.m_IconPrefab) ?? this.m_ImageSystem.placeholderIcon : this.m_ImageSystem.placeholderIcon;
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
    public DevTreeUISystem()
    {
    }

    private struct DevTreeNodeInfo
    {
      public Entity entity;
      public PrefabData prefabData;
      public DevTreeNodeData devTreeNodeData;
      public bool locked;
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<DevTreeNodeData> __Game_Prefabs_DevTreeNodeData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DevTreeNodeData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DevTreeNodeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>(true);
      }
    }
  }
}
