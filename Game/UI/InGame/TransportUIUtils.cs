// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TransportUIUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public static class TransportUIUtils
  {
    public static int CountLines(
      NativeArray<UITransportLineData> lines,
      TransportType type,
      bool cargo = false)
    {
      int num = 0;
      for (int index = 0; index < lines.Length; ++index)
      {
        UITransportLineData line = lines[index];
        if (line.type == type)
        {
          line = lines[index];
          if (line.isCargo == cargo)
            ++num;
        }
      }
      return num;
    }

    public static NativeArray<UITransportLineData> GetSortedLines(
      EntityQuery query,
      EntityManager entityManager,
      PrefabSystem prefabSystem)
    {
      NativeArray<Entity> entityArray = query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      int length = entityArray.Length;
      NativeArray<UITransportLineData> array = new NativeArray<UITransportLineData>(length, Allocator.Temp);
      for (int index = 0; index < length; ++index)
        array[index] = TransportUIUtils.BuildTransportLine(entityArray[index], entityManager, prefabSystem);
      array.Sort<UITransportLineData>();
      entityArray.Dispose();
      return array;
    }

    public static UITransportLineData BuildTransportLine(
      Entity entity,
      EntityManager entityManager,
      PrefabSystem m_PrefabSystem)
    {
      Route componentData1 = entityManager.GetComponentData<Route>(entity);
      PrefabRef componentData2 = entityManager.GetComponentData<PrefabRef>(entity);
      // ISSUE: reference to a compiler-generated method
      TransportLinePrefab prefab = m_PrefabSystem.GetPrefab<TransportLinePrefab>(componentData2.m_Prefab);
      TransportLineData componentData3 = entityManager.GetComponentData<TransportLineData>(componentData2.m_Prefab);
      bool visible = !entityManager.HasComponent<HiddenRoute>(entity);
      Color componentData4 = entityManager.GetComponentData<Color>(entity);
      int cargo = 0;
      int capacity = 0;
      int stopCount = TransportUIUtils.GetStopCount(entityManager, entity);
      int routeVehiclesCount = TransportUIUtils.GetRouteVehiclesCount(entityManager, entity, ref cargo, ref capacity);
      float routeLength = TransportUIUtils.GetRouteLength(entityManager, entity);
      float usage = capacity > 0 ? (float) cargo / (float) capacity : 0.0f;
      RouteSchedule schedule = RouteUtils.CheckOption(componentData1, RouteOption.Day) ? RouteSchedule.Day : (RouteUtils.CheckOption(componentData1, RouteOption.Night) ? RouteSchedule.Night : RouteSchedule.DayAndNight);
      bool active = !RouteUtils.CheckOption(componentData1, RouteOption.Inactive);
      return new UITransportLineData(entity, active, visible, prefab.m_CargoTransport, componentData4, schedule, componentData3.m_TransportType, routeLength, stopCount, routeVehiclesCount, cargo, usage);
    }

    public static int GetStopCount(EntityManager entityManager, Entity entity)
    {
      DynamicBuffer<RouteWaypoint> buffer = entityManager.GetBuffer<RouteWaypoint>(entity, true);
      int stopCount = 0;
      for (int index = 0; index < buffer.Length; ++index)
      {
        Connected component;
        if (entityManager.TryGetComponent<Connected>(buffer[index].m_Waypoint, out component) && entityManager.HasComponent<Game.Routes.TransportStop>(component.m_Connected) && !entityManager.HasComponent<TaxiStand>(component.m_Connected))
          ++stopCount;
      }
      return stopCount;
    }

    public static float GetRouteLength(EntityManager entityManager, Entity entity)
    {
      DynamicBuffer<RouteSegment> buffer = entityManager.GetBuffer<RouteSegment>(entity, true);
      float routeLength = 0.0f;
      for (int index = 0; index < buffer.Length; ++index)
      {
        PathInformation component;
        if (entityManager.TryGetComponent<PathInformation>(buffer[index].m_Segment, out component))
          routeLength += component.m_Distance;
      }
      return routeLength;
    }

    public static int GetRouteVehiclesCount(
      EntityManager entityManager,
      Entity entity,
      ref int cargo,
      ref int capacity)
    {
      DynamicBuffer<RouteVehicle> buffer = entityManager.GetBuffer<RouteVehicle>(entity, true);
      for (int index = 0; index < buffer.Length; ++index)
        TransportUIUtils.AddCargo(entityManager, buffer[index].m_Vehicle, ref cargo, ref capacity);
      return buffer.Length;
    }

    private static void AddCargo(
      EntityManager entityManager,
      Entity entity,
      ref int cargo,
      ref int capacity)
    {
      DynamicBuffer<LayoutElement> buffer;
      if (entityManager.TryGetBuffer<LayoutElement>(entity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
          TransportUIUtils.AddVehicleCargo(entityManager, buffer[index].m_Vehicle, ref cargo, ref capacity);
      }
      else
        TransportUIUtils.AddVehicleCargo(entityManager, entity, ref cargo, ref capacity);
    }

    private static void AddVehicleCargo(
      EntityManager entityManager,
      Entity entity,
      ref int cargo,
      ref int capacity)
    {
      PrefabRef component1;
      if (!entityManager.TryGetComponent<PrefabRef>(entity, out component1))
        return;
      PublicTransportVehicleData component2;
      if (entityManager.TryGetComponent<PublicTransportVehicleData>(component1.m_Prefab, out component2))
      {
        DynamicBuffer<Passenger> buffer;
        if (entityManager.TryGetBuffer<Passenger>(entity, true, out buffer))
          cargo += buffer.Length;
        capacity += component2.m_PassengerCapacity;
      }
      else
      {
        CargoTransportVehicleData component3;
        if (!entityManager.TryGetComponent<CargoTransportVehicleData>(component1.m_Prefab, out component3))
          return;
        DynamicBuffer<Game.Economy.Resources> buffer;
        if (entityManager.TryGetBuffer<Game.Economy.Resources>(entity, true, out buffer))
        {
          for (int index = 0; index < buffer.Length; ++index)
            cargo += buffer[index].m_Amount;
        }
        capacity += component3.m_CargoCapacity;
      }
    }
  }
}
