// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MailAccumulation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Services/", new Type[] {typeof (ServicePrefab), typeof (ZonePrefab)})]
  public class MailAccumulation : ComponentBase
  {
    public bool m_RequireCollect;
    public float m_SendingRate = 1f;
    public float m_ReceivingRate = 1f;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<MailAccumulationData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      MailAccumulationData componentData;
      componentData.m_RequireCollect = this.m_RequireCollect;
      componentData.m_AccumulationRate.x = this.m_SendingRate;
      componentData.m_AccumulationRate.y = this.m_ReceivingRate;
      entityManager.SetComponentData<MailAccumulationData>(entity, componentData);
    }
  }
}
