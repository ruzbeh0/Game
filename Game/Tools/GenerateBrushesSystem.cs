// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateBrushesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateBrushesSystem : GameSystemBase
  {
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private GenerateBrushesSystem.TypeHandle __TypeHandle;

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
          ComponentType.ReadOnly<BrushDefinition>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TerraformingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BrushData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_BrushDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      GenerateBrushesSystem.GenerateBrushesJob jobData = new GenerateBrushesSystem.GenerateBrushesJob()
      {
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_BrushDefinitionType = this.__TypeHandle.__Game_Tools_BrushDefinition_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabBrushData = this.__TypeHandle.__Game_Prefabs_BrushData_RO_ComponentLookup,
        m_PrefabTerraformingData = this.__TypeHandle.__Game_Prefabs_TerraformingData_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<GenerateBrushesSystem.GenerateBrushesJob>(this.m_DefinitionQuery, this.Dependency);
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
    public GenerateBrushesSystem()
    {
    }

    [BurstCompile]
    private struct GenerateBrushesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<BrushDefinition> m_BrushDefinitionType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<BrushData> m_PrefabBrushData;
      [ReadOnly]
      public ComponentLookup<TerraformingData> m_PrefabTerraformingData;
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
        NativeArray<BrushDefinition> nativeArray2 = chunk.GetNativeArray<BrushDefinition>(ref this.m_BrushDefinitionType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          CreationDefinition creationDefinition = nativeArray1[index1];
          BrushDefinition brushDefinition = nativeArray2[index1];
          PrefabRef component1 = new PrefabRef();
          component1.m_Prefab = creationDefinition.m_Prefab;
          if (creationDefinition.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Hidden>(unfilteredChunkIndex, creationDefinition.m_Original, new Hidden());
            // ISSUE: reference to a compiler-generated field
            component1.m_Prefab = this.m_PrefabRefData[creationDefinition.m_Original].m_Prefab;
          }
          // ISSUE: reference to a compiler-generated field
          BrushData brushData = this.m_PrefabBrushData[component1.m_Prefab];
          Temp component2 = new Temp();
          component2.m_Original = creationDefinition.m_Original;
          component2.m_Flags |= TempFlags.Essential;
          if ((creationDefinition.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
            component2.m_Flags |= TempFlags.Delete;
          else if ((creationDefinition.m_Flags & CreationFlags.Select) != (CreationFlags) 0)
            component2.m_Flags |= TempFlags.Select;
          else
            component2.m_Flags |= TempFlags.Create;
          TerraformingData componentData;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabTerraformingData.TryGetComponent(brushDefinition.m_Tool, out componentData) || componentData.m_Target == TerraformingTarget.None)
            component2.m_Flags |= TempFlags.Cancel;
          float num1 = MathUtils.Length(brushDefinition.m_Line);
          float num2 = brushDefinition.m_Strength * brushDefinition.m_Strength;
          int num3 = 1 + Mathf.FloorToInt(num1 / (brushDefinition.m_Size * 0.25f));
          float num4 = brushDefinition.m_Time / (float) num3;
          Brush component3 = new Brush();
          component3.m_Tool = brushDefinition.m_Tool;
          component3.m_Angle = brushDefinition.m_Angle;
          component3.m_Size = brushDefinition.m_Size;
          component3.m_Strength = (float) ((double) num2 * (double) num2 * (1.0 - (double) num4) + (double) num2 * (double) num4) * math.sign(brushDefinition.m_Strength);
          component3.m_Opacity = 1f / (float) num3;
          component3.m_Target = brushDefinition.m_Target;
          component3.m_Start = brushDefinition.m_Start;
          for (int index2 = 1; index2 <= num3; ++index2)
          {
            component3.m_Position = MathUtils.Position(brushDefinition.m_Line, (float) index2 / (float) num3);
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, brushData.m_Archetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(unfilteredChunkIndex, entity, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Brush>(unfilteredChunkIndex, entity, component3);
            if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Temp>(unfilteredChunkIndex, entity, component2);
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
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BrushDefinition> __Game_Tools_BrushDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BrushData> __Game_Prefabs_BrushData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TerraformingData> __Game_Prefabs_TerraformingData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_BrushDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BrushDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BrushData_RO_ComponentLookup = state.GetComponentLookup<BrushData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TerraformingData_RO_ComponentLookup = state.GetComponentLookup<TerraformingData>(true);
      }
    }
  }
}
