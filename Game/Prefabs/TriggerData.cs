// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TriggerData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Triggers;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TriggerData : IBufferElementData
  {
    public TriggerType m_TriggerType;
    public TargetType m_TargetTypes;
    public Entity m_TriggerPrefab;
  }
}
