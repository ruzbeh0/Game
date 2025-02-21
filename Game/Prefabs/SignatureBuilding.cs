// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SignatureBuilding
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Objects;
using Game.UI.Editor;
using Game.UI.Widgets;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab)})]
  public class SignatureBuilding : ComponentBase
  {
    public const int kStatLevel = 5;
    public ZonePrefab m_ZoneType;
    public int m_XPReward = 300;
    [CustomField(typeof (UIIconField))]
    public string m_UnlockEventImage;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_ZoneType);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SignatureBuildingData>());
      components.Add(ComponentType.ReadWrite<SpawnableBuildingData>());
      components.Add(ComponentType.ReadWrite<PlaceableObjectData>());
      components.Add(ComponentType.ReadWrite<PlaceableInfoviewItem>());
      if (!((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null))
        return;
      this.m_ZoneType.GetBuildingPrefabComponents(components, (BuildingPrefab) this.prefab, (byte) 5);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<BuildingCondition>());
      components.Add(ComponentType.ReadWrite<Game.Buildings.Signature>());
      components.Add(ComponentType.ReadWrite<Game.Objects.UniqueObject>());
      if (!((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null))
        return;
      this.m_ZoneType.GetBuildingArchetypeComponents(components, (BuildingPrefab) this.prefab, (byte) 5);
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      PlaceableObjectData componentData = entityManager.GetComponentData<PlaceableObjectData>(entity) with
      {
        m_XPReward = this.m_XPReward
      };
      if ((componentData.m_Flags & (PlacementFlags.Shoreline | PlacementFlags.Floating | PlacementFlags.Hovering)) == PlacementFlags.None)
        componentData.m_Flags |= PlacementFlags.OnGround;
      componentData.m_Flags |= PlacementFlags.Unique;
      entityManager.SetComponentData<PlaceableObjectData>(entity, componentData);
      if (!((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null))
        return;
      this.m_ZoneType.InitializeBuilding(entityManager, entity, (BuildingPrefab) this.prefab, (byte) 5);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      SpawnableBuildingData componentData = new SpawnableBuildingData()
      {
        m_Level = 5
      };
      if ((UnityEngine.Object) this.m_ZoneType != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        componentData.m_ZonePrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_ZoneType);
      }
      entityManager.SetComponentData<SpawnableBuildingData>(entity, componentData);
    }
  }
}
