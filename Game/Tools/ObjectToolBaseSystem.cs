// Decompiled with JetBrains decompiler
// Type: Game.Tools.ObjectToolBaseSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public abstract class ObjectToolBaseSystem : ToolBaseSystem
  {
    protected ToolOutputBarrier m_ToolOutputBarrier;
    protected Game.Objects.SearchSystem m_ObjectSearchSystem;
    protected WaterSystem m_WaterSystem;
    protected TerrainSystem m_TerrainSystem;
    private ObjectToolBaseSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
    }

    protected JobHandle CreateDefinitions(
      Entity objectPrefab,
      Entity transformPrefab,
      Entity brushPrefab,
      Entity owner,
      Entity original,
      Entity laneEditor,
      Entity theme,
      NativeList<ControlPoint> controlPoints,
      NativeReference<ObjectToolBaseSystem.AttachmentData> attachmentPrefab,
      bool editorMode,
      bool lefthandTraffic,
      bool removing,
      bool stamping,
      float brushSize,
      float brushAngle,
      float brushStrength,
      float distance,
      float deltaTime,
      RandomSeed randomSeed,
      Snap snap,
      AgeMask ageMask,
      JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BrushCell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CreatureSpawnData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BrushData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle definitions = new ObjectToolBaseSystem.CreateDefinitionsJob()
      {
        m_EditorMode = editorMode,
        m_LefthandTraffic = lefthandTraffic,
        m_Removing = removing,
        m_Stamping = stamping,
        m_BrushSize = brushSize,
        m_BrushAngle = brushAngle,
        m_BrushStrength = brushStrength,
        m_Distance = distance,
        m_DeltaTime = deltaTime,
        m_ObjectPrefab = objectPrefab,
        m_TransformPrefab = transformPrefab,
        m_BrushPrefab = brushPrefab,
        m_Owner = owner,
        m_Original = original,
        m_LaneEditor = laneEditor,
        m_Theme = theme,
        m_RandomSeed = randomSeed,
        m_Snap = snap,
        m_AgeMask = ageMask,
        m_ControlPoints = controlPoints,
        m_AttachmentPrefab = attachmentPrefab,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_LotData = this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_AreaClearData = this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup,
        m_AreaSpaceData = this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup,
        m_AreaLotData = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
        m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetObjectData = this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabAssetStampData = this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup,
        m_PrefabBuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabPlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_PrefabBrushData = this.__TypeHandle.__Game_Prefabs_BrushData_RO_ComponentLookup,
        m_PrefabBuildingTerraformData = this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup,
        m_PrefabCreatureSpawnData = this.__TypeHandle.__Game_Prefabs_CreatureSpawnData_RO_ComponentLookup,
        m_PlaceholderBuildingData = this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_CachedNodes = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_PrefabSubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup,
        m_PrefabSubNets = this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup,
        m_PrefabSubLanes = this.__TypeHandle.__Game_Prefabs_SubLane_RO_BufferLookup,
        m_PrefabSubAreas = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup,
        m_PrefabSubAreaNodes = this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup,
        m_PrefabPlaceholderElements = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_PrefabRequirementElements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup,
        m_PrefabServiceUpgradeBuilding = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferLookup,
        m_PrefabBrushCells = this.__TypeHandle.__Game_Prefabs_BrushCell_RO_BufferLookup,
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
      }.Schedule<ObjectToolBaseSystem.CreateDefinitionsJob>(JobHandle.CombineDependencies(inputDeps, dependencies, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(definitions);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(definitions);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(definitions);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(definitions);
      return definitions;
    }

    public static int GetFirstNodeIndex(DynamicBuffer<SubAreaNode> nodes, int2 range)
    {
      int firstNodeIndex = 0;
      float num1 = float.MaxValue;
      for (int x = range.x; x < range.y; ++x)
      {
        int index = math.select(x + 1, range.x, x + 1 == range.y);
        SubAreaNode node = nodes[x];
        float2 xz1 = node.m_Position.xz;
        node = nodes[index];
        float2 xz2 = node.m_Position.xz;
        float num2 = MathUtils.Distance(new Line2.Segment(xz1, xz2), new float2(), out float _);
        if ((double) num2 < (double) num1)
        {
          firstNodeIndex = x;
          num1 = num2;
        }
      }
      return firstNodeIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    protected ObjectToolBaseSystem()
    {
    }

    public struct AttachmentData
    {
      public Entity m_Entity;
      public float3 m_Offset;
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_LefthandTraffic;
      [ReadOnly]
      public bool m_Removing;
      [ReadOnly]
      public bool m_Stamping;
      [ReadOnly]
      public float m_BrushSize;
      [ReadOnly]
      public float m_BrushAngle;
      [ReadOnly]
      public float m_BrushStrength;
      [ReadOnly]
      public float m_Distance;
      [ReadOnly]
      public float m_DeltaTime;
      [ReadOnly]
      public Entity m_ObjectPrefab;
      [ReadOnly]
      public Entity m_TransformPrefab;
      [ReadOnly]
      public Entity m_BrushPrefab;
      [ReadOnly]
      public Entity m_Owner;
      [ReadOnly]
      public Entity m_Original;
      [ReadOnly]
      public Entity m_LaneEditor;
      [ReadOnly]
      public Entity m_Theme;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public Snap m_Snap;
      [ReadOnly]
      public AgeMask m_AgeMask;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeReference<ObjectToolBaseSystem.AttachmentData> m_AttachmentPrefab;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Lot> m_LotData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<Clear> m_AreaClearData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Space> m_AreaSpaceData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_AreaLotData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetObjectData> m_PrefabNetObjectData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<AssetStampData> m_PrefabAssetStampData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> m_PlaceholderBuildingData;
      [ReadOnly]
      public ComponentLookup<BrushData> m_PrefabBrushData;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> m_PrefabBuildingTerraformData;
      [ReadOnly]
      public ComponentLookup<CreatureSpawnData> m_PrefabCreatureSpawnData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> m_CachedNodes;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> m_PrefabSubObjects;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> m_PrefabSubNets;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubLane> m_PrefabSubLanes;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;
      [ReadOnly]
      public BufferLookup<SubAreaNode> m_PrefabSubAreaNodes;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderElements;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_PrefabRequirementElements;
      [ReadOnly]
      public BufferLookup<ServiceUpgradeBuilding> m_PrefabServiceUpgradeBuilding;
      [ReadOnly]
      public BufferLookup<BrushCell> m_PrefabBrushCells;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint = this.m_ControlPoints[0];
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_Owner;
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = this.m_Original;
        Entity updatedTopLevel = Entity.Null;
        Entity lotEntity = Entity.Null;
        OwnerDefinition ownerDefinition = new OwnerDefinition();
        bool upgrade = false;
        bool relocate = entity2 != Entity.Null;
        bool topLevel = true;
        int parentMesh1 = entity1 != Entity.Null ? 0 : -1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!relocate && this.m_PrefabNetObjectData.HasComponent(this.m_ObjectPrefab) && this.m_AttachedData.HasComponent(controlPoint.m_OriginalEntity) && (this.m_EditorMode || !this.m_OwnerData.HasComponent(controlPoint.m_OriginalEntity)))
        {
          // ISSUE: reference to a compiler-generated field
          Attached attached = this.m_AttachedData[controlPoint.m_OriginalEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.HasComponent(attached.m_Parent) || this.m_EdgeData.HasComponent(attached.m_Parent))
          {
            entity2 = controlPoint.m_OriginalEntity;
            controlPoint.m_OriginalEntity = attached.m_Parent;
            upgrade = true;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorMode)
        {
          Entity entity3 = controlPoint.m_OriginalEntity;
          int num = controlPoint.m_ElementIndex.x;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (; this.m_OwnerData.HasComponent(entity3) && !this.m_BuildingData.HasComponent(entity3); entity3 = this.m_OwnerData[entity3].m_Owner)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.HasComponent(entity3))
            {
              // ISSUE: reference to a compiler-generated field
              int parentMesh2 = this.m_LocalTransformCacheData[entity3].m_ParentMesh;
              num = parentMesh2 + math.select(1000, -1000, parentMesh2 < 0);
            }
          }
          DynamicBuffer<InstalledUpgrade> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_InstalledUpgrades.TryGetBuffer(entity3, out bufferData1) && bufferData1.Length != 0)
            entity3 = bufferData1[0].m_Upgrade;
          bool flag = false;
          PrefabRef componentData1;
          DynamicBuffer<ServiceUpgradeBuilding> bufferData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.TryGetComponent(entity3, out componentData1) && this.m_PrefabServiceUpgradeBuilding.TryGetBuffer(this.m_ObjectPrefab, out bufferData2))
          {
            Entity entity4 = Entity.Null;
            Game.Objects.Transform componentData2;
            BuildingExtensionData componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.TryGetComponent(entity3, out componentData2) && this.m_PrefabBuildingExtensionData.TryGetComponent(this.m_ObjectPrefab, out componentData3))
            {
              for (int index = 0; index < bufferData2.Length; ++index)
              {
                if (bufferData2[index].m_Building == componentData1.m_Prefab)
                {
                  entity4 = entity3;
                  controlPoint.m_Position = ObjectUtils.LocalToWorld(componentData2, componentData3.m_Position);
                  controlPoint.m_Rotation = componentData2.m_Rotation;
                  break;
                }
              }
            }
            entity3 = entity4;
            flag = true;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(entity3) && this.m_SubObjects.HasBuffer(entity3))
          {
            entity1 = entity3;
            topLevel = flag;
            parentMesh1 = num;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(entity2))
          {
            // ISSUE: reference to a compiler-generated field
            Owner owner = this.m_OwnerData[entity2];
            if (owner.m_Owner != entity1)
            {
              entity1 = owner.m_Owner;
              topLevel = flag;
              parentMesh1 = -1;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EdgeData.HasComponent(controlPoint.m_OriginalEntity) && !this.m_NodeData.HasComponent(controlPoint.m_OriginalEntity))
            controlPoint.m_OriginalEntity = Entity.Null;
        }
        else
        {
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          if (relocate && entity1 == Entity.Null && this.m_OwnerData.TryGetComponent(entity2, out componentData))
            entity1 = componentData.m_Owner;
        }
        NativeList<ClearAreaData> clearAreas = new NativeList<ClearAreaData>();
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(entity1))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform1 = this.m_TransformData[entity1];
          Game.Objects.Elevation componentData4;
          // ISSUE: reference to a compiler-generated field
          this.m_ElevationData.TryGetComponent(entity1, out componentData4);
          Entity owner = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            owner = this.m_OwnerData[entity1].m_Owner;
          }
          // ISSUE: reference to a compiler-generated field
          ownerDefinition.m_Prefab = this.m_PrefabRefData[entity1].m_Prefab;
          ownerDefinition.m_Position = transform1.m_Position;
          ownerDefinition.m_Rotation = transform1.m_Rotation;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_Stamping || this.CheckParentPrefab(ownerDefinition.m_Prefab, this.m_ObjectPrefab))
          {
            updatedTopLevel = entity1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabServiceUpgradeBuilding.HasBuffer(this.m_ObjectPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ClearAreaHelpers.FillClearAreas(this.m_ObjectPrefab, new Game.Objects.Transform(controlPoint.m_Position, controlPoint.m_Rotation), this.m_PrefabObjectGeometryData, this.m_PrefabAreaGeometryData, this.m_PrefabSubAreas, this.m_PrefabSubAreaNodes, ref clearAreas);
              ClearAreaHelpers.InitClearAreas(clearAreas, transform1);
              if (entity2 == Entity.Null)
                lotEntity = entity1;
            }
            // ISSUE: reference to a compiler-generated field
            bool rebuild = this.m_ObjectPrefab == Entity.Null;
            Entity prefab = Entity.Null;
            DynamicBuffer<InstalledUpgrade> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (rebuild && this.m_InstalledUpgrades.TryGetBuffer(entity1, out bufferData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ClearAreaHelpers.FillClearAreas(bufferData, Entity.Null, this.m_TransformData, this.m_AreaClearData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_SubAreas, this.m_AreaNodes, this.m_AreaTriangles, ref clearAreas);
              ClearAreaHelpers.InitClearAreas(clearAreas, transform1);
            }
            Attached componentData5;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (rebuild && this.m_AttachedData.TryGetComponent(entity1, out componentData5) && this.m_BuildingData.HasComponent(componentData5.m_Parent))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Objects.Transform transform2 = this.m_TransformData[componentData5.m_Parent];
              // ISSUE: reference to a compiler-generated field
              prefab = this.m_PrefabRefData[componentData5.m_Parent].m_Prefab;
              // ISSUE: reference to a compiler-generated method
              this.UpdateObject(Entity.Null, Entity.Null, Entity.Null, componentData5.m_Parent, Entity.Null, componentData5.m_Parent, Entity.Null, transform2, 0.0f, new OwnerDefinition(), clearAreas, false, false, false, true, false, -1, -1);
            }
            // ISSUE: reference to a compiler-generated method
            this.UpdateObject(Entity.Null, Entity.Null, owner, entity1, prefab, updatedTopLevel, Entity.Null, transform1, componentData4.m_Elevation, new OwnerDefinition(), clearAreas, true, false, rebuild, true, false, -1, -1);
            if (clearAreas.IsCreated)
              clearAreas.Clear();
          }
          else
            ownerDefinition = new OwnerDefinition();
        }
        DynamicBuffer<InstalledUpgrade> bufferData3;
        // ISSUE: reference to a compiler-generated field
        if (entity2 != Entity.Null && this.m_InstalledUpgrades.TryGetBuffer(entity2, out bufferData3))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ClearAreaHelpers.FillClearAreas(bufferData3, Entity.Null, this.m_TransformData, this.m_AreaClearData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_SubAreas, this.m_AreaNodes, this.m_AreaTriangles, ref clearAreas);
          // ISSUE: reference to a compiler-generated field
          ClearAreaHelpers.TransformClearAreas(clearAreas, this.m_TransformData[entity2], new Game.Objects.Transform(controlPoint.m_Position, controlPoint.m_Rotation));
          ClearAreaHelpers.InitClearAreas(clearAreas, new Game.Objects.Transform(controlPoint.m_Position, controlPoint.m_Rotation));
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ObjectPrefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_BrushPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateBrushes(controlPoint, this.m_ControlPoints[1], updatedTopLevel, ownerDefinition, clearAreas, topLevel, parentMesh1);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_Distance > 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateCurve(controlPoint, this.m_ControlPoints[math.min(1, this.m_ControlPoints.Length - 1)], this.m_ControlPoints[this.m_ControlPoints.Length - 1], updatedTopLevel, ownerDefinition, clearAreas, topLevel, parentMesh1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity5 = this.m_ObjectPrefab;
              DynamicBuffer<PlaceholderObjectElement> bufferData4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (entity2 == Entity.Null && (!this.m_EditorMode || ownerDefinition.m_Prefab == Entity.Null) && this.m_PrefabPlaceholderElements.TryGetBuffer(this.m_ObjectPrefab, out bufferData4) && !this.m_PrefabCreatureSpawnData.HasComponent(this.m_ObjectPrefab))
              {
                // ISSUE: reference to a compiler-generated field
                Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(1000000);
                int max = 0;
                for (int index = 0; index < bufferData4.Length; ++index)
                {
                  // ISSUE: variable of a compiler-generated type
                  ObjectToolBaseSystem.CreateDefinitionsJob.VariationData variation;
                  // ISSUE: reference to a compiler-generated method
                  if (this.GetVariationData(bufferData4[index], out variation))
                  {
                    // ISSUE: reference to a compiler-generated field
                    max += variation.m_Probability;
                    // ISSUE: reference to a compiler-generated field
                    if (random.NextInt(max) < variation.m_Probability)
                    {
                      // ISSUE: reference to a compiler-generated field
                      entity5 = variation.m_Prefab;
                    }
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateObject(entity5, this.m_TransformPrefab, Entity.Null, entity2, controlPoint.m_OriginalEntity, updatedTopLevel, lotEntity, new Game.Objects.Transform(controlPoint.m_Position, controlPoint.m_Rotation), controlPoint.m_Elevation, ownerDefinition, clearAreas, upgrade, relocate, false, topLevel, false, parentMesh1, 0);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_AttachmentPrefab.IsCreated && this.m_AttachmentPrefab.Value.m_Entity != Entity.Null)
              {
                Game.Objects.Transform transform = new Game.Objects.Transform(controlPoint.m_Position, controlPoint.m_Rotation);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                transform.m_Position += math.rotate(transform.m_Rotation, this.m_AttachmentPrefab.Value.m_Offset);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateObject(this.m_AttachmentPrefab.Value.m_Entity, Entity.Null, Entity.Null, Entity.Null, entity5, updatedTopLevel, Entity.Null, transform, controlPoint.m_Elevation, ownerDefinition, clearAreas, false, false, false, topLevel, false, parentMesh1, 0);
              }
            }
          }
        }
        if (!clearAreas.IsCreated)
          return;
        clearAreas.Dispose();
      }

      private bool GetVariationData(
        PlaceholderObjectElement placeholder,
        out ObjectToolBaseSystem.CreateDefinitionsJob.VariationData variation)
      {
        // ISSUE: object of a compiler-generated type is created
        variation = new ObjectToolBaseSystem.CreateDefinitionsJob.VariationData()
        {
          m_Prefab = placeholder.m_Object,
          m_Probability = 100
        };
        DynamicBuffer<ObjectRequirementElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRequirementElements.TryGetBuffer(variation.m_Prefab, out bufferData))
        {
          int num = -1;
          bool flag = true;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            ObjectRequirementElement requirementElement = bufferData[index];
            if ((int) requirementElement.m_Group != num)
            {
              if (flag)
              {
                num = (int) requirementElement.m_Group;
                flag = false;
              }
              else
                break;
            }
            // ISSUE: reference to a compiler-generated field
            flag |= this.m_Theme == requirementElement.m_Requirement;
          }
          if (!flag)
            return false;
        }
        SpawnableObjectData componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabSpawnableObjectData.TryGetComponent(variation.m_Prefab, out componentData))
        {
          // ISSUE: reference to a compiler-generated field
          variation.m_Probability = componentData.m_Probability;
        }
        return true;
      }

      private void CreateCurve(
        ControlPoint startPoint,
        ControlPoint middlePoint,
        ControlPoint endPoint,
        Entity updatedTopLevel,
        OwnerDefinition ownerDefinition,
        NativeList<ClearAreaData> clearAreas,
        bool topLevel,
        int parentMesh)
      {
        Bezier4x3 curve = new Bezier4x3();
        bool flag = false;
        float num1 = math.distance(startPoint.m_Position.xz, endPoint.m_Position.xz);
        if (!startPoint.m_Position.xz.Equals(middlePoint.m_Position.xz) && !endPoint.m_Position.xz.Equals(middlePoint.m_Position.xz))
        {
          float3 float3_1 = middlePoint.m_Position - startPoint.m_Position;
          float3 float3_2 = endPoint.m_Position - middlePoint.m_Position;
          float3 startTangent = MathUtils.Normalize(float3_1, float3_1.xz);
          float3 endTangent = MathUtils.Normalize(float3_2, float3_2.xz);
          curve = NetUtils.FitCurve(startPoint.m_Position, startTangent, endTangent, endPoint.m_Position);
          flag = true;
          num1 = MathUtils.Length(curve.xz);
        }
        PlaceableObjectData componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabPlaceableObjectData.TryGetComponent(this.m_ObjectPrefab, out componentData);
        NativeList<ObjectToolBaseSystem.CreateDefinitionsJob.VariationData> nativeList = new NativeList<ObjectToolBaseSystem.CreateDefinitionsJob.VariationData>();
        DynamicBuffer<PlaceholderObjectElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((!this.m_EditorMode || ownerDefinition.m_Prefab == Entity.Null) && this.m_PrefabPlaceholderElements.TryGetBuffer(this.m_ObjectPrefab, out bufferData) && !this.m_PrefabCreatureSpawnData.HasComponent(this.m_ObjectPrefab))
        {
          nativeList = new NativeList<ObjectToolBaseSystem.CreateDefinitionsJob.VariationData>(bufferData.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < bufferData.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            ObjectToolBaseSystem.CreateDefinitionsJob.VariationData variation;
            // ISSUE: reference to a compiler-generated method
            if (this.GetVariationData(bufferData[index], out variation))
              nativeList.Add(in variation);
          }
        }
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(1000000);
        // ISSUE: reference to a compiler-generated field
        int num2 = (int) ((double) num1 / (double) this.m_Distance + 1.5);
        float num3 = 1f / (float) math.max(1, num2 - 1);
        for (int randomIndex = 0; randomIndex < num2; ++randomIndex)
        {
          float num4 = (float) randomIndex * num3;
          float3 position = startPoint.m_Position;
          quaternion quaternion = startPoint.m_Rotation;
          if (randomIndex != 0)
          {
            float max = 6.28318548f;
            float angle = componentData.m_RotationSymmetry != RotationSymmetry.Any ? (componentData.m_RotationSymmetry == RotationSymmetry.None ? 0.0f : max * ((float) random.NextInt((int) componentData.m_RotationSymmetry) / (float) componentData.m_RotationSymmetry)) : random.NextFloat(max);
            if (flag)
            {
              Bounds1 t = new Bounds1(0.0f, 1f);
              if (randomIndex < num2 - 1 && MathUtils.ClampLength(curve.xz, ref t, num4 * num1))
                num4 = t.max;
              position = MathUtils.Position(curve, num4);
              float2 xz1 = MathUtils.StartTangent(curve).xz;
              float2 xz2 = MathUtils.Tangent(curve, num4).xz;
              if (MathUtils.TryNormalize(ref xz1) && MathUtils.TryNormalize(ref xz2))
                angle += MathUtils.RotationAngleRight(xz1, xz2);
            }
            else
              position = math.lerp(startPoint.m_Position, endPoint.m_Position, num4);
            if ((double) angle != 0.0)
              quaternion = math.normalizesafe(math.mul(quaternion, quaternion.RotateY(angle)), quaternion.identity);
          }
          float elevation;
          // ISSUE: reference to a compiler-generated method
          Game.Objects.Transform transform = this.SampleTransform(componentData, position, quaternion, out elevation);
          if (parentMesh != -1)
          {
            transform.m_Position.y = position.y;
            transform.m_Rotation = quaternion;
            elevation = startPoint.m_Elevation;
          }
          Entity objectPrefab = Entity.Null;
          if (nativeList.IsCreated)
          {
            int max = 0;
            for (int index = 0; index < nativeList.Length; ++index)
            {
              // ISSUE: variable of a compiler-generated type
              ObjectToolBaseSystem.CreateDefinitionsJob.VariationData variationData = nativeList[index];
              // ISSUE: reference to a compiler-generated field
              max += variationData.m_Probability;
              // ISSUE: reference to a compiler-generated field
              if (random.NextInt(max) < variationData.m_Probability)
              {
                // ISSUE: reference to a compiler-generated field
                objectPrefab = variationData.m_Prefab;
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            objectPrefab = this.m_ObjectPrefab;
          }
          if (objectPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateObject(objectPrefab, this.m_TransformPrefab, Entity.Null, Entity.Null, Entity.Null, updatedTopLevel, Entity.Null, transform, elevation, ownerDefinition, clearAreas, false, false, false, topLevel, false, parentMesh, randomIndex);
          }
        }
        if (!nativeList.IsCreated)
          return;
        nativeList.Dispose();
      }

      private void CreateBrushes(
        ControlPoint startPoint,
        ControlPoint endPoint,
        Entity updatedTopLevel,
        OwnerDefinition ownerDefinition,
        NativeList<ClearAreaData> clearAreas,
        bool topLevel,
        int parentMesh)
      {
        if (endPoint.Equals(new ControlPoint()))
          return;
        CreationDefinition component1 = new CreationDefinition();
        // ISSUE: reference to a compiler-generated field
        component1.m_Prefab = this.m_BrushPrefab;
        BrushDefinition component2 = new BrushDefinition();
        // ISSUE: reference to a compiler-generated field
        component2.m_Tool = this.m_ObjectPrefab;
        ref ControlPoint local = ref startPoint;
        ControlPoint other = new ControlPoint();
        component2.m_Line = !local.Equals(other) ? new Line3.Segment(startPoint.m_Position, endPoint.m_Position) : new Line3.Segment(endPoint.m_Position, endPoint.m_Position);
        // ISSUE: reference to a compiler-generated field
        component2.m_Size = this.m_BrushSize;
        // ISSUE: reference to a compiler-generated field
        component2.m_Angle = this.m_BrushAngle;
        // ISSUE: reference to a compiler-generated field
        component2.m_Strength = this.m_BrushStrength;
        // ISSUE: reference to a compiler-generated field
        component2.m_Time = this.m_DeltaTime;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BrushDefinition>(entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BrushData brushData = this.m_PrefabBrushData[this.m_BrushPrefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BrushCell> prefabBrushCell = this.m_PrefabBrushCells[this.m_BrushPrefab];
        if (math.any(brushData.m_Resolution == 0) || prefabBrushCell.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        int num1 = 1 + Mathf.FloorToInt(MathUtils.Length(component2.m_Line) / (this.m_BrushSize * 0.25f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num2 = this.m_BrushStrength * this.m_BrushStrength * math.saturate(this.m_DeltaTime * 10f);
        // ISSUE: reference to a compiler-generated field
        quaternion q = quaternion.RotateY(this.m_BrushAngle);
        float2 xz1 = math.mul(q, new float3(1f, 0.0f, 0.0f)).xz;
        float2 xz2 = math.mul(q, new float3(0.0f, 0.0f, 1f)).xz;
        float num3 = 16f;
        // ISSUE: reference to a compiler-generated field
        float2 float2_1 = (math.abs(xz1) + math.abs(xz2)) * (this.m_BrushSize * 0.5f);
        Bounds2 bounds2_1 = new Bounds2((float2) float.MaxValue, (float2) float.MinValue);
        for (int index = 1; index <= num1; ++index)
        {
          float3 float3 = MathUtils.Position(component2.m_Line, (float) index / (float) num1);
          bounds2_1 |= new Bounds2(float3.xz - float2_1, float3.xz + float2_1);
        }
        // ISSUE: reference to a compiler-generated field
        float2 float2_2 = this.m_BrushSize / (float2) brushData.m_Resolution;
        float4 xyxy1 = (1f / float2_2).xyxy;
        float4 xyxy2 = ((float2) brushData.m_Resolution * 0.5f).xyxy;
        float num4 = (float) (1.0 / ((double) num1 * (double) num3 * (double) num3));
        // ISSUE: reference to a compiler-generated field
        if (this.m_Removing)
        {
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
          ObjectToolBaseSystem.CreateDefinitionsJob.BrushIterator iterator = new ObjectToolBaseSystem.CreateDefinitionsJob.BrushIterator()
          {
            m_Bounds = bounds2_1,
            m_RandomSeed = this.m_RandomSeed,
            m_BrushLine = component2.m_Line,
            m_BrushCellSizeFactor = xyxy1,
            m_BrushTextureSizeAdd = xyxy2,
            m_BrushDirX = xz1,
            m_BrushDirZ = xz2,
            m_BrushCellSize = float2_2,
            m_BrushResolution = brushData.m_Resolution,
            m_TileSize = num3,
            m_BrushStrength = num2,
            m_StrengthFactor = num4,
            m_BrushCount = num1,
            m_BrushCells = prefabBrushCell,
            m_OwnerData = this.m_OwnerData,
            m_TransformData = this.m_TransformData,
            m_ElevationData = this.m_ElevationData,
            m_EditorContainerData = this.m_EditorContainerData,
            m_LocalTransformCacheData = this.m_LocalTransformCacheData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
            m_CommandBuffer = this.m_CommandBuffer
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.PrefabType) != Snap.None && this.m_ObjectPrefab != Entity.Null)
          {
            DynamicBuffer<PlaceholderObjectElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabPlaceholderElements.TryGetBuffer(this.m_ObjectPrefab, out bufferData))
            {
              // ISSUE: reference to a compiler-generated field
              iterator.m_RequirePrefab = new NativeParallelHashSet<Entity>(1 + bufferData.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator.m_RequirePrefab.Add(this.m_ObjectPrefab);
              for (int index = 0; index < bufferData.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                iterator.m_RequirePrefab.Add(bufferData[index].m_Object);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              iterator.m_RequirePrefab = new NativeParallelHashSet<Entity>(1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              iterator.m_RequirePrefab.Add(this.m_ObjectPrefab);
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ObjectSearchTree.Iterate<ObjectToolBaseSystem.CreateDefinitionsJob.BrushIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          if (!iterator.m_RequirePrefab.IsCreated)
            return;
          // ISSUE: reference to a compiler-generated field
          iterator.m_RequirePrefab.Dispose();
        }
        else
        {
          PlaceableObjectData componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_PrefabPlaceableObjectData.TryGetComponent(this.m_ObjectPrefab, out componentData);
          NativeList<ObjectToolBaseSystem.CreateDefinitionsJob.VariationData> nativeList = new NativeList<ObjectToolBaseSystem.CreateDefinitionsJob.VariationData>();
          DynamicBuffer<PlaceholderObjectElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabPlaceholderElements.TryGetBuffer(this.m_ObjectPrefab, out bufferData) && !this.m_PrefabCreatureSpawnData.HasComponent(this.m_ObjectPrefab))
          {
            nativeList = new NativeList<ObjectToolBaseSystem.CreateDefinitionsJob.VariationData>(bufferData.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            for (int index = 0; index < bufferData.Length; ++index)
            {
              // ISSUE: variable of a compiler-generated type
              ObjectToolBaseSystem.CreateDefinitionsJob.VariationData variation;
              // ISSUE: reference to a compiler-generated method
              if (this.GetVariationData(bufferData[index], out variation))
                nativeList.Add(in variation);
            }
          }
          int4 int4_1 = (int4) math.floor(new float4(bounds2_1.min, bounds2_1.max) / num3);
          int2 int2 = (int2) 0;
          for (int index1 = 0; index1 < 3; ++index1)
          {
            float num5 = 0.0f;
            bool flag = false;
            for (int y1 = int4_1.y; y1 <= int4_1.w; ++y1)
            {
              Bounds2 bounds2_2;
              bounds2_2.min.y = (float) y1 * num3;
              bounds2_2.max.y = bounds2_2.min.y + num3;
              for (int x1 = int4_1.x; x1 <= int4_1.z; ++x1)
              {
                bounds2_2.min.x = (float) x1 * num3;
                bounds2_2.max.x = bounds2_2.min.x + num3;
                // ISSUE: reference to a compiler-generated field
                Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom((y1 & (int) ushort.MaxValue) << 16 | x1 & (int) ushort.MaxValue);
                float num6 = random.NextFloat(4f);
                switch (index1)
                {
                  case 0:
                    if ((double) num6 < (double) num2)
                      goto default;
                    else
                      break;
                  case 2:
                    if (x1 != int2.x || y1 != int2.y)
                      break;
                    goto default;
                  default:
                    float x2 = 0.0f;
                    if (index1 != 2)
                    {
                      for (int index2 = 1; index2 <= num1; ++index2)
                      {
                        float3 float3 = MathUtils.Position(component2.m_Line, (float) index2 / (float) num1);
                        Bounds2 bounds = bounds2_2;
                        bounds.min -= float3.xz;
                        bounds.max -= float3.xz;
                        float4 float4 = new float4(bounds.min, bounds.max);
                        float4 x3 = new float4(math.dot(float4.xy, xz1), math.dot(float4.xw, xz1), math.dot(float4.zy, xz1), math.dot(float4.zw, xz1));
                        float4 x4 = new float4(math.dot(float4.xy, xz2), math.dot(float4.xw, xz2), math.dot(float4.zy, xz2), math.dot(float4.zw, xz2));
                        int4 int4_2 = math.clamp((int4) math.floor(new float4(math.cmin(x3), math.cmin(x4), math.cmax(x3), math.cmax(x4)) * xyxy1 + xyxy2), (int4) 0, brushData.m_Resolution.xyxy - 1);
                        for (int y2 = int4_2.y; y2 <= int4_2.w; ++y2)
                        {
                          float2 float2_3 = xz2 * (((float) y2 - xyxy2.y) * float2_2.y);
                          float2 float2_4 = xz2 * (((float) (y2 + 1) - xyxy2.y) * float2_2.y);
                          for (int x5 = int4_2.x; x5 <= int4_2.z; ++x5)
                          {
                            int index3 = x5 + brushData.m_Resolution.x * y2;
                            BrushCell brushCell = prefabBrushCell[index3];
                            if ((double) brushCell.m_Opacity >= 9.9999997473787516E-05)
                            {
                              float2 float2_5 = xz1 * (((float) x5 - xyxy2.x) * float2_2.x);
                              float2 float2_6 = xz1 * (((float) (x5 + 1) - xyxy2.x) * float2_2.x);
                              Quad2 quad = new Quad2(float2_3 + float2_5, float2_3 + float2_6, float2_4 + float2_6, float2_4 + float2_5);
                              float area;
                              if (MathUtils.Intersect(bounds, quad, out area))
                                x2 += brushCell.m_Opacity * area;
                            }
                          }
                        }
                      }
                      x2 *= num4;
                      if ((double) math.abs(x2) < 9.9999997473787516E-05)
                        break;
                    }
                    float4 float4_1 = random.NextFloat4(new float4(1f, 1f, 1f, 6.28318548f));
                    if (index1 == 0)
                    {
                      if ((double) float4_1.x >= (double) x2)
                        break;
                    }
                    else if (index1 == 1)
                    {
                      num5 += x2;
                      if ((double) float4_1.x * (double) num5 < (double) x2)
                      {
                        int2 = new int2(x1, y1);
                        break;
                      }
                      break;
                    }
                    float elevation;
                    // ISSUE: reference to a compiler-generated method
                    Game.Objects.Transform transform = this.SampleTransform(componentData, new float3()
                    {
                      xz = math.lerp(bounds2_2.min, bounds2_2.max, float4_1.yz)
                    }, quaternion.RotateY(float4_1.w), out elevation);
                    Entity objectPrefab = Entity.Null;
                    if (nativeList.IsCreated)
                    {
                      int max = 0;
                      for (int index4 = 0; index4 < nativeList.Length; ++index4)
                      {
                        // ISSUE: variable of a compiler-generated type
                        ObjectToolBaseSystem.CreateDefinitionsJob.VariationData variationData = nativeList[index4];
                        // ISSUE: reference to a compiler-generated field
                        max += variationData.m_Probability;
                        // ISSUE: reference to a compiler-generated field
                        if (random.NextInt(max) < variationData.m_Probability)
                        {
                          // ISSUE: reference to a compiler-generated field
                          objectPrefab = variationData.m_Prefab;
                        }
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      objectPrefab = this.m_ObjectPrefab;
                    }
                    if (objectPrefab != Entity.Null)
                    {
                      int randomIndex = (x1 & (int) ushort.MaxValue) << 16 | y1 & (int) ushort.MaxValue;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      this.UpdateObject(objectPrefab, this.m_TransformPrefab, Entity.Null, Entity.Null, Entity.Null, updatedTopLevel, Entity.Null, transform, elevation, ownerDefinition, clearAreas, false, false, false, topLevel, true, parentMesh, randomIndex);
                    }
                    flag = true;
                    break;
                }
              }
            }
            if (flag)
              break;
          }
          if (!nativeList.IsCreated)
            return;
          nativeList.Dispose();
        }
      }

      private Game.Objects.Transform SampleTransform(
        PlaceableObjectData placeableObjectData,
        float3 position,
        quaternion rotation,
        out float elevation)
      {
        float3 normal;
        // ISSUE: reference to a compiler-generated field
        float x = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, position, out normal);
        elevation = 0.0f;
        if ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.Hovering) != Game.Objects.PlacementFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float y = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, position) + placeableObjectData.m_PlacementOffset.y;
          elevation = math.max(0.0f, y - x);
          x = math.max(x, y);
        }
        else if ((placeableObjectData.m_Flags & (Game.Objects.PlacementFlags.Shoreline | Game.Objects.PlacementFlags.Floating)) == Game.Objects.PlacementFlags.None)
        {
          x += placeableObjectData.m_PlacementOffset.y;
        }
        else
        {
          float waterDepth;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, position, out waterDepth);
          if ((double) waterDepth >= 0.20000000298023224)
          {
            float y = num + placeableObjectData.m_PlacementOffset.y;
            if ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.Floating) != Game.Objects.PlacementFlags.None)
              elevation = math.max(0.0f, y - x);
            x = math.max(x, y);
          }
        }
        Game.Objects.Transform transform;
        transform.m_Position = position;
        transform.m_Position.y = x;
        transform.m_Rotation = rotation;
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & Snap.Upright) == Snap.None)
        {
          float3 forward = math.cross(math.right(), normal);
          transform.m_Rotation = math.mul(quaternion.LookRotation(forward, normal), transform.m_Rotation);
        }
        return transform;
      }

      private bool CheckParentPrefab(Entity parentPrefab, Entity objectPrefab)
      {
        if (parentPrefab == objectPrefab)
          return false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabSubObjects.HasBuffer(objectPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Prefabs.SubObject> prefabSubObject = this.m_PrefabSubObjects[objectPrefab];
          for (int index = 0; index < prefabSubObject.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.CheckParentPrefab(parentPrefab, prefabSubObject[index].m_Prefab))
              return false;
          }
        }
        return true;
      }

      private void UpdateObject(
        Entity objectPrefab,
        Entity transformPrefab,
        Entity owner,
        Entity original,
        Entity parent,
        Entity updatedTopLevel,
        Entity lotEntity,
        Game.Objects.Transform transform,
        float elevation,
        OwnerDefinition ownerDefinition,
        NativeList<ClearAreaData> clearAreas,
        bool upgrade,
        bool relocate,
        bool rebuild,
        bool topLevel,
        bool optional,
        int parentMesh,
        int randomIndex)
      {
        OwnerDefinition ownerDefinition1 = ownerDefinition;
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(randomIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabAssetStampData.HasComponent(objectPrefab) || !this.m_Stamping && ownerDefinition.m_Prefab == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity();
          CreationDefinition component1 = new CreationDefinition();
          component1.m_Prefab = objectPrefab;
          component1.m_SubPrefab = transformPrefab;
          component1.m_Owner = owner;
          component1.m_Original = original;
          component1.m_RandomSeed = random.NextInt();
          if (optional)
            component1.m_Flags |= CreationFlags.Optional;
          // ISSUE: reference to a compiler-generated field
          if (objectPrefab == Entity.Null && this.m_PrefabRefData.HasComponent(original))
          {
            // ISSUE: reference to a compiler-generated field
            objectPrefab = this.m_PrefabRefData[original].m_Prefab;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabBuildingData.HasComponent(objectPrefab))
            parentMesh = -1;
          ObjectDefinition component2 = new ObjectDefinition();
          component2.m_ParentMesh = parentMesh;
          component2.m_Position = transform.m_Position;
          component2.m_Rotation = transform.m_Rotation;
          component2.m_Probability = 100;
          component2.m_PrefabSubIndex = -1;
          component2.m_Scale = (float3) 1f;
          component2.m_Intensity = 1f;
          if (original == Entity.Null && transformPrefab != Entity.Null)
            component2.m_GroupIndex = -1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabPlaceableObjectData.HasComponent(objectPrefab))
          {
            // ISSUE: reference to a compiler-generated field
            PlaceableObjectData placeableObjectData = this.m_PrefabPlaceableObjectData[objectPrefab];
            if ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.HasProbability) != Game.Objects.PlacementFlags.None)
              component2.m_Probability = (int) placeableObjectData.m_DefaultProbability;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorContainerData.HasComponent(original))
          {
            // ISSUE: reference to a compiler-generated field
            EditorContainer editorContainer = this.m_EditorContainerData[original];
            component1.m_SubPrefab = editorContainer.m_Prefab;
            component2.m_Scale = editorContainer.m_Scale;
            component2.m_Intensity = editorContainer.m_Intensity;
            component2.m_GroupIndex = editorContainer.m_GroupIndex;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalTransformCacheData.HasComponent(original))
          {
            // ISSUE: reference to a compiler-generated field
            LocalTransformCache localTransformCache = this.m_LocalTransformCacheData[original];
            component2.m_Probability = localTransformCache.m_Probability;
            component2.m_PrefabSubIndex = localTransformCache.m_PrefabSubIndex;
          }
          component2.m_Elevation = parentMesh == -1 ? elevation : transform.m_Position.y - ownerDefinition.m_Position.y;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          component2.m_Age = !this.m_EditorMode ? ToolUtils.GetRandomAge(ref random, this.m_AgeMask) : random.NextFloat(1f);
          if (ownerDefinition.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity1, ownerDefinition);
            Game.Objects.Transform local = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(new Game.Objects.Transform(ownerDefinition.m_Position, ownerDefinition.m_Rotation)), transform);
            component2.m_LocalPosition = local.m_Position;
            component2.m_LocalRotation = local.m_Rotation;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(owner))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Objects.Transform local = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(this.m_TransformData[owner]), transform);
              component2.m_LocalPosition = local.m_Position;
              component2.m_LocalRotation = local.m_Rotation;
            }
            else
            {
              component2.m_LocalPosition = transform.m_Position;
              component2.m_LocalRotation = transform.m_Rotation;
            }
          }
          Entity entity2 = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.HasBuffer(parent))
          {
            component1.m_Flags |= CreationFlags.Attach;
            // ISSUE: reference to a compiler-generated field
            if (parentMesh == -1 && this.m_NetElevationData.HasComponent(parent))
            {
              component2.m_ParentMesh = 0;
              // ISSUE: reference to a compiler-generated field
              component2.m_Elevation = math.csum(this.m_NetElevationData[parent].m_Elevation) * 0.5f;
              // ISSUE: reference to a compiler-generated method
              if (this.IsLoweredParent(parent))
                component1.m_Flags |= CreationFlags.Lowered;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabNetObjectData.HasComponent(objectPrefab))
            {
              entity2 = parent;
              // ISSUE: reference to a compiler-generated method
              this.UpdateAttachedParent(parent, updatedTopLevel);
            }
            else
              component1.m_Attached = parent;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PlaceholderBuildingData.HasComponent(parent))
            {
              component1.m_Flags |= CreationFlags.Attach;
              component1.m_Attached = parent;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachedData.HasComponent(original))
          {
            // ISSUE: reference to a compiler-generated field
            Attached attached = this.m_AttachedData[original];
            if (attached.m_Parent != entity2)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateAttachedParent(attached.m_Parent, updatedTopLevel);
            }
          }
          if (upgrade)
            component1.m_Flags |= CreationFlags.Upgrade | CreationFlags.Parent;
          if (relocate)
            component1.m_Flags |= CreationFlags.Relocate;
          if (rebuild)
            component1.m_Flags |= CreationFlags.Repair;
          ownerDefinition1.m_Prefab = objectPrefab;
          ownerDefinition1.m_Position = component2.m_Position;
          ownerDefinition1.m_Rotation = component2.m_Rotation;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity1, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<ObjectDefinition>(entity1, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSubObjects.HasBuffer(objectPrefab))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Prefabs.SubObject> prefabSubObject = this.m_PrefabSubObjects[objectPrefab];
            for (int index = 0; index < prefabSubObject.Length; ++index)
            {
              Game.Prefabs.SubObject subObject = prefabSubObject[index];
              Game.Objects.Transform world = ObjectUtils.LocalToWorld(transform, subObject.m_Position, subObject.m_Rotation);
              // ISSUE: reference to a compiler-generated method
              this.UpdateObject(subObject.m_Prefab, Entity.Null, owner, Entity.Null, parent, updatedTopLevel, lotEntity, world, elevation, ownerDefinition, new NativeList<ClearAreaData>(), false, false, false, false, false, parentMesh, index);
            }
          }
          original = Entity.Null;
          topLevel = true;
        }
        NativeParallelHashMap<Entity, int> selectedSpawnables = new NativeParallelHashMap<Entity, int>();
        Game.Objects.Transform mainInverseTransform = transform;
        if (original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          mainInverseTransform = ObjectUtils.InverseTransform(this.m_TransformData[original]);
        }
        // ISSUE: reference to a compiler-generated method
        this.UpdateSubObjects(transform, transform, mainInverseTransform, objectPrefab, original, relocate, rebuild, topLevel, upgrade, ownerDefinition1, ref random, ref selectedSpawnables);
        // ISSUE: reference to a compiler-generated method
        this.UpdateSubNets(transform, transform, mainInverseTransform, objectPrefab, original, lotEntity, relocate, topLevel, ownerDefinition1, clearAreas, ref random);
        // ISSUE: reference to a compiler-generated method
        this.UpdateSubAreas(transform, objectPrefab, original, relocate, rebuild, topLevel, ownerDefinition1, clearAreas, ref random, ref selectedSpawnables);
        if (!selectedSpawnables.IsCreated)
          return;
        selectedSpawnables.Dispose();
      }

      private void UpdateAttachedParent(Entity parent, Entity updatedTopLevel)
      {
        if (updatedTopLevel != Entity.Null)
        {
          Entity entity = parent;
          if (entity == updatedTopLevel)
            return;
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            entity = this.m_OwnerData[entity].m_Owner;
            if (entity == updatedTopLevel)
              return;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeData.HasComponent(parent))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge = this.m_EdgeData[parent];
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity();
          CreationDefinition component1 = new CreationDefinition();
          component1.m_Original = parent;
          component1.m_Flags |= CreationFlags.Align;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          // ISSUE: reference to a compiler-generated field
          NetCourse component2 = new NetCourse()
          {
            m_Curve = this.m_CurveData[parent].m_Bezier
          };
          component2.m_Length = MathUtils.Length(component2.m_Curve);
          component2.m_FixedIndex = -1;
          component2.m_StartPosition.m_Entity = edge.m_Start;
          component2.m_StartPosition.m_Position = component2.m_Curve.a;
          component2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component2.m_Curve));
          component2.m_StartPosition.m_CourseDelta = 0.0f;
          component2.m_EndPosition.m_Entity = edge.m_End;
          component2.m_EndPosition.m_Position = component2.m_Curve.d;
          component2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component2.m_Curve));
          component2.m_EndPosition.m_CourseDelta = 1f;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(entity, component2);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_NodeData.HasComponent(parent))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_NodeData[parent];
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity();
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, new CreationDefinition()
          {
            m_Original = parent
          });
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(entity, new NetCourse()
          {
            m_Curve = new Bezier4x3(node.m_Position, node.m_Position, node.m_Position, node.m_Position),
            m_Length = 0.0f,
            m_FixedIndex = -1,
            m_StartPosition = {
              m_Entity = parent,
              m_Position = node.m_Position,
              m_Rotation = node.m_Rotation,
              m_CourseDelta = 0.0f
            },
            m_EndPosition = {
              m_Entity = parent,
              m_Position = node.m_Position,
              m_Rotation = node.m_Rotation,
              m_CourseDelta = 1f
            }
          });
        }
      }

      private bool IsLoweredParent(Entity entity)
      {
        Composition componentData1;
        NetCompositionData componentData2;
        Orphan componentData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CompositionData.TryGetComponent(entity, out componentData1) && this.m_PrefabCompositionData.TryGetComponent(componentData1.m_Edge, out componentData2) && ((componentData2.m_Flags.m_Left | componentData2.m_Flags.m_Right) & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0 || this.m_OrphanData.TryGetComponent(entity, out componentData3) && this.m_PrefabCompositionData.TryGetComponent(componentData3.m_Composition, out componentData2) && ((componentData2.m_Flags.m_Left | componentData2.m_Flags.m_Right) & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
          return true;
        DynamicBuffer<ConnectedEdge> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedEdges.TryGetBuffer(entity, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            ConnectedEdge connectedEdge = bufferData[index];
            // ISSUE: reference to a compiler-generated field
            Game.Net.Edge edge = this.m_EdgeData[connectedEdge.m_Edge];
            if (edge.m_Start == entity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CompositionData.TryGetComponent(connectedEdge.m_Edge, out componentData1) && this.m_PrefabCompositionData.TryGetComponent(componentData1.m_StartNode, out componentData2) && ((componentData2.m_Flags.m_Left | componentData2.m_Flags.m_Right) & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
                return true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (edge.m_End == entity && this.m_CompositionData.TryGetComponent(connectedEdge.m_Edge, out componentData1) && this.m_PrefabCompositionData.TryGetComponent(componentData1.m_EndNode, out componentData2) && ((componentData2.m_Flags.m_Left | componentData2.m_Flags.m_Right) & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
                return true;
            }
          }
        }
        return false;
      }

      private void UpdateSubObjects(
        Game.Objects.Transform transform,
        Game.Objects.Transform mainTransform,
        Game.Objects.Transform mainInverseTransform,
        Entity prefab,
        Entity original,
        bool relocate,
        bool rebuild,
        bool topLevel,
        bool isParent,
        OwnerDefinition ownerDefinition,
        ref Unity.Mathematics.Random random,
        ref NativeParallelHashMap<Entity, int> selectedSpawnables)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_InstalledUpgrades.HasBuffer(original) || !this.m_TransformData.HasComponent(original))
          return;
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform inverseParentTransform = ObjectUtils.InverseTransform(this.m_TransformData[original]);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<InstalledUpgrade> installedUpgrade = this.m_InstalledUpgrades[original];
        for (int index = 0; index < installedUpgrade.Length; ++index)
        {
          Entity upgrade = installedUpgrade[index].m_Upgrade;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(upgrade == this.m_Original) && this.m_TransformData.HasComponent(upgrade))
          {
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity();
            CreationDefinition component1 = new CreationDefinition();
            component1.m_Original = upgrade;
            if (relocate)
              component1.m_Flags |= CreationFlags.Relocate;
            if (rebuild)
              component1.m_Flags |= CreationFlags.Repair;
            if (isParent)
            {
              component1.m_Flags |= CreationFlags.Parent;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ObjectPrefab == Entity.Null)
                component1.m_Flags |= CreationFlags.Upgrade;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
            if (ownerDefinition.m_Prefab != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
            }
            ObjectDefinition component2 = new ObjectDefinition();
            component2.m_Probability = 100;
            component2.m_PrefabSubIndex = -1;
            Game.Objects.Transform local;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.HasComponent(upgrade))
            {
              // ISSUE: reference to a compiler-generated field
              LocalTransformCache localTransformCache = this.m_LocalTransformCacheData[upgrade];
              component2.m_ParentMesh = localTransformCache.m_ParentMesh;
              component2.m_GroupIndex = localTransformCache.m_GroupIndex;
              component2.m_Probability = localTransformCache.m_Probability;
              component2.m_PrefabSubIndex = localTransformCache.m_PrefabSubIndex;
              local.m_Position = localTransformCache.m_Position;
              local.m_Rotation = localTransformCache.m_Rotation;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              component2.m_ParentMesh = this.m_BuildingData.HasComponent(upgrade) ? -1 : 0;
              // ISSUE: reference to a compiler-generated field
              local = ObjectUtils.WorldToLocal(inverseParentTransform, this.m_TransformData[upgrade]);
            }
            Game.Objects.Elevation componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElevationData.TryGetComponent(upgrade, out componentData1))
              component2.m_Elevation = componentData1.m_Elevation;
            Game.Objects.Transform world = ObjectUtils.LocalToWorld(transform, local);
            world.m_Rotation = math.normalize(world.m_Rotation);
            PrefabRef componentData2;
            PlaceableObjectData componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (relocate && this.m_BuildingData.HasComponent(upgrade) && this.m_PrefabRefData.TryGetComponent(upgrade, out componentData2) && this.m_PrefabPlaceableObjectData.TryGetComponent(componentData2.m_Prefab, out componentData3))
            {
              // ISSUE: reference to a compiler-generated field
              float x = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, world.m_Position);
              if ((componentData3.m_Flags & Game.Objects.PlacementFlags.Hovering) != Game.Objects.PlacementFlags.None)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float y = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, world.m_Position) + componentData3.m_PlacementOffset.y;
                component2.m_Elevation = math.max(0.0f, y - x);
                x = math.max(x, y);
              }
              else if ((componentData3.m_Flags & (Game.Objects.PlacementFlags.Shoreline | Game.Objects.PlacementFlags.Floating)) == Game.Objects.PlacementFlags.None)
              {
                x += componentData3.m_PlacementOffset.y;
              }
              else
              {
                float waterDepth;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                float num = WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, world.m_Position, out waterDepth);
                if ((double) waterDepth >= 0.20000000298023224)
                {
                  float y = num + componentData3.m_PlacementOffset.y;
                  if ((componentData3.m_Flags & Game.Objects.PlacementFlags.Floating) != Game.Objects.PlacementFlags.None)
                    component2.m_Elevation = math.max(0.0f, y - x);
                  x = math.max(x, y);
                }
              }
              world.m_Position.y = x;
            }
            component2.m_Position = world.m_Position;
            component2.m_Rotation = world.m_Rotation;
            component2.m_LocalPosition = local.m_Position;
            component2.m_LocalRotation = local.m_Rotation;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<ObjectDefinition>(entity, component2);
            // ISSUE: reference to a compiler-generated field
            OwnerDefinition ownerDefinition1 = new OwnerDefinition()
            {
              m_Prefab = this.m_PrefabRefData[upgrade].m_Prefab,
              m_Position = world.m_Position,
              m_Rotation = world.m_Rotation
            };
            // ISSUE: reference to a compiler-generated method
            this.UpdateSubNets(world, mainTransform, mainInverseTransform, ownerDefinition1.m_Prefab, upgrade, Entity.Null, relocate, true, ownerDefinition1, new NativeList<ClearAreaData>(), ref random);
            // ISSUE: reference to a compiler-generated method
            this.UpdateSubAreas(world, ownerDefinition1.m_Prefab, upgrade, relocate, rebuild, true, ownerDefinition1, new NativeList<ClearAreaData>(), ref random, ref selectedSpawnables);
          }
        }
      }

      private void CreateSubNet(
        Entity netPrefab,
        Entity lanePrefab,
        Bezier4x3 curve,
        int2 nodeIndex,
        int2 parentMesh,
        CompositionFlags upgrades,
        NativeList<float4> nodePositions,
        Game.Objects.Transform parentTransform,
        OwnerDefinition ownerDefinition,
        NativeList<ClearAreaData> clearAreas,
        BuildingUtils.LotInfo lotInfo,
        bool hasLot,
        ref Unity.Mathematics.Random random)
      {
        NetGeometryData componentData;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetGeometryData.TryGetComponent(netPrefab, out componentData);
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Prefab = netPrefab;
        component1.m_SubPrefab = lanePrefab;
        component1.m_RandomSeed = random.NextInt();
        bool linearMiddle = parentMesh.x >= 0 && parentMesh.y >= 0;
        NetCourse component2 = new NetCourse();
        if ((componentData.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
        {
          curve.y = new Bezier4x1();
          Curve curve1 = new Curve()
          {
            m_Bezier = ObjectUtils.LocalToWorld(parentTransform.m_Position, parentTransform.m_Rotation, curve)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          component2.m_Curve = NetUtils.AdjustPosition(curve1, false, false, false, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData).m_Bezier;
        }
        else if (!linearMiddle)
        {
          Curve curve2 = new Curve()
          {
            m_Bezier = ObjectUtils.LocalToWorld(parentTransform.m_Position, parentTransform.m_Rotation, curve)
          };
          bool fixedStart = parentMesh.x >= 0;
          bool fixedEnd = parentMesh.y >= 0;
          linearMiddle = fixedStart | fixedEnd;
          if ((componentData.m_Flags & Game.Net.GeometryFlags.FlattenTerrain) != (Game.Net.GeometryFlags) 0)
          {
            if (hasLot)
            {
              component2.m_Curve = NetUtils.AdjustPosition(curve2, (bool2) fixedStart, linearMiddle, (bool2) fixedEnd, ref lotInfo).m_Bezier;
              component2.m_Curve.a.y += curve.a.y;
              component2.m_Curve.b.y += curve.b.y;
              component2.m_Curve.c.y += curve.c.y;
              component2.m_Curve.d.y += curve.d.y;
            }
            else
              component2.m_Curve = curve2.m_Bezier;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            component2.m_Curve = NetUtils.AdjustPosition(curve2, fixedStart, linearMiddle, fixedEnd, ref this.m_TerrainHeightData).m_Bezier;
            component2.m_Curve.a.y += curve.a.y;
            component2.m_Curve.b.y += curve.b.y;
            component2.m_Curve.c.y += curve.c.y;
            component2.m_Curve.d.y += curve.d.y;
          }
        }
        else
          component2.m_Curve = ObjectUtils.LocalToWorld(parentTransform.m_Position, parentTransform.m_Rotation, curve);
        bool onGround = !linearMiddle || (double) math.cmin(math.abs(curve.y.abcd)) < 2.0;
        if (ClearAreaHelpers.ShouldClear(clearAreas, component2.m_Curve, onGround))
          return;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        if (ownerDefinition.m_Prefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
        }
        component2.m_StartPosition.m_Position = component2.m_Curve.a;
        component2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component2.m_Curve), parentTransform.m_Rotation);
        component2.m_StartPosition.m_CourseDelta = 0.0f;
        component2.m_StartPosition.m_Elevation = (float2) curve.a.y;
        component2.m_StartPosition.m_ParentMesh = parentMesh.x;
        float3 world;
        if (nodeIndex.x >= 0)
        {
          if ((componentData.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
          {
            ref float3 local = ref component2.m_StartPosition.m_Position;
            world = ObjectUtils.LocalToWorld(parentTransform, nodePositions[nodeIndex.x].xyz);
            float2 xz = world.xz;
            local.xz = xz;
          }
          else
            component2.m_StartPosition.m_Position = ObjectUtils.LocalToWorld(parentTransform, nodePositions[nodeIndex.x].xyz);
        }
        component2.m_EndPosition.m_Position = component2.m_Curve.d;
        component2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component2.m_Curve), parentTransform.m_Rotation);
        component2.m_EndPosition.m_CourseDelta = 1f;
        component2.m_EndPosition.m_Elevation = (float2) curve.d.y;
        component2.m_EndPosition.m_ParentMesh = parentMesh.y;
        if (nodeIndex.y >= 0)
        {
          if ((componentData.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
          {
            ref float3 local = ref component2.m_EndPosition.m_Position;
            world = ObjectUtils.LocalToWorld(parentTransform, nodePositions[nodeIndex.y].xyz);
            float2 xz = world.xz;
            local.xz = xz;
          }
          else
            component2.m_EndPosition.m_Position = ObjectUtils.LocalToWorld(parentTransform, nodePositions[nodeIndex.y].xyz);
        }
        component2.m_Length = MathUtils.Length(component2.m_Curve);
        component2.m_FixedIndex = -1;
        component2.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
        component2.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
        if (component2.m_StartPosition.m_Position.Equals(component2.m_EndPosition.m_Position))
        {
          component2.m_StartPosition.m_Flags |= CoursePosFlags.IsLast;
          component2.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst;
        }
        if (ownerDefinition.m_Prefab == Entity.Null)
        {
          component2.m_StartPosition.m_Flags |= CoursePosFlags.FreeHeight;
          component2.m_EndPosition.m_Flags |= CoursePosFlags.FreeHeight;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, component2);
        if (upgrades != new CompositionFlags())
        {
          Upgraded component3 = new Upgraded()
          {
            m_Flags = upgrades
          };
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Upgraded>(entity, component3);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EditorMode)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, new LocalCurveCache()
        {
          m_Curve = curve
        });
      }

      private bool GetOwnerLot(Entity lotOwner, out BuildingUtils.LotInfo lotInfo)
      {
        Game.Buildings.Lot componentData1;
        Game.Objects.Transform componentData2;
        PrefabRef componentData3;
        Game.Prefabs.BuildingData componentData4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_LotData.TryGetComponent(lotOwner, out componentData1) && this.m_TransformData.TryGetComponent(lotOwner, out componentData2) && this.m_PrefabRefData.TryGetComponent(lotOwner, out componentData3) && this.m_PrefabBuildingData.TryGetComponent(componentData3.m_Prefab, out componentData4))
        {
          float2 extents = new float2(componentData4.m_LotSize) * 4f;
          Game.Objects.Elevation componentData5;
          // ISSUE: reference to a compiler-generated field
          this.m_ElevationData.TryGetComponent(lotOwner, out componentData5);
          DynamicBuffer<InstalledUpgrade> bufferData;
          // ISSUE: reference to a compiler-generated field
          this.m_InstalledUpgrades.TryGetBuffer(lotOwner, out bufferData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lotInfo = BuildingUtils.CalculateLotInfo(extents, componentData2, componentData5, componentData1, componentData3, bufferData, this.m_TransformData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_PrefabBuildingTerraformData, this.m_PrefabBuildingExtensionData, false, out bool _);
          return true;
        }
        lotInfo = new BuildingUtils.LotInfo();
        return false;
      }

      private void UpdateSubNets(
        Game.Objects.Transform transform,
        Game.Objects.Transform mainTransform,
        Game.Objects.Transform mainInverseTransform,
        Entity prefab,
        Entity original,
        Entity lotEntity,
        bool relocate,
        bool topLevel,
        OwnerDefinition ownerDefinition,
        NativeList<ClearAreaData> clearAreas,
        ref Unity.Mathematics.Random random)
      {
        // ISSUE: reference to a compiler-generated field
        bool flag = original == Entity.Null || relocate && this.m_EditorMode;
        float4 float4_1;
        // ISSUE: reference to a compiler-generated field
        if (flag & topLevel && this.m_PrefabSubNets.HasBuffer(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Prefabs.SubNet> prefabSubNet = this.m_PrefabSubNets[prefab];
          NativeList<float4> nodePositions = new NativeList<float4>(prefabSubNet.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          BuildingUtils.LotInfo lotInfo;
          // ISSUE: reference to a compiler-generated method
          bool ownerLot = this.GetOwnerLot(lotEntity, out lotInfo);
          for (int index = 0; index < prefabSubNet.Length; ++index)
          {
            Game.Prefabs.SubNet subNet = prefabSubNet[index];
            if (subNet.m_NodeIndex.x >= 0)
            {
              while (nodePositions.Length <= subNet.m_NodeIndex.x)
              {
                ref NativeList<float4> local1 = ref nodePositions;
                float4_1 = new float4();
                ref float4 local2 = ref float4_1;
                local1.Add(in local2);
              }
              nodePositions[subNet.m_NodeIndex.x] += new float4(subNet.m_Curve.a, 1f);
            }
            if (subNet.m_NodeIndex.y >= 0)
            {
              while (nodePositions.Length <= subNet.m_NodeIndex.y)
              {
                ref NativeList<float4> local3 = ref nodePositions;
                float4_1 = new float4();
                ref float4 local4 = ref float4_1;
                local3.Add(in local4);
              }
              nodePositions[subNet.m_NodeIndex.y] += new float4(subNet.m_Curve.d, 1f);
            }
          }
          for (int index = 0; index < nodePositions.Length; ++index)
            nodePositions[index] /= math.max(1f, nodePositions[index].w);
          for (int index = 0; index < prefabSubNet.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Game.Prefabs.SubNet subNet = NetUtils.GetSubNet(prefabSubNet, index, this.m_LefthandTraffic, ref this.m_PrefabNetGeometryData);
            // ISSUE: reference to a compiler-generated method
            this.CreateSubNet(subNet.m_Prefab, Entity.Null, subNet.m_Curve, subNet.m_NodeIndex, subNet.m_ParentMesh, subNet.m_Upgrades, nodePositions, transform, ownerDefinition, clearAreas, lotInfo, ownerLot, ref random);
          }
          nodePositions.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (flag & topLevel && this.m_EditorMode && this.m_PrefabSubLanes.HasBuffer(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Prefabs.SubLane> prefabSubLane = this.m_PrefabSubLanes[prefab];
          NativeList<float4> nodePositions = new NativeList<float4>(prefabSubLane.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < prefabSubLane.Length; ++index)
          {
            Game.Prefabs.SubLane subLane = prefabSubLane[index];
            if (subLane.m_NodeIndex.x >= 0)
            {
              while (nodePositions.Length <= subLane.m_NodeIndex.x)
              {
                ref NativeList<float4> local5 = ref nodePositions;
                float4_1 = new float4();
                ref float4 local6 = ref float4_1;
                local5.Add(in local6);
              }
              nodePositions[subLane.m_NodeIndex.x] += new float4(subLane.m_Curve.a, 1f);
            }
            if (subLane.m_NodeIndex.y >= 0)
            {
              while (nodePositions.Length <= subLane.m_NodeIndex.y)
              {
                ref NativeList<float4> local7 = ref nodePositions;
                float4_1 = new float4();
                ref float4 local8 = ref float4_1;
                local7.Add(in local8);
              }
              nodePositions[subLane.m_NodeIndex.y] += new float4(subLane.m_Curve.d, 1f);
            }
          }
          for (int index = 0; index < nodePositions.Length; ++index)
            nodePositions[index] /= math.max(1f, nodePositions[index].w);
          for (int index = 0; index < prefabSubLane.Length; ++index)
          {
            Game.Prefabs.SubLane subLane = prefabSubLane[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CreateSubNet(this.m_LaneEditor, subLane.m_Prefab, subLane.m_Curve, subLane.m_NodeIndex, subLane.m_ParentMesh, new CompositionFlags(), nodePositions, transform, ownerDefinition, clearAreas, new BuildingUtils.LotInfo(), false, ref random);
          }
          nodePositions.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubNets.HasBuffer(original))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubNet> subNet1 = this.m_SubNets[original];
        NativeHashMap<Entity, int> nativeHashMap = new NativeHashMap<Entity, int>();
        NativeList<float4> nodePositions1 = new NativeList<float4>();
        BuildingUtils.LotInfo lotInfo1 = new BuildingUtils.LotInfo();
        bool hasLot = false;
        if (!flag & relocate)
        {
          nativeHashMap = new NativeHashMap<Entity, int>(subNet1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          nodePositions1 = new NativeList<float4>(subNet1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated method
          hasLot = this.GetOwnerLot(lotEntity, out lotInfo1);
          for (int index = 0; index < subNet1.Length; ++index)
          {
            Entity subNet2 = subNet1[index].m_SubNet;
            Game.Net.Node componentData1;
            float4 float4_2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeData.TryGetComponent(subNet2, out componentData1))
            {
              if (nativeHashMap.TryAdd(subNet2, nodePositions1.Length))
              {
                componentData1.m_Position = ObjectUtils.WorldToLocal(mainInverseTransform, componentData1.m_Position);
                ref NativeList<float4> local9 = ref nodePositions1;
                float4_2 = new float4(componentData1.m_Position, 1f);
                ref float4 local10 = ref float4_2;
                local9.Add(in local10);
              }
            }
            else
            {
              Game.Net.Edge componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EdgeData.TryGetComponent(subNet2, out componentData2))
              {
                if (nativeHashMap.TryAdd(componentData2.m_Start, nodePositions1.Length))
                {
                  // ISSUE: reference to a compiler-generated field
                  componentData1.m_Position = ObjectUtils.WorldToLocal(mainInverseTransform, this.m_NodeData[componentData2.m_Start].m_Position);
                  ref NativeList<float4> local11 = ref nodePositions1;
                  float4_2 = new float4(componentData1.m_Position, 1f);
                  ref float4 local12 = ref float4_2;
                  local11.Add(in local12);
                }
                if (nativeHashMap.TryAdd(componentData2.m_End, nodePositions1.Length))
                {
                  // ISSUE: reference to a compiler-generated field
                  componentData1.m_Position = ObjectUtils.WorldToLocal(mainInverseTransform, this.m_NodeData[componentData2.m_End].m_Position);
                  ref NativeList<float4> local13 = ref nodePositions1;
                  float4_2 = new float4(componentData1.m_Position, 1f);
                  ref float4 local14 = ref float4_2;
                  local13.Add(in local14);
                }
              }
            }
          }
        }
        for (int index = 0; index < subNet1.Length; ++index)
        {
          Entity subNet3 = subNet1[index].m_SubNet;
          Game.Net.Node componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.TryGetComponent(subNet3, out componentData3))
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.HasEdgeStartOrEnd(subNet3, original))
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity();
              CreationDefinition component1 = new CreationDefinition();
              component1.m_Original = subNet3;
              Game.Net.Elevation componentData4;
              // ISSUE: reference to a compiler-generated field
              bool component2 = this.m_NetElevationData.TryGetComponent(subNet3, out componentData4);
              bool onGround = !component2 || (double) math.cmin(math.abs(componentData4.m_Elevation)) < 2.0;
              if (flag | relocate || ClearAreaHelpers.ShouldClear(clearAreas, componentData3.m_Position, onGround))
                component1.m_Flags |= CreationFlags.Delete | CreationFlags.Hidden;
              else if (ownerDefinition.m_Prefab != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorContainerData.HasComponent(subNet3))
              {
                // ISSUE: reference to a compiler-generated field
                component1.m_SubPrefab = this.m_EditorContainerData[subNet3].m_Prefab;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
              NetCourse component3 = new NetCourse();
              component3.m_Curve = new Bezier4x3(componentData3.m_Position, componentData3.m_Position, componentData3.m_Position, componentData3.m_Position);
              component3.m_Length = 0.0f;
              component3.m_FixedIndex = -1;
              component3.m_StartPosition.m_Entity = subNet3;
              component3.m_StartPosition.m_Position = componentData3.m_Position;
              component3.m_StartPosition.m_Rotation = componentData3.m_Rotation;
              component3.m_StartPosition.m_CourseDelta = 0.0f;
              component3.m_StartPosition.m_ParentMesh = -1;
              component3.m_EndPosition.m_Entity = subNet3;
              component3.m_EndPosition.m_Position = componentData3.m_Position;
              component3.m_EndPosition.m_Rotation = componentData3.m_Rotation;
              component3.m_EndPosition.m_CourseDelta = 1f;
              component3.m_EndPosition.m_ParentMesh = -1;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<NetCourse>(entity, component3);
              if (!flag & relocate)
              {
                // ISSUE: reference to a compiler-generated field
                Entity netPrefab = (Entity) this.m_PrefabRefData[subNet3];
                componentData3.m_Position = ObjectUtils.WorldToLocal(mainInverseTransform, componentData3.m_Position);
                component3.m_Curve = new Bezier4x3(componentData3.m_Position, componentData3.m_Position, componentData3.m_Position, componentData3.m_Position);
                if (!component2)
                  component3.m_Curve.y = new Bezier4x1();
                int nodeIndex = nativeHashMap[subNet3];
                int parentMesh = component2 ? 0 : -1;
                Upgraded componentData5;
                // ISSUE: reference to a compiler-generated field
                this.m_UpgradedData.TryGetComponent(subNet3, out componentData5);
                // ISSUE: reference to a compiler-generated method
                this.CreateSubNet(netPrefab, component1.m_SubPrefab, component3.m_Curve, (int2) nodeIndex, (int2) parentMesh, componentData5.m_Flags, nodePositions1, mainTransform, ownerDefinition, clearAreas, lotInfo1, hasLot, ref random);
              }
            }
          }
          else
          {
            Game.Net.Edge componentData6;
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.TryGetComponent(subNet3, out componentData6))
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity();
              CreationDefinition component4 = new CreationDefinition();
              component4.m_Original = subNet3;
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subNet3];
              Game.Net.Elevation componentData7;
              // ISSUE: reference to a compiler-generated field
              bool component5 = this.m_NetElevationData.TryGetComponent(subNet3, out componentData7);
              bool onGround = !component5 || (double) math.cmin(math.abs(componentData7.m_Elevation)) < 2.0;
              if (flag | relocate || ClearAreaHelpers.ShouldClear(clearAreas, curve.m_Bezier, onGround))
                component4.m_Flags |= CreationFlags.Delete | CreationFlags.Hidden;
              else if (ownerDefinition.m_Prefab != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorContainerData.HasComponent(subNet3))
              {
                // ISSUE: reference to a compiler-generated field
                component4.m_SubPrefab = this.m_EditorContainerData[subNet3].m_Prefab;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component4);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
              NetCourse component6 = new NetCourse()
              {
                m_Curve = curve.m_Bezier
              };
              component6.m_Length = MathUtils.Length(component6.m_Curve);
              component6.m_FixedIndex = -1;
              component6.m_StartPosition.m_Entity = componentData6.m_Start;
              component6.m_StartPosition.m_Position = component6.m_Curve.a;
              component6.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component6.m_Curve));
              component6.m_StartPosition.m_CourseDelta = 0.0f;
              component6.m_StartPosition.m_ParentMesh = -1;
              component6.m_EndPosition.m_Entity = componentData6.m_End;
              component6.m_EndPosition.m_Position = component6.m_Curve.d;
              component6.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component6.m_Curve));
              component6.m_EndPosition.m_CourseDelta = 1f;
              component6.m_EndPosition.m_ParentMesh = -1;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<NetCourse>(entity, component6);
              if (!flag & relocate)
              {
                // ISSUE: reference to a compiler-generated field
                Entity netPrefab = (Entity) this.m_PrefabRefData[subNet3];
                component6.m_Curve.a = ObjectUtils.WorldToLocal(mainInverseTransform, component6.m_Curve.a);
                component6.m_Curve.b = ObjectUtils.WorldToLocal(mainInverseTransform, component6.m_Curve.b);
                component6.m_Curve.c = ObjectUtils.WorldToLocal(mainInverseTransform, component6.m_Curve.c);
                component6.m_Curve.d = ObjectUtils.WorldToLocal(mainInverseTransform, component6.m_Curve.d);
                if (!component5)
                  component6.m_Curve.y = new Bezier4x1();
                int2 nodeIndex = new int2(nativeHashMap[componentData6.m_Start], nativeHashMap[componentData6.m_End]);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int2 parentMesh = new int2(this.m_NetElevationData.HasComponent(componentData6.m_Start) ? 0 : -1, this.m_NetElevationData.HasComponent(componentData6.m_End) ? 0 : -1);
                Upgraded componentData8;
                // ISSUE: reference to a compiler-generated field
                this.m_UpgradedData.TryGetComponent(subNet3, out componentData8);
                // ISSUE: reference to a compiler-generated method
                this.CreateSubNet(netPrefab, component4.m_SubPrefab, component6.m_Curve, nodeIndex, parentMesh, componentData8.m_Flags, nodePositions1, mainTransform, ownerDefinition, clearAreas, lotInfo1, hasLot, ref random);
              }
            }
          }
        }
        if (nativeHashMap.IsCreated)
          nativeHashMap.Dispose();
        if (!nodePositions1.IsCreated)
          return;
        nodePositions1.Dispose();
      }

      private void UpdateSubAreas(
        Game.Objects.Transform transform,
        Entity prefab,
        Entity original,
        bool relocate,
        bool rebuild,
        bool topLevel,
        OwnerDefinition ownerDefinition,
        NativeList<ClearAreaData> clearAreas,
        ref Unity.Mathematics.Random random,
        ref NativeParallelHashMap<Entity, int> selectedSpawnables)
      {
        bool flag1 = original == Entity.Null | relocate | rebuild;
        // ISSUE: reference to a compiler-generated field
        if (flag1 & topLevel && this.m_PrefabSubAreas.HasBuffer(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Prefabs.SubArea> prefabSubArea = this.m_PrefabSubAreas[prefab];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubAreaNode> prefabSubAreaNode = this.m_PrefabSubAreaNodes[prefab];
          for (int index1 = 0; index1 < prefabSubArea.Length; ++index1)
          {
            Game.Prefabs.SubArea subArea = prefabSubArea[index1];
            int num;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_EditorMode && this.m_PrefabPlaceholderElements.HasBuffer(subArea.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<PlaceholderObjectElement> placeholderElement = this.m_PrefabPlaceholderElements[subArea.m_Prefab];
              if (!selectedSpawnables.IsCreated)
                selectedSpawnables = new NativeParallelHashMap<Entity, int>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<SpawnableObjectData> spawnableObjectData = this.m_PrefabSpawnableObjectData;
              NativeParallelHashMap<Entity, int> selectedSpawnables1 = selectedSpawnables;
              ref Unity.Mathematics.Random local1 = ref random;
              ref Entity local2 = ref subArea.m_Prefab;
              ref int local3 = ref num;
              if (!AreaUtils.SelectAreaPrefab(placeholderElement, spawnableObjectData, selectedSpawnables1, ref local1, out local2, out local3))
                continue;
            }
            else
              num = random.NextInt();
            // ISSUE: reference to a compiler-generated field
            AreaGeometryData areaGeometryData = this.m_PrefabAreaGeometryData[subArea.m_Prefab];
            if (areaGeometryData.m_Type == AreaType.Space)
            {
              if (ClearAreaHelpers.ShouldClear(clearAreas, prefabSubAreaNode, subArea.m_NodeRange, transform))
                continue;
            }
            else if (areaGeometryData.m_Type == AreaType.Lot && rebuild)
              continue;
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity();
            CreationDefinition component = new CreationDefinition();
            component.m_Prefab = subArea.m_Prefab;
            component.m_RandomSeed = num;
            if (areaGeometryData.m_Type != AreaType.Lot)
              component.m_Flags |= CreationFlags.Hidden;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
            if (ownerDefinition.m_Prefab != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
            }
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Areas.Node> dynamicBuffer1 = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity);
            dynamicBuffer1.ResizeUninitialized(subArea.m_NodeRange.y - subArea.m_NodeRange.x + 1);
            DynamicBuffer<LocalNodeCache> dynamicBuffer2 = new DynamicBuffer<LocalNodeCache>();
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode)
            {
              // ISSUE: reference to a compiler-generated field
              dynamicBuffer2 = this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity);
              dynamicBuffer2.ResizeUninitialized(dynamicBuffer1.Length);
            }
            // ISSUE: reference to a compiler-generated method
            int index2 = ObjectToolBaseSystem.GetFirstNodeIndex(prefabSubAreaNode, subArea.m_NodeRange);
            int index3 = 0;
            for (int x = subArea.m_NodeRange.x; x <= subArea.m_NodeRange.y; ++x)
            {
              float3 position = prefabSubAreaNode[index2].m_Position;
              float3 world = ObjectUtils.LocalToWorld(transform, position);
              int parentMesh = prefabSubAreaNode[index2].m_ParentMesh;
              float elevation = math.select(float.MinValue, position.y, parentMesh >= 0);
              dynamicBuffer1[index3] = new Game.Areas.Node(world, elevation);
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorMode)
                dynamicBuffer2[index3] = new LocalNodeCache()
                {
                  m_Position = position,
                  m_ParentMesh = parentMesh
                };
              ++index3;
              if (++index2 == subArea.m_NodeRange.y)
                index2 = subArea.m_NodeRange.x;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubAreas.HasBuffer(original))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.SubArea> subArea1 = this.m_SubAreas[original];
        for (int index = 0; index < subArea1.Length; ++index)
        {
          Entity area = subArea1[index].m_Area;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[area];
          bool flag2 = flag1;
          // ISSUE: reference to a compiler-generated field
          if (!flag2 && this.m_AreaSpaceData.HasComponent(area))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Triangle> areaTriangle = this.m_AreaTriangles[area];
            flag2 = ClearAreaHelpers.ShouldClear(clearAreas, areaNode, areaTriangle, transform);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_AreaLotData.HasComponent(area))
          {
            if (flag2)
              flag2 = !rebuild;
            else
              continue;
          }
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity();
          CreationDefinition component = new CreationDefinition();
          component.m_Original = area;
          if (flag2)
            component.m_Flags |= CreationFlags.Delete | CreationFlags.Hidden;
          else if (ownerDefinition.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity).CopyFrom(areaNode.AsNativeArray());
          // ISSUE: reference to a compiler-generated field
          if (this.m_CachedNodes.HasBuffer(area))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<LocalNodeCache> cachedNode = this.m_CachedNodes[area];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity).CopyFrom(cachedNode.AsNativeArray());
          }
        }
      }

      private bool HasEdgeStartOrEnd(Entity node, Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge1 = connectedEdge[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge2 = this.m_EdgeData[edge1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((edge2.m_Start == node || edge2.m_End == node) && this.m_OwnerData.HasComponent(edge1) && this.m_OwnerData[edge1].m_Owner == owner)
            return true;
        }
        return false;
      }

      private struct VariationData
      {
        public Entity m_Prefab;
        public int m_Probability;
      }

      private struct BrushIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public NativeParallelHashSet<Entity> m_RequirePrefab;
        public Bounds2 m_Bounds;
        public RandomSeed m_RandomSeed;
        public Line3.Segment m_BrushLine;
        public float4 m_BrushCellSizeFactor;
        public float4 m_BrushTextureSizeAdd;
        public float2 m_BrushDirX;
        public float2 m_BrushDirZ;
        public float2 m_BrushCellSize;
        public int2 m_BrushResolution;
        public float m_TileSize;
        public float m_BrushStrength;
        public float m_StrengthFactor;
        public int m_BrushCount;
        public DynamicBuffer<BrushCell> m_BrushCells;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
        public ComponentLookup<EditorContainer> m_EditorContainerData;
        public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
        public EntityCommandBuffer m_CommandBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || this.m_OwnerData.HasComponent(item))
            return;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[item];
          // ISSUE: reference to a compiler-generated field
          if (this.m_RequirePrefab.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_RequirePrefab.Contains(prefabRef.m_Prefab))
              return;
          }
          else
          {
            ObjectGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData) || (componentData.m_Flags & Game.Objects.GeometryFlags.Brushable) == Game.Objects.GeometryFlags.None)
              return;
          }
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[item];
          // ISSUE: reference to a compiler-generated field
          int2 int2 = (int2) math.floor(transform.m_Position.xz / this.m_TileSize);
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom((int2.y & (int) ushort.MaxValue) << 16 | int2.x & (int) ushort.MaxValue);
          // ISSUE: reference to a compiler-generated field
          if ((double) random.NextFloat(1f) >= (double) this.m_BrushStrength)
            return;
          float num = 0.0f;
          Bounds2 bounds2;
          // ISSUE: reference to a compiler-generated field
          bounds2.min = (float2) int2 * this.m_TileSize;
          // ISSUE: reference to a compiler-generated field
          bounds2.max = bounds2.min + this.m_TileSize;
          // ISSUE: reference to a compiler-generated field
          for (int index = 1; index <= this.m_BrushCount; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3 float3 = MathUtils.Position(this.m_BrushLine, (float) index / (float) this.m_BrushCount);
            Bounds2 bounds1 = bounds2;
            bounds1.min -= float3.xz;
            bounds1.max -= float3.xz;
            float4 float4 = new float4(bounds1.min, bounds1.max);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float4 x1 = new float4(math.dot(float4.xy, this.m_BrushDirX), math.dot(float4.xw, this.m_BrushDirX), math.dot(float4.zy, this.m_BrushDirX), math.dot(float4.zw, this.m_BrushDirX));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float4 x2 = new float4(math.dot(float4.xy, this.m_BrushDirZ), math.dot(float4.xw, this.m_BrushDirZ), math.dot(float4.zy, this.m_BrushDirZ), math.dot(float4.zw, this.m_BrushDirZ));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int4 int4 = math.clamp((int4) math.floor(new float4(math.cmin(x1), math.cmin(x2), math.cmax(x1), math.cmax(x2)) * this.m_BrushCellSizeFactor + this.m_BrushTextureSizeAdd), (int4) 0, this.m_BrushResolution.xyxy - 1);
            for (int y = int4.y; y <= int4.w; ++y)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float2 float2_1 = this.m_BrushDirZ * (((float) y - this.m_BrushTextureSizeAdd.y) * this.m_BrushCellSize.y);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float2 float2_2 = this.m_BrushDirZ * (((float) (y + 1) - this.m_BrushTextureSizeAdd.y) * this.m_BrushCellSize.y);
              for (int x3 = int4.x; x3 <= int4.z; ++x3)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                BrushCell brushCell = this.m_BrushCells[x3 + this.m_BrushResolution.x * y];
                if ((double) brushCell.m_Opacity >= 9.9999997473787516E-05)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float2 float2_3 = this.m_BrushDirX * (((float) x3 - this.m_BrushTextureSizeAdd.x) * this.m_BrushCellSize.x);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float2 float2_4 = this.m_BrushDirX * (((float) (x3 + 1) - this.m_BrushTextureSizeAdd.x) * this.m_BrushCellSize.x);
                  Quad2 quad = new Quad2(float2_1 + float2_3, float2_1 + float2_4, float2_2 + float2_4, float2_2 + float2_3);
                  float area;
                  if (MathUtils.Intersect(bounds1, quad, out area))
                    num += brushCell.m_Opacity * area;
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          float x = num * this.m_StrengthFactor;
          if ((double) math.abs(x) < 9.9999997473787516E-05 || (double) random.NextFloat() >= (double) x)
            return;
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity();
          CreationDefinition component1 = new CreationDefinition();
          component1.m_Original = item;
          component1.m_Flags |= CreationFlags.Delete;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          ObjectDefinition component2 = new ObjectDefinition();
          component2.m_Position = transform.m_Position;
          component2.m_Rotation = transform.m_Rotation;
          Game.Objects.Elevation componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.TryGetComponent(item, out componentData1))
          {
            component2.m_Elevation = componentData1.m_Elevation;
            component2.m_ParentMesh = ObjectUtils.GetSubParentMesh(componentData1.m_Flags);
            if ((componentData1.m_Flags & ElevationFlags.Lowered) != (ElevationFlags) 0)
              component1.m_Flags |= CreationFlags.Lowered;
          }
          else
            component2.m_ParentMesh = -1;
          component2.m_Probability = 100;
          component2.m_PrefabSubIndex = -1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalTransformCacheData.HasComponent(item))
          {
            // ISSUE: reference to a compiler-generated field
            LocalTransformCache localTransformCache = this.m_LocalTransformCacheData[item];
            component2.m_LocalPosition = localTransformCache.m_Position;
            component2.m_LocalRotation = localTransformCache.m_Rotation;
            component2.m_ParentMesh = localTransformCache.m_ParentMesh;
            component2.m_GroupIndex = localTransformCache.m_GroupIndex;
            component2.m_Probability = localTransformCache.m_Probability;
            component2.m_PrefabSubIndex = localTransformCache.m_PrefabSubIndex;
          }
          else
          {
            component2.m_LocalPosition = transform.m_Position;
            component2.m_LocalRotation = transform.m_Rotation;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorContainerData.HasComponent(item))
          {
            // ISSUE: reference to a compiler-generated field
            EditorContainer editorContainer = this.m_EditorContainerData[item];
            component1.m_SubPrefab = editorContainer.m_Prefab;
            component2.m_Scale = editorContainer.m_Scale;
            component2.m_Intensity = editorContainer.m_Intensity;
            component2.m_GroupIndex = editorContainer.m_GroupIndex;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<ObjectDefinition>(entity, component2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
        }
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Lot> __Game_Buildings_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Clear> __Game_Areas_Clear_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Space> __Game_Areas_Space_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetObjectData> __Game_Prefabs_NetObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AssetStampData> __Game_Prefabs_AssetStampData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BrushData> __Game_Prefabs_BrushData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> __Game_Prefabs_BuildingTerraformData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CreatureSpawnData> __Game_Prefabs_CreatureSpawnData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> __Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> __Game_Prefabs_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> __Game_Prefabs_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubLane> __Game_Prefabs_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubAreaNode> __Game_Prefabs_SubAreaNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceUpgradeBuilding> __Game_Prefabs_ServiceUpgradeBuilding_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<BrushCell> __Game_Prefabs_BrushCell_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentLookup = state.GetComponentLookup<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Clear_RO_ComponentLookup = state.GetComponentLookup<Clear>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Space_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Space>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetObjectData_RO_ComponentLookup = state.GetComponentLookup<NetObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AssetStampData_RO_ComponentLookup = state.GetComponentLookup<AssetStampData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BrushData_RO_ComponentLookup = state.GetComponentLookup<BrushData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup = state.GetComponentLookup<BuildingTerraformData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureSpawnData_RO_ComponentLookup = state.GetComponentLookup<CreatureSpawnData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferLookup = state.GetBufferLookup<LocalNodeCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubAreaNode_RO_BufferLookup = state.GetBufferLookup<SubAreaNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferLookup = state.GetBufferLookup<ServiceUpgradeBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BrushCell_RO_BufferLookup = state.GetBufferLookup<BrushCell>(true);
      }
    }
  }
}
