// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PoliceCarSelectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Net;
using Game.Objects;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct PoliceCarSelectData
  {
    private NativeList<ArchetypeChunk> m_PrefabChunks;
    private VehicleSelectRequirementData m_RequirementData;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<PoliceCarData> m_PoliceCarType;
    private ComponentTypeHandle<CarData> m_CarType;
    private ComponentTypeHandle<HelicopterData> m_HelicopterType;
    private ComponentLookup<ObjectData> m_ObjectData;
    private ComponentLookup<MovingObjectData> m_MovingObjectData;

    public static EntityQueryDesc GetEntityQueryDesc()
    {
      return new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<PoliceCarData>(),
          ComponentType.ReadOnly<ObjectData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<CarData>(),
          ComponentType.ReadOnly<HelicopterData>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Locked>()
        }
      };
    }

    public PoliceCarSelectData(SystemBase system)
    {
      this.m_PrefabChunks = new NativeList<ArchetypeChunk>();
      this.m_RequirementData = new VehicleSelectRequirementData(system);
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_PoliceCarType = system.GetComponentTypeHandle<PoliceCarData>(true);
      this.m_CarType = system.GetComponentTypeHandle<CarData>(true);
      this.m_HelicopterType = system.GetComponentTypeHandle<HelicopterData>(true);
      this.m_ObjectData = system.GetComponentLookup<ObjectData>(true);
      this.m_MovingObjectData = system.GetComponentLookup<MovingObjectData>(true);
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
      this.m_PoliceCarType.Update(system);
      this.m_CarType.Update(system);
      this.m_HelicopterType.Update(system);
      this.m_ObjectData.Update(system);
      this.m_MovingObjectData.Update(system);
    }

    public void PostUpdate(JobHandle jobHandle) => this.m_PrefabChunks.Dispose(jobHandle);

    public Entity SelectVehicle(
      ref Random random,
      ref PolicePurpose purposeMask,
      RoadTypes roadType)
    {
      return this.GetRandomVehicle(ref random, ref purposeMask, roadType);
    }

    public Entity CreateVehicle(
      EntityCommandBuffer commandBuffer,
      ref Random random,
      Transform transform,
      Entity source,
      Entity prefab,
      ref PolicePurpose purposeMask,
      RoadTypes roadType,
      bool parked)
    {
      if (prefab == Entity.Null)
      {
        prefab = this.GetRandomVehicle(ref random, ref purposeMask, roadType);
        if (prefab == Entity.Null)
          return Entity.Null;
      }
      Entity entity = commandBuffer.CreateEntity(this.GetArchetype(prefab, parked));
      commandBuffer.SetComponent<Transform>(entity, transform);
      commandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef(prefab));
      commandBuffer.SetComponent<PseudoRandomSeed>(entity, new PseudoRandomSeed(ref random));
      if (!parked)
      {
        commandBuffer.AddComponent<TripSource>(entity, new TripSource(source));
        commandBuffer.AddComponent<Unspawned>(entity, new Unspawned());
      }
      return entity;
    }

    public Entity CreateVehicle(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ref Random random,
      Transform transform,
      Entity source,
      Entity prefab,
      ref PolicePurpose purposeMask,
      RoadTypes roadType,
      bool parked)
    {
      if (prefab == Entity.Null)
      {
        prefab = this.GetRandomVehicle(ref random, ref purposeMask, roadType);
        if (prefab == Entity.Null)
          return Entity.Null;
      }
      Entity entity = commandBuffer.CreateEntity(jobIndex, this.GetArchetype(prefab, parked));
      commandBuffer.SetComponent<Transform>(jobIndex, entity, transform);
      commandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(prefab));
      commandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, entity, new PseudoRandomSeed(ref random));
      if (!parked)
      {
        commandBuffer.AddComponent<TripSource>(jobIndex, entity, new TripSource(source));
        commandBuffer.AddComponent<Unspawned>(jobIndex, entity, new Unspawned());
      }
      return entity;
    }

    private EntityArchetype GetArchetype(Entity prefab, bool parked)
    {
      return parked ? this.m_MovingObjectData[prefab].m_StoppedArchetype : this.m_ObjectData[prefab].m_Archetype;
    }

    private Entity GetRandomVehicle(
      ref Random random,
      ref PolicePurpose purposeMask,
      RoadTypes roadType)
    {
      Entity randomVehicle = Entity.Null;
      PolicePurpose policePurpose = (PolicePurpose) 0;
      int totalProbability = 0;
      for (int index1 = 0; index1 < this.m_PrefabChunks.Length; ++index1)
      {
        ArchetypeChunk prefabChunk = this.m_PrefabChunks[index1];
        switch (roadType)
        {
          case RoadTypes.Car:
            if (prefabChunk.Has<CarData>(ref this.m_CarType))
              break;
            continue;
          case RoadTypes.Helicopter:
            if (!prefabChunk.Has<HelicopterData>(ref this.m_HelicopterType))
              continue;
            break;
          default:
            continue;
        }
        NativeArray<Entity> nativeArray1 = prefabChunk.GetNativeArray(this.m_EntityType);
        NativeArray<PoliceCarData> nativeArray2 = prefabChunk.GetNativeArray<PoliceCarData>(ref this.m_PoliceCarType);
        VehicleSelectRequirementData.Chunk chunk = this.m_RequirementData.GetChunk(prefabChunk);
        for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
        {
          PoliceCarData policeCarData = nativeArray2[index2];
          if ((policeCarData.m_PurposeMask & purposeMask) != (PolicePurpose) 0 && this.m_RequirementData.CheckRequirements(ref chunk, index2) && this.PickVehicle(ref random, 100, ref totalProbability))
          {
            randomVehicle = nativeArray1[index2];
            policePurpose = policeCarData.m_PurposeMask;
          }
        }
      }
      purposeMask = policePurpose;
      return randomVehicle;
    }

    private bool PickVehicle(ref Random random, int probability, ref int totalProbability)
    {
      totalProbability += probability;
      return random.NextInt(totalProbability) < probability;
    }
  }
}
