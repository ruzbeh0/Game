// Decompiled with JetBrains decompiler
// Type: Colossal.Atmosphere.TopocentricCoordinates
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Colossal.Atmosphere
{
  public struct TopocentricCoordinates
  {
    public double azimuth;
    public double altitude;
    private static readonly string[] kCardinals = new string[9]
    {
      "N",
      "NE",
      "E",
      "SE",
      "S",
      "SW",
      "W",
      "NW",
      "N"
    };

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
      return (float) (((double) value - (double) from1) / ((double) to1 - (double) from1) * ((double) to2 - (double) from2)) + from2;
    }

    public float3 ToLocalCoordinates(out float planetTime)
    {
      double theta = 1.5707963705062866 - this.altitude;
      double azimuth = this.azimuth;
      planetTime = math.saturate(this.Remap((float) theta, -1.5f, 0.0f, 1.5f, 1f));
      return TopocentricCoordinates.ConvertToLocalCoordinates((float) theta, (float) azimuth);
    }

    public static float3 ConvertToLocalCoordinates(float theta, float phi)
    {
      float num = math.sin(theta);
      return new float3(num * math.sin(phi), math.cos(theta), num * math.cos(phi));
    }

    public void Quantize(double resolutionRadians)
    {
      this.azimuth = math.round(this.azimuth / resolutionRadians) * resolutionRadians;
      this.altitude = math.round(this.altitude / resolutionRadians) * resolutionRadians;
    }

    private static string DegreesToCardinal(double degrees)
    {
      return TopocentricCoordinates.kCardinals[(int) math.round(degrees % 360.0 / 45.0)];
    }

    private static string FormatAzimuth(double azimuth)
    {
      double degrees = math.degrees(azimuth);
      return string.Format("{0} ({1}° - {2})", (object) azimuth, (object) degrees, (object) TopocentricCoordinates.DegreesToCardinal(degrees));
    }

    private static string FormatAltitude(double altitude)
    {
      double num = math.degrees(altitude);
      return string.Format("{0} ({1}°)", (object) altitude, (object) num);
    }

    public override string ToString()
    {
      return "(azimuth: " + TopocentricCoordinates.FormatAzimuth(this.azimuth) + ", altitude: " + TopocentricCoordinates.FormatAltitude(this.altitude) + ")";
    }
  }
}
