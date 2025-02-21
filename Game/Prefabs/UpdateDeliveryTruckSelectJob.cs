// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UpdateDeliveryTruckSelectJob
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Prefabs
{
  [BurstCompile]
  public struct UpdateDeliveryTruckSelectJob : IJob
  {
    [ReadOnly]
    public EntityTypeHandle m_EntityType;
    [ReadOnly]
    public ComponentTypeHandle<DeliveryTruckData> m_DeliveryTruckDataType;
    [ReadOnly]
    public ComponentTypeHandle<CarTrailerData> m_CarTrailerDataType;
    [ReadOnly]
    public ComponentTypeHandle<CarTractorData> m_CarTractorDataType;
    [ReadOnly]
    public NativeList<ArchetypeChunk> m_PrefabChunks;
    [ReadOnly]
    public VehicleSelectRequirementData m_RequirementData;
    public NativeList<DeliveryTruckSelectItem> m_DeliveryTruckItems;

    public void Execute()
    {
      this.m_DeliveryTruckItems.Clear();
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<Entity> nativeArray1 = prefabChunk.GetNativeArray(this.m_EntityType);
        NativeArray<DeliveryTruckData> nativeArray2 = prefabChunk.GetNativeArray<DeliveryTruckData>(ref this.m_DeliveryTruckDataType);
        NativeArray<CarTrailerData> nativeArray3 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        NativeArray<CarTractorData> nativeArray4 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
        for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
        {
          UpdateDeliveryTruckSelectJob.TruckData truckData = new UpdateDeliveryTruckSelectJob.TruckData();
          truckData.m_DeliveryTruckData = nativeArray2[index2];
          if (truckData.m_DeliveryTruckData.m_CargoCapacity != 0 && truckData.m_DeliveryTruckData.m_TransportedResources != Resource.NoResource && this.m_RequirementData.CheckRequirements(ref chunk, index2))
          {
            Resource transportedResources = truckData.m_DeliveryTruckData.m_TransportedResources;
            truckData.m_Entity = nativeArray1[index2];
            bool firstIsTrailer = false;
            if (nativeArray3.Length != 0)
            {
              truckData.m_TrailerData = nativeArray3[index2];
              firstIsTrailer = true;
            }
            if (nativeArray4.Length != 0)
            {
              truckData.m_TractorData = nativeArray4[index2];
              if (truckData.m_TractorData.m_FixedTrailer != Entity.Null)
              {
                this.CheckTrailers(transportedResources, firstIsTrailer, truckData);
                continue;
              }
            }
            if (firstIsTrailer)
              this.CheckTractors(transportedResources, truckData);
            else
              this.m_DeliveryTruckItems.Add(new DeliveryTruckSelectItem()
              {
                m_Capacity = truckData.m_DeliveryTruckData.m_CargoCapacity,
                m_Cost = truckData.m_DeliveryTruckData.m_CostToDrive,
                m_Resources = transportedResources,
                m_Prefab1 = truckData.m_Entity
              });
          }
        }
      }
      if (this.m_DeliveryTruckItems.Length >= 2)
      {
        this.m_DeliveryTruckItems.Sort<DeliveryTruckSelectItem>();
        DeliveryTruckSelectItem deliveryTruckSelectItem1 = new DeliveryTruckSelectItem();
        DeliveryTruckSelectItem deliveryTruckSelectItem2 = this.m_DeliveryTruckItems[0];
        int index3 = 0;
        for (int index4 = 1; index4 < this.m_DeliveryTruckItems.Length; ++index4)
        {
          DeliveryTruckSelectItem deliveryTruckItem1 = this.m_DeliveryTruckItems[index4];
          if (deliveryTruckSelectItem2.m_Resources != Resource.NoResource && deliveryTruckSelectItem2.m_Cost > deliveryTruckItem1.m_Cost)
          {
            deliveryTruckSelectItem2.m_Resources &= ~deliveryTruckItem1.m_Resources;
            for (int index5 = index4 + 1; index5 < this.m_DeliveryTruckItems.Length && deliveryTruckSelectItem2.m_Resources != Resource.NoResource; ++index5)
            {
              DeliveryTruckSelectItem deliveryTruckItem2 = this.m_DeliveryTruckItems[index5];
              if (deliveryTruckSelectItem2.m_Cost > deliveryTruckItem2.m_Cost)
                deliveryTruckSelectItem2.m_Resources &= ~deliveryTruckItem2.m_Resources;
              else
                break;
            }
          }
          if (deliveryTruckSelectItem2.m_Resources != Resource.NoResource)
          {
            this.m_DeliveryTruckItems[index3++] = deliveryTruckSelectItem2;
            deliveryTruckSelectItem1 = deliveryTruckSelectItem2;
          }
          deliveryTruckSelectItem2 = deliveryTruckItem1;
          if (deliveryTruckSelectItem2.m_Resources != Resource.NoResource && deliveryTruckSelectItem2.m_Cost * deliveryTruckSelectItem1.m_Capacity > deliveryTruckSelectItem1.m_Cost * deliveryTruckSelectItem2.m_Capacity)
          {
            deliveryTruckSelectItem2.m_Resources &= ~deliveryTruckSelectItem1.m_Resources;
            for (int index6 = index3 - 2; index6 >= 0 && deliveryTruckSelectItem2.m_Resources != Resource.NoResource; --index6)
            {
              DeliveryTruckSelectItem deliveryTruckItem3 = this.m_DeliveryTruckItems[index6];
              if (deliveryTruckSelectItem2.m_Cost * deliveryTruckItem3.m_Capacity > deliveryTruckItem3.m_Cost * deliveryTruckSelectItem2.m_Capacity)
                deliveryTruckSelectItem2.m_Resources &= ~deliveryTruckItem3.m_Resources;
              else
                break;
            }
          }
        }
        if (deliveryTruckSelectItem2.m_Resources != Resource.NoResource)
          this.m_DeliveryTruckItems[index3++] = deliveryTruckSelectItem2;
        if (index3 < this.m_DeliveryTruckItems.Length)
          this.m_DeliveryTruckItems.RemoveRange(index3, this.m_DeliveryTruckItems.Length - index3);
      }
      this.m_DeliveryTruckItems.TrimExcess();
    }

    private void CheckTrailers(
      Resource resourceMask,
      bool firstIsTrailer,
      UpdateDeliveryTruckSelectJob.TruckData firstData)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTrailerData> nativeArray1 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<DeliveryTruckData> nativeArray3 = prefabChunk.GetNativeArray<DeliveryTruckData>(ref this.m_DeliveryTruckDataType);
          NativeArray<CarTractorData> nativeArray4 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            UpdateDeliveryTruckSelectJob.TruckData truckData = new UpdateDeliveryTruckSelectJob.TruckData();
            truckData.m_DeliveryTruckData = nativeArray3[index2];
            if (truckData.m_DeliveryTruckData.m_CargoCapacity == 0 && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              truckData.m_Entity = nativeArray2[index2];
              truckData.m_TrailerData = nativeArray1[index2];
              if (firstData.m_TractorData.m_TrailerType == truckData.m_TrailerData.m_TrailerType && (!(firstData.m_TractorData.m_FixedTrailer != Entity.Null) || !(firstData.m_TractorData.m_FixedTrailer != truckData.m_Entity)) && (!(truckData.m_TrailerData.m_FixedTractor != Entity.Null) || !(truckData.m_TrailerData.m_FixedTractor != firstData.m_Entity)))
              {
                if (nativeArray4.Length != 0)
                {
                  truckData.m_TractorData = nativeArray4[index2];
                  if (truckData.m_TractorData.m_FixedTrailer != Entity.Null)
                  {
                    this.CheckTrailers(resourceMask, firstIsTrailer, firstData, truckData);
                    continue;
                  }
                }
                if (firstIsTrailer)
                  this.CheckTractors(resourceMask, firstData, truckData);
                else
                  this.m_DeliveryTruckItems.Add(new DeliveryTruckSelectItem()
                  {
                    m_Capacity = firstData.m_DeliveryTruckData.m_CargoCapacity + truckData.m_DeliveryTruckData.m_CargoCapacity,
                    m_Cost = firstData.m_DeliveryTruckData.m_CostToDrive + truckData.m_DeliveryTruckData.m_CostToDrive,
                    m_Resources = resourceMask,
                    m_Prefab1 = firstData.m_Entity,
                    m_Prefab2 = truckData.m_Entity
                  });
              }
            }
          }
        }
      }
    }

    private void CheckTrailers(
      Resource resourceMask,
      bool firstIsTrailer,
      UpdateDeliveryTruckSelectJob.TruckData firstData,
      UpdateDeliveryTruckSelectJob.TruckData secondData)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTrailerData> nativeArray1 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<DeliveryTruckData> nativeArray3 = prefabChunk.GetNativeArray<DeliveryTruckData>(ref this.m_DeliveryTruckDataType);
          NativeArray<CarTractorData> nativeArray4 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            UpdateDeliveryTruckSelectJob.TruckData truckData = new UpdateDeliveryTruckSelectJob.TruckData();
            truckData.m_DeliveryTruckData = nativeArray3[index2];
            if (truckData.m_DeliveryTruckData.m_CargoCapacity == 0 && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              truckData.m_Entity = nativeArray2[index2];
              truckData.m_TrailerData = nativeArray1[index2];
              if (secondData.m_TractorData.m_TrailerType == truckData.m_TrailerData.m_TrailerType && (!(secondData.m_TractorData.m_FixedTrailer != Entity.Null) || !(secondData.m_TractorData.m_FixedTrailer != truckData.m_Entity)) && (!(truckData.m_TrailerData.m_FixedTractor != Entity.Null) || !(truckData.m_TrailerData.m_FixedTractor != secondData.m_Entity)))
              {
                if (nativeArray4.Length != 0)
                {
                  truckData.m_TractorData = nativeArray4[index2];
                  if (truckData.m_TractorData.m_FixedTrailer != Entity.Null)
                  {
                    if (!firstIsTrailer)
                    {
                      this.CheckTrailers(resourceMask, firstData, secondData, truckData);
                      continue;
                    }
                    continue;
                  }
                }
                if (firstIsTrailer)
                  this.CheckTractors(resourceMask, firstData, secondData, truckData);
                else
                  this.m_DeliveryTruckItems.Add(new DeliveryTruckSelectItem()
                  {
                    m_Capacity = firstData.m_DeliveryTruckData.m_CargoCapacity + secondData.m_DeliveryTruckData.m_CargoCapacity + truckData.m_DeliveryTruckData.m_CargoCapacity,
                    m_Cost = firstData.m_DeliveryTruckData.m_CostToDrive + secondData.m_DeliveryTruckData.m_CostToDrive + truckData.m_DeliveryTruckData.m_CostToDrive,
                    m_Resources = resourceMask,
                    m_Prefab1 = firstData.m_Entity,
                    m_Prefab2 = secondData.m_Entity,
                    m_Prefab3 = truckData.m_Entity
                  });
              }
            }
          }
        }
      }
    }

    private void CheckTrailers(
      Resource resourceMask,
      UpdateDeliveryTruckSelectJob.TruckData firstData,
      UpdateDeliveryTruckSelectJob.TruckData secondData,
      UpdateDeliveryTruckSelectJob.TruckData thirdData)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTrailerData> nativeArray1 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<DeliveryTruckData> nativeArray3 = prefabChunk.GetNativeArray<DeliveryTruckData>(ref this.m_DeliveryTruckDataType);
          NativeArray<CarTractorData> nativeArray4 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            UpdateDeliveryTruckSelectJob.TruckData truckData = new UpdateDeliveryTruckSelectJob.TruckData();
            truckData.m_DeliveryTruckData = nativeArray3[index2];
            if (truckData.m_DeliveryTruckData.m_CargoCapacity == 0 && this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              truckData.m_Entity = nativeArray2[index2];
              truckData.m_TrailerData = nativeArray1[index2];
              if (thirdData.m_TractorData.m_TrailerType == truckData.m_TrailerData.m_TrailerType && (!(thirdData.m_TractorData.m_FixedTrailer != Entity.Null) || !(thirdData.m_TractorData.m_FixedTrailer != truckData.m_Entity)) && (!(truckData.m_TrailerData.m_FixedTractor != Entity.Null) || !(truckData.m_TrailerData.m_FixedTractor != thirdData.m_Entity)))
              {
                if (nativeArray4.Length != 0)
                {
                  truckData.m_TractorData = nativeArray4[index2];
                  if (truckData.m_TractorData.m_FixedTrailer != Entity.Null)
                    continue;
                }
                this.m_DeliveryTruckItems.Add(new DeliveryTruckSelectItem()
                {
                  m_Capacity = firstData.m_DeliveryTruckData.m_CargoCapacity + secondData.m_DeliveryTruckData.m_CargoCapacity + thirdData.m_DeliveryTruckData.m_CargoCapacity + truckData.m_DeliveryTruckData.m_CargoCapacity,
                  m_Cost = firstData.m_DeliveryTruckData.m_CostToDrive + secondData.m_DeliveryTruckData.m_CostToDrive + thirdData.m_DeliveryTruckData.m_CostToDrive + truckData.m_DeliveryTruckData.m_CostToDrive,
                  m_Resources = resourceMask,
                  m_Prefab1 = firstData.m_Entity,
                  m_Prefab2 = secondData.m_Entity,
                  m_Prefab3 = thirdData.m_Entity,
                  m_Prefab4 = truckData.m_Entity
                });
              }
            }
          }
        }
      }
    }

    private void CheckTractors(
      Resource resourceMask,
      UpdateDeliveryTruckSelectJob.TruckData secondData)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTractorData> nativeArray1 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<DeliveryTruckData> nativeArray3 = prefabChunk.GetNativeArray<DeliveryTruckData>(ref this.m_DeliveryTruckDataType);
          NativeArray<CarTrailerData> nativeArray4 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            UpdateDeliveryTruckSelectJob.TruckData secondData1 = new UpdateDeliveryTruckSelectJob.TruckData();
            secondData1.m_DeliveryTruckData = nativeArray3[index2];
            Resource resourceMask1 = resourceMask;
            if (secondData1.m_DeliveryTruckData.m_CargoCapacity != 0)
            {
              resourceMask1 &= secondData1.m_DeliveryTruckData.m_TransportedResources;
              if (resourceMask1 == Resource.NoResource)
                continue;
            }
            if (this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              secondData1.m_Entity = nativeArray2[index2];
              secondData1.m_TractorData = nativeArray1[index2];
              if (secondData1.m_TractorData.m_TrailerType == secondData.m_TrailerData.m_TrailerType && (!(secondData1.m_TractorData.m_FixedTrailer != Entity.Null) || !(secondData1.m_TractorData.m_FixedTrailer != secondData.m_Entity)) && (!(secondData.m_TrailerData.m_FixedTractor != Entity.Null) || !(secondData.m_TrailerData.m_FixedTractor != secondData1.m_Entity)))
              {
                if (nativeArray4.Length != 0)
                {
                  secondData1.m_TrailerData = nativeArray4[index2];
                  this.CheckTractors(resourceMask1, secondData1, secondData);
                }
                else
                  this.m_DeliveryTruckItems.Add(new DeliveryTruckSelectItem()
                  {
                    m_Capacity = secondData1.m_DeliveryTruckData.m_CargoCapacity + secondData.m_DeliveryTruckData.m_CargoCapacity,
                    m_Cost = secondData1.m_DeliveryTruckData.m_CostToDrive + secondData.m_DeliveryTruckData.m_CostToDrive,
                    m_Resources = resourceMask1,
                    m_Prefab1 = secondData1.m_Entity,
                    m_Prefab2 = secondData.m_Entity
                  });
              }
            }
          }
        }
      }
    }

    private void CheckTractors(
      Resource resourceMask,
      UpdateDeliveryTruckSelectJob.TruckData secondData,
      UpdateDeliveryTruckSelectJob.TruckData thirdData)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTractorData> nativeArray1 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        if (nativeArray1.Length != 0)
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<DeliveryTruckData> nativeArray3 = prefabChunk.GetNativeArray<DeliveryTruckData>(ref this.m_DeliveryTruckDataType);
          NativeArray<CarTrailerData> nativeArray4 = prefabChunk.GetNativeArray<CarTrailerData>(ref this.m_CarTrailerDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            UpdateDeliveryTruckSelectJob.TruckData secondData1 = new UpdateDeliveryTruckSelectJob.TruckData();
            secondData1.m_DeliveryTruckData = nativeArray3[index2];
            Resource resourceMask1 = resourceMask;
            if (secondData1.m_DeliveryTruckData.m_CargoCapacity != 0)
            {
              resourceMask1 &= secondData1.m_DeliveryTruckData.m_TransportedResources;
              if (resourceMask1 == Resource.NoResource)
                continue;
            }
            if (this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              secondData1.m_Entity = nativeArray2[index2];
              secondData1.m_TractorData = nativeArray1[index2];
              if (secondData1.m_TractorData.m_TrailerType == secondData.m_TrailerData.m_TrailerType && (!(secondData1.m_TractorData.m_FixedTrailer != Entity.Null) || !(secondData1.m_TractorData.m_FixedTrailer != secondData.m_Entity)) && (!(secondData.m_TrailerData.m_FixedTractor != Entity.Null) || !(secondData.m_TrailerData.m_FixedTractor != secondData1.m_Entity)))
              {
                if (nativeArray4.Length != 0)
                {
                  secondData1.m_TrailerData = nativeArray4[index2];
                  this.CheckTractors(resourceMask1, secondData1, secondData, thirdData);
                }
                else
                  this.m_DeliveryTruckItems.Add(new DeliveryTruckSelectItem()
                  {
                    m_Capacity = secondData1.m_DeliveryTruckData.m_CargoCapacity + secondData.m_DeliveryTruckData.m_CargoCapacity + thirdData.m_DeliveryTruckData.m_CargoCapacity,
                    m_Cost = secondData1.m_DeliveryTruckData.m_CostToDrive + secondData.m_DeliveryTruckData.m_CostToDrive + thirdData.m_DeliveryTruckData.m_CostToDrive,
                    m_Resources = resourceMask1,
                    m_Prefab1 = secondData1.m_Entity,
                    m_Prefab2 = secondData.m_Entity,
                    m_Prefab3 = thirdData.m_Entity
                  });
              }
            }
          }
        }
      }
    }

    private void CheckTractors(
      Resource resourceMask,
      UpdateDeliveryTruckSelectJob.TruckData secondData,
      UpdateDeliveryTruckSelectJob.TruckData thirdData,
      UpdateDeliveryTruckSelectJob.TruckData forthData)
    {
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<CarTractorData> nativeArray1 = prefabChunk.GetNativeArray<CarTractorData>(ref this.m_CarTractorDataType);
        if (nativeArray1.Length != 0 && !prefabChunk.Has<CarTrailerData>(ref this.m_CarTrailerDataType))
        {
          NativeArray<Entity> nativeArray2 = prefabChunk.GetNativeArray(this.m_EntityType);
          NativeArray<DeliveryTruckData> nativeArray3 = prefabChunk.GetNativeArray<DeliveryTruckData>(ref this.m_DeliveryTruckDataType);
          VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            UpdateDeliveryTruckSelectJob.TruckData truckData = new UpdateDeliveryTruckSelectJob.TruckData();
            truckData.m_DeliveryTruckData = nativeArray3[index2];
            Resource resource = resourceMask;
            if (truckData.m_DeliveryTruckData.m_CargoCapacity != 0)
            {
              resource &= truckData.m_DeliveryTruckData.m_TransportedResources;
              if (resource == Resource.NoResource)
                continue;
            }
            if (this.m_RequirementData.CheckRequirements(ref chunk, index2))
            {
              truckData.m_Entity = nativeArray2[index2];
              truckData.m_TractorData = nativeArray1[index2];
              if (truckData.m_TractorData.m_TrailerType == secondData.m_TrailerData.m_TrailerType && (!(truckData.m_TractorData.m_FixedTrailer != Entity.Null) || !(truckData.m_TractorData.m_FixedTrailer != secondData.m_Entity)) && (!(secondData.m_TrailerData.m_FixedTractor != Entity.Null) || !(secondData.m_TrailerData.m_FixedTractor != truckData.m_Entity)))
                this.m_DeliveryTruckItems.Add(new DeliveryTruckSelectItem()
                {
                  m_Capacity = truckData.m_DeliveryTruckData.m_CargoCapacity + secondData.m_DeliveryTruckData.m_CargoCapacity + thirdData.m_DeliveryTruckData.m_CargoCapacity + forthData.m_DeliveryTruckData.m_CargoCapacity,
                  m_Cost = truckData.m_DeliveryTruckData.m_CostToDrive + secondData.m_DeliveryTruckData.m_CostToDrive + thirdData.m_DeliveryTruckData.m_CostToDrive + forthData.m_DeliveryTruckData.m_CostToDrive,
                  m_Resources = resource,
                  m_Prefab1 = truckData.m_Entity,
                  m_Prefab2 = secondData.m_Entity,
                  m_Prefab3 = thirdData.m_Entity,
                  m_Prefab4 = forthData.m_Entity
                });
            }
          }
        }
      }
    }

    private struct TruckData
    {
      public Entity m_Entity;
      public DeliveryTruckData m_DeliveryTruckData;
      public CarTrailerData m_TrailerData;
      public CarTractorData m_TractorData;
      public ObjectData m_ObjectData;
    }
  }
}
