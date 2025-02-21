// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyNotificationsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Notifications;
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
  public class ApplyNotificationsSystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private EntityQuery m_TempQuery;
    private ComponentTypeSet m_AppliedTypes;
    private ApplyNotificationsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_TempQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ToolErrorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new ApplyNotificationsSystem.ApplyTempIconsJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_IconType = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_IconElementType = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_IconData = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ToolErrorData = this.__TypeHandle.__Game_Prefabs_ToolErrorData_RO_ComponentLookup,
        m_IconElements = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferLookup,
        m_AppliedTypes = this.m_AppliedTypes,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
      }.Schedule<ApplyNotificationsSystem.ApplyTempIconsJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
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
    public ApplyNotificationsSystem()
    {
    }

    [BurstCompile]
    private struct ApplyTempIconsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Icon> m_IconType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<IconElement> m_IconElementType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Icon> m_IconData;
      [ReadOnly]
      public ComponentLookup<Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ToolErrorData> m_ToolErrorData;
      [ReadOnly]
      public BufferLookup<IconElement> m_IconElements;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          num += this.m_Chunks[index].Count;
        }
        NativeParallelMultiHashMap<Entity, Entity> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, Entity>(num, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeHashMap<Entity, Entity> nativeHashMap = new NativeHashMap<Entity, Entity>(num, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Icon>(ref this.m_IconType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              Entity e = nativeArray1[index2];
              Owner owner = nativeArray2[index2];
              PrefabRef prefabRef = nativeArray3[index2];
              bool flag = false;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ToolErrorData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                flag = (this.m_ToolErrorData[prefabRef.m_Prefab].m_Flags & ToolErrorFlags.TemporaryOnly) != 0;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_TempData.HasComponent(owner.m_Owner))
              {
                // ISSUE: reference to a compiler-generated field
                Temp temp = this.m_TempData[owner.m_Owner];
                if (temp.m_Original != Entity.Null)
                {
                  if (flag)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Deleted>(e, new Deleted());
                  }
                  else
                    parallelMultiHashMap.Add(temp.m_Original, e);
                  nativeHashMap.TryAdd(temp.m_Original, Entity.Null);
                }
                else if (flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Deleted>(e, new Deleted());
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Temp>(e);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent(e, in this.m_AppliedTypes);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(e, new Deleted());
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray4 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray5 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
            // ISSUE: reference to a compiler-generated field
            bool flag = chunk.Has<IconElement>(ref this.m_IconElementType);
            for (int index3 = 0; index3 < nativeArray5.Length; ++index3)
            {
              Entity e = nativeArray4[index3];
              Temp temp = nativeArray5[index3];
              if (temp.m_Original != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_IconElements.HasBuffer(temp.m_Original) && !nativeHashMap.TryAdd(temp.m_Original, e))
                  nativeHashMap[temp.m_Original] = e;
                if (flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<IconElement>(e);
                }
              }
            }
          }
        }
        NativeArray<Entity> keyArray = nativeHashMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<IconElement> list = new NativeList<IconElement>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index4 = 0; index4 < keyArray.Length; ++index4)
        {
          Entity entity1 = keyArray[index4];
          DynamicBuffer<IconElement> dynamicBuffer1 = new DynamicBuffer<IconElement>();
          DynamicBuffer<IconElement> dynamicBuffer2 = new DynamicBuffer<IconElement>();
          // ISSUE: reference to a compiler-generated field
          if (this.m_IconElements.HasBuffer(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer2 = this.m_IconElements[entity1];
            for (int index5 = 0; index5 < dynamicBuffer2.Length; ++index5)
              list.Add(dynamicBuffer2[index5]);
          }
          Entity entity2;
          NativeParallelMultiHashMapIterator<Entity> it;
          if (parallelMultiHashMap.TryGetFirstValue(entity1, out entity2, out it))
          {
            if (!dynamicBuffer1.IsCreated)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              dynamicBuffer1 = !dynamicBuffer2.IsCreated ? this.m_CommandBuffer.AddBuffer<IconElement>(entity1) : this.m_CommandBuffer.SetBuffer<IconElement>(entity1);
            }
            do
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[entity2];
              Target componentData1;
              // ISSUE: reference to a compiler-generated field
              this.m_TargetData.TryGetComponent(entity2, out componentData1);
              if (dynamicBuffer2.IsCreated)
              {
                for (int index6 = 0; index6 < list.Length; ++index6)
                {
                  Entity icon1 = list[index6].m_Icon;
                  // ISSUE: reference to a compiler-generated field
                  if (!(this.m_PrefabRefData[icon1].m_Prefab != prefabRef.m_Prefab))
                  {
                    Target componentData2;
                    // ISSUE: reference to a compiler-generated field
                    this.m_TargetData.TryGetComponent(icon1, out componentData2);
                    if (componentData2.m_Target == componentData1.m_Target)
                    {
                      // ISSUE: reference to a compiler-generated field
                      Icon component = this.m_IconData[entity2];
                      // ISSUE: reference to a compiler-generated field
                      Icon icon2 = this.m_IconData[icon1];
                      component.m_ClusterIndex = icon2.m_ClusterIndex;
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.SetComponent<Icon>(icon1, component);
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Updated>(icon1, new Updated());
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Deleted>(entity2, new Deleted());
                      dynamicBuffer1.Add(new IconElement(icon1));
                      CollectionUtils.Remove<IconElement>(list, index6);
                      goto label_47;
                    }
                  }
                }
              }
              dynamicBuffer1.Add(new IconElement(entity2));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Owner>(entity2, new Owner(entity1));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Temp>(entity2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(entity2, in this.m_AppliedTypes);
label_47:;
            }
            while (parallelMultiHashMap.TryGetNextValue(out entity2, ref it));
          }
          if (dynamicBuffer2.IsCreated)
          {
            Entity tempContainer = nativeHashMap[entity1];
            for (int index7 = 0; index7 < list.Length; ++index7)
            {
              Entity icon = list[index7].m_Icon;
              // ISSUE: reference to a compiler-generated method
              if (this.ValidateOldIcon(icon, entity1, tempContainer))
              {
                if (!dynamicBuffer1.IsCreated)
                {
                  // ISSUE: reference to a compiler-generated field
                  dynamicBuffer1 = this.m_CommandBuffer.SetBuffer<IconElement>(entity1);
                }
                dynamicBuffer1.Add(new IconElement(icon));
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(icon, new Deleted());
              }
            }
            if (!dynamicBuffer1.IsCreated)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<IconElement>(entity1);
            }
            list.Clear();
          }
        }
      }

      private bool ValidateOldIcon(Entity icon, Entity container, Entity tempContainer)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return !this.m_ToolErrorData.HasComponent(this.m_PrefabRefData[icon].m_Prefab) && (!(tempContainer != Entity.Null) || this.m_IconData[icon].m_Priority < IconPriority.FatalProblem || !this.m_DestroyedData.HasComponent(container) || this.m_DestroyedData.HasComponent(tempContainer));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Icon> __Game_Notifications_Icon_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<IconElement> __Game_Notifications_IconElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Icon> __Game_Notifications_Icon_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ToolErrorData> __Game_Prefabs_ToolErrorData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<IconElement> __Game_Notifications_IconElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<IconElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentLookup = state.GetComponentLookup<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ToolErrorData_RO_ComponentLookup = state.GetComponentLookup<ToolErrorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferLookup = state.GetBufferLookup<IconElement>(true);
      }
    }
  }
}
