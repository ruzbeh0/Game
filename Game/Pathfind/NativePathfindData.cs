// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.NativePathfindData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  [NativeContainer]
  [GenerateTestsForBurstCompatibility]
  public struct NativePathfindData : IDisposable
  {
    [NativeDisableUnsafePtrRestriction]
    internal unsafe UnsafePathfindData* m_PathfindData;
    internal Allocator m_AllocatorLabel;

    public NativePathfindData(Allocator allocator)
      : this(allocator, 2)
    {
    }

    private unsafe NativePathfindData(Allocator allocator, int disposeSentinelStackDepth)
    {
      this.m_PathfindData = (UnsafePathfindData*) UnsafeUtility.Malloc((long) UnsafeUtility.SizeOf<UnsafePathfindData>(), UnsafeUtility.AlignOf<UnsafePathfindData>(), allocator);
      *this.m_PathfindData = new UnsafePathfindData(allocator);
      this.m_AllocatorLabel = allocator;
    }

    [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
    private void CheckRead()
    {
    }

    [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
    private void CheckWrite()
    {
    }

    public unsafe void Dispose()
    {
      this.m_PathfindData->Dispose();
      UnsafeUtility.Free((void*) this.m_PathfindData, this.m_AllocatorLabel);
      this.m_PathfindData = (UnsafePathfindData*) null;
    }

    public unsafe bool IsCreated => (IntPtr) this.m_PathfindData != IntPtr.Zero;

    public unsafe int Size
    {
      get => this.m_PathfindData->m_Edges.Length - this.m_PathfindData->m_FreeIDs.Length;
    }

    public unsafe void Clear() => this.m_PathfindData->Clear();

    public unsafe void GetMemoryStats(out uint used, out uint allocated)
    {
      this.m_PathfindData->GetMemoryStats(out used, out allocated);
    }

    public unsafe EdgeID CreateEdge(
      PathNode startNode,
      PathNode middleNode,
      PathNode endNode,
      PathSpecification specification,
      LocationSpecification location)
    {
      return this.m_PathfindData->CreateEdge(startNode, middleNode, endNode, specification, location);
    }

    public unsafe void UpdateEdge(
      EdgeID edgeID,
      PathNode startNode,
      PathNode middleNode,
      PathNode endNode,
      PathSpecification specification,
      LocationSpecification location)
    {
      this.m_PathfindData->UpdateEdge(edgeID, startNode, middleNode, endNode, specification, location);
    }

    public unsafe void DestroyEdge(EdgeID edgeID) => this.m_PathfindData->DestroyEdge(edgeID);

    public unsafe void AddEdge(Entity owner, EdgeID edgeID)
    {
      this.m_PathfindData->AddEdge(owner, edgeID);
    }

    public unsafe void AddSecondaryEdge(Entity owner, EdgeID edgeID)
    {
      this.m_PathfindData->AddSecondaryEdge(owner, edgeID);
    }

    public unsafe bool GetEdge(Entity owner, out EdgeID edgeID)
    {
      return this.m_PathfindData->GetEdge(owner, out edgeID);
    }

    public unsafe bool GetSecondaryEdge(Entity owner, out EdgeID edgeID)
    {
      return this.m_PathfindData->GetSecondaryEdge(owner, out edgeID);
    }

    public unsafe bool RemoveEdge(Entity owner, out EdgeID edgeID)
    {
      return this.m_PathfindData->RemoveEdge(owner, out edgeID);
    }

    public unsafe bool RemoveSecondaryEdge(Entity owner, out EdgeID edgeID)
    {
      return this.m_PathfindData->RemoveSecondaryEdge(owner, out edgeID);
    }

    public unsafe ref float SetDensity(EdgeID edgeID)
    {
      return ref this.m_PathfindData->GetEdge(edgeID).m_Specification.m_Density;
    }

    public unsafe ref PathfindCosts SetCosts(EdgeID edgeID)
    {
      return ref this.m_PathfindData->GetEdge(edgeID).m_Specification.m_Costs;
    }

    public unsafe ref byte SetFlowOffset(EdgeID edgeID)
    {
      return ref this.m_PathfindData->GetEdge(edgeID).m_Specification.m_FlowOffset;
    }

    public unsafe EdgeFlags GetFlags(EdgeID edgeID)
    {
      return this.m_PathfindData->GetEdge(edgeID).m_Specification.m_Flags;
    }

    public unsafe void SetEdgeDirections(
      EdgeID edgeID,
      PathNode startNode,
      PathNode endNode,
      bool enableForward,
      bool enableBackward)
    {
      this.m_PathfindData->SetEdgeDirections(edgeID, startNode, endNode, enableForward, enableBackward);
    }

    public unsafe UnsafePathfindData GetReadOnlyData() => *this.m_PathfindData;
  }
}
