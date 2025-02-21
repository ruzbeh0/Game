// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetLabel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new System.Type[] {typeof (AggregateNetPrefab)})]
  public class NetLabel : ComponentBase
  {
    public Material m_NameMaterial;
    public Color m_NameColor = Color.white;
    public Color m_SelectedNameColor = new Color(0.5f, 0.75f, 1f, 1f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<NetNameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<LabelMaterial>());
      components.Add(ComponentType.ReadWrite<LabelExtents>());
      components.Add(ComponentType.ReadWrite<LabelPosition>());
      components.Add(ComponentType.ReadWrite<LabelVertex>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<NetNameData>(entity, new NetNameData()
      {
        m_Color = (Color32) this.m_NameColor.linear,
        m_SelectedColor = (Color32) this.m_SelectedNameColor.linear
      });
    }
  }
}
