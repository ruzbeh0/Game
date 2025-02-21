// Decompiled with JetBrains decompiler
// Type: Game.Input.UIInputOverrideAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.Input
{
  [CreateAssetMenu(menuName = "Colossal/UI/UIInputOverrideAction")]
  public class UIInputOverrideAction : UIBaseInputAction
  {
    public UIBaseInputAction m_Source;
    public bool m_OverridePriority;

    public override IProxyAction GetState(string source1)
    {
      return this.m_Source.GetState(source1, (UIBaseInputAction.DisplayGetter) ((source2, action, mask, transform) => this.m_OverridePriority ? ((mask & this.m_DisplayMask) != InputManager.DeviceType.None ? new DisplayNameOverride(source2, action, this.m_AliasName, (int) this.m_DisplayPriority, transform) : (DisplayNameOverride) null) : ((mask & this.m_Source.m_DisplayMask) != InputManager.DeviceType.None ? new DisplayNameOverride(source2, action, this.m_AliasName, (int) this.m_Source.m_DisplayPriority, transform) : (DisplayNameOverride) null)));
    }

    public override IProxyAction GetState(
      string source,
      UIBaseInputAction.DisplayGetter displayNameGetter)
    {
      return this.m_Source.GetState(source, displayNameGetter);
    }

    public override IReadOnlyList<UIInputActionPart> actionParts => this.m_Source.actionParts;
  }
}
