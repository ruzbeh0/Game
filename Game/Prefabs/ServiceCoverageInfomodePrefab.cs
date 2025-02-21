// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ServiceCoverageInfomodePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Net;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tools/Infomode/", new Type[] {})]
  public class ServiceCoverageInfomodePrefab : GradientInfomodeBasePrefab
  {
    public CoverageService m_Service;
    public Bounds1 m_Range = new Bounds1(0.0f, 5f);

    public override string infomodeTypeLocaleKey => "NetworkColor";

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<InfoviewCoverageData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<InfoviewCoverageData>(entity, new InfoviewCoverageData()
      {
        m_Service = this.m_Service,
        m_Range = this.m_Range
      });
    }
  }
}
