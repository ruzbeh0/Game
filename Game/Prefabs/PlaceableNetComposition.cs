// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlaceableNetComposition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PlaceableNetComposition : IComponentData, IQueryTypeParameter
  {
    public uint m_ConstructionCost;
    public uint m_ElevationCost;
    public float m_UpkeepCost;
  }
}
