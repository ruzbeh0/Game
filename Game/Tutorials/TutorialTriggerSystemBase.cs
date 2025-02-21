// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialTriggerSystemBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public abstract class TutorialTriggerSystemBase : GameSystemBase
  {
    protected ModificationBarrier5 m_BarrierSystem;
    protected EntityQuery m_ActiveTriggerQuery;
    private TutorialSystem m_TutorialSystem;
    private Entity m_LastPhase;

    protected bool triggersChanged { get; private set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_BarrierSystem = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      this.m_TutorialSystem = this.World.GetOrCreateSystemManaged<TutorialSystem>();
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      this.m_LastPhase = Entity.Null;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      Entity activeTutorialPhase = this.m_TutorialSystem.activeTutorialPhase;
      if (activeTutorialPhase != this.m_LastPhase)
      {
        this.m_LastPhase = activeTutorialPhase;
        this.triggersChanged = true;
      }
      else
        this.triggersChanged = false;
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      base.OnStopRunning();
      this.m_LastPhase = Entity.Null;
    }

    [Preserve]
    protected TutorialTriggerSystemBase()
    {
    }
  }
}
