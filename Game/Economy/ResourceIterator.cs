// Decompiled with JetBrains decompiler
// Type: Game.Economy.ResourceIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Economy
{
  public struct ResourceIterator
  {
    public Resource resource;

    public static ResourceIterator GetIterator()
    {
      return new ResourceIterator()
      {
        resource = Resource.NoResource
      };
    }

    public bool Next()
    {
      this.resource = (Resource) Math.Max(1UL, (ulong) this.resource << 1);
      return this.resource != Resource.Last;
    }
  }
}
