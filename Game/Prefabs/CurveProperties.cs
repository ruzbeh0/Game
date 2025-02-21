﻿// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CurveProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Serialization;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new Type[] {typeof (RenderPrefab)})]
  public class CurveProperties : ComponentBase
  {
    public int m_TilingCount;
    public float m_OverrideLength;
    public float m_SmoothingDistance;
    public bool m_GeometryTiling;
    public bool m_StraightTiling;
    public bool m_InvertCurve;
    public bool m_SubFlow;
    [FormerlySerializedAs("m_Hanging")]
    public bool m_HangingSwaying;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }
  }
}
