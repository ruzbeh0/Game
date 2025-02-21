// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CharacterStyle
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Animations;
using Colossal.IO.AssetDatabase;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public class CharacterStyle : PrefabBase
  {
    public int m_ShapeCount;
    public int m_BoneCount;
    public GenderMask m_Gender = GenderMask.Any;
    public CharacterStyle.AnimationInfo[] m_Animations;

    public override bool ignoreUnlockDependencies => true;

    public AnimationAsset GetAnimation(int index)
    {
      return Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<AnimationAsset>((Colossal.Hash128) this.m_Animations[index].animationAsset);
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Animations.Length; ++index)
      {
        CharacterStyle.AnimationInfo animation = this.m_Animations[index];
        if ((UnityEngine.Object) animation.target != (UnityEngine.Object) null)
          prefabs.Add((PrefabBase) animation.target);
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CharacterStyleData>());
      components.Add(ComponentType.ReadWrite<AnimationClip>());
      components.Add(ComponentType.ReadWrite<Game.Prefabs.AnimationMotion>());
      components.Add(ComponentType.ReadWrite<RestPoseElement>());
    }

    public void CalculateRootMotion(
      BoneHierarchy hierarchy,
      Colossal.Animations.Animation animation,
      Colossal.Animations.Animation restPose,
      int infoIndex)
    {
      int length1 = animation.shapeIndices.Length > 1 ? this.m_ShapeCount : 1;
      int index1 = 0;
      if (animation.layer == Colossal.Animations.AnimationLayer.BodyLayer)
      {
        for (int index2 = 0; index2 < animation.boneIndices.Length; ++index2)
        {
          if (animation.boneIndices[index2] == 1)
          {
            index1 = 1;
            break;
          }
        }
      }
      int[] numArray1 = new int[hierarchy.hierarchyParentIndices.Length];
      int[] numArray2 = new int[length1];
      CharacterStyle.AnimationMotion[] animationMotionArray = new CharacterStyle.AnimationMotion[length1];
      for (int index3 = 0; index3 < numArray1.Length; ++index3)
        numArray1[index3] = -1;
      for (int index4 = 0; index4 < animation.boneIndices.Length; ++index4)
      {
        if (animation.boneIndices[index4] < numArray1.Length)
          numArray1[animation.boneIndices[index4]] = index4;
      }
      if (length1 > 1)
      {
        for (int index5 = 0; index5 < animation.shapeIndices.Length; ++index5)
          numArray2[animation.shapeIndices[index5]] = index5;
      }
      int length2 = animation.shapeIndices.Length;
      int length3 = animation.boneIndices.Length;
      for (int index6 = 0; index6 < length1; ++index6)
      {
        CharacterStyle.AnimationMotion animationMotion = animationMotionArray[index6] = new CharacterStyle.AnimationMotion();
        int num1 = numArray1[index1];
        int num2 = numArray2[index6];
        Colossal.Animations.Animation.ElementRaw elementRaw1;
        Colossal.Animations.Animation.ElementRaw elementRaw2;
        if (num1 >= 0)
        {
          int num3 = num1 * length2;
          int num4 = num3 + (animation.frameCount - 1) * length3 * length2;
          elementRaw1 = animation.DecodeElement(num3 + num2);
          elementRaw2 = animation.DecodeElement(num4 + num2);
        }
        else
        {
          int num5 = index1 * restPose.shapeIndices.Length;
          elementRaw1 = restPose.DecodeElement(num5 + index6);
          elementRaw2 = elementRaw1;
        }
        for (int hierarchyParentIndex = hierarchy.hierarchyParentIndices[index1]; hierarchyParentIndex != -1; hierarchyParentIndex = hierarchy.hierarchyParentIndices[hierarchyParentIndex])
        {
          int num6 = numArray1[hierarchyParentIndex];
          Colossal.Animations.Animation.ElementRaw elementRaw3;
          Colossal.Animations.Animation.ElementRaw elementRaw4;
          if (num6 >= 0)
          {
            int num7 = num6 * length2;
            int num8 = num7 + (animation.frameCount - 1) * length3 * length2;
            elementRaw3 = animation.DecodeElement(num7 + num2);
            elementRaw4 = animation.DecodeElement(num8 + num2);
          }
          else
          {
            int num9 = hierarchyParentIndex * restPose.shapeIndices.Length;
            elementRaw3 = restPose.DecodeElement(num9 + index6);
            elementRaw4 = elementRaw3;
          }
          elementRaw1.position = elementRaw3.position + math.mul((quaternion) elementRaw3.rotation, elementRaw1.position);
          elementRaw1.rotation = math.mul((quaternion) elementRaw3.rotation, (quaternion) elementRaw1.rotation).value;
          elementRaw2.position = elementRaw4.position + math.mul((quaternion) elementRaw4.rotation, elementRaw2.position);
          elementRaw2.rotation = math.mul((quaternion) elementRaw4.rotation, (quaternion) elementRaw2.rotation).value;
        }
        animationMotion.startOffset = elementRaw1.position;
        animationMotion.endOffset = elementRaw2.position;
        animationMotion.startRotation = math.normalize((quaternion) elementRaw1.rotation);
        animationMotion.endRotation = math.normalize((quaternion) elementRaw2.rotation);
      }
      this.m_Animations[infoIndex].rootMotionBone = index1;
      this.m_Animations[infoIndex].rootMotion = animationMotionArray;
    }

    [Serializable]
    public class AnimationMotion
    {
      public float3 startOffset;
      public float3 endOffset;
      public quaternion startRotation;
      public quaternion endRotation;
    }

    [Serializable]
    public class AnimationInfo
    {
      public string name;
      public AssetReference<AnimationAsset> animationAsset;
      public RenderPrefab target;
      public Colossal.Animations.AnimationType type;
      public Colossal.Animations.AnimationLayer layer;
      public int frameCount;
      public int frameRate;
      public int rootMotionBone;
      public CharacterStyle.AnimationMotion[] rootMotion;
      public ActivityType activity;
      public AnimationType state;
      [BitMask]
      public ActivityCondition conditions;
      public AnimationPlayback playback;
    }
  }
}
