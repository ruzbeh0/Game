// Decompiled with JetBrains decompiler
// Type: Game.Areas.Batch
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Areas
{
  public struct Batch : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public NativeHeapBlock m_BatchAllocation;
    public int m_AllocatedSize;
    public int m_BatchIndex;
    public int m_VisibleCount;
    public int m_MetaIndex;
  }
}
