// Decompiled with JetBrains decompiler
// Type: Game.Input.Vector2SeparatedWithModifiersComposite
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Input
{
  [DisplayStringFormat("{up}/{left}/{down}/{right}")]
  [DisplayName("CO Up/Down/Left/Right Binding With Modifiers")]
  public class Vector2SeparatedWithModifiersComposite : AnalogValueInputBindingComposite<Vector2>
  {
    public const string kName = "2DVectorSeparatedWithModifiers";
    [InputControl(layout = "Button")]
    public int up;
    [InputControl(layout = "Button")]
    public int down;
    [InputControl(layout = "Button")]
    public int left;
    [InputControl(layout = "Button")]
    public int right;
    [InputControl(layout = "Button")]
    public int upModifier;
    [InputControl(layout = "Button")]
    public int downModifier;
    [InputControl(layout = "Button")]
    public int leftModifier;
    [InputControl(layout = "Button")]
    public int rightModifier;

    public override string typeName => "2DVectorSeparatedWithModifiers";

    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
      if (this.m_IsDummy)
        return new Vector2();
      if (this.m_Mode == Mode.Analog)
      {
        double up = (double) CompositeUtility.ReadValue<float>(ref context, this.up, this.m_AllowModifiers, this.upModifier, (IComparer<float>) DefaultComparer<float>.instance);
        float num1 = CompositeUtility.ReadValue<float>(ref context, this.down, this.m_AllowModifiers, this.downModifier, (IComparer<float>) DefaultComparer<float>.instance);
        float num2 = CompositeUtility.ReadValue<float>(ref context, this.left, this.m_AllowModifiers, this.leftModifier, (IComparer<float>) DefaultComparer<float>.instance);
        float num3 = CompositeUtility.ReadValue<float>(ref context, this.right, this.m_AllowModifiers, this.rightModifier, (IComparer<float>) DefaultComparer<float>.instance);
        double down = (double) num1;
        double left = (double) num2;
        double right = (double) num3;
        return DpadControl.MakeDpadVector((float) up, (float) down, (float) left, (float) right);
      }
      int num4 = CompositeUtility.ReadValueAsButton(ref context, this.up, this.m_AllowModifiers, this.upModifier) ? 1 : 0;
      bool flag1 = CompositeUtility.ReadValueAsButton(ref context, this.down, this.m_AllowModifiers, this.downModifier);
      bool flag2 = CompositeUtility.ReadValueAsButton(ref context, this.left, this.m_AllowModifiers, this.leftModifier);
      bool flag3 = CompositeUtility.ReadValueAsButton(ref context, this.right, this.m_AllowModifiers, this.rightModifier);
      int num5 = flag1 ? 1 : 0;
      int num6 = flag2 ? 1 : 0;
      int num7 = flag3 ? 1 : 0;
      int num8 = this.m_Mode == Mode.DigitalNormalized ? 1 : 0;
      return DpadControl.MakeDpadVector(num4 != 0, num5 != 0, num6 != 0, num7 != 0, num8 != 0);
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
      return this.ReadValue(ref context).magnitude;
    }

    static Vector2SeparatedWithModifiersComposite()
    {
      InputManager.RegisterBindingComposite<Vector2SeparatedWithModifiersComposite>("2DVectorSeparatedWithModifiers", ActionType.Vector2, new InputManager.CompositeComponentData[4]
      {
        new InputManager.CompositeComponentData(ActionComponent.Up, nameof (up), nameof (upModifier)),
        new InputManager.CompositeComponentData(ActionComponent.Down, nameof (down), nameof (downModifier)),
        new InputManager.CompositeComponentData(ActionComponent.Left, nameof (left), nameof (leftModifier)),
        new InputManager.CompositeComponentData(ActionComponent.Right, nameof (right), nameof (rightModifier))
      });
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
    }
  }
}
