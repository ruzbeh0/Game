// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AnimationProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.GPUAnimation;
using Game.Rendering;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new System.Type[] {typeof (RenderPrefab)})]
  public class AnimationProperties : ComponentBase
  {
    public AnimationProperties.BakedAnimationClip[] m_Clips;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<AnimationClip>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.m_Clips == null)
        return;
      DynamicBuffer<AnimationClip> buffer = entityManager.GetBuffer<AnimationClip>(entity);
      buffer.ResizeUninitialized(this.m_Clips.Length);
      for (int index = 0; index < this.m_Clips.Length; ++index)
      {
        AnimationProperties.BakedAnimationClip clip = this.m_Clips[index];
        AnimationClip animationClip = new AnimationClip();
        ref AnimationClip local = ref animationClip;
        clip.CalculatePlaybackData(ref local);
        buffer[index] = animationClip;
      }
    }

    [Serializable]
    public class BakedAnimationClip
    {
      public string name;
      public int pixelStart;
      public int pixelEnd;
      public float animationLength;
      public bool looping;
      public Texture2DArray animationTexture;
      public AnimationType m_Type;
      public ActivityType m_Activity;
      public float m_MovementSpeed;
      public float3 m_RootOffset;
      public quaternion m_RootRotation;

      public BakedAnimationClip(
        Texture2DArray animTexture,
        KeyframeTextureBaker.AnimationClipData clipData)
      {
        this.name = clipData.clip.name;
        this.pixelStart = clipData.pixelStart;
        this.pixelEnd = clipData.pixelEnd;
        this.animationLength = clipData.clip.length;
        this.looping = clipData.clip.wrapMode == WrapMode.Loop;
        this.animationTexture = animTexture;
      }

      public void CalculatePlaybackData(ref AnimationClip clip)
      {
        int width = this.animationTexture.width;
        float num1 = 1f / (float) width;
        float num2 = (float) this.pixelStart / (float) width;
        float num3 = (float) this.pixelEnd / (float) width;
        clip.m_TextureOffset = num2;
        clip.m_TextureRange = num3 - num2;
        clip.m_OnePixelOffset = num1;
        clip.m_TextureWidth = (float) width;
        clip.m_OneOverTextureWidth = 1f / (float) width;
        clip.m_OneOverPixelOffset = 1f / num1;
        clip.m_AnimationLength = this.animationLength;
        clip.m_MovementSpeed = this.m_MovementSpeed;
        clip.m_RootOffset = this.m_RootOffset;
        clip.m_RootRotation = this.m_RootRotation;
        clip.m_Type = this.m_Type;
        clip.m_PropID = new AnimatedPropID(-1);
        clip.m_Activity = this.m_Activity;
        clip.m_Layer = AnimationLayer.Body;
        clip.m_Playback = this.looping ? AnimationPlayback.RandomLoop : AnimationPlayback.Once;
      }
    }
  }
}
