// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BuildingLotRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
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
  public class BuildingLotRenderSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private OverlayRenderSystem m_OverlayRenderSystem;
    private EntityQuery m_LotQuery;
    private EntityQuery m_RenderingSettingsQuery;
    private BuildingLotRenderSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem = this.World.GetOrCreateSystemManaged<OverlayRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LotQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        },
        Any = new ComponentType[5]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Extension>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Taxiway>(),
          ComponentType.ReadOnly<Tree>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Overridden>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Error>()
        },
        Any = new ComponentType[5]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Extension>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Taxiway>(),
          ComponentType.ReadOnly<Tree>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Overridden>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Warning>()
        },
        Any = new ComponentType[5]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Extension>(),
          ComponentType.ReadOnly<AssetStamp>(),
          ComponentType.ReadOnly<Taxiway>(),
          ComponentType.ReadOnly<Tree>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Overridden>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<RenderingSettingsData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Entity entity = this.m_ToolSystem.actionMode.IsEditor() ? this.m_ToolSystem.selected : Entity.Null;
      // ISSUE: reference to a compiler-generated field
      bool flag = !this.m_LotQuery.IsEmptyIgnoreFilter;
      if (!flag && entity == Entity.Null)
        return;
      RenderingSettingsData renderingSettingsData = new RenderingSettingsData()
      {
        m_HoveredColor = new UnityEngine.Color(0.5f, 0.5f, 1f, 1f),
        m_ErrorColor = new UnityEngine.Color(1f, 0.5f, 0.5f, 1f),
        m_WarningColor = new UnityEngine.Color(1f, 1f, 0.5f, 1f),
        m_OwnerColor = new UnityEngine.Color(0.5f, 1f, 0.5f, 1f)
      };
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RenderingSettingsQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        RenderingSettingsData singleton = this.m_RenderingSettingsQuery.GetSingleton<RenderingSettingsData>();
        renderingSettingsData.m_HoveredColor = singleton.m_HoveredColor;
        renderingSettingsData.m_ErrorColor = singleton.m_ErrorColor;
        renderingSettingsData.m_WarningColor = singleton.m_WarningColor;
        renderingSettingsData.m_OwnerColor = singleton.m_OwnerColor;
      }
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      OverlayRenderSystem.Buffer buffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies);
      this.Dependency = JobHandle.CombineDependencies(this.Dependency, dependencies);
      if (flag)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Error_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_AssetStamp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        BuildingLotRenderSystem.BuildingLotRenderJob jobData = new BuildingLotRenderSystem.BuildingLotRenderJob()
        {
          m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
          m_ExtensionType = this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentTypeHandle,
          m_AssetStampType = this.__TypeHandle.__Game_Objects_AssetStamp_RO_ComponentTypeHandle,
          m_TreeType = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentTypeHandle,
          m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
          m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
          m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
          m_StartGeometryType = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle,
          m_EndGeometryType = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_WarningType = this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle,
          m_ErrorType = this.__TypeHandle.__Game_Tools_Error_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
          m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
          m_PrefabBuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
          m_PrefabAssetStampData = this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup,
          m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
          m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
          m_PrefabServiceUpgradeBuildings = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferLookup,
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_RenderingSettingsData = renderingSettingsData,
          m_OverlayBuffer = buffer
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.Schedule<BuildingLotRenderSystem.BuildingLotRenderJob>(this.m_LotQuery, this.Dependency);
      }
      if (entity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AdditionalBuildingTerraformElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        BuildingLotRenderSystem.BuildingTerraformRenderJob jobData = new BuildingLotRenderSystem.BuildingTerraformRenderJob()
        {
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
          m_PrefabBuildingTerraformData = this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup,
          m_PrefabAdditionalTerraform = this.__TypeHandle.__Game_Prefabs_AdditionalBuildingTerraformElement_RO_BufferLookup,
          m_Selected = entity,
          m_OverlayBuffer = buffer
        };
        this.Dependency = jobData.Schedule<BuildingLotRenderSystem.BuildingTerraformRenderJob>(this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem.AddBufferWriter(this.Dependency);
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
    public BuildingLotRenderSystem()
    {
    }

    [BurstCompile]
    private struct BuildingLotRenderJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Extension> m_ExtensionType;
      [ReadOnly]
      public ComponentTypeHandle<AssetStamp> m_AssetStampType;
      [ReadOnly]
      public ComponentTypeHandle<Tree> m_TreeType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> m_EdgeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> m_StartGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> m_EndGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Warning> m_WarningType;
      [ReadOnly]
      public ComponentTypeHandle<Error> m_ErrorType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<AssetStampData> m_PrefabAssetStampData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public BufferLookup<ServiceUpgradeBuilding> m_PrefabServiceUpgradeBuildings;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public RenderingSettingsData m_RenderingSettingsData;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray1 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        bool checkTempFlags;
        bool flag;
        UnityEngine.Color lotColor;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Error>(ref this.m_ErrorType))
        {
          checkTempFlags = false;
          flag = true;
          // ISSUE: reference to a compiler-generated field
          lotColor = this.m_RenderingSettingsData.m_ErrorColor;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Warning>(ref this.m_WarningType))
          {
            checkTempFlags = false;
            flag = true;
            // ISSUE: reference to a compiler-generated field
            lotColor = this.m_RenderingSettingsData.m_WarningColor;
          }
          else if (nativeArray1.Length != 0)
          {
            checkTempFlags = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            flag = this.m_EditorMode || !chunk.Has<Tree>(ref this.m_TreeType) || !chunk.Has<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            lotColor = this.m_RenderingSettingsData.m_HoveredColor;
          }
          else
          {
            checkTempFlags = false;
            flag = false;
            lotColor = new UnityEngine.Color();
          }
        }
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Composition> nativeArray2 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.DrawNets(in chunk, nativeArray2, nativeArray1, lotColor, checkTempFlags);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.Transform> nativeArray3 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
          if (nativeArray3.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.DrawObjects(in chunk, nativeArray3, nativeArray1, lotColor, checkTempFlags);
        }
      }

      private void DrawNets(
        in ArchetypeChunk chunk,
        NativeArray<Composition> compositions,
        NativeArray<Temp> temps,
        UnityEngine.Color lotColor,
        bool checkTempFlags)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Edge> nativeArray1 = chunk.GetNativeArray<Game.Net.Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EdgeGeometry> nativeArray2 = chunk.GetNativeArray<EdgeGeometry>(ref this.m_EdgeGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<StartNodeGeometry> nativeArray3 = chunk.GetNativeArray<StartNodeGeometry>(ref this.m_StartGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EndNodeGeometry> nativeArray4 = chunk.GetNativeArray<EndNodeGeometry>(ref this.m_EndGeometryType);
        for (int index = 0; index < compositions.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabCompositionData[compositions[index].m_Edge];
          if ((netCompositionData.m_State & CompositionState.Airspace) != (CompositionState) 0 && (netCompositionData.m_Flags.m_General & CompositionFlags.General.Elevated) != (CompositionFlags.General) 0)
          {
            Game.Net.Edge edge = nativeArray1[index];
            EdgeGeometry edgeGeometry = nativeArray2[index];
            UnityEngine.Color color = lotColor;
            if (checkTempFlags)
            {
              Temp temp = temps[index];
              if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent)) != (TempFlags) 0)
              {
                if ((temp.m_Flags & TempFlags.Parent) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  color = this.m_RenderingSettingsData.m_OwnerColor;
                }
              }
              else
                continue;
            }
            // ISSUE: reference to a compiler-generated method
            this.DrawSegment(color, edgeGeometry.m_Start);
            // ISSUE: reference to a compiler-generated method
            this.DrawSegment(color, edgeGeometry.m_End);
            Game.Net.Elevation componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetElevationData.TryGetComponent(edge.m_Start, out componentData1) && math.all(componentData1.m_Elevation > 0.0f))
            {
              EdgeNodeGeometry geometry = nativeArray3[index].m_Geometry;
              geometry.m_Left.m_Right = geometry.m_Right.m_Right;
              // ISSUE: reference to a compiler-generated method
              this.DrawSegment(color, geometry.m_Left);
            }
            Game.Net.Elevation componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetElevationData.TryGetComponent(edge.m_End, out componentData2) && math.all(componentData2.m_Elevation > 0.0f))
            {
              EdgeNodeGeometry geometry = nativeArray4[index].m_Geometry;
              geometry.m_Left.m_Right = geometry.m_Right.m_Right;
              // ISSUE: reference to a compiler-generated method
              this.DrawSegment(color, geometry.m_Left);
            }
          }
        }
      }

      private void DrawSegment(UnityEngine.Color color, Game.Net.Segment segment)
      {
        UnityEngine.Color color1 = new UnityEngine.Color(color.r, color.g, color.b, 0.25f);
        float x1 = MathUtils.Length(segment.m_Left.xz) / 32f;
        float x2 = MathUtils.Length(segment.m_Right.xz) / 32f;
        float num1 = (float) ((double) x1 / (double) math.max(1f, math.round(x1)) * 16.0);
        float num2 = (float) ((double) x2 / (double) math.max(1f, math.round(x2)) * 16.0);
        if ((double) x1 > 0.5)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedCurve(color1, segment.m_Left, 4f, num1, num1);
        }
        if ((double) x2 <= 0.5)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawDashedCurve(color1, segment.m_Right, 4f, num2, num2);
      }

      private void DrawObjects(
        in ArchetypeChunk chunk,
        NativeArray<Game.Objects.Transform> transforms,
        NativeArray<Temp> temps,
        UnityEngine.Color lotColor,
        bool checkTempFlags)
      {
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<Extension>(ref this.m_ExtensionType);
        // ISSUE: reference to a compiler-generated field
        bool flag3 = chunk.Has<AssetStamp>(ref this.m_AssetStampType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Game.Objects.Transform transform = transforms[index1];
          Entity prefab = nativeArray[index1].m_Prefab;
          UnityEngine.Color color = lotColor;
          if (checkTempFlags)
          {
            Temp temp = temps[index1];
            if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0 && (temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent)) != (TempFlags) 0)
            {
              if ((temp.m_Flags & TempFlags.Parent) != (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                color = this.m_RenderingSettingsData.m_OwnerColor;
              }
            }
            else
              continue;
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            BuildingExtensionData buildingExtensionData = this.m_PrefabBuildingExtensionData[prefab];
            bool flag4 = false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode && this.m_PrefabServiceUpgradeBuildings.HasBuffer(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ServiceUpgradeBuilding> serviceUpgradeBuilding = this.m_PrefabServiceUpgradeBuildings[prefab];
              for (int index2 = 0; index2 < serviceUpgradeBuilding.Length; ++index2)
              {
                Entity building = serviceUpgradeBuilding[index2].m_Building;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabBuildingData.HasComponent(building))
                {
                  // ISSUE: reference to a compiler-generated field
                  BuildingData buildingData = this.m_PrefabBuildingData[building];
                  // ISSUE: reference to a compiler-generated field
                  ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[building];
                  float3 position = transform.m_Position - math.mul(transform.m_Rotation, buildingExtensionData.m_Position);
                  // ISSUE: reference to a compiler-generated method
                  this.DrawLot(color, buildingData.m_LotSize, objectGeometryData, position, transform.m_Rotation);
                  flag4 = true;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (!flag4 && (this.m_EditorMode || buildingExtensionData.m_External))
            {
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
              // ISSUE: reference to a compiler-generated method
              this.DrawLot(color, buildingExtensionData.m_LotSize, objectGeometryData, transform.m_Position, transform.m_Rotation);
            }
          }
          else if (flag3)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode)
            {
              // ISSUE: reference to a compiler-generated field
              AssetStampData assetStampData = this.m_PrefabAssetStampData[prefab];
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
              // ISSUE: reference to a compiler-generated method
              this.DrawLot(color, assetStampData.m_Size, objectGeometryData, transform.m_Position, transform.m_Rotation);
            }
          }
          else if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            BuildingData buildingData = this.m_PrefabBuildingData[prefab];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
            // ISSUE: reference to a compiler-generated method
            this.DrawLot(color, buildingData.m_LotSize, objectGeometryData, transform.m_Position, transform.m_Rotation);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
            // ISSUE: reference to a compiler-generated method
            this.DrawCollision(new UnityEngine.Color(color.r, color.g, color.b, 0.25f), objectGeometryData, transform.m_Position, transform.m_Rotation, 2f);
          }
        }
      }

      private void DrawLot(
        UnityEngine.Color color,
        int2 lotSize,
        ObjectGeometryData objectGeometryData,
        float3 position,
        quaternion rotation)
      {
        UnityEngine.Color color1;
        UnityEngine.Color fillColor;
        OverlayRenderSystem.StyleFlags styleFlags;
        float outlineWidth;
        float num;
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorMode)
        {
          color1 = new UnityEngine.Color(color.r, color.g, color.b, 0.25f);
          fillColor = new UnityEngine.Color(color.r, color.g, color.b, 0.0f);
          styleFlags = OverlayRenderSystem.StyleFlags.Grid | OverlayRenderSystem.StyleFlags.Projected;
          outlineWidth = 0.1f;
          num = 0.1f;
        }
        else
        {
          color1 = new UnityEngine.Color(color.r, color.g, color.b, 0.25f);
          fillColor = new UnityEngine.Color(color.r, color.g, color.b, 0.05f);
          styleFlags = OverlayRenderSystem.StyleFlags.Projected;
          outlineWidth = 0.2f;
          num = 0.5f;
        }
        bool flag;
        float2 x;
        if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
        {
          flag = (objectGeometryData.m_Flags & Game.Objects.GeometryFlags.CircularLeg) != 0;
          x = math.min((float2) lotSize * 8f, objectGeometryData.m_LegSize.xz);
          // ISSUE: reference to a compiler-generated method
          this.DrawCollision(color1, objectGeometryData, position, rotation, 8f);
        }
        else
        {
          flag = (objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != 0;
          x = (float2) lotSize * 8f;
        }
        if (flag)
        {
          float3 float3 = math.forward(rotation);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCircle(color1, fillColor, outlineWidth, styleFlags, float3.xz, position, math.cmax(x));
        }
        else
        {
          float3 float3 = math.forward(rotation) * (x.y * 0.5f - num);
          Line3.Segment line = new Line3.Segment(position - float3, position + float3);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawLine(color1, fillColor, outlineWidth, styleFlags, line, x.x, (float2) (float) ((double) num / (double) x.x * 2.0));
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EditorMode)
          return;
        float3 float3_1 = math.forward(rotation);
        float3 _a = position + float3_1 * (x.y * 0.5f);
        Line3.Segment line1 = new Line3.Segment(_a, _a + float3_1 * 4f);
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(color1, color1, 0.0f, OverlayRenderSystem.StyleFlags.Projected, line1, 0.5f, new float2(0.0f, 1f));
      }

      private void DrawCollision(
        UnityEngine.Color color,
        ObjectGeometryData objectGeometryData,
        float3 position,
        quaternion rotation,
        float size)
      {
        float3 float3_1 = position;
        float3_1.y += objectGeometryData.m_LegSize.y;
        if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
        {
          float3 float3_2 = math.rotate(rotation, new float3(objectGeometryData.m_Size.x * 0.5f, 0.0f, 0.0f));
          float3 float3_3 = math.rotate(rotation, new float3(objectGeometryData.m_Size.x * 0.2761424f, 0.0f, 0.0f));
          float3 float3_4 = math.rotate(rotation, new float3(0.0f, 0.0f, objectGeometryData.m_Size.z * 0.5f));
          float3 float3_5 = math.rotate(rotation, new float3(0.0f, 0.0f, objectGeometryData.m_Size.z * 0.2761424f));
          Bezier4x3 curve1 = new Bezier4x3(float3_1 - float3_2, float3_1 - float3_2 - float3_5, float3_1 - float3_3 - float3_4, float3_1 - float3_4);
          Bezier4x3 curve2 = new Bezier4x3(float3_1 - float3_4, float3_1 + float3_3 - float3_4, float3_1 + float3_2 - float3_5, float3_1 + float3_2);
          Bezier4x3 curve3 = new Bezier4x3(float3_1 + float3_2, float3_1 + float3_2 + float3_5, float3_1 + float3_3 + float3_4, float3_1 + float3_4);
          Bezier4x3 curve4 = new Bezier4x3(float3_1 + float3_4, float3_1 - float3_3 + float3_4, float3_1 - float3_2 + float3_5, float3_1 - float3_2);
          float x = MathUtils.Length(curve1.xz) / size;
          float num = (float) ((double) x / (double) math.max(1f, math.round(x)) * ((double) size * 0.5));
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedCurve(color, curve1, size * 0.125f, num, num);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedCurve(color, curve2, size * 0.125f, num, num);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedCurve(color, curve3, size * 0.125f, num, num);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedCurve(color, curve4, size * 0.125f, num, num);
        }
        else
        {
          float3 float3_6 = math.rotate(rotation, new float3(objectGeometryData.m_Bounds.min.x, 0.0f, 0.0f));
          float3 float3_7 = math.rotate(rotation, new float3(objectGeometryData.m_Bounds.max.x, 0.0f, 0.0f));
          float3 float3_8 = math.rotate(rotation, new float3(0.0f, 0.0f, objectGeometryData.m_Bounds.min.z));
          float3 float3_9 = math.rotate(rotation, new float3(0.0f, 0.0f, objectGeometryData.m_Bounds.max.z));
          Line3.Segment line1 = new Line3.Segment(float3_1 + float3_6 + float3_8, float3_1 + float3_7 + float3_8);
          Line3.Segment line2 = new Line3.Segment(float3_1 + float3_7 + float3_8, float3_1 + float3_7 + float3_9);
          Line3.Segment line3 = new Line3.Segment(float3_1 + float3_7 + float3_9, float3_1 + float3_6 + float3_9);
          Line3.Segment line4 = new Line3.Segment(float3_1 + float3_6 + float3_9, float3_1 + float3_6 + float3_8);
          float x1 = MathUtils.Length(line1.xz) / size;
          float x2 = MathUtils.Length(line2.xz) / size;
          float num1 = (float) ((double) x1 / (double) math.max(1f, math.round(x1)) * ((double) size * 0.5));
          float num2 = (float) ((double) x2 / (double) math.max(1f, math.round(x2)) * ((double) size * 0.5));
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedLine(color, line1, size * 0.125f, num1, num1);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedLine(color, line2, size * 0.125f, num2, num2);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedLine(color, line3, size * 0.125f, num1, num1);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedLine(color, line4, size * 0.125f, num2, num2);
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
    private struct BuildingTerraformRenderJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> m_PrefabBuildingTerraformData;
      [ReadOnly]
      public BufferLookup<AdditionalBuildingTerraformElement> m_PrefabAdditionalTerraform;
      [ReadOnly]
      public Entity m_Selected;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        PrefabRef componentData1;
        BuildingTerraformData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.TryGetComponent(this.m_Selected, out componentData1) || !this.m_PrefabBuildingTerraformData.TryGetComponent(componentData1.m_Prefab, out componentData2))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform = this.m_TransformData[this.m_Selected];
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[componentData1.m_Prefab];
        bool circular = (objectGeometryData.m_Flags & ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != 0 ? Game.Objects.GeometryFlags.CircularLeg : Game.Objects.GeometryFlags.Circular)) != 0;
        DynamicBuffer<AdditionalBuildingTerraformElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabAdditionalTerraform.TryGetBuffer(componentData1.m_Prefab, out bufferData);
        // ISSUE: reference to a compiler-generated method
        this.DrawTerraform(componentData2, bufferData, transform.m_Position, transform.m_Rotation, circular);
      }

      private void DrawTerraform(
        BuildingTerraformData terraformData,
        DynamicBuffer<AdditionalBuildingTerraformElement> additionalElements,
        float3 position,
        quaternion rotation,
        bool circular)
      {
        UnityEngine.Color magenta = UnityEngine.Color.magenta;
        float3 float3_1 = math.rotate(rotation, math.right());
        float3 float3_2 = math.rotate(rotation, math.forward());
        if (circular)
        {
          UnityEngine.Color fillColor = new UnityEngine.Color(magenta.r, magenta.g, magenta.b, 0.0f);
          float diameter = math.cmax(terraformData.m_Smooth.zw - terraformData.m_Smooth.xy);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCircle(magenta, fillColor, 0.25f, (OverlayRenderSystem.StyleFlags) 0, float3_2.xz, position, diameter);
        }
        else
        {
          float3 float3_3 = position + float3_1 * terraformData.m_Smooth.x + float3_2 * terraformData.m_Smooth.y;
          float3 float3_4 = position + float3_1 * terraformData.m_Smooth.x + float3_2 * terraformData.m_Smooth.w;
          float3 float3_5 = position + float3_1 * terraformData.m_Smooth.z + float3_2 * terraformData.m_Smooth.w;
          float3 float3_6 = position + float3_1 * terraformData.m_Smooth.z + float3_2 * terraformData.m_Smooth.y;
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_3, float3_4), 0.25f, new float2(1f, 1f));
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_4, float3_5), 0.25f, new float2(1f, 1f));
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_5, float3_6), 0.25f, new float2(1f, 1f));
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_6, float3_3), 0.25f, new float2(1f, 1f));
        }
        if (additionalElements.IsCreated)
        {
          for (int index = 0; index < additionalElements.Length; ++index)
          {
            AdditionalBuildingTerraformElement additionalElement = additionalElements[index];
            if (additionalElement.m_Circular)
            {
              UnityEngine.Color fillColor = new UnityEngine.Color(magenta.r, magenta.g, magenta.b, 0.0f);
              float diameter = math.cmax(MathUtils.Size(additionalElement.m_Area));
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawCircle(magenta, fillColor, 0.25f, (OverlayRenderSystem.StyleFlags) 0, float3_2.xz, position, diameter);
            }
            else
            {
              float3 float3_7 = position + float3_1 * additionalElement.m_Area.min.x + float3_2 * additionalElement.m_Area.min.y;
              float3 float3_8 = position + float3_1 * additionalElement.m_Area.min.x + float3_2 * additionalElement.m_Area.max.y;
              float3 float3_9 = position + float3_1 * additionalElement.m_Area.max.x + float3_2 * additionalElement.m_Area.max.y;
              float3 float3_10 = position + float3_1 * additionalElement.m_Area.max.x + float3_2 * additionalElement.m_Area.min.y;
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_7, float3_8), 0.25f, new float2(1f, 1f));
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_8, float3_9), 0.25f, new float2(1f, 1f));
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_9, float3_10), 0.25f, new float2(1f, 1f));
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_10, float3_7), 0.25f, new float2(1f, 1f));
            }
          }
        }
        float3 float3_11 = position + float3_1 * terraformData.m_FlatX0.x + float3_2 * terraformData.m_FlatZ0.y;
        float3 float3_12 = position + float3_1 * terraformData.m_FlatX0.y + float3_2 * terraformData.m_FlatZ0.x;
        float3 float3_13 = position + float3_1 * terraformData.m_FlatX0.y + float3_2 * terraformData.m_FlatZ1.x;
        float3 float3_14 = position + float3_1 * terraformData.m_FlatX0.z + float3_2 * terraformData.m_FlatZ1.y;
        float3 float3_15 = position + float3_1 * terraformData.m_FlatX1.z + float3_2 * terraformData.m_FlatZ1.y;
        float3 float3_16 = position + float3_1 * terraformData.m_FlatX1.y + float3_2 * terraformData.m_FlatZ1.z;
        float3 float3_17 = position + float3_1 * terraformData.m_FlatX1.y + float3_2 * terraformData.m_FlatZ0.z;
        float3 float3_18 = position + float3_1 * terraformData.m_FlatX1.x + float3_2 * terraformData.m_FlatZ0.y;
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_11, float3_12), 0.5f, new float2(1f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_12, float3_13), 0.5f, new float2(1f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_13, float3_14), 0.5f, new float2(1f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_14, float3_15), 0.5f, new float2(1f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_15, float3_16), 0.5f, new float2(1f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_16, float3_17), 0.5f, new float2(1f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_17, float3_18), 0.5f, new float2(1f, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawLine(magenta, magenta, 0.0f, (OverlayRenderSystem.StyleFlags) 0, new Line3.Segment(float3_18, float3_11), 0.5f, new float2(1f, 1f));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Extension> __Game_Buildings_Extension_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AssetStamp> __Game_Objects_AssetStamp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Tree> __Game_Objects_Tree_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Warning> __Game_Tools_Warning_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Error> __Game_Tools_Error_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AssetStampData> __Game_Prefabs_AssetStampData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceUpgradeBuilding> __Game_Prefabs_ServiceUpgradeBuilding_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> __Game_Prefabs_BuildingTerraformData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<AdditionalBuildingTerraformElement> __Game_Prefabs_AdditionalBuildingTerraformElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Extension_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Extension>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_AssetStamp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AssetStamp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Warning_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Warning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Error_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Error>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AssetStampData_RO_ComponentLookup = state.GetComponentLookup<AssetStampData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeBuilding_RO_BufferLookup = state.GetBufferLookup<ServiceUpgradeBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup = state.GetComponentLookup<BuildingTerraformData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AdditionalBuildingTerraformElement_RO_BufferLookup = state.GetBufferLookup<AdditionalBuildingTerraformElement>(true);
      }
    }
  }
}
