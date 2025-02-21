// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ConnectedEdgeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class ConnectedEdgeSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private ConnectedEdgeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Net.Edge>(),
          ComponentType.ReadWrite<ConnectedNode>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ConnectedEdgeSystem.ConnectedEdgeJob jobData = new ConnectedEdgeSystem.ConnectedEdgeJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_ConnectedNodeType = this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferTypeHandle,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<ConnectedEdgeSystem.ConnectedEdgeJob>(this.m_Query, this.Dependency);
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
    public ConnectedEdgeSystem()
    {
    }

    [BurstCompile]
    private struct ConnectedEdgeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> m_EdgeType;
      public BufferTypeHandle<ConnectedNode> m_ConnectedNodeType;
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Edge> nativeArray2 = chunk.GetNativeArray<Game.Net.Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedNode> bufferAccessor = chunk.GetBufferAccessor<ConnectedNode>(ref this.m_ConnectedNodeType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity edge1 = nativeArray1[index];
          Game.Net.Edge edge2 = nativeArray2[index];
          DynamicBuffer<ConnectedEdge> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedEdges.TryGetBuffer(edge2.m_Start, out bufferData1))
            bufferData1.Add(new ConnectedEdge(edge1));
          else
            Debug.Log((object) string.Format("Start node has no ConnectedEdge: {0}:{1}", (object) edge1.Index, (object) edge1.Version));
          DynamicBuffer<ConnectedEdge> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedEdges.TryGetBuffer(edge2.m_End, out bufferData2))
            bufferData2.Add(new ConnectedEdge(edge1));
          else
            Debug.Log((object) string.Format("End node has no ConnectedEdge: {0}:{1}", (object) edge1.Index, (object) edge1.Version));
        }
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          Entity edge = nativeArray1[index1];
          DynamicBuffer<ConnectedNode> dynamicBuffer = bufferAccessor[index1];
label_20:
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            ConnectedNode connectedNode = dynamicBuffer[index2];
            for (int index3 = 0; index3 < index2; ++index3)
            {
              if (dynamicBuffer[index3].m_Node == connectedNode.m_Node)
              {
                dynamicBuffer.RemoveAt(index2--);
                goto label_20;
              }
            }
            DynamicBuffer<ConnectedEdge> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedEdges.TryGetBuffer(connectedNode.m_Node, out bufferData))
            {
              bufferData.Add(new ConnectedEdge(edge));
            }
            else
            {
              Debug.Log((object) string.Format("Middle node has no ConnectedEdge: {0}:{1}", (object) edge.Index, (object) edge.Version));
              dynamicBuffer.RemoveAt(index2--);
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      public BufferTypeHandle<ConnectedNode> __Game_Net_ConnectedNode_RW_BufferTypeHandle;
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RW_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RW_BufferLookup = state.GetBufferLookup<ConnectedEdge>();
      }
    }
  }
}
