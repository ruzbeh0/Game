// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetPiecePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Piece/", new Type[] {})]
  public class NetPiecePrefab : RenderPrefab
  {
    public NetPieceLayer m_Layer;
    public float m_Width = 3f;
    public float m_Length = 64f;
    public Bounds1 m_HeightRange = new Bounds1(0.0f, 3f);
    public float m_WidthOffset;
    public float m_NodeOffset = 0.5f;
    public float4 m_SurfaceHeights = (float4) 0.0f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<NetPieceData>());
      components.Add(ComponentType.ReadWrite<MeshMaterial>());
    }
  }
}
