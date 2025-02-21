// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateWaterSourcesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateWaterSourcesSystem : GameSystemBase
  {
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private EntityArchetype m_WaterSourceArchetype;
    private GenerateWaterSourcesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<CreationDefinition>(),
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<WaterSourceDefinition>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSourceArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<WaterSourceData>(), ComponentType.ReadWrite<Transform>(), ComponentType.ReadWrite<Temp>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_WaterSourceDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateWaterSourcesSystem.GenerateBrushesJob jobData = new GenerateWaterSourcesSystem.GenerateBrushesJob()
      {
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_WaterSourceDefinitionType = this.__TypeHandle.__Game_Tools_WaterSourceDefinition_RO_ComponentTypeHandle,
        m_WaterSourceArchetype = this.m_WaterSourceArchetype,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<GenerateWaterSourcesSystem.GenerateBrushesJob>(this.m_DefinitionQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
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
    public GenerateWaterSourcesSystem()
    {
    }

    [BurstCompile]
    private struct GenerateBrushesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<WaterSourceDefinition> m_WaterSourceDefinitionType;
      [ReadOnly]
      public EntityArchetype m_WaterSourceArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray1 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterSourceDefinition> nativeArray2 = chunk.GetNativeArray<WaterSourceDefinition>(ref this.m_WaterSourceDefinitionType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          CreationDefinition creationDefinition = nativeArray1[index];
          WaterSourceDefinition sourceDefinition = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_WaterSourceArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<WaterSourceData>(unfilteredChunkIndex, entity, new WaterSourceData()
          {
            m_ConstantDepth = sourceDefinition.m_ConstantDepth,
            m_Amount = sourceDefinition.m_Amount,
            m_Radius = sourceDefinition.m_Radius,
            m_Multiplier = sourceDefinition.m_Multiplier,
            m_Polluted = sourceDefinition.m_Polluted
          });
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(unfilteredChunkIndex, entity, new Transform()
          {
            m_Position = sourceDefinition.m_Position,
            m_Rotation = quaternion.identity
          });
          if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
          {
            Temp component = new Temp()
            {
              m_Original = creationDefinition.m_Original
            };
            if ((creationDefinition.m_Flags & CreationFlags.Select) != (CreationFlags) 0)
            {
              component.m_Flags = TempFlags.Select;
              if ((creationDefinition.m_Flags & CreationFlags.Dragging) != (CreationFlags) 0)
                component.m_Flags |= TempFlags.Dragging;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(unfilteredChunkIndex, entity, component);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Hidden>(unfilteredChunkIndex, creationDefinition.m_Original, new Hidden());
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
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterSourceDefinition> __Game_Tools_WaterSourceDefinition_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_WaterSourceDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterSourceDefinition>(true);
      }
    }
  }
}
