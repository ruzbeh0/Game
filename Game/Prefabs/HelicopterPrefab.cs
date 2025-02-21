// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HelicopterPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.PSI;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ExcludeGeneratedModTag]
  [ComponentMenu("Vehicles/", new Type[] {})]
  public class HelicopterPrefab : AircraftPrefab
  {
    public float m_FlyingMaxSpeed = 250f;
    public float m_FlyingAcceleration = 10f;
    public float m_FlyingAngularAcceleration = 10f;
    public float m_AccelerationSwayFactor = 0.5f;
    public float m_VelocitySwayFactor = 0.7f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<HelicopterData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Helicopter>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      HelicopterData componentData = new HelicopterData()
      {
        m_HelicopterType = this.GetHelicopterType(),
        m_FlyingMaxSpeed = this.m_FlyingMaxSpeed / 3.6f,
        m_FlyingAcceleration = this.m_FlyingAcceleration,
        m_FlyingAngularAcceleration = math.radians(this.m_FlyingAngularAcceleration),
        m_AccelerationSwayFactor = this.m_AccelerationSwayFactor
      };
      componentData.m_VelocitySwayFactor = this.m_VelocitySwayFactor / componentData.m_FlyingMaxSpeed;
      entityManager.SetComponentData<HelicopterData>(entity, componentData);
    }

    protected virtual HelicopterType GetHelicopterType() => HelicopterType.Helicopter;

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        yield return this.GetHelicopterType().ToString();
      }
    }
  }
}
