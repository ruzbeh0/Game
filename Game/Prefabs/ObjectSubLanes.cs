// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectSubLanes
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab)})]
  public class ObjectSubLanes : ComponentBase
  {
    public ObjectSubLaneInfo[] m_SubLanes;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_SubLanes == null)
        return;
      for (int index = 0; index < this.m_SubLanes.Length; ++index)
        prefabs.Add((PrefabBase) this.m_SubLanes[index].m_LanePrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SubLane>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      bool flag = false;
      if (this.m_SubLanes != null)
      {
        for (int index = 0; index < this.m_SubLanes.Length; ++index)
        {
          ObjectSubLaneInfo subLane = this.m_SubLanes[index];
          if (subLane.m_NodeIndex.x != subLane.m_NodeIndex.y)
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return;
      components.Add(ComponentType.ReadWrite<Game.Net.SubLane>());
    }
  }
}
