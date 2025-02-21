// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyAreasSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Areas;
using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ApplyAreasSystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private EntityQuery m_TempQuery;
    private ComponentTypeSet m_AppliedTypes;
    private ApplyAreasSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Area>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ApplyAreasSystem.PatchTempReferencesJob jobData1 = new ApplyAreasSystem.PatchTempReferencesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RW_ComponentTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      ApplyAreasSystem.HandleTempEntitiesJob jobData2 = new ApplyAreasSystem.HandleTempEntitiesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_AreaNodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_LocalNodeCacheType = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_LocalNodeCache = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup,
        m_AppliedTypes = this.m_AppliedTypes,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.Schedule<ApplyAreasSystem.PatchTempReferencesJob>(this.m_TempQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery tempQuery = this.m_TempQuery;
      JobHandle dependsOn = jobHandle;
      JobHandle producerJob = jobData2.ScheduleParallel<ApplyAreasSystem.HandleTempEntitiesJob>(tempQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public ApplyAreasSystem()
    {
    }

    [BurstCompile]
    private struct PatchTempReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      public BufferLookup<SubArea> m_SubAreas;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity area = nativeArray1[index];
          Owner owner1 = nativeArray3[index];
          Temp temp1 = nativeArray2[index];
          if (temp1.m_Original == Entity.Null && (temp1.m_Flags & TempFlags.Delete) == (TempFlags) 0)
          {
            Owner owner2 = owner1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(owner1.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              Temp temp2 = this.m_TempData[owner1.m_Owner];
              if (temp2.m_Original != Entity.Null && (temp2.m_Flags & TempFlags.Replace) == (TempFlags) 0)
                owner2.m_Owner = temp2.m_Original;
            }
            if (owner2.m_Owner != owner1.m_Owner)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubAreas.HasBuffer(owner1.m_Owner))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<SubArea>(this.m_SubAreas[owner1.m_Owner], new SubArea(area));
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubAreas.HasBuffer(owner2.m_Owner))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<SubArea>(this.m_SubAreas[owner2.m_Owner], new SubArea(area));
              }
              nativeArray3[index] = owner2;
            }
          }
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

    [BurstCompile]
    private struct HandleTempEntitiesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_AreaNodeType;
      [ReadOnly]
      public BufferTypeHandle<LocalNodeCache> m_LocalNodeCacheType;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> m_LocalNodeCache;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Areas.Node> bufferAccessor1 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_AreaNodeType);
        if (bufferAccessor1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LocalNodeCache> bufferAccessor2 = chunk.GetBufferAccessor<LocalNodeCache>(ref this.m_LocalNodeCacheType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Temp temp = nativeArray2[index];
            if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Delete(unfilteredChunkIndex, entity, temp);
            }
            else if (temp.m_Original != Entity.Null)
            {
              DynamicBuffer<LocalNodeCache> cachedNodes = new DynamicBuffer<LocalNodeCache>();
              if (bufferAccessor2.Length != 0)
                cachedNodes = bufferAccessor2[index];
              // ISSUE: reference to a compiler-generated method
              this.Update(unfilteredChunkIndex, entity, temp, bufferAccessor1[index], cachedNodes);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity);
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity entity = nativeArray1[index];
            Temp temp = nativeArray2[index];
            if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Delete(unfilteredChunkIndex, entity, temp);
            }
            else if (temp.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.Update(unfilteredChunkIndex, entity, temp);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity);
            }
          }
        }
      }

      private void Delete(int chunkIndex, Entity entity, Temp temp)
      {
        if (temp.m_Original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, temp.m_Original, new Deleted());
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Update(int chunkIndex, Entity entity, Temp temp, bool updateOriginal = true)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HiddenData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(chunkIndex, temp.m_Original);
        }
        if (updateOriginal)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(chunkIndex, temp.m_Original, new Updated());
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Update(
        int chunkIndex,
        Entity entity,
        Temp temp,
        DynamicBuffer<Game.Areas.Node> nodes,
        DynamicBuffer<LocalNodeCache> cachedNodes)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> dynamicBuffer1 = this.m_CommandBuffer.SetBuffer<Game.Areas.Node>(chunkIndex, temp.m_Original);
        dynamicBuffer1.ResizeUninitialized(nodes.Length);
        for (int index = 0; index < nodes.Length; ++index)
          dynamicBuffer1[index] = nodes[index];
        if (cachedNodes.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<LocalNodeCache> dynamicBuffer2 = !this.m_LocalNodeCache.HasBuffer(temp.m_Original) ? this.m_CommandBuffer.AddBuffer<LocalNodeCache>(chunkIndex, temp.m_Original) : this.m_CommandBuffer.SetBuffer<LocalNodeCache>(chunkIndex, temp.m_Original);
          dynamicBuffer2.ResizeUninitialized(cachedNodes.Length);
          for (int index = 0; index < cachedNodes.Length; ++index)
            dynamicBuffer2[index] = cachedNodes[index];
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalNodeCache.HasBuffer(temp.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<LocalNodeCache>(chunkIndex, temp.m_Original);
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.Update(chunkIndex, entity, temp);
      }

      private void Create(int chunkIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Temp>(chunkIndex, entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(chunkIndex, entity, in this.m_AppliedTypes);
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
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      public BufferLookup<SubArea> __Game_Areas_SubArea_RW_BufferLookup;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RW_BufferLookup = state.GetBufferLookup<SubArea>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle = state.GetBufferTypeHandle<LocalNodeCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferLookup = state.GetBufferLookup<LocalNodeCache>(true);
      }
    }
  }
}
