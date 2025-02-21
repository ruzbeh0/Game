// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CountWorkplacesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CountWorkplacesSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private EntityQuery m_WorkplaceQuery;
    private NativeAccumulator<Workplaces> m_FreeWorkplaces;
    private NativeAccumulator<Workplaces> m_TotalWorkplaces;
    public Workplaces m_LastFreeWorkplaces;
    public Workplaces m_LastTotalWorkplaces;
    private CountWorkplacesSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public Workplaces GetFreeWorkplaces() => this.m_LastFreeWorkplaces;

    public Workplaces GetUnemployedWorkspaceByLevel()
    {
      Workplaces workspaceByLevel = new Workplaces();
      int num = 0;
      for (int index = 0; index < 5; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        num += this.m_LastFreeWorkplaces[index];
        workspaceByLevel[index] = num;
      }
      return workspaceByLevel;
    }

    public Workplaces GetTotalWorkplaces() => this.m_LastTotalWorkplaces;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkplaceQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<WorkProvider>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<PropertyRenter>(),
          ComponentType.ReadOnly<Building>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_FreeWorkplaces = new NativeAccumulator<Workplaces>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TotalWorkplaces = new NativeAccumulator<Workplaces>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_WorkplaceQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_FreeWorkplaces.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TotalWorkplaces.Dispose();
      base.OnDestroy();
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreeWorkplaces = new Workplaces();
      // ISSUE: reference to a compiler-generated field
      this.m_LastTotalWorkplaces = new Workplaces();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeWorkplaces.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_TotalWorkplaces.Clear();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write<Workplaces>(this.m_LastFreeWorkplaces);
      // ISSUE: reference to a compiler-generated field
      writer.Write<Workplaces>(this.m_LastTotalWorkplaces);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.economyFix)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read<Workplaces>(out this.m_LastFreeWorkplaces);
        // ISSUE: reference to a compiler-generated field
        reader.Read<Workplaces>(out this.m_LastTotalWorkplaces);
      }
      else
      {
        NativeArray<int> nativeArray = new NativeArray<int>(5, Allocator.Temp);
        reader.Read(nativeArray);
        nativeArray.Dispose();
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastFreeWorkplaces = this.m_FreeWorkplaces.GetResult();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastTotalWorkplaces = this.m_TotalWorkplaces.GetResult();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeWorkplaces.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_TotalWorkplaces.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_FreeWorkplaces_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CountWorkplacesSystem.CountWorkplacesJob jobData = new CountWorkplacesSystem.CountWorkplacesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_FreeWorkplacesType = this.__TypeHandle.__Game_Companies_FreeWorkplaces_RO_ComponentTypeHandle,
        m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_SpawnableBuildings = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_FreeWorkplaces = this.m_FreeWorkplaces.AsParallelWriter(),
        m_TotalWorkplaces = this.m_TotalWorkplaces.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CountWorkplacesSystem.CountWorkplacesJob>(this.m_WorkplaceQuery, this.Dependency);
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
    public CountWorkplacesSystem()
    {
    }

    [BurstCompile]
    private struct CountWorkplacesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<FreeWorkplaces> m_FreeWorkplacesType;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildings;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      public NativeAccumulator<Workplaces>.ParallelWriter m_FreeWorkplaces;
      public NativeAccumulator<Workplaces>.ParallelWriter m_TotalWorkplaces;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        Workplaces workplaces = new Workplaces();
        if (chunk.Has<FreeWorkplaces>())
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<FreeWorkplaces> nativeArray = chunk.GetNativeArray<FreeWorkplaces>(ref this.m_FreeWorkplacesType);
          for (int index1 = 0; index1 < nativeArray.Length; ++index1)
          {
            for (int index2 = 0; index2 < 5; ++index2)
              workplaces[index2] += (int) nativeArray[index1].GetFree(index2);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_FreeWorkplaces.Accumulate(workplaces);
        }
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray2 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          Entity prefab1 = this.m_Prefabs[entity].m_Prefab;
          int buildingLevel = 1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PropertyRenters.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Entity property = this.m_PropertyRenters[entity].m_Property;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Prefabs.HasComponent(property))
            {
              // ISSUE: reference to a compiler-generated field
              Entity prefab2 = this.m_Prefabs[property].m_Prefab;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnableBuildings.HasComponent(prefab2))
              {
                // ISSUE: reference to a compiler-generated field
                buildingLevel = (int) this.m_SpawnableBuildings[prefab2].m_Level;
              }
            }
            else
              continue;
          }
          // ISSUE: reference to a compiler-generated field
          WorkplaceData workplaceData = this.m_WorkplaceDatas[prefab1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TotalWorkplaces.Accumulate(WorkProviderSystem.CalculateNumberOfWorkplaces(nativeArray2[index].m_MaxWorkers, workplaceData.m_Complexity, buildingLevel));
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
      public ComponentTypeHandle<FreeWorkplaces> __Game_Companies_FreeWorkplaces_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_FreeWorkplaces_RO_ComponentTypeHandle = state.GetComponentTypeHandle<FreeWorkplaces>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
      }
    }
  }
}
