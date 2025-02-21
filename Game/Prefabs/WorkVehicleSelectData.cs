// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WorkVehicleSelectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Objects;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct WorkVehicleSelectData
  {
    private NativeList<ArchetypeChunk> m_PrefabChunks;
    private VehicleSelectRequirementData m_RequirementData;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<WorkVehicleData> m_WorkVehicleDataType;
    private ComponentTypeHandle<CarTrailerData> m_CarTrailerDataType;
    private ComponentTypeHandle<CarTractorData> m_CarTractorDataType;
    private ComponentTypeHandle<ObjectData> m_ObjectDataType;

    public static EntityQueryDesc GetEntityQueryDesc()
    {
      return new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<WorkVehicleData>(),
          ComponentType.ReadOnly<CarData>(),
          ComponentType.ReadOnly<ObjectData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Locked>()
        }
      };
    }

    public WorkVehicleSelectData(SystemBase system)
    {
      this.m_PrefabChunks = new NativeList<ArchetypeChunk>();
      this.m_RequirementData = new VehicleSelectRequirementData(system);
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_WorkVehicleDataType = system.GetComponentTypeHandle<WorkVehicleData>(true);
      this.m_CarTrailerDataType = system.GetComponentTypeHandle<CarTrailerData>(true);
      this.m_CarTractorDataType = system.GetComponentTypeHandle<CarTractorData>(true);
      this.m_ObjectDataType = system.GetComponentTypeHandle<ObjectData>(true);
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
      this.m_WorkVehicleDataType.Update(system);
      this.m_CarTrailerDataType.Update(system);
      this.m_CarTractorDataType.Update(system);
      this.m_ObjectDataType.Update(system);
    }

    public void PostUpdate(JobHandle jobHandle) => this.m_PrefabChunks.Dispose(jobHandle);

    public Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      VehicleWorkType workType,
      MapFeature mapFeature,
      Resource resource,
      ref float workAmount,
      Transform transform,
      Entity source,
      WorkVehicleFlags state)
    {
      WorkVehicleSelectData.VehicleData bestFirst = new WorkVehicleSelectData.VehicleData();
      WorkVehicleSelectData.VehicleData bestSecond = new WorkVehicleSelectData.VehicleData();
      WorkVehicleSelectData.VehicleData bestThird = new WorkVehicleSelectData.VehicleData();
      WorkVehicleSelectData.VehicleData bestForth = new WorkVehicleSelectData.VehicleData();
      int totalProbability = 0;
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<Entity> nativeArray1 = prefabChunk.GetNativeArray(this.m_EntityType);
        NativeArray<WorkVehicleData> nativeArray2 = prefabChunk.GetNativeArray<WorkVehicleData>(ref this.m_WorkVehicleDataType);
        NativeArray<CarTrailerData> nativeArray3 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        NativeArray<CarTractorData> nativeArray4 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        NativeArray<ObjectData> nativeArray5 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
        VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
        for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
        {
          WorkVehicleSelectData.VehicleData vehicleData = new WorkVehicleSelectData.VehicleData();
          vehicleData.m_WorkVehicleData = nativeArray2[index2];
          if (vehicleData.m_WorkVehicleData.m_WorkType != VehicleWorkType.None && vehicleData.m_WorkVehicleData.m_WorkType == workType && (vehicleData.m_WorkVehicleData.m_MapFeature == MapFeature.None && vehicleData.m_WorkVehicleData.m_Resources == Resource.NoResource || vehicleData.m_WorkVehicleData.m_MapFeature == mapFeature || (vehicleData.m_WorkVehicleData.m_Resources & resource) != Resource.NoResource) && (double) vehicleData.m_WorkVehicleData.m_MaxWorkAmount != 0.0 && this.m_RequirementData.CheckRequirements(ref chunk, index2))
          {
            vehicleData.m_Entity = nativeArray1[index2];
            vehicleData.m_ObjectData = nativeArray5[index2];
            bool firstIsTrailer = false;
            if (nativeArray3.Length != 0)
            {
              vehicleData.m_TrailerData = nativeArray3[index2];
              firstIsTrailer = true;
            }
            if (nativeArray4.Length != 0)
            {
              vehicleData.m_TractorData = nativeArray4[index2];
              if (vehicleData.m_TractorData.m_FixedTrailer != Entity.Null)
              {
                this.CheckTrailers(workType, mapFeature, resource, firstIsTrailer, vehicleData, ref random, ref bestFirst, ref bestSecond, ref bestThird, ref bestForth, ref totalProbability);
                continue;
              }
            }
            if (firstIsTrailer)
              this.CheckTractors(workType, mapFeature, resource, vehicleData, ref random, ref bestFirst, ref bestSecond, ref bestThird, ref bestForth, ref totalProbability);
            else if (this.PickVehicle(ref random, 100, ref totalProbability))
            {
              bestFirst = vehicleData;
              bestSecond = new WorkVehicleSelectData.VehicleData();
              bestThird = new WorkVehicleSelectData.VehicleData();
              bestForth = new WorkVehicleSelectData.VehicleData();
            }
          }
        }
      }
      if (bestFirst.m_Entity == Entity.Null)
      {
        workAmount = 0.0f;
        return Entity.Null;
      }
      float workAmount1 = workAmount;
      Entity vehicle1 = this.CreateVehicle(commandBuffer, jobIndex, ref random, bestFirst, workType, ref workAmount1, transform, source, state);
      if (bestSecond.m_Entity != Entity.Null)
      {
        DynamicBuffer<LayoutElement> dynamicBuffer = commandBuffer.AddBuffer<LayoutElement>(jobIndex, vehicle1);
        dynamicBuffer.Add(new LayoutElement(vehicle1));
        Entity vehicle2 = this.CreateVehicle(commandBuffer, jobIndex, ref random, bestSecond, workType, ref workAmount1, transform, source, state & WorkVehicleFlags.ExtractorVehicle);
        commandBuffer.SetComponent<Controller>(jobIndex, vehicle2, new Controller(vehicle1));
        dynamicBuffer.Add(new LayoutElement(vehicle2));
        if (bestThird.m_Entity != Entity.Null)
        {
          Entity vehicle3 = this.CreateVehicle(commandBuffer, jobIndex, ref random, bestThird, workType, ref workAmount1, transform, source, state & WorkVehicleFlags.ExtractorVehicle);
          commandBuffer.SetComponent<Controller>(jobIndex, vehicle3, new Controller(vehicle1));
          dynamicBuffer.Add(new LayoutElement(vehicle3));
        }
        if (bestForth.m_Entity != Entity.Null)
        {
          Entity vehicle4 = this.CreateVehicle(commandBuffer, jobIndex, ref random, bestForth, workType, ref workAmount1, transform, source, state & WorkVehicleFlags.ExtractorVehicle);
          commandBuffer.SetComponent<Controller>(jobIndex, vehicle4, new Controller(vehicle1));
          dynamicBuffer.Add(new LayoutElement(vehicle4));
        }
      }
      workAmount -= workAmount1;
      return vehicle1;
    }

    private Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      WorkVehicleSelectData.VehicleData data,
      VehicleWorkType workType,
      ref float workAmount,
      Transform transform,
      Entity source,
      WorkVehicleFlags state)
    {
      Game.Vehicles.WorkVehicle component = new Game.Vehicles.WorkVehicle();
      component.m_State = state;
      if (workType == data.m_WorkVehicleData.m_WorkType && (double) workAmount > 0.0)
      {
        component.m_WorkAmount = math.min(workAmount, data.m_WorkVehicleData.m_MaxWorkAmount);
        workAmount -= component.m_WorkAmount;
      }
      Entity entity = commandBuffer.CreateEntity(jobIndex, data.m_ObjectData.m_Archetype);
      commandBuffer.SetComponent<Transform>(jobIndex, entity, transform);
      commandBuffer.SetComponent<Game.Vehicles.WorkVehicle>(jobIndex, entity, component);
      commandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(data.m_Entity));
      commandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, new PseudoRandomSeed(ref random));
      commandBuffer.AddComponent<TripSource>(jobIndex, entity, new TripSource(source));
      commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
      return entity;
    }

    private void CheckTrailers(
      VehicleWorkType workType,
      MapFeature mapFeature,
      Resource resource,
      bool firstIsTrailer,
      WorkVehicleSelectData.VehicleData firstData,
      ref Random random,
      ref WorkVehicleSelectData.VehicleData bestFirst,
      ref WorkVehicleSelectData.VehicleData bestSecond,
      ref WorkVehicleSelectData.VehicleData bestThird,
      ref WorkVehicleSelectData.VehicleData bestForth,
      ref int totalProbability)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTrailerData> nativeArray1 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<WorkVehicleData> nativeArray3 = prefabChunk.GetNativeArray<WorkVehicleData>(ref this.m_WorkVehicleDataType);
          NativeArray<CarTractorData> nativeArray4 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
          NativeArray<ObjectData> nativeArray5 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            WorkVehicleSelectData.VehicleData vehicleData = new WorkVehicleSelectData.VehicleData();
            vehicleData.m_WorkVehicleData = nativeArray3[index2];
            if ((vehicleData.m_WorkVehicleData.m_WorkType == VehicleWorkType.None || vehicleData.m_WorkVehicleData.m_WorkType == workType) && (vehicleData.m_WorkVehicleData.m_MapFeature == MapFeature.None && vehicleData.m_WorkVehicleData.m_Resources == Resource.NoResource || vehicleData.m_WorkVehicleData.m_MapFeature == mapFeature || (vehicleData.m_WorkVehicleData.m_Resources & resource) != Resource.NoResource) && (double) vehicleData.m_WorkVehicleData.m_MaxWorkAmount == 0.0 && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              vehicleData.m_Entity = nativeArray2[index2];
              vehicleData.m_TrailerData = nativeArray1[index2];
              if (firstData.m_TractorData.m_TrailerType == vehicleData.m_TrailerData.m_TrailerType && (!(firstData.m_TractorData.m_FixedTrailer != Entity.Null) || !(firstData.m_TractorData.m_FixedTrailer != vehicleData.m_Entity)) && (!(vehicleData.m_TrailerData.m_FixedTractor != Entity.Null) || !(vehicleData.m_TrailerData.m_FixedTractor != firstData.m_Entity)))
              {
                vehicleData.m_ObjectData = nativeArray5[index2];
                if (nativeArray4.Length != 0)
                {
                  vehicleData.m_TractorData = nativeArray4[index2];
                  if (vehicleData.m_TractorData.m_FixedTrailer != Entity.Null)
                  {
                    this.CheckTrailers(workType, mapFeature, resource, firstIsTrailer, firstData, vehicleData, ref random, ref bestFirst, ref bestSecond, ref bestThird, ref bestForth, ref totalProbability);
                    continue;
                  }
                }
                if (firstIsTrailer)
                  this.CheckTractors(workType, mapFeature, resource, firstData, vehicleData, ref random, ref bestFirst, ref bestSecond, ref bestThird, ref bestForth, ref totalProbability);
                else if (this.PickVehicle(ref random, 100, ref totalProbability))
                {
                  bestFirst = firstData;
                  bestSecond = vehicleData;
                  bestThird = new WorkVehicleSelectData.VehicleData();
                  bestForth = new WorkVehicleSelectData.VehicleData();
                }
              }
            }
          }
        }
      }
    }

    private void CheckTrailers(
      VehicleWorkType workType,
      MapFeature mapFeature,
      Resource resource,
      bool firstIsTrailer,
      WorkVehicleSelectData.VehicleData firstData,
      WorkVehicleSelectData.VehicleData secondData,
      ref Random random,
      ref WorkVehicleSelectData.VehicleData bestFirst,
      ref WorkVehicleSelectData.VehicleData bestSecond,
      ref WorkVehicleSelectData.VehicleData bestThird,
      ref WorkVehicleSelectData.VehicleData bestForth,
      ref int totalProbability)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTrailerData> nativeArray1 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<WorkVehicleData> nativeArray3 = prefabChunk.GetNativeArray<WorkVehicleData>(ref this.m_WorkVehicleDataType);
          NativeArray<CarTractorData> nativeArray4 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
          NativeArray<ObjectData> nativeArray5 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            WorkVehicleSelectData.VehicleData vehicleData = new WorkVehicleSelectData.VehicleData();
            vehicleData.m_WorkVehicleData = nativeArray3[index2];
            if ((vehicleData.m_WorkVehicleData.m_WorkType == VehicleWorkType.None || vehicleData.m_WorkVehicleData.m_WorkType == workType) && (vehicleData.m_WorkVehicleData.m_MapFeature == MapFeature.None && vehicleData.m_WorkVehicleData.m_Resources == Resource.NoResource || vehicleData.m_WorkVehicleData.m_MapFeature == mapFeature || (vehicleData.m_WorkVehicleData.m_Resources & resource) != Resource.NoResource) && (double) vehicleData.m_WorkVehicleData.m_MaxWorkAmount == 0.0 && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              vehicleData.m_Entity = nativeArray2[index2];
              vehicleData.m_TrailerData = nativeArray1[index2];
              if (secondData.m_TractorData.m_TrailerType == vehicleData.m_TrailerData.m_TrailerType && (!(secondData.m_TractorData.m_FixedTrailer != Entity.Null) || !(secondData.m_TractorData.m_FixedTrailer != vehicleData.m_Entity)) && (!(vehicleData.m_TrailerData.m_FixedTractor != Entity.Null) || !(vehicleData.m_TrailerData.m_FixedTractor != secondData.m_Entity)))
              {
                vehicleData.m_ObjectData = nativeArray5[index2];
                if (nativeArray4.Length != 0)
                {
                  vehicleData.m_TractorData = nativeArray4[index2];
                  if (vehicleData.m_TractorData.m_FixedTrailer != Entity.Null)
                  {
                    if (!firstIsTrailer)
                    {
                      this.CheckTrailers(workType, mapFeature, resource, firstData, secondData, vehicleData, ref random, ref bestFirst, ref bestSecond, ref bestThird, ref bestForth, ref totalProbability);
                      continue;
                    }
                    continue;
                  }
                }
                if (firstIsTrailer)
                  this.CheckTractors(workType, mapFeature, resource, firstData, secondData, vehicleData, ref random, ref bestFirst, ref bestSecond, ref bestThird, ref bestForth, ref totalProbability);
                else if (this.PickVehicle(ref random, 100, ref totalProbability))
                {
                  bestFirst = firstData;
                  bestSecond = secondData;
                  bestThird = vehicleData;
                  bestForth = new WorkVehicleSelectData.VehicleData();
                }
              }
            }
          }
        }
      }
    }

    private void CheckTrailers(
      VehicleWorkType workType,
      MapFeature mapFeature,
      Resource resource,
      WorkVehicleSelectData.VehicleData firstData,
      WorkVehicleSelectData.VehicleData secondData,
      WorkVehicleSelectData.VehicleData thirdData,
      ref Random random,
      ref WorkVehicleSelectData.VehicleData bestFirst,
      ref WorkVehicleSelectData.VehicleData bestSecond,
      ref WorkVehicleSelectData.VehicleData bestThird,
      ref WorkVehicleSelectData.VehicleData bestForth,
      ref int totalProbability)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTrailerData> nativeArray1 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<WorkVehicleData> nativeArray3 = prefabChunk.GetNativeArray<WorkVehicleData>(ref this.m_WorkVehicleDataType);
          NativeArray<CarTractorData> nativeArray4 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
          NativeArray<ObjectData> nativeArray5 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            WorkVehicleSelectData.VehicleData vehicleData = new WorkVehicleSelectData.VehicleData();
            vehicleData.m_WorkVehicleData = nativeArray3[index2];
            if ((vehicleData.m_WorkVehicleData.m_WorkType == VehicleWorkType.None || vehicleData.m_WorkVehicleData.m_WorkType == workType) && (vehicleData.m_WorkVehicleData.m_MapFeature == MapFeature.None && vehicleData.m_WorkVehicleData.m_Resources == Resource.NoResource || vehicleData.m_WorkVehicleData.m_MapFeature == mapFeature || (vehicleData.m_WorkVehicleData.m_Resources & resource) != Resource.NoResource) && (double) vehicleData.m_WorkVehicleData.m_MaxWorkAmount == 0.0 && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              vehicleData.m_Entity = nativeArray2[index2];
              vehicleData.m_TrailerData = nativeArray1[index2];
              if (thirdData.m_TractorData.m_TrailerType == vehicleData.m_TrailerData.m_TrailerType && (!(thirdData.m_TractorData.m_FixedTrailer != Entity.Null) || !(thirdData.m_TractorData.m_FixedTrailer != vehicleData.m_Entity)) && (!(vehicleData.m_TrailerData.m_FixedTractor != Entity.Null) || !(vehicleData.m_TrailerData.m_FixedTractor != thirdData.m_Entity)))
              {
                vehicleData.m_ObjectData = nativeArray5[index2];
                if (nativeArray4.Length != 0)
                {
                  vehicleData.m_TractorData = nativeArray4[index2];
                  if (vehicleData.m_TractorData.m_FixedTrailer != Entity.Null)
                    continue;
                }
                if (this.PickVehicle(ref random, 100, ref totalProbability))
                {
                  bestFirst = firstData;
                  bestSecond = secondData;
                  bestThird = thirdData;
                  bestForth = vehicleData;
                }
              }
            }
          }
        }
      }
    }

    private void CheckTractors(
      VehicleWorkType workType,
      MapFeature mapFeature,
      Resource resource,
      WorkVehicleSelectData.VehicleData secondData,
      ref Random random,
      ref WorkVehicleSelectData.VehicleData bestFirst,
      ref WorkVehicleSelectData.VehicleData bestSecond,
      ref WorkVehicleSelectData.VehicleData bestThird,
      ref WorkVehicleSelectData.VehicleData bestForth,
      ref int totalProbability)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTractorData> nativeArray1 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<WorkVehicleData> nativeArray3 = prefabChunk.GetNativeArray<WorkVehicleData>(ref this.m_WorkVehicleDataType);
          NativeArray<CarTrailerData> nativeArray4 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
          NativeArray<ObjectData> nativeArray5 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            WorkVehicleSelectData.VehicleData secondData1 = new WorkVehicleSelectData.VehicleData();
            secondData1.m_WorkVehicleData = nativeArray3[index2];
            if ((secondData1.m_WorkVehicleData.m_WorkType == VehicleWorkType.None || secondData1.m_WorkVehicleData.m_WorkType == workType) && (secondData1.m_WorkVehicleData.m_MapFeature == MapFeature.None && secondData1.m_WorkVehicleData.m_Resources == Resource.NoResource || secondData1.m_WorkVehicleData.m_MapFeature == mapFeature || (secondData1.m_WorkVehicleData.m_Resources & resource) != Resource.NoResource) && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              secondData1.m_Entity = nativeArray2[index2];
              secondData1.m_TractorData = nativeArray1[index2];
              if (secondData1.m_TractorData.m_TrailerType == secondData.m_TrailerData.m_TrailerType && (!(secondData1.m_TractorData.m_FixedTrailer != Entity.Null) || !(secondData1.m_TractorData.m_FixedTrailer != secondData.m_Entity)) && (!(secondData.m_TrailerData.m_FixedTractor != Entity.Null) || !(secondData.m_TrailerData.m_FixedTractor != secondData1.m_Entity)))
              {
                secondData1.m_ObjectData = nativeArray5[index2];
                if (nativeArray4.Length != 0)
                {
                  secondData1.m_TrailerData = nativeArray4[index2];
                  this.CheckTractors(workType, mapFeature, resource, secondData1, secondData, ref random, ref bestFirst, ref bestSecond, ref bestThird, ref bestForth, ref totalProbability);
                }
                else if (this.PickVehicle(ref random, 100, ref totalProbability))
                {
                  bestFirst = secondData1;
                  bestSecond = secondData;
                  bestThird = new WorkVehicleSelectData.VehicleData();
                  bestForth = new WorkVehicleSelectData.VehicleData();
                }
              }
            }
          }
        }
      }
    }

    private void CheckTractors(
      VehicleWorkType workType,
      MapFeature mapFeature,
      Resource resource,
      WorkVehicleSelectData.VehicleData secondData,
      WorkVehicleSelectData.VehicleData thirdData,
      ref Random random,
      ref WorkVehicleSelectData.VehicleData bestFirst,
      ref WorkVehicleSelectData.VehicleData bestSecond,
      ref WorkVehicleSelectData.VehicleData bestThird,
      ref WorkVehicleSelectData.VehicleData bestForth,
      ref int totalProbability)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTractorData> nativeArray1 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<WorkVehicleData> nativeArray3 = prefabChunk.GetNativeArray<WorkVehicleData>(ref this.m_WorkVehicleDataType);
          NativeArray<CarTrailerData> nativeArray4 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
          NativeArray<ObjectData> nativeArray5 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            WorkVehicleSelectData.VehicleData secondData1 = new WorkVehicleSelectData.VehicleData();
            secondData1.m_WorkVehicleData = nativeArray3[index2];
            if ((secondData1.m_WorkVehicleData.m_WorkType == VehicleWorkType.None || secondData1.m_WorkVehicleData.m_WorkType == workType) && (secondData1.m_WorkVehicleData.m_MapFeature == MapFeature.None && secondData1.m_WorkVehicleData.m_Resources == Resource.NoResource || secondData1.m_WorkVehicleData.m_MapFeature == mapFeature || (secondData1.m_WorkVehicleData.m_Resources & resource) != Resource.NoResource) && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              secondData1.m_Entity = nativeArray2[index2];
              secondData1.m_TractorData = nativeArray1[index2];
              if (secondData1.m_TractorData.m_TrailerType == secondData.m_TrailerData.m_TrailerType && (!(secondData1.m_TractorData.m_FixedTrailer != Entity.Null) || !(secondData1.m_TractorData.m_FixedTrailer != secondData.m_Entity)) && (!(secondData.m_TrailerData.m_FixedTractor != Entity.Null) || !(secondData.m_TrailerData.m_FixedTractor != secondData1.m_Entity)))
              {
                secondData1.m_ObjectData = nativeArray5[index2];
                if (nativeArray4.Length != 0)
                {
                  secondData1.m_TrailerData = nativeArray4[index2];
                  this.CheckTractors(workType, mapFeature, resource, secondData1, secondData, thirdData, ref random, ref bestFirst, ref bestSecond, ref bestThird, ref bestForth, ref totalProbability);
                }
                else if (this.PickVehicle(ref random, 100, ref totalProbability))
                {
                  bestFirst = secondData1;
                  bestSecond = secondData;
                  bestThird = thirdData;
                  bestForth = new WorkVehicleSelectData.VehicleData();
                }
              }
            }
          }
        }
      }
    }

    private void CheckTractors(
      VehicleWorkType workType,
      MapFeature mapFeature,
      Resource resource,
      WorkVehicleSelectData.VehicleData secondData,
      WorkVehicleSelectData.VehicleData thirdData,
      WorkVehicleSelectData.VehicleData forthData,
      ref Random random,
      ref WorkVehicleSelectData.VehicleData bestFirst,
      ref WorkVehicleSelectData.VehicleData bestSecond,
      ref WorkVehicleSelectData.VehicleData bestThird,
      ref WorkVehicleSelectData.VehicleData bestForth,
      ref int totalProbability)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTractorData> nativeArray1 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        if (nativeArray1.Length != 0 && !prefabChunk.Has<CarTrailerData>(ref this.m_CarTrailerDataType))
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<WorkVehicleData> nativeArray3 = prefabChunk.GetNativeArray<WorkVehicleData>(ref this.m_WorkVehicleDataType);
          NativeArray<ObjectData> nativeArray4 = prefabChunk.GetNativeArray<ObjectData>(ref this.m_ObjectDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            WorkVehicleSelectData.VehicleData vehicleData = new WorkVehicleSelectData.VehicleData();
            vehicleData.m_WorkVehicleData = nativeArray3[index2];
            if ((vehicleData.m_WorkVehicleData.m_WorkType == VehicleWorkType.None || vehicleData.m_WorkVehicleData.m_WorkType == workType) && (vehicleData.m_WorkVehicleData.m_MapFeature == MapFeature.None && vehicleData.m_WorkVehicleData.m_Resources == Resource.NoResource || vehicleData.m_WorkVehicleData.m_MapFeature == mapFeature || (vehicleData.m_WorkVehicleData.m_Resources & resource) != Resource.NoResource) && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              vehicleData.m_Entity = nativeArray2[index2];
              vehicleData.m_TractorData = nativeArray1[index2];
              if (vehicleData.m_TractorData.m_TrailerType == secondData.m_TrailerData.m_TrailerType && (!(vehicleData.m_TractorData.m_FixedTrailer != Entity.Null) || !(vehicleData.m_TractorData.m_FixedTrailer != secondData.m_Entity)) && (!(secondData.m_TrailerData.m_FixedTractor != Entity.Null) || !(secondData.m_TrailerData.m_FixedTractor != vehicleData.m_Entity)))
              {
                vehicleData.m_ObjectData = nativeArray4[index2];
                if (this.PickVehicle(ref random, 100, ref totalProbability))
                {
                  bestFirst = vehicleData;
                  bestSecond = secondData;
                  bestThird = thirdData;
                  bestForth = forthData;
                }
              }
            }
          }
        }
      }
    }

    private bool PickVehicle(ref Random random, int probability, ref int totalProbability)
    {
      totalProbability += probability;
      return random.NextInt(totalProbability) < probability;
    }

    private struct VehicleData
    {
      public Entity m_Entity;
      public WorkVehicleData m_WorkVehicleData;
      public CarTrailerData m_TrailerData;
      public CarTractorData m_TractorData;
      public ObjectData m_ObjectData;
    }
  }
}
