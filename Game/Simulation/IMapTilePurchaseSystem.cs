// Decompiled with JetBrains decompiler
// Type: Game.Simulation.IMapTilePurchaseSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;

#nullable disable
namespace Game.Simulation
{
  public interface IMapTilePurchaseSystem
  {
    bool selecting { get; set; }

    int cost { get; }

    float GetFeatureAmount(MapFeature feature);

    TilePurchaseErrorFlags status { get; }

    void PurchaseSelection();

    void Update();
  }
}
