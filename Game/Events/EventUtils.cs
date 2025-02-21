// Decompiled with JetBrains decompiler
// Type: Game.Events.EventUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Unity.Mathematics;

#nullable disable
namespace Game.Events
{
  public static class EventUtils
  {
    public const uint MIN_IN_DANGER_TIME = 64;
    public const float FLOOD_DEPTH_TOLERANCE = 0.5f;

    public static float GetSeverity(
      float3 position,
      WeatherPhenomenon weatherPhenomenon,
      WeatherPhenomenonData weatherPhenomenonData)
    {
      float num = math.distance(position.xz, weatherPhenomenon.m_HotspotPosition.xz) / weatherPhenomenon.m_HotspotRadius;
      float a = (float) ((double) weatherPhenomenon.m_Intensity * (double) weatherPhenomenonData.m_DamageSeverity * (1.0 - (double) num));
      return math.select(a, 0.0f, (double) a < 1.0 / 1000.0);
    }

    public static bool IsWorse(DangerFlags flags, DangerFlags other)
    {
      DangerFlags dangerFlags = flags ^ other;
      if ((dangerFlags & DangerFlags.Evacuate) != (DangerFlags) 0)
        return (flags & DangerFlags.Evacuate) > (DangerFlags) 0;
      return (dangerFlags & DangerFlags.StayIndoors) != (DangerFlags) 0 && (flags & DangerFlags.StayIndoors) > (DangerFlags) 0;
    }
  }
}
