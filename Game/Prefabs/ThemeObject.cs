// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ThemeObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Themes/", new System.Type[] {typeof (ZonePrefab), typeof (ObjectPrefab), typeof (NetPrefab), typeof (AreaPrefab), typeof (RoutePrefab), typeof (NetLanePrefab)})]
  public class ThemeObject : ComponentBase
  {
    public ThemePrefab m_Theme;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_Theme);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ObjectRequirementElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<ObjectRequirementElement> buffer = entityManager.GetBuffer<ObjectRequirementElement>(entity);
      int length = buffer.Length;
      // ISSUE: reference to a compiler-generated method
      buffer.Add(new ObjectRequirementElement(existingSystemManaged.GetEntity((PrefabBase) this.m_Theme), length, ObjectRequirementType.IgnoreExplicit));
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        if ((UnityEngine.Object) this.m_Theme != (UnityEngine.Object) null)
          yield return this.m_Theme.name;
      }
    }
  }
}
