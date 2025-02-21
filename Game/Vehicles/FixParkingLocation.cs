// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.FixParkingLocation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct FixParkingLocation : IComponentData, IQueryTypeParameter
  {
    public Entity m_ChangeLane;
    public Entity m_ResetLocation;

    public FixParkingLocation(Entity changeLane, Entity resetLocation)
    {
      this.m_ChangeLane = changeLane;
      this.m_ResetLocation = resetLocation;
    }
  }
}
