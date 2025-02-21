// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialDeactivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public class TutorialDeactivationSystem : GameSystemBase
  {
    private List<TutorialDeactivationSystemBase> m_Systems = new List<TutorialDeactivationSystemBase>();

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_Systems.Add((TutorialDeactivationSystemBase) this.World.GetOrCreateSystemManaged<TutorialControlSchemeDeactivationSystem>());
      this.m_Systems.Add((TutorialDeactivationSystemBase) this.World.GetOrCreateSystemManaged<TutorialUIDeactivationSystem>());
      this.m_Systems.Add((TutorialDeactivationSystemBase) this.World.GetOrCreateSystemManaged<TutorialObjectSelectionDeactivationSystem>());
      this.m_Systems.Add((TutorialDeactivationSystemBase) this.World.GetOrCreateSystemManaged<TutorialInfoviewDeactivationSystem>());
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
      foreach (TutorialDeactivationSystemBase system in this.m_Systems)
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

    [Preserve]
    public TutorialDeactivationSystem()
    {
    }
  }
}
