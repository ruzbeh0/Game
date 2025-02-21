// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PollutionModifierData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PollutionModifierData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<PollutionModifierData>
  {
    public float m_GroundPollutionMultiplier;
    public float m_AirPollutionMultiplier;
    public float m_NoisePollutionMultiplier;

    public void Combine(PollutionModifierData otherData)
    {
      this.m_GroundPollutionMultiplier += otherData.m_GroundPollutionMultiplier;
      this.m_AirPollutionMultiplier += otherData.m_AirPollutionMultiplier;
      this.m_NoisePollutionMultiplier += otherData.m_NoisePollutionMultiplier;
    }
  }
}
