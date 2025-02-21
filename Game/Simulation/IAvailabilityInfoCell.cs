// Decompiled with JetBrains decompiler
// Type: Game.Simulation.IAvailabilityInfoCell
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Simulation
{
  public interface IAvailabilityInfoCell
  {
    void AddAttractiveness(float amount);

    void AddConsumers(float amount);

    void AddServices(float amount);

    void AddWorkplaces(float amount);
  }
}
