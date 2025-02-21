// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TerrainPropertiesPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public class TerrainPropertiesPrefab : PrefabBase
  {
    public WaterSystem.WaterSource[] m_WaterSources;
    public int m_WaterSourceSteps;
    public int m_WaterVelocitySteps;
    public int m_WaterDepthSteps;
    public int m_WaterMaxSpeed;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TerrainPropertiesData>());
    }
  }
}
