// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingExtensionPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Effects;
using Game.Objects;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new Type[] {})]
  public class BuildingExtensionPrefab : StaticObjectPrefab
  {
    public bool m_ExternalLot;
    public float3 m_Position;
    public int2 m_OverrideLotSize;
    public float m_OverrideHeight;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<BuildingExtensionData>());
      components.Add(ComponentType.ReadWrite<Effect>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Extension>());
      components.Add(ComponentType.ReadWrite<Color>());
      components.Add(ComponentType.ReadWrite<EnabledEffect>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        yield return "Building";
      }
    }
  }
}
