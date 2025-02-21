// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialActivationEvent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/", new Type[] {typeof (TriggerPrefab)})]
  public class TutorialActivationEvent : ComponentBase
  {
    public TutorialPrefab[] m_Tutorials;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TutorialActivationEventData>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Tutorials == null)
        return;
      for (int index = 0; index < this.m_Tutorials.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Tutorials[index]);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      DynamicBuffer<TutorialActivationEventData> buffer = entityManager.GetBuffer<TutorialActivationEventData>(entity);
      if (this.m_Tutorials == null)
        return;
      for (int index = 0; index < this.m_Tutorials.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        buffer.Add(new TutorialActivationEventData()
        {
          m_Tutorial = systemManaged.GetEntity((PrefabBase) this.m_Tutorials[index])
        });
      }
    }
  }
}
