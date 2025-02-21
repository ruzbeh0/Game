// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ControllerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class ControllerSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private ControllerSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<LayoutElement>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      ControllerSystem.ControllerJob jobData = new ControllerSystem.ControllerJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<ControllerSystem.ControllerJob>(this.m_Query, this.Dependency);
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
    public ControllerSystem()
    {
    }

    [BurstCompile]
    private struct ControllerJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public ComponentLookup<Controller> m_ControllerData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          Entity entity = nativeArray[index1];
          DynamicBuffer<LayoutElement> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity vehicle = dynamicBuffer[index2].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControllerData.HasComponent(vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              Controller controller = this.m_ControllerData[vehicle] with
              {
                m_Controller = entity
              };
              // ISSUE: reference to a compiler-generated field
              this.m_ControllerData[vehicle] = controller;
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RW_ComponentLookup = state.GetComponentLookup<Controller>();
      }
    }
  }
}
