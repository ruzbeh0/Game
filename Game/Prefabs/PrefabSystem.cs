// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PrefabSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.Serialization.Entities;
using Game.Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class PrefabSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private ILog m_UnlockingLog;
    private UpdateSystem m_UpdateSystem;
    private List<PrefabBase> m_Prefabs;
    private List<PrefabSystem.ObsoleteData> m_ObsoleteIDs;
    private List<PrefabSystem.LoadedIndexData> m_LoadedIndexData;
    private System.Collections.Generic.Dictionary<PrefabBase, Entity> m_UpdateMap;
    private System.Collections.Generic.Dictionary<PrefabBase, Entity> m_Entities;
    private System.Collections.Generic.Dictionary<PrefabBase, bool> m_IsUnlockable;
    private System.Collections.Generic.Dictionary<ContentPrefab, bool> m_IsAvailable;
    private System.Collections.Generic.Dictionary<int, PrefabID> m_LoadedObsoleteIDs;
    private System.Collections.Generic.Dictionary<PrefabID, int> m_PrefabIndices;
    private ComponentTypeSet m_UnlockableTypes;

    internal IEnumerable<PrefabBase> prefabs => (IEnumerable<PrefabBase>) this.m_Prefabs;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Prefabs = new List<PrefabBase>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObsoleteIDs = new List<PrefabSystem.ObsoleteData>();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedIndexData = new List<PrefabSystem.LoadedIndexData>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateMap = new System.Collections.Generic.Dictionary<PrefabBase, Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_Entities = new System.Collections.Generic.Dictionary<PrefabBase, Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_IsUnlockable = new System.Collections.Generic.Dictionary<PrefabBase, bool>();
      // ISSUE: reference to a compiler-generated field
      this.m_IsAvailable = new System.Collections.Generic.Dictionary<ContentPrefab, bool>();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedObsoleteIDs = new System.Collections.Generic.Dictionary<int, PrefabID>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabIndices = new System.Collections.Generic.Dictionary<PrefabID, int>();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockingLog = LogManager.GetLogger("Unlocking");
    }

    public bool AddPrefab(
      PrefabBase prefab,
      string parentName = null,
      PrefabBase parentPrefab = null,
      ComponentBase parentComponent = null)
    {
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        if (parentName != null)
          COSystemBase.baseLog.WarnFormat("Trying to add null prefab in {0}", (object) parentName);
        else if ((UnityEngine.Object) parentPrefab != (UnityEngine.Object) null && (UnityEngine.Object) parentComponent != (UnityEngine.Object) null)
          COSystemBase.baseLog.WarnFormat("Trying to add null prefab in {0}/{1}", (object) parentPrefab.name, (object) parentComponent.name);
        else if ((UnityEngine.Object) parentPrefab != (UnityEngine.Object) null)
          COSystemBase.baseLog.WarnFormat("Trying to add null prefab in {0}", (object) parentPrefab.name);
        else
          COSystemBase.baseLog.WarnFormat("Trying to add null prefab");
        return false;
      }
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Entities.ContainsKey(prefab))
        {
          // ISSUE: reference to a compiler-generated method
          if (!this.IsAvailable(prefab))
          {
            if ((UnityEngine.Object) parentPrefab != (UnityEngine.Object) null)
            {
              if ((UnityEngine.Object) parentComponent != (UnityEngine.Object) null)
                COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, "Dependency not available in {0}/{1}: {2}", (object) parentPrefab.name, (object) parentComponent.name, (object) prefab.name);
              else
                COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, "Dependency not available in {0}: {1}", (object) parentPrefab.name, (object) prefab.name);
            }
            return false;
          }
          COSystemBase.baseLog.VerboseFormat((UnityEngine.Object) prefab, "Adding prefab '{0}'", (object) prefab.name);
          List<ComponentBase> list = new List<ComponentBase>();
          prefab.GetComponents<ComponentBase>(list);
          HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
          for (int index = 0; index < list.Count; ++index)
            list[index].GetPrefabComponents(componentTypeSet);
          // ISSUE: reference to a compiler-generated method
          if (this.IsUnlockable(prefab))
          {
            componentTypeSet.Add(ComponentType.ReadWrite<UnlockRequirement>());
            componentTypeSet.Add(ComponentType.ReadWrite<Locked>());
            // ISSUE: reference to a compiler-generated field
            this.m_UnlockingLog.DebugFormat("Prefab locked: {0}", (object) prefab);
          }
          componentTypeSet.Add(ComponentType.ReadWrite<Created>());
          componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
          Entity entity = this.EntityManager.CreateEntity(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.SetComponentData<PrefabData>(entity, new PrefabData()
          {
            m_Index = this.m_Prefabs.Count
          });
          PrefabID prefabId = prefab.GetPrefabID();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabIndices.ContainsKey(prefabId))
          {
            COSystemBase.baseLog.WarnFormat((UnityEngine.Object) prefab, "Duplicate prefab ID: {0}", (object) prefabId);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabIndices.Add(prefabId, this.m_Prefabs.Count);
          }
          ObsoleteIdentifiers component;
          if (prefab.TryGet<ObsoleteIdentifiers>(out component) && component.m_PrefabIdentifiers != null)
          {
            for (int index = 0; index < component.m_PrefabIdentifiers.Length; ++index)
            {
              PrefabIdentifierInfo prefabIdentifier = component.m_PrefabIdentifiers[index];
              prefabId = new PrefabID(prefabIdentifier.m_Type, prefabIdentifier.m_Name);
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabIndices.ContainsKey(prefabId))
              {
                COSystemBase.baseLog.WarnFormat((UnityEngine.Object) prefab, "Duplicate prefab ID: {0} ({1})", (object) prefabId, (AssetData) prefab.asset != (IAssetData) null ? (object) prefab.asset : (object) prefab.name);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_PrefabIndices.Add(prefabId, this.m_Prefabs.Count);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_Prefabs.Add(prefab);
          // ISSUE: reference to a compiler-generated field
          this.m_Entities.Add(prefab, entity);
          return true;
        }
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, ex, "Error when adding prefab: {0}", (object) prefab.name);
      }
      return false;
    }

    public bool RemovePrefab(PrefabBase prefab)
    {
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        COSystemBase.baseLog.WarnFormat("Trying to remove null prefab");
        return false;
      }
      try
      {
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Entities.TryGetValue(prefab, out entity1))
        {
          COSystemBase.baseLog.VerboseFormat((UnityEngine.Object) prefab, "Removing prefab '{0}'", (object) prefab.name);
          EntityManager entityManager = this.EntityManager;
          entityManager.AddComponent<Deleted>(entity1);
          entityManager = this.EntityManager;
          PrefabData componentData1 = entityManager.GetComponentData<PrefabData>(entity1);
          PrefabID key1 = prefab.GetPrefabID();
          int num1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabIndices.TryGetValue(key1, out num1) && num1 == componentData1.m_Index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabIndices.Remove(key1);
          }
          ObsoleteIdentifiers component1;
          if (prefab.TryGet<ObsoleteIdentifiers>(out component1) && component1.m_PrefabIdentifiers != null)
          {
            for (int index = 0; index < component1.m_PrefabIdentifiers.Length; ++index)
            {
              PrefabIdentifierInfo prefabIdentifier = component1.m_PrefabIdentifiers[index];
              key1 = new PrefabID(prefabIdentifier.m_Type, prefabIdentifier.m_Name);
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabIndices.TryGetValue(key1, out num1) && num1 == componentData1.m_Index)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_PrefabIndices.Remove(key1);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (componentData1.m_Index != this.m_Prefabs.Count - 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PrefabBase prefab1 = this.m_Prefabs[this.m_Prefabs.Count - 1];
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_Entities[prefab1];
            entityManager = this.EntityManager;
            PrefabData componentData2 = entityManager.GetComponentData<PrefabData>(entity2);
            PrefabID key2 = prefab1.GetPrefabID();
            int num2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabIndices.TryGetValue(key2, out num2) && num2 == componentData2.m_Index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_PrefabIndices[key2] = componentData1.m_Index;
            }
            ObsoleteIdentifiers component2;
            if (prefab1.TryGet<ObsoleteIdentifiers>(out component2) && component2.m_PrefabIdentifiers != null)
            {
              for (int index = 0; index < component2.m_PrefabIdentifiers.Length; ++index)
              {
                PrefabIdentifierInfo prefabIdentifier = component2.m_PrefabIdentifiers[index];
                key2 = new PrefabID(prefabIdentifier.m_Type, prefabIdentifier.m_Name);
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabIndices.TryGetValue(key2, out num2) && num2 == componentData2.m_Index)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_PrefabIndices[key2] = componentData1.m_Index;
                }
              }
            }
            componentData2.m_Index = componentData1.m_Index;
            entityManager = this.EntityManager;
            entityManager.SetComponentData<PrefabData>(entity2, componentData2);
            // ISSUE: reference to a compiler-generated field
            this.m_Prefabs[componentData1.m_Index] = prefab1;
          }
          componentData1.m_Index = -1000000000;
          entityManager = this.EntityManager;
          entityManager.SetComponentData<PrefabData>(entity1, componentData1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Prefabs.RemoveAt(this.m_Prefabs.Count - 1);
          // ISSUE: reference to a compiler-generated field
          this.m_Entities.Remove(prefab);
          // ISSUE: reference to a compiler-generated field
          this.m_IsUnlockable.Remove(prefab);
          return true;
        }
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, ex, "Error when removing prefab: {0}", (object) prefab.name);
      }
      return false;
    }

    public PrefabBase DuplicatePrefab(PrefabBase template, string name = null)
    {
      PrefabBase prefab = template.Clone(name);
      prefab.Remove<ObsoleteIdentifiers>();
      // ISSUE: reference to a compiler-generated method
      this.AddPrefab(prefab);
      return prefab;
    }

    public void UpdatePrefab(PrefabBase prefab, Entity sourceInstance = default (Entity))
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateMap[prefab] = sourceInstance;
    }

    public bool IsAvailable(PrefabBase prefab)
    {
      ContentPrerequisite component;
      if (!prefab.TryGet<ContentPrerequisite>(out component))
        return true;
      bool flag;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_IsAvailable.TryGetValue(component.m_ContentPrerequisite, out flag))
      {
        flag = component.m_ContentPrerequisite.IsAvailable();
        // ISSUE: reference to a compiler-generated field
        this.m_IsAvailable.Add(component.m_ContentPrerequisite, flag);
      }
      return flag;
    }

    public void ClearAvailabilityCache() => this.m_IsAvailable.Clear();

    public bool IsUnlockable(PrefabBase prefab)
    {
      bool flag1;
      // ISSUE: reference to a compiler-generated field
      if (this.m_IsUnlockable.TryGetValue(prefab, out flag1))
        return flag1;
      if (prefab is UnlockRequirementPrefab)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_IsUnlockable.Add(prefab, true);
        return true;
      }
      List<PrefabBase> dependencies = new List<PrefabBase>();
      List<ComponentBase> components = new List<ComponentBase>();
      // ISSUE: reference to a compiler-generated method
      bool flag2 = this.IsUnlockableImpl(prefab, dependencies, components);
      // ISSUE: reference to a compiler-generated field
      this.m_IsUnlockable.Add(prefab, flag2);
      return flag2;
    }

    private bool IsUnlockableImpl(
      PrefabBase prefab,
      List<PrefabBase> dependencies,
      List<ComponentBase> components)
    {
      int count = dependencies.Count;
      try
      {
        try
        {
          bool unlockDependencies = prefab.canIgnoreUnlockDependencies;
          prefab.GetComponents<ComponentBase>(components);
          for (int index = 0; index < components.Count; ++index)
          {
            ComponentBase component = components[index];
            if (component is UnlockableBase)
              return true;
            if (!unlockDependencies || !component.ignoreUnlockDependencies)
              component.GetDependencies(dependencies);
          }
        }
        finally
        {
          components.Clear();
        }
        for (int index = count; index < dependencies.Count; ++index)
        {
          PrefabBase dependency = dependencies[index];
          if (!((UnityEngine.Object) dependency == (UnityEngine.Object) null))
          {
            bool flag;
            // ISSUE: reference to a compiler-generated field
            if (this.m_IsUnlockable.TryGetValue(dependency, out flag))
            {
              if (flag)
                return true;
            }
            else
            {
              if (dependency is UnlockRequirementPrefab)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_IsUnlockable.Add(dependency, true);
                return true;
              }
              // ISSUE: reference to a compiler-generated method
              flag = this.IsUnlockableImpl(dependency, dependencies, components);
              // ISSUE: reference to a compiler-generated field
              this.m_IsUnlockable.Add(dependency, flag);
              if (flag)
                return true;
            }
          }
        }
        return false;
      }
      finally
      {
        dependencies.RemoveRange(count, dependencies.Count - count);
      }
    }

    public void AddUnlockRequirement(PrefabBase unlocker, PrefabBase unlocked)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.IsUnlockable(unlocker))
      {
        // ISSUE: reference to a compiler-generated method
        if (this.IsUnlockable(unlocked))
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.GetBuffer<UnlockRequirement>(unlocked, false).Add(new UnlockRequirement(this.GetEntity(unlocker), UnlockFlags.RequireAll));
        }
        else
          COSystemBase.baseLog.WarnFormat((UnityEngine.Object) unlocked, "{0} is trying to add unlock requirement to non-unlockable prefab {1}", (object) unlocker.name, (object) unlocked.name);
      }
      else
        COSystemBase.baseLog.WarnFormat((UnityEngine.Object) unlocker, "{0} is trying to add unlock requirements, but is non-unlockable", (object) unlocker.name);
    }

    public void AddUnlockRequirement(PrefabBase unlocker, PrefabBase[] unlocked)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.IsUnlockable(unlocker))
      {
        // ISSUE: reference to a compiler-generated method
        Entity entity = this.GetEntity(unlocker);
        for (int index = 0; index < unlocked.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.IsUnlockable(unlocked[index]))
          {
            // ISSUE: reference to a compiler-generated method
            this.GetBuffer<UnlockRequirement>(unlocked[index], false).Add(new UnlockRequirement(entity, UnlockFlags.RequireAll));
          }
          else
            COSystemBase.baseLog.WarnFormat((UnityEngine.Object) unlocked[index], "{0} is trying to add unlock requirement to non-unlockable prefab {1}", (object) unlocker.name, (object) unlocked[index].name);
        }
      }
      else
        COSystemBase.baseLog.WarnFormat((UnityEngine.Object) unlocker, "{0} is trying to add unlock requirements, but is non-unlockable", (object) unlocker.name);
    }

    public T GetPrefab<T>(PrefabData prefabData) where T : PrefabBase
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_Prefabs[prefabData.m_Index] as T;
    }

    public T GetPrefab<T>(Entity entity) where T : PrefabBase
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetPrefab<T>(this.EntityManager.GetComponentData<PrefabData>(entity));
    }

    public T GetPrefab<T>(PrefabRef refData) where T : PrefabBase
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetPrefab<T>(this.EntityManager.GetComponentData<PrefabData>(refData.m_Prefab));
    }

    public bool TryGetPrefab<T>(PrefabData prefabData, out T prefab) where T : PrefabBase
    {
      if (prefabData.m_Index >= 0)
      {
        // ISSUE: reference to a compiler-generated field
        prefab = this.m_Prefabs[prefabData.m_Index] as T;
        return true;
      }
      prefab = default (T);
      return false;
    }

    public bool TryGetPrefab<T>(Entity entity, out T prefab) where T : PrefabBase
    {
      PrefabData component;
      if (this.EntityManager.TryGetComponent<PrefabData>(entity, out component))
      {
        // ISSUE: reference to a compiler-generated method
        return this.TryGetPrefab<T>(component, out prefab);
      }
      prefab = default (T);
      return false;
    }

    public bool TryGetPrefab<T>(PrefabRef refData, out T prefab) where T : PrefabBase
    {
      PrefabData component;
      if (this.EntityManager.TryGetComponent<PrefabData>(refData.m_Prefab, out component))
      {
        // ISSUE: reference to a compiler-generated method
        return this.TryGetPrefab<T>(component, out prefab);
      }
      prefab = default (T);
      return false;
    }

    public T GetSingletonPrefab<T>(EntityQuery group) where T : PrefabBase
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetPrefab<T>(group.GetSingletonEntity());
    }

    public bool TryGetSingletonPrefab<T>(EntityQuery group, out T prefab) where T : PrefabBase
    {
      if (!group.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        prefab = this.GetPrefab<T>(group.GetSingletonEntity());
        return true;
      }
      prefab = default (T);
      return false;
    }

    public Entity GetEntity(PrefabBase prefab) => this.m_Entities[prefab];

    public bool TryGetEntity(PrefabBase prefab, out Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_Entities.TryGetValue(prefab, out entity);
    }

    public bool HasComponent<T>(PrefabBase prefab) where T : unmanaged
    {
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.HasComponent<T>(this.m_Entities[prefab]);
    }

    public bool HasEnabledComponent<T>(PrefabBase prefab) where T : unmanaged, IEnableableComponent
    {
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.HasEnabledComponent<T>(this.m_Entities[prefab]);
    }

    public T GetComponentData<T>(PrefabBase prefab) where T : unmanaged, IComponentData
    {
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.GetComponentData<T>(this.m_Entities[prefab]);
    }

    public bool TryGetComponentData<T>(PrefabBase prefab, out T component) where T : unmanaged, IComponentData
    {
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.TryGetComponent<T>(this.m_Entities[prefab], out component);
    }

    public DynamicBuffer<T> GetBuffer<T>(PrefabBase prefab, bool isReadOnly) where T : unmanaged, IBufferElementData
    {
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.GetBuffer<T>(this.m_Entities[prefab], isReadOnly);
    }

    public bool TryGetBuffer<T>(PrefabBase prefab, bool isReadOnly, out DynamicBuffer<T> buffer) where T : unmanaged, IBufferElementData
    {
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.TryGetBuffer<T>(this.m_Entities[prefab], isReadOnly, out buffer);
    }

    public void AddComponentData<T>(PrefabBase prefab, T componentData) where T : unmanaged, IComponentData
    {
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.AddComponentData<T>(this.m_Entities[prefab], componentData);
    }

    public void RemoveComponent<T>(PrefabBase prefab) where T : unmanaged, IComponentData
    {
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<T>(this.m_Entities[prefab]);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      int num = this.UpdatePrefabs() ? 1 : 0;
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.PrefabUpdate);
      if (num == 0)
        return;
      // ISSUE: reference to a compiler-generated method
      this.World.GetOrCreateSystemManaged<ReplacePrefabSystem>().FinalizeReplaces();
    }

    private bool UpdatePrefabs()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpdateMap.Count == 0)
        return false;
      try
      {
        // ISSUE: reference to a compiler-generated field
        foreach (KeyValuePair<PrefabBase, Entity> update in this.m_UpdateMap)
        {
          PrefabBase key = update.Key;
          Entity sourceInstance = update.Value;
          try
          {
            Entity entity1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Entities.TryGetValue(key, out entity1))
            {
              EntityManager entityManager = this.EntityManager;
              entityManager.AddComponent<Deleted>(entity1);
              List<ComponentBase> list = new List<ComponentBase>();
              key.GetComponents<ComponentBase>(list);
              HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
              for (int index = 0; index < list.Count; ++index)
                list[index].GetPrefabComponents(componentTypeSet);
              // ISSUE: reference to a compiler-generated method
              int num = this.IsUnlockable(key) ? 1 : 0;
              if (num != 0)
              {
                componentTypeSet.Add(ComponentType.ReadWrite<UnlockRequirement>());
                componentTypeSet.Add(ComponentType.ReadWrite<Locked>());
              }
              componentTypeSet.Add(ComponentType.ReadWrite<Created>());
              componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
              entityManager = this.EntityManager;
              Entity entity2 = entityManager.CreateEntity(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
              entityManager = this.EntityManager;
              entityManager.SetComponentData<PrefabData>(entity2, this.EntityManager.GetComponentData<PrefabData>(entity1));
              if (num != 0 && !this.EntityManager.HasEnabledComponent<Locked>(entity1))
              {
                entityManager = this.EntityManager;
                entityManager.SetComponentEnabled<Locked>(entity2, false);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_Entities[key] = entity2;
              // ISSUE: reference to a compiler-generated method
              this.World.GetOrCreateSystemManaged<ReplacePrefabSystem>().ReplacePrefab(entity1, entity2, sourceInstance);
            }
          }
          catch (Exception ex)
          {
            COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) key, ex, "Error when updating prefab: {0}", (object) key.name);
          }
        }
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateMap.Clear();
      }
      return true;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      int count1 = this.m_Prefabs.Count;
      // ISSUE: reference to a compiler-generated field
      int count2 = this.m_ObsoleteIDs.Count;
      List<PrefabID> prefabIdList1 = new List<PrefabID>(10000);
      List<PrefabID> prefabIdList2 = new List<PrefabID>(100);
      for (int index = 0; index < count1; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabBase prefab = this.m_Prefabs[index];
        // ISSUE: reference to a compiler-generated method
        if (this.EntityManager.IsComponentEnabled<PrefabData>(this.GetEntity(prefab)))
          prefabIdList1.Add(prefab.GetPrefabID());
      }
      for (int index = 0; index < count2; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PrefabSystem.ObsoleteData obsoleteId = this.m_ObsoleteIDs[index];
        // ISSUE: reference to a compiler-generated field
        if (this.EntityManager.IsComponentEnabled<PrefabData>(obsoleteId.m_Entity))
        {
          // ISSUE: reference to a compiler-generated field
          prefabIdList2.Add(obsoleteId.m_ID);
        }
      }
      int count3 = prefabIdList1.Count;
      int count4 = prefabIdList2.Count;
      writer.Write(count3);
      writer.Write(count4);
      for (int index = 0; index < count3; ++index)
        writer.Write<PrefabID>(prefabIdList1[index]);
      for (int index = 0; index < count4; ++index)
        writer.Write<PrefabID>(prefabIdList2[index]);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ObsoleteIDs.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedObsoleteIDs.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedIndexData.Clear();
      int num1;
      reader.Read(out num1);
      int num2 = 0;
      if (reader.context.version >= Game.Version.obsoletePrefabFix)
        reader.Read(out num2);
      for (int key = 0; key < num1; ++key)
      {
        PrefabID id;
        reader.Read<PrefabID>(out id);
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated method
        if (this.TryGetPrefab(id, out prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          this.m_LoadedIndexData.Add(new PrefabSystem.LoadedIndexData()
          {
            m_Entity = this.GetEntity(prefab),
            m_Index = key
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LoadedObsoleteIDs.Add(key, id);
        }
      }
      for (int index = 0; index < num2; ++index)
      {
        int key = -1 - index;
        PrefabID id;
        reader.Read<PrefabID>(out id);
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated method
        if (this.TryGetPrefab(id, out prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          this.m_LoadedIndexData.Add(new PrefabSystem.LoadedIndexData()
          {
            m_Entity = this.GetEntity(prefab),
            m_Index = key
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LoadedObsoleteIDs.Add(key, id);
        }
      }
    }

    public void SetDefaults(Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ObsoleteIDs.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedObsoleteIDs.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedIndexData.Clear();
    }

    public void UpdateLoadedIndices()
    {
      // ISSUE: reference to a compiler-generated field
      int count1 = this.m_Prefabs.Count;
      for (int index = 0; index < count1; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.EntityManager.GetBuffer<LoadedIndex>(this.GetEntity(this.m_Prefabs[index])).Clear();
      }
      // ISSUE: reference to a compiler-generated field
      int count2 = this.m_LoadedIndexData.Count;
      for (int index = 0; index < count2; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PrefabSystem.LoadedIndexData loadedIndexData = this.m_LoadedIndexData[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.GetBuffer<LoadedIndex>(loadedIndexData.m_Entity).Add(new LoadedIndex()
        {
          m_Index = loadedIndexData.m_Index
        });
      }
    }

    public bool TryGetPrefab(PrefabID id, out PrefabBase prefab)
    {
      int index;
      // ISSUE: reference to a compiler-generated field
      if (this.m_PrefabIndices.TryGetValue(id, out index))
      {
        // ISSUE: reference to a compiler-generated field
        prefab = this.m_Prefabs[index];
        return true;
      }
      prefab = (PrefabBase) null;
      return false;
    }

    public PrefabID GetLoadedObsoleteID(int loadedIndex)
    {
      PrefabID loadedObsoleteId;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LoadedObsoleteIDs.TryGetValue(loadedIndex, out loadedObsoleteId))
        loadedObsoleteId = new PrefabID("[Missing]", "[Missing]");
      return loadedObsoleteId;
    }

    public void AddObsoleteID(Entity entity, PrefabID id)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_ObsoleteIDs.Add(new PrefabSystem.ObsoleteData()
      {
        m_Entity = entity,
        m_ID = id
      });
      COSystemBase.baseLog.WarnFormat("Unknown prefab ID: {0}", (object) id);
    }

    public PrefabID GetObsoleteID(PrefabData prefabData)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_ObsoleteIDs[-1 - prefabData.m_Index].m_ID;
    }

    public PrefabID GetObsoleteID(Entity entity)
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetObsoleteID(this.EntityManager.GetComponentData<PrefabData>(entity));
    }

    public string GetPrefabName(Entity entity)
    {
      PrefabData component;
      if (!this.EntityManager.TryGetComponent<PrefabData>(entity, out component))
        return entity.ToString();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return component.m_Index >= 0 ? this.m_Prefabs[component.m_Index].name : this.GetObsoleteID(component).GetName();
    }

    [Preserve]
    public PrefabSystem()
    {
    }

    private struct ObsoleteData
    {
      public Entity m_Entity;
      public PrefabID m_ID;
    }

    private struct LoadedIndexData
    {
      public Entity m_Entity;
      public int m_Index;
    }
  }
}
