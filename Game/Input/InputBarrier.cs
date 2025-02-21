// Decompiled with JetBrains decompiler
// Type: Game.Input.InputBarrier
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.Input
{
  public class InputBarrier : IDisposable
  {
    private readonly ProxyActionMap[] m_Maps;
    private readonly ProxyAction[] m_Actions;
    private readonly string m_Name;
    private bool m_Blocked;
    private InputManager.DeviceType m_Mask;
    private bool m_Disposed;

    public string name => this.m_Name;

    public IReadOnlyList<ProxyActionMap> maps => (IReadOnlyList<ProxyActionMap>) this.m_Maps;

    public IReadOnlyList<ProxyAction> actions => (IReadOnlyList<ProxyAction>) this.m_Actions;

    public InputBarrier(
      string barrierName,
      ProxyActionMap map,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool blocked = false)
    {
      this.m_Name = barrierName ?? nameof (InputBarrier);
      ProxyActionMap[] proxyActionMapArray = new ProxyActionMap[1];
      proxyActionMapArray[0] = map ?? throw new ArgumentNullException(nameof (map));
      this.m_Maps = proxyActionMapArray;
      this.m_Actions = Array.Empty<ProxyAction>();
      this.m_Mask = mask;
      map.m_Barriers.Add(this);
      this.blocked = blocked;
    }

    public InputBarrier(
      string barrierName,
      ProxyAction action,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool blocked = false)
    {
      this.m_Name = barrierName ?? nameof (InputBarrier);
      ProxyAction[] proxyActionArray = new ProxyAction[1];
      proxyActionArray[0] = action ?? throw new ArgumentNullException(nameof (action));
      this.m_Actions = proxyActionArray;
      this.m_Maps = Array.Empty<ProxyActionMap>();
      this.m_Mask = mask;
      action.m_Barriers.Add(this);
      this.blocked = blocked;
    }

    public InputBarrier(
      string barrierName,
      IList<ProxyActionMap> maps,
      IList<ProxyAction> actions,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool blocked = false)
    {
      if (maps == null)
        throw new ArgumentNullException(nameof (maps));
      if (actions == null)
        throw new ArgumentNullException(nameof (actions));
      this.m_Name = barrierName ?? nameof (InputBarrier);
      this.m_Maps = maps.Where<ProxyActionMap>((Func<ProxyActionMap, bool>) (m => m != null)).Distinct<ProxyActionMap>().ToArray<ProxyActionMap>();
      this.m_Actions = actions.Where<ProxyAction>((Func<ProxyAction, bool>) (a => a != null)).Distinct<ProxyAction>().ToArray<ProxyAction>();
      this.m_Mask = mask;
      foreach (ProxyActionMap map in this.m_Maps)
        map.m_Barriers.Add(this);
      foreach (ProxyAction action in this.m_Actions)
        action.m_Barriers.Add(this);
      this.blocked = blocked;
    }

    public InputBarrier(
      string barrierName,
      IList<ProxyActionMap> maps,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool blocked = false)
      : this(barrierName, maps, (IList<ProxyAction>) Array.Empty<ProxyAction>(), mask, blocked)
    {
    }

    public InputBarrier(
      string barrierName,
      IList<ProxyAction> actions,
      InputManager.DeviceType mask = InputManager.DeviceType.All,
      bool blocked = false)
      : this(barrierName, (IList<ProxyActionMap>) Array.Empty<ProxyActionMap>(), actions, mask, blocked)
    {
    }

    public bool blocked
    {
      get => this.m_Blocked;
      set
      {
        if (this.m_Disposed || value == this.m_Blocked)
          return;
        this.m_Blocked = value;
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
      foreach (ProxyActionMap map in this.m_Maps)
        map.UpdateState();
      foreach (ProxyAction action in this.m_Actions)
        action.UpdateState();
    }

    public void Dispose()
    {
      if (this.m_Disposed)
        return;
      this.m_Disposed = true;
      foreach (ProxyActionMap map in this.m_Maps)
      {
        map.m_Barriers.Remove(this);
        map.UpdateState();
      }
      foreach (ProxyAction action in this.m_Actions)
      {
        action.m_Barriers.Remove(this);
        action.UpdateState();
      }
    }
  }
}
