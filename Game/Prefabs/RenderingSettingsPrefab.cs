// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RenderingSettingsPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class RenderingSettingsPrefab : PrefabBase
  {
    public Color m_HoveredColor = new Color(0.5f, 0.5f, 1f, 0.1f);
    public Color m_OverrideColor = new Color(1f, 1f, 1f, 0.1f);
    public Color m_WarningColor = new Color(1f, 1f, 0.5f, 0.1f);
    public Color m_ErrorColor = new Color(1f, 0.5f, 0.5f, 0.1f);
    public Color m_OwnerColor = new Color(0.5f, 1f, 0.5f, 0.1f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<RenderingSettingsData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      RenderingSettingsData componentData = new RenderingSettingsData()
      {
        m_HoveredColor = this.m_HoveredColor,
        m_OverrideColor = this.m_OverrideColor,
        m_WarningColor = this.m_WarningColor,
        m_ErrorColor = this.m_ErrorColor,
        m_OwnerColor = this.m_OwnerColor
      };
      entityManager.SetComponentData<RenderingSettingsData>(entity, componentData);
    }
  }
}
