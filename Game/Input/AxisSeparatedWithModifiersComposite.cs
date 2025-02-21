// Decompiled with JetBrains decompiler
// Type: Game.Input.AxisSeparatedWithModifiersComposite
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Processors;
using UnityEngine.InputSystem.Utilities;

#nullable disable
namespace Game.Input
{
  [DisplayStringFormat("{negative}/{positive}")]
  [DisplayName("CO Positive/Negative Binding With Modifiers")]
  public class AxisSeparatedWithModifiersComposite : AnalogValueInputBindingComposite<float>
  {
    public const string kName = "1DAxisSeparatedWithModifiers";
    [InputControl(layout = "Button")]
    public int negative;
    [InputControl(layout = "Button")]
    public int positive;
    [InputControl(layout = "Button")]
    public int negativeModifier;
    [InputControl(layout = "Button")]
    public int positiveModifier;
    public float m_MinValue = -1f;
    public float m_MaxValue = 1f;
    public AxisSeparatedWithModifiersComposite.WhichSideWins m_WhichSideWins;

    public override string typeName => "1DAxisSeparatedWithModifiers";

    public float midPoint => (float) (((double) this.m_MaxValue + (double) this.m_MinValue) / 2.0);

    public override float ReadValue(ref InputBindingCompositeContext context)
    {
      if (this.m_IsDummy)
        return 0.0f;
      float num1;
      float num2;
      if (this.m_Mode == Mode.Analog)
      {
        num1 = Mathf.Abs(CompositeUtility.ReadValue<float>(ref context, this.negative, this.m_AllowModifiers, this.negativeModifier, (IComparer<float>) DefaultComparer<float>.instance));
        num2 = Mathf.Abs(CompositeUtility.ReadValue<float>(ref context, this.positive, this.m_AllowModifiers, this.positiveModifier, (IComparer<float>) DefaultComparer<float>.instance));
      }
      else
      {
        num1 = CompositeUtility.ReadValueAsButton(ref context, this.negative, this.m_AllowModifiers, this.negativeModifier) ? 1f : 0.0f;
        num2 = CompositeUtility.ReadValueAsButton(ref context, this.positive, this.m_AllowModifiers, this.positiveModifier) ? 1f : 0.0f;
      }
      bool flag1 = (double) num1 > (double) Mathf.Epsilon;
      bool flag2 = (double) num2 > (double) Mathf.Epsilon;
      if (flag1 == flag2)
      {
        switch (this.m_WhichSideWins)
        {
          case AxisSeparatedWithModifiersComposite.WhichSideWins.Neither:
            return this.midPoint;
          case AxisSeparatedWithModifiersComposite.WhichSideWins.Positive:
            flag1 = false;
            break;
          case AxisSeparatedWithModifiersComposite.WhichSideWins.Negative:
            flag2 = false;
            break;
        }
      }
      float midPoint = this.midPoint;
      if (flag1)
        return midPoint - (midPoint - this.m_MinValue) * num1;
      return flag2 ? midPoint + (this.m_MaxValue - midPoint) * num2 : this.midPoint;
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
      float num = this.ReadValue(ref context);
      return (double) num < (double) this.midPoint ? NormalizeProcessor.Normalize(Mathf.Abs(num - this.midPoint), 0.0f, Mathf.Abs(this.m_MinValue), 0.0f) : NormalizeProcessor.Normalize(Mathf.Abs(num - this.midPoint), 0.0f, Mathf.Abs(this.m_MaxValue), 0.0f);
    }

    static AxisSeparatedWithModifiersComposite()
    {
      InputManager.RegisterBindingComposite<AxisSeparatedWithModifiersComposite>("1DAxisSeparatedWithModifiers", ActionType.Axis, new InputManager.CompositeComponentData[2]
      {
        new InputManager.CompositeComponentData(ActionComponent.Negative, nameof (negative), nameof (negativeModifier)),
        new InputManager.CompositeComponentData(ActionComponent.Positive, nameof (positive), nameof (positiveModifier))
      });
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
    }

    public enum WhichSideWins
    {
      Neither,
      Positive,
      Negative,
    }
  }
}
