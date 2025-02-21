// Decompiled with JetBrains decompiler
// Type: Game.Net.AggregateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class AggregateSystem : GameSystemBase
  {
    private EntityQuery m_ModifiedQuery;
    private ModificationBarrier2B m_ModificationBarrier;
    private AggregateSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2B>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Aggregated>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[0]
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ModifiedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_ModifiedQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_AggregateElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Aggregated_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AggregateNetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new AggregateSystem.UpdateAgregatesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabAggregateData = this.__TypeHandle.__Game_Prefabs_AggregateNetData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_AggregatedData = this.__TypeHandle.__Game_Net_Aggregated_RW_ComponentLookup,
        m_AggregateElements = this.__TypeHandle.__Game_Net_AggregateElement_RW_BufferLookup,
        m_Chunks = archetypeChunkListAsync,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<AggregateSystem.UpdateAgregatesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
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
    public AggregateSystem()
    {
    }

    [BurstCompile]
    private struct UpdateAgregatesJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<AggregateNetData> m_PrefabAggregateData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      public ComponentLookup<Aggregated> m_AggregatedData;
      public BufferLookup<AggregateElement> m_AggregateElements;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        int capacity = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index];
          capacity += chunk.Count;
        }
        NativeParallelHashSet<Entity> edgeSet = new NativeParallelHashSet<Entity>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashSet<Entity> emptySet = new NativeParallelHashSet<Entity>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashMap<Entity, Entity> updateMap = new NativeParallelHashMap<Entity, Entity>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<Temp>(ref this.m_TempType);
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Created>(ref this.m_CreatedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
            for (int index2 = 0; index2 < nativeArray.Length; ++index2)
            {
              Entity entity = nativeArray[index2];
              // ISSUE: reference to a compiler-generated field
              Aggregated aggregated = this.m_AggregatedData[entity];
              if (aggregated.m_Aggregate != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                if (flag && !this.m_TempData.HasComponent(aggregated.m_Aggregate))
                {
                  updateMap.TryAdd(aggregated.m_Aggregate, aggregated.m_Aggregate);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_AggregateElements[aggregated.m_Aggregate].Add(new AggregateElement(entity));
                  edgeSet.Add(aggregated.m_Aggregate);
                }
              }
              else
                emptySet.Add(entity);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
            for (int index3 = 0; index3 < nativeArray.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              Aggregated aggregated = this.m_AggregatedData[nativeArray[index3]];
              if (aggregated.m_Aggregate != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                if (flag && !this.m_TempData.HasComponent(aggregated.m_Aggregate))
                  updateMap.TryAdd(aggregated.m_Aggregate, aggregated.m_Aggregate);
                else
                  edgeSet.Add(aggregated.m_Aggregate);
              }
            }
          }
        }
        if (!edgeSet.IsEmpty)
        {
          NativeArray<Entity> nativeArray = edgeSet.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          edgeSet.Clear();
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.ValidateAggregate(nativeArray[index], edgeSet, emptySet, updateMap);
          }
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.CombineAggregate(nativeArray[index], updateMap);
          }
          nativeArray.Dispose();
        }
        if (!emptySet.IsEmpty)
        {
          NativeArray<Entity> nativeArray = emptySet.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeList<AggregateElement> edgeList = new NativeList<AggregateElement>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            Entity startEdge = nativeArray[index];
            if (emptySet.Contains(startEdge))
            {
              emptySet.Remove(startEdge);
              // ISSUE: reference to a compiler-generated method
              this.CreateAggregate(startEdge, emptySet, edgeList, updateMap);
            }
          }
          edgeList.Dispose();
          nativeArray.Dispose();
        }
        if (!updateMap.IsEmpty)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index4 = 0; index4 < this.m_Chunks.Length; ++index4)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk chunk = this.m_Chunks[index4];
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Temp>(ref this.m_TempType))
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
              for (int index5 = 0; index5 < nativeArray.Length; ++index5)
              {
                Entity entity1 = nativeArray[index5];
                // ISSUE: reference to a compiler-generated field
                Aggregated aggregated = this.m_AggregatedData[entity1];
                Entity entity2;
                if (aggregated.m_Aggregate != Entity.Null && updateMap.TryGetValue(aggregated.m_Aggregate, out entity2) && entity2 != aggregated.m_Aggregate)
                {
                  aggregated.m_Aggregate = entity2;
                  // ISSUE: reference to a compiler-generated field
                  this.m_AggregatedData[entity1] = aggregated;
                }
              }
            }
          }
          NativeParallelHashMap<Entity, Entity>.Enumerator enumerator = updateMap.GetEnumerator();
          while (enumerator.MoveNext())
          {
            Entity entity = enumerator.Current.Value;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AggregateElements.HasBuffer(entity))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_DeletedData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Deleted>(entity);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<BatchesUpdated>(entity, new BatchesUpdated());
              }
            }
          }
        }
        edgeSet.Dispose();
        emptySet.Dispose();
        updateMap.Dispose();
      }

      private void CreateAggregate(
        Entity startEdge,
        NativeParallelHashSet<Entity> emptySet,
        NativeList<AggregateElement> edgeList,
        NativeParallelHashMap<Entity, Entity> updateMap)
      {
        // ISSUE: reference to a compiler-generated method
        Entity aggregateType = this.GetAggregateType(startEdge);
        if (aggregateType == Entity.Null)
          return;
        // ISSUE: reference to a compiler-generated field
        Edge edge1 = this.m_EdgeData[startEdge];
        // ISSUE: reference to a compiler-generated method
        this.AddElements(startEdge, edge1.m_Start, true, aggregateType, emptySet, edgeList);
        CollectionUtils.Reverse<AggregateElement>(edgeList.AsArray());
        edgeList.Add(new AggregateElement(startEdge));
        // ISSUE: reference to a compiler-generated method
        this.AddElements(startEdge, edge1.m_End, false, aggregateType, emptySet, edgeList);
        // ISSUE: reference to a compiler-generated field
        bool isTemp = this.m_TempData.HasComponent(startEdge);
        // ISSUE: reference to a compiler-generated method
        if (!this.TryCombine(aggregateType, edgeList, isTemp, updateMap))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(this.m_PrefabAggregateData[aggregateType].m_Archetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef(aggregateType));
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<AggregateElement> dynamicBuffer = this.m_CommandBuffer.SetBuffer<AggregateElement>(entity);
          if (isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(entity, new Temp(Entity.Null, TempFlags.Create));
          }
          for (int index = 0; index < edgeList.Length; ++index)
          {
            AggregateElement edge2 = edgeList[index];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Aggregated>(edge2.m_Edge, new Aggregated()
            {
              m_Aggregate = entity
            });
            dynamicBuffer.Add(edge2);
          }
        }
        edgeList.Clear();
      }

      private bool TryCombine(
        Entity prefab,
        NativeList<AggregateElement> edgeList,
        bool isTemp,
        NativeParallelHashMap<Entity, Entity> updateMap)
      {
        Entity edge1;
        Entity node1;
        bool isStart1;
        Entity otherAggregate1;
        bool otherIsStart1;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (this.GetStart(edgeList.AsArray(), out edge1, out node1, out isStart1) && this.ShouldCombine(edge1, node1, isStart1, prefab, Entity.Null, isTemp, out otherAggregate1, out otherIsStart1))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<AggregateElement> aggregateElement1 = this.m_AggregateElements[otherAggregate1];
          int length1 = aggregateElement1.Length;
          aggregateElement1.ResizeUninitialized(aggregateElement1.Length + edgeList.Length);
          if (otherIsStart1)
          {
            for (int index = length1 - 1; index >= 0; --index)
              aggregateElement1[edgeList.Length + index] = aggregateElement1[index];
          }
          for (int index = 0; index < edgeList.Length; ++index)
          {
            AggregateElement edge2 = edgeList[index];
            // ISSUE: reference to a compiler-generated field
            this.m_AggregatedData[edge2.m_Edge] = new Aggregated()
            {
              m_Aggregate = otherAggregate1
            };
            aggregateElement1[math.select(length1, 0, otherIsStart1) + edgeList.Length - index - 1] = edge2;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(otherAggregate1);
          Entity edge3;
          Entity node2;
          bool isStart2;
          Entity otherAggregate2;
          bool otherIsStart2;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          if (this.GetEnd(edgeList.AsArray(), out edge3, out node2, out isStart2) && this.ShouldCombine(edge3, node2, isStart2, prefab, otherAggregate1, isTemp, out otherAggregate2, out otherIsStart2))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<AggregateElement> aggregateElement2 = this.m_AggregateElements[otherAggregate2];
            int length2 = aggregateElement1.Length;
            aggregateElement1.ResizeUninitialized(aggregateElement2.Length + aggregateElement1.Length);
            if (otherIsStart1)
            {
              for (int index = length2 - 1; index >= 0; --index)
                aggregateElement1[aggregateElement2.Length + index] = aggregateElement1[index];
            }
            for (int index = 0; index < aggregateElement2.Length; ++index)
            {
              AggregateElement aggregateElement3 = aggregateElement2[index];
              // ISSUE: reference to a compiler-generated field
              this.m_AggregatedData[aggregateElement3.m_Edge] = new Aggregated()
              {
                m_Aggregate = otherAggregate1
              };
              aggregateElement1[math.select(length2, 0, otherIsStart1) + math.select(index, aggregateElement2.Length - index - 1, otherIsStart2 == otherIsStart1)] = aggregateElement3;
            }
            aggregateElement2.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(otherAggregate2);
            if (updateMap.ContainsKey(otherAggregate2))
              updateMap[otherAggregate2] = otherAggregate1;
          }
          return true;
        }
        Entity edge4;
        Entity node3;
        bool isStart3;
        Entity otherAggregate3;
        bool otherIsStart3;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (!this.GetEnd(edgeList.AsArray(), out edge4, out node3, out isStart3) || !this.ShouldCombine(edge4, node3, isStart3, prefab, Entity.Null, isTemp, out otherAggregate3, out otherIsStart3))
          return false;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<AggregateElement> aggregateElement = this.m_AggregateElements[otherAggregate3];
        int length = aggregateElement.Length;
        aggregateElement.ResizeUninitialized(aggregateElement.Length + edgeList.Length);
        if (otherIsStart3)
        {
          for (int index = length - 1; index >= 0; --index)
            aggregateElement[edgeList.Length + index] = aggregateElement[index];
        }
        for (int index = 0; index < edgeList.Length; ++index)
        {
          AggregateElement edge5 = edgeList[index];
          // ISSUE: reference to a compiler-generated field
          this.m_AggregatedData[edge5.m_Edge] = new Aggregated()
          {
            m_Aggregate = otherAggregate3
          };
          aggregateElement[math.select(length, 0, otherIsStart3) + index] = edge5;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(otherAggregate3);
        return true;
      }

      private bool GetBestConnectionEdge(
        Entity prefab,
        Entity prevEdge,
        Entity prevNode,
        bool prevIsStart,
        out Entity nextEdge,
        out Entity nextNode,
        out bool nextIsStart)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve1 = this.m_CurveData[prevEdge];
        float3 float3;
        float2 float2_1;
        float2 x1;
        float2 xz;
        if (prevIsStart)
        {
          float3 = MathUtils.StartTangent(curve1.m_Bezier);
          float2_1 = math.normalizesafe(-float3.xz);
          x1 = math.normalizesafe(curve1.m_Bezier.a.xz - curve1.m_Bezier.d.xz);
          xz = curve1.m_Bezier.a.xz;
        }
        else
        {
          float3 = MathUtils.EndTangent(curve1.m_Bezier);
          float2_1 = math.normalizesafe(float3.xz);
          x1 = math.normalizesafe(curve1.m_Bezier.d.xz - curve1.m_Bezier.a.xz);
          xz = curve1.m_Bezier.d.xz;
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge1 = this.m_ConnectedEdges[prevNode];
        float num1 = 2f;
        nextEdge = Entity.Null;
        nextNode = Entity.Null;
        nextIsStart = false;
        for (int index = 0; index < connectedEdge1.Length; ++index)
        {
          ConnectedEdge connectedEdge2 = connectedEdge1[index];
          if (!(connectedEdge2.m_Edge == prevEdge))
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge = this.m_EdgeData[connectedEdge2.m_Edge];
            if (edge.m_Start == prevNode)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.GetAggregateType(connectedEdge2.m_Edge) == prefab)
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve2 = this.m_CurveData[connectedEdge2.m_Edge];
                float3 = MathUtils.StartTangent(curve2.m_Bezier);
                float2 float2_2 = math.normalizesafe(-float3.xz);
                float2 y = math.normalizesafe(curve2.m_Bezier.a.xz - curve2.m_Bezier.d.xz);
                float2 x2 = curve2.m_Bezier.a.xz - xz;
                float num2 = (float) (0.5 - 0.5 / (1.0 + (double) (math.abs(math.dot(x2, MathUtils.Right(float2_1))) + math.abs(math.dot(x2, MathUtils.Right(float2_2)))) * 0.10000000149011612));
                float num3 = math.dot(float2_1, float2_2) + math.dot(x1, y) * 0.5f + num2;
                if ((double) num3 < (double) num1)
                {
                  num1 = num3;
                  nextEdge = connectedEdge2.m_Edge;
                  nextNode = edge.m_End;
                  nextIsStart = false;
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (edge.m_End == prevNode && this.GetAggregateType(connectedEdge2.m_Edge) == prefab)
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve3 = this.m_CurveData[connectedEdge2.m_Edge];
                float3 = MathUtils.EndTangent(curve3.m_Bezier);
                float2 float2_3 = math.normalizesafe(float3.xz);
                float2 y = math.normalizesafe(curve3.m_Bezier.d.xz - curve3.m_Bezier.a.xz);
                float2 x3 = curve3.m_Bezier.d.xz - xz;
                float num4 = (float) (0.5 - 0.5 / (1.0 + (double) (math.abs(math.dot(x3, MathUtils.Right(float2_1))) + math.abs(math.dot(x3, MathUtils.Right(float2_3)))) * 0.10000000149011612));
                float num5 = math.dot(float2_1, float2_3) + math.dot(x1, y) * 0.5f + num4;
                if ((double) num5 < (double) num1)
                {
                  num1 = num5;
                  nextEdge = connectedEdge2.m_Edge;
                  nextNode = edge.m_Start;
                  nextIsStart = true;
                }
              }
            }
          }
        }
        return nextEdge != Entity.Null;
      }

      private void AddElements(
        Entity startEdge,
        Entity startNode,
        bool isStartNode,
        Entity prefab,
        NativeParallelHashSet<Entity> emptySet,
        NativeList<AggregateElement> elements)
      {
        Entity nextEdge1;
        Entity nextNode;
        bool nextIsStart;
        Entity nextEdge2;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        for (; this.GetBestConnectionEdge(prefab, startEdge, startNode, isStartNode, out nextEdge1, out nextNode, out nextIsStart) && this.GetBestConnectionEdge(prefab, nextEdge1, startNode, !nextIsStart, out nextEdge2, out Entity _, out bool _) && nextEdge2 == startEdge && emptySet.Contains(nextEdge1); isStartNode = nextIsStart)
        {
          elements.Add(new AggregateElement(nextEdge1));
          emptySet.Remove(nextEdge1);
          startEdge = nextEdge1;
          startNode = nextNode;
        }
      }

      private void CombineAggregate(
        Entity aggregate,
        NativeParallelHashMap<Entity, Entity> updateMap)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<AggregateElement> aggregateElement1 = this.m_AggregateElements[aggregate];
        // ISSUE: reference to a compiler-generated field
        Entity prefab = this.m_PrefabRefData[aggregate].m_Prefab;
        // ISSUE: reference to a compiler-generated field
        bool isTemp = this.m_TempData.HasComponent(aggregate);
        Entity edge1;
        Entity node1;
        bool isStart1;
        Entity otherAggregate1;
        bool otherIsStart1;
        Aggregated aggregated1;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        while (this.GetStart(aggregateElement1.AsNativeArray(), out edge1, out node1, out isStart1) && this.ShouldCombine(edge1, node1, isStart1, prefab, aggregate, isTemp, out otherAggregate1, out otherIsStart1))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<AggregateElement> aggregateElement2 = this.m_AggregateElements[otherAggregate1];
          int length = aggregateElement1.Length;
          aggregateElement1.ResizeUninitialized(aggregateElement2.Length + aggregateElement1.Length);
          for (int index = length - 1; index >= 0; --index)
            aggregateElement1[aggregateElement2.Length + index] = aggregateElement1[index];
          for (int index = 0; index < aggregateElement2.Length; ++index)
          {
            AggregateElement aggregateElement3 = aggregateElement2[index];
            // ISSUE: reference to a compiler-generated field
            ref ComponentLookup<Aggregated> local = ref this.m_AggregatedData;
            Entity edge2 = aggregateElement3.m_Edge;
            aggregated1 = new Aggregated();
            aggregated1.m_Aggregate = aggregate;
            Aggregated aggregated2 = aggregated1;
            local[edge2] = aggregated2;
            aggregateElement1[math.select(index, aggregateElement2.Length - index - 1, otherIsStart1)] = aggregateElement3;
          }
          aggregateElement2.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(otherAggregate1);
          if (updateMap.ContainsKey(otherAggregate1))
            updateMap[otherAggregate1] = aggregate;
        }
        Entity edge3;
        Entity node2;
        bool isStart2;
        Entity otherAggregate2;
        bool otherIsStart2;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        while (this.GetEnd(aggregateElement1.AsNativeArray(), out edge3, out node2, out isStart2) && this.ShouldCombine(edge3, node2, isStart2, prefab, aggregate, isTemp, out otherAggregate2, out otherIsStart2))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<AggregateElement> aggregateElement4 = this.m_AggregateElements[otherAggregate2];
          int length = aggregateElement1.Length;
          aggregateElement1.ResizeUninitialized(aggregateElement4.Length + aggregateElement1.Length);
          for (int index = 0; index < aggregateElement4.Length; ++index)
          {
            AggregateElement aggregateElement5 = aggregateElement4[index];
            // ISSUE: reference to a compiler-generated field
            ref ComponentLookup<Aggregated> local = ref this.m_AggregatedData;
            Entity edge4 = aggregateElement5.m_Edge;
            aggregated1 = new Aggregated();
            aggregated1.m_Aggregate = aggregate;
            Aggregated aggregated3 = aggregated1;
            local[edge4] = aggregated3;
            aggregateElement1[length + math.select(index, aggregateElement4.Length - index - 1, otherIsStart2)] = aggregateElement5;
          }
          aggregateElement4.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(otherAggregate2);
          if (updateMap.ContainsKey(otherAggregate2))
            updateMap[otherAggregate2] = aggregate;
        }
      }

      private bool GetStart(
        NativeArray<AggregateElement> elements,
        out Entity edge,
        out Entity node,
        out bool isStart)
      {
        if (elements.Length == 0)
        {
          edge = Entity.Null;
          node = Entity.Null;
          isStart = false;
          return false;
        }
        if (elements.Length == 1)
        {
          edge = elements[0].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge1 = this.m_EdgeData[edge];
          node = edge1.m_Start;
          isStart = true;
          return true;
        }
        edge = elements[0].m_Edge;
        Entity edge2 = elements[1].m_Edge;
        // ISSUE: reference to a compiler-generated field
        Edge edge3 = this.m_EdgeData[edge];
        // ISSUE: reference to a compiler-generated field
        Edge edge4 = this.m_EdgeData[edge2];
        if (edge3.m_End == edge4.m_Start || edge3.m_End == edge4.m_End)
        {
          node = edge3.m_Start;
          isStart = true;
        }
        else
        {
          node = edge3.m_End;
          isStart = false;
        }
        return true;
      }

      private bool GetEnd(
        NativeArray<AggregateElement> elements,
        out Entity edge,
        out Entity node,
        out bool isStart)
      {
        if (elements.Length == 0)
        {
          edge = Entity.Null;
          node = Entity.Null;
          isStart = false;
          return false;
        }
        if (elements.Length == 1)
        {
          edge = elements[0].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge1 = this.m_EdgeData[edge];
          node = edge1.m_End;
          isStart = false;
          return true;
        }
        edge = elements[elements.Length - 1].m_Edge;
        Entity edge2 = elements[elements.Length - 2].m_Edge;
        // ISSUE: reference to a compiler-generated field
        Edge edge3 = this.m_EdgeData[edge];
        // ISSUE: reference to a compiler-generated field
        Edge edge4 = this.m_EdgeData[edge2];
        if (edge3.m_End == edge4.m_Start || edge3.m_End == edge4.m_End)
        {
          node = edge3.m_Start;
          isStart = true;
        }
        else
        {
          node = edge3.m_End;
          isStart = false;
        }
        return true;
      }

      private void ValidateAggregate(
        Entity aggregate,
        NativeParallelHashSet<Entity> edgeSet,
        NativeParallelHashSet<Entity> emptySet,
        NativeParallelHashMap<Entity, Entity> updateMap)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<AggregateElement> aggregateElement1 = this.m_AggregateElements[aggregate];
        Entity edge1 = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        Entity prefab = this.m_PrefabRefData[aggregate].m_Prefab;
        for (int index = 0; index < aggregateElement1.Length; ++index)
        {
          AggregateElement aggregateElement2 = aggregateElement1[index];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(aggregateElement2.m_Edge))
          {
            // ISSUE: reference to a compiler-generated method
            if (this.GetAggregateType(aggregateElement2.m_Edge) != prefab)
            {
              emptySet.Add(aggregateElement2.m_Edge);
              // ISSUE: reference to a compiler-generated field
              this.m_AggregatedData[aggregateElement2.m_Edge] = new Aggregated();
            }
            else if (edge1 == Entity.Null)
              edge1 = aggregateElement2.m_Edge;
            else
              edgeSet.Add(aggregateElement2.m_Edge);
          }
        }
        aggregateElement1.Clear();
        if (edge1 == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(aggregate);
          if (updateMap.ContainsKey(aggregate))
            updateMap[aggregate] = Entity.Null;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Edge edge2 = this.m_EdgeData[edge1];
          // ISSUE: reference to a compiler-generated method
          this.AddElements(edge1, edge2.m_Start, true, prefab, edgeSet, aggregateElement1);
          CollectionUtils.Reverse<AggregateElement>(aggregateElement1.AsNativeArray());
          int length = aggregateElement1.Length;
          aggregateElement1.Add(new AggregateElement(edge1));
          // ISSUE: reference to a compiler-generated method
          this.AddElements(edge1, edge2.m_End, false, prefab, edgeSet, aggregateElement1);
          if (length > aggregateElement1.Length - length - 1)
            CollectionUtils.Reverse<AggregateElement>(aggregateElement1.AsNativeArray());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(aggregate);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(aggregate);
        }
        if (edgeSet.IsEmpty)
          return;
        NativeArray<Entity> nativeArray = edgeSet.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Entity entity = nativeArray[index];
          emptySet.Add(entity);
          // ISSUE: reference to a compiler-generated field
          this.m_AggregatedData[entity] = new Aggregated();
        }
        nativeArray.Dispose();
        edgeSet.Clear();
      }

      private bool ShouldCombine(
        Entity startEdge,
        Entity startNode,
        bool isStartNode,
        Entity prefab,
        Entity aggregate,
        bool isTemp,
        out Entity otherAggregate,
        out bool otherIsStart)
      {
        Entity nextEdge1;
        Entity nextNode;
        bool nextIsStart;
        Entity nextEdge2;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (this.GetBestConnectionEdge(prefab, startEdge, startNode, isStartNode, out nextEdge1, out nextNode, out nextIsStart) && this.GetBestConnectionEdge(prefab, nextEdge1, startNode, !nextIsStart, out nextEdge2, out nextNode, out bool _) && nextEdge2 == startEdge && this.m_AggregatedData.HasComponent(nextEdge1))
        {
          // ISSUE: reference to a compiler-generated field
          Aggregated aggregated = this.m_AggregatedData[nextEdge1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (aggregated.m_Aggregate != aggregate && this.m_AggregateElements.HasBuffer(aggregated.m_Aggregate) && this.m_TempData.HasComponent(aggregated.m_Aggregate) == isTemp)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<AggregateElement> aggregateElement = this.m_AggregateElements[aggregated.m_Aggregate];
            if (aggregateElement[0].m_Edge == nextEdge1)
            {
              otherAggregate = aggregated.m_Aggregate;
              otherIsStart = true;
              return true;
            }
            if (aggregateElement[aggregateElement.Length - 1].m_Edge == nextEdge1)
            {
              otherAggregate = aggregated.m_Aggregate;
              otherIsStart = false;
              return true;
            }
          }
        }
        otherAggregate = Entity.Null;
        otherIsStart = false;
        return false;
      }

      private void AddElements(
        Entity startEdge,
        Entity startNode,
        bool isStartNode,
        Entity prefab,
        NativeParallelHashSet<Entity> edgeSet,
        DynamicBuffer<AggregateElement> elements)
      {
        Entity nextEdge1;
        Entity nextNode;
        bool nextIsStart;
        Entity nextEdge2;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        for (; this.GetBestConnectionEdge(prefab, startEdge, startNode, isStartNode, out nextEdge1, out nextNode, out nextIsStart) && this.GetBestConnectionEdge(prefab, nextEdge1, startNode, !nextIsStart, out nextEdge2, out Entity _, out bool _) && nextEdge2 == startEdge && edgeSet.Contains(nextEdge1); isStartNode = nextIsStart)
        {
          elements.Add(new AggregateElement(nextEdge1));
          edgeSet.Remove(nextEdge1);
          startEdge = nextEdge1;
          startNode = nextNode;
        }
      }

      private Entity GetAggregateType(Entity edge)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabGeometryData[this.m_PrefabRefData[edge].m_Prefab].m_AggregateType;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AggregateNetData> __Game_Prefabs_AggregateNetData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      public ComponentLookup<Aggregated> __Game_Net_Aggregated_RW_ComponentLookup;
      public BufferLookup<AggregateElement> __Game_Net_AggregateElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AggregateNetData_RO_ComponentLookup = state.GetComponentLookup<AggregateNetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Aggregated_RW_ComponentLookup = state.GetComponentLookup<Aggregated>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_AggregateElement_RW_BufferLookup = state.GetBufferLookup<AggregateElement>();
      }
    }
  }
}
