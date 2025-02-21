// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DilationProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new Type[] {typeof (RenderPrefab)})]
  public class DilationProperties : ComponentBase
  {
    public float m_MinSize = 0.1f;
    public float m_InfoviewFactor = 1f;
    public bool m_InfoviewOnly;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }
  }
}
