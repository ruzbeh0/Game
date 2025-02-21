// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.WorkplacesInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class WorkplacesInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "workplaces";
    private EntityQuery m_WorkplaceQuery;
    private EntityQuery m_WorkplaceModifiedQuery;
    private GetterValueBinding<EmploymentData> m_EmployeesData;
    private GetterValueBinding<EmploymentData> m_WorkplacesData;
    private GetterValueBinding<int> m_Workplaces;
    private GetterValueBinding<int> m_Workers;
    private NativeArray<int> m_IntResults;
    private NativeArray<EmploymentData> m_EmploymentDataResults;
    private WorkplacesInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkplaceQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Employee>(),
          ComponentType.ReadOnly<WorkProvider>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<PropertyRenter>(),
          ComponentType.ReadOnly<Building>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_WorkplaceModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Employee>(),
          ComponentType.ReadOnly<WorkProvider>(),
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
      this.m_IntResults = new NativeArray<int>(2, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_EmploymentDataResults = new NativeArray<EmploymentData>(2, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_WorkplacesData = new GetterValueBinding<EmploymentData>("workplaces", "workplacesData", (Func<EmploymentData>) (() => !this.m_EmploymentDataResults.IsCreated || this.m_EmploymentDataResults.Length != 2 ? new EmploymentData() : this.m_EmploymentDataResults[0]), (IWriter<EmploymentData>) new ValueWriter<EmploymentData>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_EmployeesData = new GetterValueBinding<EmploymentData>("workplaces", "employeesData", (Func<EmploymentData>) (() => !this.m_EmploymentDataResults.IsCreated || this.m_EmploymentDataResults.Length != 2 ? new EmploymentData() : this.m_EmploymentDataResults[1]), (IWriter<EmploymentData>) new ValueWriter<EmploymentData>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Workplaces = new GetterValueBinding<int>("workplaces", "workplaces", (Func<int>) (() => !this.m_IntResults.IsCreated || this.m_IntResults.Length != 2 ? 0 : this.m_IntResults[0]))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Workers = new GetterValueBinding<int>("workplaces", "employees", (Func<int>) (() => !this.m_IntResults.IsCreated || this.m_IntResults.Length != 2 ? 0 : this.m_IntResults[1]))));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_IntResults.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_EmploymentDataResults.Dispose();
      base.OnDestroy();
    }

    protected override bool Active
    {
      get => base.Active || this.m_EmployeesData.active || this.m_WorkplacesData.active;
    }

    protected override bool Modified => !this.m_WorkplaceModifiedQuery.IsEmptyIgnoreFilter;

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.ResetResults();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new WorkplacesInfoviewUISystem.CalculateWorkplaceDataJob()
      {
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EmployeeHandle = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
        m_WorkProviderHandle = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
        m_PropertyRenterHandle = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_PrefabRefHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_WorkplaceDataFromEntity = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_SpawnableBuildingFromEntity = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_IntResults = this.m_IntResults,
        m_EmploymentDataResults = this.m_EmploymentDataResults
      }.Schedule<WorkplacesInfoviewUISystem.CalculateWorkplaceDataJob>(this.m_WorkplaceQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_EmployeesData.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkplacesData.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_Workplaces.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_Workers.Update();
    }

    private void ResetResults()
    {
      for (int index = 0; index < 2; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_EmploymentDataResults[index] = new EmploymentData();
        // ISSUE: reference to a compiler-generated field
        this.m_IntResults[index] = 0;
      }
    }

    private int GetWorkplaces()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return !this.m_IntResults.IsCreated || this.m_IntResults.Length != 2 ? 0 : this.m_IntResults[0];
    }

    private int GetWorkers()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return !this.m_IntResults.IsCreated || this.m_IntResults.Length != 2 ? 0 : this.m_IntResults[1];
    }

    private EmploymentData GetWorkplacesData()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return !this.m_EmploymentDataResults.IsCreated || this.m_EmploymentDataResults.Length != 2 ? new EmploymentData() : this.m_EmploymentDataResults[0];
    }

    private EmploymentData GetEmployeesData()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return !this.m_EmploymentDataResults.IsCreated || this.m_EmploymentDataResults.Length != 2 ? new EmploymentData() : this.m_EmploymentDataResults[1];
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
    public WorkplacesInfoviewUISystem()
    {
    }

    private enum Result
    {
      Workplaces,
      Employees,
      Count,
    }

    [BurstCompile]
    private struct CalculateWorkplaceDataJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDataFromEntity;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingFromEntity;
      public NativeArray<int> m_IntResults;
      public NativeArray<EmploymentData> m_EmploymentDataResults;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray3 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray4 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderHandle);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeHandle);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          int buildingLevel = 1;
          WorkProvider workProvider = nativeArray4[index];
          DynamicBuffer<Employee> employees = bufferAccessor[index];
          // ISSUE: reference to a compiler-generated field
          WorkplaceData workplaceData = this.m_WorkplaceDataFromEntity[nativeArray2[index].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<PropertyRenter>(ref this.m_PropertyRenterHandle))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefFromEntity[nativeArray3[index].m_Property];
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnableBuildingFromEntity.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              buildingLevel = (int) this.m_SpawnableBuildingFromEntity[prefabRef.m_Prefab].m_Level;
            }
          }
          EmploymentData workplacesData = EmploymentData.GetWorkplacesData(workProvider.m_MaxWorkers, buildingLevel, workplaceData.m_Complexity);
          EmploymentData employeesData = EmploymentData.GetEmployeesData(employees, workplacesData.total - employees.Length);
          // ISSUE: reference to a compiler-generated field
          this.m_IntResults[0] += workplacesData.total;
          // ISSUE: reference to a compiler-generated field
          this.m_IntResults[1] += employees.Length;
          // ISSUE: reference to a compiler-generated field
          this.m_EmploymentDataResults[0] += workplacesData;
          // ISSUE: reference to a compiler-generated field
          this.m_EmploymentDataResults[1] += employeesData;
        }
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
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
      }
    }
  }
}
