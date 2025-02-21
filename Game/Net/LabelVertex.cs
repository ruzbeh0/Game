// Decompiled with JetBrains decompiler
// Type: Game.Net.LabelVertex
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Net
{
  [InternalBufferCapacity(0)]
  public struct LabelVertex : IBufferElementData, IEmptySerializable
  {
    public float3 m_Position;
    public float2 m_UV0;
    public float2 m_UV1;
    public int2 m_Material;
    public Color32 m_Color;
  }
}
