// Decompiled with JetBrains decompiler
// Type: Game.Rendering.MeshGroup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Rendering
{
  [InternalBufferCapacity(1)]
  public struct MeshGroup : IBufferElementData, IEmptySerializable
  {
    public ushort m_SubMeshGroup;
    public byte m_MeshOffset;
    public byte m_ColorOffset;
  }
}
