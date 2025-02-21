// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.WeatherPlaceholder
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs.Climate
{
  [ComponentMenu("Weather/", new System.Type[] {typeof (WeatherPrefab)})]
  public class WeatherPlaceholder : ComponentBase
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceholderObjectElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (!this.prefab.Has<WeatherOverride>())
        return;
      ComponentBase.baseLog.WarnFormat((UnityEngine.Object) this.prefab, "WeatherPlaceholder is WeatherOverride: {0}", (object) this.prefab.name);
    }
  }
}
