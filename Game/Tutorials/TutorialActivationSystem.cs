// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialActivationSystem
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
  public class TutorialActivationSystem : GameSystemBase
  {
    private readonly List<GameSystemBase> m_Systems = new List<GameSystemBase>();

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_Systems.Add((GameSystemBase) this.World.GetOrCreateSystemManaged<TutorialUIActivationSystem>());
      this.m_Systems.Add((GameSystemBase) this.World.GetOrCreateSystemManaged<TutorialAutoActivationSystem>());
      this.m_Systems.Add((GameSystemBase) this.World.GetOrCreateSystemManaged<TutorialControlSchemeActivationSystem>());
      this.m_Systems.Add((GameSystemBase) this.World.GetOrCreateSystemManaged<TutorialObjectSelectedActivationSystem>());
      this.m_Systems.Add((GameSystemBase) this.World.GetOrCreateSystemManaged<TutorialInfoviewActivationSystem>());
      this.m_Systems.Add((GameSystemBase) this.World.GetOrCreateSystemManaged<TutorialFireActivationSystem>());
      this.m_Systems.Add((GameSystemBase) this.World.GetOrCreateSystemManaged<TutorialHealthProblemActivationSystem>());
      this.m_Systems.Add((GameSystemBase) this.World.GetOrCreateSystemManaged<TutorialEventActivationSystem>());
      this.Enabled = false;
    }

    protected override void OnGamePreload(Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame() || mode.IsEditor();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      foreach (GameSystemBase system in this.m_Systems)
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
    public TutorialActivationSystem()
    {
    }
  }
}
