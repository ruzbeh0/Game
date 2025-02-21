// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DevTreeNodePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [RequireComponent(typeof (ManualUnlockable))]
  public class DevTreeNodePrefab : PrefabBase
  {
    public ServicePrefab m_Service;
    public DevTreeNodePrefab[] m_Requirements;
    public int m_Cost;
    public int m_HorizontalPosition;
    public float m_VerticalPosition;
    public string m_IconPath;
    public PrefabBase m_IconPrefab;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if ((Object) this.m_Service != (Object) null)
        prefabs.Add((PrefabBase) this.m_Service);
      if (this.m_Requirements == null)
        return;
      foreach (DevTreeNodePrefab requirement in this.m_Requirements)
      {
        if ((Object) requirement != (Object) null)
          prefabs.Add((PrefabBase) requirement);
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<DevTreeNodeData>());
      if (this.HasRequirements())
        components.Add(ComponentType.ReadWrite<DevTreeNodeRequirement>());
      if (this.m_Cost != 0)
        return;
      components.Add(ComponentType.ReadWrite<DevTreeNodeAutoUnlock>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) this.m_Service);
      entityManager.SetComponentData<DevTreeNodeData>(entity, new DevTreeNodeData()
      {
        m_Cost = this.m_Cost,
        m_Service = entity1
      });
      if (!entityManager.HasComponent<DevTreeNodeRequirement>(entity))
        return;
      DynamicBuffer<DevTreeNodeRequirement> buffer = entityManager.GetBuffer<DevTreeNodeRequirement>(entity);
      foreach (DevTreeNodePrefab requirement in this.m_Requirements)
      {
        if ((Object) requirement != (Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          Entity entity2 = existingSystemManaged.GetEntity((PrefabBase) requirement);
          buffer.Add(new DevTreeNodeRequirement()
          {
            m_Node = entity2
          });
        }
      }
    }

    private bool HasRequirements()
    {
      if (this.m_Requirements != null)
      {
        foreach (Object requirement in this.m_Requirements)
        {
          if (requirement != (Object) null)
            return true;
        }
      }
      return false;
    }
  }
}
