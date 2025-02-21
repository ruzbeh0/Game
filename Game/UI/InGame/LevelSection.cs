// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LevelSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.City;
using Game.Prefabs;
using Game.Zones;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class LevelSection : InfoSectionBase
  {
    private EntityQuery m_SpawnableBuildingQuery;
    private EntityQuery m_CityQuery;
    private NativeArray<int> m_Result;
    private LevelSection.TypeHandle __TypeHandle;

    protected override string group => nameof (LevelSection);

    private int level { get; set; }

    private int maxLevel { get; set; }

    private bool isUnderConstruction { get; set; }

    private float progress { get; set; }

    protected override bool displayForUnderConstruction => true;

    protected override void Reset()
    {
      this.level = 0;
      this.maxLevel = 0;
      this.isUnderConstruction = false;
      this.progress = 0.0f;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SpawnableBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingData>(), ComponentType.ReadOnly<SpawnableBuildingData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityQuery = this.GetEntityQuery(ComponentType.ReadOnly<CityModifier>());
      // ISSUE: reference to a compiler-generated field
      this.m_Result = new NativeArray<int>(1, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_Result.Dispose();
    }

    private bool Visible()
    {
      return !this.EntityManager.HasComponent<SignatureBuildingData>(this.selectedPrefab) && !this.EntityManager.HasComponent<Abandoned>(this.selectedEntity) && this.EntityManager.HasComponent<Renter>(this.selectedEntity) && this.EntityManager.HasComponent<BuildingData>(this.selectedPrefab) && this.EntityManager.HasComponent<SpawnableBuildingData>(this.selectedPrefab);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.visible = this.Visible();
      if (!this.visible)
        return;
      Game.Objects.UnderConstruction component;
      if (this.EntityManager.TryGetComponent<Game.Objects.UnderConstruction>(this.selectedEntity, out component) && component.m_NewPrefab == Entity.Null)
      {
        this.isUnderConstruction = true;
        this.progress = (float) Math.Min((int) component.m_Progress, 100);
      }
      else
      {
        EntityManager entityManager = this.EntityManager;
        BuildingData componentData1 = entityManager.GetComponentData<BuildingData>(this.selectedPrefab);
        entityManager = this.EntityManager;
        SpawnableBuildingData componentData2 = entityManager.GetComponentData<SpawnableBuildingData>(this.selectedPrefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        new LevelSection.CalculateMaxLevelJob()
        {
          m_LotSize = componentData1.m_LotSize,
          m_ZonePrefabEntity = componentData2.m_ZonePrefab,
          m_BuildingDataTypeHandle = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle,
          m_SpawnableBuildingDataTypeHandle = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle,
          m_Result = this.m_Result
        }.Schedule<LevelSection.CalculateMaxLevelJob>(this.m_SpawnableBuildingQuery, this.Dependency).Complete();
      }
    }

    protected override void OnProcess()
    {
      SpawnableBuildingData componentData1 = this.EntityManager.GetComponentData<SpawnableBuildingData>(this.selectedPrefab);
      ZoneData componentData2 = this.EntityManager.GetComponentData<ZoneData>(componentData1.m_ZonePrefab);
      if (this.isUnderConstruction)
      {
        this.tooltipKeys.Add("UnderConstruction");
        // ISSUE: reference to a compiler-generated field
        this.m_InfoUISystem.tooltipTags.Add(TooltipTags.UnderConstruction);
      }
      else
      {
        switch (componentData2.m_AreaType)
        {
          case AreaType.Residential:
            this.tooltipKeys.Add("Residential");
            break;
          case AreaType.Commercial:
            this.tooltipKeys.Add("Commercial");
            break;
          case AreaType.Industrial:
            this.tooltipKeys.Add((componentData2.m_ZoneFlags & ZoneFlags.Office) != (ZoneFlags) 0 ? "Office" : "Industrial");
            break;
        }
        BuildingPropertyData componentData3 = this.EntityManager.GetComponentData<BuildingPropertyData>(this.selectedPrefab);
        this.level = (int) componentData1.m_Level;
        // ISSUE: reference to a compiler-generated field
        this.maxLevel = math.max(this.m_Result[0], this.level);
        this.progress = 0.0f;
        if ((int) componentData1.m_Level >= this.maxLevel)
          return;
        int condition = this.EntityManager.GetComponentData<BuildingCondition>(this.selectedEntity).m_Condition;
        // ISSUE: reference to a compiler-generated field
        int levelingCost = BuildingUtils.GetLevelingCost(componentData2.m_AreaType, componentData3, this.level, this.EntityManager.GetBuffer<CityModifier>(this.m_CityQuery.GetSingletonEntity(), true));
        this.progress = levelingCost > 0 ? (float) ((double) condition / (double) levelingCost * 100.0) : 100f;
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("level");
      writer.Write(this.level);
      writer.PropertyName("maxLevel");
      writer.Write(this.maxLevel);
      writer.PropertyName("isUnderConstruction");
      writer.Write(this.isUnderConstruction);
      writer.PropertyName("progress");
      writer.Write(this.progress);
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
    public LevelSection()
    {
    }

    [BurstCompile]
    private struct CalculateMaxLevelJob : IJobChunk
    {
      [ReadOnly]
      public int2 m_LotSize;
      [ReadOnly]
      public Entity m_ZonePrefabEntity;
      [ReadOnly]
      public ComponentTypeHandle<BuildingData> m_BuildingDataTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> m_SpawnableBuildingDataTypeHandle;
      public NativeArray<int> m_Result;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<BuildingData> nativeArray1 = chunk.GetNativeArray<BuildingData>(ref this.m_BuildingDataTypeHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<SpawnableBuildingData> nativeArray2 = chunk.GetNativeArray<SpawnableBuildingData>(ref this.m_SpawnableBuildingDataTypeHandle);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          SpawnableBuildingData spawnableBuildingData = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((int) spawnableBuildingData.m_Level > this.m_Result[0] && spawnableBuildingData.m_ZonePrefab == this.m_ZonePrefabEntity && nativeArray1[index].m_LotSize.Equals(this.m_LotSize))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Result[0] = (int) spawnableBuildingData.m_Level;
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
      public ComponentTypeHandle<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableBuildingData>(true);
      }
    }
  }
}
