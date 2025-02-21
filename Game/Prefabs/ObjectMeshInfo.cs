// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectMeshInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using Game.UI.Widgets;
using System;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class ObjectMeshInfo
  {
    public RenderPrefabBase m_Mesh;
    [InputField]
    [RangeN(-10000f, 10000f, true)]
    public float3 m_Position;
    public quaternion m_Rotation = quaternion.identity;
    public ObjectState m_RequireState;
  }
}
