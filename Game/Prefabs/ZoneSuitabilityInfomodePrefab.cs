// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneSuitabilityInfomodePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Zones;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tools/Infomode/", new Type[] {})]
  public class ZoneSuitabilityInfomodePrefab : GradientInfomodeBasePrefab
  {
    public AreaType m_AreaType;
    public bool m_Office;

    public override string infomodeTypeLocaleKey => "NetworkColor";

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<InfoviewAvailabilityData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<InfoviewAvailabilityData>(entity, new InfoviewAvailabilityData()
      {
        m_AreaType = this.m_AreaType,
        m_Office = this.m_Office
      });
    }
  }
}
