// Decompiled with JetBrains decompiler
// Type: Game.UI.DialogTitle
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.UI
{
  public static class DialogTitle
  {
    public const string kWarning = "Common.DIALOG_TITLE[Warning]";

    public static string GetId(string value) => "Common.DIALOG_TITLE[" + value + "]";
  }
}
