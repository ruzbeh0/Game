// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AudioGroupingSettingsPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class AudioGroupingSettingsPrefab : PrefabBase
  {
    public AudioGroupSettings[] m_Settings;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AudioGroupingSettingsData>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      if (this.m_Settings != null)
      {
        for (int index = 0; index < this.m_Settings.Length; ++index)
        {
          if ((UnityEngine.Object) this.m_Settings[index].m_GroupSoundFar != (UnityEngine.Object) null)
            prefabs.Add((PrefabBase) this.m_Settings[index].m_GroupSoundFar);
          if ((UnityEngine.Object) this.m_Settings[index].m_GroupSoundNear != (UnityEngine.Object) null)
            prefabs.Add((PrefabBase) this.m_Settings[index].m_GroupSoundNear);
        }
      }
      base.GetDependencies(prefabs);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      DynamicBuffer<AudioGroupingSettingsData> buffer = entityManager.GetBuffer<AudioGroupingSettingsData>(entity);
      if (this.m_Settings == null)
        return;
      for (int index = 0; index < this.m_Settings.Length; ++index)
      {
        AudioGroupSettings setting = this.m_Settings[index];
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        buffer.Add(new AudioGroupingSettingsData()
        {
          m_Type = setting.m_Type,
          m_FadeSpeed = setting.m_FadeSpeed,
          m_Scale = setting.m_Scale,
          m_GroupSoundFar = systemManaged.GetEntity((PrefabBase) setting.m_GroupSoundFar),
          m_GroupSoundNear = (UnityEngine.Object) setting.m_GroupSoundNear != (UnityEngine.Object) null ? systemManaged.GetEntity((PrefabBase) setting.m_GroupSoundNear) : Entity.Null,
          m_Height = setting.m_Height,
          m_NearHeight = setting.m_NearHeight,
          m_NearWeight = setting.m_NearWeight
        });
      }
    }
  }
}
