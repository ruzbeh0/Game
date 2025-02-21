// Decompiled with JetBrains decompiler
// Type: Colossal.Atmosphere.SunMoonData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Runtime.InteropServices;
using Unity.Mathematics;

#nullable disable
namespace Colossal.Atmosphere
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct SunMoonData
  {
    private static readonly double e = math.radians(23.4397);
    private const double J0 = 0.0009;
    private const double kDayMs = 86400000.0;
    private const double J2000 = 2451545.0;
    public static readonly double h0 = math.radians(-0.833);
    public static readonly double h1 = math.radians(-0.3);
    public static readonly double d0 = math.radians(-6.0);
    public static readonly double d1 = math.radians(-12.0);
    public static readonly double d2 = math.radians(-18.0);
    public static readonly double g0 = math.radians(6.0);

    private double GetRightAscension(double l, double b)
    {
      return math.atan2(math.sin(l) * math.cos(SunMoonData.e) - math.tan(b) * math.sin(SunMoonData.e), math.cos(l));
    }

    private double GetDeclination(double l, double b)
    {
      return math.asin(math.sin(b) * math.cos(SunMoonData.e) + math.cos(b) * math.sin(SunMoonData.e) * math.sin(l));
    }

    private double GetAzimuth(double H, double phi, double dec)
    {
      return math.atan2(math.sin(H), math.cos(H) * math.sin(phi) - math.tan(dec) * math.cos(phi)) + Math.PI;
    }

    private double GetAltitude(double H, double phi, double dec)
    {
      return math.asin(math.sin(phi) * math.sin(dec) + math.cos(phi) * math.cos(dec) * math.cos(H));
    }

    private double GetSiderealTime(double d, double lw)
    {
      return math.radians(280.16 + 360.9856235 * d) - lw;
    }

    private double GetAstroRefraction(double h)
    {
      if (h < 0.0)
        h = 0.0;
      return 0.0002967 / math.tan(h + 0.00312536 / (h + 0.08901179));
    }

    private double GetSolarMeanAnomaly(double d) => math.radians(357.5291 + 0.98560028 * d);

    private double GetEclipticLongitude(double M)
    {
      double num1 = math.radians(1.9148 * math.sin(M) + 0.02 * math.sin(2.0 * M) + 0.0003 * math.sin(3.0 * M));
      double num2 = math.radians(102.9372);
      return M + num1 + num2 + Math.PI;
    }

    private EquatorialCoordinate GetSunCoords(double d)
    {
      double eclipticLongitude = this.GetEclipticLongitude(this.GetSolarMeanAnomaly(d));
      return new EquatorialCoordinate()
      {
        declination = this.GetDeclination(eclipticLongitude, 0.0),
        rightAscension = this.GetRightAscension(eclipticLongitude, 0.0)
      };
    }

    public TopocentricCoordinates GetSunPosition(
      JulianDateTime date,
      double latitude,
      double longitude)
    {
      double lw = math.radians(-longitude);
      double phi = math.radians(latitude);
      double d = date.ToDouble() - 2451545.0;
      EquatorialCoordinate sunCoords = this.GetSunCoords(d);
      double H = this.GetSiderealTime(d, lw) - sunCoords.rightAscension;
      return new TopocentricCoordinates()
      {
        azimuth = this.GetAzimuth(H, phi, sunCoords.declination),
        altitude = this.GetAltitude(H, phi, sunCoords.declination)
      };
    }

    private double GetJulianCycle(double d, double lw)
    {
      return math.round(d - 0.0009 - lw / (2.0 * Math.PI));
    }

    private double GetApproximateSolarTransit(double Ht, double lw, double n)
    {
      return 0.0009 + (Ht + lw) / (2.0 * Math.PI) + n;
    }

    private double GetSolarTransit(double ds, double M, double L)
    {
      return 2451545.0 + ds + 0.0053 * math.sin(M) - 0.0069 * math.sin(2.0 * L);
    }

    private double GetHourAngle(double h, double phi, double d)
    {
      return math.acos((math.sin(h) - math.sin(phi) * math.sin(d)) / (math.cos(phi) * math.cos(d)));
    }

    private double GetTimeForSunAltitude(
      double h,
      double lw,
      double phi,
      double dec,
      double n,
      double M,
      double L)
    {
      return this.GetSolarTransit(this.GetApproximateSolarTransit(this.GetHourAngle(h, phi, dec), lw, n), M, L);
    }

    public SunMoonData.SunTimes GetSunTimes(JulianDateTime date, double latitude, double longitude)
    {
      double lw = math.radians(-longitude);
      double phi = math.radians(latitude);
      double julianCycle = this.GetJulianCycle(date.ToDouble() - 2451545.0, lw);
      double approximateSolarTransit = this.GetApproximateSolarTransit(0.0, lw, julianCycle);
      double solarMeanAnomaly = this.GetSolarMeanAnomaly(approximateSolarTransit);
      double eclipticLongitude = this.GetEclipticLongitude(solarMeanAnomaly);
      double declination = this.GetDeclination(eclipticLongitude, 0.0);
      double solarTransit = this.GetSolarTransit(approximateSolarTransit, solarMeanAnomaly, eclipticLongitude);
      SunMoonData.SunTimes sunTimes;
      sunTimes.solarNoon = new JulianDateTime(solarTransit);
      sunTimes.nadir = new JulianDateTime(solarTransit - 0.5);
      double timeForSunAltitude1 = this.GetTimeForSunAltitude(SunMoonData.h0, lw, phi, declination, julianCycle, solarMeanAnomaly, eclipticLongitude);
      double j1 = solarTransit - (timeForSunAltitude1 - solarTransit);
      sunTimes.sunrise = new JulianDateTime(j1);
      sunTimes.sunset = new JulianDateTime(timeForSunAltitude1);
      double timeForSunAltitude2 = this.GetTimeForSunAltitude(SunMoonData.h1, lw, phi, declination, julianCycle, solarMeanAnomaly, eclipticLongitude);
      double j2 = solarTransit - (timeForSunAltitude2 - solarTransit);
      sunTimes.sunriseEnd = new JulianDateTime(j2);
      sunTimes.sunsetStart = new JulianDateTime(timeForSunAltitude2);
      double timeForSunAltitude3 = this.GetTimeForSunAltitude(SunMoonData.d0, lw, phi, declination, julianCycle, solarMeanAnomaly, eclipticLongitude);
      double j3 = solarTransit - (timeForSunAltitude3 - solarTransit);
      sunTimes.dawn = new JulianDateTime(j3);
      sunTimes.dusk = new JulianDateTime(timeForSunAltitude3);
      double timeForSunAltitude4 = this.GetTimeForSunAltitude(SunMoonData.d1, lw, phi, declination, julianCycle, solarMeanAnomaly, eclipticLongitude);
      double j4 = solarTransit - (timeForSunAltitude4 - solarTransit);
      sunTimes.nauticalDawn = new JulianDateTime(j4);
      sunTimes.nauticalDusk = new JulianDateTime(timeForSunAltitude4);
      double timeForSunAltitude5 = this.GetTimeForSunAltitude(SunMoonData.d2, lw, phi, declination, julianCycle, solarMeanAnomaly, eclipticLongitude);
      double j5 = solarTransit - (timeForSunAltitude5 - solarTransit);
      sunTimes.nightEnd = new JulianDateTime(j5);
      sunTimes.night = new JulianDateTime(timeForSunAltitude5);
      double timeForSunAltitude6 = this.GetTimeForSunAltitude(SunMoonData.g0, lw, phi, declination, julianCycle, solarMeanAnomaly, eclipticLongitude);
      double j6 = solarTransit - (timeForSunAltitude6 - solarTransit);
      sunTimes.goldenHourEnd = new JulianDateTime(j6);
      sunTimes.goldenHour = new JulianDateTime(timeForSunAltitude6);
      return sunTimes;
    }

    private MoonCoords GetMoonCoords(double d)
    {
      double num1 = math.radians(218.316 + 13.176396 * d);
      double x1 = math.radians(134.963 + 13.064993 * d);
      double x2 = math.radians(93.272 + 13.22935 * d);
      double num2 = math.radians(6.289 * math.sin(x1));
      double l = num1 + num2;
      double b = math.radians(5.128 * math.sin(x2));
      double num3 = 385001.0 - 20905.0 * math.cos(x1);
      return new MoonCoords()
      {
        equatorialCoords = new EquatorialCoordinate()
        {
          rightAscension = this.GetRightAscension(l, b),
          declination = this.GetDeclination(l, b)
        },
        distance = num3
      };
    }

    public MoonCoordinate GetMoonPosition(JulianDateTime date, double latitude, double longitude)
    {
      double lw = math.radians(-longitude);
      double num1 = math.radians(latitude);
      double d = date.ToDouble() - 2451545.0;
      MoonCoords moonCoords = this.GetMoonCoords(d);
      double num2 = this.GetSiderealTime(d, lw) - moonCoords.equatorialCoords.rightAscension;
      double altitude = this.GetAltitude(num2, num1, moonCoords.equatorialCoords.declination);
      double num3 = math.atan2(math.sin(num2), math.tan(num1) * math.cos(moonCoords.equatorialCoords.declination) - math.sin(moonCoords.equatorialCoords.declination) * math.cos(num2));
      double num4 = altitude + this.GetAstroRefraction(altitude);
      return new MoonCoordinate()
      {
        topoCoords = new TopocentricCoordinates()
        {
          azimuth = this.GetAzimuth(num2, num1, moonCoords.equatorialCoords.declination),
          altitude = num4
        },
        distance = moonCoords.distance,
        parallacticAngle = num3
      };
    }

    public MoonIllumination GetMoonIllumination(JulianDateTime date)
    {
      double d = date.ToDouble() - 2451545.0;
      EquatorialCoordinate sunCoords = this.GetSunCoords(d);
      MoonCoords moonCoords = this.GetMoonCoords(d);
      double num1 = 149598000.0;
      double x1 = math.acos(math.sin(sunCoords.declination) * math.sin(moonCoords.equatorialCoords.declination) + math.cos(sunCoords.declination) * math.cos(moonCoords.equatorialCoords.declination) * math.cos(sunCoords.rightAscension - moonCoords.equatorialCoords.rightAscension));
      double x2 = math.atan2(num1 * math.sin(x1), moonCoords.distance - num1 * math.cos(x1));
      double num2 = math.atan2(math.cos(sunCoords.declination) * math.sin(sunCoords.rightAscension - moonCoords.equatorialCoords.rightAscension), math.sin(sunCoords.declination) * math.cos(moonCoords.equatorialCoords.declination) - math.cos(sunCoords.declination) * math.sin(moonCoords.equatorialCoords.declination) * math.cos(sunCoords.rightAscension - moonCoords.equatorialCoords.rightAscension));
      return new MoonIllumination()
      {
        fraction = (1.0 + math.cos(x2)) / 2.0,
        phase = 0.5 + 0.5 * x2 * (num2 < 0.0 ? -1.0 : 1.0) / Math.PI,
        angle = num2
      };
    }

    public struct SunTimes
    {
      public JulianDateTime solarNoon;
      public JulianDateTime nadir;
      public JulianDateTime sunrise;
      public JulianDateTime sunset;
      public JulianDateTime sunriseEnd;
      public JulianDateTime sunsetStart;
      public JulianDateTime dawn;
      public JulianDateTime dusk;
      public JulianDateTime nauticalDawn;
      public JulianDateTime nauticalDusk;
      public JulianDateTime nightEnd;
      public JulianDateTime night;
      public JulianDateTime goldenHourEnd;
      public JulianDateTime goldenHour;

      public override string ToString()
      {
        return string.Format("SolarNoon: {0}\nNadir: {1}\nSunrise: {2}\nSunset: {3}\nSunriseEnd: {4}\nSunsetStart: {5}\nDawn: {6}\nDusk: {7}\nNauticalDawn: {8}\nNauticalDusk: {9}\nNightEnd: {10}\nNight: {11}\nGoldenHourEnd: {12}\nGoldenHour: {13}\n", (object) this.solarNoon, (object) this.nadir, (object) this.sunrise, (object) this.sunset, (object) this.sunriseEnd, (object) this.sunsetStart, (object) this.dawn, (object) this.dusk, (object) this.nauticalDawn, (object) this.nauticalDusk, (object) this.nightEnd, (object) this.night, (object) this.goldenHourEnd, (object) this.goldenHour);
      }
    }
  }
}
