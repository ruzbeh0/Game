// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CreaturePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Creatures;
using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Creatures/", new Type[] {})]
  public class CreaturePrefab : MovingObjectPrefab
  {
    public GenderMask m_Gender = GenderMask.Any;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CreatureData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Creature>());
      components.Add(ComponentType.ReadWrite<Color>());
      components.Add(ComponentType.ReadWrite<Surface>());
    }
  }
}
