// Decompiled with JetBrains decompiler
// Type: Game.UI.UISystemBootstrapper
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using cohtml.Net;
using Colossal.PSI.Environment;
using Colossal.UI;
using Game.Input;
using Game.SceneFlow;
using Game.UI.Localization;
using System;
using UnityEngine;

#nullable disable
namespace Game.UI
{
  public class UISystemBootstrapper : MonoBehaviour, IUIViewComponent
  {
    private UIView m_View;
    private InputManager m_InputManager;
    private UIManager m_UIManager;
    private UIInputSystem m_UIInputSystem;
    private InputBindings m_InputBindings;
    public string m_Url;

    public View View => this.m_View.View;

    public IUnityViewListener Listener => (IUnityViewListener) this.m_View.Listener;

    private void Awake()
    {
      Debug.LogWarning((object) "UISystemBootstrapper is only meant for development purpose");
      UIManager.log.Info((object) "Bootstrapping cohtmlUISystem");
      this.m_InputManager = InputManager.instance;
      Colossal.UI.UISystem.Settings settings = Colossal.UI.UISystem.Settings.New with
      {
        enableDebugger = true
      };
      if ((UnityEngine.Object) GameManager.instance != (UnityEngine.Object) null)
        settings.localizationManager = (ILocalizationManager) new UILocalizationManager(GameManager.instance.localizationManager);
      settings.resourceHandler = (IResourceHandler) new GameUIResourceHandler((MonoBehaviour) this);
      settings.enableDebugger = true;
      this.m_UIManager = UIManager.instance;
      this.m_UIManager.CreateUISystem(settings).AddHostLocation("gameui", EnvPath.kContentPath + "Game/~UI~");
      this.m_View = UIManager.defaultUISystem.CreateView(this.m_Url, UIView.Settings.New, this.GetComponent<Camera>());
      this.m_View.enabled = true;
      this.m_View.Listener.ReadyForBindings += new Action(this.OnReadyForBindings);
      this.m_UIInputSystem = new UIInputSystem(UIManager.defaultUISystem);
      this.m_InputBindings = new InputBindings();
    }

    private void Update()
    {
      this.m_InputManager.Update();
      this.m_UIManager.Update();
      this.m_InputBindings.Update();
    }

    private void LateUpdate() => this.m_UIInputSystem.DispatchInputEvents();

    private void OnReadyForBindings() => this.m_InputBindings.Attach(this.m_View.View);

    private void OnDestroy()
    {
      this.m_View.Listener.ReadyForBindings -= new Action(this.OnReadyForBindings);
      this.m_InputBindings.Detach();
      this.m_InputBindings.Dispose();
      this.m_UIInputSystem.Dispose();
      this.m_UIManager.Dispose();
      this.m_InputManager.Dispose();
    }

    bool IUIViewComponent.get_enabled() => this.enabled;
  }
}
