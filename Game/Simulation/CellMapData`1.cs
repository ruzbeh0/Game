// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CellMapData`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Collections;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct CellMapData<T> where T : struct, ISerializable
  {
    public NativeArray<T> m_Buffer;
    public float2 m_CellSize;
    public int2 m_TextureSize;
  }
}
