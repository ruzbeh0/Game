// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Logging;
using Colossal.Mathematics;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Rendering;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class BuildingInitializeSystem : GameSystemBase
  {
    private static ILog log;
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_ConfigurationQuery;
    private PrefabSystem m_PrefabSystem;
    private BuildingInitializeSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_547773812_0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated field
      BuildingInitializeSystem.log = LogManager.GetLogger("Simulation");
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadWrite<BuildingData>(),
          ComponentType.ReadWrite<BuildingExtensionData>(),
          ComponentType.ReadWrite<ServiceUpgradeData>(),
          ComponentType.ReadWrite<SpawnableBuildingData>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadWrite<ServiceUpgradeData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob, PlaybackPolicy.SinglePlayback);
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Deleted> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<BuildingData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_BuildingData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<BuildingExtensionData> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<BuildingTerraformData> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConsumptionData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ConsumptionData> componentTypeHandle6 = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ObjectGeometryData> componentTypeHandle7 = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<SpawnableBuildingData> componentTypeHandle8 = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<SignatureBuildingData> componentTypeHandle9 = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PlaceableObjectData> componentTypeHandle10 = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ServiceUpgradeData> componentTypeHandle11 = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<BuildingPropertyData> componentTypeHandle12 = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPoweredData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<WaterPoweredData> componentTypeHandle13 = this.__TypeHandle.__Game_Prefabs_WaterPoweredData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<SewageOutletData> componentTypeHandle14 = this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<ServiceUpgradeBuilding> bufferTypeHandle1 = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedServiceBuildingBudgetData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<CollectedServiceBuildingBudgetData> componentTypeHandle15 = this.__TypeHandle.__Game_Simulation_CollectedServiceBuildingBudgetData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<ServiceUpkeepData> bufferTypeHandle2 = this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RW_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ZoneData> rwComponentLookup = this.__TypeHandle.__Game_Prefabs_ZoneData_RW_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneServiceConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ZoneServiceConsumptionData> roComponentLookup = this.__TypeHandle.__Game_Prefabs_ZoneServiceConsumptionData_RO_ComponentLookup;
      this.CompleteDependency();
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
        BufferAccessor<ServiceUpgradeBuilding> bufferAccessor1 = archetypeChunk.GetBufferAccessor<ServiceUpgradeBuilding>(ref bufferTypeHandle1);
        if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
        {
          if (bufferAccessor1.Length != 0)
          {
            for (int index2 = 0; index2 < bufferAccessor1.Length; ++index2)
            {
              Entity upgrade = nativeArray1[index2];
              DynamicBuffer<ServiceUpgradeBuilding> dynamicBuffer = bufferAccessor1[index2];
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
                CollectionUtils.RemoveValue<BuildingUpgradeElement>(this.EntityManager.GetBuffer<BuildingUpgradeElement>(dynamicBuffer[index3].m_Building), new BuildingUpgradeElement(upgrade));
            }
          }
        }
        else
        {
          NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle2);
          NativeArray<ObjectGeometryData> nativeArray3 = archetypeChunk.GetNativeArray<ObjectGeometryData>(ref componentTypeHandle7);
          NativeArray<BuildingData> nativeArray4 = archetypeChunk.GetNativeArray<BuildingData>(ref componentTypeHandle3);
          NativeArray<BuildingExtensionData> nativeArray5 = archetypeChunk.GetNativeArray<BuildingExtensionData>(ref componentTypeHandle4);
          NativeArray<ConsumptionData> nativeArray6 = archetypeChunk.GetNativeArray<ConsumptionData>(ref componentTypeHandle6);
          NativeArray<SpawnableBuildingData> nativeArray7 = archetypeChunk.GetNativeArray<SpawnableBuildingData>(ref componentTypeHandle8);
          NativeArray<PlaceableObjectData> nativeArray8 = archetypeChunk.GetNativeArray<PlaceableObjectData>(ref componentTypeHandle10);
          NativeArray<ServiceUpgradeData> nativeArray9 = archetypeChunk.GetNativeArray<ServiceUpgradeData>(ref componentTypeHandle11);
          NativeArray<BuildingPropertyData> nativeArray10 = archetypeChunk.GetNativeArray<BuildingPropertyData>(ref componentTypeHandle12);
          BufferAccessor<ServiceUpkeepData> bufferAccessor2 = archetypeChunk.GetBufferAccessor<ServiceUpkeepData>(ref bufferTypeHandle2);
          bool flag1 = archetypeChunk.Has<CollectedServiceBuildingBudgetData>(ref componentTypeHandle15);
          bool flag2 = archetypeChunk.Has<SignatureBuildingData>(ref componentTypeHandle9);
          bool flag3 = archetypeChunk.Has<WaterPoweredData>(ref componentTypeHandle13);
          bool flag4 = archetypeChunk.Has<SewageOutletData>(ref componentTypeHandle14);
          if (nativeArray4.Length != 0)
          {
            NativeArray<BuildingTerraformData> nativeArray11 = archetypeChunk.GetNativeArray<BuildingTerraformData>(ref componentTypeHandle5);
            for (int index4 = 0; index4 < nativeArray4.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              BuildingPrefab prefab = this.m_PrefabSystem.GetPrefab<BuildingPrefab>(nativeArray2[index4]);
              BuildingTerraformOverride component = prefab.GetComponent<BuildingTerraformOverride>();
              ObjectGeometryData objectGeometryData = nativeArray3[index4];
              BuildingTerraformData buildingTerraformData = nativeArray11[index4];
              BuildingData buildingData = nativeArray4[index4];
              // ISSUE: reference to a compiler-generated method
              this.InitializeLotSize(prefab, component, ref objectGeometryData, ref buildingTerraformData, ref buildingData);
              if (nativeArray7.Length != 0 && !flag2)
              {
                objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.DeleteOverridden;
              }
              else
              {
                objectGeometryData.m_Flags &= ~Game.Objects.GeometryFlags.Overridable;
                objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.OverrideZone;
              }
              if (flag3)
                objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.CanSubmerge;
              else if (flag4 && prefab.GetComponent<SewageOutlet>().m_AllowSubmerged)
                objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.CanSubmerge;
              objectGeometryData.m_Flags &= ~Game.Objects.GeometryFlags.Brushable;
              objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.ExclusiveGround | Game.Objects.GeometryFlags.WalkThrough | Game.Objects.GeometryFlags.OccupyZone | Game.Objects.GeometryFlags.HasLot;
              nativeArray3[index4] = objectGeometryData;
              nativeArray11[index4] = buildingTerraformData;
              nativeArray4[index4] = buildingData;
            }
          }
          if (nativeArray5.Length != 0)
          {
            NativeArray<BuildingTerraformData> nativeArray12 = archetypeChunk.GetNativeArray<BuildingTerraformData>(ref componentTypeHandle5);
            for (int index5 = 0; index5 < nativeArray5.Length; ++index5)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              BuildingExtensionPrefab prefab1 = this.m_PrefabSystem.GetPrefab<BuildingExtensionPrefab>(nativeArray2[index5]);
              ObjectGeometryData objectGeometryData = nativeArray3[index5];
              Bounds2 bounds2_1;
              if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
              {
                float2 xz = objectGeometryData.m_Pivot.xz;
                float2 float2 = objectGeometryData.m_LegSize.xz * 0.5f;
                bounds2_1 = new Bounds2(xz - float2, xz + float2);
              }
              else
                bounds2_1 = objectGeometryData.m_Bounds.xz;
              objectGeometryData.m_Bounds.min = math.min(objectGeometryData.m_Bounds.min, new float3(-0.5f, 0.0f, -0.5f));
              objectGeometryData.m_Bounds.max = math.max(objectGeometryData.m_Bounds.max, new float3(0.5f, 5f, 0.5f));
              objectGeometryData.m_Flags &= ~(Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.Brushable);
              objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.ExclusiveGround | Game.Objects.GeometryFlags.WalkThrough | Game.Objects.GeometryFlags.OccupyZone | Game.Objects.GeometryFlags.HasLot;
              BuildingExtensionData buildingExtensionData = nativeArray5[index5] with
              {
                m_Position = prefab1.m_Position,
                m_LotSize = prefab1.m_OverrideLotSize,
                m_External = prefab1.m_ExternalLot
              };
              if ((double) prefab1.m_OverrideHeight > 0.0)
                objectGeometryData.m_Bounds.max.y = prefab1.m_OverrideHeight;
              Bounds2 bounds2_2;
              if (math.all(buildingExtensionData.m_LotSize > 0))
              {
                float2 float2_1 = (float2) buildingExtensionData.m_LotSize * 8f;
                bounds2_2 = new Bounds2(float2_1 * -0.5f, float2_1 * 0.5f);
                float2 float2_2 = float2_1 - 0.4f;
                objectGeometryData.m_Bounds.min.xz = float2_2 * -0.5f;
                objectGeometryData.m_Bounds.max.xz = float2_2 * 0.5f;
                if (bufferAccessor1.Length != 0)
                  objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.OverrideZone;
              }
              else
              {
                Bounds3 bounds = objectGeometryData.m_Bounds;
                bounds2_2 = objectGeometryData.m_Bounds.xz;
                if (bufferAccessor1.Length != 0)
                {
                  DynamicBuffer<ServiceUpgradeBuilding> dynamicBuffer = bufferAccessor1[index5];
                  for (int index6 = 0; index6 < dynamicBuffer.Length; ++index6)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    BuildingPrefab prefab2 = this.m_PrefabSystem.GetPrefab<BuildingPrefab>(dynamicBuffer[index6].m_Building);
                    float2 float2_3 = (float2) new int2(prefab2.m_LotWidth, prefab2.m_LotDepth) * 8f;
                    float2 float2_4 = float2_3;
                    float2 float2_5 = float2_3 - 0.4f;
                    StandingObject component;
                    if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) == Game.Objects.GeometryFlags.None && prefab2.TryGet<StandingObject>(out component))
                    {
                      float2_5 = component.m_LegSize.xz;
                      float2_4 = component.m_LegSize.xz;
                      if (component.m_CircularLeg)
                        objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.Circular;
                    }
                    if (index6 == 0)
                    {
                      bounds.xz = new Bounds2(float2_5 * -0.5f, float2_5 * 0.5f) - prefab1.m_Position.xz;
                      bounds2_2 = new Bounds2(float2_4 * -0.5f, float2_4 * 0.5f) - prefab1.m_Position.xz;
                    }
                    else
                    {
                      bounds.xz &= new Bounds2(float2_5 * -0.5f, float2_5 * 0.5f) - prefab1.m_Position.xz;
                      bounds2_2 &= new Bounds2(float2_4 * -0.5f, float2_4 * 0.5f) - prefab1.m_Position.xz;
                    }
                  }
                  objectGeometryData.m_Bounds.xz = bounds.xz;
                  objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.OverrideZone;
                }
                float2 float2 = math.min(-bounds.min.xz, bounds.max.xz) * 0.25f - 0.01f;
                buildingExtensionData.m_LotSize.x = math.max(1, Mathf.CeilToInt(float2.x));
                buildingExtensionData.m_LotSize.y = math.max(1, Mathf.CeilToInt(float2.y));
              }
              if (buildingExtensionData.m_External)
              {
                float2 float2 = (float2) buildingExtensionData.m_LotSize * 8f;
                objectGeometryData.m_Layers |= MeshLayer.Default;
                objectGeometryData.m_MinLod = math.min(objectGeometryData.m_MinLod, RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(float2.x, 0.0f, float2.y))));
              }
              if (nativeArray12.Length != 0)
              {
                BuildingTerraformOverride component = prefab1.GetComponent<BuildingTerraformOverride>();
                BuildingTerraformData buildingTerraformData = nativeArray12[index5];
                ref BuildingTerraformData local = ref buildingTerraformData;
                Bounds2 lotBounds = bounds2_2;
                Bounds2 flatBounds = bounds2_1;
                // ISSUE: reference to a compiler-generated method
                BuildingInitializeSystem.InitializeTerraformData(component, ref local, lotBounds, flatBounds);
                nativeArray12[index5] = buildingTerraformData;
              }
              objectGeometryData.m_Size = math.max(ObjectUtils.GetSize(objectGeometryData.m_Bounds), new float3(1f, 5f, 1f));
              nativeArray3[index5] = objectGeometryData;
              nativeArray5[index5] = buildingExtensionData;
            }
          }
          if (nativeArray7.Length != 0)
          {
            for (int index7 = 0; index7 < nativeArray7.Length; ++index7)
            {
              Entity e = nativeArray1[index7];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              BuildingPrefab prefab = this.m_PrefabSystem.GetPrefab<BuildingPrefab>(nativeArray2[index7]);
              BuildingPropertyData buildingPropertyData = nativeArray10.Length != 0 ? nativeArray10[index7] : new BuildingPropertyData();
              SpawnableBuildingData spawnableBuildingData = nativeArray7[index7];
              if (spawnableBuildingData.m_ZonePrefab != Entity.Null)
              {
                Entity zonePrefab = spawnableBuildingData.m_ZonePrefab;
                ZoneData zoneData = rwComponentLookup[zonePrefab];
                if (!flag2)
                {
                  entityCommandBuffer.SetSharedComponent<BuildingSpawnGroupData>(e, new BuildingSpawnGroupData(zoneData.m_ZoneType));
                  ushort num = (ushort) math.clamp(Mathf.CeilToInt(nativeArray3[index7].m_Size.y), 0, (int) ushort.MaxValue);
                  if (spawnableBuildingData.m_Level == (byte) 1)
                  {
                    if (prefab.m_LotWidth == 1 && (zoneData.m_ZoneFlags & ZoneFlags.SupportNarrow) == (ZoneFlags) 0)
                    {
                      zoneData.m_ZoneFlags |= ZoneFlags.SupportNarrow;
                      rwComponentLookup[zonePrefab] = zoneData;
                    }
                    if (prefab.m_AccessType == BuildingAccessType.LeftCorner && (zoneData.m_ZoneFlags & ZoneFlags.SupportLeftCorner) == (ZoneFlags) 0)
                    {
                      zoneData.m_ZoneFlags |= ZoneFlags.SupportLeftCorner;
                      rwComponentLookup[zonePrefab] = zoneData;
                    }
                    if (prefab.m_AccessType == BuildingAccessType.RightCorner && (zoneData.m_ZoneFlags & ZoneFlags.SupportRightCorner) == (ZoneFlags) 0)
                    {
                      zoneData.m_ZoneFlags |= ZoneFlags.SupportRightCorner;
                      rwComponentLookup[zonePrefab] = zoneData;
                    }
                    if (prefab.m_AccessType == BuildingAccessType.Front && prefab.m_LotWidth <= 3 && prefab.m_LotDepth <= 2)
                    {
                      if ((prefab.m_LotWidth == 1 || prefab.m_LotWidth == 3) && (int) num < (int) zoneData.m_MinOddHeight)
                      {
                        zoneData.m_MinOddHeight = num;
                        rwComponentLookup[zonePrefab] = zoneData;
                      }
                      if ((prefab.m_LotWidth == 1 || prefab.m_LotWidth == 2) && (int) num < (int) zoneData.m_MinEvenHeight)
                      {
                        zoneData.m_MinEvenHeight = num;
                        rwComponentLookup[zonePrefab] = zoneData;
                      }
                    }
                  }
                  if ((int) num > (int) zoneData.m_MaxHeight)
                  {
                    zoneData.m_MaxHeight = num;
                    rwComponentLookup[zonePrefab] = zoneData;
                  }
                }
                int level = (int) spawnableBuildingData.m_Level;
                BuildingData buildingData = nativeArray4[index7];
                int lotSize = buildingData.m_LotSize.x * buildingData.m_LotSize.y;
                if (nativeArray6.Length != 0 && !prefab.Has<ServiceConsumption>() && roComponentLookup.HasComponent(zonePrefab))
                {
                  ZoneServiceConsumptionData serviceConsumptionData = roComponentLookup[zonePrefab];
                  ref ConsumptionData local = ref nativeArray6.ElementAt<ConsumptionData>(index7);
                  if (flag2)
                    level = 2;
                  bool isStorage = buildingPropertyData.m_AllowedStored > Resource.NoResource;
                  // ISSUE: reference to a compiler-generated field
                  EconomyParameterData singleton = this.__query_547773812_0.GetSingleton<EconomyParameterData>();
                  // ISSUE: reference to a compiler-generated method
                  int upkeep = PropertyRenterSystem.GetUpkeep(level, serviceConsumptionData.m_Upkeep, lotSize, zoneData.m_AreaType, ref singleton, isStorage);
                  local.m_Upkeep = upkeep;
                }
              }
            }
          }
          if (nativeArray8.Length != 0)
          {
            if (nativeArray9.Length != 0)
            {
              for (int index8 = 0; index8 < nativeArray8.Length; ++index8)
              {
                PlaceableObjectData placeableObjectData = nativeArray8[index8];
                ServiceUpgradeData serviceUpgradeData = nativeArray9[index8];
                if (nativeArray4.Length != 0)
                {
                  placeableObjectData.m_Flags |= Game.Objects.PlacementFlags.OwnerSide;
                  if ((double) serviceUpgradeData.m_MaxPlacementDistance != 0.0)
                    placeableObjectData.m_Flags |= Game.Objects.PlacementFlags.RoadSide;
                }
                placeableObjectData.m_ConstructionCost = serviceUpgradeData.m_UpgradeCost;
                nativeArray8[index8] = placeableObjectData;
              }
            }
            else
            {
              for (int index9 = 0; index9 < nativeArray8.Length; ++index9)
              {
                PlaceableObjectData placeableObjectData = nativeArray8[index9];
                ObjectGeometryData objectGeometryData = nativeArray3[index9];
                if (nativeArray4.Length != 0)
                  placeableObjectData.m_Flags |= Game.Objects.PlacementFlags.RoadSide;
                if ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.NetObject) != Game.Objects.PlacementFlags.None)
                {
                  objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.IgnoreLegCollision;
                  if (nativeArray4.Length != 0)
                  {
                    BuildingData buildingData = nativeArray4[index9];
                    buildingData.m_Flags |= BuildingFlags.CanBeOnRoad;
                    nativeArray4[index9] = buildingData;
                  }
                }
                nativeArray8[index9] = placeableObjectData;
                nativeArray3[index9] = objectGeometryData;
              }
            }
          }
          bool flag5 = false;
          ServiceUpkeepData serviceUpkeepData;
          ResourceStack resourceStack1;
          if (flag1)
          {
            for (int index10 = 0; index10 < nativeArray1.Length; ++index10)
            {
              if (nativeArray6.Length != 0 && nativeArray6[index10].m_Upkeep > 0)
              {
                bool flag6 = false;
                DynamicBuffer<ServiceUpkeepData> dynamicBuffer = bufferAccessor2[index10];
                for (int index11 = 0; index11 < dynamicBuffer.Length; ++index11)
                {
                  if (dynamicBuffer[index11].m_Upkeep.m_Resource == Resource.Money)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    BuildingInitializeSystem.log.WarnFormat("Warning: {0} has monetary upkeep in both ConsumptionData and CityServiceUpkeep", (object) this.m_PrefabSystem.GetPrefab<PrefabBase>(nativeArray1[index10]).name);
                  }
                }
                if (!flag6)
                {
                  ref DynamicBuffer<ServiceUpkeepData> local1 = ref dynamicBuffer;
                  serviceUpkeepData = new ServiceUpkeepData();
                  serviceUpkeepData.m_ScaleWithUsage = false;
                  ref ServiceUpkeepData local2 = ref serviceUpkeepData;
                  resourceStack1 = new ResourceStack();
                  resourceStack1.m_Amount = nativeArray6[index10].m_Upkeep;
                  resourceStack1.m_Resource = Resource.Money;
                  ResourceStack resourceStack2 = resourceStack1;
                  local2.m_Upkeep = resourceStack2;
                  ServiceUpkeepData elem = serviceUpkeepData;
                  local1.Add(elem);
                  flag5 = true;
                }
              }
            }
          }
          if (bufferAccessor1.Length != 0)
          {
            for (int index12 = 0; index12 < bufferAccessor1.Length; ++index12)
            {
              Entity upgrade = nativeArray1[index12];
              DynamicBuffer<ServiceUpgradeBuilding> dynamicBuffer1 = bufferAccessor1[index12];
              for (int index13 = 0; index13 < dynamicBuffer1.Length; ++index13)
                this.EntityManager.GetBuffer<BuildingUpgradeElement>(dynamicBuffer1[index13].m_Building).Add(new BuildingUpgradeElement(upgrade));
              if (!flag5 && nativeArray6.Length != 0 && nativeArray6[index12].m_Upkeep > 0)
              {
                DynamicBuffer<ServiceUpkeepData> dynamicBuffer2 = bufferAccessor2[index12];
                ref DynamicBuffer<ServiceUpkeepData> local3 = ref dynamicBuffer2;
                serviceUpkeepData = new ServiceUpkeepData();
                serviceUpkeepData.m_ScaleWithUsage = false;
                ref ServiceUpkeepData local4 = ref serviceUpkeepData;
                resourceStack1 = new ResourceStack();
                resourceStack1.m_Amount = nativeArray6[index12].m_Upkeep;
                resourceStack1.m_Resource = Resource.Money;
                ResourceStack resourceStack3 = resourceStack1;
                local4.m_Upkeep = resourceStack3;
                ServiceUpkeepData elem = serviceUpkeepData;
                local3.Add(elem);
              }
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioSpotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioSourceData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VFXData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CargoTransportStationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PublicTransportStationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransformerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WastewaterTreatmentPlantData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterTowerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorFacilityData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      new BuildingInitializeSystem.FindConnectionRequirementsJob()
      {
        m_SpawnableBuildingDataType = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle,
        m_ServiceUpgradeDataType = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentTypeHandle,
        m_ExtractorFacilityDataType = this.__TypeHandle.__Game_Prefabs_ExtractorFacilityData_RO_ComponentTypeHandle,
        m_ConsumptionDataType = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentTypeHandle,
        m_WorkplaceDataType = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentTypeHandle,
        m_WaterPumpingStationDataType = this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentTypeHandle,
        m_WaterTowerDataType = this.__TypeHandle.__Game_Prefabs_WaterTowerData_RO_ComponentTypeHandle,
        m_SewageOutletDataType = this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle,
        m_WastewaterTreatmentPlantDataType = this.__TypeHandle.__Game_Prefabs_WastewaterTreatmentPlantData_RO_ComponentTypeHandle,
        m_TransformerDataType = this.__TypeHandle.__Game_Prefabs_TransformerData_RO_ComponentTypeHandle,
        m_ParkingFacilityDataType = this.__TypeHandle.__Game_Prefabs_ParkingFacilityData_RO_ComponentTypeHandle,
        m_PublicTransportStationDataType = this.__TypeHandle.__Game_Prefabs_PublicTransportStationData_RO_ComponentTypeHandle,
        m_CargoTransportStationDataType = this.__TypeHandle.__Game_Prefabs_CargoTransportStationData_RO_ComponentTypeHandle,
        m_SubNetType = this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferTypeHandle,
        m_SubMeshType = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferTypeHandle,
        m_BuildingDataType = this.__TypeHandle.__Game_Prefabs_BuildingData_RW_ComponentTypeHandle,
        m_EffectType = this.__TypeHandle.__Game_Prefabs_Effect_RW_BufferTypeHandle,
        m_NetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_MeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
        m_EffectData = this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup,
        m_VFXData = this.__TypeHandle.__Game_Prefabs_VFXData_RO_ComponentLookup,
        m_AudioSourceData = this.__TypeHandle.__Game_Prefabs_AudioSourceData_RO_BufferLookup,
        m_AudioSpotData = this.__TypeHandle.__Game_Prefabs_AudioSpotData_RO_ComponentLookup,
        m_AudioEffectData = this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_Chunks = archetypeChunkArray,
        m_BuildingConfigurationData = this.m_ConfigurationQuery.GetSingleton<BuildingConfigurationData>()
      }.Schedule<BuildingInitializeSystem.FindConnectionRequirementsJob>(archetypeChunkArray.Length, 1).Complete();
      archetypeChunkArray.Dispose();
      entityCommandBuffer.Playback(this.EntityManager);
      entityCommandBuffer.Dispose();
    }

    private void InitializeLotSize(
      BuildingPrefab buildingPrefab,
      BuildingTerraformOverride terraformOverride,
      ref ObjectGeometryData objectGeometryData,
      ref BuildingTerraformData buildingTerraformData,
      ref BuildingData buildingData)
    {
      buildingData.m_LotSize = new int2(buildingPrefab.m_LotWidth, buildingPrefab.m_LotDepth);
      float2 float2_1 = new float2((float) buildingPrefab.m_LotWidth, (float) buildingPrefab.m_LotDepth) * 8f;
      bool flag = false;
      Bounds2 flatBounds;
      if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
      {
        int2 int2;
        int2.x = Mathf.RoundToInt(objectGeometryData.m_LegSize.x / 8f);
        int2.y = Mathf.RoundToInt(objectGeometryData.m_LegSize.z / 8f);
        flag = math.all(int2 == buildingData.m_LotSize);
        buildingData.m_LotSize = int2;
        float2 xz = objectGeometryData.m_Pivot.xz;
        float2 float2_2 = objectGeometryData.m_LegSize.xz * 0.5f;
        flatBounds = new Bounds2(xz - float2_2, xz + float2_2);
      }
      else
        flatBounds = objectGeometryData.m_Bounds.xz;
      Bounds2 lotBounds;
      lotBounds.max = (float2) buildingData.m_LotSize * 4f;
      lotBounds.min = -lotBounds.max;
      // ISSUE: reference to a compiler-generated method
      BuildingInitializeSystem.InitializeTerraformData(terraformOverride, ref buildingTerraformData, lotBounds, flatBounds);
      objectGeometryData.m_Layers |= MeshLayer.Default;
      objectGeometryData.m_MinLod = math.min(objectGeometryData.m_MinLod, RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(float2_1.x, 0.0f, float2_1.y))));
      switch (buildingPrefab.m_AccessType)
      {
        case BuildingAccessType.LeftCorner:
          buildingData.m_Flags |= BuildingFlags.LeftAccess;
          break;
        case BuildingAccessType.RightCorner:
          buildingData.m_Flags |= BuildingFlags.RightAccess;
          break;
        case BuildingAccessType.LeftAndRightCorner:
          buildingData.m_Flags |= BuildingFlags.LeftAccess | BuildingFlags.RightAccess;
          break;
        case BuildingAccessType.LeftAndBackCorner:
          buildingData.m_Flags |= BuildingFlags.LeftAccess | BuildingFlags.BackAccess;
          break;
        case BuildingAccessType.RightAndBackCorner:
          buildingData.m_Flags |= BuildingFlags.RightAccess | BuildingFlags.BackAccess;
          break;
        case BuildingAccessType.FrontAndBack:
          buildingData.m_Flags |= BuildingFlags.BackAccess;
          break;
        case BuildingAccessType.All:
          buildingData.m_Flags |= BuildingFlags.LeftAccess | BuildingFlags.RightAccess | BuildingFlags.BackAccess;
          break;
      }
      if (!flag)
      {
        if (math.any(objectGeometryData.m_Size.xz > float2_1 + 0.5f))
        {
          // ISSUE: reference to a compiler-generated field
          BuildingInitializeSystem.log.WarnFormat("Building geometry doesn't fit inside the lot ({0}): {1}m x {2}m ({3}x{4})", (object) buildingPrefab.name, (object) objectGeometryData.m_Size.x, (object) objectGeometryData.m_Size.z, (object) buildingData.m_LotSize.x, (object) buildingData.m_LotSize.y);
        }
        float2 float2_3 = float2_1 - 0.4f;
        objectGeometryData.m_Size.xz = float2_3;
        objectGeometryData.m_Bounds.min.xz = float2_3 * -0.5f;
        objectGeometryData.m_Bounds.max.xz = float2_3 * 0.5f;
      }
      objectGeometryData.m_Size.y = math.max(objectGeometryData.m_Size.y, 5f);
      objectGeometryData.m_Bounds.min.y = math.min(objectGeometryData.m_Bounds.min.y, 0.0f);
      objectGeometryData.m_Bounds.max.y = math.max(objectGeometryData.m_Bounds.max.y, 5f);
    }

    public static void InitializeTerraformData(
      BuildingTerraformOverride terraformOverride,
      ref BuildingTerraformData buildingTerraformData,
      Bounds2 lotBounds,
      Bounds2 flatBounds)
    {
      float3 float3_1 = new float3(1f, 0.0f, 1f);
      float3 float3_2 = new float3(1f, 0.0f, 1f);
      float3 float3_3 = new float3(1f, 0.0f, 1f);
      float3 float3_4 = new float3(1f, 0.0f, 1f);
      buildingTerraformData.m_Smooth.xy = lotBounds.min;
      buildingTerraformData.m_Smooth.zw = lotBounds.max;
      if ((UnityEngine.Object) terraformOverride != (UnityEngine.Object) null)
      {
        flatBounds.min += terraformOverride.m_LevelMinOffset;
        flatBounds.max += terraformOverride.m_LevelMaxOffset;
        float3_1.x = terraformOverride.m_LevelBackRight.x;
        float3_1.z = terraformOverride.m_LevelFrontRight.x;
        float3_2.x = terraformOverride.m_LevelBackRight.y;
        float3_2.z = terraformOverride.m_LevelBackLeft.y;
        float3_3.x = terraformOverride.m_LevelBackLeft.x;
        float3_3.z = terraformOverride.m_LevelFrontLeft.x;
        float3_4.x = terraformOverride.m_LevelFrontRight.y;
        float3_4.z = terraformOverride.m_LevelFrontLeft.y;
        buildingTerraformData.m_Smooth.xy += terraformOverride.m_SmoothMinOffset;
        buildingTerraformData.m_Smooth.zw += terraformOverride.m_SmoothMaxOffset;
        buildingTerraformData.m_HeightOffset = terraformOverride.m_HeightOffset;
        buildingTerraformData.m_DontRaise = terraformOverride.m_DontRaise;
        buildingTerraformData.m_DontLower = terraformOverride.m_DontLower;
      }
      float3 float3_5 = flatBounds.min.x + float3_1;
      float3 float3_6 = flatBounds.min.y + float3_2;
      float3 float3_7 = flatBounds.max.x - float3_3;
      float3 float3_8 = flatBounds.max.y - float3_4;
      float3 x1 = (float3_5 + float3_7) * 0.5f;
      float3 x2 = (float3_6 + float3_8) * 0.5f;
      buildingTerraformData.m_FlatX0 = math.min(float3_5, math.max(x1, float3_7));
      buildingTerraformData.m_FlatZ0 = math.min(float3_6, math.max(x2, float3_8));
      buildingTerraformData.m_FlatX1 = math.max(float3_7, math.min(x1, float3_5));
      buildingTerraformData.m_FlatZ1 = math.max(float3_8, math.min(x2, float3_6));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_547773812_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EconomyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public BuildingInitializeSystem()
    {
    }

    [BurstCompile]
    private struct FindConnectionRequirementsJob : IJobParallelFor
    {
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> m_SpawnableBuildingDataType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceUpgradeData> m_ServiceUpgradeDataType;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorFacilityData> m_ExtractorFacilityDataType;
      [ReadOnly]
      public ComponentTypeHandle<ConsumptionData> m_ConsumptionDataType;
      [ReadOnly]
      public ComponentTypeHandle<WorkplaceData> m_WorkplaceDataType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPumpingStationData> m_WaterPumpingStationDataType;
      [ReadOnly]
      public ComponentTypeHandle<WaterTowerData> m_WaterTowerDataType;
      [ReadOnly]
      public ComponentTypeHandle<SewageOutletData> m_SewageOutletDataType;
      [ReadOnly]
      public ComponentTypeHandle<WastewaterTreatmentPlantData> m_WastewaterTreatmentPlantDataType;
      [ReadOnly]
      public ComponentTypeHandle<TransformerData> m_TransformerDataType;
      [ReadOnly]
      public ComponentTypeHandle<ParkingFacilityData> m_ParkingFacilityDataType;
      [ReadOnly]
      public ComponentTypeHandle<PublicTransportStationData> m_PublicTransportStationDataType;
      [ReadOnly]
      public ComponentTypeHandle<CargoTransportStationData> m_CargoTransportStationDataType;
      [ReadOnly]
      public BufferTypeHandle<SubNet> m_SubNetType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      [ReadOnly]
      public BufferTypeHandle<SubMesh> m_SubMeshType;
      public ComponentTypeHandle<BuildingData> m_BuildingDataType;
      public BufferTypeHandle<Effect> m_EffectType;
      [ReadOnly]
      public ComponentLookup<NetData> m_NetData;
      [ReadOnly]
      public ComponentLookup<SpawnLocationData> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<MeshData> m_MeshData;
      [ReadOnly]
      public ComponentLookup<EffectData> m_EffectData;
      [ReadOnly]
      public ComponentLookup<VFXData> m_VFXData;
      [ReadOnly]
      public BufferLookup<AudioSourceData> m_AudioSourceData;
      [ReadOnly]
      public ComponentLookup<AudioSpotData> m_AudioSpotData;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> m_AudioEffectData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        NativeArray<BuildingData> nativeArray1 = chunk.GetNativeArray<BuildingData>(ref this.m_BuildingDataType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubMesh> bufferAccessor1 = chunk.GetBufferAccessor<SubMesh>(ref this.m_SubMeshType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubObject> bufferAccessor2 = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Effect> bufferAccessor3 = chunk.GetBufferAccessor<Effect>(ref this.m_EffectType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<SpawnableBuildingData>(ref this.m_SpawnableBuildingDataType))
        {
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            BuildingData buildingData = nativeArray1[index1];
            buildingData.m_Flags |= BuildingFlags.RequireRoad | BuildingFlags.RestrictedPedestrian | BuildingFlags.RestrictedCar | BuildingFlags.RestrictedParking | BuildingFlags.RestrictedTrack;
            if (bufferAccessor1[index1].Length == 0)
              buildingData.m_Flags |= BuildingFlags.ColorizeLot;
            DynamicBuffer<SubObject> subObjects;
            if (CollectionUtils.TryGet<SubObject>(bufferAccessor2, index1, out subObjects))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckPropFlags(ref buildingData.m_Flags, subObjects);
            }
            nativeArray1[index1] = buildingData;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<ServiceUpgradeData>(ref this.m_ServiceUpgradeDataType) || chunk.Has<ExtractorFacilityData>(ref this.m_ExtractorFacilityDataType))
          {
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              BuildingData buildingData = nativeArray1[index2];
              buildingData.m_Flags |= BuildingFlags.NoRoadConnection | BuildingFlags.RestrictedPedestrian | BuildingFlags.RestrictedCar | BuildingFlags.RestrictedParking | BuildingFlags.RestrictedTrack;
              if (bufferAccessor1[index2].Length == 0)
                buildingData.m_Flags |= BuildingFlags.ColorizeLot;
              DynamicBuffer<SubObject> subObjects;
              if (CollectionUtils.TryGet<SubObject>(bufferAccessor2, index2, out subObjects))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckPropFlags(ref buildingData.m_Flags, subObjects);
              }
              nativeArray1[index2] = buildingData;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<ConsumptionData> nativeArray2 = chunk.GetNativeArray<ConsumptionData>(ref this.m_ConsumptionDataType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<WorkplaceData> nativeArray3 = chunk.GetNativeArray<WorkplaceData>(ref this.m_WorkplaceDataType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<SubNet> bufferAccessor4 = chunk.GetBufferAccessor<SubNet>(ref this.m_SubNetType);
            // ISSUE: reference to a compiler-generated field
            bool flag1 = chunk.Has<WaterPumpingStationData>(ref this.m_WaterPumpingStationDataType);
            // ISSUE: reference to a compiler-generated field
            bool flag2 = chunk.Has<WaterTowerData>(ref this.m_WaterTowerDataType);
            // ISSUE: reference to a compiler-generated field
            bool flag3 = chunk.Has<SewageOutletData>(ref this.m_SewageOutletDataType);
            // ISSUE: reference to a compiler-generated field
            bool flag4 = chunk.Has<WastewaterTreatmentPlantData>(ref this.m_WastewaterTreatmentPlantDataType);
            // ISSUE: reference to a compiler-generated field
            bool flag5 = chunk.Has<TransformerData>(ref this.m_TransformerDataType);
            // ISSUE: reference to a compiler-generated field
            int num = chunk.Has<ParkingFacilityData>(ref this.m_ParkingFacilityDataType) ? 1 : 0;
            // ISSUE: reference to a compiler-generated field
            bool flag6 = chunk.Has<PublicTransportStationData>(ref this.m_PublicTransportStationDataType);
            // ISSUE: reference to a compiler-generated field
            bool flag7 = chunk.Has<CargoTransportStationData>(ref this.m_CargoTransportStationDataType);
            BuildingFlags buildingFlags = (BuildingFlags) 0;
            if (num == 0 && !flag6)
              buildingFlags |= BuildingFlags.RestrictedPedestrian;
            if (num == 0 && !flag7 && !flag6)
              buildingFlags |= BuildingFlags.RestrictedCar;
            if (num == 0)
              buildingFlags |= BuildingFlags.RestrictedParking;
            if (!flag6 && !flag7)
              buildingFlags |= BuildingFlags.RestrictedTrack;
            for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
            {
              Layer layer1 = Layer.None;
              Layer layer2 = Layer.None;
              Layer layer3 = Layer.None;
              if (nativeArray2.Length != 0)
              {
                ConsumptionData consumptionData = nativeArray2[index3];
                if ((double) consumptionData.m_ElectricityConsumption > 0.0)
                  layer1 |= Layer.PowerlineLow;
                if ((double) consumptionData.m_GarbageAccumulation > 0.0)
                  layer1 |= Layer.Road;
                if ((double) consumptionData.m_WaterConsumption > 0.0)
                  layer1 |= Layer.WaterPipe | Layer.SewagePipe;
              }
              if (nativeArray3.Length != 0 && nativeArray3[index3].m_MaxWorkers > 0)
                layer1 |= Layer.Road;
              if (flag1 | flag2)
                layer1 |= Layer.WaterPipe;
              if (flag3 | flag4)
                layer1 |= Layer.SewagePipe;
              if (flag5)
                layer1 |= Layer.PowerlineLow;
              if (layer1 != Layer.None && bufferAccessor4.Length != 0)
              {
                DynamicBuffer<SubNet> dynamicBuffer = bufferAccessor4[index3];
                for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
                {
                  SubNet subNet = dynamicBuffer[index4];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_NetData.HasComponent(subNet.m_Prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    NetData netData = this.m_NetData[subNet.m_Prefab];
                    if ((netData.m_RequiredLayers & Layer.Road) == Layer.None)
                    {
                      layer2 |= netData.m_RequiredLayers | netData.m_LocalConnectLayers;
                      layer3 |= netData.m_RequiredLayers;
                    }
                  }
                }
              }
              BuildingData buildingData = nativeArray1[index3];
              buildingData.m_Flags |= buildingFlags;
              if ((layer1 & ~layer2) != Layer.None)
                buildingData.m_Flags |= BuildingFlags.RequireRoad;
              if ((layer3 & Layer.PowerlineLow) != Layer.None)
                buildingData.m_Flags |= BuildingFlags.HasLowVoltageNode;
              if ((layer3 & Layer.WaterPipe) != Layer.None)
                buildingData.m_Flags |= BuildingFlags.HasWaterNode;
              if ((layer3 & Layer.SewagePipe) != Layer.None)
                buildingData.m_Flags |= BuildingFlags.HasSewageNode;
              if (bufferAccessor1[index3].Length == 0)
                buildingData.m_Flags |= BuildingFlags.ColorizeLot;
              DynamicBuffer<SubObject> subObjects;
              if (CollectionUtils.TryGet<SubObject>(bufferAccessor2, index3, out subObjects))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckPropFlags(ref buildingData.m_Flags, subObjects);
              }
              nativeArray1[index3] = buildingData;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(index);
label_82:
        Effect effect1;
        for (int index5 = 0; index5 < bufferAccessor3.Length; ++index5)
        {
          DynamicBuffer<Effect> dynamicBuffer1 = bufferAccessor3[index5];
          DynamicBuffer<SubMesh> dynamicBuffer2 = bufferAccessor1[index5];
          bool2 x1 = new bool2(false, nativeArray1.Length == 0);
          for (int index6 = 0; index6 < dynamicBuffer1.Length; ++index6)
          {
            EffectData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_EffectData.TryGetComponent(dynamicBuffer1[index6].m_Effect, out componentData) && (componentData.m_Flags.m_RequiredFlags & EffectConditionFlags.Collapsing) != EffectConditionFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              x1.x |= this.m_VFXData.HasComponent(dynamicBuffer1[index6].m_Effect);
              // ISSUE: reference to a compiler-generated field
              x1.y |= this.m_AudioSourceData.HasBuffer(dynamicBuffer1[index6].m_Effect);
              if (math.all(x1))
                goto label_82;
            }
          }
          for (int index7 = 0; index7 < dynamicBuffer2.Length; ++index7)
          {
            SubMesh subMesh = dynamicBuffer2[index7];
            MeshData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshData.TryGetComponent(subMesh.m_SubMesh, out componentData))
            {
              float2 float2_1 = MathUtils.Center(componentData.m_Bounds.xz);
              float2 x2 = MathUtils.Size(componentData.m_Bounds.xz);
              int2 int2 = math.max((int2) 1, (int2) (math.sqrt(x2) * 0.5f));
              float2 float2_2 = x2 / (float2) int2;
              float3 float3_1 = math.rotate(subMesh.m_Rotation, new float3(float2_2.x, 0.0f, 0.0f));
              float3 float3_2 = math.rotate(subMesh.m_Rotation, new float3(0.0f, 0.0f, float2_2.y));
              float3 float3_3 = subMesh.m_Position + math.rotate(subMesh.m_Rotation, new float3(float2_1.x, 0.0f, float2_1.y));
              if (!x1.y)
              {
                ref DynamicBuffer<Effect> local = ref dynamicBuffer1;
                effect1 = new Effect();
                // ISSUE: reference to a compiler-generated field
                effect1.m_Effect = this.m_BuildingConfigurationData.m_CollapseSFX;
                effect1.m_Position = float3_3;
                effect1.m_Rotation = subMesh.m_Rotation;
                effect1.m_Scale = (float3) 1f;
                effect1.m_Intensity = 1f;
                effect1.m_ParentMesh = index7;
                effect1.m_AnimationIndex = -1;
                effect1.m_Procedural = true;
                Effect elem = effect1;
                local.Add(elem);
              }
              if (!x1.x)
              {
                float3 float3_4 = new float3(float2_2.x * 0.05f, 1f, float2_2.y * 0.05f);
                float3 float3_5 = float3_3 - (float3_1 * (float) ((double) int2.x * 0.5 - 0.5) + float3_2 * (float) ((double) int2.y * 0.5 - 0.5));
                float3_4.y = (float) (((double) float3_4.x + (double) float3_4.y) * 0.5);
                dynamicBuffer1.Capacity = dynamicBuffer1.Length + int2.x * int2.y;
                for (int y = 0; y < int2.y; ++y)
                {
                  for (int x3 = 0; x3 < int2.x; ++x3)
                  {
                    float2 float2_3 = new float2((float) x3, (float) y) + random.NextFloat2((float2) -0.25f, (float2) 0.25f);
                    ref DynamicBuffer<Effect> local = ref dynamicBuffer1;
                    effect1 = new Effect();
                    // ISSUE: reference to a compiler-generated field
                    effect1.m_Effect = this.m_BuildingConfigurationData.m_CollapseVFX;
                    effect1.m_Position = float3_5 + float3_1 * float2_3.x + float3_2 * float2_3.y;
                    effect1.m_Rotation = subMesh.m_Rotation;
                    effect1.m_Scale = float3_4;
                    effect1.m_Intensity = 1f;
                    effect1.m_ParentMesh = index7;
                    effect1.m_AnimationIndex = -1;
                    effect1.m_Procedural = true;
                    Effect elem = effect1;
                    local.Add(elem);
                  }
                }
              }
            }
          }
        }
        NativeList<Effect> nativeList1 = new NativeList<Effect>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<float3> nativeList2 = new NativeList<float3>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        float num1 = 125f;
        for (int index8 = 0; index8 < bufferAccessor3.Length; ++index8)
        {
          DynamicBuffer<Effect> effects = bufferAccessor3[index8];
          bool2 hasFireSfxEffects = new bool2(false, false);
          for (int index9 = 0; index9 < effects.Length; ++index9)
          {
            EffectData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_EffectData.TryGetComponent(effects[index9].m_Effect, out componentData) && (componentData.m_Flags.m_RequiredFlags & EffectConditionFlags.OnFire) != EffectConditionFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              hasFireSfxEffects.x |= this.m_AudioEffectData.HasComponent(effects[index9].m_Effect);
              // ISSUE: reference to a compiler-generated field
              hasFireSfxEffects.y |= this.m_AudioSpotData.HasComponent(effects[index9].m_Effect);
              ref NativeList<Effect> local1 = ref nativeList1;
              effect1 = effects[index9];
              ref Effect local2 = ref effect1;
              local1.Add(in local2);
            }
          }
          for (int index10 = 0; index10 < nativeList1.Length; ++index10)
          {
            Effect effect2 = nativeList1[index10];
            bool flag = false;
            for (int index11 = 0; index11 < nativeList2.Length; ++index11)
            {
              if ((double) math.distance(effect2.m_Position, nativeList2[index11]) < (double) num1)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              nativeList2.Add(in effect2.m_Position);
              // ISSUE: reference to a compiler-generated method
              this.AddFireSfxToBuilding(ref hasFireSfxEffects, effects, effect2.m_Position, effect2.m_Rotation, effect2.m_ParentMesh);
            }
          }
          nativeList1.Clear();
          nativeList2.Clear();
        }
      }

      private void AddFireSfxToBuilding(
        ref bool2 hasFireSfxEffects,
        DynamicBuffer<Effect> effects,
        float3 position,
        quaternion rotation,
        int parent)
      {
        Effect effect;
        if (!hasFireSfxEffects.x)
        {
          ref DynamicBuffer<Effect> local = ref effects;
          effect = new Effect();
          // ISSUE: reference to a compiler-generated field
          effect.m_Effect = this.m_BuildingConfigurationData.m_FireLoopSFX;
          effect.m_Position = position;
          effect.m_Rotation = rotation;
          effect.m_Scale = (float3) 1f;
          effect.m_Intensity = 1f;
          effect.m_ParentMesh = parent;
          effect.m_AnimationIndex = -1;
          effect.m_Procedural = true;
          Effect elem = effect;
          local.Add(elem);
        }
        if (hasFireSfxEffects.y)
          return;
        ref DynamicBuffer<Effect> local1 = ref effects;
        effect = new Effect();
        // ISSUE: reference to a compiler-generated field
        effect.m_Effect = this.m_BuildingConfigurationData.m_FireSpotSFX;
        effect.m_Position = position;
        effect.m_Rotation = rotation;
        effect.m_Scale = (float3) 1f;
        effect.m_Intensity = 1f;
        effect.m_ParentMesh = parent;
        effect.m_AnimationIndex = -1;
        effect.m_Procedural = true;
        Effect elem1 = effect;
        local1.Add(elem1);
      }

      private void CheckPropFlags(
        ref BuildingFlags flags,
        DynamicBuffer<SubObject> subObjects,
        int maxDepth = 10)
      {
        if (--maxDepth < 0)
          return;
        for (int index = 0; index < subObjects.Length; ++index)
        {
          SubObject subObject = subObjects[index];
          SpawnLocationData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpawnLocationData.TryGetComponent(subObject.m_Prefab, out componentData) && componentData.m_ActivityMask.m_Mask == 0U && componentData.m_ConnectionType == RouteConnectionType.Pedestrian)
            flags |= BuildingFlags.HasInsideRoom;
          DynamicBuffer<SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(subObject.m_Prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckPropFlags(ref flags, bufferData, maxDepth);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<BuildingData> __Game_Prefabs_BuildingData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<BuildingTerraformData> __Game_Prefabs_BuildingTerraformData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ConsumptionData> __Game_Prefabs_ConsumptionData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ServiceUpgradeData> __Game_Prefabs_ServiceUpgradeData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterPoweredData> __Game_Prefabs_WaterPoweredData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SewageOutletData> __Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ServiceUpgradeBuilding> __Game_Prefabs_ServiceUpgradeBuilding_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CollectedServiceBuildingBudgetData> __Game_Simulation_CollectedServiceBuildingBudgetData_RO_ComponentTypeHandle;
      public BufferTypeHandle<ServiceUpkeepData> __Game_Prefabs_ServiceUpkeepData_RW_BufferTypeHandle;
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneServiceConsumptionData> __Game_Prefabs_ZoneServiceConsumptionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<ExtractorFacilityData> __Game_Prefabs_ExtractorFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ConsumptionData> __Game_Prefabs_ConsumptionData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterPumpingStationData> __Game_Prefabs_WaterPumpingStationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterTowerData> __Game_Prefabs_WaterTowerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WastewaterTreatmentPlantData> __Game_Prefabs_WastewaterTreatmentPlantData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransformerData> __Game_Prefabs_TransformerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkingFacilityData> __Game_Prefabs_ParkingFacilityData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PublicTransportStationData> __Game_Prefabs_PublicTransportStationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CargoTransportStationData> __Game_Prefabs_CargoTransportStationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubNet> __Game_Prefabs_SubNet_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Prefabs_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubMesh> __Game_Prefabs_SubMesh_RO_BufferTypeHandle;
      public BufferTypeHandle<Effect> __Game_Prefabs_Effect_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EffectData> __Game_Prefabs_EffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VFXData> __Game_Prefabs_VFXData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<AudioSourceData> __Game_Prefabs_AudioSourceData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<AudioSpotData> __Game_Prefabs_AudioSpotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> __Game_Prefabs_AudioEffectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Prefabs_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingExtensionData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingTerraformData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingTerraformData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ConsumptionData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectGeometryData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PlaceableObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceUpgradeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPoweredData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SewageOutletData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SewageOutletData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferTypeHandle = state.GetBufferTypeHandle<ServiceUpgradeBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedServiceBuildingBudgetData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CollectedServiceBuildingBudgetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpkeepData_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceUpkeepData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RW_ComponentLookup = state.GetComponentLookup<ZoneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneServiceConsumptionData_RO_ComponentLookup = state.GetComponentLookup<ZoneServiceConsumptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ExtractorFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ConsumptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPumpingStationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPumpingStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterTowerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterTowerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WastewaterTreatmentPlantData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WastewaterTreatmentPlantData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransformerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransformerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingFacilityData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkingFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PublicTransportStationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PublicTransportStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportStationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CargoTransportStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RW_BufferTypeHandle = state.GetBufferTypeHandle<Effect>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectData_RO_ComponentLookup = state.GetComponentLookup<EffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VFXData_RO_ComponentLookup = state.GetComponentLookup<VFXData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioSourceData_RO_BufferLookup = state.GetBufferLookup<AudioSourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioSpotData_RO_ComponentLookup = state.GetComponentLookup<AudioSpotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioEffectData_RO_ComponentLookup = state.GetComponentLookup<AudioEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
      }
    }
  }
}
