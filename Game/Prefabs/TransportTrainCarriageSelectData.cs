// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportTrainCarriageSelectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Economy;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct TransportTrainCarriageSelectData
  {
    private NativeList<ArchetypeChunk> m_PrefabChunks;
    private VehicleSelectRequirementData m_RequirementData;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<CargoTransportVehicleData> m_CargoTransportVehicleType;

    public static EntityQueryDesc GetEntityQueryDesc()
    {
      return new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<CargoTransportVehicleData>(),
          ComponentType.ReadOnly<TrainCarriageData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Locked>()
        }
      };
    }

    public TransportTrainCarriageSelectData(SystemBase system)
    {
      this.m_PrefabChunks = new NativeList<ArchetypeChunk>();
      this.m_RequirementData = new VehicleSelectRequirementData(system);
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_CargoTransportVehicleType = system.GetComponentTypeHandle<CargoTransportVehicleData>(true);
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
      this.m_CargoTransportVehicleType.Update(system);
    }

    public void PostUpdate(JobHandle jobHandle) => this.m_PrefabChunks.Dispose(jobHandle);

    public Entity SelectCarriagePrefab(ref Random random, Resource resource, int amount)
    {
      Entity entity = Entity.Null;
      int num1 = -amount;
      int totalProbability = 0;
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        NativeArray<Entity> nativeArray1 = prefabChunk.GetNativeArray(this.m_EntityType);
        NativeArray<CargoTransportVehicleData> nativeArray2 = prefabChunk.GetNativeArray<CargoTransportVehicleData>(ref this.m_CargoTransportVehicleType);
        VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
        for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
        {
          CargoTransportVehicleData transportVehicleData = nativeArray2[index2];
          if ((transportVehicleData.m_Resources & resource) != Resource.NoResource && this.m_RequirementData.CheckRequirements(ref chunk, index2))
          {
            int num2 = transportVehicleData.m_CargoCapacity - amount;
            if (num2 != num1)
            {
              if ((num2 >= 0 || num1 <= num2) && (num1 < 0 || num1 >= num2))
              {
                num1 = num2;
                totalProbability = 0;
              }
              else
                continue;
            }
            if (this.PickVehicle(ref random, 100, ref totalProbability))
              entity = nativeArray1[index2];
          }
        }
      }
      return entity;
    }

    private bool PickVehicle(ref Random random, int probability, ref int totalProbability)
    {
      totalProbability += probability;
      return random.NextInt(totalProbability) < probability;
    }
  }
}
