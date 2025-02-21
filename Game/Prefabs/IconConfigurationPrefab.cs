// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.IconConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class IconConfigurationPrefab : PrefabBase
  {
    public Material m_Material;
    public NotificationIconPrefab m_SelectedMarker;
    public NotificationIconPrefab m_FollowedMarker;
    public IconAnimationInfo[] m_Animations;
    public Texture2D m_MissingIcon;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_SelectedMarker);
      prefabs.Add((PrefabBase) this.m_FollowedMarker);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<IconConfigurationData>());
      components.Add(ComponentType.ReadWrite<IconAnimationElement>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      IconConfigurationData componentData;
      // ISSUE: reference to a compiler-generated method
      componentData.m_SelectedMarker = existingSystemManaged.GetEntity((PrefabBase) this.m_SelectedMarker);
      // ISSUE: reference to a compiler-generated method
      componentData.m_FollowedMarker = existingSystemManaged.GetEntity((PrefabBase) this.m_FollowedMarker);
      entityManager.SetComponentData<IconConfigurationData>(entity, componentData);
      if (this.m_Animations == null)
        return;
      DynamicBuffer<IconAnimationElement> buffer = entityManager.GetBuffer<IconAnimationElement>(entity);
      for (int index = 0; index < this.m_Animations.Length; ++index)
      {
        IconAnimationInfo animation = this.m_Animations[index];
        IconAnimationElement animationElement = new IconAnimationElement();
        animationElement.m_Duration = animation.m_Duration;
        animationElement.m_AnimationCurve = new AnimationCurve3(animation.m_Scale, animation.m_Alpha, animation.m_ScreenY);
        IconAnimationElement elem1 = animationElement;
        int type = (int) animation.m_Type;
        if (buffer.Length > type)
        {
          buffer[type] = elem1;
        }
        else
        {
          while (buffer.Length < type)
          {
            ref DynamicBuffer<IconAnimationElement> local = ref buffer;
            animationElement = new IconAnimationElement();
            IconAnimationElement elem2 = animationElement;
            local.Add(elem2);
          }
          buffer.Add(elem1);
        }
      }
    }
  }
}
