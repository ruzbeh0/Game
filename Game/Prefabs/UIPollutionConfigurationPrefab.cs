// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIPollutionConfigurationPrefab
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
  public class UIPollutionConfigurationPrefab : PrefabBase
  {
    public UIPollutionThresholds m_GroundPollution;
    public UIPollutionThresholds m_AirPollution;
    public UIPollutionThresholds m_NoisePollution;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<UIPollutionConfigurationData>());
    }
  }
}
