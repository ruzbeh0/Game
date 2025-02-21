// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.QuantityObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Economy;
using Game.Objects;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new System.Type[] {typeof (StaticObjectPrefab)})]
  public class QuantityObject : ComponentBase
  {
    public ResourceInEditor[] m_Resources;
    public MapFeature m_MapFeature = MapFeature.None;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<QuantityObjectData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Quantity>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      QuantityObjectData componentData = new QuantityObjectData();
      if (this.m_Resources != null)
      {
        for (int index = 0; index < this.m_Resources.Length; ++index)
          componentData.m_Resources |= EconomyUtils.GetResource(this.m_Resources[index]);
      }
      componentData.m_MapFeature = this.m_MapFeature;
      entityManager.SetComponentData<QuantityObjectData>(entity, componentData);
      if (componentData.m_Resources != Resource.NoResource || componentData.m_MapFeature != MapFeature.None || this.prefab.Has<PlaceholderObject>())
        return;
      ComponentBase.baseLog.WarnFormat((UnityEngine.Object) this.prefab, "QuantityObject has no resource: {0}", (object) this.prefab.name);
    }
  }
}
