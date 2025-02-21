// Decompiled with JetBrains decompiler
// Type: Game.Net.NodeAlignSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class NodeAlignSystem : GameSystemBase
  {
    private EntityQuery m_NodeQuery;
    private NodeAlignSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Node>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_NodeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Standalone_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new NodeAlignSystem.UpdateNodeRotationsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_StandaloneType = this.__TypeHandle.__Game_Net_Standalone_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RW_ComponentTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup
      }.ScheduleParallel<NodeAlignSystem.UpdateNodeRotationsJob>(this.m_NodeQuery, this.Dependency);
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
    public NodeAlignSystem()
    {
    }

    [BurstCompile]
    private struct UpdateNodeRotationsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Standalone> m_StandaloneType;
      public ComponentTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Node> nativeArray2 = chunk.GetNativeArray<Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        bool isStandalone = chunk.Has<Standalone>(ref this.m_StandaloneType);
        NativeList<float> angleBuffer = new NativeList<float>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<Line2.Segment> lineBuffer = new NativeList<Line2.Segment>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Node node = nativeArray2[index];
          // ISSUE: reference to a compiler-generated method
          this.AlignNode(entity, isStandalone, angleBuffer, lineBuffer, ref node);
          nativeArray2[index] = node;
          angleBuffer.Clear();
          lineBuffer.Clear();
        }
        angleBuffer.Dispose();
        lineBuffer.Dispose();
      }

      private void AlignNode(
        Entity entity,
        bool isStandalone,
        NativeList<float> angleBuffer,
        NativeList<Line2.Segment> lineBuffer,
        ref Node node)
      {
        float2 y = new float2();
        float3 float3_1 = new float3();
        float3 position = node.m_Position;
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, entity, this.m_Edges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        float3 float3_2;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[edgeIteratorValue.m_Edge];
          float3 float3_3;
          float2 _b;
          if (edgeIteratorValue.m_End)
          {
            float3_3 = curve.m_Bezier.d - position;
            float3_2 = MathUtils.EndTangent(curve.m_Bezier);
            _b = -float3_2.xz;
            y -= _b;
          }
          else
          {
            float3_3 = curve.m_Bezier.a - position;
            float3_2 = MathUtils.StartTangent(curve.m_Bezier);
            _b = float3_2.xz;
            y += _b;
          }
          float3_1 += float3_3;
          ++num1;
          if (MathUtils.TryNormalize(ref _b))
          {
            float x1 = (float) ((double) math.atan2(_b.x, -_b.y) * 0.15915493667125702 + 1.25);
            angleBuffer.Add(x1 - math.floor(x1));
            float x2 = x1 + 0.5f;
            angleBuffer.Add(x2 - math.floor(x2));
            lineBuffer.Add(new Line2.Segment(float3_3.xz, _b));
          }
        }
        if (!isStandalone)
        {
          if (num1 > 0)
          {
            node.m_Position = position + float3_1 / (float) num1;
            float3_1 = new float3();
            num1 = 0;
          }
          if (lineBuffer.Length >= 2)
          {
            NativeList<Line2.Segment> list = lineBuffer;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            NodeAlignSystem.LineComparer lineComparer = new NodeAlignSystem.LineComparer();
            // ISSUE: variable of a compiler-generated type
            NodeAlignSystem.LineComparer comp = lineComparer;
            list.Sort<Line2.Segment, NodeAlignSystem.LineComparer>(comp);
            for (int index1 = 1; index1 < lineBuffer.Length; ++index1)
            {
              Line2.Segment segment1 = lineBuffer[index1];
              for (int index2 = 0; index2 < index1; ++index2)
              {
                Line2.Segment segment2 = lineBuffer[index2];
                float x = math.dot(segment1.b, segment2.b);
                float3 float3_4 = new float3();
                if ((double) math.abs(x) > 0.99900001287460327)
                {
                  float3_4.xy = segment1.a + segment2.a;
                  float3_4.z = 2f;
                }
                else
                {
                  float2 float2 = math.distance(segment1.a, segment2.a) * new float2(math.abs(x) - 1f, 1f - math.abs(x));
                  Line2.Segment segment3 = new Line2.Segment(segment1.a - segment1.b * float2.x, segment1.a - segment1.b * float2.y);
                  Line2.Segment segment4 = new Line2.Segment(segment2.a - segment2.b * float2.x, segment2.a - segment2.b * float2.y);
                  float2 t;
                  double num2 = (double) MathUtils.Distance(segment3, segment4, out t);
                  float3_4.xy = MathUtils.Position(segment3, t.x) + MathUtils.Position(segment4, t.y);
                  float3_4.z = 2f;
                }
                float num3 = 1.01f - math.abs(x);
                float3_1 += float3_4 * num3;
                ++num1;
              }
            }
            if (num1 > 0)
              node.m_Position.xz = position.xz + float3_1.xy / float3_1.z;
          }
        }
        if (angleBuffer.Length == 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OutsideConnectionData.HasComponent(entity))
            return;
          float2 float2 = math.abs(node.m_Position.xz);
          if ((double) float2.x > (double) float2.y)
          {
            node.m_Rotation = quaternion.LookRotation(new float3(-math.sign(node.m_Position.x), 0.0f, 0.0f), math.up());
          }
          else
          {
            if ((double) float2.y <= (double) float2.x)
              return;
            node.m_Rotation = quaternion.LookRotation(new float3(0.0f, 0.0f, -math.sign(node.m_Position.z)), math.up());
          }
        }
        else
        {
          float num4;
          if (angleBuffer.Length == 2)
          {
            num4 = angleBuffer[0] + 0.75f;
          }
          else
          {
            angleBuffer.Sort<float>();
            float num5 = angleBuffer[angleBuffer.Length - 1];
            float num6 = angleBuffer[0];
            float num7 = num6 + 1f - num5;
            num4 = (float) (((double) num5 + (double) num6) * 0.5);
            for (int index = 1; index < angleBuffer.Length; ++index)
            {
              float num8 = angleBuffer[index - 1];
              float num9 = angleBuffer[index];
              float num10 = num9 - num8;
              if ((double) num10 > (double) num7)
              {
                num7 = num10;
                num4 = (float) (((double) num8 + (double) num9) * 0.5);
              }
            }
          }
          node.m_Rotation = quaternion.RotateY(num4 * -6.28318548f);
          float3_2 = math.rotate(node.m_Rotation, new float3(0.0f, 0.0f, 1f));
          if ((double) math.dot(float3_2.xz, y) >= 0.0)
            return;
          node.m_Rotation = quaternion.RotateY((float) (3.1415927410125732 - (double) num4 * 6.2831854820251465));
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

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct LineComparer : IComparer<Line2.Segment>
    {
      public int Compare(Line2.Segment x, Line2.Segment y)
      {
        return math.csum(math.select((int4) 0, math.select(new int4(-8, -4, -2, -1), new int4(8, 4, 2, 1), x.ab > y.ab), x.ab != y.ab));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Standalone> __Game_Net_Standalone_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Node> __Game_Net_Node_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Standalone_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Standalone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Node>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
      }
    }
  }
}
