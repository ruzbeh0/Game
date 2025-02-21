// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CompanyInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Economy;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class CompanyInitializeSystem : GameSystemBase
  {
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_CompanyQuery;
    private PrefabSystem m_PrefabSystem;
    private CompanyInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadWrite<BrandData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<CompanyBrandElement>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Deleted> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BrandData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<BrandData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_BrandData_RW_ComponentTypeHandle;
      this.CompleteDependency();
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        if (!archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
        {
          NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle2);
          if (archetypeChunk.Has<BrandData>(ref componentTypeHandle3))
          {
            NativeArray<Entity> nativeArray2 = archetypeChunk.GetNativeArray(entityTypeHandle);
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              Entity brand = nativeArray2[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              BrandPrefab prefab = this.m_PrefabSystem.GetPrefab<BrandPrefab>(nativeArray1[index2]);
              for (int index3 = 0; index3 < prefab.m_Companies.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_PrefabSystem.GetBuffer<CompanyBrandElement>((PrefabBase) prefab.m_Companies[index3], false).Add(new CompanyBrandElement(brand));
              }
            }
          }
        }
      }
      archetypeChunkArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AffiliatedBrandElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CompanyBrandElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CommercialCompanyData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      CompanyInitializeSystem.InitializeAffiliatedBrandsJob jobData = new CompanyInitializeSystem.InitializeAffiliatedBrandsJob()
      {
        m_Chunks = this.m_CompanyQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_CommercialCompanyDataType = this.__TypeHandle.__Game_Prefabs_CommercialCompanyData_RO_ComponentTypeHandle,
        m_StorageCompanyDataType = this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentTypeHandle,
        m_IndustrialProcessDataType = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentTypeHandle,
        m_CompanyBrandElementType = this.__TypeHandle.__Game_Prefabs_CompanyBrandElement_RW_BufferTypeHandle,
        m_AffiliatedBrandElementType = this.__TypeHandle.__Game_Prefabs_AffiliatedBrandElement_RW_BufferTypeHandle
      };
      this.Dependency = jobData.Schedule<CompanyInitializeSystem.InitializeAffiliatedBrandsJob>(outJobHandle);
      // ISSUE: reference to a compiler-generated field
      jobData.m_Chunks.Dispose(this.Dependency);
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
    public CompanyInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeAffiliatedBrandsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentTypeHandle<CommercialCompanyData> m_CommercialCompanyDataType;
      [ReadOnly]
      public ComponentTypeHandle<StorageCompanyData> m_StorageCompanyDataType;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProcessData> m_IndustrialProcessDataType;
      public BufferTypeHandle<CompanyBrandElement> m_CompanyBrandElementType;
      public BufferTypeHandle<AffiliatedBrandElement> m_AffiliatedBrandElementType;

      public void Execute()
      {
        NativeParallelMultiHashMap<sbyte, Entity> parallelMultiHashMap1 = new NativeParallelMultiHashMap<sbyte, Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelMultiHashMap<sbyte, Entity> parallelMultiHashMap2 = new NativeParallelMultiHashMap<sbyte, Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          bool flag1 = chunk.Has<CommercialCompanyData>(ref this.m_CommercialCompanyDataType);
          // ISSUE: reference to a compiler-generated field
          bool flag2 = chunk.Has<StorageCompanyData>(ref this.m_StorageCompanyDataType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<IndustrialProcessData> nativeArray = chunk.GetNativeArray<IndustrialProcessData>(ref this.m_IndustrialProcessDataType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<CompanyBrandElement> bufferAccessor = chunk.GetBufferAccessor<CompanyBrandElement>(ref this.m_CompanyBrandElementType);
          for (int index2 = 0; index2 < bufferAccessor.Length; ++index2)
          {
            DynamicBuffer<CompanyBrandElement> dynamicBuffer = bufferAccessor[index2];
            for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_DeletedData.HasComponent(dynamicBuffer[index3].m_Brand))
                dynamicBuffer.RemoveAtSwapBack(index3--);
            }
          }
          for (int index4 = 0; index4 < nativeArray.Length; ++index4)
          {
            IndustrialProcessData industrialProcessData = nativeArray[index4];
            if (!flag2 && industrialProcessData.m_Input1.m_Resource != Resource.NoResource)
            {
              int resourceIndex = EconomyUtils.GetResourceIndex(industrialProcessData.m_Input1.m_Resource);
              DynamicBuffer<CompanyBrandElement> dynamicBuffer = bufferAccessor[index4];
              for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
                parallelMultiHashMap2.Add((sbyte) resourceIndex, dynamicBuffer[index5].m_Brand);
            }
            if (!flag2 && industrialProcessData.m_Input2.m_Resource != Resource.NoResource)
            {
              int resourceIndex = EconomyUtils.GetResourceIndex(industrialProcessData.m_Input2.m_Resource);
              DynamicBuffer<CompanyBrandElement> dynamicBuffer = bufferAccessor[index4];
              for (int index6 = 0; index6 < dynamicBuffer.Length; ++index6)
                parallelMultiHashMap2.Add((sbyte) resourceIndex, dynamicBuffer[index6].m_Brand);
            }
            if (!flag1 && !flag2 && industrialProcessData.m_Output.m_Resource != Resource.NoResource)
            {
              int resourceIndex = EconomyUtils.GetResourceIndex(industrialProcessData.m_Output.m_Resource);
              DynamicBuffer<CompanyBrandElement> dynamicBuffer = bufferAccessor[index4];
              for (int index7 = 0; index7 < dynamicBuffer.Length; ++index7)
                parallelMultiHashMap1.Add((sbyte) resourceIndex, dynamicBuffer[index7].m_Brand);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index8 = 0; index8 < this.m_Chunks.Length; ++index8)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index8];
          // ISSUE: reference to a compiler-generated field
          bool flag3 = chunk.Has<CommercialCompanyData>(ref this.m_CommercialCompanyDataType);
          // ISSUE: reference to a compiler-generated field
          bool flag4 = chunk.Has<StorageCompanyData>(ref this.m_StorageCompanyDataType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<IndustrialProcessData> nativeArray = chunk.GetNativeArray<IndustrialProcessData>(ref this.m_IndustrialProcessDataType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<AffiliatedBrandElement> bufferAccessor = chunk.GetBufferAccessor<AffiliatedBrandElement>(ref this.m_AffiliatedBrandElementType);
          for (int index9 = 0; index9 < bufferAccessor.Length; ++index9)
          {
            IndustrialProcessData industrialProcessData = nativeArray[index9];
            DynamicBuffer<AffiliatedBrandElement> dynamicBuffer = bufferAccessor[index9];
            dynamicBuffer.Clear();
            if (!flag4 && industrialProcessData.m_Input1.m_Resource != Resource.NoResource)
            {
              int resourceIndex = EconomyUtils.GetResourceIndex(industrialProcessData.m_Input1.m_Resource);
              Entity entity;
              NativeParallelMultiHashMapIterator<sbyte> it;
              if (parallelMultiHashMap1.TryGetFirstValue((sbyte) resourceIndex, out entity, out it))
              {
                do
                {
                  dynamicBuffer.Add(new AffiliatedBrandElement()
                  {
                    m_Brand = entity
                  });
                }
                while (parallelMultiHashMap1.TryGetNextValue(out entity, ref it));
              }
            }
            if (!flag4 && industrialProcessData.m_Input2.m_Resource != Resource.NoResource)
            {
              int resourceIndex = EconomyUtils.GetResourceIndex(industrialProcessData.m_Input2.m_Resource);
              Entity entity;
              NativeParallelMultiHashMapIterator<sbyte> it;
              if (parallelMultiHashMap1.TryGetFirstValue((sbyte) resourceIndex, out entity, out it))
              {
                do
                {
                  dynamicBuffer.Add(new AffiliatedBrandElement()
                  {
                    m_Brand = entity
                  });
                }
                while (parallelMultiHashMap1.TryGetNextValue(out entity, ref it));
              }
            }
            if (!flag3 && industrialProcessData.m_Output.m_Resource != Resource.NoResource)
            {
              int resourceIndex = EconomyUtils.GetResourceIndex(industrialProcessData.m_Output.m_Resource);
              Entity entity;
              NativeParallelMultiHashMapIterator<sbyte> it;
              if (parallelMultiHashMap2.TryGetFirstValue((sbyte) resourceIndex, out entity, out it))
              {
                do
                {
                  dynamicBuffer.Add(new AffiliatedBrandElement()
                  {
                    m_Brand = entity
                  });
                }
                while (parallelMultiHashMap2.TryGetNextValue(out entity, ref it));
              }
              if (flag4 && parallelMultiHashMap1.TryGetFirstValue((sbyte) resourceIndex, out entity, out it))
              {
                do
                {
                  dynamicBuffer.Add(new AffiliatedBrandElement()
                  {
                    m_Brand = entity
                  });
                }
                while (parallelMultiHashMap1.TryGetNextValue(out entity, ref it));
              }
            }
            if (dynamicBuffer.Length >= 3)
              dynamicBuffer.AsNativeArray().Sort<AffiliatedBrandElement>();
            int index10 = 0;
            AffiliatedBrandElement affiliatedBrandElement1 = new AffiliatedBrandElement();
            for (int index11 = 0; index11 < dynamicBuffer.Length; ++index11)
            {
              AffiliatedBrandElement affiliatedBrandElement2 = dynamicBuffer[index11];
              if (affiliatedBrandElement2.m_Brand != affiliatedBrandElement1.m_Brand)
              {
                if (affiliatedBrandElement1.m_Brand != Entity.Null)
                  dynamicBuffer[index10++] = affiliatedBrandElement1;
                affiliatedBrandElement1 = affiliatedBrandElement2;
              }
            }
            if (affiliatedBrandElement1.m_Brand != Entity.Null)
              dynamicBuffer[index10++] = affiliatedBrandElement1;
            if (index10 < dynamicBuffer.Length)
              dynamicBuffer.RemoveRange(index10, dynamicBuffer.Length - index10);
            dynamicBuffer.TrimExcess();
          }
        }
        parallelMultiHashMap1.Dispose();
        parallelMultiHashMap2.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<BrandData> __Game_Prefabs_BrandData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<CommercialCompanyData> __Game_Prefabs_CommercialCompanyData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StorageCompanyData> __Game_Prefabs_StorageCompanyData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentTypeHandle;
      public BufferTypeHandle<CompanyBrandElement> __Game_Prefabs_CompanyBrandElement_RW_BufferTypeHandle;
      public BufferTypeHandle<AffiliatedBrandElement> __Game_Prefabs_AffiliatedBrandElement_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BrandData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BrandData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CommercialCompanyData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CommercialCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageCompanyData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StorageCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CompanyBrandElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<CompanyBrandElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AffiliatedBrandElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<AffiliatedBrandElement>();
      }
    }
  }
}
