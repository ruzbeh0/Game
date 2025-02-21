// Decompiled with JetBrains decompiler
// Type: Game.Serialization.SubObjectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Creatures;
using Game.Objects;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class SubObjectSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private SubObjectSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.Object>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<Attached>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Vehicle>(),
          ComponentType.ReadOnly<Creature>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      SubObjectSystem.SubObjectJob jobData = new SubObjectSystem.SubObjectJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<SubObjectSystem.SubObjectJob>(this.m_Query, this.Dependency);
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
    public SubObjectSystem()
    {
    }

    [BurstCompile]
    private struct SubObjectJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      public BufferLookup<SubObject> m_SubObjects;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Attached> nativeArray3 = chunk.GetNativeArray<Attached>(ref this.m_AttachedType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity subObject = nativeArray1[index];
          DynamicBuffer<SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(nativeArray2[index].m_Owner, out bufferData))
            bufferData.Add(new SubObject(subObject));
          else
            Debug.Log((object) string.Format("Owner has no SubObject: {0}:{1}", (object) subObject.Index, (object) subObject.Version));
        }
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity subObject = nativeArray1[index];
          DynamicBuffer<SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(nativeArray3[index].m_Parent, out bufferData))
            CollectionUtils.TryAddUniqueValue<SubObject>(bufferData, new SubObject(subObject));
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RO_ComponentTypeHandle;
      public BufferLookup<SubObject> __Game_Objects_SubObject_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RW_BufferLookup = state.GetBufferLookup<SubObject>();
      }
    }
  }
}
