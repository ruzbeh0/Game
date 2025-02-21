// Decompiled with JetBrains decompiler
// Type: Game.Objects.PlaceholderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class PlaceholderSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_EntityQuery;
    private PlaceholderSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EntityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Placeholder>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EntityQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.purpose != Colossal.Serialization.Entities.Purpose.NewGame)
        return;
      EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new PlaceholderSystem.PlaceholderJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_AreaNodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_PlaceholderData = this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_PrefabPlaceholderElements = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_PrefabRequirementElements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup,
        m_Theme = this.m_CityConfigurationSystem.defaultTheme,
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = entityCommandBuffer.AsParallelWriter()
      }.ScheduleParallel<PlaceholderSystem.PlaceholderJob>(this.m_EntityQuery, this.Dependency).Complete();
      entityCommandBuffer.Playback(this.EntityManager);
      entityCommandBuffer.Dispose();
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
    public PlaceholderSystem()
    {
    }

    [BurstCompile]
    private struct PlaceholderJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_AreaNodeType;
      [ReadOnly]
      public ComponentLookup<Placeholder> m_PlaceholderData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderElements;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_PrefabRequirementElements;
      [ReadOnly]
      public Entity m_Theme;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray1 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        if (nativeArray1.Length != 0 && nativeArray2.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Owner owner = nativeArray1[index];
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData))
              owner = componentData;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PlaceholderData.HasComponent(owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, owner.m_Owner, new Updated());
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Game.Areas.Node> bufferAccessor = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_AreaNodeType);
          // ISSUE: reference to a compiler-generated field
          Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          for (int index1 = 0; index1 < nativeArray4.Length; ++index1)
          {
            Entity e = nativeArray3[index1];
            PrefabRef prefabRef = nativeArray4[index1];
            Entity entity1 = Entity.Null;
            DynamicBuffer<PlaceholderObjectElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabPlaceholderElements.TryGetBuffer(prefabRef.m_Prefab, out bufferData))
            {
              int max = 0;
              for (int index2 = 0; index2 < bufferData.Length; ++index2)
              {
                PlaceholderObjectElement placeholder = bufferData[index2];
                int probability;
                // ISSUE: reference to a compiler-generated method
                if (this.GetVariationProbability(placeholder, out probability))
                {
                  max += probability;
                  if (random.NextInt(max) < probability)
                    entity1 = placeholder.m_Object;
                }
              }
            }
            if (entity1 != Entity.Null)
            {
              CreationDefinition component = new CreationDefinition();
              component.m_Prefab = entity1;
              component.m_Flags |= CreationFlags.Permanent | CreationFlags.Native;
              component.m_RandomSeed = random.NextInt();
              Owner componentData;
              if (CollectionUtils.TryGet<Owner>(nativeArray1, index1, out componentData))
              {
                component.m_Owner = componentData.m_Owner;
                // ISSUE: reference to a compiler-generated field
                while (!this.m_PlaceholderData.HasComponent(componentData.m_Owner))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_OwnerData.TryGetComponent(componentData.m_Owner, out componentData))
                    goto label_23;
                }
                continue;
              }
label_23:
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CreationDefinition>(unfilteredChunkIndex, entity2, component);
              Transform transform;
              if (CollectionUtils.TryGet<Transform>(nativeArray2, index1, out transform))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<ObjectDefinition>(unfilteredChunkIndex, entity2, new ObjectDefinition()
                {
                  m_ParentMesh = -1,
                  m_Position = transform.m_Position,
                  m_Rotation = transform.m_Rotation,
                  m_LocalPosition = transform.m_Position,
                  m_LocalRotation = transform.m_Rotation
                });
              }
              DynamicBuffer<Game.Areas.Node> dynamicBuffer1;
              if (CollectionUtils.TryGet<Game.Areas.Node>(bufferAccessor, index1, out dynamicBuffer1))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Areas.Node> dynamicBuffer2 = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(unfilteredChunkIndex, entity2);
                if (dynamicBuffer1.Length != 0)
                {
                  dynamicBuffer2.Capacity = dynamicBuffer1.Length + 1;
                  dynamicBuffer2.AddRange(dynamicBuffer1.AsNativeArray());
                  dynamicBuffer2.Add(dynamicBuffer1[0]);
                }
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, entity2, new Updated());
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, e, new Deleted());
          }
        }
      }

      private bool GetVariationProbability(
        PlaceholderObjectElement placeholder,
        out int probability)
      {
        probability = 100;
        DynamicBuffer<ObjectRequirementElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRequirementElements.TryGetBuffer(placeholder.m_Object, out bufferData))
        {
          int num = -1;
          bool flag = true;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            ObjectRequirementElement requirementElement = bufferData[index];
            if ((int) requirementElement.m_Group != num)
            {
              if (flag)
              {
                num = (int) requirementElement.m_Group;
                flag = false;
              }
              else
                break;
            }
            // ISSUE: reference to a compiler-generated field
            flag |= this.m_Theme == requirementElement.m_Requirement;
          }
          if (!flag)
            return false;
        }
        SpawnableObjectData componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabSpawnableObjectData.TryGetComponent(placeholder.m_Object, out componentData))
          probability = componentData.m_Probability;
        return true;
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Placeholder> __Game_Objects_Placeholder_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Placeholder_RO_ComponentLookup = state.GetComponentLookup<Placeholder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
      }
    }
  }
}
