// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectSubNets
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class ObjectSubNets : ComponentBase
  {
    public NetInvertMode m_InvertWhen;
    public ObjectSubNetInfo[] m_SubNets;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_SubNets == null)
        return;
      for (int index = 0; index < this.m_SubNets.Length; ++index)
        prefabs.Add((PrefabBase) this.m_SubNets[index].m_NetPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SubNet>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Net.SubNet>());
    }
  }
}
