// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PlaceholderBuilding
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab)})]
  public class PlaceholderBuilding : ComponentBase
  {
    public const int kStatLevel = 1;
    public BuildingType m_BuildingType;
    public ZonePrefab m_ZoneType;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_ZoneType);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceholderBuildingData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Placeholder>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<PlaceholderBuildingData>(entity, new PlaceholderBuildingData()
      {
        m_Type = this.m_BuildingType,
        m_ZonePrefab = (UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null ? existingSystemManaged.GetEntity((PrefabBase) this.m_ZoneType) : Entity.Null
      });
    }
  }
}
