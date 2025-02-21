// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PoliceConfigurationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PoliceConfigurationData : IComponentData, IQueryTypeParameter
  {
    public Entity m_PoliceServicePrefab;
    public Entity m_TrafficAccidentNotificationPrefab;
    public Entity m_CrimeSceneNotificationPrefab;
    public float m_MaxCrimeAccumulation;
    public float m_CrimeAccumulationTolerance;
    public int m_HomeCrimeEffect;
    public int m_WorkplaceCrimeEffect;
    public float m_WelfareCrimeRecurrenceFactor;
    public float m_CrimeIncreaseMultiplier;
    public float m_CrimePopulationReduction;
  }
}
