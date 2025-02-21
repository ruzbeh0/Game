// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.SetupTargetType
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Pathfind
{
  public enum SetupTargetType
  {
    None = -1, // 0xFFFFFFFF
    CurrentLocation = 0,
    ResourceSeller = 1,
    RouteWaypoints = 2,
    TransportVehicle = 3,
    GarbageCollector = 4,
    RandomTraffic = 5,
    JobSeekerTo = 6,
    SchoolSeekerTo = 7,
    FireEngine = 8,
    PolicePatrol = 9,
    Leisure = 10, // 0x0000000A
    Taxi = 11, // 0x0000000B
    ResourceExport = 12, // 0x0000000C
    Ambulance = 13, // 0x0000000D
    StorageTransfer = 14, // 0x0000000E
    Maintenance = 15, // 0x0000000F
    PostVan = 16, // 0x00000010
    MailTransfer = 17, // 0x00000011
    MailBox = 18, // 0x00000012
    OutsideConnection = 19, // 0x00000013
    AccidentLocation = 20, // 0x00000014
    Hospital = 21, // 0x00000015
    Safety = 22, // 0x00000016
    EmergencyShelter = 23, // 0x00000017
    EvacuationTransport = 24, // 0x00000018
    Hearse = 25, // 0x00000019
    CrimeProducer = 26, // 0x0000001A
    PrisonerTransport = 27, // 0x0000001B
    WoodResource = 28, // 0x0000001C
    AreaLocation = 29, // 0x0000001D
    Sightseeing = 30, // 0x0000001E
    Attraction = 31, // 0x0000001F
    GarbageTransfer = 32, // 0x00000020
    HomelessShelter = 33, // 0x00000021
    FindHome = 34, // 0x00000022
    TransportVehicleRequest = 35, // 0x00000023
    TaxiRequest = 36, // 0x00000024
    PrisonerTransportRequest = 37, // 0x00000025
    EvacuationRequest = 38, // 0x00000026
    GarbageCollectorRequest = 39, // 0x00000027
    PoliceRequest = 40, // 0x00000028
    FireRescueRequest = 41, // 0x00000029
    PostVanRequest = 42, // 0x0000002A
    MaintenanceRequest = 43, // 0x0000002B
    HealthcareRequest = 44, // 0x0000002C
    TouristFindTarget = 45, // 0x0000002D
  }
}
