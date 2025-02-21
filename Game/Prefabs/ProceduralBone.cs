// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ProceduralBone
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct ProceduralBone : IBufferElementData
  {
    public float3 m_Position;
    public float3 m_ObjectPosition;
    public quaternion m_Rotation;
    public quaternion m_ObjectRotation;
    public float3 m_Scale;
    public float4x4 m_BindPose;
    public BoneType m_Type;
    public int m_ParentIndex;
    public int m_BindIndex;
    public int m_HierarchyDepth;
    public int m_ConnectionID;
    public float m_Speed;
    public float m_Acceleration;
  }
}
