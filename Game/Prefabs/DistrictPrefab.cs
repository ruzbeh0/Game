// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DistrictPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Policies;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Areas/", new System.Type[] {})]
  public class DistrictPrefab : AreaPrefab
  {
    public Color m_NameColor = Color.white;
    public Color m_SelectedNameColor = new Color(0.5f, 0.75f, 1f, 1f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<DistrictData>());
      components.Add(ComponentType.ReadWrite<AreaNameData>());
      components.Add(ComponentType.ReadWrite<AreaGeometryData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<District>());
      components.Add(ComponentType.ReadWrite<Geometry>());
      components.Add(ComponentType.ReadWrite<LabelExtents>());
      components.Add(ComponentType.ReadWrite<LabelVertex>());
      components.Add(ComponentType.ReadWrite<DistrictModifier>());
      components.Add(ComponentType.ReadWrite<Policy>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<AreaNameData>(entity, new AreaNameData()
      {
        m_Color = (Color32) this.m_NameColor,
        m_SelectedColor = (Color32) this.m_SelectedNameColor
      });
    }
  }
}
