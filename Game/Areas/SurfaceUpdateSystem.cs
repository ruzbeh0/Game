// Decompiled with JetBrains decompiler
// Type: Game.Areas.SurfaceUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [CompilerGenerated]
  public class SurfaceUpdateSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private SearchSystem m_AreaSearchSystem;
    private EntityQuery m_UpdatedNetQuery;
    private SurfaceUpdateSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedNetQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdatedNetQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Surface_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new SurfaceUpdateSystem.UpdateAreasJob()
      {
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_SurfaceData = this.__TypeHandle.__Game_Areas_Surface_RO_ComponentLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<SurfaceUpdateSystem.UpdateAreasJob>(this.m_UpdatedNetQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
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
    public SurfaceUpdateSystem()
    {
    }

    [BurstCompile]
    private struct UpdateAreasJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      [ReadOnly]
      public ComponentLookup<Surface> m_SurfaceData;
      [ReadOnly]
      public BufferLookup<Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Node> nativeArray = chunk.GetNativeArray<Game.Net.Node>(ref this.m_NodeType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Game.Net.Node node = nativeArray[index];
          // ISSUE: reference to a compiler-generated method
          this.UpdateSurfaces(unfilteredChunkIndex, node.m_Position);
        }
      }

      private void UpdateSurfaces(int jobIndex, float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SurfaceUpdateSystem.UpdateAreasJob.SurfaceIterator iterator = new SurfaceUpdateSystem.UpdateAreasJob.SurfaceIterator()
        {
          m_Position = position,
          m_JobIndex = jobIndex,
          m_SurfaceData = this.m_SurfaceData,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles,
          m_CommandBuffer = this.m_CommandBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<SurfaceUpdateSystem.UpdateAreasJob.SurfaceIterator>(ref iterator);
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

      private struct SurfaceIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public float3 m_Position;
        public int m_JobIndex;
        public ComponentLookup<Surface> m_SurfaceData;
        public BufferLookup<Node> m_AreaNodes;
        public BufferLookup<Triangle> m_AreaTriangles;
        public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position.xz) || !this.m_SurfaceData.HasComponent(item.m_Area) || !MathUtils.Intersect(AreaUtils.GetTriangle2(this.m_AreaNodes[item.m_Area], this.m_AreaTriangles[item.m_Area][item.m_Triangle]), this.m_Position.xz))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(this.m_JobIndex, item.m_Area, new Updated());
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Surface> __Game_Areas_Surface_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Surface_RO_ComponentLookup = state.GetComponentLookup<Surface>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
      }
    }
  }
}
