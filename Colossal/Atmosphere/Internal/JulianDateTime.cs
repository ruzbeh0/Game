// Decompiled with JetBrains decompiler
// Type: Colossal.Atmosphere.Internal.JulianDateTime
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Colossal.Atmosphere.Internal
{
  public static class JulianDateTime
  {
    public static bool IsJulianDate(int year, int month, int day)
    {
      if (year < 1582)
        return true;
      if (year > 1582)
        return false;
      if (month < 10)
        return true;
      if (month > 10)
        return false;
      if (day < 5)
        return true;
      if (day > 14)
        return false;
      throw new ArgumentOutOfRangeException("This date is not valid as it does not exist in either the Julian or the Gregorian calendars.");
    }

    private static double DateToJulianDate(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond)
    {
      int num1 = JulianDateTime.IsJulianDate(year, month, day) ? 1 : 0;
      int num2 = month > 2 ? month : month + 12;
      int num3 = month > 2 ? year : year - 1;
      double num4 = (double) day + (double) hour / 24.0 + (double) minute / 1440.0 + ((double) second + (double) millisecond / 1000.0) / 86400.0;
      int num5 = num1 != 0 ? 0 : 2 - num3 / 100 + num3 / 100 / 4;
      return (double) ((int) (365.25 * (double) (num3 + 4716)) + (int) (30.6001 * (double) (num2 + 1))) + num4 + (double) num5 - 1524.5;
    }

    private static DateTime JulianDateToDate(double julianDate) => new DateTime();

    public static double ConvertToJulianDateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond)
    {
      return JulianDateTime.DateToJulianDate(year, month, day, hour, minute, second, millisecond);
    }

    public static double ConvertToJulianDateTime(this DateTime date)
    {
      return JulianDateTime.DateToJulianDate(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
    }

    public static DateTime ConvertToDateTime(this double date)
    {
      return JulianDateTime.JulianDateToDate(date);
    }
  }
}
