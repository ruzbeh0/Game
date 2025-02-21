// Decompiled with JetBrains decompiler
// Type: Game.GameSystemBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.SceneFlow;
using Game.Serialization;
using System;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game
{
  public abstract class GameSystemBase : COSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      if (this.World == World.DefaultGameObjectInjectionWorld)
      {
        this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
        this.m_LoadGameSystem.onOnSaveGameLoaded += new LoadGameSystem.EventGameLoaded(this.GameLoaded);
      }
      GameManager.instance.onGamePreload += new GameManager.EventGamePreload(this.GamePreload);
      GameManager.instance.onGameLoadingComplete += new GameManager.EventGamePreload(this.GameLoadingComplete);
      Application.focusChanged += new Action<bool>(this.FocusChanged);
    }

    private void FocusChanged(bool hasfocus)
    {
      try
      {
        this.OnFocusChanged(hasfocus);
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex, (object) (this.GetType().Name + ": Error on Focus change"));
      }
    }

    [Preserve]
    protected override void OnDestroy()
    {
      GameManager.instance.onGamePreload -= new GameManager.EventGamePreload(this.GamePreload);
      GameManager.instance.onGameLoadingComplete -= new GameManager.EventGamePreload(this.GameLoadingComplete);
      if (this.World == World.DefaultGameObjectInjectionWorld && this.m_LoadGameSystem != null)
        this.m_LoadGameSystem.onOnSaveGameLoaded -= new LoadGameSystem.EventGameLoaded(this.GameLoaded);
      Application.focusChanged -= new Action<bool>(this.FocusChanged);
      base.OnDestroy();
    }

    private void GameLoadingComplete(Purpose purpose, GameMode mode)
    {
      try
      {
        this.OnGameLoadingComplete(purpose, mode);
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex, (object) (this.GetType().Name + ": Error on state change, disabling system..."));
      }
    }

    private void GameLoaded(Context serializationContext)
    {
      try
      {
        this.OnGameLoaded(serializationContext);
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex, (object) (this.GetType().Name + ": Error on game load, disabling system..."));
        this.Enabled = false;
      }
    }

    private void GamePreload(Purpose purpose, GameMode mode)
    {
      try
      {
        this.OnGamePreload(purpose, mode);
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Error(ex, (object) (this.GetType().Name + ": Error on game preload, disabling system..."));
        this.Enabled = false;
      }
    }

    protected virtual void OnGamePreload(Purpose purpose, GameMode mode)
    {
    }

    protected virtual void OnGameLoaded(Context serializationContext)
    {
    }

    protected virtual void OnGameLoadingComplete(Purpose purpose, GameMode mode)
    {
    }

    protected virtual void OnFocusChanged(bool hasFocus)
    {
    }

    public virtual int GetUpdateInterval(SystemUpdatePhase phase) => 1;

    public virtual int GetUpdateOffset(SystemUpdatePhase phase) => -1;

    public void ResetDependency() => this.Dependency = new JobHandle();

    [Preserve]
    protected GameSystemBase()
    {
    }
  }
}
