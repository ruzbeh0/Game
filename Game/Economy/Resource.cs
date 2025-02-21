// Decompiled with JetBrains decompiler
// Type: Game.Economy.Resource
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Economy
{
  public enum Resource : ulong
  {
    NoResource = 0,
    Money = 1,
    Grain = 2,
    ConvenienceFood = 4,
    Food = 8,
    Vegetables = 16, // 0x0000000000000010
    Meals = 32, // 0x0000000000000020
    Wood = 64, // 0x0000000000000040
    Timber = 128, // 0x0000000000000080
    Paper = 256, // 0x0000000000000100
    Furniture = 512, // 0x0000000000000200
    Vehicles = 1024, // 0x0000000000000400
    Lodging = 2048, // 0x0000000000000800
    UnsortedMail = 4096, // 0x0000000000001000
    LocalMail = 8192, // 0x0000000000002000
    OutgoingMail = 16384, // 0x0000000000004000
    Oil = 32768, // 0x0000000000008000
    Petrochemicals = 65536, // 0x0000000000010000
    Ore = 131072, // 0x0000000000020000
    Plastics = 262144, // 0x0000000000040000
    Metals = 524288, // 0x0000000000080000
    Electronics = 1048576, // 0x0000000000100000
    Software = 2097152, // 0x0000000000200000
    Coal = 4194304, // 0x0000000000400000
    Stone = 8388608, // 0x0000000000800000
    Livestock = 16777216, // 0x0000000001000000
    Cotton = 33554432, // 0x0000000002000000
    Steel = 67108864, // 0x0000000004000000
    Minerals = 134217728, // 0x0000000008000000
    Concrete = 268435456, // 0x0000000010000000
    Machinery = 536870912, // 0x0000000020000000
    Chemicals = 1073741824, // 0x0000000040000000
    Pharmaceuticals = 2147483648, // 0x0000000080000000
    Beverages = 4294967296, // 0x0000000100000000
    Textiles = 8589934592, // 0x0000000200000000
    Telecom = 17179869184, // 0x0000000400000000
    Financial = 34359738368, // 0x0000000800000000
    Media = 68719476736, // 0x0000001000000000
    Entertainment = 137438953472, // 0x0000002000000000
    Recreation = 274877906944, // 0x0000004000000000
    Garbage = 549755813888, // 0x0000008000000000
    Last = 1099511627776, // 0x0000010000000000
    All = 18446744073709551615, // 0xFFFFFFFFFFFFFFFF
  }
}
