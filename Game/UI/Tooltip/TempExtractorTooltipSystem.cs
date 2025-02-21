// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TempExtractorTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class TempExtractorTooltipSystem : TooltipSystemBase
  {
    private NaturalResourceSystem m_NaturalResourceSystem;
    private ResourceSystem m_ResourceSystem;
    private CitySystem m_CitySystem;
    private IndustrialDemandSystem m_IndustrialDemandSystem;
    private CountCompanyDataSystem m_CountCompanyDataSystem;
    private ClimateSystem m_ClimateSystem;
    private PrefabSystem m_PrefabSystem;
    private Game.Objects.SearchSystem m_SearchSystem;
    private EntityQuery m_ErrorQuery;
    private EntityQuery m_TempQuery;
    private EntityQuery m_ExtractorParameterQuery;
    private StringTooltip m_ResourceAvailable;
    private StringTooltip m_ResourceUnavailable;
    private IntTooltip m_Surplus;
    private IntTooltip m_Deficit;
    private StringTooltip m_ClimateAvailable;
    private StringTooltip m_ClimateUnavailable;
    private TempExtractorTooltipSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NaturalResourceSystem = this.World.GetOrCreateSystemManaged<NaturalResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandSystem = this.World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountCompanyDataSystem = this.World.GetOrCreateSystemManaged<CountCompanyDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ErrorQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Error>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Placeholder>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExtractorParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ExtractorParameterData>());
      StringTooltip stringTooltip1 = new StringTooltip();
      stringTooltip1.path = (PathSegment) "extractorMapFeatureAvailable";
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceAvailable = stringTooltip1;
      StringTooltip stringTooltip2 = new StringTooltip();
      stringTooltip2.path = (PathSegment) "extractorMapFeatureUnavailable";
      stringTooltip2.color = TooltipColor.Warning;
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceUnavailable = stringTooltip2;
      StringTooltip stringTooltip3 = new StringTooltip();
      stringTooltip3.path = (PathSegment) "extractorClimateAvailable";
      stringTooltip3.value = LocalizedString.Id("Tools.EXTRACTOR_CLIMATE_REQUIRED_AVAILABLE");
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateAvailable = stringTooltip3;
      StringTooltip stringTooltip4 = new StringTooltip();
      stringTooltip4.path = (PathSegment) "extractorClimateUnavailable";
      stringTooltip4.value = LocalizedString.Id("Tools.EXTRACTOR_CLIMATE_REQUIRED_UNAVAILABLE");
      stringTooltip4.color = TooltipColor.Warning;
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateUnavailable = stringTooltip4;
      IntTooltip intTooltip1 = new IntTooltip();
      intTooltip1.path = (PathSegment) "extractorCityProductionSurplus";
      intTooltip1.label = LocalizedString.Id("Tools.EXTRACTOR_PRODUCTION_SURPLUS");
      intTooltip1.unit = "weightPerMonth";
      // ISSUE: reference to a compiler-generated field
      this.m_Surplus = intTooltip1;
      IntTooltip intTooltip2 = new IntTooltip();
      intTooltip2.path = (PathSegment) "extractorCityProductionDeficit";
      intTooltip2.label = LocalizedString.Id("Tools.EXTRACTOR_PRODUCTION_DEFICIT");
      intTooltip2.unit = "weightPerMonth";
      // ISSUE: reference to a compiler-generated field
      this.m_Deficit = intTooltip2;
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    private bool FindWoodResource(Circle2 circle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      TempExtractorTooltipSystem.TreeIterator iterator = new TempExtractorTooltipSystem.TreeIterator()
      {
        m_OverriddenData = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabTreeData = this.__TypeHandle.__Game_Prefabs_TreeData_RO_ComponentLookup,
        m_Bounds = new Bounds2(circle.position - circle.radius, circle.position + circle.radius),
        m_Circle = circle,
        m_Result = 0.0f
      };
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, QuadTreeBoundsXZ> staticSearchTree = this.m_SearchSystem.GetStaticSearchTree(true, out dependencies);
      dependencies.Complete();
      staticSearchTree.Iterate<TempExtractorTooltipSystem.TreeIterator>(ref iterator);
      // ISSUE: reference to a compiler-generated field
      return (double) iterator.m_Result > 0.0;
    }

    private bool FindResource(
      Circle2 circle,
      MapFeature requiredFeature,
      CellMapData<NaturalResourceCell> resourceMap,
      DynamicBuffer<CityModifier> cityModifiers)
    {
      int2 cell1 = CellMapSystem<NaturalResourceCell>.GetCell(new float3(circle.position.x - circle.radius, 0.0f, circle.position.y - circle.radius), CellMapSystem<NaturalResourceCell>.kMapSize, resourceMap.m_TextureSize.x);
      int2 cell2 = CellMapSystem<NaturalResourceCell>.GetCell(new float3(circle.position.x + circle.radius, 0.0f, circle.position.y + circle.radius), CellMapSystem<NaturalResourceCell>.kMapSize, resourceMap.m_TextureSize.x);
      int2 int2_1 = math.max(new int2(0, 0), cell1);
      int2 int2_2 = math.min(new int2(resourceMap.m_TextureSize.x - 1, resourceMap.m_TextureSize.y - 1), cell2);
      int2 cell3;
      for (cell3.x = int2_1.x; cell3.x <= int2_2.x; ++cell3.x)
      {
        for (cell3.y = int2_1.y; cell3.y <= int2_2.y; ++cell3.y)
        {
          if (MathUtils.Intersect(circle, CellMapSystem<NaturalResourceCell>.GetCellCenter(cell3, resourceMap.m_TextureSize.x).xz))
          {
            NaturalResourceCell naturalResourceCell = resourceMap.m_Buffer[cell3.x + cell3.y * resourceMap.m_TextureSize.x];
            float num = 0.0f;
            switch (requiredFeature)
            {
              case MapFeature.FertileLand:
                num = (float) naturalResourceCell.m_Fertility.m_Base;
                num -= (float) naturalResourceCell.m_Fertility.m_Used;
                break;
              case MapFeature.Oil:
                num = (float) naturalResourceCell.m_Oil.m_Base;
                if (cityModifiers.IsCreated)
                  CityUtils.ApplyModifier(ref num, cityModifiers, CityModifierType.OilResourceAmount);
                num -= (float) naturalResourceCell.m_Oil.m_Used;
                break;
              case MapFeature.Ore:
                num = (float) naturalResourceCell.m_Ore.m_Base;
                if (cityModifiers.IsCreated)
                  CityUtils.ApplyModifier(ref num, cityModifiers, CityModifierType.OreResourceAmount);
                num -= (float) naturalResourceCell.m_Ore.m_Used;
                break;
              default:
                num = 0.0f;
                break;
            }
            if ((double) num > 0.0)
              return true;
          }
        }
      }
      return false;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      DynamicBuffer<CityModifier> buffer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ErrorQuery.IsEmptyIgnoreFilter || !this.EntityManager.TryGetBuffer<CityModifier>(this.m_CitySystem.City, true, out buffer))
        return;
      this.CompleteDependency();
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      CellMapData<NaturalResourceCell> data = this.m_NaturalResourceSystem.GetData(true, out dependencies);
      dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PlaceholderBuildingData> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<BuildingPropertyData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ExtractorAreaData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<LotData> roComponentLookup4 = this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup5 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Game.Prefabs.SubArea> areaRoBufferLookup = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      MapFeature mapFeature = MapFeature.None;
      bool flag1 = false;
      bool flag2 = false;
      Resource resource = Resource.NoResource;
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Transform> componentTypeHandle2 = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Temp> componentTypeHandle3 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
        dependencies.Complete();
        foreach (ArchetypeChunk archetypeChunk in archetypeChunkArray)
        {
          NativeArray<Temp> nativeArray1 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle3);
          NativeArray<PrefabRef> nativeArray2 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle1);
          NativeArray<Transform> nativeArray3 = archetypeChunk.GetNativeArray<Transform>(ref componentTypeHandle2);
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            if ((nativeArray1[index1].m_Flags & (TempFlags.Create | TempFlags.Upgrade)) != (TempFlags) 0)
            {
              Entity prefab1 = nativeArray2[index1].m_Prefab;
              if (roComponentLookup1.HasComponent(prefab1) && roComponentLookup2.HasComponent(prefab1) && roComponentLookup1[prefab1].m_Type == BuildingType.ExtractorBuilding)
              {
                resource = roComponentLookup2[prefab1].m_AllowedManufactured;
                DynamicBuffer<Game.Prefabs.SubArea> dynamicBuffer = areaRoBufferLookup[prefab1];
                for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
                {
                  Entity prefab2 = dynamicBuffer[index2].m_Prefab;
                  if (roComponentLookup3.HasComponent(prefab2) && roComponentLookup4.HasComponent(prefab2))
                  {
                    flag2 = true;
                    float maxRadius = roComponentLookup4[prefab2].m_MaxRadius;
                    // ISSUE: reference to a compiler-generated method
                    mapFeature = ExtractorCompanySystem.GetRequiredMapFeature(resource, prefab2, prefabs, roComponentLookup5, roComponentLookup3);
                    if (mapFeature != MapFeature.None)
                    {
                      float3 position = nativeArray3[index1].m_Position;
                      Circle2 circle = new Circle2(maxRadius, position.xz);
                      // ISSUE: reference to a compiler-generated method
                      // ISSUE: reference to a compiler-generated method
                      flag1 = mapFeature != MapFeature.Forest ? this.FindResource(circle, mapFeature, data, buffer) : this.FindWoodResource(circle);
                    }
                  }
                }
              }
            }
          }
        }
      }
      finally
      {
        archetypeChunkArray.Dispose();
      }
      if (!flag2)
        return;
      JobHandle deps1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> production = this.m_CountCompanyDataSystem.GetProduction(out deps1);
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> consumption = this.m_IndustrialDemandSystem.GetConsumption(out deps2);
      int resourceIndex = EconomyUtils.GetResourceIndex(resource);
      deps1.Complete();
      deps2.Complete();
      int num = production[resourceIndex] - consumption[resourceIndex];
      Entity entity = prefabs[resource];
      ResourceData resourceData = roComponentLookup5[entity];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      string icon = ImageSystem.GetIcon(this.m_PrefabSystem.GetPrefab<PrefabBase>(entity));
      if (num > 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Surplus.value = num;
        // ISSUE: reference to a compiler-generated field
        this.m_Surplus.icon = icon;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_Surplus);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Deficit.value = -num;
        // ISSUE: reference to a compiler-generated field
        this.m_Deficit.icon = icon;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_Deficit);
      }
      if (mapFeature != MapFeature.None)
      {
        string mapFeatureIconName = AreaTools.GetMapFeatureIconName(mapFeature);
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceAvailable.icon = "Media/Game/Icons/" + mapFeatureIconName + ".svg";
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceAvailable.value = LocalizedString.Id("Tools.EXTRACTOR_MAP_FEATURE_REQUIRED_AVAILABLE");
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_ResourceAvailable);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceUnavailable.icon = "Media/Game/Icons/" + mapFeatureIconName + ".svg";
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceUnavailable.value = LocalizedString.Id("Tools.EXTRACTOR_MAP_FEATURE_REQUIRED_MISSING");
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_ResourceUnavailable);
        }
      }
      if (!resourceData.m_RequireTemperature)
        return;
      // ISSUE: reference to a compiler-generated field
      if ((double) this.m_ClimateSystem.averageTemperature >= (double) resourceData.m_RequiredTemperature)
      {
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_ClimateAvailable);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_ClimateUnavailable);
      }
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

    [Preserve]
    public TempExtractorTooltipSystem()
    {
    }

    private struct TreeIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Bounds2 m_Bounds;
      public Circle2 m_Circle;
      public ComponentLookup<Overridden> m_OverriddenData;
      public ComponentLookup<Transform> m_TransformData;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<TreeData> m_PrefabTreeData;
      public float m_Result;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        // ISSUE: reference to a compiler-generated field
        return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_OverriddenData.HasComponent(entity) || !MathUtils.Intersect(this.m_Circle, this.m_TransformData[entity].m_Position.xz))
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabTreeData.HasComponent(prefabRef.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        TreeData treeData = this.m_PrefabTreeData[prefabRef.m_Prefab];
        if ((double) treeData.m_WoodAmount < 1.0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Result += treeData.m_WoodAmount;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Overridden> __Game_Common_Overridden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TreeData> __Game_Prefabs_TreeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> __Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LotData> __Game_Prefabs_LotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentLookup = state.GetComponentLookup<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TreeData_RO_ComponentLookup = state.GetComponentLookup<TreeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup = state.GetComponentLookup<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LotData_RO_ComponentLookup = state.GetComponentLookup<LotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
      }
    }
  }
}
