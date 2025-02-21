// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WorkProviderParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct WorkProviderParameterData : IComponentData, IQueryTypeParameter
  {
    public Entity m_UneducatedNotificationPrefab;
    public Entity m_EducatedNotificationPrefab;
    public short m_UneducatedNotificationDelay;
    public short m_EducatedNotificationDelay;
    public float m_UneducatedNotificationLimit;
    public float m_EducatedNotificationLimit;
    public int m_SeniorEmployeeLevel;
  }
}
