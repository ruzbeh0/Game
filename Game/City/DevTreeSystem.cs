// Decompiled with JetBrains decompiler
// Type: Game.City.DevTreeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.City
{
  [CompilerGenerated]
  public class DevTreeSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_MilestoneReachedQuery;
    private EntityQuery m_DevTreePointsQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private PrefabSystem m_PrefabSystem;
    private DevTreeSystem.TypeHandle __TypeHandle;

    public int points
    {
      set
      {
        if (this.m_DevTreePointsQuery.IsEmptyIgnoreFilter)
          return;
        this.m_DevTreePointsQuery.SetSingleton<DevTreePoints>(new DevTreePoints()
        {
          m_Points = value
        });
      }
      get
      {
        return !this.m_DevTreePointsQuery.IsEmptyIgnoreFilter ? this.m_DevTreePointsQuery.GetSingleton<DevTreePoints>().m_Points : 0;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneReachedQuery = this.GetEntityQuery(ComponentType.ReadOnly<MilestoneReachedEvent>());
      // ISSUE: reference to a compiler-generated field
      this.m_DevTreePointsQuery = this.GetEntityQuery(ComponentType.ReadWrite<DevTreePoints>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Unlock>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DevTreePointsQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_MilestoneReachedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_DevTreePoints_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MilestoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DevTreeSystem.AppendPointsJob jobData = new DevTreeSystem.AppendPointsJob()
      {
        m_Chunks = this.m_DevTreePointsQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1),
        m_MilestoneReached = this.m_MilestoneReachedQuery.ToComponentDataListAsync<MilestoneReachedEvent>((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2),
        m_Milestones = this.__TypeHandle.__Game_Prefabs_MilestoneData_RO_ComponentLookup,
        m_PointsType = this.__TypeHandle.__Game_City_DevTreePoints_RW_ComponentTypeHandle
      };
      this.Dependency = jobData.Schedule<DevTreeSystem.AppendPointsJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle1, outJobHandle2));
      // ISSUE: reference to a compiler-generated field
      jobData.m_Chunks.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      jobData.m_MilestoneReached.Dispose(this.Dependency);
    }

    public void Purchase(DevTreeNodePrefab nodePrefab)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_DevTreePointsQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.Purchase(this.m_PrefabSystem.GetEntity((PrefabBase) nodePrefab));
    }

    public void Purchase(Entity node)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_DevTreePointsQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DevTreeNodeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<DevTreeNodeData> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_DevTreeNodeData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Locked> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup;
      DevTreeNodeData devTreeNodeData = roComponentLookup1[node];
      int points = this.points;
      // ISSUE: reference to a compiler-generated method
      if (devTreeNodeData.m_Cost > points || !roComponentLookup2.HasEnabledComponent<Locked>(node) || !DevTreeSystem.CheckService(devTreeNodeData.m_Service, roComponentLookup2))
        return;
      EntityManager entityManager = this.EntityManager;
      if (entityManager.HasComponent<DevTreeNodeRequirement>(node))
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated method
        if (!DevTreeSystem.CheckRequirements(entityManager.GetBuffer<DevTreeNodeRequirement>(node, true), roComponentLookup2))
          return;
      }
      this.points = points - devTreeNodeData.m_Cost;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      Entity entity = commandBuffer.CreateEntity(this.m_UnlockEventArchetype);
      commandBuffer.SetComponent<Unlock>(entity, new Unlock(node));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Game.PSI.Telemetry.DevNodePurchased(this.m_PrefabSystem.GetPrefab<DevTreeNodePrefab>(node));
    }

    private static bool CheckRequirements(
      DynamicBuffer<DevTreeNodeRequirement> requirements,
      ComponentLookup<Locked> locked)
    {
      bool flag = false;
      for (int index = 0; index < requirements.Length; ++index)
      {
        if (requirements[index].m_Node != Entity.Null)
        {
          flag = true;
          if (!locked.HasEnabledComponent<Locked>(requirements[index].m_Node))
            return true;
        }
      }
      return !flag;
    }

    private static bool CheckService(Entity service, ComponentLookup<Locked> locked)
    {
      return service == Entity.Null || !locked.HasEnabledComponent<Locked>(service);
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
    public DevTreeSystem()
    {
    }

    [BurstCompile]
    private struct AppendPointsJob : IJob
    {
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public NativeList<MilestoneReachedEvent> m_MilestoneReached;
      [ReadOnly]
      public ComponentLookup<MilestoneData> m_Milestones;
      public ComponentTypeHandle<DevTreePoints> m_PointsType;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<DevTreePoints> nativeArray = this.m_Chunks[0].GetNativeArray<DevTreePoints>(ref this.m_PointsType);
        int points = nativeArray[0].m_Points;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_MilestoneReached.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          points += this.m_MilestoneReached[index].m_Milestone != Entity.Null ? this.m_Milestones[this.m_MilestoneReached[index].m_Milestone].m_DevTreePoints : this.GetDefaultPoints(this.m_MilestoneReached[index].m_Index);
        }
        nativeArray[0] = new DevTreePoints()
        {
          m_Points = points
        };
      }

      private int GetDefaultPoints(int level)
      {
        if (level <= 0)
          return 0;
        return level >= 19 ? 10 : (level + 1) / 2 + 1;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<MilestoneData> __Game_Prefabs_MilestoneData_RO_ComponentLookup;
      public ComponentTypeHandle<DevTreePoints> __Game_City_DevTreePoints_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<DevTreeNodeData> __Game_Prefabs_DevTreeNodeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MilestoneData_RO_ComponentLookup = state.GetComponentLookup<MilestoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_DevTreePoints_RW_ComponentTypeHandle = state.GetComponentTypeHandle<DevTreePoints>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DevTreeNodeData_RO_ComponentLookup = state.GetComponentLookup<DevTreeNodeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
      }
    }
  }
}
