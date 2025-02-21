// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ResourcePrefabs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ResourcePrefabs
  {
    private NativeArray<Entity> m_ResourcePrefabs;

    public ResourcePrefabs(NativeArray<Entity> resourcePrefabs)
    {
      this.m_ResourcePrefabs = resourcePrefabs;
    }

    public Entity this[Resource resource]
    {
      get
      {
        int resourceIndex = EconomyUtils.GetResourceIndex(resource);
        return resourceIndex >= 0 && resourceIndex < this.m_ResourcePrefabs.Length ? this.m_ResourcePrefabs[resourceIndex] : Entity.Null;
      }
    }
  }
}
