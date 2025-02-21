// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingModifierData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct BuildingModifierData : IBufferElementData
  {
    public BuildingModifierType m_Type;
    public ModifierValueMode m_Mode;
    public Bounds1 m_Range;

    public BuildingModifierData(BuildingModifierType type, ModifierValueMode mode, Bounds1 range)
    {
      this.m_Type = type;
      this.m_Mode = mode;
      this.m_Range = range;
    }
  }
}
