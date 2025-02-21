// Decompiled with JetBrains decompiler
// Type: Game.Triggers.TriggerType
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Triggers
{
  public enum TriggerType
  {
    NewNotification = 0,
    NotificationResolved = 1,
    LevelUpResidentialBuilding = 2,
    LevelUpCommercialBuilding = 3,
    LevelUpIndustrialBuilding = 4,
    LevelUpOfficeBuilding = 5,
    LevelDownResidentialBuilding = 6,
    LevelDownCommercialBuilding = 7,
    LevelDownIndustrialBuilding = 8,
    LevelDownOfficeBuilding = 9,
    CitizenStartedWorking = 10, // 0x0000000A
    CitizenStartedSchool = 11, // 0x0000000B
    CitizenFailedSchool = 12, // 0x0000000C
    CitizenGraduated = 13, // 0x0000000D
    CitizenBecameUnemployed = 14, // 0x0000000E
    CitizenDied = 15, // 0x0000000F
    CitizenGotSick = 16, // 0x00000010
    CitizenGotInjured = 17, // 0x00000011
    CitizenGotTrapped = 18, // 0x00000012
    CitizenGotInDanger = 19, // 0x00000013
    CitizenPartneredUp = 20, // 0x00000014
    CitizenDivorced = 21, // 0x00000015
    CitizenMovedHouse = 22, // 0x00000016
    CitizenMovedOutOfCity = 23, // 0x00000017
    CitizenSingleMadeBaby = 24, // 0x00000018
    CitizensFamilyMemberDied = 25, // 0x00000019
    TouristLeftCity = 26, // 0x0000001A
    CitizenCommitedCrime = 27, // 0x0000001B
    CitizenGotArrested = 28, // 0x0000001C
    CitizenGotSentencedToPrison = 29, // 0x0000001D
    PolicyActivated = 30, // 0x0000001E
    MapTilePurchased = 31, // 0x0000001F
    BrandRented = 32, // 0x00000020
    FreePublicTransport = 33, // 0x00000021
    EventHappened = 34, // 0x00000022
    AverageAirPollution = 35, // 0x00000023
    ObjectCreated = 36, // 0x00000024
    Temperature = 37, // 0x00000025
    WeatherStormy = 38, // 0x00000026
    WeatherClear = 39, // 0x00000027
    WeatherRainy = 40, // 0x00000028
    WeatherSunny = 41, // 0x00000029
    WeatherCloudy = 42, // 0x0000002A
    AuroraBorealis = 43, // 0x0000002B
    UnpaidLoan = 44, // 0x0000002C
    StatisticsValue = 45, // 0x0000002D
    ResidentialDemand = 46, // 0x0000002E
    NoisePollutionHappinessFactor = 47, // 0x0000002F
    WeatherSnowy = 48, // 0x00000030
    NoOutsideConnection = 49, // 0x00000031
    ServiceTradeBalance = 50, // 0x00000032
    TrafficBottleneck = 51, // 0x00000033
    TelecomHappinessFactor = 52, // 0x00000034
    CrimeHappinessFactor = 53, // 0x00000035
    AirPollutionHappinessFactor = 54, // 0x00000036
    ApartmentHappinessFactor = 55, // 0x00000037
    ElectricityHappinessFactor = 56, // 0x00000038
    HealthcareHappinessFactor = 57, // 0x00000039
    GroundPollutionHappinessFactor = 58, // 0x0000003A
    WaterHappinessFactor = 59, // 0x0000003B
    WaterPollutionHappinessFactor = 60, // 0x0000003C
    SewageHappinessFactor = 61, // 0x0000003D
    GarbageHappinessFactor = 62, // 0x0000003E
    EntertainmentHappinessFactor = 63, // 0x0000003F
    EducationHappinessFactor = 64, // 0x00000040
    MailHappinessFactor = 65, // 0x00000041
    WelfareHappinessFactor = 66, // 0x00000042
    LeisureHappinessFactor = 67, // 0x00000043
    CityServicePost = 68, // 0x00000044
    CityServiceEducation = 69, // 0x00000045
    CityServiceElectricity = 70, // 0x00000046
    CityServiceFireAndRescue = 71, // 0x00000047
    CityServiceGarbage = 72, // 0x00000048
    CityServiceHealthcare = 73, // 0x00000049
    CityServicePolice = 74, // 0x0000004A
    CityServiceWaterSewage = 75, // 0x0000004B
    TaxHappinessFactor = 76, // 0x0000004C
    BuildingsHappinessFactor = 77, // 0x0000004D
    WealthHappinessFactor = 78, // 0x0000004E
    TrafficPenaltyHappinessFactor = 79, // 0x0000004F
    DeathPenaltyHappinessFactor = 80, // 0x00000050
    CitizenCoupleMadeBaby = 81, // 0x00000051
    HomelessnessHappinessFactor = 82, // 0x00000052
    CitizenDroppedOutSchool = 84, // 0x00000054
    ElectricityFeeHappinessFactor = 85, // 0x00000055
    WaterFeeHappinessFactor = 86, // 0x00000056
  }
}
