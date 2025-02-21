// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneBuiltRequirementPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Zones;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Prefabs/Unlocking/", new System.Type[] {})]
  public class ZoneBuiltRequirementPrefab : UnlockRequirementPrefab
  {
    public ThemePrefab m_RequiredTheme;
    public ZonePrefab m_RequiredZone;
    public AreaType m_RequiredType;
    public int m_MinimumSquares = 2500;
    public int m_MinimumCount;
    public int m_MinimumLevel = 1;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if ((UnityEngine.Object) this.m_RequiredTheme != (UnityEngine.Object) null)
        prefabs.Add((PrefabBase) this.m_RequiredTheme);
      if (!((UnityEngine.Object) this.m_RequiredZone != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_RequiredZone);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ZoneBuiltRequirementData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      entityManager.GetBuffer<UnlockRequirement>(entity).Add(new UnlockRequirement(entity, UnlockFlags.RequireAll));
      ZoneBuiltRequirementData componentData = new ZoneBuiltRequirementData();
      if ((UnityEngine.Object) this.m_RequiredTheme != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        componentData.m_RequiredTheme = existingSystemManaged.GetEntity((PrefabBase) this.m_RequiredTheme);
      }
      if ((UnityEngine.Object) this.m_RequiredZone != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        componentData.m_RequiredZone = existingSystemManaged.GetEntity((PrefabBase) this.m_RequiredZone);
      }
      componentData.m_RequiredType = this.m_RequiredType;
      componentData.m_MinimumSquares = this.m_MinimumSquares;
      componentData.m_MinimumCount = this.m_MinimumCount;
      componentData.m_MinimumLevel = (byte) this.m_MinimumLevel;
      entityManager.SetComponentData<ZoneBuiltRequirementData>(entity, componentData);
    }
  }
}
