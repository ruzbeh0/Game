// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MilestonePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public class MilestonePrefab : PrefabBase
  {
    public int m_Index;
    public int m_Reward;
    public int m_DevTreePoints;
    public int m_MapTiles;
    public int m_LoanLimit;
    public int m_XpRequried;
    public bool m_Major;
    public string m_Image;
    public Color m_BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    public Color m_AccentColor = new Color(0.18f, 0.235f, 0.337f, 1f);
    public Color m_TextColor = Color.white;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<MilestoneData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<MilestoneData>(entity, new MilestoneData()
      {
        m_Index = this.m_Index,
        m_Reward = this.m_Reward,
        m_DevTreePoints = this.m_DevTreePoints,
        m_MapTiles = this.m_MapTiles,
        m_LoanLimit = this.m_LoanLimit,
        m_XpRequried = this.m_XpRequried,
        m_Major = this.m_Major
      });
    }
  }
}
