// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Effects.RandomTransform
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs.Effects
{
  [ComponentMenu("Effects/", new Type[] {typeof (EffectPrefab)})]
  public class RandomTransform : ComponentBase
  {
    public float3 m_MinAngle = new float3(0.0f, 0.0f, 0.0f);
    public float3 m_MaxAngle = new float3(0.0f, 0.0f, 360f);
    public float3 m_MinPosition = new float3(0.0f, 0.0f, 0.0f);
    public float3 m_MaxPosition = new float3(0.0f, 0.0f, 0.0f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<RandomTransformData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      RandomTransformData componentData;
      componentData.m_AngleRange.min = math.radians(this.m_MinAngle);
      componentData.m_AngleRange.max = math.radians(this.m_MaxAngle);
      componentData.m_PositionRange.min = this.m_MinPosition;
      componentData.m_PositionRange.max = this.m_MaxPosition;
      entityManager.SetComponentData<RandomTransformData>(entity, componentData);
    }
  }
}
