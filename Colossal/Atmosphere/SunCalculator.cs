// Decompiled with JetBrains decompiler
// Type: Colossal.Atmosphere.SunCalculator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Mathematics;

#nullable disable
namespace Colossal.Atmosphere
{
  public class SunCalculator
  {
    private static readonly double J2000 = 2451545.0;
    private static readonly double h0 = math.radians(-0.83);
    private static readonly double d0 = math.radians(0.53);
    private static readonly double h1 = (double) math.radians(-6f);
    private static readonly double h2 = (double) math.radians(-12f);
    private static readonly double h3 = (double) math.radians(-18f);
    private static readonly double P = math.radians(102.9372);
    private static readonly double e = math.radians(23.45);
    private static readonly double th0 = math.radians(280.16);
    private static readonly double th1 = math.radians(360.9856235);
    private static readonly double M0 = math.radians(357.5291);
    private static readonly double M1 = math.radians(0.98560028);
    private static readonly double C1 = math.radians(1.9148);
    private static readonly double C2 = math.radians(0.02);
    private static readonly double C3 = math.radians(0.0003);
    private static readonly double J0 = 0.0009;
    private static readonly double J1 = 0.0053;
    private static readonly double J2 = -0.0069;

    private static double GetEclipticLongitude(double M, double C)
    {
      return M + SunCalculator.P + C + 3.1415927410125732;
    }

    private static double GetHourAngle(double h, double phi, double d)
    {
      return math.acos((math.sin(h) - math.sin(phi) * math.sin(d)) / (math.cos(phi) * math.cos(d)));
    }

    private static double GetSunDeclination(double Lsun)
    {
      return math.asin(math.sin(Lsun) * math.sin(SunCalculator.e));
    }

    private static double GetSolarMeanAnomaly(double Js)
    {
      return SunCalculator.M0 + SunCalculator.M1 * (Js - SunCalculator.J2000);
    }

    private static double GetEquationOfCenter(double M)
    {
      return SunCalculator.C1 * math.sin(M) + SunCalculator.C2 * math.sin(2.0 * M) + SunCalculator.C3 * math.sin(3.0 * M);
    }

    private static double GetJulianCycle(double J, double lw)
    {
      return math.round(J - SunCalculator.J2000 - SunCalculator.J0 - lw / (2.0 * Math.PI));
    }

    private static double GetSolarTransit(double Js, double M, double Lsun)
    {
      return Js + SunCalculator.J1 * math.sin(M) + SunCalculator.J2 * math.sin(2.0 * Lsun);
    }

    private static double GetApproxSolarTransit(double Ht, double lw, double n)
    {
      return SunCalculator.J2000 + SunCalculator.J0 + (Ht + lw) / 6.2831854820251465 + n;
    }

    private static double GetSunsetJulianDate(
      double w0,
      double M,
      double Lsun,
      double lw,
      double n)
    {
      return SunCalculator.GetSolarTransit(SunCalculator.GetApproxSolarTransit(w0, lw, n), M, Lsun);
    }

    private static double GetSunriseJulianDate(double Jtransit, double Jset)
    {
      return Jtransit - (Jset - Jtransit);
    }

    private static double GetRightAscension(double Lsun)
    {
      return math.atan2(math.sin(Lsun) * math.cos(SunCalculator.e), math.cos(Lsun));
    }

    private static double GetSiderealTime(double J, double lw)
    {
      return SunCalculator.th0 + SunCalculator.th1 * (J - SunCalculator.J2000) - lw;
    }

    private static double GetAzimuth(double th, double a, double phi, double d)
    {
      double x = th - a;
      return math.atan2(math.sin(x), math.cos(x) * math.sin(phi) - math.tan(d) * math.cos(phi)) + 3.1415927410125732;
    }

    private static double GetAltitude(double th, double a, double phi, double d)
    {
      double x = th - a;
      return math.asin(math.sin(phi) * math.sin(d) + math.cos(phi) * math.cos(d) * math.cos(x));
    }

