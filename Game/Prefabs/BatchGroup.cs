// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BatchGroup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(4)]
  public struct BatchGroup : IBufferElementData
  {
    public int m_GroupIndex;
    public int m_MergeIndex;
    public MeshLayer m_Layer;
    public MeshType m_Type;
    public ushort m_Partition;
  }
}
