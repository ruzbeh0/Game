// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ResourceSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Economy;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class ResourceSystem : GameSystemBase
  {
    private EntityQuery m_PrefabGroup;
    private EntityQuery m_InfoGroup;
    private PrefabSystem m_PrefabSystem;
    private NativeArray<Entity> m_ResourcePrefabs;
    private NativeArray<Entity> m_ResourceInfos;
    private JobHandle m_PrefabsReaders;
    private int m_BaseConsumptionSum;
    private ResourceSystem.TypeHandle __TypeHandle;

    public int BaseConsumptionSum => this.m_BaseConsumptionSum;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<ResourceData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_InfoGroup = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<ResourceInfo>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResourcePrefabs = new NativeArray<Entity>(EconomyUtils.ResourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceInfos = new NativeArray<Entity>(EconomyUtils.ResourceCount, Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourcePrefabs.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceInfos.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PrefabGroup.IsEmptyIgnoreFilter)
      {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer((AllocatorManager.AllocatorHandle) Allocator.Temp);
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
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<ResourceData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_ResourceData_RW_ComponentTypeHandle;
        float f = 0.0f;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
          NativeArray<ResourceData> nativeArray3 = archetypeChunk.GetNativeArray<ResourceData>(ref componentTypeHandle2);
          for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            ResourcePrefab prefab = this.m_PrefabSystem.GetPrefab<ResourcePrefab>(nativeArray2[index2]);
            ResourceData resourceData = nativeArray3[index2] with
            {
              m_IsMaterial = prefab.m_IsMaterial,
              m_IsProduceable = prefab.m_IsProduceable,
              m_IsTradable = prefab.m_IsTradable,
              m_IsLeisure = prefab.m_IsLeisure,
              m_Weight = prefab.m_Weight,
              m_Price = prefab.m_InitialPrice,
              m_WealthModifier = prefab.m_WealthModifier,
              m_BaseConsumption = prefab.m_BaseConsumption,
              m_ChildWeight = prefab.m_ChildWeight,
              m_TeenWeight = prefab.m_TeenWeight,
              m_AdultWeight = prefab.m_AdultWeight,
              m_ElderlyWeight = prefab.m_ElderlyWeight,
              m_CarConsumption = prefab.m_CarConsumption,
              m_RequireTemperature = prefab.m_RequireTemperature,
              m_RequiredTemperature = prefab.m_RequiredTemperature,
              m_RequireNaturalResource = prefab.m_RequireNaturalResource,
              m_NeededWorkPerUnit = prefab.m_NeededWorkPerUnit
            };
            nativeArray3[index2] = resourceData;
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            f += math.lerp((float) HouseholdBehaviorSystem.GetWeight(200, resourceData, 1, false), (float) HouseholdBehaviorSystem.GetWeight(200, resourceData, 0, false), 0.2f);
            int index3 = (int) (prefab.m_Resource - 1);
            // ISSUE: reference to a compiler-generated field
            if (this.m_ResourcePrefabs[index3] == Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ResourcePrefabs[index3] = entity;
              // ISSUE: reference to a compiler-generated field
              this.m_ResourceInfos[index3] = entityCommandBuffer.CreateEntity();
              // ISSUE: reference to a compiler-generated field
              entityCommandBuffer.AddComponent<ResourceInfo>(this.m_ResourceInfos[index3], new ResourceInfo()
              {
                m_Resource = EconomyUtils.GetResource(prefab.m_Resource)
              });
              // ISSUE: reference to a compiler-generated field
              entityCommandBuffer.AddComponent<Created>(this.m_ResourceInfos[index3], new Created());
            }
          }
        }
        entityCommandBuffer.Playback(this.EntityManager);
        entityCommandBuffer.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_BaseConsumptionSum = Mathf.RoundToInt(f);
        archetypeChunkArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_InfoGroup.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray1 = this.m_InfoGroup.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle1 = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_ResourceInfo_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ResourceInfo> componentTypeHandle = this.__TypeHandle.__Game_Economy_ResourceInfo_RO_ComponentTypeHandle;
      for (int index4 = 0; index4 < archetypeChunkArray1.Length; ++index4)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray1[index4];
        NativeArray<Entity> nativeArray4 = archetypeChunk.GetNativeArray(entityTypeHandle1);
        NativeArray<ResourceInfo> nativeArray5 = archetypeChunk.GetNativeArray<ResourceInfo>(ref componentTypeHandle);
        for (int index5 = 0; index5 < nativeArray4.Length; ++index5)
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(nativeArray5[index5].m_Resource);
          // ISSUE: reference to a compiler-generated field
          if (resourceIndex >= 0 && this.m_ResourceInfos[resourceIndex] != nativeArray4[index5])
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ResourceInfos[resourceIndex] = nativeArray4[index5];
          }
        }
      }
      archetypeChunkArray1.Dispose();
    }

    public ResourcePrefabs GetPrefabs() => new ResourcePrefabs(this.m_ResourcePrefabs);

    public void AddPrefabsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders = JobHandle.CombineDependencies(this.m_PrefabsReaders, handle);
    }

    public Entity GetPrefab(Resource resource)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_ResourcePrefabs[EconomyUtils.GetResourceIndex(resource)];
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
    public ResourceSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ResourceData> __Game_Prefabs_ResourceData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResourceInfo> __Game_Economy_ResourceInfo_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ResourceData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_ResourceInfo_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResourceInfo>(true);
      }
    }
  }
}
