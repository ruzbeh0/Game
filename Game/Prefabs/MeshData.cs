// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Rendering;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct MeshData : IComponentData, IQueryTypeParameter
  {
    public Bounds3 m_Bounds;
    public MeshFlags m_State;
    public DecalLayers m_DecalLayer;
    public MeshLayer m_DefaultLayers;
    public MeshLayer m_AvailableLayers;
    public MeshType m_AvailableTypes;
    public byte m_MinLod;
    public byte m_ShadowLod;
    public float m_LodBias;
    public float m_ShadowBias;
    public float m_SmoothingDistance;
    public int m_SubMeshCount;
    public int m_IndexCount;
    public int m_TilingCount;
  }
}
