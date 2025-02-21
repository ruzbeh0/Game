// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlaceableNetData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PlaceableNetData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Bounds1 m_ElevationRange;
    public Entity m_UndergroundPrefab;
    public PlacementFlags m_PlacementFlags;
    public CompositionFlags m_SetUpgradeFlags;
    public CompositionFlags m_UnsetUpgradeFlags;
    public uint m_DefaultConstructionCost;
    public float m_DefaultUpkeepCost;
    public float m_SnapDistance;
    public int m_XPReward;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_PlacementFlags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      this.m_PlacementFlags = (PlacementFlags) num;
      this.m_SnapDistance = 8f;
    }
  }
}
