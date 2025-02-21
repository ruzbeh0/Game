// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DisasterConfigurationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct DisasterConfigurationData : IComponentData, IQueryTypeParameter
  {
    public Entity m_WeatherDamageNotificationPrefab;
    public Entity m_WeatherDestroyedNotificationPrefab;
    public Entity m_WaterDamageNotificationPrefab;
    public Entity m_WaterDestroyedNotificationPrefab;
    public Entity m_DestroyedNotificationPrefab;
    public float m_FloodDamageRate;
    public AnimationCurve1 m_EmergencyShelterDangerLevelExitProbability;
    public float m_InoperableEmergencyShelterExitProbability;
  }
}
