// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LandValueInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class LandValueInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "landValueInfo";
    private ValueBinding<float> m_AverageLandValue;
    private EntityQuery m_LandValueQuery;
    private NativeArray<float> m_Results;
    private LandValueInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<BuildingCondition>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AverageLandValue = new ValueBinding<float>("landValueInfo", "averageLandValue", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<float>(2, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
    }

    protected override bool Active => base.Active || this.m_AverageLandValue.active;

    protected override void PerformUpdate() => this.UpdateLandValue();

    private void UpdateLandValue()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0.0f;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new LandValueInfoviewUISystem.CalculateAverageLandValueJob()
      {
        m_BuildingTypeHandle = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_LandValues = this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup,
        m_Results = this.m_Results
      }.Schedule<LandValueInfoviewUISystem.CalculateAverageLandValueJob>(this.m_LandValueQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      float result = this.m_Results[1];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AverageLandValue.Update((double) result > 0.0 ? this.m_Results[0] / result : 0.0f);
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
    public LandValueInfoviewUISystem()
    {
    }

    [BurstCompile]
    private struct CalculateAverageLandValueJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingTypeHandle;
      [ReadOnly]
      public ComponentLookup<LandValue> m_LandValues;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray = chunk.GetNativeArray<Building>(ref this.m_BuildingTypeHandle);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Building building = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LandValues.HasComponent(building.m_RoadEdge))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Results[0] += this.m_LandValues[building.m_RoadEdge].m_LandValue;
            // ISSUE: reference to a compiler-generated field
            ++this.m_Results[1];
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
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<LandValue> __Game_Net_LandValue_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LandValue_RO_ComponentLookup = state.GetComponentLookup<LandValue>(true);
      }
    }
  }
}
