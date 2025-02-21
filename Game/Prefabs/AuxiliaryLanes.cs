// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AuxiliaryLanes
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new Type[] {typeof (NetLanePrefab)})]
  public class AuxiliaryLanes : ComponentBase
  {
    public AuxiliaryLaneInfo[] m_AuxiliaryLanes;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_AuxiliaryLanes.Length; ++index)
        prefabs.Add((PrefabBase) this.m_AuxiliaryLanes[index].m_Lane);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<AuxiliaryNetLane>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }
  }
}
