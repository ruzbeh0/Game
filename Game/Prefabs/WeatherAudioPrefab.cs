// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WeatherAudioPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Game.Prefabs
{
  public class WeatherAudioPrefab : PrefabBase
  {
    [Header("Water")]
    [FormerlySerializedAs("m_SmallWaterAudio")]
    [Tooltip("The water ambient audio clip")]
    public EffectPrefab m_WaterAmbientAudio;
    [Tooltip("The water audio's intensity that will be applied to the audio clip's volume")]
    public float m_WaterAudioIntensity;
    [Tooltip("The water audio's fade in & fade out speed")]
    public float m_WaterFadeSpeed;
    [Tooltip("The water audio will not be heard when the camera zoom is bigger than it")]
    public int m_WaterAudioEnabledZoom;
    [Tooltip("The near cell's index distance that can play the water audio ")]
    public int m_WaterAudioNearDistance;
    [Header("Lightning")]
    [Tooltip("The lightning sound speed that affect the delay")]
    public float m_LightningSoundSpeed;
    [Tooltip("The lightning audio clip")]
    public EffectPrefab m_LightningAudio;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<WeatherAudioData>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs) => base.GetDependencies(prefabs);

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      WeatherAudioData componentData = new WeatherAudioData()
      {
        m_WaterFadeSpeed = this.m_WaterFadeSpeed,
        m_WaterAudioIntensity = this.m_WaterAudioIntensity,
        m_WaterAudioEnabledZoom = this.m_WaterAudioEnabledZoom,
        m_WaterAudioNearDistance = this.m_WaterAudioNearDistance,
        m_WaterAmbientAudio = systemManaged.GetEntity((PrefabBase) this.m_WaterAmbientAudio),
        m_LightningAudio = systemManaged.GetEntity((PrefabBase) this.m_LightningAudio),
        m_LightningSoundSpeed = this.m_LightningSoundSpeed
      };
      entityManager.SetComponentData<WeatherAudioData>(entity, componentData);
    }
  }
}
