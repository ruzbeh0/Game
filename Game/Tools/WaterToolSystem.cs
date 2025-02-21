// Decompiled with JetBrains decompiler
// Type: Game.Tools.WaterToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class WaterToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Water Tool";
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private EntityQuery m_DefinitionQuery;
    private ControlPoint m_RaycastPoint;
    private ControlPoint m_StartPoint;
    private WaterToolSystem.State m_State;
    private WaterToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Water Tool";

    public WaterToolSystem.Attribute attribute { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionQuery = this.GetDefinitionQuery();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStartRunning()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_RaycastPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = WaterToolSystem.State.Default;
      this.attribute = WaterToolSystem.Attribute.None;
    }

    private protected override void UpdateActions()
    {
      this.applyAction.enabled = this.actionsEnabled;
      this.secondaryApplyAction.enabled = this.actionsEnabled;
    }

    public override PrefabBase GetPrefab() => (PrefabBase) null;

    public override bool TrySetPrefab(PrefabBase prefab) => false;

    public override void InitializeRaycast()
    {
      // ISSUE: reference to a compiler-generated method
      base.InitializeRaycast();
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == WaterToolSystem.State.Dragging)
      {
        if (this.attribute != WaterToolSystem.Attribute.Location)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Outside;
        Game.Simulation.WaterSourceData component;
        // ISSUE: reference to a compiler-generated field
        if (!this.EntityManager.TryGetComponent<Game.Simulation.WaterSourceData>(this.m_StartPoint.m_OriginalEntity, out component))
          return;
        float amount = component.m_Amount;
        if (component.m_ConstantDepth > 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          amount += this.m_TerrainSystem.positionOffset.y - TerrainUtils.SampleHeight(ref heightData, this.m_StartPoint.m_Position);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.rayOffset = new float3(0.0f, -amount, 0.0f);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.WaterSources;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateInfoview(Entity.Null);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.GetAvailableSnapMask(out this.m_SnapOnMask, out this.m_SnapOffMask);
      // ISSUE: reference to a compiler-generated field
      if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) != (RaycastFlags) 0)
      {
        // ISSUE: reference to a compiler-generated method
        return this.Clear(inputDeps);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != WaterToolSystem.State.Default)
      {
        if (this.applyAction.WasPressedThisFrame() || this.applyAction.WasReleasedThisFrame())
        {
          // ISSUE: reference to a compiler-generated method
          return this.Apply(inputDeps);
        }
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return this.secondaryApplyAction.WasPressedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame() ? this.Cancel(inputDeps) : this.Update(inputDeps);
      }
      if (this.secondaryApplyAction.WasPressedThisFrame())
      {
        // ISSUE: reference to a compiler-generated method
        return this.Cancel(inputDeps, this.secondaryApplyAction.WasReleasedThisFrame());
      }
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return this.applyAction.WasPressedThisFrame() ? this.Apply(inputDeps, this.applyAction.WasReleasedThisFrame()) : this.Update(inputDeps);
    }

    public override void GetAvailableSnapMask(out Snap onMask, out Snap offMask)
    {
      // ISSUE: reference to a compiler-generated method
      base.GetAvailableSnapMask(out onMask, out offMask);
      onMask |= Snap.ContourLines;
      offMask |= Snap.ContourLines;
    }

    private JobHandle Clear(JobHandle inputDeps)
    {
      this.applyMode = ApplyMode.Clear;
      return inputDeps;
    }

    private JobHandle Cancel(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      WaterToolSystem.State state = this.m_State;
      switch (state)
      {
        case WaterToolSystem.State.Default:
          this.applyMode = ApplyMode.None;
          return inputDeps;
        case WaterToolSystem.State.MouseDown:
          // ISSUE: reference to a compiler-generated field
          this.m_State = WaterToolSystem.State.Default;
          this.applyMode = ApplyMode.Clear;
          return inputDeps;
        case WaterToolSystem.State.Dragging:
          // ISSUE: reference to a compiler-generated field
          this.m_State = WaterToolSystem.State.Default;
          this.applyMode = ApplyMode.Clear;
          return inputDeps;
        default:
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
      }
    }

    private JobHandle Apply(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      WaterToolSystem.State state = this.m_State;
      switch (state)
      {
        case WaterToolSystem.State.Default:
          // ISSUE: reference to a compiler-generated field
          if (this.m_RaycastPoint.m_OriginalEntity != Entity.Null && !singleFrameOnly)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_State = WaterToolSystem.State.MouseDown;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_StartPoint = this.m_RaycastPoint;
          }
          this.applyMode = ApplyMode.None;
          return inputDeps;
        case WaterToolSystem.State.MouseDown:
          // ISSUE: reference to a compiler-generated field
          this.m_State = WaterToolSystem.State.Default;
          this.applyMode = ApplyMode.Clear;
          return inputDeps;
        case WaterToolSystem.State.Dragging:
          // ISSUE: reference to a compiler-generated field
          this.m_State = WaterToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated method
          this.applyMode = this.GetAllowApply() ? ApplyMode.Apply : ApplyMode.Clear;
          return inputDeps;
        default:
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
      }
    }

    private JobHandle Update(JobHandle inputDeps)
    {
      ControlPoint controlPoint;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out controlPoint))
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_RaycastPoint.Equals(controlPoint))
        {
          this.applyMode = ApplyMode.None;
          return inputDeps;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == WaterToolSystem.State.Default)
        {
          // ISSUE: reference to a compiler-generated method
          this.attribute = this.GetAttribute(controlPoint);
        }
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        this.m_RaycastPoint = controlPoint;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == WaterToolSystem.State.MouseDown && (double) math.distance(controlPoint.m_HitPosition, this.m_StartPoint.m_HitPosition) >= 1.0)
        {
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.UpdateDefinitions(inputDeps);
          // ISSUE: reference to a compiler-generated field
          this.m_State = WaterToolSystem.State.Dragging;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.UpdateDefinitions(inputDeps);
        }
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_RaycastPoint.Equals(new ControlPoint()))
      {
        this.applyMode = ApplyMode.None;
        return inputDeps;
      }
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_RaycastPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == WaterToolSystem.State.MouseDown)
      {
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.UpdateDefinitions(inputDeps);
        // ISSUE: reference to a compiler-generated field
        this.m_State = WaterToolSystem.State.Dragging;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == WaterToolSystem.State.Default)
          this.attribute = WaterToolSystem.Attribute.None;
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.UpdateDefinitions(inputDeps);
      }
      return inputDeps;
    }

    private WaterToolSystem.Attribute GetAttribute(ControlPoint controlPoint)
    {
      Game.Simulation.WaterSourceData component;
      Viewer viewer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.EntityManager.TryGetComponent<Game.Simulation.WaterSourceData>(controlPoint.m_OriginalEntity, out component) || !this.m_CameraUpdateSystem.TryGetViewer(out viewer))
        return WaterToolSystem.Attribute.None;
      float2 float2 = controlPoint.m_HitPosition.xz - controlPoint.m_Position.xz;
      if ((double) math.length(float2) < (double) component.m_Radius * 0.89999997615814209)
        return WaterToolSystem.Attribute.Location;
      float2 xz = viewer.right.xz;
      float2 x = MathUtils.Left(xz);
      if ((double) math.abs(math.dot(xz, float2)) > (double) math.abs(math.dot(x, float2)))
        return WaterToolSystem.Attribute.Radius;
      return component.m_ConstantDepth != 0 ? WaterToolSystem.Attribute.Height : WaterToolSystem.Attribute.Rate;
    }

    protected override bool GetRaycastResult(out ControlPoint controlPoint)
    {
      Game.Simulation.WaterSourceData component;
      Viewer viewer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_State != WaterToolSystem.State.Dragging || this.attribute == WaterToolSystem.Attribute.None || this.attribute == WaterToolSystem.Attribute.Location || !this.EntityManager.TryGetComponent<Game.Simulation.WaterSourceData>(this.m_StartPoint.m_OriginalEntity, out component) || !this.m_CameraUpdateSystem.TryGetViewer(out viewer))
      {
        // ISSUE: reference to a compiler-generated method
        return base.GetRaycastResult(out controlPoint);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      Line3.Segment raycastLine = ToolRaycastSystem.CalculateRaycastLine(viewer.camera);
      // ISSUE: reference to a compiler-generated field
      controlPoint = this.m_StartPoint;
      if (this.attribute == WaterToolSystem.Attribute.Radius)
      {
        // ISSUE: reference to a compiler-generated field
        float3 position = this.m_StartPoint.m_Position;
        // ISSUE: reference to a compiler-generated field
        position.y = component.m_ConstantDepth <= 0 ? TerrainUtils.SampleHeight(ref heightData, position) + component.m_Amount : this.m_TerrainSystem.positionOffset.y + component.m_Amount;
        float t;
        if (MathUtils.Intersect(raycastLine.y, position.y, out t))
          controlPoint.m_HitPosition = MathUtils.Position(raycastLine, t);
      }
      else
      {
        float2 t;
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(new Circle2(component.m_Radius, this.m_StartPoint.m_Position.xz), raycastLine.xz, out t))
        {
          float3 float3_1 = MathUtils.Position(raycastLine, t.x);
          float3 float3_2 = MathUtils.Position(raycastLine, t.y);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          controlPoint.m_HitPosition = (double) math.distancesq(float3_1.xz, this.m_StartPoint.m_HitPosition.xz) > (double) math.distancesq(float3_2.xz, this.m_StartPoint.m_HitPosition.xz) ? float3_2 : float3_1;
        }
      }
      return true;
    }

    private JobHandle UpdateDefinitions(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      // ISSUE: reference to a compiler-generated field
      if (this.m_RaycastPoint.m_OriginalEntity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle = new WaterToolSystem.CreateDefinitionsJob()
        {
          m_StartPoint = this.m_StartPoint,
          m_RaycastPoint = this.m_RaycastPoint,
          m_State = this.m_State,
          m_Attribute = this.attribute,
          m_WaterSourceData = this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentLookup,
          m_PositionOffset = this.m_TerrainSystem.positionOffset,
          m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
        }.Schedule<WaterToolSystem.CreateDefinitionsJob>(inputDeps);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      return job0;
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
    public WaterToolSystem()
    {
    }

    public enum Attribute
    {
      None,
      Location,
      Radius,
      Rate,
      Height,
    }

    private enum State
    {
      Default,
      MouseDown,
      Dragging,
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public ControlPoint m_StartPoint;
      [ReadOnly]
      public ControlPoint m_RaycastPoint;
      [ReadOnly]
      public WaterToolSystem.State m_State;
      [ReadOnly]
      public WaterToolSystem.Attribute m_Attribute;
      [ReadOnly]
      public ComponentLookup<Game.Simulation.WaterSourceData> m_WaterSourceData;
      [ReadOnly]
      public float3 m_PositionOffset;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        Entity originalEntity = Entity.Null;
        float3 float3 = new float3();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        WaterToolSystem.State state = this.m_State;
        switch (state)
        {
          case WaterToolSystem.State.Default:
            // ISSUE: reference to a compiler-generated field
            originalEntity = this.m_RaycastPoint.m_OriginalEntity;
            // ISSUE: reference to a compiler-generated field
            float3 = this.m_RaycastPoint.m_Position;
            break;
          case WaterToolSystem.State.MouseDown:
            // ISSUE: reference to a compiler-generated field
            originalEntity = this.m_StartPoint.m_OriginalEntity;
            // ISSUE: reference to a compiler-generated field
            float3 = this.m_StartPoint.m_Position;
            break;
          case WaterToolSystem.State.Dragging:
            // ISSUE: reference to a compiler-generated field
            originalEntity = this.m_StartPoint.m_OriginalEntity;
            // ISSUE: reference to a compiler-generated field
            float3 = this.m_RaycastPoint.m_Position;
            break;
        }
        Game.Simulation.WaterSourceData componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_WaterSourceData.TryGetComponent(originalEntity, out componentData))
          return;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Original = originalEntity;
        component1.m_Flags |= CreationFlags.Select;
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == WaterToolSystem.State.Dragging)
        {
          component1.m_Flags |= CreationFlags.Dragging;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          WaterToolSystem.Attribute attribute = this.m_Attribute;
          switch (attribute)
          {
            case WaterToolSystem.Attribute.Location:
              if (componentData.m_ConstantDepth == 1 || componentData.m_ConstantDepth == 2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                componentData.m_Amount += this.m_RaycastPoint.m_Position.y - this.m_StartPoint.m_Position.y;
                break;
              }
              break;
            case WaterToolSystem.Attribute.Radius:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              componentData.m_Radius = math.clamp(math.distance(this.m_RaycastPoint.m_HitPosition.xz, this.m_StartPoint.m_Position.xz), 1f, 20000f);
              break;
            case WaterToolSystem.Attribute.Rate:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              componentData.m_Amount = math.clamp(this.m_RaycastPoint.m_HitPosition.y - this.m_StartPoint.m_Position.y, 1f, 1000f);
              break;
            case WaterToolSystem.Attribute.Height:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              componentData.m_Amount = this.m_RaycastPoint.m_HitPosition.y - this.m_PositionOffset.y;
              break;
          }
        }
        WaterSourceDefinition component2 = new WaterSourceDefinition();
        component2.m_Position = float3;
        component2.m_ConstantDepth = componentData.m_ConstantDepth;
        component2.m_Amount = componentData.m_Amount;
        component2.m_Radius = componentData.m_Radius;
        component2.m_Multiplier = componentData.m_Multiplier;
        component2.m_Polluted = componentData.m_Polluted;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<WaterSourceDefinition>(entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Game.Simulation.WaterSourceData> __Game_Simulation_WaterSourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterSourceData_RO_ComponentLookup = state.GetComponentLookup<Game.Simulation.WaterSourceData>(true);
      }
    }
  }
}
