// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.UserInterface
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using cohtml.Net;
using Colossal.Localization;
using Colossal.Logging;
using Colossal.UI;
using Colossal.UI.Binding;
using Game.Input;
using Game.PSI;
using Game.UI;
using Game.UI.Localization;
using Game.UI.Menu;
using System;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace Game.SceneFlow
{
  public class UserInterface : IDisposable
  {
    private static ILog log = UIManager.log;
    private UICursorCollection m_CursorCollection;
    private CompositeBinding m_Bindings;
    private TaskCompletionSource<bool> m_BindingsReady;

    public UIView view { get; private set; }

    public LocalizationBindings localizationBindings { get; private set; }

    public OverlayBindings overlayBindings { get; private set; }

    public AppBindings appBindings { get; private set; }

    public InputHintBindings inputHintBindings { get; private set; }

    public ParadoxBindings paradoxBindings { get; private set; }

    public VirtualKeyboard virtualKeyboard { get; private set; }

    public IBindingRegistry bindings => (IBindingRegistry) this.m_Bindings;

    public UserInterface(string url, LocalizationManager localizationManager, Colossal.UI.UISystem uiSystem)
    {
      this.m_BindingsReady = new TaskCompletionSource<bool>();
      this.m_CursorCollection = UnityEngine.Resources.Load<UICursorCollection>("Input/UI Cursors");
      this.m_Bindings = new CompositeBinding();
      this.virtualKeyboard = new VirtualKeyboard();
      UIView.Settings settings = UIView.Settings.New with
      {
        textInputHandler = (TextInputHandler) this.virtualKeyboard,
        acceptsInput = true
      };
      this.view = uiSystem.CreateView(url, settings);
      this.view.Listener.ReadyForBindings += new Action(this.OnReadyForBindings);
      this.view.Listener.NavigateTo += new Func<string, bool>(this.OnNavigateTo);
      this.view.Listener.NodeMouseEvent += new Func<INodeProxy, IMouseEventData, IntPtr, PhaseType, cohtml.Net.Actions>(this.OnNodeMouseEvent);
      this.view.Listener.CursorChanged += new Action<Cursors, string>(this.OnCursorChanged);
      this.view.Listener.TextInputTypeChanged += new Action<ControlType>(this.OnTextInputTypeChanged);
      this.view.Listener.CaretRectChanged += new Action<int, int, uint, uint>(this.OnCaretRectChanged);
      this.view.enabled = true;
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) (this.localizationBindings = new LocalizationBindings(localizationManager)));
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) (this.appBindings = new AppBindings()));
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) (this.overlayBindings = new OverlayBindings()));
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) new AudioBindings());
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) new UserBindings());
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) new InputBindings());
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) new InputActionBindings());
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) (this.inputHintBindings = new InputHintBindings()));
      this.m_Bindings.AddUpdateBinding((IUpdateBinding) (this.paradoxBindings = new ParadoxBindings()));
      this.overlayBindings.hintMessages = localizationManager.activeDictionary.GetIndexedLocaleIDs("Loading.HINTMESSAGE");
      if (!this.view.View.IsReadyForBindings())
        return;
      this.OnReadyForBindings();
    }

    public void Update() => this.m_Bindings.Update();

    public void Dispose()
    {
      this.overlayBindings.DeactivateAllScreens();
      this.appBindings.activeUI = (string) null;
      this.m_Bindings.DisposeBindings();
      if (this.m_Bindings.attached)
        this.m_Bindings.Detach();
      if (this.view == null)
        return;
      this.view.Listener.ReadyForBindings -= new Action(this.OnReadyForBindings);
      this.view.Listener.NavigateTo -= new Func<string, bool>(this.OnNavigateTo);
      this.view.Listener.NodeMouseEvent -= new Func<INodeProxy, IMouseEventData, IntPtr, PhaseType, cohtml.Net.Actions>(this.OnNodeMouseEvent);
      this.view.Listener.CursorChanged -= new Action<Cursors, string>(this.OnCursorChanged);
      this.view.Listener.TextInputTypeChanged -= new Action<ControlType>(this.OnTextInputTypeChanged);
      this.view.uiSystem.DestroyView(this.view);
      this.view = (UIView) null;
    }

    public Task WaitForBindings() => (Task) this.m_BindingsReady.Task;

    private void OnReadyForBindings()
    {
      UserInterface.log.Info((object) "Ready for bindings");
      this.m_Bindings.Attach(this.view.View);
      this.appBindings.ready = true;
      this.m_BindingsReady.TrySetResult(true);
    }

    private bool OnNavigateTo(string url)
    {
      this.m_Bindings.Detach();
      return true;
    }

    private cohtml.Net.Actions OnNodeMouseEvent(
      INodeProxy node,
      IMouseEventData ev,
      IntPtr userData,
      PhaseType phaseType)
    {
      if (InputManager.instance.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse && phaseType == PhaseType.AT_TARGET)
        InputManager.instance.mouseOverUI = node.GetTag() != HTMLTag.BODY;
      return cohtml.Net.Actions.ContinueHandling;
    }

    private void OnCursorChanged(Cursors cursor, string url)
    {
      if ((UnityEngine.Object) this.m_CursorCollection == (UnityEngine.Object) null)
        UICursorCollection.ResetCursor();
      else if (cursor == Cursors.URL && url != null)
        this.m_CursorCollection.SetCursor(url);
      else
        this.m_CursorCollection.SetCursor(cursor);
    }

    private void OnTextInputTypeChanged(ControlType type)
    {
      InputManager.instance.hasInputFieldFocus = type == ControlType.TextInput;
    }

    private void OnCaretRectChanged(int x, int y, uint width, uint height)
    {
      InputManager.instance.caretRect = (new Vector2((float) x, (float) y), new Vector2((float) width, (float) height));
    }
  }
}
