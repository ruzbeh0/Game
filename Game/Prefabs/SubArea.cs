// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SubArea
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(1)]
  public struct SubArea : IBufferElementData
  {
    public Entity m_Prefab;
    public int2 m_NodeRange;
  }
}
