// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Wildlife
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Creatures/", new Type[] {typeof (AnimalPrefab)})]
  public class Wildlife : ComponentBase
  {
    public Bounds1 m_TripLength = new Bounds1(20f, 200f);
    public Bounds1 m_IdleTime = new Bounds1(10f, 60f);
    public int m_MinGroupMemberCount = 1;
    public int m_MaxGroupMemberCount = 4;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WildlifeData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Creatures.Wildlife>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      WildlifeData componentData;
      componentData.m_TripLength = this.m_TripLength;
      componentData.m_IdleTime = this.m_IdleTime;
      componentData.m_GroupMemberCount.x = this.m_MinGroupMemberCount;
      componentData.m_GroupMemberCount.y = this.m_MaxGroupMemberCount;
      entityManager.SetComponentData<WildlifeData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(13));
    }
  }
}
