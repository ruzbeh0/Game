// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AudioGroupSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using System;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class AudioGroupSettings
  {
    public GroupAmbienceType m_Type;
    [Tooltip("The higher the value, the faster the traffic group sound reacts to changes in the traffic; balance between delay and jumpiness")]
    [Range(0.0f, 1f)]
    public float m_FadeSpeed = 0.05f;
    [Tooltip("The higher this value, the less traffic is needed for the group sound effect to be heard")]
    public float m_Scale;
    [Tooltip("Controls min and max height for the camera height linear rolloff for the far sound")]
    public float2 m_Height;
    public EffectPrefab m_GroupSoundNear;
    public EffectPrefab m_GroupSoundFar;
    [Tooltip("Controls min and max height for the camera height linear rolloff for the near sound")]
    public float2 m_NearHeight;
    [Tooltip("Larger values mean the near sound is only heard more near the sources, 1 = same as far")]
    public float m_NearWeight = 2f;
  }
}
