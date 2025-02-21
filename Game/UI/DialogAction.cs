// Decompiled with JetBrains decompiler
// Type: Game.UI.DialogAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.UI
{
  public static class DialogAction
  {
    public const string kYes = "Common.DIALOG_ACTION[Yes]";
    public const string kNo = "Common.DIALOG_ACTION[No]";

    public static string GetId(string value) => "Common.DIALOG_ACTION[" + value + "]";
  }
}
