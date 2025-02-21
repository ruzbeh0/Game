// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HouseholdData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct HouseholdData : IComponentData, IQueryTypeParameter
  {
    public int m_InitialWealthRange;
    public int m_InitialWealthOffset;
    public int m_InitialCarProbability;
    public int m_ChildCount;
    public int m_AdultCount;
    public int m_ElderCount;
    public int m_StudentCount;
    public int m_FirstPetProbability;
    public int m_NextPetProbability;
    public int m_Weight;
  }
}
