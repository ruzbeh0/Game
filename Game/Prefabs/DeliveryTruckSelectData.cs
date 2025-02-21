// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DeliveryTruckSelectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Economy;
using Game.Objects;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct DeliveryTruckSelectData
  {
    private NativeArray<DeliveryTruckSelectItem> m_Items;

    public DeliveryTruckSelectData(NativeArray<DeliveryTruckSelectItem> items)
    {
      this.m_Items = items;
    }

    public void GetCapacityRange(Resource resources, out int min, out int max)
    {
      min = 0;
      max = 0;
      for (int index = 0; index < this.m_Items.Length; ++index)
      {
        DeliveryTruckSelectItem deliveryTruckSelectItem = this.m_Items[index];
        if ((deliveryTruckSelectItem.m_Resources & resources) == resources)
        {
          min = deliveryTruckSelectItem.m_Capacity;
          break;
        }
      }
      for (int index = this.m_Items.Length - 1; index >= 0; --index)
      {
        DeliveryTruckSelectItem deliveryTruckSelectItem = this.m_Items[index];
        if ((deliveryTruckSelectItem.m_Resources & resources) == resources)
        {
          max = deliveryTruckSelectItem.m_Capacity;
          break;
        }
      }
    }

    public bool TrySelectItem(
      ref Random random,
      Resource resources,
      int capacity,
      out DeliveryTruckSelectItem item)
    {
      int2 x;
      int num1;
      DeliveryTruckSelectItem deliveryTruckSelectItem1;
      for (x = new int2(0, this.m_Items.Length); x.y > x.x; x = math.select(new int2(num1 + 1, x.y), new int2(x.x, num1), deliveryTruckSelectItem1.m_Capacity > capacity))
      {
        num1 = math.csum(x) >> 1;
        deliveryTruckSelectItem1 = this.m_Items[num1];
        if (deliveryTruckSelectItem1.m_Capacity == capacity)
        {
          x = (int2) num1;
          break;
        }
      }
      item = new DeliveryTruckSelectItem();
      int num2 = 0;
      while (x.y < this.m_Items.Length)
      {
        DeliveryTruckSelectItem deliveryTruckSelectItem2 = this.m_Items[x.y++];
        int2 int2 = new int2(deliveryTruckSelectItem2.m_Cost, item.m_Cost) * math.min((int2) capacity, new int2(item.m_Capacity, deliveryTruckSelectItem2.m_Capacity));
        if (int2.x <= int2.y)
        {
          bool c = (deliveryTruckSelectItem2.m_Resources & resources) == resources;
          int num3 = math.select(0, 100, c);
          num2 = num3 + math.select(num2, 0, c & int2.x < int2.y);
          if (random.NextInt(num2) < num3)
            item = deliveryTruckSelectItem2;
        }
        else
          break;
      }
      while (x.x > 0)
      {
        DeliveryTruckSelectItem deliveryTruckSelectItem3 = this.m_Items[--x.x];
        int2 int2 = new int2(deliveryTruckSelectItem3.m_Cost, item.m_Cost) * math.min((int2) capacity, new int2(item.m_Capacity, deliveryTruckSelectItem3.m_Capacity));
        if (int2.x <= int2.y)
        {
          bool c = (deliveryTruckSelectItem3.m_Resources & resources) == resources;
          int num4 = math.select(0, 100, c);
          num2 = num4 + math.select(num2, 0, c & int2.x < int2.y);
          if (random.NextInt(num2) < num4)
            item = deliveryTruckSelectItem3;
        }
        else
          break;
      }
      return item.m_Prefab1 != Entity.Null;
    }

    public Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      ref ComponentLookup<DeliveryTruckData> deliveryTruckDatas,
      ref ComponentLookup<ObjectData> objectDatas,
      Resource resource,
      Resource returnResource,
      ref int amount,
      ref int returnAmount,
      Transform transform,
      Entity source,
      DeliveryTruckFlags state,
      uint delay = 0)
    {
      Resource resources = resource | returnResource;
      int capacity = math.max(amount, returnAmount);
      DeliveryTruckSelectItem selectItem;
      return this.TrySelectItem(ref random, resources, capacity, out selectItem) ? this.CreateVehicle(commandBuffer, jobIndex, ref random, ref deliveryTruckDatas, ref objectDatas, selectItem, resource, returnResource, ref amount, ref returnAmount, transform, source, state, delay) : Entity.Null;
    }

    public Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      ref ComponentLookup<DeliveryTruckData> deliveryTruckDatas,
      ref ComponentLookup<ObjectData> objectDatas,
      DeliveryTruckSelectItem selectItem,
      Resource resource,
      Resource returnResource,
      ref int amount,
      ref int returnAmount,
      Transform transform,
      Entity source,
      DeliveryTruckFlags state,
      uint delay = 0)
    {
      int amount1 = amount;
      int returnAmount1 = returnAmount;
      Entity vehicle1 = this.CreateVehicle(commandBuffer, jobIndex, ref random, ref deliveryTruckDatas, ref objectDatas, selectItem.m_Prefab1, resource, returnResource, ref amount1, ref returnAmount1, transform, source, state, delay);
      if (selectItem.m_Prefab2 != Entity.Null)
      {
        DynamicBuffer<LayoutElement> dynamicBuffer = commandBuffer.AddBuffer<LayoutElement>(jobIndex, vehicle1);
        dynamicBuffer.Add(new LayoutElement(vehicle1));
        Entity vehicle2 = this.CreateVehicle(commandBuffer, jobIndex, ref random, ref deliveryTruckDatas, ref objectDatas, selectItem.m_Prefab2, resource, returnResource, ref amount1, ref returnAmount1, transform, source, state & DeliveryTruckFlags.Loaded, delay);
        commandBuffer.SetComponent<Controller>(jobIndex, vehicle2, new Controller(vehicle1));
        dynamicBuffer.Add(new LayoutElement(vehicle2));
        if (selectItem.m_Prefab3 != Entity.Null)
        {
          Entity vehicle3 = this.CreateVehicle(commandBuffer, jobIndex, ref random, ref deliveryTruckDatas, ref objectDatas, selectItem.m_Prefab3, resource, returnResource, ref amount1, ref returnAmount1, transform, source, state & DeliveryTruckFlags.Loaded, delay);
          commandBuffer.SetComponent<Controller>(jobIndex, vehicle3, new Controller(vehicle1));
          dynamicBuffer.Add(new LayoutElement(vehicle3));
        }
        if (selectItem.m_Prefab4 != Entity.Null)
        {
          Entity vehicle4 = this.CreateVehicle(commandBuffer, jobIndex, ref random, ref deliveryTruckDatas, ref objectDatas, selectItem.m_Prefab4, resource, returnResource, ref amount1, ref returnAmount1, transform, source, state & DeliveryTruckFlags.Loaded, delay);
          commandBuffer.SetComponent<Controller>(jobIndex, vehicle4, new Controller(vehicle1));
          dynamicBuffer.Add(new LayoutElement(vehicle4));
        }
      }
      amount -= amount1;
      returnAmount -= returnAmount1;
      return vehicle1;
    }

    private Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      ref ComponentLookup<DeliveryTruckData> deliveryTruckDatas,
      ref ComponentLookup<ObjectData> objectDatas,
      Entity prefab,
      Resource resource,
      Resource returnResource,
      ref int amount,
      ref int returnAmount,
      Transform transform,
      Entity source,
      DeliveryTruckFlags state,
      uint delay)
    {
      DeliveryTruckData deliveryTruckData = deliveryTruckDatas[prefab];
      ObjectData objectData = objectDatas[prefab];
      Game.Vehicles.DeliveryTruck component1 = new Game.Vehicles.DeliveryTruck();
      component1.m_State = state;
      if ((resource & deliveryTruckData.m_TransportedResources) != Resource.NoResource && amount > 0)
      {
        component1.m_Amount = math.min(amount, deliveryTruckData.m_CargoCapacity);
        if (component1.m_Amount > 0)
        {
          component1.m_Resource = resource;
          amount -= component1.m_Amount;
        }
      }
      Entity entity = commandBuffer.CreateEntity(jobIndex, objectData.m_Archetype);
      commandBuffer.SetComponent<Transform>(jobIndex, entity, transform);
      commandBuffer.SetComponent<Game.Vehicles.DeliveryTruck>(jobIndex, entity, component1);
      commandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(prefab));
      commandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, new PseudoRandomSeed(ref random));
      if (source != Entity.Null)
      {
        commandBuffer.AddComponent<TripSource>(jobIndex, entity, new TripSource(source, delay));
        commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
      }
      if ((returnResource & deliveryTruckData.m_TransportedResources) != Resource.NoResource)
      {
        ReturnLoad component2 = new ReturnLoad();
        component2.m_Amount = math.min(returnAmount, deliveryTruckData.m_CargoCapacity);
        if (component2.m_Amount > 0)
        {
          component2.m_Resource = returnResource;
          returnAmount -= component2.m_Amount;
          commandBuffer.AddComponent<ReturnLoad>(jobIndex, entity, component2);
        }
      }
      return entity;
    }
  }
}
