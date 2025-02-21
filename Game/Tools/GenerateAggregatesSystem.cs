// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateAggregatesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateAggregatesSystem : GameSystemBase
  {
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_DeletedQuery;
    private GenerateAggregatesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreationDefinition>(), ComponentType.ReadOnly<AggregateElement>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(ComponentType.ReadOnly<AggregateElement>(), ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Deleted>());
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
      this.__TypeHandle.__Game_Prefabs_AggregateNetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new GenerateAggregatesSystem.CreateAggregatesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_AggregateElementType = this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_AggregateData = this.__TypeHandle.__Game_Prefabs_AggregateNetData_RO_ComponentLookup,
        m_DefinitionChunks = archetypeChunkListAsync1,
        m_DeletedChunks = archetypeChunkListAsync2,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<GenerateAggregatesSystem.CreateAggregatesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2));
      archetypeChunkListAsync1.Dispose(jobHandle);
      archetypeChunkListAsync2.Dispose(jobHandle);
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
    public GenerateAggregatesSystem()
    {
    }

    [BurstCompile]
    private struct CreateAggregatesJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public BufferTypeHandle<AggregateElement> m_AggregateElementType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AggregateNetData> m_AggregateData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DefinitionChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DeletedChunks;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeParallelMultiHashMap<Entity, Entity> deletedAggregates = new NativeParallelMultiHashMap<Entity, Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_DeletedChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FillDeletedAggregates(this.m_DeletedChunks[index], deletedAggregates);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_DefinitionChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateAggregates(this.m_DefinitionChunks[index], deletedAggregates);
        }
        deletedAggregates.Dispose();
      }

      private void FillDeletedAggregates(
        ArchetypeChunk chunk,
        NativeParallelMultiHashMap<Entity, Entity> deletedAggregates)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          deletedAggregates.Add(prefabRef.m_Prefab, entity);
        }
      }

      private void CreateAggregates(
        ArchetypeChunk chunk,
        NativeParallelMultiHashMap<Entity, Entity> deletedAggregates)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<AggregateElement> bufferAccessor = chunk.GetBufferAccessor<AggregateElement>(ref this.m_AggregateElementType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          CreationDefinition creationDefinition = nativeArray[index1];
          DynamicBuffer<AggregateElement> dynamicBuffer1 = bufferAccessor[index1];
          TempFlags tempFlags = (TempFlags) 0;
          if (creationDefinition.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Hidden>(creationDefinition.m_Original, new Hidden());
            // ISSUE: reference to a compiler-generated field
            creationDefinition.m_Prefab = this.m_PrefabRefData[creationDefinition.m_Original].m_Prefab;
            if ((creationDefinition.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
              tempFlags |= TempFlags.Delete;
            else if ((creationDefinition.m_Flags & CreationFlags.Select) != (CreationFlags) 0)
              tempFlags |= TempFlags.Select;
            else if ((creationDefinition.m_Flags & CreationFlags.Relocate) != (CreationFlags) 0)
              tempFlags |= TempFlags.Modify;
          }
          else
            tempFlags |= TempFlags.Create;
          TempFlags flags = tempFlags | TempFlags.Essential;
          Entity entity;
          NativeParallelMultiHashMapIterator<Entity> it;
          if (deletedAggregates.TryGetFirstValue(creationDefinition.m_Prefab, out entity, out it))
          {
            deletedAggregates.Remove(it);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Temp>(entity, new Temp(creationDefinition.m_Original, flags));
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Deleted>(entity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            entity = this.m_CommandBuffer.CreateEntity(this.m_AggregateData[creationDefinition.m_Prefab].m_Archetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef(creationDefinition.m_Prefab));
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(entity, new Temp(creationDefinition.m_Original, flags));
          }
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<AggregateElement> dynamicBuffer2 = this.m_CommandBuffer.SetBuffer<AggregateElement>(entity);
          dynamicBuffer2.ResizeUninitialized(dynamicBuffer1.Length);
          for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
          {
            AggregateElement aggregateElement = dynamicBuffer1[index2];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Highlighted>(aggregateElement.m_Edge, new Highlighted());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(aggregateElement.m_Edge, new BatchesUpdated());
            dynamicBuffer2[index2] = aggregateElement;
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
      public BufferTypeHandle<AggregateElement> __Game_Net_AggregateElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AggregateNetData> __Game_Prefabs_AggregateNetData_RO_ComponentLookup;

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
        this.__Game_Net_AggregateElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<AggregateElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AggregateNetData_RO_ComponentLookup = state.GetComponentLookup<AggregateNetData>(true);
      }
    }
  }
}
