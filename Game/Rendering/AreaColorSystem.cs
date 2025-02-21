// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AreaColorSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class AreaColorSystem : GameSystemBase
  {
    private AreaBatchSystem m_AreaBatchSystem;
    private ToolSystem m_ToolSystem;
    private EntityQuery m_AreaQuery;
    private AreaColorSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaBatchSystem = this.World.GetOrCreateSystemManaged<AreaBatchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Areas.Batch>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_ToolSystem.activeInfoview != (UnityEngine.Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Batch_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new AreaColorSystem.FillColorDataJob()
      {
        m_BatchType = this.__TypeHandle.__Game_Areas_Batch_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_LotType = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentTypeHandle,
        m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_ColorData = this.__TypeHandle.__Game_Objects_Color_RO_ComponentLookup,
        m_AreaColorData = this.m_AreaBatchSystem.GetColorData(out dependencies)
      }.ScheduleParallel<AreaColorSystem.FillColorDataJob>(this.m_AreaQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaBatchSystem.AddColorWriter(jobHandle);
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
    public AreaColorSystem()
    {
    }

    [BurstCompile]
    private struct FillColorDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Areas.Batch> m_BatchType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Lot> m_LotType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Color> m_ColorData;
      [NativeDisableParallelForRestriction]
      public NativeList<AreaColorData> m_AreaColorData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Areas.Batch> nativeArray1 = chunk.GetNativeArray<Game.Areas.Batch>(ref this.m_BatchType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Triangle> bufferAccessor = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Lot>(ref this.m_LotType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Game.Areas.Batch batch = nativeArray1[index1];
          if (!batch.m_BatchAllocation.Empty)
          {
            DynamicBuffer<Triangle> dynamicBuffer = bufferAccessor[index1];
            Vector3 vector3 = new Vector3();
            if (nativeArray2.Length != 0)
            {
              Owner owner;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              for (owner = nativeArray2[index1]; !this.m_ColorData.HasComponent(owner.m_Owner); owner = this.m_OwnerData[owner.m_Owner])
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_OwnerData.HasComponent(owner.m_Owner))
                  goto label_12;
              }
              // ISSUE: reference to a compiler-generated field
              Game.Objects.Color color = this.m_ColorData[owner.m_Owner];
              if (flag)
              {
                if (color.m_Index != (byte) 0)
                {
                  vector3.x = (float) color.m_Index + 0.5f;
                  vector3.y = (float) color.m_Value * 0.003921569f;
                  vector3.z = 1f;
                }
              }
              else if (color.m_SubColor)
              {
                vector3.x = (float) color.m_Index + 0.5f;
                vector3.y = (float) color.m_Value * 0.003921569f;
                vector3.z = 1f;
              }
            }
label_12:
            int begin = (int) batch.m_BatchAllocation.Begin;
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_AreaColorData[begin++] = new AreaColorData()
              {
                m_Color = (float3) vector3
              };
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
      public ComponentTypeHandle<Game.Areas.Batch> __Game_Areas_Batch_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lot> __Game_Areas_Lot_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Triangle> __Game_Areas_Triangle_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Color> __Game_Objects_Color_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Batch_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Areas.Batch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferTypeHandle = state.GetBufferTypeHandle<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Color>(true);
      }
    }
  }
}
