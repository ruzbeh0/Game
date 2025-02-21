// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BrushPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Tools;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tools/", new System.Type[] {})]
  public class BrushPrefab : PrefabBase
  {
    public Texture2D m_Texture;
    public int m_Priority;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<BrushData>());
      components.Add(ComponentType.ReadWrite<BrushCell>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Brush>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      this.RefreshArchetype(entityManager, entity);
    }

    protected virtual void RefreshArchetype(EntityManager entityManager, Entity entity)
    {
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      BrushData componentData;
      componentData.m_Archetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      componentData.m_Priority = this.m_Priority;
      componentData.m_Resolution = (int2) 0;
      entityManager.SetComponentData<BrushData>(entity, componentData);
    }
  }
}
