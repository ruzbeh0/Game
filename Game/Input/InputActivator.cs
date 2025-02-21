// Decompiled with JetBrains decompiler
// Type: Game.Input.InputActivator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.Input
{
  public class InputActivator : IDisposable
  {
    private readonly ProxyAction[] m_Actions;
    private readonly string m_Name;
    private bool m_Enabled;
    private InputManager.DeviceType m_Mask;
    private bool m_Disposed;

    public string name => this.m_Name;

    public IReadOnlyList<ProxyAction> actions => (IReadOnlyList<ProxyAction>) this.m_Actions;

    public InputActivator(
      string activatorName,
      ProxyAction action,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool enabled = false)
      : this(false, activatorName, action, mask, enabled)
    {
    }

    internal InputActivator(
      bool ignoreIsBuiltIn,
      string activatorName,
      ProxyAction action,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool enabled = false)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (!ignoreIsBuiltIn && action.isBuiltIn)
        throw new ArgumentException("Activator can not be created for built-in action");
      this.m_Name = activatorName ?? nameof (InputActivator);
      this.m_Actions = new ProxyAction[1]{ action };
      this.m_Mask = mask;
      action.m_Activators.Add(this);
      this.enabled = enabled;
    }

    public InputActivator(
      string activatorName,
      IList<ProxyAction> actions,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool enabled = false)
      : this(false, activatorName, actions, mask, enabled)
    {
    }

    internal InputActivator(
      bool ignoreIsBuiltIn,
      string activatorName,
      IList<ProxyAction> actions,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool enabled = false)
    {
      this.m_Actions = actions != null ? actions.Where<ProxyAction>((Func<ProxyAction, bool>) (a => a != null)).Distinct<ProxyAction>().ToArray<ProxyAction>() : throw new ArgumentNullException(nameof (actions));
      if (!ignoreIsBuiltIn && ((IEnumerable<ProxyAction>) this.m_Actions).Any<ProxyAction>((Func<ProxyAction, bool>) (a => a.isBuiltIn)))
        throw new ArgumentException("Activator can not be created for built-in action");
      this.m_Name = activatorName ?? nameof (InputActivator);
      this.m_Mask = mask;
      foreach (ProxyAction action in this.m_Actions)
        action.m_Activators.Add(this);
      this.enabled = enabled;
    }

    public bool enabled
    {
      get => this.m_Enabled;
      set
      {
        if (this.m_Disposed || value == this.m_Enabled)
          return;
        this.m_Enabled = value;
        this.Update();
      }
    }

    public InputManager.DeviceType mask
    {
      get => this.m_Mask;
      set
      {
        if (this.m_Disposed || value == this.m_Mask)
          return;
        this.m_Mask = value;
        this.Update();
      }
    }

    private void Update()
    {
      foreach (ProxyAction action in this.m_Actions)
        action.UpdateState();
    }

    public void Dispose()
    {
      if (this.m_Disposed)
        return;
      this.m_Disposed = true;
      foreach (ProxyAction action in this.m_Actions)
      {
        action.m_Activators.Remove(this);
        action.UpdateState();
      }
    }
  }
}
