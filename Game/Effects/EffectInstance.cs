// Decompiled with JetBrains decompiler
// Type: Game.Effects.EffectInstance
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Effects
{
  public struct EffectInstance : IComponentData, IQueryTypeParameter
  {
    public float3 m_Position;
    public quaternion m_Rotation;
    public float m_Intensity;
  }
}
