// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectSubAreas
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
  public class ObjectSubAreas : ComponentBase
  {
    public ObjectSubAreaInfo[] m_SubAreas;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_SubAreas == null)
        return;
      for (int index = 0; index < this.m_SubAreas.Length; ++index)
        prefabs.Add((PrefabBase) this.m_SubAreas[index].m_AreaPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SubArea>());
      components.Add(ComponentType.ReadWrite<SubAreaNode>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Areas.SubArea>());
    }
  }
}
