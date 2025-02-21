// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIConfirmationAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Settings
{
  public class SettingsUIConfirmationAttribute : Attribute
  {
    public readonly string confirmMessageValue;
    public readonly string confirmMessageId;

    public SettingsUIConfirmationAttribute(
      string overrideConfirmMessageId = null,
      string overrideConfirmMessageValue = null)
    {
      this.confirmMessageValue = overrideConfirmMessageValue;
      this.confirmMessageId = overrideConfirmMessageId;
    }
  }
}
