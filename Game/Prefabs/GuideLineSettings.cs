// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GuideLineSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {typeof (RenderingSettingsPrefab)})]
  public class GuideLineSettings : ComponentBase
  {
    public Color m_VeryLowPriorityColor = new Color(0.7f, 0.7f, 1f, 0.025f);
    public Color m_LowPriorityColor = new Color(0.7f, 0.7f, 1f, 0.05f);
    public Color m_MediumPriorityColor = new Color(0.7f, 0.7f, 1f, 0.1f);
    public Color m_HighPriorityColor = new Color(0.7f, 0.7f, 1f, 0.2f);
    public GuideLineSettings.WaterSourceColor[] m_WaterSourceColors;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<GuideLineSettingsData>());
      components.Add(ComponentType.ReadWrite<WaterSourceColorElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      GuideLineSettingsData componentData = new GuideLineSettingsData()
      {
        m_VeryLowPriorityColor = this.m_VeryLowPriorityColor,
        m_LowPriorityColor = this.m_LowPriorityColor,
        m_MediumPriorityColor = this.m_MediumPriorityColor,
        m_HighPriorityColor = this.m_HighPriorityColor
      };
      entityManager.SetComponentData<GuideLineSettingsData>(entity, componentData);
      if (this.m_WaterSourceColors == null)
        return;
      DynamicBuffer<WaterSourceColorElement> buffer = entityManager.GetBuffer<WaterSourceColorElement>(entity);
      buffer.ResizeUninitialized(this.m_WaterSourceColors.Length);
      for (int index = 0; index < this.m_WaterSourceColors.Length; ++index)
      {
        GuideLineSettings.WaterSourceColor waterSourceColor = this.m_WaterSourceColors[index];
        buffer[index] = new WaterSourceColorElement()
        {
          m_Outline = waterSourceColor.m_Outline,
          m_Fill = waterSourceColor.m_Fill,
          m_ProjectedOutline = waterSourceColor.m_ProjectedOutline,
          m_ProjectedFill = waterSourceColor.m_ProjectedFill
        };
      }
    }

    [Serializable]
    public class WaterSourceColor
    {
      public Color m_Outline;
      public Color m_Fill;
      public Color m_ProjectedOutline;
      public Color m_ProjectedFill;
    }
  }
}
