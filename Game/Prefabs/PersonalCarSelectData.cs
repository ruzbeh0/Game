// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PersonalCarSelectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Objects;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct PersonalCarSelectData
  {
    private NativeList<ArchetypeChunk> m_PrefabChunks;
    private VehicleSelectRequirementData m_RequirementData;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<Game.Prefabs.CarData> m_CarDataType;
    private ComponentTypeHandle<PersonalCarData> m_PersonalCarDataType;
    private ComponentTypeHandle<CarTrailerData> m_CarTrailerDataType;
    private ComponentTypeHandle<CarTractorData> m_CarTractorDataType;
    private ComponentTypeHandle<ObjectData> m_ObjectDataType;
    private ComponentTypeHandle<MovingObjectData> m_MovingObjectDataType;

    public static EntityQueryDesc GetEntityQueryDesc()
    {
      return new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<PersonalCarData>(),
          ComponentType.ReadOnly<Game.Prefabs.CarData>(),
          ComponentType.ReadOnly<MovingObjectData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Locked>()
        }
      };
    }

    public PersonalCarSelectData(SystemBase system)
    {
      this.m_PrefabChunks = new NativeList<ArchetypeChunk>();
      this.m_RequirementData = new VehicleSelectRequirementData(system);
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_CarDataType = system.GetComponentTypeHandle<Game.Prefabs.CarData>(true);
      this.m_PersonalCarDataType = system.GetComponentTypeHandle<PersonalCarData>(true);
      this.m_CarTrailerDataType = system.GetComponentTypeHandle<CarTrailerData>(true);
      this.m_CarTractorDataType = system.GetComponentTypeHandle<CarTractorData>(true);
      this.m_ObjectDataType = system.GetComponentTypeHandle<ObjectData>(true);
      this.m_MovingObjectDataType = system.GetComponentTypeHandle<MovingObjectData>(true);
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
      this.m_CarDataType.Update(system);
      this.m_PersonalCarDataType.Update(system);
      this.m_CarTrailerDataType.Update(system);
      this.m_CarTractorDataType.Update(system);
      this.m_ObjectDataType.Update(system);
      this.m_MovingObjectDataType.Update(system);
    }

    public void PostUpdate(JobHandle jobHandle) => this.m_PrefabChunks.Dispose(jobHandle);

    public Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      int passengerAmount,
      int baggageAmount,
      bool avoidTrailers,
      bool noSlowVehicles,
      Transform transform,
      Entity source,
      Entity keeper,
      PersonalCarFlags state,
      bool stopped,
      uint delay = 0)
    {
      return this.CreateVehicle(commandBuffer, jobIndex, ref random, passengerAmount, baggageAmount, avoidTrailers, noSlowVehicles, transform, source, keeper, state, stopped, delay, out Entity _, out Entity _, out Entity _);
    }

    public Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      int passengerAmount,
      int baggageAmount,
      bool avoidTrailers,
      bool noSlowVehicles,
      Transform transform,
      Entity source,
      Entity keeper,
      PersonalCarFlags state,
      bool stopped,
      uint delay,
      out Entity trailer,
      out Entity vehiclePrefab,
      out Entity trailerPrefab)
    {
      trailer = Entity.Null;
      vehiclePrefab = Entity.Null;
      trailerPrefab = Entity.Null;
      PersonalCarSelectData.CarData bestFirst;
      PersonalCarSelectData.CarData bestSecond;
      if (!this.GetVehicleData(ref random, passengerAmount, baggageAmount, avoidTrailers, noSlowVehicles, out bestFirst, out bestSecond))
        return Entity.Null;
      Entity vehicle = this.CreateVehicle(commandBuffer, jobIndex, ref random, bestFirst, transform, source, keeper, state, stopped, delay);
      vehiclePrefab = bestFirst.m_Entity;
      if (bestSecond.m_Entity != Entity.Null)
      {
        DynamicBuffer<LayoutElement> dynamicBuffer = commandBuffer.AddBuffer<LayoutElement>(jobIndex, vehicle);
        dynamicBuffer.Add(new LayoutElement(vehicle));
        trailer = this.CreateVehicle(commandBuffer, jobIndex, ref random, bestSecond, transform, source, Entity.Null, (PersonalCarFlags) 0, stopped, delay);
        trailerPrefab = bestSecond.m_Entity;
        commandBuffer.SetComponent<Controller>(jobIndex, trailer, new Controller(vehicle));
        dynamicBuffer.Add(new LayoutElement(trailer));
      }
      return vehicle;
    }

    public Entity CreateVehicle(
      EntityCommandBuffer commandBuffer,
      ref Random random,
      int passengerAmount,
      int baggageAmount,
      bool avoidTrailers,
      bool noSlowVehicles,
      Transform transform,
      Entity source,
      Entity keeper,
      PersonalCarFlags state,
      bool stopped,
      uint delay = 0)
    {
      PersonalCarSelectData.CarData bestFirst;
      PersonalCarSelectData.CarData bestSecond;
      if (!this.GetVehicleData(ref random, passengerAmount, baggageAmount, avoidTrailers, noSlowVehicles, out bestFirst, out bestSecond))
        return Entity.Null;
      Entity vehicle1 = this.CreateVehicle(commandBuffer, ref random, bestFirst, transform, source, keeper, state, stopped, delay);
      if (bestSecond.m_Entity != Entity.Null)
      {
        DynamicBuffer<LayoutElement> dynamicBuffer = commandBuffer.AddBuffer<LayoutElement>(vehicle1);
        dynamicBuffer.Add(new LayoutElement(vehicle1));
        Entity vehicle2 = this.CreateVehicle(commandBuffer, ref random, bestSecond, transform, source, Entity.Null, (PersonalCarFlags) 0, stopped, delay);
        commandBuffer.SetComponent<Controller>(vehicle2, new Controller(vehicle1));
        dynamicBuffer.Add(new LayoutElement(vehicle2));
      }
      return vehicle1;
    }

    public Entity CreateTrailer(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      int passengerAmount,
      int baggageAmount,
      bool noSlowVehicles,
      Entity tractorPrefab,
      Transform tractorTransform,
      PersonalCarFlags state,
      bool stopped,
      uint delay = 0)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<Entity> nativeArray1 = prefabChunk.GetNativeArray(this.m_EntityType);
        NativeArray<PersonalCarData> nativeArray2 = prefabChunk.GetNativeArray<PersonalCarData>(ref this.m_PersonalCarDataType);
        NativeArray<CarTractorData> nativeArray3 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
        for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
        {
          if (!(nativeArray1[index2] != tractorPrefab) && this.m_RequirementData.CheckRequirements(ref chunk, index2))
          {
            PersonalCarSelectData.CarData firstData = new PersonalCarSelectData.CarData();
            firstData.m_Entity = tractorPrefab;
            firstData.m_PersonalCarData = nativeArray2[index2];
            firstData.m_TractorData = nativeArray3[index2];
            PersonalCarSelectData.CarData bestFirst = new PersonalCarSelectData.CarData();
            PersonalCarSelectData.CarData bestSecond = new PersonalCarSelectData.CarData();
            int probability;
            int offset;
            this.CalculateProbability(passengerAmount, baggageAmount, firstData, new PersonalCarSelectData.CarData(), out probability, out offset);
            this.CheckTrailers(passengerAmount, baggageAmount, 0, firstData, false, noSlowVehicles, ref random, ref bestFirst, ref bestSecond, ref probability, ref offset);
            if (bestSecond.m_Entity == Entity.Null)
              return Entity.Null;
            Transform transform = tractorTransform;
            transform.m_Position += math.rotate(tractorTransform.m_Rotation, firstData.m_TractorData.m_AttachPosition);
            transform.m_Position -= math.rotate(transform.m_Rotation, bestSecond.m_TrailerData.m_AttachPosition);
            return this.CreateVehicle(commandBuffer, jobIndex, ref random, bestSecond, transform, Entity.Null, Entity.Null, (PersonalCarFlags) 0, stopped, delay);
          }
        }
      }
      return Entity.Null;
    }

    private bool GetVehicleData(
      ref Random random,
      int passengerAmount,
      int baggageAmount,
      bool avoidTrailers,
      bool noSlowVehicles,
      out PersonalCarSelectData.CarData bestFirst,
      out PersonalCarSelectData.CarData bestSecond)
    {
      bestFirst = new PersonalCarSelectData.CarData();
      bestSecond = new PersonalCarSelectData.CarData();
      int totalProbability = 0;
      int bestOffset = -11 - (passengerAmount + baggageAmount);
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<Entity> nativeArray1 = prefabChunk.GetNativeArray(this.m_EntityType);
        NativeArray<Game.Prefabs.CarData> nativeArray2 = prefabChunk.GetNativeArray<Game.Prefabs.CarData>(ref this.m_CarDataType);
        NativeArray<PersonalCarData> nativeArray3 = prefabChunk.GetNativeArray<PersonalCarData>(ref this.m_PersonalCarDataType);
        NativeArray<CarTrailerData> nativeArray4 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        NativeArray<CarTractorData> nativeArray5 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        NativeArray<ObjectData> nativeArray6 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
        NativeArray<MovingObjectData> nativeArray7 = prefabChunk.GetNativeArray<MovingObjectData>(ref this.m_MovingObjectDataType);
        VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
        for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
        {
          Game.Prefabs.CarData carData1 = nativeArray2[index2];
          if ((!noSlowVehicles || (double) carData1.m_MaxSpeed >= 22.222223281860352) && this.m_RequirementData.CheckRequirements(ref chunk, index2))
          {
            PersonalCarSelectData.CarData carData2 = new PersonalCarSelectData.CarData();
            carData2.m_PersonalCarData = nativeArray3[index2];
            if (carData2.m_PersonalCarData.m_PassengerCapacity != 0 || carData2.m_PersonalCarData.m_BaggageCapacity != 0)
            {
              carData2.m_Entity = nativeArray1[index2];
              carData2.m_ObjectData = nativeArray6[index2];
              carData2.m_MovingObjectData = nativeArray7[index2];
              bool flag = false;
              if (nativeArray4.Length != 0)
              {
                carData2.m_TrailerData = nativeArray4[index2];
                flag = true;
              }
              if (nativeArray5.Length != 0)
              {
                carData2.m_TractorData = nativeArray5[index2];
                if (carData2.m_TractorData.m_FixedTrailer != Entity.Null)
                {
                  if (!flag)
                  {
                    int extraOffset = math.select(0, -1, avoidTrailers);
                    this.CheckTrailers(passengerAmount, baggageAmount, extraOffset, carData2, true, noSlowVehicles, ref random, ref bestFirst, ref bestSecond, ref totalProbability, ref bestOffset);
                    continue;
                  }
                  continue;
                }
              }
              if (flag)
              {
                int extraOffset = math.select(0, -1, avoidTrailers);
                this.CheckTractors(passengerAmount, baggageAmount, extraOffset, carData2, noSlowVehicles, ref random, ref bestFirst, ref bestSecond, ref totalProbability, ref bestOffset);
              }
              else
              {
                int probability;
                int offset;
                this.CalculateProbability(passengerAmount, baggageAmount, carData2, new PersonalCarSelectData.CarData(), out probability, out offset);
                if (this.PickVehicle(ref random, probability, offset, ref totalProbability, ref bestOffset))
                {
                  bestFirst = carData2;
                  bestSecond = new PersonalCarSelectData.CarData();
                }
              }
            }
          }
        }
      }
      return bestFirst.m_Entity != Entity.Null;
    }

    private Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      PersonalCarSelectData.CarData data,
      Transform transform,
      Entity source,
      Entity keeper,
      PersonalCarFlags state,
      bool stopped,
      uint delay)
    {
      Entity e = !stopped ? commandBuffer.CreateEntity(jobIndex, data.m_ObjectData.m_Archetype) : commandBuffer.CreateEntity(jobIndex, data.m_MovingObjectData.m_StoppedArchetype);
      commandBuffer.SetComponent<Transform>(jobIndex, e, transform);
      commandBuffer.SetComponent<Game.Vehicles.PersonalCar>(jobIndex, e, new Game.Vehicles.PersonalCar(keeper, state));
      commandBuffer.SetComponent<PrefabRef>(jobIndex, e, new PrefabRef(data.m_Entity));
      commandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, e, new PseudoRandomSeed(ref random));
      if (source != Entity.Null)
      {
        commandBuffer.AddComponent<TripSource>(jobIndex, e, new TripSource(source, delay));
        commandBuffer.AddComponent<Unspawned>(jobIndex, e, new Unspawned());
      }
      return e;
    }

    private Entity CreateVehicle(
      EntityCommandBuffer commandBuffer,
      ref Random random,
      PersonalCarSelectData.CarData data,
      Transform transform,
      Entity source,
      Entity keeper,
      PersonalCarFlags state,
      bool stopped,
      uint delay)
    {
      Entity e = !stopped ? commandBuffer.CreateEntity(data.m_ObjectData.m_Archetype) : commandBuffer.CreateEntity(data.m_MovingObjectData.m_StoppedArchetype);
      commandBuffer.SetComponent<Transform>(e, transform);
      commandBuffer.SetComponent<Game.Vehicles.PersonalCar>(e, new Game.Vehicles.PersonalCar(keeper, state));
      commandBuffer.SetComponent<PrefabRef>(e, new PrefabRef(data.m_Entity));
      commandBuffer.SetComponent<PseudoRandomSeed>(e, new PseudoRandomSeed(ref random));
      if (source != Entity.Null)
      {
        commandBuffer.AddComponent<TripSource>(e, new TripSource(source, delay));
        commandBuffer.AddComponent<Unspawned>(e, new Unspawned());
      }
      return e;
    }

    private void CheckTrailers(
      int passengerAmount,
      int baggageAmount,
      int extraOffset,
      PersonalCarSelectData.CarData firstData,
      bool emptyOnly,
      bool noSlowVehicles,
      ref Random random,
      ref PersonalCarSelectData.CarData bestFirst,
      ref PersonalCarSelectData.CarData bestSecond,
      ref int totalProbability,
      ref int bestOffset)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTrailerData> nativeArray1 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<Game.Prefabs.CarData> nativeArray3 = prefabChunk.GetNativeArray<Game.Prefabs.CarData>(ref this.m_CarDataType);
          NativeArray<PersonalCarData> nativeArray4 = prefabChunk.GetNativeArray<PersonalCarData>(ref this.m_PersonalCarDataType);
          NativeArray<CarTractorData> nativeArray5 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
          NativeArray<ObjectData> nativeArray6 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
          NativeArray<MovingObjectData> nativeArray7 = prefabChunk.GetNativeArray<MovingObjectData>(ref this.m_MovingObjectDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Game.Prefabs.CarData carData = nativeArray3[index2];
            if ((!noSlowVehicles || (double) carData.m_MaxSpeed >= 22.222223281860352) && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              PersonalCarSelectData.CarData secondData = new PersonalCarSelectData.CarData();
              secondData.m_PersonalCarData = nativeArray4[index2];
              if (!emptyOnly || secondData.m_PersonalCarData.m_PassengerCapacity == 0 && secondData.m_PersonalCarData.m_BaggageCapacity == 0)
              {
                secondData.m_Entity = nativeArray2[index2];
                secondData.m_TrailerData = nativeArray1[index2];
                if (firstData.m_TractorData.m_TrailerType == secondData.m_TrailerData.m_TrailerType && (!(firstData.m_TractorData.m_FixedTrailer != Entity.Null) || !(firstData.m_TractorData.m_FixedTrailer != secondData.m_Entity)) && (!(secondData.m_TrailerData.m_FixedTractor != Entity.Null) || !(secondData.m_TrailerData.m_FixedTractor != firstData.m_Entity)))
                {
                  secondData.m_ObjectData = nativeArray6[index2];
                  secondData.m_MovingObjectData = nativeArray7[index2];
                  if (nativeArray5.Length != 0)
                  {
                    secondData.m_TractorData = nativeArray5[index2];
                    if (secondData.m_TractorData.m_FixedTrailer != Entity.Null)
                      continue;
                  }
                  int probability;
                  int offset;
                  this.CalculateProbability(passengerAmount, baggageAmount, firstData, secondData, out probability, out offset);
                  if (this.PickVehicle(ref random, probability, offset + extraOffset, ref totalProbability, ref bestOffset))
                  {
                    bestFirst = firstData;
                    bestSecond = secondData;
                  }
                }
              }
            }
          }
        }
      }
    }

    private void CheckTractors(
      int passengerAmount,
      int baggageAmount,
      int extraOffset,
      PersonalCarSelectData.CarData secondData,
      bool noSlowVehicles,
      ref Random random,
      ref PersonalCarSelectData.CarData bestFirst,
      ref PersonalCarSelectData.CarData bestSecond,
      ref int totalProbability,
      ref int bestOffset)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTractorData> nativeArray1 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        if (nativeArray1.Length != 0 && !prefabChunk.Has<CarTrailerData>(ref this.m_CarTrailerDataType))
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<Game.Prefabs.CarData> nativeArray3 = prefabChunk.GetNativeArray<Game.Prefabs.CarData>(ref this.m_CarDataType);
          NativeArray<PersonalCarData> nativeArray4 = prefabChunk.GetNativeArray<PersonalCarData>(ref this.m_PersonalCarDataType);
          NativeArray<ObjectData> nativeArray5 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
          NativeArray<MovingObjectData> nativeArray6 = prefabChunk.GetNativeArray<MovingObjectData>(ref this.m_MovingObjectDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Game.Prefabs.CarData carData = nativeArray3[index2];
            if ((!noSlowVehicles || (double) carData.m_MaxSpeed >= 22.222223281860352) && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              PersonalCarSelectData.CarData firstData = new PersonalCarSelectData.CarData();
              firstData.m_PersonalCarData = nativeArray4[index2];
              firstData.m_Entity = nativeArray2[index2];
              firstData.m_TractorData = nativeArray1[index2];
              if (firstData.m_TractorData.m_TrailerType == secondData.m_TrailerData.m_TrailerType && (!(firstData.m_TractorData.m_FixedTrailer != Entity.Null) || !(firstData.m_TractorData.m_FixedTrailer != secondData.m_Entity)) && (!(secondData.m_TrailerData.m_FixedTractor != Entity.Null) || !(secondData.m_TrailerData.m_FixedTractor != firstData.m_Entity)))
              {
                firstData.m_ObjectData = nativeArray5[index2];
                firstData.m_MovingObjectData = nativeArray6[index2];
                int probability;
                int offset;
                this.CalculateProbability(passengerAmount, baggageAmount, firstData, secondData, out probability, out offset);
                if (this.PickVehicle(ref random, probability, offset + extraOffset, ref totalProbability, ref bestOffset))
                {
                  bestFirst = firstData;
                  bestSecond = secondData;
                }
              }
            }
          }
        }
      }
    }

    private void CalculateProbability(
      int passengerAmount,
      int baggageAmount,
      PersonalCarSelectData.CarData firstData,
      PersonalCarSelectData.CarData secondData,
      out int probability,
      out int offset)
    {
      int num1 = firstData.m_PersonalCarData.m_PassengerCapacity + secondData.m_PersonalCarData.m_PassengerCapacity;
      int num2 = firstData.m_PersonalCarData.m_BaggageCapacity + secondData.m_PersonalCarData.m_BaggageCapacity;
      int y1 = num1 - passengerAmount;
      int num3 = baggageAmount;
      int y2 = num2 - num3;
      offset = math.min(0, y1) + math.min(0, y2);
      offset = math.select(0, offset - 10, offset != 0) + math.min(0, 4 - y1) + math.min(0, 4 - y2);
      probability = firstData.m_PersonalCarData.m_Probability;
      probability = math.select(probability, probability * secondData.m_PersonalCarData.m_Probability / 50, secondData.m_Entity != Entity.Null);
      probability = math.max(1, probability / ((1 << math.max(0, y1)) + (1 << math.max(0, y2))));
    }

    private bool PickVehicle(
      ref Random random,
      int probability,
      int offset,
      ref int totalProbability,
      ref int bestOffset)
    {
      if (offset == bestOffset)
      {
        totalProbability += probability;
        return random.NextInt(totalProbability) < probability;
      }
      if (offset <= bestOffset)
        return false;
      totalProbability = probability;
      bestOffset = offset;
      return true;
    }

    private struct CarData
    {
      public Entity m_Entity;
      public PersonalCarData m_PersonalCarData;
      public CarTrailerData m_TrailerData;
      public CarTractorData m_TractorData;
      public ObjectData m_ObjectData;
      public MovingObjectData m_MovingObjectData;
    }
  }
}
