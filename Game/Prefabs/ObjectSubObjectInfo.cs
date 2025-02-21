// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectSubObjectInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Widgets;
using System;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class ObjectSubObjectInfo
  {
    public ObjectPrefab m_Object;
    [InputField]
    [RangeN(-10000f, 10000f, true)]
    public float3 m_Position;
    public quaternion m_Rotation;
    public int m_ParentMesh;
    public int m_GroupIndex;
    [Range(0.0f, 100f)]
    public int m_Probability = 100;
  }
}
