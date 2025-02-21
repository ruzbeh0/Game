// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.PolicyAdjustmentTriggerData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Tutorials
{
  public struct PolicyAdjustmentTriggerData : IComponentData, IQueryTypeParameter
  {
    public PolicyAdjustmentTriggerFlags m_Flags;
    public PolicyAdjustmentTriggerTargetFlags m_TargetFlags;

    public PolicyAdjustmentTriggerData(
      PolicyAdjustmentTriggerFlags flags,
      PolicyAdjustmentTriggerTargetFlags targetFlags)
    {
      this.m_Flags = flags;
      this.m_TargetFlags = targetFlags;
    }
  }
}
