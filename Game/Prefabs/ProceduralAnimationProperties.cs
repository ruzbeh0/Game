// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ProceduralAnimationProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new System.Type[] {typeof (RenderPrefab)})]
  public class ProceduralAnimationProperties : ComponentBase
  {
    public ProceduralAnimationProperties.BoneInfo[] m_Bones;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ProceduralBone>());
    }

    [Serializable]
    public class BoneInfo
    {
      public string name;
      public Vector3 position;
      [EulerAngles]
      public Quaternion rotation;
      public Vector3 scale;
      public Matrix4x4 bindPose;
      public int parentId;
      public BoneType m_Type;
      public float m_Speed;
      public float m_Acceleration;
      public int m_ConnectionID;
    }
  }
}
