// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Bone
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [InternalBufferCapacity(0)]
  public struct Bone : IBufferElementData, IEmptySerializable
  {
    public float3 m_Position;
    public quaternion m_Rotation;
    public float3 m_Scale;
  }
}
