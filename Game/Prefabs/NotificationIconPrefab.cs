// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NotificationIconPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Notifications;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Notifications/", new System.Type[] {})]
  public class NotificationIconPrefab : PrefabBase
  {
    public Texture2D m_Icon;
    public string m_Description;
    public string m_TargetDescription;
    public Bounds1 m_DisplaySize = new Bounds1(3f, 3f);
    public Bounds1 m_PulsateAmplitude = new Bounds1(0.01f, 0.1f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<NotificationIconData>());
      components.Add(ComponentType.ReadWrite<NotificationIconDisplayData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Icon>());
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
      entityManager.SetComponentData<NotificationIconData>(entity, new NotificationIconData()
      {
        m_Archetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet))
      });
    }
  }
}
