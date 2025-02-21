// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LodMesh
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(2)]
  public struct LodMesh : IBufferElementData, IEmptySerializable
  {
    public Entity m_LodMesh;
  }
}
