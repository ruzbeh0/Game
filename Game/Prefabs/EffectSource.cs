// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EffectSource
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Effects;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Effects/", new System.Type[] {})]
  public class EffectSource : ComponentBase
  {
    public List<EffectSource.EffectSettings> m_Effects;
    public List<EffectSource.AnimationProperties> m_AnimationCurves;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Effect>());
      if (this.m_AnimationCurves == null || this.m_AnimationCurves.Count == 0)
        return;
      components.Add(ComponentType.ReadWrite<EffectAnimation>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (components.Contains(ComponentType.ReadWrite<NetCompositionData>()) || components.Contains(ComponentType.ReadWrite<Game.Net.Node>()))
        return;
      components.Add(ComponentType.ReadWrite<EnabledEffect>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Effects == null)
        return;
      foreach (EffectSource.EffectSettings effect in this.m_Effects)
        prefabs.Add((PrefabBase) effect.m_Effect);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      if (this.m_Effects != null)
      {
        DynamicBuffer<Effect> buffer = entityManager.GetBuffer<Effect>(entity);
        buffer.EnsureCapacity(this.m_Effects.Count);
        for (int index = 0; index < this.m_Effects.Count; ++index)
        {
          EffectSource.EffectSettings effect = this.m_Effects[index];
          if (!((UnityEngine.Object) effect.m_Effect == (UnityEngine.Object) null))
          {
            if ((double) effect.m_Intensity == 0.0)
              effect.m_Intensity = 1f;
            // ISSUE: reference to a compiler-generated method
            buffer.Add(new Effect()
            {
              m_Effect = existingSystemManaged.GetEntity((PrefabBase) effect.m_Effect),
              m_Position = effect.m_PositionOffset,
              m_Rotation = effect.m_Rotation,
              m_Scale = effect.m_Scale,
              m_Intensity = effect.m_Intensity,
              m_ParentMesh = effect.m_ParentMesh,
              m_AnimationIndex = effect.m_AnimationIndex
            });
          }
        }
      }
      if (this.m_AnimationCurves == null || this.m_AnimationCurves.Count == 0)
        return;
      DynamicBuffer<EffectAnimation> buffer1 = entityManager.GetBuffer<EffectAnimation>(entity);
      buffer1.ResizeUninitialized(this.m_AnimationCurves.Count);
      for (int index = 0; index < this.m_AnimationCurves.Count; ++index)
      {
        EffectSource.AnimationProperties animationCurve = this.m_AnimationCurves[index];
        buffer1[index] = new EffectAnimation()
        {
          m_DurationFrames = (uint) Mathf.RoundToInt(animationCurve.m_Duration * 60f),
          m_AnimationCurve = new AnimationCurve1(animationCurve.m_Curve)
        };
      }
    }

    [Serializable]
    public class EffectSettings
    {
      public EffectPrefab m_Effect;
      [InputField]
      [RangeN(-10000f, 10000f, true)]
      public float3 m_PositionOffset;
      public quaternion m_Rotation = quaternion.identity;
      public float3 m_Scale = new float3(1f, 1f, 1f);
      public float m_Intensity = 1f;
      public int m_ParentMesh;
      public int m_AnimationIndex = -1;
    }

    [Serializable]
    public class AnimationProperties
    {
      public float m_Duration;
      public AnimationCurve m_Curve;
    }
  }
}
