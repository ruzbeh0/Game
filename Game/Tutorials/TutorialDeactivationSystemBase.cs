// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialDeactivationSystemBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public abstract class TutorialDeactivationSystemBase : GameSystemBase
  {
    private EntityQuery m_ActivePhaseQuery;
    protected EntityCommandBufferSystem m_BarrierSystem;

    protected bool phaseCanDeactivate => !this.m_ActivePhaseQuery.IsEmptyIgnoreFilter;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_BarrierSystem = (EntityCommandBufferSystem) this.World.GetOrCreateSystemManaged<ModificationBarrier3>();
      this.m_ActivePhaseQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialPhaseData>(), ComponentType.ReadOnly<TutorialPhaseActive>(), ComponentType.ReadOnly<TutorialPhaseCanDeactivate>());
    }

    [Preserve]
    protected TutorialDeactivationSystemBase()
    {
    }
  }
}
