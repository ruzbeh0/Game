// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Serialization;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public class TutorialTriggerSystem : GameSystemBase, IPreDeserialize
  {
    private readonly List<TutorialTriggerSystemBase> m_Systems = new List<TutorialTriggerSystemBase>();
    private EntityQuery m_TriggerQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_Systems.Add((TutorialTriggerSystemBase) this.World.GetOrCreateSystemManaged<TutorialObjectPlacementTriggerSystem>());
      this.m_Systems.Add((TutorialTriggerSystemBase) this.World.GetOrCreateSystemManaged<TutorialInputTriggerSystem>());
      this.m_Systems.Add((TutorialTriggerSystemBase) this.World.GetOrCreateSystemManaged<TutorialAreaTriggerSystem>());
      this.m_Systems.Add((TutorialTriggerSystemBase) this.World.GetOrCreateSystemManaged<TutorialObjectSelectionTriggerSystem>());
      this.m_Systems.Add((TutorialTriggerSystemBase) this.World.GetOrCreateSystemManaged<TutorialUpgradeTriggerSystem>());
      this.m_Systems.Add((TutorialTriggerSystemBase) this.World.GetOrCreateSystemManaged<TutorialUITriggerSystem>());
      this.m_Systems.Add((TutorialTriggerSystemBase) this.World.GetOrCreateSystemManaged<TutorialPolicyAdjustmentTriggerSystem>());
      this.m_Systems.Add((TutorialTriggerSystemBase) this.World.GetOrCreateSystemManaged<TutorialZoningTriggerSystem>());
      this.m_TriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialTriggerData>());
      this.Enabled = false;
    }

    protected override void OnGamePreload(Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      foreach (TutorialTriggerSystemBase system in this.m_Systems)
      {
        try
        {
          system.Update();
        }
        catch (Exception ex)
        {
          COSystemBase.baseLog.Critical(ex);
        }
      }
    }

    public void PreDeserialize(Context context)
    {
      this.EntityManager.RemoveComponent<TriggerActive>(this.m_TriggerQuery);
      this.EntityManager.RemoveComponent<TriggerPreCompleted>(this.m_TriggerQuery);
      this.EntityManager.RemoveComponent<TriggerCompleted>(this.m_TriggerQuery);
      this.EntityManager.RemoveComponent<TutorialNextPhase>(this.m_TriggerQuery);
    }

    [Preserve]
    public TutorialTriggerSystem()
    {
    }
  }
}
