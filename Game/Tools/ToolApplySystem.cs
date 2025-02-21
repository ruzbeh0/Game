// Decompiled with JetBrains decompiler
// Type: Game.Tools.ToolApplySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ToolApplySystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private CitySystem m_CitySystem;
    private EntityQuery m_ApplyQuery;
    private ToolApplySystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Warning>(),
          ComponentType.ReadOnly<Override>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ApplyQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_ApplyQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Override_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new ToolApplySystem.ApplyEntitiesJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_WarningType = this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle,
        m_OverrideType = this.__TypeHandle.__Game_Tools_Override_RO_ComponentTypeHandle,
        m_City = this.m_CitySystem.City,
        m_PlayerMoney = this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
      }.Schedule<ToolApplySystem.ApplyEntitiesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
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
    public ToolApplySystem()
    {
    }

    [BurstCompile]
    private struct ApplyEntitiesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Warning> m_WarningType;
      [ReadOnly]
      public ComponentTypeHandle<Override> m_OverrideType;
      [ReadOnly]
      public Entity m_City;
      public ComponentLookup<PlayerMoney> m_PlayerMoney;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        int amount = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Warning>(ref this.m_WarningType))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(nativeArray1);
          }
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Override>(ref this.m_OverrideType))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(nativeArray1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Overridden>(nativeArray1);
          }
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Temp temp = nativeArray2[index2];
            if ((temp.m_Flags & TempFlags.Cancel) == (TempFlags) 0)
              amount += temp.m_Cost;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (amount == 0 || !this.m_PlayerMoney.HasComponent(this.m_City))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PlayerMoney playerMoney = this.m_PlayerMoney[this.m_City];
        playerMoney.Subtract(amount);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PlayerMoney[this.m_City] = playerMoney;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Warning> __Game_Tools_Warning_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Override> __Game_Tools_Override_RO_ComponentTypeHandle;
      public ComponentLookup<PlayerMoney> __Game_City_PlayerMoney_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Warning_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Warning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Override_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Override>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_PlayerMoney_RW_ComponentLookup = state.GetComponentLookup<PlayerMoney>();
      }
    }
  }
}
