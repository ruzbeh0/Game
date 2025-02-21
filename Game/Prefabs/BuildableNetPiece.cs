// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildableNetPiece
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new Type[] {typeof (NetPiecePrefab)})]
  public class BuildableNetPiece : ComponentBase
  {
    public float3 m_Position;
    public float m_Width;
    public float3 m_SnapPosition;
    public float m_SnapWidth;
    public bool m_AllowOnBridge;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<NetPieceArea>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }
  }
}
