// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Effects.AudioSpot
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs.Effects
{
  [ComponentMenu("Effects/", new Type[] {typeof (EffectPrefab)})]
  public class AudioSpot : ComponentBase
  {
    [EditorName("Sound Effects")]
    public EffectPrefab[] m_SFXes;
    public float m_MinimumInterval;
    public float m_MaximumInterval;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<AudioSpotData>());
      components.Add(ComponentType.ReadWrite<AudioSourceData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      if (this.m_SFXes != null)
      {
        for (int index1 = 0; index1 < this.m_SFXes.Length; ++index1)
        {
          bool flag = false;
          for (int index2 = 0; index2 < this.m_SFXes[index1].components.Count; ++index2)
          {
            ComponentBase component = this.m_SFXes[index1].components[index2];
            if (component is SFX)
            {
              flag = true;
              if (((SFX) component).m_Loop)
              {
                // ISSUE: reference to a compiler-generated method
                string name = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>().GetPrefab<EffectPrefab>(entity).name;
                ComponentBase.baseLog.WarnFormat("Warning: AudioSpot {0} SFX {1} is looping", (object) name, (object) index2);
              }
            }
          }
          if (!flag)
            ComponentBase.baseLog.WarnFormat("Warning: AudioSpot {0} has SFX without SFX component", (object) this.name);
        }
        DynamicBuffer<AudioSourceData> buffer = entityManager.GetBuffer<AudioSourceData>(entity);
        buffer.ResizeUninitialized(this.m_SFXes.Length);
        for (int index = 0; index < this.m_SFXes.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          buffer[index] = new AudioSourceData()
          {
            m_SFXEntity = systemManaged.GetEntity((PrefabBase) this.m_SFXes[index])
          };
        }
      }
      else
        ComponentBase.baseLog.WarnFormat("Warning: AudioSpot {0} has no sound effects", (object) this.name);
      entityManager.SetComponentData<AudioSpotData>(entity, new AudioSpotData()
      {
        m_Interval = new float2(this.m_MinimumInterval, this.m_MaximumInterval)
      });
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_SFXes == null)
        return;
      foreach (EffectPrefab sfXe in this.m_SFXes)
        prefabs.Add((PrefabBase) sfXe);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }
  }
}
