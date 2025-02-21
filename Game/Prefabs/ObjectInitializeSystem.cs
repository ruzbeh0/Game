// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Rendering;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class ObjectInitializeSystem : GameSystemBase
  {
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_PlaceholderQuery;
    private PrefabSystem m_PrefabSystem;
    private ObjectInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<ObjectData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PlaceholderQuery = this.GetEntityQuery(ComponentType.ReadOnly<ObjectData>(), ComponentType.ReadOnly<PlaceholderObjectElement>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      bool flag1 = false;
      try
      {
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
        this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<UtilityObjectData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_UtilityObjectData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PillarData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PillarData> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_PillarData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetObjectData> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlantData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PlantData> componentTypeHandle6 = this.__TypeHandle.__Game_Prefabs_PlantData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<HumanData> componentTypeHandle7 = this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<BuildingData> componentTypeHandle8 = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_VehicleData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<VehicleData> componentTypeHandle9 = this.__TypeHandle.__Game_Prefabs_VehicleData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<BuildingExtensionData> componentTypeHandle10 = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<ObjectGeometryData> componentTypeHandle11 = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PlaceableObjectData> componentTypeHandle12 = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<SpawnableObjectData> componentTypeHandle13 = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AssetStampData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<AssetStampData> componentTypeHandle14 = this.__TypeHandle.__Game_Prefabs_AssetStampData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<GrowthScaleData> componentTypeHandle15 = this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_StackData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<StackData> componentTypeHandle16 = this.__TypeHandle.__Game_Prefabs_StackData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<QuantityObjectData> componentTypeHandle17 = this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_CreatureData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<CreatureData> componentTypeHandle18 = this.__TypeHandle.__Game_Prefabs_CreatureData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<BuildingTerraformData> componentTypeHandle19 = this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubMesh> bufferTypeHandle1 = this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubMeshGroup> bufferTypeHandle2 = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_CharacterElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<CharacterElement> bufferTypeHandle3 = this.__TypeHandle.__Game_Prefabs_CharacterElement_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubObject_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubObject> bufferTypeHandle4 = this.__TypeHandle.__Game_Prefabs_SubObject_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubNet_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubNet> bufferTypeHandle5 = this.__TypeHandle.__Game_Prefabs_SubNet_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubLane> bufferTypeHandle6 = this.__TypeHandle.__Game_Prefabs_SubLane_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubArea_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubArea> bufferTypeHandle7 = this.__TypeHandle.__Game_Prefabs_SubArea_RW_BufferTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubAreaNode_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferTypeHandle<SubAreaNode> bufferTypeHandle8 = this.__TypeHandle.__Game_Prefabs_SubAreaNode_RW_BufferTypeHandle;
        this.CompleteDependency();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
          {
            flag1 = archetypeChunk.Has<SpawnableObjectData>(ref componentTypeHandle13);
          }
          else
          {
            NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle2);
            NativeArray<ObjectGeometryData> nativeArray2 = archetypeChunk.GetNativeArray<ObjectGeometryData>(ref componentTypeHandle11);
            NativeArray<PlaceableObjectData> nativeArray3 = archetypeChunk.GetNativeArray<PlaceableObjectData>(ref componentTypeHandle12);
            NativeArray<SpawnableObjectData> nativeArray4 = archetypeChunk.GetNativeArray<SpawnableObjectData>(ref componentTypeHandle13);
            NativeArray<AssetStampData> nativeArray5 = archetypeChunk.GetNativeArray<AssetStampData>(ref componentTypeHandle14);
            NativeArray<GrowthScaleData> nativeArray6 = archetypeChunk.GetNativeArray<GrowthScaleData>(ref componentTypeHandle15);
            NativeArray<StackData> nativeArray7 = archetypeChunk.GetNativeArray<StackData>(ref componentTypeHandle16);
            NativeArray<QuantityObjectData> nativeArray8 = archetypeChunk.GetNativeArray<QuantityObjectData>(ref componentTypeHandle17);
            NativeArray<CreatureData> nativeArray9 = archetypeChunk.GetNativeArray<CreatureData>(ref componentTypeHandle18);
            NativeArray<PillarData> nativeArray10 = archetypeChunk.GetNativeArray<PillarData>(ref componentTypeHandle4);
            NativeArray<BuildingTerraformData> nativeArray11 = archetypeChunk.GetNativeArray<BuildingTerraformData>(ref componentTypeHandle19);
            NativeArray<BuildingExtensionData> nativeArray12 = archetypeChunk.GetNativeArray<BuildingExtensionData>(ref componentTypeHandle10);
            BufferAccessor<SubObject> bufferAccessor1 = archetypeChunk.GetBufferAccessor<SubObject>(ref bufferTypeHandle4);
            BufferAccessor<SubArea> bufferAccessor2 = archetypeChunk.GetBufferAccessor<SubArea>(ref bufferTypeHandle7);
            BufferAccessor<SubMesh> bufferAccessor3 = archetypeChunk.GetBufferAccessor<SubMesh>(ref bufferTypeHandle1);
            BufferAccessor<SubMeshGroup> bufferAccessor4 = archetypeChunk.GetBufferAccessor<SubMeshGroup>(ref bufferTypeHandle2);
            BufferAccessor<CharacterElement> bufferAccessor5 = archetypeChunk.GetBufferAccessor<CharacterElement>(ref bufferTypeHandle3);
            BufferAccessor<SubNet> bufferAccessor6 = archetypeChunk.GetBufferAccessor<SubNet>(ref bufferTypeHandle5);
            BufferAccessor<SubLane> bufferAccessor7 = archetypeChunk.GetBufferAccessor<SubLane>(ref bufferTypeHandle6);
            bool flag2 = archetypeChunk.Has<UtilityObjectData>(ref componentTypeHandle3);
            bool flag3 = archetypeChunk.Has<NetObjectData>(ref componentTypeHandle5);
            bool isPlantObject = archetypeChunk.Has<PlantData>(ref componentTypeHandle6);
            bool isHumanObject = archetypeChunk.Has<HumanData>(ref componentTypeHandle7);
            bool isBuildingObject = archetypeChunk.Has<BuildingData>(ref componentTypeHandle8) || nativeArray12.Length != 0;
            bool isVehicleObject = archetypeChunk.Has<VehicleData>(ref componentTypeHandle9);
            bool isCreatureObject = nativeArray9.Length != 0;
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              ObjectPrefab prefab = this.m_PrefabSystem.GetPrefab<ObjectPrefab>(nativeArray1[index2]);
              ObjectGeometryData objectGeometryData = nativeArray2[index2] with
              {
                m_MinLod = (int) byte.MaxValue,
                m_Layers = (MeshLayer) 0
              };
              PlaceableObjectData placeableObjectData = new PlaceableObjectData();
              if (nativeArray3.Length != 0)
                placeableObjectData = nativeArray3[index2];
              GrowthScaleData growthScaleData = new GrowthScaleData();
              if (nativeArray6.Length != 0)
                growthScaleData = nativeArray6[index2];
              StackData stackData = new StackData();
              if (nativeArray7.Length != 0)
                stackData = nativeArray7[index2];
              QuantityObjectData quantityObjectData = new QuantityObjectData();
              if (nativeArray8.Length != 0)
                quantityObjectData = nativeArray8[index2];
              CreatureData creatureData = new CreatureData();
              if (nativeArray9.Length != 0)
              {
                creatureData = nativeArray9[index2];
                CreaturePrefab creaturePrefab = prefab as CreaturePrefab;
                creatureData.m_Gender = creaturePrefab.m_Gender;
                if (!isHumanObject)
                  objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.LowCollisionPriority;
              }
              switch (prefab)
              {
                case AssetStampPrefab _:
                  AssetStampData assetStampData = nativeArray5[index2];
                  // ISSUE: reference to a compiler-generated method
                  this.InitializePrefab(prefab as AssetStampPrefab, ref assetStampData, ref placeableObjectData, ref objectGeometryData);
                  nativeArray5[index2] = assetStampData;
                  objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.ExclusiveGround | Game.Objects.GeometryFlags.WalkThrough | Game.Objects.GeometryFlags.OccupyZone | Game.Objects.GeometryFlags.Stampable | Game.Objects.GeometryFlags.HasLot;
                  break;
                case ObjectGeometryPrefab _:
                  DynamicBuffer<SubMeshGroup> meshGroups;
                  CollectionUtils.TryGet<SubMeshGroup>(bufferAccessor4, index2, out meshGroups);
                  DynamicBuffer<CharacterElement> characterElements;
                  CollectionUtils.TryGet<CharacterElement>(bufferAccessor5, index2, out characterElements);
                  // ISSUE: reference to a compiler-generated method
                  this.InitializePrefab(prefab as ObjectGeometryPrefab, placeableObjectData, ref objectGeometryData, ref growthScaleData, ref stackData, ref quantityObjectData, ref creatureData, bufferAccessor3[index2], meshGroups, characterElements, isPlantObject, isHumanObject, isBuildingObject, isVehicleObject, isCreatureObject);
                  if (nativeArray10.Length != 0)
                  {
                    if (nativeArray10[index2].m_Type == PillarType.Horizontal)
                      objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.IgnoreBottomCollision;
                    else
                      objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.BaseCollision;
                    objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.OccupyZone | Game.Objects.GeometryFlags.CanSubmerge | Game.Objects.GeometryFlags.OptionalAttach;
                    break;
                  }
                  if (flag2)
                  {
                    objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.IgnoreSecondaryCollision;
                    break;
                  }
                  if (flag3)
                  {
                    objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.OccupyZone;
                    break;
                  }
                  objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.Brushable;
                  break;
                case MarkerObjectPrefab _:
                  // ISSUE: reference to a compiler-generated method
                  this.InitializePrefab(prefab as MarkerObjectPrefab, placeableObjectData, ref objectGeometryData, bufferAccessor3[index2]);
                  placeableObjectData.m_Flags |= Game.Objects.PlacementFlags.CanOverlap;
                  objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.WalkThrough;
                  break;
              }
              if (!isBuildingObject && nativeArray11.Length != 0)
              {
                BuildingTerraformOverride component = prefab.GetComponent<BuildingTerraformOverride>();
                Bounds2 bounds2;
                if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
                {
                  float2 xz = objectGeometryData.m_Pivot.xz;
                  float2 float2 = objectGeometryData.m_LegSize.xz * 0.5f;
                  bounds2 = new Bounds2(xz - float2, xz + float2);
                }
                else
                  bounds2 = objectGeometryData.m_Bounds.xz;
                BuildingTerraformData buildingTerraformData = nativeArray11[index2];
                ref BuildingTerraformData local = ref buildingTerraformData;
                Bounds2 lotBounds = bounds2;
                Bounds2 flatBounds = bounds2;
                // ISSUE: reference to a compiler-generated method
                BuildingInitializeSystem.InitializeTerraformData(component, ref local, lotBounds, flatBounds);
                nativeArray11[index2] = buildingTerraformData;
              }
              nativeArray2[index2] = objectGeometryData;
              if (nativeArray3.Length != 0)
                nativeArray3[index2] = placeableObjectData;
              if (nativeArray6.Length != 0)
                nativeArray6[index2] = growthScaleData;
              if (nativeArray7.Length != 0)
                nativeArray7[index2] = stackData;
              if (nativeArray8.Length != 0)
                nativeArray8[index2] = quantityObjectData;
              if (nativeArray9.Length != 0)
                nativeArray9[index2] = creatureData;
            }
            for (int index3 = 0; index3 < bufferAccessor1.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              ObjectSubObjects component = this.m_PrefabSystem.GetPrefab<ObjectPrefab>(nativeArray1[index3]).GetComponent<ObjectSubObjects>();
              if (component.m_SubObjects != null)
              {
                DynamicBuffer<SubObject> dynamicBuffer = bufferAccessor1[index3];
                for (int index4 = 0; index4 < component.m_SubObjects.Length; ++index4)
                {
                  ObjectSubObjectInfo subObject = component.m_SubObjects[index4];
                  ObjectPrefab prefab = subObject.m_Object;
                  Entity entity;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  if (!((UnityEngine.Object) prefab == (UnityEngine.Object) null) && this.m_PrefabSystem.TryGetEntity((PrefabBase) prefab, out entity))
                  {
                    SubObject elem = new SubObject();
                    elem.m_Prefab = entity;
                    elem.m_Position = subObject.m_Position;
                    elem.m_Rotation = subObject.m_Rotation;
                    elem.m_ParentIndex = subObject.m_ParentMesh;
                    elem.m_GroupIndex = subObject.m_GroupIndex;
                    elem.m_Probability = math.select(subObject.m_Probability, 100, subObject.m_Probability == 0);
                    if (subObject.m_ParentMesh == -1)
                      elem.m_Flags |= SubObjectFlags.OnGround;
                    dynamicBuffer.Add(elem);
                  }
                }
              }
            }
            for (int index5 = 0; index5 < bufferAccessor6.Length; ++index5)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              ObjectPrefab prefab = this.m_PrefabSystem.GetPrefab<ObjectPrefab>(nativeArray1[index5]);
              ObjectSubNets component = prefab.GetComponent<ObjectSubNets>();
              if (component.m_SubNets != null)
              {
                bool flag4 = false;
                DynamicBuffer<SubNet> dynamicBuffer = bufferAccessor6[index5];
                for (int p2 = 0; p2 < component.m_SubNets.Length; ++p2)
                {
                  ObjectSubNetInfo subNet = component.m_SubNets[p2];
                  NetPrefab netPrefab = subNet.m_NetPrefab;
                  Entity entity;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  if (!((UnityEngine.Object) netPrefab == (UnityEngine.Object) null) && this.m_PrefabSystem.TryGetEntity((PrefabBase) netPrefab, out entity))
                  {
                    SubNet elem = new SubNet();
                    elem.m_Prefab = entity;
                    elem.m_Curve = subNet.m_BezierCurve;
                    elem.m_NodeIndex = subNet.m_NodeIndex;
                    elem.m_InvertMode = component.m_InvertWhen;
                    elem.m_ParentMesh = subNet.m_ParentMesh;
                    if ((double) MathUtils.Min(subNet.m_BezierCurve).y <= -2.0)
                      flag4 = true;
                    NetSectionFlags sectionFlags;
                    NetCompositionHelpers.GetRequirementFlags(subNet.m_Upgrades, out elem.m_Upgrades, out sectionFlags);
                    if (sectionFlags != (NetSectionFlags) 0)
                      COSystemBase.baseLog.ErrorFormat((UnityEngine.Object) prefab, "ObjectSubNets ({0}[{1}]) cannot upgrade section flags: {2}", (object) prefab.name, (object) p2, (object) sectionFlags);
                    dynamicBuffer.Add(elem);
                  }
                }
                if (flag4)
                {
                  if (nativeArray3.Length != 0)
                  {
                    PlaceableObjectData placeableObjectData = nativeArray3[index5];
                    placeableObjectData.m_Flags |= Game.Objects.PlacementFlags.HasUndergroundElements;
                    nativeArray3[index5] = placeableObjectData;
                  }
                  if (nativeArray12.Length != 0)
                  {
                    BuildingExtensionData buildingExtensionData = nativeArray12[index5] with
                    {
                      m_HasUndergroundElements = true
                    };
                    nativeArray12[index5] = buildingExtensionData;
                  }
                }
              }
            }
            for (int index6 = 0; index6 < bufferAccessor7.Length; ++index6)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              ObjectSubLanes component = this.m_PrefabSystem.GetPrefab<ObjectPrefab>(nativeArray1[index6]).GetComponent<ObjectSubLanes>();
              if (component.m_SubLanes != null)
              {
                DynamicBuffer<SubLane> dynamicBuffer = bufferAccessor7[index6];
                for (int index7 = 0; index7 < component.m_SubLanes.Length; ++index7)
                {
                  ObjectSubLaneInfo subLane = component.m_SubLanes[index7];
                  NetLanePrefab lanePrefab = subLane.m_LanePrefab;
                  Entity entity;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  if (!((UnityEngine.Object) lanePrefab == (UnityEngine.Object) null) && this.m_PrefabSystem.TryGetEntity((PrefabBase) lanePrefab, out entity))
                    dynamicBuffer.Add(new SubLane()
                    {
                      m_Prefab = entity,
                      m_Curve = subLane.m_BezierCurve,
                      m_NodeIndex = subLane.m_NodeIndex,
                      m_ParentMesh = subLane.m_ParentMesh
                    });
                }
              }
            }
            if (bufferAccessor2.Length != 0)
            {
              BufferAccessor<SubAreaNode> bufferAccessor8 = archetypeChunk.GetBufferAccessor<SubAreaNode>(ref bufferTypeHandle8);
              for (int index8 = 0; index8 < bufferAccessor2.Length; ++index8)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                ObjectSubAreas component = this.m_PrefabSystem.GetPrefab<ObjectPrefab>(nativeArray1[index8]).GetComponent<ObjectSubAreas>();
                if (component.m_SubAreas != null)
                {
                  int length = 0;
                  for (int index9 = 0; index9 < component.m_SubAreas.Length; ++index9)
                  {
                    ObjectSubAreaInfo subArea = component.m_SubAreas[index9];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    if (!((UnityEngine.Object) subArea.m_AreaPrefab == (UnityEngine.Object) null) && this.m_PrefabSystem.TryGetEntity((PrefabBase) subArea.m_AreaPrefab, out Entity _))
                      length += subArea.m_NodePositions.Length;
                  }
                  DynamicBuffer<SubArea> dynamicBuffer1 = bufferAccessor2[index8];
                  DynamicBuffer<SubAreaNode> dynamicBuffer2 = bufferAccessor8[index8];
                  dynamicBuffer1.EnsureCapacity(component.m_SubAreas.Length);
                  dynamicBuffer2.ResizeUninitialized(length);
                  int num = 0;
                  for (int index10 = 0; index10 < component.m_SubAreas.Length; ++index10)
                  {
                    ObjectSubAreaInfo subArea = component.m_SubAreas[index10];
                    Entity entity;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    if (!((UnityEngine.Object) subArea.m_AreaPrefab == (UnityEngine.Object) null) && this.m_PrefabSystem.TryGetEntity((PrefabBase) subArea.m_AreaPrefab, out entity))
                    {
                      SubArea elem;
                      elem.m_Prefab = entity;
                      elem.m_NodeRange.x = num;
                      if (subArea.m_ParentMeshes != null && subArea.m_ParentMeshes.Length != 0)
                      {
                        for (int index11 = 0; index11 < subArea.m_NodePositions.Length; ++index11)
                        {
                          float3 nodePosition = subArea.m_NodePositions[index11];
                          int parentMesh = subArea.m_ParentMeshes[index11];
                          dynamicBuffer2[num++] = new SubAreaNode(nodePosition, parentMesh);
                        }
                      }
                      else
                      {
                        for (int index12 = 0; index12 < subArea.m_NodePositions.Length; ++index12)
                        {
                          float3 nodePosition = subArea.m_NodePositions[index12];
                          int parentMesh = -1;
                          dynamicBuffer2[num++] = new SubAreaNode(nodePosition, parentMesh);
                        }
                      }
                      elem.m_NodeRange.y = num;
                      dynamicBuffer1.Add(elem);
                    }
                  }
                }
              }
            }
            if (nativeArray4.Length != 0)
            {
              NativeArray<Entity> nativeArray13 = archetypeChunk.GetNativeArray(entityTypeHandle);
              for (int index13 = 0; index13 < nativeArray4.Length; ++index13)
              {
                Entity entity1 = nativeArray13[index13];
                SpawnableObjectData spawnableObjectData = nativeArray4[index13];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                SpawnableObject component = this.m_PrefabSystem.GetPrefab<ObjectPrefab>(nativeArray1[index13]).GetComponent<SpawnableObject>();
                if (component.m_Placeholders != null)
                {
                  for (int index14 = 0; index14 < component.m_Placeholders.Length; ++index14)
                  {
                    ObjectPrefab placeholder = component.m_Placeholders[index14];
                    Entity entity2;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    if (!((UnityEngine.Object) placeholder == (UnityEngine.Object) null) && this.m_PrefabSystem.TryGetEntity((PrefabBase) placeholder, out entity2))
                      this.EntityManager.GetBuffer<PlaceholderObjectElement>(entity2).Add(new PlaceholderObjectElement(entity1));
                  }
                }
                if ((UnityEngine.Object) component.m_RandomizationGroup != (UnityEngine.Object) null)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  spawnableObjectData.m_RandomizationGroup = this.m_PrefabSystem.GetEntity((PrefabBase) component.m_RandomizationGroup);
                }
                spawnableObjectData.m_Probability = component.m_Probability;
                nativeArray4[index13] = spawnableObjectData;
              }
            }
          }
        }
        JobHandle dependsOn1 = new JobHandle();
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          dependsOn1 = new ObjectInitializeSystem.FixPlaceholdersJob()
          {
            m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
            m_PlaceholderObjectElementType = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle
          }.ScheduleParallel<ObjectInitializeSystem.FixPlaceholdersJob>(this.m_PlaceholderQuery, this.Dependency);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceholderObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        ObjectInitializeSystem.FindPlaceholderRequirementsJob jobData1 = new ObjectInitializeSystem.FindPlaceholderRequirementsJob()
        {
          m_Chunks = archetypeChunkArray,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_PlaceholderObjectElementType = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferTypeHandle,
          m_PlaceholderObjectDataType = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectData_RW_ComponentTypeHandle,
          m_ObjectRequirementElements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        ObjectInitializeSystem.FindSubObjectRequirementsJob jobData2 = new ObjectInitializeSystem.FindSubObjectRequirementsJob()
        {
          m_Chunks = archetypeChunkArray,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_SubObjectType = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferTypeHandle,
          m_ObjectGeometryDataType = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle,
          m_PlaceholderObjectData = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup,
          m_SubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubNet_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        ObjectInitializeSystem.InitializeSubNetsJob jobData3 = new ObjectInitializeSystem.InitializeSubNetsJob()
        {
          m_Chunks = archetypeChunkArray,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_PlaceableObjectDataType = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle,
          m_SubNetType = this.__TypeHandle.__Game_Prefabs_SubNet_RW_BufferTypeHandle,
          m_NetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup
        };
        JobHandle dependsOn2 = jobData1.Schedule<ObjectInitializeSystem.FindPlaceholderRequirementsJob>(archetypeChunkArray.Length, 1, dependsOn1);
        JobHandle job0 = jobData2.Schedule<ObjectInitializeSystem.FindSubObjectRequirementsJob>(archetypeChunkArray.Length, 1, dependsOn2);
        int length1 = archetypeChunkArray.Length;
        JobHandle dependsOn3 = new JobHandle();
        JobHandle job1 = jobData3.Schedule<ObjectInitializeSystem.InitializeSubNetsJob>(length1, 1, dependsOn3);
        this.Dependency = JobHandle.CombineDependencies(job0, job1);
      }
      finally
      {
        archetypeChunkArray.Dispose(this.Dependency);
      }
    }

    private void InitializePrefab(
      AssetStampPrefab stampPrefab,
      ref AssetStampData assetStampData,
      ref PlaceableObjectData placeableObjectData,
      ref ObjectGeometryData objectGeometryData)
    {
      assetStampData.m_Size = new int2(stampPrefab.m_Width, stampPrefab.m_Depth);
      placeableObjectData.m_ConstructionCost = stampPrefab.m_ConstructionCost;
      assetStampData.m_UpKeepCost = stampPrefab.m_UpKeepCost;
      float2 float2_1 = (float2) assetStampData.m_Size * 8f;
      objectGeometryData.m_MinLod = math.min(objectGeometryData.m_MinLod, RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float3(float2_1.x, 0.0f, float2_1.y))));
      objectGeometryData.m_Layers = MeshLayer.Default;
      float2 float2_2 = float2_1 - 0.4f;
      objectGeometryData.m_Size.xz = float2_2;
      objectGeometryData.m_Size.y = math.max(objectGeometryData.m_Size.y, 5f);
      objectGeometryData.m_Bounds.min.xz = float2_2 * -0.5f;
      objectGeometryData.m_Bounds.min.y = math.min(objectGeometryData.m_Bounds.min.y, 0.0f);
      objectGeometryData.m_Bounds.max.xz = float2_2 * 0.5f;
      objectGeometryData.m_Bounds.max.y = math.max(objectGeometryData.m_Bounds.max.y, 5f);
    }

    private static MeshGroupFlags GetMeshGroupFlag(ObjectState state, bool inverse)
    {
      if (inverse)
      {
        switch (state)
        {
          case ObjectState.Cold:
            return MeshGroupFlags.RequireWarm;
          case ObjectState.Warm:
            return MeshGroupFlags.RequireCold;
          case ObjectState.Home:
            return MeshGroupFlags.RequireHomeless;
          case ObjectState.Homeless:
            return MeshGroupFlags.RequireHome;
          case ObjectState.Motorcycle:
            return MeshGroupFlags.ForbidMotorcycle;
        }
      }
      else
      {
        switch (state)
        {
          case ObjectState.Cold:
            return MeshGroupFlags.RequireCold;
          case ObjectState.Warm:
            return MeshGroupFlags.RequireWarm;
          case ObjectState.Home:
            return MeshGroupFlags.RequireHome;
          case ObjectState.Homeless:
            return MeshGroupFlags.RequireHomeless;
          case ObjectState.Motorcycle:
            return MeshGroupFlags.RequireMotorcycle;
        }
      }
      return (MeshGroupFlags) 0;
    }

    private void InitializePrefab(
      ObjectGeometryPrefab objectPrefab,
      PlaceableObjectData placeableObjectData,
      ref ObjectGeometryData objectGeometryData,
      ref GrowthScaleData growthScaleData,
      ref StackData stackData,
      ref QuantityObjectData quantityObjectData,
      ref CreatureData creatureData,
      DynamicBuffer<SubMesh> meshes,
      DynamicBuffer<SubMeshGroup> meshGroups,
      DynamicBuffer<CharacterElement> characterElements,
      bool isPlantObject,
      bool isHumanObject,
      bool isBuildingObject,
      bool isVehicleObject,
      bool isCreatureObject)
    {
      Bounds3 bounds1 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
      bool flag1 = false;
      Bounds3 bounds2 = new Bounds3();
      Bounds3 bounds3 = new Bounds3();
      Bounds3 bounds4 = new Bounds3();
      Bounds3 bounds5 = new Bounds3();
      Bounds3 bounds6 = new Bounds3();
      if (objectPrefab.m_Meshes != null)
      {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int length = objectPrefab.m_Meshes.Length;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 0;
        ObjectMeshInfo objectMeshInfo = (ObjectMeshInfo) null;
        CharacterGroup.Character[] characterArray = (CharacterGroup.Character[]) null;
        CharacterGroup.OverrideInfo[] overrideInfoArray = (CharacterGroup.OverrideInfo[]) null;
        CharacterGroup.OverrideInfo overrideInfo1 = (CharacterGroup.OverrideInfo) null;
        RenderPrefab[] renderPrefabArray = (RenderPrefab[]) null;
        List<RenderPrefab> renderPrefabList = (List<RenderPrefab>) null;
        MeshGroupFlags meshGroupFlags1 = (MeshGroupFlags) 0;
        MeshGroupFlags meshGroupFlags2 = (MeshGroupFlags) 0;
        Entity entity1 = Entity.Null;
label_2:
        RenderPrefab prefab1;
        EntityManager entityManager;
        MeshData componentData1;
        bool flag2;
        LodProperties component1;
        do
        {
          prefab1 = (RenderPrefab) null;
          while (num4 >= num8)
          {
            if (num2 < num5)
            {
              CharacterGroup.Character character1 = characterArray[num2++];
              if ((character1.m_Style.m_Gender & creatureData.m_Gender) == creatureData.m_Gender)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                entity1 = this.m_PrefabSystem.GetEntity((PrefabBase) character1.m_Style);
                num4 = 0;
                CharacterElement characterElement = new CharacterElement();
                characterElement.m_Style = entity1;
                characterElement.m_ShapeWeights = RenderingUtils.GetBlendWeights(character1.m_Meta.shapeWeights);
                characterElement.m_TextureWeights = RenderingUtils.GetBlendWeights(character1.m_Meta.textureWeights);
                characterElement.m_OverlayWeights = RenderingUtils.GetBlendWeights(character1.m_Meta.overlayWeights);
                characterElement.m_MaskWeights = RenderingUtils.GetBlendWeights(character1.m_Meta.maskWeights);
                ref CharacterElement local = ref characterElement;
                entityManager = this.EntityManager;
                int restPoseClipIndex = entityManager.GetComponentData<CharacterStyleData>(entity1).m_RestPoseClipIndex;
                local.m_RestPoseClipIndex = restPoseClipIndex;
                characterElement.m_CorrectiveClipIndex = -1;
                CharacterElement elem = characterElement;
                if (overrideInfo1 != null)
                {
                  CharacterGroup.Character character2 = overrideInfo1.m_Group.m_Characters[num2 - 1];
                  if (overrideInfo1.m_OverrideShapeWeights)
                    elem.m_ShapeWeights = RenderingUtils.GetBlendWeights(character2.m_Meta.shapeWeights);
                  if (renderPrefabList == null)
                    renderPrefabList = new List<RenderPrefab>();
                  else
                    renderPrefabList.Clear();
                  for (int index = 0; index < character1.m_MeshPrefabs.Length; ++index)
                  {
                    RenderPrefab meshPrefab = character1.m_MeshPrefabs[index];
                    CharacterProperties component2;
                    if (!meshPrefab.TryGet<CharacterProperties>(out component2) || (component2.m_BodyParts & overrideInfo1.m_OverrideBodyParts) == (CharacterProperties.BodyPart) 0)
                      renderPrefabList.Add(meshPrefab);
                  }
                  for (int index = 0; index < character2.m_MeshPrefabs.Length; ++index)
                  {
                    RenderPrefab meshPrefab = character2.m_MeshPrefabs[index];
                    CharacterProperties component3;
                    if (!meshPrefab.TryGet<CharacterProperties>(out component3) || (component3.m_BodyParts & overrideInfo1.m_OverrideBodyParts) != (CharacterProperties.BodyPart) 0)
                      renderPrefabList.Add(meshPrefab);
                  }
                  renderPrefabArray = (RenderPrefab[]) null;
                  num8 = renderPrefabList.Count;
                }
                else
                {
                  renderPrefabArray = character1.m_MeshPrefabs;
                  num8 = renderPrefabArray.Length;
                }
                // ISSUE: reference to a compiler-generated method
                meshGroups.Add(new SubMeshGroup()
                {
                  m_SubGroupCount = num7,
                  m_SubMeshRange = new int2(meshes.Length, meshes.Length + num8),
                  m_Flags = meshGroupFlags2 | ObjectInitializeSystem.GetMeshGroupFlag(objectMeshInfo.m_RequireState, false)
                });
                characterElements.Add(elem);
              }
            }
            else if (num3 < num6)
            {
              overrideInfo1 = overrideInfoArray[num3++];
              num2 = 0;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              meshGroupFlags2 = meshGroupFlags1 & ~ObjectInitializeSystem.GetMeshGroupFlag(overrideInfo1.m_RequireState, true) | ObjectInitializeSystem.GetMeshGroupFlag(overrideInfo1.m_RequireState, false);
            }
            else if (num1 < length)
            {
              objectMeshInfo = objectPrefab.m_Meshes[num1++];
              characterArray = (CharacterGroup.Character[]) null;
              if (objectMeshInfo.m_Mesh is RenderPrefab mesh2)
              {
                prefab1 = mesh2;
                entity1 = Entity.Null;
                goto label_41;
              }
              else if (objectMeshInfo.m_Mesh is CharacterGroup mesh1)
              {
                characterArray = mesh1.m_Characters;
                overrideInfoArray = mesh1.m_Overrides;
                overrideInfo1 = (CharacterGroup.OverrideInfo) null;
                num2 = 0;
                num5 = characterArray.Length;
                num3 = 0;
                num6 = overrideInfoArray != null ? overrideInfoArray.Length : 0;
                num7 = 0;
                foreach (CharacterGroup.Character character in characterArray)
                {
                  if ((character.m_Style.m_Gender & creatureData.m_Gender) == creatureData.m_Gender)
                    ++num7;
                }
                meshGroupFlags1 = (MeshGroupFlags) 0;
                foreach (CharacterGroup.OverrideInfo overrideInfo2 in overrideInfoArray)
                {
                  // ISSUE: reference to a compiler-generated method
                  meshGroupFlags1 |= ObjectInitializeSystem.GetMeshGroupFlag(overrideInfo2.m_RequireState, true);
                }
                meshGroupFlags2 = meshGroupFlags1;
              }
            }
            else
              goto label_41;
          }
          prefab1 = renderPrefabArray == null ? renderPrefabList[num4++] : renderPrefabArray[num4++];
label_41:
          if (!((UnityEngine.Object) prefab1 == (UnityEngine.Object) null))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            Entity entity2 = this.m_PrefabSystem.GetEntity((PrefabBase) prefab1);
            entityManager = this.EntityManager;
            componentData1 = entityManager.GetComponentData<MeshData>(entity2);
            Bounds3 meshBounds = prefab1.bounds;
            float3 float3 = MathUtils.Size(meshBounds);
            if (objectPrefab.m_Circular || objectMeshInfo.m_Rotation.Equals(quaternion.identity))
              meshBounds += objectMeshInfo.m_Position;
            else
              meshBounds = MathUtils.Bounds(MathUtils.Box(meshBounds, objectMeshInfo.m_Rotation, objectMeshInfo.m_Position));
            SubMeshFlags flags = (SubMeshFlags) 0;
            switch (objectMeshInfo.m_RequireState)
            {
              case ObjectState.Child:
                flags |= SubMeshFlags.RequireChild;
                bounds2 |= meshBounds;
                break;
              case ObjectState.Teen:
                flags |= SubMeshFlags.RequireTeen;
                bounds3 |= meshBounds;
                break;
              case ObjectState.Adult:
                flags |= SubMeshFlags.RequireAdult;
                bounds4 |= meshBounds;
                break;
              case ObjectState.Elderly:
                flags |= SubMeshFlags.RequireElderly;
                bounds5 |= meshBounds;
                break;
              case ObjectState.Dead:
                flags |= SubMeshFlags.RequireDead;
                bounds6 |= meshBounds;
                break;
              case ObjectState.Stump:
                flags |= SubMeshFlags.RequireStump;
                break;
              case ObjectState.Empty:
                flags |= SubMeshFlags.RequireEmpty;
                quantityObjectData.m_StepMask |= 1U;
                break;
              case ObjectState.Full:
                flags |= SubMeshFlags.RequireFull;
                quantityObjectData.m_StepMask |= 8U;
                break;
              case ObjectState.Clear:
                flags |= SubMeshFlags.RequireClear;
                break;
              case ObjectState.Track:
                flags |= SubMeshFlags.RequireTrack;
                break;
              case ObjectState.Partial1:
                flags |= SubMeshFlags.RequirePartial1;
                quantityObjectData.m_StepMask |= 2U;
                break;
              case ObjectState.Partial2:
                flags |= SubMeshFlags.RequirePartial2;
                quantityObjectData.m_StepMask |= 4U;
                break;
              case ObjectState.LefthandTraffic:
                flags |= SubMeshFlags.RequireLeftHandTraffic;
                break;
              case ObjectState.RighthandTraffic:
                flags |= SubMeshFlags.RequireRightHandTraffic;
                break;
              case ObjectState.Forward:
                flags |= SubMeshFlags.RequireForward;
                break;
              case ObjectState.Backward:
                flags |= SubMeshFlags.RequireBackward;
                break;
              case ObjectState.Outline:
                flags |= SubMeshFlags.OutlineOnly;
                break;
            }
            float renderingSize;
            float metersPerPixel;
            if ((componentData1.m_State & (MeshFlags.StackX | MeshFlags.StackY | MeshFlags.StackZ)) != (MeshFlags) 0)
            {
              StackProperties component4 = prefab1.GetComponent<StackProperties>();
              renderingSize = RenderingUtils.GetRenderingSize(float3, component4.m_Direction);
              metersPerPixel = RenderingUtils.GetShadowRenderingSize(float3, component4.m_Direction);
              if (stackData.m_Direction != StackDirection.None && stackData.m_Direction != component4.m_Direction || component4.m_Direction == StackDirection.None)
                COSystemBase.baseLog.WarnFormat((UnityEngine.Object) objectPrefab, "{0}: Stack direction mismatch ({1})", (object) objectPrefab.name, (object) prefab1.name);
              else
                stackData.m_Direction = component4.m_Direction;
              switch (component4.m_Order)
              {
                case StackOrder.First:
                  flags |= SubMeshFlags.IsStackStart;
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateStackBounds(ref stackData.m_FirstBounds, ref meshBounds, component4);
                  break;
                case StackOrder.Middle:
                  flags |= SubMeshFlags.IsStackMiddle;
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateStackBounds(ref stackData.m_MiddleBounds, ref meshBounds, component4);
                  break;
                case StackOrder.Last:
                  flags |= SubMeshFlags.IsStackEnd;
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateStackBounds(ref stackData.m_LastBounds, ref meshBounds, component4);
                  break;
              }
            }
            else
            {
              if ((double) prefab1.surfaceArea <= 0.0)
              {
                if (isHumanObject)
                  float3.x /= 3f;
                else if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
                  float3.xz = math.lerp(float3.xz, math.min(float3.xz, objectGeometryData.m_LegSize.xz), math.saturate(objectGeometryData.m_LegSize.y / math.max(1f / 1000f, float3.y)));
              }
              renderingSize = RenderingUtils.GetRenderingSize(math.min(float3, math.max(math.min(float3.yzx, float3.zxy) * 8f, math.max(float3.yzx, float3.zxy) * 4f)));
              metersPerPixel = renderingSize;
            }
            componentData1.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(renderingSize, componentData1.m_LodBias);
            componentData1.m_ShadowLod = (byte) RenderingUtils.CalculateLodLimit(metersPerPixel, componentData1.m_ShadowBias);
            MeshLayer meshLayer = componentData1.m_DefaultLayers == (MeshLayer) 0 ? MeshLayer.Default : componentData1.m_DefaultLayers;
            objectGeometryData.m_Layers |= meshLayer;
            if (!objectMeshInfo.m_Position.Equals(new float3()) || !objectMeshInfo.m_Rotation.Equals(quaternion.identity))
              flags |= SubMeshFlags.HasTransform;
            ushort randomSeed = (ushort) num4;
            if (characterArray != null)
            {
              // ISSUE: reference to a compiler-generated method
              randomSeed = this.GetRandomSeed(prefab1.name);
            }
            meshes.Add(new SubMesh(entity2, objectMeshInfo.m_Position, objectMeshInfo.m_Rotation, flags, randomSeed));
            flag1 |= (componentData1.m_State & MeshFlags.Decal) == (MeshFlags) 0;
            if (characterArray == null || num4 == 1)
            {
              bounds1 |= meshBounds;
              objectGeometryData.m_MinLod = math.min(objectGeometryData.m_MinLod, (int) componentData1.m_MinLod);
            }
            else
              componentData1.m_MinLod = (byte) math.max((int) componentData1.m_MinLod, objectGeometryData.m_MinLod);
            DynamicBuffer<AnimationClip> buffer;
            if (this.EntityManager.TryGetBuffer<AnimationClip>(entity2, false, out buffer))
            {
              float num9 = float.MaxValue;
              float num10 = 0.0f;
              for (int index = 0; index < buffer.Length; ++index)
              {
                AnimationClip animationClip = buffer[index];
                if (animationClip.m_Type == AnimationType.Move)
                {
                  switch (animationClip.m_Activity)
                  {
                    case ActivityType.Walking:
                      num9 = animationClip.m_MovementSpeed;
                      break;
                    case ActivityType.Running:
                      num10 = animationClip.m_MovementSpeed;
                      break;
                  }
                }
                creatureData.m_SupportedActivities.m_Mask |= new ActivityMask(animationClip.m_Activity).m_Mask;
              }
              for (int index = 0; index < buffer.Length; ++index)
              {
                AnimationClip animationClip = buffer[index];
                if (animationClip.m_Type == AnimationType.Move)
                {
                  animationClip.m_SpeedRange = new Bounds1(0.0f, float.MaxValue);
                  switch (animationClip.m_Activity)
                  {
                    case ActivityType.Walking:
                      animationClip.m_SpeedRange.max = math.select((float) (((double) num9 + (double) num10) * 0.5), float.MaxValue, (double) num10 <= (double) num9);
                      break;
                    case ActivityType.Running:
                      animationClip.m_SpeedRange.min = math.select((float) (((double) num9 + (double) num10) * 0.5), 0.0f, (double) num9 >= (double) num10);
                      break;
                  }
                  buffer[index] = animationClip;
                }
              }
            }
            else
            {
              CharacterStyleData component5;
              if (this.EntityManager.TryGetComponent<CharacterStyleData>(entity1, out component5))
              {
                creatureData.m_SupportedActivities.m_Mask |= component5.m_ActivityMask.m_Mask;
                CharacterProperties component6;
                if (prefab1.TryGet<CharacterProperties>(out component6) && !string.IsNullOrEmpty(component6.m_CorrectiveAnimationName))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  CharacterStyle prefab2 = this.m_PrefabSystem.GetPrefab<CharacterStyle>(entity1);
                  ref CharacterElement local = ref characterElements.ElementAt(characterElements.Length - 1);
                  for (int index = 0; index < prefab2.m_Animations.Length; ++index)
                  {
                    if (prefab2.m_Animations[index].name == component6.m_CorrectiveAnimationName)
                    {
                      local.m_CorrectiveClipIndex = index;
                      break;
                    }
                  }
                }
              }
            }
            if (isBuildingObject)
              componentData1.m_DecalLayer |= DecalLayers.Buildings;
            if (isVehicleObject)
              componentData1.m_DecalLayer |= DecalLayers.Vehicles;
            if (isCreatureObject)
              componentData1.m_DecalLayer |= DecalLayers.Creatures;
            BaseProperties component7;
            flag2 = isBuildingObject && (!prefab1.TryGet<BaseProperties>(out component7) || (UnityEngine.Object) component7.m_BaseType != (UnityEngine.Object) null);
            if (flag2 && (componentData1.m_State & (MeshFlags.Decal | MeshFlags.Impostor)) == (MeshFlags) 0)
              componentData1.m_State |= MeshFlags.Base;
            if ((componentData1.m_State & MeshFlags.Base) != (MeshFlags) 0)
              objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.HasBase;
            if (characterArray != null)
              componentData1.m_State |= MeshFlags.Character;
            entityManager = this.EntityManager;
            entityManager.SetComponentData<MeshData>(entity2, componentData1);
          }
          else
            goto label_138;
        }
        while (!(isBuildingObject | isVehicleObject | isCreatureObject) || !prefab1.TryGet<LodProperties>(out component1) || component1.m_LodMeshes == null);
        for (int index = 0; index < component1.m_LodMeshes.Length; ++index)
        {
          if (!((UnityEngine.Object) component1.m_LodMeshes[index] == (UnityEngine.Object) null))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            Entity entity3 = this.m_PrefabSystem.GetEntity((PrefabBase) component1.m_LodMeshes[index]);
            entityManager = this.EntityManager;
            MeshData componentData2 = entityManager.GetComponentData<MeshData>(entity3);
            if (isBuildingObject)
              componentData2.m_DecalLayer |= DecalLayers.Buildings;
            if (isVehicleObject)
              componentData2.m_DecalLayer |= DecalLayers.Vehicles;
            if (isCreatureObject)
              componentData2.m_DecalLayer |= DecalLayers.Creatures;
            if (flag2 && (componentData2.m_State & (MeshFlags.Decal | MeshFlags.Impostor)) == (MeshFlags) 0)
              componentData2.m_State |= componentData1.m_State & MeshFlags.Base;
            if (characterArray != null)
              componentData2.m_State |= MeshFlags.Character;
            entityManager = this.EntityManager;
            entityManager.SetComponentData<MeshData>(entity3, componentData2);
          }
        }
        goto label_2;
      }
