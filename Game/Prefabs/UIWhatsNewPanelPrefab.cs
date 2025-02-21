// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIWhatsNewPanelPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("UI/", new System.Type[] {})]
  public class UIWhatsNewPanelPrefab : PrefabBase
  {
    public UIWhatsNewPanelPrefab.UIWhatsNewPanelPage[] m_Pages;

    public override void GetPrefabComponents(HashSet<ComponentType> prefabComponents)
    {
      base.GetPrefabComponents(prefabComponents);
      prefabComponents.Add(ComponentType.ReadWrite<UIWhatsNewPanelPrefabData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      UIWhatsNewPanelPrefabData componentData = entityManager.GetComponentData<UIWhatsNewPanelPrefabData>(entity);
      DlcRequirement component = this.prefab.GetComponent<DlcRequirement>();
      componentData.m_Id = component.m_Dlc.id;
      entityManager.SetComponentData<UIWhatsNewPanelPrefabData>(entity, componentData);
    }

    [Serializable]
    public class UIWhatsNewPanelPage
    {
      public UIWhatsNewPanelPrefab.UIWhatsNewPanelPageItem[] m_Items;
    }

    [Serializable]
    public class UIWhatsNewPanelPageItem
    {
      public UIWhatsNewPanelPrefab.UIWhatsNewPanelImage[] m_Images;
      [CanBeNull]
      public string m_TitleId;
      [CanBeNull]
      public string m_SubTitleId;
      [CanBeNull]
      public string m_ParagraphsId;
      public UIWhatsNewPanelPrefab.UIWhatsNewPanelPageItem.Justify m_Justify;

      public enum Justify
      {
        Left,
        Center,
        Right,
      }
    }

    [Serializable]
    public class UIWhatsNewPanelImage
    {
      [NotNull]
      public string m_Uri;
      public int2 m_AspectRatio;
      [Range(10f, 100f)]
      [Tooltip("The percentage of the total width this image should use.")]
      public int m_Width;
    }
  }
}
