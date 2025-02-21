// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingTerraformOverride
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new Type[] {typeof (StaticObjectPrefab)})]
  public class BuildingTerraformOverride : ComponentBase
  {
    public float2 m_LevelMinOffset;
    public float2 m_LevelMaxOffset;
    public float2 m_LevelFrontLeft = new float2(1f, 1f);
    public float2 m_LevelFrontRight = new float2(1f, 1f);
    public float2 m_LevelBackLeft = new float2(1f, 1f);
    public float2 m_LevelBackRight = new float2(1f, 1f);
    public float2 m_SmoothMinOffset;
    public float2 m_SmoothMaxOffset;
    public float m_HeightOffset;
    public BuildingTerraformOverride.SubLot[] m_AdditionalSmoothAreas;
    public bool m_DontRaise;
    public bool m_DontLower;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!(this.prefab is BuildingExtensionPrefab))
        return;
      components.Add(ComponentType.ReadWrite<Lot>());
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<BuildingTerraformData>());
      if (this.m_AdditionalSmoothAreas == null || this.m_AdditionalSmoothAreas.Length == 0)
        return;
      components.Add(ComponentType.ReadWrite<AdditionalBuildingTerraformElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      if (this.m_AdditionalSmoothAreas == null || this.m_AdditionalSmoothAreas.Length == 0)
        return;
      DynamicBuffer<AdditionalBuildingTerraformElement> buffer = entityManager.GetBuffer<AdditionalBuildingTerraformElement>(entity);
      buffer.ResizeUninitialized(this.m_AdditionalSmoothAreas.Length);
      for (int index = 0; index < this.m_AdditionalSmoothAreas.Length; ++index)
      {
        BuildingTerraformOverride.SubLot additionalSmoothArea = this.m_AdditionalSmoothAreas[index];
        buffer[index] = new AdditionalBuildingTerraformElement()
        {
          m_Area = additionalSmoothArea.m_Area,
          m_HeightOffset = additionalSmoothArea.m_HeightOffset,
          m_Circular = additionalSmoothArea.m_Circular,
          m_DontRaise = additionalSmoothArea.m_DontRaise,
          m_DontLower = additionalSmoothArea.m_DontLower
        };
      }
    }

    [Serializable]
    public class SubLot
    {
      public Bounds2 m_Area = new Bounds2((float2) -4f, (float2) 4f);
      public float m_HeightOffset;
      public bool m_Circular;
      public bool m_DontRaise;
      public bool m_DontLower;
    }
  }
}
