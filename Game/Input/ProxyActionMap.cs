// Decompiled with JetBrains decompiler
// Type: Game.Input.ProxyActionMap
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Input
{
  [DebuggerDisplay("{name}")]
  public class ProxyActionMap
  {
    private readonly InputActionMap m_SourceMap;
    private readonly Dictionary<string, ProxyAction> m_Actions = new Dictionary<string, ProxyAction>();
    internal HashSet<InputBarrier> m_Barriers = new HashSet<InputBarrier>();
    private bool m_Enabled;
    private InputManager.DeviceType m_Mask = InputManager.DeviceType.All;

    internal InputActionMap sourceMap => this.m_SourceMap;

    public string name => this.m_SourceMap.name;

    public IReadOnlyDictionary<string, ProxyAction> actions
    {
      get => (IReadOnlyDictionary<string, ProxyAction>) this.m_Actions;
    }

    internal IReadOnlyCollection<InputBarrier> barriers
    {
      get => (IReadOnlyCollection<InputBarrier>) this.m_Barriers;
    }

    public IEnumerable<ProxyBinding> bindings
    {
      get
      {
        foreach (KeyValuePair<string, ProxyAction> action in this.m_Actions)
        {
          string str;
          ProxyAction proxyAction;
          action.Deconstruct(ref str, ref proxyAction);
          foreach (KeyValuePair<InputManager.DeviceType, ProxyComposite> composite in (IEnumerable<KeyValuePair<InputManager.DeviceType, ProxyComposite>>) proxyAction.composites)
          {
            InputManager.DeviceType deviceType;
            ProxyComposite proxyComposite;
            composite.Deconstruct(ref deviceType, ref proxyComposite);
            foreach (KeyValuePair<ActionComponent, ProxyBinding> binding1 in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite.bindings)
            {
              ActionComponent actionComponent;
              ProxyBinding binding2;
              binding1.Deconstruct(ref actionComponent, ref binding2);
              yield return binding2;
            }
          }
        }
      }
    }

    public bool enabled => this.m_Enabled;

    public InputManager.DeviceType mask
    {
      get => this.m_Mask;
      internal set
      {
        if (value == this.m_Mask)
          return;
        this.m_Mask = value;
        this.m_SourceMap.bindingMask = value.ToInputBinding();
        foreach (KeyValuePair<string, ProxyAction> action in this.m_Actions)
        {
          string str;
          ProxyAction proxyAction;
          action.Deconstruct(ref str, ref proxyAction);
          proxyAction.UpdateState();
        }
      }
    }

    internal ProxyActionMap(InputActionMap sourceMap) => this.m_SourceMap = sourceMap;

    internal void InitActions()
    {
      foreach (InputAction action in this.sourceMap.actions)
      {
        ProxyAction proxyAction = new ProxyAction(this, action);
        this.m_Actions.Add(proxyAction.name, proxyAction);
      }
      this.UpdateState();
    }

    public ProxyAction FindAction(string name)
    {
      return this.m_Actions.GetValueOrDefault<string, ProxyAction>(name);
    }

    internal ProxyAction FindAction(InputAction action) => this.FindAction(action.name);

    public bool TryFindAction(string name, out ProxyAction action)
    {
      return this.m_Actions.TryGetValue(name, out action);
    }

    public ProxyAction AddAction(ProxyAction.Info actionInfo, bool bulk = false)
    {
      using (Colossal.PerformanceCounter.Start((Action<TimeSpan>) (t => InputManager.log.InfoFormat("Action \"{1}\" added in {0}ms", (object) t.TotalMilliseconds, (object) actionInfo.m_Name))))
      {
        using (InputManager.DeferUpdating())
        {
          ProxyAction action1;
          if (this.TryFindAction(actionInfo.m_Name, out action1))
            return action1;
          InputAction inputAction = this.m_SourceMap.AddAction(actionInfo.m_Name, actionInfo.m_Type.GetInputActionType(), expectedControlLayout: actionInfo.m_Type.GetExpectedControlLayout());
          foreach (ProxyComposite.Info composite in actionInfo.m_Composites)
            InputManager.instance.CreateCompositeBinding(inputAction, composite);
          ProxyAction action2 = new ProxyAction(this, inputAction);
          this.m_Actions.Add(action2.name, action2);
          InputManager.instance.InitializeMasks(action2);
          return action2;
        }
      }
    }

    internal void UpdateState()
    {
      bool flag = this.m_Barriers.All<InputBarrier>((Func<InputBarrier, bool>) (b => !b.blocked));
      if (flag == this.m_Enabled)
        return;
      this.m_Enabled = flag;
      foreach (KeyValuePair<string, ProxyAction> action in this.m_Actions)
      {
        string str;
        ProxyAction proxyAction;
        action.Deconstruct(ref str, ref proxyAction);
        proxyAction.UpdateState();
      }
    }
  }
}
