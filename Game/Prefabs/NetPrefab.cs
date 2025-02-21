// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Prefab/", new Type[] {})]
  public class NetPrefab : PrefabBase
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<NetData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<Game.Net.Node>()))
      {
        components.Add(ComponentType.ReadWrite<ConnectedEdge>());
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<Edge>()))
          return;
        components.Add(ComponentType.ReadWrite<ConnectedNode>());
      }
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      HashSet<ComponentType> componentTypeSet1 = new HashSet<ComponentType>();
      HashSet<ComponentType> componentTypeSet2 = new HashSet<ComponentType>();
      componentTypeSet1.Add(ComponentType.ReadWrite<Game.Net.Node>());
      componentTypeSet2.Add(ComponentType.ReadWrite<Edge>());
      for (int index = 0; index < list.Count; ++index)
      {
        list[index].GetArchetypeComponents(componentTypeSet1);
        list[index].GetArchetypeComponents(componentTypeSet2);
      }
      componentTypeSet1.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet2.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet1.Add(ComponentType.ReadWrite<Updated>());
      componentTypeSet2.Add(ComponentType.ReadWrite<Updated>());
      NetData componentData = entityManager.GetComponentData<NetData>(entity) with
      {
        m_NodeArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet1)),
        m_EdgeArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet2))
      };
      entityManager.SetComponentData<NetData>(entity, componentData);
    }
  }
}
