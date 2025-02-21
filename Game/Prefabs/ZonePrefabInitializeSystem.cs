// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZonePrefabInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Simulation;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class ZonePrefabInitializeSystem : GameSystemBase
  {
    private EntityQuery m_PrefabGroup;
    private EntityQuery m_ProcessGroup;
    private EntityQuery m_EconomyParameterGroup;
    private PrefabSystem m_PrefabSystem;
    private ResourceSystem m_ResourceSystem;
    private ZonePrefabInitializeSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<ZoneData>()
        },
        Any = new ComponentType[0]
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessGroup = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.ReadOnly<IndustrialCompanyData>(), ComponentType.ReadOnly<WorkplaceData>(), ComponentType.Exclude<StorageCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ProcessGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterGroup);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_PrefabGroup.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabGroup.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      ZonePrefabInitializeSystem.TypeHandle typeHandle = this.__TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ZoneData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_ZoneData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<BuildingPropertyData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ProcessEstimate_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<ProcessEstimate> bufferTypeHandle = this.__TypeHandle.__Game_Zones_ProcessEstimate_RW_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      EconomyParameterData singleton = this.m_EconomyParameterGroup.GetSingleton<EconomyParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ProcessGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<IndustrialProcessData> componentDataArray1 = this.m_ProcessGroup.ToComponentDataArray<IndustrialProcessData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<WorkplaceData> componentDataArray2 = this.m_ProcessGroup.ToComponentDataArray<WorkplaceData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
        NativeArray<ZoneData> nativeArray2 = archetypeChunk.GetNativeArray<ZoneData>(ref componentTypeHandle1);
        NativeArray<BuildingPropertyData> nativeArray3 = archetypeChunk.GetNativeArray<BuildingPropertyData>(ref componentTypeHandle2);
        BufferAccessor<ProcessEstimate> bufferAccessor = archetypeChunk.GetBufferAccessor<ProcessEstimate>(ref bufferTypeHandle);
        if (nativeArray2.Length > 0)
        {
          for (int index2 = 0; index2 < archetypeChunk.Count; ++index2)
          {
            DynamicBuffer<ProcessEstimate> dynamicBuffer = bufferAccessor[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            bool office = this.m_PrefabSystem.GetPrefab<ZonePrefab>(nativeArray1[index2]).m_Office;
            if (office)
            {
              ZoneData zoneData = nativeArray2[index2];
              zoneData.m_ZoneFlags |= ZoneFlags.Office;
              nativeArray2[index2] = zoneData;
            }
            if (nativeArray2[index2].m_AreaType == AreaType.Industrial && !office)
            {
              float num1 = 1f;
              if (nativeArray3.Length > 0)
                num1 = nativeArray3[index2].m_SpaceMultiplier;
              ProcessEstimate processEstimate1;
              for (int index3 = 0; index3 < EconomyUtils.ResourceCount; ++index3)
              {
                ref DynamicBuffer<ProcessEstimate> local = ref dynamicBuffer;
                processEstimate1 = new ProcessEstimate();
                ProcessEstimate elem = processEstimate1;
                local.Add(elem);
              }
              for (int index4 = 0; index4 < componentDataArray1.Length; ++index4)
              {
                IndustrialProcessData industrialProcessData = componentDataArray1[index4];
                int num2 = Mathf.RoundToInt((float) ((double) num1 * (double) industrialProcessData.m_MaxWorkersPerCell * 100.0));
                WorkplaceData workplaceData = componentDataArray2[index4];
                // ISSUE: reference to a compiler-generated method
                Workplaces numberOfWorkplaces = WorkProviderSystem.CalculateNumberOfWorkplaces(num2, workplaceData.m_Complexity, 1);
                float num3 = 0.0f;
                float num4 = 1f;
                for (int index5 = 0; index5 < 5; ++index5)
                {
                  // ISSUE: reference to a compiler-generated method
                  float num5 = (float) numberOfWorkplaces[index5] * WorkProviderSystem.GetWorkerWorkforce(50, index5);
                  if (index5 < 2)
                    num3 += num5;
                  else
                    num4 += num5;
                }
                int resourceIndex = EconomyUtils.GetResourceIndex(industrialProcessData.m_Output.m_Resource);
                new BuildingData().m_LotSize = new int2(10, 10);
                EconomyUtils.BuildPseudoTradeCost(5000f, industrialProcessData, roComponentLookup, prefabs);
                SpawnableBuildingData spawnableBuildingData = new SpawnableBuildingData()
                {
                  m_Level = 1
                };
                float num6 = 1f * (float) EconomyUtils.GetCompanyProductionPerDay(1f, num2, (int) spawnableBuildingData.m_Level, true, workplaceData, industrialProcessData, prefabs, roComponentLookup, ref singleton) / (float) EconomyUtils.kCompanyUpdatesPerDay;
                processEstimate1 = new ProcessEstimate();
                processEstimate1.m_ProductionPerCell = 0.01f * num6;
                processEstimate1.m_WorkerProductionPerCell = 0.01f * num6 / (num1 * industrialProcessData.m_MaxWorkersPerCell);
                processEstimate1.m_LowEducationWeight = num3 / (num3 + num4);
                processEstimate1.m_ProcessEntity = entityArray[index4];
                ProcessEstimate processEstimate2 = processEstimate1;
                dynamicBuffer[resourceIndex] = processEstimate2;
              }
            }
          }
        }
      }
      componentDataArray1.Dispose();
      componentDataArray2.Dispose();
      archetypeChunkArray.Dispose();
      entityArray.Dispose();
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

    [Preserve]
    public ZonePrefabInitializeSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ZoneData> __Game_Prefabs_ZoneData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle;
      public BufferTypeHandle<ProcessEstimate> __Game_Zones_ProcessEstimate_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ZoneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_ProcessEstimate_RW_BufferTypeHandle = state.GetBufferTypeHandle<ProcessEstimate>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
      }
    }
  }
}
