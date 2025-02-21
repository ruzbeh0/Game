// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PostServiceConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new Type[] {})]
  public class PostServiceConfigurationPrefab : PrefabBase
  {
    public ServicePrefab m_PostServicePrefab;
    public int m_MaxMailAccumulation = 2000;
    public int m_MailAccumulationTolerance = 10;
    public int m_OutgoingMailPercentage = 15;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_PostServicePrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<PostConfigurationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<PostConfigurationData>(entity, new PostConfigurationData()
      {
        m_PostServicePrefab = systemManaged.GetEntity((PrefabBase) this.m_PostServicePrefab),
        m_MaxMailAccumulation = this.m_MaxMailAccumulation,
        m_MailAccumulationTolerance = this.m_MailAccumulationTolerance,
        m_OutgoingMailPercentage = this.m_OutgoingMailPercentage
      });
    }
  }
}
