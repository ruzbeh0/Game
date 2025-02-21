// Decompiled with JetBrains decompiler
// Type: Game.UI.UISystemBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Logging;
using Colossal.Serialization.Entities;
using Colossal.UI;
using Colossal.UI.Binding;
using Game.SceneFlow;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI
{
  public abstract class UISystemBase : GameSystemBase
  {
    protected static ILog log = UIManager.log;
    private List<IBinding> m_Bindings;
    private List<IUpdateBinding> m_UpdateBindings;

    public virtual GameMode gameMode => GameMode.All;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_Bindings = new List<IBinding>();
      this.m_UpdateBindings = new List<IUpdateBinding>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      foreach (IBinding binding in this.m_Bindings)
        GameManager.instance.userInterface.bindings.RemoveBinding(binding);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      foreach (IUpdateBinding updateBinding in this.m_UpdateBindings)
        updateBinding.Update();
    }

    protected void AddBinding(IBinding binding)
    {
      this.m_Bindings.Add(binding);
      GameManager.instance.userInterface.bindings.AddBinding(binding);
    }

    protected void AddUpdateBinding(IUpdateBinding binding)
    {
      this.AddBinding((IBinding) binding);
      this.m_UpdateBindings.Add(binding);
    }

    protected override void OnGamePreload(Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = (this.gameMode & mode) != 0;
    }

    [Preserve]
    protected UISystemBase()
    {
    }
  }
}
