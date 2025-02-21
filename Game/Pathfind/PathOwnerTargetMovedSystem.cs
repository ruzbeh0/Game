// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathOwnerTargetMovedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Creatures;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  [CompilerGenerated]
  public class PathOwnerTargetMovedSystem : GameSystemBase
  {
    private EntityQuery m_EventQuery;
    private EntityQuery m_PathOwnerQuery;
    private PathOwnerTargetMovedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<PathTargetMoved>(), ComponentType.ReadOnly<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_PathOwnerQuery = this.GetEntityQuery(ComponentType.ReadOnly<PathOwner>(), ComponentType.ReadOnly<PathElement>(), ComponentType.ReadOnly<Target>(), ComponentType.Exclude<GroupMember>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EventQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PathOwnerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<PathTargetMoved> componentDataArray = this.m_EventQuery.ToComponentDataArray<PathTargetMoved>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PathOwnerTargetMovedSystem.CheckPathOwnerTargetsJob jobData = new PathOwnerTargetMovedSystem.CheckPathOwnerTargetsJob()
        {
          m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
          m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
          m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle
        };
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          jobData.m_RandomSeed = RandomSeed.Next();
          // ISSUE: reference to a compiler-generated field
          jobData.m_MovedEntity = componentDataArray[index].m_Target;
          // ISSUE: reference to a compiler-generated field
          this.Dependency = jobData.ScheduleParallel<PathOwnerTargetMovedSystem.CheckPathOwnerTargetsJob>(this.m_PathOwnerQuery, this.Dependency);
        }
      }
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
    public PathOwnerTargetMovedSystem()
    {
    }

    [BurstCompile]
    private struct CheckPathOwnerTargetsJob : IJobChunk
    {
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public Entity m_MovedEntity;
      [ReadOnly]
      public ComponentTypeHandle<Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public BufferTypeHandle<PathElement> m_PathElementType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray1 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray2 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          if (nativeArray1[index1].m_Target == this.m_MovedEntity)
          {
            PathOwner pathOwner = nativeArray2[index1];
            DynamicBuffer<PathElement> dynamicBuffer = bufferAccessor[index1];
            if (pathOwner.m_ElementIndex < dynamicBuffer.Length)
            {
              int index2 = random.NextInt(pathOwner.m_ElementIndex, dynamicBuffer.Length);
              PathElement pathElement = dynamicBuffer[index2] with
              {
                m_Target = Entity.Null
              };
              dynamicBuffer[index2] = pathElement;
            }
            else
            {
              pathOwner.m_State |= PathFlags.Obsolete;
              nativeArray2[index1] = pathOwner;
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
      public ComponentTypeHandle<Target> __Game_Common_Target_RO_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>();
      }
    }
  }
}
