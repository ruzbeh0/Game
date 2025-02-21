﻿// Decompiled with JetBrains decompiler
// Type: Game.UI.ConfirmationDialogBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.UI.Localization;

#nullable disable
namespace Game.UI
{
  public abstract class ConfirmationDialogBase : IJsonWritable
  {
    protected const string kDefaultSkin = "Default";
    protected const string kParadoxSkin = "Paradox";
    private LocalizedString? title;
    private LocalizedString message;
    private LocalizedString confirmAction;
    private LocalizedString? cancelAction;
    [CanBeNull]
    private LocalizedString[] otherActions;
    private LocalizedString? details;
    private bool copyButton;

    protected virtual string skin => "Default";

    protected virtual bool dismissible => false;

    protected ConfirmationDialogBase(
      LocalizedString? title,
      LocalizedString message,
      LocalizedString? details,
      bool copyButton,
      LocalizedString confirmAction,
      LocalizedString? cancelAction,
      [CanBeNull] params LocalizedString[] otherActions)
    {
      this.title = title;
      this.message = message;
      this.confirmAction = confirmAction;
      this.cancelAction = cancelAction;
      this.otherActions = otherActions;
      this.details = details;
      this.copyButton = copyButton;
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("dismissible");
      writer.Write(this.dismissible);
      writer.PropertyName("skin");
      writer.Write(this.skin);
      writer.PropertyName("title");
      writer.Write<LocalizedString>(this.title);
      writer.PropertyName("message");
      writer.Write<LocalizedString>(this.message);
      writer.PropertyName("confirmAction");
      writer.Write<LocalizedString>(this.confirmAction);
      writer.PropertyName("cancelAction");
      writer.Write<LocalizedString>(this.cancelAction);
      writer.PropertyName("otherActions");
      if (this.otherActions != null)
      {
        writer.ArrayBegin(this.otherActions.Length);
        for (int index = 0; index < this.otherActions.Length; ++index)
          writer.Write<LocalizedString>(this.otherActions[index]);
        writer.ArrayEnd();
      }
      else
        writer.WriteEmptyArray();
      writer.PropertyName("details");
      writer.Write<LocalizedString>(this.details);
      writer.PropertyName("copyButton");
      writer.Write(this.copyButton);
      writer.TypeEnd();
    }
  }
}
