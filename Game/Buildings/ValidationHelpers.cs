// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ValidationHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Buildings
{
  public static class ValidationHelpers
  {
    public static void ValidateBuilding(
      Entity entity,
      Building building,
      Transform transform,
      PrefabRef prefabRef,
      ValidationSystem.EntityData data,
      NativeArray<GroundWater> groundWaterMap,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      if (building.m_RoadEdge == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Prefabs.BuildingData buildingData = data.m_PrefabBuilding[prefabRef.m_Prefab];
        if ((buildingData.m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) != (Game.Prefabs.BuildingFlags) 0)
        {
          float3 frontPosition = BuildingUtils.CalculateFrontPosition(transform, buildingData.m_LotSize.y);
          errorQueue.Enqueue(new ErrorData()
          {
            m_ErrorSeverity = ErrorSeverity.Warning,
            m_ErrorType = ErrorType.NoRoadAccess,
            m_TempEntity = entity,
            m_Position = frontPosition
          });
        }
      }
      WaterPumpingStationData componentData;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if ((!data.m_WaterPumpingStationData.TryGetComponent(prefabRef.m_Prefab, out componentData) || (componentData.m_Types & AllowedWaterTypes.Groundwater) == AllowedWaterTypes.None) && !data.m_GroundWaterPoweredData.HasComponent(prefabRef.m_Prefab) || GroundWaterSystem.GetGroundWater(transform.m_Position, groundWaterMap).m_Max > (short) 500)
        return;
      errorQueue.Enqueue(new ErrorData()
      {
        m_ErrorSeverity = ErrorSeverity.Error,
        m_ErrorType = ErrorType.NoGroundWater,
        m_TempEntity = entity,
        m_Position = transform.m_Position
      });
    }

    public static void ValidateUpgrade(
      Entity entity,
      Owner owner,
      PrefabRef prefabRef,
      ValidationSystem.EntityData data,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (data.m_PrefabBuilding.HasComponent(prefabRef.m_Prefab) || !data.m_Upgrades.HasBuffer(owner.m_Owner))
        return;
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<InstalledUpgrade> upgrade1 = data.m_Upgrades[owner.m_Owner];
      for (int index = 0; index < upgrade1.Length; ++index)
      {
        Entity upgrade2 = upgrade1[index].m_Upgrade;
        // ISSUE: reference to a compiler-generated field
        if (upgrade2 != entity && data.m_PrefabRef[upgrade2].m_Prefab == prefabRef.m_Prefab)
        {
          // ISSUE: reference to a compiler-generated field
          errorQueue.Enqueue(new ErrorData()
          {
            m_ErrorSeverity = ErrorSeverity.Error,
            m_ErrorType = ErrorType.AlreadyUpgraded,
            m_TempEntity = entity,
            m_PermanentEntity = owner.m_Owner,
            m_Position = data.m_Transform[owner.m_Owner].m_Position
          });
        }
      }
    }
  }
}
