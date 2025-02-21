// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AirplanePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new Type[] {})]
  public class AirplanePrefab : AircraftPrefab
  {
    public float2 m_FlyingSpeed = new float2(200f, 1000f);
    public float m_FlyingAcceleration = 20f;
    public float m_FlyingBraking = 20f;
    public float m_FlyingTurning = 10f;
    public float m_FlyingAngularAcceleration = 20f;
    public float m_ClimbAngle = 20f;
    public float m_SlowPitchAngle = 20f;
    public float m_TurningRollFactor = 0.5f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AirplaneData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Airplane>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<AirplaneData>(entity, new AirplaneData()
      {
        m_FlyingSpeed = this.m_FlyingSpeed / 3.6f,
        m_FlyingAcceleration = this.m_FlyingAcceleration,
        m_FlyingBraking = this.m_FlyingBraking,
        m_FlyingTurning = math.radians(this.m_FlyingTurning),
        m_FlyingAngularAcceleration = math.radians(this.m_FlyingAngularAcceleration),
        m_ClimbAngle = math.radians(this.m_ClimbAngle),
        m_SlowPitchAngle = math.radians(this.m_SlowPitchAngle),
        m_TurningRollFactor = this.m_TurningRollFactor
      });
    }
  }
}
