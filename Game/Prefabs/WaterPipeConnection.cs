// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WaterPipeConnection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new Type[] {typeof (NetPrefab)})]
  public class WaterPipeConnection : ComponentBase
  {
    public int m_FreshCapacity = 1073741823;
    public int m_SewageCapacity = 1073741823;
    public int m_StormCapacity;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WaterPipeConnectionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!components.Contains(ComponentType.ReadWrite<Edge>()))
        return;
      components.Add(ComponentType.ReadWrite<Game.Net.WaterPipeConnection>());
    }
  }
}
