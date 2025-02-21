// Decompiled with JetBrains decompiler
// Type: Game.Net.FlipTrafficHandednessSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class FlipTrafficHandednessSystem : GameSystemBase
  {
    private EntityQuery m_RoadEdgeQuery;
    private FlipTrafficHandednessSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RoadEdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Road>(), ComponentType.ReadOnly<Edge>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RoadEdgeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_BuildOrder_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      FlipTrafficHandednessSystem.FlipOnewayRoadsJob jobData = new FlipTrafficHandednessSystem.FlipOnewayRoadsJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RW_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RW_ComponentTypeHandle,
        m_ElevationType = this.__TypeHandle.__Game_Net_Elevation_RW_ComponentTypeHandle,
        m_UpgradedType = this.__TypeHandle.__Game_Net_Upgraded_RW_ComponentTypeHandle,
        m_BuildOrderType = this.__TypeHandle.__Game_Net_BuildOrder_RW_ComponentTypeHandle,
        m_ConnectedNodeType = this.__TypeHandle.__Game_Net_ConnectedNode_RW_BufferTypeHandle,
        m_ServiceCoverageType = this.__TypeHandle.__Game_Net_ServiceCoverage_RW_BufferTypeHandle,
        m_ResourceAvailabilityType = this.__TypeHandle.__Game_Net_ResourceAvailability_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<FlipTrafficHandednessSystem.FlipOnewayRoadsJob>(this.m_RoadEdgeQuery, this.Dependency);
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
    public FlipTrafficHandednessSystem()
    {
    }

    [BurstCompile]
    private struct FlipOnewayRoadsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      public ComponentTypeHandle<Edge> m_EdgeType;
      public ComponentTypeHandle<Curve> m_CurveType;
      public ComponentTypeHandle<Elevation> m_ElevationType;
      public ComponentTypeHandle<Upgraded> m_UpgradedType;
      public ComponentTypeHandle<BuildOrder> m_BuildOrderType;
      public BufferTypeHandle<ConnectedNode> m_ConnectedNodeType;
      public BufferTypeHandle<ServiceCoverage> m_ServiceCoverageType;
      public BufferTypeHandle<ResourceAvailability> m_ResourceAvailabilityType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Edge> nativeArray2 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Elevation> nativeArray4 = chunk.GetNativeArray<Elevation>(ref this.m_ElevationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Upgraded> nativeArray5 = chunk.GetNativeArray<Upgraded>(ref this.m_UpgradedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<BuildOrder> nativeArray6 = chunk.GetNativeArray<BuildOrder>(ref this.m_BuildOrderType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedNode> bufferAccessor1 = chunk.GetBufferAccessor<ConnectedNode>(ref this.m_ConnectedNodeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceCoverage> bufferAccessor2 = chunk.GetBufferAccessor<ServiceCoverage>(ref this.m_ServiceCoverageType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ResourceAvailability> bufferAccessor3 = chunk.GetBufferAccessor<ResourceAvailability>(ref this.m_ResourceAvailabilityType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          PrefabRef prefabRef = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabGeometryData[prefabRef.m_Prefab].m_Flags & GeometryFlags.FlipTrafficHandedness) == (GeometryFlags) 0)
            {
              if (nativeArray5.Length != 0)
              {
                Upgraded upgraded = nativeArray5[index1];
                NetUtils.FlipUpgradeTrafficHandedness(ref upgraded.m_Flags);
                nativeArray5[index1] = upgraded;
              }
            }
            else
            {
              Edge edge = nativeArray2[index1];
              CommonUtils.Swap<Entity>(ref edge.m_Start, ref edge.m_End);
              nativeArray2[index1] = edge;
              Curve curve = nativeArray3[index1];
              curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
              nativeArray3[index1] = curve;
              if (nativeArray4.Length != 0)
              {
                Elevation elevation = nativeArray4[index1];
                elevation.m_Elevation = elevation.m_Elevation.yx;
                nativeArray4[index1] = elevation;
              }
              if (nativeArray6.Length != 0)
              {
                BuildOrder buildOrder = nativeArray6[index1];
                CommonUtils.Swap<uint>(ref buildOrder.m_Start, ref buildOrder.m_End);
                nativeArray6[index1] = buildOrder;
              }
              if (bufferAccessor1.Length != 0)
              {
                DynamicBuffer<ConnectedNode> dynamicBuffer = bufferAccessor1[index1];
                for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
                {
                  ConnectedNode connectedNode = dynamicBuffer[index2];
                  connectedNode.m_CurvePosition = math.saturate(1f - connectedNode.m_CurvePosition);
                  dynamicBuffer[index2] = connectedNode;
                }
              }
              if (bufferAccessor2.Length != 0)
              {
                DynamicBuffer<ServiceCoverage> dynamicBuffer = bufferAccessor2[index1];
                for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
                {
                  ServiceCoverage serviceCoverage = dynamicBuffer[index3];
                  serviceCoverage.m_Coverage = serviceCoverage.m_Coverage.yx;
                  dynamicBuffer[index3] = serviceCoverage;
                }
              }
              if (bufferAccessor3.Length != 0)
              {
                DynamicBuffer<ResourceAvailability> dynamicBuffer = bufferAccessor3[index1];
                for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
                {
                  ResourceAvailability resourceAvailability = dynamicBuffer[index4];
                  resourceAvailability.m_Availability = resourceAvailability.m_Availability.yx;
                  dynamicBuffer[index4] = resourceAvailability;
                }
              }
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Elevation> __Game_Net_Elevation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Upgraded> __Game_Net_Upgraded_RW_ComponentTypeHandle;
      public ComponentTypeHandle<BuildOrder> __Game_Net_BuildOrder_RW_ComponentTypeHandle;
      public BufferTypeHandle<ConnectedNode> __Game_Net_ConnectedNode_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceCoverage> __Game_Net_ServiceCoverage_RW_BufferTypeHandle;
      public BufferTypeHandle<ResourceAvailability> __Game_Net_ResourceAvailability_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Elevation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Upgraded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_BuildOrder_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BuildOrder>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RW_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceCoverage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RW_BufferTypeHandle = state.GetBufferTypeHandle<ResourceAvailability>();
      }
    }
  }
}
