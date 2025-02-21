// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CityEffects
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab)})]
  public class CityEffects : ComponentBase, IServiceUpgrade
  {
    public CityEffectInfo[] m_Effects;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CityModifierData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<CityEffectProvider>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CityEffectProvider>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.m_Effects == null)
        return;
      DynamicBuffer<CityModifierData> buffer = entityManager.GetBuffer<CityModifierData>(entity);
      for (int index = 0; index < this.m_Effects.Length; ++index)
      {
        CityEffectInfo effect = this.m_Effects[index];
        buffer.Add(new CityModifierData(effect.m_Type, effect.m_Mode, new Bounds1(0.0f, effect.m_Delta)));
      }
    }
  }
}
