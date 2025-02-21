// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Legacy.Plane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.Rendering.Legacy
{
  public struct Plane
  {
    private float3 m_Normal;
    private float m_Distance;

    public float3 normal
    {
      get => this.m_Normal;
      set => this.m_Normal = value;
    }

    public float distance
    {
      get => this.m_Distance;
      set => this.m_Distance = value;
    }

    public Plane(float3 inNormal, float3 inPoint)
    {
      this.m_Normal = math.normalize(inNormal);
      this.m_Distance = -math.dot(this.m_Normal, inPoint);
    }

    public Plane(float3 inNormal, float d)
    {
      this.m_Normal = math.normalize(inNormal);
      this.m_Distance = d;
    }

    public Plane(float3 a, float3 b, float3 c)
    {
      this.m_Normal = math.normalize(math.cross(b - a, c - a));
      this.m_Distance = -math.dot(this.m_Normal, a);
    }

    public void SetNormalAndPosition(float3 inNormal, float3 inPoint)
    {
      this.m_Normal = math.normalize(inNormal);
      this.m_Distance = -math.dot(inNormal, inPoint);
    }

    public void Set3Points(float3 a, float3 b, float3 c)
    {
      this.m_Normal = math.normalize(math.cross(b - a, c - a));
      this.m_Distance = -math.dot(this.m_Normal, a);
    }

    public void Flip()
    {
      this.m_Normal = -this.m_Normal;
      this.m_Distance = -this.m_Distance;
    }

    public Plane flipped => new Plane(-this.m_Normal, -this.m_Distance);

    public void Translate(float3 translation)
    {
      this.m_Distance += math.dot(this.m_Normal, translation);
    }

    public static Plane Translate(Plane plane, float3 translation)
    {
      return new Plane(plane.m_Normal, plane.m_Distance += math.dot(plane.m_Normal, translation));
    }

    public float3 ClosestPointOnPlane(float3 point)
    {
      float num = math.dot(this.m_Normal, point) + this.m_Distance;
      return point - this.m_Normal * num;
    }

    public float GetDistanceToPoint(float3 point)
    {
      return math.dot(this.m_Normal, point) + this.m_Distance;
    }

    public bool GetSide(float3 point)
    {
      return (double) math.dot(this.m_Normal, point) + (double) this.m_Distance > 0.0;
    }

    public bool SameSide(float3 inPt0, float3 inPt1)
    {
      float distanceToPoint1 = this.GetDistanceToPoint(inPt0);
      float distanceToPoint2 = this.GetDistanceToPoint(inPt1);
      if ((double) distanceToPoint1 > 0.0 && (double) distanceToPoint2 > 0.0)
        return true;
      return (double) distanceToPoint1 <= 0.0 && (double) distanceToPoint2 <= 0.0;
    }

    public override string ToString()
    {
      return string.Format("(normal:({0:F1}, {1:F1}, {2:F1}), distance:{3:F1})", (object) this.m_Normal.x, (object) this.m_Normal.y, (object) this.m_Normal.z, (object) this.m_Distance);
    }
  }
}
