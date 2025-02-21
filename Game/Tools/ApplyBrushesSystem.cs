// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyBrushesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ApplyBrushesSystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private NaturalResourceSystem m_NaturalResourceSystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private TerrainSystem m_TerrainSystem;
    private TerrainMaterialSystem m_TerrainMaterialSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_TempQuery;
    private ComponentTypeSet m_AppliedDeletedTypes;
    private ApplyBrushesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_NaturalResourceSystem = this.World.GetOrCreateSystemManaged<NaturalResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainMaterialSystem = this.World.GetOrCreateSystemManaged<TerrainMaterialSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Brush>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedDeletedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Brush> componentTypeHandle1 = this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabRef> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer();
      JobHandle jobHandle = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        this.CompleteDependency();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<Brush> nativeArray2 = archetypeChunk.GetNativeArray<Brush>(ref componentTypeHandle1);
          NativeArray<PrefabRef> nativeArray3 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity e = nativeArray1[index2];
            Brush brush1 = nativeArray2[index2];
            PrefabRef prefabRef = nativeArray3[index2];
            TerraformingData component;
            if (this.EntityManager.TryGetComponent<TerraformingData>(brush1.m_Tool, out component))
            {
              switch (component.m_Target)
              {
                case TerraformingTarget.Height:
                  // ISSUE: reference to a compiler-generated method
                  this.ApplyHeight(brush1, prefabRef.m_Prefab, component.m_Type);
                  break;
                case TerraformingTarget.Ore:
                  JobHandle job0_1 = jobHandle;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  NaturalResourceSystem naturalResourceSystem1 = this.m_NaturalResourceSystem;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.NaturalResourcesModifier modifier1 = new ApplyBrushesSystem.NaturalResourcesModifier(MapFeature.Ore);
                  Brush brush2 = brush1;
                  Entity prefab1 = prefabRef.m_Prefab;
                  int type1 = (int) component.m_Type;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier> applyCellMapBrushJob1 = new ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier>();
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier> applyCellMapBrushJob2 = applyCellMapBrushJob1;
                  // ISSUE: reference to a compiler-generated method
                  JobHandle job1_1 = this.ApplyCellMapBrush<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier>((CellMapSystem<NaturalResourceCell>) naturalResourceSystem1, modifier1, brush2, prefab1, (TerraformingType) type1, applyCellMapBrushJob2);
                  jobHandle = JobHandle.CombineDependencies(job0_1, job1_1);
                  break;
                case TerraformingTarget.Oil:
                  JobHandle job0_2 = jobHandle;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  NaturalResourceSystem naturalResourceSystem2 = this.m_NaturalResourceSystem;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.NaturalResourcesModifier modifier2 = new ApplyBrushesSystem.NaturalResourcesModifier(MapFeature.Oil);
                  Brush brush3 = brush1;
                  Entity prefab2 = prefabRef.m_Prefab;
                  int type2 = (int) component.m_Type;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier> applyCellMapBrushJob3 = new ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier>();
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier> applyCellMapBrushJob4 = applyCellMapBrushJob3;
                  // ISSUE: reference to a compiler-generated method
                  JobHandle job1_2 = this.ApplyCellMapBrush<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier>((CellMapSystem<NaturalResourceCell>) naturalResourceSystem2, modifier2, brush3, prefab2, (TerraformingType) type2, applyCellMapBrushJob4);
                  jobHandle = JobHandle.CombineDependencies(job0_2, job1_2);
                  break;
                case TerraformingTarget.FertileLand:
                  JobHandle job0_3 = jobHandle;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  NaturalResourceSystem naturalResourceSystem3 = this.m_NaturalResourceSystem;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.NaturalResourcesModifier modifier3 = new ApplyBrushesSystem.NaturalResourcesModifier(MapFeature.FertileLand);
                  Brush brush4 = brush1;
                  Entity prefab3 = prefabRef.m_Prefab;
                  int type3 = (int) component.m_Type;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier> applyCellMapBrushJob5 = new ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier>();
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.ApplyCellMapBrushJob<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier> applyCellMapBrushJob6 = applyCellMapBrushJob5;
                  // ISSUE: reference to a compiler-generated method
                  JobHandle job1_3 = this.ApplyCellMapBrush<NaturalResourceCell, ApplyBrushesSystem.NaturalResourcesModifier>((CellMapSystem<NaturalResourceCell>) naturalResourceSystem3, modifier3, brush4, prefab3, (TerraformingType) type3, applyCellMapBrushJob6);
                  jobHandle = JobHandle.CombineDependencies(job0_3, job1_3);
                  break;
                case TerraformingTarget.GroundWater:
                  JobHandle job0_4 = jobHandle;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  GroundWaterSystem groundWaterSystem = this.m_GroundWaterSystem;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.GroundWaterModifier groundWaterModifier = new ApplyBrushesSystem.GroundWaterModifier();
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.GroundWaterModifier modifier4 = groundWaterModifier;
                  Brush brush5 = brush1;
                  Entity prefab4 = prefabRef.m_Prefab;
                  int type4 = (int) component.m_Type;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.ApplyCellMapBrushJob<GroundWater, ApplyBrushesSystem.GroundWaterModifier> applyCellMapBrushJob7 = new ApplyBrushesSystem.ApplyCellMapBrushJob<GroundWater, ApplyBrushesSystem.GroundWaterModifier>();
                  // ISSUE: variable of a compiler-generated type
                  ApplyBrushesSystem.ApplyCellMapBrushJob<GroundWater, ApplyBrushesSystem.GroundWaterModifier> applyCellMapBrushJob8 = applyCellMapBrushJob7;
                  // ISSUE: reference to a compiler-generated method
                  JobHandle job1_4 = this.ApplyCellMapBrush<GroundWater, ApplyBrushesSystem.GroundWaterModifier>((CellMapSystem<GroundWater>) groundWaterSystem, modifier4, brush5, prefab4, (TerraformingType) type4, applyCellMapBrushJob8);
                  jobHandle = JobHandle.CombineDependencies(job0_4, job1_4);
                  break;
                case TerraformingTarget.Material:
                  // ISSUE: reference to a compiler-generated method
                  this.ApplyMaterial(brush1, prefabRef.m_Prefab);
                  break;
              }
            }
            // ISSUE: reference to a compiler-generated field
            commandBuffer.AddComponent(e, in this.m_AppliedDeletedTypes);
          }
        }
      }
      finally
      {
        archetypeChunkArray.Dispose();
        this.Dependency = jobHandle;
      }
    }

    private void ApplyMaterial(Brush brush, Entity prefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainMaterialSystem.GetOrAddMaterialIndex(brush.m_Tool);
    }

    private void ApplyHeight(Brush brush, Entity prefab, TerraformingType terraformingType)
    {
      Bounds2 bounds = ToolUtils.GetBounds(brush);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      BrushPrefab prefab1 = this.m_PrefabSystem.GetPrefab<BrushPrefab>(prefab);
      if ((terraformingType == TerraformingType.Level || terraformingType == TerraformingType.Slope) && (double) brush.m_Strength < 0.0)
        return;
      if (terraformingType == TerraformingType.Soften && (double) brush.m_Strength < 0.0)
        brush.m_Strength = math.abs(brush.m_Strength) * 2f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.ApplyBrush(terraformingType, bounds, brush, (Texture) prefab1.m_Texture);
    }

    private JobHandle ApplyCellMapBrush<TCell, TModifier>(
      CellMapSystem<TCell> cellMapSystem,
      TModifier modifier,
      Brush brush,
      Entity prefab,
      TerraformingType terraformingType,
      ApplyBrushesSystem.ApplyCellMapBrushJob<TCell, TModifier> applyCellMapBrushJob)
      where TCell : struct, ISerializable
      where TModifier : ApplyBrushesSystem.ICellModifier<TCell>
    {
      Bounds2 bounds = ToolUtils.GetBounds(brush);
      JobHandle dependencies;
      CellMapData<TCell> data = cellMapSystem.GetData(false, out dependencies);
      float4 xyxy1 = (1f / data.m_CellSize).xyxy;
      float4 xyxy2 = ((float2) data.m_TextureSize * 0.5f).xyxy;
      int4 int4 = math.clamp((int4) math.floor(new float4(bounds.min, bounds.max) * xyxy1 + xyxy2), (int4) 0, data.m_TextureSize.xyxy - 1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BrushCell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BrushData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      applyCellMapBrushJob = new ApplyBrushesSystem.ApplyCellMapBrushJob<TCell, TModifier>()
      {
        m_Coords = int4,
        m_Brush = brush,
        m_Prefab = prefab,
        m_TerraformingType = terraformingType,
        m_CellModifier = modifier,
        m_TextureSizeAdd = xyxy2,
        m_CellSize = data.m_CellSize,
        m_TextureSize = data.m_TextureSize,
        m_BrushData = this.__TypeHandle.__Game_Prefabs_BrushData_RO_ComponentLookup,
        m_BrushCells = this.__TypeHandle.__Game_Prefabs_BrushCell_RO_BufferLookup,
        m_Buffer = data.m_Buffer
      };
      JobHandle jobHandle = applyCellMapBrushJob.Schedule<ApplyBrushesSystem.ApplyCellMapBrushJob<TCell, TModifier>>(int4.w - int4.y + 1, 1, dependencies);
      cellMapSystem.AddWriter(jobHandle);
      return jobHandle;
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
    public ApplyBrushesSystem()
    {
    }

    private interface ICellModifier<TCell> where TCell : struct, ISerializable
    {
      void Apply(ref TCell cell, float strength);
    }

    private struct NaturalResourcesModifier : ApplyBrushesSystem.ICellModifier<NaturalResourceCell>
    {
      private MapFeature m_MapFeature;

      public NaturalResourcesModifier(MapFeature mapFeature) => this.m_MapFeature = mapFeature;

      public void Apply(ref NaturalResourceCell cell, float strength)
      {
        // ISSUE: reference to a compiler-generated field
        switch (this.m_MapFeature)
        {
          case MapFeature.FertileLand:
            // ISSUE: reference to a compiler-generated method
            this.Apply(ref cell.m_Fertility, strength);
            break;
          case MapFeature.Oil:
            // ISSUE: reference to a compiler-generated method
            this.Apply(ref cell.m_Oil, strength);
            break;
          case MapFeature.Ore:
            // ISSUE: reference to a compiler-generated method
            this.Apply(ref cell.m_Ore, strength);
            break;
        }
      }

      private void Apply(ref NaturalResourceAmount cellData, float strength)
      {
        float amount = (float) cellData.m_Base * 0.0001f;
        // ISSUE: reference to a compiler-generated method
        this.Apply(ref amount, strength);
        cellData.m_Base = (ushort) math.clamp(Mathf.RoundToInt(amount * 10000f), 0, 10000);
      }

      private void Apply(ref float amount, float strength) => amount += strength;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct GroundWaterModifier : ApplyBrushesSystem.ICellModifier<GroundWater>
    {
      public void Apply(ref GroundWater cell, float strength)
      {
        float amount = (float) cell.m_Amount * 0.0001f;
        // ISSUE: reference to a compiler-generated method
        this.Apply(ref amount, strength);
        cell.m_Amount = (short) math.clamp(Mathf.RoundToInt(amount * 10000f), 0, 10000);
        cell.m_Max = cell.m_Amount;
      }

      private void Apply(ref float amount, float strength) => amount += strength;
    }

    [BurstCompile]
    private struct ApplyCellMapBrushJob<TCell, TModifier> : IJobParallelFor
      where TCell : struct, ISerializable
      where TModifier : ApplyBrushesSystem.ICellModifier<TCell>
    {
      [ReadOnly]
      public int4 m_Coords;
      [ReadOnly]
      public Brush m_Brush;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public TerraformingType m_TerraformingType;
      [ReadOnly]
      public TModifier m_CellModifier;
      [ReadOnly]
      public float4 m_TextureSizeAdd;
      [ReadOnly]
      public float2 m_CellSize;
      [ReadOnly]
      public int2 m_TextureSize;
      [ReadOnly]
      public ComponentLookup<BrushData> m_BrushData;
      [ReadOnly]
      public BufferLookup<BrushCell> m_BrushCells;
      [NativeDisableParallelForRestriction]
      public NativeArray<TCell> m_Buffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        int num1 = this.m_Coords.y + index;
        Bounds2 bounds;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bounds.min.y = ((float) num1 - this.m_TextureSizeAdd.y) * this.m_CellSize.y - this.m_Brush.m_Position.z;
        // ISSUE: reference to a compiler-generated field
        bounds.max.y = bounds.min.y + this.m_CellSize.y;
        // ISSUE: reference to a compiler-generated field
        quaternion q = quaternion.RotateY(this.m_Brush.m_Angle);
        float2 xz1 = math.mul(q, new float3(1f, 0.0f, 0.0f)).xz;
        float2 xz2 = math.mul(q, new float3(0.0f, 0.0f, 1f)).xz;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BrushData brushData = this.m_BrushData[this.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BrushCell> brushCell1 = this.m_BrushCells[this.m_Prefab];
        if (math.any(brushData.m_Resolution == 0) || brushCell1.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        float2 float2_1 = this.m_Brush.m_Size / (float2) brushData.m_Resolution;
        float4 xyxy1 = (1f / float2_1).xyxy;
        float4 xyxy2 = ((float2) brushData.m_Resolution * 0.5f).xyxy;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num2 = this.m_Brush.m_Strength / (this.m_CellSize.x * this.m_CellSize.y);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (int x1 = this.m_Coords.x; x1 <= this.m_Coords.z; ++x1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bounds.min.x = ((float) x1 - this.m_TextureSizeAdd.x) * this.m_CellSize.x - this.m_Brush.m_Position.x;
          // ISSUE: reference to a compiler-generated field
          bounds.max.x = bounds.min.x + this.m_CellSize.x;
          float4 float4 = new float4(bounds.min, bounds.max);
          float4 x2 = new float4(math.dot(float4.xy, xz1), math.dot(float4.xw, xz1), math.dot(float4.zy, xz1), math.dot(float4.zw, xz1));
          float4 x3 = new float4(math.dot(float4.xy, xz2), math.dot(float4.xw, xz2), math.dot(float4.zy, xz2), math.dot(float4.zw, xz2));
          int4 int4 = math.clamp((int4) math.floor(new float4(math.cmin(x2), math.cmin(x3), math.cmax(x2), math.cmax(x3)) * xyxy1 + xyxy2), (int4) 0, brushData.m_Resolution.xyxy - 1);
          float num3 = 0.0f;
          for (int y = int4.y; y <= int4.w; ++y)
          {
            float2 float2_2 = xz2 * (((float) y - xyxy2.y) * float2_1.y);
            float2 float2_3 = xz2 * (((float) (y + 1) - xyxy2.y) * float2_1.y);
            for (int x4 = int4.x; x4 <= int4.z; ++x4)
            {
              int index1 = x4 + brushData.m_Resolution.x * y;
              BrushCell brushCell2 = brushCell1[index1];
              if ((double) brushCell2.m_Opacity >= 9.9999997473787516E-05)
              {
                float2 float2_4 = xz1 * (((float) x4 - xyxy2.x) * float2_1.x);
                float2 float2_5 = xz1 * (((float) (x4 + 1) - xyxy2.x) * float2_1.x);
                Quad2 quad = new Quad2(float2_2 + float2_4, float2_2 + float2_5, float2_3 + float2_5, float2_3 + float2_4);
                float area;
                if (MathUtils.Intersect(bounds, quad, out area))
                  num3 += brushCell2.m_Opacity * area;
              }
            }
          }
          float num4 = num3 * num2;
          if ((double) math.abs(num4) >= 9.9999997473787516E-05)
          {
            // ISSUE: reference to a compiler-generated field
            int index2 = x1 + this.m_TextureSize.x * num1;
            // ISSUE: reference to a compiler-generated field
            TCell cell = this.m_Buffer[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_CellModifier.Apply(ref cell, num4);
            // ISSUE: reference to a compiler-generated field
            this.m_Buffer[index2] = cell;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Brush> __Game_Tools_Brush_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<BrushData> __Game_Prefabs_BrushData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<BrushCell> __Game_Prefabs_BrushCell_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Brush_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Brush>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BrushData_RO_ComponentLookup = state.GetComponentLookup<BrushData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BrushCell_RO_BufferLookup = state.GetBufferLookup<BrushCell>(true);
      }
    }
  }
}
