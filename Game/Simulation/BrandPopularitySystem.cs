// Decompiled with JetBrains decompiler
// Type: Game.Simulation.BrandPopularitySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Objects;
using Game.Serialization;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class BrandPopularitySystem : GameSystemBase, IPreDeserialize
  {
    private EntityQuery m_ModifiedQuery;
    private NativeList<BrandPopularitySystem.BrandPopularity> m_BrandPopularity;
    private JobHandle m_Readers;
    public const int kUpdatesPerDay = 128;
    private BrandPopularitySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 2048;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<CompanyData>(),
          ComponentType.ReadOnly<PropertyRenter>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BrandPopularity = new NativeList<BrandPopularitySystem.BrandPopularity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ModifiedQuery);
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_BrandPopularity.Clear();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_BrandPopularity.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_ModifiedQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps = new BrandPopularitySystem.UpdateBrandPopularityJob()
      {
        m_CompanyChunks = archetypeChunkListAsync,
        m_CompanyDataType = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentTypeHandle,
        m_CompanyRentPropertyType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_UnderConstructions = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup,
        m_BrandPopularity = this.m_BrandPopularity
      }.Schedule<BrandPopularitySystem.UpdateBrandPopularityJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, this.m_Readers));
      archetypeChunkListAsync.Dispose(inputDeps);
      this.Dependency = inputDeps;
      // ISSUE: reference to a compiler-generated field
      this.m_Readers = new JobHandle();
    }

    public NativeList<BrandPopularitySystem.BrandPopularity> ReadBrandPopularity(
      out JobHandle dependency)
    {
      dependency = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      return this.m_BrandPopularity;
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
    public BrandPopularitySystem()
    {
    }

    public struct BrandPopularity : IComparable<BrandPopularitySystem.BrandPopularity>
    {
      public Entity m_BrandPrefab;
      public int m_Popularity;

      public int CompareTo(BrandPopularitySystem.BrandPopularity other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return other.m_Popularity - this.m_Popularity;
      }
    }

    [BurstCompile]
    private struct UpdateBrandPopularityJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_CompanyChunks;
      [ReadOnly]
      public ComponentTypeHandle<CompanyData> m_CompanyDataType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_CompanyRentPropertyType;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> m_UnderConstructions;
      public NativeList<BrandPopularitySystem.BrandPopularity> m_BrandPopularity;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BrandPopularity.Clear();
        // ISSUE: reference to a compiler-generated field
        if (this.m_CompanyChunks.Length == 0)
          return;
        NativeParallelHashMap<Entity, int> nativeParallelHashMap = new NativeParallelHashMap<Entity, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_CompanyChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk companyChunk = this.m_CompanyChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<CompanyData> nativeArray1 = companyChunk.GetNativeArray<CompanyData>(ref this.m_CompanyDataType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PropertyRenter> nativeArray2 = companyChunk.GetNativeArray<PropertyRenter>(ref this.m_CompanyRentPropertyType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            CompanyData companyData = nativeArray1[index2];
            PropertyRenter propertyRenter = nativeArray2[index2];
            // ISSUE: reference to a compiler-generated field
            if (companyData.m_Brand != Entity.Null && propertyRenter.m_Property != Entity.Null && !this.m_UnderConstructions.HasComponent(propertyRenter.m_Property))
            {
              int num;
              if (nativeParallelHashMap.TryGetValue(companyData.m_Brand, out num))
              {
                nativeParallelHashMap[companyData.m_Brand] = num + 1;
              }
              else
              {
                nativeParallelHashMap.Add(companyData.m_Brand, 1);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_BrandPopularity.Add(new BrandPopularitySystem.BrandPopularity()
                {
                  m_BrandPrefab = companyData.m_Brand
                });
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_BrandPopularity.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          BrandPopularitySystem.BrandPopularity brandPopularity = this.m_BrandPopularity[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          brandPopularity.m_Popularity = nativeParallelHashMap[brandPopularity.m_BrandPrefab];
          // ISSUE: reference to a compiler-generated field
          this.m_BrandPopularity[index] = brandPopularity;
        }
        nativeParallelHashMap.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_BrandPopularity.Sort<BrandPopularitySystem.BrandPopularity>();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<CompanyData> __Game_Companies_CompanyData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentLookup = state.GetComponentLookup<UnderConstruction>(true);
      }
    }
  }
}
