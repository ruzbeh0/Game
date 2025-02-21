// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MultipleUnitTrainFrontPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Vehicles/", new Type[] {})]
  public class MultipleUnitTrainFrontPrefab : TrainPrefab
  {
    public int m_MinMultipleUnitCount = 1;
    public int m_MaxMultipleUnitCount = 1;
    public MultipleUnitTrainCarriageInfo[] m_Carriages;
    public bool m_AddReversedEndCarriage = true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Carriages == null)
        return;
      for (int index = 0; index < this.m_Carriages.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Carriages[index].m_Carriage);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TrainEngineData>());
      components.Add(ComponentType.ReadWrite<MultipleUnitTrainData>());
      components.Add(ComponentType.ReadWrite<VehicleCarriageElement>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.SetComponentData<TrainEngineData>(entity, new TrainEngineData(this.m_MinMultipleUnitCount, this.m_MaxMultipleUnitCount));
      DynamicBuffer<VehicleCarriageElement> buffer = entityManager.GetBuffer<VehicleCarriageElement>(entity);
      if (this.m_Carriages != null)
      {
        // ISSUE: variable of a compiler-generated type
        PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
        for (int index = 0; index < this.m_Carriages.Length; ++index)
        {
          MultipleUnitTrainCarriageInfo carriage = this.m_Carriages[index];
          // ISSUE: reference to a compiler-generated method
          Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) carriage.m_Carriage);
          buffer.Add(new VehicleCarriageElement(entity1, carriage.m_MinCount, carriage.m_MaxCount, carriage.m_Direction));
        }
      }
      if (!this.m_AddReversedEndCarriage)
        return;
      buffer.Add(new VehicleCarriageElement(entity, 1, 1, VehicleCarriageDirection.Reversed));
    }
  }
}
