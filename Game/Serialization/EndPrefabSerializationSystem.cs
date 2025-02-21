// Decompiled with JetBrains decompiler
// Type: Game.Serialization.EndPrefabSerializationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class EndPrefabSerializationSystem : GameSystemBase
  {
    private SaveGameSystem m_SaveGameSystem;
    private EntityQuery m_LoadedPrefabsQuery;
    private EntityQuery m_ContentPrefabQuery;
    private EndPrefabSerializationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SaveGameSystem = this.World.GetOrCreateSystemManaged<SaveGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedPrefabsQuery = this.GetEntityQuery(ComponentType.ReadOnly<LoadedIndex>());
      // ISSUE: reference to a compiler-generated field
      this.m_ContentPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<ContentData>(), ComponentType.ReadOnly<PrefabData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SaveGameSystem.referencedContent.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SaveGameSystem.referencedContent.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SaveGameSystem.referencedContent = this.m_ContentPrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LoadedIndex_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new EndPrefabSerializationSystem.EndPrefabSerializationJob()
      {
        m_LoadedIndexType = this.__TypeHandle.__Game_Prefabs_LoadedIndex_RO_BufferTypeHandle,
        m_PrefabDataType = this.__TypeHandle.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle
      }.ScheduleParallel<EndPrefabSerializationSystem.EndPrefabSerializationJob>(this.m_LoadedPrefabsQuery, this.Dependency).Complete();
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
    public EndPrefabSerializationSystem()
    {
    }

    [BurstCompile]
    private struct EndPrefabSerializationJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<LoadedIndex> m_LoadedIndexType;
      public ComponentTypeHandle<PrefabData> m_PrefabDataType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabData> nativeArray = chunk.GetNativeArray<PrefabData>(ref this.m_PrefabDataType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LoadedIndex> bufferAccessor = chunk.GetBufferAccessor<LoadedIndex>(ref this.m_LoadedIndexType);
        // ISSUE: reference to a compiler-generated field
        EnabledMask enabledMask = chunk.GetEnabledMask<PrefabData>(ref this.m_PrefabDataType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          PrefabData prefabData;
          if (enabledMask[index])
          {
            DynamicBuffer<LoadedIndex> dynamicBuffer = bufferAccessor[index];
            prefabData.m_Index = dynamicBuffer[0].m_Index;
            nativeArray[index] = prefabData;
          }
          else
            prefabData = nativeArray[index];
          enabledMask[index] = prefabData.m_Index >= 0;
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
      public BufferTypeHandle<LoadedIndex> __Game_Prefabs_LoadedIndex_RO_BufferTypeHandle;
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LoadedIndex_RO_BufferTypeHandle = state.GetBufferTypeHandle<LoadedIndex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>();
      }
    }
  }
}
