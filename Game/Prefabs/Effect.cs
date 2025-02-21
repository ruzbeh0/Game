// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Effect
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct Effect : IBufferElementData
  {
    public Entity m_Effect;
    public float3 m_Position;
    public float3 m_Scale;
    public quaternion m_Rotation;
    public int2 m_BoneIndex;
    public float m_Intensity;
    public int m_ParentMesh;
    public int m_AnimationIndex;
    public bool m_Procedural;
  }
}
