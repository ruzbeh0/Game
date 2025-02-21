// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.InfoviewsUIUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.InGame
{
  public class InfoviewsUIUtils
  {
    public static void UpdateFiveSlicePieChartData(
      IJsonWriter binder,
      int a,
      int b,
      int c,
      int d,
      int e)
    {
      binder.TypeBegin("infoviews.ChartData");
      binder.PropertyName("values");
      binder.ArrayBegin(5U);
      binder.Write(a);
      binder.Write(b);
      binder.Write(c);
      binder.Write(d);
      binder.Write(e);
      binder.ArrayEnd();
      binder.PropertyName("total");
      binder.Write(a + b + c + d + e);
      binder.TypeEnd();
    }
  }
}
