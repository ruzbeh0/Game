// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CountResidentialPropertySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Debug;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CountResidentialPropertySystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private NativeAccumulator<CountResidentialPropertySystem.ResidentialPropertyData> m_ResidentialPropertyData;
    private CountResidentialPropertySystem.ResidentialPropertyData m_LastResidentialPropertyData;
    private EntityQuery m_ResidentialPropertyQuery;
    private CountResidentialPropertySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [DebugWatchValue]
    public int3 FreeProperties => this.m_LastResidentialPropertyData.m_FreeProperties;

    [DebugWatchValue]
    public int3 TotalProperties => this.m_LastResidentialPropertyData.m_TotalProperties;

    public CountResidentialPropertySystem.ResidentialPropertyData GetResidentialPropertyData()
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_LastResidentialPropertyData;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialPropertyQuery = this.GetEntityQuery(ComponentType.ReadOnly<ResidentialProperty>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialPropertyData = new NativeAccumulator<CountResidentialPropertySystem.ResidentialPropertyData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ResidentialPropertyQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialPropertyData.Dispose();
      base.OnDestroy();
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_LastResidentialPropertyData = new CountResidentialPropertySystem.ResidentialPropertyData();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialPropertyData.Clear();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastResidentialPropertyData = this.m_ResidentialPropertyData.GetResult();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialPropertyData.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CountResidentialPropertySystem.CountResidentialPropertyJob jobData = new CountResidentialPropertySystem.CountResidentialPropertyJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_ZonePropertiesDatas = this.__TypeHandle.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_ResidentialPropertyData = this.m_ResidentialPropertyData.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CountResidentialPropertySystem.CountResidentialPropertyJob>(this.m_ResidentialPropertyQuery, this.Dependency);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write<CountResidentialPropertySystem.ResidentialPropertyData>(this.m_LastResidentialPropertyData);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (!(reader.context.version >= Version.homelessFix))
        return;
      // ISSUE: reference to a compiler-generated field
      reader.Read<CountResidentialPropertySystem.ResidentialPropertyData>(out this.m_LastResidentialPropertyData);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public CountResidentialPropertySystem()
    {
    }

    public struct ResidentialPropertyData : 
      IAccumulable<CountResidentialPropertySystem.ResidentialPropertyData>,
      ISerializable
    {
      public int3 m_FreeProperties;
      public int3 m_TotalProperties;

      public void Accumulate(
        CountResidentialPropertySystem.ResidentialPropertyData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_FreeProperties += other.m_FreeProperties;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TotalProperties += other.m_TotalProperties;
      }

      public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_FreeProperties);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_TotalProperties);
      }

      public void Deserialize<TReader>(TReader reader) where TReader : IReader
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_FreeProperties);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_TotalProperties);
      }
    }

    [BurstCompile]
    private struct CountResidentialPropertyJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<ZonePropertiesData> m_ZonePropertiesDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      public NativeAccumulator<CountResidentialPropertySystem.ResidentialPropertyData>.ParallelWriter m_ResidentialPropertyData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CountResidentialPropertySystem.ResidentialPropertyData residentialPropertyData = new CountResidentialPropertySystem.ResidentialPropertyData();
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          if (!chunk.Has<Abandoned>() && !chunk.Has<Condemned>() && !chunk.Has<Destroyed>())
          {
            Entity prefab = nativeArray[index1].m_Prefab;
            ZonePropertiesData componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingPropertyDatas.HasComponent(prefab) && this.m_ZonePropertiesDatas.TryGetComponent(this.m_SpawnableBuildingDatas[prefab].m_ZonePrefab, out componentData))
            {
              float num1 = componentData.m_ResidentialProperties / componentData.m_SpaceMultiplier;
              // ISSUE: reference to a compiler-generated field
              BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[prefab];
              DynamicBuffer<Renter> dynamicBuffer = bufferAccessor[index1];
              int num2 = 0;
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_Households.HasComponent(dynamicBuffer[index2].m_Renter))
                  ++num2;
              }
              if (!componentData.m_ScaleResidentials)
              {
                // ISSUE: reference to a compiler-generated field
                ++residentialPropertyData.m_TotalProperties.x;
                if (chunk.Has<PropertyOnMarket>() || chunk.Has<PropertyToBeOnMarket>())
                {
                  // ISSUE: reference to a compiler-generated field
                  residentialPropertyData.m_FreeProperties.x += 1 - num2;
                }
              }
              else if ((double) num1 < 1.0)
              {
                // ISSUE: reference to a compiler-generated field
                residentialPropertyData.m_TotalProperties.y += buildingPropertyData.m_ResidentialProperties;
                if (chunk.Has<PropertyOnMarket>() || chunk.Has<PropertyToBeOnMarket>())
                {
                  // ISSUE: reference to a compiler-generated field
                  residentialPropertyData.m_FreeProperties.y += buildingPropertyData.m_ResidentialProperties - num2;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                residentialPropertyData.m_TotalProperties.z += buildingPropertyData.m_ResidentialProperties;
                if (chunk.Has<PropertyOnMarket>() || chunk.Has<PropertyToBeOnMarket>())
                {
                  // ISSUE: reference to a compiler-generated field
                  residentialPropertyData.m_FreeProperties.z += buildingPropertyData.m_ResidentialProperties - num2;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ResidentialPropertyData.Accumulate(residentialPropertyData);
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZonePropertiesData> __Game_Prefabs_ZonePropertiesData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZonePropertiesData_RO_ComponentLookup = state.GetComponentLookup<ZonePropertiesData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
      }
    }
  }
}
