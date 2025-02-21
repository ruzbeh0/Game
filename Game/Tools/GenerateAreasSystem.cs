// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateAreasSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Areas;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateAreasSystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_DeletedQuery;
    private GenerateAreasSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreationDefinition>(), ComponentType.ReadOnly<Node>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync1 = this.m_DefinitionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync2 = this.m_DeletedQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Storage_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new GenerateAreasSystem.CreateAreasJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_OwnerDefinitionType = this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_LocalNodeCacheType = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle,
        m_StorageData = this.__TypeHandle.__Game_Areas_Storage_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_AreaData = this.__TypeHandle.__Game_Prefabs_AreaData_RO_ComponentLookup,
        m_AreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_LocalNodeCache = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup,
        m_PrefabSubAreas = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup,
        m_DefinitionChunks = archetypeChunkListAsync1,
        m_DeletedChunks = archetypeChunkListAsync2,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<GenerateAreasSystem.CreateAreasJob>(JobUtils.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2, deps));
      archetypeChunkListAsync1.Dispose(jobHandle);
      archetypeChunkListAsync2.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public GenerateAreasSystem()
    {
    }

    private struct OldAreaData : IEquatable<GenerateAreasSystem.OldAreaData>
    {
      public Entity m_Prefab;
      public Entity m_Original;
      public Entity m_Owner;

      public bool Equals(GenerateAreasSystem.OldAreaData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Prefab.Equals(other.m_Prefab) && this.m_Original.Equals(other.m_Original) && this.m_Owner.Equals(other.m_Owner);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return ((17 * 31 + this.m_Prefab.GetHashCode()) * 31 + this.m_Original.GetHashCode()) * 31 + this.m_Owner.GetHashCode();
      }
    }

    [BurstCompile]
    private struct CreateAreasJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> m_OwnerDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public BufferTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<LocalNodeCache> m_LocalNodeCacheType;
      [ReadOnly]
      public ComponentLookup<Storage> m_StorageData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AreaData> m_AreaData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_AreaGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> m_LocalNodeCache;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DefinitionChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DeletedChunks;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeParallelMultiHashMap<GenerateAreasSystem.OldAreaData, Entity> deletedAreas = new NativeParallelMultiHashMap<GenerateAreasSystem.OldAreaData, Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_DeletedChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FillDeletedAreas(this.m_DeletedChunks[index], deletedAreas);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_DefinitionChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateAreas(this.m_DefinitionChunks[index], deletedAreas);
        }
        deletedAreas.Dispose();
      }

      private void FillDeletedAreas(
        ArchetypeChunk chunk,
        NativeParallelMultiHashMap<GenerateAreasSystem.OldAreaData, Entity> deletedAreas)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          GenerateAreasSystem.OldAreaData key = new GenerateAreasSystem.OldAreaData()
          {
            m_Prefab = nativeArray4[index].m_Prefab,
            m_Original = nativeArray2[index].m_Original
          };
          if (nativeArray3.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            key.m_Owner = nativeArray3[index].m_Owner;
          }
          deletedAreas.Add(key, entity);
        }
      }

      private void CreateAreas(
        ArchetypeChunk chunk,
        NativeParallelMultiHashMap<GenerateAreasSystem.OldAreaData, Entity> deletedAreas)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray1 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<OwnerDefinition> nativeArray2 = chunk.GetNativeArray<OwnerDefinition>(ref this.m_OwnerDefinitionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Node> bufferAccessor1 = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LocalNodeCache> bufferAccessor2 = chunk.GetBufferAccessor<LocalNodeCache>(ref this.m_LocalNodeCacheType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          CreationDefinition creationDefinition = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(creationDefinition.m_Owner))
          {
            OwnerDefinition component = new OwnerDefinition();
            if (nativeArray2.Length != 0)
              component = nativeArray2[index1];
            DynamicBuffer<Node> dynamicBuffer1 = bufferAccessor1[index1];
            AreaFlags flags1 = (AreaFlags) 0;
            TempFlags flags2 = (TempFlags) 0;
            if (creationDefinition.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Hidden>(creationDefinition.m_Original, new Hidden());
              // ISSUE: reference to a compiler-generated field
              creationDefinition.m_Prefab = this.m_PrefabRefData[creationDefinition.m_Original].m_Prefab;
              if ((creationDefinition.m_Flags & CreationFlags.Recreate) != (CreationFlags) 0)
              {
                flags2 |= TempFlags.Modify;
              }
              else
              {
                flags1 |= AreaFlags.Complete;
                if ((creationDefinition.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
                  flags2 |= TempFlags.Delete;
                else if ((creationDefinition.m_Flags & CreationFlags.Select) != (CreationFlags) 0)
                  flags2 |= TempFlags.Select;
                else if ((creationDefinition.m_Flags & CreationFlags.Relocate) != (CreationFlags) 0)
                  flags2 |= TempFlags.Modify;
              }
            }
            else
              flags2 |= TempFlags.Create;
            if (component.m_Prefab == Entity.Null)
              flags2 |= TempFlags.Essential;
            if ((creationDefinition.m_Flags & CreationFlags.Hidden) != (CreationFlags) 0)
              flags2 |= TempFlags.Hidden;
            bool flag1 = false;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            GenerateAreasSystem.OldAreaData oldAreaData = new GenerateAreasSystem.OldAreaData();
            // ISSUE: reference to a compiler-generated field
            oldAreaData.m_Prefab = creationDefinition.m_Prefab;
            // ISSUE: reference to a compiler-generated field
            oldAreaData.m_Original = creationDefinition.m_Original;
            // ISSUE: reference to a compiler-generated field
            oldAreaData.m_Owner = creationDefinition.m_Owner;
            // ISSUE: variable of a compiler-generated type
            GenerateAreasSystem.OldAreaData key1 = oldAreaData;
            Entity entity1;
            NativeParallelMultiHashMapIterator<GenerateAreasSystem.OldAreaData> it1;
            if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && deletedAreas.TryGetFirstValue(key1, out entity1, out it1))
            {
              deletedAreas.Remove(it1);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Temp>(entity1, new Temp(creationDefinition.m_Original, flags2));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Deleted>(entity1);
              if (component.m_Prefab != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Owner>(entity1, new Owner());
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity1, component);
              }
              else
              {
                if (creationDefinition.m_Owner != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Owner>(entity1, new Owner(creationDefinition.m_Owner));
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Owner>(entity1);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<OwnerDefinition>(entity1);
              }
              // ISSUE: reference to a compiler-generated field
              if ((creationDefinition.m_Flags & CreationFlags.Native) != (CreationFlags) 0 || this.m_NativeData.HasComponent(creationDefinition.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Native>(entity1, new Native());
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Native>(entity1);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              entity1 = this.m_CommandBuffer.CreateEntity(this.m_AreaData[creationDefinition.m_Prefab].m_Archetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PrefabRef>(entity1, new PrefabRef(creationDefinition.m_Prefab));
              if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Temp>(entity1, new Temp(creationDefinition.m_Original, flags2));
              }
              if (component.m_Prefab != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Owner>(entity1, new Owner());
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity1, component);
              }
              else if (creationDefinition.m_Owner != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Owner>(entity1, new Owner(creationDefinition.m_Owner));
              }
              // ISSUE: reference to a compiler-generated field
              if ((creationDefinition.m_Flags & CreationFlags.Native) != (CreationFlags) 0 || this.m_NativeData.HasComponent(creationDefinition.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Native>(entity1, new Native());
              }
              flag1 = true;
            }
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Node> dynamicBuffer2 = this.m_CommandBuffer.SetBuffer<Node>(entity1);
            bool flag2 = false;
            if ((flags1 & AreaFlags.Complete) == (AreaFlags) 0 && dynamicBuffer1.Length >= 4 && dynamicBuffer1[0].m_Position.Equals(dynamicBuffer1[dynamicBuffer1.Length - 1].m_Position))
            {
              dynamicBuffer2.ResizeUninitialized(dynamicBuffer1.Length - 1);
              for (int index2 = 0; index2 < dynamicBuffer1.Length - 1; ++index2)
                dynamicBuffer2[index2] = dynamicBuffer1[index2];
              flags1 |= AreaFlags.Complete;
              flag2 = true;
            }
            else
            {
              dynamicBuffer2.ResizeUninitialized(dynamicBuffer1.Length);
              for (int index3 = 0; index3 < dynamicBuffer1.Length; ++index3)
                dynamicBuffer2[index3] = dynamicBuffer1[index3];
            }
            bool flag3 = false;
            bool flag4 = false;
            AreaGeometryData componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AreaGeometryData.TryGetComponent(creationDefinition.m_Prefab, out componentData1))
            {
              flag3 = (componentData1.m_Flags & GeometryFlags.OnWaterSurface) != 0;
              flag4 = (componentData1.m_Flags & GeometryFlags.PseudoRandom) != 0;
            }
            for (int index4 = 0; index4 < dynamicBuffer2.Length; ++index4)
            {
              ref Node local = ref dynamicBuffer2.ElementAt(index4);
              if ((double) local.m_Elevation == -3.4028234663852886E+38)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Node node = !flag3 ? AreaUtils.AdjustPosition(local, ref this.m_TerrainHeightData) : AreaUtils.AdjustPosition(local, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData);
                bool c = (double) math.abs(node.m_Position.y - local.m_Position.y) >= 0.0099999997764825821;
                local.m_Position = math.select(local.m_Position, node.m_Position, c);
              }
            }
            if (bufferAccessor2.Length != 0)
            {
              DynamicBuffer<LocalNodeCache> dynamicBuffer3 = bufferAccessor2[index1];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<LocalNodeCache> dynamicBuffer4 = flag1 || !this.m_LocalNodeCache.HasBuffer(entity1) ? this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity1) : this.m_CommandBuffer.SetBuffer<LocalNodeCache>(entity1);
              if (flag2)
              {
                dynamicBuffer4.ResizeUninitialized(dynamicBuffer3.Length - 1);
                for (int index5 = 0; index5 < dynamicBuffer3.Length - 1; ++index5)
                  dynamicBuffer4[index5] = dynamicBuffer3[index5];
              }
              else
              {
                dynamicBuffer4.ResizeUninitialized(dynamicBuffer3.Length);
                for (int index6 = 0; index6 < dynamicBuffer3.Length; ++index6)
                  dynamicBuffer4[index6] = dynamicBuffer3[index6];
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!flag1 && this.m_LocalNodeCache.HasBuffer(entity1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<LocalNodeCache>(entity1);
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Area>(entity1, new Area(flags1));
            // ISSUE: reference to a compiler-generated field
            if (this.m_StorageData.HasComponent(creationDefinition.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Storage>(entity1, this.m_StorageData[creationDefinition.m_Original]);
            }
            PseudoRandomSeed componentData2 = new PseudoRandomSeed();
            if (flag4)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PseudoRandomSeedData.TryGetComponent(creationDefinition.m_Original, out componentData2))
                componentData2 = new PseudoRandomSeed((ushort) creationDefinition.m_RandomSeed);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(entity1, componentData2);
            }
            DynamicBuffer<Game.Prefabs.SubArea> bufferData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSubAreas.TryGetBuffer(creationDefinition.m_Prefab, out bufferData1))
            {
              NativeParallelMultiHashMap<Entity, Entity> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, Entity>();
              TempFlags flags3 = flags2 & ~TempFlags.Essential;
              AreaFlags flags4 = flags1 | AreaFlags.Slave;
              DynamicBuffer<Game.Areas.SubArea> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubAreas.TryGetBuffer(creationDefinition.m_Original, out bufferData2) && bufferData2.Length != 0)
              {
                parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                for (int index7 = 0; index7 < bufferData2.Length; ++index7)
                {
                  Game.Areas.SubArea subArea = bufferData2[index7];
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[subArea.m_Area];
                  parallelMultiHashMap.Add(prefabRef.m_Prefab, subArea.m_Area);
                }
              }
              for (int index8 = 0; index8 < bufferData1.Length; ++index8)
              {
                Game.Prefabs.SubArea subArea = bufferData1[index8];
                // ISSUE: object of a compiler-generated type is created
                oldAreaData = new GenerateAreasSystem.OldAreaData();
                // ISSUE: reference to a compiler-generated field
                oldAreaData.m_Prefab = subArea.m_Prefab;
                // ISSUE: reference to a compiler-generated field
                oldAreaData.m_Owner = entity1;
                // ISSUE: variable of a compiler-generated type
                GenerateAreasSystem.OldAreaData key2 = oldAreaData;
                NativeParallelMultiHashMapIterator<Entity> it2;
                // ISSUE: reference to a compiler-generated field
                if (parallelMultiHashMap.IsCreated && parallelMultiHashMap.TryGetFirstValue(subArea.m_Prefab, out key2.m_Original, out it2))
                  parallelMultiHashMap.Remove(it2);
                Entity entity2;
                if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && deletedAreas.TryGetFirstValue(key2, out entity2, out it1))
                {
                  deletedAreas.Remove(it1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Temp>(entity2, new Temp(key2.m_Original, flags3));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(entity2, new Updated());
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Deleted>(entity2);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Owner>(entity2, new Owner(entity1));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((creationDefinition.m_Flags & CreationFlags.Native) != (CreationFlags) 0 || this.m_NativeData.HasComponent(key2.m_Original))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Native>(entity2, new Native());
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.RemoveComponent<Native>(entity2);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  entity2 = this.m_CommandBuffer.CreateEntity(this.m_AreaData[subArea.m_Prefab].m_Archetype);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, new PrefabRef(subArea.m_Prefab));
                  if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Temp>(entity2, new Temp(key2.m_Original, flags3));
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Owner>(entity2, new Owner(entity1));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((creationDefinition.m_Flags & CreationFlags.Native) != (CreationFlags) 0 || this.m_NativeData.HasComponent(key2.m_Original))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Native>(entity1, new Native());
                  }
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Area>(entity2, new Area(flags4));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_StorageData.HasComponent(key2.m_Original))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Storage>(entity2, this.m_StorageData[key2.m_Original]);
                }
                if (flag4)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(entity2, componentData2);
                }
              }
              if (parallelMultiHashMap.IsCreated)
                parallelMultiHashMap.Dispose();
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> __Game_Tools_OwnerDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Storage> __Game_Areas_Storage_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaData> __Game_Prefabs_AreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OwnerDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle = state.GetBufferTypeHandle<LocalNodeCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Storage_RO_ComponentLookup = state.GetComponentLookup<Storage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaData_RO_ComponentLookup = state.GetComponentLookup<AreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferLookup = state.GetBufferLookup<LocalNodeCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
      }
    }
  }
}
