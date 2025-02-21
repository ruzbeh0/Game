// Decompiled with JetBrains decompiler
// Type: Game.Triggers.LifePathEventCreationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Triggers
{
  public struct LifePathEventCreationData
  {
    public TriggerType m_TriggerType;
    public Entity m_EventPrefab;
    public Entity m_Sender;
    public Entity m_Target;
    public Entity m_OriginalSender;
  }
}
