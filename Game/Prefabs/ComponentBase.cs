// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ComponentBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public abstract class ComponentBase : ScriptableObject, IComponentBase, IComparable
  {
    protected static ILog baseLog;
    public bool active = true;

    protected virtual void OnEnable() => ComponentBase.baseLog = LogManager.GetLogger("SceneFlow");

    protected virtual void OnDisable()
    {
    }

    public virtual bool ignoreUnlockDependencies => false;

    public PrefabBase prefab { get; set; }

    public T GetComponent<T>() where T : ComponentBase
    {
      if ((UnityEngine.Object) this.prefab == (UnityEngine.Object) null)
        throw new NullReferenceException(string.Format("GetComponent<{0}>() -> prefab is null", (object) typeof (T)));
      T component;
      return this.prefab.TryGet<T>(out component) ? component : default (T);
    }

    public ComponentBase GetComponentExactly(System.Type type)
    {
      if ((UnityEngine.Object) this.prefab == (UnityEngine.Object) null)
        throw new NullReferenceException(string.Format("GetComponentExactly<{0}>() -> prefab is null", (object) type));
      ComponentBase component;
      return this.prefab.TryGetExactly(type, out component) ? component : (ComponentBase) null;
    }

    public bool GetComponents<T>(List<T> list) where T : ComponentBase
    {
      System.Type type = typeof (T);
      return !((UnityEngine.Object) this.prefab == (UnityEngine.Object) null) ? this.prefab.TryGet<T>(list) : throw new NullReferenceException(string.Format("GetComponents<{0}>() -> prefab is null", (object) type));
    }

    public virtual void GetDependencies(List<PrefabBase> prefabs)
    {
    }

    public virtual void Initialize(EntityManager entityManager, Entity entity)
    {
    }

    public virtual void LateInitialize(EntityManager entityManager, Entity entity)
    {
    }

    public abstract void GetPrefabComponents(HashSet<ComponentType> components);

    public abstract void GetArchetypeComponents(HashSet<ComponentType> components);

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      ComponentBase componentBase = obj as ComponentBase;
      return (UnityEngine.Object) componentBase != (UnityEngine.Object) null ? this.name.CompareTo(componentBase.name) : throw new ArgumentException("Object is not a ComponentBase");
    }

    public virtual IEnumerable<string> modTags => Enumerable.Empty<string>();
  }
}
