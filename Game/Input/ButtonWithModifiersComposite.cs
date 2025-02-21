// Decompiled with JetBrains decompiler
// Type: Game.Input.ButtonWithModifiersComposite
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Input
{
  [DisplayStringFormat("{binding}")]
  [DisplayName("CO Binding With Modifiers")]
  public class ButtonWithModifiersComposite : AnalogValueInputBindingComposite<float>
  {
    public const string kName = "ButtonWithModifiers";
    [InputControl(layout = "Button")]
    public int binding;
    [InputControl(layout = "Button")]
    public int modifier;

    public override string typeName => "ButtonWithModifiers";

    public override float ReadValue(ref InputBindingCompositeContext context)
    {
      if (this.m_IsDummy)
        return 0.0f;
      if (this.m_Mode == Mode.Analog)
        return CompositeUtility.ReadValue<float>(ref context, this.binding, this.allowModifiers, this.modifier, (IComparer<float>) DefaultComparer<float>.instance);
      return !CompositeUtility.ReadValueAsButton(ref context, this.binding, this.allowModifiers, this.modifier) ? 0.0f : 1f;
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
      return this.ReadValue(ref context);
    }

    static ButtonWithModifiersComposite()
    {
      InputManager.RegisterBindingComposite<ButtonWithModifiersComposite>("ButtonWithModifiers", ActionType.Button, new InputManager.CompositeComponentData[1]
      {
        new InputManager.CompositeComponentData(ActionComponent.Press, nameof (binding), nameof (modifier))
      });
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
    }
  }
}
