// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportVehicleSelectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct TransportVehicleSelectData
  {
    private NativeList<ArchetypeChunk> m_PrefabChunks;
    private VehicleSelectRequirementData m_RequirementData;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<PublicTransportVehicleData> m_PublicTransportVehicleType;
    private ComponentTypeHandle<CargoTransportVehicleData> m_CargoTransportVehicleType;
    private ComponentTypeHandle<TrainData> m_TrainType;
    private ComponentTypeHandle<TrainEngineData> m_TrainEngineType;
    private ComponentTypeHandle<TrainCarriageData> m_TrainCarriageType;
    private ComponentTypeHandle<MultipleUnitTrainData> m_MultipleUnitTrainType;
    private ComponentTypeHandle<TaxiData> m_TaxiType;
    private ComponentTypeHandle<CarData> m_CarType;
    private ComponentTypeHandle<AirplaneData> m_AirplaneType;
    private ComponentTypeHandle<HelicopterData> m_HelicopterType;
    private ComponentTypeHandle<WatercraftData> m_WatercraftType;
    private ComponentLookup<ObjectData> m_ObjectData;
    private ComponentLookup<MovingObjectData> m_MovingObjectData;
    private ComponentLookup<TrainObjectData> m_TrainObjectData;
    private ComponentLookup<PublicTransportVehicleData> m_PublicTransportVehicleData;
    private ComponentLookup<CargoTransportVehicleData> m_CargoTransportVehicleData;
    private BufferLookup<VehicleCarriageElement> m_VehicleCarriages;

    public static EntityQueryDesc GetEntityQueryDesc()
    {
      return new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<VehicleData>(),
          ComponentType.ReadOnly<ObjectData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<PublicTransportVehicleData>(),
          ComponentType.ReadOnly<CargoTransportVehicleData>(),
          ComponentType.ReadOnly<TrainEngineData>(),
          ComponentType.ReadOnly<TaxiData>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Locked>()
        }
      };
    }

    public TransportVehicleSelectData(SystemBase system)
    {
      this.m_PrefabChunks = new NativeList<ArchetypeChunk>();
      this.m_RequirementData = new VehicleSelectRequirementData(system);
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_PublicTransportVehicleType = system.GetComponentTypeHandle<PublicTransportVehicleData>(true);
      this.m_CargoTransportVehicleType = system.GetComponentTypeHandle<CargoTransportVehicleData>(true);
      this.m_TrainType = system.GetComponentTypeHandle<TrainData>(true);
      this.m_TrainEngineType = system.GetComponentTypeHandle<TrainEngineData>(true);
      this.m_TrainCarriageType = system.GetComponentTypeHandle<TrainCarriageData>(true);
      this.m_MultipleUnitTrainType = system.GetComponentTypeHandle<MultipleUnitTrainData>(true);
      this.m_TaxiType = system.GetComponentTypeHandle<TaxiData>(true);
      this.m_CarType = system.GetComponentTypeHandle<CarData>(true);
      this.m_AirplaneType = system.GetComponentTypeHandle<AirplaneData>(true);
      this.m_HelicopterType = system.GetComponentTypeHandle<HelicopterData>(true);
      this.m_WatercraftType = system.GetComponentTypeHandle<WatercraftData>(true);
      this.m_ObjectData = system.GetComponentLookup<ObjectData>(true);
      this.m_MovingObjectData = system.GetComponentLookup<MovingObjectData>(true);
      this.m_TrainObjectData = system.GetComponentLookup<TrainObjectData>(true);
      this.m_PublicTransportVehicleData = system.GetComponentLookup<PublicTransportVehicleData>(true);
      this.m_CargoTransportVehicleData = system.GetComponentLookup<CargoTransportVehicleData>(true);
      this.m_VehicleCarriages = system.GetBufferLookup<VehicleCarriageElement>(true);
    }

    public void PreUpdate(
      SystemBase system,
      CityConfigurationSystem cityConfigurationSystem,
      EntityQuery query,
      Allocator allocator,
      out JobHandle jobHandle)
    {
      this.m_PrefabChunks = query.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) allocator, out jobHandle);
      this.m_RequirementData.Update(system, cityConfigurationSystem);
      this.m_EntityType.Update(system);
      this.m_PublicTransportVehicleType.Update(system);
      this.m_CargoTransportVehicleType.Update(system);
      this.m_TrainType.Update(system);
      this.m_TrainEngineType.Update(system);
      this.m_TrainCarriageType.Update(system);
      this.m_MultipleUnitTrainType.Update(system);
      this.m_TaxiType.Update(system);
      this.m_CarType.Update(system);
      this.m_AirplaneType.Update(system);
      this.m_HelicopterType.Update(system);
      this.m_WatercraftType.Update(system);
      this.m_ObjectData.Update(system);
      this.m_MovingObjectData.Update(system);
      this.m_TrainObjectData.Update(system);
      this.m_PublicTransportVehicleData.Update(system);
      this.m_CargoTransportVehicleData.Update(system);
      this.m_VehicleCarriages.Update(system);
    }

    public void PostUpdate(JobHandle jobHandle) => this.m_PrefabChunks.Dispose(jobHandle);

    public void ListVehicles(
      TransportType transportType,
      EnergyTypes energyTypes,
      PublicTransportPurpose publicTransportPurpose,
      Resource cargoResources,
      NativeList<Entity> primaryPrefabs,
      NativeList<Entity> secondaryPrefabs)
    {
      Random fromIndex = Random.CreateFromIndex(0U);
      int2 passengerCapacity = publicTransportPurpose != (PublicTransportPurpose) 0 ? new int2(1, int.MaxValue) : (int2) 0;
      int2 cargoCapacity = cargoResources != Resource.NoResource ? new int2(1, int.MaxValue) : (int2) 0;
      this.GetRandomVehicle(ref fromIndex, transportType, energyTypes, publicTransportPurpose, cargoResources, Entity.Null, Entity.Null, primaryPrefabs, secondaryPrefabs, false, out bool _, out int _, out Entity _, ref passengerCapacity, ref cargoCapacity);
    }

    public void SelectVehicle(
      ref Random random,
      TransportType transportType,
      EnergyTypes energyTypes,
      PublicTransportPurpose publicTransportPurpose,
      Resource cargoResources,
      out Entity primaryPrefab,
      out Entity secondaryPrefab,
      ref int2 passengerCapacity,
      ref int2 cargoCapacity)
    {
      primaryPrefab = this.GetRandomVehicle(ref random, transportType, energyTypes, publicTransportPurpose, cargoResources, Entity.Null, Entity.Null, new NativeList<Entity>(), new NativeList<Entity>(), false, out bool _, out int _, out secondaryPrefab, ref passengerCapacity, ref cargoCapacity);
    }

    public Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      Transform transform,
      Entity source,
      Entity primaryPrefab,
      Entity secondaryPrefab,
      TransportType transportType,
      EnergyTypes energyTypes,
      PublicTransportPurpose publicTransportPurpose,
      Resource cargoResources,
      ref int2 passengerCapacity,
      ref int2 cargoCapacity,
      bool parked)
    {
      NativeList<LayoutElement> layout = new NativeList<LayoutElement>();
      Entity vehicle = this.CreateVehicle(commandBuffer, jobIndex, ref random, transform, source, primaryPrefab, secondaryPrefab, transportType, energyTypes, publicTransportPurpose, cargoResources, ref passengerCapacity, ref cargoCapacity, parked, ref layout);
      if (!layout.IsCreated)
        return vehicle;
      layout.Dispose();
      return vehicle;
    }

    public Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      Transform transform,
      Entity source,
      Entity primaryPrefab,
      Entity secondaryPrefab,
      TransportType transportType,
      EnergyTypes energyTypes,
      PublicTransportPurpose publicTransportPurpose,
      Resource cargoResources,
      ref int2 passengerCapacity,
      ref int2 cargoCapacity,
      bool parked,
      ref NativeList<LayoutElement> layout)
    {
      bool isMultipleUnitTrain;
      int unitCount;
      Entity secondaryResult;
      primaryPrefab = this.GetRandomVehicle(ref random, transportType, energyTypes, publicTransportPurpose, cargoResources, primaryPrefab, secondaryPrefab, new NativeList<Entity>(), new NativeList<Entity>(), false, out isMultipleUnitTrain, out unitCount, out secondaryResult, ref passengerCapacity, ref cargoCapacity);
      secondaryPrefab = secondaryResult;
      if (primaryPrefab == Entity.Null)
        return Entity.Null;
      Entity vehicle = transportType == TransportType.Train || transportType == TransportType.Tram || transportType == TransportType.Subway ? commandBuffer.CreateEntity(jobIndex, this.GetArchetype(primaryPrefab, true, parked)) : commandBuffer.CreateEntity(jobIndex, this.GetArchetype(primaryPrefab, false, parked));
      commandBuffer.SetComponent<Transform>(jobIndex, vehicle, transform);
      commandBuffer.SetComponent<PrefabRef>(jobIndex, vehicle, new PrefabRef(primaryPrefab));
      commandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, vehicle, new PseudoRandomSeed(ref random));
      this.AddTransportComponents(commandBuffer, jobIndex, publicTransportPurpose, vehicle);
      if (!parked && source != Entity.Null)
      {
        commandBuffer.AddComponent<TripSource>(jobIndex, vehicle, new TripSource(source));
        commandBuffer.AddComponent<Unspawned>(jobIndex, vehicle, new Unspawned());
      }
      bool flag = false;
      if (transportType == TransportType.Train || transportType == TransportType.Tram || transportType == TransportType.Subway)
      {
        commandBuffer.SetComponent<Controller>(jobIndex, vehicle, new Controller(vehicle));
        flag = true;
        if (layout.IsCreated)
          layout.Clear();
        else
          layout = new NativeList<LayoutElement>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      }
      if (flag)
      {
        int num = 0;
        LayoutElement layoutElement;
        if (isMultipleUnitTrain)
        {
          ref NativeList<LayoutElement> local1 = ref layout;
          layoutElement = new LayoutElement(vehicle);
          ref LayoutElement local2 = ref layoutElement;
          local1.Add(in local2);
        }
        Entity entity1 = isMultipleUnitTrain ? primaryPrefab : secondaryPrefab;
        if (entity1 != Entity.Null)
        {
          EntityArchetype archetype1 = this.GetArchetype(entity1, false, parked);
          for (int index1 = 0; index1 < unitCount; ++index1)
          {
            if (!isMultipleUnitTrain || index1 != 0)
            {
              Game.Vehicles.TrainFlags flags = (Game.Vehicles.TrainFlags) 0;
              if (!isMultipleUnitTrain && index1 != 0 && random.NextBool())
                flags |= Game.Vehicles.TrainFlags.Reversed;
              Entity entity2 = commandBuffer.CreateEntity(jobIndex, archetype1);
              commandBuffer.SetComponent<Transform>(jobIndex, entity2, transform);
              commandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, new PrefabRef(entity1));
              commandBuffer.SetComponent<Controller>(jobIndex, entity2, new Controller(vehicle));
              commandBuffer.SetComponent<Train>(jobIndex, entity2, new Train(flags));
              commandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity2, new PseudoRandomSeed(ref random));
              this.AddTransportComponents(commandBuffer, jobIndex, publicTransportPurpose, entity2);
              if (!parked && source != Entity.Null)
              {
                commandBuffer.AddComponent<TripSource>(jobIndex, entity2, new TripSource(source));
                commandBuffer.AddComponent<Unspawned>(jobIndex, entity2, new Unspawned());
              }
              ref NativeList<LayoutElement> local3 = ref layout;
              layoutElement = new LayoutElement(entity2);
              ref LayoutElement local4 = ref layoutElement;
              local3.Add(in local4);
            }
            if (this.m_VehicleCarriages.HasBuffer(entity1))
            {
              DynamicBuffer<VehicleCarriageElement> vehicleCarriage = this.m_VehicleCarriages[entity1];
              for (int index2 = 0; index2 < vehicleCarriage.Length; ++index2)
              {
                VehicleCarriageElement vehicleCarriageElement = vehicleCarriage[index2];
                if (vehicleCarriageElement.m_Prefab == Entity.Null)
                {
                  num += vehicleCarriageElement.m_Count.x;
                }
                else
                {
                  EntityArchetype archetype2 = this.GetArchetype(vehicleCarriageElement.m_Prefab, false, parked);
                  for (int index3 = 0; index3 < vehicleCarriageElement.m_Count.x; ++index3)
                  {
                    Game.Vehicles.TrainFlags flags = (Game.Vehicles.TrainFlags) 0;
                    switch (vehicleCarriageElement.m_Direction)
                    {
                      case VehicleCarriageDirection.Reversed:
                        flags |= Game.Vehicles.TrainFlags.Reversed;
                        break;
                      case VehicleCarriageDirection.Random:
                        if (random.NextBool())
                        {
                          flags |= Game.Vehicles.TrainFlags.Reversed;
                          break;
                        }
                        break;
                    }
                    Entity entity3 = commandBuffer.CreateEntity(jobIndex, archetype2);
                    commandBuffer.SetComponent<Transform>(jobIndex, entity3, transform);
                    commandBuffer.SetComponent<PrefabRef>(jobIndex, entity3, new PrefabRef(vehicleCarriageElement.m_Prefab));
                    commandBuffer.SetComponent<Controller>(jobIndex, entity3, new Controller(vehicle));
                    commandBuffer.SetComponent<Train>(jobIndex, entity3, new Train(flags));
                    commandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity3, new PseudoRandomSeed(ref random));
                    this.AddTransportComponents(commandBuffer, jobIndex, publicTransportPurpose, entity3);
                    if (!parked && source != Entity.Null)
                    {
                      commandBuffer.AddComponent<TripSource>(jobIndex, entity3, new TripSource(source));
                      commandBuffer.AddComponent<Unspawned>(jobIndex, entity3, new Unspawned());
                    }
                    ref NativeList<LayoutElement> local5 = ref layout;
                    layoutElement = new LayoutElement(entity3);
                    ref LayoutElement local6 = ref layoutElement;
                    local5.Add(in local6);
                  }
                }
              }
            }
          }
        }
        if (!isMultipleUnitTrain)
        {
          ref NativeList<LayoutElement> local7 = ref layout;
          layoutElement = new LayoutElement(vehicle);
          ref LayoutElement local8 = ref layoutElement;
          local7.Add(in local8);
          --num;
        }
        if (num > 0)
        {
          EntityArchetype archetype = this.GetArchetype(primaryPrefab, false, parked);
          for (int index = 0; index < num; ++index)
          {
            Game.Vehicles.TrainFlags flags = (Game.Vehicles.TrainFlags) 0;
            if (random.NextBool())
              flags |= Game.Vehicles.TrainFlags.Reversed;
            Entity entity4 = commandBuffer.CreateEntity(jobIndex, archetype);
            commandBuffer.SetComponent<Transform>(jobIndex, entity4, transform);
            commandBuffer.SetComponent<PrefabRef>(jobIndex, entity4, new PrefabRef(primaryPrefab));
            commandBuffer.SetComponent<Controller>(jobIndex, entity4, new Controller(vehicle));
            commandBuffer.SetComponent<Train>(jobIndex, entity4, new Train(flags));
            commandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity4, new PseudoRandomSeed(ref random));
            this.AddTransportComponents(commandBuffer, jobIndex, publicTransportPurpose, entity4);
            if (!parked && source != Entity.Null)
            {
              commandBuffer.AddComponent<TripSource>(jobIndex, entity4, new TripSource(source));
              commandBuffer.AddComponent<Unspawned>(jobIndex, entity4, new Unspawned());
            }
            ref NativeList<LayoutElement> local9 = ref layout;
            layoutElement = new LayoutElement(entity4);
            ref LayoutElement local10 = ref layoutElement;
            local9.Add(in local10);
          }
        }
        commandBuffer.SetBuffer<LayoutElement>(jobIndex, vehicle).CopyFrom(layout.AsArray());
      }
      return vehicle;
    }

    private void AddTransportComponents(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      PublicTransportPurpose publicTransportPurpose,
      Entity entity)
    {
      if ((publicTransportPurpose & PublicTransportPurpose.TransportLine) != (PublicTransportPurpose) 0)
        commandBuffer.AddComponent<PassengerTransport>(jobIndex, entity, new PassengerTransport());
      if ((publicTransportPurpose & PublicTransportPurpose.Evacuation) != (PublicTransportPurpose) 0)
        commandBuffer.AddComponent<EvacuatingTransport>(jobIndex, entity, new EvacuatingTransport());
      if ((publicTransportPurpose & PublicTransportPurpose.PrisonerTransport) == (PublicTransportPurpose) 0)
        return;
      commandBuffer.AddComponent<PrisonerTransport>(jobIndex, entity, new PrisonerTransport());
    }

    private EntityArchetype GetArchetype(Entity prefab, bool controller, bool parked)
    {
      if (controller)
      {
        TrainObjectData trainObjectData = this.m_TrainObjectData[prefab];
        return !parked ? trainObjectData.m_ControllerArchetype : trainObjectData.m_StoppedControllerArchetype;
      }
      return parked ? this.m_MovingObjectData[prefab].m_StoppedArchetype : this.m_ObjectData[prefab].m_Archetype;
    }

    private Entity GetRandomVehicle(
      ref Random random,
      TransportType transportType,
      EnergyTypes energyTypes,
      PublicTransportPurpose publicTransportPurpose,
      Resource cargoResources,
      Entity primaryPrefab,
      Entity secondaryPrefab,
      NativeList<Entity> primaryPrefabs,
      NativeList<Entity> secondaryPrefabs,
      bool ignoreTheme,
      out bool isMultipleUnitTrain,
      out int unitCount,
      out Entity secondaryResult,
      ref int2 passengerCapacity,
      ref int2 cargoCapacity)
    {
      Entity randomVehicle = Entity.Null;
      secondaryResult = Entity.Null;
      int2 int2_1 = (int2) 0;
      int2 int2_2 = (int2) 0;
      isMultipleUnitTrain = false;
      unitCount = 1;
      int num1 = 1;
      TrackTypes trackTypes = TrackTypes.None;
      HelicopterType helicopterType = HelicopterType.Helicopter;
      switch (transportType)
      {
        case TransportType.Train:
          trackTypes = TrackTypes.Train;
          break;
        case TransportType.Tram:
          trackTypes = TrackTypes.Tram;
          break;
        case TransportType.Helicopter:
          helicopterType = HelicopterType.Helicopter;
          break;
        case TransportType.Subway:
          trackTypes = TrackTypes.Subway;
          break;
        case TransportType.Rocket:
          helicopterType = HelicopterType.Rocket;
          break;
      }
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        switch (transportType)
        {
          case TransportType.Bus:
            NativeArray<CarData> nativeArray1 = prefabChunk.GetNativeArray<CarData>(ref this.m_CarType);
            if (nativeArray1.Length != 0)
            {
              NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
              NativeArray<PublicTransportVehicleData> nativeArray3 = prefabChunk.GetNativeArray<PublicTransportVehicleData>(ref this.m_PublicTransportVehicleType);
              if (nativeArray3.Length != 0)
              {
                VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
                for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
                {
                  if ((nativeArray3[index2].m_PurposeMask & publicTransportPurpose) != (PublicTransportPurpose) 0)
                  {
                    CarData carData = nativeArray1[index2];
                    if (carData.m_EnergyType == EnergyTypes.None || (carData.m_EnergyType & energyTypes) != EnergyTypes.None)
                    {
                      Entity entity = nativeArray2[index2];
                      if (this.m_RequirementData.CheckRequirements(ref chunk, index2, ignoreTheme || entity == primaryPrefab))
                      {
                        int priority = math.select(0, 2, entity == primaryPrefab) + math.select(0, 1, (carData.m_EnergyType & EnergyTypes.Fuel) != 0);
                        if (primaryPrefabs.IsCreated)
                          primaryPrefabs.Add(in entity);
                        if (this.PickVehicle(ref random, 100, priority, ref int2_1.x, ref int2_2.x))
                        {
                          randomVehicle = entity;
                          passengerCapacity = (int2) nativeArray3[index2].m_PassengerCapacity;
                        }
                      }
                    }
                  }
                }
                break;
              }
              break;
            }
            break;
          case TransportType.Train:
          case TransportType.Tram:
          case TransportType.Subway:
            NativeArray<TrainData> nativeArray4 = prefabChunk.GetNativeArray<TrainData>(ref this.m_TrainType);
            if (nativeArray4.Length != 0)
            {
              NativeArray<Entity> nativeArray5 = prefabChunk.GetNativeArray(this.m_EntityType);
              NativeArray<TrainEngineData> nativeArray6 = prefabChunk.GetNativeArray<TrainEngineData>(ref this.m_TrainEngineType);
              NativeArray<PublicTransportVehicleData> nativeArray7 = prefabChunk.GetNativeArray<PublicTransportVehicleData>(ref this.m_PublicTransportVehicleType);
              NativeArray<CargoTransportVehicleData> nativeArray8 = prefabChunk.GetNativeArray<CargoTransportVehicleData>(ref this.m_CargoTransportVehicleType);
              bool flag1 = nativeArray6.Length != 0;
              bool flag2 = prefabChunk.Has<TrainCarriageData>(ref this.m_TrainCarriageType);
              bool flag3 = prefabChunk.Has<MultipleUnitTrainData>(ref this.m_MultipleUnitTrainType);
              VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
              if (flag1 & flag3 || flag2 && !flag3)
              {
                if (publicTransportPurpose != 0 == (nativeArray7.Length != 0) && cargoResources > Resource.NoResource == (nativeArray8.Length != 0))
                {
                  for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
                  {
                    if ((publicTransportPurpose == (PublicTransportPurpose) 0 || (nativeArray7[index3].m_PurposeMask & publicTransportPurpose) != (PublicTransportPurpose) 0) && (cargoResources == Resource.NoResource || (nativeArray8[index3].m_Resources & cargoResources) != Resource.NoResource))
                    {
                      TrainData trainData = nativeArray4[index3];
                      if ((trainData.m_EnergyType == EnergyTypes.None || (trainData.m_EnergyType & energyTypes) != EnergyTypes.None) && trainData.m_TrackType == trackTypes)
                      {
                        Entity entity = nativeArray5[index3];
                        if (this.m_RequirementData.CheckRequirements(ref chunk, index3, ignoreTheme || entity == primaryPrefab))
                        {
                          int priority = math.select(0, 2, entity == primaryPrefab) + math.select(0, 1, (trainData.m_EnergyType & EnergyTypes.Fuel) != 0);
                          if (primaryPrefabs.IsCreated)
                            primaryPrefabs.Add(in entity);
                          if (this.PickVehicle(ref random, 100, priority, ref int2_1.x, ref int2_2.x))
                          {
                            isMultipleUnitTrain = flag3;
                            if (flag1)
                              unitCount = nativeArray6[index3].m_Count.x;
                            randomVehicle = entity;
                            if (publicTransportPurpose != (PublicTransportPurpose) 0)
                              passengerCapacity = (int2) nativeArray7[index3].m_PassengerCapacity;
                            if (cargoResources != Resource.NoResource)
                              cargoCapacity = (int2) nativeArray8[index3].m_CargoCapacity;
                          }
                        }
                      }
                    }
                  }
                  break;
                }
                break;
              }
              if (flag1 && !flag3)
              {
                for (int index4 = 0; index4 < nativeArray4.Length; ++index4)
                {
                  TrainData trainData = nativeArray4[index4];
                  if ((trainData.m_EnergyType == EnergyTypes.None || (trainData.m_EnergyType & energyTypes) != EnergyTypes.None) && trainData.m_TrackType == trackTypes)
                  {
                    Entity entity = nativeArray5[index4];
                    if (this.m_RequirementData.CheckRequirements(ref chunk, index4, ignoreTheme || entity == secondaryPrefab))
                    {
                      int priority = math.select(0, 2, entity == secondaryPrefab) + math.select(0, 1, (trainData.m_EnergyType & EnergyTypes.Fuel) != 0);
                      if (secondaryPrefabs.IsCreated)
                        secondaryPrefabs.Add(in entity);
                      if (this.PickVehicle(ref random, 100, priority, ref int2_1.y, ref int2_2.y))
                      {
                        num1 = nativeArray6[index4].m_Count.x;
                        secondaryResult = entity;
                      }
                    }
                  }
                }
                break;
              }
              break;
            }
            break;
          case TransportType.Taxi:
            NativeArray<CarData> nativeArray9 = prefabChunk.GetNativeArray<CarData>(ref this.m_CarType);
            if (nativeArray9.Length != 0)
            {
              NativeArray<Entity> nativeArray10 = prefabChunk.GetNativeArray(this.m_EntityType);
              NativeArray<TaxiData> nativeArray11 = prefabChunk.GetNativeArray<TaxiData>(ref this.m_TaxiType);
              if (nativeArray11.Length != 0)
              {
                VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
                for (int index5 = 0; index5 < nativeArray9.Length; ++index5)
                {
                  CarData carData = nativeArray9[index5];
                  if (carData.m_EnergyType == EnergyTypes.None || (carData.m_EnergyType & energyTypes) != EnergyTypes.None)
                  {
                    Entity entity = nativeArray10[index5];
                    if (this.m_RequirementData.CheckRequirements(ref chunk, index5, ignoreTheme || entity == primaryPrefab))
                    {
                      int priority = math.select(0, 2, entity == primaryPrefab) + math.select(0, 1, (carData.m_EnergyType & EnergyTypes.Electricity) != 0);
                      if (primaryPrefabs.IsCreated)
                        primaryPrefabs.Add(in entity);
                      if (this.PickVehicle(ref random, 100, priority, ref int2_1.x, ref int2_2.x))
                      {
                        randomVehicle = entity;
                        passengerCapacity = (int2) nativeArray11[index5].m_PassengerCapacity;
                      }
                    }
                  }
                }
                break;
              }
              break;
            }
            break;
          case TransportType.Ship:
            NativeArray<WatercraftData> nativeArray12 = prefabChunk.GetNativeArray<WatercraftData>(ref this.m_WatercraftType);
            if (nativeArray12.Length != 0)
            {
              NativeArray<Entity> nativeArray13 = prefabChunk.GetNativeArray(this.m_EntityType);
              NativeArray<PublicTransportVehicleData> nativeArray14 = prefabChunk.GetNativeArray<PublicTransportVehicleData>(ref this.m_PublicTransportVehicleType);
              NativeArray<CargoTransportVehicleData> nativeArray15 = prefabChunk.GetNativeArray<CargoTransportVehicleData>(ref this.m_CargoTransportVehicleType);
              if (publicTransportPurpose != 0 == (nativeArray14.Length != 0) && cargoResources > Resource.NoResource == (nativeArray15.Length != 0))
              {
                VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
                for (int index6 = 0; index6 < nativeArray12.Length; ++index6)
                {
                  WatercraftData watercraftData = nativeArray12[index6];
                  if ((watercraftData.m_EnergyType == EnergyTypes.None || (watercraftData.m_EnergyType & energyTypes) != EnergyTypes.None) && (publicTransportPurpose == (PublicTransportPurpose) 0 || (nativeArray14[index6].m_PurposeMask & publicTransportPurpose) != (PublicTransportPurpose) 0) && (cargoResources == Resource.NoResource || (nativeArray15[index6].m_Resources & cargoResources) != Resource.NoResource))
                  {
                    Entity entity = nativeArray13[index6];
                    if (this.m_RequirementData.CheckRequirements(ref chunk, index6, ignoreTheme || entity == primaryPrefab))
                    {
                      int priority = math.select(0, 2, entity == primaryPrefab) + math.select(0, 1, (watercraftData.m_EnergyType & EnergyTypes.Fuel) != 0);
                      if (primaryPrefabs.IsCreated)
                        primaryPrefabs.Add(in entity);
                      if (this.PickVehicle(ref random, 100, priority, ref int2_1.x, ref int2_2.x))
                      {
                        randomVehicle = entity;
                        if (publicTransportPurpose != (PublicTransportPurpose) 0)
                          passengerCapacity = (int2) nativeArray14[index6].m_PassengerCapacity;
                        if (cargoResources != Resource.NoResource)
                          cargoCapacity = (int2) nativeArray15[index6].m_CargoCapacity;
                      }
                    }
                  }
                }
                break;
              }
              break;
            }
            break;
          case TransportType.Helicopter:
          case TransportType.Rocket:
            NativeArray<HelicopterData> nativeArray16 = prefabChunk.GetNativeArray<HelicopterData>(ref this.m_HelicopterType);
            if (nativeArray16.Length != 0)
            {
              NativeArray<Entity> nativeArray17 = prefabChunk.GetNativeArray(this.m_EntityType);
              NativeArray<PublicTransportVehicleData> nativeArray18 = prefabChunk.GetNativeArray<PublicTransportVehicleData>(ref this.m_PublicTransportVehicleType);
              NativeArray<CargoTransportVehicleData> nativeArray19 = prefabChunk.GetNativeArray<CargoTransportVehicleData>(ref this.m_CargoTransportVehicleType);
              if (publicTransportPurpose != 0 == (nativeArray18.Length != 0) && cargoResources > Resource.NoResource == (nativeArray19.Length != 0))
              {
                VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
                for (int index7 = 0; index7 < nativeArray17.Length; ++index7)
                {
                  if ((publicTransportPurpose == (PublicTransportPurpose) 0 || (nativeArray18[index7].m_PurposeMask & publicTransportPurpose) != (PublicTransportPurpose) 0) && (cargoResources == Resource.NoResource || (nativeArray19[index7].m_Resources & cargoResources) != Resource.NoResource) && nativeArray16[index7].m_HelicopterType == helicopterType)
                  {
                    Entity entity = nativeArray17[index7];
                    if (this.m_RequirementData.CheckRequirements(ref chunk, index7, ignoreTheme || entity == primaryPrefab))
                    {
                      int priority = math.select(0, 2, entity == primaryPrefab);
                      if (primaryPrefabs.IsCreated)
                        primaryPrefabs.Add(in entity);
                      if (this.PickVehicle(ref random, 100, priority, ref int2_1.x, ref int2_2.x))
                      {
                        randomVehicle = entity;
                        if (publicTransportPurpose != (PublicTransportPurpose) 0)
                          passengerCapacity = (int2) nativeArray18[index7].m_PassengerCapacity;
                        if (cargoResources != Resource.NoResource)
                          cargoCapacity = (int2) nativeArray19[index7].m_CargoCapacity;
                      }
                    }
                  }
                }
                break;
              }
              break;
            }
            break;
          case TransportType.Airplane:
            if (prefabChunk.Has<AirplaneData>(ref this.m_AirplaneType))
            {
              NativeArray<Entity> nativeArray20 = prefabChunk.GetNativeArray(this.m_EntityType);
              NativeArray<PublicTransportVehicleData> nativeArray21 = prefabChunk.GetNativeArray<PublicTransportVehicleData>(ref this.m_PublicTransportVehicleType);
              NativeArray<CargoTransportVehicleData> nativeArray22 = prefabChunk.GetNativeArray<CargoTransportVehicleData>(ref this.m_CargoTransportVehicleType);
              if (publicTransportPurpose != 0 == (nativeArray21.Length != 0) && cargoResources > Resource.NoResource == (nativeArray22.Length != 0))
              {
                VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
                for (int index8 = 0; index8 < nativeArray20.Length; ++index8)
                {
                  if ((publicTransportPurpose == (PublicTransportPurpose) 0 || (nativeArray21[index8].m_PurposeMask & publicTransportPurpose) != (PublicTransportPurpose) 0) && (cargoResources == Resource.NoResource || (nativeArray22[index8].m_Resources & cargoResources) != Resource.NoResource))
                  {
                    Entity entity = nativeArray20[index8];
                    if (this.m_RequirementData.CheckRequirements(ref chunk, index8, ignoreTheme || entity == primaryPrefab))
                    {
                      int priority = math.select(0, 2, entity == primaryPrefab);
                      if (primaryPrefabs.IsCreated)
                        primaryPrefabs.Add(in entity);
                      if (this.PickVehicle(ref random, 100, priority, ref int2_1.x, ref int2_2.x))
                      {
                        randomVehicle = entity;
                        if (publicTransportPurpose != (PublicTransportPurpose) 0)
                          passengerCapacity = (int2) nativeArray21[index8].m_PassengerCapacity;
                        if (cargoResources != Resource.NoResource)
                          cargoCapacity = (int2) nativeArray22[index8].m_CargoCapacity;
                      }
                    }
                  }
                }
                break;
              }
              break;
            }
            break;
        }
      }
      if (isMultipleUnitTrain)
        secondaryResult = Entity.Null;
      else
        unitCount = num1;
      bool flag = false;
      if (transportType == TransportType.Train || transportType == TransportType.Tram || transportType == TransportType.Subway)
        flag = true;
      if (flag)
      {
        passengerCapacity.y = 0;
        cargoCapacity.y = 0;
        int num2 = 0;
        if (isMultipleUnitTrain)
        {
          passengerCapacity.y += passengerCapacity.x;
          cargoCapacity.y += cargoCapacity.x;
        }
        Entity entity = isMultipleUnitTrain ? randomVehicle : secondaryResult;
        if (entity != Entity.Null)
        {
          for (int index9 = 0; index9 < unitCount; ++index9)
          {
            if (isMultipleUnitTrain && index9 != 0)
            {
              passengerCapacity.y += passengerCapacity.x;
              cargoCapacity.y += cargoCapacity.x;
            }
            if (this.m_VehicleCarriages.HasBuffer(entity))
            {
              DynamicBuffer<VehicleCarriageElement> vehicleCarriage = this.m_VehicleCarriages[entity];
              for (int index10 = 0; index10 < vehicleCarriage.Length; ++index10)
              {
                VehicleCarriageElement vehicleCarriageElement = vehicleCarriage[index10];
                if (vehicleCarriageElement.m_Prefab == Entity.Null)
                {
                  num2 += vehicleCarriageElement.m_Count.x;
                }
                else
                {
                  PublicTransportVehicleData componentData1;
                  if (publicTransportPurpose != (PublicTransportPurpose) 0 && this.m_PublicTransportVehicleData.TryGetComponent(vehicleCarriageElement.m_Prefab, out componentData1))
                    passengerCapacity.y += componentData1.m_PassengerCapacity * vehicleCarriageElement.m_Count.x;
                  CargoTransportVehicleData componentData2;
                  if (cargoResources != Resource.NoResource && this.m_CargoTransportVehicleData.TryGetComponent(vehicleCarriageElement.m_Prefab, out componentData2))
                    cargoCapacity.y += componentData2.m_CargoCapacity * vehicleCarriageElement.m_Count.x;
                }
              }
            }
          }
        }
        if (!isMultipleUnitTrain)
        {
          passengerCapacity.y += passengerCapacity.x;
          cargoCapacity.y += cargoCapacity.x;
          --num2;
        }
        if (num2 > 0)
        {
          passengerCapacity.y += passengerCapacity.x * num2;
          cargoCapacity.y += cargoCapacity.x * num2;
        }
        passengerCapacity.x = passengerCapacity.y;
        cargoCapacity.x = cargoCapacity.y;
      }
      return randomVehicle;
    }

    private bool PickVehicle(
      ref Random random,
      int probability,
      int priority,
      ref int totalProbability,
      ref int selectedPriority)
    {
      if (priority < selectedPriority)
        return false;
      if (priority > selectedPriority)
      {
        totalProbability = 0;
        selectedPriority = priority;
      }
      totalProbability += probability;
      return random.NextInt(totalProbability) < probability;
    }
  }
}
