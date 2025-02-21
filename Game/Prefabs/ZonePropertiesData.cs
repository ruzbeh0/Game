// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZonePropertiesData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ZonePropertiesData : IComponentData, IQueryTypeParameter
  {
    public bool m_ScaleResidentials;
    public float m_ResidentialProperties;
    public float m_SpaceMultiplier;
    public Resource m_AllowedSold;
    public Resource m_AllowedManufactured;
    public Resource m_AllowedStored;
    public float m_FireHazardModifier;
  }
}
