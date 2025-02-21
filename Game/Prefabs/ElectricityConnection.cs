// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ElectricityConnection
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
  public class ElectricityConnection : ComponentBase
  {
    public ElectricityConnection.Voltage m_Voltage;
    public FlowDirection m_Direction = FlowDirection.Both;
    public int m_Capacity;
    public NetPieceRequirements[] m_RequireAll;
    public NetPieceRequirements[] m_RequireAny;
    public NetPieceRequirements[] m_RequireNone;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ElectricityConnectionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public enum Voltage : byte
    {
      Low = 0,
      High = 1,
      Invalid = 255, // 0xFF
    }
  }
}
