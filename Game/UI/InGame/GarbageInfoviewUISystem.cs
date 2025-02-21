// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.GarbageInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class GarbageInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "garbageInfo";
    private GarbageAccumulationSystem m_GarbageAccumulationSystem;
    private GetterValueBinding<int> m_Capacity;
    private GetterValueBinding<int> m_StoredGarbage;
    private GetterValueBinding<float> m_ProcessingRate;
    private GetterValueBinding<float> m_GarbageRate;
    private GetterValueBinding<IndicatorValue> m_ProcessingAvailability;
    private GetterValueBinding<IndicatorValue> m_LandfillAvailability;
    private EntityQuery m_GarbageFacilityQuery;
    private EntityQuery m_GarbageFacilityModifiedQuery;
    private NativeArray<float> m_Results;
    private GarbageInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageAccumulationSystem = this.World.GetOrCreateSystemManaged<GarbageAccumulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageFacilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.GarbageFacility>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageFacilityModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Buildings.GarbageFacility>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<ServiceDispatch>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Capacity = new GetterValueBinding<int>("garbageInfo", "capacity", (Func<int>) (() => (int) this.m_Results[1]))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_StoredGarbage = new GetterValueBinding<int>("garbageInfo", "storedGarbage", (Func<int>) (() => (int) this.m_Results[2]))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ProcessingRate = new GetterValueBinding<float>("garbageInfo", "processingRate", (Func<float>) (() => this.m_Results[0]))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_GarbageRate = new GetterValueBinding<float>("garbageInfo", "productionRate", (Func<float>) (() => (float) this.m_GarbageAccumulationSystem.garbageAccumulation))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ProcessingAvailability = new GetterValueBinding<IndicatorValue>("garbageInfo", "processingAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate(this.m_Results[0], (float) math.max(this.m_GarbageAccumulationSystem.garbageAccumulation, 0L))), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_LandfillAvailability = new GetterValueBinding<IndicatorValue>("garbageInfo", "landfillAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate(this.m_Results[1], this.m_Results[2], 0.0f)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<float>(3, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_Capacity.active || this.m_StoredGarbage.active || this.m_ProcessingRate.active || this.m_GarbageRate.active || this.m_ProcessingAvailability.active || this.m_LandfillAvailability.active;
      }
    }

    protected override bool Modified => !this.m_GarbageFacilityModifiedQuery.IsEmptyIgnoreFilter;

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.ResetResults();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Storage_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      new GarbageInfoviewUISystem.UpdateGarbageJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingEfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_Storages = this.__TypeHandle.__Game_Areas_Storage_RO_ComponentLookup,
        m_Geometries = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup,
        m_GarbageFacilities = this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup,
        m_StorageAreaDatas = this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_Results = this.m_Results
      }.Schedule<GarbageInfoviewUISystem.UpdateGarbageJob>(this.m_GarbageFacilityQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_Capacity.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_StoredGarbage.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingRate.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageRate.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_LandfillAvailability.Update();
    }

    private void ResetResults()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0.0f;
      }
    }

    private int GetGarbageCapacity() => (int) this.m_Results[1];

    private int GetStoredGarbage() => (int) this.m_Results[2];

    private float GetProcessingRate() => this.m_Results[0];

    private IndicatorValue GetLandfillAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate(this.m_Results[1], this.m_Results[2], 0.0f);
    }

    private IndicatorValue GetProcessingAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate(this.m_Results[0], (float) math.max(this.m_GarbageAccumulationSystem.garbageAccumulation, 0L));
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
    public GarbageInfoviewUISystem()
    {
    }

    private enum Result
    {
      ProcessingRate,
      GarbageCapacity,
      StoredGarbage,
      ResultCount,
    }

    [BurstCompile]
    private struct UpdateGarbageJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_BuildingEfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentLookup<Storage> m_Storages;
      [ReadOnly]
      public ComponentLookup<Geometry> m_Geometries;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> m_GarbageFacilities;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> m_StorageAreaDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_BuildingEfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          PrefabRef prefabRef = nativeArray2[index1];
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index1);
          if ((double) efficiency != 0.0)
          {
            GarbageFacilityData data = new GarbageFacilityData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarbageFacilities.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              data = this.m_GarbageFacilities[prefabRef.m_Prefab];
            }
            if (bufferAccessor2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<GarbageFacilityData>(ref data, bufferAccessor2[index1], ref this.m_Prefabs, ref this.m_GarbageFacilities);
            }
            DynamicBuffer<Game.Economy.Resources> bufferData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Resources.TryGetBuffer(entity, out bufferData1))
              num4 += (float) EconomyUtils.GetResources(Resource.Garbage, bufferData1);
            DynamicBuffer<Game.Areas.SubArea> bufferData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubAreas.TryGetBuffer(entity, out bufferData2))
            {
              for (int index2 = 0; index2 < bufferData2.Length; ++index2)
              {
                Entity area = bufferData2[index2].m_Area;
                // ISSUE: reference to a compiler-generated field
                Entity prefab = this.m_Prefabs[area].m_Prefab;
                Storage componentData1;
                StorageAreaData componentData2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Storages.TryGetComponent(area, out componentData1) && this.m_StorageAreaDatas.TryGetComponent(prefab, out componentData2))
                {
                  // ISSUE: reference to a compiler-generated field
                  Geometry geometry = this.m_Geometries[area];
                  data.m_GarbageCapacity += AreaUtils.CalculateStorageCapacity(geometry, componentData2);
                  num4 += (float) componentData1.m_Amount;
                }
              }
            }
            num1 += efficiency * (float) data.m_ProcessingSpeed;
            num2 += data.m_LongTermStorage ? (float) data.m_GarbageCapacity : 0.0f;
            num3 += data.m_LongTermStorage ? num4 : 0.0f;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += num2;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += num3;
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Storage> __Game_Areas_Storage_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> __Game_Prefabs_GarbageFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> __Game_Prefabs_StorageAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Storage_RO_ComponentLookup = state.GetComponentLookup<Storage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup = state.GetComponentLookup<GarbageFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageAreaData_RO_ComponentLookup = state.GetComponentLookup<StorageAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
      }
    }
  }
}
