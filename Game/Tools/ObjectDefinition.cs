// Decompiled with JetBrains decompiler
// Type: Game.Tools.ObjectDefinition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct ObjectDefinition : IComponentData, IQueryTypeParameter
  {
    public float3 m_Position;
    public float3 m_LocalPosition;
    public float3 m_Scale;
    public quaternion m_Rotation;
    public quaternion m_LocalRotation;
    public float m_Elevation;
    public float m_Intensity;
    public float m_Age;
    public int m_ParentMesh;
    public int m_GroupIndex;
    public int m_Probability;
    public int m_PrefabSubIndex;
  }
}
