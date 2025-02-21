// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.InputHintTooltip
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Input;
using Game.UI.Widgets;

#nullable disable
namespace Game.UI.Tooltip
{
  public class InputHintTooltip : Widget
  {
    private const string kInputHint = "hint";
    public ProxyAction m_Action;
    private InputHintBindings.InputHint m_Hint;

    public InputHintTooltip(ProxyAction action)
    {
      this.m_Action = action;
      this.path = (PathSegment) action.title;
      this.Refresh();
    }

    public void Refresh()
    {
      if (this.m_Hint != null && !(this.m_Hint.name != (this.m_Action.displayOverride?.displayName ?? this.m_Action.title)))
        return;
      this.m_Hint = InputHintBindings.InputHint.Create(this.m_Action);
      InputHintBindings.InputHint hint = this.m_Hint;
      ProxyAction action = this.m_Action;
      DisplayNameOverride displayOverride = this.m_Action.displayOverride;
      int transform = displayOverride != null ? (int) displayOverride.transform : 0;
      InputHintBindings.CollectHintItems(hint, action, InputManager.ControlScheme.Gamepad, (UIBaseInputAction.Transform) transform);
      this.SetPropertiesChanged();
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("hint");
      writer.Write<InputHintBindings.InputHint>(this.m_Hint);
    }
  }
}
