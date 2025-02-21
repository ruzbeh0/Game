// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Emissive
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Rendering
{
  [InternalBufferCapacity(1)]
  public struct Emissive : IBufferElementData, IEmptySerializable
  {
    public NativeHeapBlock m_BufferAllocation;
    public int m_LightOffset;
    public bool m_Updated;
  }
}
