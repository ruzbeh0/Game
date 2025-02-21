// Decompiled with JetBrains decompiler
// Type: Game.PSI.VirtualKeyboard
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.UI;
using Game.Input;
using Game.SceneFlow;

#nullable disable
namespace Game.PSI
{
  public class VirtualKeyboard : TextInputHandler
  {
    public VirtualKeyboard()
    {
      PlatformManager.instance.onInputDismissed += (InputDismissedEventHandler) ((psi, text) =>
      {
        if (psi.passThroughVKeyboard)
          return;
        this.RefreshText(text);
      });
    }

    private string GetVkTitle()
    {
      string attribute = this.proxy.GetAttribute("vk-title");
      return !string.IsNullOrEmpty(attribute) ? attribute : "Input";
    }

    private string GetVkDescription()
    {
      string attribute = this.proxy.GetAttribute("vk-description");
      return !string.IsNullOrEmpty(attribute) ? attribute : string.Empty;
    }

    private InputType TextToInputType(string text)
    {
      switch (text)
      {
        case nameof (text):
          return InputType.Text;
        case "password":
          return InputType.Password;
        case "email":
          return InputType.Email;
        default:
          return InputType.Other;
      }
    }

    private InputType GetVkType() => this.TextToInputType(this.proxy.GetAttribute("vk-type"));

    protected override void OnFocusCallback(string str)
    {
      if (InputManager.instance.activeControlScheme != InputManager.ControlScheme.Gamepad)
        return;
      GameManager.UIInputSystem.emulateBackspaceOnTextEvent = PlatformManager.instance.ShowVirtualKeyboard(this.GetVkType(), this.GetVkTitle(), this.GetVkDescription(), 100, str) && PlatformManager.instance.passThroughVKeyboard;
    }

    protected override void OnBlurCallback()
    {
      GameManager.UIInputSystem.emulateBackspaceOnTextEvent = false;
      PlatformManager.instance.DismissVirtualKeyboard();
    }
  }
}
