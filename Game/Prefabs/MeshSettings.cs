// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new Type[] {typeof (RenderingSettingsPrefab)})]
  public class MeshSettings : ComponentBase
  {
    public RenderPrefab m_MissingObjectMesh;
    public RenderPrefab m_DefaultBaseMesh;
    public NetSectionPrefab m_MissingNetSection;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_MissingObjectMesh);
      prefabs.Add((PrefabBase) this.m_DefaultBaseMesh);
      prefabs.Add((PrefabBase) this.m_MissingNetSection);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<MeshSettingsData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      MeshSettingsData componentData = new MeshSettingsData()
      {
        m_MissingObjectMesh = existingSystemManaged.GetEntity((PrefabBase) this.m_MissingObjectMesh),
        m_DefaultBaseMesh = existingSystemManaged.GetEntity((PrefabBase) this.m_DefaultBaseMesh),
        m_MissingNetSection = existingSystemManaged.GetEntity((PrefabBase) this.m_MissingNetSection)
      };
      entityManager.SetComponentData<MeshSettingsData>(entity, componentData);
    }
  }
}
