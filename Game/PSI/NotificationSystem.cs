// Decompiled with JetBrains decompiler
// Type: Game.PSI.NotificationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Game.UI.Localization;
using Game.UI.Menu;
using System;

#nullable disable
namespace Game.PSI
{
  public static class NotificationSystem
  {
    private static NotificationUISystem s_System;

    public static void BindUI(NotificationUISystem value) => NotificationSystem.s_System = value;

    public static void UnbindUI() => NotificationSystem.s_System = (NotificationUISystem) null;

    public static void Push(
      string identifier,
      LocalizedString? title = null,
      LocalizedString? text = null,
      string titleId = null,
      string textId = null,
      string thumbnail = null,
      ProgressState? progressState = null,
      int? progress = null,
      Action onClicked = null)
    {
      // ISSUE: variable of a compiler-generated type
      NotificationUISystem system = NotificationSystem.s_System;
      if (system == null)
        return;
      string identifier1 = identifier;
      LocalizedString? nullable = title;
      // ISSUE: reference to a compiler-generated method
      LocalizedString? title1 = new LocalizedString?(nullable ?? (LocalizedString) NotificationUISystem.GetTitle(titleId));
      nullable = text;
      // ISSUE: reference to a compiler-generated method
      LocalizedString? text1 = new LocalizedString?(nullable ?? (LocalizedString) NotificationUISystem.GetText(textId));
      string thumbnail1 = thumbnail;
      ProgressState? progressState1 = progressState;
      int? progress1 = progress;
      Action onClicked1 = onClicked;
      // ISSUE: reference to a compiler-generated method
      system.AddOrUpdateNotification(identifier1, title1, text1, thumbnail1, progressState1, progress1, onClicked1);
    }

    public static void Pop(
      string identifier,
      float delay = 0.0f,
      LocalizedString? title = null,
      LocalizedString? text = null,
      string titleId = null,
      string textId = null,
      string thumbnail = null,
      ProgressState? progressState = null,
      int? progress = null,
      Action onClicked = null)
    {
      // ISSUE: variable of a compiler-generated type
      NotificationUISystem system = NotificationSystem.s_System;
      if (system == null)
        return;
      string identifier1 = identifier;
      double delay1 = (double) delay;
      LocalizedString? nullable = title;
      // ISSUE: reference to a compiler-generated method
      LocalizedString? title1 = new LocalizedString?(nullable ?? (LocalizedString) NotificationUISystem.GetTitle(titleId));
      nullable = text;
      // ISSUE: reference to a compiler-generated method
      LocalizedString? text1 = new LocalizedString?(nullable ?? (LocalizedString) NotificationUISystem.GetText(textId));
      string thumbnail1 = thumbnail;
      ProgressState? progressState1 = progressState;
      int? progress1 = progress;
      Action onClicked1 = onClicked;
      // ISSUE: reference to a compiler-generated method
      system.RemoveNotification(identifier1, (float) delay1, title1, text1, thumbnail1, progressState1, progress1, onClicked1);
    }

    public static bool Exist(string identifier)
    {
      // ISSUE: variable of a compiler-generated type
      NotificationUISystem system = NotificationSystem.s_System;
      // ISSUE: reference to a compiler-generated method
      return system != null && system.NotificationExists(identifier);
    }
  }
}
