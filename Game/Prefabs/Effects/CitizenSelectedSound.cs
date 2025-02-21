// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Effects.CitizenSelectedSound
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs.Effects
{
  [ComponentMenu("Effects/", new Type[] {typeof (CitizenPrefab)})]
  public class CitizenSelectedSound : ComponentBase
  {
    public CitizenSelectedSoundInfo[] m_CitizenSelectedSounds;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CitizenSelectedSoundData>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_CitizenSelectedSounds.Length; ++index)
        prefabs.Add((PrefabBase) this.m_CitizenSelectedSounds[index].m_SelectedSound);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      if (this.m_CitizenSelectedSounds == null)
        return;
      DynamicBuffer<CitizenSelectedSoundData> buffer = entityManager.GetBuffer<CitizenSelectedSoundData>(entity);
      for (int index = 0; index < this.m_CitizenSelectedSounds.Length; ++index)
      {
        CitizenSelectedSoundInfo citizenSelectedSound = this.m_CitizenSelectedSounds[index];
        // ISSUE: reference to a compiler-generated method
        buffer.Add(new CitizenSelectedSoundData(citizenSelectedSound.m_IsSickOrInjured, citizenSelectedSound.m_Age, citizenSelectedSound.m_Happiness, systemManaged.GetEntity((PrefabBase) citizenSelectedSound.m_SelectedSound)));
      }
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }
  }
}
