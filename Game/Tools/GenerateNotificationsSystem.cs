// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateNotificationsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Notifications;
using Game.Prefabs;
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
  public class GenerateNotificationsSystem : GameSystemBase
  {
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private EntityArchetype m_DefaultArchetype;
    private GenerateNotificationsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<CreationDefinition>(),
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<IconDefinition>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<PrefabRef>(), ComponentType.ReadWrite<Icon>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NotificationIconData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_IconDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle producerJob = new GenerateNotificationsSystem.GenerateIconsJob()
      {
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_IconDefinitionType = this.__TypeHandle.__Game_Tools_IconDefinition_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_NotificationIconData = this.__TypeHandle.__Game_Prefabs_NotificationIconData_RO_ComponentLookup,
        m_DefaultArchetype = this.m_DefaultArchetype,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<GenerateNotificationsSystem.GenerateIconsJob>(this.m_DefinitionQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
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
    public GenerateNotificationsSystem()
    {
    }

    [BurstCompile]
    private struct GenerateIconsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<IconDefinition> m_IconDefinitionType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NotificationIconData> m_NotificationIconData;
      [ReadOnly]
      public EntityArchetype m_DefaultArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray1 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<IconDefinition> nativeArray2 = chunk.GetNativeArray<IconDefinition>(ref this.m_IconDefinitionType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          CreationDefinition creationDefinition = nativeArray1[index];
          IconDefinition iconDefinition = nativeArray2[index];
          Icon component1 = new Icon();
          component1.m_Location = iconDefinition.m_Location;
          component1.m_Priority = iconDefinition.m_Priority;
          component1.m_ClusterLayer = iconDefinition.m_ClusterLayer;
          component1.m_Flags = iconDefinition.m_Flags;
          PrefabRef component2 = new PrefabRef();
          component2.m_Prefab = creationDefinition.m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (creationDefinition.m_Original != Entity.Null && this.m_PrefabRefData.HasComponent(creationDefinition.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            component2.m_Prefab = this.m_PrefabRefData[creationDefinition.m_Original].m_Prefab;
          }
          // ISSUE: reference to a compiler-generated field
          if (!(component2.m_Prefab == Entity.Null) && this.m_NotificationIconData.HasComponent(component2.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            NotificationIconData notificationIconData = this.m_NotificationIconData[component2.m_Prefab];
            if (!notificationIconData.m_Archetype.Valid)
            {
              if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                notificationIconData.m_Archetype = this.m_DefaultArchetype;
              }
              else
                continue;
            }
            if (creationDefinition.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Hidden>(unfilteredChunkIndex, creationDefinition.m_Original, new Hidden());
            }
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, notificationIconData.m_Archetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(unfilteredChunkIndex, entity, component2);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Icon>(unfilteredChunkIndex, entity, component1);
            if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
            {
              Temp component3 = new Temp();
              component3.m_Original = creationDefinition.m_Original;
              component3.m_Flags |= TempFlags.Essential;
              if ((creationDefinition.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
                component3.m_Flags |= TempFlags.Delete;
              else if ((creationDefinition.m_Flags & CreationFlags.Select) != (CreationFlags) 0)
                component3.m_Flags |= TempFlags.Select;
              else
                component3.m_Flags |= TempFlags.Create;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Temp>(unfilteredChunkIndex, entity, component3);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<DisallowCluster>(unfilteredChunkIndex, entity, new DisallowCluster());
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

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<IconDefinition> __Game_Tools_IconDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NotificationIconData> __Game_Prefabs_NotificationIconData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_IconDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<IconDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NotificationIconData_RO_ComponentLookup = state.GetComponentLookup<NotificationIconData>(true);
      }
    }
  }
}
