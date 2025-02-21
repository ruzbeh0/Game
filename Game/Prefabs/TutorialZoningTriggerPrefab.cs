// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialZoningTriggerPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Triggers/", new System.Type[] {})]
  public class TutorialZoningTriggerPrefab : TutorialTriggerPrefabBase
  {
    [NotNull]
    public ZonePrefab[] m_Zones;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      foreach (ZonePrefab zone in this.m_Zones)
      {
        if ((UnityEngine.Object) zone != (UnityEngine.Object) null)
          prefabs.Add((PrefabBase) zone);
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ZoningTriggerData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      DynamicBuffer<ZoningTriggerData> buffer = entityManager.GetBuffer<ZoningTriggerData>(entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      foreach (ZonePrefab zone in this.m_Zones)
      {
        // ISSUE: reference to a compiler-generated method
        buffer.Add(new ZoningTriggerData(existingSystemManaged.GetEntity((PrefabBase) zone)));
      }
    }

    protected override void GenerateBlinkTags()
    {
      base.GenerateBlinkTags();
      foreach (ZonePrefab zone in this.m_Zones)
      {
        UIObject component;
        if (zone.TryGet<UIObject>(out component) && component.m_Group is UIAssetCategoryPrefab group && (UnityEngine.Object) group.m_Menu != (UnityEngine.Object) null)
        {
          this.AddBlinkTagAtPosition(zone.uiTag, 0);
          this.AddBlinkTagAtPosition(group.uiTag, 1);
          this.AddBlinkTagAtPosition(group.m_Menu.uiTag, 2);
        }
      }
    }

    public override void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
      base.GenerateTutorialLinks(entityManager, linkedPrefabs);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_Zones.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        linkedPrefabs.Add(existingSystemManaged.GetEntity((PrefabBase) this.m_Zones[index]));
      }
    }
  }
}
