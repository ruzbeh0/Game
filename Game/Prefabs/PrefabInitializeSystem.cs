// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PrefabInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Game.Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class PrefabInitializeSystem : GameSystemBase
  {
    private EntityQuery m_PrefabQuery;
    private PrefabSystem m_PrefabSystem;
    private PrefabInitializeSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
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
        ComponentTypeHandle<PrefabData> componentTypeHandle = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        this.CompleteDependency();
        List<PrefabInitializeSystem.ListItem> listItemList = new List<PrefabInitializeSystem.ListItem>();
        Queue<PrefabInitializeSystem.QueueItem> queue = new Queue<PrefabInitializeSystem.QueueItem>();
        HashSet<PrefabBase> prefabSet = new HashSet<PrefabBase>();
        List<PrefabBase> dependencies = new List<PrefabBase>();
        List<ComponentBase> components = new List<ComponentBase>();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(nativeArray2[index2]);
            // ISSUE: object of a compiler-generated type is created
            listItemList.Add(new PrefabInitializeSystem.ListItem(nativeArray1[index2], prefab));
            prefabSet.Add(prefab);
          }
        }
        foreach (PrefabInitializeSystem.ListItem listItem in listItemList)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.InitializePrefab(listItem.m_Entity, listItem.m_Prefab, queue, prefabSet, dependencies, components);
        }
        // ISSUE: variable of a compiler-generated type
        PrefabInitializeSystem.QueueItem queueItem;
        while (queue.TryDequeue(ref queueItem))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_PrefabSystem.AddPrefab(queueItem.m_Prefab, parentPrefab: queueItem.m_ParentPrefab, parentComponent: queueItem.m_ParentComponent))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            Entity entity = this.m_PrefabSystem.GetEntity(queueItem.m_Prefab);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.InitializePrefab(entity, queueItem.m_Prefab, queue, prefabSet, dependencies, components);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            listItemList.Add(new PrefabInitializeSystem.ListItem(entity, queueItem.m_Prefab));
          }
        }
        foreach (PrefabInitializeSystem.ListItem listItem in listItemList)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.LateInitializePrefab(listItem.m_Entity, listItem.m_Prefab, dependencies, components);
        }
      }
    }

    private void InitializePrefab(
      Entity entity,
      PrefabBase prefab,
      Queue<PrefabInitializeSystem.QueueItem> queue,
      HashSet<PrefabBase> prefabSet,
      List<PrefabBase> dependencies,
      List<ComponentBase> components)
    {
      prefab.GetComponents<ComponentBase>(components);
      for (int index = 0; index < components.Count; ++index)
      {
        ComponentBase component = components[index];
        try
        {
          component.Initialize(this.EntityManager, entity);
          component.GetDependencies(dependencies);
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, ex, "Error when initializing prefab: {0} ({1})", (object) prefab.name, (AssetData) prefab.asset != (IAssetData) null ? (object) prefab.asset : (object) "No asset");
        }
        finally
        {
          foreach (PrefabBase dependency in dependencies)
          {
            if ((UnityEngine.Object) dependency == (UnityEngine.Object) null || prefabSet.Add(dependency))
            {
              // ISSUE: object of a compiler-generated type is created
              queue.Enqueue(new PrefabInitializeSystem.QueueItem(dependency, prefab, component));
            }
          }
          dependencies.Clear();
        }
      }
      components.Clear();
    }

    private void LateInitializePrefab(
      Entity entity,
      PrefabBase prefab,
      List<PrefabBase> dependencies,
      List<ComponentBase> components)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      bool flag = this.m_PrefabSystem.IsUnlockable(prefab);
      bool unlockDependencies = prefab.canIgnoreUnlockDependencies;
      prefab.GetComponents<ComponentBase>(components);
      UnlockableBase unlockableBase = (UnlockableBase) null;
      if (flag)
        unlockableBase = prefab.GetComponent<UnlockableBase>();
      for (int index = 0; index < components.Count; ++index)
      {
        ComponentBase component = components[index];
        if ((UnityEngine.Object) component != (UnityEngine.Object) unlockableBase)
        {
          try
          {
            component.LateInitialize(this.EntityManager, entity);
            if (unlockDependencies)
            {
              if (component.ignoreUnlockDependencies)
                continue;
            }
            component.GetDependencies(dependencies);
          }
          catch (Exception ex)
          {
            COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, ex, "Error when initializing prefab: {0} ({1})", (object) prefab.name, (AssetData) prefab.asset != (IAssetData) null ? (object) prefab.asset : (object) "No asset");
          }
        }
      }
      if (flag)
      {
        try
        {
          if ((UnityEngine.Object) unlockableBase != (UnityEngine.Object) null)
            unlockableBase.LateInitialize(this.EntityManager, entity, dependencies);
          else
            UnlockableBase.DefaultLateInitialize(this.EntityManager, entity, dependencies);
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, ex, "Error when initializing prefab: {0} ({1})", (object) prefab.name, (AssetData) prefab.asset != (IAssetData) null ? (object) prefab.asset : (object) "No asset");
        }
      }
      components.Clear();
      dependencies.Clear();
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
    public PrefabInitializeSystem()
    {
    }

    private struct ListItem
    {
      public readonly Entity m_Entity;
      public readonly PrefabBase m_Prefab;

      public ListItem(Entity entity, PrefabBase prefab)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = prefab;
      }
    }

    private struct QueueItem
    {
      public readonly PrefabBase m_Prefab;
      public readonly PrefabBase m_ParentPrefab;
      public readonly ComponentBase m_ParentComponent;

      public QueueItem(PrefabBase prefab, PrefabBase parentPrefab, ComponentBase parentComponent)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = prefab;
        // ISSUE: reference to a compiler-generated field
        this.m_ParentPrefab = parentPrefab;
        // ISSUE: reference to a compiler-generated field
        this.m_ParentComponent = parentComponent;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
      }
    }
  }
}
