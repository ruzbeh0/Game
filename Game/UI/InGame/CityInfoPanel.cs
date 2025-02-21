// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CityInfoPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.UI.InGame
{
  public class CityInfoPanel : TabbedGamePanel
  {
    public override bool blocking => true;

    public override GamePanel.LayoutPosition position => GamePanel.LayoutPosition.Center;

    public enum Tab
    {
      Demand,
      CityPolicies,
    }
  }
}
