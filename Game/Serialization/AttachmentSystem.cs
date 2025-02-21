// Decompiled with JetBrains decompiler
// Type: Game.Serialization.AttachmentSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Objects;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class AttachmentSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private AttachmentSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<Attached>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AttachmentSystem.AttachmentJob jobData = new AttachmentSystem.AttachmentJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<AttachmentSystem.AttachmentJob>(this.m_Query, this.Dependency);
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
    public AttachmentSystem()
    {
    }

    [BurstCompile]
    private struct AttachmentJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      public ComponentLookup<Attachment> m_AttachmentData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Attached> nativeArray2 = chunk.GetNativeArray<Attached>(ref this.m_AttachedType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Attached attached = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachmentData.HasComponent(attached.m_Parent))
          {
            // ISSUE: reference to a compiler-generated field
            Attachment attachment = this.m_AttachmentData[attached.m_Parent] with
            {
              m_Attached = entity
            };
            // ISSUE: reference to a compiler-generated field
            this.m_AttachmentData[attached.m_Parent] = attachment;
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
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RO_ComponentTypeHandle;
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RW_ComponentLookup = state.GetComponentLookup<Attachment>();
      }
    }
  }
}
