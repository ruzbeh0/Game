// Decompiled with JetBrains decompiler
// Type: Game.Buildings.WaterPoweredInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
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
namespace Game.Buildings
{
  [CompilerGenerated]
  public class WaterPoweredInitializeSystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_WaterPoweredQuery;
    private WaterPoweredInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPoweredQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<WaterPowered>(), ComponentType.Exclude<ServiceUpgrade>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_WaterPoweredQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TerrainComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterPowered_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle handle = new WaterPoweredInitializeSystem.WaterPoweredInitializeJob()
      {
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_SubnetType = this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle,
        m_WaterPoweredType = this.__TypeHandle.__Game_Buildings_WaterPowered_RW_ComponentTypeHandle,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PlaceableNetData = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup,
        m_NetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_TerrainCompositionData = this.__TypeHandle.__Game_Prefabs_TerrainComposition_RO_ComponentLookup,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetVelocitiesSurfaceData(out deps)
      }.ScheduleParallel<WaterPoweredInitializeSystem.WaterPoweredInitializeJob>(this.m_WaterPoweredQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(handle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(handle);
      this.Dependency = handle;
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
    public WaterPoweredInitializeSystem()
    {
    }

    [BurstCompile]
    private struct WaterPoweredInitializeJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> m_SubnetType;
      public ComponentTypeHandle<WaterPowered> m_WaterPoweredType;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> m_PlaceableNetData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      [ReadOnly]
      public ComponentLookup<TerrainComposition> m_TerrainCompositionData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPowered> nativeArray = chunk.GetNativeArray<WaterPowered>(ref this.m_WaterPoweredType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubNet> bufferAccessor = chunk.GetBufferAccessor<Game.Net.SubNet>(ref this.m_SubnetType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<Temp>(ref this.m_TempType) && !chunk.Has<Created>(ref this.m_CreatedType))
          return;
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          WaterPowered waterPowered = nativeArray[index];
          DynamicBuffer<Game.Net.SubNet> subNets = bufferAccessor[index];
          waterPowered.m_Length = 0.0f;
          waterPowered.m_Height = 0.0f;
          waterPowered.m_Estimate = 0.0f;
          // ISSUE: reference to a compiler-generated method
          this.CalculateWaterPowered(ref waterPowered, subNets);
          waterPowered.m_Height /= math.max(1f, waterPowered.m_Length);
          nativeArray[index] = waterPowered;
        }
      }

      private void CalculateWaterPowered(
        ref WaterPowered waterPowered,
        DynamicBuffer<Game.Net.SubNet> subNets)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          Entity subNet = subNets[index].m_SubNet;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabData[subNet];
          Curve componentData1;
          Composition componentData2;
          PlaceableNetData componentData3;
          NetCompositionData componentData4;
          TerrainComposition componentData5;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.TryGetComponent(subNet, out componentData1) && this.m_CompositionData.TryGetComponent(subNet, out componentData2) && this.m_PlaceableNetData.TryGetComponent(prefabRef.m_Prefab, out componentData3) && this.m_NetCompositionData.TryGetComponent(componentData2.m_Edge, out componentData4) && this.m_TerrainCompositionData.TryGetComponent(componentData2.m_Edge, out componentData5) && (componentData3.m_PlacementFlags & (PlacementFlags.FlowLeft | PlacementFlags.FlowRight)) != PlacementFlags.None)
          {
            // ISSUE: reference to a compiler-generated method
            this.CalculateWaterPowered(ref waterPowered, componentData1, componentData3, componentData4, componentData5);
          }
        }
      }

      private void CalculateWaterPowered(
        ref WaterPowered waterPowered,
        Curve curve,
        PlaceableNetData placeableData,
        NetCompositionData compositionData,
        TerrainComposition terrainComposition)
      {
        // ISSUE: reference to a compiler-generated field
        int num1 = math.max(1, Mathf.RoundToInt(curve.m_Length * this.m_WaterSurfaceData.scale.x));
        bool c = (placeableData.m_PlacementFlags & PlacementFlags.FlowLeft) != 0;
        float num2 = 0.0f;
        float num3 = 0.0f;
        for (int index = 0; index < num1; ++index)
        {
          float t = ((float) index + 0.5f) / (float) num1;
          float3 worldPosition = MathUtils.Position(curve.m_Bezier, t);
          float3 float3 = MathUtils.Tangent(curve.m_Bezier, t);
          float2 y = math.normalizesafe(math.select(MathUtils.Right(float3.xz), MathUtils.Left(float3.xz), c));
          worldPosition.y += compositionData.m_SurfaceHeight.min + terrainComposition.m_MaxHeightOffset.y;
          float terrainHeight;
          float waterHeight;
          float waterDepth;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, worldPosition, out terrainHeight, out waterHeight, out waterDepth);
          // ISSUE: reference to a compiler-generated field
          float2 x = WaterUtils.SampleVelocity(ref this.m_WaterSurfaceData, worldPosition);
          num2 += math.max(0.0f, worldPosition.y - terrainHeight);
          num3 += math.dot(x, y) * waterDepth * math.max(0.0f, worldPosition.y - waterHeight);
        }
        waterPowered.m_Length += curve.m_Length;
        waterPowered.m_Height += num2 * curve.m_Length / (float) num1;
        waterPowered.m_Estimate += num3 * curve.m_Length / (float) num1;
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
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferTypeHandle;
      public ComponentTypeHandle<WaterPowered> __Game_Buildings_WaterPowered_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> __Game_Prefabs_PlaceableNetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TerrainComposition> __Game_Prefabs_TerrainComposition_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterPowered_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPowered>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TerrainComposition_RO_ComponentLookup = state.GetComponentLookup<TerrainComposition>(true);
      }
    }
  }
}