label_138:
      if ((double) bounds1.min.x > (double) bounds1.max.x)
        bounds1 = new Bounds3();
      objectGeometryData.m_Bounds = bounds1;
      objectGeometryData.m_Size = ObjectUtils.GetSize(bounds1);
      if (isPlantObject)
      {
        float x = 0.5f;
        PlantObject component;
        if (objectPrefab.TryGet<PlantObject>(out component))
          x = math.min(x, 1f - component.m_PotCoverage);
        float3 float3 = new float3()
        {
          xz = objectGeometryData.m_Size.xz * x
        };
        float3.y = math.min(objectGeometryData.m_Size.y * x, math.cmin(float3.xz) * 0.25f);
        objectGeometryData.m_Size -= float3;
        objectGeometryData.m_Bounds.min.xz = math.max(objectGeometryData.m_Bounds.min.xz, objectGeometryData.m_Size.xz * -0.5f);
        objectGeometryData.m_Bounds.max.xz = math.min(objectGeometryData.m_Bounds.max.xz, objectGeometryData.m_Size.xz * 0.5f);
        objectGeometryData.m_Bounds.max.y = objectGeometryData.m_Size.y;
      }
      else if (isHumanObject)
      {
        objectGeometryData.m_Size.x = math.max(objectGeometryData.m_Size.z, objectGeometryData.m_Size.x / 3f);
        objectGeometryData.m_Bounds.min.x = objectGeometryData.m_Size.x * -0.5f;
        objectGeometryData.m_Bounds.max.x = objectGeometryData.m_Size.x * 0.5f;
      }
      objectGeometryData.m_Pivot = (placeableObjectData.m_Flags & Game.Objects.PlacementFlags.Wall) == Game.Objects.PlacementFlags.None ? ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.Hanging) == Game.Objects.PlacementFlags.None ? new float3(0.0f, math.lerp(bounds1.min.y, bounds1.max.y, 0.25f), 0.0f) : new float3(0.0f, math.lerp(bounds1.min.y, bounds1.max.y, 0.9f), 0.0f)) : new float3();
      if (objectPrefab.m_Circular)
      {
        objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.Circular;
        objectGeometryData.m_Size.xz = (float2) math.max(objectGeometryData.m_Size.x, objectGeometryData.m_Size.z);
      }
      if (flag1)
        objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.Physical;
      else
        objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.WalkThrough;
      growthScaleData.m_ChildSize = ObjectUtils.GetSize(bounds2);
      growthScaleData.m_TeenSize = ObjectUtils.GetSize(bounds3);
      growthScaleData.m_AdultSize = ObjectUtils.GetSize(bounds4);
      growthScaleData.m_ElderlySize = ObjectUtils.GetSize(bounds5);
      growthScaleData.m_DeadSize = ObjectUtils.GetSize(bounds6);
      if (!meshGroups.IsCreated)
        return;
      MeshGroupFlags meshGroupFlags = (MeshGroupFlags) 0;
      for (int index = 0; index < meshGroups.Length; ++index)
        meshGroupFlags |= meshGroups[index].m_Flags;
      if ((meshGroupFlags & (MeshGroupFlags.RequireHome | MeshGroupFlags.RequireHomeless)) != MeshGroupFlags.RequireHomeless)
        return;
      for (int index = 0; index < meshGroups.Length; ++index)
      {
        ref SubMeshGroup local = ref meshGroups.ElementAt(index);
        if ((local.m_Flags & (MeshGroupFlags.RequireCold | MeshGroupFlags.RequireWarm)) != (MeshGroupFlags) 0)
          local.m_Flags |= MeshGroupFlags.RequireHome;
      }
    }

    private ushort GetRandomSeed(string name)
    {
      uint randomSeed = 0;
      for (int index = 0; index < name.Length; ++index)
        randomSeed = randomSeed << 1 ^ (uint) name[index];
      return (ushort) randomSeed;
    }

    private void UpdateStackBounds(
      ref Bounds1 stackBounds,
      ref Bounds3 meshBounds,
      StackProperties properties)
    {
      switch (properties.m_Direction)
      {
        case StackDirection.Right:
          stackBounds |= new Bounds1(meshBounds.min.x + properties.m_StartOverlap, meshBounds.max.x - properties.m_EndOverlap);
          meshBounds.min.x = properties.m_Order == StackOrder.First ? math.min(meshBounds.min.x, 0.0f) : 0.0f;
          meshBounds.max.x = properties.m_Order == StackOrder.Last ? math.max(meshBounds.max.x, 0.0f) : 0.0f;
          break;
        case StackDirection.Up:
          stackBounds |= new Bounds1(meshBounds.min.y + properties.m_StartOverlap, meshBounds.max.y - properties.m_EndOverlap);
          meshBounds.min.y = properties.m_Order == StackOrder.First ? math.min(meshBounds.min.y, 0.0f) : 0.0f;
          meshBounds.max.y = properties.m_Order == StackOrder.Last ? math.max(meshBounds.max.y, 0.0f) : 0.0f;
          break;
        case StackDirection.Forward:
          stackBounds |= new Bounds1(meshBounds.min.z + properties.m_StartOverlap, meshBounds.max.z - properties.m_EndOverlap);
          meshBounds.min.z = properties.m_Order == StackOrder.First ? math.min(meshBounds.min.z, 0.0f) : 0.0f;
          meshBounds.max.z = properties.m_Order == StackOrder.Last ? math.max(meshBounds.max.z, 0.0f) : 0.0f;
          break;
      }
    }

    private void InitializePrefab(
      MarkerObjectPrefab objectPrefab,
      PlaceableObjectData placeableObjectData,
      ref ObjectGeometryData objectGeometryData,
      DynamicBuffer<SubMesh> meshes)
    {
      Bounds3 bounds1 = new Bounds3();
      if ((UnityEngine.Object) objectPrefab.m_Mesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) objectPrefab.m_Mesh);
        MeshData componentData = this.EntityManager.GetComponentData<MeshData>(entity);
        Bounds3 bounds2 = objectPrefab.m_Mesh.bounds;
        float renderingSize = RenderingUtils.GetRenderingSize(MathUtils.Size(bounds2));
        componentData.m_MinLod = (byte) RenderingUtils.CalculateLodLimit(renderingSize, componentData.m_LodBias);
        componentData.m_ShadowLod = (byte) RenderingUtils.CalculateLodLimit(renderingSize, componentData.m_ShadowBias);
        objectGeometryData.m_MinLod = math.min(objectGeometryData.m_MinLod, (int) componentData.m_MinLod);
        objectGeometryData.m_Layers = componentData.m_DefaultLayers == (MeshLayer) 0 ? MeshLayer.Default : componentData.m_DefaultLayers;
        bounds1 |= bounds2;
        this.EntityManager.SetComponentData<MeshData>(entity, componentData);
        meshes.Add(new SubMesh(entity, (SubMeshFlags) 0, (ushort) 0));
      }
      objectGeometryData.m_Bounds = bounds1;
      objectGeometryData.m_Size = ObjectUtils.GetSize(bounds1);
      objectGeometryData.m_Pivot = (placeableObjectData.m_Flags & Game.Objects.PlacementFlags.Wall) == Game.Objects.PlacementFlags.None ? ((placeableObjectData.m_Flags & Game.Objects.PlacementFlags.Hanging) == Game.Objects.PlacementFlags.None ? new float3(0.0f, math.lerp(bounds1.min.y, bounds1.max.y, 0.25f), 0.0f) : new float3(0.0f, math.lerp(bounds1.min.y, bounds1.max.y, 0.9f), 0.0f)) : new float3();
      if (objectPrefab.m_Circular)
      {
        objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.Circular;
        objectGeometryData.m_Size.xz = (float2) math.max(objectGeometryData.m_Size.x, objectGeometryData.m_Size.z);
      }
      objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.Marker;
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
    public ObjectInitializeSystem()
    {
    }

    [BurstCompile]
    private struct FixPlaceholdersJob : IJobChunk
    {
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      public BufferTypeHandle<PlaceholderObjectElement> m_PlaceholderObjectElementType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PlaceholderObjectElement> bufferAccessor = chunk.GetBufferAccessor<PlaceholderObjectElement>(ref this.m_PlaceholderObjectElementType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<PlaceholderObjectElement> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeletedData.HasComponent(dynamicBuffer[index2].m_Object))
              dynamicBuffer.RemoveAtSwapBack(index2--);
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

    [BurstCompile]
    private struct InitializeSubNetsJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      public ComponentTypeHandle<PlaceableObjectData> m_PlaceableObjectDataType;
      public BufferTypeHandle<SubNet> m_SubNetType;
      [ReadOnly]
      public ComponentLookup<NetData> m_NetData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<PlaceableObjectData> nativeArray = chunk.GetNativeArray<PlaceableObjectData>(ref this.m_PlaceableObjectDataType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubNet> bufferAccessor = chunk.GetBufferAccessor<SubNet>(ref this.m_SubNetType);
        NativeList<int> nativeList = new NativeList<int>();
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<SubNet> dynamicBuffer = bufferAccessor[index1];
          Game.Objects.PlacementFlags placementFlags = Game.Objects.PlacementFlags.None;
          if (dynamicBuffer.Length != 0)
          {
            if (!nativeList.IsCreated)
              nativeList = new NativeList<int>(dynamicBuffer.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              ref SubNet local = ref dynamicBuffer.ElementAt(index2);
              if (local.m_NodeIndex.x >= 0)
              {
                while (nativeList.Length <= local.m_NodeIndex.x)
                  nativeList.Add(0);
                nativeList[local.m_NodeIndex.x]++;
              }
              if (local.m_NodeIndex.y >= 0 && local.m_NodeIndex.y != local.m_NodeIndex.x)
              {
                while (nativeList.Length <= local.m_NodeIndex.y)
                  nativeList.Add(0);
                nativeList[local.m_NodeIndex.y]++;
              }
            }
            for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
            {
              ref SubNet local = ref dynamicBuffer.ElementAt(index3);
              // ISSUE: reference to a compiler-generated method
              local.m_Snapping.x = local.m_NodeIndex.x >= 0 && nativeList[local.m_NodeIndex.x] == 1 && this.GetEnableSnapping(local.m_Prefab);
              // ISSUE: reference to a compiler-generated method
              local.m_Snapping.y = local.m_NodeIndex.y >= 0 && local.m_NodeIndex.y != local.m_NodeIndex.x && nativeList[local.m_NodeIndex.y] == 1 && this.GetEnableSnapping(local.m_Prefab);
              if (math.any(local.m_Snapping))
                placementFlags |= Game.Objects.PlacementFlags.SubNetSnap;
            }
            nativeList.Clear();
          }
          if (nativeArray.Length != 0)
            nativeArray.ElementAt<PlaceableObjectData>(index1).m_Flags |= placementFlags;
        }
        if (!nativeList.IsCreated)
          return;
        nativeList.Dispose();
      }

      private bool GetEnableSnapping(Entity prefab)
      {
        NetData componentData;
        // ISSUE: reference to a compiler-generated field
        return this.m_NetData.TryGetComponent(prefab, out componentData) && (componentData.m_RequiredLayers & (Layer.MarkerPathway | Layer.MarkerTaxiway)) == Layer.None;
      }
    }

    [BurstCompile]
    private struct FindPlaceholderRequirementsJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public BufferTypeHandle<PlaceholderObjectElement> m_PlaceholderObjectElementType;
      public ComponentTypeHandle<PlaceholderObjectData> m_PlaceholderObjectDataType;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_ObjectRequirementElements;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<PlaceholderObjectData> nativeArray = chunk.GetNativeArray<PlaceholderObjectData>(ref this.m_PlaceholderObjectDataType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PlaceholderObjectElement> bufferAccessor = chunk.GetBufferAccessor<PlaceholderObjectElement>(ref this.m_PlaceholderObjectElementType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          ref PlaceholderObjectData local = ref nativeArray.ElementAt<PlaceholderObjectData>(index1);
          DynamicBuffer<PlaceholderObjectElement> dynamicBuffer = bufferAccessor[index1];
          ObjectRequirementFlags requirementFlags = (ObjectRequirementFlags) 0;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            DynamicBuffer<ObjectRequirementElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectRequirementElements.TryGetBuffer(dynamicBuffer[index2].m_Object, out bufferData))
            {
              for (int index3 = 0; index3 < bufferData.Length; ++index3)
              {
                ObjectRequirementElement requirementElement = bufferData[index3];
                requirementFlags = requirementFlags | requirementElement.m_RequireFlags | requirementElement.m_ForbidFlags;
              }
            }
          }
          local.m_RequirementMask = requirementFlags;
        }
      }
    }

    [BurstCompile]
    private struct FindSubObjectRequirementsJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      public ComponentTypeHandle<ObjectGeometryData> m_ObjectGeometryDataType;
      [ReadOnly]
      public ComponentLookup<PlaceholderObjectData> m_PlaceholderObjectData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_Chunks[index];
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<ObjectGeometryData> nativeArray = chunk.GetNativeArray<ObjectGeometryData>(ref this.m_ObjectGeometryDataType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubObject> bufferAccessor = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated method
          nativeArray.ElementAt<ObjectGeometryData>(index1).m_SubObjectMask = this.GetRequirementMask(bufferAccessor[index1]);
        }
      }

      private ObjectRequirementFlags GetRequirementMask(DynamicBuffer<SubObject> subObjects)
      {
        ObjectRequirementFlags requirementMask = (ObjectRequirementFlags) 0;
        for (int index = 0; index < subObjects.Length; ++index)
        {
          SubObject subObject = subObjects[index];
          PlaceholderObjectData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceholderObjectData.TryGetComponent(subObject.m_Prefab, out componentData))
          {
            requirementMask |= componentData.m_RequirementMask;
          }
          else
          {
            DynamicBuffer<SubObject> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.TryGetBuffer(subObject.m_Prefab, out bufferData))
            {
              // ISSUE: reference to a compiler-generated method
              requirementMask |= this.GetRequirementMask(bufferData);
            }
          }
        }
        return requirementMask;
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
      [ReadOnly]
      public ComponentTypeHandle<UtilityObjectData> __Game_Prefabs_UtilityObjectData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PillarData> __Game_Prefabs_PillarData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetObjectData> __Game_Prefabs_NetObjectData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PlantData> __Game_Prefabs_PlantData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HumanData> __Game_Prefabs_HumanData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<VehicleData> __Game_Prefabs_VehicleData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AssetStampData> __Game_Prefabs_AssetStampData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<GrowthScaleData> __Game_Prefabs_GrowthScaleData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<StackData> __Game_Prefabs_StackData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<QuantityObjectData> __Game_Prefabs_QuantityObjectData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CreatureData> __Game_Prefabs_CreatureData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<BuildingTerraformData> __Game_Prefabs_BuildingTerraformData_RW_ComponentTypeHandle;
      public BufferTypeHandle<SubMesh> __Game_Prefabs_SubMesh_RW_BufferTypeHandle;
      public BufferTypeHandle<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RW_BufferTypeHandle;
      public BufferTypeHandle<CharacterElement> __Game_Prefabs_CharacterElement_RW_BufferTypeHandle;
      public BufferTypeHandle<SubObject> __Game_Prefabs_SubObject_RW_BufferTypeHandle;
      public BufferTypeHandle<SubNet> __Game_Prefabs_SubNet_RW_BufferTypeHandle;
      public BufferTypeHandle<SubLane> __Game_Prefabs_SubLane_RW_BufferTypeHandle;
      public BufferTypeHandle<SubArea> __Game_Prefabs_SubArea_RW_BufferTypeHandle;
      public BufferTypeHandle<SubAreaNode> __Game_Prefabs_SubAreaNode_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      public BufferTypeHandle<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<PlaceholderObjectData> __Game_Prefabs_PlaceholderObjectData_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Prefabs_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PlaceholderObjectData> __Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Prefabs_SubObject_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;

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
        this.__Game_Prefabs_UtilityObjectData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UtilityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PillarData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PillarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetObjectData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlantData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PlantData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HumanData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HumanData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<VehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingExtensionData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectGeometryData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PlaceableObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnableObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AssetStampData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AssetStampData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GrowthScaleData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<GrowthScaleData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<StackData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_QuantityObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<QuantityObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CreatureData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingTerraformData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BuildingTerraformData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubMesh>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubMeshGroup>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<CharacterElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubNet>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubArea>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubAreaNode_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubAreaNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PlaceholderObjectElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PlaceholderObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
      }
    }
  }
}
