// Decompiled with JetBrains decompiler
// Type: Colossal.Atmosphere.JulianDateTime
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Colossal.Atmosphere
{
  public struct JulianDateTime
  {
    private static readonly JulianDateTime J2000 = new JulianDateTime(2451545.0);
    private const double kSecPerDay = 86400.0;
    private const double kOmegaE = 1.00273790934;
    private long m_Day;
    private double m_Fraction;

    public static implicit operator JulianDateTime(DateTime utc) => new JulianDateTime(utc);

    public static implicit operator double(JulianDateTime aDate) => aDate.ToDouble();

    public static implicit operator DateTime(JulianDateTime aDate) => aDate.ToDateTime();

    public JulianDateTime(double j)
    {
      this.m_Day = (long) j;
      this.m_Fraction = j - (double) this.m_Day;
    }

    public JulianDateTime(JulianDateTime j)
    {
      this.m_Day = j.m_Day;
      this.m_Fraction = j.m_Fraction;
    }

    public JulianDateTime(DateTime utc)
    {
      long year = (long) utc.Year;
      long month = (long) utc.Month;
      long day = (long) utc.Day;
      long num1 = 1461L * (year + 4800L + (month - 14L) / 12L) / 4L + 367L * (month - 2L - 12L * ((month - 14L) / 12L)) / 12L - 3L * ((year + 4900L + (month - 14L) / 12L) / 100L) / 4L + (day - 32075L);
      double num2 = utc.TimeOfDay.TotalDays - 0.5;
      if (num2 < 0.0)
      {
        ++num2;
        --num1;
      }
      this.m_Day = num1;
      this.m_Fraction = num2;
    }

    public double ToDouble() => (double) this.m_Day + this.m_Fraction;

    public DateTime ToDateTime()
    {
      long day1 = this.m_Day;
      double num1 = this.m_Fraction + 0.5;
      if (num1 >= 1.0)
      {
        --num1;
        ++day1;
      }
      long num2 = day1 + 68569L;
      long num3 = 4L * num2 / 146097L;
      long num4 = num2 - (146097L * num3 + 3L) / 4L;
      long num5 = 4000L * (num4 + 1L) / 1461001L;
      long num6 = num4 - (1461L * num5 / 4L - 31L);
      long num7 = 80L * num6 / 2447L;
      int day2 = (int) (num6 - 2447L * num7 / 80L);
      long num8 = num7 / 11L;
      int month = (int) (num7 + 2L - 12L * num8);
      return new DateTime((int) (100L * (num3 - 49L) + num5 + num8), month, day2, 0, 0, 0, DateTimeKind.Utc).AddDays(num1);
    }

    public override string ToString()
    {
      if (double.IsNaN(this.ToDouble()))
        return "N/A";
      DateTime dateTime = this.ToDateTime();
      dateTime = dateTime.ToLocalTime();
      return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
    }

    public double ToGMST()
    {
      double num1 = (this.ToDouble() + 0.5) % 1.0;
      double num2 = (JulianDateTime.J2000 - num1) / 36525.0;
      double num3 = (24110.54841 + num2 * (8640184.812866 + num2 * (0.093104 - num2 * 6.2E-06)) + 86636.555366976 * num1) % 86400.0;
      if (num3 < 0.0)
        num3 += 86400.0;
      return 6.2831854820251465 * (num3 / 86400.0);
    }

    public double ToLMST(double longitude)
    {
      return (this.ToGMST() + longitude) % 3.1415927410125732 * 2.0;
    }

    public void AddSeconds(double seconds)
    {
      this.m_Fraction += seconds / 86400.0;
      while (this.m_Fraction >= 1.0)
      {
        --this.m_Fraction;
        ++this.m_Day;
      }
      while (this.m_Fraction < 0.0)
      {
        ++this.m_Fraction;
        --this.m_Day;
      }
    }

    public double Subtract(JulianDateTime j)
    {
      this.m_Day -= j.m_Day;
      this.m_Fraction -= j.m_Fraction;
      if (this.m_Fraction < 0.0)
      {
        ++this.m_Fraction;
        --this.m_Day;
      }
      return this.ToDouble();
    }

    public double Subtract(double j) => this.ToDouble() - j;

    public static double operator -(JulianDateTime l, double r)
    {
      return new JulianDateTime(l).Subtract(r);
    }

    public static double operator -(JulianDateTime l, JulianDateTime j)
    {
      return new JulianDateTime(l).Subtract(j);
    }
  }
}
