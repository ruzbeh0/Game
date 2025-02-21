// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PrefabBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Json;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Prefabs
{
  public abstract class PrefabBase : ComponentBase, ISerializationCallbackReceiver, IPrefabBase
  {
    public List<ComponentBase> components = new List<ComponentBase>();
    [NonSerialized]
    public bool isDirty = true;

    public string thumbnailUrl { get; private set; }

    public bool builtin
    {
      get => Colossal.IO.AssetDatabase.AssetDatabase.global.resources.prefabsMap.TryGetGuid((UnityEngine.Object) this, out string _);
    }

    public PrefabAsset asset { get; set; }

    public void OnBeforeSerialize()
    {
    }

    public virtual void OnAfterDeserialize() => this.prefab = this;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.prefab = this;
      foreach (ComponentBase component in this.components)
      {
        if ((UnityEngine.Object) component != (UnityEngine.Object) null || (UnityEngine.Object) component.prefab == (UnityEngine.Object) component)
        {
          component.prefab = this;
        }
        else
        {
          if ((UnityEngine.Object) component == (UnityEngine.Object) null)
            ComponentBase.baseLog.ErrorFormat((UnityEngine.Object) this.prefab, "Null component on prefab: {0}", (object) this.prefab.name);
          if ((UnityEngine.Object) component.prefab != (UnityEngine.Object) component)
            ComponentBase.baseLog.ErrorFormat((UnityEngine.Object) this.prefab, "Component on prefab {0} is referenced from another prefab prefab: {1}", (object) this.prefab.name, (object) component.prefab.name);
        }
      }
      this.components.RemoveAll((Predicate<ComponentBase>) (x => (UnityEngine.Object) x == (UnityEngine.Object) null));
      this.thumbnailUrl = "thumbnail://ThumbnailCamera/" + Uri.EscapeDataString(this.GetType().Name) + "/" + Uri.EscapeDataString(this.name);
    }

    public virtual void Reset() => this.isDirty = true;

    public T AddOrGetComponent<T>() where T : ComponentBase
    {
      T component;
      if (!this.TryGetExactly<T>(out component))
        component = this.AddComponent<T>();
      return component;
    }

    public ComponentBase AddOrGetComponent(System.Type type)
    {
      ComponentBase component;
      if (!this.TryGetExactly(type, out component))
        component = this.AddComponent(type);
      return component;
    }

    public T AddComponent<T>() where T : ComponentBase => (T) this.AddComponent(typeof (T));

    public T AddComponentFrom<T>(T from) where T : ComponentBase
    {
      T component = this.AddOrGetComponent<T>();
      JsonUtility.FromJsonOverwrite(JsonUtility.ToJson((object) from), (object) component);
      return component;
    }

    public ComponentBase AddComponentFrom(ComponentBase from)
    {
      ComponentBase component = this.AddOrGetComponent(from.GetType());
      JsonUtility.FromJsonOverwrite(JsonUtility.ToJson((object) from), (object) component);
      return component;
    }

    public ComponentBase AddComponent(System.Type type)
    {
      ComponentBase componentBase = !this.Has(type) ? (ComponentBase) ScriptableObject.CreateInstance(type) : throw new InvalidOperationException("Component already exists");
      componentBase.name = type.Name;
      componentBase.prefab = this;
      this.components.Add(componentBase);
      this.isDirty = true;
      return componentBase;
    }

    public ComponentBase ReplaceComponentWith(ComponentBase target, System.Type type)
    {
      ComponentBase instance = (ComponentBase) ScriptableObject.CreateInstance(type);
      instance.prefab = this;
      this.components[this.components.IndexOf(target)] = instance;
      this.isDirty = true;
      return instance;
    }

    public void Remove<T>() where T : ComponentBase => this.Remove(typeof (T));

    public void Remove(System.Type type)
    {
      int index1 = -1;
      for (int index2 = 0; index2 < this.components.Count; ++index2)
      {
        if (this.components[index2].GetType() == type)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 < 0)
        return;
      this.components.RemoveAt(index1);
      this.isDirty = true;
    }

    public bool Has<T>() where T : ComponentBase => this.Has(typeof (T));

    public bool Has(System.Type type)
    {
      if (this.GetType() == type)
        return true;
      foreach (object component in this.components)
      {
        if (component.GetType() == type)
          return true;
      }
      return false;
    }

    public bool HasSubclassOf(System.Type type)
    {
      if (this.GetType().IsSubclassOf(type))
        return true;
      foreach (object component in this.components)
      {
        if (component.GetType().IsSubclassOf(type))
          return true;
      }
      return false;
    }

    public bool TryGet<T>(out T component) where T : ComponentBase
    {
      ComponentBase component1;
      int num = this.TryGet(typeof (T), out component1) ? 1 : 0;
      component = (T) component1;
      return num != 0;
    }

    public bool TryGet(System.Type type, out ComponentBase component)
    {
      System.Type type1 = this.GetType();
      component = (ComponentBase) null;
      if (type1 == type || type1.IsSubclassOf(type))
      {
        component = (ComponentBase) this;
        return true;
      }
      foreach (ComponentBase component1 in this.components)
      {
        System.Type type2 = component1.GetType();
        if (type2 == type || type2.IsSubclassOf(type))
        {
          component = component1;
          return true;
        }
      }
      return false;
    }

    public bool TryGetExactly<T>(out T component) where T : ComponentBase
    {
      ComponentBase component1;
      int num = this.TryGetExactly(typeof (T), out component1) ? 1 : 0;
      component = (T) component1;
      return num != 0;
    }

    public bool TryGetExactly(System.Type type, out ComponentBase component)
    {
      component = (ComponentBase) null;
      if (this.GetType() == type)
      {
        component = (ComponentBase) this;
        return true;
      }
      foreach (ComponentBase component1 in this.components)
      {
        if (component1.GetType() == type)
        {
          component = component1;
          return true;
        }
      }
      return false;
    }

    public bool TryGet<T>(List<T> result)
    {
      Assert.IsNotNull<List<ComponentBase>>(this.components);
      int count = result.Count;
      PrefabBase prefabBase = this;
      if (prefabBase is T)
      {
        T obj = prefabBase as T;
        result.Add(obj);
      }
      foreach (ComponentBase component in this.components)
      {
        if (component.active && component is T)
        {
          T obj = component as T;
          result.Add(obj);
        }
      }
      return count != result.Count;
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PrefabData>());
      components.Add(ComponentType.ReadWrite<LoadedIndex>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PrefabRef>());
    }

    public PrefabID GetPrefabID() => new PrefabID(this);

    public virtual bool canIgnoreUnlockDependencies => true;

    public virtual string uiTag => this.GetPrefabID().ToString();

    public PrefabBase Clone(string newName = null)
    {
      PrefabBase instance = (PrefabBase) ScriptableObject.CreateInstance(this.GetType());
      if (JSON.Load(JsonUtility.ToJson((object) this)) is ProxyObject proxyObject)
      {
        proxyObject.Remove("components");
        proxyObject.Remove("m_NameOverride");
      }
      JsonUtility.FromJsonOverwrite(proxyObject.ToJSON(), (object) instance);
      instance.name = newName ?? this.name + " (copy)";
      foreach (ComponentBase component in this.components)
        instance.AddComponentFrom(component);
      return instance;
    }
  }
}
