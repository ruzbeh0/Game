// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Legacy.LegacyFrustumPlanes
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Rendering.Legacy
{
  public struct LegacyFrustumPlanes
  {
    public Plane left;
    public Plane right;
    public Plane bottom;
    public Plane top;
    public Plane zNear;
    public Plane zFar;

    public Plane this[int i]
    {
      get
      {
        switch (i)
        {
          case 0:
            return this.left;
          case 1:
            return this.right;
          case 2:
            return this.bottom;
          case 3:
            return this.top;
          case 4:
            return this.zNear;
          case 5:
            return this.zFar;
          default:
            return new Plane();
        }
      }
    }
  }
}
