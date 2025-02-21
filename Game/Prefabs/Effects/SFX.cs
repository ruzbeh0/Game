// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Effects.SFX
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Audio;
using Game.Effects;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs.Effects
{
  [ComponentMenu("Effects/", new System.Type[] {typeof (EffectPrefab)})]
  public class SFX : ComponentBase
  {
    public AudioClip m_AudioClip;
    [Range(0.0f, 1f)]
    public float m_Volume = 1f;
    [Range(-3f, 3f)]
    public float m_Pitch = 1f;
    [Range(0.0f, 1f)]
    public float m_SpatialBlend = 1f;
    [Range(0.0f, 1f)]
    public float m_Doppler = 1f;
    public float m_Spread;
    public AudioRolloffMode m_RolloffMode = AudioRolloffMode.Linear;
    public float2 m_MinMaxDistance = new float2(1f, 200f);
    public bool m_Loop;
    public MixerGroup m_MixerGroup;
    public byte m_Priority = 128;
    public AnimationCurve m_RolloffCurve;
    public float3 m_SourceSize;
    public float2 m_FadeTimes;
    public bool m_RandomStartTime;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<AudioEffectData>());
      components.Add(ComponentType.ReadWrite<AudioSourceData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      AudioManager existingSystemManaged = entityManager.World.GetExistingSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<AudioEffectData>(entity, new AudioEffectData()
      {
        m_AudioClipId = existingSystemManaged.RegisterSFX(this),
        m_MaxDistance = this.m_MinMaxDistance.y,
        m_SourceSize = this.m_SourceSize,
        m_FadeTimes = this.m_FadeTimes
      });
      DynamicBuffer<AudioSourceData> buffer = entityManager.GetBuffer<AudioSourceData>(entity);
      buffer.ResizeUninitialized(1);
      buffer[0] = new AudioSourceData()
      {
        m_SFXEntity = entity
      };
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }
  }
}
