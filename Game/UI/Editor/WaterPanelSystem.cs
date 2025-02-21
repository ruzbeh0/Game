// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.WaterPanelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Input;
using Game.Prefabs;
using Game.Reflection;
using Game.Rendering;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class WaterPanelSystem : EditorPanelSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private ToolSystem m_ToolSystem;
    private WaterToolSystem m_WaterToolSystem;
    private WaterSystem m_WaterSystem;
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_WaterSourceQuery;
    private EntityQuery m_UpdatedSourceQuery;
    private EntityArchetype m_WaterSourceArchetype;
    private WaterPanelSystem.WaterConfig m_Config = new WaterPanelSystem.WaterConfig();
    private static readonly int[] kWaterSpeedValues = new int[7]
    {
      0,
      1,
      8,
      16,
      32,
      64,
      128
    };

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterToolSystem = this.World.GetOrCreateSystemManaged<WaterToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSourceQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Simulation.WaterSourceData>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.Exclude<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedSourceQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Simulation.WaterSourceData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSourceArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Simulation.WaterSourceData>(), ComponentType.ReadWrite<Game.Objects.Transform>());
      EditorGenerator editorGenerator = new EditorGenerator();
      this.title = (LocalizedString) "Editor.WATER";
      IWidget[] widgetArray = new IWidget[1];
      IWidget[] children = new IWidget[2];
      EditorSection editorSection1 = new EditorSection();
      editorSection1.displayName = (LocalizedString) "Editor.WATER_SETTINGS";
      editorSection1.tooltip = new LocalizedString?((LocalizedString) "Editor.WATER_SETTINGS_TOOLTIP");
      editorSection1.expanded = true;
      // ISSUE: reference to a compiler-generated field
      editorSection1.children = (IList<IWidget>) editorGenerator.BuildMembers((IValueAccessor) new ObjectAccessor<WaterPanelSystem.WaterConfig>(this.m_Config), 0, "WaterSettings").ToArray<IWidget>();
      children[0] = (IWidget) editorSection1;
      EditorSection editorSection2 = new EditorSection();
      editorSection2.displayName = (LocalizedString) "Editor.WATER_SIMULATION_SPEED";
      editorSection2.tooltip = new LocalizedString?((LocalizedString) "Editor.WATER_SIMULATION_SPEED_TOOLTIP");
      // ISSUE: reference to a compiler-generated method
      editorSection2.children = (IList<IWidget>) this.BuildWaterSpeedToggles();
      children[1] = (IWidget) editorSection2;
      widgetArray[0] = (IWidget) Scrollable.WithChildren((IList<IWidget>) children);
      this.children = (IList<IWidget>) widgetArray;
    }

    private IWidget[] BuildWaterSpeedToggles()
    {
      // ISSUE: reference to a compiler-generated field
      IWidget[] widgetArray1 = new IWidget[WaterPanelSystem.kWaterSpeedValues.Length];
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < WaterPanelSystem.kWaterSpeedValues.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        int speed = WaterPanelSystem.kWaterSpeedValues[index1];
        IWidget[] widgetArray2 = widgetArray1;
        int index2 = index1;
        ToggleField toggleField = new ToggleField();
        toggleField.displayName = (LocalizedString) string.Format("{0}x", (object) speed);
        // ISSUE: reference to a compiler-generated field
        toggleField.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_WaterSystem.WaterSimSpeed == speed), (Action<bool>) (val =>
        {
          if (!val)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_WaterSystem.WaterSimSpeed = speed;
        }));
        widgetArray2[index2] = (IWidget) toggleField;
      }
      return widgetArray1;
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated method
      this.FetchWaterSources();
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      if (InputManager.instance.activeControlScheme != InputManager.ControlScheme.KeyboardAndMouse)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_WaterToolSystem;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpdatedSourceQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated method
      this.FetchWaterSources();
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_WaterToolSystem)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.ActivatePrefabTool((PrefabBase) null);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem.WaterSimSpeed = 1;
      base.OnStopRunning();
    }

    protected override bool OnCancel()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool != this.m_WaterToolSystem)
        return base.OnCancel();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool((PrefabBase) null);
      return false;
    }

    protected override void OnValueChanged(IWidget widget) => this.ApplyWaterSources();

    public void FetchWaterSources()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Config.m_ConstantRateWaterSources.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Config.m_ConstantLevelWaterSources.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Config.m_BorderRiverWaterSources.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Config.m_BorderSeaWaterSources.Clear();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Game.Simulation.WaterSourceData> componentDataArray1 = this.m_WaterSourceQuery.ToComponentDataArray<Game.Simulation.WaterSourceData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Game.Objects.Transform> componentDataArray2 = this.m_WaterSourceQuery.ToComponentDataArray<Game.Objects.Transform>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        for (int index = 0; index < componentDataArray1.Length; ++index)
        {
          switch (componentDataArray1[index].m_ConstantDepth)
          {
            case 0:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              List<WaterPanelSystem.WaterConfig.ConstantRateWaterSource> rateWaterSources = this.m_Config.m_ConstantRateWaterSources;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              WaterPanelSystem.WaterConfig.ConstantRateWaterSource constantRateWaterSource = new WaterPanelSystem.WaterConfig.ConstantRateWaterSource();
              // ISSUE: reference to a compiler-generated field
              constantRateWaterSource.m_Initialized = true;
              // ISSUE: reference to a compiler-generated field
              constantRateWaterSource.m_Rate = componentDataArray1[index].m_Amount;
              // ISSUE: reference to a compiler-generated field
              constantRateWaterSource.m_Position = componentDataArray2[index].m_Position.xz;
              // ISSUE: reference to a compiler-generated field
              constantRateWaterSource.m_Radius = componentDataArray1[index].m_Radius;
              // ISSUE: reference to a compiler-generated field
              constantRateWaterSource.m_Pollution = componentDataArray1[index].m_Polluted;
              rateWaterSources.Add(constantRateWaterSource);
              break;
            case 1:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              List<WaterPanelSystem.WaterConfig.ConstantLevelWaterSource> levelWaterSources = this.m_Config.m_ConstantLevelWaterSources;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              WaterPanelSystem.WaterConfig.ConstantLevelWaterSource levelWaterSource = new WaterPanelSystem.WaterConfig.ConstantLevelWaterSource();
              // ISSUE: reference to a compiler-generated field
              levelWaterSource.m_Initialized = true;
              // ISSUE: reference to a compiler-generated field
              levelWaterSource.m_Height = componentDataArray1[index].m_Amount;
              // ISSUE: reference to a compiler-generated field
              levelWaterSource.m_Position = componentDataArray2[index].m_Position.xz;
              // ISSUE: reference to a compiler-generated field
              levelWaterSource.m_Radius = componentDataArray1[index].m_Radius;
              // ISSUE: reference to a compiler-generated field
              levelWaterSource.m_Pollution = componentDataArray1[index].m_Polluted;
              levelWaterSources.Add(levelWaterSource);
              break;
            case 2:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              List<WaterPanelSystem.WaterConfig.BorderWaterSource> riverWaterSources = this.m_Config.m_BorderRiverWaterSources;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              WaterPanelSystem.WaterConfig.BorderWaterSource borderWaterSource1 = new WaterPanelSystem.WaterConfig.BorderWaterSource();
              // ISSUE: reference to a compiler-generated field
              borderWaterSource1.m_Initialized = true;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource1.m_FloodHeight = componentDataArray1[index].m_Multiplier;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource1.m_Height = componentDataArray1[index].m_Amount;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource1.m_Position = componentDataArray2[index].m_Position.xz;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource1.m_Radius = componentDataArray1[index].m_Radius;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource1.m_Pollution = componentDataArray1[index].m_Polluted;
              riverWaterSources.Add(borderWaterSource1);
              break;
            case 3:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              List<WaterPanelSystem.WaterConfig.BorderWaterSource> borderSeaWaterSources = this.m_Config.m_BorderSeaWaterSources;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              WaterPanelSystem.WaterConfig.BorderWaterSource borderWaterSource2 = new WaterPanelSystem.WaterConfig.BorderWaterSource();
              // ISSUE: reference to a compiler-generated field
              borderWaterSource2.m_Initialized = true;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource2.m_FloodHeight = componentDataArray1[index].m_Multiplier;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource2.m_Height = componentDataArray1[index].m_Amount;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource2.m_Position = componentDataArray2[index].m_Position.xz;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource2.m_Radius = componentDataArray1[index].m_Radius;
              // ISSUE: reference to a compiler-generated field
              borderWaterSource2.m_Pollution = componentDataArray1[index].m_Polluted;
              borderSeaWaterSources.Add(borderWaterSource2);
              break;
          }
        }
      }
      finally
      {
        componentDataArray1.Dispose();
        componentDataArray2.Dispose();
      }
    }

    private void ApplyWaterSources()
    {
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_WaterSourceQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      int sourceCount = 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (WaterPanelSystem.WaterConfig.ConstantRateWaterSource constantRateWaterSource in this.m_Config.m_ConstantRateWaterSources)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.AddSource(commandBuffer, this.GetSource(commandBuffer, entityArray, ref sourceCount), (WaterPanelSystem.WaterConfig.WaterSource) constantRateWaterSource, 0, ref constantRateWaterSource.m_Rate, ref heightData);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (WaterPanelSystem.WaterConfig.ConstantLevelWaterSource levelWaterSource in this.m_Config.m_ConstantLevelWaterSources)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.AddSource(commandBuffer, this.GetSource(commandBuffer, entityArray, ref sourceCount), (WaterPanelSystem.WaterConfig.WaterSource) levelWaterSource, 1, ref levelWaterSource.m_Height, ref heightData);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (WaterPanelSystem.WaterConfig.BorderWaterSource riverWaterSource in this.m_Config.m_BorderRiverWaterSources)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.AddBorderSource(commandBuffer, this.GetSource(commandBuffer, entityArray, ref sourceCount), riverWaterSource, 2, ref heightData);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (WaterPanelSystem.WaterConfig.BorderWaterSource borderSeaWaterSource in this.m_Config.m_BorderSeaWaterSources)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.AddBorderSource(commandBuffer, this.GetSource(commandBuffer, entityArray, ref sourceCount), borderSeaWaterSource, 3, ref heightData);
      }
      while (sourceCount < entityArray.Length)
        commandBuffer.AddComponent<Deleted>(entityArray[sourceCount++], new Deleted());
      entityArray.Dispose();
    }

    private Entity GetSource(
      EntityCommandBuffer buffer,
      NativeArray<Entity> sources,
      ref int sourceCount)
    {
      // ISSUE: reference to a compiler-generated field
      return sourceCount < sources.Length ? sources[sourceCount++] : buffer.CreateEntity(this.m_WaterSourceArchetype);
    }

    private void AddSource(
      EntityCommandBuffer buffer,
      Entity entity,
      WaterPanelSystem.WaterConfig.WaterSource source,
      int constantDepth,
      ref float amount,
      ref TerrainHeightData terrainHeightData)
    {
      // ISSUE: reference to a compiler-generated field
      if (!source.m_Initialized)
      {
        // ISSUE: variable of a compiler-generated type
        CameraUpdateSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<CameraUpdateSystem>();
        float3 pivot = (float3) existingSystemManaged.activeCameraController.pivot;
        // ISSUE: reference to a compiler-generated field
        source.m_Initialized = true;
        // ISSUE: reference to a compiler-generated field
        source.m_Position = pivot.xz;
        // ISSUE: reference to a compiler-generated field
        Bounds3 bounds3 = MathUtils.Expand(TerrainUtils.GetBounds(ref terrainHeightData), (float3) -source.m_Radius);
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds3.xz, source.m_Position))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          source.m_Position = MathUtils.Clamp(source.m_Position, bounds3.xz);
          // ISSUE: reference to a compiler-generated field
          pivot.xz = source.m_Position;
          existingSystemManaged.activeCameraController.pivot = (Vector3) pivot;
        }
        if (constantDepth == 0)
        {
          // ISSUE: reference to a compiler-generated field
          source.m_Radius = 30f;
          amount = 20f;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          source.m_Radius = 40f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          amount = TerrainUtils.SampleHeight(ref terrainHeightData, new float3(source.m_Position.x, 0.0f, source.m_Position.y));
          // ISSUE: reference to a compiler-generated field
          amount += 25f - this.m_TerrainSystem.positionOffset.y;
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float3 pos = new float3(source.m_Position.x, 0.0f, source.m_Position.y);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Game.Simulation.WaterSourceData waterSourceData = new Game.Simulation.WaterSourceData()
      {
        m_Amount = amount,
        m_ConstantDepth = constantDepth,
        m_Radius = source.m_Radius,
        m_Polluted = source.m_Pollution
      };
      // ISSUE: reference to a compiler-generated method
      waterSourceData.m_Multiplier = WaterSystem.CalculateSourceMultiplier(waterSourceData, pos);
      buffer.SetComponent<Game.Simulation.WaterSourceData>(entity, waterSourceData);
      buffer.SetComponent<Game.Objects.Transform>(entity, new Game.Objects.Transform()
      {
        m_Position = pos,
        m_Rotation = quaternion.identity
      });
    }

    private void AddBorderSource(
      EntityCommandBuffer buffer,
      Entity entity,
      WaterPanelSystem.WaterConfig.BorderWaterSource source,
      int constantDepth,
      ref TerrainHeightData terrainHeightData)
    {
      // ISSUE: reference to a compiler-generated field
      if (!source.m_Initialized)
      {
        // ISSUE: variable of a compiler-generated type
        CameraUpdateSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<CameraUpdateSystem>();
        float3 pivot = (float3) existingSystemManaged.activeCameraController.pivot;
        // ISSUE: reference to a compiler-generated field
        source.m_Initialized = true;
        // ISSUE: reference to a compiler-generated field
        source.m_Position = pivot.xz;
        Bounds3 bounds = TerrainUtils.GetBounds(ref terrainHeightData);
        // ISSUE: reference to a compiler-generated field
        Bounds3 bounds3 = MathUtils.Expand(bounds, (float3) -source.m_Radius);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(MathUtils.Expand(bounds, (float3) source.m_Radius).xz, source.m_Position))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          source.m_Position = MathUtils.Clamp(source.m_Position, bounds.xz);
          // ISSUE: reference to a compiler-generated field
          pivot.xz = source.m_Position;
          existingSystemManaged.activeCameraController.pivot = (Vector3) pivot;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (MathUtils.Intersect(bounds3.xz, source.m_Position))
          {
            // ISSUE: reference to a compiler-generated field
            float2 x = source.m_Position - bounds3.min.xz;
            // ISSUE: reference to a compiler-generated field
            float2 y = bounds3.max.xz - source.m_Position;
            float2 b = math.select(bounds.min.xz, bounds.max.xz, y < x);
            float2 float2 = math.min(x, y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            source.m_Position = math.select(source.m_Position, b, float2.xy < float2.yx);
            // ISSUE: reference to a compiler-generated field
            pivot.xz = source.m_Position;
            existingSystemManaged.activeCameraController.pivot = (Vector3) pivot;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        source.m_Height = TerrainUtils.SampleHeight(ref terrainHeightData, new float3(source.m_Position.x, 0.0f, source.m_Position.y));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        source.m_Height -= this.m_TerrainSystem.positionOffset.y;
        if (constantDepth == 2)
        {
          // ISSUE: reference to a compiler-generated field
          source.m_Radius = 50f;
          // ISSUE: reference to a compiler-generated field
          source.m_Height += 30f;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          source.m_Radius = 5000f;
          // ISSUE: reference to a compiler-generated field
          source.m_Height += 100f;
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      buffer.SetComponent<Game.Simulation.WaterSourceData>(entity, new Game.Simulation.WaterSourceData()
      {
        m_Amount = source.m_Height,
        m_ConstantDepth = constantDepth,
        m_Radius = source.m_Radius,
        m_Multiplier = source.m_FloodHeight,
        m_Polluted = source.m_Pollution
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      buffer.SetComponent<Game.Objects.Transform>(entity, new Game.Objects.Transform()
      {
        m_Position = new float3(source.m_Position.x, 0.0f, source.m_Position.y),
        m_Rotation = quaternion.identity
      });
    }

    [Preserve]
    public WaterPanelSystem()
    {
    }

    private class WaterConfig
    {
      [InspectorName("Editor.CONSTANT_RATE_WATER_SOURCES")]
      public List<WaterPanelSystem.WaterConfig.ConstantRateWaterSource> m_ConstantRateWaterSources = new List<WaterPanelSystem.WaterConfig.ConstantRateWaterSource>();
      [InspectorName("Editor.CONSTANT_LEVEL_WATER_SOURCES")]
      public List<WaterPanelSystem.WaterConfig.ConstantLevelWaterSource> m_ConstantLevelWaterSources = new List<WaterPanelSystem.WaterConfig.ConstantLevelWaterSource>();
      [InspectorName("Editor.BORDER_RIVER_WATER_SOURCES")]
      public List<WaterPanelSystem.WaterConfig.BorderWaterSource> m_BorderRiverWaterSources = new List<WaterPanelSystem.WaterConfig.BorderWaterSource>();
      [InspectorName("Editor.BORDER_SEA_WATER_SOURCES")]
      public List<WaterPanelSystem.WaterConfig.BorderWaterSource> m_BorderSeaWaterSources = new List<WaterPanelSystem.WaterConfig.BorderWaterSource>();

      [Serializable]
      public class ConstantRateWaterSource : WaterPanelSystem.WaterConfig.WaterSource
      {
        [InspectorName("Editor.WATER_RATE")]
        public float m_Rate;
      }

      [Serializable]
      public class ConstantLevelWaterSource : WaterPanelSystem.WaterConfig.WaterSource
      {
        [InspectorName("Editor.HEIGHT")]
        public float m_Height;
      }

      [Serializable]
      public class BorderWaterSource : WaterPanelSystem.WaterConfig.ConstantLevelWaterSource
      {
        [InspectorName("Editor.FLOOD_HEIGHT")]
        [NonSerialized]
        public float m_FloodHeight;
      }

      public abstract class WaterSource
      {
        [NonSerialized]
        public bool m_Initialized;
        [CustomField(typeof (WaterPanelSystem.WaterConfig.WaterSourcePositionFactory))]
        [InspectorName("Editor.POSITION")]
        public float2 m_Position;
        [InspectorName("Editor.RADIUS")]
        public float m_Radius;
        [InspectorName("Editor.POLLUTION")]
        public float m_Pollution;
      }

      public class WaterSourcePositionFactory : IFieldBuilderFactory
      {
        public FieldBuilder TryCreate(System.Type memberType, object[] attributes)
        {
          return (FieldBuilder) (accessor =>
          {
            CastAccessor<float2> castAccessor = new CastAccessor<float2>(accessor);
            Column column1 = new Column();
            Column column2 = column1;
            // ISSUE: reference to a compiler-generated method
            IWidget[] widgetArray = new IWidget[2]
            {
              (IWidget) new Float2InputField()
              {
                displayName = (LocalizedString) "Editor.POSITION",
                tooltip = new LocalizedString?((LocalizedString) "Editor.POSITION_TOOLTIP"),
                accessor = (ITypedValueAccessor<float2>) castAccessor
              },
              (IWidget) new Button()
              {
                displayName = (LocalizedString) "Editor.LOCATE",
                tooltip = new LocalizedString?((LocalizedString) "Editor.LOCATE_TOOLTIP"),
                action = (System.Action) (() => this.Locate(castAccessor))
              }
            };
            column2.children = (IList<IWidget>) widgetArray;
            return (IWidget) column1;
          });
        }

        private void Locate(CastAccessor<float2> accessor)
        {
          float2 typedValue = accessor.GetTypedValue();
          World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<CameraUpdateSystem>().activeCameraController.pivot = new Vector3(typedValue.x, 0.0f, typedValue.y);
        }
      }
    }
  }
}
