// Decompiled with JetBrains decompiler
// Type: Game.Input.UIInputActionCollection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Game.Input
{
  [CreateAssetMenu(menuName = "Colossal/UI/UIInputActionCollection")]
  public class UIInputActionCollection : ScriptableObject
  {
    public UIBaseInputAction[] m_InputActions;

    public IProxyAction GetActionState(string actionName, string source)
    {
      UIBaseInputAction uiBaseInputAction = ((IEnumerable<UIBaseInputAction>) this.m_InputActions).FirstOrDefault<UIBaseInputAction>((Func<UIBaseInputAction, bool>) (a => a.aliasName == actionName));
      return !((UnityEngine.Object) uiBaseInputAction != (UnityEngine.Object) null) ? (IProxyAction) null : uiBaseInputAction.GetState(actionName + " (" + source + ")");
    }
  }
}
