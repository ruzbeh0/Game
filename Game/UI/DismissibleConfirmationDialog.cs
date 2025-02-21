// Decompiled with JetBrains decompiler
// Type: Game.UI.DismissibleConfirmationDialog
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.UI.Localization;

#nullable disable
namespace Game.UI
{
  public class DismissibleConfirmationDialog : ConfirmationDialogBase
  {
    protected override bool dismissible => true;

    public DismissibleConfirmationDialog(
      LocalizedString? title,
      LocalizedString message,
      LocalizedString confirmAction,
      LocalizedString? cancelAction,
      [CanBeNull] params LocalizedString[] otherActions)
      : base(title, message, new LocalizedString?(), false, confirmAction, cancelAction, otherActions)
    {
    }
  }
}
