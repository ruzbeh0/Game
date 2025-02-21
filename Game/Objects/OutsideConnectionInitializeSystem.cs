// Decompiled with JetBrains decompiler
// Type: Game.Objects.OutsideConnectionInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class OutsideConnectionInitializeSystem : GameSystemBase, IPostDeserialize
  {
    private const float kNearbyMaxDistanceSqr = 10000f;
    private EntityQuery m_ExistingQuery;
    private EntityQuery m_CreatedQuery;
    private OutsideConnectionInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ExistingQuery = this.GetEntityQuery(ComponentType.ReadOnly<OutsideConnection>(), ComponentType.ReadOnly<RandomLocalizationIndex>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<OutsideConnection>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadWrite<RandomLocalizationIndex>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      NativeList<OutsideConnectionInitializeSystem.OutsideConnectionInfo> nativeList = new NativeList<OutsideConnectionInitializeSystem.OutsideConnectionInfo>(this.m_ExistingQuery.CalculateEntityCount() + this.m_CreatedQuery.CalculateEntityCount(), (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_RandomLocalizationIndex_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle dependsOn = new OutsideConnectionInitializeSystem.CollectOutsideConnectionsJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_RandomLocalizationIndexType = this.__TypeHandle.__Game_Common_RandomLocalizationIndex_RO_BufferTypeHandle,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_OutsideConnections = nativeList.AsParallelWriter()
      }.ScheduleParallel<OutsideConnectionInitializeSystem.CollectOutsideConnectionsJob>(this.m_ExistingQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalizationCount_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_RandomLocalizationIndex_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      OutsideConnectionInitializeSystem.InitializeLocalizationJob jobData = new OutsideConnectionInitializeSystem.InitializeLocalizationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_RandomLocalizationIndexType = this.__TypeHandle.__Game_Common_RandomLocalizationIndex_RW_BufferTypeHandle,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_LocalizationCounts = this.__TypeHandle.__Game_Prefabs_LocalizationCount_RO_BufferLookup,
        m_OutsideConnections = nativeList,
        m_RandomSeed = RandomSeed.Next()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<OutsideConnectionInitializeSystem.InitializeLocalizationJob>(this.m_CreatedQuery, dependsOn);
      nativeList.Dispose(this.Dependency);
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (!(context.version < Version.outsideConnNames))
        return;
      EntityQuery entityQuery = this.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<OutsideConnection>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<RandomLocalizationIndex>());
      if (!entityQuery.IsEmptyIgnoreFilter)
      {
        NativeList<OutsideConnectionInitializeSystem.OutsideConnectionInfo> connections = new NativeList<OutsideConnectionInitializeSystem.OutsideConnectionInfo>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeArray<PrefabRef> componentDataArray1 = entityQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeArray<Transform> componentDataArray2 = entityQuery.ToComponentDataArray<Transform>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        RandomSeed randomSeed = RandomSeed.Next();
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Entity prefab = componentDataArray1[index].m_Prefab;
          OutsideConnectionData component;
          OutsideConnectionTransferType transferType = this.EntityManager.TryGetComponent<OutsideConnectionData>(prefab, out component) ? component.m_Type : OutsideConnectionTransferType.None;
          float3 position = componentDataArray2[index].m_Position;
          EntityManager entityManager = this.EntityManager;
          if (entityManager.HasBuffer<LocalizationCount>(prefab))
          {
            entityManager = this.EntityManager;
            DynamicBuffer<RandomLocalizationIndex> indices = entityManager.AddBuffer<RandomLocalizationIndex>(entityArray[index]);
            entityManager = this.EntityManager;
            DynamicBuffer<LocalizationCount> buffer = entityManager.GetBuffer<LocalizationCount>(prefab, true);
            RandomLocalizationIndex randomIndex;
            // ISSUE: reference to a compiler-generated method
            if (buffer.Length == 1 && OutsideConnectionInitializeSystem.TryGetNearestConnectionRandomIndex(connections, transferType, position, out randomIndex) && randomIndex.m_Index < buffer[0].m_Count)
            {
              indices.ResizeUninitialized(1);
              indices[0] = randomIndex;
            }
            else
            {
              Random random = randomSeed.GetRandom(entityArray[index].Index + 1);
              RandomLocalizationIndex.GenerateRandomIndices(indices, buffer, ref random);
            }
            if (indices.Length == 1)
            {
              // ISSUE: object of a compiler-generated type is created
              connections.Add(new OutsideConnectionInitializeSystem.OutsideConnectionInfo()
              {
                m_TransferType = transferType,
                m_Position = position,
                m_RandomIndex = indices[0]
              });
            }
          }
        }
        entityArray.Dispose();
        componentDataArray1.Dispose();
        componentDataArray2.Dispose();
        connections.Dispose();
      }
      entityQuery.Dispose();
    }

    private static OutsideConnectionTransferType GetTransferType(
      Entity prefab,
      ref ComponentLookup<OutsideConnectionData> outsideConnectionData)
    {
      OutsideConnectionData componentData;
      return !outsideConnectionData.TryGetComponent(prefab, out componentData) ? OutsideConnectionTransferType.None : componentData.m_Type;
    }

    private static bool TryGetNearestConnectionRandomIndex(
      NativeList<OutsideConnectionInitializeSystem.OutsideConnectionInfo> connections,
      OutsideConnectionTransferType transferType,
      float3 position,
      out RandomLocalizationIndex randomIndex)
    {
      randomIndex = new RandomLocalizationIndex();
      float num1 = 10000f;
      foreach (OutsideConnectionInitializeSystem.OutsideConnectionInfo connection in connections)
      {
        // ISSUE: reference to a compiler-generated field
        if ((connection.m_TransferType & transferType) != OutsideConnectionTransferType.None)
        {
          // ISSUE: reference to a compiler-generated field
          float num2 = math.distancesq(connection.m_Position, position);
          if ((double) num2 < (double) num1)
          {
            // ISSUE: reference to a compiler-generated field
            randomIndex = connection.m_RandomIndex;
            num1 = num2;
          }
        }
      }
      return (double) num1 < 10000.0;
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
    public OutsideConnectionInitializeSystem()
    {
    }

    [BurstCompile]
    private struct CollectOutsideConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public BufferTypeHandle<RandomLocalizationIndex> m_RandomLocalizationIndexType;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionData;
      public NativeList<OutsideConnectionInitializeSystem.OutsideConnectionInfo>.ParallelWriter m_OutsideConnections;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RandomLocalizationIndex> bufferAccessor = chunk.GetBufferAccessor<RandomLocalizationIndex>(ref this.m_RandomLocalizationIndexType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          DynamicBuffer<RandomLocalizationIndex> dynamicBuffer = bufferAccessor[index];
          if (dynamicBuffer.Length == 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: object of a compiler-generated type is created
            this.m_OutsideConnections.AddNoResize(new OutsideConnectionInitializeSystem.OutsideConnectionInfo()
            {
              m_TransferType = OutsideConnectionInitializeSystem.GetTransferType(nativeArray1[index].m_Prefab, ref this.m_OutsideConnectionData),
              m_Position = nativeArray2[index].m_Position,
              m_RandomIndex = dynamicBuffer[0]
            });
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
    private struct InitializeLocalizationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      public BufferTypeHandle<RandomLocalizationIndex> m_RandomLocalizationIndexType;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionData;
      [ReadOnly]
      public BufferLookup<LocalizationCount> m_LocalizationCounts;
      public NativeList<OutsideConnectionInitializeSystem.OutsideConnectionInfo> m_OutsideConnections;
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RandomLocalizationIndex> bufferAccessor = chunk.GetBufferAccessor<RandomLocalizationIndex>(ref this.m_RandomLocalizationIndexType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          float3 position = nativeArray3[index].m_Position;
          DynamicBuffer<RandomLocalizationIndex> indices = bufferAccessor[index];
          DynamicBuffer<LocalizationCount> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalizationCounts.TryGetBuffer(prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            OutsideConnectionTransferType transferType = OutsideConnectionInitializeSystem.GetTransferType(prefab, ref this.m_OutsideConnectionData);
            RandomLocalizationIndex randomIndex;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (bufferData.Length == 1 && OutsideConnectionInitializeSystem.TryGetNearestConnectionRandomIndex(this.m_OutsideConnections, transferType, position, out randomIndex) && randomIndex.m_Index <= bufferData[0].m_Count)
            {
              indices.ResizeUninitialized(1);
              indices[0] = randomIndex;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Random random = this.m_RandomSeed.GetRandom(entity.Index + 1);
              RandomLocalizationIndex.GenerateRandomIndices(indices, bufferData, ref random);
            }
            if (indices.Length == 1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_OutsideConnections.Add(new OutsideConnectionInitializeSystem.OutsideConnectionInfo()
              {
                m_TransferType = transferType,
                m_Position = position,
                m_RandomIndex = indices[0]
              });
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

    private struct OutsideConnectionInfo
    {
      public OutsideConnectionTransferType m_TransferType;
      public float3 m_Position;
      public RandomLocalizationIndex m_RandomIndex;
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<RandomLocalizationIndex> __Game_Common_RandomLocalizationIndex_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public BufferTypeHandle<RandomLocalizationIndex> __Game_Common_RandomLocalizationIndex_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<LocalizationCount> __Game_Prefabs_LocalizationCount_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_RandomLocalizationIndex_RO_BufferTypeHandle = state.GetBufferTypeHandle<RandomLocalizationIndex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_RandomLocalizationIndex_RW_BufferTypeHandle = state.GetBufferTypeHandle<RandomLocalizationIndex>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalizationCount_RO_BufferLookup = state.GetBufferLookup<LocalizationCount>(true);
      }
    }
  }
}
