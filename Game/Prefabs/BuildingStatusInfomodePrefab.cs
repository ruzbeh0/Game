// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingStatusInfomodePrefab
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
  [ComponentMenu("Tools/Infomode/", new Type[] {})]
  public class BuildingStatusInfomodePrefab : GradientInfomodeBasePrefab
  {
    public BuildingStatusType m_Type;
    public Bounds1 m_Range;

    public override string infomodeTypeLocaleKey => "BuildingColor";

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<InfoviewBuildingStatusData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<InfoviewBuildingStatusData>(entity, new InfoviewBuildingStatusData()
      {
        m_Type = this.m_Type,
        m_Range = this.m_Range
      });
    }
  }
}
