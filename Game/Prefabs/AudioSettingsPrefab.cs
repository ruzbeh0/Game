// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AudioSettingsPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class AudioSettingsPrefab : PrefabBase
  {
    public EffectPrefab[] m_Effects;
    public float m_MinHeight;
    public float m_MaxHeight;
    [Range(0.0f, 1f)]
    public float m_OverlapRatio = 0.8f;
    [Range(0.0f, 1f)]
    public float m_MinDistanceRatio = 0.5f;
    [Header("Culling")]
    public int m_FireCullMaxAmount;
    public float m_FireCullMaxDistance;
    public int m_CarEngineCullMaxAmount;
    public float m_CarEngineCullMaxDistance;
    public int m_PublicTransCullMaxAmount;
    public float m_PublicTransCullMaxDistance;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AmbientAudioSettingsData>());
      components.Add(ComponentType.ReadWrite<AmbientAudioEffect>());
      components.Add(ComponentType.ReadWrite<CullingAudioSettingsData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      DynamicBuffer<AmbientAudioEffect> buffer = entityManager.GetBuffer<AmbientAudioEffect>(entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_Effects.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        buffer.Add(new AmbientAudioEffect()
        {
          m_Effect = systemManaged.GetEntity((PrefabBase) this.m_Effects[index])
        });
      }
      AmbientAudioSettingsData componentData1 = new AmbientAudioSettingsData()
      {
        m_MaxHeight = this.m_MaxHeight,
        m_MinDistanceRatio = this.m_MinDistanceRatio,
        m_MinHeight = this.m_MinHeight,
        m_OverlapRatio = this.m_OverlapRatio
      };
      entityManager.SetComponentData<AmbientAudioSettingsData>(entity, componentData1);
      CullingAudioSettingsData componentData2 = new CullingAudioSettingsData()
      {
        m_FireCullMaxAmount = this.m_FireCullMaxAmount,
        m_FireCullMaxDistance = this.m_FireCullMaxDistance,
        m_CarEngineCullMaxAmount = this.m_CarEngineCullMaxAmount,
        m_CarEngineCullMaxDistance = this.m_CarEngineCullMaxDistance,
        m_PublicTransCullMaxAmount = this.m_PublicTransCullMaxAmount,
        m_PublicTransCullMaxDistance = this.m_PublicTransCullMaxDistance
      };
      entityManager.SetComponentData<CullingAudioSettingsData>(entity, componentData2);
    }
  }
}
