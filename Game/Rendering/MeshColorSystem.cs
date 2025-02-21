// Decompiled with JetBrains decompiler
// Type: Game.Rendering.MeshColorSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Objects;
using Game.Prefabs;
using Game.Prefabs.Climate;
using Game.Routes;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class MeshColorSystem : GameSystemBase
  {
    private ClimateSystem m_ClimateSystem;
    private SimulationSystem m_SimulationSystem;
    private PrefabSystem m_PrefabSystem;
    private RenderPrefabBase m_OverridePrefab;
    private Dictionary<string, int> m_GroupIDs;
    private EntityQuery m_UpdateQuery;
    private EntityQuery m_AllQuery;
    private EntityQuery m_PlantQuery;
    private EntityQuery m_BuildingSettingsQuery;
    private Entity m_LastSeason1;
    private Entity m_LastSeason2;
    private Entity m_OverrideEntity;
    private uint m_LastUpdateGroup;
    private uint m_UpdateGroupCount;
    private int m_OverrideIndex;
    private float m_LastSeasonBlend;
    private bool m_Loaded;
    private MeshColorSystem.TypeHandle __TypeHandle;

    public bool smoothColorsUpdated { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<MeshColor>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Common.Event>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<RentersUpdated>(),
          ComponentType.ReadOnly<ColorUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllQuery = this.GetEntityQuery(ComponentType.ReadOnly<MeshColor>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlantQuery = this.GetEntityQuery(ComponentType.ReadOnly<MeshColor>(), ComponentType.ReadOnly<Plant>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_GroupIDs = new Dictionary<string, int>();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    public void SetOverride(Entity entity, RenderPrefabBase prefab, int variationIndex)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_OverrideEntity != entity && this.m_OverrideEntity != Entity.Null && this.EntityManager.Exists(this.m_OverrideEntity) && !this.EntityManager.HasComponent<Deleted>(this.m_OverrideEntity))
      {
        // ISSUE: reference to a compiler-generated field
        this.World.GetExistingSystemManaged<EndFrameBarrier>().CreateCommandBuffer().AddComponent<BatchesUpdated>(this.m_OverrideEntity);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_OverrideEntity = entity;
      // ISSUE: reference to a compiler-generated field
      this.m_OverridePrefab = prefab;
      // ISSUE: reference to a compiler-generated field
      this.m_OverrideIndex = variationIndex;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      bool flag1 = this.GetLoaded() && !this.m_AllQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      bool flag2 = !flag1 && !this.m_UpdateQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex >> 9 & 15U;
      // ISSUE: reference to a compiler-generated field
      Entity currentClimate = this.m_ClimateSystem.currentClimate;
      // ISSUE: reference to a compiler-generated field
      this.smoothColorsUpdated = !flag1 && !this.m_PlantQuery.IsEmptyIgnoreFilter;
      Entity entity1 = Entity.Null;
      if (currentClimate != entity1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ClimatePrefab prefab = this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.m_ClimateSystem.currentClimate);
        // ISSUE: reference to a compiler-generated field
        float currentDate = (float) this.m_ClimateSystem.currentDate;
        (ClimateSystem.SeasonInfo seasonInfo1, float num2, float num3) = prefab.FindSeasonByTime(currentDate);
        if ((double) currentDate < (double) num2)
          ++currentDate;
        float a = (float) (((double) num2 + (double) num3) * 0.5);
        ClimateSystem.SeasonInfo seasonInfo2;
        float num4;
        float num5;
        if ((double) currentDate < (double) a)
        {
          float time = num2 - 1f / 1000f;
          if ((double) time < 0.0)
            ++time;
          (seasonInfo2, num4, num5) = prefab.FindSeasonByTime(time);
          if ((double) num4 > (double) num2)
          {
            ++a;
            ++currentDate;
          }
        }
        else
        {
          float time = num3 + 1f / 1000f;
          if ((double) time >= 1.0)
            --time;
          (seasonInfo2, num4, num5) = prefab.FindSeasonByTime(time);
          if ((double) num4 < (double) num2)
          {
            ++num4;
            ++num5;
          }
        }
        float b = (float) (((double) num4 + (double) num5) * 0.5);
        float num6 = math.round(math.smoothstep(a, b, currentDate) * 1600f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity2 = seasonInfo1 != null ? this.m_PrefabSystem.GetEntity((PrefabBase) seasonInfo1.m_Prefab) : Entity.Null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity3 = seasonInfo2 != null ? this.m_PrefabSystem.GetEntity((PrefabBase) seasonInfo2.m_Prefab) : Entity.Null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (entity2 != this.m_LastSeason1 || entity3 != this.m_LastSeason2 || (double) num6 != (double) this.m_LastSeasonBlend)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastSeason1 = entity2;
          // ISSUE: reference to a compiler-generated field
          this.m_LastSeason2 = entity3;
          // ISSUE: reference to a compiler-generated field
          this.m_LastSeasonBlend = num6;
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateGroupCount = 16U;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdateGroupCount != 0U && (int) this.m_LastUpdateGroup != (int) num1)
        {
          // ISSUE: reference to a compiler-generated field
          --this.m_UpdateGroupCount;
        }
        else
          this.smoothColorsUpdated = false;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_LastUpdateGroup = num1;
      if (!flag1 && !flag2 && !this.smoothColorsUpdated)
        return;
      JobHandle outJobHandle;
      NativeList<Entity> list;
      if (flag1)
      {
        // ISSUE: reference to a compiler-generated field
        list = this.m_AllQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      }
      else if (this.smoothColorsUpdated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PlantQuery.ResetFilter();
        // ISSUE: reference to a compiler-generated field
        this.m_PlantQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame()
        {
          m_Index = num1
        });
        // ISSUE: reference to a compiler-generated field
        list = this.m_PlantQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      }
      else
      {
        list = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        outJobHandle = new JobHandle();
      }
      if (flag2)
      {
        NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_ColorUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: variable of a compiler-generated type
        MeshColorSystem.FindUpdatedMeshColorsJob jobData1 = new MeshColorSystem.FindUpdatedMeshColorsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_RentersUpdatedType = this.__TypeHandle.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle,
          m_ColorUpdatedType = this.__TypeHandle.__Game_Routes_ColorUpdated_RO_ComponentTypeHandle,
          m_MeshColors = this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup,
          m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
          m_RouteVehicles = this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup,
          m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
          m_Queue = nativeQueue.AsParallelWriter()
        };
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MeshColorSystem.ListUpdatedMeshColorsJob jobData2 = new MeshColorSystem.ListUpdatedMeshColorsJob()
        {
          m_Queue = nativeQueue,
          m_List = list
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle job1 = jobData1.ScheduleParallel<MeshColorSystem.FindUpdatedMeshColorsJob>(this.m_UpdateQuery, this.Dependency);
        JobHandle dependsOn = JobHandle.CombineDependencies(outJobHandle, job1);
        JobHandle inputDeps = jobData2.Schedule<MeshColorSystem.ListUpdatedMeshColorsJob>(dependsOn);
        outJobHandle = inputDeps;
        nativeQueue.Dispose(inputDeps);
      }
      else
        outJobHandle = JobHandle.CombineDependencies(outJobHandle, this.Dependency);
      Entity entity4 = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_OverridePrefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.TryGetEntity((PrefabBase) this.m_OverridePrefab, out entity4);
      }
      NativeQueue<MeshColorSystem.CopyColorData> nativeQueue1 = new NativeQueue<MeshColorSystem.CopyColorData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshColor_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OverlayElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ColorFilter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ColorVariation_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResidentData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BrandData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Plant_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      MeshColorSystem.SetMeshColorsJob jobData3 = new MeshColorSystem.SetMeshColorsJob()
      {
        m_RandomSeed = RandomSeed.Next(),
        m_DefaultBrand = this.m_BuildingSettingsQuery.GetSingleton<BuildingConfigurationData>().m_DefaultRenterBrand,
        m_Season1 = this.m_LastSeason1,
        m_Season2 = this.m_LastSeason2,
        m_SeasonBlend = this.m_LastSeasonBlend,
        m_OverrideEntity = this.m_OverrideEntity,
        m_OverrideMesh = entity4,
        m_OverrideIndex = this.m_OverrideIndex,
        m_Stage = flag1 ? MeshColorSystem.UpdateStage.IgnoreSubs : MeshColorSystem.UpdateStage.Default,
        m_Entities = list.AsDeferredJobArray(),
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PlantData = this.__TypeHandle.__Game_Objects_Plant_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_CurrentRouteData = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentLookup,
        m_RouteColorData = this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup,
        m_CompanyData = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_BrandData = this.__TypeHandle.__Game_Prefabs_BrandData_RO_ComponentLookup,
        m_CreatureData = this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Prefabs_ResidentData_RO_ComponentLookup,
        m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_ColorVariations = this.__TypeHandle.__Game_Prefabs_ColorVariation_RO_BufferLookup,
        m_ColorFilters = this.__TypeHandle.__Game_Prefabs_ColorFilter_RO_BufferLookup,
        m_OverlayElements = this.__TypeHandle.__Game_Prefabs_OverlayElement_RO_BufferLookup,
        m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
        m_MeshColors = this.__TypeHandle.__Game_Rendering_MeshColor_RW_BufferLookup,
        m_CopyColors = nativeQueue1.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshColor_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MeshColorSystem.CopyMeshColorsJob jobData4 = new MeshColorSystem.CopyMeshColorsJob()
      {
        m_MeshColors = this.__TypeHandle.__Game_Rendering_MeshColor_RW_BufferLookup,
        m_CopyColors = nativeQueue1
      };
      JobHandle jobHandle = jobData3.Schedule<MeshColorSystem.SetMeshColorsJob, Entity>(list, 4, outJobHandle);
      if (flag1)
      {
        // ISSUE: reference to a compiler-generated field
        jobData3.m_Stage = MeshColorSystem.UpdateStage.IgnoreOwners;
        jobHandle = jobData3.Schedule<MeshColorSystem.SetMeshColorsJob, Entity>(list, 4, jobHandle);
      }
      JobHandle dependsOn1 = jobHandle;
      JobHandle inputDeps1 = jobData4.Schedule<MeshColorSystem.CopyMeshColorsJob>(dependsOn1);
      list.Dispose(jobHandle);
      nativeQueue1.Dispose(inputDeps1);
      this.Dependency = inputDeps1;
    }

    public ColorGroupID GetColorGroupID(string name)
    {
      int index = -1;
      // ISSUE: reference to a compiler-generated field
      if (!string.IsNullOrEmpty(name) && !this.m_GroupIDs.TryGetValue(name, out index))
      {
        // ISSUE: reference to a compiler-generated field
        index = this.m_GroupIDs.Count;
        // ISSUE: reference to a compiler-generated field
        this.m_GroupIDs.Add(name, index);
      }
      return new ColorGroupID(index);
    }

    private static void RandomizeColor(ref UnityEngine.Color color, ref Unity.Mathematics.Random random, float3 min, float3 max)
    {
      float3 float3_1;
      UnityEngine.Color.RGBToHSV(color, out float3_1.x, out float3_1.y, out float3_1.z);
      float a = color.a;
      float3 float3_2 = random.NextFloat3(min, max);
      float3_1.x = math.frac(float3_1.x + float3_2.x);
      float3_1.yz = math.saturate(float3_1.yz * float3_2.yz);
      color = UnityEngine.Color.HSVToRGB(float3_1.x, float3_1.y, float3_1.z);
      color.a = a;
    }

    private static void RandomizeAlphas(
      ref ColorSet colorSet,
      ref Unity.Mathematics.Random random,
      float3 min,
      float3 max)
    {
      float3 float3 = math.saturate(new float3(colorSet.m_Channel0.a, colorSet.m_Channel1.a, colorSet.m_Channel2.a) + random.NextFloat3(min, max));
      colorSet.m_Channel0.a = float3.x;
      colorSet.m_Channel1.a = float3.y;
      colorSet.m_Channel2.a = float3.z;
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
    public MeshColorSystem()
    {
    }

    [BurstCompile]
    private struct FindUpdatedMeshColorsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<RentersUpdated> m_RentersUpdatedType;
      [ReadOnly]
      public ComponentTypeHandle<ColorUpdated> m_ColorUpdatedType;
      [ReadOnly]
      public BufferLookup<MeshColor> m_MeshColors;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<RouteVehicle> m_RouteVehicles;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      public NativeQueue<Entity>.ParallelWriter m_Queue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<RentersUpdated> nativeArray1 = chunk.GetNativeArray<RentersUpdated>(ref this.m_RentersUpdatedType);
        if (nativeArray1.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity property = nativeArray1[index].m_Property;
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshColors.HasBuffer(property))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Queue.Enqueue(property);
            }
            // ISSUE: reference to a compiler-generated method
            this.AddSubObjects(property);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<ColorUpdated> nativeArray2 = chunk.GetNativeArray<ColorUpdated>(ref this.m_ColorUpdatedType);
          if (nativeArray2.Length != 0)
          {
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddRouteVehicles(nativeArray2[index].m_Route);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityType);
            for (int index = 0; index < nativeArray3.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Queue.Enqueue(nativeArray3[index]);
            }
          }
        }
      }

      private void AddSubObjects(Entity owner)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(owner, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MeshColors.HasBuffer(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Queue.Enqueue(subObject);
          }
          // ISSUE: reference to a compiler-generated method
          this.AddSubObjects(subObject);
        }
      }

      private void AddRouteVehicles(Entity owner)
      {
        DynamicBuffer<RouteVehicle> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_RouteVehicles.TryGetBuffer(owner, out bufferData1))
          return;
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          Entity vehicle = bufferData1[index1].m_Vehicle;
          DynamicBuffer<LayoutElement> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LayoutElements.TryGetBuffer(vehicle, out bufferData2) && bufferData2.Length != 0)
          {
            for (int index2 = 0; index2 < bufferData2.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MeshColors.HasBuffer(bufferData2[index2].m_Vehicle))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Queue.Enqueue(bufferData2[index2].m_Vehicle);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_MeshColors.HasBuffer(vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Queue.Enqueue(vehicle);
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

    [BurstCompile]
    private struct ListUpdatedMeshColorsJob : IJob
    {
      public NativeQueue<Entity> m_Queue;
      public NativeList<Entity> m_List;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_Queue.Count;
        if (count == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        int length = this.m_List.Length;
        // ISSUE: reference to a compiler-generated field
        this.m_List.ResizeUninitialized(length + count);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_List[length + index] = this.m_Queue.Dequeue();
        }
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> list = this.m_List;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MeshColorSystem.ListUpdatedMeshColorsJob.EntityComparer entityComparer = new MeshColorSystem.ListUpdatedMeshColorsJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        MeshColorSystem.ListUpdatedMeshColorsJob.EntityComparer comp = entityComparer;
        list.Sort<Entity, MeshColorSystem.ListUpdatedMeshColorsJob.EntityComparer>(comp);
        Entity entity1 = Entity.Null;
        int num = 0;
        int index1 = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_List.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_List[num++];
          if (entity2 != entity1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_List[index1++] = entity2;
            entity1 = entity2;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index1 >= this.m_List.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_List.RemoveRange(index1, this.m_List.Length - index1);
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    private struct CopyColorData
    {
      public Entity m_Source;
      public Entity m_Target;
      public uint m_RandomSeed;
      public int m_ColorIndex;
      public sbyte m_ExternalChannel0;
      public sbyte m_ExternalChannel1;
      public sbyte m_ExternalChannel2;
      public byte m_HueRange;
      public byte m_SaturationRange;
      public byte m_ValueRange;
      public byte m_AlphaRange0;
      public byte m_AlphaRange1;
      public byte m_AlphaRange2;

      public bool hasVariationRanges
      {
        get
        {
          return this.m_HueRange > (byte) 0 | this.m_SaturationRange > (byte) 0 | this.m_ValueRange > (byte) 0;
        }
      }

      public bool hasAlphaRanges
      {
        get
        {
          return this.m_AlphaRange0 > (byte) 0 | this.m_AlphaRange1 > (byte) 0 | this.m_AlphaRange2 > (byte) 0;
        }
      }

      public int GetExternalChannelIndex(int colorIndex)
      {
        switch (colorIndex)
        {
          case 0:
            // ISSUE: reference to a compiler-generated field
            return (int) this.m_ExternalChannel0;
          case 1:
            // ISSUE: reference to a compiler-generated field
            return (int) this.m_ExternalChannel1;
          case 2:
            // ISSUE: reference to a compiler-generated field
            return (int) this.m_ExternalChannel2;
          default:
            return -1;
        }
      }
    }

    private enum UpdateStage
    {
      Default,
      IgnoreSubs,
      IgnoreOwners,
    }

    [BurstCompile]
    private struct SetMeshColorsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public Entity m_DefaultBrand;
      [ReadOnly]
      public Entity m_Season1;
      [ReadOnly]
      public Entity m_Season2;
      [ReadOnly]
      public Entity m_OverrideEntity;
      [ReadOnly]
      public Entity m_OverrideMesh;
      [ReadOnly]
      public float m_SeasonBlend;
      [ReadOnly]
      public int m_OverrideIndex;
      [ReadOnly]
      public MeshColorSystem.UpdateStage m_Stage;
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Plant> m_PlantData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> m_CurrentRouteData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> m_RouteColorData;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_CompanyData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<BrandData> m_BrandData;
      [ReadOnly]
      public ComponentLookup<CreatureData> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<ResidentData> m_ResidentData;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<ColorVariation> m_ColorVariations;
      [ReadOnly]
      public BufferLookup<ColorFilter> m_ColorFilters;
      [ReadOnly]
      public BufferLookup<OverlayElement> m_OverlayElements;
      [ReadOnly]
      public BufferLookup<CharacterElement> m_CharacterElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<MeshColor> m_MeshColors;
      public NativeQueue<MeshColorSystem.CopyColorData>.ParallelWriter m_CopyColors;

      public unsafe void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Stage != MeshColorSystem.UpdateStage.Default && this.m_OwnerData.HasComponent(entity1) == (this.m_Stage == MeshColorSystem.UpdateStage.IgnoreSubs))
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity1];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<MeshColor> meshColor = this.m_MeshColors[entity1];
        DynamicBuffer<MeshGroup> bufferData1 = new DynamicBuffer<MeshGroup>();
        int num1 = 0;
        int length = 0;
        DynamicBuffer<SubMesh> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubMeshes.TryGetBuffer(prefabRef.m_Prefab, out bufferData2))
        {
          num1 = 1;
          length = bufferData2.Length;
        }
        bool flag = false;
        DynamicBuffer<SubMeshGroup> bufferData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData3))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_MeshGroups.TryGetBuffer(entity1, out bufferData1))
            num1 = bufferData1.Length;
          length = 0;
          for (int index1 = 0; index1 < num1; ++index1)
          {
            MeshGroup meshGroup;
            CollectionUtils.TryGet<MeshGroup>(bufferData1, index1, out meshGroup);
            SubMeshGroup subMeshGroup = bufferData3[(int) meshGroup.m_SubMeshGroup];
            length += subMeshGroup.m_SubMeshRange.y - subMeshGroup.m_SubMeshRange.x;
            for (int x = subMeshGroup.m_SubMeshRange.x; x < subMeshGroup.m_SubMeshRange.y; ++x)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_OverlayElements.HasBuffer(bufferData2[x].m_SubMesh))
              {
                flag = true;
                length += 8;
                break;
              }
            }
          }
        }
        if (length == 0)
        {
          meshColor.Clear();
        }
        else
        {
          MeshColorSystem.SetMeshColorsJob.SyncData* syncData1 = stackalloc MeshColorSystem.SetMeshColorsJob.SyncData[length * 2];
          meshColor.ResizeUninitialized(length);
          int num2 = 0;
          PseudoRandomSeed componentData;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PseudoRandomSeedData.TryGetComponent(entity1, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(index);
            componentData.m_Seed = (ushort) random.NextUInt(65536U);
          }
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          MeshColorSystem.SetMeshColorsJob.SearchData searchData = new MeshColorSystem.SetMeshColorsJob.SearchData();
          int syncIndex = 0;
          for (int index2 = 0; index2 < num1; ++index2)
          {
            MeshGroup meshGroup = new MeshGroup();
            SubMeshGroup subMeshGroup;
            if (bufferData3.IsCreated)
            {
              CollectionUtils.TryGet<MeshGroup>(bufferData1, index2, out meshGroup);
              subMeshGroup = bufferData3[(int) meshGroup.m_SubMeshGroup];
            }
            else
              subMeshGroup.m_SubMeshRange = new int2(0, bufferData2.Length);
            for (int x = subMeshGroup.m_SubMeshRange.x; x < subMeshGroup.m_SubMeshRange.y; ++x)
            {
              SubMesh subMesh = bufferData2[x];
              Unity.Mathematics.Random random = componentData.GetRandom((uint) PseudoRandomSeed.kColorVariation | (uint) subMesh.m_RandomSeed << 16);
              // ISSUE: reference to a compiler-generated method
              this.SetColor(meshColor, syncData1, num2++, entity1, subMesh.m_SubMesh, ref random, ref searchData, ref syncIndex);
            }
            if (flag)
            {
              for (int x = subMeshGroup.m_SubMeshRange.x; x < subMeshGroup.m_SubMeshRange.y; ++x)
              {
                DynamicBuffer<OverlayElement> bufferData4;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OverlayElements.TryGetBuffer(bufferData2[x].m_SubMesh, out bufferData4))
                {
                  CharacterElement characterElement = new CharacterElement();
                  DynamicBuffer<CharacterElement> bufferData5;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CharacterElements.TryGetBuffer(prefabRef.m_Prefab, out bufferData5))
                    characterElement = bufferData5[(int) meshGroup.m_SubMeshGroup];
                  DynamicBuffer<MeshColor> meshColors1 = meshColor;
                  MeshColorSystem.SetMeshColorsJob.SyncData* syncData2 = syncData1;
                  int colorIndex1 = num2;
                  int num3 = colorIndex1 + 1;
                  Entity entity2 = entity1;
                  DynamicBuffer<OverlayElement> overlayElements1 = bufferData4;
                  BlendWeight weight0 = characterElement.m_OverlayWeights.m_Weight0;
                  PseudoRandomSeed pseudoRandomSeed1 = componentData;
                  ref MeshColorSystem.SetMeshColorsJob.SearchData local1 = ref searchData;
                  ref int local2 = ref syncIndex;
                  // ISSUE: reference to a compiler-generated method
                  this.SetColor(meshColors1, syncData2, colorIndex1, entity2, overlayElements1, weight0, pseudoRandomSeed1, ref local1, ref local2);
                  DynamicBuffer<MeshColor> meshColors2 = meshColor;
                  MeshColorSystem.SetMeshColorsJob.SyncData* syncData3 = syncData1;
                  int colorIndex2 = num3;
                  int num4 = colorIndex2 + 1;
                  Entity entity3 = entity1;
                  DynamicBuffer<OverlayElement> overlayElements2 = bufferData4;
                  BlendWeight weight1 = characterElement.m_OverlayWeights.m_Weight1;
                  PseudoRandomSeed pseudoRandomSeed2 = componentData;
                  ref MeshColorSystem.SetMeshColorsJob.SearchData local3 = ref searchData;
                  ref int local4 = ref syncIndex;
                  // ISSUE: reference to a compiler-generated method
                  this.SetColor(meshColors2, syncData3, colorIndex2, entity3, overlayElements2, weight1, pseudoRandomSeed2, ref local3, ref local4);
                  DynamicBuffer<MeshColor> meshColors3 = meshColor;
                  MeshColorSystem.SetMeshColorsJob.SyncData* syncData4 = syncData1;
                  int colorIndex3 = num4;
                  int num5 = colorIndex3 + 1;
                  Entity entity4 = entity1;
                  DynamicBuffer<OverlayElement> overlayElements3 = bufferData4;
                  BlendWeight weight2 = characterElement.m_OverlayWeights.m_Weight2;
                  PseudoRandomSeed pseudoRandomSeed3 = componentData;
                  ref MeshColorSystem.SetMeshColorsJob.SearchData local5 = ref searchData;
                  ref int local6 = ref syncIndex;
                  // ISSUE: reference to a compiler-generated method
                  this.SetColor(meshColors3, syncData4, colorIndex3, entity4, overlayElements3, weight2, pseudoRandomSeed3, ref local5, ref local6);
                  DynamicBuffer<MeshColor> meshColors4 = meshColor;
                  MeshColorSystem.SetMeshColorsJob.SyncData* syncData5 = syncData1;
                  int colorIndex4 = num5;
                  int num6 = colorIndex4 + 1;
                  Entity entity5 = entity1;
                  DynamicBuffer<OverlayElement> overlayElements4 = bufferData4;
                  BlendWeight weight3 = characterElement.m_OverlayWeights.m_Weight3;
                  PseudoRandomSeed pseudoRandomSeed4 = componentData;
                  ref MeshColorSystem.SetMeshColorsJob.SearchData local7 = ref searchData;
                  ref int local8 = ref syncIndex;
                  // ISSUE: reference to a compiler-generated method
                  this.SetColor(meshColors4, syncData5, colorIndex4, entity5, overlayElements4, weight3, pseudoRandomSeed4, ref local7, ref local8);
                  DynamicBuffer<MeshColor> meshColors5 = meshColor;
                  MeshColorSystem.SetMeshColorsJob.SyncData* syncData6 = syncData1;
                  int colorIndex5 = num6;
                  int num7 = colorIndex5 + 1;
                  Entity entity6 = entity1;
                  DynamicBuffer<OverlayElement> overlayElements5 = bufferData4;
                  BlendWeight weight4 = characterElement.m_OverlayWeights.m_Weight4;
                  PseudoRandomSeed pseudoRandomSeed5 = componentData;
                  ref MeshColorSystem.SetMeshColorsJob.SearchData local9 = ref searchData;
                  ref int local10 = ref syncIndex;
                  // ISSUE: reference to a compiler-generated method
                  this.SetColor(meshColors5, syncData6, colorIndex5, entity6, overlayElements5, weight4, pseudoRandomSeed5, ref local9, ref local10);
                  DynamicBuffer<MeshColor> meshColors6 = meshColor;
                  MeshColorSystem.SetMeshColorsJob.SyncData* syncData7 = syncData1;
                  int colorIndex6 = num7;
                  int num8 = colorIndex6 + 1;
                  Entity entity7 = entity1;
                  DynamicBuffer<OverlayElement> overlayElements6 = bufferData4;
                  BlendWeight weight5 = characterElement.m_OverlayWeights.m_Weight5;
                  PseudoRandomSeed pseudoRandomSeed6 = componentData;
                  ref MeshColorSystem.SetMeshColorsJob.SearchData local11 = ref searchData;
                  ref int local12 = ref syncIndex;
                  // ISSUE: reference to a compiler-generated method
                  this.SetColor(meshColors6, syncData7, colorIndex6, entity7, overlayElements6, weight5, pseudoRandomSeed6, ref local11, ref local12);
                  DynamicBuffer<MeshColor> meshColors7 = meshColor;
                  MeshColorSystem.SetMeshColorsJob.SyncData* syncData8 = syncData1;
                  int colorIndex7 = num8;
                  int num9 = colorIndex7 + 1;
                  Entity entity8 = entity1;
                  DynamicBuffer<OverlayElement> overlayElements7 = bufferData4;
                  BlendWeight weight6 = characterElement.m_OverlayWeights.m_Weight6;
                  PseudoRandomSeed pseudoRandomSeed7 = componentData;
                  ref MeshColorSystem.SetMeshColorsJob.SearchData local13 = ref searchData;
                  ref int local14 = ref syncIndex;
                  // ISSUE: reference to a compiler-generated method
                  this.SetColor(meshColors7, syncData8, colorIndex7, entity8, overlayElements7, weight6, pseudoRandomSeed7, ref local13, ref local14);
                  DynamicBuffer<MeshColor> meshColors8 = meshColor;
                  MeshColorSystem.SetMeshColorsJob.SyncData* syncData9 = syncData1;
                  int colorIndex8 = num9;
                  num2 = colorIndex8 + 1;
                  Entity entity9 = entity1;
                  DynamicBuffer<OverlayElement> overlayElements8 = bufferData4;
                  BlendWeight weight7 = characterElement.m_OverlayWeights.m_Weight7;
                  PseudoRandomSeed pseudoRandomSeed8 = componentData;
                  ref MeshColorSystem.SetMeshColorsJob.SearchData local15 = ref searchData;
                  ref int local16 = ref syncIndex;
                  // ISSUE: reference to a compiler-generated method
                  this.SetColor(meshColors8, syncData9, colorIndex8, entity9, overlayElements8, weight7, pseudoRandomSeed8, ref local15, ref local16);
                  break;
                }
              }
            }
          }
        }
      }

      private unsafe void SetColor(
        DynamicBuffer<MeshColor> meshColors,
        MeshColorSystem.SetMeshColorsJob.SyncData* syncData,
        int colorIndex,
        Entity entity,
        DynamicBuffer<OverlayElement> overlayElements,
        BlendWeight overlayWeight,
        PseudoRandomSeed pseudoRandomSeed,
        ref MeshColorSystem.SetMeshColorsJob.SearchData searchData,
        ref int syncIndex)
      {
        Entity overlay = Entity.Null;
        if (overlayWeight.m_Index >= 0 && overlayWeight.m_Index < overlayElements.Length && (double) overlayWeight.m_Weight > 0.0)
          overlay = overlayElements[overlayWeight.m_Index].m_Overlay;
        Unity.Mathematics.Random random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kColorVariation | (uint) (overlayWeight.m_Index << 16));
        // ISSUE: reference to a compiler-generated method
        this.SetColor(meshColors, syncData, colorIndex, entity, overlay, ref random, ref searchData, ref syncIndex);
      }

      private unsafe void SetColor(
        DynamicBuffer<MeshColor> meshColors,
        MeshColorSystem.SetMeshColorsJob.SyncData* syncData,
        int colorIndex,
        Entity entity,
        Entity prefab,
        ref Unity.Mathematics.Random random,
        ref MeshColorSystem.SetMeshColorsJob.SearchData searchData,
        ref int syncIndex)
      {
        MeshColor meshColor1 = new MeshColor()
        {
          m_ColorSet = new ColorSet(UnityEngine.Color.white)
        };
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MeshColorSystem.SetMeshColorsJob.SyncData syncValue1 = new MeshColorSystem.SetMeshColorsJob.SyncData()
        {
          m_GroupID = new ColorGroupID(-1),
          m_RandomSeed = 0,
          m_ColorIndex = -1
        };
        DynamicBuffer<ColorVariation> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ColorVariations.TryGetBuffer(prefab, out bufferData1))
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          MeshColorSystem.SetMeshColorsJob.ColorDatas colors1 = new MeshColorSystem.SetMeshColorsJob.ColorDatas();
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          MeshColorSystem.SetMeshColorsJob.ColorDatas colors2 = new MeshColorSystem.SetMeshColorsJob.ColorDatas();
          int num1 = 0;
          int num2 = 0;
          uint num3 = 0;
          bool flag1 = false;
          bool anyGroupUsed = false;
          ColorGroupID colorGroupId = new ColorGroupID(-2);
          ColorFilter colorFilter1 = new ColorFilter();
          ref MeshColorSystem.SetMeshColorsJob.ColorDatas local1 = ref colors1;
          float t = 0.0f;
          DynamicBuffer<ColorFilter> bufferData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ColorFilters.TryGetBuffer(prefab, out bufferData2) && !searchData.m_FiltersSearched)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FindFilters(entity, ref searchData.m_Age, ref searchData.m_Gender);
            // ISSUE: reference to a compiler-generated field
            searchData.m_FiltersSearched = true;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num4 = math.select(-1, this.m_OverrideIndex, entity == this.m_OverrideEntity && prefab == this.m_OverrideMesh && this.m_OverrideIndex < bufferData1.Length);
          for (int index1 = 0; index1 < bufferData1.Length; ++index1)
          {
            ColorVariation colorVariation = bufferData1[index1];
            if (colorVariation.m_GroupID != colorGroupId)
            {
              num1 = 0;
              num2 = -1;
              num3 = 0U;
              colorGroupId = colorVariation.m_GroupID;
              flag1 = false;
              colorFilter1.m_OverrideProbability = (sbyte) -1;
              colorFilter1.m_OverrideAlpha = (float3) -1f;
              local1 = ref colors1;
              t = 0.0f;
              if (colorVariation.m_SyncFlags != ColorSyncFlags.None)
              {
                for (int index2 = 0; index2 < syncIndex; ++index2)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (syncData[index2].m_GroupID == colorGroupId)
                  {
                    // ISSUE: reference to a compiler-generated field
                    num2 = syncData[index2].m_ColorIndex;
                    // ISSUE: reference to a compiler-generated field
                    num3 = syncData[index2].m_RandomSeed;
                    flag1 = true;
                    break;
                  }
                }
                anyGroupUsed |= flag1;
              }
              if (bufferData2.IsCreated)
              {
                for (int index3 = 0; index3 < bufferData2.Length; ++index3)
                {
                  ColorFilter colorFilter2 = bufferData2[index3];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!(colorFilter2.m_GroupID != colorGroupId) && (colorFilter2.m_AgeFilter & searchData.m_Age) != (Game.Prefabs.AgeMask) 0 && (colorFilter2.m_GenderFilter & searchData.m_Gender) != (GenderMask) 0)
                  {
                    if ((colorFilter2.m_Flags & ColorFilterFlags.SeasonFilter) != (ColorFilterFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_Season1 != colorFilter2.m_EntityFilter)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_Season2 == colorFilter2.m_EntityFilter)
                        {
                          local1 = ref colors2;
                          // ISSUE: reference to a compiler-generated field
                          t = this.m_SeasonBlend;
                        }
                        else
                          continue;
                      }
                      // ISSUE: reference to a compiler-generated field
                      local1.m_SeedOffset += (uint) (index3 * -1571468583);
                    }
                    if (colorFilter2.m_OverrideProbability >= (sbyte) 0)
                      colorFilter1.m_OverrideProbability = colorFilter2.m_OverrideProbability;
                    colorFilter1.m_OverrideAlpha = math.select(colorFilter1.m_OverrideAlpha, colorFilter2.m_OverrideAlpha, colorFilter2.m_OverrideAlpha >= 0.0f);
                  }
                }
              }
            }
            if (num4 != -1)
              colorVariation.m_Probability = (byte) math.select(0, 100, index1 == num4);
            else if (colorFilter1.m_OverrideProbability != (sbyte) -1)
              colorVariation.m_Probability = (byte) colorFilter1.m_OverrideProbability;
            bool3 x = colorFilter1.m_OverrideAlpha >= 0.0f;
            if (math.any(x))
            {
              colorVariation.m_ColorSet.m_Channel0.a = math.select(colorVariation.m_ColorSet.m_Channel0.a, colorFilter1.m_OverrideAlpha.x, x.x);
              colorVariation.m_ColorSet.m_Channel1.a = math.select(colorVariation.m_ColorSet.m_Channel1.a, colorFilter1.m_OverrideAlpha.y, x.y);
              colorVariation.m_ColorSet.m_Channel2.a = math.select(colorVariation.m_ColorSet.m_Channel2.a, colorFilter1.m_OverrideAlpha.z, x.z);
            }
            if (colorVariation.m_SyncFlags != ColorSyncFlags.None)
            {
              bool flag2 = true;
              if ((colorVariation.m_SyncFlags & ColorSyncFlags.SameGroup) != ColorSyncFlags.None)
                flag2 &= flag1;
              if ((colorVariation.m_SyncFlags & ColorSyncFlags.DifferentGroup) != ColorSyncFlags.None)
                flag2 &= !flag1;
              if ((colorVariation.m_SyncFlags & ColorSyncFlags.SameIndex) != ColorSyncFlags.None)
                flag2 &= num1 == num2;
              if ((colorVariation.m_SyncFlags & ColorSyncFlags.DifferentIndex) != ColorSyncFlags.None)
                flag2 &= num1 != num2;
              // ISSUE: reference to a compiler-generated field
              ref MeshColorSystem.SetMeshColorsJob.ColorData local2 = ref local1.m_Unmatch;
              if (flag2)
              {
                // ISSUE: reference to a compiler-generated field
                local2 = ref local1.m_Match;
              }
              // ISSUE: reference to a compiler-generated field
              local2.m_Probability += (int) colorVariation.m_Probability;
              // ISSUE: reference to a compiler-generated field
              if (random.NextInt(local2.m_Probability) < (int) colorVariation.m_Probability)
              {
                // ISSUE: reference to a compiler-generated field
                local2.m_Color = colorVariation;
                // ISSUE: reference to a compiler-generated field
                local2.m_Index = num1;
                // ISSUE: reference to a compiler-generated field
                local2.m_RandomSeed = num3;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              local1.m_Unsync.m_Probability += (int) colorVariation.m_Probability;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (random.NextInt(local1.m_Unsync.m_Probability) < (int) colorVariation.m_Probability)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                local1.m_Unsync.m_Color = colorVariation;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                local1.m_Unsync.m_Color.m_GroupID = new ColorGroupID(-1);
              }
            }
            ++num1;
          }
          Unity.Mathematics.Random random1 = random;
          // ISSUE: reference to a compiler-generated field
          random1.state += colors1.m_SeedOffset;
          random1.state = math.select(random1.state, random.state, random1.state == 0U);
          // ISSUE: reference to a compiler-generated method
          this.CalculateMeshColor(ref meshColor1, ref syncValue1, ref random1, ref searchData, ref colors1, entity, colorIndex, anyGroupUsed);
          if ((double) t != 0.0)
          {
            Unity.Mathematics.Random random2 = random;
            // ISSUE: reference to a compiler-generated field
            random2.state += colors2.m_SeedOffset;
            random2.state = math.select(random2.state, random.state, random2.state == 0U);
            MeshColor meshColor2 = new MeshColor();
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            MeshColorSystem.SetMeshColorsJob.SyncData syncValue2 = new MeshColorSystem.SetMeshColorsJob.SyncData()
            {
              m_GroupID = new ColorGroupID(-1),
              m_RandomSeed = 0,
              m_ColorIndex = -1
            };
            // ISSUE: reference to a compiler-generated method
            this.CalculateMeshColor(ref meshColor2, ref syncValue2, ref random2, ref searchData, ref colors2, entity, colorIndex, anyGroupUsed);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (colors2.m_Match.m_Probability > 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (colors1.m_Match.m_Probability > 0)
              {
                meshColor1.m_ColorSet.m_Channel0 = UnityEngine.Color.Lerp(meshColor1.m_ColorSet.m_Channel0, meshColor2.m_ColorSet.m_Channel0, t);
                meshColor1.m_ColorSet.m_Channel1 = UnityEngine.Color.Lerp(meshColor1.m_ColorSet.m_Channel1, meshColor2.m_ColorSet.m_Channel1, t);
                meshColor1.m_ColorSet.m_Channel2 = UnityEngine.Color.Lerp(meshColor1.m_ColorSet.m_Channel2, meshColor2.m_ColorSet.m_Channel2, t);
              }
              else
                meshColor1.m_ColorSet = meshColor2.m_ColorSet;
              syncData[syncIndex++] = syncValue2;
            }
          }
        }
        meshColors[colorIndex] = meshColor1;
        syncData[syncIndex++] = syncValue1;
      }

      private void CalculateMeshColor(
        ref MeshColor meshColor,
        ref MeshColorSystem.SetMeshColorsJob.SyncData syncValue,
        ref Unity.Mathematics.Random random,
        ref MeshColorSystem.SetMeshColorsJob.SearchData searchData,
        ref MeshColorSystem.SetMeshColorsJob.ColorDatas colors,
        Entity entity,
        int colorIndex,
        bool anyGroupUsed)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        colors.m_Match.m_Probability += colors.m_Unmatch.m_Probability;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!anyGroupUsed && random.NextInt(colors.m_Match.m_Probability) < colors.m_Unmatch.m_Probability)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          colors.m_Match.m_Color = colors.m_Unmatch.m_Color;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          colors.m_Match.m_Index = colors.m_Unmatch.m_Index;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          colors.m_Match.m_RandomSeed = colors.m_Unmatch.m_RandomSeed;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        colors.m_Match.m_Probability += colors.m_Unsync.m_Probability;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (random.NextInt(colors.m_Match.m_Probability) < colors.m_Unsync.m_Probability)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          colors.m_Match.m_Color = colors.m_Unsync.m_Color;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          colors.m_Match.m_Index = -1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          colors.m_Match.m_RandomSeed = 0U;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (colors.m_Match.m_Probability <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        meshColor.m_ColorSet = colors.m_Match.m_Color.m_ColorSet;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        syncValue.m_GroupID = colors.m_Match.m_Color.m_GroupID;
        // ISSUE: reference to a compiler-generated field
        syncValue.m_RandomSeed = random.state;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        syncValue.m_ColorIndex = colors.m_Match.m_Index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((colors.m_Match.m_Color.m_SyncFlags & ColorSyncFlags.SyncRangeVariation) != ColorSyncFlags.None && colors.m_Match.m_RandomSeed != 0U)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          syncValue.m_RandomSeed = colors.m_Match.m_RandomSeed;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (colors.m_Match.m_Color.hasExternalChannels)
        {
          // ISSUE: reference to a compiler-generated field
          if (!searchData.m_ExternalSearched)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            searchData.m_ColorSource = this.FindExternalSource(entity, colors.m_Match.m_Color.m_ColorSourceType);
            // ISSUE: reference to a compiler-generated field
            searchData.m_ExternalSearched = true;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (colors.m_Match.m_Color.m_ColorSourceType == ColorSourceType.Parent)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Stage == MeshColorSystem.UpdateStage.Default || this.m_OwnerData.HasComponent(searchData.m_ColorSource) == (this.m_Stage == MeshColorSystem.UpdateStage.IgnoreOwners))
            {
              // ISSUE: reference to a compiler-generated field
              if (searchData.m_ColorSource != Entity.Null)
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
                this.m_CopyColors.Enqueue(new MeshColorSystem.CopyColorData()
                {
                  m_Source = searchData.m_ColorSource,
                  m_Target = entity,
                  m_RandomSeed = syncValue.m_RandomSeed,
                  m_ColorIndex = colorIndex,
                  m_ExternalChannel0 = colors.m_Match.m_Color.m_ExternalChannel0,
                  m_ExternalChannel1 = colors.m_Match.m_Color.m_ExternalChannel1,
                  m_ExternalChannel2 = colors.m_Match.m_Color.m_ExternalChannel2,
                  m_HueRange = colors.m_Match.m_Color.m_HueRange,
                  m_SaturationRange = colors.m_Match.m_Color.m_SaturationRange,
                  m_ValueRange = colors.m_Match.m_Color.m_ValueRange,
                  m_AlphaRange0 = colors.m_Match.m_Color.m_AlphaRange0,
                  m_AlphaRange1 = colors.m_Match.m_Color.m_AlphaRange1,
                  m_AlphaRange2 = colors.m_Match.m_Color.m_AlphaRange2
                });
              }
            }
            else
            {
              DynamicBuffer<MeshColor> bufferData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_MeshColors.TryGetBuffer(searchData.m_ColorSource, out bufferData) && bufferData.Length != 0)
              {
                MeshColor meshColor1 = bufferData[math.min(colorIndex, bufferData.Length - 1)];
                for (int index = 0; index < 3; ++index)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int externalChannelIndex = colors.m_Match.m_Color.GetExternalChannelIndex(index);
                  if (externalChannelIndex >= 0)
                    meshColor.m_ColorSet[externalChannelIndex] = meshColor1.m_ColorSet[index];
                }
              }
            }
          }
          else
          {
            BrandData componentData1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_BrandData.TryGetComponent(searchData.m_ColorSource, out componentData1))
            {
              for (int index = 0; index < 3; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int externalChannelIndex = colors.m_Match.m_Color.GetExternalChannelIndex(index);
                if (externalChannelIndex >= 0)
                  meshColor.m_ColorSet[externalChannelIndex] = componentData1.m_ColorSet[index];
              }
            }
            else
            {
              Game.Routes.Color componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_RouteColorData.TryGetComponent(searchData.m_ColorSource, out componentData2))
              {
                for (int colorIndex1 = 0; colorIndex1 < 3; ++colorIndex1)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int externalChannelIndex = colors.m_Match.m_Color.GetExternalChannelIndex(colorIndex1);
                  if (externalChannelIndex >= 0)
                    meshColor.m_ColorSet[externalChannelIndex] = (UnityEngine.Color) componentData2.m_Color;
                }
              }
            }
          }
        }
        Unity.Mathematics.Random random1 = new Unity.Mathematics.Random();
        // ISSUE: reference to a compiler-generated field
        random1.state = syncValue.m_RandomSeed;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (colors.m_Match.m_Color.hasVariationRanges)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3 = new float3((float) colors.m_Match.m_Color.m_HueRange, (float) colors.m_Match.m_Color.m_SaturationRange, (float) colors.m_Match.m_Color.m_ValueRange) * 0.01f;
          float3 min = 1f - float3;
          float3 max = 1f + float3;
          // ISSUE: reference to a compiler-generated method
          MeshColorSystem.RandomizeColor(ref meshColor.m_ColorSet.m_Channel0, ref random1, min, max);
          // ISSUE: reference to a compiler-generated method
          MeshColorSystem.RandomizeColor(ref meshColor.m_ColorSet.m_Channel1, ref random1, min, max);
          // ISSUE: reference to a compiler-generated method
          MeshColorSystem.RandomizeColor(ref meshColor.m_ColorSet.m_Channel2, ref random1, min, max);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (colors.m_Match.m_Color.hasAlphaRanges)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3 = new float3((float) colors.m_Match.m_Color.m_AlphaRange0, (float) colors.m_Match.m_Color.m_AlphaRange1, (float) colors.m_Match.m_Color.m_AlphaRange2) * 0.01f;
          float3 min = -float3;
          float3 max = float3;
          // ISSUE: reference to a compiler-generated method
          MeshColorSystem.RandomizeAlphas(ref meshColor.m_ColorSet, ref random1, min, max);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((colors.m_Match.m_Color.m_SyncFlags & ColorSyncFlags.SyncRangeVariation) != ColorSyncFlags.None && colors.m_Match.m_RandomSeed != 0U)
          return;
        random = random1;
      }

      private void FindFilters(Entity entity, ref Game.Prefabs.AgeMask age, ref GenderMask gender)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        CreatureData componentData1;
        // ISSUE: reference to a compiler-generated field
        gender = !this.m_CreatureData.TryGetComponent(prefabRef.m_Prefab, out componentData1) ? GenderMask.Any : componentData1.m_Gender;
        ResidentData componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ResidentData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          age = componentData2.m_Age;
        else
          age = Game.Prefabs.AgeMask.Any;
      }

      private Entity FindExternalSource(Entity entity, ColorSourceType colorSourceType)
      {
        bool flag1 = false;
        bool flag2 = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlantData.HasComponent(entity))
          return Entity.Null;
        Temp componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.TryGetComponent(entity, out componentData1))
        {
          if (componentData1.m_Original != Entity.Null)
            entity = componentData1.m_Original;
          else
            flag1 = true;
        }
        Controller componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControllerData.TryGetComponent(entity, out componentData2) && componentData2.m_Controller != Entity.Null)
          entity = componentData2.m_Controller;
        switch (colorSourceType)
        {
          case ColorSourceType.Brand:
            DynamicBuffer<Renter> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Renters.TryGetBuffer(entity, out bufferData))
            {
              Entity brand;
              // ISSUE: reference to a compiler-generated method
              if (this.FindBrand(bufferData, out brand))
                return brand;
              flag2 = true;
            }
            CurrentRoute componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentRouteData.TryGetComponent(entity, out componentData3) && this.m_RouteColorData.HasComponent(componentData3.m_Route))
              return componentData3.m_Route;
            // ISSUE: reference to a compiler-generated field
            if (this.m_RouteColorData.HasComponent(entity))
              return entity;
            Owner componentData4;
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(entity, out componentData4))
            {
              entity = componentData4.m_Owner;
              // ISSUE: reference to a compiler-generated field
              if (flag1 && this.m_TempData.TryGetComponent(entity, out componentData1) && componentData1.m_Original != Entity.Null)
              {
                entity = componentData1.m_Original;
                flag1 = false;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_Renters.TryGetBuffer(entity, out bufferData))
              {
                Entity brand;
                // ISSUE: reference to a compiler-generated method
                if (this.FindBrand(bufferData, out brand))
                  return brand;
                flag2 = true;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurrentRouteData.TryGetComponent(entity, out componentData3) && this.m_RouteColorData.HasComponent(componentData3.m_Route))
                return componentData3.m_Route;
              // ISSUE: reference to a compiler-generated field
              if (this.m_RouteColorData.HasComponent(entity))
                return entity;
            }
            // ISSUE: reference to a compiler-generated field
            return flag2 ? this.m_DefaultBrand : Entity.Null;
          case ColorSourceType.Parent:
            Entity externalSource = Entity.Null;
            Owner componentData5;
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(entity, out componentData5))
            {
              entity = componentData5.m_Owner;
              // ISSUE: reference to a compiler-generated field
              if (flag1 && this.m_TempData.TryGetComponent(entity, out componentData1) && componentData1.m_Original != Entity.Null)
              {
                entity = componentData1.m_Original;
                flag1 = false;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_MeshColors.HasBuffer(entity))
                externalSource = entity;
            }
            return externalSource;
          default:
            return Entity.Null;
        }
      }

      private bool FindBrand(DynamicBuffer<Renter> renters, out Entity brand)
      {
        for (int index = 0; index < renters.Length; ++index)
        {
          Entity renter = renters[index].m_Renter;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompanyData.HasComponent(renter))
          {
            // ISSUE: reference to a compiler-generated field
            CompanyData companyData = this.m_CompanyData[renter];
            if (companyData.m_Brand != Entity.Null)
            {
              brand = companyData.m_Brand;
              return true;
            }
          }
        }
        brand = Entity.Null;
        return false;
      }

      private struct SyncData
      {
        public ColorGroupID m_GroupID;
        public uint m_RandomSeed;
        public int m_ColorIndex;
      }

      private struct SearchData
      {
        public Entity m_ColorSource;
        public Game.Prefabs.AgeMask m_Age;
        public GenderMask m_Gender;
        public bool m_ExternalSearched;
        public bool m_FiltersSearched;
      }

      private struct ColorData
      {
        public ColorVariation m_Color;
        public int m_Probability;
        public int m_Index;
        public uint m_RandomSeed;
      }

      private struct ColorDatas
      {
        public MeshColorSystem.SetMeshColorsJob.ColorData m_Match;
        public MeshColorSystem.SetMeshColorsJob.ColorData m_Unmatch;
        public MeshColorSystem.SetMeshColorsJob.ColorData m_Unsync;
        public uint m_SeedOffset;
      }
    }

    [BurstCompile]
    private struct CopyMeshColorsJob : IJob
    {
      public BufferLookup<MeshColor> m_MeshColors;
      public NativeQueue<MeshColorSystem.CopyColorData> m_CopyColors;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        MeshColorSystem.CopyColorData copyColorData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_CopyColors.TryDequeue(out copyColorData))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<MeshColor> meshColor1 = this.m_MeshColors[copyColorData.m_Source];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<MeshColor> meshColor2 = this.m_MeshColors[copyColorData.m_Target];
          if (meshColor1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            MeshColor meshColor3 = meshColor1[math.min(copyColorData.m_ColorIndex, meshColor1.Length - 1)];
            // ISSUE: reference to a compiler-generated field
            ref MeshColor local = ref meshColor2.ElementAt(copyColorData.m_ColorIndex);
            Unity.Mathematics.Random random = new Unity.Mathematics.Random();
            // ISSUE: reference to a compiler-generated field
            random.state = copyColorData.m_RandomSeed;
            if (copyColorData.hasVariationRanges)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3 = new float3((float) copyColorData.m_HueRange, (float) copyColorData.m_SaturationRange, (float) copyColorData.m_ValueRange) * 0.01f;
              float3 min = 1f - float3;
              float3 max = 1f + float3;
              // ISSUE: reference to a compiler-generated method
              MeshColorSystem.RandomizeColor(ref meshColor3.m_ColorSet.m_Channel0, ref random, min, max);
              // ISSUE: reference to a compiler-generated method
              MeshColorSystem.RandomizeColor(ref meshColor3.m_ColorSet.m_Channel1, ref random, min, max);
              // ISSUE: reference to a compiler-generated method
              MeshColorSystem.RandomizeColor(ref meshColor3.m_ColorSet.m_Channel2, ref random, min, max);
            }
            if (copyColorData.hasAlphaRanges)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3 = new float3((float) copyColorData.m_AlphaRange0, (float) copyColorData.m_AlphaRange1, (float) copyColorData.m_AlphaRange2) * 0.01f;
              float3 min = -float3;
              float3 max = float3;
              // ISSUE: reference to a compiler-generated method
              MeshColorSystem.RandomizeAlphas(ref meshColor3.m_ColorSet, ref random, min, max);
            }
            for (int index = 0; index < 3; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              int externalChannelIndex = copyColorData.GetExternalChannelIndex(index);
              if (externalChannelIndex >= 0)
                local.m_ColorSet[externalChannelIndex] = meshColor3.m_ColorSet[index];
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RentersUpdated> __Game_Buildings_RentersUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ColorUpdated> __Game_Routes_ColorUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<MeshColor> __Game_Rendering_MeshColor_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteVehicle> __Game_Routes_RouteVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Plant> __Game_Objects_Plant_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> __Game_Routes_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CompanyData> __Game_Companies_CompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BrandData> __Game_Prefabs_BrandData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CreatureData> __Game_Prefabs_CreatureData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResidentData> __Game_Prefabs_ResidentData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ColorVariation> __Game_Prefabs_ColorVariation_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ColorFilter> __Game_Prefabs_ColorFilter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<OverlayElement> __Game_Prefabs_OverlayElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CharacterElement> __Game_Prefabs_CharacterElement_RO_BufferLookup;
      public BufferLookup<MeshColor> __Game_Rendering_MeshColor_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RentersUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RentersUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ColorUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ColorUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshColor_RO_BufferLookup = state.GetBufferLookup<MeshColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RO_BufferLookup = state.GetBufferLookup<RouteVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Plant_RO_ComponentLookup = state.GetComponentLookup<Plant>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurrentRoute_RO_ComponentLookup = state.GetComponentLookup<CurrentRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RO_ComponentLookup = state.GetComponentLookup<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BrandData_RO_ComponentLookup = state.GetComponentLookup<BrandData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureData_RO_ComponentLookup = state.GetComponentLookup<CreatureData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResidentData_RO_ComponentLookup = state.GetComponentLookup<ResidentData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferLookup = state.GetBufferLookup<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ColorVariation_RO_BufferLookup = state.GetBufferLookup<ColorVariation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ColorFilter_RO_BufferLookup = state.GetBufferLookup<ColorFilter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OverlayElement_RO_BufferLookup = state.GetBufferLookup<OverlayElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterElement_RO_BufferLookup = state.GetBufferLookup<CharacterElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshColor_RW_BufferLookup = state.GetBufferLookup<MeshColor>();
      }
    }
  }
}
