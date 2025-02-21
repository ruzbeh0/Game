// Decompiled with JetBrains decompiler
// Type: Game.City.TaxRate
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.City
{
  public enum TaxRate
  {
    Main = 0,
    ResidentialOffset = 1,
    CommercialOffset = 2,
    IndustrialOffset = 3,
    OfficeOffset = 4,
    EducationZeroOffset = 5,
    CommercialResourceZeroOffset = 10, // 0x0000000A
    IndustrialResourceZeroOffset = 50, // 0x00000032
    Count = 90, // 0x0000005A
  }
}
