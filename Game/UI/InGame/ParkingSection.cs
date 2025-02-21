// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ParkingSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Net;
using Game.Prefabs;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ParkingSection : InfoSectionBase
  {
    protected override string group => nameof (ParkingSection);

    private int parkingFee { get; set; }

    private int parkedCars { get; set; }

    private int parkingCapacity { get; set; }

    protected override void Reset()
    {
      this.parkingFee = 0;
      this.parkedCars = 0;
      this.parkingCapacity = 0;
    }

    private bool Visible() => this.EntityManager.HasComponent<Game.Buildings.ParkingFacility>(this.selectedEntity);

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      int laneCount = 0;
      DynamicBuffer<Game.Net.SubLane> buffer1;
      if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(this.selectedEntity, true, out buffer1))
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckParkingLanes(buffer1, ref laneCount);
      }
      DynamicBuffer<Game.Net.SubNet> buffer2;
      if (this.EntityManager.TryGetBuffer<Game.Net.SubNet>(this.selectedEntity, true, out buffer2))
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckParkingLanes(buffer2, ref laneCount);
      }
      DynamicBuffer<Game.Objects.SubObject> buffer3;
      if (this.EntityManager.TryGetBuffer<Game.Objects.SubObject>(this.selectedEntity, true, out buffer3))
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckParkingLanes(buffer3, ref laneCount);
      }
      if (laneCount != 0)
        this.parkingFee /= laneCount;
      if (this.parkingCapacity >= 0)
        return;
      this.parkingCapacity = 0;
    }

    private void CheckParkingLanes(DynamicBuffer<Game.Objects.SubObject> subObjects, ref int laneCount)
    {
      for (int index = 0; index < subObjects.Length; ++index)
      {
        Entity subObject = subObjects[index].m_SubObject;
        DynamicBuffer<Game.Net.SubLane> buffer1;
        if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(subObject, true, out buffer1))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer1, ref laneCount);
        }
        DynamicBuffer<Game.Objects.SubObject> buffer2;
        if (this.EntityManager.TryGetBuffer<Game.Objects.SubObject>(subObject, true, out buffer2))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer2, ref laneCount);
        }
      }
    }

    private void CheckParkingLanes(DynamicBuffer<Game.Net.SubNet> subNets, ref int laneCount)
    {
      for (int index = 0; index < subNets.Length; ++index)
      {
        DynamicBuffer<Game.Net.SubLane> buffer;
        if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(subNets[index].m_SubNet, true, out buffer))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingLanes(buffer, ref laneCount);
        }
      }
    }

    private void CheckParkingLanes(DynamicBuffer<Game.Net.SubLane> subLanes, ref int laneCount)
    {
      for (int index1 = 0; index1 < subLanes.Length; ++index1)
      {
        Entity subLane = subLanes[index1].m_SubLane;
        Game.Net.ParkingLane component1;
        if (this.EntityManager.TryGetComponent<Game.Net.ParkingLane>(subLane, out component1))
        {
          if ((component1.m_Flags & ParkingLaneFlags.VirtualLane) == (ParkingLaneFlags) 0)
          {
            EntityManager entityManager = this.EntityManager;
            Entity prefab = entityManager.GetComponentData<PrefabRef>(subLane).m_Prefab;
            entityManager = this.EntityManager;
            Curve componentData1 = entityManager.GetComponentData<Curve>(subLane);
            entityManager = this.EntityManager;
            DynamicBuffer<LaneObject> buffer = entityManager.GetBuffer<LaneObject>(subLane, true);
            entityManager = this.EntityManager;
            ParkingLaneData componentData2 = entityManager.GetComponentData<ParkingLaneData>(prefab);
            if ((double) componentData2.m_SlotInterval != 0.0)
              this.parkingCapacity += NetUtils.GetParkingSlotCount(componentData1, component1, componentData2);
            else
              this.parkingCapacity = -1000000;
            for (int index2 = 0; index2 < buffer.Length; ++index2)
            {
              entityManager = this.EntityManager;
              if (entityManager.HasComponent<ParkedCar>(buffer[index2].m_LaneObject))
                ++this.parkedCars;
            }
            this.parkingFee += (int) component1.m_ParkingFee;
            ++laneCount;
          }
        }
        else
        {
          GarageLane component2;
          if (this.EntityManager.TryGetComponent<GarageLane>(subLane, out component2))
          {
            this.parkingCapacity += (int) component2.m_VehicleCapacity;
            this.parkedCars += (int) component2.m_VehicleCount;
            this.parkingFee += (int) component2.m_ParkingFee;
            ++laneCount;
          }
        }
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("parkedCars");
      writer.Write(this.parkedCars);
      writer.PropertyName("parkingCapacity");
      writer.Write(this.parkingCapacity);
    }

    [Preserve]
    public ParkingSection()
    {
    }
  }
}
