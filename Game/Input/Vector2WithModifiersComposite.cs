// Decompiled with JetBrains decompiler
// Type: Game.Input.Vector2WithModifiersComposite
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
  [DisplayName("CO Vector 2D With Modifiers")]
  public class Vector2WithModifiersComposite : AnalogValueInputBindingComposite<Vector2>
  {
    private const string kName = "2DVectorWithModifiers";
    [InputControl(layout = "Vector2")]
    public int binding;
    [InputControl(layout = "Button")]
    public int modifier;

    public override string typeName => "2DVectorWithModifiers";

    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
      if (this.m_IsDummy)
        return new Vector2();
      if (this.m_Mode == Mode.Analog)
        return CompositeUtility.ReadValue<Vector2>(ref context, this.binding, this.allowModifiers, this.modifier, (IComparer<Vector2>) AnalogValueInputBindingComposite<Vector2>.Vector2Comparer.instance);
      return !CompositeUtility.ReadValueAsButton(ref context, this.binding, this.allowModifiers, this.modifier) ? Vector2.zero : Vector2.one;
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
      return this.ReadValue(ref context).magnitude;
    }

    static Vector2WithModifiersComposite()
    {
      InputManager.RegisterBindingComposite<Vector2WithModifiersComposite>("2DVectorWithModifiers", ActionType.Button, new InputManager.CompositeComponentData[1]
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
