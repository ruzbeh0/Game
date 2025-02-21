// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ElectricityParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ElectricityParameterData : IComponentData, IQueryTypeParameter
  {
    public float m_InitialBatteryCharge;
    public AnimationCurve1 m_TemperatureConsumptionMultiplier;
    public float m_CloudinessSolarPenalty;
    public Entity m_ElectricityServicePrefab;
    public Entity m_ElectricityNotificationPrefab;
    public Entity m_LowVoltageNotConnectedPrefab;
    public Entity m_HighVoltageNotConnectedPrefab;
    public Entity m_BottleneckNotificationPrefab;
    public Entity m_BuildingBottleneckNotificationPrefab;
    public Entity m_NotEnoughProductionNotificationPrefab;
    public Entity m_TransformerNotificationPrefab;
    public Entity m_NotEnoughConnectedNotificationPrefab;
    public Entity m_BatteryEmptyNotificationPrefab;
  }
}
