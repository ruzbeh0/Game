// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EffectInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Rendering;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class EffectInitializeSystem : GameSystemBase
  {
    private EntityQuery m_PrefabQuery;
    private EffectInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadWrite<Effect>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new EffectInitializeSystem.InitializeEffectsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_ProceduralBones = this.__TypeHandle.__Game_Prefabs_ProceduralBone_RO_BufferLookup,
        m_EffectType = this.__TypeHandle.__Game_Prefabs_Effect_RW_BufferTypeHandle
      }.ScheduleParallel<EffectInitializeSystem.InitializeEffectsJob>(this.m_PrefabQuery, this.Dependency);
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
    public EffectInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeEffectsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      public BufferTypeHandle<Effect> m_EffectType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Effect> bufferAccessor = chunk.GetBufferAccessor<Effect>(ref this.m_EffectType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          Entity prefab = nativeArray[index1];
          DynamicBuffer<Effect> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Effect effect = dynamicBuffer[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            effect.m_BoneIndex = RenderingUtils.FindBoneIndex(prefab, ref effect.m_Position, ref effect.m_Rotation, effect.m_ParentMesh, ref this.m_SubMeshes, ref this.m_ProceduralBones);
            dynamicBuffer[index2] = effect;
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
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ProceduralBone> __Game_Prefabs_ProceduralBone_RO_BufferLookup;
      public BufferTypeHandle<Effect> __Game_Prefabs_Effect_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralBone_RO_BufferLookup = state.GetBufferLookup<ProceduralBone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RW_BufferTypeHandle = state.GetBufferTypeHandle<Effect>();
      }
    }
  }
}
