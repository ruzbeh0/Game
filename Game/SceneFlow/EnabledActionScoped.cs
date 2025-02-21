// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.EnabledActionScoped
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using System;
using Unity.Assertions;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.SceneFlow
{
  public class EnabledActionScoped : IDisposable
  {
    private readonly OverlayBindings m_Bindings;
    private readonly ProxyAction m_Proxy;
    private readonly DisplayNameOverride m_NameOverride;
    private readonly Func<OverlayScreen, bool> m_ShouldBeEnabled;

    public EnabledActionScoped(
      GameManager manager,
      string actionMapName,
      string actionName,
      Func<OverlayScreen, bool> shouldBeEnabled = null,
      string displayProperty = null,
      int displayPriority = 20)
    {
      this.m_Proxy = manager.inputManager.FindAction(actionMapName, actionName);
      this.m_Bindings = manager.userInterface.overlayBindings;
      this.m_NameOverride = new DisplayNameOverride(nameof (EnabledActionScoped), this.m_Proxy, displayProperty, displayPriority);
      Assert.IsNotNull<ProxyAction>(this.m_Proxy);
      this.m_ShouldBeEnabled = shouldBeEnabled;
      this.m_Bindings.onScreenActivated += new Action<OverlayScreen>(this.HandleScreenChange);
    }

    private void HandleScreenChange(OverlayScreen screen)
    {
      bool flag = this.m_ShouldBeEnabled == null || this.m_ShouldBeEnabled(screen);
      this.m_Proxy.enabled = flag;
      this.m_NameOverride.active = flag;
    }

    public void Dispose()
    {
      this.m_Bindings.onScreenActivated -= new Action<OverlayScreen>(this.HandleScreenChange);
      this.m_Proxy.enabled = false;
      this.m_NameOverride.Dispose();
    }

    public static implicit operator InputAction(EnabledActionScoped scoped)
    {
      return scoped.m_Proxy.sourceAction;
    }
  }
}
