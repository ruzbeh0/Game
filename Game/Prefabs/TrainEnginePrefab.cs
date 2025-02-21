// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrainEnginePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new System.Type[] {})]
  public class TrainEnginePrefab : TrainPrefab
  {
    public int m_MinEngineCount = 2;
    public int m_MaxEngineCount = 2;
    public int m_MinCarriagesPerEngine = 5;
    public int m_MaxCarriagesPerEngine = 5;
    public TrainCarPrefab m_Tender;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TrainEngineData>());
      components.Add(ComponentType.ReadWrite<VehicleCarriageElement>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.SetComponentData<TrainEngineData>(entity, new TrainEngineData(this.m_MinEngineCount, this.m_MaxEngineCount));
      DynamicBuffer<VehicleCarriageElement> buffer = entityManager.GetBuffer<VehicleCarriageElement>(entity);
      if ((UnityEngine.Object) this.m_Tender != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = entityManager.World.GetExistingSystemManaged<PrefabSystem>().GetEntity((PrefabBase) this.m_Tender);
        buffer.Add(new VehicleCarriageElement(entity1, 1, 1, VehicleCarriageDirection.Default));
      }
      buffer.Add(new VehicleCarriageElement(Entity.Null, this.m_MinCarriagesPerEngine, this.m_MaxCarriagesPerEngine, VehicleCarriageDirection.Random));
    }
  }
}
