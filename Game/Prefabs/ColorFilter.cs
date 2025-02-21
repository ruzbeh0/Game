// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ColorFilter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(2)]
  public struct ColorFilter : IBufferElementData
  {
    public ColorGroupID m_GroupID;
    public float3 m_OverrideAlpha;
    public Entity m_EntityFilter;
    public ColorFilterFlags m_Flags;
    public AgeMask m_AgeFilter;
    public GenderMask m_GenderFilter;
    public sbyte m_OverrideProbability;
  }
}