    public static SunCalculator.DayInfo GetDayInfo(DateTime date, float latitude, float longitude)
    {
      double lw1 = (double) math.radians(-longitude);
      double phi = (double) math.radians(latitude);
      double julianCycle = SunCalculator.GetJulianCycle(date.ConvertToJulianDateTime(), lw1);
      double approxSolarTransit = SunCalculator.GetApproxSolarTransit(0.0, lw1, julianCycle);
      double solarMeanAnomaly = SunCalculator.GetSolarMeanAnomaly(approxSolarTransit);
      double equationOfCenter = SunCalculator.GetEquationOfCenter(solarMeanAnomaly);
      double eclipticLongitude = SunCalculator.GetEclipticLongitude(solarMeanAnomaly, equationOfCenter);
      double sunDeclination = SunCalculator.GetSunDeclination(eclipticLongitude);
      double solarTransit = SunCalculator.GetSolarTransit(approxSolarTransit, solarMeanAnomaly, eclipticLongitude);
      double hourAngle1 = SunCalculator.GetHourAngle(SunCalculator.h0, phi, sunDeclination);
      double hourAngle2 = SunCalculator.GetHourAngle(SunCalculator.h0 + SunCalculator.d0, phi, sunDeclination);
      double hourAngle3 = SunCalculator.GetHourAngle(SunCalculator.h1, phi, sunDeclination);
      double hourAngle4 = SunCalculator.GetHourAngle(SunCalculator.h2, phi, sunDeclination);
      double hourAngle5 = SunCalculator.GetHourAngle(SunCalculator.h3, phi, sunDeclination);
      double sunsetJulianDate1 = SunCalculator.GetSunsetJulianDate(hourAngle1, solarMeanAnomaly, eclipticLongitude, lw1, julianCycle);
      double M = solarMeanAnomaly;
      double Lsun = eclipticLongitude;
      double lw2 = lw1;
      double n = julianCycle;
      double sunsetJulianDate2 = SunCalculator.GetSunsetJulianDate(hourAngle2, M, Lsun, lw2, n);
      double sunsetJulianDate3 = SunCalculator.GetSunsetJulianDate(hourAngle3, solarMeanAnomaly, eclipticLongitude, lw1, julianCycle);
      double sunsetJulianDate4 = SunCalculator.GetSunsetJulianDate(hourAngle4, solarMeanAnomaly, eclipticLongitude, lw1, julianCycle);
      double sunsetJulianDate5 = SunCalculator.GetSunsetJulianDate(hourAngle5, solarMeanAnomaly, eclipticLongitude, lw1, julianCycle);
      double sunriseJulianDate1 = SunCalculator.GetSunriseJulianDate(solarTransit, sunsetJulianDate1);
      double sunriseJulianDate2 = SunCalculator.GetSunriseJulianDate(solarTransit, sunsetJulianDate2);
      double sunriseJulianDate3 = SunCalculator.GetSunriseJulianDate(solarTransit, sunsetJulianDate4);
      double sunriseJulianDate4 = SunCalculator.GetSunriseJulianDate(solarTransit, sunsetJulianDate5);
      double sunriseJulianDate5 = SunCalculator.GetSunriseJulianDate(solarTransit, sunsetJulianDate3);
      return new SunCalculator.DayInfo()
      {
        dawn = sunriseJulianDate5.ConvertToDateTime(),
        sunrise = {
          start = sunriseJulianDate1.ConvertToDateTime(),
          end = sunriseJulianDate2.ConvertToDateTime()
        },
        transit = solarTransit.ConvertToDateTime(),
        sunset = {
          start = sunsetJulianDate2.ConvertToDateTime(),
          end = sunsetJulianDate1.ConvertToDateTime()
        },
        dusk = sunsetJulianDate3.ConvertToDateTime(),
        morningTwilight = {
          astronomical = {
            start = sunriseJulianDate4.ConvertToDateTime(),
            end = sunriseJulianDate3.ConvertToDateTime()
          },
          nautical = {
            start = sunriseJulianDate3.ConvertToDateTime(),
            end = sunriseJulianDate5.ConvertToDateTime()
          },
          civil = {
            start = sunriseJulianDate5.ConvertToDateTime(),
            end = sunriseJulianDate1.ConvertToDateTime()
          }
        },
        nightTwilight = {
          civil = {
            start = sunsetJulianDate1.ConvertToDateTime(),
            end = sunsetJulianDate3.ConvertToDateTime()
          },
          nautical = {
            start = sunsetJulianDate3.ConvertToDateTime(),
            end = sunsetJulianDate4.ConvertToDateTime()
          },
          astronomical = {
            start = sunsetJulianDate4.ConvertToDateTime(),
            end = sunsetJulianDate5.ConvertToDateTime()
          }
        }
      };
    }

    private static TopocentricCoordinates GetSunPosition(double J, double lw, double phi)
    {
      double solarMeanAnomaly = SunCalculator.GetSolarMeanAnomaly(J);
      double eclipticLongitude = SunCalculator.GetEclipticLongitude(solarMeanAnomaly, SunCalculator.GetEquationOfCenter(solarMeanAnomaly));
      double sunDeclination = SunCalculator.GetSunDeclination(eclipticLongitude);
      double rightAscension = SunCalculator.GetRightAscension(eclipticLongitude);
      double siderealTime = SunCalculator.GetSiderealTime(J, lw);
      return new TopocentricCoordinates()
      {
        azimuth = SunCalculator.GetAzimuth(siderealTime, rightAscension, phi, sunDeclination),
        altitude = SunCalculator.GetAltitude(siderealTime, rightAscension, phi, sunDeclination)
      };
    }

    public static TopocentricCoordinates GetSunPosition(
      DateTime date,
      float latitude,
      float longitude)
    {
      return SunCalculator.GetSunPosition(date.ConvertToJulianDateTime(), -(double) math.radians(longitude), (double) math.radians(latitude));
    }

    public struct Twiligth
    {
      public SunCalculator.TimeFrame astronomical;
      public SunCalculator.TimeFrame nautical;
      public SunCalculator.TimeFrame civil;
    }

    public struct TimeFrame
    {
      public DateTime start;
      public DateTime end;

      public override string ToString()
      {
        return string.Format("Start: {0} End: {1}", (object) this.start, (object) this.end);
      }
    }

    public struct DayInfo
    {
      public DateTime dawn;
      public SunCalculator.TimeFrame sunrise;
      public DateTime transit;
      public SunCalculator.TimeFrame sunset;
      public DateTime dusk;
      public SunCalculator.Twiligth morningTwilight;
      public SunCalculator.Twiligth nightTwilight;

      public override string ToString()
      {
        return string.Format("Dawn: {0}\nSunrise: {1}\nTransit: {2}\nSunset: {3}\nDusk: {4}\nAstronomical morning twilight: {5}\nNautical morning twilight: {6}\nCivil morning twilight: {7}\nCivil night twilight: {8}\nNautical night twilight: {9}\nAstronomical morning twilight: {10}", (object) this.dawn, (object) this.sunrise, (object) this.transit, (object) this.sunset, (object) this.dusk, (object) this.morningTwilight.astronomical, (object) this.morningTwilight.nautical, (object) this.morningTwilight.civil, (object) this.nightTwilight.civil, (object) this.nightTwilight.nautical, (object) this.nightTwilight.astronomical);
      }
    }
  }
}
